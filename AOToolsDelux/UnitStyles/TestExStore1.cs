#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Input;
using AOTools.Cells.ExDataStorage;
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
using static AOTools.Cells.ExDataStorage.DataStorageManager;

#endregion

// itemname:	UnitStylesCommand
// username:	jeffs
// created:		1/6/2018 3:55:08 PM


namespace AOTools
{
	[Transaction(TransactionMode.Manual)]
	class TestExStore0 : IExternalCommand
	{
		ExStorageTests xsTest = new ExStorageTests();

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
			Transaction t = null;

			AppRibbon.xm = XsMgr;
			AppRibbon.dm = DsMgr;

			// test to reset and eliminate everything from memory
			try
			{
				using (t = new Transaction(AppRibbon.Doc, "Test01"))
				{
					t.Start();
					{
						if (AppRibbon.Doc.IsDetached) return Result.Cancelled;

						AppRibbon.msg += "before reset\n";

						if (!xsTest.Reset())
						{
							xsTest.showStat("Reset", ExStoreRtnCodes.XRC_UNDEFINED, 0);
							t.RollBack();
							return Result.Failed;
						}

						AppRibbon.msg += "after reset\n";

						xsTest.showStat("Reset", ExStoreRtnCodes.XRC_UNDEFINED, 0);

						AppRibbon.msg += "before config\n";

						XsMgr.Configure();

						AppRibbon.msg += "after config\n";

						xsTest.showStat("re-configure", ExStoreRtnCodes.XRC_UNDEFINED, 0);
					}

					t.Commit();
				}
			}
			catch (OperationCanceledException)
			{
				if (t != null && t.HasStarted())
				{
					t.RollBack();
				}

				return Result.Failed;
			}

			return Result.Succeeded;
		}

		private void deleteSchema(Guid g)
		{
			if (g == Guid.Empty) return;

			Schema s = Schema.Lookup(g);

			Schema.EraseSchemaAndAllEntities(s, false);
		}
	}

	[Transaction(TransactionMode.Manual)]
	class TestExStore1 : IExternalCommand
	{
		ExStorageTests xsTest = new ExStorageTests();

		public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
		{
			AppRibbon.UiApp = commandData.Application;
			AppRibbon.Uidoc = AppRibbon.UiApp.ActiveUIDocument;
			AppRibbon.App =  AppRibbon.UiApp.Application;
			AppRibbon.Doc =  AppRibbon.Uidoc.Document;

			OutLocation = OutputLocation.DEBUG;

			AppRibbon.xm = XsMgr;
			AppRibbon.dm = DsMgr;

			return Test01();
		}

		private Result Test01()
		{
			Transaction t = null;

			// test to create the root datastorage / schema / entity
			// and create the app+cell datastorage

			AppRibbon.msg += "before sample data\n";

			// create the test data
			makeSampleData();
			// xsTest.showStat("Create root", ExStoreRtnCodes.XRC_UNDEFINED, 1);

			bool noSkip = true;

			try
			{
				using (t = new Transaction(AppRibbon.Doc, "Test01"))
				{
					t.Start();
					{
						if (AppRibbon.Doc.IsDetached) return Result.Cancelled;

						ExStoreRtnCodes result;

						AppRibbon.msg += "before make root\n";
						result = XsMgr.MakeRoot();
						AppRibbon.msg += "after make root\n";

						xsTest.showStat("Create root", result, 1);
						if (result != ExStoreRtnCodes.XRC_GOOD)
						{
							t.Commit();
							return Result.Failed;
						}


						if (noSkip)
						{
							AppRibbon.msg += "before make app\n";
							result = XsMgr.MakeApp();
							AppRibbon.msg += "after make app\n";

							xsTest.showStat("Create root", result, 1);
							if (result != ExStoreRtnCodes.XRC_GOOD)
							{
								t.Commit();
								return Result.Failed;
							}
						}
					}
					t.Commit();
				}
			}
			catch (OperationCanceledException)
			{
				if (t != null && t.HasStarted())
				{
					t.RollBack();
				}

				return Result.Failed;
			}

			return Result.Succeeded;
		}

		private void makeSampleData()
		{
			XsMgr.XRoot = ExStoreRoot.Instance();
			XsMgr.XRoot.IsDefault = false;

			// make data and flag as not default
			xsTest.makeSampleDataAppAndCell();
		}
	}


	[Transaction(TransactionMode.Manual)]
	class TestExStore2 : IExternalCommand
	{
		ExStorageTests xsTest = new ExStorageTests();

		public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
		{
			AppRibbon.UiApp = commandData.Application;
			AppRibbon.Uidoc = AppRibbon.UiApp.ActiveUIDocument;
			AppRibbon.App =  AppRibbon.UiApp.Application;
			AppRibbon.Doc =  AppRibbon.Uidoc.Document;

			OutLocation = OutputLocation.DEBUG;

			AppRibbon.xm = XsMgr;
			AppRibbon.dm = DsMgr;

			return Test01();
		}

		private Result Test01()
		{
			return Result.Succeeded;

			Transaction t = null;

			try
			{
				using (t = new Transaction(AppRibbon.Doc, "Test01"))
				{
					t.Start();
					{
						if (AppRibbon.Doc.IsDetached) return Result.Cancelled;

						if (XsMgr.AppExStorExists) return Result.Failed;

						ExStoreRtnCodes result;

						xsTest.makeSampleDataAppAndCell();
						result = XsMgr.MakeApp();
						xsTest.showStat("Create app", result, 2);
						if (result != ExStoreRtnCodes.XRC_GOOD) return Result.Failed;
					}

					t.Commit();
				}
			}
			catch (OperationCanceledException)
			{
				if (t != null && t.HasStarted())
				{
					t.RollBack();
				}

				return Result.Failed;
			}

			return Result.Succeeded;
		}
	}

	[Transaction(TransactionMode.Manual)]
	class TestExStore3 : IExternalCommand
	{
		ExStorageTests xsTest = new ExStorageTests();

		public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
		{
			AppRibbon.UiApp = commandData.Application;
			AppRibbon.Uidoc = AppRibbon.UiApp.ActiveUIDocument;
			AppRibbon.App =  AppRibbon.UiApp.Application;
			AppRibbon.Doc =  AppRibbon.Uidoc.Document;

			OutLocation = OutputLocation.DEBUG;

			AppRibbon.xm = XsMgr;
			AppRibbon.dm = DsMgr;

			return Test01();
		}

		private Result Test01()
		{
			try
			{
				if (AppRibbon.Doc.IsDetached) return Result.Cancelled;

				// ExStoreRtnCodes result = ExStoreRtnCodes.XRC_UNDEFINED;

				test01a();

			}
			catch (OperationCanceledException)
			{
				return Result.Failed;
			}

			return Result.Succeeded;
		}

		private void test01a()
		{
			// AppRibbon.idx = AppRibbon.idx++;

			string ms = AppRibbon.getTestMsg1s();
			string m1 = AppRibbon.appR.getTestMsg1();
			string m1t = AppRibbon.appR.testMessage1;

			string mxs = ExStoreMgr.ms;
			string mx1 = XsMgr.m1;

			string msg2 = $"messages| idx| {AppRibbon.idx}\n"
				+ $"rib static| {ms}\n"
				+ $"rib static indirect meth| {m1}\n"
				+ $"rib static indirect prop| {m1t}\n"
				+ $" xs static| {mxs}\n"
				+ $" xs static indirect prop| {mx1}";

			xsTest.taskDialogWarning_Ok("Static test",
				"Test static objects", msg2);
		}



		private void test01b()
		{
			Entity ex;
			DataStorage ds;
			bool result;
			string fieldName = XsMgr.XRoot.SchemaDefinition.Fields[SchemaRootKey.RK_APP_GUID].Name;

			result = DsMgr.GetDataStorage(
				DsMgr[DataStoreIdx.ROOT_DATA_STORE].Schema, out ex, out ds);

			if (!result)
			{
				xsTest.taskDialogWarning_Ok("schema lookup",
					$"datastorage| not found", 
					"\n"
					+ $"app guid| n/a");

				return;
			}

			string msg1 = ds.Name;
			string msg2 = ex.Get<string>(fieldName);

			xsTest.taskDialogWarning_Ok("schema lookup",
				$"datastorage name| {msg1}", 
				"\n"
				+ $"app guid|\n{msg2}");

		}

	}















	[Transaction(TransactionMode.Manual)]
	class TestExStore : IExternalCommand
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
			ExStoreMgr a = XsMgr;

			try
			{
				if (AppRibbon.Doc.IsDetached) return Result.Cancelled;

				ExStoreRtnCodes result;

				// result = test01_1();
				// if (result != ExStoreRtnCodes.GOOD) return Result.Failed;
				// info();

				// result = test01_2();
				// if (result != ExStoreRtnCodes.GOOD) return Result.Failed;

				// result = test01_3();
				// if (result != ExStoreRtnCodes.GOOD) return Result.Failed;

				result = test01_4();
				if (result != ExStoreRtnCodes.XRC_GOOD) return Result.Failed;


				// // create sample root storage
				// result = ExStorageTests.MakeRootExStorage();
				// if (result != ExStoreRtnCodes.GOOD) return Result.Failed;
				//
				// // create sample app and cell storage
				// result = ExStorageTests.MakeAppAndCellsExStorage();
				// if (result != ExStoreRtnCodes.GOOD) return Result.Failed;
			}
			catch (OperationCanceledException)
			{
				return Result.Failed;
			}


			return Result.Succeeded;
		}


		private void stat(string mainMsg, ExStoreRtnCodes result,
			string contentMsg = "status|")
		{
			bool init = XsMgr.Initialized;
			bool cfg = XsMgr.Configured;
			bool xr = XsMgr.RootExStorExists;
			bool xa = XsMgr.AppExStorExists;

			string status =
				$"{contentMsg}\n"
				+ $"result| {result}\n"
				+ $"Init| {init}\n"
				+ $"config| {cfg}\n"
				+ $"root exists| {xr}\n"
				+ $"app  exists| {xa}";

			TaskDialog td = new TaskDialog("Test Status");
			td.MainInstruction = $"{mainMsg}\nStep {step} status";
			td.MainContent = status;
			td.MainIcon = TaskDialogIcon.TaskDialogIconWarning;
			td.Show();
		}


		private static int step = 0;


		private ExStoreRtnCodes test01_4()
		{
			ExStoreRtnCodes result = ExStoreRtnCodes.XRC_GOOD;

			switch (step)
			{
			case 0:
				{
					// step 0 - just the initial status
					stat("Initial status", result);
					break;
				}

			case 1:
				{
					// step one - make the root data store
					XsMgr.XRoot = ExStoreRoot.Instance();
					result = XsMgr.CreateRoot();

					stat("step 1| create root", result);
					break;
				}

			case 2:
				{
					// step two - make the app & cell data store
					makeSampleDataAppAndCell();
					result = XsMgr.CreateAppDs();

					stat("Step 2| create app", result);
					break;
				}

			case 3:
				{
					result = ExStoreRtnCodes.XRC_FAIL;
					stat("Step 3", result);
					break;
				}

			case 4:
				{
					result = ExStoreRtnCodes.XRC_FAIL;
					stat("Step 4", result);
					break;
				}

			case 5:
				{
					result = ExStoreRtnCodes.XRC_FAIL;
					stat("Step 5", result);
					step = 0;
					break;
				}
			}

			if (result != ExStoreRtnCodes.XRC_GOOD) step -= 1;

			step++;

			return result;
		}


		private void makeSampleDataAppAndCell()
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
		}


		private ExStoreRtnCodes test01_3()
		{
			ExStoreRtnCodes result = ExStoreRtnCodes.XRC_GOOD;

			ExStoreRoot xRoot = ExStoreRoot.Instance();

			stat("Status| ", result);

			result = makeRoot();
			if (result != ExStoreRtnCodes.XRC_GOOD) return result;

			return ExStoreRtnCodes.XRC_GOOD;
		}


		private ExStoreRtnCodes makeRoot()
		{
			ExStoreRtnCodes result;

			XsMgr.XRoot = ExStoreRoot.Instance();

			result = XsMgr.WriteRoot();
			if (result != ExStoreRtnCodes.XRC_GOOD) return result;

			return ExStoreRtnCodes.XRC_GOOD;
		}

		// private ExStoreRtnCodes test01_2()
		// {
		// 	Entity e = null;
		//
		// 	ExStoreRoot xRoot = ExStoreRoot.Instance();
		//
		// 	Schema sRoot = XsMgr.MakeRootSchema(xRoot);
		//
		// 	DsMgr.CreateDataStorage(sRoot, out e);
		//
		// 	// xRoot.ExStoreGuid = Guid.NewGuid();
		// 	//
		// 	// sRoot = XsMgr.MakeRootSchema(xRoot);
		// 	//
		// 	// DataStorageManager.Instance.CreateDataStorage(sRoot);
		// 	//
		// 	//
		// 	//
		// 	// xRoot.ExStoreGuid = Guid.NewGuid();
		// 	//
		// 	// sRoot = XsMgr.MakeRootSchema(xRoot);
		// 	//
		// 	// DataStorageManager.Instance.CreateDataStorage(sRoot);
		//
		// 	e = XsMgr.CheckRootDataStorExists();
		//
		// 	return ExStoreRtnCodes.XRC_GOOD;
		// }

		private ExStoreRtnCodes test01_1()
		{
			ExStoreRtnCodes result;

			if (!DsMgr[DataStoreIdx.ROOT_DATA_STORE].GotDataStorage)
			{
				DsNotExist();
				return ExStoreRtnCodes.XRC_FAIL;
			}

			// create sample root storage
			result = ExStorageTests.MakeRootExStorage();
			if (result != ExStoreRtnCodes.XRC_GOOD) return result;

			// create sample app and cell storage
			result = ExStorageTests.MakeAppAndCellsExStorage();
			if (result != ExStoreRtnCodes.XRC_GOOD) return result;

			return ExStoreRtnCodes.XRC_GOOD;
		}

		private void DsNotExist()
		{
			TaskDialog td = new TaskDialog("DS Not Exist");
			td.MainInstruction = "The Root Data Store was not found";
			td.MainIcon = TaskDialogIcon.TaskDialogIconWarning;
			td.Show();
		}


		private void info()
		{
			ExStoreMgr x = XsMgr;

			ExStoreRoot xrc = XsMgr.XRoot;
			// SchemaDictionaryRoot xrf = xrc.Fields;
			SchemaDictionaryRoot xrl = xrc.Data;
			SchemaDictionaryRoot xrv = xrc.DefaultValues();

			ExStoreApp xac = XsMgr.XApp;
			// SchemaDictionaryApp xaf = xac.Fields;
			SchemaDictionaryApp xal = xac.Data;
			SchemaDictionaryApp xav = xac.DefaultValues();

			ExStoreCell xcc = XsMgr.XCell;
			SchemaDictionaryCell xcf = xcc.Fields;
			List<SchemaDictionaryCell> xcl = xcc.Data;
			Dictionary<string, string> xcd = xcc.SubSchemaFields;
			SchemaDictionaryCell xcv = xcc.DefaultValues();
		}
	}
}