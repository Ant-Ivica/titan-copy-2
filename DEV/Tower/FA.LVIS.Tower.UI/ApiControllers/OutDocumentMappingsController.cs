using System.Collections.Generic;
using System.Web.Http;
using DC = FA.LVIS.Tower.DataContracts;
using FA.LVIS.Tower.Services;
using FA.LVIS.Tower.Data;
using System.Linq;
using System;
using FA.LVIS.Tower.UI.ApiControllers.Filters;

namespace FA.LVIS.Tower.UI.ApiControllers
{
    [ValidateAntiForgeryTokenFilter]
    [RoutePrefix("OutboundDocs")]

    [CustomAuthorize]
    public class OutDocumentMappingsController : ApiController
    {
        // GET: api/OutDocumentMappings  
        [Route("GetGroupDocs/{categoryId}/{Applicationid}", Name = "GetGroupDocs")]
        [HttpGet]
        public IEnumerable<DC.OutDocMapping> Get(int categoryId, int Applicationid)
        {
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();

            var tenantId = (claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault() != null) ?
                Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value) : 0;

            IOutDocMappingService outDoc = ServiceFactory.Resolve<IOutDocMappingService>();
            return outDoc.GetOutboundDocumentsByCategory(categoryId, tenantId, Applicationid);
        }

        // GET: api/OutDocumentMappings  
        [Route("GetDocs/{Customerid}/{Applicationid}", Name = "GetDocsbyLender")]
        [HttpGet]
        public IEnumerable<DC.OutDocMapping> GetDocsbyLender(int Customerid, int Applicationid)
        {
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();

            var tenantId = (claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault() != null) ?
                Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value) : 0;

            IOutDocMappingService outDoc = ServiceFactory.Resolve<IOutDocMappingService>();
            return outDoc.GetLVISLenderOutboundDocuments(Customerid, Applicationid, tenantId);

        }

        [Route("GetOutboundMessageTypes/{categoryId}/{applicationId}", Name = "GetOutboundMessageTypes")]
        [HttpGet]
        public IEnumerable<DC.MessageType> GetOutboundMessageTypes(int categoryId, int applicationId)
        {
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();

            var tenantId = (claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault() != null) ?
                Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value) : 0;

            IOutDocMappingService OutDocMappingService = ServiceFactory.Resolve<IOutDocMappingService>();
            return OutDocMappingService.GetOutboundMessageTypes(categoryId, applicationId, tenantId);
        }

        [Route("GetOutboundMessageTypesbyCustomer/{Customerid}/{applicationId}", Name = "GetOutboundMessageTypesbyCustomer")]
        [HttpGet]
        public IEnumerable<DC.MessageType> GetOutboundMessageTypesbyCustomer(int Customerid, int applicationId)
        {
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();

            var tenantId = (claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault() != null) ?
                Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value) : 0;

            IOutDocMappingService OutDocMappingService = ServiceFactory.Resolve<IOutDocMappingService>();
            return OutDocMappingService.GetOutboundMessageTypesbyCustomer(Customerid, applicationId, tenantId);
        }

        // GET: api/OutDocumentMappings  
        [Route("GetStatus", Name = "GetOutboundStatus")]
        [HttpGet]
        public IEnumerable<DC.DocumentType> GetStatus()
        {
          IOutDocMappingService outDoc = ServiceFactory.Resolve<IOutDocMappingService>();
          return outDoc.GetStatus();
        }

        // GET: api/OutDocumentMappings  
        ////[Route("", Name = "AddDocs")]
        [HttpPost]
        [CustomAuthorize("SuperAdmin", "Admin")]
        public DC.OutDocMapping AddDocs(DC.OutDocMapping Doc)
        {
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();

            var tenantId = (claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault() != null) ?
                Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value) : 0;
            var Userid = (claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault() != null) ?
               Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault().Value) : 0;

            AuditLogHelper.sSection = "Mappings//Outbound Document Mapping";
            IOutDocMappingService outDoc = ServiceFactory.Resolve<IOutDocMappingService>();
            return outDoc.AddDoc(Doc,tenantId,Userid);
        }

        ////[Route("UpdateDoc")]
        [HttpPost]
        [CustomAuthorize("SuperAdmin", "Admin")]
        public DC.OutDocMapping UpdateDoc(DC.OutDocMapping Doc)
        {
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();

            var tenantId = (claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault() != null) ?
                Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value) : 0;
            var Userid = (claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault() != null) ?
               Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault().Value) : 0;

            AuditLogHelper.sSection = "Mappings//Outbound Document Mapping";
            IOutDocMappingService outDoc = ServiceFactory.Resolve<IOutDocMappingService>();
            return outDoc.UpdateDoc(Doc,tenantId,Userid);
        }

        [Route("DeleteDoc")]
        [HttpPost]
        [CustomAuthorize("SuperAdmin", "Admin")]
        public int DeleteDoc([FromBody]int value)
        {
            AuditLogHelper.sSection = "Mappings//Outbound Document Mapping";
            IOutDocMappingService outDoc = ServiceFactory.Resolve<IOutDocMappingService>();
            return outDoc.Delete(value);
        }
    }
}
