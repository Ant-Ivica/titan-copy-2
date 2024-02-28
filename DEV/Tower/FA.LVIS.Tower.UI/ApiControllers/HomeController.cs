using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FA.LVIS.Tower.UI.ApiControllers
{
    public class HomeController : Controller
    {
        // GET: Home

        [CustomExceptionFilter]
        public ActionResult Index()
        {
            return File("~/index.html", "text/html");
        }
    }
}