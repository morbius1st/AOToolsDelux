using System;

// Solution:     AOToolsDelux
// Project:       AOToolsDelux
// File:             ISchemaDef.cs
// Created:      2021-07-11 (2:40 PM)


namespace CSToolsDelux.Fields.SchemaInfo.SchemaDefinitions
{
	public abstract class ASchemaFieldsRootApp : ASchemaFields<SchemaRootAppKey, SchemaDictionaryBase<SchemaRootAppKey>>
	{
		protected SchemaRootAppKey defineField<TT>(SchemaRootAppKey key, string name,
			string desc, TT val,
			RevitUnitType unittype = RevitUnitType.UT_UNDEFINED)
		{
			Fields.Add(key, 
				new SchemaFieldRootApp<TT>(key, name, desc, val, unittype));

			return key;
		}
	}

	public abstract class ASchemaFieldsCell : ASchemaFields<SchemaCellKey, SchemaDictionaryBase<SchemaCellKey>>
	{
		protected SchemaCellKey defineField<TT>(SchemaCellKey key, string name,
			string desc, TT val,
			RevitUnitType unittype = RevitUnitType.UT_UNDEFINED)
		{
			Fields.Add(key, 
				new SchemaFieldCell<TT>(key, name, desc, val, unittype));

			return key;
		}
	}

	public abstract class ASchemaFields<TE, TD>
		where TE : Enum  where TD : SchemaDictionaryBase<TE>, new()
	{
		public abstract TE[] KeyOrder { get; set; }

		public TD Fields { get; protected set; }

		public  ISchemaFieldDef<TE> this[TE key] => Fields[key];

	}

}