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
    [RoutePrefix("FASTOffices")]

    [CustomAuthorize]
    public class FASTOfficeMappingsController:ApiController
    {
        [Route("GetFASTOfficeMapping", Name = "GetFASTOfficeMapping")]
        [HttpGet]
        public IEnumerable<DC.FASTOfficeMap> GetFASTOfficeMapping()
        {
            AuditLogHelper.sSection = "Mappings\\FASTOfficeMap\\GetFASTOfficeMapping";
            IFASTOfficeMappingService OfficeMapping = ServiceFactory.Resolve<IFASTOfficeMappingService>();
            List<DC.FASTOfficeMap> newList = new List<DC.FASTOfficeMap>();

            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();
            var tenantId = (claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault() != null) ?
                Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value) : 0;

            newList = OfficeMapping.GetFASTOfficeMappings(tenantId);
            return newList;
        }

        [Route("GetFASTOfficeDetailsByID/{fastOfficeMapId}", Name = "GetFASTOfficeDetailsByID")]
        [HttpGet]
        public DC.FASTOfficeMap GetFASTOfficeDetailsByID(int fastOfficeMapId)
        {
            AuditLogHelper.sSection = "Mappings\\FASTOfficeMap\\GetFASTOfficeMapping";
            IFASTOfficeMappingService OfficeMapping = ServiceFactory.Resolve<IFASTOfficeMappingService>();
            DC.FASTOfficeMap newList = new DC.FASTOfficeMap();

            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();
            var tenantId = (claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault() != null) ?
                Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value) : 0;

            newList = OfficeMapping.GetFASTOfficeDetailsByID(tenantId, fastOfficeMapId);
            return newList;
        }

        [Route("GetFASTOfficeMappingsprovider/{Providerid}", Name = "GetFASTOfficeMappingsprovider")]
        [HttpGet]
        public IEnumerable<DC.FASTOfficeMap> GetFASTOfficeMappingsprovider(int ProviderId)
        {
            AuditLogHelper.sSection = "Mappings\\FASTOfficeMap\\GetFASTOfficeMappingsprovider";
            IFASTOfficeMappingService OfficeMapping = ServiceFactory.Resolve<IFASTOfficeMappingService>();
            List<DC.FASTOfficeMap> newList = new List<DC.FASTOfficeMap>();

            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();
            var tenantId = (claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault() != null) ?
                Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value) : 0;

            newList = OfficeMapping.GetFASTOfficeMappingsprovider(tenantId, ProviderId);
            return newList;
        }

        [Route("AddFastOffice",Name = "AddFastOffice")]
        [HttpPost]
        [CustomAuthorize("SuperAdmin", "Admin")]
        public DC.FASTOfficeMap Post(DC.FASTOfficeMap value)
        {
            AuditLogHelper.sSection = "Mappings\\FASTOfficeMap\\AddFastOffice";
            IFASTOfficeMappingService OfficeMapping = ServiceFactory.Resolve<IFASTOfficeMappingService>();

            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();
            var userId = (claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault() != null) ?
             Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault().Value) : 0;

            return OfficeMapping.AddFASTOffice(value, userId);
        }

        [Route("UpdateFastOffice",Name = "UpdateFastOffice")]
        [HttpPost]
        [CustomAuthorize("SuperAdmin", "Admin")]
        public DC.FASTOfficeMap UpdateFastOfficeDetails(DC.FASTOfficeMap value)
        {
            AuditLogHelper.sSection = "Mappings\\FASTOfficeMap\\UpdateFastOffice";
            IFASTOfficeMappingService OfficeMapping = ServiceFactory.Resolve<IFASTOfficeMappingService>();

            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();
            var userId = (claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault() != null) ?
             Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault().Value) : 0;

            return OfficeMapping.UpdateFASTOffice(value, userId);
        }
    
        [Route("DeleteFASTOffice", Name = "DeleteFASTOffice")]
        [HttpPost]
        [CustomAuthorize("SuperAdmin", "Admin")]
        public int DeleteFASTOffice([FromBody] int id)
        {
            AuditLogHelper.sSection = "Mappings\\FASTOfficeMap\\DeleteFASTOffice";
            IFASTOfficeMappingService OfficeMapping = ServiceFactory.Resolve<IFASTOfficeMappingService>();
            return OfficeMapping.DeleteFASTOffice(id);
        }

        [Route("ConfirmDeleteFASTOffice", Name = "ConfirmDeleteFASTOffice")]
        [HttpPost]
        [CustomAuthorize("SuperAdmin", "Admin")]
        public int ConfirmDeleteFASTOffice([FromBody] int id)
        {
            AuditLogHelper.sSection = "Mappings\\FASTOfficeMap\\DeleteFASTOffice";
            IFASTOfficeMappingService OfficeMapping = ServiceFactory.Resolve<IFASTOfficeMappingService>();
            return OfficeMapping.ConfirmDeleteFASTOffice(id);
        }

        [Route("GetProviderList", Name = "GetProviderList")]
        [HttpGet]
        public IEnumerable<DC.Provider> GetProviderList()
        {
            AuditLogHelper.sSection = "Mappings\\FASTOfficeMap\\GetProviderList";
            IFASTOfficeMappingService offices = ServiceFactory.Resolve<IFASTOfficeMappingService>();

            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();
            var tenantId = (claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault() != null) ?
                Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value) : 0;

            return offices.GetProviderList(tenantId);
        }

        [Route("GetLocationsList", Name = "GetLocationsList")]
        [HttpGet]
        public IEnumerable<DC.LocationsMappings> GetLocationList()
        {
            AuditLogHelper.sSection = "Mappings\\FASTOfficeMap\\GetLocationsList";
            IFASTOfficeMappingService offices = ServiceFactory.Resolve<IFASTOfficeMappingService>();

            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();
            var tenantId = (claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault() != null) ?
                Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value) : 0;

            return offices.GetLocationList(tenantId);
        }

        [Route("GetLocationsListByCustId/{customerId}", Name = "GetLocationsListByCustId")]
        [HttpGet]
        public IEnumerable<DC.LocationsMappings> GetLocationsListByCustId(int customerId)
        {
            AuditLogHelper.sSection = "Mappings\\FASTOfficeMap\\GetLocationsList";
            IFASTOfficeMappingService offices = ServiceFactory.Resolve<IFASTOfficeMappingService>();

            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();
            var tenantId = (claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault() != null) ?
                Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value) : 0;

            return offices.GetLocationsListByCustId(customerId, tenantId);
        }

        [Route("GetExternalId/{providerId}", Name = "GetExternalId")]
        [HttpGet]
        public string GetExternalId(int providerId)
        {
            AuditLogHelper.sSection = "Mappings\\FASTOfficeMap\\GetExternalId";
            IFASTOfficeMappingService offices = ServiceFactory.Resolve<IFASTOfficeMappingService>();

            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();
            var tenantId = (claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault() != null) ?
                Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value) : 0;

            return offices.GetExternalIdByProviderId(tenantId, providerId);
        }

        [Route("GetOfficeDetails/{StateFipsId:maxlength(50)}/{CountyFipsid:maxlength(50)}/{TitlePriority:bool}", Name = "GetOfficeDetails")]
        [HttpGet]
        public IEnumerable<DC.FASTOfficeMap> GetOfficeDetails(string StateFipsId, string CountyFipsid, bool TitlePriority)
        {
            AuditLogHelper.sSection = "Mappings\\FASTOfficeMap\\GetOfficeDetails";
            IFASTOfficeMappingService offices = ServiceFactory.Resolve<IFASTOfficeMappingService>();
            List<DC.FASTOfficeMap> newList = new List<DC.FASTOfficeMap>();

            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();
            var tenantId = (claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault() != null) ?
                Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value) : 0;

            newList = offices.GetOfficeDetails(StateFipsId, CountyFipsid, TitlePriority, tenantId);

            return newList;
        }

        [Route("GetTitleEscrowOfficers/{officeId}/{funcType}", Name = "GetEscrowOfficers")]
        [HttpGet]
        public IEnumerable<DC.UserProfile> GetTitleEscrowOfficers(int officeId, int funcType)
        {
            AuditLogHelper.sSection = "Mappings\\FASTOfficeMap\\GetEscrowOfficers";
            IFASTOfficeMappingService offices = ServiceFactory.Resolve<IFASTOfficeMappingService>();
            List<DC.UserProfile> newList = new List<DC.UserProfile>();

            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();
            var tenantId = (claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault() != null) ?
                Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value) : 0;

            newList = offices.GetTitleEscrowOfficers(officeId, funcType, tenantId);

            return newList;
        }
        [Route("GetAuthenticationToken", Name = "GetAuthenticationToken")]
        [HttpGet]
        public IEnumerable<DC.TypeCodeDTO> GetAuthenticationToken()
        {
            AuditLogHelper.sSection = "Mappings\\FASTOfficeMap\\GetAuthenticationToken";
            IFASTOfficeMappingService offices = ServiceFactory.Resolve<IFASTOfficeMappingService>();
         
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();
            var tenantId = (claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault() != null) ?
                Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value) : 0;

       
            return offices.GetAuthenticationTokens(tenantId);
        }

        //[Route("GetTitleOfficers/{officeId}", Name = "GetTitleOfficers")]
        //[HttpGet]
        //public IEnumerable<DC.UserProfile> GetTitleOfficers(int officeId)
        //{
        //    AuditLogHelper.sSection = "Mappings\\FASTOfficeMap\\GetTitleOfficers";
        //    IFASTOfficeMappingService offices = ServiceFactory.Resolve<IFASTOfficeMappingService>();
        //    List<DC.UserProfile> newList = new List<DC.UserProfile>();

        //    var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();
        //    var tenantId = (claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault() != null) ?
        //        Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value) : 0;

        //    newList = offices.GetTitleOfficers(officeId, tenantId);

        //    return newList;
        //}
    }
}












