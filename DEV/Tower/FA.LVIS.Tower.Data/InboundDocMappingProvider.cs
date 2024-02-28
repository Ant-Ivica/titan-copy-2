using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FA.LVIS.Tower.DataContracts;
using FA.LVIS.Tower.Data.TerminalDBEntities;
using LVIS.Common;

namespace FA.LVIS.Tower.Data
{
    class InboundDocMappingProvider : Core.DataProviderBase, IInboundDocMappingProvider
    {
        public object TerminalEnumTypes { get; private set; }
        IUtils Utils;

        public InboundDocMappingProvider()
        {
            Utils = new Utils();
        }

        public InboundDocumentMapDTO AddDoc(InboundDocumentMapDTO doc, int tenantId, int userId)
        {
            Entities dbcontext = new Entities();
            TerminalDBEntities.DocumentMap addDoc = new TerminalDBEntities.DocumentMap();
            addDoc.IsInbound = true;
            if (doc.ExternalDocumentType == 0)
                addDoc.ExternalDocTypeId = 1000;
            else 
                addDoc.ExternalDocTypeId = doc.ExternalDocumentType;
            addDoc.DocumentTypeId = doc.InternalDocumentType;
            if (doc.Service == 0)
                addDoc.ServiceId = null;
            else
            addDoc.ServiceId = doc.Service;
            addDoc.TenantId = tenantId;
            addDoc.CreatedById = userId;
            addDoc.CreatedDate = DateTime.Now;
            addDoc.LastModifiedById = userId;
            addDoc.LastModifiedDate = DateTime.Now;
            addDoc.DocumentMapDesc = doc.ExternalDocumentDescription == null ? string.Empty : doc.ExternalDocumentDescription;
            addDoc.DocumentMapName = Utils.ComputeName(doc.ExternalDocumentDescription == null ? string.Empty : doc.ExternalDocumentDescription);
            dbcontext.DocumentMaps.Add(addDoc);

            int Success = AuditLogHelper.SaveChanges(dbcontext);

            if (Success > 0)
                return GetDocument(addDoc.DocumentMapId);
            else
                return doc;

        }

        private InboundDocumentMapDTO GetDocument(int DocumentMapid)
        {
            Entities dbContext = new Entities();

            var Mappings = dbContext.DocumentMaps
                 .Where(se => se.DocumentMapId == DocumentMapid && se.IsDeleted != true)
                 .Select(Map => new
                 {
                     inboundDocumentMapid = Map.DocumentMapId,
                     InternalDocumentType = Map.DocumentTypeId,
                     InternalDocumentTypeName = Map.DocumentType.DocumentTypeName,
                     InternalDocumentTypeDesc = Map.DocumentType.DocumentTypeDesc,
                     service = Map.ServiceId,
                     ServiceName = Map.Service.ServiceName,
                     ExternalDocumentType = dbContext.DocumentTypes.Where(se => se.DocumentTypeId == Map.ExternalDocTypeId).FirstOrDefault(),
                     ExternalDocumentTypeDesc = dbContext.DocumentTypes.Where(se => se.DocumentTypeId == Map.ExternalDocTypeId).FirstOrDefault().DocumentTypeDesc,
                     ExternalDocumentTypeId = Map.ExternalDocTypeId,
                     tenantId = Map.TenantId,
                     Tenantname = Map.Tenant.TenantName,
                     ExternalDocumentDescription = Map.DocumentMapDesc
                 }).FirstOrDefault();

            return new DataContracts.InboundDocumentMapDTO()
            {
                ExternalApplication = Mappings.ExternalDocumentType != null ? Mappings.ExternalDocumentType.ApplicationId : 0,
                ExternalApplicationName = Mappings.ExternalDocumentType != null ? Mappings.ExternalDocumentType.Application.ApplicationName : "",
                ExternalDocumentType = Mappings.ExternalDocumentTypeId,
                ExternalDocumentTypeName = Mappings.ExternalDocumentType != null ? Mappings.ExternalDocumentType.DocumentTypeName : "",
                ExternalDocumentDescription = Mappings.ExternalDocumentDescription,
                ExternalDocumentTypeDesc = Mappings.ExternalDocumentTypeDesc,
                inboundDocumentMapid = Mappings.inboundDocumentMapid,
                InternalDocumentType = Mappings.InternalDocumentType,
                InternalDocumentTypeName = Mappings.InternalDocumentTypeName,
                InternalDocumentTypeDesc = Mappings.InternalDocumentTypeDesc,
                Service = (Mappings.service.HasValue == true ? Mappings.service.Value : 0),
                ServiceName = Mappings.ServiceName != null ? Mappings.ServiceName : "Any",
                TenantId = Mappings.tenantId,
                Tenant = Mappings.Tenantname
            };
        }

        public IEnumerable<DataContracts.DocumentType> GetDocTypes(int applicationId)
        {
            Entities dbContext = new Entities();
            return dbContext.DocumentTypes.Where(se => se.ApplicationId == applicationId)
                 .Select(sl => new DataContracts.DocumentType()
                 {
                     ID = sl.DocumentTypeId,
                     Name = sl.DocumentTypeDesc
                 }).OrderBy(x => x.Name);
        }

        public List<InboundDocumentMapDTO> GetLvisInboundDocMaps(int tenantId)
        {
            //getting lender Data from PID MAPPING Table
            Entities dbContext = new Entities();
            List<DataContracts.InboundDocumentMapDTO> DocMappings = new List<DataContracts.InboundDocumentMapDTO>();

            if (dbContext.DocumentMaps.Where(se => se.IsInbound && se.DocumentType1.ApplicationId != (int)ApplicationEnum.FAST && se.IsDeleted != true).Count() > 0)
            {
                var Mappings = dbContext.DocumentMaps
                  .Where(se => se.IsInbound && se.DocumentType1.ApplicationId != (int)ApplicationEnum.FAST && se.IsDeleted != true)
                  .Select(Map => new
                  {
                      inboundDocumentMapid = Map.DocumentMapId,
                      InternalDocumentType = Map.DocumentTypeId,
                      InternalDocumentTypeName = Map.DocumentType.DocumentTypeName,
                      InternalDocumentTypeDesc = Map.DocumentType.DocumentTypeDesc,
                      service = Map.ServiceId.HasValue ?Map.ServiceId:0,
                      ServiceName =Map.Service != null? Map.Service.ServiceName:string.Empty,
                      ExternalDocumentType = dbContext.DocumentTypes.Where(se => se.DocumentTypeId == Map.ExternalDocTypeId).FirstOrDefault(),
                      ExternalDocumentTypeDesc = dbContext.DocumentTypes.Where(se => se.DocumentTypeId == Map.ExternalDocTypeId).FirstOrDefault().DocumentTypeDesc,
                      ExternalDocumentTypeId = Map.ExternalDocTypeId,
                      tenantId = Map.TenantId,
                      Tenantname = Map.Tenant.TenantName,
                      ExternalDocumentDescription = Map.DocumentMapDesc
                  });

                DocMappings = Mappings.Select(se => new DataContracts.InboundDocumentMapDTO()
                {
                    ExternalApplication = se.ExternalDocumentType != null ? se.ExternalDocumentType.ApplicationId : 0,
                    ExternalApplicationName = se.ExternalDocumentType != null ? se.ExternalDocumentType.Application.ApplicationName : "",
                    ExternalDocumentType = se.ExternalDocumentTypeId,
                    ExternalDocumentTypeName = se.ExternalDocumentType != null ? se.ExternalDocumentType.DocumentTypeName : "",
                    ExternalDocumentDescription = se.ExternalDocumentDescription,
                    ExternalDocumentTypeDesc = se.ExternalDocumentTypeDesc,
                    inboundDocumentMapid = se.inboundDocumentMapid,
                    InternalDocumentType = se.InternalDocumentType,
                    InternalDocumentTypeName = se.InternalDocumentTypeName,
                    InternalDocumentTypeDesc = se.InternalDocumentTypeDesc,
                    Service = (se.service.HasValue == true ? se.service.Value : 0),
                    ServiceName = se.ServiceName,
                    TenantId = se.tenantId,
                    Tenant = se.Tenantname
                }).ToList();

            }

            if (DocMappings.Count() > 0 && tenantId != (int)TenantIdEnum.LVIS)
            {
                DocMappings = DocMappings
                    .Where(sel => sel.TenantId == tenantId).ToList();
            }

            return DocMappings;
        }

        public IEnumerable<DataContracts.Service> GetServices(int iTenantid)
        {
            Entities dbContext = new Entities();
            return dbContext.Services
                 .Where(x => x.ServiceId == 2 || x.ServiceId == 3 || x.ServiceId == 4).OrderBy(s => s.ServiceName)
                 .Select(sl => new DataContracts.Service()
                 {
                     ID = sl.ServiceId,
                     Name = sl.ServiceName
                 });
        }

        public InboundDocumentMapDTO UpdateDoc(InboundDocumentMapDTO doc, int tenantId, int userId)
        {
            Entities dbcontext = new Entities();
            TerminalDBEntities.DocumentMap UpdateDoc = dbcontext.DocumentMaps.Where(se => se.DocumentMapId == doc.inboundDocumentMapid).FirstOrDefault();
            UpdateDoc.IsInbound = true;

            if (doc.ExternalDocumentType == 0)
                UpdateDoc.ExternalDocTypeId = 1000;
            else
                UpdateDoc.ExternalDocTypeId = doc.ExternalDocumentType;

            UpdateDoc.DocumentTypeId = doc.InternalDocumentType;
            if (doc.Service == 0)
                UpdateDoc.ServiceId = null;
            else
              UpdateDoc.ServiceId = doc.Service;
            //UpdateDoc.TenantId = tenantId;
            UpdateDoc.LastModifiedById = userId;
            UpdateDoc.LastModifiedDate = DateTime.Now;
            UpdateDoc.DocumentMapDesc = doc.ExternalDocumentDescription == null?string.Empty:doc.ExternalDocumentDescription;
            UpdateDoc.DocumentMapName = Utils.ComputeName(doc.ExternalDocumentDescription == null ? string.Empty : doc.ExternalDocumentDescription);

            dbcontext.Entry(UpdateDoc).State = System.Data.Entity.EntityState.Modified;

            int Success = AuditLogHelper.SaveChanges(dbcontext);
            return GetDocument(UpdateDoc.DocumentMapId);
        }

        public int DeleteDocument(int value, int userId)
        {
            using (var dbContext = new Entities())
            {
                var docDelete = (from doc in dbContext.DocumentMaps
                                 where doc.DocumentMapId == value
                                 select doc).FirstOrDefault();
                if (docDelete != null)
                {
                    docDelete.IsDeleted = (bool)true;
                    docDelete.LastModifiedById = userId;
                    docDelete.LastModifiedDate = DateTime.Now;

                    dbContext.Entry(docDelete).State = System.Data.Entity.EntityState.Modified;
                    return AuditLogHelper.SaveChanges(dbContext);
                }
                else
                { return -1; }
            }
        }
    }
}
