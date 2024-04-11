using FA.LVIS.Tower.Core;
using System.Collections.Generic;
using DC = FA.LVIS.Tower.DataContracts;

namespace FA.LVIS.Tower.Data
{
   public interface IFastTaskMappingDataProvider : IDataProviderBase
    {
        List<DC.FastTaskMapDTO> GetFastTaskDetails(int tenantId);

        List<DC.Service> GetServiceDetails(int tenantId);

        DC.FastTaskMapDTO AddFastTask(DC.FastTaskMapDTO AddFasttaskDTO, int TenantId, int userId);

        DC.FastTaskMapDTO UpdateFastTask(DC.FastTaskMapDTO updateFasttaskDTO, int TenantId, int userId);
        
        List<DC.MessageType> GetMessageType();

        List<DC.TypeCodeDTO> GetTypeCode();

        int Delete(int id);

        int ConfirmDelete(int Id);
    }
}
