using FA.LVIS.Tower.Data;
using FA.LVIS.Tower.DataContracts;
using System.Collections.Generic;
using System;

namespace FA.LVIS.Tower.Services
{
    public class FastTaskMappingServie123 : Core.IServiceBase, IFastTaskMappingService123
    {
        public List<FastTaskMapDTO> GetFastTaskDetails(int TenantId)
        {
            IFastTaskMappingDataProvider FastTaskProvider = DataProviderFactory.Resolve<IFastTaskMappingDataProvider>();
            return FastTaskProvider.GetFastTaskDetails(TenantId);
        }
    }
}
