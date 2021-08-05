// Solution:     AOToolsDelux
// Project:       AOToolsDelux
// File:             ISchemaDef.cs
// Created:      2021-07-11 (2:40 PM)

using System;
using System.Collections.Generic;

namespace AOTools.Cells2.SchemaDefinition
{
	public interface ISchemaDef<TE, TD> // where TE : Enum  where TD : SchemaDictionaryBase<TE>
	{
		TE[] KeyOrder { get; }

		TD DefaultFields { get; }
	}

	public abstract class ASchemaDef<TE, TD> : ISchemaDef<TE, TD> 
		where TE : Enum  where TD : SchemaDictionaryBase<TE>, new()
	{
		public ASchemaDef()
		{
			Init();
		}

		public abstract TE[] KeyOrder { get; set; }
		public Enum[] KeyOrderX { get; set; }
		public abstract TD DefaultFields { get;}

		public void Init()
		{
			KeyOrder = new TE[DefaultFields.Count];

			int j = 0;

			foreach (KeyValuePair<TE, SchemaFieldDef<TE>> kvp in DefaultFields)
			{
				KeyOrder[j++] = kvp.Key;
			}

			j = 0;

			KeyOrderX = new Enum[DefaultFields.Count];

			foreach (KeyValuePair<TE, SchemaFieldDef<TE>> kvp in DefaultFields)
			{
				KeyOrderX[j++] = kvp.Key;
			}
		}
	}

}