// Solution:     AOToolsDelux
// Project:       AOToolsDelux
// File:             ISchemaDef.cs
// Created:      2021-07-11 (2:40 PM)

using System;
using System.Collections.Generic;

namespace AOTools.Cells.SchemaDefinition
{
	public interface ISchemaDef<TD> // where TE : Enum  where TD : SchemaDictionaryBase<TE>
	{
		// TE[] KeyOrder { get; }

		TD DefaultFields { get; }
	}

	public abstract class ASchemaDef<TE> where TE : Enum
	{
		public abstract SchemaDictionaryBase<string> DefaultFields { get; }

		public string[] FIELD_NAMES;

		public SchemaFieldDef this[string key]
		{
			get
			{
				return DefaultFields[key];
			}
		}

		public abstract string this[TE key] { get; }

	}

}