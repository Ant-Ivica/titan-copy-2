using FA.LVIS.Tower.Data;
using FA.LVIS.Tower.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using DC = FA.LVIS.Tower.DataContracts;
using FA.LVIS.Tower.UI.ApiControllers.Filters;

namespace FA.LVIS.Tower.UI.ApiControllers
{
    [ValidateAntiForgeryTokenFilter]
    [RoutePrefix("CustomerRegistration")]

    [CustomAuthorize]
    public class CustomerRegistrationController : ApiController
    {
        [Route("GetCustomerRegistration", Name = "GetCustomerRegistration")]
        [HttpGet]   
        public IEnumerable<DC.CustomerRegistrationDTO> Get()
        {
            AuditLogHelper.sSection = "Mappings\\Customers\\GetCustomers";
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();

            var tenantId = (claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value != null) ?
                Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value) : 0;

            return ServiceFactory.Resolve<ICustomerRegistrationService>().GetCustomerRegistrations();
        }

        [Route("GetCustomerRegistration/{emailid:maxlength(100)}/", Name = "GetCustomerRegistrationbyEmail")]
        [HttpGet]
        public IEnumerable<DC.CustomerRegistrationDTO> GetCustomerRegistration([FromUri]string emailid)
        {
            AuditLogHelper.sSection = "Mappings\\Customers\\GetCustomers";
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();

            var tenantId = (claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value != null) ?
                Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value) : 0;

            return ServiceFactory.Resolve<ICustomerRegistrationService>().GetCustomerRegistrations(emailid);
        }

        [Route("GetCustomerRegistrationStatus", Name = "GetCustomerRegistrationStatus")]
        [HttpGet]
        // GET: api/LenderMappings
        public IEnumerable<DC.Status> GetCustomerRegistrationStatus()
        {
            AuditLogHelper.sSection = "Mappings\\Customers\\GetCustomerRegistrationStatus";
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();

            var tenantId = (claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value != null) ?
                Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value) : 0;


            return ServiceFactory.Resolve<ICustomerRegistrationService>().GetStatus();
        }

        [HttpPost]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public DC.CustomerRegistrationDTO AddCustomer([FromBody]DC.CustomerRegistrationDTO value)
        {
            AuditLogHelper.sSection = "Mappings\\Customers\\AddCustomerRegistrationDTO";
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();

            var tenantId = (claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault() != null) ?
                Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value) : 0;

            var userId = (claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault() != null) ?
             Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault().Value) : 0;

            var role = claims.Where(c => c.Type == ClaimTypes.Role).ToList().FirstOrDefault();

            AuditLogHelper.sSection = "Mappings\\CustomerRegistrationDTO";
            ICustomerRegistrationService customer = ServiceFactory.Resolve<ICustomerRegistrationService>();
            return customer.AddCustomerRegistrationDTO(value, tenantId, userId);
        }

    }
}