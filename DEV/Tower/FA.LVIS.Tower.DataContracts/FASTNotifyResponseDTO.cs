using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FA.LVIS.Tower.DataContracts
{

    [XmlRoot(ElementName = "FASTWEB_ACK_NACK")]
    public class FASTWEB_ACK_NACK
    {
        [XmlElement(ElementName = "DateTime")]
        public string DateTime { get; set; }
        [XmlElement(ElementName = "StatusCd")]
        public string StatusCd { get; set; }
        [XmlElement(ElementName = "StatusDescription")]
        public string StatusDescription { get; set; }
    }

    [XmlRoot(ElementName = "FASTWEB_HEADER")]
    public class FASTWEB_HEADER
    {
        [XmlElement(ElementName = "FWActionType")]
        public string FWActionType { get; set; }
    }

    [XmlRoot(ElementName = "FASTWEB_ORDER")]
    public class FASTWEB_ORDER
    {
        [XmlElement(ElementName = "BorrowerName")]
        public string BorrowerName { get; set; }
        [XmlElement(ElementName = "CustomerReferenceNumber")]
        public string CustomerReferenceNumber { get; set; }
        [XmlElement(ElementName = "FWOrderNumber")]
        public string FWOrderNumber { get; set; }
        [XmlElement(ElementName = "OrderDate")]
        public string OrderDate { get; set; }
        [XmlElement(ElementName = "PortalOrderAlert")]
        public string PortalOrderAlert { get; set; }
        [XmlElement(ElementName = "PropertyAddress")]
        public string PropertyAddress { get; set; }
        [XmlElement(ElementName = "PropertyCity")]
        public string PropertyCity { get; set; }
        [XmlElement(ElementName = "PropertyState")]
        public string PropertyState { get; set; }
        [XmlElement(ElementName = "PropertyZip")]
        public string PropertyZip { get; set; }
        [XmlElement(ElementName = "ServiceName")]
        public string ServiceName { get; set; }
    }

    [XmlRoot(ElementName = "FASTWEB_ORDERS")]
    public class FASTWEB_ORDERS
    {
        [XmlElement(ElementName = "FASTWEB_ORDER")]
        public List<FASTWEB_ORDER> FASTWEB_ORDER { get; set; }
    }

    [XmlRoot(ElementName = "FASTNOTIFY_RESONSE")]
    public class FASTNOTIFY_RESONSE
    {
        [XmlElement(ElementName = "FASTWEB_ORDERS")]
        public FASTWEB_ORDERS FASTWEB_ORDERS { get; set; }
    }

    [XmlRoot(ElementName = "FASTWEB_ORDER_RESPONSE")]
    public class FASTWEB_ORDER_RESPONSE
    {
        [XmlElement(ElementName = "FASTNOTIFY_RESONSE")]
        public FASTNOTIFY_RESONSE FASTNOTIFY_RESONSE { get; set; }
        [XmlElement(ElementName = "ORDER_DETAIL_RESPONSE")]
        public ORDER_DETAIL_RESPONSE ORDER_DETAIL_RESPONSE { get; set; }
    }

    [XmlRoot(ElementName = "MESSAGE")]
    public class FASTNotifyResponseDTO
    {
        [XmlElement(ElementName = "FASTWEB_ACK_NACK")]
        public FASTWEB_ACK_NACK FASTWEB_ACK_NACK { get; set; }
        [XmlElement(ElementName = "FASTWEB_HEADER")]
        public FASTWEB_HEADER FASTWEB_HEADER { get; set; }
        [XmlElement(ElementName = "FASTWEB_ORDER_RESPONSE")]
        public FASTWEB_ORDER_RESPONSE FASTWEB_ORDER_RESPONSE { get; set; }
    }
}
