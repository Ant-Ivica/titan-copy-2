using FA.LVIS.Tower.DataContracts;
using System.Collections.Generic;

namespace FA.LVIS.Tower.Services
{
    public interface IIdentityUserService<TUser>: Core.IServiceBase where TUser : IdentityUser
    {
        bool InsertApplicationUser(TUser user);

        List<TUser> GetActiveDirectoryUser(TUser user);

        List<TUser> GetApplicationUserByName(string userName);

        TUser GetApplicationUserById(string userId);

        IEnumerable<string> GetClaimsByUserId(string userId);

        //IEnumerable<string> GetClaimsByUser(TUser user);

        TUser FindByEmail(string emailId);

        bool UpdateUser(TUser user);

        bool AddClaims(UserClaims userClaims);

        bool RemoveClaims(UserClaims userClaims);

        bool RemoveUser(TUser user);
                
        bool AddUserToRole(UserRole userRole);
                
        IEnumerable<string> FindRolesByUserId(string userId);

        //string GetRoleByUserId(string userId);

        bool RemoveUserFromRole(UserRole userRole);
    }
}
