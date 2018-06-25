using System;

namespace Study.OData.Api.Models
{
	public class Order
	{
		public int OrderId { get; set; } // structural property, primitive type, key
		public Guid Token { get; set; } // structural property, primitive type
	}
}