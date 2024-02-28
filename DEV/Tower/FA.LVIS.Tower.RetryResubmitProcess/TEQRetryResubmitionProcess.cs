using System;
using System.Linq;
using System.Text;
using LVIS.Adapters.EMSAdapter;
using System.Xml.Linq;
using FA.LVIS.Tower.DataContracts;
using System.Net.Mail;
using System.Net;
using System.Configuration;
using System.IO;
using LVIS.Infrastructure.Logging;
using FA.LVIS.Tower.Data;
using FA.LVIS.Tower.ResubmitProcess;

namespace FA.LVIS.Tower.RetryResubmitProcess
{

    public class TEQRetryResubmitionProcess
    {
       
        IEMSAdapter EMSAdapter;


        //Fetch & Resubmit TEQ Exception
        public void FetchResubmitTeqException()
        {
            try
            {
                EMSAdapter = new EMSAdapter();
                Common.Logger sLogger = new Common.Logger();
                double retryInterval = Convert.ToDouble(System.Configuration.ConfigurationManager.AppSettings["RetryInterval"]);
                
                DateTime Startdate = DateTime.Now.AddMinutes(retryInterval);
                using (TerminalEntities dbcontext = new TerminalEntities())
                {
                        var ExceptionResubmitted = dbcontext.Exceptions.Where(se => se.TypeCodeId == 201 &&
                        se.ExceptionType.ExceptionGroupId == (int)ExceptionGroupEnum.TEQ  
                        && se.ExceptionTypeId == 87).Select(x => new { Excecptionid = x.ExceptionId }).ToList();

                        if (ExceptionResubmitted != null)
                        {

                        int Userid = dbcontext.Tower_Users.Where(se => se.Name == "Resubmit Job - Hourly").FirstOrDefault().UserId;
                        foreach (var item in ExceptionResubmitted)
                        {
                            try
                            {
                                ResubmitProcess.Exception ExceptionInfo = dbcontext.Exceptions.Where(se => se.ExceptionId == item.Excecptionid).FirstOrDefault();
                                if (ExceptionInfo.MessageLogId != 0)
                                {
                                    MessageLog MessageLogInfo = dbcontext.MessageLogs.Where(se => se.MessageLogId == ExceptionInfo.MessageLogId).FirstOrDefault();

                                    if (MessageLogInfo.ServiceRequest.ApplicationId == 28 && ExceptionInfo.CreatedDate >= Startdate && MessageLogInfo.MessageMap.MessageTypeId == 12101)
                                        continue;
                                }

                                TerminalDocumentEntities DbDocumentcontext = new TerminalDocumentEntities();
                                DocumentObject Docobject = DbDocumentcontext.DocumentObjects.Where(se => se.DocumentObjectId == ExceptionInfo.DocumentObjectId).FirstOrDefault();
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
                                                 ServiceReqId = t.Element("ServiceRequestId")?.Value,
                                                 MessageLogId = Convert.ToInt32(t.Element("MessageLogId")?.Value),
                                                 RestartStep = t.Element("FailedStepNumber")?.Value,
                                                 DocumentObjectId = t.Element("DocumentObjectId")?.Value,
                                                 ExternalRefNumber = t.Element("ExternalRefNum")?.Value,
                                                 TagRef = t.Element("TagRef")?.Value,
                                                 OrderSourceId = t.Element("OrderSourceID")?.Value,
                                                 SecondOrderSourceID = t.Element("SecondOrderSourceID")?.Value,
                                                 ProcessName = t.Element("ProcessName")?.Value,
                                                 ProcessType = t.Element("ProcessType")?.Value,
                                                 ObjectCD = t.Element("ObjectCD")?.Value,
                                                 ServiceFileProcessID = t.Element("ServiceFileProcessID")?.Value,
                                                 RegionID = t.Element("RegionID")?.Value,
                                                 Retry = Convert.ToInt32(t.Element("Retry")?.Value)
                                             }).FirstOrDefault();


                                var DocumentObjectId = dbcontext.MessageLogs.Where(se => se.MessageLogId == ExceptionInfo.MessageLogId).Select(s => s.DocumentObjectId).FirstOrDefault();

                                if (DocumentObjectId == 0)
                                    DocumentObjectId = Convert.ToInt64(items.DocumentObjectId);

                                if (DocumentObjectId == 0)
                                    continue;
                                DocumentObject Docobject1 = DbDocumentcontext.DocumentObjects.Where(se => se.DocumentObjectId == DocumentObjectId).FirstOrDefault();
                                var content1 = Docobject1.Object;
                                if (string.IsNullOrEmpty(content1) && !string.IsNullOrEmpty(Docobject1.ObjectPath))
                                {
                                    content1 = File.ReadAllText(Docobject1.ObjectPath);
                                }
                                DocumentObject docObj = new DocumentObject()
                                {
                                    Object = content1,
                                    DocumentObjectFormat = "xml",
                                    CreatedById = Userid,
                                    CreatedDate = DateTime.Now
                                };
                                DbDocumentcontext.DocumentObjects.Add(docObj);
                                DbDocumentcontext.SaveChanges();

                               sLogger.Info(string.Format("New DocumentObject added successfully, DocumentObjectId: {0}", items.DocumentObjectId));

                                var isResubmitSuccess = false;
                                MessageLog messageLog;

                                if (ExceptionInfo.MessageLogId > 0)
                                {
                                   sLogger.Info(string.Format("MessageLogId: {0} found", ExceptionInfo.MessageLogId));
                                    MessageLog MessageLogInfo = dbcontext.MessageLogs.Where(se => se.MessageLogId == ExceptionInfo.MessageLogId).FirstOrDefault();
                                    if (MessageLogInfo == null)
                                        continue;
                                    messageLog = new MessageLog()
                                    {
                                        CreatedById = Userid,
                                        LastModifiedById = 0,
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
                                    dbcontext.MessageLogs.Add(messageLog);
                                    dbcontext.SaveChanges();

                                    if (messageLog != null && items != null)
                                    {
                                        int retry = int.Parse(items.Retry.ToString());

                                        retry++;

                                        string dest = items.Destination?.Replace(DataContracts.Constants.APPLICATION_LVIS, "").Replace("QUEUE", "").Replace(".", "") ?? DataContracts.Constants.ENRICHMENT;
                                       // sLogger.Debug(string.Format("Publishing resubmit message DocumentObjectId: {0} to Destination: {1}", docObj.DocumentObjectId, dest));

                                        if (EMSAdapter.PublishMessage(dest, items.Source, messageLog.ServiceRequestId, messageLog.MessageLogId, docObj.DocumentObjectId.ToString(), retry))
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

                                int LastModifiedId = dbcontext.Tower_Users.Where(se => se.Name == "Resubmit Job - Hourly").FirstOrDefault().UserId;


                                if (isResubmitSuccess)
                                {
                                    ExceptionInfo.TypeCodeId = (int)ExceptionStatusEnum.Resolved;
                                    ExceptionInfo.LastModifiedById = LastModifiedId;
                                    ExceptionInfo.LastModifiedDate = DateTime.Now;
                                    ExceptionInfo.ExceptionNotes.Add(
                                        new ExceptionNote()
                                        {
                                            CreatedById = Userid,
                                            ExceptionId = item.Excecptionid,
                                            CreatedDate = DateTime.Now,
                                            ExceptionNotes = "Resubmitted, exception status changed from " + ExceptionStatusEnum.Resubmitted + " to " + ExceptionStatusEnum.Resolved.ToString()
                                        });

                                    dbcontext.Entry(ExceptionInfo).State = System.Data.Entity.EntityState.Modified;
                                    AuditLogHelper.SaveChanges(dbcontext);
                                }
                                else
                                {
                                    ExceptionInfo.TypeCodeId = (int)ExceptionStatusEnum.Active;
                                    ExceptionInfo.LastModifiedById = LastModifiedId;
                                    ExceptionInfo.LastModifiedDate = DateTime.Now;
                                    ExceptionInfo.ExceptionNotes.Add(
                                        new ExceptionNote()
                                        {
                                            CreatedById = Userid,
                                            ExceptionId = item.Excecptionid,
                                            CreatedDate = DateTime.Now,
                                            ExceptionNotes = "There was an error resubmitting/publishing message to EMS"
                                        });
                                    dbcontext.Entry(ExceptionInfo).State = System.Data.Entity.EntityState.Modified;
                                    AuditLogHelper.SaveChanges(dbcontext);


                                }
                            }
                            catch (System.Exception Except)
                            {
                                sLogger.Error("Error in resubmit",Except);
                            }
                        }
                        }
                }
            }
            catch (System.Exception ex)
            {
           
            }
        }



    }
}
