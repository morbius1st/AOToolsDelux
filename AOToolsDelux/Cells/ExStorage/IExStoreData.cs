// Solution:     AOToolsDelux
// Project:       AOToolsDelux
// File:             IExStoreData.cs
// Created:      2021-07-04 (5:12 PM)

using System;
using System.Collections.Generic;
using AOTools.Cells.SchemaDefinition;

namespace AOTools.Cells.ExStorage
{
	public class ExData<T> : IExData
	{
		public ExData(T value)
		{
			Value = value;
			this.Type = typeof(T);
		}

		public T Value { get; set; }

		public Type Type { get; }

		public string ValueString => Value.ToString();

		public override string ToString()
		{
			return $"value| {ValueString} ({typeof(T).Name})";
		}
	}

	public interface IExData
	{
		string ValueString { get; }
		Type Type { get; }
	}
	
	public interface IExStore<TE, TT> where TT : SchemaDictionaryBase<TE> where TE : Enum
	{
		string Name { get; } 
		string Description { get; }
		Guid ExStoreGuid { get; }
		TT FieldDefs { get; }
	}

	public interface IExStore2<TE, TD> where TE : Enum where TD : ICopyable<TD>
	{
		string Name { get; } 
		string Description { get; }
		Guid ExStoreGuid { get; }
		Dictionary<TE, TD> FieldDefs { get; }
	}

	public interface ICopyable<T>
	{
		T Copy();
	}

}