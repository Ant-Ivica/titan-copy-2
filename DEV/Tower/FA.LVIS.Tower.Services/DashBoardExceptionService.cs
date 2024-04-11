using System.Collections.Generic;

using DC = FA.LVIS.Tower.DataContracts;
using FA.LVIS.Tower.Data;

namespace FA.LVIS.Tower.Services
{

    public class DashBoardExceptionService : Core.ServiceBase, IDashBoardExceptionService
    {
        public IEnumerable<DC.DashBoardExceptionDTO> GetBEQExceptions(int tenantId)
        {
            IExceptionDataProvider ExceptionProvider = DataProviderFactory.Resolve<IExceptionDataProvider>();
            return ExceptionProvider.GetBEQExceptions(tenantId);
        }

        public IEnumerable<DC.DashBoardGraphicalExceptionDTO> GetBEQGraphicalExceptions(int tenantId)
        {

            IExceptionDataProvider ExceptionProvider = DataProviderFactory.Resolve<IExceptionDataProvider>();
            return ExceptionProvider.GetBEQGraphicalExceptions(tenantId);
        }

        public IEnumerable<DC.DashBoardExceptionDTO> GetTEQExceptions(int tenantId)
        {
            IExceptionDataProvider ExceptionProvider = DataProviderFactory.Resolve<IExceptionDataProvider>();
            return ExceptionProvider.GetTEQExceptions(tenantId);
        }

        public IEnumerable<DC.DashBoardGraphicalExceptionDTO> GetTEQGraphs(int tenantId)
        {
            IExceptionDataProvider ExceptionProvider = DataProviderFactory.Resolve<IExceptionDataProvider>();
            return ExceptionProvider.GetTEQGraphs(tenantId);
        }
        
    }
}
