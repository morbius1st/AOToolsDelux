#region Using directives

using System;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using AOTools.Cells.SchemaDefinition;
using static UtilityLibrary.MessageUtilities;

using AOTools.Cells.ExStorage;
using static AOTools.Cells.ExStorage.ExStoreMgr;

#endregion

// itemname:	UnitStylesCommand
// username:	jeffs
// created:		1/6/2018 3:55:08 PM


namespace AOTools
{
	[Transaction(TransactionMode.Manual)]
	class DataStore : IExternalCommand
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
			try
			{
				// DataStorage ds;
				//
				// using (Transaction t = new Transaction(AppRibbon.Doc, "Create Unit Storage"))
				// {
				// 	t.Start();
				//
				// 	ds = DataStorage.Create(AppRibbon.Doc);
				// 	t.Commit();
				// }

				ExStoreApp xApp = ExStoreApp.AppConfig;
				ExStoreCell xCell = ExStoreCell.CellConfig;

				for (int i = 0; i < 3; i++)
				{
					SampleCellData(xCell, i);
				}

				XsMgr.Init();
				XsMgr.Save(xApp, xCell);

				// if (ds != null && ds.IsValidObject)
				// {
				// 	RsMgr.SetElement(ds);
				// 	RsMgr.Init();
				// 	RsMgr.Save();
				// }
			}
			catch (OperationCanceledException)
			{
				return Result.Failed;
			}

			return Result.Succeeded;
		}

		private void SampleCellData(ExStoreCell xCell, int id)
		{
			xCell.Data[id] = xCell.DefaultValues().Clone();

			xCell.Data[id][SchemaCellKey.NAME].Value = "Alpha";
			xCell.Data[id][SchemaCellKey.VERSION].Value = $"beta {id:D3}";
			xCell.Data[id][SchemaCellKey.SEQUENCE].Value = id;
			xCell.Data[id][SchemaCellKey.UPDATE_RULE].Value = UpdateRules.UPON_REQUEST;
			xCell.Data[id][SchemaCellKey.CELL_FAMILY_NAME].Value = $"CoolCell{id:D3}";
			xCell.Data[id][SchemaCellKey.SKIP].Value = false;
			xCell.Data[id][SchemaCellKey.XL_FILE_PATH].Value = $"c:\\file path\\filename{id:D3}.xls";
			xCell.Data[id][SchemaCellKey.XL_WORKSHEET_NAME].Value = $"worksheet {id:d3}";
		}

	}

}