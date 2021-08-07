// Solution:     AOToolsDelux
// Project:       AOToolsDelux
// File:             ISchemaDef.cs
// Created:      2021-07-11 (2:40 PM)

using System;
using System.Collections.Generic;
using AOTools.Cells.ExStorage;

namespace AOTools.Cells.SchemaDefinition
{
	public interface ISchemaDef
	{
		// TE[] KeyOrder { get; }
		//
		// TD DefaultFields { get; }
	}

	public abstract class ASchemaDef<TE, TD> : ISchemaDef
		where TE : Enum  where TD : SchemaDictionaryBase<TE>, new()
	{
		public TE[] KeyOrder { get; set; }

		public TD Fields { get; protected set; }

		protected TE defineField<TD>(TE key, string name,
			string desc, ExData<TD> val,
			RevitUnitType unittype = RevitUnitType.UT_UNDEFINED)
		{
			Fields.Add(key, 
				new SchemaFieldDef<TE>(key, name, desc, val, unittype));

			return key;
		}

		// public abstract TD DefaultFields { get;}

		// public void Init()
		// {
		// 	KeyOrder = new TE[DefaultFields.Count];
		//
		// 	int j = 0;
		//
		// 	foreach (KeyValuePair<TE, SchemaFieldDef<TE>> kvp in DefaultFields)
		// 	{
		// 		KeyOrder[j++] = kvp.Key;
		// 	}
		//
		// 	j = 0;
		//
		// 	KeyOrderX = new Enum[DefaultFields.Count];
		//
		// 	foreach (KeyValuePair<TE, SchemaFieldDef<TE>> kvp in DefaultFields)
		// 	{
		// 		KeyOrderX[j++] = kvp.Key;
		// 	}
		// }
	}

}