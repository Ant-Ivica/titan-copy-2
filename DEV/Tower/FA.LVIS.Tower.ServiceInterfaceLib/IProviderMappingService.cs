using System.Collections.Generic;

using DC = FA.LVIS.Tower.DataContracts;

namespace FA.LVIS.Tower.Services
{
    public interface IProviderMappingService : Core.IServiceBase
    {
        List<DC.Provider> GetProviderMappings(int tenantId);
        DC.Provider AddProvider(DC.Provider UserProfile,int employeeId, int tenantId);
        int DeleteOffice(int ID);

        int ConfirmDelete(int ID);
        DC.Provider UpdateProvider(DC.Provider UserProfile, int employeeId, int tenantId);
        List<DC.ApplicationMappingDTO> GetApplicationsList();
        DC.Provider[] AddImportData(DC.Provider[] values, int tenantId, int userId);
        DC.Provider GetProviderDetailsByID(int tenantId, int providerId);
    }
}
