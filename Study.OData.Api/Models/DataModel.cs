using Study.OData.Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Study.OData.Api.Models
{
	internal static class DataModel
	{
		static DataModel()
		{
			Threads = Enumerable.Range(1, 5).Select(i => new MessageThread() { Id = i, Name = $"Thread_{i:00}" }).ToList();
			Boxes = Enumerable.Range(1, 5).Select(i => new Box() { Id = i, Name = $"Box_{i:00}" }).ToList();
			Registrations = Enumerable.Range(1, 50).Select(i => new Registration() { ThreadId = i / 10, ParticipantLogin = $"User_{i%5:00}", BoxId = i / 10 }).ToList();
			Participants = Enumerable.Range(1, 5).Select(i => new Participant() { Login = $"User_{i:00}", Name = $"John {i:00}" }).ToList();
		}

		public static List<MessageThread> Threads { get; private set; }
		public static List<Box> Boxes { get; private set; }
		public static List<Registration> Registrations { get; private set; }
		public static List<Participant> Participants { get; private set; }
	}
}