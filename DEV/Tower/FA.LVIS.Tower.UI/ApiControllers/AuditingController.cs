using FA.LVIS.Tower.Data;
using FA.LVIS.Tower.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using DC = FA.LVIS.Tower.DataContracts;
namespace FA.LVIS.Tower.UI.ApiControllers
{

    [ValidateAntiForgeryTokenFilter]
    [RoutePrefix("AuditController")]
    public class AuditController : ApiController
    {
                
        [HttpPost]
        public IEnumerable<DC.AuditingDTO> GetAuditDetails(DC.SearchDetail value)
        {
            AuditLogHelper.sSection = "Audit\\GetAuditDetails";
            List<DC.AuditingDTO> Auditdetails = new List<DC.AuditingDTO>();
            IAuditService customer = ServiceFactory.Resolve<IAuditService>();
            Auditdetails = customer.GetAuditDetails(value);
            return Auditdetails;            
        }
        
        [Route("GetAuditDetailsFilter/{sFilter:maxlength(3)}", Name = "GetAuditDetailsFilter")]
        [HttpGet]
        public IEnumerable<DC.AuditingDTO> GetAuditDetailsFilter(string sFilter)
        {
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();
            var tenantId = (claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault() != null) ?
                Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value) : 0;

            AuditLogHelper.sSection = "Audit\\GetAuditDetailsFilter";
            return ServiceFactory.Resolve<IAuditService>().GetAuditDetails(sFilter, tenantId);
        }

        // GET: api/Audit/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Audit
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Audit/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Audit/5
        public void Delete(int id)
        {
        }
    }
}
