// Solution:     AOToolsDelux
// Project:       CSToolsDelux
// File:             ISchemaData.cs
// Created:      2021-09-11 (4:51 PM)

using System;
using CSToolsDelux.Fields.SchemaInfo.SchemaData.SchemaDataDefinitions;
using CSToolsDelux.Fields.SchemaInfo.SchemaDefinitions;

namespace CSToolsDelux.Fields.SchemaInfo.SchemaData
{
	public abstract class ISchemaData<TE, TD, TF> 
		where TE : Enum
		where TD : SchemaDataDictionaryBase<TE> 
		where TF :  SchemaDictionaryBase<TE>
	{
		public abstract TD Data { get; protected set;}
		public abstract TF Fields { get;}

		public ASchemaDataFieldDef<TE> this[TE key]
		{
			get
			{
				if (!Data.ContainsKey(key)) return null;
				return Data[key];
			}
		}

		public abstract TX GetValue<TX>(TE key);
		public abstract void SetValue<TX>(TE key, TX value);
		public abstract void Add<TX>(TE key, TX value);
		public abstract void AddDefault<TX>(TE key);
	}
}