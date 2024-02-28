using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FA.LVIS.Tower.DataContracts
{
    public partial class CustomerRegistrationDTO
    {
        public int CustomerRegistrationId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailId { get; set; }
        public string PhoneNo { get; set; }
        public string CompanyName { get; set; }
        public string ProjectName { get; set; }
        public bool TitleAndSettlement { get; set; }
        public bool IneractiveOfficeDirectory { get; set; }
        public string Comments { get; set; }
        public int CustomerStatus { get; set; }
        public string CustomerStatusName { get; set; }

        public bool Other { get; set; }
        public string OtherRequirement { get; set; }


    }
}
