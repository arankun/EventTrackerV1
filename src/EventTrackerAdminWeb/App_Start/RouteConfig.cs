#region directives

using System.Web.Mvc;
using System.Web.Routing;

#endregion

namespace EventTrackerAdminWeb
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapMvcAttributeRoutes();
            //AT: this is the default. This will be reserve for login. Comment for testing purpose
            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Account", action = "Index", id = UrlParameter.Optional }
            //);

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "membership", action = "households", id = UrlParameter.Optional }
            );
        }
    }
}
