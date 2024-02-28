using FA.LVIS.Tower.Core;
using FA.LVIS.Tower.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FA.LVIS.Tower.Data
{
    public class FastWorkFlowMappingsDataProvider11 : Core.DataProviderBase, IFastWorkFlowMappingDataProvider11
    {
        //public List<FASTWorkFlowMapDTO> GetFastWorkFlowDetails(int tenantId)
        //{
        //    TerminalDBEntities.Entities dbContext = new TerminalDBEntities.Entities();
        //    List<DataContracts.FASTWorkFlowMapDTO> FastWorkFlowMappings = new List<DataContracts.FASTWorkFlowMapDTO>();
        //    if (dbContext.FASTWorkflowMaps.Count() > 0)
        //    {
        //        FastWorkFlowMappings = dbContext.FASTWorkflowMaps
        //           //.Where(se => se.Provider.TenantId == iTenantid)
        //           .Select(x => new FASTWorkFlowMapDTO
        //           {
        //               FASTWorkFlowMapId = x.FASTWorkflowMapId,
        //               MessageTypeId=x.MessageTypeId,
        //               ProcessEventId=x.ProcessEventId,
        //               TaskeventId=x.TaskEventId,
        //               FASTOWorkFlowMapDesc=x.FASTWorkflowMapDesc                      
        //           }).ToList();
        //    }
        //    if (FastWorkFlowMappings.Count() > 0 && tenantId != (int)TerminalDBEntities.TenantIdEnum.LVIS)
        //    {
        //        FastWorkFlowMappings = FastWorkFlowMappings
        //            .Where(sel => (from s in dbContext.FASTWorkflowMaps where s.FASTWorkflowMapId == sel.FASTWorkFlowMapId && s.TenantId == tenantId select s.FASTWorkflowMapId).Contains(sel.FASTWorkFlowMapId)).ToList();
        //    }
        //    return FastWorkFlowMappings;
        //}
       //public List<FASTWorkFlowMapDTO> GetFastWorkFlowDetails(int tenantId)
       // {
       //     TerminalDBEntities.Entities dbContext = new TerminalDBEntities.Entities();
       //     List<DataContracts.FASTWorkFlowMapDTO> FastWorkFlowMappings = new List<DataContracts.FASTWorkFlowMapDTO>();
       //     if (dbContext.FASTWorkflowMaps.Count() > 0)
       //     {
       //         FastWorkFlowMappings = dbContext.FASTWorkflowMaps
       //            //.Where(se => se.Provider.TenantId == iTenantid)
       //            .Select(x => new FASTWorkFlowMapDTO
       //            {
       //                FASTWorkFlowMapId = x.FASTWorkflowMapId,
       //                MessageTypeId = x.MessageTypeId,
       //                ProcessEventId = x.ProcessEventId,
       //                TaskeventId = x.TaskEventId,
       //                FASTOWorkFlowMapDesc = x.FASTWorkflowMapDesc
       //            }).ToList();
       //     }
       //     if (FastWorkFlowMappings.Count() > 0 && tenantId != (int)TerminalDBEntities.TenantIdEnum.LVIS)
       //     {
       //         FastWorkFlowMappings = FastWorkFlowMappings
       //             .Where(sel => (from s in dbContext.FASTWorkflowMaps where s.FASTWorkflowMapId == sel.FASTWorkFlowMapId && s.TenantId == tenantId select s.FASTWorkflowMapId).Contains(sel.FASTWorkFlowMapId)).ToList();
       //     }
       //     return FastWorkFlowMappings;
       // }
    }
}
