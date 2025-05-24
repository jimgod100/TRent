using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Rent
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "LinePayAPI",
                url: "api/LinePay/{action}/{id}",
                defaults: new { controller = "LinePay", id = UrlParameter.Optional }
            );


            // 將 Hangfire 的儀表板路由添加到路由表中
            routes.MapRoute(
                name: "Hangfire",
                url: "hangfire",
                defaults: new { controller = "Hangfire", action = "Index" }
            );
            
            // 預設路由
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
