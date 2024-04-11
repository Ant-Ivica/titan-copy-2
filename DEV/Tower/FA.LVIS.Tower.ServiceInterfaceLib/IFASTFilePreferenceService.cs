using System.Collections.Generic;
using FA.LVIS.Tower.DataContracts;

namespace FA.LVIS.Tower.Services
{
    public interface IFASTFilePreferenceService : Core.IServiceBase
    {
        List<StateMappingDTO> GetStatesList();

        List<CountyMappingDTO> GetCountyList(string StateFips);

        List<FASTFilePreferenceDTO> GetFASTFilePreferencesDetails(int tenantId);

        List<FASTFilePreferenceDTO> GetFilePreferences(string stateFipsId, string countyFipsId, string loanAmount, int tenantId, string Regionid);

        IEnumerable<ProgramTypeMappingDTO> GetProgramTypeList(int regionId);

        //List<ProgramTypeMappingDTO> GetProgramType();

        IEnumerable<ProductTypeMappingDTO> GetProductTypeList(int appId);

        IEnumerable<SearchTypeMappingDTO> GetSearchTypeList();

        int Delete(int id);

        int ConfirmDelete(int id);

        IEnumerable<ExceptionStatus> GetLoanPurposeList();

        FASTFilePreferenceDTO AddFastFile(FASTFilePreferenceDTO value, int tenantId, int userId);

        FASTFilePreferenceDTO GetFASTFilePreferencesDetailsById(int tenantId, int FastPreferencemapid);

        FASTFilePreferenceDTO UpdateFastFile(FASTFilePreferenceDTO value, int tenantId, int userId);

        IEnumerable<BusinessProgramTypeMappingDTO> GetBusinessFASTProgramTypeList(int regionId);

        IEnumerable<LocationsMappings> GetLocationList();

        IEnumerable<TenantMappingDTO> GetTenant();

        List<FASTFilePreferenceDTO> GetValidatorFilePreferences(string state, string county, string loanAmount, int serviceId, int locationId, int regionId, int loanPurposeTypeCodeId, int TenantId, int ProductId);

    }
}
