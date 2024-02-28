using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FA.LVIS.Tower.DataContracts
{
    public class EMSQueue
    {
        public bool Incoming { get; set; }
        public string QueueName { get; set; }
        public bool Active { get; set; }
    }


    public class BizTalkConnectorDTO
    {

        public List<ConnectorDTO> HostInstances { get; set; }

        public List<ConnectorDTO> SendPorts { get; set; }

        public List<ConnectorDTO> RecievePorts { get; set; }
    }

    public class ConnectorDTO
    {
        public string Name { get; set; }
        public Boolean Status { get; set; }
    }
}