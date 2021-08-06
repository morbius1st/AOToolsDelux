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
			ExStoreRoot xRoot = ExStoreRoot.Instance();
			ExStoreRtnCodes result;

			xRoot.Data[SchemaRootKey.NAME].Value
				= "RootEx4"+AppRibbon.Doc.Title;

			xRoot.Data[SchemaRootKey.DESCRIPTION].Value
				= "Root Ex Storage Data for| "+AppRibbon.Doc.Title;

			result = XsMgr.WriteRoot(xRoot);
			if (result != ExStoreRtnCodes.GOOD) return result;

			return ExStoreRtnCodes.GOOD;
		}

		public static ExStoreRtnCodes MakeAppAndCellsExStorage()
		{
			ExStoreApp xApp = ExStoreApp.Instance();
			ExStoreRtnCodes result;

			xApp.Data[SchemaAppKey.NAME].Value = "Special Name 01";
			xApp.Data[SchemaAppKey.DESCRIPTION].Value = "Special Description 01";

			ExStoreCell xCell = ExStoreCell.Instance(3);

			for (int i = 0; i < 3; i++)
			{
				SampleCellData(xCell, i);
			}

			result = XsMgr.WriteAppAndCells(xApp, xCell);

			if (result != ExStoreRtnCodes.GOOD) return result;

			return ExStoreRtnCodes.GOOD;
		}

		public static void ShowData(ExStoreCell xCell)
		{
			TaskDialog td = new TaskDialog("Ex Storage App Data");

			td.MainInstruction = "Cell Schema was read successfully\ncontents:";

			StringBuilder sb = new StringBuilder();

			for (int i = 0; i < xCell.Data.Count; i++)
			{
				sb.AppendLine($"date group| {i:D}");


				foreach (KeyValuePair<SchemaCellKey, 
					SchemaFieldDef<SchemaCellKey>> kvp in xCell.FieldDefs)
				{
					string name = xCell.FieldDefs[kvp.Key].Name;
					string value = xCell.Data[i][kvp.Key].Value.ToString();

					sb.Append(name).Append("| ").AppendLine(value);	
				}

				sb.Append("\n");
			}

			td.MainContent = sb.ToString();
			td.MainIcon = TaskDialogIcon.TaskDialogIconNone;

			td.Show();
		}


	#endregion

	#region private methods

		private static void SampleCellData(ExStoreCell xCell, int id)
		{
			xCell.Data[id][SchemaCellKey.NAME].Value = "Alpha";
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