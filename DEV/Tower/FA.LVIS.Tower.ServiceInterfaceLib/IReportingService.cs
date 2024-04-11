using System.Collections.Generic;

using DC = FA.LVIS.Tower.DataContracts;
namespace FA.LVIS.Tower.Services
{
    public  interface IReportingService : Core.IServiceBase
    {
        List<DC.ReportingDTO> GetLVISServiceRequests(DC.SearchDetail value, int tenantId);
        DC.MessageLogDTO GetServiceReportDetail(int iServiceRequestid);
        IEnumerable<DC.ReportingDTO> GetLVISServiceRequests(string sFilter, int tenantId);

        IEnumerable<DC.ReportingDTO> GetLVISServiceRequestsbyReferenceNo(DC.SearchDetail value, int tenantId);

        string[] InvalidateOrderData(DC.ReportingDTO[] values, int tenantId, int userId);
    }
}
