
using System;
using System.Collections.Generic;
using SharedCode.Fields.SchemaInfo.SchemaSupport;
using SharedCode.Fields.SchemaInfo.SchemaDefinitions;

// Solution:     SharedCode
// Project:       SharedCode
// File:             ISchemaFieldDef.cs
// Created:      2022-01-23 (9:25 PM)

// defines the properties of a SchemaField
namespace SharedCode.Fields.SchemaInfo.SchemaFields.FieldsTemplates
{
	public interface AFieldsMembers<TE> where TE : Enum
	{
		KeyValuePair<string, int> Type { get; }
		TE Key { get; }
		int Sequence { get;}
		string Name { get;}
		string Desc { get;}
		FieldUnitType UnitType { get;}
		string Guid { get; }
		Type ValueType { get; }
		string ValueString { get; }
		SchemaFieldDisplayLevel DisplayLevel { get; }
		string DisplayOrder { get; }
		int DisplayWidth { get; }

		AFieldsMembers<TE> Clone();
	}
}