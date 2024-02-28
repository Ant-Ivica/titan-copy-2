using FA.LVIS.Tower.Data.TerminalDBEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Threading;

namespace FA.LVIS.Tower.Data
{
    public class IAuditable
    {
    }

    public class AuditLogHelper
    {        
        public static string sSection;

        public static int SaveChanges(DbContext dbContext)
        {
            // Cast the Thread.CurrentPrincipal
            IPrincipal icp = Thread.CurrentPrincipal as IPrincipal;

            // Access IClaimsIdentity which contains claims
            IIdentity claimsIdentity = (IIdentity)icp.Identity;
            string username = claimsIdentity.Name;
            List<DbEntityEntry> addedEntities = new List<DbEntityEntry>();
            List<AuditLog> AuditEntries = new List<AuditLog>();


            // Get all Added/Deleted/Modified entities (not Unmodified or Detached)
            foreach (var ent in dbContext.ChangeTracker.Entries().Where(p => p.State == EntityState.Added || p.State == EntityState.Deleted || p.State == EntityState.Modified))
            {
                if (ent.State == EntityState.Added)
                {
                    // Queue these up for later, we need to do them after the savechanges method is called so that the ID/Key value is populated
                    addedEntities.Add(ent);
                }
                else
                {
                    // For each changed record, get the audit record entries and add them
                    foreach (AuditLog x in GetAuditRecordsForChange(dbContext, ent, ent.State, username))
                    {
                        AuditEntries.Add(x);
                    }
                }
            }
            int saved = 0;
            try
            {
                // Call the original SaveChanges(), which will save the changes that the user made, afterwards we can then save the audit logs
                saved = dbContext.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                // Throw a new DbEntityValidationException with the improved exception message.
                //    throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
            // OK now create audit logs for all the new/added entities
            foreach (var ent in addedEntities)
            {
                // For each changed record, get the audit record entries and add them
                foreach (AuditLog x in GetAuditRecordsForChange(dbContext, ent, EntityState.Added, username))
                {
                    AuditEntries.Add(x);
                }
            }

            if (saved >= 1)
            {
                try
                {
                    using (Entities dbContext1 = new Entities())
                    {
                        dbContext1.AuditLogs.AddRange(AuditEntries);

                        dbContext1.SaveChanges();
                    }
                }
                catch (System.Exception)
                {
                    // if we cant audit for some reason, then there isnt much we can do about it at this point in time
                    // the original save worked, so we still need to return a success back to the client

                    // potentially could log this out to a log file somewhere if one existed
                }
            }
            return saved;
        }

        public static List<AuditLog> GetAuditRecordsForChange(DbContext Context, DbEntityEntry dbEntry, EntityState state, string userName)
        {
            List<AuditLog> result = new List<AuditLog>();

            // only log audits for entities that inherit from IAuditable, this way other entities can move through the system
            // just fine without forcing them to be logged whenever something changes
            if ((state != EntityState.Deleted && dbEntry.CurrentValues != null) ||
                 (dbEntry.OriginalValues != null))
            {
                TableAttribute tableAttr =
                    dbEntry.Entity.GetType().GetCustomAttributes(typeof(TableAttribute), true).SingleOrDefault() as
                        TableAttribute;

                // Get table name (if it has a Table attribute, use that, otherwise get the pluralized name)
                string tableName = tableAttr != null ? tableAttr.Name : (dbEntry.Entity.GetType()).BaseType.Name;

                var manager = ((IObjectContextAdapter)Context).ObjectContext.ObjectStateManager;
                EntitySetBase setBase = manager.GetObjectStateEntry(dbEntry.Entity).EntitySet;

                List<string> keyNames = setBase.ElementType.KeyMembers.Select(k => k.Name).ToList();


                if (keyNames == null || keyNames.Count == 0)
                {
                    keyNames = dbEntry.Entity.GetType().GetProperties().Where(p =>
                        p.Name.Equals("ID", StringComparison.OrdinalIgnoreCase)
                        || p.Name.Equals(dbEntry.Entity.GetType().Name + "ID", StringComparison.OrdinalIgnoreCase)).Select(se => se.Name).ToList();
                }

                string recordId = "Unknown";

                if (keyNames.Count > 0)
                {
                    if (state == EntityState.Added)
                    {
                        recordId = dbEntry.CurrentValues.GetValue<object>(keyNames[0]).ToString();
                    }
                    else
                    {
                        recordId = dbEntry.OriginalValues.GetValue<object>(keyNames[0]).ToString();
                    }
                }

                if (keyNames.Count > 1)
                {
                    if (state == EntityState.Added)
                    {
                        recordId += "\\" + dbEntry.CurrentValues.GetValue<object>(keyNames[1]).ToString();
                    }
                    else
                    {
                        recordId += "\\" + dbEntry.OriginalValues.GetValue<object>(keyNames[1]).ToString();
                    }
                }

                if (state == EntityState.Added)
                {
                    // For Inserts, just add the whole record
                    // If the entity implements IDescribableEntity, use the description from Describe(), otherwise use ToString()
                    result.Add(new AuditLog()
                    {
                        UserName = userName,
                        EventDateutc = DateTime.Now,
                        EventType = AuditLogEventTypeEnum.Added.ToString(),
                        TableName = tableName,
                        RecordId = recordId,
                        Property = "ALL",
                        NewValue = Describe(dbEntry.CurrentValues),
                        Section = sSection
                    });
                }

                else if (state == EntityState.Deleted)
                {
                    // go get the original values, this is not the best performance, and I am not sure I would recommend doing this for an application
                    // that requires high performance, but editing and deleting entities should not happen often, so the overhead is currently considered
                    // acceptable
                    DbPropertyValues values = dbEntry.GetDatabaseValues();
                    // Same with deletes, do the whole record, and use either the description from Describe() or ToString()
                    result.Add(new AuditLog()
                    {
                        UserName = userName,
                        EventDateutc = DateTime.Now,
                        EventType = AuditLogEventTypeEnum.Deleted.ToString(),
                        TableName = tableName,
                        RecordId = recordId,
                        Property = "ALL",
                        OriginalValue = Describe(values),
                        Section = sSection
                    });
                }
                else if (state == EntityState.Modified)
                {
                    DateTime changeTime = DateTime.Now;
                    DbPropertyValues values = dbEntry.GetDatabaseValues();
                    foreach (string propertyName in dbEntry.OriginalValues.PropertyNames)
                    {
                        // go get the original values, this is not the best performance, and I am not sure I would recommend doing this for an application
                        // that requires high performance, but editing and deleting entities should not happen often, so the overhead is currently considered
                        // acceptable
                        object originalValue = values.GetValue<object>(propertyName);
                        object currentValue = dbEntry.CurrentValues.GetValue<object>(propertyName);
                        // For updates, we only want to capture the columns that actually changed
                        if (!object.Equals(originalValue, currentValue))
                        {
                            result.Add(new AuditLog()
                            {
                                UserName = userName,
                                EventDateutc = changeTime,
                                EventType = AuditLogEventTypeEnum.Modified.ToString(),
                                TableName = tableName,
                                RecordId = recordId,
                                Property = propertyName,
                                OriginalValue = originalValue == null ? null : originalValue.ToString(),
                                NewValue = currentValue == null ? null : currentValue.ToString(),
                                Section = sSection
                            }
                                );

                        }
                    }
                }
                // Otherwise, don't do anything, we don't care about Unchanged or Detached entities
            }
            return result;
        }

        public static string Describe(object o)
        {
            StringBuilder result = new StringBuilder();
            result.Append("{");

            if (o is DbPropertyValues)
            {
                DbPropertyValues values = o as DbPropertyValues;
                foreach (string p in values.PropertyNames)
                {
                    if (values[p] != null)
                        result.Append(string.Format(" {0} : \" {1} \" ", p, values[p].ToString()));
                }
            }
            else
            {
                foreach (PropertyInfo p in o.GetType().GetProperties())
                {
                    result.Append(string.Format(" {0} : \" {1} \" ", p.Name, p.GetValue(o).ToString()));
                }
            }

            result.Append(" }");
            return result.ToString();
        }
    }

}
