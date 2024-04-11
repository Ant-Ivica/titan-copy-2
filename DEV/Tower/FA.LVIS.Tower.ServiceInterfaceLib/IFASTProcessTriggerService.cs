using System.Collections.Generic;
using DC = FA.LVIS.Tower.DataContracts;

namespace FA.LVIS.Tower.Services
{
   public interface IFASTProcessTriggerService : Core.IServiceBase
    {
        List<DC.FASTProcessTriggerDTO> GetFastWorkFlowDetails(int TenantId);
        List<DC.MessageType> GetMessageTypeDetails();
        DC.FASTProcessTriggerDTO UpdateFastWorkflow(DC.FASTProcessTriggerDTO Fastworkflowmap, int TenantId, int userid);

        DC.FASTProcessTriggerDTO AddFastWorkflow(DC.FASTProcessTriggerDTO Fastworkflowmap, int TenantId, int userid);
        int Delete(int id);

        int ConfirmDelete(int id);

        List<DC.Service> GetServiceTypeDetails();

    }
}
