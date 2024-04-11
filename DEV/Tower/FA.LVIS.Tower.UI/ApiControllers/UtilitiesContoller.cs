using FA.LVIS.Tower.Data;
using FA.LVIS.Tower.Data.TerminalDBEntities;
using FA.LVIS.Tower.DataContracts;
using FA.LVIS.Tower.Services;
using FA.LVIS.Tower.UI.ApiControllers.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Web.Http;
using DC = FA.LVIS.Tower.DataContracts;

namespace FA.LVIS.Tower.UI.ApiControllers
{
    [ValidateAntiForgeryTokenFilter]
    [RoutePrefix("UtilitiesController")]

    public class UtilitiesController : ApiController
    {

        [Route("UpdateExternalRefNum/{servicerequestid}/{externalRefnum:maxlength(50)}/{newExternalRefnum:maxlength(50)}", Name = "UpdateExternalRefNum")]
        [HttpPost]
        [CustomAuthorize("SuperAdmin")]
        public int UpdateExternalRefNum(int servicerequestid, string externalRefnum, string newExternalRefnum)
        {
            AuditLogHelper.sSection = "Utilities\\Update ExternalRefNumber";
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();

            var userId = (claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault() != null) ?
             Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault().Value) : 0;

            return ServiceFactory.Resolve<IUtilitiesService>().UpdateExternalRefNum(servicerequestid, externalRefnum, newExternalRefnum, userId);

        }

        [Route("UpdateandAcceptExternalRefNum/{servicerequestid}/{externalRefnum:maxlength(50)}/{newExternalRefnum:maxlength(50)}", Name = "UpdateandAcceptExternalRefNum")]
        [HttpPost]
        [CustomAuthorize("SuperAdmin")]
        public int UpdateandAcceptExternalRefNum(int servicerequestid, string externalRefnum, string newExternalRefnum)
        {
            AuditLogHelper.sSection = "Utilities\\Update and Accept ExternalRefNumber";
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();

            var userId = (claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault() != null) ?
             Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault().Value) : 0;

            return ServiceFactory.Resolve<IUtilitiesService>().UpdateandAcceptExternalRefNum(servicerequestid, externalRefnum, newExternalRefnum, userId);

        }

        [Route("UpdateServiceRequestInfo/{servicerequestid}/{externalRefnum:maxlength(50)}/{internalRefnum:maxlength(50)}/{internalRefid}/{customerRefnum:maxlength(50)}/{status}/{chkUniqueID}/{chkExternalRefNum}", Name = "UpdateServiceRequestInfo")]
        [HttpPost]
        [CustomAuthorize("SuperAdmin")]
        public int UpdateServiceRequestInfo(int servicerequestid, string externalRefnum, string internalRefnum, int internalRefid, string customerRefnum, int status, bool chkUniqueID, bool chkExternalRefNum)
        {

            AuditLogHelper.sSection = "Utilities\\Update Service Req Info";
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();
            var userId = (claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault() != null) ?
            Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault().Value) : 0;

            return ServiceFactory.Resolve<IUtilitiesService>().UpdateServiceRequestInfo(servicerequestid, externalRefnum, internalRefnum, internalRefid, customerRefnum, userId, status, chkUniqueID, chkExternalRefNum);

        }

        [Route("GetServiceReqInfo/{servicerequestid:int:min(1)}", Name = "GetServiceReqInfo")]
        [HttpGet]
        [CustomAuthorize("SuperAdmin")]
        public ServiceRequestDTO GetServiceReqInfo(int servicerequestid)
        {
            AuditLogHelper.sSection = "Utilities\\Get Service Request Info";
            return ServiceFactory.Resolve<IUtilitiesService>().GetServiceReqInfo(servicerequestid);

        }
        [Route("GetServiceReqInfoWithTenant/{servicerequestid:int:min(1)}", Name = "GetServiceReqInfoWithTenant")]
        [HttpGet]
        [CustomAuthorize("SuperAdmin")]
        public ServiceRequestDTO GetServiceReqInfoWithTenant(int servicerequestid)
        {
            AuditLogHelper.sSection = "Utilities\\Get Service Request Info";
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();
            var tenantId = (claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault() != null) ?
             Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value) : 0;
            ServiceRequestDTO serviceRequest = ServiceFactory.Resolve<IUtilitiesService>().GetServiceReqInfo(servicerequestid);
            if (tenantId == (int)TenantIdEnum.LVIS)
            {
                return serviceRequest;
            }
            else if (serviceRequest.tenantID != tenantId)
            {
                return new ServiceRequestDTO();
            }
            return serviceRequest;
        }


        [Route("GetStatus", Name = "GetStatus")]
        [HttpGet]
        [CustomAuthorize("SuperAdmin", "Admin")]
        public IEnumerable<ExceptionStatus> GetStatus()
        {
            AuditLogHelper.sSection = "Utilities\\GetStatus";
            return ServiceFactory.Resolve<IUtilitiesService>().GetStatus();
        }


        [Route("ConfirmService/{servicerequestid:int:min(1)}", Name = "ConfirmService")]
        [HttpGet]
        [CustomAuthorize("SuperAdmin")]
        public bool ConfirmService(int servicerequestid)
        {
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();

            var userId = (claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault() != null) ?
           Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault().Value) : 0;

            AuditLogHelper.sSection = "Utilities\\ConfirmService";
            return ServiceFactory.Resolve<IUtilitiesService>().ConfirmService(servicerequestid, userId);

        }

        [Route("GetEndPointApplications", Name = "GetEndPointApplications")]
        [HttpGet]
        public IEnumerable<ApplicationMappingDTO> GetEndPointApplications()
        {
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();

            var userId = (claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault() != null) ?
           Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault().Value) : 0;

            AuditLogHelper.sSection = "Utilities\\GetApplications";
            return ServiceFactory.Resolve<IUtilitiesService>().GetApplications();

        }

        [Route("AddCredentials", Name = "AddCredentials")]
        [HttpPost]
        [CustomAuthorize("SuperAdmin")]
        public bool AddCredentials([FromBody]AddCredentialDTO ApplicationDetails)
        {
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();

            var userId = (claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault() != null) ?
           Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault().Value) : 0;

            AuditLogHelper.sSection = "Utilities\\AddCredentials";
            return ServiceFactory.Resolve<IUtilitiesService>().AddCredentials(ApplicationDetails, userId);

        }

        [Route("GetFastEnvironmentInfo", Name = "GetFastEnvironmentInfo")]
        [HttpGet]        
        public IHttpActionResult GetFastEnvironmentInfo()
        {       
            var value= ServiceFactory.Resolve<IUtilitiesService>().PopulateFastAppEnvironmentDetails();
            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(JsonConvert.SerializeObject(value), Encoding.UTF8, "application/json");
            return ResponseMessage(response);
        }

        [Route("GetAppVersionInfo", Name = "GetAppVersionInfo")]
        [HttpGet]        
        public IHttpActionResult GetAppVersionInfo()
        {
            Assembly web = Assembly.GetExecutingAssembly();
            var versionInfo = web.GetName().Version.ToString();
            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(JsonConvert.SerializeObject(new {Version = versionInfo }), Encoding.UTF8, "application/json");
            return ResponseMessage(response);
        }
    }
}