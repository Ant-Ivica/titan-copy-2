using FA.LVIS.Tower.Data;
using FA.LVIS.Tower.DataContracts;
using System.Collections.Generic;
using System;

namespace FA.LVIS.Tower.Services
{
    public class FASTProcessTriggerService : Core.ServiceBase, IFASTProcessTriggerService
    {
        public FASTProcessTriggerDTO AddFastWorkflow(FASTProcessTriggerDTO Fastworkflowmap, int TenantId, int userid)
        {
            IFASTProcessTriggerDataProvider workflowProvider = DataProviderFactory.Resolve<IFASTProcessTriggerDataProvider>();
            return workflowProvider.AddFastWorkflow(Fastworkflowmap, TenantId, userid);
        }
        public FASTProcessTriggerDTO UpdateFastWorkflow(FASTProcessTriggerDTO Fastworkflowmap, int TenantId, int userid)
        {
            IFASTProcessTriggerDataProvider workflowProvider = DataProviderFactory.Resolve<IFASTProcessTriggerDataProvider>();
            return workflowProvider.UpdateFastWorkflow(Fastworkflowmap, TenantId, userid);
        }

        public List<FASTProcessTriggerDTO> GetFastWorkFlowDetails(int TenantId)
        {
            IFASTProcessTriggerDataProvider FastWorkFlowProvider = DataProviderFactory.Resolve<IFASTProcessTriggerDataProvider>();
            return FastWorkFlowProvider.GetFastWorkFlowDetails(TenantId);
        }

        public List<MessageType> GetMessageTypeDetails()
        {
            IFASTProcessTriggerDataProvider FastWorkFlowProvider = DataProviderFactory.Resolve<IFASTProcessTriggerDataProvider>();
            return FastWorkFlowProvider.GetMessageTypeDetails();
        }

        public int Delete(int id)
        {
            IFASTProcessTriggerDataProvider FastWorkFlowProvider = DataProviderFactory.Resolve<IFASTProcessTriggerDataProvider>();
            return FastWorkFlowProvider.Delete(id);
        }

        public int ConfirmDelete(int id)
        {
            IFASTProcessTriggerDataProvider FastWorkFlowProvider = DataProviderFactory.Resolve<IFASTProcessTriggerDataProvider>();
            return FastWorkFlowProvider.ConfirmDelete(id);
        }


        public List<Service> GetServiceTypeDetails()
        {
            IFASTProcessTriggerDataProvider FastWorkFlowProvider = DataProviderFactory.Resolve<IFASTProcessTriggerDataProvider>();
            return FastWorkFlowProvider.GetServiceTypeDetails();
        }

    }
}
