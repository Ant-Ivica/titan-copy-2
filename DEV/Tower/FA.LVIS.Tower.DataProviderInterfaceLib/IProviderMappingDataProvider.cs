using FA.LVIS.Tower.Core;
using System.Collections.Generic;
using DC = FA.LVIS.Tower.DataContracts;

namespace FA.LVIS.Tower.Data
{

    public interface IProviderMappingDataProvider : IDataProviderBase
    {
        List<DC.Provider> GetProviderMappings(int tenantId);
        DC.Provider AddProvider(DC.Provider UserProfile,int employeeId,int tenantId);
        int DeleteProvider(int ID);

        int ConfirmDelete(int ID);

        DC.Provider UpdateProvider(DC.Provider UserProfile, int employeeId, int tenantId);
        List<DC.ApplicationMappingDTO> GetApplicationsList();
        DC.Provider[] AddImportData(DC.Provider[] values, int tenantId, int userId);
        DC.Provider GetProviderDetailsByID(int providerId);
    }
}
