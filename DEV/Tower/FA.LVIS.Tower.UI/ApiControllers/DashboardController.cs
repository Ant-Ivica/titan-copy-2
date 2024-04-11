using System.Collections.Generic;
using System.Web.Http;
using FA.LVIS.Tower.DataContracts;
using FA.LVIS.Tower.Services;
using System.Linq;
using System;
using FA.LVIS.Tower.Data;
using FA.LVIS.Tower.UI.ApiControllers.Filters;

namespace FA.LVIS.Tower.UI.ApiControllers
{

    [ValidateAntiForgeryTokenFilter]
    [RoutePrefix("Dashboard")]

    [CustomAuthorize]
    public class DashboardController : ApiController
    {
        [Route("GraphicalBEQException", Name = "GraphicalBEQExceptionDetails")]
        [HttpGet]
        public IEnumerable<DashBoardGraphicalExceptionDTO> GetGraphicalBEQException()
        {
            AuditLogHelper.sSection = "Dashboard\\GraphicalBEQExceptionDetails";
            IDashBoardExceptionService BEQList = ServiceFactory.Resolve<IDashBoardExceptionService>();
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();
            
            var tenantId = (claims.Where(c => c.Type == Constants.TENANT_ID).FirstOrDefault() != null) ?
            Convert.ToInt32(claims.Where(c => c.Type == Constants.TENANT_ID).FirstOrDefault().Value) : 0;
            return BEQList.GetBEQGraphicalExceptions(tenantId);
        }

        [Route("BEQException", Name = "BEQExceptionDetails")]
        [HttpGet]
        public IEnumerable<DashBoardExceptionDTO> GetBEQ()
        {
            AuditLogHelper.sSection = "Dashboard\\BEQExceptionDetails";
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();
            
            var tenantId = (claims.Where(c => c.Type == Constants.TENANT_ID).FirstOrDefault() != null) ?
            Convert.ToInt32(claims.Where(c => c.Type == Constants.TENANT_ID).FirstOrDefault().Value) : 0;

            IDashBoardExceptionService BEQList = ServiceFactory.Resolve<IDashBoardExceptionService>();
            return BEQList.GetBEQExceptions(tenantId);
        }

    
        [Route("TEQException", Name = "TEQExceptionDetails")]
        [HttpGet]
        public IEnumerable<DashBoardExceptionDTO> GetTEQ()
        {
            AuditLogHelper.sSection = "Dashboard\\TEQExceptionDetails";
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();
            
            var tenantId = (claims.Where(c => c.Type == Constants.TENANT_ID).FirstOrDefault() != null) ?
            Convert.ToInt32(claims.Where(c => c.Type == Constants.TENANT_ID).FirstOrDefault().Value) : 0;

            IDashBoardExceptionService TEQList = ServiceFactory.Resolve<IDashBoardExceptionService>();
            return TEQList.GetTEQExceptions(tenantId);
        }

        [Route("GraphicalTEQException", Name = "GraphicalTEQExceptionDetails")]
        [HttpGet]
        public IEnumerable<DashBoardGraphicalExceptionDTO> GetTEQGraphs()
        {
            AuditLogHelper.sSection = "Dashboard\\GraphicalTEQExceptionDetails";
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();
            
            var tenantId = (claims.Where(c => c.Type == Constants.TENANT_ID).FirstOrDefault() != null) ?
            Convert.ToInt32(claims.Where(c => c.Type == Constants.TENANT_ID).FirstOrDefault().Value) : 0;
            IDashBoardExceptionService TEQList = ServiceFactory.Resolve<IDashBoardExceptionService>();
            return TEQList.GetTEQGraphs(tenantId);
        }
    }
}
