using FA.LVIS.Tower.Core;
using FA.LVIS.Tower.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using FA.LVIS.Tower.FASTProcessing;
using System.Configuration;

namespace FA.LVIS.Tower.Data
{
    public class FASTProcessTriggerDataProvider : Core.DataProviderBase, IFASTProcessTriggerDataProvider
    {
        public FASTProcessTriggerDTO AddFastWorkflow(FASTProcessTriggerDTO FastworkflowMap, int tenantId, int userId)
        {
            TerminalDBEntities.Entities dbContext = new TerminalDBEntities.Entities();
            TerminalDBEntities.FASTWorkflowMap AddFASTworkflow = new TerminalDBEntities.FASTWorkflowMap();

            AddFASTworkflow.MessageTypeId = FastworkflowMap.MessageTypeId;
            AddFASTworkflow.ProcessEventId = FastworkflowMap.ProcessEventId;
            AddFASTworkflow.TaskEventId = FastworkflowMap.TaskeventId;
            AddFASTworkflow.FASTWorkflowMapDesc = FastworkflowMap.FASTOWorkFlowMapDesc;
            AddFASTworkflow.ServiceId = FastworkflowMap.serviceId == 0 ? default(int?) : FastworkflowMap.serviceId ;
            AddFASTworkflow.CreatedDate = DateTime.Now;
            AddFASTworkflow.LastModifiedDate = DateTime.Now;
            AddFASTworkflow.CreatedById = userId;
            AddFASTworkflow.LastModifiedById = userId;
            AddFASTworkflow.TenantId = tenantId;
            if (FastworkflowMap.Customerid == 0)
                AddFASTworkflow.CustomerId = null;
            else
                AddFASTworkflow.CustomerId = FastworkflowMap.Customerid;


            dbContext.FASTWorkflowMaps.Add(AddFASTworkflow);
            int Success = AuditLogHelper.SaveChanges(dbContext);

            if (Success == 1)
            {
                RunAccount impAccount = new RunAccount();
                impAccount.Tenantid = tenantId;
                EQFASTSearch searchClient = new EQFASTSearch(impAccount);
                List<Workflowprocesstaskevent> ProcessTriggers = searchClient.GetFastWorkFlowProcessTaskEvent(487);
                List<Workflowprocesstaskevent> TaskTriggers = searchClient.GetFastWorkFlowProcessTaskEvent(488);
                FastworkflowMap.FASTWorkFlowMapId = AddFASTworkflow.FASTWorkflowMapId;
                FastworkflowMap.MessageType = dbContext.MessageTypes.Where(v => v.MessageTypeId == AddFASTworkflow.MessageTypeId).Select(v => v.MessageTypeName).FirstOrDefault();
                FastworkflowMap.ProcessEventId = AddFASTworkflow.ProcessEventId;
                FastworkflowMap.serviceId = AddFASTworkflow.ServiceId.GetValueOrDefault() > 0 ? AddFASTworkflow.ServiceId.Value : AddFASTworkflow.ServiceId.GetValueOrDefault(0);
                FastworkflowMap.serviceName = AddFASTworkflow.ServiceId.GetValueOrDefault() > 0 ? dbContext.Services.Where(sel => sel.ServiceId == AddFASTworkflow.ServiceId).Select(se => se.ServiceName).FirstOrDefault() : "Any";
                FastworkflowMap.ProcessEvent = ProcessTriggers.Where(se => se.Id == AddFASTworkflow.ProcessEventId).FirstOrDefault().Description;
                FastworkflowMap.TaskeventId = AddFASTworkflow.TaskEventId;
                FastworkflowMap.Taskevent = TaskTriggers.Where(se => se.Id == AddFASTworkflow.TaskEventId).FirstOrDefault().Description;
                //dbContext.FASTWorkflowMaps.Where(se => se.MessageTypeId == AddFASTworkflow.MessageTypeId).Select(v => v.FASTWorkflowMapDesc + " (" + v.TaskEventId + ")").FirstOrDefault();
                FastworkflowMap.FASTOWorkFlowMapDesc = AddFASTworkflow.FASTWorkflowMapDesc;
                FastworkflowMap.Customer = AddFASTworkflow.CustomerId.HasValue ? dbContext.Customers.Where(v => v.CustomerId == (AddFASTworkflow.CustomerId.HasValue ? AddFASTworkflow.CustomerId.Value : 0)).Select(v => v.CustomerName + "(" + v.CustomerId + ")").FirstOrDefault():"Any";

            }
            return FastworkflowMap;

        }

        public int ConfirmDelete(int id)
        {
            int success = 0;
            using (var dbContext = new TerminalDBEntities.Entities())
            {
                var WorkFlowToDelete = (from WorkFlow in dbContext.FASTWorkflowMaps
                                        where WorkFlow.FASTWorkflowMapId == id
                                        select WorkFlow);

                if (WorkFlowToDelete != null)
                {
                    dbContext.FASTWorkflowMaps.RemoveRange(WorkFlowToDelete);
                    success= AuditLogHelper.SaveChanges(dbContext);
                }
            }
            return success;
        }

        public int Delete(int id)
        {
            using (var dbContext = new TerminalDBEntities.Entities())
            {
                var result = dbContext.GetDependancyRecordOutput(id, "FASTWorkflowMap").FirstOrDefault();

                if (result != null)
                {
                    return 0;
                }

                else
                {
                    return 1;
                }
            }

        }

        public List<FASTProcessTriggerDTO> GetFastWorkFlowDetails(int tenantId)
        {
            RunAccount impAccount = new RunAccount();
            impAccount.Tenantid = tenantId;
            EQFASTSearch searchClient = new EQFASTSearch(impAccount);
            impAccount.Tenantid = tenantId;
            if (impAccount.Tenantid == (int)TerminalDBEntities.TenantIdEnum.LVIS)
                impAccount.Tenantid = 0;
            List<Workflowprocesstaskevent> ProcessTriggers = searchClient.GetFastWorkFlowProcessTaskEvent(487);
            List<Workflowprocesstaskevent> TaskTriggers = searchClient.GetFastWorkFlowProcessTaskEvent(488);

            TerminalDBEntities.Entities dbContext = new TerminalDBEntities.Entities();
            List<DataContracts.FASTProcessTriggerDTO> FastWorkFlowMappings = new List<DataContracts.FASTProcessTriggerDTO>();
            if (dbContext.FASTWorkflowMaps.Count() > 0)
            {
                FastWorkFlowMappings = dbContext.FASTWorkflowMaps
                   .AsEnumerable()
                   .Select(x => new FASTProcessTriggerDTO
                   {
                       FASTWorkFlowMapId = x.FASTWorkflowMapId,
                       MessageTypeId = x.MessageTypeId,
                       MessageType = dbContext.MessageTypes.Where(sel => sel.MessageTypeId == x.MessageTypeId).Select(se => se.MessageTypeName).FirstOrDefault(),
                       ProcessEventId = x.ProcessEventId,
                       ProcessEvent = x.ProcessEventId > 0 ? ProcessTriggers.Where(se => se.Id == x.ProcessEventId).FirstOrDefault()?.Description : string.Empty,
                       TaskeventId = x.TaskEventId,
                       Taskevent = x.TaskEventId > 0 ? TaskTriggers.Where(se => se.Id == x.TaskEventId).FirstOrDefault()?.Description : string.Empty,
                       serviceId=x.ServiceId.GetValueOrDefault() > 0 ? x.ServiceId.Value : x.ServiceId.GetValueOrDefault(0),
                       serviceName= x.ServiceId.GetValueOrDefault() > 0 ? dbContext.Services.Where(sel=>sel.ServiceId==x.ServiceId).Select(se=>se.ServiceName).FirstOrDefault(): "Any",
                       FASTOWorkFlowMapDesc = x.FASTWorkflowMapDesc,
                       Tenatid = x.TenantId,
                       Tenant = x.Tenant?.TenantName,
                       Customer = x.CustomerId != null ? x.Customer.CustomerName + "(" + x.Customer.CustomerId + ")":"Any",
                       Customerid= x.CustomerId != null?x.CustomerId.Value:0

            }).ToList();
            }
            if (FastWorkFlowMappings.Count() > 0 && tenantId != (int)TerminalDBEntities.TenantIdEnum.LVIS)
            {
                FastWorkFlowMappings = FastWorkFlowMappings
                    .Where(sel => sel.Tenatid == tenantId).ToList();
            }
            return FastWorkFlowMappings;
        }

        public List<MessageType> GetMessageTypeDetails()
        {
            TerminalDBEntities.Entities dbContext = new TerminalDBEntities.Entities();
            var distinctMessageType = dbContext.MessageTypes.
                Where(sl => sl.ApplicationId == (int)TerminalDBEntities.ApplicationEnum.LVIS && (sl.MessageTypeName.Contains("#") == false))
                .Select(se => new DataContracts.MessageType()
                {
                    MessageTypeId = se.MessageTypeId,
                    MessageTypeName = se.MessageTypeName
                });
            return distinctMessageType.DistinctBy(Sm => Sm.MessageTypeId).ToList();
        }

        public List<Service> GetServiceTypeDetails()
        {
            TerminalDBEntities.Entities dbContext = new TerminalDBEntities.Entities();
            var distinctServiceType = dbContext.Services               
                .Select(se => new DataContracts.Service()
                {
                    ID = se.ServiceId,
                    Name = se.ServiceName
                });
            return distinctServiceType.DistinctBy(Sm =>Sm.Name).ToList();
        }

        public FASTProcessTriggerDTO UpdateFastWorkflow(FASTProcessTriggerDTO fastworkflow, int tenantId, int userId)
        {
            using (var dbContext = new TerminalDBEntities.Entities())
            {
                var FASTworkflowMapToUpdate = (from x in dbContext.FASTWorkflowMaps
                                               where x.FASTWorkflowMapId == fastworkflow.FASTWorkFlowMapId
                                               select x).FirstOrDefault();

                if (FASTworkflowMapToUpdate != null)
                {
                    FASTworkflowMapToUpdate.LastModifiedDate = DateTime.Now;
                    FASTworkflowMapToUpdate.LastModifiedById = userId;
                    FASTworkflowMapToUpdate.ProcessEventId = fastworkflow.ProcessEventId;
                    FASTworkflowMapToUpdate.TaskEventId = fastworkflow.TaskeventId;
                    FASTworkflowMapToUpdate.ServiceId = fastworkflow.serviceId == 0 ? default(int?) : fastworkflow.serviceId;
                    FASTworkflowMapToUpdate.FASTWorkflowMapDesc = fastworkflow.FASTOWorkFlowMapDesc;
                    FASTworkflowMapToUpdate.MessageTypeId = fastworkflow.MessageTypeId;
                    if (fastworkflow.Customerid == 0)
                        FASTworkflowMapToUpdate.CustomerId = null;
                    else
                        FASTworkflowMapToUpdate.CustomerId = fastworkflow.Customerid;

                    dbContext.Entry(FASTworkflowMapToUpdate).State = System.Data.Entity.EntityState.Modified;

                    int Success = AuditLogHelper.SaveChanges(dbContext);

                    if (Success == 1)
                    {
                        RunAccount impAccount = new RunAccount();
                        impAccount.Tenantid = tenantId;
                        EQFASTSearch searchClient = new EQFASTSearch(impAccount);
                        List<Workflowprocesstaskevent> ProcessTriggers = searchClient.GetFastWorkFlowProcessTaskEvent(487);
                        List<Workflowprocesstaskevent> TaskTriggers = searchClient.GetFastWorkFlowProcessTaskEvent(488);

                        fastworkflow.FASTWorkFlowMapId = FASTworkflowMapToUpdate.FASTWorkflowMapId;
                        fastworkflow.MessageType = dbContext.MessageTypes.Where(v => v.MessageTypeId == FASTworkflowMapToUpdate.MessageTypeId).Select(v => v.MessageTypeName).FirstOrDefault();
                        fastworkflow.ProcessEventId = FASTworkflowMapToUpdate.ProcessEventId;
                        fastworkflow.ProcessEvent = ProcessTriggers.Where(se => se.Id == FASTworkflowMapToUpdate.ProcessEventId).FirstOrDefault().Description;
                        fastworkflow.TaskeventId = FASTworkflowMapToUpdate.TaskEventId;
                        fastworkflow.serviceId = FASTworkflowMapToUpdate.ServiceId.GetValueOrDefault() > 0 ? FASTworkflowMapToUpdate.ServiceId.Value : FASTworkflowMapToUpdate.ServiceId.GetValueOrDefault(0);
                        fastworkflow.serviceName = FASTworkflowMapToUpdate.ServiceId.GetValueOrDefault() > 0 ? dbContext.Services.Where(sel => sel.ServiceId == FASTworkflowMapToUpdate.ServiceId).Select(se => se.ServiceName).FirstOrDefault() : "Any";
                        fastworkflow.Taskevent = TaskTriggers.Where(se => se.Id == FASTworkflowMapToUpdate.TaskEventId).FirstOrDefault().Description;
                        //dbContext.FASTWorkflowMaps.Where(se => se.MessageTypeId == FASTworkflowMapToUpdate.MessageTypeId).Select(v => v.FASTWorkflowMapDesc + " (" + v.TaskEventId + ")").FirstOrDefault();
                        fastworkflow.Customer = FASTworkflowMapToUpdate.CustomerId.HasValue ?dbContext.Customers.Where(v => v.CustomerId == (FASTworkflowMapToUpdate.CustomerId.HasValue ? FASTworkflowMapToUpdate.CustomerId.Value : 0)).Select(v => v.CustomerName + "(" + v.CustomerId + ")").FirstOrDefault():"Any";
                     
                        fastworkflow.FASTOWorkFlowMapDesc = FASTworkflowMapToUpdate.FASTWorkflowMapDesc;
                    }
                    return fastworkflow;
                }
            }
            return fastworkflow;
        }
    }
}
