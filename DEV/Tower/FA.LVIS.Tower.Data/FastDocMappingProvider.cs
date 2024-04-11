using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FA.LVIS.Tower.DataContracts;
using FA.LVIS.Tower.Data.TerminalDBEntities;

namespace FA.LVIS.Tower.Data
{
    class FastDocMappingProvider : Core.DataProviderBase, IFastDocMappingProvider
    {
        public InboundDocumentMapDTO AddDoc(InboundDocumentMapDTO doc, int tenantId, int userId)
        {
            Entities dbcontext = new Entities();
            TerminalDBEntities.DocumentMap addDoc = new TerminalDBEntities.DocumentMap();
            addDoc.IsInbound = false;
            addDoc.ExternalDocTypeId = doc.ExternalDocumentType;
            addDoc.DocumentTypeId = doc.InternalDocumentType;
            addDoc.ServiceId = doc.Service;
            addDoc.TenantId = tenantId;
            addDoc.CreatedById = userId;
            addDoc.CreatedDate = DateTime.Now;
            addDoc.LastModifiedById = userId;
            addDoc.LastModifiedDate = DateTime.Now;
            addDoc.DocumentMapDesc = doc.ExternalDocumentDescription;

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
                ServiceName = Mappings.ServiceName,
                TenantId = Mappings.tenantId,
                Tenant = Mappings.Tenantname
            };
        }

        public List<InboundDocumentMapDTO> GetFASTToLVISDocs(int tenantId)
        {
            //getting lender Data from PID MAPPING Table
            Entities dbContext = new Entities();
            List<DataContracts.InboundDocumentMapDTO> DocMappings = new List<DataContracts.InboundDocumentMapDTO>();

            if (dbContext.DocumentMaps.Where(se => se.IsInbound).Count() > 0)
            {
                var Mappings = dbContext.DocumentMaps
                  .Where(se => se.IsInbound && se.DocumentType1.ApplicationId ==(int)ApplicationEnum.FAST && se.IsDeleted != true)
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
                      tenantId =Map.TenantId,
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
                    ServiceName = se.ServiceName != null ? se.ServiceName : "Any",
                    TenantId =se.tenantId,
                    Tenant =se.Tenantname
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
                 .Where(x => x.ServiceId == 2 || x.ServiceId == 3 || x.ServiceId == 4)
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
            UpdateDoc.IsInbound = false;
            UpdateDoc.ExternalDocTypeId = doc.ExternalDocumentType;
            UpdateDoc.DocumentTypeId = doc.InternalDocumentType;
            UpdateDoc.ServiceId = doc.Service;
            //UpdateDoc.TenantId = tenantId;
            UpdateDoc.LastModifiedById = userId;
            UpdateDoc.LastModifiedDate = DateTime.Now;
            UpdateDoc.DocumentMapDesc = doc.ExternalDocumentDescription;
            dbcontext.Entry(UpdateDoc).State = System.Data.Entity.EntityState.Modified;

            int Success = AuditLogHelper.SaveChanges(dbcontext);

            return GetDocument(UpdateDoc.DocumentMapId);
        }

        public List<InboundDocumentMapDTO> GetLVISToFastDocs(int tenantId)
        {
            //getting lender Data from PID MAPPING Table
            Entities dbContext = new Entities();
            List<DataContracts.InboundDocumentMapDTO> DocMappings = new List<DataContracts.InboundDocumentMapDTO>();

            if (dbContext.DocumentMaps.Where(se => !se.IsInbound).Count() > 0)
            {
                var Mappings = dbContext.DocumentMaps
                  .Where(se => !se.IsInbound && se.DocumentType1.ApplicationId == (int)ApplicationEnum.FAST && se.IsDeleted != true)
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
    }
}
