using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FA.LVIS.Tower.DataContracts
{
    public class FASTWEBHEADER
    {
        [JsonProperty("FWActionType")]
        public string FWActionType { get; set; }
    }
    
    public class FORWARD_TO_LIST_REQUEST
    {
        [JsonProperty("UserId")]
        public string UserId { get; set; }
    }

    public class FORWARD_TO_REQUEST
    {
        [JsonProperty("FORWARD_TO_LIST_REQUEST")]
        public FORWARD_TO_LIST_REQUEST FORWARD_TO_LIST_REQUEST { get; set; }
        [JsonProperty("FORWARD_TO_OFFICE_REQUEST")]
        public ForwardToOfficeRequest FORWARD_TO_OFFICE_REQUEST { get; set; }
        [JsonProperty("ADD_FORWARD_TO_OFFICE_REQEUST")]
        public ADD_FORWARD_TO_OFFICE_REQEUST AddForwardToOfficeRequest { get; set; }
    }

    public class ForwardToListRequestDTO
    {
        [JsonProperty("FASTWEB_HEADER")]
        public FASTWEBHEADER FASTWEB_HEADER { get; set; }
        [JsonProperty("FORWARD_TO_REQUEST")]
        public FORWARD_TO_REQUEST FORWARD_TO_REQUEST { get; set; }
    }
}
