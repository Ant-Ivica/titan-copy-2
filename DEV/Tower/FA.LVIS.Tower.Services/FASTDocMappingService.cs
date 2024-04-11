using System.Collections.Generic;

using DC = FA.LVIS.Tower.DataContracts;
using FA.LVIS.Tower.Data;
using System;

namespace FA.LVIS.Tower.Services
{
    class FASTDocMappingService : Core.ServiceBase, IFASTDocMappingService
    {
        public DC.InboundDocumentMapDTO AddDoc(DC.InboundDocumentMapDTO doc, int tenantId, int userId)
        {
            IFastDocMappingProvider InDocProvider = DataProviderFactory.Resolve<IFastDocMappingProvider>();

            return InDocProvider.AddDoc(doc,tenantId,userId);
        }


        public List<DC.InboundDocumentMapDTO> GetFASTToLVISDocs(int tenantId)
        {
            IFastDocMappingProvider InDocProvider = DataProviderFactory.Resolve<IFastDocMappingProvider>();

            return InDocProvider.GetFASTToLVISDocs(tenantId);
        }

        public List<DC.InboundDocumentMapDTO> GetLVISToFastDocs(int tenantId)
        {
            IFastDocMappingProvider OutDocProvider = DataProviderFactory.Resolve<IFastDocMappingProvider>();

            return OutDocProvider.GetLVISToFastDocs(tenantId);
        }

        public IEnumerable<DataContracts.Service> GetServices(int iTenantid)
        {
            IFastDocMappingProvider InDocProvider = DataProviderFactory.Resolve<IFastDocMappingProvider>();

            return InDocProvider.GetServices(iTenantid);

        }

        public DC.InboundDocumentMapDTO UpdateDoc(DC.InboundDocumentMapDTO doc, int tenantId, int userId)
        {
            IFastDocMappingProvider InDocProvider = DataProviderFactory.Resolve<IFastDocMappingProvider>();

            return InDocProvider.UpdateDoc(doc, tenantId, userId);
        }
    }
}
