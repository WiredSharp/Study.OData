using Microsoft.OData.Edm;
using Microsoft.OData.Edm.Csdl;
using Study.OData.Api.Models;
using Study.OData.Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.OData.Builder;
using System.Web.OData.Query;

namespace Study.OData.Api
{
	internal class StudyOdataModelBuilder
	{
		private const string Namespace = "OdataStudyNS";

		public static IEdmModel CreateEdm()
		{
			var model = new EdmModel();

			// Complex Type
			EdmComplexType address = new EdmComplexType(Namespace, "Address");
			address.AddStructuralProperty("Country", EdmPrimitiveTypeKind.String);
			address.AddStructuralProperty("City", EdmPrimitiveTypeKind.String);
			model.AddElement(address);

			EdmComplexType subAddress = new EdmComplexType(Namespace, "SubAddress", address);
			subAddress.AddStructuralProperty("Street", EdmPrimitiveTypeKind.String);
			model.AddElement(subAddress);

			// Enum type
			EdmEnumType color = new EdmEnumType(Namespace, "Color");
			color.AddMember(new EdmEnumMember(color, "None", new EdmEnumMemberValue(0)));
			color.AddMember(new EdmEnumMember(color, "Red", new EdmEnumMemberValue(1)));
			color.AddMember(new EdmEnumMember(color, "Blue", new EdmEnumMemberValue(2)));
			color.AddMember(new EdmEnumMember(color, "Green", new EdmEnumMemberValue(3)));
			model.AddElement(color);

			// Entity type
			EdmEntityType customer = new EdmEntityType(Namespace, "Customer");
			customer.AddKeys(customer.AddStructuralProperty("CustomerId", EdmPrimitiveTypeKind.Int32));
			customer.AddStructuralProperty("Location", new EdmComplexTypeReference(address, isNullable: true));
			model.AddElement(customer);

			EdmEntityType vipCustomer = new EdmEntityType(Namespace, "VipCustomer", customer);
			vipCustomer.AddStructuralProperty("FavoriteColor", new EdmEnumTypeReference(color, isNullable: false));
			model.AddElement(vipCustomer);

			EdmEntityType order = new EdmEntityType(Namespace, "Order");
			order.AddKeys(order.AddStructuralProperty("OrderId", EdmPrimitiveTypeKind.Int32));
			order.AddStructuralProperty("Token", EdmPrimitiveTypeKind.Guid);
			model.AddElement(order);

			EdmEntityContainer container = new EdmEntityContainer(Namespace, "Container");
			EdmEntitySet customers = container.AddEntitySet("Customers", customer);
			EdmEntitySet orders = container.AddEntitySet("Orders", order);
			model.AddElement(container);

			// EdmSingleton mary = container.AddSingleton("Mary", customer);

			// navigation properties
			EdmNavigationProperty ordersNavProp = customer.AddUnidirectionalNavigation(
				 new EdmNavigationPropertyInfo
				 {
					 Name = "Orders",
					 TargetMultiplicity = EdmMultiplicity.Many,
					 Target = order
				 });
			customers.AddNavigationTarget(ordersNavProp, orders);

			// function
			IEdmTypeReference stringType = EdmCoreModel.Instance.GetPrimitive(EdmPrimitiveTypeKind.String, isNullable: false);
			IEdmTypeReference intType = EdmCoreModel.Instance.GetPrimitive(EdmPrimitiveTypeKind.Int32, isNullable: false);

			EdmFunction getFirstName = new EdmFunction(Namespace, "GetFirstName", stringType, isBound: true, entitySetPathExpression: null, isComposable: false);
			getFirstName.AddParameter("entity", new EdmEntityTypeReference(customer, false));
			model.AddElement(getFirstName);

			EdmFunction getNumber = new EdmFunction(Namespace, "GetOrderCount", intType, isBound: false, entitySetPathExpression: null, isComposable: false);
			model.AddElement(getNumber);
			container.AddFunctionImport("GetOrderCount", getNumber);

			// action
			EdmAction calculate = new EdmAction(Namespace, "CalculateOrderPrice", returnType: null, isBound: true, entitySetPathExpression: null);
			calculate.AddParameter("entity", new EdmEntityTypeReference(customer, false));
			model.AddElement(calculate);

			EdmAction change = new EdmAction(Namespace, "ChangeCustomerById", returnType: null, isBound: false, entitySetPathExpression: null);
			change.AddParameter("Id", intType);
			model.AddElement(change);
			container.AddActionImport("ChangeCustomerById", change);


			return model;
		}

		public static IEdmModel CreateConventionalEdm()
		{
			var builder = new ODataConventionModelBuilder();
			builder.EntityType<Registration>()
				.HasKey(r => new { r.ParticipantLogin, r.ThreadId }).Filter(QueryOptionSetting.Allowed).Expand(SelectExpandType.Allowed);

			builder.EntityType<Participant>();
			builder.EntityType<MessageThread>().Filter(QueryOptionSetting.Allowed).Expand(SelectExpandType.Allowed);
			builder.EntityType<Box>().Filter(QueryOptionSetting.Allowed);
			builder.EntitySet<MessageThread>("Threads");
			builder.EntitySet<Box>("Boxes");
			builder.EntitySet<Participant>("Participants");
			builder.EntitySet<Registration>("Registrations");
			return builder.GetEdmModel();

		}
	}
}