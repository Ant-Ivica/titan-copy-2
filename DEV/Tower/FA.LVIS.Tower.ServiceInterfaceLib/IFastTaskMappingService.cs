using System.Collections.Generic;
using DC = FA.LVIS.Tower.DataContracts;

namespace FA.LVIS.Tower.Services
{
   public interface IFastTaskMappingService : Core.IServiceBase
    {
        List<DC.FastTaskMapDTO> GetFastTaskDetails(int TenantId);

        List<DC.Service> GetServiceDetails(int tenantId);

        DC.FastTaskMapDTO AddFastTask(DC.FastTaskMapDTO AddFasttaskDTO,int TenantId, int userId);

        DC.FastTaskMapDTO UpdateFastTask(DC.FastTaskMapDTO updateFasttaskDTO, int TenantId, int userId);

        List<DC.MessageType> GetMessageType();

        List<DC.TypeCodeDTO> GetTypeCode();

        int Delete(int Id);

        int ConfirmDelete(int Id);


    }
}
