using FA.LVIS.Tower.DataContracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace FA.LVIS.Tower.Data
{
    public class IdentityUserDataProvider : Core.DataProviderBase, Data.IIdentityUserDataProvider<ApplicationUser>
    {
        Common.Logger sLogger;

        public IdentityUserDataProvider()
        {
            sLogger = new Common.Logger(typeof(IdentityUserDataProvider));
        }

        private bool AuthenticateUsingAd(string userName, string password)
        {
            return false;

        }

        public List<ApplicationUser> GetUserByName(string userName)
        {
            sLogger.Debug("GetUserByName: " + userName);

            List<ApplicationUser> users = new List<ApplicationUser>();
            using (var dbContext = new TerminalDBEntities.Entities())
            {
                var Matched = dbContext.Tower_Users
                    .Where(user => user.UserName.ToLower() == userName.ToLower() && user.IsActive == true);

                sLogger.Debug("GetUserByName found user: " + Matched.Count());

                if (Matched.Count() > 0)
                {
                    users = Matched.Select(se => new ApplicationUser
                    {
                        Id = se.Id,
                        Email = se.Email,
                        UserName = se.UserName,
                        UserId = se.UserId,
                        Name = se.Name,
                        EmployeeId = se.EmployeeId.ToString(),
                        AccessFailedCount = se.AccessFailedCount == null ? 0 : (int)se.AccessFailedCount,
                        EmailConfirmed = se.EmailConfirmed == null ? false : (bool)se.EmailConfirmed,
                        PasswordHash = se.PasswordHash == null ? string.Empty : se.PasswordHash,
                        PhoneNumber = se.PhoneNumber == null ? string.Empty : se.PhoneNumber,
                        PhoneNumberConfirmed = se.PhoneNumberConfirmed == null ? false : (bool)se.PhoneNumberConfirmed,
                        SecurityStamp = se.SecurityStamp == null ? Guid.NewGuid().ToString() : se.SecurityStamp,
                        TwoFactorEnabled = se.TwoFactorEnabled == null ? false : (bool)se.TwoFactorEnabled,
                        LockoutEndDateUtc = se.LockoutEndDateUtc == null ? DateTime.Now : se.LockoutEndDateUtc,
                        LockoutEnabled = se.LockoutEnabled == null ? false : (bool)se.LockoutEnabled,
                        TenantId =se.TenantId.ToString(),
                        ActivityRight = se.Tower_Roles.Select(s1 => s1.Name).FirstOrDefault()
                    }).ToList();
                }
            }

            return users;
        }

        public ApplicationUser GetUserById(string id)
        {
            sLogger.Debug("GetUserById: " + id.ToLower());

            ApplicationUser users = new ApplicationUser();

            using (var dbContext = new TerminalDBEntities.Entities())
            {
                var Matched = dbContext.Tower_Users.Where(user => user.Id.ToLower() == id.ToLower() &&  user.IsActive == true);

                sLogger.Debug("GetUserById found user: " + Matched.Count());

                if (Matched.Count() > 0)
                {
                    //userRole = Matched.First().Tower_Roles.Select(sl => sl.Name).First();

                    users = Matched.Select(se => new ApplicationUser
                    {
                        Id = se.Id,
                        Email = se.Email,
                        UserName = se.UserName,
                        UserId = se.UserId,
                        Name = se.Name,
                        EmployeeId = se.EmployeeId.ToString(),
                        AccessFailedCount = se.AccessFailedCount == null ? 0 : (int)se.AccessFailedCount,
                        EmailConfirmed = se.EmailConfirmed == null ? false : (bool)se.EmailConfirmed,
                        PasswordHash = se.PasswordHash == null ? string.Empty : se.PasswordHash,
                        PhoneNumber = se.PhoneNumber == null ? string.Empty : se.PhoneNumber,
                        PhoneNumberConfirmed = se.PhoneNumberConfirmed == null ? false : (bool)se.PhoneNumberConfirmed,
                        SecurityStamp = se.SecurityStamp == null ? Guid.NewGuid().ToString() : se.SecurityStamp,
                        TwoFactorEnabled = se.TwoFactorEnabled == null ? false : (bool)se.TwoFactorEnabled,
                        LockoutEndDateUtc = se.LockoutEndDateUtc == null ? DateTime.Now : se.LockoutEndDateUtc,
                        LockoutEnabled = se.LockoutEnabled == null ? false : (bool)se.LockoutEnabled,
                        TenantId = se.TenantId.ToString(),
                        ActivityRight = se.Tower_Roles.Select(s1 => s1.Name).FirstOrDefault()
                    }).FirstOrDefault();
                }
            }

            //users.ActivityRight = FindRolesByUserId(users.Id).First();
            sLogger.Debug("GetUserById found user: " + users.UserName);

            return users;
        }

        public IEnumerable<string> FindRolesByUserId(string userId)
        {
            sLogger.Debug("FindRolesByUserId: " + userId);

            List<string> userRole = new List<string>();

            using (var dbContext = new TerminalDBEntities.Entities())
            {
                var Matched = dbContext.Tower_Users
                        .Where(user => user.Id == userId).First();

                if (Matched != null && Matched.Tower_Roles.Count() > 0)
                {
                    userRole = Matched.Tower_Roles.Select(sl => sl.Name).ToList();
                }
            }

            sLogger.Debug("FindRolesByUserId found");

            return userRole;
        }

        public IEnumerable<string> GetClaimsByUser(ApplicationUser user)
        {
            List<string> userClaims = new List<string>();
            using (var dbContext = new TerminalDBEntities.Entities())
            {
                var Matched = dbContext.Tower_UserClaims
                        .Where(Claim => Claim.UserId == user.Id);

                if (Matched != null && Matched.Count() > 0)
                {
                    userClaims = Matched.Select(sl => sl.ClaimType + ":" + sl.ClaimValue).ToList();
                }
            }

            return userClaims;
        }

        public void Dispose()
        {

        }

        public bool InsertUser(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public List<ApplicationUser> GetActiveDirectoryUser(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> GetClaimsByUserId(string userId)
        {
            List<string> userClaims = new List<string>();
            using (var dbContext = new TerminalDBEntities.Entities())
            {
                var Matched = dbContext.Tower_UserClaims
                        .Where(Claim => Claim.UserId == userId);

                if (Matched != null && Matched.Count() > 0)
                {
                    userClaims = Matched.Select(sl => sl.ClaimType + ":" + sl.ClaimValue).ToList();
                }
            }

            return userClaims;
        }

        public ApplicationUser FindByEmail(string emailId)
        {
            throw new NotImplementedException();
        }

        public bool UpdateUser(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public bool AddClaims(UserClaims userClaims)
        {
            throw new NotImplementedException();
        }

        public bool RemoveClaims(UserClaims userClaims)
        {
            throw new NotImplementedException();
        }

        public bool RemoveUser(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public bool AddUserToRole(UserRole userRole)
        {
            throw new NotImplementedException();
        }

        public bool RemoveUserFromRole(UserRole userRole)
        {
            throw new NotImplementedException();
        }
    }
}
