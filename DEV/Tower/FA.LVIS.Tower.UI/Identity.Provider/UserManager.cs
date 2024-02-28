using FA.LVIS.Tower.DataContracts;
using FA.LVIS.Tower.Services;
using Microsoft.AspNet.Identity;
using System;
using System.Threading.Tasks;

namespace FA.LVIS.Tower.UI.Identity.Provider
{
    public class UserManager<TUser> : Microsoft.AspNet.Identity.UserManager<TUser> where TUser : IdentityUser
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserManager{TUser}"/> class.
        /// </summary>
        /// <param name="store">The store.</param>
        public UserManager(IUserStore<TUser> store)
            : base(store)
        {
        }

        /// <summary>
        /// Gets or sets the domain validator.
        /// </summary>
        /// <value>
        /// The domain validator.
        /// </value>
        public IDomainUserValidator DomainValidator { get; set; }

        /// <summary>
        /// Gets or sets the password history limit.
        /// </summary>
        /// <value>
        /// The password history limit.
        /// </value>
        public int? PasswordHistoryLimit { get; set; }

        /// <summary>
        /// Gets or sets the password expiry in days.
        /// </summary>
        /// <value>
        /// The password expiry in days.
        /// </value>
        public int? PasswordExpiryInDays { get; set; }

        /// <summary>
        /// Finds the asynchronous.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        public override async Task<TUser> FindAsync(string userName, string password)
        {
            TUser tUser;

            // If the account is managed through AD, directly call the user store without hashing the password
            if (DomainValidator.IsDomainUserName(userName))
            {
                var storeExtension = this.Store as IUserStore<TUser>;
                //if (storeExtension == null) return null;
                var user = (TUser)Activator.CreateInstance(typeof(TUser));
                //user.ActiveDirectoryPassword = password;
                //user.UserName = userName;

                Common.Logger sLogger = new Common.Logger(typeof(UserManager<TUser>));

                sLogger.Debug("Calling FindByIdAsync in FindAsync for user: " + user.Id);

                tUser = await storeExtension.FindByIdAsync(user.Id);

                sLogger.Debug("Received FindByIdAsync in FindAsync for user: " + tUser.UserName);
            }
            else
            {
                tUser = await base.FindAsync(userName, password);
            }

            return tUser;
        }
    }
}