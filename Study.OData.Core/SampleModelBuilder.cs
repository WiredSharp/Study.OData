using Microsoft.OData.Edm;
using Microsoft.OData.Edm.Csdl;
using Microsoft.OData.Edm.Vocabularies;
using System;
using System.Collections.Generic;
using System.Text;

namespace Study.OData.Core
{
    public class SampleModelBuilder
    {
		private const string Namespace = "Sample.NS";
		private readonly EdmModel _model;
		private EdmComplexType _addressType;
		private EdmEnumType _categoryType;
		private EdmEntityType _customerType;
		private EdmEntityContainer _defaultContainer;
		private EdmEntitySet _customerSet;
		private EdmNavigationProperty _friendsProperty;
		private EdmEntityType _orderType;
      private EdmEntitySet _orderSet;
		private EdmNavigationProperty _purchasesProperty;
		private EdmNavigationProperty _intentionsProperty;
		private EdmSingleton _vipCustomer;
		private EdmEntityType _urgentOrderType;
		private EdmComplexType _workAddressType;
		private EdmAction _rateAction;
		private EdmFunction _mostExpensiveFunction;
		private EdmFunctionImport _mostValuableFunctionImport;

		public IEdmModel Model => _model;

		public EdmComplexType AddressType { get { if (_addressType == null) BuildAddressType(); return _addressType; } set => _addressType = value; }
		public EdmComplexType WorkAddressType { get { if (_workAddressType == null) BuildWorkAddressType(); return _workAddressType; } }
		public EdmEnumType CategoryType { get { if (_categoryType == null) BuildCategoryType(); return _categoryType; } set => _categoryType = value; }
		public EdmEntityType CustomerType { get { if (_customerType == null) BuildCustomerType(); return _customerType; } set => _customerType = value; }
		public EdmEntityContainer DefaultContainer { get { if (_defaultContainer == null) BuildDefaultContainer(); return _defaultContainer; } set => _defaultContainer = value; }
		public EdmEntitySet CustomerSet { get { if (_customerSet == null) BuildCustomerSet(); return _customerSet; } set => _customerSet = value; }
		public EdmEntityType OrderType { get { if (_orderType == null) BuildOrderType(); return _orderType; } }
		public EdmEntityType UrgentOrderType { get { if (_urgentOrderType == null) BuildUrgentOrderType(); return _urgentOrderType; } }
		public EdmEntitySet OrderSet { get { if (_orderSet == null) BuildOrderSet(); return _orderSet; } }
		public EdmNavigationProperty FriendsProperty { get { if (_friendsProperty == null) BuildCustomerType(); return _friendsProperty; } }
		public EdmNavigationProperty PurchasesProperty { get { if (_purchasesProperty == null) BuildCustomerType(); return _purchasesProperty; } }
		public EdmNavigationProperty IntentionsProperty { get { if (_intentionsProperty == null) BuildCustomerType(); return _intentionsProperty; } }
		public EdmSingleton VipCustomer { get { if (_vipCustomer == null) BuildVipCustomer(); return _vipCustomer; } }
		public EdmAction RateAction { get { if (_rateAction == null) BuildRateAction(); return _rateAction; } }
		public EdmFunction MostExpensiveFunction { get { if (_mostExpensiveFunction == null) BuildMostExpensiveFunction(); return _mostExpensiveFunction; } }
		public EdmFunctionImport MostValuableFunctionImport { get { if (_mostValuableFunctionImport == null) BuildMostValuableFunctionImport(); return _mostValuableFunctionImport; } }

		public SampleModelBuilder()
		{
			_model = new EdmModel();
		}

		public SampleModelBuilder BuildAddressType()
		{
			_addressType = new EdmComplexType(Namespace, "Address");
			_addressType.AddStructuralProperty("Street", EdmPrimitiveTypeKind.String);
			_addressType.AddStructuralProperty("City", EdmPrimitiveTypeKind.String);
			_addressType.AddStructuralProperty("PostalCode", EdmPrimitiveTypeKind.Int32);
			_model.AddElement(_addressType);
			return this;
		}

		public SampleModelBuilder BuildWorkAddressType()
		{
			_workAddressType = new EdmComplexType(Namespace, "WorkAddress", AddressType);
			_workAddressType.AddStructuralProperty("Company", EdmPrimitiveTypeKind.String);
			_model.AddElement(_workAddressType);
			return this;
		}

		public SampleModelBuilder BuildCategoryType()
		{
			_categoryType = new EdmEnumType(Namespace, "Category", EdmPrimitiveTypeKind.Int64, isFlags: true);
			_categoryType.AddMember("Books", new EdmEnumMemberValue(1L));
			_categoryType.AddMember("Dresses", new EdmEnumMemberValue(2L));
			_categoryType.AddMember("Sports", new EdmEnumMemberValue(4L));
			_model.AddElement(_categoryType);
			return this;
		}

		public SampleModelBuilder BuildCustomerType()
		{
			_customerType = new EdmEntityType(Namespace, "Customer");
			_customerType.AddKeys(_customerType.AddStructuralProperty("Id", EdmPrimitiveTypeKind.Int32, isNullable: false));
			_customerType.AddStructuralProperty("Name", EdmPrimitiveTypeKind.String, isNullable: false);
			_customerType.AddStructuralProperty("Credits",
				 new EdmCollectionTypeReference(new EdmCollectionType(EdmCoreModel.Instance.GetInt64(isNullable: true))));
			_customerType.AddStructuralProperty("Interests", new EdmEnumTypeReference(CategoryType, isNullable: true));
			_customerType.AddStructuralProperty("Address", new EdmComplexTypeReference(AddressType, isNullable: false));
			_friendsProperty = _customerType.AddUnidirectionalNavigation(
				new EdmNavigationPropertyInfo
				{
					ContainsTarget = false,
					Name = "Friends",
					Target = _customerType,
					TargetMultiplicity = EdmMultiplicity.Many
				});
			_purchasesProperty = _customerType.AddUnidirectionalNavigation(
				 new EdmNavigationPropertyInfo
				 {
					 ContainsTarget = false,
					 Name = "Purchases",
					 Target = OrderType,
					 TargetMultiplicity = EdmMultiplicity.Many
				 });
			_intentionsProperty = _customerType.AddUnidirectionalNavigation(
				 new EdmNavigationPropertyInfo
				 {
					 ContainsTarget = true,
					 Name = "Intentions",
					 Target = OrderType,
					 TargetMultiplicity = EdmMultiplicity.Many
				 });
			_model.AddElement(_customerType);
			return this;
		}

		public SampleModelBuilder BuildOrderType()
		{
			_orderType = new EdmEntityType(Namespace, "Order");
			_orderType.AddKeys(_orderType.AddStructuralProperty("Id", EdmPrimitiveTypeKind.Int32, isNullable: false));
			_orderType.AddStructuralProperty("Price", EdmPrimitiveTypeKind.Decimal);
			_model.AddElement(_orderType);
			return this;
		}

		public SampleModelBuilder BuildUrgentOrderType()
		{
			_urgentOrderType = new EdmEntityType(Namespace, "UrgentOrder", OrderType);
			_urgentOrderType.AddStructuralProperty("Deadline", EdmPrimitiveTypeKind.Date);
			_model.AddElement(_urgentOrderType);
			return this;
		}

		public SampleModelBuilder BuildRateAction()
		{
			_rateAction = new EdmAction(Namespace, "Rate",
				 returnType: null, isBound: true, entitySetPathExpression: null);
			_rateAction.AddParameter("customer", new EdmEntityTypeReference(CustomerType, false));
			_rateAction.AddParameter("rating", EdmCoreModel.Instance.GetInt32(false));
			_model.AddElement(_rateAction);
			return this;
		}

		public SampleModelBuilder BuildMostExpensiveFunction()
		{
			_mostExpensiveFunction = new EdmFunction(Namespace, "MostExpensive",
				 new EdmEntityTypeReference(OrderType, true), isBound: false, entitySetPathExpression: null, isComposable: true);
			_model.AddElement(_mostExpensiveFunction);
			return this;
		}

		public SampleModelBuilder BuildMostValuableFunctionImport()
		{
			_mostValuableFunctionImport = DefaultContainer.AddFunctionImport("MostValuable", MostExpensiveFunction, new EdmPathExpression("Orders"));
			return this;
		}

		public SampleModelBuilder BuildOrderSet()
		{
			_orderSet = DefaultContainer.AddEntitySet("Orders", OrderType);
			return this;
		}

		public SampleModelBuilder BuildCustomerSet()
		{
			_customerSet = DefaultContainer.AddEntitySet("Customers", CustomerType);
			_customerSet.AddNavigationTarget(FriendsProperty, _customerSet);
			_customerSet.AddNavigationTarget(PurchasesProperty, OrderSet);
			return this;
		}

		public SampleModelBuilder BuildVipCustomer()
		{
			_vipCustomer = DefaultContainer.AddSingleton("VipCustomer", CustomerType);
			return this;
		}

		public SampleModelBuilder BuildAnnotations()
		{
			_model.AddVocabularyAnnotation(new EdmVocabularyAnnotation(CustomerSet, 
				new EdmTerm(Namespace, "MaxCount", EdmCoreModel.Instance.GetInt32(true))
				, new EdmIntegerConstant(10000000L)));
			EdmVocabularyAnnotation annotation = new EdmVocabularyAnnotation(CustomerType,
				new EdmTerm(Namespace, "KeyName", EdmCoreModel.Instance.GetString(true))
				, new EdmStringConstant("Id"));
			annotation.SetSerializationLocation(_model, EdmVocabularyAnnotationSerializationLocation.Inline);
			_model.AddVocabularyAnnotation(annotation);
			EdmVocabularyAnnotation annotation2 = new EdmVocabularyAnnotation(CustomerType.FindProperty("Name"),
				new EdmTerm(Namespace, "Width", EdmCoreModel.Instance.GetInt32(true))
				, new EdmIntegerConstant(10L));
			annotation2.SetSerializationLocation(_model, EdmVocabularyAnnotationSerializationLocation.Inline);
			_model.AddVocabularyAnnotation(annotation2);
			return this;
		}

		public SampleModelBuilder BuildDefaultContainer()
		{
			_defaultContainer = new EdmEntityContainer(Namespace, "DefaultContainer");
			_model.AddElement(_defaultContainer);
			return this;
		}
	}
}
