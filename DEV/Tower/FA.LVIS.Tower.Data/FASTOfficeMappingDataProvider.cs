using FA.LVIS.Tower.Core;
using FA.LVIS.Tower.Data.DBEntities;
using FA.LVIS.Tower.DataContracts;
using FA.LVIS.Tower.FASTProcessing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FA.LVIS.Tower.Data
{
    public class FASTOfficeMappingDataProvider : Core.DataProviderBase, IFASTOfficeMappingDataProvider
    {
        public FASTOfficeMap AddFASTOffice(FASTOfficeMap FASTOfficeMap, int userId)
        {
            TerminalDBEntities.Entities dbContext = new TerminalDBEntities.Entities();
            TerminalDBEntities.FASTOfficeMap AddFASTOffice = new TerminalDBEntities.FASTOfficeMap();

            AddFASTOffice.ProviderId = FASTOfficeMap.ProviderId;
            AddFASTOffice.RegionId = FASTOfficeMap.RegionId;
            AddFASTOffice.EscrowRegionId = FASTOfficeMap.EscrowRegionId;
            AddFASTOffice.TitleOfficeId = FASTOfficeMap.TitleOfficeId;
            AddFASTOffice.EscrowOfficeId = FASTOfficeMap.EscrowOfficeId;
            AddFASTOffice.FASTOfficeMapDesc = FASTOfficeMap.FASTOfficeMapDesc;
            AddFASTOffice.LocationId = FASTOfficeMap.locationId;
            AddFASTOffice.EscrowOfficerCode = FASTOfficeMap.EscrowOfficerCode;
            AddFASTOffice.TitleOfficerCode = FASTOfficeMap.TitleOfficerCode;
            AddFASTOffice.CustomerId = FASTOfficeMap.CustomerId;
            AddFASTOffice.ContactId = ((FASTOfficeMap.Contactid.HasValue && FASTOfficeMap.Contactid.Value > 0) ? FASTOfficeMap.Contactid.Value : (int?)null);
            AddFASTOffice.TokenTypeCodeId = FASTOfficeMap.TokenTypeCodeId;
            AddFASTOffice.CreatedDate = DateTime.Now;
            AddFASTOffice.LastModifiedDate = DateTime.Now;
            AddFASTOffice.CreatedById = userId;
            AddFASTOffice.LastModifiedById = userId;
            AddFASTOffice.EscrowAssistantCode = FASTOfficeMap.EscrowAssistantCode;
            dbContext.FASTOfficeMaps.Add(AddFASTOffice);
            int Success = AuditLogHelper.SaveChanges(dbContext);

            if (Success == 1)
            {
                FASTOfficeMap.FASTOfficeMapId = AddFASTOffice.FASTOfficeMapId;
                //FASTOfficeMap.ExternalId = dbContext.Providers.Where(v => v.ProviderId == FASTOfficeMap.ProviderId).Select(v => v.ExternalId).FirstOrDefault();
                FASTOfficeMap.Region = dbContext.FASTRegions.Where(v => v.RegionID == FASTOfficeMap.RegionId).Select(v => v.Name + " (" + v.RegionID + ")").FirstOrDefault();
                FASTOfficeMap.TitleOffice = dbContext.FASTOffices.Where(v => v.OfficeID == FASTOfficeMap.TitleOfficeId).Select(v => v.OfficeName + " (" + v.OfficeID + ")").FirstOrDefault();
                FASTOfficeMap.EscrowOffice = dbContext.FASTOffices.Where(v => v.OfficeID == FASTOfficeMap.EscrowOfficeId).Select(v => v.OfficeName + " (" + v.OfficeID + ")").FirstOrDefault();
                FASTOfficeMap.EscrowOfficerCode = FASTOfficeMap.EscrowOfficerCode;
                FASTOfficeMap.TitleOfficerCode = FASTOfficeMap.TitleOfficerCode;

                FASTOfficeMap.Location = dbContext.Locations.Where(v => v.LocationId == FASTOfficeMap.locationId).Select(v => v.LocationName + "(" + v.LocationId + ")").FirstOrDefault();

                FASTOfficeMap.FASTOfficeMapDesc = dbContext.FASTOfficeMaps.Where(v => v.FASTOfficeMapId == FASTOfficeMap.FASTOfficeMapId).Select(v => v.FASTOfficeMapDesc).FirstOrDefault();
                FASTOfficeMap.Tenant = dbContext.Providers.Where(v => v.ProviderId == FASTOfficeMap.ProviderId).Select(v => v.Tenant.TenantName).FirstOrDefault();
                FASTOfficeMap.CustomerId = FASTOfficeMap.CustomerId;
                FASTOfficeMap.Contactid = ((FASTOfficeMap.Contactid.HasValue && FASTOfficeMap.Contactid.Value > 0) ? FASTOfficeMap.Contactid.Value : (int?)null);
                FASTOfficeMap.ExternalApplicationID = dbContext.Providers.Where(v => v.ProviderId == FASTOfficeMap.ProviderId).Select(v => v.ExternalApplicationId.Value).FirstOrDefault();

                if (FASTOfficeMap.LocationCondition != null)
                {
                    foreach (var item in FASTOfficeMap.LocationCondition)
                    {
                        if (item.PreferenceState != null)
                        {
                            TerminalDBEntities.OfficeLocationCondition LocationDet = new TerminalDBEntities.OfficeLocationCondition();
                            LocationDet.FASTOfficeMapId = FASTOfficeMap.FASTOfficeMapId;
                            LocationDet.ConditionTypeCodeId = (int)Conditions.State;
                            LocationDet.ConditionValue = item.PreferenceState.StateCodes;

                            LocationDet.CreatedDate = DateTime.Now;
                            LocationDet.LastModifiedDate = DateTime.Now;
                            LocationDet.CreatedById = userId;
                            LocationDet.LastModifiedById = userId;

                            dbContext.OfficeLocationConditions.Add(LocationDet);
                            int parentLocation = AuditLogHelper.SaveChanges(dbContext);

                            if (parentLocation > 0 && item.PreferenceCounty.countyFIPS != "0")
                            {
                                TerminalDBEntities.OfficeLocationCondition LocationDetCounty = new TerminalDBEntities.OfficeLocationCondition();
                                LocationDetCounty.FASTOfficeMapId = FASTOfficeMap.FASTOfficeMapId;
                                LocationDetCounty.ConditionTypeCodeId = (int)Conditions.county;
                                LocationDetCounty.ConditionValue = item.PreferenceCounty.county;
                                LocationDetCounty.ParentLocationConditionId = LocationDet.OfficeLocationConditionId;
                                LocationDetCounty.CreatedDate = DateTime.Now;
                                LocationDetCounty.LastModifiedDate = DateTime.Now;
                                LocationDetCounty.LastModifiedById = userId;
                                LocationDetCounty.CreatedById = userId;

                                dbContext.OfficeLocationConditions.Add(LocationDetCounty);
                                AuditLogHelper.SaveChanges(dbContext);
                            }
                        }
                    }
                }
            }

            return FASTOfficeMap;
        }

        public List<FASTOfficeMap> GetFASTOfficeMappings(int tenantId)
        {
            TerminalDBEntities.Entities dbContext = new TerminalDBEntities.Entities();
            List<DataContracts.FASTOfficeMap> OfficeMappings = new List<DataContracts.FASTOfficeMap>();
            if (dbContext.FASTOfficeMaps.Count() > 0)
            {
                OfficeMappings = dbContext.FASTOfficeMaps
                   //.Where(se => se.Provider.TenantId == iTenantid)
                   .Select(x => new FASTOfficeMap
                   {
                       FASTOfficeMapId = x.FASTOfficeMapId,
                       ExternalId = dbContext.Providers.Where(v => v.ProviderId == x.ProviderId).Select(v => v.ExternalId).FirstOrDefault(),
                       ApplicationName = dbContext.Applications.Where(v => v.ApplicationId == x.Provider.ExternalApplicationId).Select(v => v.ApplicationName).FirstOrDefault(),
                       Region = dbContext.FASTRegions.Where(v => v.RegionID == x.RegionId).Select(v => v.Name + " (" + v.RegionID + ")").FirstOrDefault(),
                       TitleOffice = dbContext.FASTOffices.Where(v => v.OfficeID == x.TitleOfficeId).Select(v => v.OfficeName + " (" + v.OfficeID + ")").FirstOrDefault(),
                       EscrowOffice = dbContext.FASTOffices.Where(v => v.OfficeID == x.EscrowOfficeId).Select(v => v.OfficeName + " (" + v.OfficeID + ")").FirstOrDefault(),
                       FASTOfficeMapDesc = x.FASTOfficeMapDesc,
                       ProviderId = x.ProviderId,
                       ProviderName = dbContext.Providers.Where(v => v.ProviderId == x.ProviderId).Select(v => v.ProviderName).FirstOrDefault(),
                       ExternalApplicationID = dbContext.Providers.Where(v => v.ProviderId == x.ProviderId).Select(v => v.ExternalApplicationId.Value).FirstOrDefault(),
                       RegionId = x.RegionId,
                       EscrowRegionId = x.EscrowRegionId,
                       TitleOfficeId = x.TitleOfficeId.Value,
                       EscrowOfficeId = x.EscrowOfficeId.Value,
                       locationId = x.LocationId.Value,
                       Location = dbContext.Locations.Where(v => v.LocationId == (x.LocationId.HasValue ? x.LocationId.Value : 0) && v.TenantId == x.Provider.TenantId).Select(v => v.LocationName + "(" + v.LocationId + ")").FirstOrDefault(),
                       TenantId = x.Provider.TenantId,
                       Tenant = x.Provider.Tenant.TenantName,
                       EscrowOfficer = x.EscrowOfficerCode,
                       TitleOfficer = x.TitleOfficerCode,
                       EscrowOfficerCode = x.EscrowOfficerCode,
                       TitleOfficerCode = x.TitleOfficerCode,
                       EscrowAssistantCode = x.EscrowAssistantCode,
                       CustomerId = x.CustomerId,
                       Contactid = x.ContactId != null ? x.ContactId.Value : 0
                   }).ToList();
            }

            if (OfficeMappings.Count() > 0 && tenantId != (int)TerminalDBEntities.TenantIdEnum.LVIS)
            {
                OfficeMappings = OfficeMappings
                    .Where(sel => (from s in dbContext.Providers where s.ProviderId == sel.ProviderId && s.TenantId == tenantId select s.ProviderId).Contains(sel.ProviderId)).ToList();
            }

            return OfficeMappings;
        }

        public FASTOfficeMap GetFASTOfficeDetailsByID(int fastOfficeMapId)
        {
            TerminalDBEntities.Entities dbContext = new TerminalDBEntities.Entities();
            FASTOfficeMap OfficeMapping = new FASTOfficeMap();

            if (dbContext.FASTOfficeMaps.Count() > 0)
            {
                OfficeMapping = dbContext.FASTOfficeMaps
                   .Where(se => se.FASTOfficeMapId == fastOfficeMapId)
                   .Select(x => new FASTOfficeMap
                   {
                       FASTOfficeMapId = x.FASTOfficeMapId,
                       ExternalId = dbContext.Providers.Where(v => v.ProviderId == x.ProviderId).Select(v => v.ExternalId).FirstOrDefault(),
                       Region = dbContext.FASTRegions.Where(v => v.RegionID == x.RegionId).Select(v => v.Name + " (" + v.RegionID + ")").FirstOrDefault(),
                       TitleOffice = dbContext.FASTOffices.Where(v => v.OfficeID == x.TitleOfficeId).Select(v => v.OfficeName + " (" + v.OfficeID + ")").FirstOrDefault(),
                       EscrowOffice = dbContext.FASTOffices.Where(v => v.OfficeID == x.EscrowOfficeId).Select(v => v.OfficeName + " (" + v.OfficeID + ")").FirstOrDefault(),
                       FASTOfficeMapDesc = x.FASTOfficeMapDesc,
                       ProviderId = x.ProviderId,
                       ExternalApplicationID = dbContext.Providers.Where(v => v.ProviderId == x.ProviderId).Select(v => v.ExternalApplicationId.Value).FirstOrDefault(),
                       ProviderName = dbContext.Providers.Where(v => v.ProviderId == x.ProviderId).Select(v => v.ProviderName).FirstOrDefault(),
                       RegionId = x.RegionId,
                       EscrowRegionId = x.EscrowRegionId,
                       TitleOfficeId = x.TitleOfficeId.Value,
                       EscrowOfficeId = x.EscrowOfficeId.Value,
                       locationId = x.LocationId.Value,
                       Location = dbContext.Locations.Where(v => v.LocationId == (x.LocationId.HasValue ? x.LocationId.Value : 0) && v.TenantId == x.Provider.TenantId).Select(v => v.LocationName + "(" + v.LocationId + ")").FirstOrDefault(),
                       TenantId = x.Provider.TenantId,
                       Tenant = x.Provider.Tenant.TenantName,
                       EscrowOfficerCode = x.EscrowOfficerCode,
                       TitleOfficerCode = x.TitleOfficerCode,
                       EscrowAssistantCode = x.EscrowAssistantCode,
                       CustomerId = x.CustomerId,
                       Contactid = x.ContactId != null ? x.ContactId.Value : 0,
                       TokenTypeCodeId = x.TokenTypeCodeId
                   }).FirstOrDefault();
            }

            if (OfficeMapping.FASTOfficeMapId > 0)
            {

                OfficeMapping.LocationCondition = new List<ConditionPreferenceDTO>();
                var stateConditions = dbContext.OfficeLocationConditions.Where(se => se.FASTOfficeMapId == OfficeMapping.FASTOfficeMapId && se.ParentLocationConditionId == null && se.ConditionTypeCodeId == (int)Conditions.State);

                foreach (var item in stateConditions)
                {
                    StateMappingDTO PreferenceState = new StateMappingDTO();
                    PreferenceState = FIPSUtilities.GetStatesList().Where(se => se.StateCodes == item.ConditionValue).FirstOrDefault();

                    var counties = dbContext.OfficeLocationConditions.Where(se => se.FASTOfficeMapId == OfficeMapping.FASTOfficeMapId && se.ParentLocationConditionId == item.OfficeLocationConditionId && se.ConditionTypeCodeId == (int)Conditions.county);

                    if (counties.Count() == 0)
                    {
                        CountyMappingDTO preferenceCounty = FIPSUtilities.GetCountyList(PreferenceState.StateFIPS).Where(se => se.county == "ALL").FirstOrDefault();
                        OfficeMapping.LocationCondition.Add(new ConditionPreferenceDTO { PreferenceState = PreferenceState, PreferenceCounty = preferenceCounty });
                        continue;
                    }

                    foreach (var child in counties)
                    {
                        CountyMappingDTO preferenceCounty = FIPSUtilities.GetCountyList(PreferenceState.StateFIPS).Where(se => se.county == child.ConditionValue).FirstOrDefault();

                        OfficeMapping.LocationCondition.Add(new ConditionPreferenceDTO { PreferenceState = PreferenceState, PreferenceCounty = preferenceCounty });
                    }
                }
            }

            return OfficeMapping;
        }

        public List<FASTOfficeMap> GetFASTOfficeMappingsprovider(int tenantId, int providerId)
        {
            TerminalDBEntities.Entities dbContext = new TerminalDBEntities.Entities();
            List<FASTOfficeMap> OfficeMappings = new List<FASTOfficeMap>();
            if (dbContext.FASTOfficeMaps.Count() > 0)
            {
                OfficeMappings = dbContext.FASTOfficeMaps
                    .Where(se => se.ProviderId == providerId)
                   .Select(x => new FASTOfficeMap
                   {
                       FASTOfficeMapId = x.FASTOfficeMapId,
                       ExternalId = dbContext.Providers.Where(v => v.ProviderId == x.ProviderId).Select(v => v.ExternalId).FirstOrDefault(),
                       Region = dbContext.FASTRegions.Where(v => v.RegionID == x.RegionId).Select(v => v.Name + " (" + v.RegionID + ")").FirstOrDefault(),
                       TitleOffice = dbContext.FASTOffices.Where(v => v.OfficeID == x.TitleOfficeId).Select(v => v.OfficeName + " (" + v.OfficeID + ")").FirstOrDefault(),
                       EscrowOffice = dbContext.FASTOffices.Where(v => v.OfficeID == x.EscrowOfficeId).Select(v => v.OfficeName + " (" + v.OfficeID + ")").FirstOrDefault(),
                       FASTOfficeMapDesc = x.FASTOfficeMapDesc,
                       ProviderId = x.ProviderId,
                       ProviderName = dbContext.Providers.Where(v => v.ProviderId == x.ProviderId).Select(v => v.ProviderName).FirstOrDefault(),
                       ExternalApplicationID = dbContext.Providers.Where(v => v.ProviderId == x.ProviderId).Select(v => v.ExternalApplicationId.Value).FirstOrDefault(),
                       RegionId = x.RegionId,
                       TitleOfficeId = x.TitleOfficeId.Value,
                       EscrowOfficeId = x.EscrowOfficeId.Value,
                       locationId = x.LocationId.HasValue ? x.LocationId : 0,
                       Location = dbContext.Locations.Where(v => v.LocationId == (x.LocationId.HasValue ? x.LocationId.Value : 0) && v.TenantId == x.Provider.TenantId).Select(v => v.LocationName + "(" + v.LocationId + ")").FirstOrDefault(),
                       TenantId = x.Provider.TenantId,
                       Tenant = x.Provider.Tenant.TenantName,
                       EscrowOfficer = x.EscrowOfficerCode,
                       TitleOfficer = x.TitleOfficerCode,
                       EscrowOfficerCode = !string.IsNullOrEmpty(x.EscrowOfficerCode) ? x.EscrowOfficerCode : string.Empty,
                       TitleOfficerCode = !string.IsNullOrEmpty(x.TitleOfficerCode) ? x.TitleOfficerCode : string.Empty,
                       EscrowAssistantCode = !string.IsNullOrEmpty(x.EscrowAssistantCode) ? x.EscrowAssistantCode : string.Empty,
                       TokenTypeCodeId = x.TokenTypeCodeId
                   }).ToList();
            }

            if (OfficeMappings.Count() > 0 && tenantId != (int)TerminalDBEntities.TenantIdEnum.LVIS)
            {
                OfficeMappings = OfficeMappings
                    .Where(sel => (from s in dbContext.Providers where s.ProviderId == sel.ProviderId && s.TenantId == tenantId select s.ProviderId).Contains(sel.ProviderId)).ToList();
            }


            return OfficeMappings;
        }

        public FASTOfficeMap UpdateFASTOffice(FASTOfficeMap FASTOfficeMap, int userId)
        {
            using (var dbContext = new TerminalDBEntities.Entities())
            {
                var FASTOfficeMapToUpdate = (from x in dbContext.FASTOfficeMaps
                                             where x.FASTOfficeMapId == FASTOfficeMap.FASTOfficeMapId
                                             select x).FirstOrDefault();

                if (FASTOfficeMapToUpdate != null)
                {
                    FASTOfficeMapToUpdate.LastModifiedDate = DateTime.Now;
                    FASTOfficeMapToUpdate.LastModifiedById = userId;
                    FASTOfficeMapToUpdate.ProviderId = FASTOfficeMap.ProviderId;
                    FASTOfficeMapToUpdate.RegionId = FASTOfficeMap.RegionId;
                    FASTOfficeMapToUpdate.EscrowRegionId = FASTOfficeMap.EscrowRegionId;
                    FASTOfficeMapToUpdate.TitleOfficeId = FASTOfficeMap.TitleOfficeId;
                    FASTOfficeMapToUpdate.LocationId = FASTOfficeMap.locationId;
                    FASTOfficeMapToUpdate.EscrowOfficeId = FASTOfficeMap.EscrowOfficeId;
                    FASTOfficeMapToUpdate.EscrowOfficerCode = FASTOfficeMap.EscrowOfficerCode;
                    FASTOfficeMapToUpdate.TitleOfficerCode = FASTOfficeMap.TitleOfficerCode;
                    FASTOfficeMapToUpdate.TokenTypeCodeId = FASTOfficeMap.TokenTypeCodeId;
                    FASTOfficeMapToUpdate.EscrowAssistantCode = FASTOfficeMap.EscrowAssistantCode;
                    FASTOfficeMapToUpdate.FASTOfficeMapDesc = FASTOfficeMap.FASTOfficeMapDesc;
                    FASTOfficeMapToUpdate.CustomerId = FASTOfficeMap.CustomerId;
                    FASTOfficeMapToUpdate.ContactId = ((FASTOfficeMap.Contactid.HasValue && FASTOfficeMap.Contactid.Value > 0) ? FASTOfficeMap.Contactid.Value : (int?)null);

                    dbContext.Entry(FASTOfficeMapToUpdate).State = System.Data.Entity.EntityState.Modified;

                    int Success = AuditLogHelper.SaveChanges(dbContext);

                    if (Success == 1)
                    {
                        FASTOfficeMap.ExternalId = dbContext.Providers.Where(v => v.ProviderId == FASTOfficeMap.ProviderId).Select(v => v.ExternalId).FirstOrDefault();
                        FASTOfficeMap.Region = dbContext.FASTRegions.Where(v => v.RegionID == FASTOfficeMap.RegionId).Select(v => v.Name + " (" + v.RegionID + ")").FirstOrDefault();
                        FASTOfficeMap.TitleOffice = dbContext.FASTOffices.Where(v => v.OfficeID == FASTOfficeMap.TitleOfficeId).Select(v => v.OfficeName + " (" + v.OfficeID + ")").FirstOrDefault();
                        FASTOfficeMap.EscrowOffice = dbContext.FASTOffices.Where(v => v.OfficeID == FASTOfficeMap.EscrowOfficeId).Select(v => v.OfficeName + " (" + v.OfficeID + ")").FirstOrDefault();
                        FASTOfficeMap.Location = dbContext.Locations.Where(v => v.LocationId == FASTOfficeMap.locationId).Select(v => v.LocationName + "(" + v.LocationId + ")").FirstOrDefault();
                        FASTOfficeMap.FASTOfficeMapDesc = dbContext.FASTOfficeMaps.Where(v => v.FASTOfficeMapId == FASTOfficeMap.FASTOfficeMapId).Select(v => v.FASTOfficeMapDesc).FirstOrDefault();
                        //FASTOfficeMap.TitleOfficerCode = FASTOfficeMap.TitleOfficerCode?.ToUpper();
                        //FASTOfficeMap.EscrowOfficerCode = FASTOfficeMap.EscrowOfficerCode?.ToUpper();
                        FASTOfficeMap.ExternalApplicationID = dbContext.Providers.Where(v => v.ProviderId == FASTOfficeMap.ProviderId).Select(v => v.ExternalApplicationId.Value).FirstOrDefault();

                        if (FASTOfficeMapToUpdate.OfficeLocationConditions != null && FASTOfficeMapToUpdate.OfficeLocationConditions.Count >= 0)
                        {
                            IEnumerable<TerminalDBEntities.OfficeLocationCondition> conditionstoupdate = dbContext.OfficeLocationConditions
                                     .RemoveRange(dbContext.OfficeLocationConditions
                                     .Where(se => (se.FASTOfficeMapId == FASTOfficeMapToUpdate.FASTOfficeMapId)));

                            AuditLogHelper.SaveChanges(dbContext);

                            foreach (var item in FASTOfficeMap.LocationCondition)
                            {
                                if (item.PreferenceState != null)
                                {
                                    TerminalDBEntities.OfficeLocationCondition LocationDet = new TerminalDBEntities.OfficeLocationCondition();
                                    LocationDet.FASTOfficeMapId = FASTOfficeMapToUpdate.FASTOfficeMapId;
                                    LocationDet.ConditionTypeCodeId = (int)Conditions.State;
                                    LocationDet.ConditionValue = item.PreferenceState.StateCodes;

                                    LocationDet.CreatedDate = DateTime.Now;
                                    LocationDet.LastModifiedDate = DateTime.Now;
                                    LocationDet.CreatedById = userId;

                                    dbContext.OfficeLocationConditions.Add(LocationDet);
                                    int parentLocation = AuditLogHelper.SaveChanges(dbContext);

                                    if (parentLocation > 0 && item.PreferenceCounty.countyFIPS != "0")
                                    {
                                        TerminalDBEntities.OfficeLocationCondition LocationDetCounty = new TerminalDBEntities.OfficeLocationCondition();
                                        LocationDetCounty.FASTOfficeMapId = FASTOfficeMap.FASTOfficeMapId;
                                        LocationDetCounty.ConditionTypeCodeId = (int)Conditions.county;
                                        LocationDetCounty.ConditionValue = item.PreferenceCounty.county;
                                        LocationDetCounty.ParentLocationConditionId = LocationDet.OfficeLocationConditionId;
                                        LocationDetCounty.CreatedDate = DateTime.Now;
                                        LocationDetCounty.LastModifiedDate = DateTime.Now;
                                        LocationDetCounty.LastModifiedById = userId;
                                        LocationDetCounty.CreatedById = userId;

                                        dbContext.OfficeLocationConditions.Add(LocationDetCounty);
                                        AuditLogHelper.SaveChanges(dbContext);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return FASTOfficeMap;
        }

        public List<Provider> GetProviderList(int tenantId)
        {
            TerminalDBEntities.Entities dbContext = new TerminalDBEntities.Entities();

            //dont populate providers which are already assigned witht Office Map
            //var distinctProvider = dbContext.Providers
            //    .Where(sel => sel.TenantId == tenantId && !(from s in dbContext.FASTOfficeMaps
            //                                                select s.ProviderId).Contains(sel.ProviderId)
            //                                           && sel.IsUseRuleEngine == false)

            var distinctProvider = dbContext.Providers
                .Where(sel => sel.TenantId == tenantId && sel.IsUseRuleEngine == false)

                .Select(se => new DataContracts.Provider()
                {
                    ProviderID = se.ProviderId,
                    ProviderName = se.ProviderName != null ? se.ProviderName + "(" + se.ExternalId + ")" : se.ProviderId + "(" + se.ExternalId + ")",
                    ExternalID = se.ExternalId,
                    TenantId = se.TenantId,
                    Tenant = se.TenantId != 0 ? se.Tenant.TenantName : string.Empty,
                    ExternalApplicationID = se.ExternalApplicationId

                });
            return distinctProvider.DistinctBy(Sm => Sm.ProviderID).ToList();
        }

        public string GetExternalIdByProviderId(int tenantId, int providerId)
        {
            TerminalDBEntities.Entities dbContext = new TerminalDBEntities.Entities();
            var distinctProvider = dbContext.Providers
                .Where(sel => sel.TenantId == tenantId && sel.ProviderId == providerId).Select(se => se.ExternalId).First();

            return distinctProvider;
        }

        public List<LocationsMappings> GetLocationList(int tenantId)
        {
            TerminalDBEntities.Entities dbContext = new TerminalDBEntities.Entities();

            var distinctLocation = dbContext.Locations
                .Select(se => new DataContracts.LocationsMappings()
                {
                    LocationId = se.LocationId,
                    LocationName = se.LocationName + "(" + se.LocationId + ")",
                    TenantId = se.TenantId
                });

            if (distinctLocation.Count() > 0 && tenantId != (int)TerminalDBEntities.TenantIdEnum.LVIS)
            {
                distinctLocation = distinctLocation.Where(sel => sel.TenantId == tenantId);
            }

            return distinctLocation.DistinctBy(Sm => Sm.LocationId).ToList();
        }

        public List<LocationsMappings> GetLocationsListByCustId(int customerId, int tenantId)
        {
            TerminalDBEntities.Entities dbContext = new TerminalDBEntities.Entities();

            var distinctLocation = dbContext.Locations
                                    .Where(sel => sel.CustomerId == customerId)
                                    .Select(se => new DataContracts.LocationsMappings()
                                    {
                                        LocationId = se.LocationId,
                                        LocationName = se.LocationName + "(" + se.LocationId + ")",
                                        TenantId = se.TenantId
                                    });

            if (distinctLocation.Count() > 0 && tenantId != (int)TerminalDBEntities.TenantIdEnum.LVIS)
            {
                distinctLocation = distinctLocation.Where(sel => sel.TenantId == tenantId);
            }

            return distinctLocation.DistinctBy(Sm => Sm.LocationId).ToList();
        }

        public List<FASTOfficeMap> GetOfficeDetails(string stateFipsId, string countyFipsid, bool titlePriority, int tenantId)
        {
            List<DataContracts.FASTOfficeMap> OfficeMappings = new List<DataContracts.FASTOfficeMap>();

            using (var dbContext = new TerminalDBEntities.Entities())
            {
                FIPSEntities FIPSContext = new FIPSEntities();
                string stateCode = FIPSContext.FIPSCodes.Where(x => x.StateFIPS == stateFipsId).Select(v => v.StateCode)?.FirstOrDefault();
                int countyFips = countyFipsid.ToIntDefEx();
                string countyName = (countyFips > 0 ? FIPSContext.FIPSCodes.AsEnumerable().Where(x => x.CountyFIPS.ToIntDefEx() == countyFips
                && x.StateFIPS == stateFipsId).Select(v => v.County).FirstOrDefault() : string.Empty);

                IEnumerable<TerminalDBEntities.FASTOfficeMap> Mappings;
                if (!string.IsNullOrEmpty(stateCode) && !string.IsNullOrEmpty(countyName))
                {
                    if (titlePriority)
                        Mappings = from s in dbContext.FASTOfficeMaps
                                   join sa in dbContext.FASTOffices on s.TitleOfficeId equals sa.OfficeID
                                   where sa.State.ToLower() == stateCode.ToLower() && sa.County.ToLower() == countyName.ToLower()
                                   select s;
                    else
                        Mappings = from s in dbContext.FASTOfficeMaps
                                   join sa in dbContext.FASTOffices on s.EscrowOfficeId equals sa.OfficeID
                                   where sa.State.ToLower() == stateCode.ToLower() && sa.County.ToLower() == countyName.ToLower()
                                   select s;
                }
                else if (!string.IsNullOrEmpty(stateCode))
                {
                    if (titlePriority)
                        Mappings = from s in dbContext.FASTOfficeMaps
                                   join sa in dbContext.FASTOffices on s.TitleOfficeId equals sa.OfficeID
                                   where sa.State.ToLower() == stateCode.ToLower()
                                   select s;
                    else
                        Mappings = from s in dbContext.FASTOfficeMaps
                                   join sa in dbContext.FASTOffices on s.EscrowOfficeId equals sa.OfficeID
                                   where sa.State.ToLower() == stateCode.ToLower()
                                   select s;
                }
                else
                {
                    if (tenantId == (int)TerminalDBEntities.TenantIdEnum.LVIS)
                    {
                        Mappings = from s in dbContext.FASTOfficeMaps
                                   select s;
                    }
                    else
                    {
                        Mappings = from s in dbContext.FASTOfficeMaps
                                   join sa in dbContext.Providers on s.ProviderId equals sa.ProviderId
                                   where sa.TenantId == tenantId
                                   select s;
                    }
                }

                //.Where(se => se.Provider.TenantId == iTenantid)
                OfficeMappings = Mappings.Select(x => new FASTOfficeMap
                {
                    FASTOfficeMapId = x.FASTOfficeMapId,
                    ExternalId = dbContext.Providers.Where(v => v.ProviderId == x.ProviderId).Select(v => v.ExternalId).FirstOrDefault(),
                    Region = dbContext.FASTRegions.Where(v => v.RegionID == x.RegionId).Select(v => v.Name + " (" + v.RegionID + ")").FirstOrDefault(),
                    TitleOffice = dbContext.FASTOffices.Where(v => v.OfficeID == x.TitleOfficeId).Select(v => v.OfficeName + " (" + v.OfficeID + ")").FirstOrDefault(),
                    EscrowOffice = dbContext.FASTOffices.Where(v => v.OfficeID == x.EscrowOfficeId).Select(v => v.OfficeName + " (" + v.OfficeID + ")").FirstOrDefault(),
                    FASTOfficeMapDesc = x.FASTOfficeMapDesc,
                    ProviderId = x.ProviderId,
                    ProviderName = dbContext.Providers.Where(v => v.ProviderId == x.ProviderId).Select(v => v.ProviderName).FirstOrDefault(),
                    RegionId = x.RegionId,
                    //EscrowRegionId = x.EscrowRegionId,
                    TitleOfficeId = x.TitleOfficeId.Value,
                    EscrowOfficeId = x.EscrowOfficeId.Value,
                    TitleOfficerCode = x.TitleOfficerCode,
                    EscrowOfficerCode = x.EscrowOfficerCode,
                    TitleOfficer = x.TitleOfficerCode,
                    EscrowOfficer = x.EscrowOfficerCode,
                    EscrowAssistantCode = x.EscrowAssistantCode,
                    TenantId = x.Provider.TenantId,
                    Tenant = x.Provider.Tenant.TenantName,
                    Location = dbContext.Locations.Where(v => v.LocationId == (x.LocationId.HasValue ? x.LocationId.Value : 0) && v.TenantId == x.Provider.TenantId).Select(v => v.LocationName + "(" + v.LocationId + ")").FirstOrDefault(),
                }).ToList();

                return OfficeMappings;
            }
        }

        public int ConfirmDeleteFASTOffice(int ID)
        {
            int success = 0;
            using (var dbContext = new TerminalDBEntities.Entities())
            {
                IEnumerable<TerminalDBEntities.OfficeLocationCondition> Conditions = dbContext.OfficeLocationConditions
                             .RemoveRange(dbContext.OfficeLocationConditions
                             .Where(se => (se.FASTOfficeMapId == ID)));

                var OfficemaptoDelete = (from Officemap in dbContext.FASTOfficeMaps
                                         where Officemap.FASTOfficeMapId == ID
                                         select Officemap);

                if (OfficemaptoDelete != null)
                {
                    dbContext.FASTOfficeMaps.RemoveRange(OfficemaptoDelete);
                    success = AuditLogHelper.SaveChanges(dbContext);
                }
            }
            return success;
        }

        public int DeleteFASTOffice(int ID)
        {
            using (var dbContext = new TerminalDBEntities.Entities())
            {
                var result = dbContext.GetDependancyRecordOutput(ID, "FASTOfficeMap").FirstOrDefault();

                if (result != null)
                {
                    return 0;
                }

                else
                {
                    return 1;
                }
            }
        }

        public List<UserProfile> GetTitleEscrowOfficers(int tenantId, int funcType, int officeId)
        {
            return GetOfficersList(officeId, funcType, tenantId);
        }

        public List<TypeCodeDTO> GetAuthenticationTokens(int tenantId)
        {
            TerminalDBEntities.Entities dbContext = new TerminalDBEntities.Entities();
            List<DataContracts.TypeCodeDTO> Typecodemap = new List<DataContracts.TypeCodeDTO>();
            if (dbContext.TypeCodes.Count() > 0)
            {
                Typecodemap = dbContext.TypeCodes
                    .Where(se => se.GroupTypeCode == 977)
                   .Select(x => new TypeCodeDTO
                   {
                       TypeCodeId = x.TypeCodeId,
                       TypeCodeDesc = x.TypeCodeDesc
                   }).ToList();
            }
            if (Typecodemap.Count() > 0 && tenantId != (int)TerminalDBEntities.TenantIdEnum.LVIS)
            {
                if (tenantId == (int)TerminalDBEntities.TenantIdEnum.MortgageServices)
                    Typecodemap = Typecodemap
                        .Where(sel => sel.TypeCodeDesc == "MortgageSolutions").ToList();
                else
                    Typecodemap.RemoveAll(sel => sel.TypeCodeDesc == "MortgageSolutions");
            }

            return Typecodemap;
        }

        //public List<UserProfile> GetTitleOfficers(int tenantId, int officeId)
        //{
        //    return GetOfficersList(officeId, 78);
        //}

        private List<UserProfile> GetOfficersList(int officeId, int funcType, int itenantid)
        {
            RunAccount impAccount = new RunAccount();
            impAccount.Tenantid = itenantid;
            EQFASTSearch FastSearchClient = new EQFASTSearch(impAccount);

            var userProfileList = new List<UserProfile>();

            var result = FastSearchClient.GetEmployeesByFunctionTypes(officeId, funcType);

            if (result != null && result.Count > 0)
            {
                foreach (var user in result)
                {
                    UserProfile profile = new UserProfile()
                    {
                        Employeeid = user.OfficerID.ToString(),
                        Name = user.OfficerName + " (" + user.OfficerCode + ")",
                        ID = user.OfficerCode,
                        IsActive = true
                    };

                    userProfileList.Add(profile);
                }
            }

            return userProfileList;
        }
    }
}
