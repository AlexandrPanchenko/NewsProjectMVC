using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
///
using NewsDotNet.DomainModel.Entities;
using System.Web.Optimization;

namespace NewsDotNet.WebUI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();

            if (ex is HttpRequestValidationException)
            {
                Server.ClearError();
                Response.Redirect("~/Error/InvalidInput");
            }
            else if (ex is HttpException)
            {
                if (((HttpException)ex).GetHttpCode() == 404)
                {
                    Server.ClearError();
                    Response.Redirect("~/Error/Http404");
                }
            }
        }
    }
}
