// Solution:     AOToolsDelux
// Project:       AOToolsDelux
// File:             SchemaDictionaryBase.cs
// Created:      2021-07-03 (11:28 PM)

using System;
using System.Collections.Generic;

namespace AOTools.Cells.SchemaDefinition
{
	public class SchemaDictionaryBase<TE> : Dictionary<TE, SchemaFieldDef<TE>>  where TE : Enum
	{
		public SchemaDictionaryBase() { }

		public SchemaDictionaryBase(int capacity) : base(capacity) { }

		public TC Clone<TC>(TC original) where TC : SchemaDictionaryBase<TE>, new()
		{
			TC copy = new TC();

			foreach (KeyValuePair<TE, SchemaFieldDef<TE>> kvp in original)
			{
				// kvp.Value.Clone();
				//
				// copy.Add(kvp.Key, new SchemaFieldDef{Value = kvp.Value });

				copy.Add(kvp.Key, (SchemaFieldDef<TE>) kvp.Value.Clone());
			}
			return copy;
		}
	}
}