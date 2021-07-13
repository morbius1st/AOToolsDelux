#region Using directives

using System;
using System.Diagnostics;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using AOTools.Cells.SchemaDefinition;
using static UtilityLibrary.MessageUtilities;

using AOTools.Cells.ExStorage;
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
			ExStoreHelper xsHlpr = new ExStoreHelper();

			try
			{

				// 1. make a root ex store
				// 2. using the configured app guid, save the app data, save the cell data
				// 3. reset all to default values or null
				// 4. read the root ex store
				// 5. set the app guid
				// 6. read the app ex store
				// 7. read the cell ex store



			}
			catch (OperationCanceledException)
			{
				return Result.Failed;
			}

			// Schema schemaUnit= SchemaUnit;
			// Entity entityUnit= EntityUnit;
			// 				   
			// Schema schemaDS =  SchemaDS;
			// Entity entityDS =  EntityDS;

			return Result.Succeeded;
		}

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

	}

}