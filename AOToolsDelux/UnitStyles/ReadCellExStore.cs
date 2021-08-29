#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
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
	class ReadCellExStore : IExternalCommand
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
			// ExStoreCell xCell = ExStoreCell.Instance(3);

			// ExStoreCell xCell = null;

			// ExStoreRtnCodes result = XsMgr.ReadCells();
			ExStoreRtnCodes result = XsMgr.AppExStorExists ? 
				ExStoreRtnCodes.XRC_GOOD : ExStoreRtnCodes.XRC_EX_STORE_NOT_EXISTS;

			if (result != ExStoreRtnCodes.XRC_GOOD)
			{
				XsMgr.ReadSchemaFail(XsMgr.OpDescription);
				return Result.Failed;
			}

			ShowData(XsMgr.XCell);

			return Result.Succeeded;
		}

		private void ShowData(ExStoreCell xCell)
		{
			TaskDialog td = new TaskDialog("Ex Storage App Data");

			td.MainInstruction = "Cell Schema was read successfully\ncontents:";

			StringBuilder sb = new StringBuilder();

			for (int i = 0; i < xCell.Data.Count; i++)
			{
				sb.AppendLine($"date group| {i:D}");


				foreach (KeyValuePair<SchemaCellKey, 
					SchemaFieldDef<SchemaCellKey>> kvp in xCell.Fields)
				{
					string name = xCell.Fields[kvp.Key].Name;
					string value = xCell.Data[i][kvp.Key].Value.ToString();

					sb.Append(name).Append("| ").AppendLine(value);	
				}

				sb.Append("\n");
			}

			td.MainContent = sb.ToString();
			td.MainIcon = TaskDialogIcon.TaskDialogIconNone;

			td.Show();
		}
		

	}

}