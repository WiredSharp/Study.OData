using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Study.OData.Client.Models
{
	public class MessageThread
	{
		[Key]
		public int Id { get; set; }

		public string Name { get; set; }

		public IList<Participant> Participants { get; set; }

		public override string ToString()
		{
			return Name;
		}
	}
}
