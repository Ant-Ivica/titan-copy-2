using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FA.LVIS.Tower.DataContracts
{
    public partial class ForwardToListResponseDTO
    {
        [JsonProperty("FASTWEBAckNack")]
        public FASTWEBackNack FastwebAckNack { get; set; }

        [JsonProperty("FASTWebHeader")]
        public FastWebHeader FastWebHeader { get; set; }

        [JsonProperty("FASTWebOrderResponse")]
        public object FastWebOrderResponse { get; set; }

        [JsonProperty("FORWARDTOLISTRESPONSE")]
        public Forwardtolistresponse Forwardtolistresponse { get; set; }
    }

    public partial class FastWebHeader
    {
        [JsonProperty("FWActionType")]
        public string FWActionType { get; set; }
    }

    public partial class FASTWEBackNack
    {
        [JsonProperty("Datetime")]
        public DateTimeOffset Datetime { get; set; }

        [JsonProperty("StatusCd")]
        public long StatusCd { get; set; }

        [JsonProperty("StatusDescription")]
        public string StatusDescription { get; set; }
    }

    public partial class Forwardtolistresponse
    {
        [JsonProperty("Offices")]
        public Offices Offices { get; set; }
    }

    public partial class Offices
    {
        [JsonProperty("Office")]
        public Office[] Office { get; set; }
    }

    public partial class Office
    {
        [JsonProperty("Firstname")]
        public string Firstname { get; set; }

        [JsonProperty("Lastname")]
        public string Lastname { get; set; }

        [JsonProperty("Userid")]
        public long Userid { get; set; }

        [JsonProperty("Username")]
        public string Username { get; set; }

        public string Fullname { get; set; }
    }
}
