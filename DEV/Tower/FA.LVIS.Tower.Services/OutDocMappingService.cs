using System.Collections.Generic;

using DC = FA.LVIS.Tower.DataContracts;
using FA.LVIS.Tower.Data;

namespace FA.LVIS.Tower.Services
{
    public class OutDocMappingService : Core.ServiceBase, IOutDocMappingService
    {
        public List<DC.OutDocMapping> GetOutboundDocumentsByCategory(int categoryId, int tenantId, int extApplicationId)
        {
            IOutDocMappingDataProvider outDocProvider = DataProviderFactory.Resolve<IOutDocMappingDataProvider>();
            return outDocProvider.GetOutboundDocumentsByCategory(categoryId, tenantId, extApplicationId);
        }

        public List<DC.OutDocMapping> GetLVISLenderOutboundDocuments(int Customerid, int Applicationid, int iTenantid)
        {
            IOutDocMappingDataProvider outDocProvider = DataProviderFactory.Resolve<IOutDocMappingDataProvider>();

             return outDocProvider.GetLVISLenderOutboundDocuments(Customerid, Applicationid, iTenantid);
        }

        public DC.OutDocMapping AddDoc(DC.OutDocMapping doc, int Tenantid, int userId)
        {
            IOutDocMappingDataProvider outDocProvider = DataProviderFactory.Resolve<IOutDocMappingDataProvider>();

            return outDocProvider.AddDoc(doc,Tenantid,userId);
        }

        public DC.OutDocMapping UpdateDoc(DC.OutDocMapping doc, int tenantId, int userid)
        {
            IOutDocMappingDataProvider outDocProvider = DataProviderFactory.Resolve<IOutDocMappingDataProvider>();

            return outDocProvider.UpdateDoc(doc,  tenantId,  userid);
        }

        public int Delete(int value)
        {
            IOutDocMappingDataProvider outDocProvider = DataProviderFactory.Resolve<IOutDocMappingDataProvider>();

            return outDocProvider.Delete(value);
        }

        public IEnumerable<DC.DocumentType> GetStatus()
        {
            IOutDocMappingDataProvider outDocProvider = DataProviderFactory.Resolve<IOutDocMappingDataProvider>();

            return outDocProvider.GetStatus();
        }

        public IEnumerable<DC.MessageType> GetOutboundMessageTypes(int categoryId, int applicationId, int tenantId)
        {
            IOutDocMappingDataProvider OutDocMappingDataProvider = DataProviderFactory.Resolve<IOutDocMappingDataProvider>();

            return OutDocMappingDataProvider.GetOutboundMessageTypes(categoryId, applicationId, tenantId);
        }

        public IEnumerable<DC.MessageType> GetOutboundMessageTypesbyCustomer(int customerid, int applicationId, int tenantId)
        {
            IOutDocMappingDataProvider OutDocMappingDataProvider = DataProviderFactory.Resolve<IOutDocMappingDataProvider>();

            return OutDocMappingDataProvider.GetOutboundMessageTypesbyCustomer(customerid, applicationId, tenantId);
        }
    }
}
