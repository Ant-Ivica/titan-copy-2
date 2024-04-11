using System.Collections.Generic;
using DC = FA.LVIS.Tower.DataContracts;

namespace FA.LVIS.Tower.Services
{
    public interface IFASTOfficeMappingService : Core.IServiceBase
    {
        List<DC.TypeCodeDTO> GetAuthenticationTokens(int tenantId);

        List<DC.FASTOfficeMap> GetFASTOfficeMappings(int tenantId);

        List<DC.FASTOfficeMap> GetFASTOfficeMappingsprovider(int tenantId, int ProviderId);

        DC.FASTOfficeMap AddFASTOffice(DC.FASTOfficeMap UserProfile, int Employeeid);

        int DeleteFASTOffice(int ID);

        int ConfirmDeleteFASTOffice(int ID);

        DC.FASTOfficeMap UpdateFASTOffice(DC.FASTOfficeMap UserProfile, int Employeeid);

        //List<DC.ApplicationMappingDTO> GetApplicationsList();

        List<DC.Provider> GetProviderList(int TenantId);

        List<DC.LocationsMappings> GetLocationList(int tenantId);

        List<DC.LocationsMappings> GetLocationsListByCustId(int customerId, int tenantId);

        string GetExternalIdByProviderId(int tenantId, int providerId);

        List<DC.FASTOfficeMap> GetOfficeDetails(string stateFipsId, string countyFipsid, bool titlePriority, int tenantId);

        DC.FASTOfficeMap GetFASTOfficeDetailsByID(int tenantId, int fastOfficeMapId);

        List<DC.UserProfile> GetTitleEscrowOfficers(int officeId, int funcType, int tenantId);

        //List<DC.UserProfile> GetTitleOfficers(int officeId, int tenantId);
    }
}
