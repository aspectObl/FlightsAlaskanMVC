using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TestApplication
{
	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapRoute(
				name: "Flights",
				url: "Flights/{action}",
				defaults : new { Controller = "Flights", Action = "Search" }
				);

			routes.MapRoute(
				name: "Default",
				url: "{controller}/{action}",
				defaults: new { controller = "Flights", action = "Search" }
			);
		}
	}
}
