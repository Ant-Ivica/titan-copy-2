using FA.LVIS.Tower.Core;
using System.Collections.Generic;
using DC = FA.LVIS.Tower.DataContracts;

namespace FA.LVIS.Tower.Data
{
    public interface IFASTFilePreferenceDataProvider : IDataProviderBase
    {
        List<DC.StateMappingDTO> GetStatesList();

        List<DC.CountyMappingDTO> GetCountyList(string StateFips);

        List<DC.FASTFilePreferenceDTO> GetFASTFilePreferencesDetails(int tenantId);

        List<DC.FASTFilePreferenceDTO> GetFASTFilePreferencesDetails(string stateFipsId, string countyFipsId, string loanAmountId, int tenantId,string Regionid);

        IEnumerable<DC.ProgramTypeMappingDTO> GetProgramTypeList(int regionId);

        IEnumerable<DC.ProductTypeMappingDTO> GetProductTypeList(int appId);

        IEnumerable<DC.SearchTypeMappingDTO> GetSearchTypeList();

        IEnumerable<DC.ExceptionStatus> GetLoanPurposeList();

        DC.FASTFilePreferenceDTO AddFASTFilePreference(DC.FASTFilePreferenceDTO value, int tenantId, int userId);

        DC.FASTFilePreferenceDTO GetFASTFilePreferencesDetailsById(int tenantId, int FastPreferencemapid);

        DC.FASTFilePreferenceDTO UpdateFastFile(DC.FASTFilePreferenceDTO value, int tenantId, int userId);

        IEnumerable<DC.BusinessProgramTypeMappingDTO> GetBusinessFASTProgramTypeList(int regionId);

        int Delete(int id);

        int ConfirmDelete(int id);
        
        IEnumerable<DC.LocationsMappings> GetLocation();

        IEnumerable<DC.TenantMappingDTO> GetTenant();

        List<DC.FASTFilePreferenceDTO> GetValidatorFilePreferences(string state, string county, string loanAmount, int serviceId, int locationId, int regionId, int loanPurposeTypeCodeId,int TenantId,int ProductId);

    }
}
