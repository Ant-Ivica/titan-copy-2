using FA.LVIS.Tower.Common;
using FA.LVIS.Tower.Data.TerminalDBEntities;
using FA.LVIS.Tower.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using LVIS.DTO.Canonical.Order;
using System.Xml.Serialization;
using System.IO;
using LVIS.Common;
using FA.LVIS.Tower.Core;
using System.Transactions;
using System.Threading.Tasks;
using System.Net.Mail;
using EMS =   LVIS.Adapters.EMSAdapter;
using ExceptionType = FA.LVIS.Tower.DataContracts.ExceptionType;
using FA.LVIS.Tower.Core;
using System.Web;
using FA.LVIS.Tower.WebhookProcessing;

namespace FA.LVIS.Tower.Data
{
    public class ExceptionDataProvider : Core.DataProviderBase, IExceptionDataProvider
    {
        Logger sLogger = new Common.Logger(typeof(ExceptionDataProvider));
        IUtils Utils;
        EMS.IEMSAdapter EMSAdapter;

        public ExceptionDataProvider()
        {
            Utils = new Utils();
            EMSAdapter = new EMS.EMSAdapter();
        }

        #region " BEQ "

        public IEnumerable<DashBoardExceptionDTO> GetBEQExceptions(int tenantId)
        {

            Entities LvisDbcontext = new Entities();

            List<DataContracts.DashBoardExceptionDTO> ExceptionsList = new List<DataContracts.DashBoardExceptionDTO>();

            var resultIds = LvisDbcontext.ExceptionTypes.Where(se => se.ExceptionGroupId == (int)ExceptionGroupEnum.BEQ);


            if (tenantId == (int)TerminalDBEntities.TenantIdEnum.LVIS)
            {
                foreach (var Execption in resultIds)
                {
                    DataContracts.DashBoardExceptionDTO dto = new DashBoardExceptionDTO();
                    dto.ExceptionName = Execption.ExceptionTypeName.Replace("_", " ");
                    dto.NoOfExceptions = LvisDbcontext.Exceptions.Where(se => se.ExceptionTypeId == Execption.ExceptionTypeId && se.TypeCodeId != (int)ExceptionStatusEnum.Resolved).Count();
                    dto.DateTime = LvisDbcontext.Exceptions.Where(se => se.ExceptionTypeId == Execption.ExceptionTypeId).Count() > 0 ?
                                LvisDbcontext.Exceptions.Where(se => se.ExceptionTypeId == Execption.ExceptionTypeId).Max(me => me.CreatedDate).ToString()
                                : DateTime.Now.ToString();
                    dto.ThreshholdCount = Execption.ThresholdCount;
                    ExceptionsList.Add(dto);
                }
            }
            else
            {
                foreach (var Execption in resultIds)
                {
                    DataContracts.DashBoardExceptionDTO dto = new DashBoardExceptionDTO();
                    dto.ExceptionName = Execption.ExceptionTypeName.Replace("_", " ");
                    dto.NoOfExceptions = LvisDbcontext.Exceptions.Where(se => se.ExceptionTypeId == Execption.ExceptionTypeId && se.TypeCodeId != (int)ExceptionStatusEnum.Resolved
                         && tenantId == (se.MessageLogId != 0 ? LvisDbcontext.MessageLogs.Where(ml => ml.MessageLogId == se.MessageLogId).FirstOrDefault().TenantId : 0)).Count();
                    dto.DateTime = LvisDbcontext.Exceptions.Where(se => se.ExceptionTypeId == Execption.ExceptionTypeId
                                     && tenantId == (se.MessageLogId != 0 ? LvisDbcontext.MessageLogs.Where(ml => ml.MessageLogId == se.MessageLogId).FirstOrDefault().TenantId : 0)).Count() > 0 ?
                                LvisDbcontext.Exceptions.Where(se => se.ExceptionTypeId == Execption.ExceptionTypeId
                                && tenantId == (se.MessageLogId != 0 ? LvisDbcontext.MessageLogs.Where(ml => ml.MessageLogId == se.MessageLogId).FirstOrDefault().TenantId : 0)).Max(me => me.CreatedDate).ToString()
                                : DateTime.Now.ToString();
                    dto.ThreshholdCount = Execption.ThresholdCount;
                    ExceptionsList.Add(dto);
                }
            }

            return ExceptionsList.Where(ex => ex.ExceptionName != null);
        }

        public IEnumerable<DashBoardGraphicalExceptionDTO> GetBEQGraphicalExceptions(int tenantId)
        {
            DateTime? date = DateTime.Now.AddHours(-12);

            List<DashBoardGraphicalExceptionDTO> Graphicalcount = new List<DashBoardGraphicalExceptionDTO>();
            using (Entities dbContext = new Entities())
            {
                if (tenantId == (int)TenantIdEnum.LVIS)
                {
                    var ExceptionLogitems = dbContext.ExceptionQueueLogs.Where(y => y.ExceptionGroupId == (int)ExceptionGroupEnum.BEQ
                              && y.CreateDate >= date).ToList();

                    Graphicalcount = GetGraphicalList(ExceptionLogitems);
                }
                else
                {
                    var ExceptionLogitems = dbContext.ExceptionQueueLogs.Where(y => y.ExceptionGroupId == (int)ExceptionGroupEnum.BEQ
                             && y.TenantId == tenantId
                             && y.CreateDate >= date).ToList();

                    Graphicalcount = GetGraphicalList(ExceptionLogitems);
                }
            }

            Graphicalcount.Reverse();

            return Graphicalcount;
        }

        private List<DashBoardGraphicalExceptionDTO> GetGraphicalList(List<ExceptionQueueLog> ExceptionLogitems)
        {
            List<DashBoardGraphicalExceptionDTO> Graphicalcount = Enumerable.Range(00, 12)
            .Select(excep => new DashBoardGraphicalExceptionDTO()
            {
                Hour = (DateTime.Now.AddHours(-excep)).ToString("hh:00 tt"),
                QueueCount = ExceptionLogitems.Where(ie => ie.CreateDate == Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd hh:00 tt")).AddHours(-excep)).Select(s => s.QueueCount).FirstOrDefault(),
                NewCount = ExceptionLogitems.Where(ie => ie.CreateDate == Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd hh:00 tt")).AddHours(-excep)).Select(s => s.NewCount).FirstOrDefault(),
                ActiveCount = ExceptionLogitems.Where(ie => ie.CreateDate == Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd hh:00 tt")).AddHours(-excep)).Select(s => s.ActiveCount).FirstOrDefault(),
                HoldCount = ExceptionLogitems.Where(ie => ie.CreateDate == Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd hh:00 tt")).AddHours(-excep)).Select(s => s.HoldCount).FirstOrDefault(),
                ArchiveCount = ExceptionLogitems.Where(ie => ie.CreateDate == Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd hh:00 tt")).AddHours(-excep)).Select(s => s.ArchiveCount).FirstOrDefault(),
                Datetime = DateTime.Today.Date.ToShortDateString()
            }
            ).ToList();

            return Graphicalcount;
        }

        public IEnumerable<ExceptionDTO> GetBEQExceptions(SearchDetail value, int tenantId)
        {
            DateTime startDateTime = Convert.ToDateTime(value.Fromdate);

            DateTime endDateTime = Convert.ToDateTime(value.ThroughDate);
            endDateTime = endDateTime.Subtract(startDateTime.TimeOfDay).AddDays(1).AddMilliseconds(-1);

            List<TerminalDBEntities.Exception> ExceptionDetails = new List<TerminalDBEntities.Exception>();

            using (Entities dbContext = new Entities())
            {
                var result = GetExceptionInfo_BEQ(startDateTime, endDateTime, (int)ExceptionGroupEnum.BEQ, tenantId, value.Typecodestatus);

                List<ExceptionDTO> Exception = new List<ExceptionDTO>();

                foreach (string item in result.Select(se => se.ParentExternalRefNum).Distinct())
                {
                    List<ExceptionDTO> Details = result.Where(se => se.ParentExternalRefNum == item).OrderBy(ord => ord.Reporting.createddate).ToList();
                    ExceptionDTO Parent = new ExceptionDTO();

                    List<String> ServiceTypes = new List<string>();
                    List<string> TransactionTypes = new List<string>();
                    List<string> Buyers = new List<string>();

                    Parent.children = new List<ExceptionDTO>();

                    if (Details.Count == 1)
                    {
                        Parent = Details[0];
                        Exception.Add(Parent);
                        continue;
                    }

                    Parent.children = new List<ExceptionDTO>();
                    foreach (var child in Details)
                    {
                        Parent.ExceptionTypeid = child.ExceptionTypeid;
                        Parent.Exceptionid = 0;
                        Parent.ExceptionType = Parent.ExceptionType = child.ExceptionType;
                        Parent.Status = child.Status;
                        Parent.ExternalRefNum = child.ParentExternalRefNum;
                        Parent.ParentExternalRefNum = child.ParentExternalRefNum;
                        Parent.Tenant = child.Tenant;
                        Parent.TenantId = child.TenantId;
                        Parent.Comments = new List<string>();
                        //Parent.Comments.AddRange(child.Comments);

                        Parent.Reporting = new ReportingDTO();
                        Parent.Reporting.CustomerId = child.Reporting.CustomerId;
                        Parent.Reporting.CustomerName = child.Reporting.CustomerName;
                        Parent.Reporting.ApplicationId = child.Reporting.ApplicationId;
                        Parent.Reporting.CustomerRefNum = child.Reporting.CustomerRefNum;
                        Parent.Reporting.InternalRefId = child.Reporting.InternalRefId;
                        Parent.Reporting.InternalRefNum = child.Reporting.InternalRefNum;
                        Parent.Reporting.LenderId = child.Reporting.LenderId;

                        ServiceTypes.Add(child.ServiceType);
                        TransactionTypes.Add(child.TransactionType);
                        Buyers.Add(child.Buyer);

                        Parent.children.Add(child);
                    }

                    Parent.ServiceType = String.Join(", ", ServiceTypes.Distinct().ToArray());
                    Parent.TransactionType = String.Join(", ", TransactionTypes.Distinct().ToArray());
                    Parent.Buyer = String.Join(", ", Buyers.Distinct().ToArray());
                    Parent.CreatedDate = Parent.children[0].Reporting.createddate;

                    Exception.Add(Parent);
                }

                return Exception;
            }

        }

        public IEnumerable<ExceptionDTO> GetBEQExceptionsbyType(SearchDetail value, int tenantId, string ExceptionTypeName)
        {
            DateTime startDateTime = Convert.ToDateTime(value.Fromdate);

            DateTime endDateTime = Convert.ToDateTime(value.ThroughDate);
            endDateTime = endDateTime.Subtract(startDateTime.TimeOfDay).AddDays(1).AddMilliseconds(-1);

            Entities dbContext = new Entities();

            List<TerminalDBEntities.Exception> ExceptionDetails = new List<TerminalDBEntities.Exception>();

            if ((tenantId == (int)TerminalDBEntities.TenantIdEnum.LVIS))
            {
                if (value.Typecodestatus)
                {
                    ExceptionDetails = dbContext.Exceptions.Where(sel => sel.CreatedDate >= startDateTime && sel.CreatedDate <= endDateTime
                    && sel.ExceptionType.ExceptionGroupId == (int)ExceptionGroupEnum.BEQ && sel.ExceptionType.ExceptionTypeDesc == ExceptionTypeName).ToList();
                }
                else

                {
                    ExceptionDetails = dbContext.Exceptions.Where(sel => sel.CreatedDate >= startDateTime && sel.CreatedDate <= endDateTime
                    && sel.ExceptionType.ExceptionGroupId == (int)ExceptionGroupEnum.BEQ && sel.TypeCodeId != (int)ExceptionStatusEnum.Resolved && sel.ExceptionType.ExceptionTypeDesc == ExceptionTypeName).ToList();
                }
            }
            else
            {
                if (value.Typecodestatus)
                {
                    ExceptionDetails = dbContext.Exceptions.Where(sel => sel.CreatedDate >= startDateTime && sel.CreatedDate <= endDateTime
                    && sel.ExceptionType.ExceptionGroupId == (int)ExceptionGroupEnum.BEQ
               && tenantId == (sel.MessageLogId != 0 ? dbContext.MessageLogs.Where(ml => ml.MessageLogId == sel.MessageLogId).FirstOrDefault().TenantId : 0) && sel.ExceptionType.ExceptionTypeDesc == ExceptionTypeName).ToList();
                }
                else
                {
                    ExceptionDetails = dbContext.Exceptions.Where(sel => sel.CreatedDate >= startDateTime && sel.CreatedDate <= endDateTime
                    && sel.ExceptionType.ExceptionGroupId == (int)ExceptionGroupEnum.BEQ
                    && sel.TypeCodeId != (int)ExceptionStatusEnum.Resolved && tenantId == (sel.MessageLogId != 0 ? dbContext.MessageLogs.Where(ml => ml.MessageLogId == sel.MessageLogId).FirstOrDefault().TenantId : 0) && sel.ExceptionType.ExceptionTypeDesc == ExceptionTypeName).ToList();
                }
            }
            TerminalDocumentEntities DbDocumentcontext = new TerminalDocumentEntities();
            List<ExceptionDTO> Det = new List<ExceptionDTO>();

            foreach (var se in ExceptionDetails.OrderByDescending(sel => sel.CreatedDate))
            {
                var Status = se.TypeCode;
                var messageLog = dbContext.MessageLogs.Where(ml => ml.MessageLogId == se.MessageLogId).FirstOrDefault();

                var ServiceReq = dbContext.MessageLogs.Where(ml => ml.MessageLogId == se.MessageLogId).FirstOrDefault() != null
                    ? dbContext.MessageLogs.Where(ml => ml.MessageLogId == se.MessageLogId).FirstOrDefault().ServiceRequest : new ServiceRequest();

                ExceptionDTO Detail = new ExceptionDTO();
                Detail.Exceptionid = se.ExceptionId;
                Detail.ExceptionType = se.ExceptionType.ExceptionTypeDesc;
                Detail.ExceptionDesc = se.ExceptionDesc;
                Detail.ExceptionTypeid = se.ExceptionTypeId;
                Detail.CreatedBy = se.CreatedById == 0 ? string.Empty : dbContext.Tower_Users.Single(fi => fi.UserId == se.CreatedById).UserName;
                Detail.CreatedDate = se.CreatedDate.ToString();
                Detail.LastModifiedBy = se.LastModifiedById == 0 ? string.Empty : dbContext.Tower_Users.Single(fi => fi.UserId == se.LastModifiedById).UserName;
                Detail.LastModifiedDate = se.LastModifiedDate.ToString();

                Detail.Status = new ExceptionStatus() { ID = Status.TypeCodeId, Name = Status.TypeCodeDesc };
                Detail.DocumentObjectid = se.DocumentObjectId;
                Detail.ExternalRefNum = ServiceReq != null ? ServiceReq.ExternalRefNum : string.Empty;
                if (!string.IsNullOrEmpty(ServiceReq.Application.ApplicationName) && (ServiceReq.Application.ApplicationName == DataContracts.Constants.APPLICATION_REALEC
                      || ServiceReq.Application.ApplicationName == DataContracts.Constants.APPLICATION_KEYSTONE))
                {
                    Detail.ParentExternalRefNum = Detail.ExternalRefNum != null
                        ? Detail.ExternalRefNum.Substring(0, (Detail.ExternalRefNum.LastIndexOf("-") > -1 == true
                        ? Detail.ExternalRefNum.LastIndexOf("-") : Detail.ExternalRefNum.Length - 1)) : string.Empty;
                }
                else
                { Detail.ParentExternalRefNum = Detail.ExternalRefNum != null ? Detail.ExternalRefNum : string.Empty; }
                //Detail.ParentExternalRefNum = Detail.ExternalRefNum != null ? Detail.ExternalRefNum.Substring(0, (Detail.ExternalRefNum.LastIndexOf("-") > -1 == true ? Detail.ExternalRefNum.LastIndexOf("-") : Detail.ExternalRefNum.Length - 1)) : string.Empty;
                Detail.MessageType = messageLog != null ? messageLog.MessageMap.MessageType.MessageTypeName : string.Empty;
                Detail.ServiceType = messageLog != null ? ServiceReq.Service.ServiceName : string.Empty;
                Detail.ServiceRequestId = messageLog != null ? ServiceReq.ServiceRequestId : 0;
                Detail.TypeCodeId = se.TypeCodeId;
                Detail.Reporting = new ReportingDTO()
                {
                    ServiceRequestId = messageLog != null ? ServiceReq.ServiceRequestId : 0,
                    createddate = ServiceReq?.CreatedDate.ToString(),
                    CustomerRefNum = ServiceReq?.CustomerRefNum,
                    CustomerName = ServiceReq?.Location?.Customer.CustomerName,
                    ApplicationId = ServiceReq?.Application?.ApplicationName,
                    InternalRefNum = ServiceReq?.InternalRefNum,
                    LenderId = ServiceReq?.Location?.ExternalId
                };
                Det.Add(Detail);
            }

            List<ExceptionDTO> Exception = new List<ExceptionDTO>();

            foreach (string item in Det.Select(se => se.ParentExternalRefNum).Distinct())
            {
                List<ExceptionDTO> Details = Det.Where(se => se.ParentExternalRefNum == item).OrderBy(ord => ord.Reporting.createddate).ToList();
                ExceptionDTO Parent = new ExceptionDTO();

                List<String> ServiceTypes = new List<string>();
                List<string> TransactionTypes = new List<string>();
                List<string> Buyers = new List<string>();

                if (Details.Count == 1)
                {
                    Parent = Details[0];
                    Exception.Add(Parent);
                    continue;
                }

                Parent.children = new List<ExceptionDTO>();
                foreach (var child in Details)
                {
                    Parent.Exceptionid = 0;
                    Parent.ExceptionTypeid = child.ExceptionTypeid;
                    Parent.ExceptionType = child.ExceptionType;
                    Parent.Status = child.Status;
                    Parent.ExternalRefNum = child.ParentExternalRefNum;
                    Parent.ParentExternalRefNum = child.ParentExternalRefNum;
                    Parent.Tenant = child.Tenant;
                    Parent.TenantId = child.TenantId;
                    Parent.Reporting = new ReportingDTO();
                    Parent.Reporting.CustomerId = child.Reporting.CustomerId;
                    Parent.Reporting.CustomerName = child.Reporting.CustomerName;
                    Parent.Reporting.ApplicationId = child.Reporting.ApplicationId;
                    Parent.Reporting.LenderId = child.Reporting.LenderId;

                    ServiceTypes.Add(child.ServiceType);
                    TransactionTypes.Add(child.TransactionType);
                    Buyers.Add(child.Buyer);

                    Parent.children.Add(child);
                }

                Parent.ServiceType = string.Join(", ", ServiceTypes.Distinct().ToArray());
                Parent.TransactionType = string.Join(", ", TransactionTypes.Distinct().ToArray());
                Parent.Buyer = String.Join(", ", Buyers.Distinct().ToArray());
                Parent.CreatedDate = Parent.children[0].Reporting.createddate;

                Exception.Add(Parent);
            }

            return Exception;
        }

        public IEnumerable<ExceptionDTO> GetBEQExceptions(string sFilter, int tenantId, bool isIncludeResolved)
        {
            DateTime startDateTime = DateTime.Today;
            DateTime endDateTime = DateTime.Today;

            if (sFilter == "2") //2-represents to Pull All Record from DB from Inception of same ExceptionType
            {
                startDateTime = startDateTime.Subtract(startDateTime.TimeOfDay).AddYears(-1);  //To Pull data for 1 year from now
                endDateTime = endDateTime.Subtract(startDateTime.TimeOfDay).AddDays(1).AddMilliseconds(-1);
            }
            if (sFilter == "19") 
            {
                startDateTime = Convert.ToDateTime("1/01/2016 12:00:00 AM"); ;  //To Pull data from Inception Date.
                endDateTime = endDateTime.Subtract(startDateTime.TimeOfDay).AddDays(1).AddMilliseconds(-1);
            }
            else if (sFilter.Contains("24"))
            {
                startDateTime = DateTime.Now;
                startDateTime = startDateTime.AddDays(-1);
                endDateTime = DateTime.Now;
            }
            else
            {
                endDateTime = endDateTime.Subtract(startDateTime.TimeOfDay).AddDays(1).AddMilliseconds(-1);
                startDateTime = startDateTime.AddDays(-(int.Parse(sFilter)));
            }


            using (Entities dbContext = new Entities())
            {
                var result = GetExceptionInfo_BEQ(startDateTime, endDateTime, (int)ExceptionGroupEnum.BEQ, tenantId, isIncludeResolved);

                List<ExceptionDTO> Exception = new List<ExceptionDTO>();

                foreach (string item in result.Select(se => se.ParentExternalRefNum).Distinct())
                {
                    List<ExceptionDTO> Details = result.Where(se => se.ParentExternalRefNum == item).OrderBy(ord => ord.Reporting.createddate).ToList();
                    ExceptionDTO Parent = new ExceptionDTO();

                    List<string> ServiceTypes = new List<string>();
                    List<string> TransactionTypes = new List<string>();
                    List<string> Buyers = new List<string>();

                    if (Details.Count == 1)
                    {
                        Parent = Details[0];
                        Exception.Add(Parent);
                        continue;
                    }

                    Parent.children = new List<ExceptionDTO>();
                    foreach (var child in Details)
                    {
                        Parent.ExceptionTypeid = child.ExceptionTypeid;
                        Parent.Exceptionid = 0;
                        Parent.ExceptionType = Parent.ExceptionType = child.ExceptionType;
                        Parent.Status = child.Status;
                        Parent.ExternalRefNum = child.ParentExternalRefNum;
                        Parent.ParentExternalRefNum = child.ParentExternalRefNum;
                        Parent.Tenant = child.Tenant;
                        Parent.TenantId = child.TenantId;
                        Parent.Comments = new List<string>();
                        //Parent.Comments.AddRange(child.Comments);

                        Parent.Reporting = new ReportingDTO();
                        Parent.Reporting.CustomerId = child.Reporting.CustomerId;
                        Parent.Reporting.CustomerName = child.Reporting.CustomerName;
                        Parent.Reporting.ApplicationId = child.Reporting.ApplicationId;
                        Parent.Reporting.CustomerRefNum = child.Reporting.CustomerRefNum;
                        Parent.Reporting.InternalRefId = child.Reporting.InternalRefId;
                        Parent.Reporting.InternalRefNum = child.Reporting.InternalRefNum;
                        Parent.Reporting.LenderId = child.Reporting.LenderId;

                        ServiceTypes.Add(child.ServiceType);
                        TransactionTypes.Add(child.TransactionType);
                        Buyers.Add(child.Buyer);

                        Parent.children.Add(child);
                    }

                    Parent.ServiceType = String.Join(", ", ServiceTypes.Distinct().ToArray());
                    Parent.TransactionType = String.Join(", ", TransactionTypes.Distinct().ToArray());
                    Parent.Buyer = String.Join(", ", Buyers.Distinct().ToArray());
                    Parent.CreatedDate = Parent.children[0].Reporting.createddate;

                    Exception.Add(Parent);
                }

                return Exception;
            }
        }

        public IEnumerable<ExceptionDTO> GetBEQExceptionsbyTypeName(string sFilter, int tenantId, bool isIncludeResolved, string exceptionTypeName)
        {
            DateTime startDateTime = DateTime.Today;
            DateTime endDateTime = DateTime.Today;
            if (sFilter == "2") //2-represents to Pull All Record from DB from Inception of same ExceptionType
            {
                startDateTime = startDateTime.Subtract(startDateTime.TimeOfDay).AddYears(-1);  //To Pull data from Inception Date.
                endDateTime = endDateTime.Subtract(startDateTime.TimeOfDay).AddDays(1).AddMilliseconds(-1);
            }
            else if (sFilter == "19")
            {
                startDateTime = Convert.ToDateTime("1/01/2016 12:00:00 AM"); ;  //To Pull data from Inception Date.
                endDateTime = endDateTime.Subtract(startDateTime.TimeOfDay).AddDays(1).AddMilliseconds(-1);
            }
            else if (sFilter.Contains("24"))
            {
                startDateTime = DateTime.Now;
                startDateTime = startDateTime.AddDays(-1);
                endDateTime = DateTime.Now;
            }
            else
            {
                endDateTime = endDateTime.Subtract(startDateTime.TimeOfDay).AddDays(1).AddMilliseconds(-1);
                startDateTime = startDateTime.AddDays(-(int.Parse(sFilter)));
            }


            using (Entities dbContext = new Entities())
            {
                exceptionTypeName = exceptionTypeName != null ? exceptionTypeName.Trim().Replace(" ", "_") : exceptionTypeName;
                var det = GetExceptionInfo_BEQ(startDateTime, endDateTime, (int)ExceptionGroupEnum.BEQ, tenantId, isIncludeResolved, exceptionTypeName);

                List<ExceptionDTO> Exception = new List<ExceptionDTO>();

                foreach (string item in det.Select(se => se.ParentExternalRefNum).Distinct())
                {
                    List<ExceptionDTO> Details = det.Where(se => se.ParentExternalRefNum == item).OrderBy(ord => ord.Reporting.createddate).ToList();
                    ExceptionDTO Parent = new ExceptionDTO();

                    List<string> ServiceTypes = new List<string>();
                    List<string> TransactionTypes = new List<string>();
                    List<string> Buyers = new List<string>();

                    if (Details.Count == 1)
                    {
                        Parent = Details[0];
                        Exception.Add(Parent);
                        continue;
                    }

                    Parent.children = new List<ExceptionDTO>();
                    foreach (var child in Details)
                    {
                        Parent.ExceptionTypeid = child.ExceptionTypeid;
                        Parent.Exceptionid = 0;
                        Parent.ExceptionType = child.ExceptionType;
                        Parent.Status = child.Status;
                        Parent.ExternalRefNum = child.ParentExternalRefNum;
                        Parent.ParentExternalRefNum = child.ParentExternalRefNum;
                        Parent.Tenant = child.Tenant;
                        Parent.TenantId = child.TenantId;
                        Parent.Comments = new List<string>();
                        Parent.Reporting = new ReportingDTO();
                        Parent.Reporting.CustomerId = child.Reporting.CustomerId;
                        Parent.Reporting.CustomerName = child.Reporting.CustomerName;
                        Parent.Reporting.ApplicationId = child.Reporting.ApplicationId;
                        Parent.Reporting.CustomerRefNum = child.Reporting.CustomerRefNum;
                        Parent.Reporting.InternalRefId = child.Reporting.InternalRefId;
                        Parent.Reporting.InternalRefNum = child.Reporting.InternalRefNum;
                        Parent.Reporting.LenderId = child.Reporting.LenderId;

                        ServiceTypes.Add(child.ServiceType);
                        TransactionTypes.Add(child.TransactionType);
                        Buyers.Add(child.Buyer);

                        Parent.children.Add(child);
                    }

                    Parent.ServiceType = String.Join(", ", ServiceTypes.Distinct().ToArray());
                    Parent.TransactionType = String.Join(", ", TransactionTypes.Distinct().ToArray());
                    Parent.Buyer = String.Join(", ", Buyers.Distinct().ToArray());
                    Parent.CreatedDate = Parent.children[0].Reporting.createddate;

                    Exception.Add(Parent);
                }

                return Exception;
            }
        }

        public ExceptionDTO SaveBEQExceptionComments(ExceptionDTO Exception, int userId)
        {
            if (Exception.Exceptionid == 0)
            {
                foreach (var item in Exception.children)
                {
                    using (Entities dbContext = new Entities())
                    {
                        TerminalDBEntities.Exception ExceptionUpdate = dbContext.Exceptions.Where(se => se.ExceptionId == item.Exceptionid).FirstOrDefault();
                        if (ExceptionUpdate.TypeCodeId != Exception.Status.ID)
                        {
                            ExceptionUpdate.ExceptionNotes.Add(new ExceptionNote() { CreatedById = userId, ExceptionId = item.Exceptionid, CreatedDate = DateTime.Now, ExceptionNotes = "Changed Status from " + ExceptionUpdate.TypeCode.TypeCodeDesc + " to " + Exception.Status.Name });
                            ExceptionUpdate.TypeCodeId = Exception.Status.ID;
                            ExceptionUpdate.LastModifiedById = userId;
                            ExceptionUpdate.LastModifiedDate = DateTime.Now;
                            dbContext.Entry(ExceptionUpdate).State = System.Data.Entity.EntityState.Modified;
                            AuditLogHelper.SaveChanges(dbContext);
                        }
                    }
                    using (Entities dbContext = new Entities())
                    {
                        TerminalDBEntities.ExceptionNote EceptionComment = new TerminalDBEntities.ExceptionNote();
                        EceptionComment.ExceptionId = item.Exceptionid;
                        EceptionComment.ExceptionNotes = Exception.Notes;
                        EceptionComment.CreatedById = userId;
                        EceptionComment.CreatedDate = DateTime.Now;

                        dbContext.ExceptionNotes.Add(EceptionComment);
                        AuditLogHelper.SaveChanges(dbContext);
                    }
                }

            }
            else
            {
                using (Entities dbContext = new Entities())
                {
                    TerminalDBEntities.Exception ExceptionUpdate = dbContext.Exceptions.Where(se => se.ExceptionId == Exception.Exceptionid).FirstOrDefault();
                    if (ExceptionUpdate.TypeCodeId != Exception.Status.ID)
                    {
                        ExceptionUpdate.ExceptionNotes.Add(new ExceptionNote() { CreatedById = userId, ExceptionId = Exception.Exceptionid, CreatedDate = DateTime.Now, ExceptionNotes = "Changed Status from " + ExceptionUpdate.TypeCode.TypeCodeDesc + " to " + Exception.Status.Name });
                        ExceptionUpdate.TypeCodeId = Exception.Status.ID;
                        ExceptionUpdate.LastModifiedById = userId;
                        ExceptionUpdate.LastModifiedDate = DateTime.Now;
                        dbContext.Entry(ExceptionUpdate).State = System.Data.Entity.EntityState.Modified;
                        AuditLogHelper.SaveChanges(dbContext);
                    }
                }
                using (Entities dbContext = new Entities())
                {
                    TerminalDBEntities.ExceptionNote EceptionComment = new TerminalDBEntities.ExceptionNote();
                    EceptionComment.ExceptionId = Exception.Exceptionid;
                    EceptionComment.ExceptionNotes = Exception.Notes;
                    EceptionComment.CreatedById = userId;
                    EceptionComment.CreatedDate = DateTime.Now;

                    dbContext.ExceptionNotes.Add(EceptionComment);
                    AuditLogHelper.SaveChanges(dbContext);
                    Exception.Comments = dbContext.ExceptionNotes.Where(se => se.ExceptionId == Exception.Exceptionid).OrderBy(sl => sl.CreatedDate)
                            .Select(ENote => ENote.CreatedDate + " " + dbContext.Tower_Users.Where(fi => fi.UserId == ENote.CreatedById).Select(sl => sl.UserName).FirstOrDefault() + " " + ENote.ExceptionNotes).ToList();
                    //Exception.LastModifiedDate = DateTime.Now.ToString();
                }
            }

            return Exception;
        }
        private void GetEscrowOfficerNameAndEmail(IndividualParty escrowOfficer, out string lastName, out string firstName, out string email)
        {
            email = null;
            lastName = escrowOfficer.LastName;
            firstName = escrowOfficer.FirstName;
            string fullName = "";
            if (!String.IsNullOrEmpty(firstName))
                fullName = firstName;
            if (!String.IsNullOrEmpty(lastName))
            {
                if (fullName.Length > 0)
                    fullName += " " + lastName;
                else
                    fullName = lastName;
            }
            if (fullName.Length > 0)
                escrowOfficer.FullName = fullName;
            string work_email = null;
            if (escrowOfficer.ContactPoints != null && escrowOfficer.ContactPoints.Count > 0)
            {
                foreach (var ct in ((IndividualParty)escrowOfficer).ContactPoints)
                {
                    if (ct.ContactPointType == ContactPointTypeEnum.Work && !String.IsNullOrEmpty(ct.EmailAccount))
                        work_email = ct.EmailAccount;
                    else if (!String.IsNullOrEmpty(ct.EmailAccount))
                        email = ct.EmailAccount;
                }
            }
            if (work_email != null)
                email = work_email;
            return;
        }

        public BEQParseXMLDTO BEQParse(long documentObjectid)
        {
            //var docObjectid = GetMessageContentDocumentObject(documentObjectid);

            BEQParseXMLDTO Data = new BEQParseXMLDTO();
            if (documentObjectid == 0)
                return Data;

            Entities Context = new Entities();

            int tenantId = 0;

            if (Context.MessageLogs.Where(se => se.DocumentObjectId == documentObjectid).Count() > 0)
                tenantId = Context.MessageLogs.Where(se => se.DocumentObjectId == documentObjectid).FirstOrDefault().TenantId.Value;
            else
                return Data;

            if (tenantId == Convert.ToInt32(DataContracts.Constants.TENANT_ID_RF))
                return Data;

            using (TerminalDocumentEntities DbDocumentcontext = new TerminalDocumentEntities())
            {
                string Content = "";

                try
                {
                    DocumentObject Docobject = DbDocumentcontext.DocumentObjects.Where(se => se.DocumentObjectId == documentObjectid).FirstOrDefault();
                    if (Docobject != null)
                    {
                        Content = Docobject.Object;
                        if (string.IsNullOrEmpty(Content) && !string.IsNullOrEmpty(Docobject.ObjectPath)
                            && File.Exists(Docobject.ObjectPath))
                        {
                            Content = File.ReadAllText(Docobject.ObjectPath);
                        }
                    }
                }
                catch
                {

                }
                //XmlSerializer mySerializer = new XmlSerializer(typeof(Order));
                Order order = Utils.DeSerializeToObject<Order>(Content);
                string serviceType = "";
                int officeId = 0;

                Data.FastFileIDs = new List<int>();
                if (order.InternalData != null)
                {
                    if (order.Services[0].ServiceId == (int)ServiceEnum.Escrow || order.Services[0].ServiceId == (int)ServiceEnum.Signing)
                    {
                        officeId = order.InternalData.EscrowOfficeId;
                        if (order.Services[0].ServiceId == (int)ServiceEnum.Escrow)
                            serviceType = "Escrow";
                        else
                            serviceType = "Signing";
                    }
                    else
                    {
                        officeId = order.InternalData.TitleOfficeId;
                        serviceType = "Title";
                    }

                    Data.RegionName = Context.FASTRegions.Where(se => se.RegionID == order.InternalData.ProviderRegionId).FirstOrDefault().Name;

                    Data.OfficeName = string.Empty;
                    var officeName = Context.FASTOffices.Where(se => se.OfficeID == officeId && se.RegionID == order.InternalData.ProviderRegionId);
                    if (officeName != null && officeName.Count() > 0)
                        Data.OfficeName = officeName.First().OfficeName;

                    if (order.InternalData.PotentialMatches != null)
                        Data.FastFileIDs = order.InternalData.PotentialMatches.FileID;
                }

                Data.Service = serviceType;

                if (order.Loans.Count > 0)
                    Data.Transaction = order.Loans[0].LoanPurposeType.ToString();

                Data.FastFile = order.InternalRefNum;



                foreach (var party in order.Parties.Where(i => i != null && i.PartyRoleType == PartyRoleTypeEnum.EscrowOfficer))
                {
                    IndividualParty agent = null;
                    if (party.GetType() == typeof(IndividualParty))
                    {
                        agent = (IndividualParty)party;

                    }
                    else if (party.GetType() == typeof(LegalEntityParty))

                    {
                        var Officer = (LegalEntityParty)party;
                        if (Officer.Contacts != null && Officer.Contacts.Count > 0)
                        {
                            agent = (IndividualParty)Officer.Contacts[0];


                        }

                    }
                    if (agent != null)
                    {
                        string lastName = null, firstName = null, email = null;

                        GetEscrowOfficerNameAndEmail(agent, out lastName, out firstName, out email);

                        if (!string.IsNullOrEmpty(agent.FirstName) && agent.FirstName.ToUpper() != "NA" && !string.IsNullOrEmpty(agent.LastName) && agent.FirstName.ToUpper() != "NA" && string.IsNullOrEmpty(Data.escrowOfficer))
                        {
                            Data.escrowOfficer = agent.FullName;

                        }
                        if (string.IsNullOrEmpty(Data.escrowOfficerEmail) && !string.IsNullOrEmpty(email) && email.ToLower().Contains("@firstam.com"))
                            Data.escrowOfficerEmail = email;
                    }
                }





                string state = order.Properties != null && order.Properties.Count > 0 ? order.Properties[0].PropertyAddress.StateCode : null;
                string county = order.Properties != null && order.Properties.Count > 0 ? order.Properties[0].PropertyAddress.CountyName : null;

                var servicereq = Context.ServiceRequests.Where(se => se.ServiceRequestId == order.ServiceRequestId).FirstOrDefault();
                if (servicereq != null)
                {

                    var Provider = Context.Providers.Where(se => servicereq.ProviderId == se.ProviderId).FirstOrDefault();
                    Data.IsBindonly = Provider.IsBindOnly;

                    var custLocationId = servicereq.LocationId.HasValue ? servicereq.LocationId.Value : (int?)null;
                    var custId = (custLocationId.HasValue && custLocationId.Value > 0) ? Context.Locations.Where(se => se.LocationId == custLocationId.Value).FirstOrDefault().CustomerId : (int?)null;
                    var result = Context.GetFASTOfficeMap(servicereq.ProviderId, custLocationId, state, county, custId, null).FirstOrDefault();

                    if (String.IsNullOrEmpty(Data.escrowOfficer) && string.IsNullOrEmpty(Data.escrowOfficerEmail))
                    {
                        if (result != null && !string.IsNullOrEmpty(result.EscrowOfficerCode) && result.EscrowOfficeId.HasValue)
                        {
                            FASTProcessing.RunAccount impAccount = new FASTProcessing.RunAccount();
                            impAccount.Tenantid = tenantId;
                            FASTProcessing.EQFASTSearch searchClient = new FASTProcessing.EQFASTSearch(impAccount);

                            var fastofficers = searchClient.SearchEmplyeeByType(result.EscrowOfficeId.Value, 79, result.EscrowRegionId);
                            if (fastofficers != null)
                            {
                                var fastoff = fastofficers.Where(se => se.OfficerCode.ToLower() == result.EscrowOfficerCode.ToLower()).FirstOrDefault();

                                Data.escrowOfficer = fastoff.OfficerName;

                                Data.escrowOfficerEmail = fastoff.Email; ;

                            }
                        }
                    }

                    //Escrow Assistant
                    if (result != null && !string.IsNullOrEmpty(result.EscrowAssistantCode) && result.EscrowOfficeId.HasValue)
                    {
                        FASTProcessing.RunAccount impAccount = new FASTProcessing.RunAccount();
                        impAccount.Tenantid = tenantId;
                        FASTProcessing.EQFASTSearch searchClient = new FASTProcessing.EQFASTSearch(impAccount);

                        var fastofficers = searchClient.SearchEmplyeeByType(result.EscrowOfficeId.Value, 238, result.EscrowRegionId);
                        if (fastofficers != null)
                        {
                            var fastoff = fastofficers.Where(se => se.OfficerCode.ToLower() == result.EscrowAssistantCode.ToLower()).FirstOrDefault();

                            Data.escrowAssistant = fastoff.OfficerName;

                            Data.escrowAssistantEmail = fastoff.Email;

                        }
                    }

                }



                if (order.Properties.Count > 0)
                {
                    Data.County = order.Properties[0].PropertyAddress.CountyName;
                    Data.City = order.Properties[0].PropertyAddress.CityName;
                    Data.State = order.Properties[0].PropertyAddress.StateCode;
                    Data.Address = order.Properties[0].PropertyAddress.AddressLine1;
                }

                Data.Buyer = GetBuyerInfo(order.Parties);

                Data.LoanNumber = order.Loans[0].LoanRefNum;

                if (order.InternalData != null &&
                        order.InternalData.LenderABEIDName != null &&
                        order.InternalData.LenderABEIDName != string.Empty)
                {
                    Data.LenderName = order.InternalData.LenderABEIDName;
                }
                else if (order.InternalData != null &&
                         order.InternalData.BusinessSourceABEIDName != null &&
                         order.InternalData.BusinessSourceABEIDName != string.Empty)
                {
                    Data.LenderName = order.InternalData.BusinessSourceABEIDName;
                }

                try
                {

                    var businessentities = order.Parties.Where(i => i != null && i.GetType() == typeof(LegalEntityParty));
                    if (businessentities != null && businessentities.Count() > 0)
                    {
                        foreach (var party in businessentities)
                        {


                            var loanOfficer1 = (LegalEntityParty)party;


                            if (loanOfficer1.Contacts != null && loanOfficer1.Contacts.Count > 0)
                            {
                                var LegalParty = (IndividualParty)loanOfficer1.Contacts[0];

                                if (LegalParty.ContactPoints != null && LegalParty.ContactPoints.Count > 0 && LegalParty.PartyRoleType == PartyRoleTypeEnum.LoanOfficer)
                                {
                                    Data.LoanOfficer = LegalParty.ContactPoints[0].EmailAccount;
                                    break;
                                }
                            }


                        }
                    }



                    var loanOfficer = order.Parties.Where(i => i != null && i.PartyRoleType == PartyRoleTypeEnum.LoanOfficer).FirstOrDefault();

                    if (loanOfficer != null)
                    {
                        if (String.IsNullOrEmpty(Data.LoanOfficer) && loanOfficer.GetType() == typeof(IndividualParty))

                        {
                            var Party = (IndividualParty)loanOfficer;
                            if (Party.ContactPoints != null && Party.ContactPoints.Count > 0)
                                Data.LoanOfficer = Party.ContactPoints[0].EmailAccount;
                        }
                    }



                }
                catch { }

            }

            return Data;
        }

        string IExceptionDataProvider.GetMessageContent(long documentObjectid)
        {
            using (TerminalDocumentEntities DbDocumentcontext = new TerminalDocumentEntities())
            {
                if (documentObjectid > 0)
                {
                    string Content = "";
                    DocumentObject Docobject = DbDocumentcontext.DocumentObjects.Where(se => se.DocumentObjectId == documentObjectid).FirstOrDefault();

                    if (Docobject != null)
                    {
                        Content = Docobject.Object;
                        if (string.IsNullOrEmpty(Content) 
                            && !string.IsNullOrEmpty(Docobject.ObjectPath)
                            && File.Exists(Docobject.ObjectPath))
                        {
                            Content = File.ReadAllText(Docobject.ObjectPath);
                        }
                        return ReportingDataProvider.FormatXML(Content);
                    }
                }
            }
            return string.Empty;

        }

        public bool BindMatch(PotentialMatchDTO bindMatch, int userId, string FileNotes)
        {
            using (Entities dbContext = new Entities())
            {
                TerminalDBEntities.Exception ExceptionInfo = dbContext.Exceptions.Where(se => se.ExceptionId == bindMatch.ExceptionID).FirstOrDefault();
                TerminalDocumentEntities DbDocumentcontext = new TerminalDocumentEntities();
                var content = "";
                DocumentObject Docobject = DbDocumentcontext.DocumentObjects.Where(se => se.DocumentObjectId == ExceptionInfo.DocumentObjectId).FirstOrDefault();
                if (Docobject != null)
                {
                    content = Docobject.Object;
                    if (string.IsNullOrEmpty(content) && !string.IsNullOrEmpty(Docobject.ObjectPath)
                        && File.Exists(Docobject.ObjectPath))
                    {
                        content = File.ReadAllText(Docobject.ObjectPath);
                    }

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
                                 TrackingId = t.Element("TrackingId")?.Value
                             }).FirstOrDefault();
                
                SetTrackingId(items?.TrackingId);
                string Content = "";
                DocumentObject Docobjectitem = DbDocumentcontext.DocumentObjects.Where(se => se.DocumentObjectId == items.DocumentObjectId).FirstOrDefault();
                if (Docobjectitem != null)
                {
                    Content = Docobjectitem.Object;
                    if (string.IsNullOrEmpty(Content) && !string.IsNullOrEmpty(Docobjectitem.ObjectPath)
                        && File.Exists(Docobjectitem.ObjectPath))
                    {
                        Content = File.ReadAllText(Docobjectitem.ObjectPath);
                    }
                }

                //XmlSerializer mySerializer = new XmlSerializer(typeof(Order));

                //Order order = (Order)mySerializer.Deserialize(new StringReader(Content));
                Order order = Utils.DeSerializeToObject<Order>(Content);
                order.InternalRefNum = bindMatch.FileNumber;
                order.InternalRefId = bindMatch.FileID.Value;
                order.OrderComments = AddAppendFilesNotes(order.OrderComments, FileNotes);

                string CanonicalContent = Utils.SerializeToString<Order>(order);
                DocumentObject Canonical = new DocumentObject();
                Canonical.CreatedById = userId;
                Canonical.CreatedDate = DateTime.Now;
                Canonical.DocumentObjectFormat = "XML";
                Canonical.Object = CanonicalContent;
                DbDocumentcontext.DocumentObjects.Add(Canonical);
                AuditLogHelper.SaveChanges(DbDocumentcontext);

                MessageLog messageLog;

                bool isResubmitSuccess = false;
                if (ExceptionInfo.MessageLogId > 0)
                {
                    sLogger.Debug(string.Format("MessageLogId: {0} found", ExceptionInfo.MessageLogId));
                    MessageLog MessageLogInfo = dbContext.MessageLogs.Where(se => se.MessageLogId == ExceptionInfo.MessageLogId).FirstOrDefault();

                    messageLog = new MessageLog()
                    {
                        CreatedById = userId,
                        LastModifiedById = userId,
                        CreatedDate = DateTime.Now,
                        LastModifiedDate = DateTime.Now,
                        MessageMapId = dbContext.MessageMaps.Where(se => se.MessageTypeId == 10025 && se.TenantId == MessageLogInfo.TenantId).FirstOrDefault() != null ? dbContext.MessageMaps.Where(se => se.MessageTypeId == 10025 && se.TenantId == MessageLogInfo.TenantId).FirstOrDefault().MessageMapId : 0,
                        TenantId = MessageLogInfo.TenantId,
                        ServiceRequestId = MessageLogInfo.ServiceRequestId,
                        DocumentObjectId = Canonical.DocumentObjectId,
                        ParentMessageLogId = MessageLogInfo.ParentMessageLogId,
                        MessageLogDesc = FileNotes,
                        RestartStep = null
                    };

                    dbContext.MessageLogs.Add(messageLog);
                    dbContext.SaveChanges();

                    string Dest = items.Destination?.Replace(DataContracts.Constants.APPLICATION_LVIS, "").Replace("QUEUE", "").Replace(".", "") ?? DataContracts.Constants.APPLICATION_FAST;

                    sLogger.Debug(string.Format("Publishing BIND message DocumentObjectId: {0} to Destination: {1}", messageLog.DocumentObjectId, Dest));

                    if (EMSAdapter.PublishMessage(Dest.ToUpper(), items.Source.ToUpper(), MessageLogInfo.ServiceRequestId, messageLog.MessageLogId, messageLog.DocumentObjectId.ToString()))
                    {
                        sLogger.Info(string.Format("Publishing BIND message DocumentObjectId: {0}  SUCCESSFUL", messageLog.DocumentObjectId));
                        isResubmitSuccess = true;
                    }
                    else
                    {
                        sLogger.Error(string.Format("Publishing BIND message DocumentObjectId: {0} FAILED", messageLog.DocumentObjectId.ToString()));
                    }
                }

                if (isResubmitSuccess)
                {
                    string Status = ExceptionInfo.TypeCode.TypeCodeDesc;
                    ExceptionInfo.TypeCodeId = (int)ExceptionStatusEnum.Resolved;
                    ExceptionInfo.LastModifiedById = userId;
                    ExceptionInfo.LastModifiedDate = DateTime.Now;
                    dbContext.Entry(ExceptionInfo).State = System.Data.Entity.EntityState.Modified;
                    AuditLogHelper.SaveChanges(dbContext);

                    TerminalDBEntities.ExceptionNote EceptionComment = new TerminalDBEntities.ExceptionNote();
                    EceptionComment.ExceptionId = bindMatch.ExceptionID;
                    EceptionComment.ExceptionNotes = "BIND completed, exception status changed from " + Status + " to " + ExceptionStatusEnum.Resolved.ToString();
                    EceptionComment.CreatedById = userId;
                    EceptionComment.CreatedDate = DateTime.Now;

                    dbContext.ExceptionNotes.Add(EceptionComment);
                    AuditLogHelper.SaveChanges(dbContext);

                    return true;
                }
                else
                {
                    ExceptionInfo.TypeCodeId = (int)ExceptionStatusEnum.Active;
                    ExceptionInfo.LastModifiedById = userId;
                    ExceptionInfo.LastModifiedDate = DateTime.Now;
                    dbContext.Entry(ExceptionInfo).State = System.Data.Entity.EntityState.Modified;
                    AuditLogHelper.SaveChanges(dbContext);

                    TerminalDBEntities.ExceptionNote EceptionComment = new TerminalDBEntities.ExceptionNote();
                    EceptionComment.ExceptionId = bindMatch.ExceptionID;
                    EceptionComment.ExceptionNotes = "There was an error resubmitting/publishing message to EMS";
                    EceptionComment.CreatedById = userId;
                    EceptionComment.CreatedDate = DateTime.Now;

                    dbContext.ExceptionNotes.Add(EceptionComment);
                    AuditLogHelper.SaveChanges(dbContext);
                    return false;
                }
            }
        }

        public BEQParseXMLDTO BEQParseParent(ExceptionDTO parent)
        {
            using (Entities Context = new Entities())
            {
                BEQParseXMLDTO Data = new BEQParseXMLDTO();

                Data.FastFileIDs = new List<int>();

                int TitleofficeId = 0;
                int EscrowOfficeid = 0;

                int tenantId = 0;


                Order order = new Order();
                foreach (var item in parent.children)
                {

                    using (TerminalDocumentEntities DbDocumentcontext = new TerminalDocumentEntities())
                    {
                        string Content = "";
                        DocumentObject Docobject = DbDocumentcontext.DocumentObjects.Where(se => se.DocumentObjectId == item.DocumentObjectid).FirstOrDefault();
                        if (Docobject != null)
                        {
                            Content = Docobject.Object;
                            if (string.IsNullOrEmpty(Content)
                                && !string.IsNullOrEmpty(Docobject.ObjectPath)
                                && File.Exists(Docobject.ObjectPath))
                            {
                                Content = File.ReadAllText(Docobject.ObjectPath);
                            }

                        }

                        if (Context.MessageLogs.Where(se => se.DocumentObjectId == item.DocumentObjectid).Count() > 0)
                            tenantId = Context.MessageLogs.Where(se => se.DocumentObjectId == item.DocumentObjectid).FirstOrDefault().TenantId.Value;
                        else
                            continue;

                        if (tenantId == Convert.ToInt32(DataContracts.Constants.TENANT_ID_RF))
                            continue;

                        order = Utils.DeSerializeToObject<Order>(Content);

                        string serviceType = "";

                        if (order.InternalData != null)
                        {
                            if (order.Services[0].ServiceId == (int)ServiceEnum.Escrow || order.Services[0].ServiceId == (int)ServiceEnum.Signing)
                            {
                                EscrowOfficeid = order.InternalData.EscrowOfficeId;
                                if (order.Services[0].ServiceId == (int)ServiceEnum.Escrow)
                                    serviceType = "Escrow";
                                else
                                    serviceType = "Signing";
                            }
                            else
                            {
                                TitleofficeId = order.InternalData.TitleOfficeId;
                                serviceType = "Title";
                            }

                            Data.RegionID = order.InternalData.ProviderRegionId;

                            Data.RegionName = Context.FASTRegions.Where(se => se.RegionID == order.InternalData.ProviderRegionId).FirstOrDefault()?.Name;

                            if (order.InternalData.PotentialMatches != null)
                                Data.FastFileIDs = order.InternalData.PotentialMatches.FileID;
                        }
                        Data.Service += serviceType + "|";

                        Data.Transaction = order.Loans[0].LoanPurposeType.ToString();

                        Data.FastFile = order.InternalRefNum;

                        if (String.IsNullOrEmpty(Data.escrowOfficer) || string.IsNullOrEmpty(Data.escrowOfficerEmail))
                        {
                            foreach (var party in order.Parties.Where(i => i != null && i.PartyRoleType == PartyRoleTypeEnum.EscrowOfficer))
                            {
                                IndividualParty agent = null;
                                if (party.GetType() == typeof(IndividualParty))
                                {
                                    agent = (IndividualParty)party;

                                }
                                else if (party.GetType() == typeof(LegalEntityParty))

                                {
                                    var Officer = (LegalEntityParty)party;
                                    if (Officer.Contacts != null && Officer.Contacts.Count > 0)
                                    {
                                        agent = (IndividualParty)Officer.Contacts[0];


                                    }


                                }
                                if (agent != null)
                                {
                                    string lastName = null, firstName = null, email = null;

                                    GetEscrowOfficerNameAndEmail(agent, out lastName, out firstName, out email);

                                    if (!string.IsNullOrEmpty(agent.FirstName) && agent.FirstName.ToUpper() != "NA" && !string.IsNullOrEmpty(agent.LastName) && agent.FirstName.ToUpper() != "NA" && string.IsNullOrEmpty(Data.escrowOfficer))
                                    {
                                        Data.escrowOfficer = agent.FullName;

                                    }
                                    if (string.IsNullOrEmpty(Data.escrowOfficerEmail) && !string.IsNullOrEmpty(email) && email.ToLower().Contains("@firstam.com"))
                                        Data.escrowOfficerEmail = email;
                                }

                            }

                        }

                        if (order.Properties.Count > 0)
                        {
                            Data.County = order.Properties[0].PropertyAddress.CountyName;
                            Data.City = order.Properties[0].PropertyAddress.CityName;
                            Data.State = order.Properties[0].PropertyAddress.StateCode;
                            Data.Address = order.Properties[0].PropertyAddress.AddressLine1;
                        }

                        Data.Buyer = GetBuyerInfo(order.Parties);


                        Data.LoanNumber = order.Loans[0].LoanRefNum;

                        if (order.InternalData != null &&
                                order.InternalData.LenderABEIDName != null &&
                                order.InternalData.LenderABEIDName != string.Empty)
                        {
                            Data.LenderName = order.InternalData.LenderABEIDName;
                        }
                        else if (order.InternalData != null &&
                               order.InternalData.BusinessSourceABEIDName != null &&
                               order.InternalData.BusinessSourceABEIDName != string.Empty)
                        {
                            Data.LenderName = order.InternalData.BusinessSourceABEIDName;
                        }


                        try
                        {

                            var businessentities = order.Parties.Where(i => i != null && i.GetType() == typeof(LegalEntityParty));
                            if (businessentities != null && businessentities.Count() > 0)
                            {
                                foreach (var party in businessentities)
                                {


                                    var loanOfficer1 = (LegalEntityParty)party;


                                    if (loanOfficer1.Contacts != null && loanOfficer1.Contacts.Count > 0)
                                    {
                                        var LegalParty = (IndividualParty)loanOfficer1.Contacts[0];

                                        if (LegalParty.ContactPoints != null && LegalParty.ContactPoints.Count > 0 && LegalParty.PartyRoleType == PartyRoleTypeEnum.LoanOfficer)
                                        {
                                            Data.LoanOfficer = LegalParty.ContactPoints[0].EmailAccount;
                                            break;
                                        }
                                    }

                                }
                            }



                            var loanOfficer = order.Parties.Where(i => i != null && i.PartyRoleType == PartyRoleTypeEnum.LoanOfficer).FirstOrDefault();

                            if (loanOfficer != null && loanOfficer.GetType() == typeof(IndividualParty) && String.IsNullOrEmpty(Data.LoanOfficer))

                            {
                                var Party = (IndividualParty)loanOfficer;
                                if (Party.ContactPoints != null && Party.ContactPoints.Count > 0)
                                    Data.LoanOfficer = Party.ContactPoints[0].EmailAccount;
                            }



                        }
                        catch { }

                    }

                }
                string state = Data.State;
                string county = Data.County;

                var servicereq = Context.ServiceRequests.Where(se => se.ServiceRequestId == order.ServiceRequestId).FirstOrDefault();
                if (servicereq != null)
                {
                    var Provider = Context.Providers.Where(se => servicereq.ProviderId == se.ProviderId).FirstOrDefault();
                    Data.IsBindonly = Provider.IsBindOnly;

                    var custLocationId = servicereq.LocationId.HasValue ? servicereq.LocationId.Value : (int?)null;
                    var custId = (custLocationId.HasValue && custLocationId.Value > 0) ? Context.Locations.Where(se => se.LocationId == custLocationId.Value).FirstOrDefault().CustomerId : (int?)null;
                    var result = Context.GetFASTOfficeMap(servicereq.ProviderId, custLocationId, state, county, custId, null).FirstOrDefault();

                    if (String.IsNullOrEmpty(Data.escrowOfficer) && string.IsNullOrEmpty(Data.escrowOfficerEmail))
                    {

                        if (result != null && !string.IsNullOrEmpty(result.EscrowOfficerCode) && result.EscrowOfficeId.HasValue)
                        {
                            FASTProcessing.RunAccount impAccount = new FASTProcessing.RunAccount();
                            impAccount.Tenantid = tenantId;
                            FASTProcessing.EQFASTSearch searchClient = new FASTProcessing.EQFASTSearch(impAccount);

                            var fastofficers = searchClient.SearchEmplyeeByType(result.EscrowOfficeId.Value, 79, result.EscrowRegionId);
                            if (fastofficers != null)
                            {
                                var fastoff = fastofficers.Where(se => se.OfficerCode.ToLower() == result.EscrowOfficerCode.ToLower()).FirstOrDefault();

                                Data.escrowOfficer = fastoff.OfficerName;

                                Data.escrowOfficerEmail = fastoff.Email; ;

                            }
                        }


                    }


                    if (result != null && !string.IsNullOrEmpty(result.EscrowAssistantCode) && result.EscrowOfficeId.HasValue)
                    {
                        FASTProcessing.RunAccount impAccount = new FASTProcessing.RunAccount();
                        impAccount.Tenantid = tenantId;
                        FASTProcessing.EQFASTSearch searchClient = new FASTProcessing.EQFASTSearch(impAccount);

                        var fastofficers = searchClient.SearchEmplyeeByType(result.EscrowOfficeId.Value, 238, result.EscrowRegionId);
                        if (fastofficers != null)
                        {
                            var fastoff = fastofficers.Where(se => se.OfficerCode.ToLower() == result.EscrowAssistantCode.ToLower()).FirstOrDefault();

                            Data.escrowAssistant = fastoff.OfficerName;

                            Data.escrowAssistantEmail = fastoff.Email;

                        }
                    }

                }



                if (EscrowOfficeid != 0)
                    Data.OfficeName = Context.FASTOffices.Where(se => se.OfficeID == EscrowOfficeid && se.RegionID == Data.RegionID).FirstOrDefault().OfficeName;
                else if (TitleofficeId != 0)
                    Data.OfficeName = Context.FASTOffices.Where(se => se.OfficeID == TitleofficeId && se.RegionID == Data.RegionID).FirstOrDefault().OfficeName;
                Data.FastFileIDs = Data.FastFileIDs.Distinct().ToList();
                return Data;
            }
        }

        /// <summary>
        /// Get borrower and co-borrower names seperated by semi colon
        /// </summary>
        /// <param name="parties"></param>
        /// <returns></returns>
        private string GetBuyerInfo(List<Party> parties)
        {
            string buyerName = string.Empty;
            List<string> buyerNames = new List<string>();
            var buyer = parties.Where(i => i != null && i.PartyRoleType == PartyRoleTypeEnum.Buyer_Borrower);

            foreach (var b in buyer)
            {

                buyerNames.Add(buyer != null && b is IndividualParty ? ((IndividualParty)b).FirstName + " " + ((IndividualParty)b).LastName : b.FullName);

            }

            buyerName = buyerNames != null ? string.Join(" ; ", buyerNames) : string.Empty;

            return buyerName;
        }

        public bool BEQBindAllOrder(string fileNumber, string fileID, ExceptionDTO BEQMatches, int userId, string fileNotes)
        {
            using (Entities dbContext = new Entities())
            {
                bool isSuccess = true;

                foreach (var item in BEQMatches.children.Where(se => !se.InvolveResolved))
                {
                    TerminalDBEntities.Exception ExceptionInfo = dbContext.Exceptions.Where(se => se.ExceptionId == item.Exceptionid).FirstOrDefault();
                    TerminalDocumentEntities DbDocumentcontext = new TerminalDocumentEntities();
                    string content = "";
                    DocumentObject Docobject = DbDocumentcontext.DocumentObjects.Where(se => se.DocumentObjectId == ExceptionInfo.DocumentObjectId).FirstOrDefault();
                    if (Docobject != null)
                    {
                        content = Docobject.Object;
                        if (string.IsNullOrEmpty(content)
                            
                            && !string.IsNullOrEmpty(Docobject.ObjectPath)
                            && File.Exists(Docobject.ObjectPath))
                        {
                            content = File.ReadAllText(Docobject.ObjectPath);
                        }
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
                                     TrackingId = t.Element("TrackingId")?.Value
                                 }).FirstOrDefault();

                    SetTrackingId(items?.TrackingId);
                    string Content = "";
                    DocumentObject Docobjectitems = DbDocumentcontext.DocumentObjects.Where(se => se.DocumentObjectId == items.DocumentObjectId).FirstOrDefault();
                    Content = Docobjectitems.Object;
                    if (string.IsNullOrEmpty(Content)
                        && !string.IsNullOrEmpty(Docobjectitems.ObjectPath)
                        && File.Exists(Docobjectitems.ObjectPath))
                    {
                        Content = File.ReadAllText(Docobjectitems.ObjectPath);
                    }
                    //XmlSerializer mySerializer = new XmlSerializer(typeof(Order));
                    //Order order = (Order)mySerializer.Deserialize(new StringReader(Content));

                    Order order = Utils.DeSerializeToObject<Order>(Content);

                    order.InternalRefNum = fileNumber;
                    order.InternalRefId = fileID.ToIntDefEx();
                    order.OrderComments = AddAppendFilesNotes(order.OrderComments, fileNotes);

                    string canonicalContent = Utils.SerializeToString<Order>(order);
                    DocumentObject Canonical = new DocumentObject();
                    Canonical.CreatedById = userId;
                    Canonical.CreatedDate = DateTime.Now;
                    Canonical.DocumentObjectFormat = "XML";
                    Canonical.Object = canonicalContent;
                    DbDocumentcontext.DocumentObjects.Add(Canonical);
                    AuditLogHelper.SaveChanges(DbDocumentcontext);

                    MessageLog messageLog;

                    bool isResubmitSuccess = false;
                    if (ExceptionInfo.MessageLogId > 0)
                    {
                        sLogger.Info(string.Format("MessageLogId: {0} found", ExceptionInfo.MessageLogId));
                        MessageLog MessageLogInfo = dbContext.MessageLogs.Where(se => se.MessageLogId == ExceptionInfo.MessageLogId).FirstOrDefault();

                        messageLog = new MessageLog()
                        {
                            CreatedById = userId,
                            LastModifiedById = userId,
                            CreatedDate = DateTime.Now,
                            LastModifiedDate = DateTime.Now,
                            MessageMapId = dbContext.MessageMaps.Where(se => se.MessageTypeId == 10025 && se.TenantId == MessageLogInfo.TenantId).FirstOrDefault() != null ? dbContext.MessageMaps.Where(se => se.MessageTypeId == 10025 && se.TenantId == MessageLogInfo.TenantId).FirstOrDefault().MessageMapId : 0,
                            TenantId = MessageLogInfo.TenantId,
                            ServiceRequestId = MessageLogInfo.ServiceRequestId,
                            DocumentObjectId = Canonical.DocumentObjectId,
                            ParentMessageLogId = MessageLogInfo.ParentMessageLogId,
                            MessageLogDesc = fileNotes,
                            RestartStep = null,
                        };

                        dbContext.MessageLogs.Add(messageLog);
                        dbContext.SaveChanges();

                        //string Dest = items.Destination?.Replace(DataContracts.Constants.APPLICATION_LVIS, "").Replace("QUEUE", "").Replace(".", "") ?? DataContracts.Constants.ENRICHMENT;
                        var Dest = DataContracts.Constants.CONVOY;
                        sLogger.Debug(string.Format("Publishing BIND message DocumentObjectId: {0} to Destination: {1}", Canonical.DocumentObjectId, Dest));

                        if (EMSAdapter.PublishMessage(Dest.ToUpper(), items.Source.ToUpper(), messageLog.ServiceRequestId, messageLog.MessageLogId, Canonical.DocumentObjectId.ToString(), BEQMatches.ParentExternalRefNum, DateTime.Now))
                        {
                            sLogger.Info(string.Format("Publishing BIND message DocumentObjectId: {0}  SUCCESSFUL", Canonical.DocumentObjectId));
                            isResubmitSuccess = true;
                        }
                        else
                        {
                            sLogger.Error(string.Format("Publishing BIND message DocumentObjectId: {0} FAILED", items.DocumentObjectId.ToString()));
                        }
                    }

                    if (isResubmitSuccess)
                    {
                        string Status = ExceptionInfo.TypeCode.TypeCodeDesc;
                        ExceptionInfo.TypeCodeId = (int)ExceptionStatusEnum.Resolved;
                        ExceptionInfo.LastModifiedById = userId;
                        ExceptionInfo.LastModifiedDate = DateTime.Now;
                        dbContext.Entry(ExceptionInfo).State = System.Data.Entity.EntityState.Modified;
                        AuditLogHelper.SaveChanges(dbContext);

                        TerminalDBEntities.ExceptionNote EceptionComment = new TerminalDBEntities.ExceptionNote();
                        EceptionComment.ExceptionId = item.Exceptionid;
                        EceptionComment.ExceptionNotes = "BIND completed, exception status changed from " + Status + " to " + ExceptionStatusEnum.Resolved.ToString();
                        EceptionComment.CreatedById = userId;
                        EceptionComment.CreatedDate = DateTime.Now;

                        dbContext.ExceptionNotes.Add(EceptionComment);
                        AuditLogHelper.SaveChanges(dbContext);
                    }
                    else
                    {
                        ExceptionInfo.TypeCodeId = (int)ExceptionStatusEnum.Active;
                        ExceptionInfo.LastModifiedById = userId;
                        ExceptionInfo.LastModifiedDate = DateTime.Now;
                        dbContext.Entry(ExceptionInfo).State = System.Data.Entity.EntityState.Modified;
                        AuditLogHelper.SaveChanges(dbContext);

                        TerminalDBEntities.ExceptionNote EceptionComment = new TerminalDBEntities.ExceptionNote();
                        EceptionComment.ExceptionId = item.Exceptionid;
                        EceptionComment.ExceptionNotes = "There was an error resubmitting/publishing message to EMS";
                        EceptionComment.CreatedById = userId;
                        EceptionComment.CreatedDate = DateTime.Now;

                        dbContext.ExceptionNotes.Add(EceptionComment);
                        AuditLogHelper.SaveChanges(dbContext);
                        isSuccess = false;
                    }
                }

                return isSuccess;
            }
        }

        bool BEQReject(ExceptionDTO matchException, int userId, string fileNotes)
        {
            using (Entities dbContext = new Entities())
            {
                TerminalDBEntities.Exception ExceptionInfo = dbContext.Exceptions.Where(se => se.ExceptionId == matchException.Exceptionid).FirstOrDefault();
                TerminalDocumentEntities DbDocumentcontext = new TerminalDocumentEntities();
                
                string content = "";
                DocumentObject Docobject = DbDocumentcontext.DocumentObjects.Where(se => se.DocumentObjectId == ExceptionInfo.DocumentObjectId).FirstOrDefault();

                if (Docobject != null)
                {
                    content = Docobject.Object;
                    if (string.IsNullOrEmpty(content)
                         && File.Exists(Docobject.ObjectPath)
                        && !string.IsNullOrEmpty(Docobject.ObjectPath))
                    {
                        content = File.ReadAllText(Docobject.ObjectPath);
                    }
                }
                var items = (from t in XDocument.Parse(content)?.Descendants(DataContracts.Constants.EXCEPTION)
                             select new
                             {
                                 Source = t.Element("Source")?.Value,
                                 Destination = t.Element("CurrentSource")?.Value,
                             }).FirstOrDefault();

                MessageLog messageLog;

                bool isResubmitSuccess = false;
                MessageLog MessageLogInfo = null;
                if (ExceptionInfo.MessageLogId > 0)
                {
                    sLogger.Debug(string.Format("MessageLogId: {0} found", ExceptionInfo.MessageLogId));
                    MessageLogInfo = dbContext.MessageLogs.Where(se => se.MessageLogId == ExceptionInfo.MessageLogId).FirstOrDefault();
                    var exceptionListIds = GetExceptionIds(MessageLogInfo.ServiceRequestId);
                    if(exceptionListIds!=null && exceptionListIds.Count>0)
                    {
                        foreach(int id in exceptionListIds)
                        {
                            TerminalDBEntities.Exception exceptionData = dbContext.Exceptions.Where(se => se.ExceptionId == id).FirstOrDefault();
                            exceptionData.TypeCodeId = (int)ExceptionStatusEnum.Resolved;
                            exceptionData.LastModifiedById = userId;
                            exceptionData.LastModifiedDate = DateTime.Now;
                            dbContext.Entry(exceptionData).State = System.Data.Entity.EntityState.Modified;
                            AuditLogHelper.SaveChanges(dbContext);
                        }
                    }
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

                    string Dest = items.Source;
                    if (items.Source.ToUpper() == "ENCOMPASS")
                        Dest = "Fastweb";
                    if (EMSAdapter.PublishMessage(Dest.ToUpper(), "LVIS", MessageLogInfo.ServiceRequestId, messageLog.MessageLogId, documentObjId))
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
                    string Status = ExceptionInfo.TypeCode.TypeCodeDesc;
                    ExceptionInfo.TypeCodeId = (int)ExceptionStatusEnum.Resolved;
                    ExceptionInfo.LastModifiedById = userId;
                    ExceptionInfo.LastModifiedDate = DateTime.Now;
                    dbContext.Entry(ExceptionInfo).State = System.Data.Entity.EntityState.Modified;
                    AuditLogHelper.SaveChanges(dbContext);
                    TerminalDBEntities.ExceptionNote EceptionComment = new TerminalDBEntities.ExceptionNote();
                    EceptionComment.ExceptionId = matchException.Exceptionid;
                    EceptionComment.ExceptionNotes = "REJECT completed, exception status changed from " + Status + " to " + ExceptionStatusEnum.Resolved.ToString() + " and resolved all the associated TEQ exceptions";
                    EceptionComment.CreatedById = userId;
                    EceptionComment.CreatedDate = DateTime.Now;

                    dbContext.ExceptionNotes.Add(EceptionComment);
                    AuditLogHelper.SaveChanges(dbContext);

                    TerminalDBEntities.ServiceRequest serviceReq = MessageLogInfo.ServiceRequest;

                    serviceReq.LastModifiedById = userId;
                    serviceReq.LastModifiedDate = DateTime.Now;

                    //dbContext.ServiceRequests.Add(serviceReq);
                    AuditLogHelper.SaveChanges(dbContext);

                    return true;
                }
                else
                {
                    ExceptionInfo.TypeCodeId = (int)ExceptionStatusEnum.Active;
                    ExceptionInfo.LastModifiedById = userId;
                    ExceptionInfo.LastModifiedDate = DateTime.Now;
                    dbContext.Entry(ExceptionInfo).State = System.Data.Entity.EntityState.Modified;
                    AuditLogHelper.SaveChanges(dbContext);

                    TerminalDBEntities.ExceptionNote EceptionComment = new TerminalDBEntities.ExceptionNote();
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

        public List<int> GetExceptionIds(int serviceRequestId)
        {
            using (Entities dbContext = new Entities())
            {
                return (from exception in dbContext.Exceptions
                        join messageLog in dbContext.MessageLogs on exception.MessageLogId equals messageLog.MessageLogId
                        where messageLog.ServiceRequestId == serviceRequestId && exception.ExceptionTypeId == (int)ExceptionTypeEnum.CheckIfOrderCreatedOrNot && exception.TypeCodeId != (int)ExceptionStatusEnum.Resolved
                        select exception.ExceptionId).ToList();
            }
        }
        public bool BEQRejectOrder(ExceptionDTO matchException, int userId, string fileNotes)
        {
            bool Status = true;

            if (matchException.Exceptionid == 0)
            {
                foreach (var child in matchException.children.Where(se => !se.InvolveResolved))
                {
                    Status = BEQReject(child, userId, fileNotes);
                }
            }
            else
                Status = BEQReject(matchException, userId, fileNotes);

            return Status;
        }

        public IEnumerable<ExceptionDTO> GetBEQExceptionByReferenceNum(SearchDetail value, int tenantId)
        {
            Entities dbContext = new Entities();
            List<ExceptionDTO> ExceptionDetails = new List<ExceptionDTO>();
            if (value.ReferenceNoType == "1" && !string.IsNullOrEmpty(value.ReferenceNo))
            {

                var Details = (from sr in dbContext.ServiceRequests
                               join ml in dbContext.MessageLogs on sr.ServiceRequestId equals ml.ServiceRequestId
                               join e in dbContext.Exceptions on ml.MessageLogId equals e.MessageLogId
                               where sr.ExternalRefNum.Contains(value.ReferenceNo)
                                 && e.ExceptionType.ExceptionGroupId == (int)ExceptionGroupEnum.BEQ
                                 && (sr.TenantId == tenantId || tenantId == (int)TenantIdEnum.LVIS)
                               select new
                               {
                                   MessageLogId = ml.MessageLogId,
                                   MessageLog = dbContext.MessageLogs.Where(ml => ml.MessageLogId == e.MessageLogId).FirstOrDefault(),
                                   ExceptionId = e.ExceptionId,
                                   ApplicationId = sr.ApplicationId,
                                   CustomerRefNum = sr.CustomerRefNum,
                                   ExternalRefNum = sr.ExternalRefNum,
                                   CreatedBy = e.CreatedById,
                                   CreatedDate = e.CreatedDate.ToString(),
                                   LastModifiedBy = e.LastModifiedById,
                                   LastModifiedDate = e.LastModifiedDate.ToString(),
                                   InternalRefId = sr.InternalRefId,
                                   InternalRefNum = sr.InternalRefNum,
                                   LocationId = sr.LocationId,
                                   ProviderId = sr.ProviderId,
                                   ServiceRequestId = sr.ServiceRequestId,
                                   ServiceId = sr.ServiceId,
                                   Comments = e.Comments,
                                   documentobjectid = e.DocumentObjectId,
                                   e
                               }).ToList();

                foreach (var se in Details)
                {
                    ExceptionDTO Detail = new ExceptionDTO();
                    Detail.Exceptionid = se.ExceptionId;
                    Detail.ExceptionTypeid = se.e.ExceptionTypeId;
                    Detail.ExceptionType = !string.IsNullOrEmpty(se.e.ExceptionType.ExceptionTypeName) ? se.e.ExceptionType.ExceptionTypeName : string.Empty;
                    Detail.ExceptionDesc = !string.IsNullOrEmpty(se.e.ExceptionDesc) ? se.e.ExceptionDesc : string.Empty;
                    Detail.CreatedBy = se.e.CreatedById == 0 ? string.Empty : dbContext.Tower_Users.Where(fi => fi.UserId == se.e.CreatedById).Select(sl => sl.UserName).FirstOrDefault();
                    Detail.CreatedDate = se.CreatedDate.ToString();
                    Detail.LastModifiedBy = se.e.LastModifiedById == 0 ? string.Empty : dbContext.Tower_Users.Where(fi => fi.UserId == se.e.LastModifiedById).Select(sl => sl.UserName).FirstOrDefault();
                    Detail.LastModifiedDate = se.LastModifiedDate.ToString();
                    Detail.Status = new ExceptionStatus() { ID = se.e.TypeCodeId, Name = dbContext.TypeCodes.Where(x => x.TypeCodeId == se.e.TypeCodeId).Select(x => x.TypeCodeDesc).FirstOrDefault() };
                    Detail.DocumentObjectid = se.MessageLog != null ? se.MessageLog.DocumentObjectId : se.e.DocumentObjectId;
                    Detail.ExternalRefNum = !string.IsNullOrEmpty(se.ExternalRefNum) ? se.ExternalRefNum : string.Empty;
                    Detail.MessageType = se.MessageLog != null ? se.MessageLog.MessageMap.MessageType.MessageTypeName : string.Empty;
                    Detail.MessageTypeid = se.MessageLog != null ? se.MessageLog.MessageMap.MessageTypeId : 0;
                    Detail.ServiceType = se.MessageLog != null ? se.MessageLog.ServiceRequest.Service.ServiceName : string.Empty;
                    Detail.ServiceRequestId = se.ServiceRequestId;
                    Detail.TypeCodeId = se.e.TypeCodeId;
                    Detail.ParentExternalRefNum = string.Empty;
                    Detail.Tenant = se.MessageLog != null ? se.MessageLog.Tenant.TenantName : string.Empty;
                    Detail.TenantId = se.MessageLog != null ? se.MessageLog.TenantId.Value : 0;

                    Detail.Reporting = new ReportingDTO();

                    if (Detail.ServiceRequestId > 0)
                    {
                        var ServiceReq = dbContext.ServiceRequests.Where(sel => sel.ServiceRequestId == Detail.ServiceRequestId)?.FirstOrDefault();
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
                        Detail.Reporting = new ReportingDTO()
                        {
                            ServiceRequestId = Detail.ServiceRequestId,
                            createddate = ServiceReq?.CreatedDate.ToString(),
                            CustomerRefNum = ServiceReq?.CustomerRefNum,
                            CustomerName = ServiceReq?.Location?.Customer.CustomerName,
                            ApplicationId = ServiceReq?.Application?.ApplicationName,
                            InternalRefNum = ServiceReq?.InternalRefNum,
                            LenderId = ServiceReq?.Location?.ExternalId
                        };
                        //Get incoming order 
                        Order order = GetIncomingOrderDTO(Detail.ServiceRequestId, dbContext);

                        //Transaction Type
                        string cannonicalLoanPurposeType = order != null ? GetTransaction(order.Loans) : string.Empty;
                        LoanPurposeTypeEnum loanPurposeType;
                        Enum.TryParse(cannonicalLoanPurposeType, out loanPurposeType);
                        Detail.TransactionType = dbContext.TypeCodes.Where(sel => sel.TypeCodeId == (int)loanPurposeType).Select(sl => sl.TypeCodeDesc).FirstOrDefault();

                        //Buyers
                        Detail.Buyer = order != null ? GetBuyerInfo(order.Parties) : string.Empty;

                    }

                    ExceptionDetails.Add(Detail);
                }
            }

            else if (value.ReferenceNoType == "2" && !string.IsNullOrEmpty(value.ReferenceNo))
            {

                var Details = (from sr in dbContext.ServiceRequests
                               join ml in dbContext.MessageLogs on sr.ServiceRequestId equals ml.ServiceRequestId
                               join e in dbContext.Exceptions on ml.MessageLogId equals e.MessageLogId
                               where sr.InternalRefNum.Contains(value.ReferenceNo)
                               && e.ExceptionType.ExceptionGroupId == (int)ExceptionGroupEnum.BEQ
                                 && (sr.TenantId == tenantId || tenantId == (int)TenantIdEnum.LVIS)
                               select new
                               {
                                   MessageLogId = ml.MessageLogId,
                                   MessageLog = dbContext.MessageLogs.Where(ml => ml.MessageLogId == e.MessageLogId).FirstOrDefault(),
                                   ExceptionId = e.ExceptionId,
                                   ApplicationId = sr.ApplicationId,
                                   CustomerRefNum = sr.CustomerRefNum,
                                   ExternalRefNum = sr.ExternalRefNum,
                                   CreatedBy = e.CreatedById,
                                   CreatedDate = e.CreatedDate.ToString(),
                                   LastModifiedBy = e.LastModifiedById,
                                   LastModifiedDate = e.LastModifiedDate.ToString(),
                                   InternalRefId = sr.InternalRefId,
                                   InternalRefNum = sr.InternalRefNum,
                                   LocationId = sr.LocationId,
                                   ProviderId = sr.ProviderId,
                                   ServiceRequestId = sr.ServiceRequestId,
                                   ServiceId = sr.ServiceId,
                                   Comments = e.Comments,
                                   documentobjectid = e.DocumentObjectId,
                                   e
                               }).ToList();

                foreach (var se in Details)
                {
                    ExceptionDTO Detail = new ExceptionDTO();
                    Detail.Exceptionid = se.ExceptionId;

                    Detail.ExceptionTypeid = se.e.ExceptionTypeId;
                    Detail.ExceptionType = !string.IsNullOrEmpty(se.e.ExceptionType.ExceptionTypeName) ? se.e.ExceptionType.ExceptionTypeName : string.Empty;
                    Detail.ExceptionDesc = !string.IsNullOrEmpty(se.e.ExceptionDesc) ? se.e.ExceptionDesc : string.Empty;
                    Detail.CreatedBy = se.e.CreatedById == 0 ? string.Empty : dbContext.Tower_Users.Where(fi => fi.UserId == se.e.CreatedById).Select(sl => sl.UserName).FirstOrDefault();
                    Detail.CreatedDate = se.CreatedDate.ToString();
                    Detail.LastModifiedBy = se.e.LastModifiedById == 0 ? string.Empty : dbContext.Tower_Users.Where(fi => fi.UserId == se.e.LastModifiedById).Select(sl => sl.UserName).FirstOrDefault();
                    Detail.LastModifiedDate = se.LastModifiedDate.ToString();
                    Detail.Status = new ExceptionStatus() { ID = se.e.TypeCodeId, Name = dbContext.TypeCodes.Where(x => x.TypeCodeId == se.e.TypeCodeId).Select(x => x.TypeCodeDesc).FirstOrDefault() };
                    Detail.DocumentObjectid = se.MessageLog != null ? se.MessageLog.DocumentObjectId : se.e.DocumentObjectId;
                    Detail.ExternalRefNum = !string.IsNullOrEmpty(se.ExternalRefNum) ? se.ExternalRefNum : string.Empty;
                    Detail.MessageType = se.MessageLog != null ? se.MessageLog.MessageMap.MessageType.MessageTypeName : string.Empty;
                    Detail.MessageTypeid = se.MessageLog != null ? se.MessageLog.MessageMap.MessageTypeId : 0;
                    Detail.ServiceType = se.MessageLog != null ? se.MessageLog.ServiceRequest.Service.ServiceName : string.Empty;
                    Detail.ServiceRequestId = se.ServiceRequestId;
                    Detail.TypeCodeId = se.e.TypeCodeId;
                    Detail.ParentExternalRefNum = string.Empty;
                    Detail.Tenant = se.MessageLog != null ? se.MessageLog.Tenant.TenantName : string.Empty;
                    Detail.TenantId = se.MessageLog != null ? se.MessageLog.TenantId.Value : 0;

                    Detail.Reporting = new ReportingDTO();

                    if (Detail.ServiceRequestId > 0)
                    {
                        var ServiceReq = dbContext.ServiceRequests.Where(sel => sel.ServiceRequestId == Detail.ServiceRequestId)?.FirstOrDefault();
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
                        Detail.Reporting = new ReportingDTO()
                        {
                            ServiceRequestId = Detail.ServiceRequestId,
                            createddate = ServiceReq?.CreatedDate.ToString(),
                            CustomerRefNum = ServiceReq?.CustomerRefNum,
                            CustomerName = ServiceReq?.Location?.Customer.CustomerName,
                            ApplicationId = ServiceReq?.Application?.ApplicationName,
                            InternalRefNum = ServiceReq?.InternalRefNum,
                            LenderId = ServiceReq?.Location?.ExternalId
                        };
                        //Get incoming order 
                        Order order = GetIncomingOrderDTO(Detail.ServiceRequestId, dbContext);

                        //Transaction Type
                        string cannonicalLoanPurposeType = order != null ? GetTransaction(order.Loans) : string.Empty;
                        LoanPurposeTypeEnum loanPurposeType;
                        Enum.TryParse(cannonicalLoanPurposeType, out loanPurposeType);
                        Detail.TransactionType = dbContext.TypeCodes.Where(sel => sel.TypeCodeId == (int)loanPurposeType).Select(sl => sl.TypeCodeDesc).FirstOrDefault();

                        //Buyers
                        Detail.Buyer = order != null ? GetBuyerInfo(order.Parties) : string.Empty;

                    }

                    ExceptionDetails.Add(Detail);
                }


            }
            else if (value.ReferenceNoType == "3" && !string.IsNullOrEmpty(value.ReferenceNo))
            {
                var Details = (from sr in dbContext.ServiceRequests
                               join ml in dbContext.MessageLogs on sr.ServiceRequestId equals ml.ServiceRequestId
                               join e in dbContext.Exceptions on ml.MessageLogId equals e.MessageLogId
                               where sr.CustomerRefNum.Contains(value.ReferenceNo)
                               && e.ExceptionType.ExceptionGroupId == (int)ExceptionGroupEnum.BEQ
                                 && (sr.TenantId == tenantId || tenantId == (int)TenantIdEnum.LVIS)
                               select new
                               {
                                   MessageLogId = ml.MessageLogId,
                                   MessageLog = dbContext.MessageLogs.Where(ml => ml.MessageLogId == e.MessageLogId).FirstOrDefault(),
                                   ExceptionId = e.ExceptionId,
                                   ApplicationId = sr.ApplicationId,
                                   CustomerRefNum = sr.CustomerRefNum,
                                   ExternalRefNum = sr.ExternalRefNum,
                                   CreatedBy = e.CreatedById,
                                   CreatedDate = e.CreatedDate.ToString(),
                                   LastModifiedBy = e.LastModifiedById,
                                   LastModifiedDate = e.LastModifiedDate.ToString(),
                                   InternalRefId = sr.InternalRefId,
                                   InternalRefNum = sr.InternalRefNum,
                                   LocationId = sr.LocationId,
                                   ProviderId = sr.ProviderId,
                                   ServiceRequestId = sr.ServiceRequestId,
                                   ServiceId = sr.ServiceId,
                                   Comments = e.Comments,
                                   documentobjectid = e.DocumentObjectId,
                                   e
                               }).ToList();

                foreach (var se in Details)
                {
                    ExceptionDTO Detail = new ExceptionDTO();
                    Detail.Exceptionid = se.ExceptionId;

                    Detail.ExceptionTypeid = se.e.ExceptionTypeId;
                    Detail.ExceptionType = !string.IsNullOrEmpty(se.e.ExceptionType.ExceptionTypeName) ? se.e.ExceptionType.ExceptionTypeName : string.Empty;
                    Detail.ExceptionDesc = !string.IsNullOrEmpty(se.e.ExceptionDesc) ? se.e.ExceptionDesc : string.Empty;
                    Detail.CreatedBy = se.e.CreatedById == 0 ? string.Empty : dbContext.Tower_Users.Where(fi => fi.UserId == se.e.CreatedById).Select(sl => sl.UserName).FirstOrDefault();
                    Detail.CreatedDate = se.CreatedDate.ToString();
                    Detail.LastModifiedBy = se.e.LastModifiedById == 0 ? string.Empty : dbContext.Tower_Users.Where(fi => fi.UserId == se.e.LastModifiedById).Select(sl => sl.UserName).FirstOrDefault();
                    Detail.LastModifiedDate = se.LastModifiedDate.ToString();
                    Detail.Status = new ExceptionStatus() { ID = se.e.TypeCodeId, Name = dbContext.TypeCodes.Where(x => x.TypeCodeId == se.e.TypeCodeId).Select(x => x.TypeCodeDesc).FirstOrDefault() };
                    Detail.DocumentObjectid = se.MessageLog != null ? se.MessageLog.DocumentObjectId : se.e.DocumentObjectId;
                    Detail.ExternalRefNum = !string.IsNullOrEmpty(se.ExternalRefNum) ? se.ExternalRefNum : string.Empty;
                    Detail.MessageType = se.MessageLog != null ? se.MessageLog.MessageMap.MessageType.MessageTypeName : string.Empty;
                    Detail.MessageTypeid = se.MessageLog != null ? se.MessageLog.MessageMap.MessageTypeId : 0;
                    Detail.ServiceType = se.MessageLog != null ? se.MessageLog.ServiceRequest.Service.ServiceName : string.Empty;
                    Detail.ServiceRequestId = se.ServiceRequestId;
                    Detail.TypeCodeId = se.e.TypeCodeId;
                    Detail.ParentExternalRefNum = string.Empty;
                    Detail.Tenant = se.MessageLog != null ? se.MessageLog.Tenant.TenantName : string.Empty;
                    Detail.TenantId = se.MessageLog != null ? se.MessageLog.TenantId.Value : 0;

                    Detail.Reporting = new ReportingDTO();

                    if (Detail.ServiceRequestId > 0)
                    {
                        var ServiceReq = dbContext.ServiceRequests.Where(sel => sel.ServiceRequestId == Detail.ServiceRequestId)?.FirstOrDefault();
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
                        Detail.Reporting = new ReportingDTO()
                        {
                            ServiceRequestId = Detail.ServiceRequestId,
                            createddate = ServiceReq?.CreatedDate.ToString(),
                            CustomerRefNum = ServiceReq?.CustomerRefNum,
                            CustomerName = ServiceReq?.Location?.Customer.CustomerName,
                            ApplicationId = ServiceReq?.Application?.ApplicationName,
                            InternalRefNum = ServiceReq?.InternalRefNum,
                            LenderId = ServiceReq?.Location?.ExternalId
                        };
                        //Get incoming order 
                        Order order = GetIncomingOrderDTO(Detail.ServiceRequestId, dbContext);

                        //Transaction Type
                        string cannonicalLoanPurposeType = order != null ? GetTransaction(order.Loans) : string.Empty;
                        LoanPurposeTypeEnum loanPurposeType;
                        Enum.TryParse(cannonicalLoanPurposeType, out loanPurposeType);
                        Detail.TransactionType = dbContext.TypeCodes.Where(sel => sel.TypeCodeId == (int)loanPurposeType).Select(sl => sl.TypeCodeDesc).FirstOrDefault();

                        //Buyers
                        Detail.Buyer = order != null ? GetBuyerInfo(order.Parties) : string.Empty;

                    }

                    ExceptionDetails.Add(Detail);
                }
            }

            else if (value.ReferenceNoType == "4" && !string.IsNullOrEmpty(value.ReferenceNo))
            {
                var Details = (from sr in dbContext.ServiceRequests
                               join ml in dbContext.MessageLogs on sr.ServiceRequestId equals ml.ServiceRequestId
                               join e in dbContext.Exceptions on ml.MessageLogId equals e.MessageLogId
                               where sr.InternalRefId.ToString().Contains(value.ReferenceNo)
                               && e.ExceptionType.ExceptionGroupId == (int)ExceptionGroupEnum.BEQ
                                 && (sr.TenantId == tenantId || tenantId == (int)TenantIdEnum.LVIS)
                               select new
                               {
                                   MessageLogId = ml.MessageLogId,
                                   MessageLog = dbContext.MessageLogs.Where(ml => ml.MessageLogId == e.MessageLogId).FirstOrDefault(),
                                   ExceptionId = e.ExceptionId,
                                   ApplicationId = sr.ApplicationId,
                                   CustomerRefNum = sr.CustomerRefNum,
                                   ExternalRefNum = sr.ExternalRefNum,
                                   CreatedBy = e.CreatedById,
                                   CreatedDate = e.CreatedDate.ToString(),
                                   LastModifiedBy = e.LastModifiedById,
                                   LastModifiedDate = e.LastModifiedDate.ToString(),
                                   InternalRefId = sr.InternalRefId,
                                   InternalRefNum = sr.InternalRefNum,
                                   LocationId = sr.LocationId,
                                   ProviderId = sr.ProviderId,
                                   ServiceRequestId = sr.ServiceRequestId,
                                   ServiceId = sr.ServiceId,
                                   Comments = e.Comments,
                                   documentobjectid = e.DocumentObjectId,
                                   e
                               }).ToList();

                foreach (var se in Details)
                {
                    ExceptionDTO Detail = new ExceptionDTO();
                    Detail.Exceptionid = se.ExceptionId;

                    Detail.ExceptionTypeid = se.e.ExceptionTypeId;
                    Detail.ExceptionType = !string.IsNullOrEmpty(se.e.ExceptionType.ExceptionTypeName) ? se.e.ExceptionType.ExceptionTypeName : string.Empty;
                    Detail.ExceptionDesc = !string.IsNullOrEmpty(se.e.ExceptionDesc) ? se.e.ExceptionDesc : string.Empty;
                    Detail.CreatedBy = se.e.CreatedById == 0 ? string.Empty : dbContext.Tower_Users.Where(fi => fi.UserId == se.e.CreatedById).Select(sl => sl.UserName).FirstOrDefault();
                    Detail.CreatedDate = se.CreatedDate.ToString();
                    Detail.LastModifiedBy = se.e.LastModifiedById == 0 ? string.Empty : dbContext.Tower_Users.Where(fi => fi.UserId == se.e.LastModifiedById).Select(sl => sl.UserName).FirstOrDefault();
                    Detail.LastModifiedDate = se.LastModifiedDate.ToString();
                    Detail.Status = new ExceptionStatus() { ID = se.e.TypeCodeId, Name = dbContext.TypeCodes.Where(x => x.TypeCodeId == se.e.TypeCodeId).Select(x => x.TypeCodeDesc).FirstOrDefault() };
                    Detail.DocumentObjectid = se.MessageLog != null ? se.MessageLog.DocumentObjectId : se.e.DocumentObjectId;
                    Detail.ExternalRefNum = !string.IsNullOrEmpty(se.ExternalRefNum) ? se.ExternalRefNum : string.Empty;
                    Detail.MessageType = se.MessageLog != null ? se.MessageLog.MessageMap.MessageType.MessageTypeName : string.Empty;
                    Detail.MessageTypeid = se.MessageLog != null ? se.MessageLog.MessageMap.MessageTypeId : 0;
                    Detail.ServiceType = se.MessageLog != null ? se.MessageLog.ServiceRequest.Service.ServiceName : string.Empty;
                    Detail.ServiceRequestId = se.ServiceRequestId;
                    Detail.TypeCodeId = se.e.TypeCodeId;
                    Detail.ParentExternalRefNum = string.Empty;
                    Detail.Tenant = se.MessageLog != null ? se.MessageLog.Tenant.TenantName : string.Empty;
                    Detail.TenantId = se.MessageLog != null ? se.MessageLog.TenantId.Value : 0;

                    Detail.Reporting = new ReportingDTO();

                    if (Detail.ServiceRequestId > 0)
                    {
                        var ServiceReq = dbContext.ServiceRequests.Where(sel => sel.ServiceRequestId == Detail.ServiceRequestId)?.FirstOrDefault();
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
                        Detail.Reporting = new ReportingDTO()
                        {
                            ServiceRequestId = Detail.ServiceRequestId,
                            createddate = ServiceReq?.CreatedDate.ToString(),
                            CustomerRefNum = ServiceReq?.CustomerRefNum,
                            CustomerName = ServiceReq?.Location?.Customer.CustomerName,
                            ApplicationId = ServiceReq?.Application?.ApplicationName,
                            InternalRefNum = ServiceReq?.InternalRefNum,
                            LenderId = ServiceReq?.Location?.ExternalId
                        };
                        //Get incoming order 
                        Order order = GetIncomingOrderDTO(Detail.ServiceRequestId, dbContext);

                        //Transaction Type
                        string cannonicalLoanPurposeType = order != null ? GetTransaction(order.Loans) : string.Empty;
                        LoanPurposeTypeEnum loanPurposeType;
                        Enum.TryParse(cannonicalLoanPurposeType, out loanPurposeType);
                        Detail.TransactionType = dbContext.TypeCodes.Where(sel => sel.TypeCodeId == (int)loanPurposeType).Select(sl => sl.TypeCodeDesc).FirstOrDefault();

                        //Buyers
                        Detail.Buyer = order != null ? GetBuyerInfo(order.Parties) : string.Empty;

                    }

                    ExceptionDetails.Add(Detail);
                }
            }

            return ExceptionDetails;
        }

        public string BEQParentXml(int Servicerequestid)
        {
            using (Entities dbContext = new Entities())
            {
                TerminalDocumentEntities DbDocumentcontext = new TerminalDocumentEntities();

                MessageLog OrigMessageLogInfo = dbContext.MessageLogs.Where(se => se.ServiceRequestId == Servicerequestid).OrderBy(se => se.MessageLogId).FirstOrDefault();
                var doc1 = DbDocumentcontext.DocumentObjects.Where(se => se.DocumentObjectId == OrigMessageLogInfo.DocumentObjectId).FirstOrDefault();
                if (doc1 != null)
                {
                    string Content = "";
                    Content = doc1.Object;
                    if (string.IsNullOrEmpty(Content) && !string.IsNullOrEmpty(doc1.ObjectPath)
                         && File.Exists(doc1.ObjectPath))
                    {
                        Content = File.ReadAllText(doc1.ObjectPath);
                    }
                    return ReportingDataProvider.FormatXML(Content);
                }


                return string.Empty;
            }
        }

        public bool BEQUnBindOrder(PotentialMatchDTO bindMatch, int userId, int iTenantid)
        {
            TerminalDocumentEntities DbDocumentcontext = new TerminalDocumentEntities();
            List<ServiceRequest> ServiceRequests = new List<TerminalDBEntities.ServiceRequest>(); ;
            using (Entities dbContext = new Entities())
            {
                ServiceRequests = dbContext.ServiceRequests.Where(se => se.InternalRefId == bindMatch.FileID).ToList();
            }
            if (ServiceRequests.Count() == 0)
                return false;
            else
            {
                bool isResubmitSuccess = false;
                foreach (var Serv in ServiceRequests)
                {
                    XmlSerializer mySerializer = new XmlSerializer(typeof(Order));
                    Order order = new Order();
                    order.InternalRefNum = bindMatch.FileNumber;
                    order.InternalRefId = bindMatch.FileID.Value;
                    order.ServiceRequestId = Serv.ServiceRequestId;
                    order.IsFromExceptionQueue = true;
                    order.TenantId = Serv.TenantId.Value;
                    order.RecievedDateTime = Serv.CreatedDate;
                    order.OriginatingApplicationId = Serv.ApplicationId;

                    string CanonicalContent = Utils.SerializeToString<Order>(order);
                    DocumentObject Canonical = new DocumentObject();
                    Canonical.CreatedById = userId;
                    Canonical.CreatedDate = DateTime.Now;
                    Canonical.DocumentObjectFormat = "XML";
                    Canonical.Object = CanonicalContent;
                    DbDocumentcontext.DocumentObjects.Add(Canonical);
                    int Success = AuditLogHelper.SaveChanges(DbDocumentcontext);

                    Entities dbContext1 = new Entities();
                    MessageLog messageLog = new MessageLog()
                    {
                        CreatedById = userId,
                        LastModifiedById = userId,
                        CreatedDate = DateTime.Now,
                        LastModifiedDate = DateTime.Now,
                        MessageMapId = dbContext1.MessageMaps.Where(se => se.MessageTypeId == 10024 && se.TenantId == Serv.TenantId).FirstOrDefault() != null ? dbContext1.MessageMaps.Where(se => se.MessageTypeId == 10024 && se.TenantId == Serv.TenantId).FirstOrDefault().MessageMapId : 0,
                        TenantId = Serv.TenantId,
                        ServiceRequestId = Serv.ServiceRequestId,
                        DocumentObjectId = Canonical.DocumentObjectId,
                        ParentMessageLogId = null,
                        MessageLogDesc = "UnBind order",
                        RestartStep = null
                    };

                    dbContext1.MessageLogs.Add(messageLog);
                    AuditLogHelper.SaveChanges(dbContext1);

                    sLogger.Debug(string.Format("Publishing UNBIND message MessageLogId: {0} ", messageLog.MessageLogId));
                    ApplicationEnum Dest = (ApplicationEnum)Serv.ApplicationId;
                    if (EMSAdapter.PublishMessage("FAST", Dest.ToString().ToUpper(), Serv.ServiceRequestId, messageLog.MessageLogId, Canonical.DocumentObjectId.ToString()))
                    {
                        sLogger.Info(string.Format("Publishing UNBIND message MessageLogId: {0}  SUCCESSFUL", messageLog.MessageLogId));
                        isResubmitSuccess = true;
                    }
                    else
                    {
                        sLogger.Info(string.Format("Publishing UNBIND message MessageLogId: {0}  Failed", messageLog.MessageLogId));
                    }
                }
                return isResubmitSuccess;

            }
        }

        bool BEQUpdateRejectOrder(ExceptionDTO item, string externalRefnum, string internalRefNum, int internalRefId, int userId, int tenantid)
        {
            using (Entities dbContext = new Entities())
            {
                bool isSuccess = true;

                TerminalDBEntities.Exception ExceptionInfo = dbContext.Exceptions.Where(se => se.ExceptionId == item.Exceptionid).FirstOrDefault();
                TerminalDocumentEntities DbDocumentcontext = new TerminalDocumentEntities();

                string content = "";
                DocumentObject Docobject = DbDocumentcontext.DocumentObjects.Where(se => se.DocumentObjectId == ExceptionInfo.DocumentObjectId).FirstOrDefault();
                if (Docobject != null)
                {
                    content = Docobject.Object;
                    if (string.IsNullOrEmpty(content) && !string.IsNullOrEmpty(Docobject.ObjectPath)
                         && File.Exists(Docobject.ObjectPath))
                    {
                        content = File.ReadAllText(Docobject.ObjectPath);
                    }
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
                                 TrackingId = t.Element("TrackingId")?.Value
                             }).FirstOrDefault();

                SetTrackingId(items?.TrackingId);
                Order order = new Order();
                order.ExternalRefNum = externalRefnum;
                order.InternalRefNum = internalRefNum;
                order.InternalRefId = internalRefId;
                order.ServiceRequestId = item.ServiceRequestId;

                string CanonicalContent = Utils.SerializeToString<Order>(order);
                DocumentObject Canonical = new DocumentObject();
                Canonical.CreatedById = userId;
                Canonical.CreatedDate = DateTime.Now;
                Canonical.DocumentObjectFormat = "XML";
                Canonical.Object = CanonicalContent;
                DbDocumentcontext.DocumentObjects.Add(Canonical);
                int Success = AuditLogHelper.SaveChanges(DbDocumentcontext);

                MessageLog messageLog;

                bool isResubmitSuccess = false;
                if (ExceptionInfo.MessageLogId > 0)
                {
                    var ServiceReq = dbContext.ServiceRequests.Where(sel => sel.ServiceRequestId == item.ServiceRequestId)?.FirstOrDefault();

                    sLogger.Debug(string.Format("MessageLogId: {0} found", ExceptionInfo.MessageLogId));
                    MessageLog MessageLogInfo = dbContext.MessageLogs.Where(se => se.MessageLogId == ExceptionInfo.MessageLogId).FirstOrDefault();

                    var appId = MessageLogInfo.ServiceRequest.ApplicationId;

                    var messageMapId = (from mm in dbContext.MessageMaps
                                        join mt in dbContext.MessageTypes on mm.MessageTypeId equals mt.MessageTypeId
                                        join mt1 in dbContext.MessageTypes on mm.ExternalMessageTypeId equals mt1.MessageTypeId
                                        where mt1.ApplicationId == appId
                                        && mt.MessageTypeId == 10028
                                        && mm.TenantId == MessageLogInfo.TenantId
                                        select mm.MessageMapId).FirstOrDefault();

                    messageLog = new MessageLog()
                    {
                        CreatedById = userId,
                        LastModifiedById = userId,
                        CreatedDate = DateTime.Now,
                        LastModifiedDate = DateTime.Now,
                        //MessageMapId = dbContext.MessageMaps.Where(se => (se.MessageTypeId == 10028 && se.MessageType.ApplicationId == ServiceReq.ApplicationId) && se.TenantId == MessageLogInfo.TenantId).FirstOrDefault() != null ? 
                        //dbContext.MessageMaps.Where(se => (se.MessageTypeId == 10028 && se.MessageType.ApplicationId == ServiceReq.ApplicationId) && se.TenantId == MessageLogInfo.TenantId).FirstOrDefault().MessageMapId : 0,
                        MessageMapId = messageMapId,
                        TenantId = MessageLogInfo.TenantId,
                        ServiceRequestId = item.ServiceRequestId,
                        DocumentObjectId = Canonical.DocumentObjectId,
                        ParentMessageLogId = MessageLogInfo.ParentMessageLogId,
                        MessageLogDesc = "BEQUpdateOrder",
                        RestartStep = null,
                    };

                    dbContext.MessageLogs.Add(messageLog);
                    dbContext.SaveChanges();

                    string Dest = items.Destination?.Replace(DataContracts.Constants.APPLICATION_LVIS, "").Replace("QUEUE", "").Replace(".", "") ?? DataContracts.Constants.ENRICHMENT;

                    sLogger.Debug(string.Format("Publishing  Update External Reference ID message DocumentObjectId: {0} to Destination: {1}", Canonical.DocumentObjectId, Dest));

                    if (EMSAdapter.PublishMessage("FAST", items.Source.ToUpper(), messageLog.ServiceRequestId, messageLog.MessageLogId, Canonical.DocumentObjectId.ToString()))
                    {
                        sLogger.Info(string.Format("Publishing update External refernce id message DocumentObjectId: {0}  SUCCESSFUL", Canonical.DocumentObjectId));
                        isResubmitSuccess = true;
                    }
                    else
                    {
                        sLogger.Error(string.Format("Publishing  Update External Reference ID message DocumentObjectId: {0} FAILED", Canonical.DocumentObjectId));
                    }
                }

                if (isResubmitSuccess)
                {
                    string Status = ExceptionInfo.TypeCode.TypeCodeDesc;
                    ExceptionInfo.TypeCodeId = (int)ExceptionStatusEnum.Resolved;
                    ExceptionInfo.LastModifiedById = userId;
                    ExceptionInfo.LastModifiedDate = DateTime.Now;
                    dbContext.Entry(ExceptionInfo).State = System.Data.Entity.EntityState.Modified;
                    AuditLogHelper.SaveChanges(dbContext);

                    TerminalDBEntities.ExceptionNote ExceptionComment = new TerminalDBEntities.ExceptionNote();
                    ExceptionComment.ExceptionId = item.Exceptionid;
                    ExceptionComment.ExceptionNotes = "Update and Reject External Reference ID";
                    ExceptionComment.CreatedById = userId;
                    ExceptionComment.CreatedDate = DateTime.Now;

                    dbContext.ExceptionNotes.Add(ExceptionComment);
                    AuditLogHelper.SaveChanges(dbContext);
                    TerminalDBEntities.ServiceRequest serviceReq = new TerminalDBEntities.ServiceRequest();
                    serviceReq.LastModifiedById = userId;
                    serviceReq.LastModifiedDate = DateTime.Now;

                    dbContext.ServiceRequests.Add(serviceReq);
                    AuditLogHelper.SaveChanges(dbContext);
                }
                else
                {
                    TerminalDBEntities.ExceptionNote ExceptionComment = new TerminalDBEntities.ExceptionNote();
                    ExceptionComment.ExceptionId = item.Exceptionid;
                    ExceptionComment.ExceptionNotes = "Error Update and Reject External Reference ID";
                    ExceptionComment.CreatedById = userId;
                    ExceptionComment.CreatedDate = DateTime.Now;

                    dbContext.ExceptionNotes.Add(ExceptionComment);
                    AuditLogHelper.SaveChanges(dbContext);
                    isSuccess = false;
                }

                return isSuccess;
            }
        }

        bool BEQUpdateOrder(ExceptionDTO item, string externalRefnum, string internalRefNum, int internalRefId, int userId, int tenantid)
        {
            using (Entities dbContext = new Entities())
            {
                bool isSuccess = true;

                TerminalDBEntities.Exception ExceptionInfo = dbContext.Exceptions.Where(se => se.ExceptionId == item.Exceptionid).FirstOrDefault();
                TerminalDocumentEntities DbDocumentcontext = new TerminalDocumentEntities();

                string content = "";
                DocumentObject Docobject = DbDocumentcontext.DocumentObjects.Where(se => se.DocumentObjectId == ExceptionInfo.DocumentObjectId).FirstOrDefault();
                if (Docobject != null)
                {
                    content = Docobject.Object;
                    if (string.IsNullOrEmpty(content) 
                        && !string.IsNullOrEmpty(Docobject.ObjectPath)
                          && File.Exists(Docobject.ObjectPath))
                    {
                        content = File.ReadAllText(Docobject.ObjectPath);
                    }
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
                                 TrackingId = t.Element("TrackingId")?.Value
                             }).FirstOrDefault();

                SetTrackingId(items?.TrackingId);

                Order order = new Order();
                order.ExternalRefNum = externalRefnum;
                order.InternalRefNum = internalRefNum;
                order.InternalRefId = internalRefId;
                order.ServiceRequestId = item.ServiceRequestId;
                
                string CanonicalContent = Utils.SerializeToString<Order>(order);

                DocumentObject Canonical = new DocumentObject();
                Canonical.CreatedById = userId;
                Canonical.CreatedDate = DateTime.Now;
                Canonical.DocumentObjectFormat = "XML";
                Canonical.Object = CanonicalContent;
                DbDocumentcontext.DocumentObjects.Add(Canonical);
                int Success = AuditLogHelper.SaveChanges(DbDocumentcontext);

                MessageLog messageLog;

                bool isResubmitSuccess = false;
                if (ExceptionInfo.MessageLogId > 0)
                {
                    sLogger.Debug(string.Format("MessageLogId: {0} found", ExceptionInfo.MessageLogId));
                    MessageLog MessageLogInfo = dbContext.MessageLogs.Where(se => se.MessageLogId == ExceptionInfo.MessageLogId).FirstOrDefault();

                    var appId = MessageLogInfo.ServiceRequest.ApplicationId;

                    var messageMapId = (from mm in dbContext.MessageMaps
                                        join mt in dbContext.MessageTypes on mm.MessageTypeId equals mt.MessageTypeId
                                        join mt1 in dbContext.MessageTypes on mm.ExternalMessageTypeId equals mt1.MessageTypeId
                                        where mt1.ApplicationId == appId
                                        && mt.MessageTypeId == 10026
                                        && mm.TenantId == MessageLogInfo.TenantId
                                        select mm.MessageMapId).FirstOrDefault();

                    messageLog = new MessageLog()
                    {
                        CreatedById = userId,
                        LastModifiedById = userId,
                        CreatedDate = DateTime.Now,
                        LastModifiedDate = DateTime.Now,
                        //MessageMapId = dbContext.MessageMaps.Where(se => se.MessageTypeId == 10026 && se.TenantId == MessageLogInfo.TenantId).FirstOrDefault() != null
                        //? dbContext.MessageMaps.Where(se => se.MessageTypeId == 10026 && se.TenantId == MessageLogInfo.TenantId).FirstOrDefault().MessageMapId : 0,
                        MessageMapId = messageMapId,
                        TenantId = MessageLogInfo.TenantId,
                        ServiceRequestId = item.ServiceRequestId,
                        DocumentObjectId = Canonical.DocumentObjectId,
                        ParentMessageLogId = MessageLogInfo.ParentMessageLogId,
                        MessageLogDesc = "BEQUpdateOrder",
                        RestartStep = null,
                    };

                    dbContext.MessageLogs.Add(messageLog);
                    dbContext.SaveChanges();

                    string Dest = items.Destination?.Replace(DataContracts.Constants.APPLICATION_LVIS, "").Replace("QUEUE", "").Replace(".", "") ?? DataContracts.Constants.ENRICHMENT;

                    sLogger.Debug(string.Format("Publishing Update External Reference Number message DocumentObjectId: {0} to Destination: {1}", Canonical.DocumentObjectId, Dest));

                    if (EMSAdapter.PublishMessage("FAST", items.Source.ToUpper(), messageLog.ServiceRequestId, messageLog.MessageLogId, Canonical.DocumentObjectId.ToString()))
                    {
                        sLogger.Info(string.Format("Publishing Update External Refernce Number message DocumentObjectId: {0} SUCCESSFUL", Canonical.DocumentObjectId));
                        isResubmitSuccess = true;
                    }
                    else
                    {
                        sLogger.Error(string.Format("Publishing Update External Reference Number message DocumentObjectId: {0} FAILED", Canonical.DocumentObjectId));
                    }
                }

                if (isResubmitSuccess)
                {
                    string Status = ExceptionInfo.TypeCode.TypeCodeDesc;
                    ExceptionInfo.TypeCodeId = (int)ExceptionStatusEnum.Resolved;
                    ExceptionInfo.LastModifiedById = userId;
                    ExceptionInfo.LastModifiedDate = DateTime.Now;
                    dbContext.Entry(ExceptionInfo).State = System.Data.Entity.EntityState.Modified;
                    AuditLogHelper.SaveChanges(dbContext);

                    TerminalDBEntities.ExceptionNote EceptionComment = new TerminalDBEntities.ExceptionNote();
                    EceptionComment.ExceptionId = item.Exceptionid;
                    EceptionComment.ExceptionNotes = "Update External Reference Number";
                    EceptionComment.CreatedById = userId;
                    EceptionComment.CreatedDate = DateTime.Now;

                    dbContext.ExceptionNotes.Add(EceptionComment);
                    AuditLogHelper.SaveChanges(dbContext);
                }
                else
                {
                    TerminalDBEntities.ExceptionNote EceptionComment = new TerminalDBEntities.ExceptionNote();
                    EceptionComment.ExceptionId = item.Exceptionid;
                    EceptionComment.ExceptionNotes = "Error in Update External Reference Number";
                    EceptionComment.CreatedById = userId;
                    EceptionComment.CreatedDate = DateTime.Now;

                    dbContext.ExceptionNotes.Add(EceptionComment);
                    AuditLogHelper.SaveChanges(dbContext);
                    isSuccess = false;
                }

                return isSuccess;
            }
        }

        public bool BEQUpdate(ExceptionDTO matchException, string externalRefnum, string internalRefNum, int internalRefId, int userId, int tenantid)
        {
            bool Success = false;
            if (matchException.Exceptionid == 0)
            {
                foreach (var item in matchException.children.Where(se => !se.InvolveResolved))
                {
                    Success = BEQUpdateOrder(item, externalRefnum, internalRefNum, internalRefId, userId, tenantid);

                }
                return Success;

            }
            else
                return BEQUpdateOrder(matchException, externalRefnum, internalRefNum, internalRefId, userId, tenantid);
        }

        public bool BEQUpdateReject(ExceptionDTO matchException, string externalRefnum, string internalRefNum, int internalRefId, int userId, int tenantid)
        {
            bool Success = false;
            if (matchException.Exceptionid == 0)
            {
                foreach (var item in matchException.children.Where(se => !se.InvolveResolved))
                {
                    Success = BEQUpdateRejectOrder(item, externalRefnum, internalRefNum, internalRefId, userId, tenantid);

                }
                return Success;

            }
            else
                return BEQUpdateRejectOrder(matchException, externalRefnum, internalRefNum, internalRefId, userId, tenantid);
        }

        public bool DeleteBEQOrder(ExceptionDTO matchException, int userId, string fileNotes)
        {
            using (var dbContext = new Entities())
            {
                var orderDelete = (from beqexception in dbContext.Exceptions
                                   where beqexception.ExceptionId == matchException.Exceptionid
                                   select beqexception).FirstOrDefault();

                bool isDeleteSuccess = false;
                if (orderDelete != null)
                {
                    string Status = orderDelete.TypeCode.TypeCodeDesc;
                    orderDelete.LastModifiedById = userId;
                    orderDelete.LastModifiedDate = DateTime.Now;
                    orderDelete.TypeCodeId = (int)ExceptionStatusEnum.Resolved;
                    //orderDelete.Comments = fileNotes;
                    dbContext.Entry(orderDelete).State = System.Data.Entity.EntityState.Modified;
                    int Success = AuditLogHelper.SaveChanges(dbContext);
                    if (orderDelete.MessageLogId > 0)
                    {
                        sLogger.Debug(string.Format("MessageLogId: {0} found", orderDelete.MessageLogId));
                        MessageLog MessageLogInfo = dbContext.MessageLogs.Where(se => se.MessageLogId == orderDelete.MessageLogId).FirstOrDefault();
                        var exceptionListIds = GetExceptionIds(MessageLogInfo.ServiceRequestId);
                        if (exceptionListIds != null && exceptionListIds.Count > 0)
                        {
                            foreach (int id in exceptionListIds)
                            {
                                TerminalDBEntities.Exception exceptionData = dbContext.Exceptions.Where(se => se.ExceptionId == id).FirstOrDefault();
                                exceptionData.TypeCodeId = (int)ExceptionStatusEnum.Resolved;
                                exceptionData.LastModifiedById = userId;
                                exceptionData.LastModifiedDate = DateTime.Now;
                                dbContext.Entry(exceptionData).State = System.Data.Entity.EntityState.Modified;
                                AuditLogHelper.SaveChanges(dbContext);                                
                            }
                        }

                    }
                    isDeleteSuccess = true;
                    if (isDeleteSuccess)
                    {
                        TerminalDBEntities.ExceptionNote EceptionComment = new TerminalDBEntities.ExceptionNote();
                        EceptionComment.ExceptionId = matchException.Exceptionid;
                        EceptionComment.ExceptionNotes = "DELETE completed, exception status changed from " + Status + " to " + ExceptionStatusEnum.Resolved.ToString() + " and resolved all the associated exceptions from TEQ";
                        EceptionComment.CreatedById = userId;
                        EceptionComment.CreatedDate = DateTime.Now;

                        dbContext.ExceptionNotes.Add(EceptionComment);
                        AuditLogHelper.SaveChanges(dbContext);
                        return true;
                    }
                    else
                    {
                        TerminalDBEntities.ExceptionNote EceptionComment = new TerminalDBEntities.ExceptionNote();
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
            return false;
        }

        public bool BEQDeleteOrderdetails(ExceptionDTO matchException, int userId, string fileNotes)
        {
            bool Status = true;
            if (matchException.Exceptionid == 0)
            {
                foreach (var child in matchException.children.Where(se => !se.InvolveResolved))
                {
                    Status = DeleteBEQOrder(child, userId, fileNotes);
                }
            }
            else
                Status = DeleteBEQOrder(matchException, userId, fileNotes);
            return Status;
        }

        public ExceptionDTO BEQResubmitException(ExceptionDTO ExceptionDetails, int userId)
        {
            TerminalDBEntities.Exception ExceptionInfo;
            if (ExceptionDetails.children != null && ExceptionDetails.children.Count() > 0 && ExceptionDetails.Exceptionid == 0)
            {
                foreach (var s in ExceptionDetails.children.Where(se => !se.InvolveResolved))
                {
                    using (Entities dbContext = new Entities())
                    {
                        ExceptionInfo = dbContext.Exceptions.Where(se => se.ExceptionId == s.Exceptionid).FirstOrDefault();
                    }

                    if (ExceptionInfo != null)
                    {
                        var dest = DataContracts.Constants.CONVOY;
                        var result = PostBEQResubmit(ExceptionInfo, userId, dest, ExceptionDetails.ParentExternalRefNum);
                        if (result)
                        {
                            AddNotesBEQ(ExceptionInfo, userId, result, ExceptionDetails);
                        }
                    }
                }
            }
            else
            {
                using (Entities dbContext = new Entities())
                {
                    ExceptionInfo = dbContext.Exceptions.Where(se => se.ExceptionId == ExceptionDetails.Exceptionid).FirstOrDefault();
                }

                if (ExceptionInfo != null)
                {
                    var result = PostBEQResubmit(ExceptionInfo, userId);
                    if (result)
                    {
                        AddNotesBEQ(ExceptionInfo, userId, result, ExceptionDetails);
                    }
                }
            }

            return ExceptionDetails;
        }

        private void AddNotesBEQ(TerminalDBEntities.Exception ExceptionInfo, int userId, bool isResubmitSuccess, ExceptionDTO ExceptionDetails)
        {
            using (Entities dbContext = new Entities())
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

        private bool PostBEQResubmit(TerminalDBEntities.Exception ExceptionInfo, int userId, string dest = null, string externalRefNum = null)
        {
            var isResubmitSuccess = false;
            MessageLog messageLog;

            using (Entities dbContext = new Entities())
            {
                //TerminalDBEntities.Exception ExceptionInfo = dbContext.Exceptions.Where(se => se.ExceptionId == ExceptionDetails.ExceptionId).FirstOrDefault();
                TerminalDocumentEntities DbDocumentcontext = new TerminalDocumentEntities();
                string content = "";
                DocumentObject Docobject = DbDocumentcontext.DocumentObjects.Where(se => se.DocumentObjectId == ExceptionInfo.DocumentObjectId).FirstOrDefault();
                if (Docobject != null)
                {
                    content = Docobject.Object;
                    if (string.IsNullOrEmpty(content) 
                        && !string.IsNullOrEmpty(Docobject.ObjectPath)
                         && File.Exists(Docobject.ObjectPath))
                    {
                        content = File.ReadAllText(Docobject.ObjectPath);
                    }
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
                                 TrackingId = t.Element("TrackingId")?.Value
                             }).FirstOrDefault();

                SetTrackingId(items?.TrackingId);
                string docObjContent = "";
                DocumentObject Docobjectitems = DbDocumentcontext.DocumentObjects.Where(se => se.DocumentObjectId == items.DocumentObjectId).FirstOrDefault();
                docObjContent = Docobjectitems.Object;
                if (string.IsNullOrEmpty(docObjContent)
                    && !string.IsNullOrEmpty(Docobjectitems.ObjectPath)
                      && File.Exists(Docobjectitems.ObjectPath))
                {
                    docObjContent = File.ReadAllText(Docobjectitems.ObjectPath);
                }
                if (ExceptionInfo.MessageLogId > 0)
                {
                    sLogger.Info(string.Format("MessageLogId: {0} found", ExceptionInfo.MessageLogId));
                    MessageLog MessageLogInfo = dbContext.MessageLogs.Where(se => se.MessageLogId == ExceptionInfo.MessageLogId).FirstOrDefault();

                    messageLog = new MessageLog()
                    {
                        CreatedById = userId,
                        LastModifiedById = userId,
                        CreatedDate = DateTime.Now,
                        LastModifiedDate = DateTime.Now,
                        MessageMapId = MessageLogInfo.MessageMapId,
                        TenantId = MessageLogInfo.TenantId,
                        ServiceRequestId = MessageLogInfo.ServiceRequestId,
                        DocumentObjectId = items.DocumentObjectId,
                        ParentMessageLogId = MessageLogInfo.ParentMessageLogId,
                        MessageLogDesc = MessageLogInfo.MessageLogDesc,
                        RestartStep = Convert.ToInt16(items.RestartStep?.ToString())
                    };
                    dbContext.MessageLogs.Add(messageLog);
                    AuditLogHelper.SaveChanges(dbContext);

                    if (messageLog != null && items != null)
                    {
                        if (string.IsNullOrEmpty(dest))
                            dest = items.Destination?.Replace(DataContracts.Constants.APPLICATION_LVIS, "").Replace("QUEUE", "").Replace(".", "") ?? DataContracts.Constants.ENRICHMENT;
                        sLogger.Debug(string.Format("Publishing resubmit message DocumentObjectId: {0} to Destination: {1}", items.DocumentObjectId, dest));

                        if (EMSAdapter.PublishMessage(dest, items.Source, messageLog.ServiceRequestId, messageLog.MessageLogId, items.DocumentObjectId.ToString(), externalRefNum, DateTime.Now))
                        {
                            sLogger.Info(string.Format("PublishingMessage resubmit message DocumentObjectId: {0} SUCCESSFUL", items.DocumentObjectId));
                            isResubmitSuccess = true;
                        }
                        else
                        {
                            sLogger.Error(string.Format("PublishingMessage resubmit message DocumentObjectId: {0} FAILED", items.DocumentObjectId.ToString()));
                        }
                    }
                }
                else
                {
                    sLogger.Info("MessageLogId missing for Exception");
                    if (items != null)
                    {
                        if (string.IsNullOrEmpty(dest))
                            dest = items.Destination?.Replace(DataContracts.Constants.APPLICATION_LVIS, "").Replace("QUEUE", "").Replace(".", "") ?? DataContracts.Constants.ENRICHMENT;
                        sLogger.Debug(string.Format("Publishing resubmit message DocumentObjectId: {0} to Destination: {1}", items.DocumentObjectId, dest));

                        if (EMSAdapter.PublishMessage(dest, items.Source, items.DocumentObjectId))
                        {
                            sLogger.Info(string.Format("Publishing resubmit message DocumentObjectId: {0}  SUCCESSFUL", items.DocumentObjectId));
                            isResubmitSuccess = true;
                        }
                        else
                        {
                            sLogger.Error(string.Format("Publishing resubmit message DocumentObjectId: {0} FAILED", items.DocumentObjectId.ToString()));
                        }
                    }
                }
            }

            return isResubmitSuccess;
        }



        public bool sendMail(string subject, string emailTo, string body)
        {
            try
            {
                if (!string.IsNullOrEmpty(body) && body.Contains("\n"))
                    body = body.Replace("\n", "<br>");
                string emailFrom = Utils.GetConfig("EmailFrom");
                string cc = Utils.GetConfig("EmailCC");
                string defaultSubject = Utils.GetConfig("EmailSubject");
                if (string.IsNullOrEmpty(emailFrom) || string.IsNullOrEmpty(emailTo))
                    throw new System.Exception("Sender and Recipient is mandatory");
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(emailFrom);
                foreach (var address in emailTo.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    mailMessage.To.Add(address);
                }

                if (!string.IsNullOrEmpty(cc) && !string.IsNullOrWhiteSpace(cc))
                {
                    foreach (var address in cc.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        mailMessage.CC.Add(address);
                    }
                }

                if (!string.IsNullOrEmpty(defaultSubject) && string.IsNullOrEmpty(subject))
                    subject = defaultSubject;
                mailMessage.Subject = subject;
                mailMessage.Body = body;
                mailMessage.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.corp.firstam.com";
                smtp.UseDefaultCredentials = true;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Port = 25;
                smtp.Send(mailMessage);
            }
            catch (System.Exception Ex)
            {
                throw new System.Exception(string.Format("Failed to send Email due to {0}", Ex));
            }

            return true;
        }

        #endregion " BEQ "

        #region " TEQ "

        public IEnumerable<DataContracts.DashBoardExceptionDTO> GetTEQExceptions(int tenantId)
        {
            List<DataContracts.DashBoardExceptionDTO> ExceptionsList = new List<DataContracts.DashBoardExceptionDTO>();

            using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            }))
            {
                using (Entities LvisDbcontext = new Entities())
                {
                    var resultIds = LvisDbcontext.ExceptionTypes.Where(se => se.ExceptionGroupId == (int)ExceptionGroupEnum.TEQ);


                    if (tenantId == (int)TerminalDBEntities.TenantIdEnum.LVIS)
                    {
                        foreach (var Execption in resultIds)
                        {
                            DataContracts.DashBoardExceptionDTO dto = new DashBoardExceptionDTO();
                            dto.ExceptionName = Execption.ExceptionTypeName.Replace("_", " ");
                            dto.NoOfExceptions = LvisDbcontext.Exceptions.Where(se => se.ExceptionTypeId == Execption.ExceptionTypeId && se.TypeCodeId != (int)ExceptionStatusEnum.Resolved).Count();
                            dto.DateTime = LvisDbcontext.Exceptions.Where(se => se.ExceptionTypeId == Execption.ExceptionTypeId).Count() > 0 ?
                                        LvisDbcontext.Exceptions.Where(se => se.ExceptionTypeId == Execption.ExceptionTypeId).Max(me => me.CreatedDate).ToString()
                                        : DateTime.Now.ToString();
                            dto.ThreshholdCount = Execption.ThresholdCount;
                            ExceptionsList.Add(dto);
                        }
                    }
                    else
                    {
                        foreach (var Execption in resultIds)
                        {
                            DataContracts.DashBoardExceptionDTO dto = new DashBoardExceptionDTO();
                            dto.ExceptionName = Execption.ExceptionTypeName.Replace("_", " ");
                            dto.NoOfExceptions = LvisDbcontext.Exceptions.Where(se => se.ExceptionTypeId == Execption.ExceptionTypeId && se.TypeCodeId != (int)ExceptionStatusEnum.Resolved
                                 && tenantId == (se.MessageLogId != 0 ? LvisDbcontext.MessageLogs.Where(ml => ml.MessageLogId == se.MessageLogId).FirstOrDefault().TenantId : 0)).Count();
                            dto.DateTime = LvisDbcontext.Exceptions.Where(se => se.ExceptionTypeId == Execption.ExceptionTypeId
                                             && tenantId == (se.MessageLogId != 0 ? LvisDbcontext.MessageLogs.Where(ml => ml.MessageLogId == se.MessageLogId).FirstOrDefault().TenantId : 0)).Count() > 0 ?
                                        LvisDbcontext.Exceptions.Where(se => se.ExceptionTypeId == Execption.ExceptionTypeId
                                        && tenantId == (se.MessageLogId != 0 ? LvisDbcontext.MessageLogs.Where(ml => ml.MessageLogId == se.MessageLogId).FirstOrDefault().TenantId : 0)).Max(me => me.CreatedDate).ToString()
                                        : DateTime.Now.ToString();
                            dto.ThreshholdCount = Execption.ThresholdCount;
                            ExceptionsList.Add(dto);
                        }
                    }
                }
            }

            return ExceptionsList.Where(ex => ex.ExceptionName != null);
        }

        public IEnumerable<DashBoardGraphicalExceptionDTO> GetTEQGraphs(int tenantId)
        {
            DateTime? date = DateTime.Now.AddHours(-12);

            List<DashBoardGraphicalExceptionDTO> Graphicalcount = new List<DashBoardGraphicalExceptionDTO>();
            using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            }))
            {
                using (Entities dbContext = new Entities())
                {
                    if (tenantId == (int)TenantIdEnum.LVIS)
                    {
                        var ExceptionLogitems = dbContext.ExceptionQueueLogs.Where(y => y.ExceptionGroupId == (int)ExceptionGroupEnum.TEQ
                              && y.CreateDate >= date).ToList();

                        Graphicalcount = GetGraphicalList(ExceptionLogitems);
                    }
                    else
                    {
                        var ExceptionLogitems = dbContext.ExceptionQueueLogs.Where(y => y.ExceptionGroupId == (int)ExceptionGroupEnum.TEQ
                              && y.TenantId == tenantId
                              && y.CreateDate >= date).ToList();

                        Graphicalcount = GetGraphicalList(ExceptionLogitems);
                    }
                }

                scope.Complete();
            }

            Graphicalcount.Reverse();

            return Graphicalcount;
        }

        public IEnumerable<ExceptionDTO> GetTEQExceptions(SearchDetail value, int tenantId)
        {
            DateTime startDateTime = Convert.ToDateTime(value.Fromdate);

            DateTime endDateTime = Convert.ToDateTime(value.ThroughDate);
            endDateTime = endDateTime.Subtract(startDateTime.TimeOfDay).AddDays(1).AddMilliseconds(-1);

            bool isIncludeResolved = value.Typecodestatus;

            var ret = GetExceptionInfo_TEQ(startDateTime, endDateTime, (int)ExceptionGroupEnum.TEQ, tenantId, isIncludeResolved);

            if (ret == null)
            {
                ret = new List<ExceptionDTO>();
            }

            return ret;
        }

        public IEnumerable<MessageLogDetailDTO> GetMessageDetails(int exceptionId)
        {
            Entities dbContext = new Entities();
            TerminalDBEntities.TerminalDocumentEntities DbDocumentcontext = new TerminalDBEntities.TerminalDocumentEntities();

            List<MessageLogDetailDTO> MessageLogs = new List<MessageLogDetailDTO>();
            var Logid = dbContext.Exceptions.Where(sel => sel.ExceptionId == exceptionId).Select(se => se.MessageLogId).FirstOrDefault();

            var MesageLog = dbContext.MessageLogs.Where(sel => sel.MessageLogId == Logid).FirstOrDefault();

            if (MesageLog != null)
            {
                var DocLog = DbDocumentcontext.DocumentObjects.Where(se => se.DocumentObjectId == MesageLog.DocumentObjectId).FirstOrDefault();

                MessageLogDetailDTO LogDetail = new MessageLogDetailDTO();
                LogDetail.Application = MesageLog.MessageMap.MessageType.Application.ApplicationName;
                LogDetail.ExternalApplication = dbContext.MessageTypes.Where(sel => sel.MessageTypeId == MesageLog.MessageMap.ExternalMessageTypeId).Select(app => app.Application.ApplicationName).FirstOrDefault();
                LogDetail.IsIncoming = MesageLog.MessageMap.IsInbound;
                LogDetail.DataFormat = DocLog.DocumentObjectFormat;
                string Content = "";
                if (DocLog != null)
                {
                    Content = DocLog.Object;
                    if (string.IsNullOrEmpty(Content) && !string.IsNullOrEmpty(DocLog.ObjectPath)
                       && File.Exists(DocLog.ObjectPath))
                    {
                        Content = File.ReadAllText(DocLog.ObjectPath);
                    }
                }
                LogDetail.DataContent = ReportingDataProvider.FormatXML(Content);
                LogDetail.CreatedDateTime = DocLog.CreatedDate.ToString();
                LogDetail.Description = dbContext.MessageTypes.Where(sel => sel.MessageTypeId == MesageLog.MessageMap.ExternalMessageTypeId).Select(app => app.MessageTypeName).FirstOrDefault();
                MessageLogs.Add(LogDetail);
            }

            return MessageLogs;
        }

        public IEnumerable<ExceptionDTO> GetTEQExceptions(string sFilter, int tenantId, bool isIncludeResolved)
        {
            DateTime startDateTime = DateTime.Today;
            DateTime endDateTime = DateTime.Today;
            if (sFilter == "2") //2-Represents select All From DB
            {
                startDateTime = startDateTime.Subtract(startDateTime.TimeOfDay).AddYears(-1); //To Pull data from Inception Date.
                endDateTime = endDateTime.Subtract(startDateTime.TimeOfDay).AddDays(1).AddMilliseconds(-1);
            }
            else if (sFilter == "19")
            {
                startDateTime = Convert.ToDateTime("1/01/2016 12:00:00 AM"); ;  //To Pull data from Inception Date.
                endDateTime = endDateTime.Subtract(startDateTime.TimeOfDay).AddDays(1).AddMilliseconds(-1);
            }
            else if (sFilter.Contains("24"))
            {
                startDateTime = DateTime.Now;
                startDateTime = startDateTime.AddDays(-1);
                endDateTime = DateTime.Now;
            }
            else
            {
                endDateTime = endDateTime.Subtract(startDateTime.TimeOfDay).AddDays(1).AddMilliseconds(-1);
                startDateTime = startDateTime.AddDays(-(int.Parse(sFilter)));
            }

            var ret = GetExceptionInfo_TEQ(startDateTime, endDateTime, (int)ExceptionGroupEnum.TEQ, tenantId, isIncludeResolved);

            if (ret == null)
            {
                return ret = new List<ExceptionDTO>();
            }

            return ret;
        }

        private List<ExceptionDTO> GetExceptionInfo_BEQ(DateTime startDate, DateTime endDate, int exceptionGroup, int tenantId, bool isIncludeResolved, string exceptionTypeName = null)
        {
            List<ExceptionDTO> det = new List<ExceptionDTO>();

            using (var dbContext = new Entities())
            {
                dbContext.Database.CommandTimeout = 120;

                var result = dbContext.GetExceptionInfo_BEQ(startDate, endDate, exceptionGroup, tenantId, isIncludeResolved, exceptionTypeName).ToList();

                if (result != null)
                {
                    foreach (var se in result)
                    {
                        var ExceptionTypeID = dbContext.ExceptionTypes.Where(de => de.ExceptionTypeName == se.ExceptionTypeName).FirstOrDefault();
                        ExceptionDTO Detail = new ExceptionDTO();
                        Detail.Exceptionid = se.ExceptionId.GetValueOrDefault();
                        Detail.ExceptionTypeid = ExceptionTypeID != null ? ExceptionTypeID.ExceptionTypeId : 0;
                        Detail.ExceptionType = !string.IsNullOrEmpty(se.ExceptionTypeName) ? se.ExceptionTypeName : string.Empty;
                        Detail.ExceptionDesc = !string.IsNullOrEmpty(se.ExceptionDesc) ? se.ExceptionDesc : string.Empty;
                        Detail.CreatedBy = (string.IsNullOrEmpty(se.CreatedBy) && se.CreatedById == 0 ? string.Empty : se.CreatedBy);
                        Detail.CreatedDate = se.CreatedDate.ToString();
                        Detail.LastModifiedBy = (string.IsNullOrEmpty(se.LastModifiedBy) && se.LastModifiedById == 0 ? string.Empty : se.LastModifiedBy);
                        Detail.LastModifiedDate = se.LastModifiedDate.ToString();
                        Detail.Status = new ExceptionStatus() { ID = se.TypeCodeId.GetValueOrDefault(), Name = se.ExceptionStatus };
                        Detail.DocumentObjectid = se.DocumentObjectId.HasValue ? se.DocumentObjectId.Value : 0;
                        Detail.ExternalRefNum = !string.IsNullOrEmpty(se.ExternalRefNum) ? se.ExternalRefNum : string.Empty;
                        Detail.MessageType = !string.IsNullOrEmpty(se.MessageTypeName) ? se.MessageTypeName : string.Empty;
                        Detail.MessageTypeid = se.MessageTypeId.HasValue ? se.MessageTypeId.Value : 0;
                        Detail.ServiceType = !string.IsNullOrEmpty(se.ServiceName) ? se.ServiceName : string.Empty;
                        Detail.ServiceRequestId = se.ServiceRequestId.HasValue ? se.ServiceRequestId.Value : 0;
                        Detail.TypeCodeId = se.TypeCodeId.GetValueOrDefault();
                        Detail.ParentExternalRefNum = string.Empty;
                        Detail.Tenant = !string.IsNullOrEmpty(se.TenantName) ? se.TenantName : string.Empty;
                        Detail.TenantId = dbContext.Tenants.Where(te => te.TenantName == Detail.Tenant).Select(sl => sl.TenantId).FirstOrDefault();

                        Detail.Reporting = new ReportingDTO();

                        if (Detail.ServiceRequestId > 0 && exceptionGroup == (int)ExceptionGroupEnum.BEQ)
                        {
                            var ServiceReq = dbContext.ServiceRequests.Where(sel => sel.ServiceRequestId == Detail.ServiceRequestId)?.FirstOrDefault();
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
                            Detail.Reporting = new ReportingDTO()
                            {
                                ServiceRequestId = Detail.ServiceRequestId,
                                createddate = ServiceReq?.CreatedDate.ToString(),
                                CustomerRefNum = ServiceReq?.CustomerRefNum,
                                CustomerName = ServiceReq?.Location?.Customer.CustomerName,
                                ApplicationId = ServiceReq?.Application?.ApplicationName,
                                InternalRefNum = ServiceReq?.InternalRefNum,
                                LenderId = ServiceReq?.Location?.ExternalId
                            };

                            if (!string.IsNullOrEmpty(se.BuyerName) && !string.IsNullOrEmpty(se.TransactionType))
                            {
                                //Buyers
                                Detail.Buyer = se.BuyerName;
                                //Transaction Type
                                LoanPurposeTypeEnum loanPurposeType;
                                Enum.TryParse(se.TransactionType, out loanPurposeType);
                                Detail.TransactionType = dbContext.TypeCodes.Where(sel => sel.TypeCodeId == (int)loanPurposeType).Select(sl => sl.TypeCodeDesc).FirstOrDefault();
                            }
                            else
                            {
                                //Get incoming order 
                                Order order = GetIncomingOrderDTO(Detail.ServiceRequestId, dbContext);

                                //Transaction Type
                                string cannonicalLoanPurposeType = order != null ? GetTransaction(order.Loans) : string.Empty;
                                LoanPurposeTypeEnum loanPurposeType;
                                Enum.TryParse(cannonicalLoanPurposeType, out loanPurposeType);
                                Detail.TransactionType = dbContext.TypeCodes.Where(sel => sel.TypeCodeId == (int)loanPurposeType).Select(sl => sl.TypeCodeDesc).FirstOrDefault();

                                //Buyers
                                Detail.Buyer = order != null ? GetBuyerInfo(order.Parties) : string.Empty;
                            }
                        }

                        det.Add(Detail);
                    }
                }
            }

            return det;

        }


        private List<ExceptionDTO> GetExceptionInfo_TEQ(DateTime startDate, DateTime endDate, int exceptionGroup, int tenantId, bool isIncludeResolved, string exceptionTypeName = null)
        {
            List<ExceptionDTO> det = new List<ExceptionDTO>();

            using (var dbContext = new Entities())
            {
                var result = dbContext.GetExceptionInfo(startDate, endDate, exceptionGroup, tenantId, isIncludeResolved, exceptionTypeName).ToList();

                if (result != null)
                {
                    foreach (var se in result)
                    {
                        var ExceptionTypeID = dbContext.ExceptionTypes.Where(de => de.ExceptionTypeName == se.ExceptionTypeName).FirstOrDefault();
                        ExceptionDTO Detail = new ExceptionDTO();
                        Detail.Exceptionid = se.ExceptionId;
                        Detail.ExceptionTypeid = ExceptionTypeID != null ? ExceptionTypeID.ExceptionTypeId : 0;
                        Detail.ExceptionType = !string.IsNullOrEmpty(se.ExceptionTypeName) ? se.ExceptionTypeName : string.Empty;
                        Detail.ExceptionDesc = !string.IsNullOrEmpty(se.ExceptionDesc) ? se.ExceptionDesc : string.Empty;
                        Detail.CreatedBy = (string.IsNullOrEmpty(se.CreatedBy) && se.CreatedById == 0 ? string.Empty : se.CreatedBy);
                        Detail.CreatedDate = se.CreatedDate.ToString();
                        Detail.LastModifiedBy = (string.IsNullOrEmpty(se.LastModifiedBy) && se.LastModifiedById == 0 ? string.Empty : se.LastModifiedBy);
                        Detail.LastModifiedDate = se.LastModifiedDate.ToString();
                        Detail.Status = new ExceptionStatus() { ID = se.TypeCodeId, Name = se.ExceptionStatus };
                        if (!string.IsNullOrEmpty(se.DocObjectPath)
                            && File.Exists(se.DocObjectPath))
                        {
                            string content = File.ReadAllText(se.DocObjectPath);
                            var items = (from t in XDocument.Parse(content)?.Descendants(DataContracts.Constants.EXCEPTION)
                                         select new
                                         {
                                             DocumentObjectId = Convert.ToInt64(t.Element("DocumentObjectId")?.Value),
                                             ExternalRefNumber = t.Element("ExternalRefNum")?.Value,
                                         }).FirstOrDefault();
                            se.ExternalRefNum = items.ExternalRefNumber;
                            se.DocumentObjectId = items.DocumentObjectId;
                        }
                        Detail.DocumentObjectid = se.DocumentObjectId.HasValue ? se.DocumentObjectId.Value : 0;
                        Detail.ExternalRefNum = !string.IsNullOrEmpty(se.ExternalRefNum) ? se.ExternalRefNum : string.Empty;
                        Detail.MessageType = !string.IsNullOrEmpty(se.MessageTypeName) ? se.MessageTypeName : string.Empty;
                        Detail.MessageTypeid = se.MessageTypeId.HasValue ? se.MessageTypeId.Value : 0;
                        Detail.ServiceType = !string.IsNullOrEmpty(se.ServiceName) ? se.ServiceName : string.Empty;
                        Detail.ServiceRequestId = se.ServiceRequestId.HasValue ? se.ServiceRequestId.Value : 0;
                        Detail.TypeCodeId = se.TypeCodeId;
                        Detail.ParentExternalRefNum = string.Empty;
                        Detail.Tenant = !string.IsNullOrEmpty(se.TenantName) ? se.TenantName : string.Empty;
                        Detail.TenantId = dbContext.Tenants.Where(te => te.TenantName == Detail.Tenant).Select(sl => sl.TenantId).FirstOrDefault();

                        Detail.Reporting = new ReportingDTO();

                        det.Add(Detail);
                    }
                }
            }

            return det;

        }

        public string GetTransactionType(List<MessageLogDetailDTO> msgLogs)
        {
            TerminalDBEntities.TerminalDocumentEntities DbDocumentcontext = new TerminalDBEntities.TerminalDocumentEntities();
            string transactionType = "";
            if (msgLogs != null)
            {
                foreach (var se in msgLogs)
                {
                    if (se.IsIncoming == true && se.ParentMessageLogId > 0)
                    {
                        string Content = "";
                        DocumentObject Docobject = DbDocumentcontext.DocumentObjects.Where(ms => ms.DocumentObjectId == se.Documentobjectid).FirstOrDefault();
                        if (Docobject != null)
                        {
                            Content = Docobject.Object;
                            if (string.IsNullOrEmpty(Content) 
                                && !string.IsNullOrEmpty(Docobject.ObjectPath)
                                 && File.Exists(Docobject.ObjectPath))
                            {
                                Content = File.ReadAllText(Docobject.ObjectPath);
                            }
                        }
                        try
                        {
                            //XmlSerializer mySerializer = new XmlSerializer(typeof(Order));
                            //Order order = (Order)mySerializer.Deserialize(new StringReader(Content));
                            Order order = Utils.DeSerializeToObject<Order>(Content);
                            if (order.Loans != null)
                            {
                                transactionType = order.Loans[0].LoanPurposeType.ToString();
                                break;
                            }
                        }
                        catch
                        {
                            transactionType = string.Empty;
                        }
                    }
                }
            }

            return transactionType;
        }


        /// <summary>
        /// Get canonical transaction type from Loans
        /// </summary>
        /// <param name="Loans"></param>
        /// <returns></returns>
        public string GetTransaction(List<Loan> Loans)
        {

            string transaction = string.Empty;

            try
            {
                if (Loans != null)
                {
                    transaction = Loans[0].LoanPurposeType.ToString();
                }
            }
            catch
            {
                return string.Empty;
            }

            return transaction;
        }

        /// <summary>
        /// Get Incoming Order 
        /// </summary>
        /// <param name="msgLogs"></param>
        /// <returns></returns>
        public Order GetIncomingOrderDTO(int ServiceRequestId, Entities dbContext)
        {
            Order order = null;

            if (dbContext != null)
            {
                List<MessageLogDetailDTO> msgLogs = new List<MessageLogDetailDTO>();
                msgLogs = dbContext.MessageLogs.Where(sel => sel.ServiceRequestId == ServiceRequestId && sel.MessageMap.IsInbound == true && sel.ParentMessageLogId > 0)
                        .Select(x => new MessageLogDetailDTO
                        {
                            IsIncoming = x.MessageMap.IsInbound,
                            ParentMessageLogId = x.ParentMessageLogId >= 0 ? (int)x.ParentMessageLogId : 0,
                            Documentobjectid = x.DocumentObjectId,
                        }).ToList();

                TerminalDBEntities.TerminalDocumentEntities DbDocumentcontext = new TerminalDBEntities.TerminalDocumentEntities();

                if (msgLogs != null)
                {
                    foreach (var se in msgLogs)
                    {
                        if (se.IsIncoming == true && se.ParentMessageLogId > 0)
                        {

                            string Content = "";
                            DocumentObject Docobject = DbDocumentcontext.DocumentObjects.Where(ms => ms.DocumentObjectId == se.Documentobjectid).FirstOrDefault();
                            if (Docobject != null)
                            {
                                Content = Docobject.Object;
                                if (string.IsNullOrEmpty(Content)
                                    && !string.IsNullOrEmpty(Docobject.ObjectPath)
                                     && File.Exists(Docobject.ObjectPath))
                                {
                                    try
                                    {
                                        Content = File.ReadAllText(Docobject.ObjectPath);
                                    }
                                    catch
                                    {
                                        break;

                                    }

                                }
                                try
                                {
                                    //XmlSerializer mySerializer = new XmlSerializer(typeof(Order));
                                    //order = (Order)mySerializer.Deserialize(new StringReader(Content));

                                    order = Utils.DeSerializeToObject<Order>(Content);
                                    if (order != null) { break; }
                                }
                                catch { continue; }
                            }
                        }
                    }
                }
            }

            return order;
        }

        //private string GetMessageContent(long documentObjectId)
        //{
        //    using (TerminalDocumentEntities DbDocumentcontext = new TerminalDocumentEntities())
        //    {
        //        var content = DbDocumentcontext.DocumentObjects.Where(se => se.DocumentObjectId == documentObjectId).FirstOrDefault();

        //        if (content != null)
        //        {
        //            var items = (from t in XDocument.Parse(content.Object)?.Descendants(DataContracts.Constants.EXCEPTION)
        //                         select new
        //                         {
        //                             DocumentObjectId = Convert.ToInt64(t.Element("DocumentObjectId")?.Value),
        //                         }).FirstOrDefault();

        //            string messageContent = string.Empty;

        //            if (items.DocumentObjectId > 0)
        //                messageContent = ReportingDataProvider.FormatXML(DbDocumentcontext.DocumentObjects.Where(se => se.DocumentObjectId == items.DocumentObjectId).FirstOrDefault().Object);

        //            return messageContent;
        //        }

        //        return string.Empty;
        //    }
        //}

        private long GetMessageContentDocumentObject(long documentObjectId)
        {
            long docObjectId = 0;
            using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            }))
            {
                using (TerminalDocumentEntities DbDocumentcontext = new TerminalDocumentEntities())
                {
                    var content = DbDocumentcontext.DocumentObjects.Where(se => se.DocumentObjectId == documentObjectId).FirstOrDefault();


                    if (content != null)
                    {
                        string DataContent = "";
                        DataContent = content.Object;
                        if (string.IsNullOrEmpty(DataContent)
                            && !string.IsNullOrEmpty(content.ObjectPath)
                             && File.Exists(content.ObjectPath))
                        {
                            DataContent = File.ReadAllText(content.ObjectPath);
                        }

                        var items = (from t in XDocument.Parse(DataContent)?.Descendants(DataContracts.Constants.EXCEPTION)
                                     select new
                                     {
                                         DocumentObjectId = Convert.ToInt64(t.Element("DocumentObjectId")?.Value),
                                     }).FirstOrDefault();

                        docObjectId = (items != null ? items.DocumentObjectId : 0);
                    }
                }

                scope.Complete();
            }

            return docObjectId;
        }

        //private string GetExternalRefNum(long documentObjectId)
        //{
        //    string extRefNum = string.Empty;
        //    using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
        //    {
        //        IsolationLevel = IsolationLevel.ReadUncommitted
        //    }))
        //    {
        //        using (var DbDocumentcontext = new TerminalDocumentEntities())
        //        {
        //            var content = DbDocumentcontext.DocumentObjects.Where(se => se.DocumentObjectId == documentObjectId).FirstOrDefault();
        //            if (content != null)
        //            {
        //                var items = (from t in XDocument.Parse(content.Object)?.Descendants(DataContracts.Constants.EXCEPTION)
        //                             select new
        //                             {
        //                                 ExternalRefNumber = t.Element("ExternalRefNum")?.Value,
        //                             }).FirstOrDefault();

        //                extRefNum = items?.ExternalRefNumber;
        //            }
        //        }

        //        scope.Complete();
        //    }

        //    return extRefNum;
        //}

        public ExceptionDTO SaveExceptionComments(ExceptionDTO Exception, int userId)
        {
            using (Entities dbContext = new Entities())
            {
                TerminalDBEntities.Exception ExceptionUpdate = dbContext.Exceptions.Where(se => se.ExceptionId == Exception.Exceptionid).FirstOrDefault();
                if (ExceptionUpdate.TypeCodeId != Exception.Status.ID)
                {
                    ExceptionUpdate.ExceptionNotes.Add(new ExceptionNote() { CreatedById = userId, ExceptionId = Exception.Exceptionid, CreatedDate = DateTime.Now, ExceptionNotes = "Changed Status from " + ExceptionUpdate.TypeCode.TypeCodeDesc + " to " + Exception.Status.Name });
                    ExceptionUpdate.TypeCodeId = Exception.Status.ID;
                    ExceptionUpdate.LastModifiedById = userId;
                    ExceptionUpdate.LastModifiedDate = DateTime.Now;
                    dbContext.Entry(ExceptionUpdate).State = System.Data.Entity.EntityState.Modified;
                    AuditLogHelper.SaveChanges(dbContext);
                }
            }
            using (Entities dbContext = new Entities())
            {
                TerminalDBEntities.ExceptionNote EceptionComment = new TerminalDBEntities.ExceptionNote();
                EceptionComment.ExceptionId = Exception.Exceptionid;
                EceptionComment.ExceptionNotes = Exception.Notes;
                EceptionComment.CreatedById = userId;
                EceptionComment.CreatedDate = DateTime.Now;

                dbContext.ExceptionNotes.Add(EceptionComment);
                AuditLogHelper.SaveChanges(dbContext);
                Exception.Comments = dbContext.ExceptionNotes.Where(se => se.ExceptionId == Exception.Exceptionid).OrderBy(sl => sl.CreatedDate)
                        .Select(ENote => ENote.CreatedDate + " " + dbContext.Tower_Users.Where(fi => fi.UserId == ENote.CreatedById).Select(sl => sl.UserName).FirstOrDefault() + " " + ENote.ExceptionNotes).ToList();
                //Exception.LastModifiedDate = DateTime.Now.ToString();
            }

            return Exception;
        }

        public List<string> GetExceptionNotes(int exceptionid)
        {
            using (Entities dbContext = new Entities())
            {
                if (dbContext.ExceptionNotes.Where(se => se.ExceptionId == exceptionid).Count() > 0)
                {
                    return dbContext.ExceptionNotes.Where(se => se.ExceptionId == exceptionid).OrderBy(sl => sl.CreatedDate)
                                .Select(ENote => ENote.CreatedDate + " " + dbContext.Tower_Users.Where(fi => fi.UserId == ENote.CreatedById).Select(sl => sl.UserName).FirstOrDefault() + " " + ENote.ExceptionNotes).ToList();
                }
                return new List<string>();
            }
        }

        public List<string> GetBEQExceptionNotes(int exceptionid, string serviceType)
        {
            using (Entities dbContext = new Entities())
            {
                if (dbContext.ExceptionNotes.Where(se => se.ExceptionId == exceptionid).Count() > 0)
                {

                    return dbContext.ExceptionNotes.Where(se => se.ExceptionId == exceptionid).OrderBy(sl => sl.CreatedDate)
                                .Select(ENote => string.IsNullOrEmpty(serviceType) ?
                                                (ENote.CreatedDate + " " + dbContext.Tower_Users.Where(fi => fi.UserId == ENote.CreatedById).Select(sl => sl.UserName).FirstOrDefault() + " " + ENote.ExceptionNotes) :
                                                (serviceType + ": " + ENote.CreatedDate + " " + dbContext.Tower_Users.Where(fi => fi.UserId == ENote.CreatedById).Select(sl => sl.UserName).FirstOrDefault() + " " + ENote.ExceptionNotes)).ToList();
                }

                return new List<string>();
            }
        }

        public IEnumerable<ExceptionStatus> GetExceptionStatus()
        {
            using (Entities dbContext = new Entities())
            {
                List<ExceptionStatus> Typecodes = dbContext.TypeCodes.Where(se => se.GroupTypeCode == 200).Select(sl => new ExceptionStatus
                {
                    ID = sl.TypeCodeId,
                    Name = sl.TypeCodeDesc
                }).ToList();

                return Typecodes;
            }
        }

        public ExceptionDTO ResubmitException(ExceptionDTO ExceptionDetails, int userId)
        {
            using (Entities dbContext = new Entities())
            {
                TerminalDBEntities.ServiceRequest serviceRequest = dbContext.ServiceRequests.Where(se => se.ServiceRequestId == ExceptionDetails.ServiceRequestId).FirstOrDefault();
                TerminalDBEntities.Exception ExceptionInfo = dbContext.Exceptions.Where(se => se.ExceptionId == ExceptionDetails.Exceptionid).FirstOrDefault();
                TerminalDocumentEntities DbDocumentcontext = new TerminalDocumentEntities();
                string content = "";
                DocumentObject Docobject = DbDocumentcontext.DocumentObjects.Where(se => se.DocumentObjectId == ExceptionInfo.DocumentObjectId).FirstOrDefault();
                if (Docobject != null)
                {
                    content = Docobject.Object;
                    if (string.IsNullOrEmpty(content) && !string.IsNullOrEmpty(Docobject.ObjectPath)
                         && File.Exists(Docobject.ObjectPath))
                    {
                        content = File.ReadAllText(Docobject.ObjectPath);
                    }
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
                                 InternalRefNum = t.Element("InternalRefNum")?.Value,
                                 TrackingId = t.Element("TrackingId")?.Value
                             }).FirstOrDefault();

                SetTrackingId(items?.TrackingId);
                DocumentObject docObj = new DocumentObject()
                {
                    Object = ExceptionDetails.MessageContent,
                    DocumentObjectFormat = "xml",
                    CreatedById = userId,
                    CreatedDate = DateTime.Now
                };
                DbDocumentcontext.DocumentObjects.Add(docObj);
                DbDocumentcontext.SaveChanges();

                sLogger.Info(string.Format("New DocumentObject added successfully, DocumentObjectId: {0}", docObj.DocumentObjectId));

                var isResubmitSuccess = false;
                MessageLog messageLog;

                if (ExceptionInfo.MessageLogId > 0)
                {
                    sLogger.Info(string.Format("MessageLogId: {0} found", ExceptionInfo.MessageLogId));
                    MessageLog MessageLogInfo = dbContext.MessageLogs.Where(se => se.MessageLogId == ExceptionInfo.MessageLogId).FirstOrDefault();

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
                        //US2068715: modified the condition not to remove the "." from ENRICHMENT.FAST
                        string dest = string.Empty;
                        if ((!string.IsNullOrWhiteSpace(items.Destination)) && items.Destination.ToUpper().Equals(DataContracts.Constants.ENRICHMENT_FAST_QUEUE))
                        {
                            dest = items.Destination?.Replace(DataContracts.Constants.APPLICATION_LVIS, "").Replace("QUEUE", "");
                        }
                        else {
                            dest = items.Destination?.Replace(DataContracts.Constants.APPLICATION_LVIS, "").Replace("QUEUE", "").Replace(".", "") ?? DataContracts.Constants.ENRICHMENT;
                        }
                        if (dest == "FAST" && serviceRequest!=null &&(serviceRequest.InternalRefId == 0 || serviceRequest.InternalRefNum == null || serviceRequest.InternalRefId == null))
                        {
                            dest = "CONVOY";
                        }
                        sLogger.Debug(string.Format("Publishing resubmit message DocumentObjectId: {0} to Destination: {1}", docObj.DocumentObjectId, dest));
                        if (dest == "EVENTS")
                        {
                            EMSAdapter.PublishMessageToEventsQueue(dest, items.Source, docObj.DocumentObjectId, items.TagRef, items.ProcessName, items.ProcessType, items.ServiceFileProcessID, items.OrderSourceId, items.SecondOrderSourceID, items.RegionID, items.ObjectCD);
                            sLogger.Info(string.Format("Publishing resubmit message DocumentObjectId: {0}  SUCCESSFUL", docObj.DocumentObjectId));
                            isResubmitSuccess = true;
                        }
                        else if (EMSAdapter.PublishMessage(dest, items.Source, messageLog.ServiceRequestId, messageLog.MessageLogId, docObj.DocumentObjectId.ToString(), null, null, items.TagRef, items.InternalRefNum))
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
                    sLogger.Info("MessageLogId missing for Exception");
                    if (items != null)
                    {
                        //US2068715: modified the condition not to remove the "." from ENRICHMENT.FAST
                        string dest = string.Empty;
                        if ((!string.IsNullOrWhiteSpace(items.Destination)) && items.Destination.ToUpper().Equals(DataContracts.Constants.ENRICHMENT_FAST_QUEUE))
                        {
                            dest = items.Destination?.Replace(DataContracts.Constants.APPLICATION_LVIS, "").Replace("QUEUE", "");
                        }
                        else
                        {
                            dest = items.Destination?.Replace(DataContracts.Constants.APPLICATION_LVIS, "").Replace("QUEUE", "").Replace(".", "") ?? DataContracts.Constants.ENRICHMENT;
                        }                        
                        sLogger.Debug(string.Format("Publishing resubmit message DocumentObjectId: {0} to Destination: {1}", docObj.DocumentObjectId, dest));
                        if (dest == "EVENTS")
                        {
                            EMSAdapter.PublishMessageToEventsQueue(dest, items.Source, docObj.DocumentObjectId, items.TagRef, items.ProcessName, items.ProcessType, items.ServiceFileProcessID, items.OrderSourceId, items.SecondOrderSourceID, items.RegionID, items.ObjectCD);
                            sLogger.Info(string.Format("Publishing resubmit message DocumentObjectId: {0}  SUCCESSFUL", docObj.DocumentObjectId));
                            isResubmitSuccess = true;
                        }
                        else if (EMSAdapter.PublishMessage(dest, items.Source, docObj.DocumentObjectId, ((items.InternalRefNum != null) ? items.InternalRefNum : ""), items.TagRef))
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

                if (isResubmitSuccess)
                {
                    ExceptionInfo.TypeCodeId = (int)ExceptionStatusEnum.Resolved;
                    ExceptionInfo.LastModifiedById = userId;
                    ExceptionInfo.LastModifiedDate = DateTime.Now;
                    ExceptionInfo.ExceptionNotes.Add(
                        new ExceptionNote()
                        {
                            CreatedById = userId,
                            ExceptionId = ExceptionDetails.Exceptionid,
                            CreatedDate = DateTime.Now,
                            ExceptionNotes = "Resubmitted, exception status changed from " + ExceptionDetails.Status.Name + " to " + ExceptionStatusEnum.Resolved.ToString()
                        });

                    dbContext.Entry(ExceptionInfo).State = System.Data.Entity.EntityState.Modified;
                    AuditLogHelper.SaveChanges(dbContext);

                    ExceptionDetails.Comments = dbContext.ExceptionNotes.Where(se => se.ExceptionId == ExceptionDetails.Exceptionid).OrderByDescending(sl => sl.CreatedDate)
                .Select(ENote => ENote.CreatedDate + " " + dbContext.Tower_Users.Where(fi => fi.UserId == ENote.CreatedById).Select(sl => sl.UserName).FirstOrDefault() + " " + ENote.ExceptionNotes).ToList();
                    ExceptionDetails.LastModifiedDate = ExceptionInfo.LastModifiedDate.ToString();
                    ExceptionDetails.Status = new ExceptionStatus { ID = (int)ExceptionStatusEnum.Resolved, Name = ExceptionStatusEnum.Resolved.ToString() };
                    ExceptionDetails.LastModifiedBy = dbContext.Tower_Users.Where(fi => fi.UserId == userId).Select(sl => sl.UserName).FirstOrDefault();
                }
                else
                {
                    ExceptionInfo.TypeCodeId = (int)ExceptionStatusEnum.Active;
                    ExceptionInfo.LastModifiedById = userId;
                    ExceptionInfo.LastModifiedDate = DateTime.Now;
                    ExceptionInfo.ExceptionNotes.Add(
                        new ExceptionNote()
                        {
                            CreatedById = userId,
                            ExceptionId = ExceptionDetails.Exceptionid,
                            CreatedDate = DateTime.Now,
                            ExceptionNotes = "There was an error resubmitting/publishing message to EMS"
                        });
                    dbContext.Entry(ExceptionInfo).State = System.Data.Entity.EntityState.Modified;
                    AuditLogHelper.SaveChanges(dbContext);

                    ExceptionDetails.Comments = dbContext.ExceptionNotes.Where(se => se.ExceptionId == ExceptionDetails.Exceptionid).OrderByDescending(sl => sl.CreatedDate)
                .Select(ENote => ENote.CreatedDate + " " + dbContext.Tower_Users.Where(fi => fi.UserId == ENote.CreatedById).Select(sl => sl.UserName).FirstOrDefault() + " " + ENote.ExceptionNotes).ToList();
                    ExceptionDetails.LastModifiedDate = ExceptionInfo.LastModifiedDate.ToString();
                    ExceptionDetails.Status = new ExceptionStatus { ID = (int)ExceptionStatusEnum.Active, Name = ExceptionStatusEnum.Active.ToString() };
                    ExceptionDetails.LastModifiedBy = dbContext.Tower_Users.Where(fi => fi.UserId == userId).Select(sl => sl.UserName).FirstOrDefault();
                }
            }
            return ExceptionDetails;
        }

        // PDL
        public ExceptionDTO ResubmitExceptionToHttp(ExceptionDTO ExceptionDetails, int userId)
        {
            using (Entities dbContext = new Entities())
            {
                TerminalDBEntities.Exception ExceptionInfo = dbContext.Exceptions.Where(se => se.ExceptionId == ExceptionDetails.Exceptionid).FirstOrDefault();
                TerminalDocumentEntities DbDocumentcontext = new TerminalDocumentEntities();
                string content = "";
                DocumentObject Docobject = DbDocumentcontext.DocumentObjects.Where(se => se.DocumentObjectId == ExceptionInfo.DocumentObjectId).FirstOrDefault();
                if (Docobject != null)
                {
                    content = Docobject.Object;
                    if (string.IsNullOrEmpty(content) && !string.IsNullOrEmpty(Docobject.ObjectPath)
                         && File.Exists(Docobject.ObjectPath))
                    {
                        content = File.ReadAllText(Docobject.ObjectPath);
                    }
                }

                DocumentObject docObj = new DocumentObject()
                {
                    Object = ExceptionDetails.MessageContent,
                    DocumentObjectFormat = "json",
                    CreatedById = userId,
                    CreatedDate = DateTime.Now
                };
                DbDocumentcontext.DocumentObjects.Add(docObj);
                DbDocumentcontext.SaveChanges();

                sLogger.Info(string.Format("New DocumentObject added successfully, DocumentObjectId: {0}", docObj.DocumentObjectId));

                var isResubmitSuccess = false;

                string dest = "";

                if (ExceptionDetails.ExceptionTypeid == (int)Data.TerminalDBEntities.ExceptionTypeEnum.DomainNotConfigured)
                {
                    dest = "OpenApi.WebhookController.POST";
                }

                sLogger.Debug(string.Format("Publishing resubmit message DocumentObjectId: {0} to Destination: {1}", docObj.DocumentObjectId, dest));

                var response = "";
                var errorMessage = "";
                try
                {
                    WebhookAdapter webhookAdapter = new WebhookAdapter();
                    response = webhookAdapter.PostWebhook(ExceptionDetails.MessageContent);
                    sLogger.Info(string.Format("Publishing resubmit message DocumentObjectId: {0}  SUCCESSFUL", docObj.DocumentObjectId));
                    isResubmitSuccess = true;
                }
                catch (System.Exception e)
                {
                    sLogger.Info(string.Format("Publishing resubmit message DocumentObjectId: {0}  FAILED", docObj.DocumentObjectId));
                    isResubmitSuccess = false;
                    errorMessage = e.Message;

                }

                if (isResubmitSuccess)
                {
                    ExceptionInfo.TypeCodeId = (int)ExceptionStatusEnum.Resolved;
                    ExceptionInfo.LastModifiedById = userId;
                    ExceptionInfo.LastModifiedDate = DateTime.Now;
                    ExceptionInfo.ExceptionNotes.Add(
                        new ExceptionNote()
                        {
                            CreatedById = userId,
                            ExceptionId = ExceptionDetails.Exceptionid,
                            CreatedDate = DateTime.Now,
                            ExceptionNotes = "Resubmitted, exception status changed from " + ExceptionDetails.Status.Name + " to " + ExceptionStatusEnum.Resolved.ToString()
                        });

                    dbContext.Entry(ExceptionInfo).State = System.Data.Entity.EntityState.Modified;
                    AuditLogHelper.SaveChanges(dbContext);

                    ExceptionDetails.Comments = dbContext.ExceptionNotes.Where(se => se.ExceptionId == ExceptionDetails.Exceptionid).OrderByDescending(sl => sl.CreatedDate)
                .Select(ENote => ENote.CreatedDate + " " + dbContext.Tower_Users.Where(fi => fi.UserId == ENote.CreatedById).Select(sl => sl.UserName).FirstOrDefault() + " " + ENote.ExceptionNotes).ToList();
                    ExceptionDetails.LastModifiedDate = ExceptionInfo.LastModifiedDate.ToString();
                    ExceptionDetails.Status = new ExceptionStatus { ID = (int)ExceptionStatusEnum.Resolved, Name = ExceptionStatusEnum.Resolved.ToString() };
                    ExceptionDetails.LastModifiedBy = dbContext.Tower_Users.Where(fi => fi.UserId == userId).Select(sl => sl.UserName).FirstOrDefault();
                }
                else
                {
                    ExceptionInfo.TypeCodeId = (int)ExceptionStatusEnum.Active;
                    ExceptionInfo.LastModifiedById = userId;
                    ExceptionInfo.LastModifiedDate = DateTime.Now;
                    ExceptionInfo.ExceptionNotes.Add(
                        new ExceptionNote()
                        {
                            CreatedById = userId,
                            ExceptionId = ExceptionDetails.Exceptionid,
                            CreatedDate = DateTime.Now,
                            ExceptionNotes = $"There was an error resubmitting message to {dest}.  {errorMessage}"
                        });
                    dbContext.Entry(ExceptionInfo).State = System.Data.Entity.EntityState.Modified;
                    AuditLogHelper.SaveChanges(dbContext);

                    ExceptionDetails.Comments = dbContext.ExceptionNotes.Where(se => se.ExceptionId == ExceptionDetails.Exceptionid).OrderByDescending(sl => sl.CreatedDate)
                .Select(ENote => ENote.CreatedDate + " " + dbContext.Tower_Users.Where(fi => fi.UserId == ENote.CreatedById).Select(sl => sl.UserName).FirstOrDefault() + " " + ENote.ExceptionNotes).ToList();
                    ExceptionDetails.LastModifiedDate = ExceptionInfo.LastModifiedDate.ToString();
                    ExceptionDetails.Status = new ExceptionStatus { ID = (int)ExceptionStatusEnum.Active, Name = ExceptionStatusEnum.Active.ToString() };
                    ExceptionDetails.LastModifiedBy = dbContext.Tower_Users.Where(fi => fi.UserId == userId).Select(sl => sl.UserName).FirstOrDefault();
                }
            }
            return ExceptionDetails;
        }

        public Tuple<ResubmitBulkExceptionDTO, List<ExceptionDTO>> BulkResubmitException(List<ExceptionDTO> exceptionDetails, int userId)
        {
            List<ExceptionDTO> ReturnExceptionDetails = new List<ExceptionDTO>();
            ResubmitBulkExceptionDTO resubmitExceptionDTO = new ResubmitBulkExceptionDTO();
            using (Entities dbContext = new Entities())
            {
                List<ExceptionDTO> ExceptionIds = new List<ExceptionDTO>();
                ExceptionIds = exceptionDetails.Select(se => new ExceptionDTO()
                {
                    Exceptionid = se.Exceptionid,
                }).ToList();
                if (ExceptionIds != null)
                {
                    int scuccessfullysubmittedcnt = 0;
                    int unscuccessfullysubmittedcnt = 0;

                    foreach (var item in exceptionDetails)
                    {
                        TerminalDBEntities.ServiceRequest serviceRequest = dbContext.ServiceRequests.Where(se => se.ServiceRequestId == item.ServiceRequestId).FirstOrDefault();
                        TerminalDBEntities.Exception ExceptionInfo = dbContext.Exceptions.Where(se => se.ExceptionId == item.Exceptionid).FirstOrDefault();
                        TerminalDocumentEntities DbDocumentcontext = new TerminalDocumentEntities();
                        string content = "";
                        DocumentObject Docobject = DbDocumentcontext.DocumentObjects.Where(se => se.DocumentObjectId == ExceptionInfo.DocumentObjectId).FirstOrDefault();
                        if (Docobject != null)
                        {
                            content = Docobject.Object;
                            if (string.IsNullOrEmpty(content)
                                && !string.IsNullOrEmpty(Docobject.ObjectPath)
                                 && File.Exists(Docobject.ObjectPath))
                            {
                                content = File.ReadAllText(Docobject.ObjectPath);
                            }
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
                                         InternalRefNum = t.Element("InternalRefNum")?.Value,
                                         TrackingId  = t.Element("TrackingId")?.Value
                                     }).FirstOrDefault();

                        SetTrackingId(items?.TrackingId);

                        var DocumentObjectId = dbContext.MessageLogs.Where(se => se.MessageLogId == ExceptionInfo.MessageLogId).Select(s => s.DocumentObjectId).FirstOrDefault();

                        if (DocumentObjectId == 0)
                            DocumentObjectId = Convert.ToInt64(items.DocumentObjectId);

                        if (DocumentObjectId == 0)
                            continue;

                        var isResubmitSuccess = false;
                        MessageLog messageLog;

                        if (ExceptionInfo.MessageLogId > 0)
                        {
                            sLogger.Info(string.Format("MessageLogId: {0} found", ExceptionInfo.MessageLogId));
                            MessageLog MessageLogInfo = dbContext.MessageLogs.Where(se => se.MessageLogId == ExceptionInfo.MessageLogId).FirstOrDefault();

                            messageLog = new MessageLog()
                            {
                                CreatedById = userId,
                                LastModifiedById = userId,
                                CreatedDate = DateTime.Now,
                                LastModifiedDate = DateTime.Now,
                                MessageMapId = MessageLogInfo.MessageMapId,
                                TenantId = MessageLogInfo.TenantId,
                                ServiceRequestId = MessageLogInfo.ServiceRequestId,
                                DocumentObjectId = DocumentObjectId,
                                ParentMessageLogId = MessageLogInfo.ParentMessageLogId,
                                MessageLogDesc = MessageLogInfo.MessageLogDesc,
                                RestartStep = Convert.ToInt16(items.RestartStep?.ToString())
                            };
                            dbContext.MessageLogs.Add(messageLog);
                            dbContext.SaveChanges();

                            if (messageLog != null && items != null)
                            {
                                string dest = items.Destination?.Replace(DataContracts.Constants.APPLICATION_LVIS, "").Replace("QUEUE", "").Replace(".", "") ?? DataContracts.Constants.ENRICHMENT;
                                if (dest == "FAST" && serviceRequest != null && (serviceRequest.InternalRefId == 0 || serviceRequest.InternalRefNum == null || serviceRequest.InternalRefId == null))
                                {
                                    dest = "CONVOY";
                                }
                                sLogger.Debug(string.Format("Publishing resubmit message DocumentObjectId: {0} to Destination: {1}", DocumentObjectId, dest));
                                if (dest == "EVENTS")
                                {
                                    EMSAdapter.PublishMessageToEventsQueue(dest, items.Source, DocumentObjectId, items.TagRef, items.ProcessName, items.ProcessType, items.ServiceFileProcessID, items.OrderSourceId, items.SecondOrderSourceID, items.RegionID, items.ObjectCD);
                                    sLogger.Info(string.Format("Publishing resubmit message DocumentObjectId: {0}  SUCCESSFUL", DocumentObjectId));
                                    isResubmitSuccess = true;
                                }
                                else if (EMSAdapter.PublishMessage(dest, items.Source, messageLog.ServiceRequestId, messageLog.MessageLogId, DocumentObjectId.ToString(), null, null, items.TagRef, items.InternalRefNum))
                                {
                                    sLogger.Info(string.Format("PublishingMessage resubmit message DocumentObjectId: {0} SUCCESSFUL", DocumentObjectId));
                                    isResubmitSuccess = true;
                                }
                                else
                                {
                                    sLogger.Error(string.Format("PublishingMessage resubmit message DocumentObjectId: {0} FAILED", DocumentObjectId.ToString()));
                                }
                            }
                        }
                        else
                        {
                            sLogger.Info("MessageLogId missing for Exception");
                            if (items != null)
                            {
                                string dest = items.Destination?.Replace(DataContracts.Constants.APPLICATION_LVIS, "").Replace("QUEUE", "").Replace(".", "") ?? DataContracts.Constants.ENRICHMENT;
                                sLogger.Debug(string.Format("Publishing resubmit message DocumentObjectId: {0} to Destination: {1}", DocumentObjectId, dest));
                                if (dest == "EVENTS")
                                {
                                    EMSAdapter.PublishMessageToEventsQueue(dest, items.Source, DocumentObjectId, items.TagRef, items.ProcessName, items.ProcessType, items.ServiceFileProcessID, items.OrderSourceId, items.SecondOrderSourceID, items.RegionID, items.ObjectCD);
                                    sLogger.Info(string.Format("Publishing resubmit message DocumentObjectId: {0}  SUCCESSFUL", DocumentObjectId));
                                    isResubmitSuccess = true;
                                }
                                else if (EMSAdapter.PublishMessage(dest, items.Source, DocumentObjectId, ((items.InternalRefNum != null) ? items.InternalRefNum : ""), items.TagRef))
                                {
                                    sLogger.Info(string.Format("Publishing resubmit message DocumentObjectId: {0}  SUCCESSFUL", DocumentObjectId));
                                    isResubmitSuccess = true;
                                }
                                else
                                {
                                    sLogger.Error(string.Format("Publishing resubmit message DocumentObjectId: {0} FAILED", DocumentObjectId.ToString()));
                                }
                            }
                        }

                        if (isResubmitSuccess)
                        {
                            scuccessfullysubmittedcnt += 1;
                            ExceptionInfo.TypeCodeId = (int)ExceptionStatusEnum.Resolved;
                            ExceptionInfo.LastModifiedById = userId;
                            ExceptionInfo.LastModifiedDate = DateTime.Now;
                            ExceptionInfo.ExceptionNotes.Add(
                                new ExceptionNote()
                                {
                                    CreatedById = userId,
                                    ExceptionId = item.Exceptionid,
                                    CreatedDate = DateTime.Now,
                                    ExceptionNotes = "Resubmitted, exception status changed from " + item.Status.Name + " to " + ExceptionStatusEnum.Resolved.ToString()
                                });

                            dbContext.Entry(ExceptionInfo).State = System.Data.Entity.EntityState.Modified;
                            AuditLogHelper.SaveChanges(dbContext);

                            item.Comments = dbContext.ExceptionNotes.Where(se => se.ExceptionId == item.Exceptionid).OrderByDescending(sl => sl.CreatedDate)
                        .Select(ENote => ENote.CreatedDate + " " + dbContext.Tower_Users.Where(fi => fi.UserId == ENote.CreatedById).Select(sl => sl.UserName).FirstOrDefault() + " " + ENote.ExceptionNotes).ToList();
                            item.LastModifiedDate = ExceptionInfo.LastModifiedDate.ToString();
                            item.Status = new ExceptionStatus { ID = (int)ExceptionStatusEnum.Resolved, Name = ExceptionStatusEnum.Resolved.ToString() };
                            item.LastModifiedBy = dbContext.Tower_Users.Where(fi => fi.UserId == userId).Select(sl => sl.UserName).FirstOrDefault();
                        }
                        else
                        {
                            unscuccessfullysubmittedcnt += 1;
                            ExceptionInfo.TypeCodeId = (int)ExceptionStatusEnum.Active;
                            ExceptionInfo.LastModifiedById = userId;
                            ExceptionInfo.LastModifiedDate = DateTime.Now;
                            ExceptionInfo.ExceptionNotes.Add(
                                new ExceptionNote()
                                {
                                    CreatedById = userId,
                                    ExceptionId = item.Exceptionid,
                                    CreatedDate = DateTime.Now,
                                    ExceptionNotes = "There was an error resubmitting/publishing message to EMS"
                                });
                            dbContext.Entry(ExceptionInfo).State = System.Data.Entity.EntityState.Modified;
                            AuditLogHelper.SaveChanges(dbContext);

                            item.Comments = dbContext.ExceptionNotes.Where(se => se.ExceptionId == item.Exceptionid).OrderByDescending(sl => sl.CreatedDate)
                        .Select(ENote => ENote.CreatedDate + " " + dbContext.Tower_Users.Where(fi => fi.UserId == ENote.CreatedById).Select(sl => sl.UserName).FirstOrDefault() + " " + ENote.ExceptionNotes).ToList();
                            item.LastModifiedDate = ExceptionInfo.LastModifiedDate.ToString();
                            item.Status = new ExceptionStatus { ID = (int)ExceptionStatusEnum.Active, Name = ExceptionStatusEnum.Active.ToString() };
                            item.LastModifiedBy = dbContext.Tower_Users.Where(fi => fi.UserId == userId).Select(sl => sl.UserName).FirstOrDefault();
                        }
                        resubmitExceptionDTO.SuccessResubmitCount = scuccessfullysubmittedcnt;
                        resubmitExceptionDTO.UnSuccessResubmitCount = unscuccessfullysubmittedcnt;
                        resubmitExceptionDTO.TotalResubmitCount = ExceptionIds.Count;
                        ReturnExceptionDetails.Add(item);
                    }

                }


            }
            return new Tuple<ResubmitBulkExceptionDTO, List<ExceptionDTO>>(resubmitExceptionDTO, ReturnExceptionDetails);
        }

        private static ExceptionDTO UpdateExceptionInfo(ExceptionDTO ExceptionDetails, int userId, TerminalDBEntities.Exception ExceptionInfo)
        {
            using (Entities dbContext = new Entities())
            {

            }

            return ExceptionDetails;
        }

        public IEnumerable<ExceptionDTO> GetTEQExceptionByReferenceNum(SearchDetail value, int tenantId)
        {
            Entities dbContext = new Entities();
            List<ExceptionDTO> ExceptionDetails = new List<ExceptionDTO>();
            if (value.ReferenceNoType == "1" && !string.IsNullOrEmpty(value.ReferenceNo))
            {
                //var Excepdet = dbContext.ServiceRequests.Where(sel => sel.TenantId == tenantId && sel.ExternalRefNum.Contains(value.ReferenceNo)).ToList();

                var Details = (from sr in dbContext.ServiceRequests
                               join ml in dbContext.MessageLogs on sr.ServiceRequestId equals ml.ServiceRequestId
                               join e in dbContext.Exceptions on ml.MessageLogId equals e.MessageLogId
                               where sr.ExternalRefNum.Contains(value.ReferenceNo)
                               && e.ExceptionType.ExceptionGroupId == (int)ExceptionGroupEnum.TEQ
                                 && (sr.TenantId == tenantId || tenantId == (int)TenantIdEnum.LVIS)
                               select new
                               {
                                   MessageLogId = ml.MessageLogId,
                                   MessageLog = dbContext.MessageLogs.Where(ml => ml.MessageLogId == e.MessageLogId).FirstOrDefault(),
                                   ExceptionId = e.ExceptionId,
                                   ApplicationId = sr.ApplicationId,
                                   CustomerRefNum = sr.CustomerRefNum,
                                   ExternalRefNum = sr.ExternalRefNum,
                                   CreatedBy = e.CreatedById,
                                   CreatedDate = e.CreatedDate.ToString(),
                                   LastModifiedBy = e.LastModifiedById,
                                   LastModifiedDate = e.LastModifiedDate.ToString(),
                                   InternalRefId = sr.InternalRefId,
                                   InternalRefNum = sr.InternalRefNum,
                                   LocationId = sr.LocationId,
                                   ProviderId = sr.ProviderId,
                                   ServiceRequestId = sr.ServiceRequestId,
                                   ServiceId = sr.ServiceId,
                                   Comments = e.Comments,
                                   documentobjectid = (ml.DocumentObjectId > 0) ? ml.DocumentObjectId : e.DocumentObjectId,
                                   e
                               }).ToList();

                ExceptionDetails = Details.Select(se => new ExceptionDTO()
                {
                    Exceptionid = se.ExceptionId,
                    ExceptionTypeid = se.e.ExceptionTypeId,
                    //ExceptionType = dbContext.ExceptionTypes.Where(x => x.ExceptionTypeId == se.e.ExceptionTypeId).Select(x => x.ExceptionTypeName)?.FirstOrDefault(),
                    ExceptionType = se.e.ExceptionType.ExceptionTypeName,
                    ExceptionDesc = se.e.ExceptionDesc,
                    CreatedBy = se.e.CreatedById.ToString(),
                    CreatedDate = se.e.CreatedDate.ToString(),
                    LastModifiedBy = dbContext.Tower_Users.Where(fi => fi.UserId == se.e.LastModifiedById).Select(sl => sl.UserName).FirstOrDefault(),
                    LastModifiedDate = se.e.LastModifiedDate.ToString(),
                    Comments = se.e.ExceptionNotes.Count == 0 ? new List<string>() : se.e.ExceptionNotes.OrderByDescending(sl => sl.CreatedDate)
                     .Select(ENote => ENote.CreatedDate + " " + dbContext.Tower_Users.Where(fi => fi.UserId == ENote.CreatedById).Select(sl => sl.UserName).FirstOrDefault() + " " + ENote.ExceptionNotes).ToList(),
                    Status = new ExceptionStatus() { ID = se.e.TypeCodeId, Name = dbContext.TypeCodes.Where(x => x.TypeCodeId == se.e.TypeCodeId).Select(x => x.TypeCodeDesc).FirstOrDefault() },
                    DocumentObjectid = (se.documentobjectid > 0) ? se.documentobjectid : se.e.DocumentObjectId,
                    ExternalRefNum = se.ExternalRefNum,
                    Reporting = new ReportingDTO()
                    {
                        InternalRefNum = se.InternalRefNum,
                        InternalRefId = se.InternalRefId.ToString(),
                    },
                    MessageType = se.MessageLog != null ? se.MessageLog.MessageMap.MessageType.MessageTypeName : string.Empty,
                    TypeCodeId = se.e.TypeCodeId,
                    ServiceType = se.MessageLog != null ? se.MessageLog.ServiceRequest.Service.ServiceName : string.Empty,
                    ServiceRequestId = se.MessageLog != null ? se.MessageLog.ServiceRequestId : 0
                }).ToList();
            }

            else if (value.ReferenceNoType == "2" && !string.IsNullOrEmpty(value.ReferenceNo))
            {
                //var Excepdet = dbContext.ServiceRequests.Where(sel => sel.TenantId == tenantId && sel.InternalRefNum.Contains(value.ReferenceNo)).ToList();

                var Details = (from sr in dbContext.ServiceRequests
                               join ml in dbContext.MessageLogs on sr.ServiceRequestId equals ml.ServiceRequestId
                               join e in dbContext.Exceptions on ml.MessageLogId equals e.MessageLogId
                               where sr.InternalRefNum.Contains(value.ReferenceNo)
                               && e.ExceptionType.ExceptionGroupId == (int)ExceptionGroupEnum.TEQ
                                 && (sr.TenantId == tenantId || tenantId == (int)TenantIdEnum.LVIS)
                               select new
                               {
                                   MessageLogId = ml.MessageLogId,
                                   MessageLog = dbContext.MessageLogs.Where(ml => ml.MessageLogId == e.MessageLogId).FirstOrDefault(),
                                   ExceptionId = e.ExceptionId,
                                   ApplicationId = sr.ApplicationId,
                                   CustomerRefNum = sr.CustomerRefNum,
                                   ExternalRefNum = sr.ExternalRefNum,
                                   CreatedBy = e.CreatedById,
                                   CreatedDate = e.CreatedDate.ToString(),
                                   LastModifiedBy = e.LastModifiedById,
                                   LastModifiedDate = e.LastModifiedDate.ToString(),
                                   InternalRefId = sr.InternalRefId,
                                   InternalRefNum = sr.InternalRefNum,
                                   LocationId = sr.LocationId,
                                   ProviderId = sr.ProviderId,
                                   ServiceRequestId = sr.ServiceRequestId,
                                   ServiceId = sr.ServiceId,
                                   Comments = e.Comments,
                                   documentobjectid = (ml.DocumentObjectId > 0) ? ml.DocumentObjectId : e.DocumentObjectId,
                                   e
                               }).ToList();

                ExceptionDetails = Details.Select(se => new ExceptionDTO()
                {
                    Exceptionid = se.ExceptionId,
                    //ExceptionType = dbContext.ExceptionTypes.Where(x => x.ExceptionTypeId == se.e.ExceptionTypeId).Select(x => x.ExceptionTypeName)?.FirstOrDefault(),
                    ExceptionType = se.e.ExceptionType.ExceptionTypeName,
                    ExceptionDesc = se.e.ExceptionDesc,
                    CreatedBy = se.e.CreatedById.ToString(),
                    CreatedDate = se.e.CreatedDate.ToString(),
                    LastModifiedBy = dbContext.Tower_Users.Where(fi => fi.UserId == se.e.LastModifiedById).Select(sl => sl.UserName).FirstOrDefault(),
                    LastModifiedDate = se.e.LastModifiedDate.ToString(),
                    Comments = se.e.ExceptionNotes.Count == 0 ? new List<string>() : se.e.ExceptionNotes.OrderByDescending(sl => sl.CreatedDate)
                     .Select(ENote => ENote.CreatedDate + " " + dbContext.Tower_Users.Where(fi => fi.UserId == ENote.CreatedById).Select(sl => sl.UserName).FirstOrDefault() + " " + ENote.ExceptionNotes).ToList(),
                    Status = new ExceptionStatus() { ID = se.e.TypeCodeId, Name = dbContext.TypeCodes.Where(x => x.TypeCodeId == se.e.TypeCodeId).Select(x => x.TypeCodeDesc).FirstOrDefault() },
                    DocumentObjectid = (se.documentobjectid > 0) ? se.documentobjectid : se.e.DocumentObjectId,
                    ExternalRefNum = se.ExternalRefNum,
                    Reporting = new ReportingDTO()
                    {
                        InternalRefNum = se.InternalRefNum,
                        InternalRefId = se.InternalRefId.ToString(),
                    },
                    MessageType = se.MessageLog != null ? se.MessageLog.MessageMap.MessageType.MessageTypeName : string.Empty,
                    TypeCodeId = se.e.TypeCodeId,
                    ServiceType = se.MessageLog != null ? se.MessageLog.ServiceRequest.Service.ServiceName : string.Empty,
                    ServiceRequestId = se.MessageLog != null ? se.MessageLog.ServiceRequestId : 0
                }).ToList();
            }
            else if (value.ReferenceNoType == "3" && !string.IsNullOrEmpty(value.ReferenceNo))
            {
                var Details = (from sr in dbContext.ServiceRequests
                               join ml in dbContext.MessageLogs on sr.ServiceRequestId equals ml.ServiceRequestId
                               join e in dbContext.Exceptions on ml.MessageLogId equals e.MessageLogId
                               where sr.CustomerRefNum.Contains(value.ReferenceNo)
                               && e.ExceptionType.ExceptionGroupId == (int)ExceptionGroupEnum.TEQ
                                 && (sr.TenantId == tenantId || tenantId == (int)TenantIdEnum.LVIS)
                               select new
                               {
                                   MessageLogId = ml.MessageLogId,
                                   MessageLog = dbContext.MessageLogs.Where(ml => ml.MessageLogId == e.MessageLogId).FirstOrDefault(),
                                   ExceptionId = e.ExceptionId,
                                   ApplicationId = sr.ApplicationId,
                                   CustomerRefNum = sr.CustomerRefNum,
                                   ExternalRefNum = sr.ExternalRefNum,
                                   CreatedBy = e.CreatedById,
                                   CreatedDate = e.CreatedDate.ToString(),
                                   LastModifiedBy = e.LastModifiedById,
                                   LastModifiedDate = e.LastModifiedDate.ToString(),
                                   InternalRefId = sr.InternalRefId,
                                   InternalRefNum = sr.InternalRefNum,
                                   LocationId = sr.LocationId,
                                   ProviderId = sr.ProviderId,
                                   ServiceRequestId = sr.ServiceRequestId,
                                   ServiceId = sr.ServiceId,
                                   Comments = e.Comments,
                                   documentobjectid = (ml.DocumentObjectId > 0) ? ml.DocumentObjectId : e.DocumentObjectId,
                                   e
                               }).ToList();

                ExceptionDetails = Details.Select(se => new ExceptionDTO()
                {
                    Exceptionid = se.ExceptionId,
                    ExceptionType = se.e.ExceptionType.ExceptionTypeName,
                    ExceptionDesc = se.e.ExceptionDesc,
                    CreatedBy = se.e.CreatedById == 0 ? string.Empty : dbContext.Tower_Users.Where(fi => fi.UserId == se.e.CreatedById).Select(sl => sl.UserName).FirstOrDefault(),
                    CreatedDate = se.e.CreatedDate.ToString(),
                    LastModifiedBy = se.e.LastModifiedById == 0 ? string.Empty : dbContext.Tower_Users.Where(fi => fi.UserId == se.e.LastModifiedById).Select(sl => sl.UserName).FirstOrDefault(),
                    LastModifiedDate = se.e.LastModifiedDate.ToString(),
                    Status = new ExceptionStatus() { ID = se.e.TypeCodeId, Name = dbContext.TypeCodes.Where(x => x.TypeCodeId == se.e.TypeCodeId).Select(x => x.TypeCodeDesc).FirstOrDefault() },
                    DocumentObjectid = (se.documentobjectid > 0) ? se.documentobjectid : se.e.DocumentObjectId,
                    ExternalRefNum = se.ExternalRefNum,
                    Reporting = new ReportingDTO()
                    {
                        InternalRefNum = se.InternalRefNum,
                        InternalRefId = se.InternalRefId.ToString(),
                    },
                    MessageType = se.MessageLog != null ? se.MessageLog.MessageMap.MessageType.MessageTypeName : string.Empty,
                    TypeCodeId = se.e.TypeCodeId,
                    ServiceType = se.MessageLog != null ? se.MessageLog.ServiceRequest.Service.ServiceName : string.Empty,
                    ServiceRequestId = se.MessageLog != null ? se.MessageLog.ServiceRequestId : 0
                }).ToList();
            }

            else if (value.ReferenceNoType == "4" && !string.IsNullOrEmpty(value.ReferenceNo))
            {
                var Details = (from sr in dbContext.ServiceRequests
                               join ml in dbContext.MessageLogs on sr.ServiceRequestId equals ml.ServiceRequestId
                               join e in dbContext.Exceptions on ml.MessageLogId equals e.MessageLogId
                               where sr.InternalRefId.ToString().Contains(value.ReferenceNo)
                               && e.ExceptionType.ExceptionGroupId == (int)ExceptionGroupEnum.TEQ
                                 && (sr.TenantId == tenantId || tenantId == (int)TenantIdEnum.LVIS)
                               select new
                               {
                                   MessageLogId = ml.MessageLogId,
                                   MessageLog = dbContext.MessageLogs.Where(ml => ml.MessageLogId == e.MessageLogId).FirstOrDefault(),
                                   ExceptionId = e.ExceptionId,
                                   ApplicationId = sr.ApplicationId,
                                   CustomerRefNum = sr.CustomerRefNum,
                                   ExternalRefNum = sr.ExternalRefNum,
                                   CreatedBy = e.CreatedById,
                                   CreatedDate = e.CreatedDate.ToString(),
                                   LastModifiedBy = e.LastModifiedById,
                                   LastModifiedDate = e.LastModifiedDate.ToString(),
                                   InternalRefId = sr.InternalRefId,
                                   InternalRefNum = sr.InternalRefNum,
                                   LocationId = sr.LocationId,
                                   ProviderId = sr.ProviderId,
                                   ServiceRequestId = sr.ServiceRequestId,
                                   ServiceId = sr.ServiceId,
                                   Comments = e.Comments,
                                   documentobjectid = (ml.DocumentObjectId > 0) ? ml.DocumentObjectId : e.DocumentObjectId,
                                   e
                               }).ToList();

                ExceptionDetails = Details.Select(se => new ExceptionDTO()
                {
                    Exceptionid = se.ExceptionId,
                    ExceptionType = se.e.ExceptionType.ExceptionTypeName,
                    ExceptionDesc = se.e.ExceptionDesc,
                    CreatedBy = se.e.CreatedById == 0 ? string.Empty : dbContext.Tower_Users.Where(fi => fi.UserId == se.e.CreatedById).Select(sl => sl.UserName).FirstOrDefault(),
                    CreatedDate = se.e.CreatedDate.ToString(),
                    LastModifiedBy = se.e.LastModifiedById == 0 ? string.Empty : dbContext.Tower_Users.Where(fi => fi.UserId == se.e.LastModifiedById).Select(sl => sl.UserName).FirstOrDefault(),
                    LastModifiedDate = se.e.LastModifiedDate.ToString(),
                    Status = new ExceptionStatus() { ID = se.e.TypeCodeId, Name = dbContext.TypeCodes.Where(x => x.TypeCodeId == se.e.TypeCodeId).Select(x => x.TypeCodeDesc).FirstOrDefault() },
                    DocumentObjectid = (se.documentobjectid > 0) ? se.documentobjectid : se.e.DocumentObjectId,
                    ExternalRefNum = se.ExternalRefNum,
                    Reporting = new ReportingDTO()
                    {
                        InternalRefNum = se.InternalRefNum,
                        InternalRefId = se.InternalRefId.ToString(),
                    },
                    MessageType = se.MessageLog != null ? se.MessageLog.MessageMap.MessageType.MessageTypeName : string.Empty,
                    TypeCodeId = se.e.TypeCodeId,
                    ServiceType = se.MessageLog != null ? se.MessageLog.ServiceRequest.Service.ServiceName : string.Empty,
                    ServiceRequestId = se.MessageLog != null ? se.MessageLog.ServiceRequestId : 0
                }).ToList();
            }

            return ExceptionDetails;
        }

        public bool TEQRejectOrder(ExceptionDTO matchException, int userId, string fileNotes)
        {
            using (Entities dbContext = new Entities())
            {
                TerminalDBEntities.Exception ExceptionInfo = dbContext.Exceptions.Where(se => se.ExceptionId == matchException.Exceptionid).FirstOrDefault();

                TerminalDocumentEntities DbDocumentcontext = new TerminalDocumentEntities();
                var content = DbDocumentcontext.DocumentObjects.Where(se => se.DocumentObjectId == ExceptionInfo.DocumentObjectId).FirstOrDefault();

                string Dest = "";

                if (content != null)
                {
                    string Content = "";
                    Content = content.Object;
                    if (string.IsNullOrEmpty(Content) && !string.IsNullOrEmpty(content.ObjectPath)
                         && File.Exists(content.ObjectPath))
                    {
                        Content = File.ReadAllText(content.ObjectPath);
                    }
                    var items = (from t in XDocument.Parse(Content)?.Descendants(DataContracts.Constants.EXCEPTION)
                                 select new
                                 {
                                     Dest = t.Element("Source")?.Value,
                                 }).FirstOrDefault();

                    Dest = items.Dest;
                }

                MessageLog messageLog;

                bool isResubmitSuccess = false;
                if (ExceptionInfo.MessageLogId > 0)
                {
                    sLogger.Info(string.Format("MessageLogId: {0} found", ExceptionInfo.MessageLogId));
                    MessageLog MessageLogInfo = dbContext.MessageLogs.Where(se => se.MessageLogId == ExceptionInfo.MessageLogId).FirstOrDefault();

                    messageLog = new MessageLog()
                    {
                        CreatedById = userId,
                        LastModifiedById = userId,
                        CreatedDate = DateTime.Now,
                        LastModifiedDate = DateTime.Now,
                        MessageMapId = dbContext.MessageMaps.Where(se => se.IsInbound == false && se.MessageTypeId == 10022
                                            && se.TenantId == MessageLogInfo.TenantId).FirstOrDefault() != null ? dbContext.MessageMaps.Where(se =>
                                             se.IsInbound == false && se.MessageTypeId == 10022
                                            && se.TenantId == MessageLogInfo.TenantId
                                            ).FirstOrDefault().MessageMapId : 0,
                        TenantId = MessageLogInfo.TenantId,
                        ServiceRequestId = MessageLogInfo.ServiceRequestId,
                        DocumentObjectId = matchException.DocumentObjectid,
                        ParentMessageLogId = MessageLogInfo.ParentMessageLogId,
                        MessageLogDesc = fileNotes,
                        RestartStep = null
                    };

                    dbContext.MessageLogs.Add(messageLog);
                    dbContext.SaveChanges();

                    sLogger.Info(string.Format("Publishing Reject message DocumentObjectId: {0}", matchException.DocumentObjectid.ToString()));

                    if (EMSAdapter.PublishMessage(Dest.ToUpper(), "LVIS", MessageLogInfo.ServiceRequestId, messageLog.MessageLogId, matchException.DocumentObjectid.ToString()))
                    {
                        sLogger.Info(string.Format("Publishing Reject message DocumentObjectId: {0}  SUCCESSFUL", matchException.DocumentObjectid.ToString()));
                        isResubmitSuccess = true;
                    }
                    else
                    {
                        sLogger.Error(string.Format("Publishing Reject message DocumentObjectId: {0} FAILED", GetMessageContentDocumentObject(matchException.DocumentObjectid).ToString()));
                    }
                }

                if (isResubmitSuccess)
                {
                    string Status = ExceptionInfo.TypeCode.TypeCodeDesc;
                    ExceptionInfo.TypeCodeId = (int)ExceptionStatusEnum.Resolved;
                    ExceptionInfo.LastModifiedById = userId;
                    ExceptionInfo.LastModifiedDate = DateTime.Now;
                    dbContext.Entry(ExceptionInfo).State = System.Data.Entity.EntityState.Modified;
                    AuditLogHelper.SaveChanges(dbContext);

                    TerminalDBEntities.ExceptionNote EceptionComment = new TerminalDBEntities.ExceptionNote();
                    EceptionComment.ExceptionId = matchException.Exceptionid;
                    EceptionComment.ExceptionNotes = "Resubmitted, exception status changed from " + Status + " to " + ExceptionStatusEnum.Resolved.ToString();
                    EceptionComment.CreatedById = userId;
                    EceptionComment.CreatedDate = DateTime.Now;

                    dbContext.ExceptionNotes.Add(EceptionComment);
                    AuditLogHelper.SaveChanges(dbContext);

                    return true;
                }
                else
                {
                    ExceptionInfo.TypeCodeId = (int)ExceptionStatusEnum.Active;
                    ExceptionInfo.LastModifiedById = userId;
                    ExceptionInfo.LastModifiedDate = DateTime.Now;
                    dbContext.Entry(ExceptionInfo).State = System.Data.Entity.EntityState.Modified;
                    AuditLogHelper.SaveChanges(dbContext);


                    TerminalDBEntities.ExceptionNote EceptionComment = new TerminalDBEntities.ExceptionNote();
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


        public bool BEQCreateOrder(ExceptionDTO matchException, int userId, int tenantid, string FileNote)
        {
            bool Status = true;

            if (matchException.Exceptionid == 0 && matchException.children != null && matchException.children.Count() > 0)
            {
                foreach (var child in matchException.children.Where(se => !se.InvolveResolved))
                {
                    var dest = DataContracts.Constants.CONVOY;
                    Status = BEQCreateOrderMatch(FileNote, child, userId, tenantid, dest, child.ParentExternalRefNum);
                }
            }
            else
                Status = BEQCreateOrderMatch(FileNote, matchException, userId, tenantid);

            return Status;
        }

        private bool BEQCreateOrderMatch(string fileNotes, ExceptionDTO bindMatch, int userId, int tenantid, string dest = null, string ExternalRefnum = null)
        {
            using (Entities dbContext = new Entities())
            {
                TerminalDBEntities.Exception ExceptionInfo = dbContext.Exceptions.Where(se => se.ExceptionId == bindMatch.Exceptionid).FirstOrDefault();
                TerminalDocumentEntities DbDocumentcontext = new TerminalDocumentEntities();
                string content = "";
                DocumentObject Docobject = DbDocumentcontext.DocumentObjects.Where(se => se.DocumentObjectId == ExceptionInfo.DocumentObjectId).FirstOrDefault();

                if (Docobject != null)
                {
                    content = Docobject.Object;
                    if (string.IsNullOrEmpty(content) && !string.IsNullOrEmpty(Docobject.ObjectPath)
                         && File.Exists(Docobject.ObjectPath))
                    {
                        content = File.ReadAllText(Docobject.ObjectPath);
                    }
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
                                 TrackingId = t.Element("TrackingId")?.Value
                             }).FirstOrDefault();

                SetTrackingId(items?.TrackingId); 

                string docObjContent = "";
                DocumentObject Docobjectitems = DbDocumentcontext.DocumentObjects.Where(se => se.DocumentObjectId == items.DocumentObjectId).FirstOrDefault();
                if (Docobjectitems != null)
                {
                    docObjContent = Docobjectitems.Object;
                    if (string.IsNullOrEmpty(docObjContent)
                        && !string.IsNullOrEmpty(Docobjectitems.ObjectPath)
                         && File.Exists(Docobjectitems.ObjectPath))
                    {
                        docObjContent = File.ReadAllText(Docobjectitems.ObjectPath);
                    }
                }

                //XmlSerializer mySerializer = new XmlSerializer(typeof(Order));
                //Order order = (Order)mySerializer.Deserialize(new StringReader(docObjContent));

                bool isElite = false;
                if (!string.IsNullOrEmpty(items?.ServiceReqId))
                {
                    int serviceReqId = int.Parse(items.ServiceReqId);
                    var sr = dbContext.ServiceRequests.Where(x => x.ServiceRequestId == serviceReqId).FirstOrDefault();
                    if (sr?.ApplicationId == (int)ApplicationEnum.Elite)
                        isElite = true;
                }

                Order order = Utils.DeSerializeToObject<Order>(docObjContent);
                if (!isElite)
                    order.InternalRefNum = "";
                order.InternalRefId = 0;
                order.OrderComments = AddAppendFilesNotes(order.OrderComments, fileNotes);

                string canonicalContent = Utils.SerializeToString<Order>(order);
                DocumentObject Canonical = new DocumentObject();
                Canonical.CreatedById = userId;
                Canonical.CreatedDate = DateTime.Now;
                Canonical.DocumentObjectFormat = "XML";
                Canonical.Object = canonicalContent;
                DbDocumentcontext.DocumentObjects.Add(Canonical);
                AuditLogHelper.SaveChanges(DbDocumentcontext);

                MessageLog messageLog;

                bool isResubmitSuccess = false;
                if (ExceptionInfo.MessageLogId > 0)
                {
                    sLogger.Debug(string.Format("MessageLogId: {0} found", ExceptionInfo.MessageLogId));
                    MessageLog MessageLogInfo = dbContext.MessageLogs.Where(se => se.MessageLogId == ExceptionInfo.MessageLogId).FirstOrDefault();

                    messageLog = new MessageLog()
                    {
                        CreatedById = userId,
                        LastModifiedById = userId,
                        CreatedDate = DateTime.Now,
                        LastModifiedDate = DateTime.Now,
                        MessageMapId = dbContext.MessageMaps.Where(se => se.MessageTypeId == 10023 && se.TenantId == MessageLogInfo.TenantId).FirstOrDefault() != null ? dbContext.MessageMaps.Where(se => se.MessageTypeId == 10023 && se.TenantId == MessageLogInfo.TenantId).FirstOrDefault().MessageMapId : 0,
                        TenantId = MessageLogInfo.TenantId,
                        ServiceRequestId = MessageLogInfo.ServiceRequestId,
                        DocumentObjectId = Canonical.DocumentObjectId,
                        ParentMessageLogId = MessageLogInfo.ParentMessageLogId,
                        MessageLogDesc = fileNotes,
                        RestartStep = null
                    };

                    dbContext.MessageLogs.Add(messageLog);
                    dbContext.SaveChanges();

                    if (string.IsNullOrEmpty(dest))
                        dest = items.Destination?.Replace(DataContracts.Constants.APPLICATION_LVIS, "").Replace("QUEUE", "").Replace(".", "") ?? DataContracts.Constants.APPLICATION_FAST;

                    sLogger.Debug(string.Format("Publishing Create message DocumentObjectId: {0} to Destination: {1}", Canonical.DocumentObjectId, dest));

                    if (EMSAdapter.PublishMessage(dest.ToUpper(), items.Source.ToUpper(), MessageLogInfo.ServiceRequestId, messageLog.MessageLogId, Canonical.DocumentObjectId.ToString(), ExternalRefnum, DateTime.Now))
                    {
                        sLogger.Info(string.Format("Publishing Create message DocumentObjectId: {0}  SUCCESSFUL", Canonical.DocumentObjectId));
                        isResubmitSuccess = true;
                    }
                    else
                    {
                        sLogger.Error(string.Format("Publishing Create message DocumentObjectId: {0} FAILED", Canonical.DocumentObjectId.ToString()));
                    }
                }

                if (isResubmitSuccess)
                {
                    string Status = ExceptionInfo.TypeCode.TypeCodeDesc;
                    ExceptionInfo.TypeCodeId = (int)ExceptionStatusEnum.Resolved;
                    ExceptionInfo.LastModifiedById = userId;
                    ExceptionInfo.LastModifiedDate = DateTime.Now;
                    dbContext.Entry(ExceptionInfo).State = System.Data.Entity.EntityState.Modified;
                    AuditLogHelper.SaveChanges(dbContext);

                    TerminalDBEntities.ExceptionNote EceptionComment = new TerminalDBEntities.ExceptionNote();
                    EceptionComment.ExceptionId = bindMatch.Exceptionid;
                    EceptionComment.ExceptionNotes = "Create BEQ Order completed, exception status changed from " + Status + " to " + ExceptionStatusEnum.Resolved.ToString();
                    EceptionComment.CreatedById = userId;
                    EceptionComment.CreatedDate = DateTime.Now;

                    dbContext.ExceptionNotes.Add(EceptionComment);
                    AuditLogHelper.SaveChanges(dbContext);

                    return true;
                }
                else
                {
                    ExceptionInfo.TypeCodeId = (int)ExceptionStatusEnum.Active;
                    ExceptionInfo.LastModifiedById = userId;
                    ExceptionInfo.LastModifiedDate = DateTime.Now;
                    dbContext.Entry(ExceptionInfo).State = System.Data.Entity.EntityState.Modified;
                    AuditLogHelper.SaveChanges(dbContext);

                    TerminalDBEntities.ExceptionNote EceptionComment = new TerminalDBEntities.ExceptionNote();
                    EceptionComment.ExceptionId = bindMatch.Exceptionid;
                    EceptionComment.ExceptionNotes = "There was an error resubmitting/publishing message to EMS";
                    EceptionComment.CreatedById = userId;
                    EceptionComment.CreatedDate = DateTime.Now;

                    dbContext.ExceptionNotes.Add(EceptionComment);
                    AuditLogHelper.SaveChanges(dbContext);
                    return false;
                }
            }
        }


        public List<string> GetExceptionComments(ExceptionDTO matchException)
        {
            List<string> sComments = new List<string>();
            List<ExceptionNote> exNote = new List<ExceptionNote>();
            Entities dbContext1 = new Entities();

            if (matchException.Exceptionid == 0)
            {
                foreach (var item in matchException.children)
                {
                    using (Entities dbContext = new Entities())
                    {
                        exNote.AddRange(dbContext.ExceptionNotes.Where(se => se.ExceptionId == item.Exceptionid));
                    }
                }

                exNote = exNote.OrderBy(sl => sl.CreatedDate).ToList();

                if (exNote.Count() > 0)
                {
                    for (var x = 0; x < exNote.Count(); x++)
                    {
                        for (var y = 0; y < matchException.children.Count(); y++)
                        {
                            if (exNote[x].ExceptionId == matchException.children[y].Exceptionid)
                            {
                                var name = dbContext1.Tower_Users.AsEnumerable().Where(fi => fi.UserId == exNote[x].CreatedById).Select(sl => sl.UserName).FirstOrDefault();

                                sComments.Add(string.IsNullOrEmpty(matchException.children[y].ServiceType) ?
                                                     (exNote[x].CreatedDate + " " + name + " " + exNote[x].ExceptionNotes).ToString() :
                                                     (matchException.children[y].ServiceType + ": " + exNote[x].CreatedDate.ToString() + " " + name + " " + exNote[x].ExceptionNotes.ToString()));
                            }
                        }
                    }
                }

            }
            else
                sComments.AddRange(GetBEQExceptionNotes(matchException.Exceptionid, matchException.ServiceType));

            return sComments;

        }

        public IEnumerable<ExceptionDTO> GetTEQExceptionsbyTypeName(int tenantId, SearchDetail value, string exceptionTypeName, string sFilter, bool includeResolved)
        {
            DateTime startDateTime = DateTime.Today;
            DateTime endDateTime = DateTime.Today;
            bool isIncludeResolved = false;

            switch (sFilter)
            {
                case "0": //search 'Today'
                    endDateTime = endDateTime.Subtract(startDateTime.TimeOfDay).AddDays(1).AddMilliseconds(-1);
                    isIncludeResolved = value.Typecodestatus;
                    break;
                case "1": //search 'Custom'
                    startDateTime = Convert.ToDateTime(value.Fromdate);
                    endDateTime = Convert.ToDateTime(value.ThroughDate);
                    endDateTime = endDateTime.Subtract(startDateTime.TimeOfDay).AddDays(1).AddMilliseconds(-1);
                    isIncludeResolved = value.Typecodestatus;
                    break;
                case "2": // search 'All'
                    startDateTime = startDateTime.Subtract(startDateTime.TimeOfDay).AddYears(-1); //To Pull data from Inception Date.
                    endDateTime = endDateTime.Subtract(startDateTime.TimeOfDay).AddDays(1).AddMilliseconds(-1);
                    break;
                case "19":
                        startDateTime = Convert.ToDateTime("1/01/2016 12:00:00 AM"); ;  //To Pull data from Inception Date.
                        endDateTime = endDateTime.Subtract(startDateTime.TimeOfDay).AddDays(1).AddMilliseconds(-1);
                    break;
                case "24": // search '24'
                    startDateTime = DateTime.Now;
                    startDateTime = startDateTime.AddDays(-1);
                    endDateTime = DateTime.Now;
                    break;
                default:
                    endDateTime = endDateTime.Subtract(startDateTime.TimeOfDay).AddDays(1).AddMilliseconds(-1);
                    startDateTime = startDateTime.AddDays(-(int.Parse(sFilter)));
                    isIncludeResolved = value.Typecodestatus;
                    break;
            }

            List<ExceptionDTO> det = new List<ExceptionDTO>();

            using (var dbContext = new Entities())
            {
                var result = dbContext.GetExceptionInfo(startDateTime, endDateTime, (int)ExceptionGroupEnum.TEQ, tenantId, includeResolved, exceptionTypeName).ToList();

                if (result != null)
                {
                    foreach (var se in result)
                    {
                        var ExceptionTypeID = dbContext.ExceptionTypes.Where(de => de.ExceptionTypeName == se.ExceptionTypeName).FirstOrDefault();
                        ExceptionDTO Detail = new ExceptionDTO();
                        Detail.Exceptionid = se.ExceptionId;
                        Detail.ExceptionTypeid = ExceptionTypeID != null ? ExceptionTypeID.ExceptionTypeId : 0;
                        Detail.ExceptionType = !string.IsNullOrEmpty(se.ExceptionTypeName) ? se.ExceptionTypeName : string.Empty;
                        Detail.ExceptionDesc = !string.IsNullOrEmpty(se.ExceptionDesc) ? se.ExceptionDesc : string.Empty;
                        Detail.CreatedBy = (string.IsNullOrEmpty(se.CreatedBy) && se.CreatedById == 0 ? string.Empty : se.CreatedBy);
                        Detail.CreatedDate = se.CreatedDate.ToString();
                        Detail.LastModifiedBy = (string.IsNullOrEmpty(se.LastModifiedBy) && se.LastModifiedById == 0 ? string.Empty : se.LastModifiedBy);
                        Detail.LastModifiedDate = se.LastModifiedDate.ToString();
                        Detail.Status = new ExceptionStatus() { ID = se.TypeCodeId, Name = se.ExceptionStatus };
                        if (!string.IsNullOrEmpty(se.DocObjectPath)
                             && File.Exists(se.DocObjectPath))
                        {
                            string content = File.ReadAllText(se.DocObjectPath);
                            var items = (from t in XDocument.Parse(content)?.Descendants(DataContracts.Constants.EXCEPTION)
                                         select new
                                         {
                                             DocumentObjectId = Convert.ToInt64(t.Element("DocumentObjectId")?.Value),
                                             ExternalRefNumber = t.Element("ExternalRefNum")?.Value,
                                         }).FirstOrDefault();
                            se.ExternalRefNum = items.ExternalRefNumber;
                            se.DocumentObjectId = items.DocumentObjectId;
                        }
                        Detail.DocumentObjectid = se.DocumentObjectId.HasValue ? se.DocumentObjectId.Value : 0;
                        Detail.ExternalRefNum = !string.IsNullOrEmpty(se.ExternalRefNum) ? se.ExternalRefNum : string.Empty;
                        Detail.MessageType = !string.IsNullOrEmpty(se.MessageTypeName) ? se.MessageTypeName : string.Empty;
                        Detail.MessageTypeid = se.MessageTypeId.HasValue ? se.MessageTypeId.Value : 0;
                        Detail.ServiceType = !string.IsNullOrEmpty(se.ServiceName) ? se.ServiceName : string.Empty;
                        Detail.ServiceRequestId = se.ServiceRequestId.HasValue ? se.ServiceRequestId.Value : 0;
                        Detail.TypeCodeId = se.TypeCodeId;
                        Detail.ParentExternalRefNum = string.Empty;
                        Detail.Tenant = !string.IsNullOrEmpty(se.TenantName) ? se.TenantName : string.Empty;
                        Detail.TenantId = dbContext.Tenants.Where(te => te.TenantName == Detail.Tenant).Select(sl => sl.TenantId).FirstOrDefault();

                        det.Add(Detail);
                    }
                }
            }

            return det;
        }
        private string AddAppendFilesNotes(string existringComments, string fileNotes)
        {
            return string.Join("\n", new List<string> {
               string.IsNullOrWhiteSpace(existringComments)?"":existringComments,
               string.IsNullOrWhiteSpace(fileNotes) || fileNotes == "undefined"?"":fileNotes
           }.Where(x => x != string.Empty));
        }
        private string GetContent(long documentObjectid)
        {
            using (TerminalDocumentEntities DbDocumentcontext = new TerminalDocumentEntities())
            {
                if (documentObjectid > 0)
                {
                    string Content = "";
                    DocumentObject Docobject = DbDocumentcontext.DocumentObjects.Where(se => se.DocumentObjectId == documentObjectid).FirstOrDefault();

                    if (Docobject != null)
                    {
                        Content = Docobject.Object;
                        if (string.IsNullOrEmpty(Content) 
                            && !string.IsNullOrEmpty(Docobject.ObjectPath)
                             && File.Exists(Docobject.ObjectPath))
                        {
                            Content = File.ReadAllText(Docobject.ObjectPath);
                        }
                        return ReportingDataProvider.FormatXML(Content);
                    }
                }
            }
            return string.Empty;

        }

        public Tuple<ResubmitBulkExceptionDTO, List<ExceptionDTO>> BulkResolveException(string status,List<ExceptionDTO> exceptionDetails, int userId)
        {
            List<ExceptionDTO> ReturnExceptionDetails = new List<ExceptionDTO>();
            ResubmitBulkExceptionDTO resubmitExceptionDTO = new ResubmitBulkExceptionDTO();
            int typeCodeId = Convert.ToInt32(status);
            if (typeCodeId == (int)(ExceptionStatusEnum.Resubmitted))
            {
               return BulkResubmitException(exceptionDetails, userId);
            }
            else
            {
                using (Entities dbContext = new Entities())
                {
                    List<ExceptionDTO> ExceptionIds = new List<ExceptionDTO>();
                    ExceptionIds = exceptionDetails.Select(se => new ExceptionDTO()
                    {
                        Exceptionid = se.Exceptionid,
                    }).ToList();


                    if (ExceptionIds != null)
                    {
                        int Totalsubmittedcnt = 0;
                        int scuccessfullysubmittedcnt = 0;
                        int unscuccessfullysubmittedcnt = 0;

                        Totalsubmittedcnt = ExceptionIds.Count;

                        foreach (var item in exceptionDetails)
                        {

                            TerminalDBEntities.Exception ExceptionInfo = dbContext.Exceptions.Where(se => se.ExceptionId == item.Exceptionid).FirstOrDefault();

                            //Preppare successfully Resubmitted Exceptions
                            scuccessfullysubmittedcnt = scuccessfullysubmittedcnt + 1;
                            ExceptionInfo.TypeCodeId = (int)typeCodeId;
                            ExceptionInfo.LastModifiedById = userId;
                            ExceptionInfo.LastModifiedDate = DateTime.Now;
                            ExceptionInfo.ExceptionNotes.Add(
                                new ExceptionNote()
                                {
                                    CreatedById = userId,
                                    ExceptionId = item.Exceptionid,
                                    CreatedDate = DateTime.Now,
                                    ExceptionNotes = "Resolved, exception status changed from " + item.Status.Name.ToString() + " to " + ExceptionStatusEnum.Resolved.ToString()
                                });
                            dbContext.Entry(ExceptionInfo).State = System.Data.Entity.EntityState.Modified;
                            AuditLogHelper.SaveChanges(dbContext);

                            item.Comments = dbContext.ExceptionNotes.Where(se => se.ExceptionId == item.Exceptionid).OrderByDescending(sl => sl.CreatedDate)
                            .Select(ENote => ENote.CreatedDate + " " + dbContext.Tower_Users.Where(fi => fi.UserId == ENote.CreatedById).Select(sl => sl.UserName).FirstOrDefault() + " " + ENote.ExceptionNotes).ToList();
                            item.LastModifiedDate = ExceptionInfo.LastModifiedDate.ToString();
                            item.Status = new ExceptionStatus { ID = (int)ExceptionStatusEnum.Resubmitted, Name = ExceptionStatusEnum.Resolved.ToString() };
                            item.LastModifiedBy = dbContext.Tower_Users.Where(fi => fi.UserId == userId).Select(sl => sl.UserName).FirstOrDefault();
                            resubmitExceptionDTO.SuccessResubmitCount = scuccessfullysubmittedcnt;
                            resubmitExceptionDTO.UnSuccessResubmitCount = unscuccessfullysubmittedcnt;
                            resubmitExceptionDTO.TotalResubmitCount = Totalsubmittedcnt;
                            ReturnExceptionDetails.Add(item);
                        }
                    }
                }
                return new Tuple<ResubmitBulkExceptionDTO, List<ExceptionDTO>>(resubmitExceptionDTO, ReturnExceptionDetails);
            }
            //return resubmitExceptionDTO;

         
        }

        public IEnumerable<ExceptionDTO> GetTEQExceptionsbyCondition(string exceptionType, string status, string messagetype,int tenantId, SearchDetail value)
        {
            DateTime startDateTime;
            DateTime endDateTime;
            int typeCodeId = 0;
            int messagetypeId = 0;
            if (value.Fromdate == null)
            {
                startDateTime = Convert.ToDateTime("01/01/2017");
            }
            else
            {
                startDateTime = Convert.ToDateTime(value.Fromdate);
            }
            if (value.ThroughDate == null)
            {
                endDateTime = DateTime.Today;
            }
            else
            {
                endDateTime = Convert.ToDateTime(value.ThroughDate);
            }
            List<ExceptionDTO> det = new List<ExceptionDTO>();
            typeCodeId = Convert.ToInt32(status);       
            messagetypeId = Convert.ToInt32(messagetype);
            using (var dbContext = new Entities())
            {
                var result = dbContext.GetExceptionFilterDetails(startDateTime, endDateTime, (int)ExceptionGroupEnum.TEQ,tenantId, exceptionType, messagetypeId, typeCodeId, value.search).ToList();             
                if (result != null)
                {
                    foreach (var se in result)
                    {
                        var ExceptionTypeID = dbContext.ExceptionTypes.Where(de => de.ExceptionTypeName == se.ExceptionTypeName).FirstOrDefault();
                        ExceptionDTO Detail = new ExceptionDTO();
                        Detail.Exceptionid = se.ExceptionId == null ?0:(int)se.ExceptionId ;
                        Detail.ExceptionTypeid = ExceptionTypeID != null ? ExceptionTypeID.ExceptionTypeId : 0;
                        Detail.ExceptionType = !string.IsNullOrEmpty(se.ExceptionTypeName) ? se.ExceptionTypeName : string.Empty;
                        Detail.ExceptionDesc = !string.IsNullOrEmpty(se.ExceptionDesc) ? se.ExceptionDesc : string.Empty;
                        Detail.CreatedBy = (string.IsNullOrEmpty(se.CreatedBy) && se.CreatedById == 0 ? string.Empty : se.CreatedBy);
                        Detail.CreatedDate = se.CreatedDate.ToString();
                        Detail.LastModifiedBy = (string.IsNullOrEmpty(se.LastModifiedBy) && se.LastModifiedById == 0 ? string.Empty : se.LastModifiedBy);
                        Detail.LastModifiedDate = se.LastModifiedDate.ToString();
                        Detail.Status = new ExceptionStatus() { ID = se.TypeCodeId == null ?0:(int)se.TypeCodeId, Name = se.ExceptionStatus };
                        if (!string.IsNullOrEmpty(se.DocObjectPath)
                             && File.Exists(se.DocObjectPath))
                        {
                            string content = File.ReadAllText(se.DocObjectPath);
                            var items = (from t in XDocument.Parse(content)?.Descendants(DataContracts.Constants.EXCEPTION)
                                         select new
                                         {
                                             DocumentObjectId = Convert.ToInt64(t.Element("DocumentObjectId")?.Value),
                                             ExternalRefNumber = t.Element("ExternalRefNum")?.Value,
                                         }).FirstOrDefault();
                            se.ExternalRefNum = items.ExternalRefNumber;
                            se.DocumentObjectId = items.DocumentObjectId;
                        }
                        Detail.DocumentObjectid = se.DocumentObjectId.HasValue ? se.DocumentObjectId.Value : 0;
                        Detail.ExternalRefNum = !string.IsNullOrEmpty(se.ExternalRefNum) ? se.ExternalRefNum : string.Empty;
                        Detail.MessageType = !string.IsNullOrEmpty(se.MessageTypeName) ? se.MessageTypeName : string.Empty;
                        Detail.MessageTypeid = se.MessageTypeId.HasValue ? se.MessageTypeId.Value : 0;
                        Detail.ServiceType = !string.IsNullOrEmpty(se.ServiceName) ? se.ServiceName : string.Empty;
                        Detail.ServiceRequestId = se.ServiceRequestId.HasValue ? se.ServiceRequestId.Value : 0;
                        Detail.TypeCodeId = se.TypeCodeId == null ? 0 : (int)se.TypeCodeId;
                        Detail.ParentExternalRefNum = string.Empty;
                        Detail.Tenant = !string.IsNullOrEmpty(se.TenantName) ? se.TenantName : string.Empty;
                        Detail.TenantId = dbContext.Tenants.Where(te => te.TenantName == Detail.Tenant).Select(sl => sl.TenantId).FirstOrDefault();
                        Detail.Reporting = new ReportingDTO();

                        det.Add(Detail);
                    }
                   
                }
                return det;
            }
        }
        public IEnumerable<ExceptionType> GetExceptionList()
        {
            using (Entities dbContext = new Entities())
            {
                List<ExceptionType> ExceptionType = dbContext.ExceptionTypes.Where(s1 => s1.ExceptionGroupId == (int)(ExceptionGroupEnum.TEQ)).Select(s1 => new ExceptionType
                {
                    ExceptionTypeName = s1.ExceptionTypeName

                }).ToList();
                return ExceptionType;
            }

        }
        #endregion " TEQ "

        private void SetTrackingId(string TrackingId)
        {
            //EMS.EMSAdapter.GetTrackingId = () =>
            //{          
            //    if (!HttpContext.Current.Items.Contains(DataContracts.Constants.TrackingId) )
            //        HttpContext.Current.Items[DataContracts.Constants.TrackingId] = TrackingId == null ? "":TrackingId;
            //    return HttpContext.Current.Items[DataContracts.Constants.TrackingId] as string;

            //}; 
        }
    }
}
