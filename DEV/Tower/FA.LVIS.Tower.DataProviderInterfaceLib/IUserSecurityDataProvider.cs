using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FA.LVIS.Tower.Core;
using DC = FA.LVIS.Tower.DataContracts;

namespace FA.LVIS.Tower.Data
{

    public interface IUserSecurityDataProvider : IDataProviderBase
    {
        List<DC.UserProfile> GetApplicationUsers(int iTenant);
        List<DC.UserProfile> AddUser(DC.UserProfile UserProfile, int iTenant);

        int Deleteuser(DC.UserProfile UserProfile);

        DC.UserProfile UpdateUser(DC.UserProfile UserProfile);

        DC.UserProfile GetSingleUser(DC.UserProfile value);

        List<String> GetUserRole();

        String GetTenantName(int iTennant);

        int GetTenantByName(string tenantName);
    }
}
