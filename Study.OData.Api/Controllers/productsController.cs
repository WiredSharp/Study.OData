using Study.OData.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.OData;

namespace Study.OData.Api.Controllers
{
	public class ProductsController : ODataController
	{
		private Product[] products = Enumerable.Range(1,50).Select(i => new Product() { Id = i, Name = $"product {i:00}" }).ToArray();

		[EnableQuery]
		public IHttpActionResult Get()
		{
			return Ok(products);
		}
	}
}