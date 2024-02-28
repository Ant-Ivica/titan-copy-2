using System.Collections.Generic;

using DC = FA.LVIS.Tower.DataContracts;
namespace FA.LVIS.Tower.Services
{
    public interface IDashBoardExceptionService : Core.IServiceBase
    {
        IEnumerable<DC.DashBoardExceptionDTO> GetBEQExceptions(int tenantId);

        IEnumerable<DC.DashBoardGraphicalExceptionDTO> GetBEQGraphicalExceptions(int tenantId);

        IEnumerable<DC.DashBoardExceptionDTO> GetTEQExceptions(int tenantId);

        IEnumerable<DC.DashBoardGraphicalExceptionDTO> GetTEQGraphs(int tenantId);
    }
}
