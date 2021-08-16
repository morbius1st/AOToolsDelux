// Solution:     AOToolsDelux
// Project:       AOToolsDelux
// File:             ISchemaDef.cs
// Created:      2021-07-11 (2:40 PM)

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AOTools.Cells.SchemaDefinition;


namespace AOTools.Cells.SchemaDefinition2
{
	public abstract class ASchemaDef2<TE, TD>
		where TE : Enum  where TD : SchemaDictionaryBase2<TE>, new()
	{
		public TE[] KeyOrder { get; set; }

		public TD Fields { get; protected set; }

		protected TE defineField<TD>(TE key, string name,
			string desc, TD val,
			RevitUnitType unittype = RevitUnitType.UT_UNDEFINED)
		{
			Fields.Add(key, 
				new SchemaFieldDef2<TE, TD>(key, name, desc, val, unittype));

			return key;
		}
	}

}