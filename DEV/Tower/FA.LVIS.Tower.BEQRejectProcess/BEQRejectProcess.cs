using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Threading;
using System.Xml.Linq;
using FA.LVIS.Tower.Common;
using FA.LVIS.Tower.Data;
using FA.LVIS.Tower.DataContracts;
using LVIS.Adapters.EMSAdapter;
using LVIS.Common;
using System.IO;
using LVIS.DTO.Canonical.Order;


namespace FA.LVIS.Tower.BEQRejectProcess
{

    public class BEQRejectProcess
    {
        Logger sLogger = new Common.Logger();
        IEMSAdapter EMSAdapter;
       IUtils Utils = new Utils();
        private string No_Good_Match_DefaultNotes = "Thank you for the order.  We are rejecting this order - for now - because our Escrow/Closing/Settlement Officer has not opened the order within our system yet.  All purchase orders must be received by our local office first from the Real Estate Agent/Broker or Attorney, before we can accept and connect your order to it.  Please contact the local Escrow/Closing/Settlement Office and confirm the order is opened. Then, you are welcome to resubmit the order.";
        private string Mismatch_Lender_DefaultNotes = "Thank you for the order.  We are rejecting this order - for now - because we do not show you as the new lender on the file.  Please contact the local Escrow/Closing/Settlement Office and confirm that the buyer has communicated to them that you are the new lender on the file.  Then, you are welcome to resubmit the order.";
        //Fetch & Reject BEQ Exception
        public void FetchRejectBeqException()
        {
            EMSAdapter = new EMSAdapter();
            int orderSuccessCounter = 0;
            List<string> successExtRefNumber = new List<string>();
            string body = null;

            using (TerminalEntities dbcontext = new TerminalEntities())
            {                
                try
                {
                    int Userid = dbcontext.Tower_Users.Where(se => se.Name == "BEQ Reject Job").FirstOrDefault().UserId;

                    int tenantid = 1;//Agency and ATC only and Active status only
                    int atcTenantId = 8;//Agency and ATC only and Active status only

                    var exceptionsList = dbcontext.Exceptions.Where(se => (se.TypeCodeId == 202) && (
                                               tenantid == (se.MessageLogId != 0 ? dbcontext.MessageLogs.Where(ml => ml.MessageLogId == se.MessageLogId).FirstOrDefault().TenantId : 0)
                                               || atcTenantId == (se.MessageLogId != 0 ? dbcontext.MessageLogs.Where(ml => ml.MessageLogId == se.MessageLogId).FirstOrDefault().TenantId : 0)) &&
                                               se.ExceptionType.ExceptionGroupId == (int)ExceptionGroupEnum.BEQ).ToList();

                    List<ExceptionDTO> Det = new List<ExceptionDTO>();


                    foreach (var se in exceptionsList.OrderByDescending(sel => sel.CreatedDate))
                    {
                        var Status = se.TypeCode;

                        var ServiceReq = dbcontext.MessageLogs.Where(ml => ml.MessageLogId == se.MessageLogId).FirstOrDefault() != null
                            ? dbcontext.MessageLogs.Where(ml => ml.MessageLogId == se.MessageLogId).FirstOrDefault().ServiceRequest : new ServiceRequest();

                        if (!IsSupportedTenant(ServiceReq.TenantId, ServiceReq.ApplicationId))
                            continue;

                        ExceptionDTO Detail = new ExceptionDTO();
                        Detail.Exceptionid = se.ExceptionId;
                        Detail.ExceptionTypeid = se.ExceptionTypeId;

                        Detail.Status = new ExceptionStatus() { ID = Status.TypeCodeId, Name = Status.TypeCodeDesc };


                        Detail.DocumentObjectid = se.DocumentObjectId;
                        Detail.ExternalRefNum = ServiceReq != null ? ServiceReq.ExternalRefNum : string.Empty;
                        Detail.ServiceType = ServiceReq != null ? ServiceReq.ServiceId.ToString() : "0";

                        //GetRegionid
                        //GetReofagionid
                        #region GET REGION
                        try
                        {
                            TerminalDocumentEntities DbDocumentcontext = new TerminalDocumentEntities();
                            DocumentObject Docobject = DbDocumentcontext.DocumentObjects.Where(se1 => se1.DocumentObjectId == se.DocumentObjectId).FirstOrDefault();
                            if (Docobject == null)
                                continue;
                            var content = Docobject?.Object;

                            if (string.IsNullOrEmpty(content) && !string.IsNullOrEmpty(Docobject?.ObjectPath))
                            {
                                content = File.ReadAllText(Docobject.ObjectPath);
                            }

                            var items = (from t in XDocument.Parse(content)?.Descendants(DataContracts.Constants.EXCEPTION)
                                         select new
                                         {
                                             Source = t.Element("Source")?.Value,
                                             Destination = t.Element("CurrentSource")?.Value,
                                             ServiceReqId = t.Element("ServiceRequestId")?.Value,
                                             MessageLogId = Convert.ToInt32(t.Element("MessageLogId")?.Value),
                                             RestartStep = t.Element("FailedStepNumber")?.Value,
                                             DocumentObjectId = Convert.ToInt64(t.Element("DocumentObjectId")?.Value),
                                             ExternalRefNumber = t.Element("ExternalRefNum")?.Value,
                                         }).FirstOrDefault();

                            //sLogger.Info("Starting to fetch region");
                            var Region = BEQParse(items.DocumentObjectId);

                            #endregion
                            //if (Region!=0)
                            //{
                            //Get retentiondays

                            var retentionDays = dbcontext.BEQRegionRetentions.Where(r => r.RegionID == Region).FirstOrDefault().RetentionDays;
                                //Exeception created date and today get span() compare
                               // sLogger.Info("Retention days fetched");
                                TimeSpan span = DateTime.Now.Subtract(ServiceReq.CreatedDate);
                                double days = GetBusinessDays(ServiceReq.CreatedDate, DateTime.Now);
                            //sLogger.Info("Business days fetched");
                            //double days = (int)span.TotalDays;
                            if (days > retentionDays && retentionDays != 0)
                                    Det.Add(Detail);
                            //}
                        }
                        catch (System.Exception ex)
                        {
                            sLogger.Error("BEQRejectProcess: Error occured in Reject BEQ Exception. EXCEPTION: " + ex.ToString());
                        }
                            
                    }

                    List<ExceptionDTO> Exception = new List<ExceptionDTO>();
                    List<ExceptionDTO> RejectList = new List<ExceptionDTO>();

                    foreach (var item in Det)
                    {
                    
                         if( BEQReject(item, Userid, getDefaultNotes(item.ExceptionTypeid)))
                        {
                            orderSuccessCounter++;
                            successExtRefNumber.Add(item.ExternalRefNum);
                        }                            
                        
                    }                   
                    

                    if (orderSuccessCounter > 0)
                    {
                        body += "Total Number of Rejected Orders: " + orderSuccessCounter + "<br/> <br/>";
                        body += "External Reference Number: " + "<br/>";
                        foreach (var item in successExtRefNumber)
                        {
                            body += item + "<br/>";
                        }


                       // EmailHelper.SendEmail(ConfigurationManager.AppSettings["SenderMailAddress"], ConfigurationManager.AppSettings["Recipient"], ConfigurationManager.AppSettings["SubjectSuccess"], body, null, null, null);
                    }
                    
                }
                catch (System.Exception ex)
                {
                    //Get host name
                    string hostname = "";
                    try
                    { hostname = Dns.GetHostEntry(Dns.GetHostName()).HostName; }
                    catch { hostname = ""; }

                    body += "<b>Error Message : <br/></b><i>" + ex.Message + "</i><br/><br/>";
                    body += "<b>Host Name : <br/></b><i>" + hostname + "</i><br/><br/>";
                    body += "<b>Inner Exception : <br/></b><i>" + ex.InnerException + "</i><br/><br/>";

                   // EmailHelper.SendEmail(ConfigurationManager.AppSettings["SenderMailAddress"], ConfigurationManager.AppSettings["Recipient"], ConfigurationManager.AppSettings["SubjectFailed"], body, null, null, null);
                }          
            }
        }

        private bool IsSupportedTenant(int? tenantID, int? applicationId = null)
        {
            if (tenantID == 1 || (tenantID == 8 && (applicationId == null || applicationId != 34)))
                return true;
            else
                return false;
        }

        private string getDefaultNotes(int exceptionType)
        {
            if (exceptionType == 52)
            {
                return No_Good_Match_DefaultNotes;
            }
            else if (exceptionType == 54)
            {
                return Mismatch_Lender_DefaultNotes;
            }
            else
                return null;
        }
        public string getDefaultNotes_UT(int exceptiontType)
        {
            return getDefaultNotes(exceptiontType);
        }

        int BEQParse(long documentObjectid)
        {
            //var docObjectid = GetMessageContentDocumentObject(documentObjectid);

            BEQParseXMLDTO Data = new BEQParseXMLDTO();
            if (documentObjectid == 0)
                return 0;

            TerminalEntities Context = new TerminalEntities();

            int tenantId = 0;

            if (Context.MessageLogs.Where(se => se.DocumentObjectId == documentObjectid).Count() > 0)
                tenantId = Context.MessageLogs.Where(se => se.DocumentObjectId == documentObjectid).FirstOrDefault().TenantId.Value;
     

            if (tenantId == Convert.ToInt32(DataContracts.Constants.TENANT_ID_RF))
                return 0;

            using (TerminalDocumentEntities DbDocumentcontext = new TerminalDocumentEntities())
            {
                string Content = "";

                try
                {
                    DocumentObject Docobject = DbDocumentcontext.DocumentObjects.Where(se => se.DocumentObjectId == documentObjectid).FirstOrDefault();
                    if (Docobject != null)
                    {
                        Content = Docobject.Object;
                        if (string.IsNullOrEmpty(Content) && !string.IsNullOrEmpty(Docobject.ObjectPath))
                        {
                            Content = File.ReadAllText(Docobject.ObjectPath);
                        }
                    }
                }
                catch
                {
                    return 0;
                }
                //XmlSerializer mySerializer = new XmlSerializer(typeof(Order));
                Order order = Utils.DeSerializeToObject<Order>(Content);
                var servicereq = Context.ServiceRequests.Where(se => se.ServiceRequestId == order.ServiceRequestId).FirstOrDefault();
                if (tenantId == Convert.ToInt32(DataContracts.Constants.TENANT_ID_RF))
                    return 0;
                Data.FastFileIDs = new List<int>();
                if (order.InternalData != null)
                {
                    return order.InternalData.ProviderRegionId;
                }
                else
                {

                    string state = order.Properties != null && order.Properties.Count > 0 ? order.Properties[0].PropertyAddress.StateCode : null;
                    string county = order.Properties != null && order.Properties.Count > 0 ? order.Properties[0].PropertyAddress.CountyName : null;

                    if (servicereq != null)
                    {
                        var custLocationId = servicereq.LocationId.HasValue ? servicereq.LocationId.Value : (int?)null;
                        var custId = (custLocationId.HasValue && custLocationId.Value > 0) ? Context.Locations.Where(se => se.LocationId == custLocationId.Value).FirstOrDefault().CustomerId : (int?)null;
                        var result = Context.GetFASTOfficeMap(servicereq.ProviderId, custLocationId, state, county, custId, null).FirstOrDefault();
                        if (result != null)
                            return result.RegionId;

                    }
                }



                    return 0;
            }
        }
        bool BEQReject(ExceptionDTO matchException, int userId, string fileNotes)
        {
            //sLogger.Info("Starting to reject orders");
            using (TerminalEntities dbContext = new TerminalEntities())
            {

                var exInfo = dbContext.Exceptions.Where(se => se.ExceptionId == matchException.Exceptionid).FirstOrDefault();
                TerminalDocumentEntities DbDocumentcontext = new TerminalDocumentEntities();                
                DocumentObject Docobject = DbDocumentcontext.DocumentObjects.Where(se => se.DocumentObjectId == exInfo.DocumentObjectId).FirstOrDefault();
                var content = Docobject.Object;
                if (string.IsNullOrEmpty(content) && !string.IsNullOrEmpty(Docobject.ObjectPath))
                {
                    content = File.ReadAllText(Docobject.ObjectPath);
                }
                var items = (from t in XDocument.Parse(content)?.Descendants(DataContracts.Constants.EXCEPTION)
                                select new
                                {
                                    Source = t.Element("Source")?.Value,
                                    Destination = t.Element("CurrentSource")?.Value,
                                }).FirstOrDefault();

                MessageLog messageLog;

                bool isRejectSuccess = false;
                if (exInfo.MessageLogId > 0)
                {
                    sLogger.Debug(string.Format("MessageLogId: {0} found", exInfo.MessageLogId));
                    MessageLog MessageLogInfo = dbContext.MessageLogs.Where(se => se.MessageLogId == exInfo.MessageLogId).FirstOrDefault();

                    string documentObjId = "0";
                    if (MessageLogInfo.ParentMessageLogId == null)
                    {
                        documentObjId = MessageLogInfo.DocumentObjectId.ToString();
                    }
                    else
                    {
                        MessageLog OriginalMessageLogInfo = dbContext.MessageLogs.Where(se => se.MessageLogId == MessageLogInfo.ParentMessageLogId).FirstOrDefault();
                        documentObjId = OriginalMessageLogInfo.DocumentObjectId.ToString();
                    }

                    var appId = MessageLogInfo.ServiceRequest.ApplicationId;

                    var messageMapId = (from mm in dbContext.MessageMaps
                                        join mt in dbContext.MessageTypes on mm.MessageTypeId equals mt.MessageTypeId
                                        join mt1 in dbContext.MessageTypes on mm.ExternalMessageTypeId equals mt1.MessageTypeId
                                        where mt1.ApplicationId == appId
                                        && mm.IsInbound == false
                                        && mt.MessageTypeId == 10022
                                        && mm.TenantId == MessageLogInfo.TenantId
                                        select mm.MessageMapId).FirstOrDefault();
                    sLogger.Debug(string.Format("Publishing Reject message Applicationid: {0}", appId));


                    if (messageMapId == 0)
                        return false;
                    messageLog = new MessageLog()
                    {
                        CreatedById = userId,
                        LastModifiedById = userId,
                        CreatedDate = DateTime.Now,
                        LastModifiedDate = DateTime.Now,
                        MessageMapId = messageMapId,
                        TenantId = MessageLogInfo.TenantId,
                        ServiceRequestId = MessageLogInfo.ServiceRequestId,
                        DocumentObjectId = Convert.ToInt64(documentObjId),
                        ParentMessageLogId = MessageLogInfo.ParentMessageLogId,
                        MessageLogDesc = fileNotes,
                        RestartStep = null
                    };

                    dbContext.MessageLogs.Add(messageLog);
                    dbContext.SaveChanges();

                    sLogger.Debug(string.Format("Publishing Reject message DocumentObjectId: {0}", documentObjId));

                    if (EMSAdapter.PublishMessage(items.Source.ToUpper(), "LVIS", MessageLogInfo.ServiceRequestId, messageLog.MessageLogId, documentObjId))
                    {
                        sLogger.Info(string.Format("Publishing Reject message DocumentObjectId: {0}  SUCCESSFUL", documentObjId));
                        isRejectSuccess = true;
                    }
                    else
                    {
                        sLogger.Error(string.Format("Publishing Reject message DocumentObjectId: {0} FAILED", documentObjId));
                    }
                }

                if (isRejectSuccess)
                {
                    string Status = exInfo.TypeCode.TypeCodeDesc;
                    exInfo.TypeCodeId = (int)ExceptionStatusEnum.Resolved;
                    exInfo.LastModifiedById = userId;
                    exInfo.LastModifiedDate = DateTime.Now;
                    dbContext.Entry(exInfo).State = System.Data.Entity.EntityState.Modified;
                    AuditLogHelper.SaveChanges(dbContext);

                    Thread.Sleep(3000);
                    //BEQ Notes
                    ExceptionNote EceptionComment = new ExceptionNote();
                    EceptionComment.ExceptionId = matchException.Exceptionid;
                    EceptionComment.ExceptionNotes = "REJECT completed, exception status changed from " + Status + " to " + ExceptionStatusEnum.Resolved.ToString();
                    EceptionComment.CreatedById = userId;
                    EceptionComment.CreatedDate = DateTime.Now;

                    dbContext.ExceptionNotes.Add(EceptionComment);
                    AuditLogHelper.SaveChanges(dbContext);

                    return true;
                }
                else
                {
                    exInfo.TypeCodeId = (int)ExceptionStatusEnum.Active;
                    exInfo.LastModifiedById = userId;
                    exInfo.LastModifiedDate = DateTime.Now;
                    dbContext.Entry(exInfo).State = System.Data.Entity.EntityState.Modified;
                    AuditLogHelper.SaveChanges(dbContext);

                    Thread.Sleep(3000);
                    //BEQ Notes
                    ExceptionNote EceptionComment = new ExceptionNote();
                    EceptionComment.ExceptionId = matchException.Exceptionid;
                    EceptionComment.ExceptionNotes = "There was an error rejecting/publishing message to EMS";
                    EceptionComment.CreatedById = userId;
                    EceptionComment.CreatedDate = DateTime.Now;

                    dbContext.ExceptionNotes.Add(EceptionComment);
                    AuditLogHelper.SaveChanges(dbContext);

                    return false;
                }                
            }
        }
        public static double GetBusinessDays(DateTime startD, DateTime endD)
        {
            double calcBusinessDays =
                1 + ((endD - startD).TotalDays * 5 -
                (startD.DayOfWeek - endD.DayOfWeek) * 2) / 7;

            if (endD.DayOfWeek == DayOfWeek.Saturday) calcBusinessDays--;
            if (startD.DayOfWeek == DayOfWeek.Sunday) calcBusinessDays--;

            return calcBusinessDays;
        }
    }
}
    
