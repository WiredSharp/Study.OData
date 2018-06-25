using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Study.OData.Api.Models
{
	public class Customer
	{
		public int CustomerId { get; set; } // structural property, primitive type, key
		public Address Location { get; set; } // structural property, complex type
		public IList<Order> Orders { get; set; } // navigation property, entity type
	}
}