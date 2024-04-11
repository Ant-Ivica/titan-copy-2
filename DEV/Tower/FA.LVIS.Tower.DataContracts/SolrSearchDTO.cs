using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FA.LVIS.Tower.DataContracts
{
    public class FSearchParam
    {
        public FSearchParam()
        {
            Facets = new List<string>();
            FacetKeyValue = new List<FacetKeyValue>();
            status = 0;
            SingleLineSearch = "*";
            IsFacetRequired = true;
        }
        public List<string> Facets { get; set; }
        public string Activity { get; set; }
        public List<FilterBy> FilterBy { get; set; }
        public string FilterByAC { get; set; }

        public int? RegionID { get; set; }
        public string OwningOffice { get; set; }

        public int StartIndex { get; set; }
        public int NumberOfRows { get; set; }

        public DateType dateType { get; set; }
        public string SingleLineSearch { get; set; }
        //public FilterBy FilterBy { get; set; }
        public SortBy SortBy { get; set; }
        public List<FacetKeyValue> FacetKeyValue { get; set; }
        public string EmployeeName { get; set; }

        public string Dates { get; set; }

        public FastFileStatus status { get; set; }
        public string Environment { get; set; }

        public string Source { get; set; }
        public string Originator { get; set; }
        public bool IsFacetRequired { get; set; }
        public int TestRegionFlag { get; set; }//0 dont apply filter,1 testregiononly,2 nontestregion
        public int ExchangeFlag { get; set; } //0 dont apply filter,1 only exchange, 2 no exchange
    }

    public enum FilterBy
    {
        All,
        ApnTax,
        FBP,
        Buyer,
        FileNumber,
        Invoice_Number,
        Address,
        ReferenceNumber,
        Seller,
        TaxPayer,
        Escrow_Number,
        ReverseEntity
    }
    public enum SortByEnum
    {
        Default = 0,
        FileNumberSort = 1,
        FirstBuyerSort = 2,
        FirstSellerSort = 3,
        OpenDateSort = 4,
        PrimaryTaxPayerSort = 5

    }
    public class SortBy
    {
        public SortByEnum SortByItem { get; set; }
        public bool isASC { get; set; }
    }
    public enum DateType
    {
        Open_Date,
        Cancelled_Date,
        Settlement_Date,
        EstSettlement_Date,
        FoutyFifth_Date,
        OneEightyth_Date,
        ActualId_Date,
        Coe_Date
    }
    public class FacetKeyValue
    {
        public string FacetKey { get; set; }
        public string FacetValue { get; set; }
    }

    public class AdvanceSearch : FSearchParam
    {
        public AdvanceSearch()
        {
        }
        public List<AdvanceSeachField> AdvanceSeachField { get; set; } = new List<AdvanceSeachField>();
        public PropertyInformation PropertyInformation { get; set; }
        public Shortlegal ShortLegal { get; set; }
    }

    /// <summary>
    /// Addvance search field accet a list of below object.
    /// normally we have 3 type of information passed.
    /// which type of search
    /// whats the query
    /// if there is dropdown whats the value for that dropdown.
    /// only for property and legal we accept a object of property and sortleagal 
    /// </summary>
    public class AdvanceSeachField
    {
        public SearchFieldType TypeOfSearch { get; set; }
        public dynamic SearchQuery { get; set; }
        public int SelectedItem { get; set; }
    }
    public enum SearchFieldType
    {
        Principle = 0,
        PropertyInformation = 1,
        Numbers = 2,
        //Region Office Goes to Filter
        EmployeeType = 3,
        //status goes to filter
        //dateType Goes to filter
        Parties = 4,
        SortLegal = 5,
        //Source=6, goes to filter
        //Originator=7 goes to filter
        LenderRef_Number = 8,
        LenderBusOrgId = 9,
        MutipleStatus = 10,
        DFSBuyer = 11,
        TaxPayerEA = 12
    }
    public enum Principle
    {
        Principals = 0,
        Buyer = 1,
        Seller = 2,
        Individual = 3,
        BusinessEntity = 4,
        TrustEstate = 5,
        HusbandWife = 6,
        Individual_Buyer = 7,
        BusinessEntity_Buyer = 8,
        TrustEstate_Buyer = 9,
        HusbandWife_Buyer = 10,
        Individual_Seller = 11,
        BusinessEntity_Seller = 12,
        TrustEstate_Seller = 13,
        HusbandWife_Seller = 14,
        TaxPayer = 15,
        Individual_TaxPayer = 16,
        BusinessEntity_TaxPayer = 17,
        TrustEstate_TaxPayer = 18,
        HusbandWife_TaxPayer = 19

    }
    public class PropertyInformation
    {
        public string PropertyName { get; set; }
        public string PropertyAddressLine { get; set; }
        public string ApnTax { get; set; }
        public string State { get; set; }
        public string County { get; set; }
        public string Country { get; set; }
    }

    public enum Numbers
    {
        AllFileNumbers,
        FileNumber,
        ExternalFile_Number,
        Invoice_Number,
        OutsideRef_Number,
        Policy_Number,
        PrincipalsRef_Number,
        EntityRef_Number,
        BusinessSrcRef_Number,
        Escrow_Number

    }
    public enum EmployeeType
    {
        Employees,
        EscrowOfficer_Employee,
        TitleOfficer_Employee,
        EscrowAssistant_Employee,
        TitleAssistant_Employee,
        Underwriter_Employee,
        SalesRep,
        Exchange_Officer_Employee,
        Exchange_Assistant_Employee,
    }
    public enum FastFileStatus
    {
        All,
        Open,
        Closed,
        Open_in_Error,
        Cancelled,
        Pending,
        Pending_Open_In_Error,
        Pending_Cancelled
    }



    public enum Parties
    {
        FBP = 0,//All
        AssociatedParty_FBP = 774,
        AssumptionLender_FBP = 673,
        Buyer_FBP = 113,
        Seller_FBP = 114,
        AssumptionLender_3rdParty_FBP = 2974,
        Attorney_FBP = 86,
        BrokerDisbursement_FBP = 681,
        BusinessSource_FBP = 690,
        BuyersAttorney_FBP = 322,
        BuyersBroker_FBP = 323,
        BuyersREBrokerTC_FBP = 2368,
        BuyerREAgent_FBP = 685,
        ConstructionCompany_FBP = 87,
        DirectedBy_FBP = 691,
        ExchangeBuyerAgent_FBP = 1976,
        ExchangeCPA_FBP = 1983,
        ExchangeCreditTo_FBP = 1980,
        ExchangeEscrowOfficer_FBP = 1984,
        ExchangeSellerAgent_FBP = 1977,
        ExchangeTaxPayerAgent_FBP = 2030,
        ExchangeTaxPayerRep_FBP = 1986,
        ExchangeCompany_FBP = 1501,
        ForSaleByOwner_FBP = 689,
        HazardInsuranceUnderwriter_FBP = 678,
        HOA_ManagementCompany_FBP = 357,
        HomeWarranty_FBP = 96,
        HomeOwnerAssociation_FBP = 93,
        IBABeneficiary_FBP = 1484,
        InspectionRepair_FBP = 94,
        InsuranceAgent_FBP = 677,
        Lender_FBP = 84,
        Lender_3PartyPayee_FBP = 771,
        LendersAttorney_FBP = 680,
        Lessor_FBP = 676,
        Miscellaneous_FBP = 101,
        MortgageBroker_FBP = 670,
        MortgageBroker_3PartyPayee_FBP = 772,
        NewLender_FBP = 688,
        Other_FBP = 329,
        OtherREBrokerTC_FBP = 2367,
        OtherREAgent_FBP = 687,
        OtherREBroker_FBP = 675,
        OEC_FBP = 90,
        OTC_FBP = 317,
        OwnerOffice_FBP = 683,
        Payee_FBP = 679,
        PayoffLender_FBP = 674,
        PayoffLender_3rdParty_FBP = 2665,
        ProductionOffice_FBP = 684,
        RealEstateInvestment_Trust_FBP = 261,
        SearchVendor_FBP = 1680,
        SellersAttorney_FBP = 325,
        SellersBroker_FBP = 326,
        SellersREBrokerTC_FBP = 2371,
        SellersREAgent_FBP = 686,
        ServiceFee_FBP = 1786,
        SplitEntity_FBP = 776,
        SplitOffice_FBP = 682,
        Survey_FBP = 97,
        TaxCollector_FBP = 99,
        TitleAgent_FBP = 253,
        TitleCompanyDraft_FBP = 327,
        Trustee_FBP = 92,
        UtilityCompany_FBP = 95
    }

    public class Shortlegal
    {
        public string Lot { get; set; }
        public string Block { get; set; }
        public string Unit { get; set; }
        public string Track { get; set; }
        public string Fee { get; set; }
        public string Building { get; set; }
        public string Book { get; set; }
        public string Page { get; set; }
        public string Section { get; set; }
        public string Township { get; set; }
        public string Parcel { get; set; }
        public string Range { get; set; }
        public string Subdivision_Condo { get; set; }
        public string Phase { get; set; }
        public string GovtLotNo { get; set; }
        public string Borough { get; set; }
        public string Province { get; set; }
        public string County_Parish { get; set; }
    }

    public class SolrFileSearchResponse
    {
        public SolrFileSearchResponse()
        {
            //Search = new FSearchParam();
            Facets = new Dictionary<string, ICollection<KeyValuePair<string, int>>>();
            Files = new List<SolrFileDto>();

        }
        //public FSearchParam Search { get; set; }
        public List<SolrFileDto> Files { get; set; }
        public int TotalCount { get; set; }
        public IDictionary<string, ICollection<KeyValuePair<string, int>>> Facets { get; set; }
    }

    public class SolrFileDto
    {
        public int OrderId { get; set; }
        public int RegionId { get; set; }
        public int BuyerCount { get; set; }
        public int TaxPayerCount { get; set; }
        public int SellerCount { get; set; }
        public int FileId { get; set; }
        public string OwningOffice { get; set; }
        public string TransactionType { get; set; }
        public string Status { get; set; }
        public string FileNumber { get; set; }
        public DateTime? Open_Date { get; set; }
        public string FirstBuyer { get; set; }
        public string FirstSeller { get; set; }
        public string FirstPropertyAddress { get; set; }
        public string PrimaryTaxPayer { get; set; }
        public string FirstRelProperty { get; set; }
        //List Items start here
        public List<string> ServiceType { get; set; }
        public SolrFileDto()
        {
            ServiceType = new List<string>();
        }
    }
}
