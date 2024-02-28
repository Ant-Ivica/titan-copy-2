using FA.LVIS.Tower.Data;
using FA.LVIS.Tower.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using DC = FA.LVIS.Tower.DataContracts;
using FA.LVIS.Tower.UI.ApiControllers.Filters;

namespace FA.LVIS.Tower.UI.ApiControllers
{
    [ValidateAntiForgeryTokenFilter]
    [RoutePrefix("FastTask")]

    [CustomAuthorize]
    public class FastTaskMappingController : ApiController
    {
        [Route("GetFASTTaskMappingDetails", Name = "GetFASTTaskMappingDetails")]
        [HttpGet]
        public IEnumerable<DC.FastTaskMapDTO> GetFASTTaskMappingDetails()
        {
            AuditLogHelper.sSection = "Mappings\\FASTTaskMap\\GetFASTTaskMappingDetails";
          

            IFastTaskMappingService FasttaskMapping = ServiceFactory.Resolve<IFastTaskMappingService>();
            List<DC.FastTaskMapDTO> newList = new List<DC.FastTaskMapDTO>();

            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();
            var tenantId = (claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault() != null) ?
                Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value) : 0;


            newList = FasttaskMapping.GetFastTaskDetails(tenantId);
            return newList;
        }

        [Route("GetServiceDetails", Name = "GetServiceDetails")]
        [HttpGet]
        public IEnumerable<DC.Service> GetServiceDetails()
        {
            AuditLogHelper.sSection = "Mappings\\FASTTaskMap\\GetServiceDetails";
            

            IFastTaskMappingService ServiceMap = ServiceFactory.Resolve<IFastTaskMappingService>();
            List<DC.Service> newList = new List<DC.Service>();

            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();
            var tenantId = (claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault() != null) ?
                Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value) : 0;
            newList = ServiceMap.GetServiceDetails(tenantId);
            return newList;
        }

        [Route("GetMessageType", Name = "GetMessageType")]
        [HttpGet]
        public IEnumerable<DC.MessageType> GetMessageType()
        {
            AuditLogHelper.sSection = "Mappings\\FASTTaskMap\\GetMessageType";
          

            IFastTaskMappingService MessageTypemap = ServiceFactory.Resolve<IFastTaskMappingService>();
            List<DC.MessageType> newList = new List<DC.MessageType>();

            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();
            var tenantId = (claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault() != null) ?
                Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value) : 0;
            newList = MessageTypemap.GetMessageType();
            return newList;
        }

        [Route("GetTypecode", Name = "GetTypecode")]
        [HttpGet]
        public IEnumerable<DC.TypeCodeDTO> GetTypecode()
        {
            AuditLogHelper.sSection = "Mappings\\FASTTaskMap\\GetTypecode";


            IFastTaskMappingService Typecodemap = ServiceFactory.Resolve<IFastTaskMappingService>();
            List<DC.TypeCodeDTO> newList = new List<DC.TypeCodeDTO>();

            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();
            var tenantId = (claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault() != null) ?
                Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value) : 0;
            newList = Typecodemap.GetTypeCode();
            return newList;
        }

        [Route("AddFastTaskMap", Name = "AddFastTaskMap")]
        [HttpPost]
        [CustomAuthorize("SuperAdmin", "Admin")]
        public DC.FastTaskMapDTO AddFastTaskMap(DC.FastTaskMapDTO value)
        {
            AuditLogHelper.sSection = "Mappings\\FastTaskMap\\AddFastTaskMap";
            IFastTaskMappingService AddfastTaskmap = ServiceFactory.Resolve<IFastTaskMappingService>();

            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();
            var tenantId = (claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault() != null) ?
               Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value) : 0;
            var userId = (claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault() != null) ?
             Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault().Value) : 0;
            return AddfastTaskmap.AddFastTask(value, tenantId, userId);
        }


        [Route("UpdateFastTaskMap", Name = "UpdateFastTaskMap")]
        [HttpPost]
        [CustomAuthorize("SuperAdmin", "Admin")]
        public DC.FastTaskMapDTO UpdateFastTaskMap(DC.FastTaskMapDTO value)
        {

            AuditLogHelper.sSection = "Mappings\\FastTaskMap\\UpdateFastTaskMap";
            IFastTaskMappingService AddfastTaskmap = ServiceFactory.Resolve<IFastTaskMappingService>();
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();
            var tenantId = (claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault() != null) ?
               Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value) : 0;
            var userId = (claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault() != null) ?
             Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault().Value) : 0;


            return AddfastTaskmap.UpdateFastTask(value, tenantId, userId);
        }



        [Route("DeleteFastTask", Name = "DeleteFastTask")]
        [HttpPost]
        [CustomAuthorize("SuperAdmin", "Admin")]
        public int DeleteFastTask([FromBody]int value)
        {
            AuditLogHelper.sSection = "Mappings\\FastTaskMap\\DeleteFastTask";
            IFastTaskMappingService DeleteFastTask = ServiceFactory.Resolve<IFastTaskMappingService>();
            return DeleteFastTask.Delete(value);
        }
    
        [Route("ConfirmDeleteFastTask", Name = "ConfirmDeleteFastTask")]
        [HttpPost]
        [CustomAuthorize("SuperAdmin", "Admin")]
        public int ConfirmDeleteFastTask([FromBody]int value)
        {
            AuditLogHelper.sSection = "Mappings\\FastTaskMap\\DeleteFastTask";
            IFastTaskMappingService DeleteFastTask = ServiceFactory.Resolve<IFastTaskMappingService>();
            return DeleteFastTask.ConfirmDelete(value);
        }



    }
}
