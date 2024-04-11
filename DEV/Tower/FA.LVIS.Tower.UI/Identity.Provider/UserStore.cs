using FA.LVIS.Tower.DataContracts;
using FA.LVIS.Tower.Services;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FA.LVIS.Tower.UI.Identity.Provider
{
    public sealed class UserStore<TUser>
    :
    IUserClaimStore<TUser>,
    IUserRoleStore<TUser>,
    IUserSecurityStampStore<TUser>,
    IQueryableUserStore<TUser>,
    IUserEmailStore<TUser>,
    IUserPhoneNumberStore<TUser>,
    IUserTwoFactorStore<TUser, string>,
    IUserLockoutStore<TUser, string>,
    IUserStore<TUser>,
    IUserStoreExtension<TUser>
    where TUser : IdentityUser
    {
        /// <summary>
        /// The identity client
        /// </summary>
        private IIdentityUserService<TUser> identityClient;

        //private ResourceManager userMessagesResource;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserStore{TUser}"/> class.
        /// </summary>
        /// <param name="identityClient">The identity client.</param>
        /// <exception cref="System.ArgumentNullException">identityClient</exception>
        public UserStore(IIdentityUserService<TUser> identityClient)
        {
            //userMessagesResource = new ResourceManager("FA.LVIS.Tower.UI.Resources.UserMessages", Assembly.GetExecutingAssembly());

            if (identityClient == null)
            {
                throw new ArgumentNullException("identityClient");
            }

            this.identityClient = identityClient;
        }

        /// <summary>
        /// Gets the users.
        /// </summary>
        /// <value>
        /// The users.
        /// </value>
        /// <exception cref="System.NotSupportedException"></exception>
        public IQueryable<TUser> Users
        {
            get
            {
                //We will not allow listing of all users, can be changed later if a use-case requires this
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Insert a new TUser in the UserTable
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">user</exception>
        /// <exception cref="System.ApplicationException">insert user failure</exception>
        public async Task CreateAsync(TUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (!await Task.FromResult(identityClient.InsertApplicationUser(user)))
            {
                //TODO: culture
                //var str = userMessagesResource.GetString("String1");
                throw new ApplicationException("insert user failure");
            }
        }

        /// <summary>
        /// Returns an TUser instance based on a userId query
        /// </summary>
        /// <param name="userId">The user's Id</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">Null or empty argument: userId</exception>
        public async Task<TUser> FindByIdAsync(string userId)
        {
            Common.Logger sLogger = new Common.Logger(typeof(UserStore<TUser>));

            sLogger.Debug("Calling FindByIdAsync for userid: " + userId);

            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentException("Null or empty argument: userId");
            }

            IdentityUser result = await Task.FromResult(identityClient.GetApplicationUserById(userId));

            if (result != null)
            {
                sLogger.Debug("Found FindByIdAsync for userid: " + result.UserName);
                return (TUser)result;
            }

            sLogger.Error("No results Found FindByIdAsync for userid: " + userId);
            return null;
        }

        /// <summary>
        /// Returns an TUser instance based on a userName query
        /// </summary>
        /// <param name="userName">The user's name</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">Null or empty argument: userName</exception>
        public async Task<TUser> FindByNameAsync(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                throw new ArgumentException("Null or empty argument: userName");
            }

            List<TUser> result = await Task.FromResult(identityClient.GetApplicationUserByName(userName));

            if (result != null && result.Count == 1)
            {
                return (TUser)result.FirstOrDefault();
            }

            return null;
        }

        /// <summary>
        /// Updates the UsersTable with the TUser instance values
        /// </summary>
        /// <param name="user">TUser to be updated</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">user</exception>
        /// <exception cref="System.InvalidOperationException">Could not update the user</exception>
        public async Task UpdateAsync(TUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (!await Task.FromResult(identityClient.UpdateUser(user)))
            {
                throw new InvalidOperationException("Could not update the user");
            }
        }

        /// <summary>
        /// Inserts a claim to the UserClaimsTable for the given user
        /// </summary>
        /// <param name="user">User to have claim added</param>
        /// <param name="claim">Claim to be added</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">
        /// user
        /// or
        /// user
        /// </exception>
        /// <exception cref="System.InvalidOperationException">Could not update the user</exception>
        public async Task AddClaimAsync(TUser user, Claim claim)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (claim == null)
            {
                throw new ArgumentNullException("user");
            }

            // Add claims to user's claims
            var userClaims = new UserClaims() { UserId = user.Id, Claims = new List<Claim>() { claim } };

            if (!await Task.FromResult(identityClient.AddClaims(userClaims)))
            {
                throw new InvalidOperationException("Could not update the user");
            }
        }

        /// <summary>
        /// Returns all claims for a given user
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">user</exception>
        /// <exception cref="System.FormatException">Claim is not well formed</exception>
        public async Task<IList<Claim>> GetClaimsAsync(TUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            ClaimsIdentity identity = new ClaimsIdentity();
            //IEnumerable<string> result = await Task.FromResult(identityClient.GetClaimsByUserId(user.Id));

            IEnumerable<string> result = new List<string>();
            foreach (var claimStr in result)
            {
                if (string.IsNullOrWhiteSpace(claimStr) || !claimStr.Contains(':'))
                {
                    // TODO : Exception resource
                    throw new FormatException("Claim is not well formed");
                }

                int claimStrDiv = claimStr.IndexOf(':');

                identity.AddClaim(new Claim(claimStr.Substring(0, claimStrDiv++), claimStr.Substring(claimStrDiv)));
            }

            return identity.Claims.ToList();
        }

        /// <summary>
        /// Removes a claim froma user
        /// </summary>
        /// <param name="user">User to have claim removed</param>
        /// <param name="claim">Claim to be removed</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">
        /// user
        /// or
        /// user
        /// </exception>
        /// <exception cref="System.InvalidOperationException">Could not remove the claims</exception>
        public async Task RemoveClaimAsync(TUser user, Claim claim)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (claim == null)
            {
                throw new ArgumentNullException("user");
            }

            // Create a new User claim object for list of claim removal
            var userClaims = new UserClaims() { UserId = user.Id, Claims = new List<Claim>() { claim } };

            if (!await Task.FromResult(identityClient.RemoveClaims(userClaims)))
            {
                throw new InvalidOperationException("Could not remove the claims");
            }
        }
        
        /// <summary>
        /// Inserts a entry in the UserRoles table
        /// </summary>
        /// <param name="user">User to have role added</param>
        /// <param name="roleName">Name of the role to be added to user</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">user</exception>
        /// <exception cref="System.ArgumentException">Argument cannot be null or empty: roleName.</exception>
        /// <exception cref="
        /// System.InvalidOperationException">Could not add role to user</exception>        
        public async Task AddToRoleAsync(TUser user, string roleName)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (string.IsNullOrEmpty(roleName))
            {
                throw new ArgumentException("Argument cannot be null or empty: roleName.");
            }

            if (!await Task.FromResult(identityClient.AddUserToRole(new UserRole() { UserId = user.Id, RoleId = roleName })))
            {
                throw new InvalidOperationException("Could not add role to user");
            }
        }

        /// <summary>
        /// Returns the roles for a given TUser
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">user</exception>
        public async Task<IList<string>> GetRolesAsync(TUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            var result = identityClient.FindRolesByUserId(user.Id);

            //// Identity framework expects a role, so if no roles exist, lets return a default role.
            //if (result == null)
            //{
            //    result = new Collection<string>() { "default" };
            //}

            return result.ToList();
        }

        /// <summary>
        /// Verifies if a user is in a role
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="roleName">Name of the role.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">user</exception>
        public async Task<bool> IsInRoleAsync(TUser user, string roleName)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            //var result = await Task.FromResult(identityClient.FindRolesByUserId(user.Id));

            //if (result != null && result.Contains(roleName))
            //{
            //    return true;
            //}

            return false;
        }

        /// <summary>
        /// Removes a user from a role
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="roleName">Name of the role.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">user</exception>
        /// <exception cref="System.ArgumentException">Argument cannot be null or empty: roleName.</exception>
        /// <exception cref="System.InvalidOperationException">Could not remove role from user</exception>
        public async Task RemoveFromRoleAsync(TUser user, string roleName)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (string.IsNullOrEmpty(roleName))
            {
                throw new ArgumentException("Argument cannot be null or empty: roleName.");
            }

            if (!await Task.FromResult(identityClient.RemoveUserFromRole(new UserRole() { UserId = user.Id, RoleId = roleName })))
            {
                throw new InvalidOperationException("Could not remove role from user");
            }
        }

        /// <summary>
        /// Deletes a user
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">user</exception>
        /// <exception cref="System.InvalidOperationException">Could not remove the user</exception>
        public async Task DeleteAsync(TUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (!await Task.FromResult(identityClient.RemoveUser(user)))
            {
                throw new InvalidOperationException("Could not remove the user");
            }
        }
        
        /// <summary>
        /// Find an user authenticated by Active Directory
        /// </summary>
        /// <param name="user">The user details to retrieve based on AD un/pwd</param>
        /// <returns>
        /// The user
        /// </returns>
        /// <exception cref="System.ArgumentException">Null or empty argument: userName or password</exception>
        public async Task<TUser> FindUserAsync(TUser user)
        {
            if (string.IsNullOrEmpty(user.UserName) || string.IsNullOrEmpty(user.ActiveDirectoryPassword))
            {
                throw new ArgumentException("Null or empty argument: userName or password");
            }

            List<TUser> result = await Task.FromResult(identityClient.GetActiveDirectoryUser(user));

            if (result != null && result.Count == 1)
            {
                return (TUser)result.FirstOrDefault();
            }

            return null;
        }

        /// <summary>
        /// Sets the password hash for a given TUser
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="passwordHash">The password hash.</param>
        /// <returns></returns>
        public Task SetPasswordHashAsync(TUser user, string passwordHash)
        {
            user.PasswordHash = passwordHash;
            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// Set security stamp
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="stamp">The stamp.</param>
        /// <returns></returns>
        public Task SetSecurityStampAsync(TUser user, string stamp)
        {
            user.SecurityStamp = stamp;
            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// Get security stamp
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        public Task<string> GetSecurityStampAsync(TUser user)
        {
            return Task.FromResult(user.SecurityStamp);
        }

        /// <summary>
        /// Set email on user
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="email">The email.</param>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException">Could not update the user</exception>
        public async Task SetEmailAsync(TUser user, string email)
        {
            user.Email = email;
            if (!await Task.FromResult(identityClient.UpdateUser(user)))
            {
                throw new InvalidOperationException("Could not update the user");
            }
        }

        /// <summary>
        /// Get email from user
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        public Task<string> GetEmailAsync(TUser user)
        {
            return Task.FromResult(user.Email);
        }

        /// <summary>
        /// Get if user email is confirmed
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        public Task<bool> GetEmailConfirmedAsync(TUser user)
        {
            return Task.FromResult(user.EmailConfirmed);
        }

        /// <summary>
        /// Set when user email is confirmed
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="confirmed">if set to <c>true</c> [confirmed].</param>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException">Could not update the user</exception>
        public async Task SetEmailConfirmedAsync(TUser user, bool confirmed)
        {
            user.EmailConfirmed = confirmed;
            if (!await Task.FromResult(identityClient.UpdateUser(user)))
            {
                throw new InvalidOperationException("Could not update the user");
            }
        }

        /// <summary>
        /// Get user by email
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns></returns>
        public async Task<TUser> FindByEmailAsync(string email)
        {
            return await Task.FromResult<TUser>((TUser)identityClient.FindByEmail(email));
        }

        /// <summary>
        /// Set user phone number
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="phoneNumber">The phone number.</param>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException">Could not update the user</exception>
        public async Task SetPhoneNumberAsync(TUser user, string phoneNumber)
        {
            user.PhoneNumber = phoneNumber;
            if (!await Task.FromResult(identityClient.UpdateUser(user)))
            {
                throw new InvalidOperationException("Could not update the user");
            }
        }

        /// <summary>
        /// Get user phone number
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        public Task<string> GetPhoneNumberAsync(TUser user)
        {
            return Task.FromResult(user.PhoneNumber);
        }

        /// <summary>
        /// Get if user phone number is confirmed
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        public Task<bool> GetPhoneNumberConfirmedAsync(TUser user)
        {
            return Task.FromResult(user.PhoneNumberConfirmed);
        }

        /// <summary>
        /// Set phone number if confirmed
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="confirmed">if set to <c>true</c> [confirmed].</param>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException">Could not update the user</exception>
        public async Task SetPhoneNumberConfirmedAsync(TUser user, bool confirmed)
        {
            user.PhoneNumberConfirmed = confirmed;
            if (!await Task.FromResult(identityClient.UpdateUser(user)))
            {
                throw new InvalidOperationException("Could not update the user");
            }
        }

        /// <summary>
        /// Set two factor authentication is enabled on the user
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="enabled">if set to <c>true</c> [enabled].</param>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException">Could not update the user</exception>
        public async Task SetTwoFactorEnabledAsync(TUser user, bool enabled)
        {
            user.TwoFactorEnabled = enabled;
            if (!await Task.FromResult(identityClient.UpdateUser(user)))
            {
                throw new InvalidOperationException("Could not update the user");
            }
        }

        /// <summary>
        /// Get if two factor authentication is enabled on the user
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        public Task<bool> GetTwoFactorEnabledAsync(TUser user)
        {
            return Task.FromResult(user.TwoFactorEnabled);
        }

        /// <summary>
        /// Get user lock out end date
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        public Task<DateTimeOffset> GetLockoutEndDateAsync(TUser user)
        {
            return
                Task.FromResult(user.LockoutEndDateUtc.HasValue
                    ? new DateTimeOffset(DateTime.SpecifyKind(user.LockoutEndDateUtc.Value, DateTimeKind.Utc))
                    : new DateTimeOffset());
        }


        /// <summary>
        /// Set user lockout end date
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="lockoutEnd">The lockout end.</param>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException">Could not update the user</exception>
        public async Task SetLockoutEndDateAsync(TUser user, DateTimeOffset lockoutEnd)
        {
            user.LockoutEndDateUtc = lockoutEnd.UtcDateTime;
            if (!await Task.FromResult(identityClient.UpdateUser(user)))
            {
                throw new InvalidOperationException("Could not update the user");
            }
        }

        /// <summary>
        /// Increment failed access count
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException">Could not update the user</exception>
        public async Task<int> IncrementAccessFailedCountAsync(TUser user)
        {
            user.AccessFailedCount++;
            if (!await Task.FromResult(identityClient.UpdateUser(user)))
            {
                throw new InvalidOperationException("Could not update the user");
            }

            return user.AccessFailedCount;
        }

        /// <summary>
        /// Reset failed access count
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException">Could not update the user</exception>
        public async Task ResetAccessFailedCountAsync(TUser user)
        {
            user.AccessFailedCount = 0;
            if (!await Task.FromResult(identityClient.UpdateUser(user)))
            {
                throw new InvalidOperationException("Could not update the user");
            }
        }

        /// <summary>
        /// Get failed access count
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        public Task<int> GetAccessFailedCountAsync(TUser user)
        {
            return Task.FromResult(user.AccessFailedCount);
        }

        /// <summary>
        /// Get if lockout is enabled for the user
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        public Task<bool> GetLockoutEnabledAsync(TUser user)
        {
            return Task.FromResult(user.LockoutEnabled);
        }

        /// <summary>
        /// Set lockout enabled for user
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="enabled">if set to <c>true</c> [enabled].</param>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException">Could not update the user</exception>
        public async Task SetLockoutEnabledAsync(TUser user, bool enabled)
        {
            user.LockoutEnabled = enabled;
            if (!await Task.FromResult(identityClient.UpdateUser(user)))
            {
                throw new InvalidOperationException("Could not update the user");
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (identityClient != null)
            {
                //identityClient.Dispose();
                identityClient = null;
            }
        }
    }
}