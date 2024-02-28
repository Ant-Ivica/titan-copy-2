using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FA.LVIS.Tower.DataContracts
{
    public class FastWebOrderDetailsCanonicalDTO : DataContractBase
    {
        public string FASTWebOrderNumber { get; set; }
        public string CustomerRefNumber { get; set; }
        public string BorrowerName { get; set; }
        public string PropertyAddressLine1 { get; set; }
        public string PropertyAddressLine2 { get; set; }
        public string ServiceName { get; set; }
        public string PortalOrderAlert { get; set; }
        public DateTime? OrderDate { get; set; }
        public List<FastWebOrderDetailsCanonicalDTO> children { get; set; }
    }

    public class FastWebPropertyAddress
    {
        public string PropertyAddress { get; set; }
        public string PropertyCity { get; set; }
        public string PropertyState { get; set; }
        public string PropertyZip { get; set; }
    }
}
