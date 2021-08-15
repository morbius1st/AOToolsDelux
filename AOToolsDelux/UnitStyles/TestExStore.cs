#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Input;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using AOTools.Cells.SchemaDefinition;
using AOTools.Cells.SchemaCells;
using static UtilityLibrary.MessageUtilities;

using AOTools.Cells.ExStorage;
using AOTools.Cells.Tests;
using Autodesk.Revit.DB.ExtensibleStorage;
using static AOTools.Cells.ExStorage.ExStoreMgr;

#endregion

// itemname:	UnitStylesCommand
// username:	jeffs
// created:		1/6/2018 3:55:08 PM


namespace AOTools
{

	[Transaction(TransactionMode.Manual)]
	class TestExStore : IExternalCommand
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

			return Test01();
		}

		private Result Test01()
		{
			ExStoreMgr a = XsMgr;

			try
			{
				if (AppRibbon.Doc.IsDetached) return Result.Cancelled;

				// 1. make a root ex store
				// 2. using the configured app guid, save the app data, save the cell data
				// 3. reset all to default values or null
				// 4. read the root ex store
				// 5. set the app guid
				// 6. read the app ex store
				// 7. read the cell ex store

				ExStoreRtnCodes result;

				result = ExStorageTests.MakeRootExStorage();
				if (result != ExStoreRtnCodes.GOOD) return Result.Failed;

				result = ExStorageTests.MakeAppAndCellsExStorage();
				if (result != ExStoreRtnCodes.GOOD) return Result.Failed;

			}
			catch (OperationCanceledException)
			{
				return Result.Failed;
			}

			info();

			return Result.Succeeded;
		}

		private void info()
		{
			ExStoreMgr x = XsMgr;

			ExStoreRoot xrc = XsMgr.XRoot;
			SchemaDictionaryRoot xrf = xrc.FieldDefs;
			SchemaDictionaryRoot xrl = xrc.Data;
			SchemaDictionaryRoot xrv = xrc.DefaultValues();

			ExStoreApp xac = XsMgr.XApp;
			SchemaDictionaryApp xaf = xac.FieldDefs;
			SchemaDictionaryApp xal = xac.Data;
			SchemaDictionaryApp xav = xac.DefaultValues();

			ExStoreCell xcc = XsMgr.XCell;
			SchemaDictionaryCell xcf = xcc.FieldDefs;
			List<SchemaDictionaryCell> xcl = xcc.Data;
			Dictionary<string, string> xcd = xcc.SubSchemaFields;
			SchemaDictionaryCell xcv = xcc.DefaultValues();

		}





/*
		private void SampleCellData(ExStoreCell xCell, int id)
		{

			// xCell.Data.Add(xCell.DefaultValues().Clone());

			xCell.Data[id][SchemaCellKey.NAME].Value = "Alpha";
			xCell.Data[id][SchemaCellKey.VERSION].Value = $"beta {id:D3}";
			xCell.Data[id][SchemaCellKey.SEQUENCE].Value = (double) id;
			xCell.Data[id][SchemaCellKey.UPDATE_RULE].Value = (int) UpdateRules.UPON_REQUEST;
			xCell.Data[id][SchemaCellKey.CELL_FAMILY_NAME].Value = $"CoolCell{id:D3}";
			xCell.Data[id][SchemaCellKey.SKIP].Value = false;
			xCell.Data[id][SchemaCellKey.XL_FILE_PATH].Value = $"c:\\file path\\filename{id:D3}.xls";
			xCell.Data[id][SchemaCellKey.XL_WORKSHEET_NAME].Value = $"worksheet {id:d3}";
		}

		
		private void SampleCellDataRevised(ExStoreCell xCell, int id)
		{

			// xCell.Data.Add(xCell.DefaultValues().Clone());

			xCell.Data[id][SchemaCellKey.NAME].Value = "Zeta";
			xCell.Data[id][SchemaCellKey.VERSION].Value = $"Delta {id:D3}";
			xCell.Data[id][SchemaCellKey.SEQUENCE].Value = (double) id;
			xCell.Data[id][SchemaCellKey.UPDATE_RULE].Value = (int) UpdateRules.AS_NEEDED;
			xCell.Data[id][SchemaCellKey.CELL_FAMILY_NAME].Value = $"CoolCell{id:D3}";
			xCell.Data[id][SchemaCellKey.SKIP].Value = false;
			xCell.Data[id][SchemaCellKey.XL_FILE_PATH].Value = $"c:\\file path\\filename{id:D3}.xls";
			xCell.Data[id][SchemaCellKey.XL_WORKSHEET_NAME].Value = $"worksheet {id:d3}";
		}
*/
	}

}