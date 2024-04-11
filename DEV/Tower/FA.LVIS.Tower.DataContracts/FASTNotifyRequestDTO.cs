using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FA.LVIS.Tower.DataContracts
{
    [XmlRoot(ElementName = "FASTNOTIFY_REQUEST")]
    public class FASTNOTIFY_REQUEST
    {
        [XmlElement(ElementName = "iProcessingrepID")]
        public string IProcessingrepID { get; set; }
        [XmlElement(ElementName = "SearchText")]
        public string SearchText { get; set; }
        [XmlElement(ElementName = "SearchType")]
        public string SearchType { get; set; }
        [XmlElement(ElementName = "ServiceType")]
        public string ServiceType { get; set; }
    }

    [XmlRoot(ElementName = "FASTWEB_ORDER_REQUEST")]
    public class FASTWEB_ORDER_REQUEST
    {
        [XmlElement(ElementName = "FASTNOTIFY_REQUEST")]
        public FASTNOTIFY_REQUEST FASTNOTIFY_REQUEST { get; set; }
        [XmlElement(ElementName = "ORDER_DETAIL_REQUEST")]
        public ORDER_DETAIL_REQUEST ORDER_DETAIL_REQUEST { get; set; }

    }

    [XmlRoot(ElementName = "MESSAGE")]
    public class FASTNotifyRequestDTO
    {
        [XmlElement(ElementName = "FASTWEB_ACK_NACK")]
        public FASTWEB_ACK_NACK FASTWEB_ACK_NACK { get; set; }
        [XmlElement(ElementName = "FASTWEB_HEADER")]
        public FASTWEB_HEADER FASTWEB_HEADER { get; set; }
        [XmlElement(ElementName = "FASTWEB_ORDER_REQUEST")]
        public FASTWEB_ORDER_REQUEST FASTWEB_ORDER_REQUEST { get; set; }
    }
}
