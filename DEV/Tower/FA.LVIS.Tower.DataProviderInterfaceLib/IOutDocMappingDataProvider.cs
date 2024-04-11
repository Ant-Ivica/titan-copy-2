using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FA.LVIS.Tower.Core;
using DC = FA.LVIS.Tower.DataContracts;

namespace FA.LVIS.Tower.Data
{
    public interface IOutDocMappingDataProvider: IDataProviderBase
    {
        List<DC.OutDocMapping> GetOutboundDocumentsByCategory(int categoryId, int tenantId, int extApplicationId);

        List<DC.OutDocMapping> GetLVISLenderOutboundDocuments(int Customerid, int Applicationid, int iTenantid);

        DC.OutDocMapping AddDoc(DC.OutDocMapping doc, int Tenantid, int userId);

        DC.OutDocMapping UpdateDoc(DC.OutDocMapping doc, int tenantId, int userid);

       int Delete(int value);

        IEnumerable<DC.DocumentType> GetStatus();

        IEnumerable<DC.MessageType> GetOutboundMessageTypes(int categoryId, int applicationId, int tenantId);

        IEnumerable<DC.MessageType> GetOutboundMessageTypesbyCustomer(int customerid, int applicationId, int tenantId);

    }
}
