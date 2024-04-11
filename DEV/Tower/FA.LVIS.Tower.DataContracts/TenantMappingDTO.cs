using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FA.LVIS.Tower.DataContracts
{
   public  class TenantMappingDTO:DataContractBase
    {
        public int TenantId { get; set; }
        public string TenantName { get; set;}
    }
}
