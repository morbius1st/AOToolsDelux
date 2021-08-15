// Solution:     AOToolsDelux
// Project:       AOToolsDelux
// File:             ISchemaDef.cs
// Created:      2021-07-11 (2:40 PM)

using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AOTools.Cells.SchemaDefinition
{
	public abstract class ASchemaDef<TE, TD>
		where TE : Enum  where TD : SchemaDictionaryBase<TE>, new()
	{
		public abstract TE[] KeyOrder { get; set; }

		public TD Fields { get; protected set; }

		protected TE defineField<TD>(TE key, string name,
			string desc, dynamic val,
			RevitUnitType unittype = RevitUnitType.UT_UNDEFINED)
		{
			Fields.Add(key, 
				new SchemaFieldDef<TE>(key, name, desc, val, unittype));

			return key;
		}
	}

}