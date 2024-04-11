using FA.LVIS.Tower.Data;
using FA.LVIS.Tower.DataContracts;
using System.Collections.Generic;
using System;

namespace FA.LVIS.Tower.Services
{
    public class FASTOfficeMappingService : Core.ServiceBase, IFASTOfficeMappingService
    {
 
        public FASTOfficeMap AddFASTOffice(FASTOfficeMap FASTOfficeMap, int Employeeid)
        {
            IFASTOfficeMappingDataProvider OffProvider = DataProviderFactory.Resolve<IFASTOfficeMappingDataProvider>();
            return OffProvider.AddFASTOffice(FASTOfficeMap, Employeeid);
        }

        public int DeleteFASTOffice(int ID)
        {
            IFASTOfficeMappingDataProvider OffProvider = DataProviderFactory.Resolve<IFASTOfficeMappingDataProvider>();
            return OffProvider.DeleteFASTOffice(ID);
        }

        public List<FASTOfficeMap> GetFASTOfficeMappings(int tenantId)
        {
            IFASTOfficeMappingDataProvider OffProvider = DataProviderFactory.Resolve<IFASTOfficeMappingDataProvider>();
            return OffProvider.GetFASTOfficeMappings(tenantId);
        }


        public List<FASTOfficeMap> GetFASTOfficeMappingsprovider(int tenantId, int Providerid)
        {
            IFASTOfficeMappingDataProvider OffProvider = DataProviderFactory.Resolve<IFASTOfficeMappingDataProvider>();
            return OffProvider.GetFASTOfficeMappingsprovider(tenantId, Providerid);
        }

        public List<Provider> GetProviderList(int Tenantid)
        {
            IFASTOfficeMappingDataProvider OffProvider = DataProviderFactory.Resolve<IFASTOfficeMappingDataProvider>();
            return OffProvider.GetProviderList(Tenantid);
        }

        public List<LocationsMappings> GetLocationList(int Tenantid)
        {
            IFASTOfficeMappingDataProvider OffProvider = DataProviderFactory.Resolve<IFASTOfficeMappingDataProvider>();
            return OffProvider.GetLocationList(Tenantid);
        }

        public List<LocationsMappings> GetLocationsListByCustId(int customerId, int Tenantid)
        {
            IFASTOfficeMappingDataProvider OffProvider = DataProviderFactory.Resolve<IFASTOfficeMappingDataProvider>();
            return OffProvider.GetLocationsListByCustId(customerId, Tenantid);
        }

        public string GetExternalIdByProviderId(int tenantId, int providerId)
        {
            IFASTOfficeMappingDataProvider OffProvider = DataProviderFactory.Resolve<IFASTOfficeMappingDataProvider>();
            return OffProvider.GetExternalIdByProviderId(tenantId, providerId);
        }

        public FASTOfficeMap UpdateFASTOffice(FASTOfficeMap FASTOfficeMap, int Employeeid)
        {
            IFASTOfficeMappingDataProvider OffProvider = DataProviderFactory.Resolve<IFASTOfficeMappingDataProvider>();
            return OffProvider.UpdateFASTOffice(FASTOfficeMap, Employeeid);
        }

        public List<FASTOfficeMap> GetOfficeDetails(string stateFipsId, string countyFipsid, bool titlePriority, int tenantId)
        {
            IFASTOfficeMappingDataProvider OffProvider = DataProviderFactory.Resolve<IFASTOfficeMappingDataProvider>();
            return OffProvider.GetOfficeDetails(stateFipsId, countyFipsid, titlePriority, tenantId);
        }

        public int ConfirmDeleteFASTOffice(int ID)
        {
            IFASTOfficeMappingDataProvider OffProvider = DataProviderFactory.Resolve<IFASTOfficeMappingDataProvider>();
            return OffProvider.ConfirmDeleteFASTOffice(ID);
        }

        public FASTOfficeMap GetFASTOfficeDetailsByID(int tenantId, int fastOfficeMapId)
        {
            IFASTOfficeMappingDataProvider OffProvider = DataProviderFactory.Resolve<IFASTOfficeMappingDataProvider>();
            return OffProvider.GetFASTOfficeDetailsByID(fastOfficeMapId);
        }

        public List<UserProfile> GetTitleEscrowOfficers(int officeId, int funcType, int tenantId)
        {
            IFASTOfficeMappingDataProvider OffProvider = DataProviderFactory.Resolve<IFASTOfficeMappingDataProvider>();
            return OffProvider.GetTitleEscrowOfficers(tenantId, funcType, officeId);
        }

        public List<TypeCodeDTO> GetAuthenticationTokens(int tenantId)
        {
            IFASTOfficeMappingDataProvider OffProvider = DataProviderFactory.Resolve<IFASTOfficeMappingDataProvider>();
            return OffProvider.GetAuthenticationTokens(tenantId);
        }
    }
}
