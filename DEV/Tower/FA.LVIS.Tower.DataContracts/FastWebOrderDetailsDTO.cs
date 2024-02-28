using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FA.LVIS.Tower.DataContracts
{
    public class FastWebOrderDetailsDTO : DataContractBase
    {
        public int? FASTWebOrderNumber { get; set; }
        public DateTime? OrderDate { get; set; }
        public string CustomerRefNumber { get; set; }
        public decimal? Loanamount { get; set; }
        public decimal? SalePrice { get; set; }
        public string Transactiontype { get; set; }
        public string Propertytype { get; set; }
        public string Propertyuse { get; set; }
        public string Customeroffice { get; set; }
        public string Officeaddress { get; set; }
        public string Contact { get; set; }
        public string Email { get; set; }
        public string Orderphone { get; set; }
        public string Propertyaddress { get; set; }
        public string APN { get; set; }
        public string County { get; set; }
        public string Legaldescription { get; set; }
        public string Borrowerentitytype { get; set; }
        public string Maritalstatus { get; set; }
        public string Lastname { get; set; }
        public string Firstname { get; set; }
        public string Currentaddress { get; set; }
        public string Spouselastname { get; set; }
        public string Spousefirstname { get; set; }
        public string Servicenname { get; set; }
        public string Processor { get; set; }
        public string Address { get; set; }
        public string Orderdesktype { get; set; }
        public string Contactname { get; set; }
        public string Servicephone { get; set; }
        public string Serviceemail { get; set; }
        public string Status { get; set; }
        public int? FASTfilenumber { get; set; }
        public string Portalordealert { get; set; }
        public string Comments { get; set; }
        public string Product { get; set; }
    }
}
