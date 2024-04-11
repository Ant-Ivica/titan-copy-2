using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FA.LVIS.Tower.DataContracts
{
    public class MessageLogDTO
    {
        public ReportingDTO ReportDetails { get; set; }

        public List<MessageLogDetailDTO> MessageLogDetails { get; set; }

    }

    public class MessageLogDetailDTO
    {

        public string Application { get; set; }

        public string ExternalApplication { get; set; }

        public string DataContent { get; set; }

        public string DataFormat { get; set; }

        public bool IsIncoming { get; set; }

        public string CreatedDateTime { get; set; }

        public string Description { get; set; }

        public int ParentMessageLogId { get; set; }

        public int MessageLogId { get; set; }

        public int ExceptionId { get; set; }

        public long Documentobjectid { get; set; }

        public bool hasChild { get; set; }
        public string ExceptionDescription { get; set; }


    }
}
