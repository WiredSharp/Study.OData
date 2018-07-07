using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Controllers;
using System.Web.OData.Routing;
using System.Web.OData.Routing.Conventions;

namespace Study.OData.Api
{
	internal class CompositeKeyRoutingConvention : EntityRoutingConvention
	{
		public override string SelectAction(ODataPath odataPath
			, HttpControllerContext controllerContext
			, ILookup<string, HttpActionDescriptor> actionMap)
		{
			string action = base.SelectAction(odataPath, controllerContext, actionMap);
			if (action != null)
			{
				IDictionary<string, object> routeValues = controllerContext.RouteData.Values;
				var replaced = new Dictionary<string, object>();
				foreach (KeyValuePair<string,object> item in routeValues)
				{
					if (item.Key.StartsWith(ODataRouteConstants.Key))
					{
						replaced.Add(item.Key.Replace(ODataRouteConstants.Key, ""), item.Value);
					}
				}
				foreach (KeyValuePair<string, object> item in replaced)
				{
					routeValues.Remove($"key{item.Key}");
					routeValues.Add(item.Key.Replace(ODataRouteConstants.Key, ""), item.Value);
				}
			}
			return action;
		}
	}
}