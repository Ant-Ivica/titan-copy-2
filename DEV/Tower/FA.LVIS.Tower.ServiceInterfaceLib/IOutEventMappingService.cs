using System.Collections.Generic;

using DC = FA.LVIS.Tower.DataContracts;
namespace FA.LVIS.Tower.Services
{
    public interface IOutEventMappingService:Core.IServiceBase
    {
        List<DC.OutEventMapping> GetLVISOutEvents();

        List<DC.OutEventMapping> GetLVISLenderOutEvents(string lenderABEID);
    }
}
