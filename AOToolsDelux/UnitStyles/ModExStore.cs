#region Using directives

using System;
using System.Diagnostics;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using AOTools.Cells.SchemaDefinition;
using static UtilityLibrary.MessageUtilities;

using AOTools.Cells.ExStorage;
using AOTools.Cells.Tests;
using Autodesk.Revit.DB.ExtensibleStorage;
using static AOTools.Cells.ExStorage.ExStoreMgr;
using InvalidOperationException = Autodesk.Revit.Exceptions.InvalidOperationException;

#endregion

// itemname:	UnitStylesCommand
// username:	jeffs
// created:		1/6/2018 3:55:08 PM


namespace AOTools
{

	[Transaction(TransactionMode.Manual)]
	class ModCellExData : IExternalCommand
	{
		public Result Execute(ExternalCommandData commandData, 
			ref string message, ElementSet elements)
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
			string title = "Modify Ex Storage";
			ExStoreRtnCodes result;
			Result r;

			// ExStoreRtnCodes result = XsMgr.Initialize();
			// if (result != ExStoreRtnCodes.GOOD) 
			// 	return test01Fail(title, "Initialize", result);

			ExStoreCell xCell = null;

			result = XsMgr.ReadCells(ref xCell);
			if (result != ExStoreRtnCodes.GOOD) 
				return test01Fail(title, "Read Cells", result);

			ExStorageTests.ShowData(xCell);


			result = modifyData01(ref xCell);
			if (result != ExStoreRtnCodes.GOOD) 
				return test01Fail(title, "Modify Cells 1", result);

			r = updateCellData(xCell, title);
			if (r != Result.Succeeded) return r;

			
			result = modifyData02(ref xCell);
			if (result != ExStoreRtnCodes.GOOD) 
				return test01Fail(title, "Modify Cells 2", result);

			r = updateCellData(xCell, title);
			if (r != Result.Succeeded) return r;
			
			
			result = modifyData03(ref xCell);
			if (result != ExStoreRtnCodes.GOOD) 
				return test01Fail(title, "Modify Cells 3", result);

			r = updateCellData(xCell, title);
			if (r != Result.Succeeded) return r;
			

			// result = XsMgr.UpdateCells(xCell);
			// if (result != ExStoreRtnCodes.GOOD) 
			// 	return test01Fail(title, "update Cells", result);
			//
			// ExStoreCell xCell2 = null;
			//
			// result = XsMgr.ReadCells(ref xCell2);
			// if (result != ExStoreRtnCodes.GOOD)
			// 	return test01Fail(title, "Read Cells", result);
			//
			// ExStorageTests.ShowData(xCell2);


			return Result.Succeeded;
		}

		private Result updateCellData(ExStoreCell xCell, string title)
		{
			ExStoreRtnCodes result;

			result = XsMgr.UpdateCells(xCell);
			if (result != ExStoreRtnCodes.GOOD) 
				return test01Fail(title, "update Cells", result);

			ExStoreCell xCell2 = null; // = ExStoreCell.Instance(xCell.Data.Count);

			result = XsMgr.ReadCells(ref xCell2);
			if (result != ExStoreRtnCodes.GOOD)
				return test01Fail(title, "Read Cells", result);

			ExStorageTests.ShowData(xCell2);

			return Result.Succeeded;
		}

		private Result test01Fail(string title, string desc,
			ExStoreRtnCodes result)
		{
			XsMgr.ExStoreFail(title, desc, result.ToString());

			return Result.Failed;
		}

		private ExStoreRtnCodes modifyData01(ref ExStoreCell xCell)
		{

			// ExStoreRtnCodes result = XsMgr.ReadCells();
			// if (result != ExStoreRtnCodes.GOOD) return result;
			//
			// xCell = XsMgr.XCell;

			xCell.Data[0][SchemaCellKey.NAME].Value = "Shasta 01";
			xCell.Data[0][SchemaCellKey.DESCRIPTION].Value = "One tall mountain";
			xCell.Data[0][SchemaCellKey.UPDATE_RULE].Value = (int) UpdateRules.UPON_REQUEST;

			xCell.Data[1][SchemaCellKey.NAME].Value = "Palomar 01";
			xCell.Data[1][SchemaCellKey.DESCRIPTION].Value = "Another tall mountain";
			xCell.Data[1][SchemaCellKey.UPDATE_RULE].Value = (int) UpdateRules.NEVER;

			xCell.Data[2][SchemaCellKey.NAME].Value = "K2 01";
			xCell.Data[2][SchemaCellKey.DESCRIPTION].Value = "A very tall mountain";
			xCell.Data[2][SchemaCellKey.UPDATE_RULE].Value = (int) UpdateRules.AS_NEEDED;

			return ExStoreRtnCodes.GOOD;
		}

		private ExStoreRtnCodes modifyData02(ref ExStoreCell xCell)
		{
			// ExStoreRtnCodes result;

			// result = modifyData01(ref xCell);

			int id = xCell.Data.Count;

			xCell.AddDefault();
			ExStorageTests.SampleCellDataRevised(xCell, "Whitney 01", id++);

			xCell.AddDefault();
			ExStorageTests.SampleCellDataRevised(xCell, "Everest 01", id++);

			return ExStoreRtnCodes.GOOD;
		}

		private ExStoreRtnCodes modifyData03(ref ExStoreCell xCell)
		{
			xCell.Data.RemoveAt(0);
			xCell.Data.RemoveAt(0);
			return ExStoreRtnCodes.GOOD;
		}

	}

	[Transaction(TransactionMode.Manual)]
	class ModAppExData : IExternalCommand
	{
		public Result Execute(ExternalCommandData commandData, 
			ref string message, ElementSet elements)
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
			ExStoreRtnCodes result = XsMgr.DeleteApp();

			if (result != ExStoreRtnCodes.GOOD)
			{
				XsMgr.DeleteSchemaFail(XsMgr.OpDescription);
				return Result.Failed;
			}

			return Result.Succeeded;
		}
	}
	
	// [Transaction(TransactionMode.Manual)]
	// class DelSubExStor : IExternalCommand
	// {
	// 	public Result Execute(ExternalCommandData commandData, 
	// 		ref string message, ElementSet elements)
	// 	{
	// 		AppRibbon.UiApp = commandData.Application;
	// 		AppRibbon.Uidoc = AppRibbon.UiApp.ActiveUIDocument;
	// 		AppRibbon.App =  AppRibbon.UiApp.Application;
	// 		AppRibbon.Doc =  AppRibbon.Uidoc.Document;
	//
	// 		OutLocation = OutputLocation.DEBUG;
	//
	// 		return Test01();
	// 	}
	//
	// 	private Result Test01()
	// 	{
	// 		ExStoreRtnCodes result = XsMgr.DeleteCells();
	//
	// 		if (result != ExStoreRtnCodes.GOOD)
	// 		{
	// 			XsMgr.DeleteSchemaFail(XsMgr.OpDescription);
	// 			return Result.Failed;
	// 		}
	//
	// 		return Result.Succeeded;
	// 	}
	// }

}