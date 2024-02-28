using System.Collections.Generic;

using DC = FA.LVIS.Tower.DataContracts;
namespace FA.LVIS.Tower.Services
{
    public interface IApplicationStatusService:Core.IServiceBase
    {
        List<DC.EMSQueue> GetApplicationStatus();
        
    }
}
