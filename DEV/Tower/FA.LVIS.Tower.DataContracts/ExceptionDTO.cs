using System;
using System.Collections.Generic;

namespace FA.LVIS.Tower.DataContracts
{
    public class SearchDetailDTO
    {
        public string FileNumber { get; set; }
        public string City { get; set; }
        public string County { get; set; }
        public string State { get; set; }
        public string Address { get; set; }
        public string Buyer { get; set; }
        public string Organization { get; set; }
        public string LenderName { get; set; }
        public string FastFileID { get; set; }
        public int TenantId { get; set; }


    }

    public class BEQParseXMLDTO
    {
        public string escrowAssistantEmail { get; set; }
        public string escrowAssistant { get; set; }
        public string escrowOfficerEmail { get; set; }
        public int RegionID { get; set; }
        public string RegionName { get; set; }
        public string OfficeName { get; set; }
        public string escrowOfficer { get; set; }
        public string County { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Address { get; set; }
        public string Seller { get; set; }
        public string Buyer { get; set; }
        public string Organization { get; set; }
        public string LenderName { get; set; }
        public string LoanOfficer { get; set; }
        public string Service { get; set; }
        public string FastFile { get; set; }
        public List<int> FastFileIDs { get; set; }

        public string Transaction { get; set; }
        public List<PotentialMatchDTO> PotentialMatches { get; set; }
        public string LoanNumber { get; set; }

        public bool IsBindonly { get; set; }


    }
    public class ExceptionDTO
    {
        public int Exceptionid { get; set; }

        public int ExceptionTypeid { get; set; }

        public long DocumentObjectid { get; set; }

        public string ExceptionType { get; set; }

        public string ExceptionDesc { get; set; }

        public string CreatedDate { get; set; }

        public string LastModifiedDate { get; set; }

        public string CreatedBy { get; set; }

        public string LastModifiedBy { get; set; }

        public ExceptionStatus Status { get; set; }

        public List<string> Comments { get; set; }

        public string Notes { get; set; }

        public string MessageContent { get; set; }

        public string ExternalRefNum { get; set; }

        public string ParentExternalRefNum { get; set; }

        public string MessageType { get; set; }

        public int MessageTypeid { get; set; }

        public string ServiceType { get; set; }

        public int ServiceRequestId { get; set; }

        public int TypeCodeId { get; set; }

        public bool InvolveResolved { get; set; }

        public ReportingDTO Reporting { get; set; }

        public List<ExceptionDTO> children { get; set; }



        public int TenantId { get; set; }

        public string Tenant { get; set; }

        public string TransactionType { get; set; }

        public string RejectNotes { get; set; }

        public string Buyer { get; set; }
    }

    public class ExceptionType
    {
        public int ExceptionTypeId { get; set; }

        public string ExceptionTypeName { get; set; }
        public string ExceptionTypeDescription { get; set; }
        public int ExceptionGroupId { get; set; }
        public int ThresholdCount { get; set; }
    }
    public class ResubmitBulkExceptionDTO:DataContracts.DataContractBase
    {
        public int SuccessResubmitCount { get; set; }

        public int UnSuccessResubmitCount { get; set; }

        public int TotalResubmitCount { get; set; }
    }


}