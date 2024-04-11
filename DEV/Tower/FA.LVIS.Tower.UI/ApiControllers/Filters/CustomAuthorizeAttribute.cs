using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using FA.LVIS.Tower.Services;
using DC = FA.LVIS.Tower.DataContracts;
using System.Web.Mvc;
using FA.LVIS.Tower.Common;
using FA.LVIS.Tower.DataContracts;

namespace FA.LVIS.Tower.UI.ApiControllers.Filters
{
    public class CustomAuthorizeAttribute : AuthorizationFilterAttribute
    {
        private readonly string[] allowedroles;

        public CustomAuthorizeAttribute() { }
        public CustomAuthorizeAttribute(params string[] roles)
        {
            this.allowedroles = roles;
        }

        Logger sLogger = new Common.Logger(typeof(CustomAuthorizeAttribute));
        public override Task OnAuthorizationAsync(HttpActionContext actionContext, System.Threading.CancellationToken cancellationToken)
        {
            UserProfile currentUser;
            sLogger.Debug("this debug from the auth Token !!");
            var context = SecurityExtensions.GetOwinContext(actionContext.Request);
            if (context.Authentication?.User?.Identity?.IsAuthenticated ?? false)
            {
                var claims = context.Authentication.User.Claims.ToList();

                var userId = (claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault() != null) ?
                 Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault().Value) : 0;

                sLogger.Debug(string.Format("found userId is => '{0}'", userId));

                var tenantId = (claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault() != null) ?
                 Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value) : 0;

                sLogger.Debug(string.Format("found tenant is => '{0}'", tenantId));

                IApplicationUserService UsersList = ServiceFactory.Resolve<IApplicationUserService>();
                currentUser = UsersList.GetApplicationUsers(tenantId).FirstOrDefault(x => (x.IsActive ?? false) && (x.UserId == userId));
                sLogger.Debug(string.Format("current userId is => '{0}'", currentUser?.ID));
                if (currentUser == null || currentUser.IsActive == null || !(bool)currentUser.IsActive)
                {
                    currentUser = ReAutherized(actionContext);
                }
            }
            else
                currentUser = ReAutherized(actionContext);

            AutherizationByRole(actionContext, currentUser);
            return Task.FromResult<object>(null);

        }
        private UserProfile ReAutherized(HttpActionContext actionContext)
        {
            UserProfile currentUser = null;
            sLogger.Debug("Trying to re Authenticate !!");
            var user = new SecurityController().GetCurrentUser();
            if (user != null)
            {
                IApplicationUserService UsersList = ServiceFactory.Resolve<IApplicationUserService>();
                currentUser = UsersList.GetApplicationUsers(Convert.ToInt32(user.TenantId)).
                    FirstOrDefault(x => (x.IsActive ?? false) && (x.UserId == user.UserId));
                if (currentUser == null)
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, "Not allowed to access.");
            }
            return currentUser;
        }
        private void AutherizationByRole(HttpActionContext actionContext, UserProfile currentUser)
        {
            if (this.allowedroles != null && currentUser != null && !this.allowedroles.Contains(currentUser.Role))
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Forbidden, "Not allowed to access.");
        }
    }
    //public class RoleAuthorizeAttribute : AuthorizationFilterAttribute
    //{
    //    private readonly string[] allowedroles;
    //    public RoleAuthorizeAttribute(params string[] roles)
    //    {
    //        this.allowedroles = roles;
    //    }
    //    public override Task OnAuthorizationAsync(HttpActionContext actionContext, System.Threading.CancellationToken cancellationToken)
    //    {

    //        var claims = SecurityExtensions.GetOwinContext(actionContext.Request).Authentication.User.Claims.ToList();
    //        foreach (var role in allowedroles)
    //        {
    //            var userRole = claims.Where(c => c.Type == ClaimTypes.Role).ToList().FirstOrDefault();
    //            if (role == userRole.Value)
    //                return Task.FromResult<object>(null);
    //        }

    //        actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, "Not allowed to access.");
    //        return Task.FromResult<object>(null);

    //    }
    //}
}