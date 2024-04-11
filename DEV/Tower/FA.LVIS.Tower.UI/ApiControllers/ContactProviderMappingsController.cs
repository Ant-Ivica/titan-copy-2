using FA.LVIS.Tower.Data;
using FA.LVIS.Tower.DataContracts;
using FA.LVIS.Tower.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using DC = FA.LVIS.Tower.DataContracts;

using FA.LVIS.Tower.UI.ApiControllers.Filters;

namespace FA.LVIS.Tower.UI.ApiControllers
{
    [ValidateAntiForgeryTokenFilter]
    [RoutePrefix("ContactProvider")]

    [CustomAuthorize]
    public class ContactProviderMappingsController : ApiController
    {
        // GET: ContactProviderMappings

        [HttpPost]
        [CustomAuthorize("SuperAdmin", "Admin")]
        public ContactProviderMappings Post(DC.ContactProviderMappings value)
        {            
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();

            var userId = (claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault() != null) ?
            Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault().Value) : 0;

            AuditLogHelper.sSection = "Mappings\\Customers\\ContactProvider\\post";
            return ServiceFactory.Resolve<ICustomerMappingService>().AddContactProvider(value,userId, value.TenantId);
        }


        [HttpPost]
        [CustomAuthorize("SuperAdmin", "Admin")]
        public DC.ContactProviderMappings UpdateContactProviderDetails(ContactProviderMappings value)
        {
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();

            var userId = (claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault() != null) ?
             Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault().Value) : 0;
        
            AuditLogHelper.sSection = "Mappings\\Customers\\ContactProvider\\Update";
            return ServiceFactory.Resolve<ICustomerMappingService>().UpdateContactProvider(value, userId, value.TenantId);
        }

        [Route("Delete", Name = "DeleteContactProvider")]
        [HttpPost]
        [CustomAuthorize("SuperAdmin", "Admin")]
        public int Delete([FromBody] int id)
        {
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();

            AuditLogHelper.sSection = "Mappings\\Customers\\Contacts\\DeleteContact";
            return ServiceFactory.Resolve<ICustomerMappingService>().DeleteContactProvider(id);
        }


        [Route("ConfirmDeleteContactProvider", Name = "ConfirmDeleteContactProvider")]
        [HttpPost]
        [CustomAuthorize("SuperAdmin", "Admin")]
        public int ConfirmDeleteContactProvider([FromBody] int id)
        {
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();

            AuditLogHelper.sSection = "Mappings\\Customers\\Contacts\\DeleteContact";
            return ServiceFactory.Resolve<ICustomerMappingService>().ConfirmDeleteContactProvider(id);
        }     
    }
}