using System;
using System.Collections.Generic;

namespace FA.LVIS.Tower.DataContracts
{
    public class FASTFilePreferenceDTO : DataContractBase
    {
        public int FASTPreferenceMapID { get; set; }        

        public string FASTPreferenceMapName { get; set; }

        public int FASTProductTypeId { get; set; }

        public int FASTProgramTypeId { get; set; }

        public int FASTSearchTypeId { get; set; }

        public int ServiceId { get; set; }

        public int LocationId { get; set; }

        public int TenantId { get; set; }

        public string Tenant { get; set; }

        public int? RegionId { get; set; }

        public string Region { get; set; }

        public string ProgramTypeName { get; set; }

        public string ProductTypeCd { get; set; }

        public string ProductTypeDesc { get; set; }

        public string SearchType { get; set; }

        public string LoanPurpose { get; set; }

        public string Location { get; set; }

        public decimal LoanAmount { get; set; }

        public string PreferenceState { get; set; }

        public string PreferenceCounty { get; set; }

        public string CustomerName { get; set; }

        public int? CustomerId { get; set; }

        public List<ConditionPreferenceDTO> Conditions {get; set; }

        public List<BusinessProgramTypeMappingDTO> BusinessProgramTypes{ get; set; }

        public List<ProductTypeMappingDTO>  ProductTypes { get; set; }

        public List<ProductTypeMappingDTO> FastProductTypes { get; set; }        

        public ExceptionStatus LoanPurposeDet { get; set; }

        public LocationsMappings LocationDet { get; set; }


        public List<string> ProductDesc { get; set; }

        public List<string> BuisnessProgramType { get; set; }
    }

    public class ConditionPreferenceDTO : DataContractBase
    {
        public StateMappingDTO  PreferenceState { get; set; }

        public CountyMappingDTO PreferenceCounty { get; set; }
    }

    public class ProgramTypeMappingDTO : DataContractBase
    {
        public int ProgramTypeId { get; set; }

        public string ProgramTypeName { get; set; }

        public int RegionId { get; set; }
    }

    public class BusinessProgramTypeMappingDTO : DataContractBase
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int RegionId { get; set; }

        public Boolean Ticked { get; set; }
    }

    public class ProductTypeMappingDTO : DataContractBase
    {
        public int ProductTypeId { get; set; }

        public string ProductTypecode { get; set; }

        public string ProductTypedesc { get; set; }

        public string ProductName { get; set; }

        public Boolean Ticked { get; set; }
    }

    public class SearchTypeMappingDTO : DataContractBase
    {
        public int SearchTypeId { get; set; }

        public string SearchTypecode { get; set; }

        public string SearchTypedesc { get; set; }
    }

    public class StateMappingDTO : DataContractBase
    {
        public string StateFIPS { get; set; }

        public string StateCodes { get; set; }

        public string PreferenceState { get; set; }
    }

    public class CountyMappingDTO : DataContractBase
    {
        public string countyFIPS { get; set; }
        public string county { get; set; }

    }
}
