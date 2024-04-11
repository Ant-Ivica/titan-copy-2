using FA.LVIS.Tower.Data;
using FA.LVIS.Tower.DataContracts;
using System.Collections.Generic;
using System;

namespace FA.LVIS.Tower.Services
{
    public class FASTFilePreferenceService : Core.ServiceBase, Services.IFASTFilePreferenceService
    {
        public List<StateMappingDTO> GetStatesList()
        {
            IFASTFilePreferenceDataProvider FilePrefProvider = DataProviderFactory.Resolve<IFASTFilePreferenceDataProvider>();
            return FilePrefProvider.GetStatesList();
        }

        public List<CountyMappingDTO> GetCountyList(string state)
        {
            IFASTFilePreferenceDataProvider FilePrefProvider = DataProviderFactory.Resolve<IFASTFilePreferenceDataProvider>();
            return FilePrefProvider.GetCountyList(state);
        }

        public List<FASTFilePreferenceDTO> GetFASTFilePreferencesDetails(int tenantId)
        {
            IFASTFilePreferenceDataProvider FilePrefProvider = DataProviderFactory.Resolve<IFASTFilePreferenceDataProvider>();
            return FilePrefProvider.GetFASTFilePreferencesDetails(tenantId);
        }

        public List<FASTFilePreferenceDTO> GetFilePreferences(string stateFipsId, string countyFipsId, string loanAmount, int tenantId, string Regionid)
        {
            IFASTFilePreferenceDataProvider FilePrefProvider = DataProviderFactory.Resolve<IFASTFilePreferenceDataProvider>();
            return FilePrefProvider.GetFASTFilePreferencesDetails(stateFipsId, countyFipsId, loanAmount, tenantId, Regionid);
        }

        public IEnumerable<ProgramTypeMappingDTO> GetProgramTypeList(int regionId)
        {
            IFASTFilePreferenceDataProvider OffProvider = DataProviderFactory.Resolve<IFASTFilePreferenceDataProvider>();
            return OffProvider.GetProgramTypeList(regionId);
        }

        //public List<ProgramTypeMappingDTO> GetProgramType()
        //{
        //    IFASTOfficeMappingDataProvider OffProvider = DataProviderFactory.Resolve<IFASTOfficeMappingDataProvider>();
        //    return OffProvider.GetProgramType();
        //}

        public IEnumerable<ProductTypeMappingDTO> GetProductTypeList(int appId)
        {
            IFASTFilePreferenceDataProvider OffProvider = DataProviderFactory.Resolve<IFASTFilePreferenceDataProvider>();
            return OffProvider.GetProductTypeList(appId);
        }

        public IEnumerable<SearchTypeMappingDTO> GetSearchTypeList()
        {
            IFASTFilePreferenceDataProvider OffProvider = DataProviderFactory.Resolve<IFASTFilePreferenceDataProvider>();
            return OffProvider.GetSearchTypeList();
        }

        public IEnumerable<ExceptionStatus> GetLoanPurposeList()
        {
            IFASTFilePreferenceDataProvider OffProvider = DataProviderFactory.Resolve<IFASTFilePreferenceDataProvider>();
            return OffProvider.GetLoanPurposeList();
        }

        public FASTFilePreferenceDTO AddFastFile(FASTFilePreferenceDTO value, int tenantId, int userId)
        {
            IFASTFilePreferenceDataProvider OffProvider = DataProviderFactory.Resolve<IFASTFilePreferenceDataProvider>();
            return OffProvider.AddFASTFilePreference(value, tenantId,  userId);
        }

        public FASTFilePreferenceDTO GetFASTFilePreferencesDetailsById(int tenantId, int FastPreferencemapid)
        {
            IFASTFilePreferenceDataProvider OffProvider = DataProviderFactory.Resolve<IFASTFilePreferenceDataProvider>();
            return OffProvider.GetFASTFilePreferencesDetailsById(tenantId, FastPreferencemapid);
        }

        public FASTFilePreferenceDTO UpdateFastFile(FASTFilePreferenceDTO value, int tenantId, int userId)
        {
            IFASTFilePreferenceDataProvider OffProvider = DataProviderFactory.Resolve<IFASTFilePreferenceDataProvider>();
            return OffProvider.UpdateFastFile(value, tenantId, userId);
        }

        public IEnumerable<BusinessProgramTypeMappingDTO> GetBusinessFASTProgramTypeList(int regionId)
        {
            IFASTFilePreferenceDataProvider OffProvider = DataProviderFactory.Resolve<IFASTFilePreferenceDataProvider>();
            return OffProvider.GetBusinessFASTProgramTypeList(regionId);

        }

        public int Delete(int id)
        {
            IFASTFilePreferenceDataProvider FilePrefProvider = DataProviderFactory.Resolve<IFASTFilePreferenceDataProvider>();
            return FilePrefProvider.Delete(id);
        }

        public int ConfirmDelete(int id)
        {
            IFASTFilePreferenceDataProvider FilePrefProvider = DataProviderFactory.Resolve<IFASTFilePreferenceDataProvider>();
            return FilePrefProvider.ConfirmDelete(id);
        }

        public IEnumerable<LocationsMappings> GetLocationList()
        {
            IFASTFilePreferenceDataProvider FilePrefProvider = DataProviderFactory.Resolve<IFASTFilePreferenceDataProvider>();
            return FilePrefProvider.GetLocation();
        }

        public IEnumerable<TenantMappingDTO> GetTenant()
        {
            IFASTFilePreferenceDataProvider FilePrefProvider = DataProviderFactory.Resolve<IFASTFilePreferenceDataProvider>();
            return FilePrefProvider.GetTenant();
        }

        public List<FASTFilePreferenceDTO> GetValidatorFilePreferences(string state, string county, string loanAmount, int serviceId, int locationId, int regionId, int loanPurposeTypeCodeId,int TenantId,int ProductId)
        {
            IFASTFilePreferenceDataProvider FilePrefProvider = DataProviderFactory.Resolve<IFASTFilePreferenceDataProvider>();
            return FilePrefProvider.GetValidatorFilePreferences(state, county, loanAmount, serviceId, locationId, regionId, loanPurposeTypeCodeId, TenantId, ProductId);             
        }

    }
}
