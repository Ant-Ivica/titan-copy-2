using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FA.LVIS.Tower.DataContracts
{
    class ServiceInformation
    {
        public string Comments { get; set; }
        public string ContactEmail { get; set; }
        public string ContactName { get; set; }
        public string ContactNumber { get; set; }
        public string FASTFileNumber { get; set; }
        public string OrderDeskType { get; set; }
        public string OrderStatus { get; set; }
        public string PortalOrderAlert { get; set; }
        public string ProcessorAddress { get; set; }
        public string ProcessorName { get; set; }
        public ProductsOrdered ProductsOrdered { get; set; }
        public string ServiceName { get; set; }

    }
}
