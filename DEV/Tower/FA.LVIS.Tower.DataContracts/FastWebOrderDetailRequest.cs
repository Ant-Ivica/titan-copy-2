using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FA.LVIS.Tower.DataContracts
{
    public class FastWebOrderDetailRequest
    {
        [JsonProperty("FASTWEB_HEADER")]
        public FastWebHeader FastWebHeader { get; set; }

        [JsonProperty("FASTWEB_ORDER_REQUEST")]
        public FASTWEB_ORDER_REQUEST FASTWEB_ORDER_REQUEST { get; set; }
    }
    public class FASTWEB_ORDER_REQUEST
    {
        [JsonProperty("FASTNOTIFY_REQUEST")]
        public FASTNOTIFY_REQUEST FASTNOTIFY_REQUEST { get; set; }

        [JsonProperty("ORDER_DETAIL_REQUEST")]
        public ORDER_DETAIL_REQUEST ORDER_DETAIL_REQUEST { get; set; }
    }

    public class FASTNOTIFY_REQUEST
    {
        [JsonProperty("iProcessingRepID")]
        public string iProcessingRepID { get; set; }
        [JsonProperty("SearchText")]
        public string SearchText { get; set; }
        [JsonProperty("SearchType")]
        public string SearchType { get; set; }
        [JsonProperty("ServiceTypes")]
        public ServiceTypes ServiceTypes { get; set; }
    }

    public class ORDER_DETAIL_REQUEST
    {
        [JsonProperty("FWOrderNumber")]
        public int FWOrderNumber { get; set; }
    }
}
