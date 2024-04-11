using FA.LVIS.Tower.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FA.LVIS.Tower.DataContracts;
using System.Security.Principal;
using System.Threading;
using System.Security.Claims;
using FA.LVIS.Tower.Data.DBEntities;

namespace FA.LVIS.Tower.Data
{
    public class FastGabDataProvider : Core.DataProviderBase, IFastGabDataProvider
    {
        public FASTGABMap AddFastGab(FASTGABMap value, int iEmployeeid)
        {
            TerminalDBEntities.Entities dbContext = new TerminalDBEntities.Entities();
            TerminalDBEntities.FASTGABMap AddGab = new TerminalDBEntities.FASTGABMap();
            AddGab.LocationId = value.LocationID;
            AddGab.NewLenderABEID = value.NewLenderABEID;
            AddGab.BusinessSourceABEID = value.BusinessSourceABEID;
            AddGab.FASTGABMapDesc = value.FASTGABMapDesc;
            AddGab.LoanTypeCodeId = (value.LoanTypeCodeId == 0) ? null : value.LoanTypeCodeId;
            AddGab.RegionId = value.RegionID;
            AddGab.CreatedById = iEmployeeid;
            AddGab.CreatedDate = DateTime.Today;
            AddGab.LastModifiedDate = DateTime.Today;

            dbContext.FASTGABMaps.Add(AddGab);
            int Success = AuditLogHelper.SaveChanges(dbContext);
            if (Success == 1)
                value.FASTGABMapId = AddGab.FASTGABMapId;

            //Add GAB Conditions 
            if (value.Conditions != null)
            {
                foreach (var item in value.Conditions)
                {
                    if (item.PreferenceState != null)
                    {
                        TerminalDBEntities.GABLocationCondition LocationDet = new TerminalDBEntities.GABLocationCondition();
                        LocationDet.FASTGABMapId = value.FASTGABMapId;
                        LocationDet.ConditionTypeCodeId = (int)Conditions.State;
                        LocationDet.ConditionValue = item.PreferenceState.StateCodes;

                        LocationDet.CreatedDate = DateTime.Now;
                        LocationDet.LastModifiedDate = DateTime.Now;
                        LocationDet.CreatedById = iEmployeeid;

                        dbContext.GABLocationConditions.Add(LocationDet);
                        int parentLocation = AuditLogHelper.SaveChanges(dbContext);

                        if (parentLocation > 0 && item.PreferenceCounty.countyFIPS != "0")
                        {
                            TerminalDBEntities.GABLocationCondition LocationDetCounty = new TerminalDBEntities.GABLocationCondition();
                            LocationDetCounty.FASTGABMapId = value.FASTGABMapId;
                            LocationDetCounty.ConditionTypeCodeId = (int)Conditions.county;
                            LocationDetCounty.ConditionValue = item.PreferenceCounty.county;
                            LocationDetCounty.ParentLocationConditionId = LocationDet.LocationConditionId;
                            LocationDetCounty.CreatedDate = DateTime.Now;
                            LocationDetCounty.LastModifiedDate = DateTime.Now;
                            LocationDetCounty.LastModifiedById = iEmployeeid;
                            LocationDetCounty.CreatedById = iEmployeeid;

                            dbContext.GABLocationConditions.Add(LocationDetCounty);
                            AuditLogHelper.SaveChanges(dbContext);
                        }
                    }
                }
            }
            //if (Success == 1)
            //{
            //    value.FASTGABMapId = AddGab.FASTGABMapId;
            //    value.LocationID = AddGab.LocationId;
            //    value.LocationName = dbContext.Locations.Where(v => v.LocationId == AddGab.LocationId).Select(v => v.LocationName).FirstOrDefault();
            //    value.Region = dbContext.FASTRegions.Where(v => v.RegionID == value.RegionID).Select(v => v.Name + " (" + v.RegionID + ")").FirstOrDefault();
            //    value.BusinessSourceABEID = AddGab.BusinessSourceABEID;
            //    value.NewLenderABEID = AddGab.NewLenderABEID;
            //    value.LoanTypeCodeId = (AddGab.LoanTypeCodeId == null) ? 0 : AddGab.LoanTypeCodeId.Value;
            //    value.LoanType = dbContext.TypeCodes.Where(T => T.TypeCodeId == AddGab.LoanTypeCodeId && T.GroupTypeCode == 500).Select(se => se.TypeCodeDesc).FirstOrDefault();
            //    //value.LoanTypeCodeId = AddGab.LoanTypeCodeId.Value;
            //    //value.LoanType = dbContext.TypeCodes.Where(v => v.TypeCodeId == AddGab.LoanTypeCodeId & v.GroupTypeCode == 500).Select(v => v.TypeCodeDesc).FirstOrDefault();
            //    value.FASTGABMapDesc = AddGab.FASTGABMapDesc;
            //}
            return GetFastGabMap(value.FASTGABMapId);
        }

        public List<FASTGABMap> GetFastGabDetails(string Locationid)
        {
            TerminalDBEntities.Entities dbContext = new TerminalDBEntities.Entities();
            var Listdetails = dbContext.FASTGABMaps.Where(se => se.LocationId.ToString() == Locationid)
                  .Select(gab => new FASTGABMap()
                  {
                      FASTGABMapId = gab.FASTGABMapId,
                      LocationID = gab.LocationId,
                      BusinessSourceABEID = gab.BusinessSourceABEID,
                      NewLenderABEID = gab.NewLenderABEID,
                      FASTGABMapDesc = gab.FASTGABMapDesc,
                      LocationName = gab.Location.LocationName,
                      RegionID = gab.RegionId,
                      LoanTypeCodeId = (gab.LoanTypeCodeId == null) ? 0 : gab.LoanTypeCodeId.Value,
                      LoanType = dbContext.TypeCodes.Where(T => T.TypeCodeId == gab.LoanTypeCodeId && T.GroupTypeCode == 500).Select(se => se.TypeCodeDesc).FirstOrDefault(),
                      Region = dbContext.FASTRegions.Where(v => v.RegionID == gab.RegionId).Select(v => v.Name + " (" + v.RegionID + ")").FirstOrDefault(),
                  }).ToList();

            return Listdetails;
        }

        public FASTGABMap UpdateFastGab(FASTGABMap value, int iEmployeeid)
        {
            using (var dbContext = new TerminalDBEntities.Entities())
            {
                var GabToUpdate = (from Gab in dbContext.FASTGABMaps
                                   where Gab.FASTGABMapId == value.FASTGABMapId
                                   select Gab).FirstOrDefault();
                if (value != null && GabToUpdate != null)
                {
                    GabToUpdate.LocationId = value.LocationID;
                    GabToUpdate.NewLenderABEID = value.NewLenderABEID;
                    GabToUpdate.BusinessSourceABEID = value.BusinessSourceABEID;
                    GabToUpdate.FASTGABMapDesc = value.FASTGABMapDesc;
                    GabToUpdate.LoanTypeCodeId = (value.LoanTypeCodeId == 0) ? null : value.LoanTypeCodeId;
                    GabToUpdate.RegionId = value.RegionID;
                    GabToUpdate.LastModifiedById = iEmployeeid;
                    GabToUpdate.LastModifiedDate = DateTime.Today;

                    dbContext.Entry(GabToUpdate).State = System.Data.Entity.EntityState.Modified;

                    int Success = AuditLogHelper.SaveChanges(dbContext);

                    //update Gab Conditions
                    if (value.Conditions != null && value.Conditions.Count >= 0)
                    {
                        IEnumerable<TerminalDBEntities.GABLocationCondition> conditionstoupdate = dbContext.GABLocationConditions
                                 .RemoveRange(dbContext.GABLocationConditions
                                 .Where(se => (se.FASTGABMapId == value.FASTGABMapId)));

                        AuditLogHelper.SaveChanges(dbContext);
                        foreach (var item in value.Conditions)
                        {
                            if (item.PreferenceState != null)
                            {
                                TerminalDBEntities.GABLocationCondition LocationDet = new TerminalDBEntities.GABLocationCondition();
                                LocationDet.FASTGABMapId = value.FASTGABMapId;
                                LocationDet.ConditionTypeCodeId = (int)Conditions.State;
                                LocationDet.ConditionValue = item.PreferenceState.StateCodes;

                                LocationDet.CreatedDate = DateTime.Now;
                                LocationDet.LastModifiedDate = DateTime.Now;
                                LocationDet.CreatedById = iEmployeeid;

                                dbContext.GABLocationConditions.Add(LocationDet);
                                int parentLocation = AuditLogHelper.SaveChanges(dbContext);

                                if (parentLocation > 0 && item.PreferenceCounty.countyFIPS != "0")
                                {
                                    TerminalDBEntities.GABLocationCondition LocationDetCounty = new TerminalDBEntities.GABLocationCondition();
                                    LocationDetCounty.FASTGABMapId = value.FASTGABMapId;
                                    LocationDetCounty.ConditionTypeCodeId = (int)Conditions.county;
                                    LocationDetCounty.ConditionValue = item.PreferenceCounty.county;
                                    LocationDetCounty.ParentLocationConditionId = LocationDet.LocationConditionId;
                                    LocationDetCounty.CreatedDate = DateTime.Now;
                                    LocationDetCounty.LastModifiedDate = DateTime.Now;
                                    LocationDetCounty.LastModifiedById = iEmployeeid;
                                    LocationDetCounty.CreatedById = iEmployeeid;
                                    dbContext.GABLocationConditions.Add(LocationDetCounty);
                                    AuditLogHelper.SaveChanges(dbContext);
                                }
                            }
                        }
                    }

                    //if (Success == 1)
                    //{
                    //    value.FASTGABMapId = GabToUpdate.FASTGABMapId;
                    //    value.LocationID = GabToUpdate.LocationId;
                    //    value.LocationName = dbContext.Locations.Where(v => v.LocationId == GabToUpdate.LocationId).Select(v => v.LocationName).FirstOrDefault();
                    //    //value.RegionID = GabToUpdate.RegionId;
                    //    value.Region = dbContext.FASTRegions.Where(v => v.RegionID == value.RegionID).Select(v => v.Name + " (" + v.RegionID + ")").FirstOrDefault();
                    //    value.BusinessSourceABEID = GabToUpdate.BusinessSourceABEID;
                    //    value.NewLenderABEID = GabToUpdate.NewLenderABEID;
                    //    value.LoanTypeCodeId = (GabToUpdate.LoanTypeCodeId == null) ? 0 : GabToUpdate.LoanTypeCodeId.Value;
                    //    value.LoanType = dbContext.TypeCodes.Where(T => T.TypeCodeId == GabToUpdate.LoanTypeCodeId && T.GroupTypeCode == 500).Select(se => se.TypeCodeDesc).FirstOrDefault();
                    //    //value.LoanTypeCodeId = GabToUpdate.LoanTypeCodeId.Value;
                    //    //value.LoanType=dbContext.TypeCodes.Where(v => v.TypeCodeId == GabToUpdate.LoanTypeCodeId & v.GroupTypeCode==500).Select(v => v.TypeCodeDesc).FirstOrDefault();
                    //    value.FASTGABMapDesc = GabToUpdate.FASTGABMapDesc;
                    //}

                    //return value;
                }
            }

            return GetFastGabMap(value.FASTGABMapId);
        }

        public List<TypeCodeDTO> GetLoanTypeDetatils()
        {
            TerminalDBEntities.Entities dbContext = new TerminalDBEntities.Entities();
            var distinctLoanType = dbContext.TypeCodes.Where(sel => sel.TypeCodeId == 514 || sel.TypeCodeId == 515 || sel.TypeCodeId == 529)
                .Select(se => new DataContracts.TypeCodeDTO()
                {
                    TypeCodeId = se.TypeCodeId,
                    TypeCodeDesc = se.TypeCodeDesc

                });
            return distinctLoanType.OrderByDescending(se => se.TypeCodeId).ToList();
        }

        public int DeleteGab(int value)
        {
            if (value > 0)
            {
                using (var dbContext = new TerminalDBEntities.Entities())
                {
                    var result = dbContext.GetDependancyRecordOutput(value, "FASTGABMap").FirstOrDefault();
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

        public int ConfirmDeleteGab(int value)
        {
            using (var dbContext = new TerminalDBEntities.Entities())
            {
                var GabToDelete = (from Gab in dbContext.FASTGABMaps
                                   where Gab.FASTGABMapId == value
                                   select Gab);

                if (GabToDelete != null)
                {
                    dbContext.FASTGABMaps.RemoveRange(GabToDelete);
                    return AuditLogHelper.SaveChanges(dbContext);
                }
            }
            return 0;
        }

        public FASTGABMap GetFastGabMap(int FastGabMapId)
        {
            TerminalDBEntities.Entities dbContext = new TerminalDBEntities.Entities();
            DataContracts.FASTGABMap ReturnDet = new DataContracts.FASTGABMap();
            if (dbContext.FASTGABMaps.Count() > 0)
            {
                ReturnDet = dbContext.FASTGABMaps.Where(se => se.FASTGABMapId == FastGabMapId).AsEnumerable()
                   .Select(x => new FASTGABMap
                   {
                       FASTGABMapId = x.FASTGABMapId,
                       LocationID = x.LocationId,
                       LocationName = dbContext.Locations.Where(v => v.LocationId == x.LocationId).Select(v => v.LocationName).FirstOrDefault(),
                       RegionID = x.RegionId,
                       Region = dbContext.FASTRegions.Where(v => v.RegionID == x.RegionId).Select(v => v.Name + " (" + v.RegionID + ")").FirstOrDefault(),
                       BusinessSourceABEID = x.BusinessSourceABEID,
                       NewLenderABEID = x.NewLenderABEID,
                       LoanTypeCodeId = x.LoanTypeCodeId == null ? 0 : x.LoanTypeCodeId.Value,
                       LoanType = dbContext.TypeCodes.Where(T => T.TypeCodeId == x.LoanTypeCodeId && T.GroupTypeCode == 500).Select(se => se.TypeCodeDesc).FirstOrDefault(),
                       FASTGABMapDesc = x.FASTGABMapDesc
                   }).FirstOrDefault();
            }

            if (ReturnDet.FASTGABMapId > 0)
            {
                ReturnDet.Conditions = new List<ConditionPreferenceDTO>();
                var Stateconditions = dbContext.GABLocationConditions.Where(se => se.FASTGABMapId == ReturnDet.FASTGABMapId && se.ParentLocationConditionId == null && se.ConditionTypeCodeId == (int)Conditions.State);

                foreach (var item in Stateconditions)
                {
                    StateMappingDTO PreferenceState = new StateMappingDTO();
                    PreferenceState = GetStatesList().Where(se => se.StateCodes == item.ConditionValue).FirstOrDefault();

                    var counties = dbContext.GABLocationConditions.Where(se => se.FASTGABMapId == ReturnDet.FASTGABMapId && se.ParentLocationConditionId == item.LocationConditionId && se.ConditionTypeCodeId == (int)Conditions.county);

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
      
        public List<StateMappingDTO> GetStatesList()
        {
            //TerminalDBEntities.Entities dbContext = new TerminalDBEntities.Entities();
            DBEntities.FIPSEntities dbContext = new DBEntities.FIPSEntities();
            var distinctstate = dbContext.FIPSCodes.OrderBy(x => x.State).Where(x => x.State != null && x.State != "")
                .Select(se => new DataContracts.StateMappingDTO
                {
                    StateFIPS = se.StateFIPS,
                    PreferenceState = se.State,
                    StateCodes = se.StateCode
                });
            return distinctstate.DistinctBy(Sm => Sm.StateFIPS).ToList();
        }

        public List<CountyMappingDTO> GetCountyList(string StateFips)
        {
            DBEntities.FIPSEntities dbContext = new DBEntities.FIPSEntities();

            List<CountyMappingDTO> CountyList = new List<CountyMappingDTO>();
            CountyList.Add(new CountyMappingDTO { county = "ALL", countyFIPS = "0" });

            var distinctstate = dbContext.FIPSCodes.Where(se => se.StateFIPS == StateFips && se.CountyFIPS != null).OrderBy(x => x.County)
                .Select(se => new DataContracts.CountyMappingDTO
                {
                    countyFIPS = se.CountyFIPS,
                    county = se.County
                });
            CountyList.AddRange(distinctstate.DistinctBy(Sm => Sm.countyFIPS).OrderBy(sl => sl.county).ToList());
            return CountyList;
        }

        public IEnumerable<FASTGABMap> GetFastGabSearchResults(string locationId, string stateFipsId, string countyFipsId, int tenantId)
        {
            List<DataContracts.FASTGABMap> GABMappings = new List<DataContracts.FASTGABMap>();

            using (var ctx = new TerminalDBEntities.Entities())
            {
                FIPSEntities FIPSContext = new FIPSEntities();
                string stateCode = FIPSContext.FIPSCodes.Where(x => x.StateFIPS == stateFipsId).Select(v => v.StateCode)?.FirstOrDefault();
                int countyFips = countyFipsId.ToIntDefEx();
                string countyName = (countyFips > 0 ? FIPSContext.FIPSCodes.AsEnumerable().Where(x => x.CountyFIPS == countyFipsId
                && x.StateFIPS == stateFipsId).Select(v => v.County).FirstOrDefault() : string.Empty);
                int locId = locationId.ToIntDefEx();

                List<FASTGABMap> query = null;

                if (!string.IsNullOrEmpty(stateCode) && !string.IsNullOrEmpty(countyName))
                {
                    query = (from lc in ctx.GABLocationConditions
                                 //join fgm in ctx.FASTGABMaps on new { lc.FASTGABMapId, fgm.LocationId } equals new { fgm.FASTGABMapId, locationId }
                             join fgm in ctx.FASTGABMaps on lc.FASTGABMapId equals fgm.FASTGABMapId where fgm.LocationId == locId
                             join lo in ctx.Locations on fgm.LocationId equals lo.LocationId
                             join lc1 in ctx.GABLocationConditions on lc.LocationConditionId equals lc1.ParentLocationConditionId
                             join ty in ctx.TypeCodes on fgm.LoanTypeCodeId equals ty.TypeCodeId into tmptypecodes
                             from ty in tmptypecodes.DefaultIfEmpty()
                             where lc.ConditionValue == stateCode
                             && lc1.ConditionValue == countyName
                             select new FASTGABMap()
                             {
                                 FASTGABMapId = fgm.FASTGABMapId,
                                 FASTGABMapDesc = fgm.FASTGABMapDesc,
                                 BusinessSourceABEID = fgm.BusinessSourceABEID,
                                 LocationID = fgm.LocationId,
                                 NewLenderABEID = fgm.NewLenderABEID,
                                 RegionID = fgm.RegionId,
                                 LoanType = (ty.TypeCodeDesc != null ? ty.TypeCodeDesc : string.Empty)
                             }).ToList();
                }
                else if (!string.IsNullOrEmpty(stateCode) && string.IsNullOrEmpty(countyName))
                {
                    query = (from lc in ctx.GABLocationConditions
                             join fgm in ctx.FASTGABMaps on lc.FASTGABMapId equals fgm.FASTGABMapId
                             where fgm.LocationId == locId
                             join lo in ctx.Locations on fgm.LocationId equals lo.LocationId
                             join ty in ctx.TypeCodes on fgm.LoanTypeCodeId equals ty.TypeCodeId into tmptypecodes
                             from ty in tmptypecodes.DefaultIfEmpty()
                             where lc.ConditionValue == stateCode
                             select new FASTGABMap()
                             {
                                 FASTGABMapId = fgm.FASTGABMapId,
                                 FASTGABMapDesc = fgm.FASTGABMapDesc,
                                 BusinessSourceABEID = fgm.BusinessSourceABEID,
                                 LocationID = fgm.LocationId,
                                 NewLenderABEID = fgm.NewLenderABEID,
                                 RegionID = fgm.RegionId,
                                 LoanType = (ty.TypeCodeDesc != null ? ty.TypeCodeDesc : string.Empty),
                                 LocationName = lo.LocationName,
                                 LoanTypeCodeId = ty.TypeCodeId,
                             }).ToList();
                }

                if (query != null)
                {
                    GABMappings = query.Select(sel => new FASTGABMap
                    {
                        FASTGABMapId = sel.FASTGABMapId,
                        BusinessSourceABEID = sel.BusinessSourceABEID,
                        FASTGABMapDesc = sel.FASTGABMapDesc,
                        LocationName = sel.LocationName,
                        LocationID = sel.LocationID,
                        NewLenderABEID = sel.NewLenderABEID,
                        RegionID = sel.RegionID,
                        Region = ctx.FASTRegions.Where(v => v.RegionID == sel.RegionID).Select(v => v.Name + " (" + v.RegionID + ")").FirstOrDefault(),
                    }).DistinctBy(x => x.FASTGABMapId).ToList();
                }
                else
                {
                    GABMappings = GetFastGabDetails(locationId);
                }

                return GABMappings;
            }
        }
    }
}
