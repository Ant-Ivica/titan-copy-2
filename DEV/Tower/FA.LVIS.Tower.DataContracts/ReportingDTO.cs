namespace FA.LVIS.Tower.DataContracts
{
    public class ReportingDTO
    {
        public int ServiceRequestId { get; set; }

        public string service { get; set; }

        public string createddate { get; set; }

        public string CustomerName { get; set; }

        public int CustomerId { get; set; }

        public string ExternalRefNum { get; set; }

        public string ApplicationId { get; set; }

        public string InternalRefNum { get; set; }

        public string InternalRefId { get; set; }

        public string CustomerRefNum { get; set; }

        public string Tenant { get; set; }
        public string LenderId { get; set; }

        public string LVISActionType { get; set; }

        public string OrderStatus { get; set; }
        
    }
}
