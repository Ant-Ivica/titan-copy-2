using FA.LVIS.Tower.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DC = FA.LVIS.Tower.DataContracts;
namespace FA.LVIS.Tower.Data
{
     public   interface IReportingDataProvider : IDataProviderBase
    {
        List<DC.ReportingDTO> GetLVISServiceRequests(DC.SearchDetail value, int tenantId);
        DC.MessageLogDTO GetServiceReportDetail(int iServiceRequestid);
        List<DC.ReportingDTO> GetLVISServiceRequests(string sFilter, int tenantId);
        List<DC.ReportingDTO> GetLVISServiceRequestsbyReferenceNo(DC.SearchDetail value, int tenantId);
        string[] InvalidateOrderData(DC.ReportingDTO[] values, int tenantId, int userId);
    }
}
