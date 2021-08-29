// Solution:     AOToolsDelux
// Project:       AOToolsDelux
// File:             SchemaDictionaryBase.cs
// Created:      2021-07-03 (11:28 PM)

using System;
using System.Collections.Generic;

namespace CSToolsDelux.Fields.SchemaInfo.SchemaDefinitions
{
	public class SchemaDictionaryBase<TE> : Dictionary<TE, ISchemaFieldDef<TE>>  where TE : Enum
	{
		public TC Clone<TC>(TC original) where TC : SchemaDictionaryBase<TE>, new()
		{
			TC copy = new TC();

			foreach (KeyValuePair<TE, ISchemaFieldDef<TE>> kvp in original)
			{
				copy.Add(kvp.Key, (ISchemaFieldDef<TE>) kvp.Value.Clone());
			}

			return copy;
		}
	}
}