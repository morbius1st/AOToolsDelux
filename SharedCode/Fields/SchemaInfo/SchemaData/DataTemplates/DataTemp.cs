
using System;
using System.Collections.Generic;
using SharedCode.Fields.SchemaInfo.SchemaData.DataTemplate;
using SharedCode.Fields.SchemaInfo.SchemaFields.FieldsTemplates;
using SharedCode.Fields.SchemaInfo.SchemaSupport;

// Solution:     SharedCode
// Project:       SharedCode
// File:         SchemaDataFieldDef.cs
// Created:      2022-01-25 (11:08 PM)

namespace SharedCode.Fields.SchemaInfo.SchemaData.DataTemplates
{

	public class DataMember<TE, TD> : ADataMember<TE> where TE : Enum
	{
		public DataMember() { }

		public DataMember(TD value, IFieldsTemp<TE> fieldsTemp)
		{
			Value = value;
			FieldsTemp = fieldsTemp;
			ValueType = typeof(TD);
			Key = fieldsTemp.Key;
			
		}
		

		public override TE Key						{ get; protected set; }
		public TD Value								{ get; set; }
		public override string ValueString =>		Value.ToString();
		public override Type ValueType				{ get; protected set; }
		public override IFieldsTemp<TE> FieldsTemp { get; protected set; }


		public override ADataMember<TE> Clone()
		{
			DataMember<TE, TD> copy = new DataMember<TE, TD>();

			copy.Key = Key;
			copy.FieldsTemp = FieldsTemp;
			copy.Value = Value;

			return copy;
		}

		public override string ToString()
		{
			return $"value| to string| {Value.ToString()}";
		}

	}
}