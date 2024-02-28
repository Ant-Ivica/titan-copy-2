using System;
using System.Collections.Generic;
using System.Linq;
using FA.LVIS.Tower.DataContracts;
using FA.LVIS.Tower.Data.TerminalDBEntities;
using FA.LVIS.Tower.Core;

namespace FA.LVIS.Tower.Data
{
    public class OutDocMappingDataProvider : Core.DataProviderBase, IOutDocMappingDataProvider
    {
        public List<DataContracts.OutDocMapping> GetOutboundDocumentsByCategory(int categoryId, int tenantId, int extApplicationId)
        {
            List<DataContracts.OutDocMapping> OutDocMapping = new List<OutDocMapping>();

            using (var dbContext = new TerminalDBEntities.Entities())
            {
                if (tenantId != (int)TerminalDBEntities.TenantIdEnum.LVIS)
                {
                    //var details = from sub in dbContext.Subscriptions.Where(subs => subs.CategoryId == categoryId && subs.CustomerId == null && subs.TenantId == tenantId)
                    //              join mt in dbContext.MessageTypes.Where(sl => sl.ApplicationId == extApplicationId) on sub.MessageTypeId equals mt.MessageTypeId
                    //              join mm in dbContext.MessageMaps.Where(mmap => !mmap.IsInbound && mmap.TenantId == tenantId) on mt.MessageTypeId equals mm.ExternalMessageTypeId
                    //              join dmm in dbContext.DocumentMessageMaps.Where(dmm => dmm.TenantId == tenantId) on mm.MessageMapId equals dmm.MessageMapId
                    //              join edm in dbContext.DocumentMaps.Where(dm => dm.IsDeleted != true && dm.TenantId == tenantId && dm.CategoryId == categoryId) on dmm.DocumentMapId equals edm.DocumentMapId
                    //              join edt in dbContext.DocumentTypes on edm.ExternalDocTypeId equals edt.DocumentTypeId
                    //              where (edt.ApplicationId != (int)ApplicationEnum.FAST)
                    //              join ldt in dbContext.DocumentTypes on edm.DocumentTypeId equals ldt.DocumentTypeId
                    var details = from edm in dbContext.DocumentMaps.Where(dm => dm.IsDeleted != true && dm.TenantId == tenantId && dm.CategoryId == categoryId && dm.CustomerId == null)
                                  join edt in dbContext.DocumentTypes on edm.ExternalDocTypeId equals edt.DocumentTypeId
                                  where (edt.ApplicationId != (int)ApplicationEnum.FAST)
                                  join ldt in dbContext.DocumentTypes on edm.DocumentTypeId equals ldt.DocumentTypeId
                                  join dmm in dbContext.DocumentMessageMaps.Where(dmm => dmm.TenantId == tenantId) on edm.DocumentMapId equals dmm.DocumentMapId
                                  join mm in dbContext.MessageMaps.Where(mmap => !mmap.IsInbound && mmap.TenantId == tenantId) on dmm.MessageMapId equals mm.MessageMapId
                                  join mt in dbContext.MessageTypes.Where(sl => sl.ApplicationId == extApplicationId) on mm.ExternalMessageTypeId equals mt.MessageTypeId
                                  select new
                                  {
                                      ExternalDocumentType = new DataContracts.DocumentType() { ID = edt.DocumentTypeId, Name = edt.DocumentTypeDesc },
                                      ExternalApplication = new DataContracts.Users() { ID = edt.Application.ApplicationId, Name = edt.Application.ApplicationName },
                                      ExternalMessageType = new DataContracts.MessageType() { MessageTypeId = mt.MessageTypeId, MessageTypeName = mt.MessageTypeName, MessageMapId = mm.MessageMapId, Sequence = dmm.Sequence != null ?dmm.Sequence.Value :0},
                                      InternalDocumentType = new DataContracts.DocumentType() { ID = ldt.DocumentTypeId, Name = ldt.DocumentTypeDesc },
                                      DocumentStatus = new DataContracts.DocumentType()
                                      {
                                          ID = dbContext.TypeCodes.Where(sel => edm.StatusTypeCodeId == sel.TypeCodeId).FirstOrDefault() != null ? dbContext.TypeCodes.Where(sel => edm.StatusTypeCodeId == sel.TypeCodeId).FirstOrDefault().TypeCodeId : 0,
                                          Name = dbContext.TypeCodes.Where(sel => edm.StatusTypeCodeId == sel.TypeCodeId).FirstOrDefault() != null ? dbContext.TypeCodes.Where(sel => edm.StatusTypeCodeId == sel.TypeCodeId).FirstOrDefault().TypeCodeDesc : ""
                                      },
                                      Service = edm.ServiceId != null ?
                                                  new DataContracts.Service()
                                                  {
                                                      ID = dbContext.Services.Where(sel => edm.ServiceId == sel.ServiceId).FirstOrDefault().ServiceId,
                                                      Name = dbContext.Services.Where(sel => edm.ServiceId == sel.ServiceId).FirstOrDefault().ServiceName
                                                  } :
                                                  new DataContracts.Service() { ID = -1, Name = "Any" },
                                      OutboundDocumentMapid = edm.DocumentMapId,
                                      MessageMapId = mm.MessageMapId,
                                      CategoryId = categoryId,
                                      Tenant = new DataContracts.Users() { ID = edm.TenantId, Name = edm.Tenant.TenantName }
                                  };

                    OutDocMapping = (from i in details.ToList()
                                     group i by i.OutboundDocumentMapid into g
                                     select new OutDocMapping
                                     {
                                         ExternalDocumentType = g.Select(kvp => kvp.ExternalDocumentType).FirstOrDefault(),
                                         ExternalApplication = g.Select(kvp => kvp.ExternalApplication).FirstOrDefault(),
                                         ExternalMessageTypeList = g.Select(kvp => kvp.ExternalMessageType).ToList(),
                                         InternalDocumentType = g.Select(kvp => kvp.InternalDocumentType).FirstOrDefault(),
                                         DocumentStatus = g.Select(kvp => kvp.DocumentStatus).FirstOrDefault(),
                                         Service = g.Select(kvp => kvp.Service).FirstOrDefault(),
                                         OutboundDocumentMapid = g.Key,
                                         MessageMapId = g.Select(kvp => kvp.MessageMapId).FirstOrDefault(),
                                         CategoryId = categoryId,
                                         Tenant = g.Select(kvp => kvp.Tenant).FirstOrDefault(),
                                         ExternalMessageTypeValue = string.Join(",", g.Select(kvp => kvp.ExternalMessageType.MessageTypeName))

                                     }).ToList();

                }
                else
                {
                    //var details = from sub in dbContext.Subscriptions.Where(subs => subs.CategoryId == categoryId && subs.CustomerId == null)
                    //              join mt in dbContext.MessageTypes.Where(sl => sl.ApplicationId == extApplicationId) on sub.MessageTypeId equals mt.MessageTypeId
                    //              join mm in dbContext.MessageMaps.Where(mmap => !mmap.IsInbound) on mt.MessageTypeId equals mm.ExternalMessageTypeId
                    //              join dmm in dbContext.DocumentMessageMaps on mm.MessageMapId equals dmm.MessageMapId
                    //              join edm in dbContext.DocumentMaps.Where(dm => dm.IsDeleted != true && dm.TenantId == tenantId && dm.CategoryId == categoryId) on dmm.DocumentMapId equals edm.DocumentMapId
                    //              join edt in dbContext.DocumentTypes on edm.ExternalDocTypeId equals edt.DocumentTypeId
                    //              where (edt.ApplicationId != (int)ApplicationEnum.FAST)
                    //              join ldt in dbContext.DocumentTypes on edm.DocumentTypeId equals ldt.DocumentTypeId
                    var details = from edm in dbContext.DocumentMaps.Where(dm => dm.IsDeleted != true && dm.CategoryId == categoryId && dm.CustomerId == null)
                                  join edt in dbContext.DocumentTypes on edm.ExternalDocTypeId equals edt.DocumentTypeId
                                  where (edt.ApplicationId != (int)ApplicationEnum.FAST)
                                  join idt in dbContext.DocumentTypes on edm.DocumentTypeId equals idt.DocumentTypeId
                                  join dmm in dbContext.DocumentMessageMaps on edm.DocumentMapId equals dmm.DocumentMapId
                                  join mm in dbContext.MessageMaps.Where(mmap => !mmap.IsInbound) on dmm.MessageMapId equals mm.MessageMapId
                                  join mt in dbContext.MessageTypes.Where(sl => sl.ApplicationId == extApplicationId) on mm.ExternalMessageTypeId equals mt.MessageTypeId
                                  select new
                                  {
                                      ExternalDocumentType = new DataContracts.DocumentType() { ID = edt.DocumentTypeId, Name = edt.DocumentTypeDesc },
                                      ExternalApplication = new DataContracts.Users() { ID = edt.Application.ApplicationId, Name = edt.Application.ApplicationName },
                                      ExternalMessageType = new DataContracts.MessageType() { MessageTypeId = mt.MessageTypeId, MessageTypeName = mt.MessageTypeName, MessageMapId = mm.MessageMapId, Sequence = dmm.Sequence.Value },
                                      InternalDocumentType = new DataContracts.DocumentType() { ID = idt.DocumentTypeId, Name = idt.DocumentTypeDesc },
                                      DocumentStatus = new DataContracts.DocumentType()
                                      {
                                          ID = dbContext.TypeCodes.Where(sel => edm.StatusTypeCodeId == sel.TypeCodeId).FirstOrDefault() != null ? dbContext.TypeCodes.Where(sel => edm.StatusTypeCodeId == sel.TypeCodeId).FirstOrDefault().TypeCodeId : 0,
                                          Name = dbContext.TypeCodes.Where(sel => edm.StatusTypeCodeId == sel.TypeCodeId).FirstOrDefault() != null ? dbContext.TypeCodes.Where(sel => edm.StatusTypeCodeId == sel.TypeCodeId).FirstOrDefault().TypeCodeDesc : ""
                                      },
                                      Service = edm.ServiceId != null ? 
                                                  new DataContracts.Service()
                                                  {
                                                      ID = dbContext.Services.Where(sel => edm.ServiceId == sel.ServiceId).FirstOrDefault().ServiceId,
                                                      Name = dbContext.Services.Where(sel => edm.ServiceId == sel.ServiceId).FirstOrDefault().ServiceName
                                                  } :
                                                  new DataContracts.Service() { ID = -1,  Name = "Any" },
                                      OutboundDocumentMapid = edm.DocumentMapId,
                                      MessageMapId = mm.MessageMapId,
                                      CategoryId = categoryId,
                                      Tenant = new DataContracts.Users() { ID = edm.TenantId, Name = edm.Tenant.TenantName }
                                  };

                    OutDocMapping = (from i in details.ToList()
                                     group i by i.OutboundDocumentMapid into g
                                     select new OutDocMapping
                                     {
                                         ExternalDocumentType = g.Select(kvp => kvp.ExternalDocumentType).FirstOrDefault(),
                                         ExternalApplication = g.Select(kvp => kvp.ExternalApplication).FirstOrDefault(),
                                         ExternalMessageTypeList = g.Select(kvp => kvp.ExternalMessageType).ToList(),
                                         InternalDocumentType = g.Select(kvp => kvp.InternalDocumentType).FirstOrDefault(),
                                         DocumentStatus = g.Select(kvp => kvp.DocumentStatus).FirstOrDefault(),
                                         Service = g.Select(kvp => kvp.Service).FirstOrDefault(),
                                         OutboundDocumentMapid = g.Key,
                                         MessageMapId = g.Select(kvp => kvp.MessageMapId).FirstOrDefault(),
                                         CategoryId = categoryId,
                                         Tenant = g.Select(kvp => kvp.Tenant).FirstOrDefault(),
                                         ExternalMessageTypeValue = string.Join(",", g.Select(kvp => kvp.ExternalMessageType.MessageTypeName))
                                     }).ToList();
                }

                //if (tenantId != ((int)TerminalDBEntities.TenantIdEnum.LVIS))
                //{
                //    OutDocMapping = OutDocMapping.Where(se => se.Tenant.ID == tenantId).ToList();
                //}
            }

            return OutDocMapping;
        }

        public List<DataContracts.OutDocMapping> GetLVISLenderOutboundDocuments(int Customerid, int Applicationid, int tenantId)
        {
            List<DataContracts.OutDocMapping> outDocuments = new List<OutDocMapping>();
            using (var dbContext = new TerminalDBEntities.Entities())
            {
                if (tenantId != (int)TerminalDBEntities.TenantIdEnum.LVIS)
                {

                    //var details = from sub in dbContext.Subscriptions.Where(subs => subs.CustomerId == Customerid && subs.CategoryId == null && subs.TenantId == tenantId)
                    //              join mt in dbContext.MessageTypes.Where(sl => sl.ApplicationId == Applicationid) on sub.MessageTypeId equals mt.MessageTypeId
                    //              join mm in dbContext.MessageMaps.Where(mmap => !mmap.IsInbound && mmap.TenantId == tenantId) on mt.MessageTypeId equals mm.ExternalMessageTypeId
                    //              join dmm in dbContext.DocumentMessageMaps.Where(dmm => dmm.TenantId == tenantId) on mm.MessageMapId equals dmm.MessageMapId
                    //              join edm in dbContext.DocumentMaps.Where(edm => edm.IsDeleted != true && edm.TenantId == tenantId && edm.CustomerId == Customerid) on dmm.DocumentMapId equals edm.DocumentMapId
                    //              join edt in dbContext.DocumentTypes on edm.ExternalDocTypeId equals edt.DocumentTypeId
                    //              where (edt.ApplicationId != (int)ApplicationEnum.FAST)
                    //              join ldt in dbContext.DocumentTypes on edm.DocumentTypeId equals ldt.DocumentTypeId
                    var details = from edm in dbContext.DocumentMaps.Where(dm => dm.IsDeleted != true && dm.TenantId == tenantId && dm.CategoryId == null && dm.CustomerId == Customerid)
                                  join edt in dbContext.DocumentTypes on edm.ExternalDocTypeId equals edt.DocumentTypeId
                                  where (edt.ApplicationId != (int)ApplicationEnum.FAST)
                                  join idt in dbContext.DocumentTypes on edm.DocumentTypeId equals idt.DocumentTypeId
                                  join dmm in dbContext.DocumentMessageMaps.Where(dmm => dmm.TenantId == tenantId) on edm.DocumentMapId equals dmm.DocumentMapId
                                  join mm in dbContext.MessageMaps.Where(mmap => !mmap.IsInbound && mmap.TenantId == tenantId) on dmm.MessageMapId equals mm.MessageMapId
                                  join mt in dbContext.MessageTypes.Where(sl => sl.ApplicationId == Applicationid) on mm.ExternalMessageTypeId equals mt.MessageTypeId
                                  select new OutDocMapping
                                  {
                                      ExternalDocumentType = new DataContracts.DocumentType() { ID = edt.DocumentTypeId, Name = edt.DocumentTypeDesc },
                                      ExternalApplication = new DataContracts.Users() { ID = edt.Application.ApplicationId, Name = edt.Application.ApplicationName },
                                      ExternalMessageType = new DataContracts.MessageType() { MessageTypeId = mt.MessageTypeId, MessageTypeName = mt.MessageTypeName, MessageMapId = mm.MessageMapId },
                                      InternalDocumentType = new DataContracts.DocumentType() { ID = idt.DocumentTypeId, Name = idt.DocumentTypeDesc },
                                      DocumentStatus = new DataContracts.DocumentType()
                                      {
                                          ID = dbContext.TypeCodes.Where(sel => edm.StatusTypeCodeId == sel.TypeCodeId).FirstOrDefault() != null ? dbContext.TypeCodes.Where(sel => edm.StatusTypeCodeId == sel.TypeCodeId).FirstOrDefault().TypeCodeId : 0,
                                          Name = dbContext.TypeCodes.Where(sel => edm.StatusTypeCodeId == sel.TypeCodeId).FirstOrDefault() != null ? dbContext.TypeCodes.Where(sel => edm.StatusTypeCodeId == sel.TypeCodeId).FirstOrDefault().TypeCodeDesc : ""
                                      },
                                      Service = edm.ServiceId != null ?
                                                  new DataContracts.Service()
                                                  {
                                                      ID = dbContext.Services.Where(sel => edm.ServiceId == sel.ServiceId).FirstOrDefault().ServiceId,
                                                      Name = dbContext.Services.Where(sel => edm.ServiceId == sel.ServiceId).FirstOrDefault().ServiceName
                                                  } :
                                                  new DataContracts.Service() { ID = -1, Name = "Any" },
                                      MessageMapId = mm.MessageMapId,
                                      OutboundDocumentMapid = edm.DocumentMapId,
                                      Customerid = Customerid,
                                      Tenant = new DataContracts.Users() { ID = edm.TenantId, Name = edm.Tenant.TenantName },
                                      CategoryId = (edm.CategoryId.HasValue ? edm.CategoryId : 0).Value
                                  };

                    outDocuments = (from i in details.ToList()
                                    group i by i.OutboundDocumentMapid into g
                                    select new OutDocMapping
                                    {
                                        ExternalDocumentType = g.Select(kvp => kvp.ExternalDocumentType).FirstOrDefault(),
                                        ExternalApplication = g.Select(kvp => kvp.ExternalApplication).FirstOrDefault(),
                                        ExternalMessageTypeList = g.Select(kvp => kvp.ExternalMessageType).ToList(),
                                        InternalDocumentType = g.Select(kvp => kvp.InternalDocumentType).FirstOrDefault(),
                                        DocumentStatus = g.Select(kvp => kvp.DocumentStatus).FirstOrDefault(),
                                        Service = g.Select(kvp => kvp.Service).FirstOrDefault(),
                                        OutboundDocumentMapid = g.Key,
                                        MessageMapId = g.Select(kvp => kvp.MessageMapId).FirstOrDefault(),
                                        Customerid = Customerid,
                                        Tenant = g.Select(kvp => kvp.Tenant).FirstOrDefault(),
                                        ExternalMessageTypeValue = string.Join(",", g.Select(kvp => kvp.ExternalMessageType.MessageTypeName))
                                    }).ToList();
                }
                else
                {
                    var details = from edm in dbContext.DocumentMaps.Where(dm => dm.IsDeleted != true && dm.CategoryId == null && dm.CustomerId == Customerid)
                                  join edt in dbContext.DocumentTypes on edm.ExternalDocTypeId equals edt.DocumentTypeId
                                  where (edt.ApplicationId != (int)ApplicationEnum.FAST)
                                  join idt in dbContext.DocumentTypes on edm.DocumentTypeId equals idt.DocumentTypeId
                                  join dmm in dbContext.DocumentMessageMaps on edm.DocumentMapId equals dmm.DocumentMapId
                                  join mm in dbContext.MessageMaps.Where(mmap => !mmap.IsInbound) on dmm.MessageMapId equals mm.MessageMapId
                                  join mt in dbContext.MessageTypes.Where(sl => sl.ApplicationId == Applicationid) on mm.ExternalMessageTypeId equals mt.MessageTypeId
                                  select new OutDocMapping
                                  {
                                      ExternalDocumentType = new DataContracts.DocumentType() { ID = edt.DocumentTypeId, Name = edt.DocumentTypeDesc },
                                      ExternalApplication = new DataContracts.Users() { ID = edt.Application.ApplicationId, Name = edt.Application.ApplicationName },
                                      ExternalMessageType = new DataContracts.MessageType() { MessageTypeId = mt.MessageTypeId, MessageTypeName = mt.MessageTypeName, MessageMapId = mm.MessageMapId },
                                      InternalDocumentType = new DataContracts.DocumentType() { ID = idt.DocumentTypeId, Name = idt.DocumentTypeDesc },
                                      DocumentStatus = new DataContracts.DocumentType()
                                      {
                                          ID = dbContext.TypeCodes.Where(sel => edm.StatusTypeCodeId == sel.TypeCodeId).FirstOrDefault() != null ? dbContext.TypeCodes.Where(sel => edm.StatusTypeCodeId == sel.TypeCodeId).FirstOrDefault().TypeCodeId : 0,
                                          Name = dbContext.TypeCodes.Where(sel => edm.StatusTypeCodeId == sel.TypeCodeId).FirstOrDefault() != null ? dbContext.TypeCodes.Where(sel => edm.StatusTypeCodeId == sel.TypeCodeId).FirstOrDefault().TypeCodeDesc : ""
                                      },
                                      Service = edm.ServiceId != null ?
                                                  new DataContracts.Service()
                                                  {
                                                      ID = dbContext.Services.Where(sel => edm.ServiceId == sel.ServiceId).FirstOrDefault().ServiceId,
                                                      Name = dbContext.Services.Where(sel => edm.ServiceId == sel.ServiceId).FirstOrDefault().ServiceName
                                                  } :
                                                  new DataContracts.Service() { ID = -1, Name = "Any" },
                                      MessageMapId = mm.MessageMapId,
                                      OutboundDocumentMapid = edm.DocumentMapId,
                                      Customerid = Customerid,
                                      Tenant = new DataContracts.Users() { ID = edm.TenantId, Name = edm.Tenant.TenantName }
                                  };

                    outDocuments = (from i in details.ToList()
                                    group i by i.OutboundDocumentMapid into g
                                    select new OutDocMapping
                                    {
                                        ExternalDocumentType = g.Select(kvp => kvp.ExternalDocumentType).FirstOrDefault(),
                                        ExternalApplication = g.Select(kvp => kvp.ExternalApplication).FirstOrDefault(),
                                        ExternalMessageTypeList = g.Select(kvp => kvp.ExternalMessageType).ToList(),
                                        InternalDocumentType = g.Select(kvp => kvp.InternalDocumentType).FirstOrDefault(),
                                        DocumentStatus = g.Select(kvp => kvp.DocumentStatus).FirstOrDefault(),
                                        Service = g.Select(kvp => kvp.Service).FirstOrDefault(),
                                        OutboundDocumentMapid = g.Key,
                                        MessageMapId = g.Select(kvp => kvp.MessageMapId).FirstOrDefault(),
                                        Customerid = Customerid,
                                        Tenant = g.Select(kvp => kvp.Tenant).FirstOrDefault(),
                                        ExternalMessageTypeValue = string.Join(",", g.Select(kvp => kvp.ExternalMessageType.MessageTypeName))
                                    }).ToList();
                }

                //if (tenantId != ((int)TerminalDBEntities.TenantIdEnum.LVIS))
                //{
                //    outDocuments = outDocuments.Where(se => se.Tenant.ID == tenantId).ToList();
                //}
            }

            return outDocuments;
        }

        public OutDocMapping AddDoc(OutDocMapping OutDocMapping, int Tenantid, int userId)
        {
            Entities dbcontext = new Entities();
            DocumentMap DocumentMap = new DocumentMap();
            DocumentMap.IsInbound = false;
            DocumentMap.ExternalDocTypeId = OutDocMapping.ExternalDocumentType.ID;
            DocumentMap.DocumentTypeId = OutDocMapping.InternalDocumentType.ID;
            DocumentMap.ServiceId = OutDocMapping.Service != null ? OutDocMapping.Service.ID : (int?)null;
            DocumentMap.TenantId = Tenantid;
            DocumentMap.CreatedById = userId;
            DocumentMap.CreatedDate = DateTime.Now;
            DocumentMap.LastModifiedById = userId;
            DocumentMap.LastModifiedDate = DateTime.Now;
            DocumentMap.StatusTypeCodeId = OutDocMapping.DocumentStatus.ID;
            DocumentMap.DocumentMapDesc = OutDocMapping.InternalDocumentType.Name;
            if (OutDocMapping.Customerid > 0) { DocumentMap.CustomerId = OutDocMapping.Customerid; }
            if (OutDocMapping.CategoryId > 0) { DocumentMap.CategoryId = OutDocMapping.CategoryId; }
            DocumentMap.DocumentMapName = dbcontext.DocumentTypes.Where(sel => sel.DocumentTypeId == OutDocMapping.InternalDocumentType.ID).Select(sel => sel.DocumentTypeName)?.FirstOrDefault();
            DocumentMap.IsDeleted = false;

            dbcontext.DocumentMaps.Add(DocumentMap);

            int Success = AuditLogHelper.SaveChanges(dbcontext);
            if (Success > 0)
            {
                OutDocMapping.OutboundDocumentMapid = DocumentMap.DocumentMapId;
                OutDocMapping.Tenant = new Users() { ID = Tenantid, Name = dbcontext.Tenants.Single(se => se.TenantId == Tenantid).TenantName };

                foreach (var item in OutDocMapping.ExternalMessageTypeList)
                {
                    DocumentMessageMap DocumentMessageMap = new DocumentMessageMap();
                    DocumentMessageMap.DocumentMapId = DocumentMap.DocumentMapId;
                    DocumentMessageMap.MessageMapId = item.MessageMapId;
                    DocumentMessageMap.Sequence = (short)item.Sequence;
                    DocumentMessageMap.CreatedById = userId;
                    DocumentMessageMap.CreatedDate = DateTime.Now;
                    DocumentMessageMap.LastModifiedById = userId;
                    DocumentMessageMap.LastModifiedDate = DateTime.Now;
                    DocumentMessageMap.TenantId = Tenantid;
                    dbcontext.DocumentMessageMaps.Add(DocumentMessageMap);
                    AuditLogHelper.SaveChanges(dbcontext);
                }

                OutDocMapping.ExternalMessageTypeValue = string.Join(",", OutDocMapping.ExternalMessageTypeList.Select(se => se.MessageTypeName));
            }

            if (OutDocMapping.Service == null)
            {
                OutDocMapping.Service = new DataContracts.Service { ID = -1, Name = "Any" };
            }

            return OutDocMapping;
        }

        public OutDocMapping UpdateDoc(OutDocMapping document, int tenantId, int userid)
        {
            Entities dbcontext = new Entities();
            DocumentMap UpdateDoc = dbcontext.DocumentMaps.Where(se => se.DocumentMapId == document.OutboundDocumentMapid).FirstOrDefault();

            UpdateDoc.IsInbound = false;
            UpdateDoc.ExternalDocTypeId = document.ExternalDocumentType.ID;
            UpdateDoc.DocumentTypeId = document.InternalDocumentType.ID;
            UpdateDoc.ServiceId = document.Service != null ? document.Service.ID : (int?)null;
            UpdateDoc.LastModifiedById = userid;
            UpdateDoc.LastModifiedDate = DateTime.Now;
            UpdateDoc.StatusTypeCodeId = document.DocumentStatus.ID;
            UpdateDoc.DocumentMapDesc = document.InternalDocumentType.Name;
            if (document.Customerid > 0) { UpdateDoc.CustomerId = document.Customerid; }
            if (document.CategoryId > 0) { UpdateDoc.CategoryId = document.CategoryId; }
            UpdateDoc.DocumentMapName = dbcontext.DocumentTypes.Where(sel => sel.DocumentTypeId == document.InternalDocumentType.ID).Select(sel => sel.DocumentTypeName)?.FirstOrDefault();
            UpdateDoc.IsDeleted = false;

            dbcontext.Entry(UpdateDoc).State = System.Data.Entity.EntityState.Modified;

            int Success = AuditLogHelper.SaveChanges(dbcontext);

            if (Success > 0)
            {
                IEnumerable<DocumentMessageMap> MessageMaps = dbcontext.DocumentMessageMaps
                              .RemoveRange(dbcontext.DocumentMessageMaps
                              .Where(se => (se.DocumentMapId == document.OutboundDocumentMapid)));

                AuditLogHelper.SaveChanges(dbcontext);

                foreach (var item in document.ExternalMessageTypeList)
                {
                    DocumentMessageMap DocumentMessageMap = new DocumentMessageMap();
                    DocumentMessageMap.DocumentMapId = UpdateDoc.DocumentMapId;
                    DocumentMessageMap.MessageMapId = item.MessageMapId;
                    DocumentMessageMap.Sequence = (short)item.Sequence;
                    DocumentMessageMap.CreatedById = userid;
                    DocumentMessageMap.CreatedDate = DateTime.Now;
                    DocumentMessageMap.LastModifiedById = userid;
                    DocumentMessageMap.LastModifiedDate = DateTime.Now;
                    DocumentMessageMap.TenantId = UpdateDoc.TenantId;
                    dbcontext.DocumentMessageMaps.Add(DocumentMessageMap);
                    AuditLogHelper.SaveChanges(dbcontext);
                }

                document.ExternalMessageTypeValue = string.Join(",", document.ExternalMessageTypeList.Select(se => se.MessageTypeName));                
            }

            if (document.Service == null)
            {
                document.Service = new DataContracts.Service { ID = -1, Name = "Any" };
            }

            return document;
        }

        public int Delete(int value)
        {
            using (var dbContext = new Entities())
            {
                var documentMapsDelete = (from documentMaps in dbContext.DocumentMaps
                                          where documentMaps.DocumentMapId == value
                                          select documentMaps).FirstOrDefault();

                if (dbContext.DocumentMessageMaps.Where(se => se.DocumentMapId == value).Count() > 0 && documentMapsDelete != null)
                {
                    documentMapsDelete.IsDeleted = (bool)true;
                    dbContext.Entry(documentMapsDelete).State = System.Data.Entity.EntityState.Modified;
                    AuditLogHelper.SaveChanges(dbContext);

                    IEnumerable<DocumentMessageMap> Messagemaps = dbContext.DocumentMessageMaps
                                 .RemoveRange(dbContext.DocumentMessageMaps
                                 .Where(se => (se.DocumentMapId == value)));
                    return AuditLogHelper.SaveChanges(dbContext);
                }
                else if (dbContext.DocumentMessageMaps.Where(se => se.DocumentMapId == value).Count() == 0 && documentMapsDelete != null)
                {
                    documentMapsDelete.IsDeleted = (bool)true;
                    dbContext.Entry(documentMapsDelete).State = System.Data.Entity.EntityState.Modified;
                    return AuditLogHelper.SaveChanges(dbContext);
                }
                else
                { return -1; }
            }
        }

        public IEnumerable<DataContracts.DocumentType> GetStatus()
        {
            using (TerminalDBEntities.Entities dbContext = new TerminalDBEntities.Entities())
            {
                List<DataContracts.DocumentType> Typecodes = dbContext.TypeCodes.Where(se => se.GroupTypeCode == 300).Select(sl => new DataContracts.DocumentType
                {
                    ID = sl.TypeCodeId,
                    Name = sl.TypeCodeDesc
                }).ToList();

                return Typecodes;
            }
        }

        public IEnumerable<DataContracts.MessageType> GetOutboundMessageTypes(int categoryId, int applicationId, int tenantId)
        {
            List<DataContracts.MessageType> MessageTypes = new List<DataContracts.MessageType>();

            using (var dbContext = new TerminalDBEntities.Entities())
            {
                if (tenantId != ((int)TerminalDBEntities.TenantIdEnum.LVIS))
                {

                    var details = from sub in dbContext.Subscriptions.Where(subs => subs.CategoryId == categoryId && subs.TenantId == tenantId && subs.CustomerId == null)
                                  join mt in dbContext.MessageTypes.Where(sl => sl.ApplicationId == applicationId) on sub.MessageTypeId equals mt.MessageTypeId
                                  join mm in dbContext.MessageMaps.Where(mmap => !mmap.IsInbound && mmap.TenantId == tenantId) on mt.MessageTypeId equals mm.ExternalMessageTypeId
                                  select new DataContracts.MessageType
                                  {
                                      MessageTypeId = mt.MessageTypeId,
                                      MessageTypeName = mt.MessageTypeName,
                                      MessageMapId = mm.MessageMapId,
                                  };

                    MessageTypes = details.ToList();
                }
                else
                {
                    var details = from sub in dbContext.Subscriptions.Where(subs => subs.CategoryId == categoryId && subs.CustomerId == null)
                                  join mt in dbContext.MessageTypes.Where(sl => sl.ApplicationId == applicationId) on sub.MessageTypeId equals mt.MessageTypeId
                                  join mm in dbContext.MessageMaps.Where(mmap => !mmap.IsInbound) on mt.MessageTypeId equals mm.ExternalMessageTypeId
                                  select new DataContracts.MessageType
                                  {
                                      MessageTypeId = mt.MessageTypeId,
                                      MessageTypeName = mt.MessageTypeName,
                                      MessageMapId = mm.MessageMapId,
                                  };
                    MessageTypes = details.ToList();
                }
            }

            MessageTypes = MessageTypes.GroupBy(x => x.MessageTypeId).Select(x => x.First()).OrderBy(x => x.MessageTypeName).ToList();

            return MessageTypes;
        }

        public IEnumerable<DataContracts.MessageType> GetOutboundMessageTypesbyCustomer(int customerid, int applicationId, int tenantId)
        {
            List<DataContracts.MessageType> MessageTypes = new List<DataContracts.MessageType>();

            var categoryId = GetCustomersCategory(customerid);

            using (var dbContext = new TerminalDBEntities.Entities())
            {
                if (tenantId != ((int)TerminalDBEntities.TenantIdEnum.LVIS))
                {
                    var details = from sub in dbContext.Subscriptions.Where(subs => subs.TenantId == tenantId && subs.CustomerId == customerid)
                                      join mt in dbContext.MessageTypes.Where(sl => sl.ApplicationId == applicationId) on sub.MessageTypeId equals mt.MessageTypeId
                                      join mm in dbContext.MessageMaps.Where(mmap => !mmap.IsInbound && mmap.TenantId == tenantId) on mt.MessageTypeId equals mm.ExternalMessageTypeId
                                      select new DataContracts.MessageType
                                      {
                                          MessageTypeId = mt.MessageTypeId,
                                          MessageTypeName = mt.MessageTypeName,
                                          MessageMapId = mm.MessageMapId
                                      };
                    MessageTypes = details.ToList();

                    if (MessageTypes.Count == 0 && categoryId > 0)
                    {
                        details = from sub in dbContext.Subscriptions.Where(subs => subs.TenantId == tenantId && subs.CategoryId == categoryId)
                                      join mt in dbContext.MessageTypes.Where(sl => sl.ApplicationId == applicationId) on sub.MessageTypeId equals mt.MessageTypeId
                                      join mm in dbContext.MessageMaps.Where(mmap => !mmap.IsInbound && mmap.TenantId == tenantId) on mt.MessageTypeId equals mm.ExternalMessageTypeId
                                      select new DataContracts.MessageType
                                      {
                                          MessageTypeId = mt.MessageTypeId,
                                          MessageTypeName = mt.MessageTypeName,
                                          MessageMapId = mm.MessageMapId
                                      };
                        MessageTypes = details.ToList();
                    }
                }
                else
                {
                    var details = from sub in dbContext.Subscriptions.Where(subs => subs.CustomerId == customerid)
                                  join mt in dbContext.MessageTypes.Where(sl => sl.ApplicationId == applicationId) on sub.MessageTypeId equals mt.MessageTypeId
                                  join mm in dbContext.MessageMaps.Where(mmap => !mmap.IsInbound) on mt.MessageTypeId equals mm.ExternalMessageTypeId
                                  select new DataContracts.MessageType
                                  {
                                      MessageTypeId = mt.MessageTypeId,
                                      MessageTypeName = mt.MessageTypeName,
                                      MessageMapId = mm.MessageMapId
                                  };


                    MessageTypes = details.ToList();

                    if (MessageTypes.Count == 0 && categoryId > 0)
                    {
                        details = from sub in dbContext.Subscriptions.Where(subs => subs.CategoryId == categoryId)
                                  join mt in dbContext.MessageTypes.Where(sl => sl.ApplicationId == applicationId) on sub.MessageTypeId equals mt.MessageTypeId
                                  join mm in dbContext.MessageMaps.Where(mmap => !mmap.IsInbound) on mt.MessageTypeId equals mm.ExternalMessageTypeId
                                  select new DataContracts.MessageType
                                  {
                                      MessageTypeId = mt.MessageTypeId,
                                      MessageTypeName = mt.MessageTypeName,
                                      MessageMapId = mm.MessageMapId
                                  };
                        MessageTypes = details.ToList();
                    }
                }
            }

            MessageTypes = MessageTypes.GroupBy(x => x.MessageTypeId).Select(x => x.First()).OrderBy(x => x.MessageTypeName).ToList();
            return MessageTypes;
        }

        private int GetCustomersCategory(int customerId)
        {
            using (var dbContext = new Entities())
            {
                var cat = dbContext.Customers.Where(sel => sel.CustomerId == customerId)?.Select(cust => cust.CategoryId)?.FirstOrDefault();
                return cat.HasValue ? cat.Value : 0;
            }
        }
    }
}
