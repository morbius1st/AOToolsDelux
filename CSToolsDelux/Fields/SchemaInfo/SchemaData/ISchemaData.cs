// Solution:     AOToolsDelux
// Project:       CSToolsDelux
// File:             ISchemaData.cs
// Created:      2021-09-11 (4:51 PM)

using System;
using CSToolsDelux.Fields.SchemaInfo.SchemaData.SchemaDataDefinitions;
using CSToolsDelux.Fields.SchemaInfo.SchemaDefinitions;

namespace CSToolsDelux.Fields.SchemaInfo.SchemaData
{
	public interface ISchemaData<TE, TD, TF> where TE : Enum
		where TD : SchemaDataDictionaryBase<TE> where TF :  SchemaDictionaryBase<TE>
	{
		TD Data { get; }
		TF Fields { get; }

		TX GetValue<TX>(TE key);
		void SetValue<TX>(TE key, TX value);
		void Add<TX>(TE key, TX value);
		void AddDefault<TX>(TE key);
	}
}