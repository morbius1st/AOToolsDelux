// Solution:     AOToolsDelux
// Project:       AOToolsDelux
// File:             IExStoreData.cs
// Created:      2021-07-04 (5:12 PM)

using System;
using AOTools.Cells.SchemaDefinition;

namespace AOTools.Cells.ExStorage
{
	
	public interface IExStore
	{
		string Name { get; } 
		string Description { get; }

		bool IsInitialized { get; }

		Guid ExStoreGuid { get; }
	}


	public interface IExStoreData<TT, TD>
	{
		// string Name { get; } 
		// string Description { get; }
		//
		// bool IsInitialized { get; }

		TT FieldDefs { get; }
		TD Data { get; }
	}
}