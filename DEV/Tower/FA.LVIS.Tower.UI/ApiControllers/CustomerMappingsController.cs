using System.Collections.Generic;
using System.Web.Http;

using DC = FA.LVIS.Tower.DataContracts;
using FA.LVIS.Tower.Services;
using FA.LVIS.Tower.Data;
using System;
using System.Linq;
using System.Security.Claims;
using FA.LVIS.Tower.UI.ApiControllers.Filters;

namespace FA.LVIS.Tower.UI.ApiControllers
{
    [ValidateAntiForgeryTokenFilter]
    [RoutePrefix("Customers")]

    [CustomAuthorize]
    public class CustomerMappingsController : ApiController
    {

        [Route("GetWebhooks/{webhookUser}", Name = "GetWebhooks")]
        [HttpGet]
        // GET: api/LenderMappings
        public IEnumerable<DC.webhook> GetWebhooks(string webhookUser)
        {
            AuditLogHelper.sSection = "Mappings\\Customers\\GetWebhooks";
            ICustomerMappingService customerMappingService = ServiceFactory.Resolve<ICustomerMappingService>();

            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();

            var tenantId = (claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value != null) ?
                Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value) : 0;

              return customerMappingService.GetWebhooks(webhookUser);
        }

        [Route("GetWebhookUser/{CustomerID}", Name = "GetWebhookUser")]
        [HttpGet]
        public string GetWebhookUser(int CustomerID)
        {
            AuditLogHelper.sSection = "Mappings\\Customers\\GetWebhookUser";
            ICustomerMappingService customer = ServiceFactory.Resolve<ICustomerMappingService>();

            return customer.GetWebhookUser(CustomerID);
        }

        [Route("AddWebhook/", Name = "AddWebhook")]
        [HttpPost]
        [CustomAuthorize("SuperAdmin", "Admin")]
        public int  AddWebhook([FromBody]DC.webhookDto value)
        {
            ICustomerMappingService customer = ServiceFactory.Resolve<ICustomerMappingService>();

            
            return customer.AddWebhook(value);
        }
        [Route("UpdateWebhook/", Name = "UpdateWebhook")]
        [HttpPost]
        [CustomAuthorize("SuperAdmin", "Admin")]
        public int UpdateWebhook([FromBody]DC.webhookDto value)
        {
            ICustomerMappingService customer = ServiceFactory.Resolve<ICustomerMappingService>();


            return customer.EditWebhook(value); 
        }
        

        [Route("DeleteWebhook/", Name = "DeleteWebhook")]
        [HttpPost]
        [CustomAuthorize("SuperAdmin", "Admin")]
        public int DeleteWebhook([FromBody]DC.webhookDto value)
        {
            ICustomerMappingService customer = ServiceFactory.Resolve<ICustomerMappingService>();


            return customer.DeleteWebhook(value);
        }

        [Route("GetAvailbleActionType/{User}", Name = "GetAvailbleActionType")]
        [HttpGet]

        public string[] GetAvailbleActionType(string User)
        {

            string[] availableActionTypes = ServiceFactory.Resolve<ICustomerMappingService>().GetAvailbleActionType(User); 
            return new List<string>
                { "DocumentDelivery",
                "OrderCreated",
                "MessageAdded",
                "CurativeCleared",
                "FundsDisbursed"
                }.
                Where(x=> !availableActionTypes.Contains(x)).ToArray(); 
        }


        [Route("GetCustomers", Name = "GetCustomers")]
        [HttpGet]
        // GET: api/LenderMappings
        public IEnumerable<DC.CustomerMapping> Get()
        {
            AuditLogHelper.sSection = "Mappings\\Customers\\GetCustomers";
            ICustomerMappingService customer = ServiceFactory.Resolve<ICustomerMappingService>();

            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();

            var tenantId = (claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value != null) ?
                Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value) : 0;

            return customer.GetLVISCustomers(tenantId);
        }

        [Route("GetCustomersOnly", Name = "GetCustomersOnly")]
        [HttpGet]
        // GET: api/LenderMappings
        public IEnumerable<DC.CustomerMapping> GetCustomersOnly()
        {
            AuditLogHelper.sSection = "Mappings\\Customers\\GetCustomersOnly";
            ICustomerMappingService customer = ServiceFactory.Resolve<ICustomerMappingService>();

            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();

            var tenantId = (claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault() != null) ?
                Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value) : 0;

            return customer.GetLVISCustomersOnly(tenantId);
        }

        [Route("GetTenantBasedCustomer", Name = "GetTenantBasedCustomer")]
        [HttpGet]
        public IEnumerable<DC.CustomerDetails> GetTenantBasedCustomer()
        {
            AuditLogHelper.sSection = "Mappings\\GetTenantBasedCustomer";
            ICustomerService customer = ServiceFactory.Resolve<ICustomerService>();

            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();

            var tenantId = (claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault() != null) ?
                Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value) : 0;

            return customer.GetLVISCustomers(tenantId);
        }
        
        [HttpPost]
        [CustomAuthorize("SuperAdmin", "Admin")]
        public DC.CustomerMapping AddCustomer([FromBody]DC.CustomerMapping value)
        {
            AuditLogHelper.sSection = "Mappings\\Customers\\AddCustomer";
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();

            var tenantId = (claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault() != null) ?
                Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value) : 0;

            var userId = (claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault() != null) ?
             Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault().Value) : 0;

            var role = claims.Where(c => c.Type == ClaimTypes.Role).ToList().FirstOrDefault();

            AuditLogHelper.sSection = "Mappings\\Customers";
            ICustomerMappingService customer = ServiceFactory.Resolve<ICustomerMappingService>();
            value.GenerateCredential &= this.IsCustomerCredentialEditAllowed(value.Applicationid);
            return customer.AddCustomer(value, tenantId, userId);
        }
        
        [HttpPost]
        [CustomAuthorize("SuperAdmin", "Admin")]
        public DC.CustomerMapping UpdateCustomer([FromBody]DC.CustomerMapping value)
        {
            AuditLogHelper.sSection = "Mappings\\Customers\\updateCustomer";
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();

            var userId = (claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault() != null) ?
             Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault().Value) : 0;

            AuditLogHelper.sSection = "Mappings\\Customers";
            value.GenerateCredential &= this.IsCustomerCredentialEditAllowed(value.Applicationid); 
            return ServiceFactory.Resolve<ICustomerMappingService>().UpdateCustomer(value, userId);
        }

        [Route("IsCustomerCredentialEditAllowed/{applicationId}", Name = "IsCustomerCredentialEditAllowed")]
        [HttpGet]
        public bool IsCustomerCredentialEditAllowed(int applicationId)
        {
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();
            var tenantClaim = claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault();
            int tenantId = tenantClaim == null ? 0 : Convert.ToInt32(tenantClaim.Value); 
            var roleClaim = claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault();
            int role = roleClaim == null ? 0 : Convert.ToInt32(roleClaim.Value);


            List<int> allowedApps = new List<int> { DC.Constants.SETTLMENTSERVICES_APPID, DC.Constants.OPENAPI_APPID,DC.Constants.CALCULATOR_APPID }; 

            return allowedApps.Contains(applicationId) && tenantId ==DC.Constants.TENENT_LVIS_ID && role == DC.Constants.ROLE_SUPERADMIN_ID;
        }
        [Route("GetUserName/{cusotmerId}/{applicationId}", Name = "GetUserName")]
        [HttpGet]
        public string GetUserName(int cusotmerId, int applicationId)
        {
            return ServiceFactory.Resolve<ICustomerMappingService>().GetUserName(cusotmerId, applicationId);
        }

        [Route("DeleteCustomer", Name = "DeleteCustomer")]
        [HttpPost]
        [CustomAuthorize("SuperAdmin", "Admin")]
        public int DeleteCustomer([FromBody]int value)
        {
            AuditLogHelper.sSection = "Mappings\\Customers\\DeleteCustomer";
            
            return ServiceFactory.Resolve<ICustomerMappingService>().DeleteCustomer(value);
        }

        [Route("ConfirmDeleteCustomer", Name = "ConfirmDeleteCustomer")]
        [HttpPost]
        [CustomAuthorize("SuperAdmin", "Admin")]
        public int ConfirmDeleteCustomer([FromBody]int value)
        {
            AuditLogHelper.sSection = "Mappings\\Customers\\ConfirmDeleteCustomer";

            return ServiceFactory.Resolve<ICustomerMappingService>().ConfirmDeleteCustomer(value);
        }

        [Route("GetCustomerPreferenceTypes", Name = "GetCustomerPreferenceTypes")]
        [HttpGet]
        public IEnumerable<DC.TypeCodeDTO> GetCustomerPreferenceTypes()
        {
            
            AuditLogHelper.sSection = "Mappings\\Customers\\GetCustomerPreferenceType";
            return ServiceFactory.Resolve<ICustomerMappingService>().GetCustomerPreferenceTypes();
        }

        [Route("GetCustomerPreferenceSubTypes/{typeCodeId}", Name = "GetCustomerPreferenceSubTypes")]
        [HttpGet]
        public IEnumerable<DC.TypeCodeDTO> GetCustomerPreferenceSubTypes(int typeCodeId)
        {

            AuditLogHelper.sSection = "Mappings\\Customers\\GetCustomerPreferenceSubTypes";
            return ServiceFactory.Resolve<ICustomerMappingService>().GetCustomerPreferenceSubTypes(typeCodeId);
        }

        [Route("IsUniqueUserName/{userName}/{customerId}", Name = "IsUniqueUserName")]
        [HttpGet]
        public bool IsUniqueUserName(string userName,int customerId)
        {
            AuditLogHelper.sSection = "Mappings\\Customers\\GetCustomerPreferenceSubTypes";
            return ServiceFactory.Resolve<ICustomerMappingService>().IsUniqueUserName(userName, customerId);
        }

        [Route("GetApplicationList", Name = "GetApplicationList")]
        [HttpGet]
        public IEnumerable<DC.ApplicationMappingDTO> GetApplicationList()
        {       
            AuditLogHelper.sSection = "Mappings\\Customers\\GetApplicationList";
            ICustomerService ExternalList = ServiceFactory.Resolve<ICustomerService>();

            var result = ExternalList.GetExternalApplications().ToList();
            List<DC.ApplicationMappingDTO> Applications = new List<DataContracts.ApplicationMappingDTO>();
            if (result != null)
            {
                foreach (var se in result)
                {
                    DC.ApplicationMappingDTO apps = new DC.ApplicationMappingDTO()
                    {
                        ApplicationId = se.ID,
                        ApplicationName = se.Name
                    };

                    Applications.Add(apps);
                }
            }

            return Applications;
        }

        [Route("GetCategoryList", Name = "GetCategoryList")]
        [HttpGet]
        public IEnumerable<DC.CategoryMapping> GetCategoryList()
        {
            AuditLogHelper.sSection = "Mappings\\Customers\\GetCategoryList";
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();

            var tenantId = (claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault() != null) ?
                Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value) : 0;
       
            AuditLogHelper.sSection = "Mappings\\Customers";
            return ServiceFactory.Resolve<ICustomerMappingService>().GetCategoryList(tenantId);
        }

        [Route("GetCustomerGabDetails/{Locationid:maxlength(10)}", Name = "GetCustomerGabDetails")]
        [HttpGet]
        public IEnumerable<DC.FASTGABMap> GetCustomerGabDetails(string Locationid)
        {
            AuditLogHelper.sSection = "Mappings\\Customers\\GetCustomerGabDetails";
            return ServiceFactory.Resolve<IFastGabMappingService>().GetFastGabDetails(Locationid);
        }

        [Route("GetCustomerGabMap/{GabId}", Name = "GetCustomerGabMap")]
        [HttpGet]
        public DC.FASTGABMap GetCustomerGabDetails(int GabId)
        {
            AuditLogHelper.sSection = "Mappings\\Customers\\GetCustomerGabDetails";
            return ServiceFactory.Resolve<IFastGabMappingService>().GetFastGabMap(GabId);
        }
                
        [HttpPost]
        [CustomAuthorize("SuperAdmin", "Admin")]
        public DC.FASTGABMap AddFastGabDetails([FromBody]DC.FASTGABMap value)
        {
            AuditLogHelper.sSection = "Mappings\\Customers\\AddFastGab";
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();

            var userId = (claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault() != null) ?
             Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault().Value) : 0;
           
            return ServiceFactory.Resolve<IFastGabMappingService>().AddFastGab(value, userId);
        }
                
        [HttpPost]
        [CustomAuthorize("SuperAdmin", "Admin")]
        public DC.FASTGABMap  updateFastGab([FromBody]DC.FASTGABMap value)
        {
            AuditLogHelper.sSection = "Mappings\\Customers\\updateFastGab";
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();

            var userId = (claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault() != null) ?
             Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault().Value) : 0;
         
            return ServiceFactory.Resolve<IFastGabMappingService>().UpdateFastGab(value, userId);
        }

        [Route("DeleteFastGab", Name = "DeleteFastGab")]
        [HttpPost]
        [CustomAuthorize("SuperAdmin", "Admin")]
        public int DeleteFastGab([FromBody]int value)
        {
            AuditLogHelper.sSection = "Mappings\\Customers\\Delete FastGab";

            return ServiceFactory.Resolve<IFastGabMappingService>().DeleteGab(value);
        }
        
        [Route("ConfirmDeleteFastGab", Name = "ConfirmDeleteFastGab")]
        [HttpPost]
        [CustomAuthorize("SuperAdmin", "Admin")]
        public int ConfirmDeleteFastGab([FromBody]int value)
        {
            AuditLogHelper.sSection = "Mappings\\Customers\\ConfirmDeleteFastGab";
            return ServiceFactory.Resolve<IFastGabMappingService>().ConfirmDeleteGab(value);
        }

        [Route("GetLoanTypeDetatils", Name = "GetLoanTypeDetatils")]
        [HttpGet]
        public IEnumerable<DC.TypeCodeDTO> GetLoanTypeDetatils()
        {
      
            AuditLogHelper.sSection = "Mappings\\Customers\\GetLoanTypeDetatils";
            return ServiceFactory.Resolve<IFastGabMappingService>().GetLoanTypeDetatils();
        }

        [Route("GetFastGabDetatils/{LocationID:maxlength(10)}/{StateFipsId:maxlength(50)}/{CountyFipsid:maxlength(50)}", Name = "GetFastGabDetatils")]
        [HttpGet]
        public IEnumerable<DC.FASTGABMap> GetFastGabDetatils(string locationId, string StateFipsId, string CountyFipsid)
        {
            AuditLogHelper.sSection = "Mappings\\Customers\\GetFastGabDetatils";

            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();

            var tenantId = (claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault() != null) ?
                Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value) : 0;

            return ServiceFactory.Resolve<IFastGabMappingService>().GetFastGabDetails(locationId, StateFipsId, CountyFipsid, tenantId);
        }

        [Route("GetServicePreferenceCustomer/{customerid}", Name = "GetServicePreferenceCustomer")]
        [HttpGet]
        public List<DC.ServicePreference> GetServicePreferenceCustomer(int customerid)
        {

            AuditLogHelper.sSection = "Mappings\\Customers\\ServicePreference";
            return ServiceFactory.Resolve<ICustomerMappingService>().GetServicePreferenceCustomer(customerid);

        }

        [Route("GetWebhookDomains/{CustomerId}", Name = "GetWebhookDomains")]
        [HttpGet]
        // GET: api/LenderMappings
        public IEnumerable<DC.WebhookDomain> GetWebhookDomains(int CustomerId)
        {
            AuditLogHelper.sSection = "Mappings\\Customers\\GetWebhookDomains";
            ICustomerMappingService customerMappingService = ServiceFactory.Resolve<ICustomerMappingService>();

            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();

            var tenantId = (claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value != null) ?
                Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value) : 0;

            IEnumerable<DC.WebhookDomain>  webhookDomains = customerMappingService.GetWebhookDomains(CustomerId);

            return webhookDomains;
        }

        [Route("AddWebhookDomain/", Name = "AddWebhookDomain")]
        [HttpPost]
        [CustomAuthorize("SuperAdmin", "Admin")]
        public DC.WebhookDomain AddWebhookDomain([FromBody] DC.WebhookDomain value)
        {
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();
            var userId = (claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault() != null) ?
             Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault().Value) : 0;

            ICustomerMappingService customer = ServiceFactory.Resolve<ICustomerMappingService>();
            return customer.AddWebhookDomain(value, userId);
        }

        [Route("UpdateWebhookDomain/", Name = "UpdateWebhookDomain")]
        [HttpPost]
        [CustomAuthorize("SuperAdmin", "Admin")]
        public int UpdateWebhookDomain([FromBody] DC.WebhookDomain value)
        {
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();
            var userId = (claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault() != null) ?
             Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault().Value) : 0;

            ICustomerMappingService customer = ServiceFactory.Resolve<ICustomerMappingService>();
            return customer.UpdateWebhookDomain(value, userId);
        }


        [Route("DeleteWebhookDomain/", Name = "DeleteWebhookDomain")]
        [HttpPost]
        [CustomAuthorize("SuperAdmin", "Admin")]
        public int DeleteWebhookDomain([FromBody] DC.WebhookDomain value)
        {
            ICustomerMappingService customer = ServiceFactory.Resolve<ICustomerMappingService>();
           
            return customer.DeleteWebhookDomain(value);
        }
    }
}
