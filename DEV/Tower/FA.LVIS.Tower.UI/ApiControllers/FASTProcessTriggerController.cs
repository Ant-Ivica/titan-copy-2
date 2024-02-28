using FA.LVIS.Tower.Data;
using FA.LVIS.Tower.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using DC = FA.LVIS.Tower.DataContracts;
using System.Configuration;
using FA.LVIS.Tower.FASTProcessing;
using FA.LVIS.Tower.UI.ApiControllers.Filters;

namespace FA.LVIS.Tower.UI.ApiControllers
{
    [ValidateAntiForgeryTokenFilter]
    [RoutePrefix("FASTWorkFlow")]

    [CustomAuthorize]
    public class FASTProcessTriggerController : ApiController
    {
        [Route("Delete", Name = "DeleteFASTWorkFlow")]
        [HttpPost]
        [CustomAuthorize("SuperAdmin", "Admin")]
        public int Delete([FromBody] int id)
        {
            AuditLogHelper.sSection = "Mappings\\FAST Process Trigger\\Delete";
            IFASTProcessTriggerService ProcessTrigger = ServiceFactory.Resolve<IFASTProcessTriggerService>();
            return ProcessTrigger.Delete(id);
        }


        [Route("ConfirmDelete", Name = "ConfirmDelete")]
        [HttpPost]
        [CustomAuthorize("SuperAdmin", "Admin")]
        public int ConfirmDelete([FromBody] int id)
        {
            AuditLogHelper.sSection = "Mappings\\FAST Process Trigger";
            IFASTProcessTriggerService ProcessTrigger = ServiceFactory.Resolve<IFASTProcessTriggerService>();
            return ProcessTrigger.ConfirmDelete(id);
        }

        [Route("GetFASTWorkFlowMapping", Name = "GetFASTWorkFlowMapping")]
        [HttpGet]
        public IEnumerable<DC.FASTProcessTriggerDTO> GetFASTWorkFlowMapping()
        {
            AuditLogHelper.sSection = "Mappings\\FAST Process Trigger\\GetFASTWorkFlowMapping";  
            IFASTProcessTriggerService FastworkflowMapping = ServiceFactory.Resolve<IFASTProcessTriggerService>();
            List<DC.FASTProcessTriggerDTO> newList = new List<DC.FASTProcessTriggerDTO>();

            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();
            var tenantId = (claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault() != null) ?
                Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value) : 0;

            newList = FastworkflowMapping.GetFastWorkFlowDetails(tenantId);
            return newList;
        }

        //[Route("GetMessageTypeDetails", Name = "GetMessageTypeDetails")]
        //[HttpGet]
        //public IEnumerable<DC.MessageType> GetMessageTypeDetails()
        //{
        //    AuditLogHelper.sSection = "Mappings\\FAST Process Trigger\\GetMessageTypeDetails";
        //    IFASTProcessTriggerService FastworkflowMapping = ServiceFactory.Resolve<IFASTProcessTriggerService>();
        //    List<DC.MessageType> newList = new List<DC.MessageType>();

        //    var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();
        //    var tenantId = (claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault() != null) ?
        //        Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value) : 0;
        //    newList = FastworkflowMapping.GetMessageTypeDetails();
        //    return newList;
        //}


        [Route("GetServiceTypeDetails", Name = "GetServiceTypeDetails")]
        [HttpGet]
        public IEnumerable<DC.Service> GetServiceTypeDetails()
        {
            AuditLogHelper.sSection = "Mappings\\FAST Process Trigger\\GetServiceTypeDetails";
            IFASTProcessTriggerService FastworkflowMapping = ServiceFactory.Resolve<IFASTProcessTriggerService>();
            List<DC.Service> newList = new List<DC.Service>();

            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();
            var tenantId = (claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault() != null) ?
                Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value) : 0;
            newList = FastworkflowMapping.GetServiceTypeDetails();
            return newList;
        }
        [Route("AddFastWorkflowmap", Name = "AddFastWorkflowmap")]
        [HttpPost]
        [CustomAuthorize("SuperAdmin", "Admin")]
        public DC.FASTProcessTriggerDTO Post(DC.FASTProcessTriggerDTO value)
        {
            AuditLogHelper.sSection = "Mappings\\FAST Process Trigger\\AddFastWorkflowmap";
            IFASTProcessTriggerService fastworkflowMapping = ServiceFactory.Resolve<IFASTProcessTriggerService>();

            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();
            var tenantId = (claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault() != null) ?
               Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value) : 0;
            var userId = (claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault() != null) ?
             Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault().Value) : 0;

            return fastworkflowMapping.AddFastWorkflow(value, tenantId, userId);
        }

        [Route("UpdateFastworkflowmap", Name = "UpdaUpdateFastworkflowmapteFastOffice")]
        [HttpPost]
        [CustomAuthorize("SuperAdmin", "Admin")]
        public DC.FASTProcessTriggerDTO UpdateFastworkflowmap(DC.FASTProcessTriggerDTO value)
        {
            AuditLogHelper.sSection = "Mappings\\FAST Process Trigger\\UpdaUpdateFastworkflowmapteFastOffice";
            IFASTProcessTriggerService fastworkflowMapping = ServiceFactory.Resolve<IFASTProcessTriggerService>();

            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();
            var tenantId = (claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault() != null) ?
               Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value) : 0;
            var userId = (claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault() != null) ?
             Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault().Value) : 0;

            return fastworkflowMapping.UpdateFastWorkflow(value, tenantId, userId);
        }

        [Route("BindProcessTaskEvent/{iTenatid}", Name = "BindProcessTaskEvent")]
        [HttpGet]
        public IEnumerable<Workflowprocesstaskevent> BindProcessTaskEvent(int iTenatid)
        {
            AuditLogHelper.sSection = "Mappings\\FAST Process Trigger\\BindProcessTaskEvent";
            // List<DC.FastProcessTaskEventDTO> ListProcessEvent = new  List<DC.FastProcessTaskEventDTO>();
            RunAccount impAccount = new RunAccount();
            impAccount.ImpDomain = ConfigurationManager.AppSettings["FastServiceDomain"];
            impAccount.ImpAccount = ConfigurationManager.AppSettings["FastServiceUser"];
            impAccount.ImpPassword = ConfigurationManager.AppSettings["FastServicePassword"];
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();

            impAccount.Tenantid = iTenatid;

            if(impAccount.Tenantid == 0)
            impAccount.Tenantid = (claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault() != null) ?
                        Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value) : 0;


            EQFASTSearch searchClient = new EQFASTSearch(impAccount);
            //487 has been Passed to Filter Only EventIds and Description
            return searchClient.GetFastWorkFlowProcessTaskEvent(487);
        }

        [Route("BindTaskEvent/{iTenantid}", Name = "BindTaskEvent")]
        [HttpGet]
        public IEnumerable<Workflowprocesstaskevent> BindTaskEvent(int iTenantid)
        {
            AuditLogHelper.sSection = "Mappings\\FAST Process Trigger\\BindTaskEvent";
            // List<DC.FastProcessTaskEventDTO> ListProcessEvent = new  List<DC.FastProcessTaskEventDTO>();
            RunAccount impAccount = new RunAccount();
            impAccount.ImpDomain = ConfigurationManager.AppSettings["FastServiceDomain"];
            impAccount.ImpAccount = ConfigurationManager.AppSettings["FastServiceUser"];
            impAccount.ImpPassword = ConfigurationManager.AppSettings["FastServicePassword"];
           
                var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();

            impAccount.Tenantid = iTenantid;

            if (impAccount.Tenantid == 0)
                impAccount.Tenantid = (claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault() != null) ?
                            Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value) : 0;


        
            EQFASTSearch searchClient = new EQFASTSearch(impAccount);
            //488 has been Passed to Filter Only EventIds and Description
            return searchClient.GetFastWorkFlowProcessTaskEvent(488);
        }
    }
}
