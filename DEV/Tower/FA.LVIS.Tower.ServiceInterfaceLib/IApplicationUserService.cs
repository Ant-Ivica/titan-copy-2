using System;
using System.Collections.Generic;

using DC = FA.LVIS.Tower.DataContracts;

namespace FA.LVIS.Tower.Services
{
    public interface IApplicationUserService : Core.IServiceBase
    {
        List<DC.UserProfile> GetApplicationUsers(int iTenant);

        List<DC.UserProfile> AddUser(DC.UserProfile UserProfile, int iTenant);

        int Deleteuser(DC.UserProfile UserProfile);

        DC.UserProfile UpdateUser(DC.UserProfile UserProfile);

        DC.UserProfile GetSingleUser(DC.UserProfile value);

        List<String> GetUserRole();

        String GetTenantName(int tenantId);

        int GetTenantByName(string tenantName);
    }
}
