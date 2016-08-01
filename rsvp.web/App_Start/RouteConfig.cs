using System.Web.Mvc;
using System.Web.Routing;

namespace rsvp.web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
              name: "EventAction",
              url: "{id}",
              defaults: new { controller = "Event", action = "Index", id = UrlParameter.Optional }
            );


            //routes.MapRoute("HomeActions", "{action}/{id}", new { controller = "Event" });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Event", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
