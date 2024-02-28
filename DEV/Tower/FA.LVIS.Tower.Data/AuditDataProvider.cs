using FA.LVIS.Tower.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FA.LVIS.Tower.Data
{
    public class AuditDataProvider : Core.DataProviderBase, IAuditDataProvider
    {
        public void AddUserAudit(AuditingDTO log)
        {
            using (TerminalDBEntities.Entities dbContext1 = new TerminalDBEntities.Entities())
            {
                 TerminalDBEntities.AuditLog AuditEntries = new TerminalDBEntities.AuditLog();

                AuditEntries.UserName = log.UserName;
                AuditEntries.EventDateutc = Convert.ToDateTime(log.EventDateutc);
                AuditEntries.EventType = log.EventType;
                AuditEntries.RecordId = log.RecordId;
                AuditEntries.TableName = log.TableName;
                AuditEntries.Property = log.Property;

                dbContext1.AuditLogs.Add(AuditEntries);

                dbContext1.SaveChanges();
            }
        }

        public List<AuditingDTO> GetAuditDetails(string sFilter, int tenantId)
        {
            List<AuditingDTO> AuditDetails = new List<AuditingDTO>();
            DateTime startDateTime = DateTime.Today;
            DateTime endDateTime = DateTime.Today;
            //last 24 hrs format
            if (sFilter.Contains("24"))
            {
                startDateTime = DateTime.Now;
                startDateTime = startDateTime.AddDays(-1);
                endDateTime = DateTime.Now;
            }
            else
            {
                startDateTime = startDateTime.AddDays(-(int.Parse(sFilter)));
                endDateTime = endDateTime.Subtract(startDateTime.TimeOfDay).AddDays(1).AddMilliseconds(-1);
            }
            using (TerminalDBEntities.Entities dc = new TerminalDBEntities.Entities())
            {
                var results = dc.AuditLogs.Where(ent => ent.EventDateutc >= startDateTime && ent.EventDateutc <= endDateTime);

                foreach (var dt in results)
                {
                    AuditDetails.Add(new DataContracts.AuditingDTO
                    {
                        UserName = dt.UserName,
                        EventDateutc = dt.EventDateutc.ToString(),
                        EventType = dt.EventType,
                        NewValue = dt.NewValue,
                        OriginalValue = dt.OriginalValue,
                        Property = dt.Property,
                        RecordId = dt.RecordId,
                        TableName = dt.TableName,
                        Section = dt.Section
                    });
                }

                //if (AuditDetails.Count > 0 )
                //{
                //    if (tenantId != (int)TerminalDBEntities.TenantIdEnum.LVIS)
                //        AuditDetails = AuditDetails.Where(ent => ent.UserName == (dc.Tower_Users.Where(sel => sel.TenantId == tenantId).FirstOrDefault().UserName)).ToList();

                    AuditDetails = AuditDetails.OrderByDescending(se => Convert.ToDateTime(se.EventDateutc)).ToList();
                //}
            }

            return AuditDetails;
        }

        public List<AuditingDTO> GetAuditDetails(SearchDetail SearchDetails)
        {
            List<AuditingDTO> AuditDetails = new List<AuditingDTO>();
            DateTime startDateTime = Convert.ToDateTime(SearchDetails.Fromdate);

            DateTime SearchstartDateTime = startDateTime.Subtract(startDateTime.TimeOfDay);

            DateTime endDateTime = Convert.ToDateTime(SearchDetails.ThroughDate);
            endDateTime = endDateTime.Subtract(startDateTime.TimeOfDay).AddDays(1).AddMilliseconds(-1);
            string searchString = SearchDetails.search;
            if (String.IsNullOrEmpty(searchString))
            {
                using (TerminalDBEntities.Entities dc = new TerminalDBEntities.Entities())
                {
                    var results = dc.AuditLogs.Where(ent => ent.EventDateutc >= startDateTime && ent.EventDateutc <= endDateTime);

                    foreach (var dt in results)
                    {
                        AuditDetails.Add(new DataContracts.AuditingDTO
                        {
                            UserName = dt.UserName,
                            EventDateutc = dt.EventDateutc.ToString(),
                            EventType = dt.EventType,
                            NewValue = dt.NewValue,
                            OriginalValue = dt.OriginalValue,
                            Property = dt.Property,
                            RecordId = dt.RecordId,
                            TableName = dt.TableName,
                            Section = dt.Section
                        });
                    }
                }
            }
            else
            {
                using (TerminalDBEntities.Entities dc = new TerminalDBEntities.Entities())
                {
                    var results = dc.AuditLogs.Where(ent => ent.EventDateutc >= startDateTime && ent.EventDateutc <= endDateTime);

                    results = results.Where(y => y.UserName.Contains(searchString) ||
                    y.EventType.Contains(searchString) ||
                    y.RecordId.Contains(searchString) ||
                    y.TableName.Contains(searchString) ||
                    y.Property.Contains(searchString) ||
                    y.OriginalValue.Contains(searchString) ||
                    y.NewValue.Contains(searchString));

                    foreach (var dt in results)
                    {
                        AuditDetails.Add(new DataContracts.AuditingDTO
                        {
                            UserName = dt.UserName,
                            EventDateutc = dt.EventDateutc.ToString(),
                            EventType = dt.EventType,
                            NewValue = dt.NewValue,
                            OriginalValue = dt.OriginalValue,
                            Property = dt.Property,
                            RecordId = dt.RecordId,
                            TableName = dt.TableName
                        });
                    }
                }
            }

            if (AuditDetails.Count > 0)
            {
                AuditDetails = AuditDetails.OrderByDescending(se => Convert.ToDateTime(se.EventDateutc)).ToList();
            }

            return AuditDetails;
        }
    }
}
