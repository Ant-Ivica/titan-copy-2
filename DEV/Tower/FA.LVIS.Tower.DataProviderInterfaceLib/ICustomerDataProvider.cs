using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FA.LVIS.Tower.Core;
using DC = FA.LVIS.Tower.DataContracts;

namespace FA.LVIS.Tower.Data
{
    public interface ICustomerDataProvider : IDataProviderBase
    {
        List<DC.Customer> GetLVISCustomers();
        List<DC.CustomerDetails> GetLVISCustomers(int itenantid);
        List<DC.Regions> GetFastRegions();
        IEnumerable<DC.Regions> GetFastRegions(string application);
        IEnumerable<DC.FastOffices> GetfastOffices(int Region);
        IEnumerable<DC.Users> GetExternalApplications();
        IEnumerable<DC.Users> GetInternalApplications();
        IEnumerable<DC.ContactMappings> GetContact(int locationid);
        IEnumerable<DC.ContactProviderMappings> GetContactProviderDetails(int CustomerId);
        List<DC.LocationsMappings> GetLocationList(int CustomerId);
    }
}
