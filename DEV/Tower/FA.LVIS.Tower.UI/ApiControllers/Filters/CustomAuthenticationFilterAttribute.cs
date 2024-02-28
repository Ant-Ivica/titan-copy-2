using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace FA.LVIS.Tower.UI.ApiControllers.Filters
{
    public class CustomAuthenticationFilterAttribute : AuthorizationFilterAttribute
    {
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }

        public override Task OnAuthorizationAsync(HttpActionContext actionContext, System.Threading.CancellationToken cancellationToken)
        {

            var principal = actionContext.RequestContext.Principal as ClaimsPrincipal;

            Debug.WriteLine(String.Format("Incoming principal in custom auth filter OnAuthorizationAsync method is authenticated: {0}", principal.Identity.IsAuthenticated));

            if (!principal.Identity.IsAuthenticated)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                Debug.WriteLine(String.Format("Incoming principal in custom auth filter OnAuthorizationAsync method is Not authenticated: {0}", actionContext.Response));
                return Task.FromResult<object>(null);
            }

            if (!(principal.HasClaim(x => x.Type == ClaimType && x.Value == ClaimValue)))
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                Debug.WriteLine(String.Format("Incoming principal in custom auth filter OnAuthorizationAsync method is Not Claimed: {0}", principal.Identity.Name));

                // Get the claims values
                //var Type = principal.Claims.Where(c => c.Type == ClaimTypes.Role)
                //                   .Select(c => c.Type).SingleOrDefault();

                //var Value = principal.Claims.Where(c => c.Value == "SuperAdmin")
                //                  .Select(c => c.Value).SingleOrDefault();
                return Task.FromResult<object>(null);
            }

            //User is Authorized, complete execution
            return Task.FromResult<object>(null);
        }
    }


    }