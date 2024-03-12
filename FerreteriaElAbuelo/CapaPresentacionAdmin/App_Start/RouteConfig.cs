using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CapaPresentacionAdmin
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Acceso", action = "Index", id = UrlParameter.Optional }
                //Para que desde ahora que se ejecute el proyecto devuelva al espacio del logueo
            );
        }
    }
}
