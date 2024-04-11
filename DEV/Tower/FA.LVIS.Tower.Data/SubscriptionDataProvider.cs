using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FA.LVIS.Tower.DataContracts;
using FA.LVIS.Tower.Data.TerminalDBEntities;

namespace FA.LVIS.Tower.Data
{
    public class SubscriptionDataProvider : Core.DataProviderBase, ISubscriptionDataProvider
    {
        public DataContracts.Subscription AddSubscription(DataContracts.Subscription Subscription, int tenantId, int iEmployeeid)
        {
            using (var dbContext = new TerminalDBEntities.Entities())
            {
                TerminalDBEntities.Subscription SubscriptionAdd = new TerminalDBEntities.Subscription();

                SubscriptionAdd.CustomerId = Subscription.CustomerId;
                SubscriptionAdd.MessageTypeId = Subscription.MessageTypeId;
                SubscriptionAdd.ApplicationId = Subscription.ApplicationId;
                SubscriptionAdd.CategoryId = Subscription.CategoryId;
                SubscriptionAdd.TenantId = tenantId == ((int)TerminalDBEntities.TenantIdEnum.LVIS) ? Subscription.TenantId : tenantId;
                SubscriptionAdd.CreatedDate = DateTime.Now;
                SubscriptionAdd.LastModifiedDate = DateTime.Now;
                SubscriptionAdd.CreatedById = iEmployeeid;

                dbContext.Subscriptions.Add(SubscriptionAdd);
                int Success = AuditLogHelper.SaveChanges(dbContext);

                if (Success == 1)
                {
                    Subscription.SubscriptionId = SubscriptionAdd.SubscriptionId;
                    Subscription.MessageTypeId = SubscriptionAdd.MessageTypeId;
                    Subscription.MessageTypeName = dbContext.MessageTypes.Where(v => v.MessageTypeId == Subscription.MessageTypeId).Select(v => v.MessageTypeName).FirstOrDefault();
                    Subscription.MessageTypeDesc = dbContext.MessageTypes.Where(v => v.MessageTypeId == Subscription.MessageTypeId).Select(v => v.MessageTypeDesc).FirstOrDefault();
                    Subscription.CustomerId = SubscriptionAdd.CustomerId;
                    Subscription.ApplicationId = SubscriptionAdd.ApplicationId.Value;
                    Subscription.CategoryId = SubscriptionAdd.CategoryId;
                    Subscription.TenantId = SubscriptionAdd.TenantId;
                    Subscription.TenantName = dbContext.Tenants.Where(te => te.TenantId == SubscriptionAdd.TenantId).Select(se => se.TenantName).FirstOrDefault();

                }
                //if (Success == 1)
                //    Subscription.SubscriptionId = SubscriptionAdd.SubscriptionId;
            }

            return Subscription;

      }

        public IEnumerable<ApplicationMappingDTO> GetApplicationByTenant(int TenantId)
        {
            using (TerminalDBEntities.Entities dbContext = new TerminalDBEntities.Entities())
            {
                var innerquery = dbContext.Customers.Where
                  (x => x.TenantId == TenantId).Select(x => x.ApplicationId);

                List<DataContracts.ApplicationMappingDTO> Applicationslist = 
                    dbContext.Applications
                            .Where(c => innerquery.Contains(c.ApplicationId)).
                            Select(sl=> new DataContracts.ApplicationMappingDTO
                            { ApplicationId=sl.ApplicationId,
                            ApplicationName=sl.ApplicationName}).ToList();
                return Applicationslist;
            }

        }

        public IEnumerable<DataContracts.MessageType> GetApplicationMessageList(int ApplicationId,int TenantId,int SubscriptionId)
        {
            using (TerminalDBEntities.Entities dbContext = new TerminalDBEntities.Entities())
            {
                List<DataContracts.MessageType> MessageTypes = null;
                if (SubscriptionId > 0)
                {
                   //var query = dbContext.Subscriptions.Where(t => t.TenantId == TenantId && t.ApplicationId == ApplicationId).Select(Me => Me.MessageTypeId).ToArray();
                   MessageTypes = dbContext.MessageTypes.
                   Where(se => se.ApplicationId == ApplicationId).
                   Select(sl => new DataContracts.MessageType
                   {
                       MessageTypeId = sl.MessageTypeId,
                       MessageTypeName = sl.MessageTypeName,
                       MessageTypeDescription = sl.MessageTypeDesc
                   }).ToList();
                }
                else
                {
                    var query = dbContext.Subscriptions.Where(t => t.TenantId == TenantId && t.ApplicationId == ApplicationId).Select(Me => Me.MessageTypeId).ToArray();
                     MessageTypes = dbContext.MessageTypes.
                    Where(se => !query.Contains(se.MessageTypeId) && se.ApplicationId == ApplicationId).
                    Select(sl => new DataContracts.MessageType
                    {
                        MessageTypeId = sl.MessageTypeId,
                        MessageTypeName = sl.MessageTypeName,
                        MessageTypeDescription = sl.MessageTypeDesc
                    }).ToList();
                }
                return MessageTypes;
            }
        }

        public IEnumerable<DataContracts.MessageType> GetMessageType(int MEssageTypeId)
        {
            using (TerminalDBEntities.Entities dbContext = new TerminalDBEntities.Entities())
            {
                List<DataContracts.MessageType> MessageTypes 
                = dbContext.MessageTypes.Where(se => se.MessageTypeId == MEssageTypeId)
                .Select(sl => new DataContracts.MessageType
                {                   
                    MessageTypeName = sl.MessageTypeName,
                    MessageTypeDescription = sl.MessageTypeDesc

                }).ToList();

                return MessageTypes;
            }
        }

        public IEnumerable<DataContracts.Subscription> GetSubscriptionsByCategory(int categoryId, int tenantId, int applicationId)
        {
            List<DataContracts.Subscription> Subscriptions = new List<DataContracts.Subscription>();

            using (var dbContext = new TerminalDBEntities.Entities())
            {
                if (tenantId != ((int)TerminalDBEntities.TenantIdEnum.LVIS))
                {
                    var details = from sub in dbContext.Subscriptions.Where(subs => subs.CategoryId == categoryId && subs.TenantId == tenantId && subs.CustomerId == null)
                                  join mt in dbContext.MessageTypes.Where(sl => sl.ApplicationId == applicationId) on sub.MessageTypeId equals mt.MessageTypeId
                                  join t in dbContext.Tenants.Where(sl => sl.TenantId == tenantId) on sub.TenantId equals t.TenantId
                                  select new DataContracts.Subscription
                                  {
                                      MessageTypeId = mt.MessageTypeId,
                                      MessageTypeDesc = mt.MessageTypeDesc,
                                      MessageTypeName = mt.MessageTypeName,
                                      ApplicationId = applicationId,
                                      CategoryId = categoryId,
                                      SubscriptionId = sub.SubscriptionId,
                                      TenantId = sub.TenantId,
                                      TenantName = t.TenantName,
                                      CustomerId=sub.CustomerId
                                  };

                    Subscriptions = details.ToList();
                }
                else
                {
                    var details = from sub in dbContext.Subscriptions.Where(subs => subs.CategoryId == categoryId && subs.CustomerId == null)
                                  join mt in dbContext.MessageTypes.Where(sl => sl.ApplicationId == applicationId) on sub.MessageTypeId equals mt.MessageTypeId
                                  select new DataContracts.Subscription
                                  {
                                      MessageTypeId = mt.MessageTypeId,
                                      MessageTypeName = mt.MessageTypeName,
                                      ApplicationId = applicationId,
                                      CategoryId = categoryId,
                                      SubscriptionId = sub.SubscriptionId,
                                      TenantId = sub.TenantId,
                                      TenantName = dbContext.Tenants.Where(sel => sel.TenantId == sub.TenantId).Select(sel => sel.TenantName).FirstOrDefault(),
                                      MessageTypeDesc=mt.MessageTypeDesc,
                                      CustomerId= sub.CustomerId
                                  };

                    Subscriptions = details.ToList();
                }
            }

            Subscriptions = Subscriptions.GroupBy(x => x.MessageTypeId).Select(x => x.First()).OrderBy(x => x.MessageTypeName).ToList();

            return Subscriptions;
        }

        public IEnumerable<DataContracts.Subscription> GetSubscriptionsByCustomer(int customerId, int tenantId, int applicationId)
        {
            List<DataContracts.Subscription> Subscriptions = new List<DataContracts.Subscription>();

            using (var dbContext = new TerminalDBEntities.Entities())
            {
                if (tenantId != ((int)TerminalDBEntities.TenantIdEnum.LVIS))
                {
                    var details = from sub in dbContext.Subscriptions.Where(subs => subs.TenantId == tenantId && subs.CustomerId == customerId && subs.CategoryId == null)
                                  join mt in dbContext.MessageTypes.Where(sl => sl.ApplicationId == applicationId) on sub.MessageTypeId equals mt.MessageTypeId
                                  join t in dbContext.Tenants.Where(sl => sl.TenantId == tenantId) on sub.TenantId equals t.TenantId
                                  select new DataContracts.Subscription
                                  {
                                      MessageTypeId = mt.MessageTypeId,
                                      MessageTypeName = mt.MessageTypeName,
                                      MessageTypeDesc = mt.MessageTypeDesc,
                                      ApplicationId = applicationId,
                                      CustomerId = customerId,
                                      SubscriptionId = sub.SubscriptionId,
                                      TenantId = sub.TenantId,
                                      TenantName = t.TenantName,
                                      CategoryId=sub.CategoryId
                                  };

                    Subscriptions = details.ToList();
                }
                else
                {
                    var details = from sub in dbContext.Subscriptions.Where(subs => subs.CustomerId == customerId && subs.CategoryId == null)
                                  join mt in dbContext.MessageTypes.Where(sl => sl.ApplicationId == applicationId) on sub.MessageTypeId equals mt.MessageTypeId
                                  select new DataContracts.Subscription
                                  {
                                      MessageTypeId = mt.MessageTypeId,
                                      MessageTypeName = mt.MessageTypeName,
                                      MessageTypeDesc = mt.MessageTypeDesc,
                                      ApplicationId = applicationId,
                                      CategoryId = sub.CategoryId,
                                      SubscriptionId = sub.SubscriptionId,
                                      TenantId = sub.TenantId,
                                      TenantName = dbContext.Tenants.Where(sel => sel.TenantId == sub.TenantId).Select(sel => sel.TenantName).FirstOrDefault(),
                                      CustomerId=sub.CustomerId
                                  };

                    Subscriptions = details.ToList();
                }

                if (Subscriptions.Count < 1)
                {
                    var categoryId = dbContext.Customers.Where(sel => sel.CustomerId == customerId).Select(sel => sel.CategoryId)?.FirstOrDefault();

                    if (categoryId.HasValue && categoryId.Value > 0)
                    {
                        Subscriptions = GetSubscriptionsByCategory(categoryId.Value, tenantId, applicationId).ToList();
                    }
                }
            }

            Subscriptions = Subscriptions.GroupBy(x => x.MessageTypeId).Select(x => x.First()).OrderBy(x => x.MessageTypeName).ToList();

            return Subscriptions;
        }

        public DataContracts.Subscription UpdateSubscription(DataContracts.Subscription Subscription, int tenantId, int iEmployeeid)
        {
            using (var dbContext = new TerminalDBEntities.Entities())
            {    
                          
                var SubscriptionMapToUpdate = (from x in dbContext.Subscriptions
                                               where x.SubscriptionId == Subscription.SubscriptionId 
                                            select x).FirstOrDefault();


                var count = dbContext.Subscriptions.Where(se => se.ApplicationId == Subscription.ApplicationId
                  && se.MessageTypeId == Subscription.MessageTypeId && se.TenantId == Subscription.TenantId).Count();

                if (SubscriptionMapToUpdate != null && count ==0)
                {
                    SubscriptionMapToUpdate.LastModifiedDate = DateTime.Now;
                    SubscriptionMapToUpdate.LastModifiedById = iEmployeeid;
                    SubscriptionMapToUpdate.TenantId = tenantId == ((int)TerminalDBEntities.TenantIdEnum.LVIS) ? Subscription.TenantId : tenantId;
                    SubscriptionMapToUpdate.CustomerId = Subscription.CustomerId;
                    SubscriptionMapToUpdate.MessageTypeId = Subscription.MessageTypeId;
                    SubscriptionMapToUpdate.ApplicationId = Subscription.ApplicationId;
                    SubscriptionMapToUpdate.CategoryId = Subscription.CategoryId;


                    dbContext.Entry(SubscriptionMapToUpdate).State = System.Data.Entity.EntityState.Modified;

                    int Success = AuditLogHelper.SaveChanges(dbContext);

                    if (Success == 1)
                    {
                        Subscription.SubscriptionId = SubscriptionMapToUpdate.SubscriptionId;
                        Subscription.MessageTypeId = SubscriptionMapToUpdate.MessageTypeId;
                        Subscription.MessageTypeName = dbContext.MessageTypes.Where(v => v.MessageTypeId == Subscription.MessageTypeId).Select(v => v.MessageTypeName).FirstOrDefault();
                        Subscription.MessageTypeDesc = dbContext.MessageTypes.Where(v => v.MessageTypeId == Subscription.MessageTypeId).Select(v => v.MessageTypeDesc).FirstOrDefault();
                        Subscription.CustomerId = SubscriptionMapToUpdate.CustomerId;
                        Subscription.ApplicationId = SubscriptionMapToUpdate.ApplicationId.Value;
                        Subscription.CategoryId = SubscriptionMapToUpdate.CategoryId;
                        Subscription.TenantId = SubscriptionMapToUpdate.TenantId;
                        Subscription.TenantName = dbContext.Tenants.Where(te => te.TenantId == SubscriptionMapToUpdate.TenantId).Select(se => se.TenantName).FirstOrDefault();
                    }                  
                }

                else
                {
                    Subscription.SubscriptionId = SubscriptionMapToUpdate.SubscriptionId;
                    Subscription.MessageTypeId = SubscriptionMapToUpdate.MessageTypeId;
                    Subscription.MessageTypeName = dbContext.MessageTypes.Where(v => v.MessageTypeId == Subscription.MessageTypeId).Select(v => v.MessageTypeName).FirstOrDefault();
                    Subscription.MessageTypeDesc = dbContext.MessageTypes.Where(v => v.MessageTypeId == Subscription.MessageTypeId).Select(v => v.MessageTypeDesc).FirstOrDefault();
                    Subscription.CustomerId = SubscriptionMapToUpdate.CustomerId;
                    Subscription.ApplicationId = SubscriptionMapToUpdate.ApplicationId.Value;
                    Subscription.CategoryId = SubscriptionMapToUpdate.CategoryId;
                    Subscription.TenantId = SubscriptionMapToUpdate.TenantId;
                    Subscription.TenantName = dbContext.Tenants.Where(te => te.TenantId == SubscriptionMapToUpdate.TenantId).Select(se => se.TenantName).FirstOrDefault();
                    Subscription.Rcdcount = count;

                }

            }
            return Subscription;
        }

        public int DeleteSubscription(int value, int tenantId)
        {
            if (value > 0)
            {
                using (var dbContext = new Entities())
                {
                    var result = dbContext.GetDependancyRecordOutput(value, "Subscription").FirstOrDefault();

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

        public int ConfirmDeleteSubscription(int value, int tenantId)
        {
            int success = 0;
            using (var dbContext = new Entities())
            {
                var SubscriptionDelete = (from branch in dbContext.Subscriptions
                                      where branch.SubscriptionId == value
                                      select branch);

                if (SubscriptionDelete != null)
                {
                    dbContext.Subscriptions.RemoveRange(SubscriptionDelete);
                    success = AuditLogHelper.SaveChanges(dbContext);
                }
            }
            return success;
        }

    }
}
