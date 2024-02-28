using FA.LVIS.Tower.Common;
using FA.LVIS.Tower.Data;
using FA.LVIS.Tower.DataContracts;
using FA.LVIS.Tower.FASTProcessing;
using FA.LVIS.Tower.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Http;
using DC = FA.LVIS.Tower.DataContracts;

namespace FA.LVIS.Tower.UI.ApiControllers
{
    [ValidateAntiForgeryTokenFilter]
    [RoutePrefix("LogInformation")]
    
    public class TerminalLogInformationController : ApiController
    {
        Logger sLogger = new Common.Logger(typeof(TerminalLogInformationController));
        [HttpPost]
        public IEnumerable<TerminalLogInformationDTO> GetTerminalLogInformationdetails(SearchDetail value)
        {
            AuditLogHelper.sSection = "LogInformation\\GetLogInformation";
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();
            var tenantId = (claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault() != null) ?
            Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value) : 0;
            sLogger.Debug(string.Format($"IN GetTerminalLogInformationdetails Log@ Values passed {value.currPage} , {value.EndTime},{value.ErrorEnabled},{value.Fromdate},{value.MessageText}"));
            return ServiceFactory.Resolve<ITerminalLogInformationMappingservice>().GetTerminalLogInformationdetails(value);
        }
                
        [HttpPost]
        public IEnumerable<DC.AuditingDTO> GetLogDetails(DC.SearchDetail value)
        {
            AuditLogHelper.sSection = "LogInformation\\GetLogDetails";
            List<DC.AuditingDTO> Auditdetails = new List<DC.AuditingDTO>();
            IAuditService customer = ServiceFactory.Resolve<IAuditService>();
            Auditdetails = customer.GetAuditDetails(value);
            return Auditdetails;
        }

        [HttpPost]
        public int GetLogDetailsCount(DC.SearchDetail value)
        {
            sLogger.Debug(string.Format($"IN GetLogDetailsCount Log@ Values passed {value.currPage} , {value.EndTime},{value.ErrorEnabled},{value.Fromdate},{value.MessageText}"));
            return ServiceFactory.Resolve<ITerminalLogInformationMappingservice>().GetLogDetailsCount(value);
        }


        //[Route("GetLogDetailsFilter/{sFilter}", Name = "GetLogDetailsFilter")]
        //[HttpGet]
        //public IEnumerable<DC.AuditingDTO> GetLogDetailsFilter(string sFilter)
        //{
        //    return ServiceFactory.Resolve<IAuditService>().GetAuditDetails(sFilter);
        //}


    }
}
