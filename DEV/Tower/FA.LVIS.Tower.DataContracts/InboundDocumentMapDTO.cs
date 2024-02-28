using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FA.LVIS.Tower.DataContracts
{
    public class InboundDocumentMapDTO : DataContractBase
    {
        public string ExternalApplicationName { get; set; }

        public string ExternalDocumentTypeName { get; set; }

        public string ExternalDocumentDescription { get; set; }

        public string ExternalDocumentTypeDesc { get; set; }

        public string ServiceName { get; set; }

        public string InternalDocumentTypeName { get; set; }

        public string InternalDocumentTypeDesc { get; set; }

        public int inboundDocumentMapid { get; set; }

        public int ExternalApplication { get; set; }

        public int ExternalDocumentType { get; set; }

        public int Service { get; set; }

        public int InternalDocumentType { get; set; }

        public int TenantId { get; set; }

        public string Tenant { get; set; }
    }
}
