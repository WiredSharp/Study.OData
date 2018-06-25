using Microsoft.OData.Edm;
using Microsoft.OData.Edm.Csdl;
using Microsoft.OData.Edm.Validation;
using NUnit.Framework;
using Study.OData.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Study.OData.Tests
{
	[TestFixture]
	public class SampleModelBuilderTest
	{
		[Test]
		public void i_can_generate_csdl()
		{
			var builder = new SampleModelBuilder();
			IEdmModel model = builder
				//.BuildAddressType()
				//.BuildCategoryType()
				//.BuildCustomerType()
				//.BuildDefaultContainer()
				.BuildCustomerSet()
				.Model;
			using (var writer = XmlWriter.Create(TestContext.Out))
			{
				IEnumerable<EdmError> errors;
				if (!CsdlWriter.TryWriteCsdl(model, writer, CsdlTarget.OData, out errors))
				{
					foreach (EdmError error in errors)
					{
						TestContext.WriteLine(error);
					}
					Assert.Fail("unable to generate CSDL");
				}
			}
		}
	}
}
