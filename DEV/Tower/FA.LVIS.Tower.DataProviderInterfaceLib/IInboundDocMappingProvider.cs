using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DC = FA.LVIS.Tower.DataContracts;

namespace FA.LVIS.Tower.Data
{
    public  interface IInboundDocMappingProvider : Core.IDataProviderBase
    {
        List<DC.InboundDocumentMapDTO> GetLvisInboundDocMaps(int tenantId);
        IEnumerable<DC.DocumentType> GetDocTypes(int applicationId);
        IEnumerable<DataContracts.Service> GetServices(int iTenantid);
        DC.InboundDocumentMapDTO UpdateDoc(DC.InboundDocumentMapDTO doc, int tenantId, int userId);
        DC.InboundDocumentMapDTO AddDoc(DC.InboundDocumentMapDTO doc, int tenantId, int userId);

        int DeleteDocument(int value, int userId);

    }
}
