using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Xml;
using System.Xml.Linq;
using FA.LVIS.Tower.DataContracts;
using FA.LVIS.Tower.Data.TerminalDBEntities;
using System.Data.Entity;

namespace FA.LVIS.Tower.Data
{
    public class ReportingDataProvider : Core.DataProviderBase, IReportingDataProvider
    {
        public List<ReportingDTO> GetLVISServiceRequests(SearchDetail value, int tenantId)
        {
            var Reports = new List<ReportingDTO>();

            DateTime startDateTime = Convert.ToDateTime(value.Fromdate);

            DateTime SearchstartDateTime = startDateTime.Subtract(startDateTime.TimeOfDay);

            DateTime endDateTime = Convert.ToDateTime(value.ThroughDate);
            endDateTime = endDateTime.Subtract(startDateTime.TimeOfDay).AddDays(1).AddMilliseconds(-1);

            List<Tenant> tenants = null;


            using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            }))
            {
                using (var dbContext = new TerminalDBEntities.Entities())
                {
                    tenants = dbContext.Tenants.ToList();

                    Reports = GetReports(dbContext, tenantId, startDateTime, endDateTime);

                }
                scope.Complete();
            }

            if ((Reports != null) &&
               (tenantId == tenants?.Where(te => te.TenantName == Constants.APPLICATION_RF)
                                               .Select(te => te.TenantId).FirstOrDefault()))
            {
                Reports = GetLVISActionType(Reports, tenantId);

            }

            return Reports;
        }

        public MessageLogDTO GetServiceReportDetail(int iServiceRequestid)
        {
            TerminalDBEntities.Entities dbContext = new TerminalDBEntities.Entities();
            TerminalDBEntities.TerminalDocumentEntities DbDocumentcontext = new TerminalDBEntities.TerminalDocumentEntities();

            if (iServiceRequestid == 0)
                return new MessageLogDTO();

            int? iTenant = dbContext.ServiceRequests.Where(se => se.ServiceRequestId == iServiceRequestid).FirstOrDefault().TenantId;

            ReportingDTO ReportDet = dbContext.ServiceRequests.Where(se => se.ServiceRequestId == iServiceRequestid).ToList()
                                               .Select(se => new ReportingDTO()
                                               {
                                                   ServiceRequestId = se.ServiceRequestId,
                                                   service = se.Service.ServiceName,
                                                   ApplicationId = se.Application.ApplicationName,
                                                   createddate = se.CreatedDate.ToString("yyyy-MM-dd HH:mm:ss.fff tt"),
                                                   CustomerName = se.Location.Customer.CustomerName,
                                                   CustomerId = se.Location.CustomerId,
                                                   ExternalRefNum = se.ExternalRefNum,
                                                   InternalRefNum = se.InternalRefNum,
                                                   InternalRefId = se.InternalRefId.ToString(),
                                                   CustomerRefNum = se.CustomerRefNum,
                                                   Tenant = se.Tenant.TenantName
                                               }).FirstOrDefault();

        
            List<MessageLogDetailDTO> MessageLogs = new List<MessageLogDetailDTO>();

            if (iTenant == (int)TenantIdEnum.RF)
            {
                var MesageLog = dbContext.MessageLogs.Where(sel => sel.ServiceRequestId == iServiceRequestid && sel.TenantId == iTenant)
                  .Select(se => new
                  {
                      Application = se.MessageMap.MessageType.Application.ApplicationName,
                      ExternalApplication = dbContext.MessageTypes.Where(sel => sel.MessageTypeId == se.MessageMap.ExternalMessageTypeId).Select(app => app.Application.ApplicationName).FirstOrDefault(),
                      isIncoming = se.MessageMap.IsInbound,
                      Objectid = se.DocumentObjectId,
                      description = dbContext.MessageTypes.Where(sel => sel.MessageTypeId == se.MessageMap.ExternalMessageTypeId).Select(app => app.MessageTypeName).FirstOrDefault(),
                      Exceptions = dbContext.Exceptions.Where(El => El.MessageLogId == se.MessageLogId).FirstOrDefault(),
                      ParentMessageLogId = se.ParentMessageLogId,
                      MessageLogid = se.MessageLogId,
                      CreatedDateTime = se.CreatedDate
                  }).ToList();

                MessageLogs = MesageLog.AsEnumerable()
                                 .Select(x => new MessageLogDetailDTO()
                                 {
                                     Application = x.Application,
                                     ExternalApplication = x.ExternalApplication,
                                     IsIncoming = x.isIncoming,
                                     Documentobjectid = x.Objectid,
                                     CreatedDateTime = x.CreatedDateTime.ToString("yyyy-MM-dd HH:mm:ss.fff tt"),
                                     Description = x.description,
                                     ExceptionDescription = x.Exceptions?.ExceptionDesc ?? string.Empty,
                                     ParentMessageLogId = x.ParentMessageLogId.GetValueOrDefault(),
                                     MessageLogId = x.MessageLogid,
                                     ExceptionId = x.Exceptions?.ExceptionId ?? 0,
                                     hasChild = dbContext.MessageLogs.Where(se => se.ParentMessageLogId == x.MessageLogid).Count() > 0

                                 }).OrderByDescending(sl => sl.MessageLogId).ToList();


            }
            else
            {
                var MesageLog = dbContext.MessageLogs.Where(sel => sel.ServiceRequestId == iServiceRequestid && sel.TenantId == iTenant)
                .Select(se => new
                {
                    Application = se.MessageMap.MessageType.Application.ApplicationName,
                    ExternalApplication = dbContext.MessageTypes.Where(sel => sel.MessageTypeId == se.MessageMap.ExternalMessageTypeId).Select(app => app.Application.ApplicationName).FirstOrDefault(),
                    isIncoming = se.MessageMap.IsInbound,
                    Objectid = se.DocumentObjectId,
                    description = dbContext.MessageTypes.Where(sel => sel.MessageTypeId == se.MessageMap.MessageTypeId).Select(app => app.MessageTypeName).FirstOrDefault(),
                    Exceptions = dbContext.Exceptions.Where(El => El.MessageLogId == se.MessageLogId).FirstOrDefault(),
                    ParentMessageLogId = se.ParentMessageLogId,
                    MessageLogid = se.MessageLogId,
                    CreatedDateTime = se.CreatedDate
                }).ToList();

                MessageLogs = MesageLog.AsEnumerable()
                                      .Select(x => new MessageLogDetailDTO()
                                      {
                                          Application = x.Application,
                                          ExternalApplication = x.ExternalApplication,
                                          IsIncoming = x.isIncoming,
                                          Documentobjectid = x.Objectid,
                                          CreatedDateTime = x.CreatedDateTime.ToString("yyyy-MM-dd HH:mm:ss.fff tt"),
                                          Description = x.description,
                                          ExceptionDescription = x.Exceptions?.ExceptionDesc ?? string.Empty,
                                          ParentMessageLogId = x.ParentMessageLogId.GetValueOrDefault(),
                                          MessageLogId = x.MessageLogid,
                                          ExceptionId = x.Exceptions?.ExceptionId ?? 0,
                                          hasChild = dbContext.MessageLogs.Where(se => se.ParentMessageLogId == x.MessageLogid).Count() > 0

                                      }).OrderByDescending(sl => sl.MessageLogId).ToList();
            }

            return new MessageLogDTO() { ReportDetails = ReportDet, MessageLogDetails = MessageLogs };
        }

        public static string FormatXML(string xmlData)
        {
            string returnValue = string.Empty;

            MemoryStream memoryStream = null;
            XmlTextWriter xmlTextWriter = null;
            XmlDocument xmlDocument = null;
            StreamReader streamReader = null;

            try
            {
                memoryStream = new MemoryStream();
                xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
                xmlDocument = new XmlDocument();
                xmlDocument.XmlResolver = null;
                // Load the XmlDocument with the XML.
                xmlDocument.LoadXml(xmlData);

                xmlTextWriter.Formatting = Formatting.Indented;

                // Write the XML into a formatting XmlTextWriter
                xmlDocument.WriteContentTo(xmlTextWriter);
                xmlTextWriter.Flush();
                memoryStream.Flush();

                // Have to rewind the MemoryStream in order to read
                // its contents.
                memoryStream.Position = 0;

                // Read MemoryStream contents into a StreamReader.
                //StreamReader streamReader = new StreamReader(memoryStream); //Coomented Based on Veracode Reported Memory Leak issues
                streamReader = new StreamReader(memoryStream);

                // Extract the text from the StreamReader.
                String FormattedXML = streamReader.ReadToEnd();

                returnValue = FormattedXML.TrimStart();
            }
            catch (XmlException)
            {
                
                return xmlData;
            }
            catch (System.Exception ex)
            {
               
                //Logger.Error("In Utils.FormatXML", ex);
                throw ex;
            }
            finally
            {
                if (xmlTextWriter != null)
                {
                    xmlTextWriter.Close();
                }

                if (memoryStream != null)
                {
                    memoryStream.Close();
                }

                //chnages added to avoid Memoty leak reported by Veracode scanning
                if(streamReader != null)
                {
                    streamReader.Close();
                }

               
            }

            return returnValue;
        }

        public List<ReportingDTO> GetLVISServiceRequests(string sFilter, int tenantId)
        {

            var Reports = new List<ReportingDTO>();
            DateTime startDateTime = DateTime.Today;
            DateTime endDateTime = DateTime.Today;
            //last24 hr format
            if (sFilter.Contains("24"))
            {
                startDateTime = DateTime.Now;
                startDateTime = startDateTime.AddDays(-1);
                endDateTime = DateTime.Now;

            }
            else
            {
                startDateTime = startDateTime.AddDays(-(int.Parse(sFilter)));
                endDateTime = endDateTime.Subtract(startDateTime.TimeOfDay).AddDays(1).AddMilliseconds(-1);
            }
            List<Tenant> tenants = null;


            using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            }))
            {
                using (var dbContext = new TerminalDBEntities.Entities())
                {
                    tenants = dbContext.Tenants.ToList();

                    Reports = GetReports(dbContext, tenantId, startDateTime, endDateTime);

                }
                scope.Complete();
            }

            if ((Reports != null) &&
               (tenantId == tenants?.Where(te => te.TenantName == Constants.APPLICATION_RF)
                                               .Select(te => te.TenantId).FirstOrDefault()))
            {
                Reports = GetLVISActionType(Reports, tenantId);

            }

            return Reports;
        }

        public List<ReportingDTO> GetLVISActionType(List<ReportingDTO> reports, int tenantId)
        {
            TerminalDBEntities.TerminalDocumentEntities dbContextTerminalDoc = new TerminalDBEntities.TerminalDocumentEntities();

            var dbContext = new TerminalDBEntities.Entities();

            foreach (ReportingDTO val in reports)
            {
                if (val.ServiceRequestId > 0)
                {
                    var messagelog = dbContext.MessageLogs.Where(sel => sel.ServiceRequestId == val.ServiceRequestId && sel.TenantId == tenantId
                      && sel.MessageMap.IsInbound).OrderByDescending(se => se.CreatedDate).FirstOrDefault();

                    val.LVISActionType = messagelog?.MessageLogDesc;

                    //if (val.LVISActionType == null && messagelog != null)
                    //{
                    //    string Content = "";
                    //    DocumentObject Docobject = dbContextTerminalDoc.DocumentObjects.Where(se => se.DocumentObjectId == messagelog.DocumentObjectId).FirstOrDefault();
                    //    if (Docobject != null)
                    //    {
                    //        Content = Docobject.Object;
                    //        if (string.IsNullOrEmpty(Content) && !string.IsNullOrEmpty(Docobject.ObjectPath))
                    //        {
                    //            Content = File.ReadAllText(Docobject.ObjectPath);
                    //        }
                    //    }
                    //    var lvisActionType = ((System.Xml.Linq.XElement)((System.Xml.Linq.XContainer)((System.Xml.Linq.XContainer)(XDocument.Parse(Content)).FirstNode).FirstNode).FirstNode).Value;
                    //    val.LVISActionType = lvisActionType;
                    //}

                }
            }

            return reports;
        }

        public List<ReportingDTO> GetReports(TerminalDBEntities.Entities dbContext, int tenantId, DateTime startDateTime, DateTime endDateTime)
        {
            var Reports = new List<ReportingDTO>();
            TerminalDocumentEntities dbContextTerminalDoc = new TerminalDocumentEntities();
            string sTenantName = dbContext.Tenants.Where(se => se.TenantId == tenantId).Select(val => val.TenantName).FirstOrDefault();
            var ReportQuery = (from sr in dbContext.ServiceRequests.Where(sel => sel.CreatedDate >= startDateTime && sel.CreatedDate <= endDateTime &&
                                          ((sTenantName.ToLower() == TerminalDBEntities.ApplicationEnum.LVIS.ToString().ToLower() && sel.TenantId.ToString() != Constants.TENANT_ID_RF) ||
                                          (sel.TenantId == tenantId)))
                               join ty in dbContext.TypeCodes on sr.StatusTypeCodeId equals ty.TypeCodeId
                               join te in dbContext.Tenants on sr.TenantId equals te.TenantId
                               select new
                               {
                                   ServiceRequestId = sr.ServiceRequestId,
                                   service = sr.Service.ServiceName,
                                   ApplicationId = sr.Application.ApplicationName,
                                   createddate = sr.CreatedDate,
                                   CustomerName = sr.Location.Customer.CustomerName,
                                   CustomerId = sr.Location.CustomerId,
                                   ExternalRefNum = sr.ExternalRefNum,
                                   InternalRefNum = sr.InternalRefNum,
                                   InternalRefId = sr.InternalRefId.ToString(),
                                   CustomerRefNum = sr.CustomerRefNum,
                                   TenantName = sr.Tenant.TenantName,
                                   OrderStatus = ty.TypeCodeDesc,
                                   TenantId = te.TenantId
                               });

            Reports = ReportQuery.AsEnumerable()
                .Select(se => new ReportingDTO()
                {
                    ServiceRequestId = se.ServiceRequestId,
                    service = se.service,
                    ApplicationId = se.ApplicationId,
                    createddate = se.createddate.ToString("yyyy-MM-dd HH:mm:ss.fff tt"),
                    CustomerName = se.CustomerName,
                    CustomerId = se.CustomerId,
                    ExternalRefNum = se.ExternalRefNum,
                    InternalRefNum = se.InternalRefNum,
                    InternalRefId = se.InternalRefId.ToString(),
                    CustomerRefNum = se.CustomerRefNum,
                    Tenant = se.TenantName,
                    OrderStatus = se.OrderStatus
                }).OrderByDescending(sl => sl.createddate).ToList();

            return Reports;
        }

        public List<ReportingDTO> GetLVISServiceRequestsbyReferenceNo(SearchDetail value, int tenantId)
        {
            var Reports = new List<ReportingDTO>();

            List<Tenant> tenants = null;
            

            using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            }))
            {
                using (var dbContext = new TerminalDBEntities.Entities())
                {
                    string sTenantName = dbContext.Tenants.Where(se => se.TenantId == tenantId).Select(val => val.TenantName).FirstOrDefault();
                    List<TerminalDBEntities.ServiceRequest> Details = new List<TerminalDBEntities.ServiceRequest>();

                    if (tenantId == (int)TerminalDBEntities.TenantIdEnum.LVIS)
                    {
                        string RefrenceNo = value.ReferenceNo.ToString();
                        if (value.ReferenceNoType == "1" && !string.IsNullOrEmpty(value.ReferenceNo))
                        {
                            Details = dbContext.ServiceRequests.Where(sel => sel.ExternalRefNum.Contains(value.ReferenceNo)
                            && sel.TenantId != dbContext.Tenants.Where(se => se.TenantName == Constants.APPLICATION_RF).Select(val => val.TenantId).FirstOrDefault()).ToList();
                        }
                        else if (value.ReferenceNoType == "2" && !string.IsNullOrEmpty(value.ReferenceNo))
                        {
                            Details = dbContext.ServiceRequests.Where(sel => sel.InternalRefNum.Contains(value.ReferenceNo)
                            && sel.TenantId != dbContext.Tenants.Where(se => se.TenantName == Constants.APPLICATION_RF).Select(val => val.TenantId).FirstOrDefault()).ToList();
                        }
                        else if (value.ReferenceNoType == "3" && !string.IsNullOrEmpty(value.ReferenceNo))
                        {
                            Details = dbContext.ServiceRequests.Where(sel => sel.CustomerRefNum.Contains(value.ReferenceNo)
                            && sel.TenantId != dbContext.Tenants.Where(se => se.TenantName == Constants.APPLICATION_RF).Select(val => val.TenantId).FirstOrDefault()).ToList();
                        }
                        else if (value.ReferenceNoType == "4" && !string.IsNullOrEmpty(value.ReferenceNo))
                        {
                            Details = dbContext.ServiceRequests.Where(sel => sel.InternalRefId.ToString().Contains(value.ReferenceNo)
                            && sel.TenantId != dbContext.Tenants.Where(se => se.TenantName == Constants.APPLICATION_RF).Select(val => val.TenantId).FirstOrDefault()).ToList();
                        }
                    }
                    else
                    {
                        if (value.ReferenceNoType == "1" && !string.IsNullOrEmpty(value.ReferenceNo))
                        {
                            Details = dbContext.ServiceRequests.Where(sel => sel.TenantId == tenantId
                           && sel.ExternalRefNum.Contains(value.ReferenceNo.Trim())).ToList();
                        }
                        else if (value.ReferenceNoType == "2" && !string.IsNullOrEmpty(value.ReferenceNo))
                        {
                            Details = dbContext.ServiceRequests.Where(sel => sel.TenantId == tenantId
                        && sel.InternalRefNum.Contains(value.ReferenceNo.Trim())).ToList();
                        }
                        else if (value.ReferenceNoType == "3" && !string.IsNullOrEmpty(value.ReferenceNo))
                        {
                            Details = dbContext.ServiceRequests.Where(sel => sel.TenantId == tenantId
                            && sel.CustomerRefNum.Contains(value.ReferenceNo.Trim())).ToList();
                        }

                        else if (value.ReferenceNoType == "4" && !string.IsNullOrEmpty(value.ReferenceNo))
                        {
                            Details = dbContext.ServiceRequests.Where(sel => sel.TenantId == tenantId
                            && sel.InternalRefId.ToString().Contains(value.ReferenceNo.Trim())).ToList();
                        }
                    }

                    Reports = Details.OrderByDescending(sl => sl.CreatedDate.ToString("yyyy-MM-dd HH:mm:ss.fff tt"))
                    .Select(se => new ReportingDTO()
                    {
                        ServiceRequestId = se.ServiceRequestId,
                        service = se.Service.ServiceName,
                        ApplicationId = se.Application.ApplicationName,
                        createddate = se.CreatedDate.ToString("yyyy-MM-dd HH:mm:ss.fff tt"),
                        CustomerName = se.Location.Customer.CustomerName,
                        CustomerId = se.Location.CustomerId,
                        ExternalRefNum = se.ExternalRefNum,
                        InternalRefNum = se.InternalRefNum,
                        InternalRefId = se.InternalRefId.ToString(),
                        CustomerRefNum = se.CustomerRefNum,
                        Tenant = se.Tenant.TenantName,
                        OrderStatus = dbContext.TypeCodes.Where(ty => ty.TypeCodeId == se.StatusTypeCodeId).Select(ty => ty.TypeCodeDesc).FirstOrDefault(),
                    }).ToList();

                    tenants = dbContext.Tenants.ToList();
                }

                scope.Complete();
            }

            if (tenantId == tenants.Where(te => te.TenantName == Constants.APPLICATION_RF)
                                    .Select(te => te.TenantId).FirstOrDefault())
            {
                Reports = GetLVISActionType(Reports, tenantId);

            }

            return Reports;
        }

        public string[] InvalidateOrderData(ReportingDTO[] values, int tenantId, int userId)
        {
            List<string> orderList = new List<string>();
            TerminalDBEntities.Entities dbContext = new TerminalDBEntities.Entities();
            foreach (ReportingDTO ordervalue in values) {

                //update Service Request Table                
                if (dbContext.ServiceRequests.Where(se => se.ServiceRequestId == ordervalue.ServiceRequestId).Count() > 0) {
                    var updateEntity = dbContext.ServiceRequests.Where(se => se.ServiceRequestId == ordervalue.ServiceRequestId).FirstOrDefault();
                    if (updateEntity.ApplicationId == (int)ApplicationEnum.TitlePort)
                    {
                        var titlePortlookups = dbContext.TitlePortLookUps.Where(tp => tp.ExternalRefNumber == updateEntity.ExternalRefNum && tp.ServiceId == updateEntity.ServiceId);
                        foreach (TitlePortLookUp titlePortLookup in titlePortlookups)
                        {
                            titlePortLookup.FileStatus = "Invalidated";
                            dbContext.Entry(titlePortLookup).State = System.Data.Entity.EntityState.Modified;
                          
                        }
                        AuditLogHelper.SaveChanges(dbContext);
                    }

                    if (ordervalue.ExternalRefNum.Length <= 47)
                    {
                        updateEntity.ExternalRefNum = "INV" + ordervalue.ExternalRefNum;
                        updateEntity.CustomerRefNum = "INV" + ordervalue.CustomerRefNum;
                        updateEntity.StatusTypeCodeId = dbContext.TypeCodes.Where(a => a.TypeCodeDesc == DataContracts.Constants.INVALIDATED).Select(a => a.TypeCodeId).FirstOrDefault();
                        updateEntity.LastModifiedById = userId;
                        updateEntity.LastModifiedDate = DateTime.Now;
                        dbContext.Entry(updateEntity).State = System.Data.Entity.EntityState.Modified;
                        AuditLogHelper.SaveChanges(dbContext);
                    }
                    else if (ordervalue.ExternalRefNum.Length < 50)
                    {
                        updateEntity.ExternalRefNum = ordervalue.ExternalRefNum.Length == 48 ? "IN" + ordervalue.ExternalRefNum : "I" + ordervalue.ExternalRefNum;
                        updateEntity.CustomerRefNum = ordervalue.CustomerRefNum.Length == 48 ? "IN" + ordervalue.CustomerRefNum : "I" + ordervalue.CustomerRefNum;
                        updateEntity.StatusTypeCodeId = dbContext.TypeCodes.Where(a => a.TypeCodeDesc == DataContracts.Constants.INVALIDATED).Select(a => a.TypeCodeId).FirstOrDefault();
                        updateEntity.LastModifiedById = userId;
                        updateEntity.LastModifiedDate = DateTime.Now;
                        dbContext.Entry(updateEntity).State = System.Data.Entity.EntityState.Modified;
                        AuditLogHelper.SaveChanges(dbContext);
                    }
                    else
                    {
                        orderList.Add(ordervalue.ServiceRequestId.ToString());
                    }
                }
            }
                
            return orderList.ToArray();
        }
    }
}
