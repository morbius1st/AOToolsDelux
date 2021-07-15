#region Using directives

using System;
using System.Diagnostics;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using AOTools.Cells.SchemaDefinition;
using static UtilityLibrary.MessageUtilities;

using AOTools.Cells.ExStorage;
using AOTools.Cells.SchemaCells;
using Autodesk.Revit.DB.ExtensibleStorage;
using static AOTools.Cells.ExStorage.ExStoreMgr;
using static AOTools.Cells.SchemaDefinition.SchemaAppKey;
using static AOTools.Cells.SchemaDefinition.SchemaCellKey;

using static AOTools.Cells.SchemaCells.SchemaDefCells;

#endregion

// itemname:	UnitStylesCommand
// username:	jeffs
// created:		1/6/2018 3:55:08 PM


namespace AOTools
{
	[Transaction(TransactionMode.Manual)]
	class DataStore : IExternalCommand
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
				// DataStorage ds;
				//
				// using (Transaction t = new Transaction(AppRibbon.Doc, "Create Unit Storage"))
				// {
				// 	t.Start();
				//
				// 	ds = DataStorage.Create(AppRibbon.Doc);
				// 	t.Commit();
				// }

				ExStoreApp xApp = ExStoreApp.Instance();

				xApp.Data[SchemaDefApp.Inst[AK_NAME]].Value = "Special Name 01";

				ExStoreCell xCell = ExStoreCell.Instance(3);

				for (int i = 0; i < 3; i++)
				{
					SampleCellData(xCell, i);
				}

				ExStoreRtnCodes result = XsMgr.Save(xsHlpr, xApp, xCell);

				if (result != ExStoreRtnCodes.GOOD)
				{
					Debug.WriteLine("initial save failed");
					return Result.Failed;
				}


				if (false)
				{

					xApp.Data[SchemaDefApp.Inst[AK_NAME]].Value = "new name";

					xCell = ExStoreCell.Instance(4);

					for (int i = 0; i < 4; i++)
					{
						SampleCellDataRevised(xCell, i);
					}

					// result = XsMgr.UpdateCell(xsHlpr, xCell);
					result = XsMgr.Save(xsHlpr, xApp, xCell);

					if (result != ExStoreRtnCodes.GOOD)
					{
						Debug.WriteLine("update failed");
						return Result.Failed;
					}

					
				}

				// SaveRtnCodes result = XsMgr.Save(xApp, xCell);

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

			xCell.Data[id][Inst[CK_NAME]].Value = "Alpha";
			xCell.Data[id][Inst[CK_VERSION]].Value = $"beta {id:D3}";
			xCell.Data[id][Inst[CK_SEQUENCE]].Value = (double) id;
			xCell.Data[id][Inst[CK_UPDATE_RULE]].Value = (int) UpdateRules.UPON_REQUEST;
			xCell.Data[id][Inst[CK_CELL_FAMILY_NAME]].Value = $"CoolCell{id:D3}";
			xCell.Data[id][Inst[CK_SKIP]].Value = false;
			xCell.Data[id][Inst[CK_XL_FILE_PATH]].Value = $"c:\\file path\\filename{id:D3}.xls";
			xCell.Data[id][Inst[CK_XL_WORKSHEET_NAME]].Value = $"worksheet {id:d3}";
		}

		
		private void SampleCellDataRevised(ExStoreCell xCell, int id)
		{

			// xCell.Data.Add(xCell.DefaultValues().Clone());

			xCell.Data[id][Inst[CK_NAME]].Value = "Zeta";
			xCell.Data[id][Inst[CK_VERSION]].Value = $"Delta {id:D3}";
			xCell.Data[id][Inst[CK_SEQUENCE]].Value = (double) id;
			xCell.Data[id][Inst[CK_UPDATE_RULE]].Value = (int) UpdateRules.AS_NEEDED;
			xCell.Data[id][Inst[CK_CELL_FAMILY_NAME]].Value = $"CoolCell{id:D3}";
			xCell.Data[id][Inst[CK_SKIP]].Value = false;
			xCell.Data[id][Inst[CK_XL_FILE_PATH]].Value = $"c:\\file path\\filename{id:D3}.xls";
			xCell.Data[id][Inst[CK_XL_WORKSHEET_NAME]].Value = $"worksheet {id:d3}";
		}

	}

}