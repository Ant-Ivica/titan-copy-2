using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FA.LVIS.Tower.DataContracts
{
    public class MessageType : DataContractBase
    {
        public int MessageTypeId { get; set; }

        public string MessageTypeName { get; set; }

        public int MessageMapId { get; set; }

        public int Sequence { get; set; }

        public bool Editable { get; set; }

        public string MessageTypeDescription { get; set; }
    }
}
