using Study.OData.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
using System.Web.OData.Query;

namespace Study.OData.Api
{
	public static class WebApiConfig
	{
		public static void Register(HttpConfiguration config)
		{
			// Web API configuration and services

			// Web API routes
			config.MapHttpAttributeRoutes();

			var builder = new ODataConventionModelBuilder();
			builder.EntitySet<Product>("Products");
			builder.EntityType<Product>().Filter(QueryOptionSetting.Allowed);
			config.MapODataServiceRoute("ODataRoute", "api", builder.GetEdmModel());

			config.Routes.MapHttpRoute(
				 name: "DefaultApi",
				 routeTemplate: "api/{controller}/{id}",
				 defaults: new { id = RouteParameter.Optional }
			);
		}
	}
}
