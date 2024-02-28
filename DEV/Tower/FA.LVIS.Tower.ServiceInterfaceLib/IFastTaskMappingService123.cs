using System.Collections.Generic;
using DC = FA.LVIS.Tower.DataContracts;

namespace FA.LVIS.Tower.Services
{
  public  interface IFastTaskMappingService123 :Core.IServiceBase
    {
        List<DC.FastTaskMapDTO> GetFastTaskDetails(int TenantId);
    }
}
