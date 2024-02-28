using System;
using System.Web.Mvc;

namespace FA.LVIS.Tower.UI.Controllers
{
    public class JasmineController : Controller
    {
        public ViewResult Run()
        {
            return View("SpecRunner");
        }
    }
}
