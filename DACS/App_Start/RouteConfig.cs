using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DACS
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Guest", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "tim-kiem",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Guest", action = "Search", id = UrlParameter.Optional },
                namespaces: new[] {"OnliveShop.Controllers"}
            );
        }
    }
}
