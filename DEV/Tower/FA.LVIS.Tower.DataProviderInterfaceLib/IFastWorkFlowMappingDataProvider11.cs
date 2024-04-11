using FA.LVIS.Tower.Core;
using System.Collections.Generic;
using DC = FA.LVIS.Tower.DataContracts;

namespace FA.LVIS.Tower.Data
{
   public  interface IFastWorkFlowMappingDataProvider11 : IDataProviderBase
    {
        List<DC.FASTWorkFlowMapDTO> GetFastWorkFlowDetails(int tenantId);
    }
}
