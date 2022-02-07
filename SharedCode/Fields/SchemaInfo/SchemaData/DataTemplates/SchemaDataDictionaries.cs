

// Solution:     AOToolsDelux
// Project:       AOToolsDelux
// File:             SchemaDictionaryBase.cs
// Created:      2021-07-03 (11:28 PM)


using System;
using System.Collections.Generic;
using SharedCode.Fields.SchemaInfo.SchemaData.SchemaDataDefinitions;
using SharedCode.Fields.SchemaInfo.SchemaDefinitions;

namespace CSToolsDelux.Fields.SchemaInfo.SchemaData.SchemaDataDefinitions
{

	public class SchemaDataDictCell : SchemaDataDictionary<SchemaCellKey> { }

	public class SchemaDataDictRoot : SchemaDataDictionary<SchemaRootKey> { }

	public class SchemaDataDictLock : SchemaDataDictionary<SchemaLockKey> { }

	public class SchemaDataDictionary<TE> : Dictionary<TE, ASchemaDataFieldDef<TE>>  where TE : Enum
	{
		// public TC Clone<TC>() where TC : SchemaDataDictionaryBase<TE>, new()
		// {
		// 	TC copy = new TC();
		//
		// 	foreach (KeyValuePair<TE, ASchemaDataFieldDef<TE>> field in this)
		// 	{
		// 		copy.Add(field.Key, field.Value);
		// 	}
		//
		// 	return copy;
		// }

	}
}