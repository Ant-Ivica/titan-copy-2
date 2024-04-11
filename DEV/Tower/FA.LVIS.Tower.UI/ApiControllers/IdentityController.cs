using FA.LVIS.Tower.DataContracts;
using FA.LVIS.Tower.Services;
using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Web.Http;

namespace FA.LVIS.Tower.UI.ApiControllers
{
    [RoutePrefix("Identity")]
    [AllowAnonymous]
    public class IdentityController : ApiController
    {
        //[Route("GetUserByName/{userName}", Name = "GetUserByName")]
        //[HttpGet]
        //public List<ApplicationUser> GetUserByName([FromBody] string userName)
        //{
        //    IIdentityUserService<ApplicationUser> IdentityService = ServiceFactory.Resolve<IIdentityUserService<ApplicationUser>>();
        //    return IdentityService.GetApplicationUserByName(userName);
        //}

        //[Route("GetUserById/{userId}", Name = "GetUserById")]
        //[HttpGet]
        //public ApplicationUser GetUserById(string userId)
        //{
        //    IIdentityUserService<ApplicationUser> IdentityService = ServiceFactory.Resolve<IIdentityUserService<ApplicationUser>>();
        //     ApplicationUser user =  IdentityService.GetApplicationUserById(userId);

        //    return user;
        //}


        //[Route("FindRolesByUserId/{userId}", Name = "FindRolesByUserId")]
        //[HttpGet]
        //public IEnumerable<string> FindRolesByUserId(string userId)
        //{
        //    IIdentityUserService<ApplicationUser> IdentityService = ServiceFactory.Resolve<IIdentityUserService<ApplicationUser>>();
        //    return IdentityService.FindRolesByUserId(userId);
        //}

      
        //[Route("GetClaimsByUser", Name = "GetClaimsByUser")]
        //[HttpGet]
        //public IEnumerable<string> GetClaimsByUser(ApplicationUser user)
        //{
        //    IIdentityUserService<ApplicationUser> IdentityService = ServiceFactory.Resolve<IIdentityUserService<ApplicationUser>>();

        //    ApplicationUser appuser = GetUserByName(System.Web.HttpContext.Current.Request.LogonUserIdentity.Name).FirstOrDefault();

        //    return IdentityService.GetClaimsByUser(appuser);
        //}
    }
}
