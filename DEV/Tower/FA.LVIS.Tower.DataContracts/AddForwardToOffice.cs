using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FA.LVIS.Tower.DataContracts
{
    public class AddForwardToOffice
    {
        [JsonProperty("FASTWEB_HEADER")]
        public FastWebHeader FastWebHeader { get; set; }

        [JsonProperty("FORWARD_TO_REQUEST")]
        public FORWARD_TO_REQUEST ForwardTorequest { get; set; }
    }
    public partial class ADD_FORWARD_TO_OFFICE_REQEUST
    {
        [JsonProperty("UserId")]
        public int UserId { get; set; }

        [JsonProperty("FirstName")]
        public string FirstName { get; set; }

        [JsonProperty("LastName")]
        public string LastName { get; set; }
        [JsonProperty("BUID")]
        public int BUID { get; set; }
    }
}
