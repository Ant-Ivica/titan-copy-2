using System.Collections.Generic;
using DC = FA.LVIS.Tower.DataContracts;

namespace FA.LVIS.Tower.Services
{
    public interface IFASTDocMappingService : Core.IServiceBase
    {
        List<DC.InboundDocumentMapDTO> GetFASTToLVISDocs(int tenantId);
        IEnumerable<DataContracts.Service> GetServices(int iTenantid);
        DC.InboundDocumentMapDTO AddDoc(DC.InboundDocumentMapDTO doc, int tenantId, int userId);
        DC.InboundDocumentMapDTO UpdateDoc(DC.InboundDocumentMapDTO doc, int tenantId, int userId);
        List<DC.InboundDocumentMapDTO> GetLVISToFastDocs(int tenantId);
    }
}