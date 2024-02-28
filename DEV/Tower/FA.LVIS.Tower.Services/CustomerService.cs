using System.Collections.Generic;

using DC = FA.LVIS.Tower.DataContracts;
using FA.LVIS.Tower.Data;
using System;

namespace FA.LVIS.Tower.Services
{
    public class CustomerService : Core.ServiceBase, ICustomerService
    {
        public List<DC.CustomerDetails> GetLVISCustomers(int iTenantid)
        {
            ICustomerDataProvider custProvider = DataProviderFactory.Resolve<ICustomerDataProvider>();
            return custProvider.GetLVISCustomers(iTenantid);
        }


        //public List<DC.Regions> GetFastRegions()
        //{
        //    ICustomerDataProvider custProvider = DataProviderFactory.Resolve<ICustomerDataProvider>();
        //    return custProvider.GetFastRegions();
        //}

        public IEnumerable<DC.Regions> GetFastRegions(string application)
        {
            ICustomerDataProvider custProvider = DataProviderFactory.Resolve<ICustomerDataProvider>();
            return custProvider.GetFastRegions(application);
        }

        public IEnumerable<DC.Users> GetExternalApplications()
        {

            ICustomerDataProvider custProvider = DataProviderFactory.Resolve<ICustomerDataProvider>();
            return custProvider.GetExternalApplications();
        }

        public IEnumerable<DC.Users> GetInternalApplications()
        {
            ICustomerDataProvider custProvider = DataProviderFactory.Resolve<ICustomerDataProvider>();
            return custProvider.GetInternalApplications();
        }

        public IEnumerable<DC.FastOffices> GetfastOffices(int Region)
        {
            ICustomerDataProvider custProvider = DataProviderFactory.Resolve<ICustomerDataProvider>();
            return custProvider.GetfastOffices(Region);
        }

        public IEnumerable<DC.ContactMappings> GetContact(int locationid)
        {
            ICustomerDataProvider ContactList = DataProviderFactory.Resolve<ICustomerDataProvider>();
            return ContactList.GetContact(locationid);
        }

        public IEnumerable<DC.ContactProviderMappings> GetContactProviderDetails(int CustomerId)
        {
            ICustomerDataProvider ContactProviderList = DataProviderFactory.Resolve<ICustomerDataProvider>();
            return ContactProviderList.GetContactProviderDetails(CustomerId);
        }
    }
}
