using FA.LVIS.Tower.Data;
using FA.LVIS.Tower.DataContracts;
using FA.LVIS.Tower.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FA.LVIS.Tower.UI.ApiControllers.Filters;


namespace FA.LVIS.Tower.UI.ApiControllers
{
    [ValidateAntiForgeryTokenFilter]
    [RoutePrefix("Subscriptions")]

    [CustomAuthorize]
    public class SubscriptionController : ApiController
    {
        [Route("GetSubscriptionsByCustomer/{customerId}/{applicationId}", Name = "GetSubscriptionsByCustomer")]
        [HttpGet]
        public IEnumerable<Subscription> GetSubscriptionsByCustomer(int customerId, int applicationId)
        {
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();

            var tenantId = (claims.Where(c => c.Type == Constants.TENANT_ID).FirstOrDefault() != null) ?
                Convert.ToInt32(claims.Where(c => c.Type == Constants.TENANT_ID).FirstOrDefault().Value) : 0;

            AuditLogHelper.sSection = "Mappings\\Customers\\Subscriptions";

            IEnumerable<Subscription> Subscriptions = ServiceFactory.Resolve<ISubscriptionService>().GetSubscriptionsByCustomer(customerId, tenantId, applicationId);

            return Subscriptions;
        }

        [Route("GetSubscriptionsByCategory/{categoryId}/{applicationId}", Name = "GetSubscriptionsByCategory")]
        [HttpGet]
        public IEnumerable<Subscription> GetSubscriptionsByCategory(int categoryId, int applicationId)
        {
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();

            var tenantId = (claims.Where(c => c.Type == Constants.TENANT_ID).FirstOrDefault() != null) ?
                Convert.ToInt32(claims.Where(c => c.Type == Constants.TENANT_ID).FirstOrDefault().Value) : 0;

            AuditLogHelper.sSection = "Mappings\\Category\\Subscriptions";

            IEnumerable<Subscription> Subscriptions = ServiceFactory.Resolve<ISubscriptionService>().GetSubscriptionsByCategory(categoryId, tenantId, applicationId);

            return Subscriptions;
        }
        
        [HttpPost]
        [CustomAuthorize("SuperAdmin", "Admin")]
        public Subscription AddSubscription(Subscription value)
        {
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();

            var tenantId = (claims.Where(c => c.Type == Constants.TENANT_ID).FirstOrDefault() != null) ?
                Convert.ToInt32(claims.Where(c => c.Type == Constants.TENANT_ID).FirstOrDefault().Value) : 0;

            var userId = (claims.Where(c => c.Type == Constants.USER_ID).FirstOrDefault() != null) ?
             Convert.ToInt32(claims.Where(c => c.Type == Constants.USER_ID).FirstOrDefault().Value) : 0;

            

            AuditLogHelper.sSection = "Mappings\\Subscription\\post";
            return ServiceFactory.Resolve<ISubscriptionService>().AddSubscription(value, tenantId, userId);
        }
        
        [HttpPost]
        [CustomAuthorize("SuperAdmin", "Admin")]
        public Subscription UpdateSubscription(Subscription value)
        {
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();

            var tenantId = (claims.Where(c => c.Type == Constants.TENANT_ID).FirstOrDefault() != null) ?
                Convert.ToInt32(claims.Where(c => c.Type == Constants.TENANT_ID).FirstOrDefault().Value) : 0;

            var userId = (claims.Where(c => c.Type == Constants.USER_ID).FirstOrDefault() != null) ?
             Convert.ToInt32(claims.Where(c => c.Type == Constants.USER_ID).FirstOrDefault().Value) : 0;

            AuditLogHelper.sSection = "Mappings\\Updatesubscription\\post";
            return ServiceFactory.Resolve<ISubscriptionService>().UpdateSubscription(value, tenantId, userId);
        }

        [Route("GetApplicationMessageType/{applicationId}/{TenantId}/{SubscriptionId}", Name = "GetApplicationMessageType")]
        [HttpGet]
        public IEnumerable<MessageType> GetApplicationMessageType(int applicationId,int TenantId,int SubscriptionId)
        {
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();

            var tenantId = (claims.Where(c => c.Type == Constants.TENANT_ID).FirstOrDefault() != null) ?
                Convert.ToInt32(claims.Where(c => c.Type == Constants.TENANT_ID).FirstOrDefault().Value) : 0;

            AuditLogHelper.sSection = "Mappings\\Subscription\\GetApplicationMessageType";

            IEnumerable<MessageType> Messagetype = ServiceFactory.Resolve<ISubscriptionService>().GetApplicationMessageList(applicationId, TenantId, SubscriptionId);

            return Messagetype;
        }


        [Route("GetApplicationByTenant/{TenantId}", Name = "GetApplicationByTenant")]
        [HttpGet]
        public IEnumerable<ApplicationMappingDTO> GetApplicationByTenant(int TenantId)
        {
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();

            var tenantId = (claims.Where(c => c.Type == Constants.TENANT_ID).FirstOrDefault() != null) ?
                Convert.ToInt32(claims.Where(c => c.Type == Constants.TENANT_ID).FirstOrDefault().Value) : 0;

            AuditLogHelper.sSection = "Mappings\\Subscription\\GetApplicationByTenant";

            IEnumerable<ApplicationMappingDTO> ApplicationList = ServiceFactory.Resolve<ISubscriptionService>().GetApplicationByTenant(TenantId);

            return ApplicationList;
        }




        [Route("GetMessageTypeDetails/{MessageTypeId}", Name = "GetMessageTypeDetails")]
        [HttpGet]
        public IEnumerable<MessageType> GetMessageTypeDetails(int MessageTypeId)
        {
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();

            var tenantId = (claims.Where(c => c.Type == Constants.TENANT_ID).FirstOrDefault() != null) ?
                Convert.ToInt32(claims.Where(c => c.Type == Constants.TENANT_ID).FirstOrDefault().Value) : 0;

            AuditLogHelper.sSection = "Mappings\\Subscription\\GetMessageType";

            IEnumerable < MessageType> Messagetype = ServiceFactory.Resolve<ISubscriptionService>().GetMessageType(MessageTypeId);

            return Messagetype;
        }


        [Route("Delete", Name = "DeleteSubscription")]
        [HttpPost]
        [CustomAuthorize("SuperAdmin", "Admin")]
        public int Delete([FromBody] int id)
        {
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();

            var tenantId = (claims.Where(c => c.Type == Constants.TENANT_ID).FirstOrDefault() != null) ?
                Convert.ToInt32(claims.Where(c => c.Type == Constants.TENANT_ID).FirstOrDefault().Value) : 0;

            AuditLogHelper.sSection = "Mappings\\Subscription\\DeleteSubscription";
            return ServiceFactory.Resolve<ISubscriptionService>().DeleteSubscription(id, tenantId);
        }


        [Route("ConfirmDeleteSubscription", Name = "ConfirmDeleteSubscription")]
        [HttpPost]
        [CustomAuthorize("SuperAdmin", "Admin")]
        public int ConfirmDeleteLocation([FromBody] int id)
        {
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();

            var tenantId = (claims.Where(c => c.Type == Constants.TENANT_ID).FirstOrDefault() != null) ?
                Convert.ToInt32(claims.Where(c => c.Type == Constants.TENANT_ID).FirstOrDefault().Value) : 0;

            AuditLogHelper.sSection = "Mappings\\Subscription\\DeleteSubscription";
            return ServiceFactory.Resolve<ISubscriptionService>().ConfirmDeleteSubscription(id, tenantId);
        }



    }
}
