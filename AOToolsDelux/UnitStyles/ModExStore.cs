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
using Microsoft.Office.Interop.Excel;
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
			// string title = "Modify Cell Ex Storage";
			// ExStoreRtnCodes result;
			// Result r;
			//
			// result = XsMgr.ReadCells();
			// if (result != ExStoreRtnCodes.XRC_GOOD) 
			// 	return test01Fail(title, "Read Cells", result);
			//
			// ExStorageTests.ShowDataCell(XsMgr.XCell);
			//
			// ExStoreCell xCell = null;
			// ExStoreCell xCell2 = null;
			// xCell = XsMgr.XCell.Clone();
			// xCell2 = XsMgr.XCell;
			//
			// result = modifyData01(xCell);
			// if (result != ExStoreRtnCodes.XRC_GOOD) 
			// 	return test01Fail(title, "Modify Cells 1", result);
			//
			// r = updateCellData(xCell, title);
			// if (r != Result.Succeeded) return r;
			//
			//
			// xCell = XsMgr.XCell.Clone();
			//
			// result = modifyData02(xCell);
			// if (result != ExStoreRtnCodes.XRC_GOOD) 
			// 	return test01Fail(title, "Modify Cells 2", result);
			//
			// r = updateCellData(xCell, title);
			// if (r != Result.Succeeded) return r;
			//
			//
			// xCell = XsMgr.XCell.Clone();
			//
			// result = modifyData03(xCell);
			// if (result != ExStoreRtnCodes.XRC_GOOD) 
			// 	return test01Fail(title, "Modify Cells 3", result);
			//
			// r = updateCellData(xCell, title);
			// if (r != Result.Succeeded) return r;
			
			return Result.Succeeded;
		}


		private Result updateCellData(ExStoreCell xCell, string title)
		{
			ExStoreRtnCodes result;

			// todo fix how to update cells

			XsMgr.XCell = xCell;

			result = XsMgr.UpdateCells(/*xCell*/);
			if (result != ExStoreRtnCodes.XRC_GOOD) 
				return test01Fail(title, "update Cells", result);

			ExStorageTests.ShowDataCell(XsMgr.XCell);

			return Result.Succeeded;
		}

		private Result test01Fail(string title, string desc,
			ExStoreRtnCodes result)
		{
			XsMgr.ExStoreFail(title, desc, result.ToString());

			return Result.Failed;
		}

		private ExStoreRtnCodes modifyData01(ExStoreCell xCell)
		{

			// ExStoreRtnCodes result = XsMgr.ReadCells();
			// if (result != ExStoreRtnCodes.GOOD) return result;
			//
			// xCell = XsMgr.XCell;

			xCell.Data[0][SchemaCellKey.CK_NAME].Value = "Shasta_01";
			xCell.Data[0][SchemaCellKey.CK_DESCRIPTION].Value = "One tall mountain";
			xCell.Data[0][SchemaCellKey.CK_UPDATE_RULE].Value = (int) UpdateRules.UR_UPON_REQUEST;

			xCell.Data[1][SchemaCellKey.CK_NAME].Value = "Palomar_01";
			xCell.Data[1][SchemaCellKey.CK_DESCRIPTION].Value = "Another tall mountain";
			xCell.Data[1][SchemaCellKey.CK_UPDATE_RULE].Value = (int) UpdateRules.UR_NEVER;

			xCell.Data[2][SchemaCellKey.CK_NAME].Value = "K2_01";
			xCell.Data[2][SchemaCellKey.CK_DESCRIPTION].Value = "A very tall mountain";
			xCell.Data[2][SchemaCellKey.CK_UPDATE_RULE].Value = (int) UpdateRules.UR_AS_NEEDED;

			return ExStoreRtnCodes.XRC_GOOD;
		}

		private ExStoreRtnCodes modifyData02(ExStoreCell xCell)
		{
			// ExStoreRtnCodes result;

			// result = modifyData01(ref xCell);

			int id = xCell.Data.Count;

			xCell.AddDefault();
			ExStorageTests.SampleCellDataRevised(xCell, "Whitney_01", id++);

			xCell.AddDefault();
			ExStorageTests.SampleCellDataRevised(xCell, "Everest_01", id++);

			return ExStoreRtnCodes.XRC_GOOD;
		}

		private ExStoreRtnCodes modifyData03(ExStoreCell xCell)
		{
			xCell.Data.RemoveAt(0);
			xCell.Data.RemoveAt(0);
			return ExStoreRtnCodes.XRC_GOOD;
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
			// string title = "Modify App Ex Storage";
			// ExStoreRtnCodes result;
			// Result r;
			//
			// result = XsMgr.ReadApp();
			// if (result != ExStoreRtnCodes.XRC_GOOD) 
			// 	return test01Fail(title, "Read App", result);
			//
			// ExStorageTests.ShowDataApp(XsMgr.XApp);
			// ExStorageTests.ShowDataCell(XsMgr.XCell);
			//
			// ExStoreApp xApp = XsMgr.XApp;
			//
			// modifyData(xApp);
			//
			// r = updateAppData(xApp, title);
			// if (r != Result.Succeeded) return r;

			return Result.Succeeded;
		}

		private Result updateAppData(ExStoreApp xApp, string title)
		{
			ExStoreRtnCodes result;

			// todo fix update method

			XsMgr.XApp = xApp;

			result = XsMgr.UpdateApp(/*xApp*/);
			if (result != ExStoreRtnCodes.XRC_GOOD)
				return test01Fail(title, "update App", result);

			ExStorageTests.ShowDataApp(XsMgr.XApp);
			ExStorageTests.ShowDataCell(XsMgr.XCell);

			return Result.Succeeded;
		}


		private ExStoreRtnCodes modifyData(ExStoreApp xApp)
		{
			xApp.Data[SchemaAppKey.AK_NAME].Value = "Blinken_01";
			xApp.Data[SchemaAppKey.AK_DESCRIPTION].Value = "Blinken Description";
			xApp.Data[SchemaAppKey.AK_VERSION].Value = "0.2";
			return ExStoreRtnCodes.XRC_GOOD;
		}


		private Result test01Fail(string title, string desc,
			ExStoreRtnCodes result)
		{
			XsMgr.ExStoreFail(title, desc, result.ToString());

			return Result.Failed;
		}
	}
}