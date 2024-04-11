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

namespace FA.LVIS.Tower.BEQResubmitProcess
{

    public class BEQResubmitProcess
    {
        Logger sLogger = new Common.Logger();
        IEMSAdapter EMSAdapter;
        
        //Fetch & Resubmit BEQ Exception
        public void FetchResubmitBeqException()
        {
            EMSAdapter = new EMSAdapter();
            int orderSuccessCounter = 0;
            List<string> successExtRefNumber = new List<string>();
            string body = null;

            using (TerminalEntities dbcontext = new TerminalEntities())
            {                
                try
                {
                    int Userid = dbcontext.Tower_Users.Where(se => se.Name == "Resubmit Job - Nightly").FirstOrDefault().UserId;

                    int tenantid = 1;//Agency only
                    int atcTenantId = 8;
                    var exceptionsList = dbcontext.Exceptions.Where(se => (se.TypeCodeId == 201 || se.TypeCodeId == 202 || se.TypeCodeId == 205) && (
                                               tenantid == (se.MessageLogId != 0 ? dbcontext.MessageLogs.Where(ml => ml.MessageLogId == se.MessageLogId).FirstOrDefault().TenantId : 0)
                                               || atcTenantId == (se.MessageLogId != 0 ? dbcontext.MessageLogs.Where(ml => ml.MessageLogId == se.MessageLogId).FirstOrDefault().TenantId : 0)) &&
                                               se.ExceptionType.ExceptionGroupId == (int)ExceptionGroupEnum.BEQ &&
                                               (se.ExceptionTypeId == 52 || se.ExceptionTypeId == 54)).ToList();

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

                        Detail.Status = new ExceptionStatus() { ID = Status.TypeCodeId, Name = Status.TypeCodeDesc };                        
                        Detail.DocumentObjectid = se.DocumentObjectId;
                        Detail.ExternalRefNum = ServiceReq != null ? ServiceReq.ExternalRefNum : string.Empty;
                        Detail.ServiceType = ServiceReq != null ? ServiceReq.ServiceId.ToString() : "0";

                        if (ServiceReq.Application != null)
                        {
                            if (!string.IsNullOrEmpty(ServiceReq.Application.ApplicationName) && (ServiceReq.Application.ApplicationName == DataContracts.Constants.APPLICATION_REALEC
                                || ServiceReq.Application.ApplicationName == DataContracts.Constants.APPLICATION_KEYSTONE))
                            {
                                Detail.ParentExternalRefNum = Detail.ExternalRefNum != null
                                    ? Detail.ExternalRefNum.Substring(0, (Detail.ExternalRefNum.LastIndexOf("-") > -1 == true
                                    ? Detail.ExternalRefNum.LastIndexOf("-") : Detail.ExternalRefNum.Length - 1)) : string.Empty;
                            }

                            else
                            {
                                Detail.ParentExternalRefNum = Detail.ExternalRefNum != null ? Detail.ExternalRefNum : string.Empty;
                            }
                        }

                        Det.Add(Detail);
                    }

                    List<ExceptionDTO> Exception = new List<ExceptionDTO>();
                    List<ExceptionDTO> RejectloweruidsList = new List<ExceptionDTO>();

                    foreach (string item in Det.Select(se => se.ParentExternalRefNum).Distinct())
                    {
                        List<ExceptionDTO> Details = Det.Where(se => se.ParentExternalRefNum == item).ToList();
                        ExceptionDTO Parent = new ExceptionDTO();

                        List<String> ServiceTypes = new List<string>();

                        Parent.children = new List<ExceptionDTO>();

                        if (Details.Count == 1)
                        {
                            Parent = Details[0];
                            Exception.Add(Parent);
                            continue;
                        }

                        var results = Details.GroupBy(se => se.ServiceType);
                        foreach (var list in results)
                        {
                            var child = list.OrderByDescending(se => se.ExternalRefNum).FirstOrDefault();
                            Parent.Exceptionid = 0;
                            Parent.ExternalRefNum = child.ParentExternalRefNum;
                            Parent.ParentExternalRefNum = child.ParentExternalRefNum;
                            Parent.children.Add(child);

                            //Get list of lower uids to reject
                            var LoweruidList = list.Where(re => re.Exceptionid != child.Exceptionid  && (!re.InvolveResolved) &&
                                                 ((child.ParentExternalRefNum != null) && (re.ParentExternalRefNum == child.ParentExternalRefNum)));

                            if (LoweruidList != null)
                            {
                                foreach (var Loweruid in LoweruidList)
                                {
                                    RejectloweruidsList.Add(Loweruid);
                                }
                            }

                        }

                        Exception.Add(Parent);
                    }

                    //Reject lower uids
                    if (RejectloweruidsList.Count > 0)
                    {
                        foreach (var matchException in RejectloweruidsList)
                        {
                            try
                            {
                                BEQReject(matchException, Userid, null);
                            }
                            catch
                            {

                            }
                        }
                    }

                    if (Exception.Count()> 0)
                    {
                        foreach (var ExceptionDetails in Exception)
                        {
                            Exception ExceptionInfo;
                            if (ExceptionDetails.children != null && ExceptionDetails.children.Count() > 0 && ExceptionDetails.Exceptionid == 0)
                            {
                                foreach (var s in ExceptionDetails.children)
                                {
                                    ExceptionInfo = dbcontext.Exceptions.Where(se => se.ExceptionId == s.Exceptionid).FirstOrDefault();

                                    if (ExceptionInfo != null)
                                    {
                                        var dest = DataContracts.Constants.CONVOY;

                                        try
                                        {
                                            AddNotesBEQ(ExceptionInfo, Userid, true, ExceptionDetails);

                                            var result = PostBEQResubmit(ExceptionInfo, Userid, dest, ExceptionDetails.ParentExternalRefNum);

                                            if (result)
                                            {
                                                orderSuccessCounter++;
                                                successExtRefNumber.Add(s.ExternalRefNum);
                                            }
                                            else
                                                AddNotesBEQ(ExceptionInfo, Userid, false, ExceptionDetails);


                                            Thread.Sleep(3000);

                                        }
                                        catch (System.Exception ex1)
                                        {

                                        }
                                    }
                                }
                            }
                            else
                            {
                                ExceptionInfo = dbcontext.Exceptions.Where(se => se.ExceptionId == ExceptionDetails.Exceptionid).FirstOrDefault();

                                if (ExceptionInfo != null)
                                {
                                    try
                                    {
                                        AddNotesBEQ(ExceptionInfo, Userid, true, ExceptionDetails);

                                        var result = PostBEQResubmit(ExceptionInfo, Userid);

                                        if (result)
                                        {
                                            orderSuccessCounter++;
                                            successExtRefNumber.Add(ExceptionDetails.ExternalRefNum);
                                        }
                                        else
                                            AddNotesBEQ(ExceptionInfo, Userid, false, ExceptionDetails);


                                        Thread.Sleep(3000);
                                    }
                                    catch
                                    {

                                    }

                                }
                            }
                        }
                    }

                    if (orderSuccessCounter > 0)
                    {
                        body += "Total Number of Processed Transaction: " + orderSuccessCounter + "<br/> <br/>";
                        body += "External Reference Number: " + "<br/>";
                        foreach (var item in successExtRefNumber)
                        {
                            body += item + "<br/>";
                        }


                        EmailHelper.SendEmail(ConfigurationManager.AppSettings["SenderMailAddress"], ConfigurationManager.AppSettings["Recipient"], ConfigurationManager.AppSettings["SubjectSuccess"], body, null, null, null);
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

                    EmailHelper.SendEmail(ConfigurationManager.AppSettings["SenderMailAddress"], ConfigurationManager.AppSettings["Recipient"], ConfigurationManager.AppSettings["SubjectFailed"], body, null, null, null);
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
        private bool PostBEQResubmit(Exception ExceptionInfo, int userId, string dest = null, string externalRefNum = null)
        {
            var isResubmitSuccess = false;
            MessageLog messageLog;

            using (TerminalEntities dbContext = new TerminalEntities())
            {                
                TerminalDocumentEntities DbDocumentcontext = new TerminalDocumentEntities();
                DocumentObject Docobject = DbDocumentcontext.DocumentObjects.Where(se => se.DocumentObjectId == ExceptionInfo.DocumentObjectId).FirstOrDefault();
                if (Docobject == null)
                    return false;
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

                var docObject = DbDocumentcontext.DocumentObjects.Where(se => se.DocumentObjectId == items.DocumentObjectId).FirstOrDefault();
                var docObjContent = docObject.Object;
                if (string.IsNullOrEmpty(docObjContent) && !string.IsNullOrEmpty(docObject.ObjectPath))
                {
                    docObjContent = File.ReadAllText(docObject.ObjectPath);
                }

                DocumentObject docObj = new DocumentObject()
                {
                    Object = docObjContent,
                    DocumentObjectFormat = "xml",
                    CreatedById = userId,
                    CreatedDate = DateTime.Now
                };
                DbDocumentcontext.DocumentObjects.Add(docObj);
                DbDocumentcontext.SaveChanges();

                MessageLog MessageLogInfo = dbContext.MessageLogs.Where(se => se.MessageLogId == ExceptionInfo.MessageLogId).FirstOrDefault();

                if (MessageLogInfo != null)
                {
                    sLogger.Info(string.Format("MessageLogId: {0} found", ExceptionInfo.MessageLogId));

                    messageLog = new MessageLog()
                    {
                        CreatedById = userId,
                        LastModifiedById = userId,
                        CreatedDate = DateTime.Now,
                        LastModifiedDate = DateTime.Now,
                        MessageMapId = MessageLogInfo.MessageMapId,
                        TenantId = MessageLogInfo.TenantId,
                        ServiceRequestId = MessageLogInfo.ServiceRequestId,
                        DocumentObjectId = docObj.DocumentObjectId,
                        ParentMessageLogId = MessageLogInfo.ParentMessageLogId,
                        MessageLogDesc = MessageLogInfo.MessageLogDesc,
                        RestartStep = Convert.ToInt16(items.RestartStep?.ToString())
                    };
                    dbContext.MessageLogs.Add(messageLog);
                    dbContext.SaveChanges();

                    if (messageLog != null && items != null)
                    {
                        if (string.IsNullOrEmpty(dest))
                            dest = items.Destination?.Replace(DataContracts.Constants.APPLICATION_LVIS, "").Replace("QUEUE", "").Replace(".", "") ?? DataContracts.Constants.ENRICHMENT;
                        sLogger.Debug(string.Format("Publishing resubmit message DocumentObjectId: {0} to Destination: {1}", docObj.DocumentObjectId, dest));

                        if (EMSAdapter.PublishMessage(dest, items.Source, messageLog.ServiceRequestId, messageLog.MessageLogId, docObj.DocumentObjectId.ToString(), externalRefNum, DateTime.Now))
                        {
                            sLogger.Info(string.Format("PublishingMessage resubmit message DocumentObjectId: {0} SUCCESSFUL", docObj.DocumentObjectId));
                            isResubmitSuccess = true;
                        }
                        else
                        {
                            sLogger.Error(string.Format("PublishingMessage resubmit message DocumentObjectId: {0} FAILED", docObj.DocumentObjectId.ToString()));
                        }
                    }
                }
                else
                {
                    sLogger.Info("MessageLogId or Messagelog info missing for Exception");
                    if (items != null)
                    {
                        if (string.IsNullOrEmpty(dest))
                            dest = items.Destination?.Replace(DataContracts.Constants.APPLICATION_LVIS, "").Replace("QUEUE", "").Replace(".", "") ?? DataContracts.Constants.ENRICHMENT;
                        sLogger.Debug(string.Format("Publishing resubmit message DocumentObjectId: {0} to Destination: {1}", docObj.DocumentObjectId, dest));

                        if (EMSAdapter.PublishMessage(dest, items.Source, docObj.DocumentObjectId))
                        {
                            sLogger.Info(string.Format("Publishing resubmit message DocumentObjectId: {0}  SUCCESSFUL", docObj.DocumentObjectId));
                            isResubmitSuccess = true;
                        }
                        else
                        {
                            sLogger.Error(string.Format("Publishing resubmit message DocumentObjectId: {0} FAILED", docObj.DocumentObjectId.ToString()));
                        }
                    }
                }                
            }

            return isResubmitSuccess;
        }

        private void AddNotesBEQ(Exception ExceptionInfo, int userId, bool isResubmitSuccess, ExceptionDTO ExceptionDetails)
        {
            using (TerminalEntities dbContext = new TerminalEntities())
            {             
                var exInfo = dbContext.Exceptions.Where(se => se.ExceptionId == ExceptionInfo.ExceptionId).FirstOrDefault();

                if (isResubmitSuccess)
                {
                    exInfo.LastModifiedById = userId;
                    exInfo.LastModifiedDate = DateTime.Now;
                    var status = dbContext.TypeCodes.Where(se => se.TypeCodeId == ExceptionInfo.TypeCodeId).Select(se => se.TypeCodeDesc).FirstOrDefault();
                    var note = new ExceptionNote()
                    {
                        CreatedById = userId,
                        ExceptionId = ExceptionInfo.ExceptionId,
                        CreatedDate = DateTime.Now,
                        ExceptionNotes = "Resubmitted, exception status changed from " + status + " to " + ExceptionStatusEnum.Resolved.ToString()
                    };

                    exInfo.TypeCodeId = (int)ExceptionStatusEnum.Resolved;

                    exInfo.ExceptionNotes.Add(note);

                    dbContext.Entry(exInfo).State = System.Data.Entity.EntityState.Modified;
                    AuditLogHelper.SaveChanges(dbContext);
                }
                else
                {
                    exInfo.TypeCodeId = (int)ExceptionStatusEnum.Active;
                    exInfo.LastModifiedById = userId;
                    exInfo.LastModifiedDate = DateTime.Now;
                    exInfo.ExceptionNotes.Add(
                        new ExceptionNote()
                        {
                            CreatedById = userId,
                            ExceptionId = ExceptionInfo.ExceptionId,
                            CreatedDate = DateTime.Now,
                            ExceptionNotes = "There was an error resubmitting/publishing message to EMS"
                        });
                    dbContext.Entry(exInfo).State = System.Data.Entity.EntityState.Modified;
                    AuditLogHelper.SaveChanges(dbContext);
                }                
            }
        }

        bool BEQReject(ExceptionDTO matchException, int userId, string fileNotes)
        {
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

                bool isResubmitSuccess = false;
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
                        isResubmitSuccess = true;
                    }
                    else
                    {
                        sLogger.Error(string.Format("Publishing Reject message DocumentObjectId: {0} FAILED", documentObjId));
                    }
                }

                if (isResubmitSuccess)
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
    }
}
    
