// Solution:     AOToolsDelux
// Project:       AOToolsDelux
// File:             SchemaDictionaryBase.cs
// Created:      2021-07-03 (11:28 PM)

using System;
using System.Collections.Generic;
using AOTools.Cells.SchemaDefinition;

namespace AOTools.Cells.SchemaDefinition2
{
	public class SchemaDictionaryBase2<TE> : Dictionary<TE, ISchemaFieldDef2<TE>>  where TE : Enum
	{
		public TC Clone<TC>(TC original) where TC : SchemaDictionaryBase2<TE>, new()
		{
			TC copy = new TC();

			foreach (KeyValuePair<TE, ISchemaFieldDef2<TE>> kvp in original)
			{
				copy.Add(kvp.Key, (ISchemaFieldDef2<TE>) kvp.Value.Clone());
			}

			return copy;
		}
	}
}