using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FA.LVIS.Tower.DataContracts
{
    public class FastWebOrderDetailResponse
    {
        [JsonProperty("FASTWEBackNack")]
        public FASTWEBackNack FASTWEBackNack { get; set; }

        [JsonProperty("FASTWEBHEADER")]
        public FASTWEBHEADER FASTWEBHEADER { get; set; }

        [JsonProperty("FASTWEBORDERRESPONSE")]
        public FASTWEBORDERRESPONSE FASTWEBORDERRESPONSE { get; set; }
    }

    public class FASTWEBORDERRESPONSE
    {
        [JsonProperty("ORDERDETAILRESPONSE")]
        public ORDERDETAILRESPONSE ORDERDETAILRESPONSE { get; set; }

        [JsonProperty("FASTNOTIFYRESPONSE")]
        public FASTNOTIFYRESPONSE FASTNOTIFYRESPONSE { get; set; }
    }

    public class ORDERDETAILRESPONSE
    {
        [JsonProperty("OrderDetails")]
        public OrderDetails OrderDetails { get; set; }

        [JsonProperty("PropertyData")]
        public PropertyData PropertyData { get; set; }

        [JsonProperty("PartyData")]
        public PartyData PartyData { get; set; }

        [JsonProperty("ServiceInformation")]
        public List<ServiceInformation> ServiceInformation { get; set; }
    }

    public class ServiceInformation
    {
        [JsonProperty("Comments")]
        public string Comments { get; set; }

        [JsonProperty("Contactemail")]
        public string Contactemail { get; set; }

        [JsonProperty("Contactname")]
        public string Contactname { get; set; }

        [JsonProperty("Contactnumber")]
        public string Contactnumber { get; set; }

        [JsonProperty("Fastfilenumber")]
        public int Fastfilenumber { get; set; }

        [JsonProperty("Orderdesktype")]
        public string Orderdesktype { get; set; }

        [JsonProperty("Orderstatus")]
        public string Orderstatus { get; set; }

        [JsonProperty("Portalorderalert")]
        public string Portalorderalert { get; set; }

        [JsonProperty("Processoraddress")]
        public string Processoraddress { get; set; }

        [JsonProperty("Processorname")]
        public string Processorname { get; set; }

        [JsonProperty("Productsordered")]
        public Productsordered Productsordered { get; set; }

        [JsonProperty("ServiceName")]
        public string ServiceName { get; set; }
    }

    public class Product
    {
        [JsonProperty("ProductName")]
        public string ProductName { get; set; }
    }

    public class Productsordered
    {
        [JsonProperty("Product")]
        public List<Product> Product { get; set; }
    }

    public class PartyData
    {
        [JsonProperty("Party")]
        public List<PARTY> Party { get; set; }
    }

    public class PARTY
    {
        [JsonProperty("Address")]
        public string Address { get; set; }

        [JsonProperty("Entitytype")]
        public string Entitytype { get; set; }

        [JsonProperty("Firstname")]
        public string Firstname { get; set; }

        [JsonProperty("Lastname")]
        public string Lastname { get; set; }

        [JsonProperty("Maritalstatus")]
        public string Maritalstatus { get; set; }

        [JsonProperty("Spousefirstname")]
        public string Spousefirstname { get; set; }

        [JsonProperty("Spouselastname")]
        public string Spouselastname { get; set; }
    }

    public class PropertyData
    {
        [JsonProperty("Apn")]
        public string APN { get; set; }

        [JsonProperty("County")]
        public string County { get; set; }

        [JsonProperty("Legaldescription")]
        public string Legaldescription { get; set; }

        [JsonProperty("Propertyaddress")]
        public string Propertyaddress { get; set; }
    }

    public class OrderDetails
    {
        [JsonProperty("CustomerContact")]
        public string CustomerContact { get; set; }

        [JsonProperty("Customeroffice")]
        public string Customeroffice { get; set; }

        [JsonProperty("Customerrefnumber")]
        public string Customerrefnumber { get; set; }

        [JsonProperty("Dateopened")]
        public DateTime Dateopened { get; set; }

        [JsonProperty("Email")]
        public string Email { get; set; }

        [JsonProperty("Fwordernumber")]
        public int Fwordernumber { get; set; }

        [JsonProperty("Loanamount")]
        public decimal Loanamount { get; set; }

        [JsonProperty("Officeaddress")]
        public string Officeaddress { get; set; }

        [JsonProperty("Phone")]
        public string Phone { get; set; }

        [JsonProperty("Propertytype")]
        public string Propertytype { get; set; }

        [JsonProperty("Propertyuse")]
        public string Propertyuse { get; set; }

        [JsonProperty("Saleprice")]
        public decimal Saleprice { get; set; }

        [JsonProperty("Transactiontype")]
        public string Transactiontype { get; set; }
    }

}
