using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FA.LVIS.Tower.DataContracts
{
    public class ForwardToOfficeRequestDTO
    {
        [JsonProperty("FASTWEB_HEADER")]
        public FastWebHeader FastwebHeader { get; set; }

        [JsonProperty("FORWARD_TO_REQUEST")]
        public FORWARD_TO_REQUEST ForwardToRequest { get; set; }
    }

    public class ForwardToOfficeRequest
    {
        [JsonProperty("FWOrderNumber")]
        public string FwOrderNumber { get; set; }

        [JsonProperty("UserId")]
        public string UserId { get; set; }

        [JsonProperty("NewprocessRepId")]
        public string NewprocessRepId { get; set; }

        [JsonProperty("ServiceTypes")]
        public ServiceTypes ServiceTypes { get; set; }
    }

    public class ServiceTypes
    {
        [JsonProperty("ServiceType")]
        public string[] ServiceType { get; set; }
    }
}
