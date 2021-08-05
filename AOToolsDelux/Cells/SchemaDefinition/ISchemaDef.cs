// Solution:     AOToolsDelux
// Project:       AOToolsDelux
// File:             ISchemaDef.cs
// Created:      2021-07-11 (2:40 PM)

using System;
using System.Collections.Generic;

namespace AOTools.Cells.SchemaDefinition
{
	public interface ISchemaDef<TE> // where TE : Enum  where TD : SchemaDictionaryBase<TE>
	{
		// TE[] KeyOrder { get; }

		// TD DefaultFields { get; }
	}

	public abstract class ASchemaDef<TE, TD> : ISchemaDef<TE> 
		where TE : Enum  where TD : SchemaDictionaryBase<TE>, new()
	{
		public TE[] KeyOrder { get; protected set; }

		public TD Fields { get; protected set; }

		protected TE defineField<TD>(TE key, string name,
			string desc, RevitUnitType unittype = RevitUnitType.UT_UNDEFINED)
		{
			Fields.Add(key, 
				new SchemaFieldDef<TE,TD>(key, name, desc, unittype));

			return key;
		}
	}

}