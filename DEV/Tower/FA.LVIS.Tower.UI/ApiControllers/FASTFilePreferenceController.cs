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
    [RoutePrefix("FilePreferences")]

    [CustomAuthorize]
    public class FASTFilePreferenceController : ApiController
    {
        //[Route("Delete", Name = "DeleteFASTFilePreference")]
        //[HttpPost]
        //public int Delete([FromBody] int id)
        //{
        //    AuditLogHelper.sSection = "Mappings\\FAST FilePreference\\DeleteFASTFilePreference";
        //    IFASTFilePreferenceService FilePref = ServiceFactory.Resolve<IFASTFilePreferenceService>();
        //    return FilePref.Delete(id);
        //}

        [Route("ConfirmFASTFilePreferenceDelete", Name = "ConfirmFASTFilePreferenceDelete")]
        [HttpPost]
        [CustomAuthorize("SuperAdmin", "Admin")]
        public int ConfirmFASTFilePreferenceDelete([FromBody] int id)
        {
            AuditLogHelper.sSection = "Mappings\\FAST FilePreference\\ConfirmFASTFilePreferenceDelete";
            IFASTFilePreferenceService FilePref = ServiceFactory.Resolve<IFASTFilePreferenceService>();
            return FilePref.ConfirmDelete(id);
        }

        [Route("GetBusinessFASTProgramTypeList/{RegionId}", Name = "GetBusinessFASTProgramTypeList")]
        [HttpGet]
        public IEnumerable<DC.BusinessProgramTypeMappingDTO> GetBusinessFASTProgramTypeList(int regionId)
        {
            AuditLogHelper.sSection = "Mappings\\FASTFilePreferences\\GetBusinessFASTProgramTypeList";
            IFASTFilePreferenceService offices = ServiceFactory.Resolve<IFASTFilePreferenceService>();

            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();
            var tenantId = (claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault() != null) ?
                Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value) : 0;

            return offices.GetBusinessFASTProgramTypeList(regionId);
        }

        [Route("GetFastFilePreferenceDetailsByID/{mapid}", Name = "GetFastFilePreferenceDetailsByID")]
        [HttpGet]
        public DC.FASTFilePreferenceDTO GetFastFilePreferenceDetailsByID(int mapid)
        {
            AuditLogHelper.sSection = "Mappings\\FASTFilePreferences\\GetFastFilePreferenceDetailsByID";
            IFASTFilePreferenceService FilePref = ServiceFactory.Resolve<IFASTFilePreferenceService>();
            List<DC.FASTFilePreferenceDTO> newList = new List<DC.FASTFilePreferenceDTO>();

            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();
            var tenantId = (claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault() != null) ?
                Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value) : 0;

            return FilePref.GetFASTFilePreferencesDetailsById(tenantId, mapid);
        }

        [Route("UpdateFastFile")]
        [HttpPost]
        [CustomAuthorize("SuperAdmin", "Admin")]
        public DC.FASTFilePreferenceDTO UpdateFastFile(DC.FASTFilePreferenceDTO value)
        {
            AuditLogHelper.sSection = "Mappings\\FASTFilePreferences\\UpdateFastFile";
            IFASTFilePreferenceService offices = ServiceFactory.Resolve<IFASTFilePreferenceService>();

            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();
            var tenantId = (claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault() != null) ?
                Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value) : 0;

            var userId = (claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault() != null) ?
             Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault().Value) : 0;

            return offices.UpdateFastFile(value, tenantId, userId);

        }

        [Route("AddFastFile")]
        [HttpPost]
        [CustomAuthorize("SuperAdmin", "Admin")]
        public DC.FASTFilePreferenceDTO AddFastFile(DC.FASTFilePreferenceDTO value)
        {
            AuditLogHelper.sSection = "Mappings\\FASTFilePreferences\\AddFastFile";
            IFASTFilePreferenceService offices = ServiceFactory.Resolve<IFASTFilePreferenceService>();

            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();
            var tenantId = (claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault() != null) ?
                Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value) : 0;

            var userId = (claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault() != null) ?
             Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault().Value) : 0;

            return offices.AddFastFile(value, tenantId, userId);
        }

        [Route("GetLoanPurposeList", Name = "GetLoanPurposeList")]
        [HttpGet]
        public IEnumerable<DC.ExceptionStatus> GetLoanPurposeList()
        {
            AuditLogHelper.sSection = "Mappings\\FASTFilePreferences\\GetLoanPurposeList";
            IFASTFilePreferenceService offices = ServiceFactory.Resolve<IFASTFilePreferenceService>();

            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();
            var tenantId = (claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault() != null) ?
                Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value) : 0;
            return offices.GetLoanPurposeList();
        }

        [Route("GetStateFipsList", Name = "GetStateFipsList")]
        [HttpGet]
        public IEnumerable<DC.StateMappingDTO> GetStateFips()
        {
            AuditLogHelper.sSection = "Mappings\\FASTFilePreferences\\GetStateFipsList";
            IFASTFilePreferenceService offices = ServiceFactory.Resolve<IFASTFilePreferenceService>();

            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();
            var tenantId = (claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault() != null) ?
                Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value) : 0;
            return offices.GetStatesList();
        }

        [Route("GetCountyFipsList/{StateFips:maxlength(50)}", Name = "GetCountyFipsList")]
        [HttpGet]
        public IEnumerable<DC.CountyMappingDTO> GetCountyFips(string StateFips)
        {
            AuditLogHelper.sSection = "Mappings\\FASTFilePreferences\\GetCountyFipsList";
            IFASTFilePreferenceService offices = ServiceFactory.Resolve<IFASTFilePreferenceService>();

            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();
            var tenantId = (claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault() != null) ?
                Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value) : 0;
            return offices.GetCountyList(StateFips);
        }

        [Route("GetFastFilePreferenceDetails", Name = "GetFastFilePreferenceDetails")]
        [HttpGet]
        public IEnumerable<DC.FASTFilePreferenceDTO> GetFastFilePreferenceDetails()
        {
            AuditLogHelper.sSection = "Mappings\\FASTFilePreferences\\GetFastFilePreferenceDetails";
            IFASTFilePreferenceService FilePref = ServiceFactory.Resolve<IFASTFilePreferenceService>();
            List<DC.FASTFilePreferenceDTO> newList = new List<DC.FASTFilePreferenceDTO>();

            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();
            var tenantId = (claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault() != null) ?
                Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value) : 0;

            newList = FilePref.GetFASTFilePreferencesDetails(tenantId);
            return newList;
        }

        [Route("GetFilePreferences/{StateFipsId:maxlength(50)}/{CountyFipsid:maxlength(50)}/{LoanAmount:maxlength(19)}/{Regionid:maxlength(10)}", Name = "GetFilePreferences")]
        [HttpGet]
        public IEnumerable<DC.FASTFilePreferenceDTO> GetFilePreferences(string StateFipsId, string CountyFipsid, string loanAmount, string Regionid)
        {
            AuditLogHelper.sSection = "Mappings\\FASTFilePreferences\\GetFilePreferences";
            IFASTFilePreferenceService FilePreference = ServiceFactory.Resolve<IFASTFilePreferenceService>();
            List<DC.FASTFilePreferenceDTO> newList = new List<DC.FASTFilePreferenceDTO>();

            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();
            var tenantId = (claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault() != null) ?
                Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value) : 0;

            newList = FilePreference.GetFilePreferences(StateFipsId, CountyFipsid, loanAmount.ToString(), tenantId, Regionid);

            return newList;
        }

        [Route("GetProgramTypeList/{RegionId}", Name = "GetProgramTypeList")]
        [HttpGet]
        public IEnumerable<DC.ProgramTypeMappingDTO> GetProgramTypeList(int regionId)
        {
            AuditLogHelper.sSection = "Mappings\\FASTFilePreferences\\GetProgramTypeList";
            IFASTFilePreferenceService offices = ServiceFactory.Resolve<IFASTFilePreferenceService>();

            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();
            var tenantId = (claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault() != null) ?
                Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value) : 0;

            return offices.GetProgramTypeList(regionId);
        }

        [Route("GetProductList/{appId}", Name = "GetProductList")]
        [HttpGet]
        public IEnumerable<DC.ProductTypeMappingDTO> GetProductType(int appId)
        {
            AuditLogHelper.sSection = "Mappings\\FASTFilePreferences\\GetProductList";
            IFASTFilePreferenceService offices = ServiceFactory.Resolve<IFASTFilePreferenceService>();

            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();
            var tenantId = (claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault() != null) ?
                Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value) : 0;

            return offices.GetProductTypeList(appId);
        }

        [Route("GetSearchList", Name = "GetSearchList")]
        [HttpGet]
        public IEnumerable<DC.SearchTypeMappingDTO> GetSearchType()
        {
            AuditLogHelper.sSection = "Mappings\\FASTFilePreferences\\GetSearchList";
            IFASTFilePreferenceService offices = ServiceFactory.Resolve<IFASTFilePreferenceService>();

            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();
            var tenantId = (claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault() != null) ?
                Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value) : 0;

            return offices.GetSearchTypeList();
        }

        [Route("GetLocationList", Name = "GetLocationList")]
        [HttpGet]
        public IEnumerable<DC.LocationsMappings> GetLocationList()
        {
            AuditLogHelper.sSection = "Mappings\\FASTFilePreferences\\GetLocationList";
            IFASTFilePreferenceService offices = ServiceFactory.Resolve<IFASTFilePreferenceService>();

            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();
            var tenantId = (claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault() != null) ?
                Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value) : 0;
            return offices.GetLocationList();
        }

        [Route("GetTenantList", Name = "GetTenantList")]
        [HttpGet]
        public IEnumerable<DC.TenantMappingDTO> GetTenantList()
        {
            AuditLogHelper.sSection = "Mappings\\FASTFilePreferences\\GetTenantList";
            IFASTFilePreferenceService offices = ServiceFactory.Resolve<IFASTFilePreferenceService>();

            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();
            var tenantId = (claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault() != null) ?
                Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value) : 0;
            return offices.GetTenant();
        }

        [Route("GetValidatorFilePreferences/{StateFipsId:maxlength(50)}/{CountyFipsid:maxlength(50)}/{LoanAmount:maxlength(19)}/{serviceId:int:min(1)}/{LocationId:int:min(1)}/{Regionid:int:min(1)}/{loanPurposeTypeCodeId:int:min(1)}/{Tenantid:int:min(1)}/{ProductId:int:min(1)}", Name = "GetValidatorFilePreferences")]
        [HttpPost]
        public IEnumerable<DC.FASTFilePreferenceDTO> GetValidatorFilePreferences(string StateFipsId, string CountyFipsid, string loanAmount, int serviceId, int locationId, int Regionid, int loanPurposeTypeCodeId, int Tenantid, int ProductId)
        {
            AuditLogHelper.sSection = "Mappings\\FASTFilePreferences\\GetValidatorFilePreferences";
            IFASTFilePreferenceService FilePreference = ServiceFactory.Resolve<IFASTFilePreferenceService>();
            List<DC.FASTFilePreferenceDTO> newList = new List<DC.FASTFilePreferenceDTO>();

            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();
            var tenantId = (claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault() != null) ?
                Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value) : 0;

            newList = FilePreference.GetValidatorFilePreferences(StateFipsId, CountyFipsid, loanAmount.ToString(), serviceId, locationId, Regionid, loanPurposeTypeCodeId, Tenantid, ProductId);
            return newList;
        }

        //[Route("GetAccessvalidator", Name = "GetAccessvalidator")]
        //[HttpGet]
        //public string GetAccessvalidator()
        //{
        //    string Acessvalidator = ConfigurationManager.AppSettings["Acessvalidator"];
        //    return Acessvalidator;

        //}
    }
}
