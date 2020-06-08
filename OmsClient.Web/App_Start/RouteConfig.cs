using System;
using System.Web.Mvc;
using System.Web.Routing;

/*
 * Copyright (c) 2020 深圳市德旺荣科技发展有限公司
 * All rights reserved.
 */
namespace OmsClient.Web
{
    /// <summary>
    /// 
    /// </summary>
    public class RouteConfig
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="routes"></param>
        public static void RegisterRoutes(RouteCollection routes)
        {
            //使用静态首页
            //routes.IgnoreRoute("");
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //File
            routes.MapRoute(
                name: "File",
                url: "File/{id}",
                defaults: new { controller = "File", action = "Index", id = typeof(Guid) }
            );

            //Img
            routes.MapRoute(
                name: "Img",
                url: "Img/{id}",
                defaults: new { controller = "File", action = "Img", id = typeof(Guid) }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Login", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "OmsClient.Web.Controllers" }
            );
        }
    }
}
