using System.Collections.Generic;

namespace Study.OData.Api.Models
{
	public class Address
	{
		public string Country { get; set; }
		public string City { get; set; }

		public IDictionary<string, object> Dynamics { get; set; }
	}
}