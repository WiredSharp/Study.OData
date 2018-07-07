using Microsoft.OData.Edm;
using Microsoft.OData.Edm.Csdl;
using Microsoft.OData.Edm.Validation;
using Study.OData.Api.Models;
using Study.OData.Client.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.OData.Builder;
using System.Xml;

namespace Study.OData.Client
{
	internal static class EdmBuilder
	{
		public static IEdmModel NewNonConventionalBuildModel()
		{
			var builder = new ODataModelBuilder();
			var color = builder.EnumType<Color>();
			color.Member(Color.Red);
			color.Member(Color.Blue);
			color.Member(Color.Green);
			var address = builder.ComplexType<Address>();
			address.Property(a => a.Country);
			address.Property(a => a.City);
			address.HasDynamicProperties(a => a.Dynamics);
			var subAddress = builder.ComplexType<SubAddress>().DerivesFrom<Address>();
			subAddress.Property(s => s.Street);
			var customer = builder.EntityType<Customer>();
			customer.HasKey(c => c.CustomerId);
			customer.ComplexProperty(c => c.Location);
			customer.HasMany(c => c.Orders);
			var order = builder.EntityType<Order>();
			order.HasKey(o => o.OrderId);
			order.Property(o => o.Token);
			builder.EntityType<Registration>().HasKey(r => new { r.ParticipantId, r.ThreadId });
			builder.EntityType<Participant>().HasKey(p => p.Login);
			builder.EntityType<Thread>().HasKey(t => t.Id);
			builder.EntityType<Box>().HasKey(b => b.Id);
			builder.EntitySet<Thread>("Threads");
			builder.EntitySet<Box>("Boxes");
			builder.EntitySet<Participant>("Participants");
			return builder.GetEdmModel();
		}

		public static IEdmModel NewConventionalBuildModel()
		{
			var builder = new ODataConventionModelBuilder();
			builder.EnumType<Color>();
			builder.ComplexType<Address>();
			builder.EntityType<Order>();
			builder.EntityType<Customer>();
			builder.EntityType<Registration>().HasKey(r => new { r.ParticipantId, r.ThreadId });
			builder.EntityType<Participant>().HasKey(p => p.Login);
			builder.EntityType<Thread>().HasKey(t => t.Id);
			builder.EntityType<Box>().HasKey(b => b.Id);
			builder.EntitySet<Thread>("Threads");
			builder.EntitySet<Box>("Boxes");
			builder.EntitySet<Participant>("Participants");
			return builder.GetEdmModel();
		}

		public static bool WriteCsdl(IEdmModel model, string path)
		{
			using (var file = new StreamWriter(path))
			using (var writer = XmlWriter.Create(file, new XmlWriterSettings() { Indent = true }))
			{
				IEnumerable<EdmError> errors;
				if (!CsdlWriter.TryWriteCsdl(model, writer, CsdlTarget.OData, out errors))
				{
					foreach (EdmError error in errors)
					{
						Console.WriteLine(error);
					}
					return false;
				}
			}
			return true;
		}

		private static IEdmModel CreateModelWith(ODataModelBuilder builder)
		{
			return builder.GetEdmModel();
		}
	}
}
