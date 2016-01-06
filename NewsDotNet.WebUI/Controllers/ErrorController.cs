using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NewsDotNet.WebUI.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult InvalidInput()
        {
            return View();
        }

        public ActionResult Http404()
        {
            return View("Http404", "/" as object);
        }
    }
}