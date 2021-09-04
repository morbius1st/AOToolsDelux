#region using

using System;
using static AOTools.Cells.ExStorage.ExStoreMgr;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using AOTools.Cells.ExDataStorage;
using AOTools.Cells.ExStorage;
using AOTools.Cells.SchemaDefinition;
using Autodesk.Revit.DB.ExtensibleStorage;
using Autodesk.Revit.UI;
using static Autodesk.Revit.DB.ExtensibleStorage.Schema;
using static AOTools.Cells.ExDataStorage.DataStorageManager;

#endregion

// username: jeffs
// created:  7/19/2021 11:09:28 PM

namespace AOTools.Cells.Tests
{
	public class ExStorageTests
	{
		private static int lx;
		public static string loc { get; set; }

		public static int location
		{
			get => lx;

			set
			{
				lx = value;
				loc += value.ToString() + "\n";
			}
		}

		public string guid { get; set; }

		public static StaticTest01 staticTest01 { get; } = new StaticTest01(Guid.NewGuid().ToString());


		public ExStorageTests(string guid)
		{
			this.guid = guid;
		}

	#region public methods

		public string ReadAll(string vendorId)
		{
			Schema schema;
			Entity entity;
			DataStorage dataStorage;
			bool result;
			string answer = null;

			IList<Schema> schemas = Schema.ListSchemas();

			foreach (Schema s in schemas)
			{
				if (s.VendorId.Equals(vendorId))
				{
					entity = null;
					
					try
					{
						answer += $"got| {s.SchemaName}\n";

						result = DsMgr.GetDataStorage(s, out entity, out dataStorage);
					}
					catch
					{
						answer += "failed to read\n";
						result = false;
					}

					if (result)
					{
						answer += readAll(s, entity);
					}
				}
			}
			return answer;
		}

		private string readAll(Schema s, Entity e)
		{
			if (s == null || e == null) return null;

			string result = null;
			string fieldName = null;

			IList<Field> f = s.ListFields();

			foreach (Field field in f)
			{
				Type t = field.ValueType;
				fieldName = field.FieldName;
				result += $"field| {fieldName}| type| {t.Name} |";

				if (t?.Equals(typeof(string)) ?? false)
				{
					result += $" value (str)| {e.Get<string>(fieldName)}";
				}
				else if (t?.Equals(typeof(double)) ?? false)
				{
					result += $" value (dbl)| {e.Get<double>(fieldName).ToString()}";
				}
				else if (t?.Equals(typeof(bool)) ?? false)
				{
					result += $" value (bool)| {e.Get<bool>(fieldName).ToString()}";
				}

				result += "\n";
			}

			return result + "\n";
		}



		public bool Reset()
		{
			if (!XsMgr.Reset()) return false;

			deleteSchema(XsMgr?.XRoot.ExStoreGuid ?? Guid.Empty);
			deleteSchema(XsMgr?.XApp.ExStoreGuid ?? Guid.Empty);

			XsMgr.makeSchema();

			return true;
		}

		private void deleteSchema(Guid g)
		{
			if (g == Guid.Empty) return;

			Schema s = Schema.Lookup(g);

			if (s == null) return;

			Schema.EraseSchemaAndAllEntities(s, false);
		}

		public static ExStoreRtnCodes MakeRootExStorage()
		{
			XsMgr.XRoot = ExStoreRoot.Instance();

			ExStoreRtnCodes result;

			XsMgr.XRoot.Data[SchemaRootKey.RK_NAME].Value
				= "RootEx4" + Assembly.GetExecutingAssembly().GetName().Name;

			XsMgr.XRoot.Data[SchemaRootKey.RK_DESCRIPTION].Value
				= "Root Ex Storage Data for| " + AppRibbon.Doc.Title;

			result = XsMgr.WriteRoot();
			if (result != ExStoreRtnCodes.XRC_GOOD) return result;

			return ExStoreRtnCodes.XRC_GOOD;
		}

		public static ExStoreRtnCodes MakeAppAndCellsExStorage(int qty = 3)
		{
			XsMgr.XApp = ExStoreApp.Instance();

			ExStoreRtnCodes result;

			XsMgr.XApp.Data[SchemaAppKey.AK_NAME].Value = "Special_Name_01";
			XsMgr.XApp.Data[SchemaAppKey.AK_DESCRIPTION].Value = "Special Description 01";

			XsMgr.XCell = ExStoreCell.Instance();
			XsMgr.XCell.Add(3);

			for (int i = 0; i < qty; i++)
			{
				SampleCellData(XsMgr.XCell, i);
			}

			result = XsMgr.WriteAppAndCells( /*XsMgr.XApp, XsMgr.XCell*/);

			if (result != ExStoreRtnCodes.XRC_GOOD) return result;

			return ExStoreRtnCodes.XRC_GOOD;
		}

		public void makeSampleDataAppAndCell()
		{
			XsMgr.XApp = ExStoreApp.Instance();

			XsMgr.XApp.Data[SchemaAppKey.AK_NAME].Value = "Special_Name_01";
			XsMgr.XApp.Data[SchemaAppKey.AK_DESCRIPTION].Value = "Special Description 01";

			XsMgr.XCell = ExStoreCell.Instance();
			XsMgr.XCell.Add(3);

			for (int i = 0; i < 3; i++)
			{
				ExStorageTests.SampleCellData(XsMgr.XCell, i);
			}

			XsMgr.XApp.IsDefault = false;
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

		public void showStat(string mainMsg, ExStoreRtnCodes result, int step,
			string contentMsg = "status|")
		{
			bool init = XsMgr.Initialized;
			bool cfg = XsMgr.Configured;
			bool exr = !XsMgr.XRoot.IsDefault;
			bool exa = !XsMgr.XApp.IsDefault;
			bool exc = XsMgr.XCell != null;
			bool dsr = DsMgr?[DataStoreIdx.ROOT_DATA_STORE].GotDataStorage ?? false;
			bool dsa = DsMgr?[DataStoreIdx.APP_DATA_STORE_CURR].GotDataStorage ?? false;

			string status =
					$"{contentMsg}\n"
					+ $"result| {result}\n"
					+ $"Init| {init}\n"
					+ $"config| {cfg}\n"
					+ $"root data exists| {exr}\n"
					+ $"app data exists| {exa}\n"
					+ $"cell data exists| {exc}\n"
					+ $"root datastorage exists| {dsr}\n"
					+ $"app  datastorage exists| {dsa}\n"
				;

			TaskDialog td = new TaskDialog("Test Status");
			td.MainInstruction = $"{mainMsg}\nStep {step} status";
			td.MainContent = status;
			td.MainIcon = TaskDialogIcon.TaskDialogIconWarning;
			td.Show();
		}

		public void taskDialogWarning_Ok(string title, string main, string desc)
		{
			TaskDialog td = new TaskDialog(title);
			td.MainInstruction = main;
			td.MainContent = desc;
			td.MainIcon = TaskDialogIcon.TaskDialogIconWarning;
			td.CommonButtons = TaskDialogCommonButtons.Ok;
			td.Show();
		}

	#endregion

	#region private methods

		public static void SampleCellData(ExStoreCell xCell, int id)
		{
			xCell.Data[id][SchemaCellKey.CK_NAME].Value = $"Alpha_{id:D2}";
			xCell.Data[id][SchemaCellKey.CK_VERSION].Value = $"beta {id:D3}";
			xCell.Data[id][SchemaCellKey.CK_SEQUENCE].Value = (double) id;
			xCell.Data[id][SchemaCellKey.CK_UPDATE_RULE].Value = (int) UpdateRules.UR_UPON_REQUEST;
			xCell.Data[id][SchemaCellKey.CK_CELL_FAMILY_NAME].Value = $"CoolCell{id:D3}";
			xCell.Data[id][SchemaCellKey.CK_SKIP].Value = false;
			xCell.Data[id][SchemaCellKey.CK_XL_FILE_PATH].Value = $"c:\\file path\\filename{id:D3}.xls";
			xCell.Data[id][SchemaCellKey.CK_XL_WORKSHEET_NAME].Value = $"worksheet {id:d3}";
		}


		public static void SampleCellDataRevised(ExStoreCell xCell, string name, int id)
		{
			xCell.Data[id][SchemaCellKey.CK_NAME].Value = name;
			xCell.Data[id][SchemaCellKey.CK_VERSION].Value = $"Delta {id:D3}";
			xCell.Data[id][SchemaCellKey.CK_SEQUENCE].Value = (double) id;
			xCell.Data[id][SchemaCellKey.CK_UPDATE_RULE].Value = (int) UpdateRules.UR_AS_NEEDED;
			xCell.Data[id][SchemaCellKey.CK_CELL_FAMILY_NAME].Value = $"CoolCell{id:D3}";
			xCell.Data[id][SchemaCellKey.CK_SKIP].Value = false;
			xCell.Data[id][SchemaCellKey.CK_XL_FILE_PATH].Value = $"c:\\file path\\filename{id:D3}.xls";
			xCell.Data[id][SchemaCellKey.CK_XL_WORKSHEET_NAME].Value = $"worksheet {id:d3}";
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