using FA.LVIS.Tower.Data;
using FA.LVIS.Tower.DataContracts;
using System.Collections.Generic;
using System;

namespace FA.LVIS.Tower.Services
{
   public class FastTaskMappingService : Core.ServiceBase,IFastTaskMappingService
    {
        public FastTaskMapDTO AddFastTask(FastTaskMapDTO AddFasttaskDTO, int TenantId, int userId)
        {
            IFastTaskMappingDataProvider FastTaskProvider = DataProviderFactory.Resolve<IFastTaskMappingDataProvider>();
            return FastTaskProvider.AddFastTask(AddFasttaskDTO, TenantId,userId);
        }

        public List<FastTaskMapDTO> GetFastTaskDetails(int TenantId)
        {
            IFastTaskMappingDataProvider FastTaskProvider = DataProviderFactory.Resolve<IFastTaskMappingDataProvider>();
            return FastTaskProvider.GetFastTaskDetails(TenantId);
        }

        

        public List<Service> GetServiceDetails(int tenantId)
        {
            IFastTaskMappingDataProvider FastTaskProvider = DataProviderFactory.Resolve<IFastTaskMappingDataProvider>();
            return FastTaskProvider.GetServiceDetails(tenantId);
        }
        public List<MessageType> GetMessageType()
        {
            IFastTaskMappingDataProvider FastTaskProvider = DataProviderFactory.Resolve<IFastTaskMappingDataProvider>();
            return FastTaskProvider.GetMessageType();
        }
        public List<TypeCodeDTO> GetTypeCode()
        {
            IFastTaskMappingDataProvider FastTaskProvider = DataProviderFactory.Resolve<IFastTaskMappingDataProvider>();
            return FastTaskProvider.GetTypeCode();
        }

        public FastTaskMapDTO UpdateFastTask(FastTaskMapDTO updateFasttaskDTO, int TenantId, int userId)
        {
            IFastTaskMappingDataProvider FastTaskProvider = DataProviderFactory.Resolve<IFastTaskMappingDataProvider>();
            return FastTaskProvider.UpdateFastTask(updateFasttaskDTO, TenantId, userId);
        }

        public int Delete(int Id)
        {
            IFastTaskMappingDataProvider FastTaskProvider = DataProviderFactory.Resolve<IFastTaskMappingDataProvider>();
            return FastTaskProvider.Delete(Id);
        }

        public int ConfirmDelete(int Id)
        {
            IFastTaskMappingDataProvider FastTaskProvider = DataProviderFactory.Resolve<IFastTaskMappingDataProvider>();
            return FastTaskProvider.ConfirmDelete(Id);
        }
    }
}
