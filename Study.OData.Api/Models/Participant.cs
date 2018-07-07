using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Study.OData.Client.Models
{
	public class Participant
	{
		[Key]
		public string Login { get; set; }

		public string Name { get; set; }

		public IList<Registration> Registrations { get; set; }

		public override string ToString()
		{
			return Name;
		}
	}
}
