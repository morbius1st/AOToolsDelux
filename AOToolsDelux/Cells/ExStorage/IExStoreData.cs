// Solution:     AOToolsDelux
// Project:       AOToolsDelux
// File:             IExStoreData.cs
// Created:      2021-07-04 (5:12 PM)

using System;

namespace AOTools.Cells.ExStorage
{
	
	public interface IExStore
	{
		string Name { get; } 
		string Description { get; }
		Guid ExStoreGuid { get; }
		
	}


	public interface IExStoreData<TT, TD>
	{
		// TT Fields { get; }
		TD Data { get; }
		bool IsDefault { get; set; }
	}
}