using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FA.LVIS.Tower.DataContracts
{
   public class BulkImportDTO : DataContractBase
    {
        public string ExternalId { get; set; }

        public string Name { get; set; }

        public string CustomerId { get; set; }

        public string CustomerName { get; set; }

        public int LocationID { get; set; }

        public int RegionID { get; set; }

        public string LocationName { get; set; }

        public string BusinessSourceABEID { get; set; }

        public string NewLenderABEID { get; set; }

        public int LoanTypeID { get; set; }

        public string LoanType { get; set; }

        public string Description { get; set; }

        public string TransactionMessage { get; set; }

        public string Notes { get; set; }

    }
    
}
