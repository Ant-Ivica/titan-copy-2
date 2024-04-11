using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FA.LVIS.Tower.DataContracts
{
    public class ForwardToOfficeResponseDTO
    {
        [JsonProperty("FASTWEBackNack")]
        public FastweBackNack FastweBackNack { get; set; }
    }

    public class FastweBackNack
    {
        [JsonProperty("Datetime")]
        public DateTimeOffset Datetime { get; set; }

        [JsonProperty("StatusCd")]
        public long StatusCd { get; set; }

        [JsonProperty("StatusDescription")]
        public string StatusDescription { get; set; }
    }
}
