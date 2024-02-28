using System;
using System.Collections.Generic;

using DC = FA.LVIS.Tower.DataContracts;
using FA.LVIS.Tower.Data;

namespace FA.LVIS.Tower.Services
{
    public class ApplicationUserService : Core.ServiceBase, IApplicationUserService
    {
        public List<DC.UserProfile> GetApplicationUsers(int tenantId)
        {
            return DataProviderFactory.Resolve<IUserSecurityDataProvider>().GetApplicationUsers(tenantId);
        }        

        public List<DC.UserProfile> AddUser(DC.UserProfile UserProfile,int tenantId)
        {
            return DataProviderFactory.Resolve<IUserSecurityDataProvider>().AddUser(UserProfile, tenantId);
        }

        public DC.UserProfile UpdateUser(DC.UserProfile UserProfile)
        {
            return DataProviderFactory.Resolve<IUserSecurityDataProvider>().UpdateUser(UserProfile);
        }

        public int Deleteuser(DC.UserProfile UserProfile)
        {
            return DataProviderFactory.Resolve<IUserSecurityDataProvider>().Deleteuser(UserProfile);
        }

        public DC.UserProfile GetSingleUser(DC.UserProfile value)
        {
            return DataProviderFactory.Resolve<IUserSecurityDataProvider>().GetSingleUser(value);
        }

        public List<String> GetUserRole()
        {
            return DataProviderFactory.Resolve<IUserSecurityDataProvider>().GetUserRole();
        }

        public string GetTenantName(int tenantId)
        {
            return DataProviderFactory.Resolve<IUserSecurityDataProvider>().GetTenantName(tenantId);
        }

        public int GetTenantByName(string tenantName)
        {
            return DataProviderFactory.Resolve<IUserSecurityDataProvider>().GetTenantByName(tenantName);
        }
    }
}
