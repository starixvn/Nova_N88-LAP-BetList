namespace Fanex.Athena.App_Start
{
    using System.Web.Mvc;
    using System.Web.Routing;

    public static class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes, bool includeAttributeMapping = true)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("content/{*pathInfo}");
            routes.IgnoreRoute("images/{*pathInfo}");
            routes.IgnoreRoute("scripts/{*pathInfo}");
            if (includeAttributeMapping)
            {
                routes.MapMvcAttributeRoutes();
            }

            routes.MapRoute(
                "Default",
                "{controller}/{action}",
                new { controller = "BetList", action = "Index" });
        }
    }
}