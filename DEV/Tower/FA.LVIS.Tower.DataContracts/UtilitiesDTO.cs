using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FA.LVIS.Tower.DataContracts
{

    public class ServiceRequestDTO 
    {
        public int ServiceRequestId { get; set; }

        public string ExternalRefNum { get; set; }

        public string InternalRefNum { get; set; }

        public int? InternalRefId { get; set; }

        public string CustomerRefNum { get; set; }
        public int  tenantID { get; set; }

        public ExceptionStatus Status { get; set; }

    }


    public class AddCredentialDTO
    {
        public string Application { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }

}
