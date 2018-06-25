using Microsoft.OData.Edm;
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

		public IEdmModel Model => _model;

		public EdmComplexType AddressType { get { if (_addressType == null) BuildAddressType(); return _addressType; } set => _addressType = value; }
		public EdmEnumType CategoryType { get { if (_categoryType == null) BuildCategoryType(); return _categoryType; } set => _categoryType = value; }
		public EdmEntityType CustomerType { get { if (_customerType == null) BuildCustomerType(); return _customerType; } set => _customerType = value; }
		public EdmEntityContainer DefaultContainer { get { if (_defaultContainer == null) BuildDefaultContainer(); return _defaultContainer; } set => _defaultContainer = value; }
		public EdmEntitySet CustomerSet { get { if (_customerSet == null) BuildCustomerSet(); return _customerSet; } set => _customerSet = value; }

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
			_model.AddElement(_customerType);
			return this;
		}

		public SampleModelBuilder BuildDefaultContainer()
		{
			_defaultContainer = new EdmEntityContainer(Namespace, "DefaultContainer");
			_model.AddElement(_defaultContainer);
			return this;
		}

		public SampleModelBuilder BuildCustomerSet()
		{
			_customerSet = DefaultContainer.AddEntitySet("Customers", CustomerType);
			return this;
		}
	}
}
