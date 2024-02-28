using FA.LVIS.Tower.Core;
using FA.LVIS.Tower.Data.TerminalDBEntities;
using FA.LVIS.Tower.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading;

namespace FA.LVIS.Tower.Data
{
    public class UserSecurityDataProvider : Core.DataProviderBase, IUserSecurityDataProvider
    {
        private Entities dbContext;

        public UserSecurityDataProvider()
        {
            dbContext = new Entities();
        }
        public UserSecurityDataProvider(Entities Context)
        {
            dbContext = Context;
        }

        void AddClaim(UserProfile UserProfile)
        {          
            //string sGroup = string.Empty;
            List<Tower_UserClaims> UserClaim = new List<Tower_UserClaims>();
  
            if (UserProfile.ManageBEQ)
            UserClaim.Add(new Tower_UserClaims { UserId = UserProfile.ID, ClaimType = Constants.CLAIM_MANAGEBEQ, ClaimValue = UserProfile.ManageBEQ.ToString() });

            if (UserProfile.ManageTEQ)
            UserClaim.Add(new Tower_UserClaims { UserId = UserProfile.ID, ClaimType = Constants.CLAIM_MANAGETEQ, ClaimValue = UserProfile.ManageTEQ.ToString() });

            if (UserProfile.ManageAccessREQ)
                UserClaim.Add(new Tower_UserClaims { UserId = UserProfile.ID, ClaimType = Constants.CLAIM_MANAGEACCESSREQ, ClaimValue = UserProfile.ManageAccessREQ.ToString() });

            dbContext.Tower_UserClaims.AddRange(UserClaim);
            AuditLogHelper.SaveChanges(dbContext);
        }

        public List<UserProfile> AddUser(UserProfile UserProfile, int tenantId)
        {         

            List<UserProfile> ApplicationUsers = new List<UserProfile>();
            var SelectUser = (from users in dbContext.Tower_Users
                      .Where(user => user.UserName == UserProfile.UserName)
                              select users);

            if (SelectUser.Count() > 0)
                return ApplicationUsers;

            Tower_Users User = new Tower_Users();
            User.Id = Guid.NewGuid().ToString();
            User.Name = UserProfile.Name;
            User.UserName = UserProfile.UserName;
            User.IsActive = UserProfile.IsActive;
            User.Email = UserProfile.Emailid;
            User.EmployeeId = UserProfile.Employeeid.ToIntDefEx();
            if (tenantId != ((int)TerminalDBEntities.TenantIdEnum.LVIS))
                User.TenantId = tenantId;
            else
                User.TenantId = UserProfile.TenantId;

            User.Tower_Roles = dbContext.Tower_Roles.Where(na => na.Name == UserProfile.Role).ToList();
            dbContext.Tower_Users.Add(User);

            AuditLogHelper.SaveChanges(dbContext);


                AuditLog logModified = new AuditLog()
                {
                    UserName = (Thread.CurrentPrincipal as IPrincipal).Identity.Name,
                    EventDateutc = DateTime.Now,
                    EventType = AuditLogEventTypeEnum.Added.ToString(),
                    TableName = "Tower.UserRoles",
                    RecordId = User.Id,
                    Property = "ALL",
                    NewValue = "{Userid:" + User.Id + ",Role:" + UserProfile.Role + "}",
                    Section = "Security"
                };

                dbContext.AuditLogs.Add(logModified);

            dbContext.SaveChanges();
            

            UserProfile.ID = User.Id;
            UserProfile.UserId = User.UserId;
            UserProfile.sTenant = dbContext.Tenants.Where(se => se.TenantId == User.TenantId).FirstOrDefault().TenantName;

            AddClaim(UserProfile);
            return GetApplicationUsers(tenantId);
        }

        public int Deleteuser(UserProfile UserProfile)
        {
          


                IEnumerable<Tower_UserClaims> CalimstoUpdate = dbContext.Tower_UserClaims
                    .RemoveRange(dbContext.Tower_UserClaims
                    .Where(user => user.UserId == UserProfile.ID));
                AuditLogHelper.SaveChanges(dbContext);


                IEnumerable<TerminalDBEntities.Tower_Users> UsersToDelet = dbContext.Tower_Users
                  .RemoveRange(dbContext.Tower_Users
                    .Where(user => user.Id == UserProfile.ID));

                return AuditLogHelper.SaveChanges(dbContext);
            
        }

        public List<UserProfile> GetApplicationUsers(int iTenant)
        {
          

            List<UserProfile> ApplicationUsers = new List<UserProfile>();
            if (dbContext.Tower_Users.Count() > 0)
            {
                ApplicationUsers = dbContext.Tower_Users
                    .Select(Se => new
                    {
                        ID = Se.Id,
                        EmailID = Se.Email,
                        UserName = Se.UserName,
                        UserId = Se.UserId,
                        Name = Se.Name,
                        isActive = Se.IsActive,
                        sTenant = Se.Tenant.TenantName,
                        tenantId = Se.TenantId,
                        Role = Se.Tower_Roles.Select(role => role.Name).FirstOrDefault(),

                        ManageTEQ = Se.Tower_UserClaims.Where(r => r.ClaimType == Constants.CLAIM_MANAGETEQ)
                                                  .Select(Gr => Gr.ClaimValue).FirstOrDefault(),
                        ManageBEQ = Se.Tower_UserClaims.Where(r => r.ClaimType == Constants.CLAIM_MANAGEBEQ)
                                                  .Select(Gr => Gr.ClaimValue).FirstOrDefault(),
                        ManageAccessREQ = Se.Tower_UserClaims.Where(r => r.ClaimType == Constants.CLAIM_MANAGEACCESSREQ)
                                                  .Select(Gr => Gr.ClaimValue).FirstOrDefault()

                    }).AsEnumerable()
                                   .Select(x => new UserProfile
                                   {
                                       ID = x.ID,
                                       UserName = x.UserName,
                                       UserId = x.UserId,
                                       Name = x.Name,
                                       Emailid = x.EmailID,
                                       IsActive = x.isActive,
                                       sTenant = x.sTenant,
                                       TenantId = x.tenantId,
                                       Role = x.Role,
                                       ManageBEQ = x.ManageBEQ == null ? false : Convert.ToBoolean(x.ManageBEQ),
                                       ManageTEQ = x.ManageTEQ == null ? false : Convert.ToBoolean(x.ManageTEQ),
                                       ManageAccessREQ = x.ManageAccessREQ == null ? false : Convert.ToBoolean(x.ManageAccessREQ)
                                   }).ToList();
            }


            if (ApplicationUsers.Count() > 0 && iTenant != (int)TenantIdEnum.LVIS)
            {
                ApplicationUsers = ApplicationUsers
                    .Where(sel => sel.TenantId == iTenant).ToList();
            }

            
            return ApplicationUsers;
        }

        public UserProfile GetSingleUser(UserProfile value)
        {
            UserProfile User = new UserProfile();
         
                var SelectUser = (from users in dbContext.Tower_Users
                          .Where(user => user.UserName == value.UserName)
                                  select users).FirstOrDefault();

                if (SelectUser != null)
                {
                    User.ID = SelectUser.Id;
                    User.Name = SelectUser.Name;
                    User.UserName = SelectUser.UserName;
                    User.UserId = SelectUser.UserId;
                    User.Role = SelectUser.Tower_Roles.Select(role => role.Name).FirstOrDefault();
                    User.IsActive = SelectUser.IsActive;
                    User.Emailid = SelectUser.Email;
                    User.sTenant = SelectUser.Tenant.TenantName;
                    User.TenantId = SelectUser.TenantId;
                    User.ManageTEQ = dbContext.Tower_UserClaims
                                                  .Where(r => r.UserId == SelectUser.Id && r.ClaimType == Constants.CLAIM_MANAGETEQ)
                                                  .Select(Gr => Gr.ClaimValue).FirstOrDefault() == string.Empty ? false : Convert.ToBoolean(dbContext.Tower_UserClaims
                                                  .Where(r => r.UserId == SelectUser.Id && r.ClaimType == Constants.CLAIM_MANAGETEQ)
                                                  .Select(Gr => Gr.ClaimValue).FirstOrDefault());
                    User.ManageBEQ = dbContext.Tower_UserClaims
                                             .Where(r => r.UserId == SelectUser.Id && r.ClaimType == Constants.CLAIM_MANAGEBEQ)
                                             .Select(Gr => Gr.ClaimValue).FirstOrDefault() == string.Empty ? false : Convert.ToBoolean(dbContext.Tower_UserClaims
                                             .Where(r => r.UserId == SelectUser.Id && r.ClaimType == Constants.CLAIM_MANAGEBEQ)
                                             .Select(Gr => Gr.ClaimValue).FirstOrDefault());

                User.ManageAccessREQ = dbContext.Tower_UserClaims
                                         .Where(r => r.UserId == SelectUser.Id && r.ClaimType == Constants.CLAIM_MANAGEACCESSREQ)
                                         .Select(Gr => Gr.ClaimValue).FirstOrDefault() == string.Empty ? false : Convert.ToBoolean(dbContext.Tower_UserClaims
                                         .Where(r => r.UserId == SelectUser.Id && r.ClaimType == Constants.CLAIM_MANAGEACCESSREQ)
                                         .Select(Gr => Gr.ClaimValue).FirstOrDefault());
            }
            

            return User;
        }

        public UserProfile UpdateUser(UserProfile UserProfile)
        {
            UserProfile UpdatedUsers = UserProfile;

            try
            {

                IEnumerable<Tower_UserClaims> CalimstoUpdate = dbContext.Tower_UserClaims
                         .RemoveRange(dbContext.Tower_UserClaims
                         .Where(user => (user.UserId == UserProfile.ID)));

                AuditLogHelper.SaveChanges(dbContext);



                Tower_Users UserstoUpdate = dbContext.Tower_Users
                .Where(user => user.Id == UserProfile.ID).FirstOrDefault();

                try
                {
                    if (UserstoUpdate != null)
                    {
                        UserstoUpdate.TenantId = UserProfile.TenantId;
                        UserstoUpdate.IsActive = UserProfile.IsActive;
                        string ExistingRole = UserstoUpdate.Tower_Roles.FirstOrDefault().Name;
                        if (ExistingRole != UserProfile.Role)
                        {
                            UserstoUpdate.Tower_Roles.Clear();
                            UserstoUpdate.Tower_Roles = dbContext.Tower_Roles.Where(na => na.Name == UserProfile.Role).ToList();
                            using (Entities dbContext1 = new Entities())
                            {
                                // Access IClaimsIdentity which contains claims
                                IIdentity claimsIdentity = (Thread.CurrentPrincipal as IPrincipal).Identity;

                                dbContext1.AuditLogs.Add(new AuditLog()
                                {
                                    UserName = claimsIdentity.Name,
                                    EventDateutc = DateTime.Now,
                                    EventType = AuditLogEventTypeEnum.Modified.ToString(),
                                    TableName = "Tower.UserRoles",
                                    RecordId = UserstoUpdate.Id,
                                    Property = "Role",
                                    OriginalValue = ExistingRole,
                                    NewValue = UserProfile.Role,
                                    Section = "Security"
                                });

                                dbContext1.SaveChanges();
                            }
                        }

                        dbContext.Entry(UserstoUpdate).State = System.Data.Entity.EntityState.Modified;
                        int Success = AuditLogHelper.SaveChanges(dbContext);
                    }
                }
                catch
                {


                }


                AddClaim(UserProfile);
                return GetuserDetail(UserProfile);
            }
            finally
            {

            }

        }

        private UserProfile GetuserDetail(UserProfile userProfile)
        {
            UserProfile User = new UserProfile();
         
                var SelectUser = (from users in dbContext.Tower_Users
                          .Where(user => user.UserName == userProfile.UserName)
                                  select users);

                var groupedCustomerList = SelectUser
                     .Select(Se => new
                     {
                         ID = Se.Id,
                         EmailID = Se.Email,
                         UserName = Se.UserName,
                         UserId = Se.UserId,
                         Name = Se.Name,
                         isActive = Se.IsActive,
                         sTenant = Se.Tenant.TenantName,
                         tenantId = Se.TenantId,
                         Role = Se.Tower_Roles.Select(role => role.Name).FirstOrDefault(),

                         ManageTEQ = dbContext.Tower_UserClaims
                                                        .Where(r => r.UserId == Se.Id && r.ClaimType == Constants.CLAIM_MANAGETEQ)
                                                        .Select(Gr => Gr.ClaimValue).FirstOrDefault(),
                         ManageBEQ = dbContext.Tower_UserClaims
                                                        .Where(r => r.UserId == Se.Id && r.ClaimType == Constants.CLAIM_MANAGEBEQ)
                                                        .Select(Gr => Gr.ClaimValue).FirstOrDefault(),
                         ManageAccessREQ = dbContext.Tower_UserClaims
                                                        .Where(r => r.UserId == Se.Id && r.ClaimType == Constants.CLAIM_MANAGEACCESSREQ)
                                                        .Select(Gr => Gr.ClaimValue).FirstOrDefault()
                     });

                return groupedCustomerList.AsEnumerable()
                                     .Select(x => new UserProfile
                                     {
                                         ID = x.ID,
                                         UserId = x.UserId,
                                         UserName = x.UserName,
                                         Name = x.Name,
                                         Emailid = x.EmailID,
                                         IsActive = x.isActive,
                                         sTenant = x.sTenant,
                                         TenantId = x.tenantId,
                                         Role = x.Role,
                                         ManageBEQ = x.ManageBEQ == string.Empty ? false:Convert.ToBoolean( x.ManageBEQ),
                                         ManageTEQ = x.ManageTEQ == string.Empty ? false : Convert.ToBoolean(x.ManageTEQ),
                                         ManageAccessREQ = x.ManageAccessREQ == string.Empty? false : Convert.ToBoolean(x.ManageAccessREQ)

                                     }).ToList().FirstOrDefault();
            
        }

        public List<string> GetUserRole()
        {
            List<string> userRole = new List<string>();

           
                var Matched = dbContext.Tower_Roles;

                if (Matched != null && Matched.Count() > 0)
                {
                    userRole = Matched.Select(sl => sl.Name).ToList();
                }
            
            return userRole;
        }

        public string GetTenantName(int tenantId)
        {
            return new Entities().Tenants.Where(se => se.TenantId == tenantId).Select(val => val.TenantName).FirstOrDefault();
        }

        public int GetTenantByName(string tenantName)
        {
            return new Entities().Tenants.Where(se => se.TenantName == tenantName).Select(val => val.TenantId).FirstOrDefault();
        }
    }
}
