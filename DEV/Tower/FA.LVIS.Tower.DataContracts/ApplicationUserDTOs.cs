using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FA.LVIS.Tower.DataContracts
{
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// Generates the user identity asynchronous.
        /// </summary>
        /// <param name="manager">The manager.</param>
        /// <returns></returns>
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);

            userIdentity.AddClaim(new Claim(Constants.USER_ID, this.UserId.ToString()));
            userIdentity.AddClaim(new Claim(Constants.EMPLOYEE_ID, this.EmployeeId.ToString()));
            userIdentity.AddClaim(new Claim(Constants.TENANT_ID, this.TenantId.ToString()));
            this.ActivityRight = userIdentity.Claims.Where(c => c.Type == ClaimTypes.Role).ToList().FirstOrDefault().Value;

            return userIdentity;
        }

        /// <summary>
        /// User's real name (Demo example)
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        public string EmployeeId { get; set; }

        public bool IsAdUser
        {
            get
            {
                bool result = false;
                if (!string.IsNullOrEmpty(UserName))
                {
                    if (UserName.Contains("\\"))
                    {
                        result = true;
                    }

                    if (UserName.Contains("/") && !UserName.Contains('@'))
                    {
                        result = true;
                    }

                    if (UserName.EndsWith("firstam.com"))
                    {
                        result = true;
                    }

                    if (!UserName.Contains('@'))
                    {
                        result = true;
                    }
                }
                return result;
            }
        }

        public string ActivityRight { get; set; }

        public bool CanManageTEQ { get; set; }

        public bool CanManageBEQ { get; set; }
        public bool CanAccessReq { get; set; }
        public string TenantId { get; set; }

        public int UserId { get; set; }
    }
}
