using FA.LVIS.Tower.Core;
using FA.LVIS.Tower.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FA.LVIS.Tower.Data
{
    public interface ICustomerRegistrationDataProvider : IDataProviderBase
    {
         List<CustomerRegistrationDTO> GetCustomerRegistrations();
         IEnumerable<Status> GetStatus();
        CustomerRegistrationDTO AddCustomerRegistrationDTO(CustomerRegistrationDTO value, int tenantId, int userId);
        List<CustomerRegistrationDTO> GetCustomerRegistrations(string emailid);
    }
}
