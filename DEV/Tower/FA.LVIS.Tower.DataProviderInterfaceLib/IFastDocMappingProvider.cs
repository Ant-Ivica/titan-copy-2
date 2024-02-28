using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DC = FA.LVIS.Tower.DataContracts;

namespace FA.LVIS.Tower.Data
{
    public  interface IFastDocMappingProvider : Core.IDataProviderBase
    {
        List<DC.InboundDocumentMapDTO> GetFASTToLVISDocs(int tenantId);
        IEnumerable<DataContracts.Service> GetServices(int iTenantid);
        DC.InboundDocumentMapDTO UpdateDoc(DC.InboundDocumentMapDTO doc, int tenantId, int userId);
        DC.InboundDocumentMapDTO AddDoc(DC.InboundDocumentMapDTO doc, int tenantId, int userId);
        List<DC.InboundDocumentMapDTO> GetLVISToFastDocs(int tenantId);
    }
}
