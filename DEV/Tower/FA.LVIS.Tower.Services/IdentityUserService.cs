using FA.LVIS.Tower.Data;
using FA.LVIS.Tower.DataContracts;
using System.Collections.Generic;

namespace FA.LVIS.Tower.Services
{
    public class IdentityUserService<TUser> : Core.ServiceBase, IIdentityUserService<TUser> where TUser : IdentityUser
    {
        //private IdentityServiceClient identityWebApi;        

        public bool InsertApplicationUser(TUser user)
        {
            ServicesContainer.Instance.RegisterType(typeof(IIdentityUserDataProvider<TUser>), typeof(Data.IdentityUserDataProvider));
            IIdentityUserDataProvider<TUser> IdentityProvider = DataProviderFactory.Resolve<IIdentityUserDataProvider<TUser>>();
            return IdentityProvider.InsertUser(user);
            //return identityWebApi.Post<bool, TUser>("InserApplicationUser", user);
        }
        
        public List<TUser> GetActiveDirectoryUser(TUser user)
        {
            ServicesContainer.Instance.RegisterType(typeof(IIdentityUserDataProvider<TUser>), typeof(Data.IdentityUserDataProvider));
            IIdentityUserDataProvider<TUser> IdentityProvider = DataProviderFactory.Resolve<IIdentityUserDataProvider<TUser>>();
            return IdentityProvider.GetActiveDirectoryUser(user);
            //return identityWebApi.Post<List<TUser>, TUser>("GetActiveDirectoryUser", user);
        }
        
        public List<TUser> GetApplicationUserByName(string userName)
        {
            DataProviderContainer.Instance.RegisterType(typeof(IIdentityUserDataProvider<TUser>), typeof(Data.IdentityUserDataProvider));
            IIdentityUserDataProvider<TUser> IdentityProvider = DataProviderFactory.Resolve<IIdentityUserDataProvider<TUser>>();
            return IdentityProvider.GetUserByName(userName);
            //return identityWebApi.Post<List<TUser>, string>("GeApplicationUserByName", userName);
        }
        
        public TUser GetApplicationUserById(string id)
        {
            ServicesContainer.Instance.RegisterType(typeof(IIdentityUserDataProvider<TUser>), typeof(Data.IdentityUserDataProvider));
            IIdentityUserDataProvider<TUser> IdentityProvider = DataProviderFactory.Resolve<IIdentityUserDataProvider<TUser>>();
            return IdentityProvider.GetUserById(id);
            //return identityWebApi.Post<TUser, string>("GeApplicationUserById", userId);
        }

        //public TUser GetApplicationUserByUserId(string userId)
        //{
        //    ServicesContainer.Instance.RegisterType(typeof(IIdentityUserDataProvider<TUser>), typeof(Data.IdentityUserDataProvider));
        //    IIdentityUserDataProvider<TUser> IdentityProvider = DataProviderFactory.Resolve<IIdentityUserDataProvider<TUser>>();
        //    return IdentityProvider.GetUserById(userId);
        //    //return identityWebApi.Post<TUser, string>("GeApplicationUserById", userId);
        //}

        public IEnumerable<string> GetClaimsByUserId(string userId)
        {
            ServicesContainer.Instance.RegisterType(typeof(IIdentityUserDataProvider<TUser>), typeof(Data.IdentityUserDataProvider));
            IIdentityUserDataProvider<TUser> IdentityProvider = DataProviderFactory.Resolve<IIdentityUserDataProvider<TUser>>();
            return IdentityProvider.GetClaimsByUserId(userId);
            //return identityWebApi.Post<IEnumerable<string>, string>("GetClaimsByUserId", userId);
        }

        public IEnumerable<string> GetClaimsByUser(TUser user)
        {
            ServicesContainer.Instance.RegisterType(typeof(IIdentityUserDataProvider<TUser>), typeof(Data.IdentityUserDataProvider));
            IIdentityUserDataProvider<TUser> IdentityProvider = DataProviderFactory.Resolve<IIdentityUserDataProvider<TUser>>();
            return IdentityProvider.GetClaimsByUser(user);
            //return identityWebApi.Post<IEnumerable<string>, TUser>("GetClaimsByUser", user);
        }

        public TUser FindByEmail(string emailId)
        {
            ServicesContainer.Instance.RegisterType(typeof(IIdentityUserDataProvider<TUser>), typeof(Data.IdentityUserDataProvider));
            IIdentityUserDataProvider<TUser> IdentityProvider = DataProviderFactory.Resolve<IIdentityUserDataProvider<TUser>>();
            return IdentityProvider.FindByEmail(emailId);
            //return identityWebApi.Post<TUser, string>("FindByEmail", emailId);
        }

        public bool UpdateUser(TUser user)
        {
            ServicesContainer.Instance.RegisterType(typeof(IIdentityUserDataProvider<TUser>), typeof(Data.IdentityUserDataProvider));
            IIdentityUserDataProvider<TUser> IdentityProvider = DataProviderFactory.Resolve<IIdentityUserDataProvider<TUser>>();
            return IdentityProvider.UpdateUser(user);
            //return identityWebApi.Post<bool, TUser>("UpdateUser", user);
        }

        public bool AddClaims(UserClaims userClaims)
        {
            ServicesContainer.Instance.RegisterType(typeof(IIdentityUserDataProvider<TUser>), typeof(Data.IdentityUserDataProvider));
            IIdentityUserDataProvider<TUser> IdentityProvider = DataProviderFactory.Resolve<IIdentityUserDataProvider<TUser>>();
            return IdentityProvider.AddClaims(userClaims);
            //return identityWebApi.Post<bool, UserClaims>("AddClaims", userClaims);
        }

        public bool RemoveClaims(UserClaims userClaims)
        {
            ServicesContainer.Instance.RegisterType(typeof(IIdentityUserDataProvider<TUser>), typeof(Data.IdentityUserDataProvider));
            IIdentityUserDataProvider<TUser> IdentityProvider = DataProviderFactory.Resolve<IIdentityUserDataProvider<TUser>>();
            return IdentityProvider.RemoveClaims(userClaims);
            //return identityWebApi.Post<bool, UserClaims>("RemoveClaims", userClaims);
        }

        public bool RemoveUser(TUser user)
        {
            ServicesContainer.Instance.RegisterType(typeof(IIdentityUserDataProvider<TUser>), typeof(Data.IdentityUserDataProvider));
            IIdentityUserDataProvider<TUser> IdentityProvider = DataProviderFactory.Resolve<IIdentityUserDataProvider<TUser>>();
            return IdentityProvider.RemoveUser(user);
            //return identityWebApi.Post<bool, TUser>("RemoveUser", user);
        }

        public bool AddUserToRole(UserRole userRole)
        {
            ServicesContainer.Instance.RegisterType(typeof(IIdentityUserDataProvider<TUser>), typeof(Data.IdentityUserDataProvider));
            IIdentityUserDataProvider<TUser> IdentityProvider = DataProviderFactory.Resolve<IIdentityUserDataProvider<TUser>>();
            return IdentityProvider.AddUserToRole(userRole);
            //return identityWebApi.Post<bool, UserRole>("AddUserToRole", userRole);
        }

        public bool RemoveUserFromRole(UserRole userRole)
        {
            ServicesContainer.Instance.RegisterType(typeof(IIdentityUserDataProvider<TUser>), typeof(Data.IdentityUserDataProvider));
            IIdentityUserDataProvider<TUser> IdentityProvider = DataProviderFactory.Resolve<IIdentityUserDataProvider<TUser>>();
            return IdentityProvider.RemoveUserFromRole(userRole);
            //return identityWebApi.Post<bool, UserRole>("RemoveUserFromRole", userRole);
        }

        public IEnumerable<string> FindRolesByUserId(string userId)
        {
            ServicesContainer.Instance.RegisterType(typeof(IIdentityUserDataProvider<TUser>), typeof(Data.IdentityUserDataProvider));
            IIdentityUserDataProvider<TUser> IdentityProvider = DataProviderFactory.Resolve<IIdentityUserDataProvider<TUser>>();
            return IdentityProvider.FindRolesByUserId(userId);
            //return identityWebApi.Post<IEnumerable<string>, string>("FindRolesByUserId", userId);
        }

        //public IEnumerable<string> GetRoleByUserId(string userId)
        //{
        //    IIdentityUserDataProvider<TUser> IdentityProvider = DataProviderFactory.Resolve<IIdentityUserDataProvider<TUser>>();
        //    return IdentityProvider.FindRolesByUserId(userId);
        //    //return identityWebApi.Post<IEnumerable<string>, string>("FindRolesByUserId", userId);
        //}

        //string IIdentityUserService<TUser>.GetRoleByUserId(string userId)
        //{
        //    throw new NotImplementedException();
        //}

        //public void Dispose()
        //{
        //    if (identityWebApi != null)
        //    {
        //        identityWebApi.Dispose();
        //        identityWebApi = null;
        //    }
        //}
    }
}
