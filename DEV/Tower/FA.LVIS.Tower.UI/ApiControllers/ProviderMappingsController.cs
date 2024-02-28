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
    [RoutePrefix("Providers")]

    [CustomAuthorize]
    public class ProviderMappingsController:ApiController
    {
        [Route("GetProviders", Name = "GetProviders")]
        [HttpGet]
        public IEnumerable<DC.Provider> GetProviders()
        {
            AuditLogHelper.sSection = "Mappings\\Providers\\GetProviders";
            IProviderMappingService ProviderMapping = ServiceFactory.Resolve<IProviderMappingService>();

            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();

            var tenantId = (claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault() != null) ? 
                Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value) : 0;

            List<DC.Provider> newList = new List<DC.Provider>();

            newList = ProviderMapping.GetProviderMappings(tenantId);
            
            return newList;

        }

        [Route("GetProvidersForContactProvider/{tenantId}", Name = "GetProvidersForContactProvider")]
        [HttpGet]
        public IEnumerable<DC.Provider> GetProvidersForContactProvider(int tenantId)
        {
            AuditLogHelper.sSection = "Mappings\\Providers\\GetProviders";
            IProviderMappingService ProviderMapping = ServiceFactory.Resolve<IProviderMappingService>();

            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();

            List<DC.Provider> newList = new List<DC.Provider>();

            newList = ProviderMapping.GetProviderMappings(tenantId);

            if (newList.Count() > 0)
            {
                newList = newList
                    .Where(sel => sel.ProviderName != null && sel.ProviderName!=string.Empty).ToList();
            }

            return newList;
        }


        //[Route("GetProvidersList", Name = "GetProvidersList")]
        //[HttpGet]
        //public IEnumerable<DC.Provider> GetProvidersList()
        //{
        //    AuditLogHelper.sSection = "Mappings\\Providers";
        //    IProviderMappingService ProviderMapping = ServiceFactory.Resolve<IProviderMappingService>();            
        //    List<DC.Provider> newList = new List<DC.Provider>();
        //    newList = ProviderMapping.GetProviderMappings(tenantId);
        //    return newList;
        //}

        ////[Route("")]
        [HttpPost]
        [CustomAuthorize("SuperAdmin", "Admin")]
        public DC.Provider Post(DC.Provider value)
        {
            AuditLogHelper.sSection = "Mappings\\Providers\\Post";
            IProviderMappingService ProviderMapping = ServiceFactory.Resolve<IProviderMappingService>();
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();
            var userId = (claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault() != null) ?
             Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault().Value) : 0;
            

            return ProviderMapping.AddProvider(value, userId, value.TenantId);
        }

        ////[Route("Update")]
        [HttpPost]
        [CustomAuthorize("SuperAdmin", "Admin")]
        public DC.Provider UpdateProviderDetails(DC.Provider value)
        {
            AuditLogHelper.sSection = "Mappings\\Providers\\Update";
            IProviderMappingService ProviderMapping = ServiceFactory.Resolve<IProviderMappingService>();
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();
            var userId = (claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault() != null) ?
             Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault().Value) : 0;

            return ProviderMapping.UpdateProvider(value, userId, value.TenantId);
        }
    
        [Route("DeleteProvider", Name = "DeleteProvider")]
        [HttpPost]
        [CustomAuthorize("SuperAdmin", "Admin")]
        public int DeleteProvider([FromBody] int id)
        {
            AuditLogHelper.sSection = "Mappings\\Providers\\DeleteProviders";
            IProviderMappingService ProviderMapping = ServiceFactory.Resolve<IProviderMappingService>();
            return ProviderMapping.DeleteOffice(id);
        }

        [Route("ConfirmDeleteProvider", Name = "ConfirmDeleteProvider")]
        [HttpPost]
        [CustomAuthorize("SuperAdmin", "Admin")]
        public int ConfirmDeleteProvider([FromBody] int id)
        {
            AuditLogHelper.sSection = "Mappings\\Providers\\DeleteProviders";
            IProviderMappingService ProviderMapping = ServiceFactory.Resolve<IProviderMappingService>();
            return ProviderMapping.ConfirmDelete(id);
        }

        [Route("GetListApplications", Name = "GetListApplications")]
        [HttpGet]
        public IEnumerable<DC.ApplicationMappingDTO> GetApplicationList()
        {
            AuditLogHelper.sSection = "Mappings\\Providers\\GetListApplications";
            IProviderMappingService offices = ServiceFactory.Resolve<IProviderMappingService>();           
            return offices.GetApplicationsList();
        }

        [Route("AddImportData")]
        [HttpPost]
        [CustomAuthorize("SuperAdmin", "Admin")]
        public DC.Provider[] AddImportData(DC.Provider[] values)
        {
            AuditLogHelper.sSection = "Mappings\\Providers\\AddImportData";
            IProviderMappingService ProviderMapping = ServiceFactory.Resolve<IProviderMappingService>();
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();
            var userId = (claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault() != null) ?
             Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault().Value) : 0;

            var tenantId = (claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault() != null) ?
                Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value) : 0;

            return ProviderMapping.AddImportData(values, tenantId, userId);
        }

        [Route("GetProviderDetailsByID/{providerId}", Name = "GetProviderDetailsByID")]
        [HttpGet]
        public DC.Provider GetProviderDetailsByID(int providerId)
        {
            AuditLogHelper.sSection = "Mappings\\Providers\\GetProviderDetails";
            IProviderMappingService ProviderMapping = ServiceFactory.Resolve<IProviderMappingService>();
            DC.Provider newList = new DC.Provider();

            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();
            var tenantId = (claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault() != null) ?
                Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value) : 0;

            newList = ProviderMapping.GetProviderDetailsByID(tenantId, providerId);
            return newList;
        }
    }
}












