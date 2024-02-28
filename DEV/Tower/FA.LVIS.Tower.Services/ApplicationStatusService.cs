using System.Collections.Generic;

using DC = FA.LVIS.Tower.DataContracts;
using FA.LVIS.Tower.Data;

namespace FA.LVIS.Tower.Services
{
    public class ApplicationStatusService : Core.ServiceBase, IApplicationStatusService
    {

        public List<DC.EMSQueue> GetApplicationStatus()
        {
            IApplicationStatusDataProvider AppStatusProvider = DataProviderFactory.Resolve<IApplicationStatusDataProvider>();
            return AppStatusProvider.GetApplicationStatus();
        }
    }
    
}
