// Solution:     AOToolsDelux
// Project:       AOToolsDelux
// File:             IExStoreData.cs
// Created:      2021-07-04 (5:12 PM)

using AOTools.Cells.SchemaDefinition;

namespace AOTools.Cells.ExStorage
{
	public interface IExStoreData<TT, TD>
	{
		string Name { get; } 
		string Description { get; }

		bool IsInitialized { get; }

		SchemaDictionaryBase<TT> SchemaFields { get; }
		TD Data { get; }

		void Initialize();
	}
}