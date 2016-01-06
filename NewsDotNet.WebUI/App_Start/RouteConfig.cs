using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace NewsDotNet.WebUI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                            name: "Authors",
                            url: "Authors/{authorName}",
                            defaults: new { controller = "Articles", action = "ArticlesByAuthor", page = 1 }
                            );
            routes.MapRoute(
                name: "AuthorsByPage",
                url: "Authors/{authorName}/Page{page}",
                defaults: new { controller = "Articles", action = "ArticlesByAuthor" }
                );
            routes.MapRoute(
                           name: "Search",
                           url: "Search",
                           defaults: new { controller = "Articles", action = "ArticlesForSearch" }
                           );
            routes.MapRoute(
                          name: "Archive",
                          url: "Archive",
                          defaults: new { controller = "Articles", action = "Archive" }
                          );

            routes.MapRoute(
                        name: "ArticlesWithTag",
                        url: "Tags/{tagAddressName}",
                        defaults: new { controller = "Articles", action = "ArticlesWithTag", page = 1 }
           );

            routes.MapRoute(
                       name: "ArticlesWithTagPaged",
                       url: "Tags/{tagAddressName}/Page{page}",
                      defaults: new { controller = "Articles", action = "ArticlesWithTag"}
                       );
            routes.MapRoute(
                name: "ArticleInit",
                url: "Articles/ArticleAuthorInit",
                defaults: new { controller = "Articles", action = "ArticleAuthorInit" }
                );

            routes.MapRoute(
                name: "ShowArticle",
                url: "Articles/{addressName}",
                defaults: new { controller = "Articles", action = "Show" }
                );
           
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
