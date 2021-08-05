// Solution:     AOToolsDelux
// Project:       AOToolsDelux
// File:             IExStoreData.cs
// Created:      2021-07-04 (5:12 PM)

using System;
using AOTools.Cells.SchemaDefinition;

namespace AOTools.Cells.ExStorage
{
	public class ExData<T> : IExData
	{
		public ExData(T value)
		{
			ExValue = value;
		}

		public T ExValue { get; set; }

		public string ValueString => ExValue.ToString();

		public override string ToString()
		{
			return $"value| {ValueString} ({typeof(T).Name})";
		}
	}

	public interface IExData
	{
		string ValueString { get; }
	}
	
	public interface IExStore<TE, TT> where TT : SchemaDictionaryBase<TE> where TE : Enum
	{
		string Name { get; } 
		string Description { get; }
		Guid ExStoreGuid { get; }
		TT FieldDefs { get; }
	}


	// public interface IExStoreData<TT, TD>
	// {
	// 	// string Name { get; } 
	// 	// string Description { get; }
	// 	//
	// 	// bool IsInitialized { get; }
	//
	// 	// Enum[] KeyOrder { get; }
	//
	// 	TT FieldDefs { get; }
	// 	TD Data { get; }
	// }
}