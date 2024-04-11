using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

using DC = FA.LVIS.Tower.DataContracts;
using FA.LVIS.Tower.Services;
using FA.LVIS.Tower.Data;
using FA.LVIS.Tower.UI.ApiControllers.Filters;

namespace FA.LVIS.Tower.UI.ApiControllers
{
    [ValidateAntiForgeryTokenFilter]
    [RoutePrefix("ReportingController")]

    [CustomAuthorize]
    public class ReportingController : ApiController
    {
        [Route("GetReportDetails/{tenant:maxlength(50)}", Name = "GetReportDetails")]
        [HttpPost]
        public IEnumerable<DC.ReportingDTO> Get(string tenant, DC.SearchDetail value)
        {
            AuditLogHelper.sSection = "ReportingController\\GetReportDetails";
            IApplicationUserService UsersList = ServiceFactory.Resolve<IApplicationUserService>();
            int tenantId = 0;

            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();

            if (tenant != DC.Constants.APPLICATION_RF)
            {
                tenantId = (claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault() != null) ?
                Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value) : 0;
            }
            else {
                tenantId = Convert.ToInt32(DC.Constants.TENANT_ID_RF);
            }            

            return ServiceFactory.Resolve<IReportingService>().GetLVISServiceRequests(value, tenantId);
        }


        [Route("GetMessageDetails/{iServiceRequest}", Name = "GetMessageDetails")]
        [HttpGet]
        public DC.MessageLogDTO GetMessageDetails(int iServiceRequest)
        {
            AuditLogHelper.sSection = "ReportingController\\GetMessageDetails";
            return ServiceFactory.Resolve<IReportingService>().GetServiceReportDetail(iServiceRequest);

        }

        [Route("GetReportDetailsFilter/{sFilter:maxlength(3)}/{tenant:maxlength(50)}", Name = "GetReportDetailsFilter")]
        [HttpGet]
        public IEnumerable<DC.ReportingDTO> GetReportDetailsFilter(string sFilter, string tenant)
        {
            AuditLogHelper.sSection = "ReportingController\\GetReportDetailsFilter";
            //IApplicationUserService UsersList = ServiceFactory.Resolve<IApplicationUserService>();
            int tenantId = 0;

            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();

            if (tenant != DC.Constants.APPLICATION_RF)
            {
                tenantId = (claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault() != null) ?
                Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value) : 0;
            }
            else {
                tenantId = Convert.ToInt32(DC.Constants.TENANT_ID_RF);//UsersList.GetTenantByName(tenant);
            }           

            return ServiceFactory.Resolve<IReportingService>().GetLVISServiceRequests(sFilter, tenantId);
        }        

        [Route("GetReportDetailsbyReferenceFilter/{tenant:maxlength(50)}", Name = "GetReportDetailsbyReferenceFilter")]
        [HttpPost]
        public IEnumerable<DC.ReportingDTO> GetReportDetailsbyReferenceFilter(string tenant, DC.SearchDetail value)
        {
            AuditLogHelper.sSection = "ReportingController\\GetReportDetailsbyReferenceFilter";
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();
            int tenantId = 0;

            if (tenant == DC.Constants.APPLICATION_RF) {
                tenantId = Convert.ToInt32(DC.Constants.TENANT_ID_RF);
            } else {
                 tenantId = (claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault() != null) ?
                    Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value) : 0;
            }            

            return ServiceFactory.Resolve<IReportingService>().GetLVISServiceRequestsbyReferenceNo(value, tenantId);
        }

        [Route("InvalidateOrderData")]
        [HttpPost]
        [CustomAuthorize("SuperAdmin", "Admin")]
        public string[] InvalidateOrderData(DC.ReportingDTO[] values)
        {
            AuditLogHelper.sSection = "ReportingController\\InvalidateOrderData";
            IReportingService ReportingMapping = ServiceFactory.Resolve<IReportingService>();
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();
            var userId = (claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault() != null) ?
             Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault().Value) : 0;

            var tenantId = (claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault() != null) ?
                Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value) : 0;

            return ReportingMapping.InvalidateOrderData(values, tenantId, userId);
        }

    }
}
