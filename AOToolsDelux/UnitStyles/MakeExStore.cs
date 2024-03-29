﻿#region Using directives

using System;
using System.Diagnostics;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using AOToolsDelux.Cells.SchemaDefinition;
using static UtilityLibrary.MessageUtilities;
using AOToolsDelux.Cells.ExStorage;
using AOToolsDelux.Cells.Tests;
using Autodesk.Revit.DB.ExtensibleStorage;
using static AOToolsDelux.Cells.ExStorage.ExStoreMgr;

#endregion

// itemname:	UnitStylesCommand
// username:	jeffs
// created:		1/6/2018 3:55:08 PM


namespace AOToolsDelux
{

	[Transaction(TransactionMode.Manual)]
	class MakeRootExStore : IExternalCommand
	{
		public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
		{

			AppRibbon.UiApp = commandData.Application;
			AppRibbon.Uidoc = AppRibbon.UiApp.ActiveUIDocument;
			AppRibbon.App =  AppRibbon.UiApp.Application;
			AppRibbon.Doc =  AppRibbon.Uidoc.Document;

			OutLocation = OutputLocation.DEBUG;

			return Test01();
		}

		private Result Test01()
		{
			if (AppRibbon.Doc.IsDetached) return Result.Cancelled;

			ExStoreRoot xRoot = ExStoreRoot.Instance();
			ExStoreRtnCodes result;

			// result = XsMgr.Initialize();
			// if (result != ExStoreRtnCodes.GOOD) return Result.Failed;
			//
			// result = XsMgr.Configure();
			// if (result != ExStoreRtnCodes.GOOD) return Result.Failed;

			result = ExStorageTests.MakeRootExStorage();
			if (result != ExStoreRtnCodes.XRC_GOOD) return Result.Failed;

			return Result.Succeeded;
		}
	}


	[Transaction(TransactionMode.Manual)]
	class MakeAppAndDataStore : IExternalCommand
	{
		// public static Schema SchemaUnit;
		// public static Entity EntityUnit;
		//
		// public static Schema SchemaDS;
		// public static Entity EntityDS;


		public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
		{
			AppRibbon.UiApp = commandData.Application;
			AppRibbon.Uidoc = AppRibbon.UiApp.ActiveUIDocument;
			AppRibbon.App =  AppRibbon.UiApp.Application;
			AppRibbon.Doc =  AppRibbon.Uidoc.Document;

			OutLocation = OutputLocation.DEBUG;

			return Test02();
		}

		private Result Test02()
		{
			try
			{
				ExStoreRtnCodes result;
	
				result = ExStorageTests.MakeAppAndCellsExStorage();
				if (result != ExStoreRtnCodes.XRC_GOOD) return Result.Failed;

			}
			catch (OperationCanceledException)
			{
				return Result.Failed;
			}

			return Result.Succeeded;
		}

		// make new data storage, for the first time - root / app ex storage cannot exist.
		// private Result Test01()
		// {
		// 	try
		// 	{
		// 		// DataStorage ds;
		// 		//
		// 		// using (Transaction t = new Transaction(AppRibbon.Doc, "Create Unit Storage"))
		// 		// {
		// 		// 	t.Start();
		// 		//
		// 		// 	ds = DataStorage.Create(AppRibbon.Doc);
		// 		// 	t.Commit();
		// 		// }
		//
		// 		// todo - revise to use data stored in manager
		//
		//
		// 		ExStoreApp xApp = ExStoreApp.Instance();
		// 		ExStoreRtnCodes result;
		//
		// 		xApp.Data[SchemaAppKey.NAME].Value = "Special_Name_01";
		// 		xApp.Data[SchemaAppKey.DESCRIPTION].Value = "Special Description 01";
		//
		// 		ExStoreCell xCell = ExStoreCell.Instance(3);
		//
		// 		for (int i = 0; i < 3; i++)
		// 		{
		// 			SampleCellData(xCell, i);
		// 		}
		// 		//
		// 		// result = XsMgr.Initialize();
		// 		// if (result != ExStoreRtnCodes.GOOD) return Result.Failed;
		// 		//
		// 		// result = XsMgr.Configure();
		// 		// if (result != ExStoreRtnCodes.GOOD) return Result.Failed;
		//
		// 		result = XsMgr.WriteAppAndCells(/*xApp, xCell*/);
		//
		// 		if (result != ExStoreRtnCodes.XRC_GOOD)
		// 		{
		// 			Debug.WriteLine("save failed");
		// 			return Result.Failed;
		// 		}
		//
		//
		// 		if (false)
		// 		{
		// 			xApp.Data[SchemaAppKey.NAME].Value = "new_name";
		//
		// 			xCell = ExStoreCell.Instance(4);
		//
		// 			for (int i = 0; i < 4; i++)
		// 			{
		// 				SampleCellDataRevised(xCell, i);
		// 			}
		//
		// 			// result = XsMgr.UpdateCell(xsHlpr, xCell);
		// 			result = XsMgr.WriteAppAndCells(xApp, xCell);
		//
		// 			if (result != ExStoreRtnCodes.XRC_GOOD)
		// 			{
		// 				Debug.WriteLine("update failed");
		// 				return Result.Failed;
		// 			}
		// 		}
		//
		// 		// SaveRtnCodes result = XsMgr.Save(xApp, xCell);
		//
		// 		// if (ds != null && ds.IsValidObject)
		// 		// {
		// 		// 	RsMgr.SetElement(ds);
		// 		// 	RsMgr.Init();
		// 		// 	RsMgr.Save();
		// 		// }
		// 	}
		// 	catch (OperationCanceledException)
		// 	{
		// 		return Result.Failed;
		// 	}
		//
		// 	// Schema schemaUnit= SchemaUnit;
		// 	// Entity entityUnit= EntityUnit;
		// 	// 				   
		// 	// Schema schemaDS =  SchemaDS;
		// 	// Entity entityDS =  EntityDS;
		//
		// 	return Result.Succeeded;
		// }

		private void SampleCellData(ExStoreCell xCell, int id)
		{
			// xCell.Data.Add(xCell.DefaultValues().Clone());

			xCell.Data[id][SchemaCellKey.CK_NAME].Value = "Alpha";
			xCell.Data[id][SchemaCellKey.CK_VERSION].Value = $"beta {id:D3}";
			xCell.Data[id][SchemaCellKey.CK_SEQUENCE].Value = (double) id;
			xCell.Data[id][SchemaCellKey.CK_UPDATE_RULE].Value = (int) UpdateRules.UR_UPON_REQUEST;
			xCell.Data[id][SchemaCellKey.CK_CELL_FAMILY_NAME].Value = $"CoolCell{id:D3}";
			xCell.Data[id][SchemaCellKey.CK_SKIP].Value = false;
			xCell.Data[id][SchemaCellKey.CK_XL_FILE_PATH].Value = $"c:\\file path\\filename{id:D3}.xls";
			xCell.Data[id][SchemaCellKey.CK_XL_WORKSHEET_NAME].Value = $"worksheet {id:d3}";
		}

		private void SampleCellDataRevised(ExStoreCell xCell, int id)
		{
			// xCell.Data.Add(xCell.DefaultValues().Clone());

			xCell.Data[id][SchemaCellKey.CK_NAME].Value = "Zeta";
			xCell.Data[id][SchemaCellKey.CK_VERSION].Value = $"Delta {id:D3}";
			xCell.Data[id][SchemaCellKey.CK_SEQUENCE].Value = (double) id;
			xCell.Data[id][SchemaCellKey.CK_UPDATE_RULE].Value = (int) UpdateRules.UR_AS_NEEDED;
			xCell.Data[id][SchemaCellKey.CK_CELL_FAMILY_NAME].Value = $"CoolCell{id:D3}";
			xCell.Data[id][SchemaCellKey.CK_SKIP].Value = false;
			xCell.Data[id][SchemaCellKey.CK_XL_FILE_PATH].Value = $"c:\\file path\\filename{id:D3}.xls";
			xCell.Data[id][SchemaCellKey.CK_XL_WORKSHEET_NAME].Value = $"worksheet {id:d3}";
		}
	}
}