using FA.LVIS.Tower.Data;
using FA.LVIS.Tower.DataContracts;
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
    [RoutePrefix("Contacts")]

    [CustomAuthorize]
    public class ContactMappingsController : ApiController
    {        
        [HttpPost]
        [CustomAuthorize("SuperAdmin", "Admin")]
        public ContactMappings Post(ContactMappings value)
        {
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();

            var userId = (claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault() != null) ?
            Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault().Value) : 0;

            var tenantId = (claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault() != null) ?
            Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value) : 0;

            AuditLogHelper.sSection = "Mappings\\Customers\\Contacts\\post";
            return ServiceFactory.Resolve<ICustomerMappingService>().AddContact(value, userId, tenantId);
        }
                
        [HttpPost]
        [CustomAuthorize("SuperAdmin", "Admin")]
        public int UpdateContactDetails(ContactMappings value)
        {
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();

            var userId = (claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault() != null) ?
             Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault().Value) : 0;

            var tenantId = (claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault() != null) ?
            Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value) : 0;

            AuditLogHelper.sSection = "Mappings\\Customers\\Contacts\\Update";
            return ServiceFactory.Resolve<ICustomerMappingService>().UpdateContact(value, userId, tenantId);
        }

        [Route("Delete", Name = "DeleteContact")]
        [HttpPost]
        [CustomAuthorize("SuperAdmin", "Admin")]
        public int Delete([FromBody] int id)
        {
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();

            AuditLogHelper.sSection = "Mappings\\Customers\\Contacts\\DeleteContact";
            return ServiceFactory.Resolve<ICustomerMappingService>().DeleteContact(id);
        }


        [Route("ConfirmDeleteContact", Name = "ConfirmDeleteContact")]
        [HttpPost]
        [CustomAuthorize("SuperAdmin", "Admin")]
        public int ConfirmDeleteContact([FromBody] int id)
        {
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();

            AuditLogHelper.sSection = "Mappings\\Customers\\Contacts\\DeleteContact";
            return ServiceFactory.Resolve<ICustomerMappingService>().ConfirmDeleteContact(id);
        }

        [Route("GetServicePreferenceContact/{contactid}", Name = "GetServicePreferenceContact")]
        [HttpGet]
        public List<DC.ServicePreference> GetServicePreferenceContact(int contactid)
        {

            AuditLogHelper.sSection = "Mappings\\Customers\\Contacts";
            return ServiceFactory.Resolve<ICustomerMappingService>().GetServicePreferenceContact(contactid);

        }

    }
}