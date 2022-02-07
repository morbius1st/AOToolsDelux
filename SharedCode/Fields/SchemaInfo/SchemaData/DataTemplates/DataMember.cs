
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

	public class DataMembers<TE, TD> : ADataMembers<TE> where TE : Enum
	{
		public DataMembers() { }

		public DataMembers(TD value, IFieldsMembers<TE> fieldsMembers)
		{
			Value = value;
			FieldsMembers = fieldsMembers;
			ValueType = typeof(TD);
			Key = fieldsMembers.Key;
			
		}
		

		public override TE Key						{ get; protected set; }
		public TD Value								{ get; set; }
		public override string ValueString =>		Value.ToString();
		public override Type ValueType				{ get; protected set; }
		public override IFieldsMembers<TE> FieldsMembers { get; protected set; }


		public override ADataMembers<TE> Clone()
		{
			DataMembers<TE, TD> copy = new DataMembers<TE, TD>();

			copy.Key = Key;
			copy.FieldsMembers = FieldsMembers;
			copy.Value = Value;

			return copy;
		}

		public override string ToString()
		{
			return $"value| to string| {Value.ToString()}";
		}

	}
}