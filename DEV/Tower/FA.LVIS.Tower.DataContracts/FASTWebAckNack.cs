using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FA.LVIS.Tower.DataContracts
{
    class FASTWebAckNack
    {
        public DateTime DateTime { get; set; }
        public string StatusCd { get; set; }
        public string StatusDescription { get; set; }

    }
}

