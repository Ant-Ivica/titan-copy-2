using FA.LVIS.Tower.Core;
using FA.LVIS.Tower.Data.DBEntities;
using FA.LVIS.Tower.Data.TerminalDBEntities;
using FA.LVIS.Tower.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FA.LVIS.Tower.Data
{
    public class FASTFilePreferenceDataProvider : Core.DataProviderBase, IFASTFilePreferenceDataProvider
    {
        //Start-Fast File Preference Methods
        

        public List<FASTFilePreferenceDTO> GetFASTFilePreferencesDetails(int tenantId)
        {
            TerminalDBEntities.Entities dbContext = new TerminalDBEntities.Entities();
            List<DataContracts.FASTFilePreferenceDTO> FastfilePrefMappings = new List<DataContracts.FASTFilePreferenceDTO>();
            if (dbContext.FASTPreferenceMaps.Count() > 0)
            {
                List<FASTPreferenceMap> Details = new List<FASTPreferenceMap>();

                if (tenantId != (int)TerminalDBEntities.TenantIdEnum.LVIS)
                {
                    Details = dbContext.FASTPreferenceMaps
                        .Where(sel => sel.TenantId == tenantId).ToList();
                }
                else
                    Details = dbContext.FASTPreferenceMaps.ToList();

                FastfilePrefMappings = Details.AsEnumerable()
                   .Select(x => new FASTFilePreferenceDTO
                   {
                       FASTPreferenceMapID = x.FASTPreferenceMapId,
                       FASTPreferenceMapName = x.FASTPreferenceMapName,
                       ProgramTypeName = x.FASTProgramType?.ProgramTypeName,
                       SearchType = x.FASTSearchType?.SearchTypeDesc,
                       FASTProgramTypeId = (x.FASTProgramTypeId > 0 ? x.FASTProgramTypeId : 0),
                       FASTSearchTypeId = (x.FASTSearchTypeId.HasValue ? x.FASTSearchTypeId.Value : 0),
                       Location = x.Location != null ? x.Location.LocationName+"("+x.LocationId+")" : string.Empty,
                       LoanPurpose = x.TypeCode?.TypeCodeDesc,
                       Region = dbContext.FASTRegions.Where(v => v.RegionID == x.RegionId).Select(v => v.Name + " (" + v.RegionID + ")").FirstOrDefault(),
                       TenantId = x.TenantId,
                       Tenant = x.Tenant?.TenantName,
                       RegionId = dbContext.FASTRegions.Where(v => v.RegionID == x.RegionId).Select(v => v.RegionID).FirstOrDefault(),
                       CustomerId = x.CustomerId,
                       CustomerName = x.Location != null ? x.Location.Customer.CustomerName + "(" + x.CustomerId + ")" : string.Empty,
                   }).ToList();
            }

            return FastfilePrefMappings;
        }

        public FASTFilePreferenceDTO GetFASTFilePreferencesDetailsById(int tenantId, int FastPreferencemapid)
        {
            TerminalDBEntities.Entities dbContext = new TerminalDBEntities.Entities();
            DataContracts.FASTFilePreferenceDTO ReturnDet = new DataContracts.FASTFilePreferenceDTO();
            if (dbContext.FASTPreferenceMaps.Count() > 0)
            {
                ReturnDet = dbContext.FASTPreferenceMaps.Where(se => se.FASTPreferenceMapId == FastPreferencemapid).AsEnumerable()
                   .Select(x => new FASTFilePreferenceDTO
                   {
                       FASTPreferenceMapID = x.FASTPreferenceMapId,
                       FASTPreferenceMapName = x.FASTPreferenceMapName,
                       ProgramTypeName = x.FASTProgramType != null ? x.FASTProgramType.ProgramTypeName : string.Empty,
                       FASTProgramTypeId = x.FASTProgramTypeId,
                       SearchType = x.FASTSearchType?.SearchTypeDesc,
                       FASTSearchTypeId = x.FASTSearchTypeId != null ? x.FASTSearchTypeId.Value : 0,
                       Location = x.Location?.LocationName,
                       LoanPurpose = x.TypeCode?.TypeCodeDesc,
                       RegionId = dbContext.FASTRegions.Where(v => v.RegionID == x.RegionId).Select(v => v.RegionID).FirstOrDefault(),
                       LocationDet = x.Location != null ? new LocationsMappings
                       {
                           LocationId = x.Location.LocationId,
                           ExternalId = x.Location.ExternalId,
                           LocationName = x.Location.LocationName + "(" + x.Location.LocationId + ")",
                           CustomerId = x.Location.CustomerId.ToString(),
                           CustomerName = x.Location.Customer.CustomerName,
                           TenantId = x.Location.TenantId
                       } : null
                       ,
                       LoanPurposeDet = x.TypeCode != null ? new ExceptionStatus { ID = x.TypeCode.TypeCodeId, Name = x.TypeCode.TypeCodeDesc } : new ExceptionStatus(),
                       TenantId = x.TenantId,
                       Tenant = x.Tenant?.TenantName,
                       Region = dbContext.FASTRegions.Where(v => v.RegionID == x.RegionId).Select(v => v.Name + " (" + v.RegionID + ")").FirstOrDefault(),
                       CustomerId = x.CustomerId,
                       CustomerName = x.Location != null ? x.Location.Customer.CustomerName + "(" + x.CustomerId + ")" : string.Empty,
                       BusinessProgramTypes = x.FASTPreferenceBusinessPrograms != null ? x.FASTPreferenceBusinessPrograms.Select(se => new BusinessProgramTypeMappingDTO()
                       {
                           Id = se.FASTBusinessProgramTypeId,
                           Name = se.FASTBusinessProgramType.BusinessProgramName,
                           Ticked = true
                       }).ToList() : null,

                       FastProductTypes = x.FASTPreferenceProducts != null ? x.FASTPreferenceProducts.Select(se => new ProductTypeMappingDTO()
                       {
                           ProductTypecode = se.Product.ProductCode,
                           ProductTypeId = se.ProductId,
                           ProductTypedesc = se.Product.ProductName,
                           Ticked = true
                       }).ToList() : null

                   }).FirstOrDefault();
            }

            if (ReturnDet.FASTPreferenceMapID > 0)
            {
                var loancondition = dbContext.LoanConditions.Where(se => se.FASTPreferenceMapId == ReturnDet.FASTPreferenceMapID).FirstOrDefault();

                var productConditions = dbContext.ProductConditions.Where(se => se.FASTPreferenceMapId == ReturnDet.FASTPreferenceMapID);

                if (productConditions.Count() > 0)
                {
                    ReturnDet.ProductTypes = productConditions.Select(se => new ProductTypeMappingDTO()
                    {
                        ProductTypecode = se.Product.ProductCode,
                        ProductTypeId = se.ProductId,
                        ProductTypedesc = se.Product.ProductName,
                        Ticked = true
                    }).ToList();
                }

                if (loancondition != null)
                    ReturnDet.LoanAmount = loancondition.LoanAmountGreaterThan;

                ReturnDet.Conditions = new List<ConditionPreferenceDTO>();
                var Stateconditions = dbContext.LocationConditions.Where(se => se.FASTPreferenceMapId == ReturnDet.FASTPreferenceMapID && se.ParentLocationConditionId == null && se.ConditionTypeCodeId == (int)Conditions.State);

                foreach (var item in Stateconditions)
                {
                    StateMappingDTO PreferenceState = new StateMappingDTO();
                    PreferenceState = GetStatesList().Where(se => se.StateCodes == item.ConditionValue).FirstOrDefault();

                    var counties = dbContext.LocationConditions.Where(se => se.FASTPreferenceMapId == ReturnDet.FASTPreferenceMapID && se.ParentLocationConditionId == item.LocationConditionId && se.ConditionTypeCodeId == (int)Conditions.county);

                    if (counties.Count() == 0)
                    {
                        CountyMappingDTO preferenceCounty = GetCountyList(PreferenceState.StateFIPS).Where(se => se.county == "ALL").FirstOrDefault();
                        ReturnDet.Conditions.Add(new ConditionPreferenceDTO { PreferenceState = PreferenceState, PreferenceCounty = preferenceCounty });
                        continue;
                    }

                    foreach (var child in counties)
                    {
                        CountyMappingDTO preferenceCounty = GetCountyList(PreferenceState.StateFIPS).Where(se => se.county == child.ConditionValue).FirstOrDefault();

                        ReturnDet.Conditions.Add(new ConditionPreferenceDTO { PreferenceState = PreferenceState, PreferenceCounty = preferenceCounty });
                    }
                }
            }

            return ReturnDet;
        }

        public List<FASTFilePreferenceDTO> GetFASTFilePreferencesDetails(string stateFipsId, string countyFipsId, string loanAmountId, int tenantId, string Regionid)
        {
            List<FASTFilePreferenceDTO> FASTFilePreferences = new List<FASTFilePreferenceDTO>();
            using (var ctx = new TerminalDBEntities.Entities())
            {
                FIPSEntities FIPSContext = new FIPSEntities();
                string stateCode = FIPSContext.FIPSCodes.Where(x => x.StateFIPS == stateFipsId).Select(v => v.StateCode)?.FirstOrDefault();
                int countyFips = countyFipsId.ToIntDefEx();
                string countyName = (countyFips > 0 ? FIPSContext.FIPSCodes.Where(x => x.CountyFIPS == countyFipsId
                && x.StateFIPS == stateFipsId).Select(v => v.County).FirstOrDefault() : string.Empty);
                decimal loanAmount = Convert.ToDecimal(loanAmountId);
                int serviceId = 0;
                //int locationId = 0;
                int Region = Regionid.ToIntDefEx();
                List<FASTFilePreferenceDTO> query = null;

                try
                {
                    if (!string.IsNullOrEmpty(stateCode) && !string.IsNullOrEmpty(countyName) && loanAmount > 0)
                    {
                        query = (from lc in ctx.LocationConditions
                                 join fpm in ctx.FASTPreferenceMaps on lc.FASTPreferenceMapId equals fpm.FASTPreferenceMapId
                                 join lc1 in ctx.LocationConditions on lc.LocationConditionId equals lc1.ParentLocationConditionId into tmpcounties
                                 from lc1 in tmpcounties.DefaultIfEmpty()
                                 join loc in ctx.LoanConditions on lc.FASTPreferenceMapId equals loc.FASTPreferenceMapId into tmploans
                                 from loc in tmploans.DefaultIfEmpty()
                                 join ty in ctx.TypeCodes on fpm.LoanPurposeTypeCodeId equals ty.TypeCodeId into tmptypecodes
                                 from ty in tmptypecodes.DefaultIfEmpty()
                                 where lc.ConditionValue == stateCode
                                 && lc1.ConditionValue == countyName
                                 && loc.LoanAmountGreaterThan < loanAmount
                                 && (fpm.RegionId == Regionid.ToIntDefEx() || Region == 0)
                                 select new FASTFilePreferenceDTO()
                                 {
                                     FASTPreferenceMapID = fpm.FASTPreferenceMapId,
                                     FASTPreferenceMapName = fpm.FASTPreferenceMapName,
                                     FASTProgramTypeId = (fpm.FASTProgramTypeId > 0 ? fpm.FASTProgramTypeId : 0),
                                     FASTSearchTypeId = (fpm.FASTSearchTypeId.HasValue ? fpm.FASTSearchTypeId.Value : 0),
                                     LoanAmount = loanAmount,
                                     LoanPurpose = (ty.TypeCodeDesc != null ? ty.TypeCodeDesc : string.Empty),
                                     LocationId = (fpm.LocationId.HasValue ? fpm.LocationId.Value : 0),
                                     PreferenceCounty = countyName,
                                     PreferenceState = stateCode,
                                     ServiceId = serviceId,
                                     TenantId = fpm.TenantId,
                                     Tenant = fpm.Tenant.TenantName,
                                     RegionId = (fpm.RegionId.HasValue ? fpm.RegionId.Value : 0),
                                     CustomerId = fpm.CustomerId,
                                     CustomerName = fpm.Location != null ? fpm.Location.Customer.CustomerName + "(" + fpm.CustomerId + ")" : string.Empty
                                 }).ToList();

                    }
                    if (string.IsNullOrEmpty(stateCode) && string.IsNullOrEmpty(countyName) && loanAmount > 0)
                    {
                        query = (from loc in ctx.LoanConditions
                                 join fpm in ctx.FASTPreferenceMaps on loc.FASTPreferenceMapId equals fpm.FASTPreferenceMapId
                                 join ty in ctx.TypeCodes on fpm.LoanPurposeTypeCodeId equals ty.TypeCodeId into tmptypecodes
                                 from ty in tmptypecodes.DefaultIfEmpty()
                                 where loc.LoanAmountGreaterThan < loanAmount
                                        && (fpm.RegionId == Region || Region == 0)
                                 select new FASTFilePreferenceDTO()
                                 {
                                     FASTPreferenceMapID = fpm.FASTPreferenceMapId,
                                     FASTPreferenceMapName = fpm.FASTPreferenceMapName,
                                     FASTProgramTypeId = (fpm.FASTProgramTypeId > 0 ? fpm.FASTProgramTypeId : 0),
                                     FASTSearchTypeId = (fpm.FASTSearchTypeId.HasValue ? fpm.FASTSearchTypeId.Value : 0),
                                     LoanAmount = loanAmount,
                                     LoanPurpose = (ty.TypeCodeDesc != null ? ty.TypeCodeDesc : string.Empty),
                                     LocationId = (fpm.LocationId.HasValue ? fpm.LocationId.Value : 0),
                                     PreferenceCounty = countyName,
                                     PreferenceState = stateCode,
                                     ServiceId = serviceId,
                                     TenantId = fpm.TenantId,
                                     Tenant = fpm.Tenant.TenantName,
                                     RegionId = (fpm.RegionId.HasValue ? fpm.RegionId.Value : 0),
                                     CustomerId = fpm.CustomerId,
                                     CustomerName = fpm.Location != null ? fpm.Location.Customer.CustomerName + "(" + fpm.CustomerId + ")" : string.Empty

                                 }).ToList();
                    }
                    if (!string.IsNullOrEmpty(stateCode) && string.IsNullOrEmpty(countyName) && loanAmount <= 0)
                    {
                        query = (from lc in ctx.LocationConditions
                                 join fpm in ctx.FASTPreferenceMaps on lc.FASTPreferenceMapId equals fpm.FASTPreferenceMapId
                                 join ty in ctx.TypeCodes on fpm.LoanPurposeTypeCodeId equals ty.TypeCodeId into tmptypecodes
                                 from ty in tmptypecodes.DefaultIfEmpty()
                                 where lc.ConditionValue == stateCode
                                 && (fpm.RegionId == Region || Region == 0)
                                 select new FASTFilePreferenceDTO()
                                 {
                                     FASTPreferenceMapID = fpm.FASTPreferenceMapId,
                                     FASTPreferenceMapName = fpm.FASTPreferenceMapName,
                                     FASTProgramTypeId = (fpm.FASTProgramTypeId > 0 ? fpm.FASTProgramTypeId : 0),
                                     FASTSearchTypeId = (fpm.FASTSearchTypeId.HasValue ? fpm.FASTSearchTypeId.Value : 0),
                                     LoanAmount = loanAmount,
                                     LoanPurpose = (ty.TypeCodeDesc != null ? ty.TypeCodeDesc : string.Empty),
                                     LocationId = (fpm.LocationId.HasValue ? fpm.LocationId.Value : 0),
                                     PreferenceCounty = countyName,
                                     PreferenceState = stateCode,
                                     ServiceId = serviceId,
                                     TenantId = fpm.TenantId,
                                     Tenant = fpm.Tenant.TenantName,
                                     RegionId = (fpm.RegionId.HasValue ? fpm.RegionId.Value : 0),
                                     CustomerId = fpm.CustomerId,
                                     CustomerName = fpm.Location != null ? fpm.Location.Customer.CustomerName + "(" + fpm.CustomerId + ")" : string.Empty
                                 }).ToList();

                    }
                    if (!string.IsNullOrEmpty(stateCode) && !string.IsNullOrEmpty(countyName) && loanAmount <= 0)
                    {
                        query = (from lc in ctx.LocationConditions
                                 join fpm in ctx.FASTPreferenceMaps on lc.FASTPreferenceMapId equals fpm.FASTPreferenceMapId
                                 join lc1 in ctx.LocationConditions on lc.LocationConditionId equals lc1.ParentLocationConditionId
                                 join ty in ctx.TypeCodes on fpm.LoanPurposeTypeCodeId equals ty.TypeCodeId into tmptypecodes
                                 from ty in tmptypecodes.DefaultIfEmpty()
                                 where lc.ConditionValue == stateCode
                                 && lc1.ConditionValue == countyName
                                 && (fpm.RegionId == Region || Region == 0)
                                 select new FASTFilePreferenceDTO()
                                 {
                                     FASTPreferenceMapID = fpm.FASTPreferenceMapId,
                                     FASTPreferenceMapName = fpm.FASTPreferenceMapName,
                                     FASTProgramTypeId = (fpm.FASTProgramTypeId > 0 ? fpm.FASTProgramTypeId : 0),
                                     FASTSearchTypeId = (fpm.FASTSearchTypeId.HasValue ? fpm.FASTSearchTypeId.Value : 0),
                                     LoanAmount = loanAmount,
                                     LoanPurpose = (ty.TypeCodeDesc != null ? ty.TypeCodeDesc : string.Empty),
                                     LocationId = (fpm.LocationId.HasValue ? fpm.LocationId.Value : 0),
                                     PreferenceCounty = countyName,
                                     PreferenceState = stateCode,
                                     ServiceId = serviceId,
                                     TenantId = fpm.TenantId,
                                     Tenant = fpm.Tenant.TenantName,
                                     RegionId = (fpm.RegionId.HasValue ? fpm.RegionId.Value : 0),
                                     CustomerId = fpm.CustomerId,
                                     CustomerName = fpm.Location != null ? fpm.Location.Customer.CustomerName + "(" + fpm.CustomerId + ")" : string.Empty
                                 }).ToList();
                    }
                    if (!string.IsNullOrEmpty(stateCode) && string.IsNullOrEmpty(countyName) && loanAmount > 0)
                    {
                        query = (from lc in ctx.LocationConditions
                                 join fpm in ctx.FASTPreferenceMaps on lc.FASTPreferenceMapId equals fpm.FASTPreferenceMapId
                                 join loc in ctx.LoanConditions on fpm.FASTPreferenceMapId equals loc.FASTPreferenceMapId
                                 join ty in ctx.TypeCodes on fpm.LoanPurposeTypeCodeId equals ty.TypeCodeId into tmptypecodes
                                 from ty in tmptypecodes.DefaultIfEmpty()
                                 where loc.LoanAmountGreaterThan < loanAmount
                                 && lc.ConditionValue == stateCode
                                 && (fpm.RegionId == Region || Region == 0)
                                 select new FASTFilePreferenceDTO()
                                 {
                                     FASTPreferenceMapID = fpm.FASTPreferenceMapId,
                                     FASTPreferenceMapName = fpm.FASTPreferenceMapName,
                                     FASTProgramTypeId = (fpm.FASTProgramTypeId > 0 ? fpm.FASTProgramTypeId : 0),
                                     FASTSearchTypeId = (fpm.FASTSearchTypeId.HasValue ? fpm.FASTSearchTypeId.Value : 0),
                                     LoanAmount = loanAmount,
                                     LoanPurpose = (ty.TypeCodeDesc != null ? ty.TypeCodeDesc : string.Empty),
                                     LocationId = (fpm.LocationId.HasValue ? fpm.LocationId.Value : 0),
                                     PreferenceCounty = countyName,
                                     PreferenceState = stateCode,
                                     ServiceId = serviceId,
                                     TenantId = fpm.TenantId,
                                     Tenant = fpm.Tenant.TenantName,
                                     RegionId = (fpm.RegionId.HasValue ? fpm.RegionId.Value : 0),
                                     CustomerId = fpm.CustomerId,
                                     CustomerName = fpm.Location != null ? fpm.Location.Customer.CustomerName + "(" + fpm.CustomerId + ")" : string.Empty
                                 }).ToList();
                    }
                    if (string.IsNullOrEmpty(stateCode) && string.IsNullOrEmpty(countyName) && loanAmount <= 0)
                    {
                        query = (from fpm in ctx.FASTPreferenceMaps
                                 join lc in ctx.LocationConditions on fpm.FASTPreferenceMapId equals lc.FASTPreferenceMapId into tmplocations
                                 from lc in tmplocations.DefaultIfEmpty()
                                 join loc in ctx.LoanConditions on lc.FASTPreferenceMapId equals loc.FASTPreferenceMapId into tmploans
                                 from loc in tmploans.DefaultIfEmpty()
                                 join ty in ctx.TypeCodes on fpm.LoanPurposeTypeCodeId equals ty.TypeCodeId into tmptypecodes
                                 from ty in tmptypecodes.DefaultIfEmpty()
                                 where (fpm.RegionId == Region || Region == 0)
                                 select new FASTFilePreferenceDTO()
                                 {
                                     FASTPreferenceMapID = fpm.FASTPreferenceMapId,
                                     FASTPreferenceMapName = fpm.FASTPreferenceMapName,
                                     FASTProgramTypeId = (fpm.FASTProgramTypeId > 0 ? fpm.FASTProgramTypeId : 0),
                                     FASTSearchTypeId = (fpm.FASTSearchTypeId.HasValue ? fpm.FASTSearchTypeId.Value : 0),
                                     LoanAmount = loanAmount,
                                     LoanPurpose = (ty.TypeCodeDesc != null ? ty.TypeCodeDesc : string.Empty),
                                     LocationId = (fpm.LocationId.HasValue ? fpm.LocationId.Value : 0),
                                     PreferenceCounty = countyName,
                                     PreferenceState = stateCode,
                                     ServiceId = serviceId,
                                     TenantId = fpm.TenantId,
                                     Tenant = fpm.Tenant.TenantName,
                                     RegionId = (fpm.RegionId.HasValue ? fpm.RegionId.Value : 0),
                                     CustomerId = fpm.CustomerId,
                                     CustomerName = fpm.Location != null ? fpm.Location.Customer.CustomerName + "(" + fpm.CustomerId + ")" : string.Empty
                                 }).ToList();
                    }

                    FASTFilePreferences = query.Select(sel => new FASTFilePreferenceDTO()
                    {
                        FASTPreferenceMapID = sel.FASTPreferenceMapID,
                        FASTPreferenceMapName = sel.FASTPreferenceMapName,
                        FASTProgramTypeId = sel.FASTProgramTypeId,
                        FASTSearchTypeId = sel.FASTSearchTypeId,
                        LoanAmount = sel.LoanAmount,
                        LoanPurpose = sel.LoanPurpose,
                        LocationId = sel.LocationId,
                        PreferenceCounty = sel.PreferenceCounty,
                        PreferenceState = sel.PreferenceState,
                        SearchType = ctx.FASTSearchTypes.Where(v => v.FASTSearchTypeId == sel.FASTSearchTypeId).Select(v => v.SearchTypeCd + "-" + v.SearchTypeDesc).FirstOrDefault(),
                        ProgramTypeName = ctx.FASTProgramTypes.Where(x => x.FASTProgramTypeId == sel.FASTProgramTypeId).Select(x => x.ProgramTypeName).FirstOrDefault(),
                        ServiceId = sel.ServiceId,
                        TenantId = sel.TenantId,
                        Tenant = sel.Tenant,
                        RegionId = sel.RegionId,
                        Region = ctx.FASTRegions.Where(v => v.RegionID == sel.RegionId).Select(v => v.Name + " (" + v.RegionID + ")").FirstOrDefault(),
                        Location = ctx.Locations.Where(v => v.LocationId == sel.LocationId).Select(v => v.LocationName).FirstOrDefault(),
                        CustomerId = sel.CustomerId,
                        CustomerName = sel.CustomerName
                    }).DistinctBy(x => x.FASTPreferenceMapID).ToList();

                    if (FASTFilePreferences.Count() > 0 && tenantId != (int)TerminalDBEntities.TenantIdEnum.LVIS)
                    {
                        FASTFilePreferences = FASTFilePreferences.Where(sel => sel.TenantId == tenantId).DistinctBy(x => x.FASTPreferenceMapID).ToList();
                    }

                }
                catch (System.Exception ex)
                { ex.ToString(); }
            }

            return FASTFilePreferences;
        }

        public IEnumerable<ProgramTypeMappingDTO> GetProgramTypeList(int regionId = 349)
        {
            TerminalDBEntities.Entities dbContext = new TerminalDBEntities.Entities();
            var distinctProgramtype = dbContext.FASTProgramTypes.Where(x => x.RegionId == regionId).OrderBy(x => x.ProgramTypeName)
                .Select(se => new DataContracts.ProgramTypeMappingDTO()
                {
                    ProgramTypeId = se.FASTProgramTypeId,
                    ProgramTypeName = se.ProgramTypeName
                });

            return distinctProgramtype.DistinctBy(Sm => Sm.ProgramTypeId).ToList();
        }

        public IEnumerable<ProductTypeMappingDTO> GetProductTypeList(int appId)
        {
            TerminalDBEntities.Entities dbContext = new TerminalDBEntities.Entities();
            List<ProductTypeMappingDTO> distinctproducttype = new List<ProductTypeMappingDTO>();
            distinctproducttype = dbContext.Products
                .Where(se => se.ApplicationId == appId).OrderBy(x => x.ProductName)
                .Select(se => new DataContracts.ProductTypeMappingDTO()
                {
                    ProductTypeId = se.ProductId,
                    ProductTypecode = se.ProductCode,
                    ProductTypedesc = se.ProductName
                }).ToList();

            return distinctproducttype.DistinctBy(Sm => Sm.ProductTypeId).ToList();
        }

        public IEnumerable<SearchTypeMappingDTO> GetSearchTypeList()
        {
            TerminalDBEntities.Entities dbContext = new TerminalDBEntities.Entities();
            var distinctsearchtype = dbContext.FASTSearchTypes.OrderBy(x => (x.SearchTypeCd + "-" + x.SearchTypeDesc))
                .Select(se => new DataContracts.SearchTypeMappingDTO()
                {
                    SearchTypeId = se.FASTSearchTypeId,
                    SearchTypecode = se.SearchTypeCd,
                    SearchTypedesc = se.SearchTypeCd + "-" + se.SearchTypeDesc

                });

            return distinctsearchtype.DistinctBy(Sm => Sm.SearchTypeId).ToList();
        }

        public IEnumerable<ExceptionStatus> GetLoanPurposeList()
        {
            using (Entities dbContext = new Entities())
            {
                List<ExceptionStatus> Typecodes = dbContext.TypeCodes.Where(se => se.GroupTypeCode == 400).OrderBy(x => x.TypeCodeDesc).Select(sl => new ExceptionStatus
                {
                    ID = sl.TypeCodeId,
                    Name = sl.TypeCodeDesc
                }).ToList();

                return Typecodes;
            }
        }

        public FASTFilePreferenceDTO AddFASTFilePreference(FASTFilePreferenceDTO value, int tenantId, int userId)
        {
            using (Entities dbContext = new Entities())
            {
                TerminalDBEntities.FASTPreferenceMap FASTPreferenceMapDet = new TerminalDBEntities.FASTPreferenceMap();
                FASTPreferenceMapDet.RegionId = (value.RegionId > 0 ? value.RegionId : (int?)null);
                FASTPreferenceMapDet.FASTPreferenceMapName = value.FASTPreferenceMapName;
                FASTPreferenceMapDet.FASTProgramTypeId = value.FASTProgramTypeId;
                FASTPreferenceMapDet.FASTSearchTypeId = (value.FASTSearchTypeId > 0 ? value.FASTSearchTypeId : (int?)null);

                if (value.LocationDet != null)
                {
                    FASTPreferenceMapDet.LocationId = value.LocationDet.LocationId;
                    value.LocationId = value.LocationDet.LocationId;
                }
                if (value.LoanPurposeDet != null)
                {
                    FASTPreferenceMapDet.LoanPurposeTypeCodeId = value.LoanPurposeDet.ID;
                    value.LoanPurpose = value.LoanPurposeDet.Name;
                }

                FASTPreferenceMapDet.CreateDate = DateTime.Now;
                FASTPreferenceMapDet.LastModifiedDate = DateTime.Now;
                FASTPreferenceMapDet.CreateById = userId;
                FASTPreferenceMapDet.TenantId = tenantId;
                FASTPreferenceMapDet.ServiceId = (value.ServiceId > 0 ? value.ServiceId : (int?)null);
                FASTPreferenceMapDet.LocationId = (value.LocationId > 0 ? value.LocationId : (int?)null);
                FASTPreferenceMapDet.LoanPurposeTypeCodeId = (value.LoanPurposeDet != null && value.LoanPurposeDet.ID > 0 ? value.LoanPurposeDet.ID : (int?)null);
                FASTPreferenceMapDet.CustomerId = value.CustomerId;

                dbContext.FASTPreferenceMaps.Add(FASTPreferenceMapDet);
                int success = AuditLogHelper.SaveChanges(dbContext);

                if (success == 1)
                    value.FASTPreferenceMapID = FASTPreferenceMapDet.FASTPreferenceMapId;

                if (value.LoanAmount > 0)
                {
                    TerminalDBEntities.LoanCondition LoanDet = new TerminalDBEntities.LoanCondition();
                    LoanDet.FASTPreferenceMapId = value.FASTPreferenceMapID;
                    LoanDet.LoanAmountGreaterThan = value.LoanAmount;
                    LoanDet.CreatedDate = DateTime.Now;
                    LoanDet.LastModifiedDate = DateTime.Now;
                    LoanDet.CreatedById = userId;
                    LoanDet.LastModifiedById = userId;
                    LoanDet.LastModifiedDate = DateTime.Now;

                    dbContext.LoanConditions.Add(LoanDet);
                    AuditLogHelper.SaveChanges(dbContext);
                }

                if (value.BusinessProgramTypes != null)
                {
                    foreach (var item in value.BusinessProgramTypes)
                    {
                        TerminalDBEntities.FASTPreferenceBusinessProgram BusinessMap = new FASTPreferenceBusinessProgram();
                        BusinessMap.FASTBusinessProgramTypeId = item.Id;
                        BusinessMap.FASTPreferenceMapId = value.FASTPreferenceMapID;
                        dbContext.FASTPreferenceBusinessPrograms.Add(BusinessMap);
                        int parentLocation = AuditLogHelper.SaveChanges(dbContext);
                    }
                }

                if (value.FastProductTypes != null)
                {
                    foreach (var item in value.FastProductTypes)
                    {
                        TerminalDBEntities.FASTPreferenceProduct ProductMap = new FASTPreferenceProduct();
                        ProductMap.ProductId = item.ProductTypeId;
                        ProductMap.FASTPreferenceMapId = value.FASTPreferenceMapID;
                        dbContext.FASTPreferenceProducts.Add(ProductMap);
                        int parentLocation = AuditLogHelper.SaveChanges(dbContext);
                    }
                }

                if (value.ProductTypes != null)
                {
                    foreach (var item in value.ProductTypes)
                    {
                        TerminalDBEntities.ProductCondition ProductMap = new ProductCondition();
                        ProductMap.ProductId = item.ProductTypeId;
                        ProductMap.FASTPreferenceMapId = value.FASTPreferenceMapID;
                        ProductMap.CreatedDate = DateTime.Now;
                        ProductMap.LastModifiedDate = DateTime.Now;
                        ProductMap.CreatedById = userId;
                        ProductMap.LastModifiedById = userId;
                        ProductMap.LastModifiedDate = DateTime.Now;

                        dbContext.ProductConditions.Add(ProductMap);
                        int parentLocation = AuditLogHelper.SaveChanges(dbContext);
                    }
                }

                if (value.Conditions != null)
                {
                    foreach (var item in value.Conditions)
                    {
                        if (item.PreferenceState != null)
                        {
                            TerminalDBEntities.LocationCondition LocationDet = new TerminalDBEntities.LocationCondition();
                            LocationDet.FASTPreferenceMapId = value.FASTPreferenceMapID;
                            LocationDet.ConditionTypeCodeId = (int)Conditions.State;
                            LocationDet.ConditionValue = item.PreferenceState.StateCodes;

                            LocationDet.CreatedDate = DateTime.Now;
                            LocationDet.LastModifiedDate = DateTime.Now;
                            LocationDet.CreatedById = userId;
                            LocationDet.LastModifiedById = userId;

                            dbContext.LocationConditions.Add(LocationDet);
                            int parentLocation = AuditLogHelper.SaveChanges(dbContext);

                            if (parentLocation > 0 && item.PreferenceCounty.countyFIPS != "0")
                            {
                                TerminalDBEntities.LocationCondition LocationDetCounty = new TerminalDBEntities.LocationCondition();
                                LocationDetCounty.FASTPreferenceMapId = value.FASTPreferenceMapID;
                                LocationDetCounty.ConditionTypeCodeId = (int)Conditions.county;
                                LocationDetCounty.ConditionValue = item.PreferenceCounty.county;
                                LocationDetCounty.ParentLocationConditionId = LocationDet.LocationConditionId;
                                LocationDetCounty.CreatedDate = DateTime.Now;
                                LocationDetCounty.LastModifiedDate = DateTime.Now;
                                LocationDetCounty.LastModifiedById = userId;
                                LocationDetCounty.CreatedById = userId;

                                dbContext.LocationConditions.Add(LocationDetCounty);
                                AuditLogHelper.SaveChanges(dbContext);
                            }
                        }
                    }
                }
            }

            return GetFASTFilePreferencesDetailsById(tenantId, value.FASTPreferenceMapID);
        }
        
        public FASTFilePreferenceDTO UpdateFastFile(FASTFilePreferenceDTO value, int tenantId, int userId)
        {
            using (Entities Dbcontext = new Entities())
            {
                TerminalDBEntities.FASTPreferenceMap UpdateFastPreference = (from Prefernce in Dbcontext.FASTPreferenceMaps
                                                                             where Prefernce.FASTPreferenceMapId == value.FASTPreferenceMapID
                                                                             select Prefernce).FirstOrDefault();
                UpdateFastPreference.RegionId = (value.RegionId > 0 ? value.RegionId : (int?)null);
                UpdateFastPreference.FASTPreferenceMapName = value.FASTPreferenceMapName;
                UpdateFastPreference.FASTProgramTypeId = value.FASTProgramTypeId;
                UpdateFastPreference.FASTSearchTypeId = (value.FASTSearchTypeId > 0 ? value.FASTSearchTypeId : (int?)null);
                
                if (value.LocationDet != null)
                {
                    UpdateFastPreference.LocationId = value.LocationDet.LocationId;
                    value.LocationId = value.LocationDet.LocationId;
                }
                else
                {
                    UpdateFastPreference.LocationId = null;
                    value.LocationId = 0;
                }

                if (value.LoanPurposeDet != null)
                {
                    if (value.LoanPurposeDet.ID == 0)
                        UpdateFastPreference.LoanPurposeTypeCodeId = null;
                    else
                    {
                        UpdateFastPreference.LoanPurposeTypeCodeId = value.LoanPurposeDet.ID;
                        value.LoanPurpose = value.LoanPurposeDet.Name;
                    }
                }
                else
                    UpdateFastPreference.LoanPurposeTypeCodeId = null;

                UpdateFastPreference.LastModifiedDate = DateTime.Now;
                UpdateFastPreference.LastModifiedById = userId;
                UpdateFastPreference.ServiceId = (value.ServiceId > 0 ? value.ServiceId : (int?)null);
                UpdateFastPreference.LocationId = (value.LocationId > 0 ? value.LocationId : (int?)null);
                UpdateFastPreference.LoanPurposeTypeCodeId = (value.LoanPurposeDet != null && value.LoanPurposeDet.ID > 0 ? value.LoanPurposeDet.ID : (int?)null);
                UpdateFastPreference.CustomerId = value.CustomerId;

                Dbcontext.Entry(UpdateFastPreference).State = System.Data.Entity.EntityState.Modified;
                AuditLogHelper.SaveChanges(Dbcontext);

              

                if (value.LoanAmount >= 0)
                {
                    TerminalDBEntities.LoanCondition LoanDetupdate = (from LoanDetail in Dbcontext.LoanConditions
                                                                      where LoanDetail.FASTPreferenceMapId == value.FASTPreferenceMapID
                                                                      select LoanDetail).FirstOrDefault();

                    if (LoanDetupdate == null && value.LoanAmount > 0)
                    {
                        TerminalDBEntities.LoanCondition LoanDet = new TerminalDBEntities.LoanCondition();
                        LoanDet.FASTPreferenceMapId = UpdateFastPreference.FASTPreferenceMapId;
                        LoanDet.LoanAmountGreaterThan = value.LoanAmount;
                        LoanDet.CreatedDate = DateTime.Now;
                        LoanDet.LastModifiedDate = DateTime.Now;
                        LoanDet.CreatedById = userId;
                        LoanDet.LastModifiedById = userId;
                        LoanDet.LastModifiedDate = DateTime.Now;

                        Dbcontext.LoanConditions.Add(LoanDet);
                        AuditLogHelper.SaveChanges(Dbcontext);
                    }
                    else if(LoanDetupdate != null && value.LoanAmount > 0)
                    {
                        LoanDetupdate.FASTPreferenceMapId = value.FASTPreferenceMapID;
                        LoanDetupdate.LoanAmountGreaterThan = value.LoanAmount;
                        LoanDetupdate.LastModifiedDate = DateTime.Now;
                        LoanDetupdate.LastModifiedById = userId;

                        Dbcontext.Entry(LoanDetupdate).State = System.Data.Entity.EntityState.Modified;
                    }
                    else
                    {
                        if (LoanDetupdate != null && value.LoanAmount==0)
                        {
                            Dbcontext.Entry(LoanDetupdate).State = System.Data.Entity.EntityState.Deleted;
                        }

                    }

                    AuditLogHelper.SaveChanges(Dbcontext);
                }


                if (value.BusinessProgramTypes != null && value.BusinessProgramTypes.Count >= 0)
                {
                    IEnumerable<FASTPreferenceBusinessProgram> BusinessProgramTypes = Dbcontext.FASTPreferenceBusinessPrograms
                       .RemoveRange(Dbcontext.FASTPreferenceBusinessPrograms
                       .Where(se => (se.FASTPreferenceMapId == value.FASTPreferenceMapID)));

                    AuditLogHelper.SaveChanges(Dbcontext);

                    foreach (var item in value.BusinessProgramTypes)
                    {
                        TerminalDBEntities.FASTPreferenceBusinessProgram BusinessMap = new FASTPreferenceBusinessProgram();
                        BusinessMap.FASTBusinessProgramTypeId = item.Id;
                        BusinessMap.FASTPreferenceMapId = value.FASTPreferenceMapID;

                        Dbcontext.FASTPreferenceBusinessPrograms.Add(BusinessMap);
                        int parentLocation = AuditLogHelper.SaveChanges(Dbcontext);
                    }
                }

                if (value.ProductTypes != null && value.ProductTypes.Count >= 0)
                {
                    IEnumerable<ProductCondition> Producttypes = Dbcontext.ProductConditions
                       .RemoveRange(Dbcontext.ProductConditions
                       .Where(se => (se.FASTPreferenceMapId == value.FASTPreferenceMapID)));

                    AuditLogHelper.SaveChanges(Dbcontext);

                    foreach (var item in value.ProductTypes)
                    {
                        TerminalDBEntities.ProductCondition ProductMap = new ProductCondition();
                        ProductMap.ProductId = item.ProductTypeId;
                        ProductMap.FASTPreferenceMapId = value.FASTPreferenceMapID;
                        ProductMap.CreatedDate = DateTime.Now;
                        ProductMap.LastModifiedDate = DateTime.Now;
                        ProductMap.CreatedById = userId;
                        ProductMap.LastModifiedById = userId;
                        ProductMap.LastModifiedDate = DateTime.Now;

                        Dbcontext.ProductConditions.Add(ProductMap);
                        int parentLocation = AuditLogHelper.SaveChanges(Dbcontext);
                    }
                }

                if (value.FastProductTypes != null && value.FastProductTypes.Count >= 0)
                {
                    IEnumerable<FASTPreferenceProduct> Producttypes = Dbcontext.FASTPreferenceProducts
                       .RemoveRange(Dbcontext.FASTPreferenceProducts
                       .Where(se => (se.FASTPreferenceMapId == value.FASTPreferenceMapID)));

                    AuditLogHelper.SaveChanges(Dbcontext);

                    foreach (var item in value.FastProductTypes)
                    {
                        TerminalDBEntities.FASTPreferenceProduct ProductMap = new FASTPreferenceProduct();
                        ProductMap.ProductId = item.ProductTypeId;
                        ProductMap.FASTPreferenceMapId = value.FASTPreferenceMapID;
                        Dbcontext.FASTPreferenceProducts.Add(ProductMap);
                        int parentLocation = AuditLogHelper.SaveChanges(Dbcontext);
                    }
                }

                if (value.Conditions != null && value.Conditions.Count >= 0)
                {
                    IEnumerable<LocationCondition> conditionstoupdate = Dbcontext.LocationConditions
                             .RemoveRange(Dbcontext.LocationConditions
                             .Where(se => (se.FASTPreferenceMapId == value.FASTPreferenceMapID)));

                    AuditLogHelper.SaveChanges(Dbcontext);

                    foreach (var item in value.Conditions)
                    {
                        if (item.PreferenceState != null)
                        {
                            TerminalDBEntities.LocationCondition LocationDet = new TerminalDBEntities.LocationCondition();
                            LocationDet.FASTPreferenceMapId = value.FASTPreferenceMapID;
                            LocationDet.ConditionTypeCodeId = (int)Conditions.State;
                            LocationDet.ConditionValue = item.PreferenceState.StateCodes;

                            LocationDet.CreatedDate = DateTime.Now;
                            LocationDet.LastModifiedDate = DateTime.Now;
                            LocationDet.CreatedById = userId;

                            Dbcontext.LocationConditions.Add(LocationDet);
                            int parentLocation = AuditLogHelper.SaveChanges(Dbcontext);

                            if (parentLocation > 0 && item.PreferenceCounty.countyFIPS != "0")
                            {
                                TerminalDBEntities.LocationCondition LocationDetCounty = new TerminalDBEntities.LocationCondition();
                                LocationDetCounty.FASTPreferenceMapId = value.FASTPreferenceMapID;
                                LocationDetCounty.ConditionTypeCodeId = (int)Conditions.county;
                                LocationDetCounty.ConditionValue = item.PreferenceCounty.county;
                                LocationDetCounty.ParentLocationConditionId = LocationDet.LocationConditionId;
                                LocationDetCounty.CreatedDate = DateTime.Now;
                                LocationDetCounty.LastModifiedDate = DateTime.Now;
                                LocationDetCounty.LastModifiedById = userId;
                                LocationDetCounty.CreatedById = userId;

                                Dbcontext.LocationConditions.Add(LocationDetCounty);
                                AuditLogHelper.SaveChanges(Dbcontext);
                            }
                        }
                    }
                }
            }

            return GetFASTFilePreferencesDetailsById(tenantId, value.FASTPreferenceMapID);
        }

        public IEnumerable<BusinessProgramTypeMappingDTO> GetBusinessFASTProgramTypeList(int regionId)
        {
            TerminalDBEntities.Entities dbContext = new TerminalDBEntities.Entities();
            var distinctProgramtype = dbContext.FASTBusinessProgramTypes.Where(x => x.RegionId == regionId).OrderBy(x => x.BusinessProgramName)
                .Select(se => new DataContracts.BusinessProgramTypeMappingDTO()
                {
                    Id = se.FASTBusinessProgramTypeId,
                    Name = se.BusinessProgramName
                });

            return distinctProgramtype.DistinctBy(Sm => Sm.Id).ToList();
        }

        public int Delete(int id)
        {
            if (id > 0)
            {
                using (var dbContext = new TerminalDBEntities.Entities())
                {
                    var result = dbContext.GetDependancyRecordOutput(id, "FASTPreferenceMap").FirstOrDefault();

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

            return 0;
        }

        public int ConfirmDelete(int id)
        {
            using (TerminalDBEntities.Entities dbContext = new TerminalDBEntities.Entities())
            {
                IEnumerable<FASTPreferenceBusinessProgram> BusinessProgramTypes = dbContext.FASTPreferenceBusinessPrograms
                            .RemoveRange(dbContext.FASTPreferenceBusinessPrograms
                            .Where(se => (se.FASTPreferenceMapId == id)));


                IEnumerable<FASTPreferenceProduct> FastProducttypes = dbContext.FASTPreferenceProducts
                                      .RemoveRange(dbContext.FASTPreferenceProducts
                                      .Where(se => (se.FASTPreferenceMapId == id)));

                IEnumerable<ProductCondition> Producttypes = dbContext.ProductConditions
                                   .RemoveRange(dbContext.ProductConditions
                                   .Where(se => (se.FASTPreferenceMapId == id)));

                IEnumerable<LocationCondition> Conditions = dbContext.LocationConditions
                             .RemoveRange(dbContext.LocationConditions
                             .Where(se => (se.FASTPreferenceMapId == id)));

                IEnumerable<LoanCondition> loandetails = dbContext.LoanConditions
                             .RemoveRange(dbContext.LoanConditions
                             .Where(se => (se.FASTPreferenceMapId == id)));

                AuditLogHelper.SaveChanges(dbContext);

                IEnumerable<FASTPreferenceMap> Prefernces = dbContext.FASTPreferenceMaps
                             .RemoveRange(dbContext.FASTPreferenceMaps
                             .Where(se => (se.FASTPreferenceMapId == id)));

                if (Prefernces != null)
                    return AuditLogHelper.SaveChanges(dbContext);

            }

            return 0;
        }

        public IEnumerable<LocationsMappings> GetLocation()
        {
            TerminalDBEntities.Entities dbContext = new TerminalDBEntities.Entities();
            var distinctLocation = dbContext.Locations.OrderBy(x => x.LocationId)
                .Select(se => new DataContracts.LocationsMappings()
                {
                    LocationId = se.LocationId,
                    ExternalId = se.ExternalId
                });
            return distinctLocation.DistinctBy(Sm => Sm.LocationId).ToList();
        }

        public IEnumerable<TenantMappingDTO> GetTenant()
        {
            TerminalDBEntities.Entities dbContext = new TerminalDBEntities.Entities();
            var distinctTenant = dbContext.Tenants.OrderBy(x => x.TenantId)
                .Select(se => new DataContracts.TenantMappingDTO()
                {
                    TenantId = se.TenantId,
                    TenantName = se.TenantName
                });
            return distinctTenant.DistinctBy(Sm => Sm.TenantName).ToList();
        }

        public List<FASTFilePreferenceDTO> GetValidatorFilePreferences(string state, string county, string loanAmount, int serviceId, int locationId, int regionId,
            int loanPurposeTypeCodeId, int TenantId, int ProductId)
        {
            List<FASTFilePreferenceDTO> FASTFilePreferences = new List<FASTFilePreferenceDTO>();
            try
            {
                using (var dbContext = new Entities())
                {
                    FIPSEntities FIPSContext = new FIPSEntities();
                    string stateCode = FIPSContext.FIPSCodes.Where(x => x.StateFIPS == state).Select(v => v.StateCode)?.FirstOrDefault();
                    decimal Amount = Convert.ToDecimal(loanAmount);
                    //ProductId = 0;

                    if (county.ToUpper() == "ALL") { county = "0"; }

                    var result = dbContext.GetFASTFilePreferences(stateCode, county, Amount, serviceId, locationId, regionId, loanPurposeTypeCodeId, TenantId, ProductId, null).FirstOrDefault();
                    if (result != null)
                    {
                        int FastPreferenceId = result == null ? 0 : result.FASTPreferenceMapId;

                        var preference = dbContext.FASTPreferenceMaps.Where(se => se.FASTPreferenceMapId == FastPreferenceId).FirstOrDefault();

                        var ruleSelectionResponse = new FASTFilePreferenceDTO()
                        {
                            FASTPreferenceMapName = preference.FASTPreferenceMapName,
                            ProgramTypeName = preference.FASTProgramType.ProgramTypeName,
                            SearchType = preference.FASTSearchTypeId == null ? "" : dbContext.FASTSearchTypes.Where(se => se.FASTSearchTypeId == preference.FASTSearchTypeId).Select(x => x.SearchTypeCd)
                            .FirstOrDefault() + "-" + dbContext.FASTSearchTypes.Where(se => se.FASTSearchTypeId == preference.FASTSearchTypeId).Select(x => x.SearchTypeDesc).FirstOrDefault(),
                            FASTProgramTypeId = (preference.FASTProgramTypeId > 0 ? preference.FASTProgramTypeId : 0),
                            ProductDesc = new List<string>(),
                            BuisnessProgramType = new List<string>(),
                            FASTPreferenceMapID = preference.FASTPreferenceMapId, 
                            FASTSearchTypeId = preference.FASTSearchTypeId == null ? 0 : preference.FASTSearchTypeId.Value,
                            LocationId = preference.LocationId == null ? 0 : preference.LocationId.Value,
                            RegionId = (preference.RegionId.HasValue ? preference.RegionId.Value : 0),
                            ServiceId = preference.ServiceId == null ? 0 : preference.ServiceId.Value,   
                            TenantId = preference.TenantId,
                            CustomerId = preference.CustomerId,
                            CustomerName = preference.Location.Customer.CustomerName,
                        };

                        SetProductPreference(preference, ProductId, ruleSelectionResponse, dbContext);
                        SetBusinessProgramPreference(preference, ruleSelectionResponse);
                        
                        FASTFilePreferences.Add(ruleSelectionResponse);
                    }
                    else
                    {
                        List<FASTFilePreferenceDTO> FastPreference = new List<FASTFilePreferenceDTO>();
                        FastPreference.Add(new FASTFilePreferenceDTO { FASTPreferenceMapName = "No File Preferences found based on your look up criteria." });
                        FASTFilePreferences = FastPreference.Select(x => new FASTFilePreferenceDTO
                        {
                            FASTPreferenceMapName = x.FASTPreferenceMapName
                        }).ToList();
                    }
                    return FASTFilePreferences;
                }
            }
            catch (System.Exception ex)
            {
                FASTFilePreferences.Add(new FASTFilePreferenceDTO { FASTPreferenceMapName = "There was an error retrieving FAST File Preference. Error - " + ex.Message.ToString() });

                return FASTFilePreferences;
            }
        }

        private static void SetBusinessProgramPreference(FASTPreferenceMap preference, FASTFilePreferenceDTO ruleSelectionResponse)
        {
            if (preference.FASTPreferenceBusinessPrograms.Count == 0)
            {
                // ruleSelectionResponse.BusinessProgramTypes.Add(new BusinessProgramTypeMappingDTO() { Name ="Any"});
                ruleSelectionResponse.BuisnessProgramType.Add("Any");
            }
            else
            {
                foreach (var businessProgram in preference.FASTPreferenceBusinessPrograms)
                {
                    //ruleSelectionResponse.BusinessProgramTypes.Add(new BusinessProgramTypeMappingDTO() { Name = businessProgram.FASTBusinessProgramType.BusinessProgramName != "" ? businessProgram.FASTBusinessProgramType.BusinessProgramName : "No Buisness Programme Match" });
                    ruleSelectionResponse.BuisnessProgramType.Add(businessProgram.FASTBusinessProgramType.BusinessProgramName != "" ? businessProgram.FASTBusinessProgramType.BusinessProgramName : "Any");
                }
            }
        }

        private static void SetProductPreference(FASTPreferenceMap preference, int productId, FASTFilePreferenceDTO ruleSelectionResponse, Entities ctx)
        {
            //Look for request driven product selections.
            //This is a join between Product and ProductMap using the canonical's productid to select the matching map record for the FAST application.
            var fastProducts = ctx.ProductMaps.Join(ctx.Products,
                                                maps => maps.ExternalProductId,
                                                products => products.ProductId,
                                                (maps, products) => new { products.ProductCode, products.ApplicationId, maps.ProductId, products.ProductDesc })
                                            .Where(i => i.ApplicationId == (int)ApplicationEnum.FAST && productId == i.ProductId).ToList();
            if (fastProducts.Count > 0)
            {
                foreach (var fastProduct in fastProducts)
                {
                    //ruleSelectionResponse.ProductTypes.Add(new ProductTypeMappingDTO() { ProductTypecode = fastProduct.ProductCode });
                    ruleSelectionResponse.ProductDesc.Add(fastProduct.ProductDesc);
                }
            }
            else
            {
                if (preference.FASTPreferenceProducts.Count == 0)
                {
                    //ruleSelectionResponse.ProductTypes.Add(new ProductTypeMappingDTO() { ProductTypedesc = "Any" });
                    ruleSelectionResponse.ProductDesc.Add("Any");
                }
                else
                {
                    //No request driven product selections found. Try use product selections defined by preference maps. 
                    foreach (var product in preference.FASTPreferenceProducts)
                    {
                        //ruleSelectionResponse.ProductTypes.Add(new ProductTypeMappingDTO() { ProductTypecode = product.Product.ProductCode != "" ? product.Product.ProductCode : "Any" });
                        ruleSelectionResponse.ProductDesc.Add(!string.IsNullOrEmpty(product.Product.ProductDesc) ? product.Product.ProductDesc : product.Product.ProductCode);
                    }
                }
            }
        }

        public List<StateMappingDTO> GetStatesList()
        {
            return FIPSUtilities.GetStatesList();
        }

        public List<CountyMappingDTO> GetCountyList(string StateFips)
        {
            return FIPSUtilities.GetCountyList(StateFips);
        }
    }
}
