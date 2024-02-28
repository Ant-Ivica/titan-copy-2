using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FA.LVIS.Tower.DataContracts;
using FA.LVIS.Tower.Data;

namespace FA.LVIS.Tower.Services
{
    class CustomerRegistrationService : Core.ServiceBase, ICustomerRegistrationService
    {
        public CustomerRegistrationDTO AddCustomerRegistrationDTO(CustomerRegistrationDTO value, int tenantId, int userId)
        {
            return DataProviderFactory.Resolve<ICustomerRegistrationDataProvider>().AddCustomerRegistrationDTO(value, tenantId, userId);
        }

        public List<CustomerRegistrationDTO> GetCustomerRegistrations()
        {
            return DataProviderFactory.Resolve<ICustomerRegistrationDataProvider>().GetCustomerRegistrations();
        }
        public IEnumerable<Status> GetStatus()
        {
            return DataProviderFactory.Resolve<ICustomerRegistrationDataProvider>().GetStatus();

        }

        IEnumerable<CustomerRegistrationDTO> ICustomerRegistrationService.GetCustomerRegistrations(string emailid)
        {
            return DataProviderFactory.Resolve<ICustomerRegistrationDataProvider>().GetCustomerRegistrations(emailid);
        }
    }
}
