using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FA.LVIS.Tower.DataContracts
{
    public class OutDocMapping : DataContractBase
    {
        public Users ExternalApplication { get; set; }

        public DocumentType ExternalDocumentType { get; set; }

        public Service Service { get; set; }

        public DocumentType InternalDocumentType { get; set; }

        public string ExternalMessageTypeValue { get; set; }


        public List<MessageType> ExternalMessageTypeList { get; set; }
        public MessageType ExternalMessageType { get; set; }

        public DocumentType DocumentStatus { get; set; }

        public Users Tenant { get; set; }

        public int OutboundDocumentMapid { get; set; }

        public int MessageMapId { get; set; }

        public int CategoryId { get; set; }

        public int Customerid { get; set; }
    }
}
