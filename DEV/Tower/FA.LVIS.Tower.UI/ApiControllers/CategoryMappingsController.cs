
using System.Collections.Generic;
using System.Web.Http;
using DC = FA.LVIS.Tower.DataContracts;
using FA.LVIS.Tower.Services;
using FA.LVIS.Tower.Data;
using System;
using System.Linq;
using FA.LVIS.Tower.UI.ApiControllers.Filters;

namespace FA.LVIS.Tower.UI.ApiControllers
{
    [ValidateAntiForgeryTokenFilter]
    [RoutePrefix("Categories")]

    [CustomAuthorize]
    public class CategoryMappingsController : ApiController
    {
        [Route("GetCategories", Name = "GetCategories")]
        [HttpGet]
        public IEnumerable<DC.CategoryMapping> GetCategories()
        {
            AuditLogHelper.sSection = "Mappings\\Category\\GetCategories";
            ICategoryMappingService Groups = ServiceFactory.Resolve<ICategoryMappingService>();

            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();

            var tenantId = (claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault() != null) ?
                Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value) : 0;

            return Groups.GetCategoryMappings(tenantId);
        }
        [HttpPost]
        [CustomAuthorize("SuperAdmin", "Admin")]
        public DC.CategoryMapping AddCategory([FromBody]DC.CategoryMapping value)
        {
            AuditLogHelper.sSection = "Mappings\\Category\\AddCategory";
            ICategoryMappingService Groups = ServiceFactory.Resolve<ICategoryMappingService>();

            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();

            var tenantId = (claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault() != null) ?
                Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value) : 0;

            var userId = (claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault() != null) ?
             Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault().Value) : 0;

            return Groups.AddCategory(value, tenantId, userId);
        }
        [HttpPost]
        [CustomAuthorize("SuperAdmin", "Admin")]
        public int UpdateUserDetails(DC.CategoryMapping value)
        {
            AuditLogHelper.sSection = "Mappings\\Category\\Update";
            ICategoryMappingService Groups = ServiceFactory.Resolve<ICategoryMappingService>();
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();
            var userId = (claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault() != null) ?
             Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault().Value) : 0;
            return Groups.UpdateCategory(value, userId);
        }

        [Route("DeleteCategory", Name = "DeleteCategory")]
        [HttpPost]
        [CustomAuthorize("SuperAdmin", "Admin")]
        public int DeleteCategory([FromBody]int value)
        {
            ICategoryMappingService Groups = ServiceFactory.Resolve<ICategoryMappingService>();
            AuditLogHelper.sSection = "Mappings\\Category\\DeleteCategory";
            return Groups.DeleteCategory(value);
        }



        [Route("ConfirmDeleteCategory", Name = "ConfirmDeleteCategory")]
        [HttpPost]
        [CustomAuthorize("SuperAdmin", "Admin")]
        public int ConfirmDeleteCategory([FromBody]int value)
        {
            ICategoryMappingService Groups = ServiceFactory.Resolve<ICategoryMappingService>();
            AuditLogHelper.sSection = "Mappings\\Category\\DeleteCategory";
            return Groups.ConfirmDelete(value);
        }

        // PUT: api/ GroupMappings/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/ GroupMappings/5
        public void Delete(int id)
        {
        }
    }
 }