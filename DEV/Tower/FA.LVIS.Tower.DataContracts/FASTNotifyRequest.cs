using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FA.LVIS.Tower.DataContracts
{
    public class FASTNotifyRequest
    {
        [JsonProperty("FASTWEB_HEADER")]
        public FASTWEBHEADER FASTWEB_HEADER { get; set; }
        [JsonProperty("FASTWEB_ORDER_REQUEST")]
        public FASTWEB_ORDER_REQUEST FASTWEB_ORDER_REQUEST { get; set; }
    }
}
