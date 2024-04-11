using System;
using System.Collections.Generic;
using System.Linq;
using FA.LVIS.Tower.Core;
using FA.LVIS.Tower.DataContracts;
using System.Security.Principal;
using System.Security.Claims;
using System.Threading;

namespace FA.LVIS.Tower.Data
{
    public class ProviderMappingDataProvider : Core.DataProviderBase, IProviderMappingDataProvider
    {
        int GetApplicationID(string AppName)
        {
            TerminalDBEntities.Entities dbContext = new TerminalDBEntities.Entities();

            return  dbContext.Applications.Where(se => se.ApplicationName.ToLower() == AppName.ToLower()).Select(sl => sl.ApplicationId).FirstOrDefault();
        }

        public Provider[] AddImportData(Provider[] values, int tenantId, int userId)
        {
            TerminalDBEntities.Entities dbContext = new TerminalDBEntities.Entities();
            foreach (Provider providervalue in values)
            {                               
                int ExtAppid= GetApplicationID(providervalue.ExternalApplication);
                int IntAppid= GetApplicationID(providervalue.InternalApplication);

                //Update Provider Details
                if (dbContext.Providers.Where(se => se.ExternalId == providervalue.ExternalID && se.ExternalApplicationId== ExtAppid && se.InternalApplicationId== IntAppid).Count() > 0)
                {
                    var updateEntity = dbContext.Providers.Where(se => se.ExternalId == providervalue.ExternalID).FirstOrDefault();
                    updateEntity.InternalApplicationId = IntAppid;
                    updateEntity.ExternalApplicationId = ExtAppid;
                    updateEntity.IsUseRuleEngine = (providervalue.UseRuleEngine == "True" ? true:false);
                    updateEntity.LastModifiedDate = DateTime.Now;
                    updateEntity.LastModifiedById = userId;
                    updateEntity.TenantId = tenantId;
                    dbContext.Entry(updateEntity).State = System.Data.Entity.EntityState.Modified;
                    AuditLogHelper.SaveChanges(dbContext);
                    //TerminalDBEntities.FASTOfficeMap AddpdateFastOffice = new TerminalDBEntities.FASTOfficeMap();
                    int ProviderId = updateEntity.ProviderId;

                    var AddpdateFastOffice = dbContext.FASTOfficeMaps.Where(sel => sel.ProviderId == ProviderId).FirstOrDefault();
                  
                    //Update FastMap Against Existing ProviderId
                    if (AddpdateFastOffice != null)
                    {
                        AddpdateFastOffice.RegionId = providervalue.RegionID;
                        AddpdateFastOffice.TitleOfficeId = providervalue.TitleOfficeID;
                        AddpdateFastOffice.EscrowOfficeId = providervalue.EscrowOfficeID;
                        AddpdateFastOffice.FASTOfficeMapDesc = providervalue.Description;
                        AddpdateFastOffice.CreatedDate = DateTime.Now;
                        AddpdateFastOffice.CreatedById = userId;
                        AddpdateFastOffice.LastModifiedDate = DateTime.Now;
                        AddpdateFastOffice.LastModifiedById = userId;

                        dbContext.Entry(AddpdateFastOffice).State = System.Data.Entity.EntityState.Modified;
                        AuditLogHelper.SaveChanges(dbContext);
                    }
                    //Add FastMap Against Existing  ProviderId
                    else
                    {
                        TerminalDBEntities.FASTOfficeMap AddFastOffice = new TerminalDBEntities.FASTOfficeMap();
                        AddFastOffice.ProviderId = ProviderId;
                        AddFastOffice.RegionId = providervalue.RegionID;
                        AddFastOffice.TitleOfficeId = providervalue.TitleOfficeID;
                        AddFastOffice.EscrowOfficeId = providervalue.EscrowOfficeID;
                        AddFastOffice.FASTOfficeMapDesc = providervalue.Description;
                        AddFastOffice.CreatedDate = DateTime.Now;
                        AddFastOffice.CreatedById = userId;
                        AddFastOffice.LastModifiedDate = DateTime.Now;
                        AddFastOffice.LastModifiedById = userId;

                        dbContext.FASTOfficeMaps.Add(AddFastOffice);
                        AuditLogHelper.SaveChanges(dbContext);
                    }                  
                }


                //Add Provider Details
                else
                {
                    TerminalDBEntities.Provider AddProvider = new TerminalDBEntities.Provider();
                    TerminalDBEntities.FASTOfficeMap AddFastOffice = new TerminalDBEntities.FASTOfficeMap();

                    AddProvider.ExternalId = providervalue.ExternalID;
                    AddProvider.ExternalApplicationId = ExtAppid;
                    AddProvider.InternalApplicationId = IntAppid;
                    AddProvider.IsUseRuleEngine = (providervalue.UseRuleEngine == "True" ? true : false); ;
                    AddProvider.TenantId = tenantId;
                    AddProvider.CreatedDate = DateTime.Now;
                    AddProvider.LastModifiedDate = DateTime.Now;
                    AddProvider.CreatedById = userId;
                    AddProvider.LastModifiedById = userId;

                    dbContext.Providers.Add(AddProvider);
                    AuditLogHelper.SaveChanges(dbContext);
                    int ProviderId = AddProvider.ProviderId;

                    //Add FastMap Against New Provider
                    AddFastOffice.ProviderId = ProviderId;
                    AddFastOffice.RegionId = providervalue.RegionID;
                    AddFastOffice.TitleOfficeId = providervalue.TitleOfficeID;
                    AddFastOffice.EscrowOfficeId = providervalue.EscrowOfficeID;
                    AddFastOffice.FASTOfficeMapDesc = providervalue.Description;
                    AddFastOffice.CreatedDate = DateTime.Now; ;
                    AddFastOffice.CreatedById = userId;
                    AddFastOffice.LastModifiedDate = DateTime.Now; ;
                    AddFastOffice.LastModifiedById = userId;

                    dbContext.FASTOfficeMaps.Add(AddFastOffice);
                    AuditLogHelper.SaveChanges(dbContext);
                }

            }

            //AuditLogHelper.SaveChanges(dbContext);
            return GetProviderMappings(tenantId).ToArray();
        }

        public Provider AddProvider(Provider Provider,int employeeId, int tenantId)
        {
            using (TerminalDBEntities.Entities dbContext = new TerminalDBEntities.Entities())
            {

                if (!string.IsNullOrEmpty(Provider.ProviderName))
                {

                    int DuplicateNamecount = (from provider in dbContext.Providers
                                              where provider.ProviderName.Replace(" ", "").ToLower() == Provider.ProviderName.Replace(" ", "").ToLower()
                                              select provider
                                          ).Count();

                    if (DuplicateNamecount > 0)
                        throw new Exception("Provider Name already exists");
                }

                TerminalDBEntities.Provider AddProvider = new TerminalDBEntities.Provider();
                TerminalDBEntities.ProviderLocationCondition AddProviderLocation = new TerminalDBEntities.ProviderLocationCondition();


                AddProvider.ExternalId = Provider.ExternalID;
                AddProvider.ExternalApplicationId = Provider.ExternalApplicationID;
                AddProvider.InternalApplicationId = Provider.InternalApplicationID;
                AddProvider.IsUseRuleEngine = (bool)Provider.IsUseRuleEngine;
                AddProvider.TenantId = tenantId;
                AddProvider.CreatedDate = DateTime.Now;
                AddProvider.LastModifiedDate = DateTime.Now;
                AddProvider.CreatedById = employeeId;
                AddProvider.LastModifiedById = employeeId;
                AddProvider.ProviderName = Provider.ProviderName;
                AddProvider.IsBindOnly = Provider.IsBindOnly;
                if (Provider.ExternalApplicationID == (int)TerminalDBEntities.ApplicationEnum.TitlePort)
                        AddProvider.ServiceProviderId = Provider.ServiceProviderId;
                dbContext.Providers.Add(AddProvider);
                int Success = AuditLogHelper.SaveChanges(dbContext);              

                if (Success == 1)
                {
                    Provider.ProviderID = AddProvider.ProviderId;

                    Provider.ExternalApplication =
                       dbContext.Applications.Where(v => v.ApplicationId == AddProvider.ExternalApplicationId)
                       .Select(v => v.ApplicationName).FirstOrDefault();
                    Provider.InternalApplication =
                         dbContext.Applications.Where(v => v.ApplicationId == AddProvider.InternalApplicationId)
                        .Select(v => v.ApplicationName).FirstOrDefault();

                    Provider.IsUseRuleEngine = dbContext.Providers.Where(v => v.ProviderId == AddProvider.ProviderId)
                          .Select(v => v.IsUseRuleEngine).FirstOrDefault();

                    Provider.UseRuleEngine = AddProvider.IsUseRuleEngine ? "Yes" : "No";

                    Provider.Tenant =
                        dbContext.Tenants.Where(t => t.TenantId == AddProvider.TenantId)
                        .Select(t => t.TenantName).FirstOrDefault();

                    if (Provider.LocationCondition != null) {
                        foreach (var item in Provider.LocationCondition) {
                            if (item.PreferenceState != null) {
                                AddProviderLocation.ProviderId = Provider.ProviderID;
                                AddProviderLocation.ConditionTypeCodeId = (int)Conditions.State;
                                AddProviderLocation.ConditionValue = item.PreferenceState.StateCodes;

                                AddProviderLocation.CreatedDate = DateTime.Now;
                                AddProviderLocation.LastModifiedDate = DateTime.Now;
                                AddProviderLocation.CreatedById = employeeId;
                                AddProviderLocation.LastModifiedById = employeeId;

                                dbContext.ProviderLocationConditions.Add(AddProviderLocation);
                                int parentLocation = AuditLogHelper.SaveChanges(dbContext);

                                if (parentLocation > 0 && item.PreferenceCounty.countyFIPS != "0") {
                                    TerminalDBEntities.ProviderLocationCondition AddProviderLocationDet = new TerminalDBEntities.ProviderLocationCondition();

                                    AddProviderLocationDet.ProviderId = Provider.ProviderID;
                                    AddProviderLocationDet.ConditionTypeCodeId = (int)Conditions.county;
                                    AddProviderLocationDet.ConditionValue = item.PreferenceCounty.county;
                                    AddProviderLocationDet.ParentLocationConditionId = AddProviderLocation.LocationConditionId;

                                    AddProviderLocationDet.CreatedDate = DateTime.Now;
                                    AddProviderLocationDet.LastModifiedDate = DateTime.Now;
                                    AddProviderLocationDet.LastModifiedById = employeeId;
                                    AddProviderLocationDet.CreatedById = employeeId;

                                    dbContext.ProviderLocationConditions.Add(AddProviderLocationDet);
                                    AuditLogHelper.SaveChanges(dbContext);

                                }

                            }
                        }
                    }
                    return Provider;
                }               
            }
            return Provider;
        }

        public int DeleteProvider(int ID)
        {
            using (var dbContext = new  TerminalDBEntities.Entities())
            {
                var result = dbContext.GetDependancyRecordOutput(ID, "Provider").FirstOrDefault();
                if (result != null && result != "ProviderLocationCondition")
                {
                    return 0;
                }
                else
                {
                    return 1;
                }
            }
        }

        public List<Provider> GetProviderMappings(int tenantId)
        {
            TerminalDBEntities.Entities dbContext = new TerminalDBEntities.Entities();
            List<DataContracts.Provider> ProviderMappings = new List<DataContracts.Provider>();

            if (dbContext.Providers.Count() > 0)
            {
                ProviderMappings = dbContext.Providers
                    //.Where(sel => sel.TenantId == tenantId)
                .Select(p => new DataContracts.Provider()
                {
                    ProviderID = p.ProviderId,
                    ExternalID = p.ExternalId,
                    ExternalApplicationID = p.ExternalApplicationId,
                    InternalApplicationID = p.InternalApplicationId,
                    ExternalApplication = dbContext.Applications.Where(v => v.ApplicationId == p.ExternalApplicationId)
                            .Select(v => v.ApplicationName).FirstOrDefault(),
                    InternalApplication = dbContext.Applications.Where(v => v.ApplicationId == p.InternalApplicationId)
                            .Select(v => v.ApplicationName).FirstOrDefault(),
                    IsUseRuleEngine = p.IsUseRuleEngine,
                    UseRuleEngine= p.IsUseRuleEngine ? "Yes":"No",
                    TenantId = p.TenantId,
                    Tenant=p.Tenant.TenantName,
                    ProviderName =p.ProviderName
                }).ToList();
            }

            if (ProviderMappings.Count() > 0 && tenantId != (int)TerminalDBEntities.TenantIdEnum.LVIS)
            {
                ProviderMappings = ProviderMappings
                    .Where(sel => sel.TenantId == tenantId).ToList();
            }

            return ProviderMappings;
        }        

        public Provider UpdateProvider(Provider Provider, int employeeId, int tenantId)
        {
            using (var dbContext = new TerminalDBEntities.Entities())
            {
                var updateProvider = (from provider in dbContext.Providers
                                      where provider.ProviderId == Provider.ProviderID
                                      select provider).FirstOrDefault();

                if (updateProvider != null )
                {
                    if (!string.IsNullOrEmpty(Provider.ProviderName))
                    {
                        var ProviderList = (from provider in dbContext.Providers
                                            where provider.ProviderName.Replace(" ", "").ToLower() == Provider.ProviderName.Replace(" ", "").ToLower()
                                            select provider
                                               );

                        if (ProviderList.Count() > 0)
                        {
                            if (!(ProviderList.Count() == 1 && ProviderList.FirstOrDefault().ProviderId == updateProvider.ProviderId))
                                throw new Exception("Provider Name already exists");
                        }
                    }
                    updateProvider.LastModifiedDate = DateTime.Now;
                    updateProvider.LastModifiedById = employeeId;
                    updateProvider.ExternalId = Provider.ExternalID;
                    updateProvider.ExternalApplicationId = Provider.ExternalApplicationID;
                    updateProvider.InternalApplicationId = Provider.InternalApplicationID;
                    updateProvider.IsUseRuleEngine = (bool)Provider.IsUseRuleEngine;
                    updateProvider.ProviderName = Provider.ProviderName;
                    updateProvider.IsBindOnly = Provider.IsBindOnly;
                    updateProvider.TenantId = Provider.TenantId;
                    if (Provider.ExternalApplicationID == (int)TerminalDBEntities.ApplicationEnum.TitlePort)
                        updateProvider.ServiceProviderId = Provider.ServiceProviderId;
                    dbContext.Entry(updateProvider).State = System.Data.Entity.EntityState.Modified;

                    int Success = AuditLogHelper.SaveChanges(dbContext);
                  
                    if (Success == 1)
                    {
                        Provider.ProviderID = updateProvider.ProviderId;

                        Provider.ExternalApplication =
                           dbContext.Applications.Where(v => v.ApplicationId == updateProvider.ExternalApplicationId)
                           .Select(v => v.ApplicationName).FirstOrDefault();
                        Provider.InternalApplication =
                             dbContext.Applications.Where(v => v.ApplicationId == updateProvider.InternalApplicationId)
                            .Select(v => v.ApplicationName).FirstOrDefault();
                        Provider.IsUseRuleEngine = dbContext.Providers.Where(v => v.ProviderId == updateProvider.ProviderId)
                            .Select(v => v.IsUseRuleEngine).FirstOrDefault();

                        Provider.UseRuleEngine = updateProvider.IsUseRuleEngine ? "Yes" : "No";

                        Provider.Tenant = dbContext.Tenants.Where(t => t.TenantId == updateProvider.TenantId)
                            .Select(t => t.TenantName).FirstOrDefault();

                        if (updateProvider.ProviderLocationConditions != null && updateProvider.ProviderLocationConditions.Count >= 0) {
                            IEnumerable<TerminalDBEntities.ProviderLocationCondition> conditionstoupdate = dbContext.ProviderLocationConditions
                                     .RemoveRange(dbContext.ProviderLocationConditions
                                     .Where(se => (se.ProviderId == updateProvider.ProviderId)));

                            AuditLogHelper.SaveChanges(dbContext);

                            foreach (var item in Provider.LocationCondition) {
                                if (item.PreferenceState != null) {
                                    TerminalDBEntities.ProviderLocationCondition ProviderLocationDet = new TerminalDBEntities.ProviderLocationCondition();
                                    ProviderLocationDet.ProviderId = updateProvider.ProviderId;
                                    ProviderLocationDet.ConditionTypeCodeId = (int)Conditions.State;
                                    ProviderLocationDet.ConditionValue = item.PreferenceState.StateCodes;

                                    ProviderLocationDet.CreatedDate = DateTime.Now;
                                    ProviderLocationDet.LastModifiedDate = DateTime.Now;
                                    ProviderLocationDet.CreatedById = employeeId;

                                    dbContext.ProviderLocationConditions.Add(ProviderLocationDet);
                                    int parentLocation = AuditLogHelper.SaveChanges(dbContext);

                                    if (parentLocation > 0 && item.PreferenceCounty.countyFIPS != "0") {
                                        TerminalDBEntities.ProviderLocationCondition ProviderLocationDetCounty = new TerminalDBEntities.ProviderLocationCondition();
                                        ProviderLocationDetCounty.ProviderId = Provider.ProviderID;
                                        ProviderLocationDetCounty.ConditionTypeCodeId = (int)Conditions.county;
                                        ProviderLocationDetCounty.ConditionValue = item.PreferenceCounty.county;
                                        ProviderLocationDetCounty.ParentLocationConditionId = ProviderLocationDet.LocationConditionId;
                                        ProviderLocationDetCounty.CreatedDate = DateTime.Now;
                                        ProviderLocationDetCounty.LastModifiedDate = DateTime.Now;
                                        ProviderLocationDetCounty.LastModifiedById = employeeId;
                                        ProviderLocationDetCounty.CreatedById = employeeId;

                                        dbContext.ProviderLocationConditions.Add(ProviderLocationDetCounty);
                                        AuditLogHelper.SaveChanges(dbContext);
                                    }


                                }

                            }


                        }


                        return Provider;
                    }                                     
                }
            }

            return Provider;
        }

        public List<ApplicationMappingDTO> GetApplicationsList()
        {
            TerminalDBEntities.Entities dbContext = new TerminalDBEntities.Entities();
            var distinctApplications = dbContext.Applications
                .Select(se => new DataContracts.ApplicationMappingDTO()
                {
                    ApplicationId = se.ApplicationId,
                    ApplicationName = se.ApplicationName,
                });
            return distinctApplications.DistinctBy(Sm => Sm.ApplicationId).OrderBy(x => x.ApplicationName).ToList();
        }

        public int ConfirmDelete(int ID)
        {
            if (DeleteProvider(ID) > 0)
            {
                using (var dbContext = new TerminalDBEntities.Entities())
                {
                    IEnumerable<TerminalDBEntities.ProviderLocationCondition> ProviderLocationinfo = dbContext.ProviderLocationConditions
                        .RemoveRange(dbContext.ProviderLocationConditions
                        .Where(se => (se.ProviderId == ID)));
                    AuditLogHelper.SaveChanges(dbContext);

                    IEnumerable<TerminalDBEntities.Provider> Providers = dbContext.Providers
                       .RemoveRange(dbContext.Providers
                       .Where(se => (se.ProviderId == ID)));

                    return AuditLogHelper.SaveChanges(dbContext);
                }
            }
            else
            {
                return 0;
            }
        }

        public Provider GetProviderDetailsByID(int providerId)
        {
            TerminalDBEntities.Entities dbContext = new TerminalDBEntities.Entities();
            Provider providerInfo = new Provider();

            if (dbContext.Providers.Count() > 0)
            {
                providerInfo = dbContext.Providers
                   .Where(se => se.ProviderId == providerId)
                   .Select(x => new Provider
                   {
                       ProviderID = x.ProviderId,
                       ExternalID = x.ExternalId,
                       ExternalApplicationID = x.ExternalApplicationId,
                       InternalApplicationID = x.InternalApplicationId,
                       ExternalApplication = dbContext.Applications.Where(v => v.ApplicationId == x.ExternalApplicationId)
                            .Select(v => v.ApplicationName).FirstOrDefault(),
                       InternalApplication = dbContext.Applications.Where(v => v.ApplicationId == x.InternalApplicationId)
                            .Select(v => v.ApplicationName).FirstOrDefault(),
                       IsUseRuleEngine = x.IsUseRuleEngine,
                       UseRuleEngine = x.IsUseRuleEngine ? "Yes" : "No",
                       TenantId = x.TenantId,
                       Tenant = x.Tenant.TenantName,
                       ProviderName = x.ProviderName,
                       IsBindOnly = x.IsBindOnly,
                       ServiceProviderId=x.ServiceProviderId != null ?(int)x.ServiceProviderId:0
                   }).FirstOrDefault();
            }

            if (providerInfo.ProviderID > 0)
            {

                providerInfo.LocationCondition = new List<ConditionPreferenceDTO>();
                var stateConditions = dbContext.ProviderLocationConditions.Where(se => se.ProviderId == providerInfo.ProviderID && se.ParentLocationConditionId == null && se.ConditionTypeCodeId == (int)Conditions.State);

                foreach (var item in stateConditions)
                {
                    StateMappingDTO PreferenceState = new StateMappingDTO();
                    PreferenceState = FIPSUtilities.GetStatesList().Where(se => se.StateCodes == item.ConditionValue).FirstOrDefault();

                    var counties = dbContext.ProviderLocationConditions.Where(se => se.ProviderId == providerInfo.ProviderID && se.ParentLocationConditionId == item.LocationConditionId && se.ConditionTypeCodeId == (int)Conditions.county);

                    if (counties.Count() == 0)
                    {
                        CountyMappingDTO preferenceCounty = FIPSUtilities.GetCountyList(PreferenceState.StateFIPS).Where(se => se.county == "ALL").FirstOrDefault();
                        providerInfo.LocationCondition.Add(new ConditionPreferenceDTO { PreferenceState = PreferenceState, PreferenceCounty = preferenceCounty });
                        continue;
                    }

                    foreach (var child in counties)
                    {
                        CountyMappingDTO preferenceCounty = FIPSUtilities.GetCountyList(PreferenceState.StateFIPS).Where(se => se.county == child.ConditionValue).FirstOrDefault();

                        providerInfo.LocationCondition.Add(new ConditionPreferenceDTO { PreferenceState = PreferenceState, PreferenceCounty = preferenceCounty });
                    }
                }
            }

            return providerInfo;
        }
    }
}
