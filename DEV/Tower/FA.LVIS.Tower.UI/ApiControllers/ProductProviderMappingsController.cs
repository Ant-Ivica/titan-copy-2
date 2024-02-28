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
    [RoutePrefix("ProductProvider")]

    [CustomAuthorize]
    public class ProductProviderMappingsController : ApiController
    {
        [Route("GetProductProviders/{providerId:maxlength(10)}", Name = "GetProductProviders")]
        [HttpGet]
        public IEnumerable<DC.ProductProviderMap> Get(string providerId)
        {
            AuditLogHelper.sSection = "Mappings\\Providers\\ProductProviders";
            IProductProviderMappingService ProductProviderMapping = ServiceFactory.Resolve<IProductProviderMappingService>();
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();

            var tenantId = (claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault() != null) ?
                Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value) : 0;

            List<DC.ProductProviderMap> newList = new List<DC.ProductProviderMap>();

            newList = ProductProviderMapping.GetProductProviderMappings(providerId, tenantId);

            return newList;

        }

        [HttpPost]
        [CustomAuthorize("SuperAdmin", "Admin")]
        public DC.ProductProviderMap AddProductProvider(DC.ProductProviderMap value) {
            AuditLogHelper.sSection = "Mappings\\Providers\\ProductProviders\\Post";
            IProductProviderMappingService ProductProviderMapping = ServiceFactory.Resolve<IProductProviderMappingService>();
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();
            var userId = (claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault() != null) ?
             Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault().Value) : 0;

            var tenantId = (claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault() != null) ?
                Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value) : 0;

            return ProductProviderMapping.AddProductProvider(value, userId, tenantId);
        }

        [HttpPost]
        [CustomAuthorize("SuperAdmin", "Admin")]
        public DC.ProductProviderMap UpdateProductProvider(DC.ProductProviderMap value)
        {
            AuditLogHelper.sSection = "Mappings\\Providers\\ProductProviders\\Post";
            IProductProviderMappingService ProductProviderMapping = ServiceFactory.Resolve<IProductProviderMappingService>();
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();
            var userId = (claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault() != null) ?
             Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault().Value) : 0;

            var tenantId = (claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault() != null) ?
                Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value) : 0;

            return ProductProviderMapping.UpdateProductProvider(value, userId, tenantId);
        }

        [Route("DeleteProductProvider/{productProviderId:int:min(1)}", Name = "DeleteProductProvider")]
        [HttpPost]
        [CustomAuthorize("SuperAdmin", "Admin")]
        public int DeleteProductProvider(int productProviderId)
        {            
            AuditLogHelper.sSection = "Mappings\\Providers\\ProductProviders\\DeleteProductProviders";
            IProductProviderMappingService ProductProviderMapping = ServiceFactory.Resolve<IProductProviderMappingService>();
            return ProductProviderMapping.DeleteProductProvider(productProviderId);
        }

    }
}