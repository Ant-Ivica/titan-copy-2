using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FA.LVIS.Tower.DataContracts
{
    public class AddForwardToOfficeRequst
    {
        [JsonProperty("UserId")]
        public long UserId { get; set; }

        [JsonProperty("FirstName")]
        public string FirstName { get; set; }

        [JsonProperty("LastName")]
        public string LastName { get; set; }

        [JsonProperty("BUID")]
        public string BUID { get; set; }
    }

    public class ForwardToListRequest
    {
        [JsonProperty("UserId")]
        public long UserId { get; set; }
    }

    public class ForwardToRequest
    {
        [JsonProperty("FORWARD_TO_LIST_REQUEST")]
        public ForwardToListRequest FORWARD_TO_LIST_REQUEST { get; set; }

        [JsonProperty("FORWARD_TO_OFFICE_REQUEST")]
        public ForwardToOfficeRequest FORWARD_TO_OFFICE_REQUEST { get; set; }

        [JsonProperty("ADD_FORWARD_TO_OFFICE_REQEUST")]
        public AddForwardToOfficeRequst ADD_FORWARD_TO_OFFICE_REQEUST { get; set; }
    }

    public class MESSAGE
    {
        [JsonProperty("FASTWEB_HEADER")]
        public FASTWEB_HEADER FASTWEB_HEADER { get; set; }

        [JsonProperty("FORWARD_TO_REQUEST")]
        public ForwardToRequest FORWARD_TO_REQUEST { get; set; }

    }

    public class AddForwardToOfficeRequest
    {
        [JsonProperty("MESSAGE")]
        public MESSAGE MESSAGE { get; set; }
    }
}
