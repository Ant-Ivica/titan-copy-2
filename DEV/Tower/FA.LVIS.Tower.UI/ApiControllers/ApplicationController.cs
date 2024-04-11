using System.Collections.Generic;
using System.Web.Http;
using FA.LVIS.Tower.Services;
using DC = FA.LVIS.Tower.DataContracts;
using FA.LVIS.Tower.UI.ApiControllers.Filters;

namespace FA.LVIS.Tower.UI.ApiControllers
{

    [ValidateAntiForgeryTokenFilter]
    [RoutePrefix("ApplicationController")]
    public class ApplicationController : ApiController
    {
        [Route("GetApplicationStatus", Name = "GetApplicationStatus")]
        [HttpGet]
        public IEnumerable<DC.EMSQueue> GetApplicationStatus()
        {
            IApplicationStatusService List = ServiceFactory.Resolve<IApplicationStatusService>();
            return List.GetApplicationStatus();
        }

    }
}
