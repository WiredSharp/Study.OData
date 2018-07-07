using Study.OData.Api.Models;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Http;

namespace Study.OData.Api.Controllers
{
	public class ProductsController : ApiController
	{
		private Product[] products = Enumerable.Range(1,50).Select(i => new Product() { Id = i, Name = $"product {i:00}" }).ToArray();

		[HttpGet]
		public Product[] GetAll()
		{
			Regex uriParser = new Regex(@"\(\s*(\w+)\s*=\s*('[^']+'|""[^ ""]+"" |[^, '""\s]+)\s*(,\s*(\w+)\s*=\s*('[^ ']+' | ""[^""] + ""|[^,'""\s] +)|) *\)");
			return new Product[] { new Product { Name = Request.RequestUri.Segments.Last() } };
		}
	}
}