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
    [RoutePrefix("Locations")]

    [CustomAuthorize]
    public class LocationMappingsController : ApiController
    {
        [Route("GetLocations/{LVISAbeid:maxlength(100)}", Name = "GetLocations")]
        [HttpGet]
        public IEnumerable<LocationsMappings> Get(String LVISAbeid)
        {
           
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();

            var tenantId = (claims.Where(c => c.Type == Constants.TENANT_ID).FirstOrDefault() != null) ?
                Convert.ToInt32(claims.Where(c => c.Type == Constants.TENANT_ID).FirstOrDefault().Value) : 0;
     
            AuditLogHelper.sSection = "Mappings\\Customers\\Locations";
            IEnumerable<LocationsMappings> Locations = ServiceFactory.Resolve<ICustomerMappingService>().GetLocations(LVISAbeid, tenantId);

            return Locations;
        }


        [Route("GetLocationsbyTenant", Name = "GetLocationsbyTenant")]
        [HttpGet]
        public IEnumerable<LocationsMappings> GetLocationsbyTenant()
        {
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();

            var tenantId = (claims.Where(c => c.Type == Constants.TENANT_ID).FirstOrDefault() != null) ?
                Convert.ToInt32(claims.Where(c => c.Type == Constants.TENANT_ID).FirstOrDefault().Value) : 0;

            AuditLogHelper.sSection = "Mappings\\Customers\\GetLocationsbyTenant";
            IEnumerable<LocationsMappings> Locations = ServiceFactory.Resolve<ICustomerMappingService>().GetLocations(tenantId);

            return Locations;
        }

        [Route("")]
        [HttpPost]
        [CustomAuthorize("SuperAdmin", "Admin")]
        public LocationsMappings Post(LocationsMappings value)
        {
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();

            var tenantId = (claims.Where(c => c.Type == Constants.TENANT_ID).FirstOrDefault() != null) ?
                Convert.ToInt32(claims.Where(c => c.Type == Constants.TENANT_ID).FirstOrDefault().Value) : 0;

            var userId = (claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault() != null) ?
             Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault().Value) : 0;

            AuditLogHelper.sSection = "Mappings\\Customers\\Locations\\post";
            return ServiceFactory.Resolve<ICustomerMappingService>().AddLocation(value, tenantId, userId);
        }

        [Route("Update")]
        [HttpPost]
        [CustomAuthorize("SuperAdmin", "Admin")]
        public int UpdateUserDetails(LocationsMappings value)
        {
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();

            var tenantId = (claims.Where(c => c.Type == Constants.TENANT_ID).FirstOrDefault() != null) ?
                Convert.ToInt32(claims.Where(c => c.Type == Constants.TENANT_ID).FirstOrDefault().Value) : 0;

            var userId = (claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault() != null) ?
             Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault().Value) : 0;

            AuditLogHelper.sSection = "Mappings\\Customers\\Locations\\Update";
            return ServiceFactory.Resolve<ICustomerMappingService>().UpdateLocation(value, tenantId, userId);
        }

        [Route("Delete", Name = "DeleteBranch")]
        [HttpPost]
        [CustomAuthorize("SuperAdmin", "Admin")]
        public int Delete([FromBody] int id)
        {
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();

            var tenantId = (claims.Where(c => c.Type == Constants.TENANT_ID).FirstOrDefault() != null) ?
                Convert.ToInt32(claims.Where(c => c.Type == Constants.TENANT_ID).FirstOrDefault().Value) : 0;

            AuditLogHelper.sSection = "Mappings\\Customers\\Locations\\DeleteLocation";
            return ServiceFactory.Resolve<ICustomerMappingService>().DeleteLocation(id, tenantId);
        }


        [Route("ConfirmDeleteLocation", Name = "ConfirmDeleteLocation")]
        [HttpPost]
        [CustomAuthorize("SuperAdmin", "Admin")]
        public int ConfirmDeleteLocation([FromBody] int id)
        {
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();

            var tenantId = (claims.Where(c => c.Type == Constants.TENANT_ID).FirstOrDefault() != null) ?
                Convert.ToInt32(claims.Where(c => c.Type == Constants.TENANT_ID).FirstOrDefault().Value) : 0;

            AuditLogHelper.sSection = "Mappings\\Customers\\Locations\\DeleteLocation";
            return ServiceFactory.Resolve<ICustomerMappingService>().ConfirmDeleteLocation(id, tenantId);
        }
        
        [Route("BulkImport",Name = "BulkImport")]
        [HttpPost]
        public string[] Post ([FromBody] IList<DC.BulkImportDTO> value)
        {
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();

            var tenantId = (claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault() != null) ?
                Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value) : 0;

            var userId = (claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault() != null) ?
             Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault().Value) : 0;

            AuditLogHelper.sSection = "Mappings\\Customers\\Locations\\BulkImport";

            return ServiceFactory.Resolve<ICustomerMappingService>().BulkImport(value, tenantId, userId);

        }

        [Route("GetServicePreferenceLocation/{locationid}", Name = "GetServicePreferenceLocation")]
        [HttpGet]
        public List<DC.ServicePreference> GetServicePreferenceLocation(int locationid)
        {

            AuditLogHelper.sSection = "Mappings\\Customers\\Locations";
            return ServiceFactory.Resolve<ICustomerMappingService>().GetServicePreferenceLocation(locationid);

        }

    }
}