using System.Collections.Generic;

using DC = FA.LVIS.Tower.DataContracts;
using FA.LVIS.Tower.Data;

namespace FA.LVIS.Tower.Services
{
    public class OutEventMappingService : Core.ServiceBase, IOutEventMappingService
    {

        public List<DC.OutEventMapping> GetLVISOutEvents()
        {
            IOutEventMappingDataProvider eventProvider = DataProviderFactory.Resolve<IOutEventMappingDataProvider>();
            return eventProvider.GetLVISOutEvents();
        }
        public List<DC.OutEventMapping> GetLVISLenderOutEvents(string lenderABEID)
        {
            IOutEventMappingDataProvider eventProvider = DataProviderFactory.Resolve<IOutEventMappingDataProvider>();
            return eventProvider.GetLVISLenderOutEvents(lenderABEID);
        }
    }
}
