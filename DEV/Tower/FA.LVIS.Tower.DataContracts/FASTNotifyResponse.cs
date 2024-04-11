using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FA.LVIS.Tower.DataContracts
{
    public class FASTNotifyResponse
    {
        [JsonProperty("FASTWEBackNack")]
        public FASTWEBackNack FASTWEBackNack { get; set; }
        [JsonProperty("FASTWEBHEADER")]
        public FASTWEBHEADER FASTWEBHEADER { get; set; }
        [JsonProperty("FASTWEBORDERRESPONSE")]
        public FASTWEBORDERRESPONSE FASTWEBORDERRESPONSE { get; set; }
    }
    public class FASTNOTIFYRESPONSE
    {
        [JsonProperty("FASTWEBORDERS")]
        public FASTWEBORDERS FASTWEBORDERS { get; set; }

    }
    public class FASTWEBORDERS
    {
        [JsonProperty("FASTWEBOrder")]
        public List<FASTWEBOrder> FASTWEBOrder { get; set; }

    }

    public class FASTWEBOrder
    {
        [JsonProperty("Borrowername")]
        public string Borrowername { get; set; }
        [JsonProperty("CustomerReferenceNumber")]
        public string CustomerReferenceNumber { get; set; }
        [JsonProperty("FWOrderNumber")]
        public int FWOrderNumber { get; set; }
        [JsonProperty("OrderDate")]
        public DateTime? OrderDate { get; set; }
        [JsonProperty("PortalOrderAlert")]
        public string PortalOrderAlert { get; set; }
        [JsonProperty("PropertyAddress")]
        public string PropertyAddress { get; set; }
        [JsonProperty("PropertyCity")]
        public string PropertyCity { get; set; }
        [JsonProperty("PropertyState")]
        public string PropertyState { get; set; }
        [JsonProperty("PropertyZip")]
        public string PropertyZip { get; set; }
        [JsonProperty("ServiceName")]
        public string ServiceName { get; set; }
    }
}
