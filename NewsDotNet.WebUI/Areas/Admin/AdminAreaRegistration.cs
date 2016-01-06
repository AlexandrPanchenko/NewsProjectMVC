using System.Web.Mvc;

namespace NewsDotNet.WebUI.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                name: "EditArticle",
                url: "Admin/AdminArticles/Edit/{addressName}",
                defaults: new { controller = "AdminArticles", action = "Edit" }
            );
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { action = "Index", controller = "AdminHome", id = UrlParameter.Optional }
            );
        }
    }
}