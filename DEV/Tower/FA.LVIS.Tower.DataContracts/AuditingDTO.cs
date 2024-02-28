using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FA.LVIS.Tower.DataContracts
{
    //public class SearchDetail1:DataContractBase
    //{
    //    public string search { get; set; }
    //    public string Fromdate { get; set; }
    //    public string ThroughDate { get; set; }
    //}
    public class EmailDetail : DataContractBase
    {
        public string EmailId { get; set; }

        public string EmailSubject { get; set; }

        public string EmailBody { get; set; }
    }

    public class SearchDetail : DataContractBase
    {
        public string search { get; set; }

        public string Fromdate { get; set; }

        public string ThroughDate { get; set; }

        public int statusId { get; set; }

        public bool Typecodestatus { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }

        public string ReferenceNoType { get; set; }

        public string ReferenceNo { get; set; }

        public bool ErrorEnabled { get; set; }
        public bool InfoEnabled { get; set; }
        public string MessageText { get; set; }

        public int currPage { get; set; }
        public int pageSize { get; set; }

    }

    public class AuditingDTO : DataContractBase
    {
        public string UserName { get; set; }

        public string EventDateutc { get; set; }

        public string EventType { get; set; }

        public string TableName { get; set; }

        public string RecordId { get; set; }

        public string Property { get; set; }

        public string OriginalValue { get; set; }

        public string NewValue { get; set; }

        public string Section { get; set; }

    }



}
