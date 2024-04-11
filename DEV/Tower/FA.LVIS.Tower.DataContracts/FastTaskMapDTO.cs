using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FA.LVIS.Tower.DataContracts
{
   public class FastTaskMapDTO :DataContractBase
    {
        public int FastTaskMapId { get; set; }

        public string FastTaskMapName { get; set; }

        public bool HasReasonCode { get; set; }

        public int MessageTypeId { get; set; }

        public int TypecodeId { get; set;}

        public int RegionId { get; set; }

        public int serviceId { get; set; }

        public string FastTaskMapDesc { get; set; }

        public bool? IsInbound { get; set; }

        public bool? ContainsRegioncode { get; set; }

        public string Region { get; set; }

        public string service { get; set; }

        public string MessageType { get; set; }

        public string Typecode { get; set; }

        public int TenantId { get; set; }

        public string ISstrInbound { get; set; }

        public string ISstrContainsRegioncode { get; set; }

        public string Tenant { get; set; }

        public Nullable<int> applicationId { get; set; }

        public string ApplicationName { get; set; }

        public int CustomerId { get; set; }

        public string CustomerName { get; set; }
    }
}
