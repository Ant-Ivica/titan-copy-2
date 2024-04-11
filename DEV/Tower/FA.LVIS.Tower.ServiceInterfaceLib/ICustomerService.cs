using System.Collections.Generic;

using DC = FA.LVIS.Tower.DataContracts;

namespace FA.LVIS.Tower.Services
{
    public interface ICustomerService : Core.IServiceBase
    {
        List<DC.CustomerDetails> GetLVISCustomers(int iTenantid);
        //List<DC.Regions> GetFastRegions();
        IEnumerable<DC.FastOffices> GetfastOffices(int Region);
        IEnumerable<DC.Regions> GetFastRegions(string application);
        IEnumerable<DC.Users> GetExternalApplications();
        IEnumerable<DC.Users> GetInternalApplications();
        IEnumerable<DC.ContactMappings> GetContact(int locationid);

        IEnumerable<DC.ContactProviderMappings> GetContactProviderDetails(int CustomerId);
    }
}
