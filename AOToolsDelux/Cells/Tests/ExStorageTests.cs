#region using
using static AOTools.Cells.ExStorage.ExStoreMgr;
using System.Collections.Generic;
using System.Text;
using AOTools.Cells.ExStorage;
using AOTools.Cells.SchemaDefinition;
using Autodesk.Revit.UI;
#endregion

// username: jeffs
// created:  7/19/2021 11:09:28 PM

namespace AOTools.Cells.Tests
{
	public class ExStorageTests
	{

	#region public methods

		public static ExStoreRtnCodes MakeRootExStorage()
		{
			XsMgr.XRoot = ExStoreRoot.Instance();
			ExStoreRtnCodes result;

			XsMgr.XRoot.Data[SchemaRootKey.NAME].Value
				= "RootEx4"+AppRibbon.Doc.Title;

			XsMgr.XRoot.Data[SchemaRootKey.DESCRIPTION].Value
				= "Root Ex Storage Data for| "+AppRibbon.Doc.Title;

			result = XsMgr.WriteRoot(XsMgr.XRoot);
			if (result != ExStoreRtnCodes.GOOD) return result;

			return ExStoreRtnCodes.GOOD;
		}

		public static ExStoreRtnCodes MakeAppAndCellsExStorage(int qty = 3)
		{
			XsMgr.XApp = ExStoreApp.Instance();

			ExStoreRtnCodes result;

			XsMgr.XApp.Data[SchemaAppKey.NAME].Value = "Special Name 01";
			XsMgr.XApp.Data[SchemaAppKey.DESCRIPTION].Value = "Special Description 01";

			XsMgr.XCell = ExStoreCell.Instance(3);

			for (int i = 0; i < qty; i++)
			{
				SampleCellData(XsMgr.XCell, i);
			}

			result = XsMgr.WriteAppAndCells(XsMgr.XApp, XsMgr.XCell);

			if (result != ExStoreRtnCodes.GOOD) return result;

			return ExStoreRtnCodes.GOOD;
		}

		public static void ShowDataCell(ExStoreCell xCell)
		{
			TaskDialog td = new TaskDialog("Ex Storage App Data");

			td.MainInstruction = "Cell Schema was read successfully\ncontents:";

			StringBuilder sb = new StringBuilder();

			sb.AppendLine($"guid| {xCell.ExStoreGuid.ToString()}\n");

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

		
		public static void ShowDataApp(ExStoreApp xApp)
		{
			TaskDialog td = new TaskDialog("Ex Storage App Data");

			td.MainInstruction = "App Schema was read successfully\ncontents:";

			StringBuilder sb = new StringBuilder();

			foreach (KeyValuePair<SchemaAppKey, SchemaFieldDef<SchemaAppKey>> kvp in xApp.Data)
			{
				string name = xApp.Data[kvp.Key].Name;
				string value = xApp.Data[kvp.Key].Value;

				sb.Append(name).Append("| ").AppendLine(value);
			}

			td.MainContent = sb.ToString();
			td.MainIcon = TaskDialogIcon.TaskDialogIconNone;

			td.Show();

		}


	#endregion

	#region private methods

		private static void SampleCellData(ExStoreCell xCell, int id)
		{
			xCell.Data[id][SchemaCellKey.NAME].Value = $"Alpha {id:D2}";
			xCell.Data[id][SchemaCellKey.VERSION].Value = $"beta {id:D3}";
			xCell.Data[id][SchemaCellKey.SEQUENCE].Value = (double) id;
			xCell.Data[id][SchemaCellKey.UPDATE_RULE].Value = (int) UpdateRules.UPON_REQUEST;
			xCell.Data[id][SchemaCellKey.CELL_FAMILY_NAME].Value = $"CoolCell{id:D3}";
			xCell.Data[id][SchemaCellKey.SKIP].Value = false;
			xCell.Data[id][SchemaCellKey.XL_FILE_PATH].Value = $"c:\\file path\\filename{id:D3}.xls";
			xCell.Data[id][SchemaCellKey.XL_WORKSHEET_NAME].Value = $"worksheet {id:d3}";
		}

		
		public static void SampleCellDataRevised(ExStoreCell xCell, string name, int id)
		{
			xCell.Data[id][SchemaCellKey.NAME].Value = name;
			xCell.Data[id][SchemaCellKey.VERSION].Value = $"Delta {id:D3}";
			xCell.Data[id][SchemaCellKey.SEQUENCE].Value = (double) id;
			xCell.Data[id][SchemaCellKey.UPDATE_RULE].Value = (int) UpdateRules.AS_NEEDED;
			xCell.Data[id][SchemaCellKey.CELL_FAMILY_NAME].Value = $"CoolCell{id:D3}";
			xCell.Data[id][SchemaCellKey.SKIP].Value = false;
			xCell.Data[id][SchemaCellKey.XL_FILE_PATH].Value = $"c:\\file path\\filename{id:D3}.xls";
			xCell.Data[id][SchemaCellKey.XL_WORKSHEET_NAME].Value = $"worksheet {id:d3}";
		}

	#endregion

	#region event consuming

	#endregion

	#region event publishing

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is ExStorageTests";
		}

	#endregion
	}
}