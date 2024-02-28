using FA.LVIS.Tower.Core;
using FA.LVIS.Tower.DataContracts;
using LVIS.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FA.LVIS.Tower.Data
{
    public class FastTaskMappingDataProvider : Core.DataProviderBase, IFastTaskMappingDataProvider
    {
        IUtils Utils;

        public FastTaskMappingDataProvider()
        {
            Utils = new Utils();
        }

        public FastTaskMapDTO AddFastTask(FastTaskMapDTO AddFasttaskDTO, int TenantId, int userId)
        {
            TerminalDBEntities.Entities dbContext = new TerminalDBEntities.Entities();
            TerminalDBEntities.FASTTaskMap  addFastTaskmap = new TerminalDBEntities.FASTTaskMap();

            addFastTaskmap.FASTTaskMapName = Utils.ComputeName( AddFasttaskDTO.FastTaskMapDesc);
            addFastTaskmap.MessageTypeId = AddFasttaskDTO.MessageTypeId;
            addFastTaskmap.TypeCodeId = AddFasttaskDTO.TypecodeId;
            if (AddFasttaskDTO.RegionId == 0)
                addFastTaskmap.RegionId = null;
            else
               addFastTaskmap.RegionId = AddFasttaskDTO.RegionId;
            addFastTaskmap.ServiceId = AddFasttaskDTO.serviceId;          
            addFastTaskmap.FASTTaskMapDesc = AddFasttaskDTO.FastTaskMapDesc;
            addFastTaskmap.ContainsReasonCode = AddFasttaskDTO.HasReasonCode;
            addFastTaskmap.TenantId = TenantId;
            addFastTaskmap.ApplicationId = AddFasttaskDTO.applicationId;

            addFastTaskmap.CreatedDate = DateTime.Now;
            addFastTaskmap.LastModifiedDate = DateTime.Now;
            addFastTaskmap.CreatedById = userId;
            addFastTaskmap.LastModifiedById = userId;
            if (AddFasttaskDTO.CustomerId == 0)
                addFastTaskmap.CustomerId = null;
            else
                addFastTaskmap.CustomerId = AddFasttaskDTO.CustomerId;
            
            

            dbContext.FASTTaskMaps.Add(addFastTaskmap);
            int Success = AuditLogHelper.SaveChanges(dbContext);

            if (Success == 1)
            {
                AddFasttaskDTO.FastTaskMapId = addFastTaskmap.FASTTaskMapId;
                AddFasttaskDTO.FastTaskMapName = addFastTaskmap.FASTTaskMapName;
                AddFasttaskDTO.MessageType = dbContext.MessageTypes.Where(v => v.MessageTypeId == addFastTaskmap.MessageTypeId).Select(v => v.MessageTypeName).FirstOrDefault();
                AddFasttaskDTO.Typecode = dbContext.TypeCodes.Where(v => v.TypeCodeId == addFastTaskmap.TypeCodeId).Select(v => v.TypeCodeDesc).FirstOrDefault();
                AddFasttaskDTO.Region = addFastTaskmap.RegionId.HasValue == true ? dbContext.FASTRegions.Where(v => v.RegionID == addFastTaskmap.RegionId).Select(v => v.Name + " (" + v.RegionID + ")").FirstOrDefault() : "Any";
                AddFasttaskDTO.service = dbContext.Services.Where(v => v.ServiceId == addFastTaskmap.ServiceId).Select(v => v.ServiceName).FirstOrDefault();
                AddFasttaskDTO.FastTaskMapDesc = addFastTaskmap.FASTTaskMapDesc;
                AddFasttaskDTO.ApplicationName = addFastTaskmap.ApplicationId.HasValue == true ? dbContext.Applications.Where(a => a.ApplicationId == addFastTaskmap.ApplicationId).Select(a => a.ApplicationName).FirstOrDefault() : "Any";
                AddFasttaskDTO.CustomerName = addFastTaskmap.CustomerId.HasValue ? dbContext.Customers.Where(x => x.CustomerId == addFastTaskmap.CustomerId).Select(x=>x.CustomerName).FirstOrDefault():"Any";
            }
            return AddFasttaskDTO;
        }

        public List<FastTaskMapDTO> GetFastTaskDetails(int tenantId)
        {
            TerminalDBEntities.Entities dbContext = new TerminalDBEntities.Entities();
            List<DataContracts.FastTaskMapDTO> FastTaskMappings = new List<DataContracts.FastTaskMapDTO>();
            if (dbContext.FASTTaskMaps.Where(se=>se.IsInbound==false).Count() > 0)
            {
                FastTaskMappings = dbContext.FASTTaskMaps.Where(se => se.IsInbound == false)
                   .Select(x => new FastTaskMapDTO
                   {
                       FastTaskMapId = x.FASTTaskMapId,
                       FastTaskMapName = x.FASTTaskMapName,
                       MessageTypeId = x.MessageTypeId,
                       MessageType = dbContext.MessageTypes.Where(v => v.MessageTypeId == x.MessageTypeId).Select(v => v.MessageTypeName).FirstOrDefault(),
                       TypecodeId = x.TypeCodeId,
                       Typecode = dbContext.TypeCodes.Where(v => v.TypeCodeId == x.TypeCodeId).Select(v => v.TypeCodeDesc).FirstOrDefault(),
                       RegionId = (x.RegionId.HasValue == true ? x.RegionId.Value : 0),
                       //Region = dbContext.FASTRegions.Where(sel => sel.RegionID == x.RegionId).Select(se => se.Name).FirstOrDefault(),
                       Region = x.RegionId.HasValue == true ? dbContext.FASTRegions.Where(v => v.RegionID == x.RegionId).Select(v => v.Name + " (" + v.RegionID + ")").FirstOrDefault() : "Any",
                       serviceId =(int) x.ServiceId, 
                       service = dbContext.Services.Where(sel => sel.ServiceId == x.ServiceId).Select(se => se.ServiceName).FirstOrDefault(),
                       FastTaskMapDesc = x.FASTTaskMapDesc,
                       TenantId = x.TenantId,
                       Tenant = x.Tenant.TenantName,
                       HasReasonCode = x.ContainsReasonCode,
                       applicationId = x.ApplicationId == null ? null : x.ApplicationId,
                       CustomerId = x.CustomerId== null? 0:(int)x.CustomerId,
                       ApplicationName = x.ApplicationId.HasValue == true ? dbContext.Applications.Where(a => a.ApplicationId == x.ApplicationId).Select(a => a.ApplicationName).FirstOrDefault() : "Any",
                       CustomerName = x.CustomerId.HasValue == true ? dbContext.Customers.Where(v => v.CustomerId == x.CustomerId).Select(v => v.CustomerName).FirstOrDefault():"Any"

                   }).ToList();
            }
            if (FastTaskMappings.Count() > 0 && tenantId != (int)TerminalDBEntities.TenantIdEnum.LVIS)
            {
                FastTaskMappings = FastTaskMappings
                   .Where(sel => sel.TenantId == tenantId).ToList();            
            }
            return FastTaskMappings;
        }

       public List<Service> GetServiceDetails(int tenantId)
        {
            TerminalDBEntities.Entities dbContext = new TerminalDBEntities.Entities();
            List<DataContracts.Service> servicemap = new List<DataContracts.Service>();
            if (dbContext.Services.Count() > 0)
            {
                servicemap = dbContext.Services
                   .Where(se => se.TenantId == tenantId)
                   .Select(x => new Service
                   {
                       ID=x.ServiceId,
                       Name=x.ServiceName                     
                   }).ToList();
            }
            return servicemap;
        }
        public List<MessageType> GetMessageType()
        {
            TerminalDBEntities.Entities dbContext = new TerminalDBEntities.Entities();
            List<DataContracts.MessageType> MessageTypemap = new List<DataContracts.MessageType>();
            if (dbContext.MessageTypes.Count() > 0)
            {
                MessageTypemap = dbContext.MessageTypes 
                    .Where(se=>se.ApplicationId== (int)TerminalDBEntities.ApplicationEnum.LVIS && (se.MessageTypeName.Contains("#") == false))
                   .Select(x => new MessageType
                   {
                       MessageTypeId = x.MessageTypeId,
                       MessageTypeName = x.MessageTypeName
                   }).ToList();
            }
            return MessageTypemap;
        }
        public List<TypeCodeDTO> GetTypeCode()
        {
            TerminalDBEntities.Entities dbContext = new TerminalDBEntities.Entities();
            List<DataContracts.TypeCodeDTO> Typecodemap = new List<DataContracts.TypeCodeDTO>();
            if (dbContext.TypeCodes.Count() > 0)
            {
                Typecodemap = dbContext.TypeCodes
                    .Where(se => se.GroupTypeCode == (int)TerminalDBEntities.TypecodeEnum.MessageStatus)
                   .Select(x => new TypeCodeDTO
                   {
                       TypeCodeId= x.TypeCodeId,
                       TypeCodeDesc = x.TypeCodeDesc
                   }).ToList();
            }
            return Typecodemap;
        }

        public FastTaskMapDTO UpdateFastTask(FastTaskMapDTO updatefasTaskmap, int TenantId, int userId)
        {
            using (var dbContext = new TerminalDBEntities.Entities())
            {
                var FASTTaskMapToUpdate = (from x in dbContext.FASTTaskMaps
                                               where x.FASTTaskMapId == updatefasTaskmap.FastTaskMapId
                                               select x).FirstOrDefault();

                if (FASTTaskMapToUpdate != null)
                {
                    FASTTaskMapToUpdate.LastModifiedDate = DateTime.Now;
                    FASTTaskMapToUpdate.LastModifiedById = userId;
                    FASTTaskMapToUpdate.FASTTaskMapName = Utils.ComputeName(updatefasTaskmap.FastTaskMapDesc);
                    FASTTaskMapToUpdate.MessageTypeId = updatefasTaskmap.MessageTypeId;
                    FASTTaskMapToUpdate.TypeCodeId = updatefasTaskmap.TypecodeId;

                    if (updatefasTaskmap.RegionId == 0)
                        FASTTaskMapToUpdate.RegionId = null;
                    else
                        FASTTaskMapToUpdate.RegionId = updatefasTaskmap.RegionId;
                   // FASTTaskMapToUpdate.RegionId = updatefasTaskmap.RegionId;
                    FASTTaskMapToUpdate.ServiceId = updatefasTaskmap.serviceId;
                    FASTTaskMapToUpdate.FASTTaskMapDesc = updatefasTaskmap.FastTaskMapDesc;
                    //FASTTaskMapToUpdate.TenantId = TenantId;
                    FASTTaskMapToUpdate.ContainsReasonCode = updatefasTaskmap.HasReasonCode;
                    FASTTaskMapToUpdate.ApplicationId = updatefasTaskmap.applicationId;

                    if (updatefasTaskmap.CustomerId == 0)
                        FASTTaskMapToUpdate.CustomerId = null;
                    else
                        FASTTaskMapToUpdate.CustomerId = updatefasTaskmap.CustomerId;
                    dbContext.Entry(FASTTaskMapToUpdate).State = System.Data.Entity.EntityState.Modified;
                    
                    int Success = AuditLogHelper.SaveChanges(dbContext);

                    if (Success == 1)
                    {
                        updatefasTaskmap.FastTaskMapId = FASTTaskMapToUpdate.FASTTaskMapId;
                        updatefasTaskmap.FastTaskMapName = FASTTaskMapToUpdate.FASTTaskMapName;
                        updatefasTaskmap.MessageType = dbContext.MessageTypes.Where(v => v.MessageTypeId == FASTTaskMapToUpdate.MessageTypeId).Select(v => v.MessageTypeName).FirstOrDefault();
                        updatefasTaskmap.Typecode = dbContext.TypeCodes.Where(v => v.TypeCodeId == FASTTaskMapToUpdate.TypeCodeId).Select(v => v.TypeCodeDesc).FirstOrDefault();
                        updatefasTaskmap.Region = FASTTaskMapToUpdate.RegionId.HasValue == true ? dbContext.FASTRegions.Where(v => v.RegionID == FASTTaskMapToUpdate.RegionId).Select(v => v.Name + " (" + v.RegionID + ")").FirstOrDefault() : "Any";
                        updatefasTaskmap.service = dbContext.Services.Where(v => v.ServiceId == FASTTaskMapToUpdate.ServiceId).Select(v => v.ServiceName).FirstOrDefault();
                        updatefasTaskmap.FastTaskMapDesc = FASTTaskMapToUpdate.FASTTaskMapDesc;
                        updatefasTaskmap.ApplicationName = FASTTaskMapToUpdate.ApplicationId.HasValue == true ? dbContext.Applications.Where(a => a.ApplicationId == FASTTaskMapToUpdate.ApplicationId).Select(a => a.ApplicationName).FirstOrDefault() : "Any";
                        updatefasTaskmap.CustomerName = FASTTaskMapToUpdate.CustomerId.HasValue == true ? dbContext.Customers.Where(a => a.CustomerId == FASTTaskMapToUpdate.CustomerId).Select(a => a.CustomerName).FirstOrDefault() : "Any";

                    }
                    return updatefasTaskmap;
                }
            }
            return updatefasTaskmap;
        }

        public int Delete(int id)
        {
            if(id > 0)
            {
                using (var dbContext = new TerminalDBEntities.Entities())
                {
                    var result = dbContext.GetDependancyRecordOutput(id, "FASTTaskMap").FirstOrDefault();

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

        public int ConfirmDelete(int Id)
        {
            int success = 0;
            using (var dbContext = new TerminalDBEntities. Entities())
            {
                var FastTaskDelete = (from fasttask in dbContext.FASTTaskMaps
                                      where fasttask.FASTTaskMapId == Id
                                      select fasttask);

                if (FastTaskDelete != null)
                {
                    dbContext.FASTTaskMaps.RemoveRange(FastTaskDelete);
                    success = AuditLogHelper.SaveChanges(dbContext);

                }
            }
            return success;
        }
    }
}
