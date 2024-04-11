using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FA.LVIS.Tower.DataContracts
{
    public class FASTGABMap : DataContractBase
    {
        public int FASTGABMapId { get; set; }

        public int LocationID { get; set; }

        public int RegionID { get; set; }

        public int? LoanTypeCodeId { get; set; }

        public string LoanType { get; set; }

        public string Region { get; set; }

        public string LocationName { get; set; }

        public string BusinessSourceABEID { get; set; }

        public string NewLenderABEID { get; set; }

        public string FASTGABMapDesc { get; set; }

        public List<ConditionPreferenceDTO> Conditions { get; set; }
    }
}
