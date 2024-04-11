using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FA.LVIS.Tower.DataContracts
{
    
    [XmlRoot(ElementName = "ORDER_DETAIL_REQUEST")]
    public class ORDER_DETAIL_REQUEST
    {
        [XmlElement(ElementName = "FWOrderNumber")]
        public string FWOrderNumber { get; set; }

        [XmlElement(ElementName = "SearchType")]
        public int SearchType { get; set; }
    }

    [XmlRoot(ElementName = "MESSAGE")]
    public class OrderDetailsRequest
    {
        [XmlElement(ElementName = "FASTWEB_ORDER_REQUEST")]
        public FASTWEB_ORDER_REQUEST FASTWEB_ORDER_REQUEST { get; set; }

    }
}
