using System.Collections.Generic;

using DC = FA.LVIS.Tower.DataContracts;
using FA.LVIS.Tower.Data;
using System;

namespace FA.LVIS.Tower.Services
{
    class InboundDocMappingService : Core.ServiceBase, IInboundDocMappingService
    {
        public DC.InboundDocumentMapDTO AddDoc(DC.InboundDocumentMapDTO doc, int tenantId, int userId)
        {
            IInboundDocMappingProvider InDocProvider = DataProviderFactory.Resolve<IInboundDocMappingProvider>();

            return InDocProvider.AddDoc(doc,tenantId,userId);
        }

        public int DeleteDocument(int value, int userId)
        {
            IInboundDocMappingProvider InDocProvider = DataProviderFactory.Resolve<IInboundDocMappingProvider>();
            return InDocProvider.DeleteDocument(value, userId);
        }

        public IEnumerable<DC.DocumentType> GetDocTypes(int applicationId)
        {
            IInboundDocMappingProvider InDocProvider = DataProviderFactory.Resolve<IInboundDocMappingProvider>();

            return InDocProvider.GetDocTypes(applicationId);
        }

        public List<DC.InboundDocumentMapDTO> GetLvisInboundDocMaps(int tenantId)
        {
            IInboundDocMappingProvider InDocProvider = DataProviderFactory.Resolve<IInboundDocMappingProvider>();

            return InDocProvider.GetLvisInboundDocMaps(tenantId);

        }

        public IEnumerable<DataContracts.Service> GetServices(int iTenantid)
        {
            IInboundDocMappingProvider InDocProvider = DataProviderFactory.Resolve<IInboundDocMappingProvider>();

            return InDocProvider.GetServices(iTenantid);

        }

        public DC.InboundDocumentMapDTO UpdateDoc(DC.InboundDocumentMapDTO doc, int tenantId, int userId)
        {
            IInboundDocMappingProvider InDocProvider = DataProviderFactory.Resolve<IInboundDocMappingProvider>();

            return InDocProvider.UpdateDoc(doc, tenantId, userId);
        }
    }
}
