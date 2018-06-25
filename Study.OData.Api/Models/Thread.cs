using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Study.OData.Client.Models
{
	class Thread
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public Collection<Participant> Participants { get; set; }

		public override string ToString()
		{
			return Name;
		}
	}
}
