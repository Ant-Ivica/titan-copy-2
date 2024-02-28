using System.Collections.Generic;
using System.Web.Http;

using DC = FA.LVIS.Tower.DataContracts;
using FA.LVIS.Tower.Services;
using FA.LVIS.Tower.Data;
using FA.LVIS.Tower.UI.ApiControllers.Filters;

namespace FA.LVIS.Tower.UI.ApiControllers
{
    //public class EventMappings
    //{ 
    //    public string EID  { get; set;}
    //    public string EventID { get; set; }
    //   public string RequestType { get; set; }

    //    public string ProcessTriggerID { get; set; }
    //    public string TaskName { get; set; }

    //    public string TaskStatus { get; set; }

    //}
    [ValidateAntiForgeryTokenFilter]
    [RoutePrefix("OutboundEvents")]

    [CustomAuthorize]
    public class OutEventMappingsController : ApiController
    {
        // GET: api/OutEventMappings
        [Route("GetEvents",Name ="GetEvents")]
        [HttpGet]
        public IEnumerable<DC.OutEventMapping> Get()
        {

            AuditLogHelper.sSection = "Mappings\\OutboundEvents\\GetEvents";
            IOutEventMappingService outevent = ServiceFactory.Resolve<IOutEventMappingService>();
            return outevent.GetLVISOutEvents();
           
        }

        // GET: api/OutEventMappings/5
        [Route("GetEvents/{LenderABEID:maxlength(100)}", Name = "GetEventsbyLender")]
        [HttpGet]
        public IEnumerable<DC.OutEventMapping> Get(string lenderABEID)
        {
            IOutEventMappingService outevent = ServiceFactory.Resolve<IOutEventMappingService>();
            return outevent.GetLVISLenderOutEvents(lenderABEID);

        }
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/OutEventMappings
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/OutEventMappings/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/OutEventMappings/5
        public void Delete(int id)
        {
        }
    }
}