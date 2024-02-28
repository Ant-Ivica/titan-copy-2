using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FA.LVIS.Tower.DataContracts
{
    public class PotentialMatchDTO:DataContractBase
    {
        public int? FileID { get; set; }
        public int? BUID { get; set; }
        public int? RegionID { get; set; }
        public string RegionName { get; set; }
        public string OfficeName { get; set; }
        public string FileNumber { get; set; }
        public string ExternalReferenceNumber { get; set; }
        public string PropertyAddress { get; set; }
        public string City { get; set; }
        public string StateName { get; set; }
        public string County { get; set; }
        public string Buyer { get; set; }
        public string BuyerSpouse { get; set; }
        public string Seller { get; set; }
        public string SellerSpouse { get; set; }
        public string TransactionType { get; set; }
        public string ServiceType { get; set; }
        public string NewLoanLender { get; set; }
        public string MortgageBroker { get; set; }
        public string OfficerName { get; set; }
        public decimal FirstNewLoanAmount { get; set; }
        public string LoanNumber { get; set; }
        public decimal SalePrice { get; set; }
        public DateTime OpenDate { get; set; }
        public int ExceptionID { get; set; }
        public bool ManualMatch { get; set; }
        public bool? SourceProvidedMatch { get; set; }


    }
}