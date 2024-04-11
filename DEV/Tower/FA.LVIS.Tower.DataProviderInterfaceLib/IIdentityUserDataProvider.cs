using FA.LVIS.Tower.Core;
using FA.LVIS.Tower.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FA.LVIS.Tower.Data
{
    public interface IIdentityUserDataProvider<TUser> : IDataProviderBase, IDisposable where TUser : IdentityUser
    {
        /// <summary>
        /// Inserts a new user into the data store, typically called when a new user is registered in the system
        /// </summary>
        /// <param name="user">The user details</param>
        /// <returns>
        /// True indicating a success
        /// </returns>
        bool InsertUser(TUser user);

        /// <summary>
        /// Gets the users, takes in username and password, validates it against active directory and return the user details from the data store
        /// </summary>
        /// <param name="user">Username and password from User object</param>
        /// <returns>
        /// List of usernames
        /// </returns>
        List<TUser> GetActiveDirectoryUser(TUser user);

        /// <summary>
        /// Gets the users based on the username
        /// </summary>
        /// <param name="userName">Username to obtain the users</param>
        /// <returns>
        /// List of users
        /// </returns>
        List<TUser> GetUserByName(string userName);

        /// <summary>
        /// Gets the users based on UserId
        /// </summary>
        /// <param name="userId">The unique userid</param>
        /// <returns>
        /// The user with the unique userid
        /// </returns>
        TUser GetUserById(string userId);

        /// <summary>
        /// Gets the Claims by UserId
        /// </summary>
        /// <param name="userId">The unique userid</param>
        /// <returns>
        /// IEnumerable collections of string representing the claims
        /// </returns>
        IEnumerable<string> GetClaimsByUserId(string userId);


        /// <summary>
        /// Gets the Claims by User
        /// </summary>
        /// <param name="userId">The unique userid</param>
        /// <returns>
        /// IEnumerable collections of string representing the claims
        /// </returns>
        IEnumerable<string> GetClaimsByUser(TUser user);

        /// <summary>
        /// Gets the user by unique email id
        /// </summary>
        /// <param name="emailId">The email id</param>
        /// <returns>
        /// The user details
        /// </returns>
        TUser FindByEmail(string emailId);

        /// <summary>
        /// Update the user details in data store
        /// </summary>
        /// <param name="user">The user details</param>
        /// <returns>
        /// True if update succeeded
        /// </returns>
        bool UpdateUser(TUser user);

        /// <summary>
        /// Adds a set of claims to an user
        /// </summary>
        /// <param name="userClaims">The user details along with Claim information to be updated</param>
        /// <returns>
        /// True if add succeeded
        /// </returns>
        bool AddClaims(UserClaims userClaims);

        /// <summary>
        /// Removes a set of claims of an user
        /// </summary>
        /// <param name="userClaims">The user details along with Claims to be removed</param>
        /// <returns>
        /// True if remove succeeded
        /// </returns>
        bool RemoveClaims(UserClaims userClaims);

        /// <summary>
        /// Removes the user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>True if remove succeeded</returns>
        bool RemoveUser(TUser user);

        /// <summary>
        /// Adds the user to role.
        /// </summary>
        /// <param name="userRole">The user role.</param>
        /// <returns>True if add succeeded</returns>
        bool AddUserToRole(UserRole userRole);

        /// <summary>
        /// Finds the roles by user identifier.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>Collection of roles</returns>
        IEnumerable<string> FindRolesByUserId(string userId);

        /// <summary>
        /// Removes the user from role.
        /// </summary>
        /// <param name="userRole">The user role.</param>
        /// <returns>True if remove succeeded</returns>
        bool RemoveUserFromRole(UserRole userRole);
    }
}
