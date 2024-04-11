using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FA.LVIS.Tower.DataContracts
{
   public  class TypeCodeDTO : DataContractBase
    {
        public int TypeCodeId { get; set; }
         public string TypeCodeDesc { get; set; }
        public bool Ticked { get; set; }
    }
}
