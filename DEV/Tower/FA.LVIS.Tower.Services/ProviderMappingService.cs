using System.Collections.Generic;

using DC = FA.LVIS.Tower.DataContracts;
using FA.LVIS.Tower.Data;
using System;

namespace FA.LVIS.Tower.Services
{
    public class ProviderMappingService : Core.ServiceBase, IProviderMappingService
    {
        public DC.Provider AddProvider(DC.Provider OfficeProfile, int employeeId, int tenantId)
        {
            IProviderMappingDataProvider OffProvider = DataProviderFactory.Resolve<IProviderMappingDataProvider>();
            return OffProvider.AddProvider(OfficeProfile,employeeId,tenantId);
        }

        public int DeleteOffice(int ID)
        {
            IProviderMappingDataProvider OffProvider = DataProviderFactory.Resolve<IProviderMappingDataProvider>();
            return OffProvider.DeleteProvider(ID);
        }

        public List<DC.Provider> GetProviderMappings(int tenantId)
        {
            IProviderMappingDataProvider OffProvider = DataProviderFactory.Resolve<IProviderMappingDataProvider>();
            return OffProvider.GetProviderMappings(tenantId);
        }

        public DC.Provider UpdateProvider(DC.Provider OfficeProfile, int employeeId, int tenantId)
        {
            IProviderMappingDataProvider OffProvider = DataProviderFactory.Resolve<IProviderMappingDataProvider>();
            return OffProvider.UpdateProvider(OfficeProfile,employeeId,tenantId);
        }

        public List<DC.ApplicationMappingDTO> GetApplicationsList()
        {
            IProviderMappingDataProvider officeProvider = DataProviderFactory.Resolve<IProviderMappingDataProvider>();
            return officeProvider.GetApplicationsList();
        }

        public DC.Provider[] AddImportData(DC.Provider[] values, int tenantId, int userId)
        {
            IProviderMappingDataProvider officeProvider = DataProviderFactory.Resolve<IProviderMappingDataProvider>();
            return officeProvider.AddImportData(values, tenantId, userId);
        }

        public int ConfirmDelete(int ID)
        {
            IProviderMappingDataProvider OffProvider = DataProviderFactory.Resolve<IProviderMappingDataProvider>();
            return OffProvider.ConfirmDelete(ID);
        }
        public DC.Provider GetProviderDetailsByID(int tenantId, int providerId)
        {
            IProviderMappingDataProvider OffProvider = DataProviderFactory.Resolve<IProviderMappingDataProvider>();
            return OffProvider.GetProviderDetailsByID(providerId);
        }
    }
}
