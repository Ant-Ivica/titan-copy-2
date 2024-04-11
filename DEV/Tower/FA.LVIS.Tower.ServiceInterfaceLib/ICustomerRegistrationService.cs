using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FA.LVIS.Tower.DataContracts;

namespace FA.LVIS.Tower.Services
{
    public  interface ICustomerRegistrationService : Core.IServiceBase
    {
        List<CustomerRegistrationDTO> GetCustomerRegistrations();

        IEnumerable<Status> GetStatus();
        CustomerRegistrationDTO AddCustomerRegistrationDTO(CustomerRegistrationDTO value, int tenantId, int userId);
        IEnumerable<CustomerRegistrationDTO> GetCustomerRegistrations(string emailid);
    }
}
