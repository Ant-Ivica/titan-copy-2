using FA.LVIS.Tower.Common;
using FA.LVIS.Tower.Data.TerminalDBEntities;
using FA.LVIS.Tower.DataContracts;
using LVIS.Adapters.EMSAdapter;
using LVIS.Common;
using LVIS.DTO.Canonical.Order;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Security.Cryptography;

namespace FA.LVIS.Tower.Data
{
    public class UtilitiesDataProvider : Core.DataProviderBase, IUtilitiesDataProvider

    {
        Logger sLogger = new Logger(typeof(UtilitiesDataProvider));
        IUtils Utils;
        IEMSAdapter EMSAdapter;

        public UtilitiesDataProvider()
        {
            Utils = new Utils();
            EMSAdapter = new EMSAdapter();
        }

        public int UpdateExternalRefNum(int servicerequestid, string externalRefnum, string newexternalRefnum, int userId)
        {

            Entities dbContext = new Entities();

            int success = 0;

            var ServReq = dbContext.ServiceRequests.Where(sel => sel.ServiceRequestId == servicerequestid).FirstOrDefault();

            if (ServReq != null)
            {
                ServReq.ExternalRefNum = newexternalRefnum;
                ServReq.LastModifiedById = userId;
                ServReq.LastModifiedDate = DateTime.Now;
                success = AuditLogHelper.SaveChanges(dbContext);

                using (var dbContextTransaction = dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        Order order = new Order();
                        order.ExternalRefNum = newexternalRefnum;
                        order.InternalRefNum = ServReq.InternalRefNum;
                        order.ServiceRequestId = ServReq.ServiceRequestId;

                        TerminalDocumentEntities DbDocumentcontext = new TerminalDocumentEntities();


                        string CanonicalContent = Utils.SerializeToString<Order>(order);
                        DocumentObject Canonical = new DocumentObject();
                        Canonical.CreatedById = userId;
                        Canonical.CreatedDate = DateTime.Now;
                        Canonical.DocumentObjectFormat = "XML";
                        Canonical.Object = CanonicalContent;
                        DbDocumentcontext.DocumentObjects.Add(Canonical);
                        int Success = AuditLogHelper.SaveChanges(DbDocumentcontext);

                        var messageMapId = (from mm in dbContext.MessageMaps
                                            join mt in dbContext.MessageTypes on mm.MessageTypeId equals mt.MessageTypeId
                                            join mt1 in dbContext.MessageTypes on mm.ExternalMessageTypeId equals mt1.MessageTypeId
                                            where mt1.ApplicationId == ServReq.ApplicationId
                                            && mt.MessageTypeId == 10026
                                            && mm.TenantId == ServReq.TenantId
                                            select mm.MessageMapId).FirstOrDefault();

                        MessageLog messageLog = new MessageLog()
                        {
                            CreatedById = userId,
                            LastModifiedById = userId,
                            CreatedDate = DateTime.Now,
                            LastModifiedDate = DateTime.Now,
                            MessageMapId = messageMapId,
                            TenantId = ServReq.TenantId,
                            ServiceRequestId = ServReq.ServiceRequestId,
                            DocumentObjectId = Canonical.DocumentObjectId,
                            ParentMessageLogId = null,
                            MessageLogDesc = "Update External Ref Num",
                            RestartStep = null,
                        };

                        dbContext.MessageLogs.Add(messageLog);
                        dbContext.SaveChanges();

                        //var Dest = DataContracts.Constants.APPLICATION_FAST;


                        //sLogger.Error(string.Format("Publishing  Update External Reference ID message DocumentObjectId: {0} to Destination: {1}", Canonical.DocumentObjectId, Dest));

                        //if (EMSAdapter.PublishMessage("FAST", items.Source.ToUpper(), ServReq.ServiceRequestId, messageLog.MessageLogId, Canonical.DocumentObjectId.ToString()))
                        //{
                        //    sLogger.Info(string.Format("Publishing update External refernce id message DocumentObjectId: {0}  SUCCESSFUL", Canonical.DocumentObjectId));
                        //    dbContextTransaction.Commit();
                        //}
                        //else
                        //{
                        //    sLogger.Error(string.Format("Publishing  Update External Reference ID message DocumentObjectId: {0} FAILED", Canonical.DocumentObjectId));
                        //    success = 0;
                        //    dbContextTransaction.Rollback();
                        //}
                    }
                    catch
                    {
                        dbContextTransaction.Rollback();
                    }
                }
            }
            return success;
        }

        public int UpdateandAcceptExternalRefNum(int servicerequestid, string externalRefnum, string newexternalRefnum, int userId)
        {
            if (UpdateExternalRefNum(servicerequestid, externalRefnum, newexternalRefnum, userId) != 0)
            {
                //Code for acknowledgement to RealEC
            }

            return 0;
        }

        public int UpdateServiceRequestInfo(int servicerequestid, string externalRefnum, string internalRefnum, int internalRefid, string customerRefnum, int userId, int status,bool  chkUniqueID, bool chkExternalRefNum)
        {
            Entities dbContext = new Entities();

            int success = 0;
            int extSuccess = 0;
            int unqSuccess = 0;

            var ServReq = dbContext.ServiceRequests.Where(sel => sel.ServiceRequestId == servicerequestid).FirstOrDefault();
            
            if (ServReq != null)
            {
                //Update in the LVIS
                if (!chkUniqueID && !chkExternalRefNum){
                    ServReq.ExternalRefNum = externalRefnum;
                    ServReq.InternalRefNum = internalRefnum;
                    ServReq.InternalRefId = internalRefid;
                    ServReq.CustomerRefNum = customerRefnum;
                    ServReq.LastModifiedById = userId;
                    ServReq.LastModifiedDate = DateTime.Now;
                    ServReq.StatusTypeCodeId = status;
                    success = AuditLogHelper.SaveChanges(dbContext);
                }
                //Update in the FAST
                else{
                    if (chkExternalRefNum)
                        extSuccess = UpdateExternalRefInFast(servicerequestid, externalRefnum, internalRefnum, internalRefid);
                    if(chkUniqueID)
                        unqSuccess = UpdateServiceRefnoInFast(servicerequestid, externalRefnum, internalRefnum, internalRefid);

                    if (chkExternalRefNum && chkUniqueID)
                    {
                        if (extSuccess == 1 && unqSuccess == 1)
                            success = 1;
                        else
                            success = -1;
                    }
                    else
                    {
                        if (chkExternalRefNum)
                            success = extSuccess;
                        if (chkUniqueID)
                            success = unqSuccess;
                    }
                  }
            }
            return success;
        }

        public ServiceRequestDTO GetServiceReqInfo(int servicerequestid)
        {
            Entities dbContext = new Entities();
            ServiceRequestDTO ServReqInfo = new ServiceRequestDTO();

            var ServReq = dbContext.ServiceRequests.Where(sel => sel.ServiceRequestId == servicerequestid).FirstOrDefault();
            if (ServReq != null)
            {
                ServReqInfo.ServiceRequestId = ServReq.ServiceRequestId;
                ServReqInfo.ExternalRefNum = ServReq.ExternalRefNum;
                ServReqInfo.InternalRefNum = ServReq.InternalRefNum;
                ServReqInfo.InternalRefId = ServReq.InternalRefId;
                ServReqInfo.CustomerRefNum = ServReq.CustomerRefNum;
                ServReqInfo.tenantID = ServReq.TenantId??0;

                if (ServReq.StatusTypeCodeId != null)
                    ServReqInfo.Status = new ExceptionStatus() { ID = ServReq.StatusTypeCodeId.Value, Name = dbContext.TypeCodes.Where(x => x.TypeCodeId == ServReq.StatusTypeCodeId.Value).Select(x => x.TypeCodeDesc).FirstOrDefault() };
            }

            return ServReqInfo;
        }

        public IEnumerable<ExceptionStatus> GetStatus()
        {
            using (Entities dbContext = new Entities())
            {
                List<ExceptionStatus> Typecodes = dbContext.TypeCodes.Where(se => se.GroupTypeCode == 850).Select(sl => new ExceptionStatus
                {
                    ID = sl.TypeCodeId,
                    Name = sl.TypeCodeDesc
                }).ToList();

                return Typecodes;
            }
        }
        public IEnumerable<ApplicationMappingDTO> GetApplications()
        {
            using (Entities dbContext = new Entities())
            {
                List<ApplicationMappingDTO> applications = dbContext.Applications.Where(s => s.ApplicationId == (int)ApplicationEnum.SettlementServices || s.ApplicationId == (int)ApplicationEnum.CAPI)
                    .Select(sl => new ApplicationMappingDTO
                {
                    ApplicationId = sl.ApplicationId,
                    ApplicationName = sl.ApplicationName
                }).ToList();

                return applications;
            }
        }
        private static string sSaltValue = "665f4d7b-7ed1-48e0-b477-ec579ce8921b";
        private static byte[] sSaltValueBytes = System.Text.Encoding.Default.GetBytes(sSaltValue);
        
        public string Hash(string textValue)
        {
            byte[] textBytes = System.Text.Encoding.Default.GetBytes(textValue);
            return Convert.ToBase64String(GenerateSaltedHash(textBytes, sSaltValueBytes));
        }
        private byte[] GenerateSaltedHash(byte[] plainText, byte[] salt)
        {
            HashAlgorithm algorithm = new SHA256Managed();

            byte[] plainTextWithSaltBytes =
              new byte[plainText.Length + salt.Length];

            for (int i = 0; i < plainText.Length; i++)
            {
                plainTextWithSaltBytes[i] = plainText[i];
            }
            for (int i = 0; i < salt.Length; i++)
            {
                plainTextWithSaltBytes[plainText.Length + i] = salt[i];
            }

            return algorithm.ComputeHash(plainTextWithSaltBytes);
        }

        public bool AddCredentials(string applicationName, string userName, string password,int userId)
        {
            using (Entities dbContext = new Entities())
            {
                var endpointTypeCode = dbContext.TypeCodes.Where(i => i.GroupTypeCode == 1 && i.TypeCodeDesc.ToLower() == "lvis2/api/" + applicationName.ToLower()).FirstOrDefault();
                var credential = dbContext.Credentials.FirstOrDefault(i => i.Username.ToLower() == userName.ToLower());

                if (credential != null)
                {
                    credential.Password = Hash(password);
                    credential.LastModifiedById = userId;
                    credential.LastModifiedDate = DateTime.UtcNow;
                    dbContext.Entry(credential).State = System.Data.Entity.EntityState.Modified;
                    AuditLogHelper.SaveChanges(dbContext);
                    return true;
                }
                else
                {
                    if (endpointTypeCode != null)
                    {
                        var credentials = new Credential()
                        {
                            Username = userName,
                            Password = Hash(password),
                            CreatedDate = DateTime.UtcNow,
                            LastModifiedById = userId,
                            LastModifiedDate = DateTime.UtcNow,
                            LastAuthenticatedDate = DateTime.UtcNow
                        };
                        dbContext.Credentials.Add(credentials);
                        AuditLogHelper.SaveChanges(dbContext);

                        var credentialEndPoint = new CredentialEndPoint()
                        {
                            CredentialId = credentials.CredentialId,
                            TypeCodeId = endpointTypeCode.TypeCodeId,
                            CreatedDate = DateTime.UtcNow,
                            LastModifiedById = userId,
                            LastModifiedDate = DateTime.UtcNow
                        };

                        dbContext.CredentialEndPoints.Add(credentialEndPoint);
                        AuditLogHelper.SaveChanges(dbContext);
                        return true;
                    }

                    else
                    {
                        int maxTypeCodeId = dbContext.TypeCodes.Where(i => i.GroupTypeCode == 1).AsEnumerable().OrderByDescending(i => i.TypeCodeId).FirstOrDefault().TypeCodeId;
                        // Add TypeCode
                        var typeCode = new FA.LVIS.Tower.Data.TerminalDBEntities.TypeCode()
                        {
                            TypeCodeId = maxTypeCodeId + 1,
                            TypeCodeDesc = "lvis2/api/" + applicationName.ToLower(),
                            GroupTypeCode = 1,
                            CreatedDate = DateTime.UtcNow
                        };
                        dbContext.TypeCodes.Add(typeCode);
                        AuditLogHelper.SaveChanges(dbContext);

                        //Add Credential
                        var credentials = new Credential()
                        {
                            Username = userName,
                            Password = Hash(password),
                            CreatedDate = DateTime.UtcNow,
                            LastModifiedById = userId,
                            LastModifiedDate = DateTime.UtcNow,
                            LastAuthenticatedDate = DateTime.UtcNow
                        };
                        dbContext.Credentials.Add(credentials);
                        AuditLogHelper.SaveChanges(dbContext);

                        var credentialEndPoint = new CredentialEndPoint()
                        {
                            CredentialId = credentials.CredentialId,
                            TypeCodeId = typeCode.TypeCodeId,
                            CreatedDate = DateTime.UtcNow,
                            LastModifiedById = userId,
                            LastModifiedDate = DateTime.UtcNow
                        };

                        dbContext.CredentialEndPoints.Add(credentialEndPoint);
                        AuditLogHelper.SaveChanges(dbContext);
                        return true;
                    }
                }
            }
        }

        public bool ConfirmService(int servicerequestid, int userid)
        {
            TerminalDocumentEntities DbDocumentcontext = new TerminalDocumentEntities();
            ServiceRequest ServiceRequest = null;
            using (Entities dbContext = new Entities())
            {
                ServiceRequest = dbContext.ServiceRequests.Where(se => se.ServiceRequestId == servicerequestid).FirstOrDefault();
            }

            if (ServiceRequest != null)
            {
                XmlSerializer mySerializer = new XmlSerializer(typeof(Order));
                Order order = new Order();
                order.InternalRefNum = ServiceRequest.InternalRefNum;
                order.InternalRefId = ServiceRequest.InternalRefId.Value;
                order.ServiceRequestId = ServiceRequest.ServiceRequestId;
                order.IsFromExceptionQueue = true;
                order.TenantId = ServiceRequest.TenantId.Value;
                order.RecievedDateTime = ServiceRequest.CreatedDate;
                order.OriginatingApplicationId = ServiceRequest.ApplicationId;

                string CanonicalContent = Utils.SerializeToString<Order>(order);
                DocumentObject Canonical = new DocumentObject();
                Canonical.CreatedById = 1;
                Canonical.CreatedDate = DateTime.Now;
                Canonical.DocumentObjectFormat = "XML";
                Canonical.Object = CanonicalContent;
                DbDocumentcontext.DocumentObjects.Add(Canonical);
                int Success = AuditLogHelper.SaveChanges(DbDocumentcontext);
                Entities dbContext1 = new Entities();

                var messageMapId = (from mm in dbContext1.MessageMaps
                                    join mt in dbContext1.MessageTypes on mm.MessageTypeId equals mt.MessageTypeId
                                    join mt1 in dbContext1.MessageTypes on mm.ExternalMessageTypeId equals mt1.MessageTypeId
                                    where mt1.ApplicationId == ServiceRequest.ApplicationId
                                    && mm.IsInbound == false
                                    && mt.MessageTypeId == 10021
                                    && mm.TenantId == ServiceRequest.TenantId
                                    select mm.MessageMapId).FirstOrDefault();

                if (messageMapId == 0)
                    return false;

                MessageLog messageLog = new MessageLog()
                {
                    CreatedById = userid,
                    LastModifiedById = userid,
                    CreatedDate = DateTime.Now,
                    LastModifiedDate = DateTime.Now,
                    MessageMapId = messageMapId,
                    TenantId = ServiceRequest.TenantId,
                    ServiceRequestId = ServiceRequest.ServiceRequestId,
                    DocumentObjectId = Canonical.DocumentObjectId,
                    ParentMessageLogId = null,
                    MessageLogDesc = "Confirm Service",
                    RestartStep = null
                };

                dbContext1.MessageLogs.Add(messageLog);
                AuditLogHelper.SaveChanges(dbContext1);

                sLogger.Debug(string.Format("Publishing Confirmation message MessageLogId: {0} ", messageLog.MessageLogId));
                ApplicationEnum Dest = (ApplicationEnum)ServiceRequest.ApplicationId;
                if (EMSAdapter.PublishMessage(Dest.ToString().ToUpper(), "LVIS", ServiceRequest.ServiceRequestId, messageLog.MessageLogId, Canonical.DocumentObjectId.ToString()))
                {
                    sLogger.Info(string.Format("Publishing Confirm service message MessageLogId: {0}  SUCCESSFUL", messageLog.MessageLogId));
                    return true;

                }
                else
                {
                    sLogger.Info(string.Format("Publishing ConfirmService message MessageLogId: {0}  Failed", messageLog.MessageLogId));
                    return false;
                }

            }
            return false;
        }

        private int UpdateServiceRefnoInFast(int servicerequestid,string externalRefnum, string internalRefnum, int internalRefid)
        {
            int success = 0;
            Entities dbContext = new Entities();
            string uniqueID = string.Empty;
            string serviceType = string.Empty;
            if (externalRefnum != "" && externalRefnum != null)
            {
                var ServReq = dbContext.ServiceRequests.Where(sel => sel.ServiceRequestId == servicerequestid).FirstOrDefault();
                int lastIndex = externalRefnum.LastIndexOf('-');
                uniqueID = externalRefnum.Substring(lastIndex + 1);
                if (ServReq.ServiceId == (int)ServiceEnum.Escrow)
                    serviceType = FastServiceEnum.ESCROW_SERVICE_OBJ_CD;
                else if (ServReq.ServiceId == (int)ServiceEnum.Title)
                    serviceType = FastServiceEnum.TITLE_SERVICE_OBJ_CD;
                else if (ServReq.ServiceId == (int)ServiceEnum.SubEscrow)
                    serviceType = FastServiceEnum.SUBESCROW_SERVICE_OBJ_CD;
                
                //else if(ServReq.ServiceId == (int)ServiceEnum.EscrowTitle)
                
                if (uniqueID != null && uniqueID != null)
                //Call the Fast Web Service to update the Unique ID in the FAST.
                {
                    FASTProcessing.EQFASTSearch searchClient = new FASTProcessing.EQFASTSearch();
                    var updateFastService = searchClient.GetUpdateExternaServiceNumRequest(internalRefid, serviceType, uniqueID);
                    success = searchClient.UpdateExternalServiceNumber(updateFastService);
                }
            }

            return success;
        }

        private int UpdateExternalRefInFast(int servicerequestid, string externalRefnum, string internalRefnum, int internalRefid)
        {
            int success = 0;
            Entities dbContext = new Entities();
            string secondExternalRefNum = string.Empty;
            if (externalRefnum != "" && externalRefnum != null)
            {
                var ServReq = dbContext.ServiceRequests.Where(sel => sel.ServiceRequestId == servicerequestid).FirstOrDefault();
                //Call the Fast Service to update the Unique ID in the FAST.
                {
                    FASTProcessing.EQFASTSearch searchClient = new FASTProcessing.EQFASTSearch();
                    var updtExternalRefNum = searchClient.GetExternalServiveNumRequest(internalRefid,externalRefnum, secondExternalRefNum);
                    success = searchClient.updateExternalFileNumber(updtExternalRefNum);
                }
            }
            return success;
        }
    }
}