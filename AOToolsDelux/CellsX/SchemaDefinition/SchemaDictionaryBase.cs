using System;
using System.Collections.Generic;

// Solution:     AOToolsDelux
// Project:       AOToolsDelux
// File:             SchemaDictionaryBase.cs
// Created:      2021-07-03 (11:28 PM)

namespace AOTools.Cells.SchemaDefinition
{
	public abstract class XsDictionaryBase<TE, TD> : Dictionary<TE, TD> 
		// where TD : ICloneable
		where  TE : Enum { } 

	// public abstract class SchemaDictionaryBase<TE> : Dictionary<TE, SchemaFieldDef<TE>>  where TE : Enum
	public abstract class SchemaDictionaryBase<TE> : XsDictionaryBase<TE, ISchemaFieldDef<TE>>  where TE : Enum
	{
		// public SchemaDictionaryBase() { }
		//
		// public SchemaDictionaryBase(int capacity) : base(capacity) { }

		public TC Clone<TC>(TC original) where TC : SchemaDictionaryBase<TE>, new()
		{
			TC copy = new TC();
		
			foreach (KeyValuePair<TE, ISchemaFieldDef<TE>> kvp in original)
			{
				copy.Add(kvp.Key, kvp.Value.Clone());
			}
			return copy;
		}
	}
}