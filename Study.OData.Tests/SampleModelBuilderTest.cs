using Microsoft.OData.Edm;
using Microsoft.OData.Edm.Csdl;
using Microsoft.OData.Edm.Validation;
using Microsoft.OData.Edm.Vocabularies;
using NUnit.Framework;
using Study.OData.Core;
using System;
using System.Collections.Generic;
using System.Linq;
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
			IEdmModel model = NewModel();
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

		[Test]
		public void i_can_find_an_entityset()
		{
			IEdmModel model = NewModel();
			var customerSet = model.FindDeclaredEntitySet("Customers");
			Assert.IsNotNull(customerSet, "customer set not found");
			TestContext.WriteLine($"{customerSet.NavigationSourceKind()} '{customerSet.Name}'");
		}


		[Test]
		public void i_can_find_a_singleton()
		{
			IEdmModel model = NewModel();
			var vipCustomer = model.FindDeclaredNavigationSource("VipCustomer");
			Assert.IsNotNull(vipCustomer, "Vip Customer not found");
			TestContext.WriteLine($"{vipCustomer.NavigationSourceKind()} '{vipCustomer.Name}'");
		}

		[Test]
		public void FullTest()
		{
			IEdmModel model = NewModel();
			// Find a type (complex or entity or enum).
			var orderType = model.FindDeclaredType("Sample.NS.Order");
			TestContext.WriteLine("{0} type '{1}' found.", orderType.TypeKind, orderType.FullName());
			var addressType = model.FindDeclaredType("Sample.NS.Address");
			TestContext.WriteLine("{0} type '{1}' found.", addressType.TypeKind, addressType);
			// Find derived type of some type.
			var workAddressType = model.FindAllDerivedTypes((IEdmStructuredType)addressType).Single();
			TestContext.WriteLine("Type '{0}' is the derived from '{1}'.", ((IEdmSchemaType)workAddressType).Name, addressType.Name);
			// Find an operation.
			var rateAction = model.FindDeclaredOperations("Sample.NS.Rate").Single();
			TestContext.WriteLine("{0} '{1}' found.", rateAction.SchemaElementKind, rateAction.Name);
			// Find an operation import.
			var mostValuableFunctionImport = model.FindDeclaredOperationImports("MostValuable").Single();
			TestContext.WriteLine("{0} '{1}' found.", mostValuableFunctionImport.ContainerElementKind, mostValuableFunctionImport.Name);
			// Find an annotation and get its value.
			var customerSet = model.FindDeclaredEntitySet("Customers");
			var maxCountAnnotation = model.FindDeclaredVocabularyAnnotations(customerSet).Single();
			var maxCountValue = ((IEdmIntegerValue)maxCountAnnotation.Value).Value;
			TestContext.WriteLine("'{0}' = '{1}' on '{2}'", maxCountAnnotation.Term.Name, maxCountValue, ((IEdmEntitySet)maxCountAnnotation.Target).Name);
		}

		private static IEdmModel NewModel()
		{
			var builder = new SampleModelBuilder();
			IEdmModel model = builder
				//.BuildAddressType()
				//.BuildCategoryType()
				//.BuildCustomerType()
				//.BuildDefaultContainer()
				.BuildCustomerSet()
				.BuildVipCustomer()
				.BuildUrgentOrderType()
				.BuildWorkAddressType()
				.BuildRateAction()
				.BuildMostExpensiveFunction()
				.BuildMostValuableFunctionImport()
				.BuildAnnotations()
				.Model;
			return model;
		}
	}
}
