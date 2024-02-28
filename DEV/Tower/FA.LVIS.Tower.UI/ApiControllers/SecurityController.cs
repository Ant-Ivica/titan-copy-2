using FA.LVIS.Tower.Common;
using FA.LVIS.Tower.Data;
using FA.LVIS.Tower.Services;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Http;
using DC = FA.LVIS.Tower.DataContracts;
using FA.LVIS.Tower.UI.ApiControllers.Filters; 

namespace FA.LVIS.Tower.UI.ApiControllers
{
    [RoutePrefix("Security")]
    public class SecurityController : ApiController
    {
        [HttpGet]
        [Route("antiforgerytoken")]
        public HttpResponseMessage GetAntiForgeryToken()
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);

            HttpCookie cookie = HttpContext.Current.Request.Cookies["xsrf-token"];

            string cookieToken;
            string formToken;
            AntiForgery.GetTokens(cookie == null ? "" : cookie.Value, out cookieToken, out formToken);

            AntiForgeryTokenModel content = new AntiForgeryTokenModel
            {
                AntiForgeryToken = formToken
            };

            response.Content = new StringContent(
                     JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");

            bool isSecure = false;

            if (HttpContext.Current.Request.IsSecureConnection == true && HttpContext.Current.Request.Url.Scheme == "https")
            {
                isSecure = true;
            }
            if (!string.IsNullOrEmpty(cookieToken))
            {
                response.Headers.AddCookies(new[]
                {
            new CookieHeaderValue("xsrf-token", cookieToken)
            {
                Expires = DateTimeOffset.Now.AddMinutes(10),
                Path = "/",HttpOnly = true,Secure= isSecure
            }
        });
            }
            return response;
        }
        [Route("GetEnv", Name = "GetEnv")]
        [HttpGet]

        public string GetEnv()
        {
            if (System.Configuration.ConfigurationManager.AppSettings["tibco_env"] != null)
            {
                return System.Configuration.ConfigurationManager.AppSettings["tibco_env"].ToString();
            }
            return null;
        }

        [Route("GetCurrentUser", Name = "GetCurrentUser")]
        [HttpGet]
        public DC.ApplicationUser GetCurrentUser()
        {
            Logger sLogger = new Common.Logger(typeof(SecurityController));
            AuditLogHelper.sSection = "Security\\GetCurrentUser";
            ServicesContainer.Instance.RegisterType(typeof(IIdentityUserService<DC.ApplicationUser>), typeof(Services.IdentityUserService<DC.ApplicationUser>));
            IIdentityUserService<DC.ApplicationUser> IdentityService = ServiceFactory.Resolve<IIdentityUserService<DC.ApplicationUser>>();

            DC.ApplicationUser user = new DC.ApplicationUser();
            IEnumerable<Claim> claims = new List<Claim>();
            var roles = new Claim(ClaimTypes.Role, ClaimValueTypes.String);

            Task<ClaimsIdentity> userIdentity;

            var identity = User.Identity as ClaimsIdentity;

            if (identity != null && !string.IsNullOrEmpty(identity.Name))
            {
                user = IdentityService.GetApplicationUserByName(identity.Name).FirstOrDefault();
            }
            else
            {
                user = IdentityService.GetApplicationUserByName(HttpContext.Current.Request.LogonUserIdentity.Name).FirstOrDefault();
            }

            try
            {
                var manager = ApplicationUserManager.Create(new IdentityFactoryOptions<ApplicationUserManager>(), SecurityExtensions.GetOwinContext(Request));

                //var manager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();                

                userIdentity = user.GenerateUserIdentityAsync(manager);
                if (userIdentity != null & userIdentity.Result != null)
                {
                    var claimResult = IdentityService.GetClaimsByUserId(user.Id);
                    foreach (var claimStr in claimResult)
                    {
                        int claimStrDiv = claimStr.IndexOf(':');

                        //identity.AddClaim(new Claim(claimStr.Substring(0, claimStrDiv++), claimStr.Substring(claimStrDiv)));

                        userIdentity.Result.AddClaim(new Claim(claimStr.Substring(0, claimStrDiv++), claimStr.Substring(claimStrDiv)));
                    }

                    sLogger.Debug(userIdentity.Result.Name);
                    //AuthenticationProperties AuthProp = new AuthenticationProperties() { IsPersistent = true, AllowRefresh = true };
                    
                    //SecurityExtensions.GetOwinContext(Request).Authentication.SignOut(AuthProp);
                    SecurityExtensions.GetOwinContext(Request).Authentication.SignIn(new AuthenticationProperties() { IsPersistent = false, AllowRefresh = true }, userIdentity.Result);
                    sLogger.Debug("Claims Count: " + userIdentity.Result.Claims.Count());

                    claims = userIdentity.Result.Claims;
                    roles = claims.Where(c => c.Type == ClaimTypes.Role).ToList().FirstOrDefault();
                    user.ActivityRight = roles.Value;

                    user.CanManageTEQ = (claims.Where(c => c.Type == DC.Constants.CLAIM_MANAGETEQ).FirstOrDefault() != null) ?
                        Convert.ToBoolean(claims.Where(c => c.Type == DC.Constants.CLAIM_MANAGETEQ).FirstOrDefault().Value) : false;

                    user.CanManageBEQ = (claims.Where(c => c.Type == DC.Constants.CLAIM_MANAGEBEQ).FirstOrDefault() != null) ?
                        Convert.ToBoolean(claims.Where(c => c.Type == DC.Constants.CLAIM_MANAGEBEQ).FirstOrDefault().Value) : false;

                    user.CanAccessReq = (claims.Where(c => c.Type == DC.Constants.CLAIM_MANAGEACCESSREQ).FirstOrDefault() != null) ?
                       Convert.ToBoolean(claims.Where(c => c.Type == DC.Constants.CLAIM_MANAGEACCESSREQ).FirstOrDefault().Value) : false;
                }
            }
            catch (System.AggregateException aex)
            {

                foreach (Exception ex in aex.Flatten().InnerExceptions)
                {
                    sLogger.Error("Error in GetCurrentUser: " + ex.ToString() + "\r\n");
                }

                //sLogger.Error("Error in GetCurrentUser: " + ex.ToString());
                //return "There was an error loading user profile. " + ex.Message;
            }

            return user;
        }

        [Route("GetFastOffices/{Region}", Name = "GetFastOffices")]
        [HttpGet]
        public IEnumerable<DC.FastOffices> GetFastOffices(int Region)
        {
            AuditLogHelper.sSection = "Security\\GetFastOffices";
            ICustomerService OfficeList = ServiceFactory.Resolve<ICustomerService>();
            return OfficeList.GetfastOffices(Region);
        }


        
        [Route("GetContact/{Locationid}", Name = "GetContact")]
        [HttpGet]
        public IEnumerable<DC.ContactMappings> GetContact(int Locationid)
        {
            AuditLogHelper.sSection = "Security\\GetContact";
            ICustomerService ContactList = ServiceFactory.Resolve<ICustomerService>();
            return ContactList.GetContact(Locationid);
        }
        [Route("GetContactProviderDetails/{CustomerId}", Name = "GetContactProviderDetails")]
        [HttpGet]
        public IEnumerable<DC.ContactProviderMappings> GetContactProviderDetails(int CustomerId)
        {
            AuditLogHelper.sSection = "Security\\GetContactProviderDetails";
            ICustomerService ContactProviderList = ServiceFactory.Resolve<ICustomerService>();
            return ContactProviderList.GetContactProviderDetails(CustomerId);
        }
        //[Route("GetRegions", Name = "GetRegions")]
        //[HttpGet]
        //public IEnumerable<DC.Regions> GetRegions()
        //{
        //    AuditLogHelper.sSection = "Security\\GetRegions";
        //    ICustomerService RegionList = ServiceFactory.Resolve<ICustomerService>();
        //    List<DC.Regions> regionsList = RegionList.GetFastRegions();

        //    return regionsList;
        //}

        [Route("GetApplications", Name = "GetApplications")]
        [HttpGet]
        public IEnumerable<DC.Users> GetApplications()
        {
            AuditLogHelper.sSection = "Security\\GetApplications";
            ICustomerService List = ServiceFactory.Resolve<ICustomerService>();
            List<DC.Users> applicationList = List.GetExternalApplications().Concat(List.GetInternalApplications()).Distinct().ToList();

            return applicationList;
        }

        [Route("GetExternalApplications", Name = "GetExternalApplications")]
        [HttpGet]
        public IEnumerable<DC.Users> GetExternalApplications()
        {
            AuditLogHelper.sSection = "Security\\GetExternalApplications";
            ICustomerService ExternalList = ServiceFactory.Resolve<ICustomerService>();

            return ExternalList.GetExternalApplications().ToList();
        }

        [Route("GetShowTenants", Name = "GetShowTenants")]
        [HttpGet]
        public bool GetShowTenants()
        {
           if( System.Configuration.ConfigurationManager.AppSettings["AllowTenantSwitch"] != null)

            {
                return  Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["AllowTenantSwitch"]);
            }

            return false;
        }

        [Route("GetAllApplications", Name = "GetAllApplications")]
        [HttpGet]
        public IEnumerable<DC.Users> GetAllApplications()
        {
            AuditLogHelper.sSection = "Security\\GetAllApplications";
            ICustomerService List = ServiceFactory.Resolve<ICustomerService>();
            List<DC.Users> applicationList = List.GetExternalApplications().Concat(List.GetInternalApplications()).Distinct().ToList();

            applicationList.Remove(applicationList.Where(se => se.Name == DC.Constants.APPLICATION_LVIS).FirstOrDefault());

            applicationList.Remove(applicationList.Where(se => se.Name == DC.Constants.APPLICATION_FAST).FirstOrDefault());
            return applicationList;
        }

        [Route("GetInternalApplications", Name = "GetInternalApplications")]
        [HttpGet]
        public IEnumerable<DC.Users> GetInternalApplications()
        {
            AuditLogHelper.sSection = "Security\\GetInternalApplications";
            ICustomerService InternalList = ServiceFactory.Resolve<ICustomerService>();

            return InternalList.GetInternalApplications().ToList();
        }

        [Route("GetRegions/{Application:maxlength(100)}", Name = "GetRegionsByApplication")]
        [HttpGet]
        public IEnumerable<DC.Regions> GetRegions(string Application)
        {
            AuditLogHelper.sSection = "Security\\GetRegionsByApplication";
            ICustomerService RegionList = ServiceFactory.Resolve<ICustomerService>();

            IEnumerable<DC.Regions> regionsList = RegionList.GetFastRegions(Application);

            return regionsList;

        }


        [Route("GetRegionsWithoutClaim/{Application:maxlength(100)}", Name = "GetRegionsWithoutClaim")]
        [HttpGet]
        public IEnumerable<DC.Regions> GetRegionsWithoutClaim(string Application)
        {
            AuditLogHelper.sSection = "Security\\GetRegionsWithoutClaim";
            ICustomerService RegionList = ServiceFactory.Resolve<ICustomerService>();

            IEnumerable<DC.Regions> regionsList = RegionList.GetFastRegions(Application);
            return regionsList;

        }

        [Route("GetTenant", Name = "GetTenant")]
        [HttpGet]
        public string GetTenant()
        {
            AuditLogHelper.sSection = "Security\\GetTenant";
            var identity = User;

            int tenantId;
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();
            string Tenant = claims.Where(se => se.Type == DC.Constants.TENANT_ID).Select(val => val.Value).FirstOrDefault();
            int.TryParse(Tenant, out tenantId);
            IApplicationUserService UsersList = ServiceFactory.Resolve<IApplicationUserService>();
            return UsersList.GetTenantName(tenantId);
        }

        [Route("GetCanManageTEQ", Name = "GetCanManageTEQ")]
        [HttpGet]
        public string GetCanManageTEQ()
        {
            AuditLogHelper.sSection = "Security\\GetCanManageTEQ";
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();
            return claims?.Where(se => se.Type == DC.Constants.CLAIM_MANAGETEQ).Select(val => val.Value).FirstOrDefault();
        }

        [Route("GetCanManageBEQ", Name = "GetCanManageBEQ")]
        [HttpGet]
        public string GetCanManageBEQ()
        {
            AuditLogHelper.sSection = "Security\\GetCanManageBEQ";
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();
            return claims?.Where(se => se.Type == DC.Constants.CLAIM_MANAGEBEQ).Select(val => val.Value).FirstOrDefault();
        }

        [Route("GetUsers", Name = "GetUsers")]
        [HttpGet]
        public IEnumerable<DC.UserProfile> Get()
        {
            if (!User.IsInRole(DC.Constants.ROLE_USER))
            {
                AuditLogHelper.sSection = "Security\\GetUsers";
                var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();

                var tenantId = (claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault() != null) ?
                    Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value) : 0;

                IApplicationUserService UsersList = ServiceFactory.Resolve<IApplicationUserService>();
                return UsersList.GetApplicationUsers(tenantId);
            }
            else
            {
                return null;
            }
        }

        [Route("GetRoles", Name = "GetRoles")]
        [HttpGet]
        public List<string> GetRoles()
        {
            AuditLogHelper.sSection = "Security\\GetRoles";
            IApplicationUserService UsersList = ServiceFactory.Resolve<IApplicationUserService>();
            List<string> lst = UsersList.GetUserRole();
            if (!User.IsInRole(DC.Constants.ROLE_SUPERADMIN))
            {
                lst.Remove(DC.Constants.ROLE_SUPERADMIN);
            }
            return lst;
        }

        [Route("FindUser/{Domain:length(4)}/{UserName:length(1,128)}", Name = "FindUser")]
        [HttpGet]
        public IEnumerable<DC.UserProfile> FindUserByDomain(string Domain, string UserName)
        {
            AuditLogHelper.sSection = "Security\\FindUser";
            List<DC.UserProfile> Listofusers = new List<DC.UserProfile>();
            PrincipalContext domain = null;
            if (Domain == "CORP")
                domain = new PrincipalContext(ContextType.Domain, "corp.firstam.com", "DC=corp,DC=firstam,DC=com");

            if (Domain == "INTL")
                domain = new PrincipalContext(ContextType.Domain, "intl.corp.firstam.com", "DC=intl,DC=corp,DC=firstam,DC=com");

            UserPrincipal u = new UserPrincipal(domain);

            if (UserName.Contains(","))
            {
                string[] Names = UserName.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                if (Names.Length == 0)
                    return new List<DC.UserProfile>();

                if (Names.Length > 1)
                {
                    u.Surname = Names[0];
                    u.GivenName = Names[1];
                }
                else
                    u.GivenName = Names[0];
            }
            else
            {
                u.Name = String.Format("*{0}*", UserName);
            }

            PrincipalSearcher search = new PrincipalSearcher(u);
            PrincipalSearchResult<Principal> resultCol = search.FindAll();
            foreach (Principal p in resultCol)
            {
                DC.UserProfile objSurveyUsers = new DC.UserProfile();
                objSurveyUsers.Name = p.Name;
                objSurveyUsers.UserName = Domain + "\\" + p.SamAccountName;

                objSurveyUsers.Emailid = ((UserPrincipal)p).EmailAddress;
                objSurveyUsers.Employeeid = ((UserPrincipal)p).EmployeeId;

                Listofusers.Add(objSurveyUsers);
            }

            List<DC.UserProfile> MatchedUsers = Listofusers.Where(b => (b.Name.ToLower().Split(new char[] { ',', ' ', '-' }).Contains(UserName.ToLower())) == true).ToList();

            List<DC.UserProfile> WildMatchedUsers = Listofusers.Except(MatchedUsers).ToList();

            Listofusers = MatchedUsers.Concat(WildMatchedUsers).ToList();

            return Listofusers;
        }

        ////[Route("GetUserData", Name = "GetUserData")]
        [HttpPost]
        public DC.UserProfile PostUserData([FromBody]DC.UserProfile value)
        {
            AuditLogHelper.sSection = "Security\\GetUserData";
            
            return ServiceFactory.Resolve<IApplicationUserService>().GetSingleUser(value);
        }

        ////[Route("")]
        [HttpPost]
        public IEnumerable<DC.UserProfile> Post([FromBody]DC.UserProfile value)
        {
            AuditLogHelper.sSection = "Security\\Post";
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();

            var tenantId = (claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault() != null) ?
                Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value) : 0;

            AuditLogHelper.sSection = "Security";
            return ServiceFactory.Resolve<IApplicationUserService>().AddUser(value, tenantId);
        }

        ////[Route("Update")]
        [HttpPost]
        [CustomAuthorize("SuperAdmin","Admin")]
        public DC.UserProfile UpdateUserDetails(DC.UserProfile value)
        {
             AuditLogHelper.sSection = "Security\\Update";           
            return ServiceFactory.Resolve<IApplicationUserService>().UpdateUser(value);
        }

        ////[Route("Delete", Name = "DeleteUser")]
        [HttpPost]
        [CustomAuthorize("SuperAdmin", "Admin")]
        public int Delete(DC.UserProfile User)
        {
            AuditLogHelper.sSection = "Security\\DeleteUser";
            return ServiceFactory.Resolve<IApplicationUserService>().Deleteuser(User);
        }

        [Route("GetUserByName/{userName:length(1,128)}", Name = "GetUserByName")]
        [HttpGet]
        public List<DC.ApplicationUser> GetUserByName([FromBody] string userName)
        {
            AuditLogHelper.sSection = "Security\\GetUserByName";  
            return ServiceFactory.Resolve<IIdentityUserService<DC.ApplicationUser>>().GetApplicationUserByName(userName);
        }

        [Route("GetUserById/{userId:length(1,128)}", Name = "GetUserById")]
        [HttpGet]
        public DC.ApplicationUser GetUserById(string userId)
        {
            AuditLogHelper.sSection = "Security\\GetUserById";
            DC.ApplicationUser user = ServiceFactory.Resolve<IIdentityUserService<DC.ApplicationUser>>().GetApplicationUserById(userId);

            return user;
        }

        [Route("FindRolesByUserId/{userId:length(1,128)}", Name = "FindRolesByUserId")]
        [HttpGet]
        public IEnumerable<string> FindRolesByUserId(string userId)
        {
            AuditLogHelper.sSection = "Security\\FindRolesByUserId";
            return ServiceFactory.Resolve<IIdentityUserService<DC.ApplicationUser>>().FindRolesByUserId(userId);
        }

        //[Route("GetClaimsByUser", Name = "GetClaimsByUser")]
        //[HttpPost]
        //public IEnumerable<string> GetClaimsByUser(DC.ApplicationUser user)
        //{
        //    AuditLogHelper.sSection = "Security\\GetClaimsByUser";
        //    IIdentityUserService<DC.ApplicationUser> IdentityService = ServiceFactory.Resolve<IIdentityUserService<DC.ApplicationUser>>();

        //    if (user == null)
        //    {

        //        user = GetUserByName(HttpContext.Current.Request.LogonUserIdentity.Name).FirstOrDefault();
        //    }

        //    return IdentityService.GetClaimsByUser(user);
        //}

        [Route("AddUserAuditLog", Name = "AddUserAuditLog")]
        [HttpPost]
        public void AddUserAuditLog()
        {
            AuditLogHelper.sSection = "Security\\AddUserAuditLog";
            if (!string.IsNullOrEmpty(HttpContext.Current.Request.LogonUserIdentity.Name))
            {
                var identity = User.Identity as ClaimsIdentity;
                DC.AuditingDTO log = new DC.AuditingDTO()
                {
                    UserName = identity.Name,
                    EventDateutc = DateTime.Now.ToString(),
                    EventType = AuditLogEventTypeEnum.LogIn.ToString(),
                    TableName = "Tower.Users",
                    RecordId = identity.Name,
                    Property = "Authorize",
                };

                IAuditService customer = ServiceFactory.Resolve<IAuditService>();
                customer.AddUserAudit(log);
            }
        }

        [Route("GetAssemblyVersion", Name = "GetAssemblyVersion")]
        [HttpGet]
        public string GetAssemblyVersion()
        {
            // From executing application.
            string assemblyVersion = "v" + Assembly.GetExecutingAssembly().GetName().Version.ToString();

            return assemblyVersion;
        }

        [Route("GetServerName", Name = "GetServerName")]
        [HttpGet]
        public string GetServerName()
        {
            // From executing application.
            var hostname = Dns.GetHostEntry(Dns.GetHostName()).HostName;

            return hostname;
        }
    }

    public static class SecurityExtensions
    {
        public static Microsoft.Owin.IOwinContext GetOwinContext(this HttpRequestMessage request)
        {
            Logger sLogger = new Common.Logger(typeof(SecurityExtensions));
            //var context = request.Properties["MS_HttpContext"] as HttpContextWrapper;
            //if (context != null)
            //{
            //    return HttpContextBaseExtensions.GetOwinContext(context.Request);
            //}
            //return null;
            if (request?.Properties?.ContainsKey("MS_HttpContext")??false)
            {
                sLogger.Debug("Found MS_HttpContext in GetOwinContext");

                return ((HttpContextWrapper)request.Properties["MS_HttpContext"]).Request.GetOwinContext();
            }
            if (request?.Properties?.ContainsKey("MS_OwinContext")??false)
            {
                sLogger.Debug("Found MS_OwinContext in GetOwinContext");

                return ((HttpContextWrapper)request.Properties["MS_OwinContext"]).Request.GetOwinContext();
            }
            else if (HttpContext.Current != null)
            {
                sLogger.Debug("Looking for HttpContext Current: " + HttpContext.Current);
                return new HttpContextWrapper(HttpContext.Current).GetOwinContext();
            }
            else
            {
                sLogger.Error("GetOwinContext Not Found");
                return null;
            }
        }
    }
}
