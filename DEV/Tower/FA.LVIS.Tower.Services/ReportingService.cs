using DC = FA.LVIS.Tower.DataContracts;
using FA.LVIS.Tower.Data;
using System.Collections.Generic;

namespace FA.LVIS.Tower.Services
{
    public class ReportingService : Core.ServiceBase, IReportingService
    {
        public IEnumerable<DC.ReportingDTO> GetLVISServiceRequests(string sFilter, int tenantId)
        {
             IReportingDataProvider eventProvider = DataProviderFactory.Resolve<IReportingDataProvider>();
            return eventProvider.GetLVISServiceRequests(sFilter, tenantId);
        }

        public IEnumerable<DC.ReportingDTO> GetLVISServiceRequestsbyReferenceNo(DC.SearchDetail value, int tenantId)
        {
            IReportingDataProvider eventProvider = DataProviderFactory.Resolve<IReportingDataProvider>();
            return eventProvider.GetLVISServiceRequestsbyReferenceNo(value, tenantId);
        }


        public List<DC.ReportingDTO> GetLVISServiceRequests(DC.SearchDetail value, int tenantId)
        {
            IReportingDataProvider eventProvider = DataProviderFactory.Resolve<IReportingDataProvider>();
            return eventProvider.GetLVISServiceRequests(value, tenantId);
        }

        public DC.MessageLogDTO GetServiceReportDetail(int iServiceRequestid)
        {
            IReportingDataProvider eventProvider = DataProviderFactory.Resolve<IReportingDataProvider>();
            return eventProvider.GetServiceReportDetail(iServiceRequestid);
        }

        public string[] InvalidateOrderData(DC.ReportingDTO[] values, int tenantId, int userId)
        {
            IReportingDataProvider eventProvider = DataProviderFactory.Resolve<IReportingDataProvider>();
            return eventProvider.InvalidateOrderData(values, tenantId, userId);
        }
    }
}
