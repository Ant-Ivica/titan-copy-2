using System;
using System.Collections.Generic;
using System.Linq;
using FA.LVIS.Tower.Core;
using FA.LVIS.Tower.DataContracts;

namespace FA.LVIS.Tower.Data
{
    public class LenderMappingDataProvider : Core.DataProviderBase, ILenderMappingDataProvider
    {
        public List<DataContracts.LenderMapping> GetLVISLenders()
        {

            //getting lender Data from PID MAPPING Table
            NewDBEntities.Entities dbContext = new NewDBEntities.Entities();
         

            var distinctLenders = dbContext.Customers
                .Select(se => new DataContracts.LenderMapping()
                {
                    LVISLenderID = se.CustomerId.ToString(),
                    ExternalApplication = "Test",
                    InternalApplication = "FAST",
                    Groups = se.Category.CategoryName,
                    LenderName = se.CustomerName
                });

            return distinctLenders.DistinctBy(Sm => Sm.LVISLenderID).ToList();
        }
        public IEnumerable<LocationMappings> GetBranches(string lVISAbeid)
        {
            //getting lender Data from PID MAPPING Table
            NewDBEntities.Entities dbContext = new NewDBEntities.Entities();

            return dbContext.Locations.Where(se => se.CustomerId.ToString() == lVISAbeid).
                 Select(sm => new DataContracts.LocationMappings()
                 {
                     BID = sm.LocationId,
                     ExternalLocationID = sm.ExternalId,
                     Name = sm.LocationName,
                     LvisABEID = lVISAbeid
                 }).ToList();

        }

        public LocationMappings AddBranch(LocationMappings value)
        {

            using (NewDBEntities.Entities dbContext = new NewDBEntities.Entities())
            {
                NewDBEntities.Location LocationtoAdd = new NewDBEntities.Location();

                LocationtoAdd.ExternalId = value.ExternalLocationID;
                LocationtoAdd.LocationName = value.Name;
                LocationtoAdd.CustomerId = value.LvisABEID.ToIntDefEx();
                LocationtoAdd.TenantId =3;
                LocationtoAdd.CreatedDate = DateTime.Now;
                LocationtoAdd.LastModifiedDate = DateTime.Now;

                dbContext.Locations.Add(LocationtoAdd);
                int Success = AuditLogHelper.SaveChanges(dbContext);

                if (Success == 1)
                    value.BID = LocationtoAdd.LocationId;
            }
            return value;
        }



        public int UpdateBranch(LocationMappings value)
        {
            using (var dbContext = new NewDBEntities.Entities())
            {
                var LocationToUpdate = (from branch in dbContext.Locations
                                      where branch.LocationId == value.BID
                                      select branch).FirstOrDefault();
                if (value != null)
                {



                    LocationToUpdate.ExternalId = value.ExternalLocationID;
                    LocationToUpdate.LocationName = value.Name;                 
                    LocationToUpdate.LastModifiedDate = DateTime.Now;

                    dbContext.Entry(LocationToUpdate).State = System.Data.Entity.EntityState.Modified;

                    int Success = AuditLogHelper.SaveChanges(dbContext);
                     
                    if (Success == 1)
                        return LocationToUpdate.LocationId;
                    else
                        return 0;
                }
            }

            return 0;
        }

        public int DeleteBranch(int value)
        {
            using (var dbContext = new NewDBEntities.Entities())
            {
                var LocationDelete = (from branch in dbContext.Locations
                                      where branch.LocationId == value
                                      select branch);

                if (LocationDelete != null)
                {

                    dbContext.Locations.RemoveRange(LocationDelete);

                    return AuditLogHelper.SaveChanges(dbContext);
                }
            }
            return 0;
        }
    }
}
