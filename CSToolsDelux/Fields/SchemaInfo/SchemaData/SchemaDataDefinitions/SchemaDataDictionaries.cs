// Solution:     AOToolsDelux
// Project:       AOToolsDelux
// File:             SchemaDictionaryBase.cs
// Created:      2021-07-03 (11:28 PM)

using System;
using System.Collections.Generic;
using CSToolsDelux.Fields.SchemaInfo.SchemaDefinitions;

namespace CSToolsDelux.Fields.SchemaInfo.SchemaData.SchemaDataDefinitions
{

	public class SchemaDataDictCell : SchemaDataDictionaryBase<SchemaCellKey> { }

	public class SchemaDataDictApp : SchemaDataDictionaryBase<SchemaAppKey> { }

	public class SchemaDataDictRootApp : SchemaDataDictionaryBase<SchemaRootAppKey> { }

	public class SchemaDataDictRoot : SchemaDataDictionaryBase<SchemaRootKey> { }

	public class SchemaDataDictionaryBase<TE> : Dictionary<TE, ASchemaDataFieldDef<TE>>  where TE : Enum
	{
		public TC Clone<TC>() where TC : SchemaDataDictionaryBase<TE>, new()
		{
			TC copy = new TC();

			foreach (KeyValuePair<TE, ASchemaDataFieldDef<TE>> field in this)
			{
				copy.Add(field.Key, field.Value);
			}

			return copy;
		}


		// public TC Clone<TC>(TC original) where TC : SchemaDataDictionaryBase<TE>, new()
		// {
		// 	TC copy = new TC();
		//
		// 	foreach (KeyValuePair<TE, ASchemaDataFieldDef<TE>> kvp in original)
		// 	{
		// 		copy.Add(kvp.Key, (ASchemaDataFieldDef<TE>) kvp.Value.Clone());
		// 	}
		//
		// 	return copy;
		// }
	}
}