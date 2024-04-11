using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FA.LVIS.Tower.DataContracts
{
    public class OutEventMapping : DataContractBase
    {

            public string EID { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public string ExternalEvent { get; set; }
            public string Services { get; set; }
        public string lenderABEID { get; set; }



    }
}
