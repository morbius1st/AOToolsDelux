using System;

// Solution:     AOToolsDelux
// Project:       AOToolsDelux
// File:             ISchemaDef.cs
// Created:      2021-07-11 (2:40 PM)


namespace CSToolsDelux.Fields.SchemaInfo.SchemaDefinitions
{
	public abstract class ASchemaFieldsApp : ASchemaFields<SchemaAppKey, SchemaDictionaryBase<SchemaAppKey>>
	{
		protected SchemaAppKey defineField<TT>(SchemaAppKey key, string name,
			string desc, TT val,
			RevitUnitType unittype = RevitUnitType.UT_UNDEFINED)
		{
			Fields.Add(key, 
				new SchemaFieldApp<TT>(key, name, desc, val, unittype));

			return key;
		}
	}

	public abstract class ASchemaFieldsRoot : ASchemaFields<SchemaRootKey, SchemaDictionaryBase<SchemaRootKey>>
	{
		protected SchemaRootKey defineField<TT>(SchemaRootKey key, string name,
			string desc, TT val,
			RevitUnitType unittype = RevitUnitType.UT_UNDEFINED)
		{
			Fields.Add(key, 
				new SchemaFieldRoot<TT>(key, name, desc, val, unittype));

			return key;
		}
	}

	public abstract class ASchemaFields<TE, TD>
		where TE : Enum  where TD : SchemaDictionaryBase<TE>, new()
	{
		public abstract TE[] KeyOrder { get; set; }

		public TD Fields { get; protected set; }

		// protected TE defineField<TD, TT>(TE key, string name,
		// 	string desc, TT val,
		// 	RevitUnitType unittype = RevitUnitType.UT_UNDEFINED)
		// {
		// 	Fields.Add(key, 
		// 		new SchemaFieldDef<TT, TE>(key, name, desc, val, unittype));
		//
		// 	return key;
		// }
	}

}