using FA.LVIS.Tower.DataContracts;
using FA.LVIS.Tower.Services;
//using FA.LVIS.Tower.Identity.Interfaces;
//using FA.LVIS.Tower.Identity.Providers;
using System;
using System.Collections.Generic;

namespace FA.LVIS.Tower.UI.Identity.Provider
{
    public sealed class UserWebApiClient<TUser> : IDisposable, IIdentityUserService<TUser> where TUser : IdentityUser
    {
        /// <summary>
        /// The identity web API
        /// </summary>
        private ApplicationServiceClient identityWebApi;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserWebApiClient{TUser}"/> class.
        /// </summary>
        /// <param name="webApiUrl">The web API URL.</param>
        public UserWebApiClient(string webApiUrl)
        {
            identityWebApi = new ApplicationServiceClient(webApiUrl);
        }

        /// <summary>
        /// Inserts a new user into the data store, typically called when a new user is registered in the system
        /// </summary>
        /// <param name="user">The user details</param>
        /// <returns>
        /// True indicating a success
        /// </returns>
        public bool InsertApplicationUser(TUser user)
        {
            return identityWebApi.Post<bool, TUser>("InsertUser", user);
        }

        /// <summary>
        /// Gets the users, takes in username and password, validates it against active directory and return the user details from the data store
        /// </summary>
        /// <param name="user">Username and password from User object</param>
        /// <returns>
        /// List of usernames
        /// </returns>
        public List<TUser> GetActiveDirectoryUser(TUser user)
        {
            return identityWebApi.Get<List<TUser>>("GetActiveDirectoryUser");
        }

        /// <summary>
        /// Gets the users based on the username
        /// </summary>
        /// <param name="userName">Username to obtain the users</param>
        /// <returns>
        /// List of users
        /// </returns>
        public List<TUser> GetApplicationUserByName(string userName)
        {
            return identityWebApi.Get<List<TUser>>("GetUserByName", userName);
        }

        /// <summary>
        /// Gets the users based on UserId
        /// </summary>
        /// <param name="userId">The unique userid</param>
        /// <returns>
        /// The user with the unique userid
        /// </returns>
        public TUser GetApplicationUserById(string userId)
        {
            return identityWebApi.Get<TUser>("GetUserById", userId);
        }

        /// <summary>
        /// Gets the Claims by UserId
        /// </summary>
        /// <param name="userId">The unique userid</param>
        /// <returns>
        /// IEnumerable collections of string representing the claims
        /// </returns>
        public IEnumerable<string> GetClaimsByUserId(string userId)
        {
            return identityWebApi.Get<IEnumerable<string>>("GetClaimsByUserId", userId);
        }

        /// <summary>
        /// Gets the Claims by User
        /// </summary>
        /// <param name="userId">The unique userid</param>
        /// <returns>
        /// IEnumerable collections of string representing the claims
        /// </returns>
        //public IEnumerable<string> GetClaimsByUser(TUser user)
        //{
        //    return identityWebApi.Get<IEnumerable<string>>("GetClaimsByUser");
        //}

        /// <summary>
        /// Gets the password hash for a userid
        /// </summary>
        /// <param name="userId">The unique userId</param>
        /// <returns>
        /// The password hash for the userid
        /// </returns>
        public string GetPasswordHash(string userId)
        {
            return identityWebApi.Post<string, string>("GetPasswordHash", userId);
        }

        /// <summary>
        /// Gets the user by unique email id
        /// </summary>
        /// <param name="emailId">The email id</param>
        /// <returns>
        /// The user details
        /// </returns>
        public TUser FindByEmail(string emailId)
        {
            return identityWebApi.Get<TUser>("FindByEmail", emailId);
        }

        /// <summary>
        /// Update the user details in data store
        /// </summary>
        /// <param name="user">The user details</param>
        /// <returns>
        /// True if update succeeded
        /// </returns>
        public bool UpdateUser(TUser user)
        {
            return identityWebApi.Post<bool, TUser>("UpdateUser", user);
        }

        /// <summary>
        /// Adds a set of claims to an user
        /// </summary>
        /// <param name="userClaims">The user details along with Claim information to be updated</param>
        /// <returns>
        /// True if add succeeded
        /// </returns>
        public bool AddClaims(UserClaims userClaims)
        {
            return identityWebApi.Post<bool, UserClaims>("AddClaims", userClaims);
        }

        /// <summary>
        /// Removes a set of claims of an user
        /// </summary>
        /// <param name="userClaims">The user details along with Claims to be removed</param>
        /// <returns>
        /// True if remove succeeded
        /// </returns>
        public bool RemoveClaims(UserClaims userClaims)
        {
            return identityWebApi.Post<bool, UserClaims>("RemoveClaims", userClaims);
        }       

        /// <summary>
        /// Removes the user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>
        /// True if remove succeeded
        /// </returns>
        public bool RemoveUser(TUser user)
        {
            return identityWebApi.Post<bool, TUser>("RemoveUser", user);
        }

        /// <summary>
        /// Adds the user to role.
        /// </summary>
        /// <param name="userRole">The user role.</param>
        /// <returns>
        /// True if add succeeded
        /// </returns>
        public bool AddUserToRole(UserRole userRole)
        {
            return identityWebApi.Post<bool, UserRole>("AddUserToRole", userRole);
        }

        /// <summary>
        /// Removes the user from role.
        /// </summary>
        /// <param name="userRole">The user role.</param>
        /// <returns>
        /// True if remove succeeded
        /// </returns>
        public bool RemoveUserFromRole(UserRole userRole)
        {
            return identityWebApi.Post<bool, UserRole>("RemoveUserFromRole", userRole);
        }

        /// <summary>
        /// Finds the roles by user identifier.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>
        /// Collection of roles
        /// </returns>
        public IEnumerable<string> FindRolesByUserId(string userId)
        {
            return identityWebApi.Get<IEnumerable<string>>("FindRolesByUserId", userId);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (identityWebApi != null)
            {
                identityWebApi.Dispose();
                identityWebApi = null;
            }
        }
    }
}