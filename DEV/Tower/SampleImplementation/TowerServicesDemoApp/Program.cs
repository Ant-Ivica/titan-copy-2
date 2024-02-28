using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FA.LVIS.Tower.Services;
using DC = FA.LVIS.Tower.DataContracts;
using FA.LVIS.Tower.Common;
using C = FA.LVIS.Tower.Core;

namespace TowerServicesDemoApp
{
    class Program
    {
        static void Main(string[] args)
        {
            ICustomerService custService = ServiceFactory.Resolve<ICustomerService>();

            List<DC.Customer> custList = custService.GetLVISCustomers();

        }
    }
}
