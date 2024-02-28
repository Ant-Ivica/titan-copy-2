using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FA.LVIS.Tower.DataContracts
{
    public class FASTProcessTriggerDTO : DataContractBase
    {
        public int FASTWorkFlowMapId { get; set; }

        public int MessageTypeId { get; set; }

        public int ProcessEventId { get; set; }

        public int TaskeventId { get; set; }
    
        public string FASTOWorkFlowMapDesc { get; set; }

        public string MessageType { get; set; }

        public string ProcessEvent { get; set; }

        public string Taskevent { get; set; }

        public int Tenatid { get; set; }

        public string Tenant { get; set; }

        public int serviceId { get; set; }

        public string serviceName { get; set; }

        public string  Customer { get; set; }
        public int  Customerid { get; set; }

    }
}
