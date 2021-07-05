// Solution:     AOToolsDelux
// Project:       AOToolsDelux
// File:             SchemaDictionaryBase.cs
// Created:      2021-07-03 (10:50 PM)

using System.Collections.Generic;

namespace AOTools.AppSettings.SchemaSettings
{
	public class SchemaDictionaryBase<T> : Dictionary<T, SchemaFieldUnit> 
	{
		public SchemaDictionaryBase() { }

		public SchemaDictionaryBase(int capacity) : base(capacity) { }

		protected TU Clone<TU>(TU original) where TU : SchemaDictionaryBase<T>, new()
		{
			TU copy = new TU();

			foreach (KeyValuePair<T, SchemaFieldUnit> kvp in original)
			{
				copy.Add(kvp.Key, new SchemaFieldUnit(kvp.Value));
			}
			return copy;
		}
	}
}