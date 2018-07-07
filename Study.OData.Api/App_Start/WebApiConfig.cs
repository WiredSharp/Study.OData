using System.Web.Http;
using System.Web.OData.Extensions;
using System.Web.OData.Routing;
using System.Web.OData.Routing.Conventions;

namespace Study.OData.Api
{
	public static class WebApiConfig
	{
		public static void Register(HttpConfiguration config)
		{
			// Web API configuration and services

			// Web API routes

			// Add the CompositeKeyRoutingConvention.
			var conventions = ODataRoutingConventions.CreateDefault();
			conventions.Insert(0, new CompositeKeyRoutingConvention());

			config.MapHttpAttributeRoutes();
			config.MapODataServiceRoute("ODataRoute"
				, "odata"
				, StudyOdataModelBuilder.CreateConventionalEdm()
				, pathHandler: new DefaultODataPathHandler()
				, routingConventions: conventions);

			config.Routes.MapHttpRoute(
				 name: "ResourceById",
				 routeTemplate: "api/{controller}({keys})"
			);

			config.Routes.MapHttpRoute(
				 name: "DefaultApi",
				 routeTemplate: "api/{controller}/{id}",
				 defaults: new { id = RouteParameter.Optional }
			);
		}
	}
}
