using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FA.LVIS.Tower.DataContracts
{
    [XmlRoot(ElementName = "ORDER_DETAILS")]
    public class ORDER_DETAILS
    {
        [XmlElement(ElementName = "CustomerContact")]
        public string CustomerContact { get; set; }
        [XmlElement(ElementName = "CustomerOffice")]
        public string CustomerOffice { get; set; }
        [XmlElement(ElementName = "CustomerRefNumber")]
        public string CustomerRefNumber { get; set; }
        [XmlElement(ElementName = "DateOpened")]
        public string DateOpened { get; set; }
        [XmlElement(ElementName = "Email")]
        public string Email { get; set; }
        [XmlElement(ElementName = "FWOrderNumber")]
        public string FWOrderNumber { get; set; }
        [XmlElement(ElementName = "LoanAmount")]
        public int LoanAmount { get; set; }
        [XmlElement(ElementName = "OfficeAddress")]
        public string OfficeAddress { get; set; }
        [XmlElement(ElementName = "Phone")]
        public int Phone { get; set; }
        [XmlElement(ElementName = "PropertyType")]
        public string PropertyType { get; set; }
        [XmlElement(ElementName = "PropertyUse")]
        public string PropertyUse { get; set; }
        [XmlElement(ElementName = "SalePrice")]
        public int SalePrice { get; set; }
        [XmlElement(ElementName = "TransactionType")]
        public string TransactionType { get; set; }

    }

    [XmlRoot(ElementName = "PROPERTY_DATA")]
    public class PROPERTY_DATA
    {
        [XmlElement(ElementName = "APN")]
        public string APN { get; set; }
        [XmlElement(ElementName = "County")]
        public string County { get; set; }
        [XmlElement(ElementName = "LegalDescription")]
        public string LegalDescription { get; set; }
        [XmlElement(ElementName = "PropertyAddress")]
        public string PropertyAddress { get; set; }
    }

    [XmlRoot(ElementName = "PARTY")]
    public class PARTY
    {
        [XmlElement(ElementName = "Address")]
        public string Address { get; set; }
        [XmlElement(ElementName = "EntityType")]
        public string EntityType { get; set; }
        [XmlElement(ElementName = "FirstName")]
        public string FirstName { get; set; }
        [XmlElement(ElementName = "LastName")]
        public string LastName { get; set; }
        [XmlElement(ElementName = "MaritalStatus")]
        public string MaritalStatus { get; set; }
        [XmlElement(ElementName = "SpouseFirstName")]
        public string SpouseFirstName { get; set; }
        [XmlElement(ElementName = "SpouseLastName")]
        public string SpouseLastName { get; set; }
    }

    [XmlRoot(ElementName = "PARTY_DATA")]
    public class PARTY_DATA
    {
        [XmlElement(ElementName = "PARTY")]
        public PARTY PARTY { get; set; }
    }

    [XmlRoot(ElementName = "SERVICE_INFORMATION")]
    public class SERVICE_INFORMATION
    {
        [XmlElement(ElementName = "Comments")]
        public string Comments { get; set; }
        [XmlElement(ElementName = "ContactEmail")]
        public string ContactEmail { get; set; }
        [XmlElement(ElementName = "ContactName")]
        public string ContactName { get; set; }
        [XmlElement(ElementName = "ContactNumber")]
        public string ContactNumber { get; set; }
        [XmlElement(ElementName = "FASTFileNumber")]
        public string FASTFileNumber { get; set; }
        [XmlElement(ElementName = "OrderDeskType")]
        public string OrderDeskType { get; set; }
        [XmlElement(ElementName = "OrderStatus")]
        public string OrderStatus { get; set; }
        [XmlElement(ElementName = "PortalOrderAlert")]
        public string PortalOrderAlert { get; set; }
        [XmlElement(ElementName = "ProcessorAddress")]
        public string ProcessorAddress { get; set; }
        [XmlElement(ElementName = "ProcessorName")]
        public string ProcessorName { get; set; }
        [XmlElement(ElementName = "PRODUCTS_ORDERED")]
        public PRODUCTS_ORDERED PRODUCTS_ORDERED { get; set; }
        [XmlElement(ElementName = "ServiceName")]
        public string ServiceName { get; set; }
    }

    [XmlRoot(ElementName = "PRODUCT")]
    public class PRODUCT
    {
        [XmlElement(ElementName = "ProductName")]
        public string ProductName { get; set; }
    }

    [XmlRoot(ElementName = "PRODUCTS_ORDERED")]
    public class PRODUCTS_ORDERED
    {
        [XmlElement(ElementName = "PRODUCT")]
        public PRODUCT PRODUCT { get; set; }
    }

        [XmlRoot(ElementName = "ORDER_DETAIL_RESPONSE")]
    public class ORDER_DETAIL_RESPONSE
    {
        [XmlElement(ElementName = "ORDER_DETAILS")]
        public ORDER_DETAILS ORDER_DETAILS { get; set; }
        [XmlElement(ElementName = "PROPERTY_DATA")]
        public PROPERTY_DATA PROPERTY_DATA { get; set; }
        [XmlElement(ElementName = "PARTY_DATA")]
        public PARTY_DATA PARTY_DATA { get; set; }
        [XmlElement(ElementName = "SERVICE_INFORMATION")]
        public SERVICE_INFORMATION SERVICE_INFORMATION { get; set; }

    }

    [XmlRoot(ElementName = "MESSAGE")]
    public class OrderDetailsResponse
    {

        [XmlElement(ElementName = "FASTWEB_ACK_NACK")]
        public FASTWEB_ACK_NACK FASTWEB_ACK_NACK { get; set; }
        [XmlElement(ElementName = "FASTWEB_HEADER")]
        public FASTWEB_HEADER FASTWEB_HEADER { get; set; }
        [XmlElement(ElementName = "FASTWEB_ORDER_RESPONSE")]
        public FASTWEB_ORDER_RESPONSE FASTWEB_ORDER_RESPONSE { get; set; }

    }
}
