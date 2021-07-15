// Solution:     AOToolsDelux
// Project:       AOToolsDelux
// File:             SchemaDictionaryBase.cs
// Created:      2021-07-03 (11:28 PM)

using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace AOTools.Cells.SchemaDefinition
{
	public class SchemaDictionaryBase<TU> : Dictionary<TU, SchemaFieldDef>

	{
		public SchemaDictionaryBase() { }

		public SchemaDictionaryBase(int capacity) : base(capacity) { }
		//
		// public TC Clone<TC>(TC original) where TC : SchemaDictionaryBase<TU>, new()
		// {
		// 	TC copy = new TC();
		//
		// 	foreach (KeyValuePair<TU, SchemaFieldDef> kvp in original)
		// 	{
		// 		// kvp.Value.Clone();
		// 		//
		// 		// copy.Add(kvp.Key, new SchemaFieldDef{Value = kvp.Value });
		//
		// 		copy.Add(kvp.Key, kvp.Value.Clone());
		// 	}
		//
		// 	return copy;
		// }

		public TC Clone<TC>() where TC : SchemaDictionaryBase<TU>, new()
		{
			TC copy = new TC();

			foreach (KeyValuePair<TU, SchemaFieldDef> kvp in this)
			{
				copy.Add(kvp.Key, kvp.Value.Clone());
			}

			return copy;
		}
	}
}