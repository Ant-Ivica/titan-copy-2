using FA.LVIS.Tower.Core;
using System.Collections.Generic;
using DC = FA.LVIS.Tower.DataContracts;

namespace FA.LVIS.Tower.Data
{
   public interface IFASTProcessTriggerDataProvider :IDataProviderBase
    {
        List<DC.FASTProcessTriggerDTO> GetFastWorkFlowDetails(int tenantId);
        List<DC.MessageType> GetMessageTypeDetails();
        DC.FASTProcessTriggerDTO UpdateFastWorkflow(DC.FASTProcessTriggerDTO workflow, int tenantId, int userId);
        DC.FASTProcessTriggerDTO AddFastWorkflow(DC.FASTProcessTriggerDTO workflow, int tenantId, int userId);
        int Delete(int id);
        int ConfirmDelete(int id);
        List<DC.Service> GetServiceTypeDetails();


    }
}
