using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace FA.LVIS.Tower.UI
{
    public class ApplicationUserManager : Identity.Provider.UserManager<DataContracts.ApplicationUser>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationUserManager"/> class.
        /// </summary>
        /// <param name="store">The store.</param>
        public ApplicationUserManager(IUserStore<DataContracts.ApplicationUser> store)
            : base(store)
        {
        }

        /// <summary>
        /// Creates and instance of default ApplicationUserManager using WebAPI and FA Policies, FA email service 
        /// </summary>
        /// <param name="options">Options from OWIN context</param>
        /// <param name="context">The OWIN context</param>
        /// <returns></returns>
        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var manager = new ApplicationUserManager(new Identity.Provider.UserStore<DataContracts.ApplicationUser>(
                new Identity.Provider.UserWebApiClient<DataContracts.ApplicationUser>("~/Security")));

            //var dataProtectionProvider = options.DataProtectionProvider;
            //if (dataProtectionProvider != null)
            //{
            //    manager.UserTokenProvider = new DataProtectorTokenProvider<DataContracts.ApplicationUser>(dataProtectionProvider.Create("ConfirmUser"));
            //}
            return manager;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext()
            : base("Entities", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}