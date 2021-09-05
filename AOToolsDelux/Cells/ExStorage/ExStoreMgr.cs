#region + Using Directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using AOTools.Cells.ExDataStorage;
using AOTools.Cells.SchemaDefinition;
using AOTools.Cells.Tests;
using Autodesk.Revit.DB.ExtensibleStorage;
using Autodesk.Revit.Exceptions;
using Autodesk.Revit.UI;
using static AOTools.Cells.ExDataStorage.DataStoreIdx;
using static AOTools.Cells.ExDataStorage.DataStorageManager;

#endregion

// user name: jeffs
// created:   7/5/2021 6:55:42 AM

/*
 * process:
 *  config
 *   +-> finds info -> read info.
 *   +-> not find info -> create info -> write info
 *
 * make root path
 * makeroot()-> DsMgr.CreateDataStorage<-
 *           +> WriteRoot-> xsHlpr.WriteRootData -> makeRootSchema -> makeSchemaDef <-
 *                                                                 +> makeSchemaFields -> makeSchemaField <-
 *                                               +> writeData <-
 *
 *
 *
 * note: since a schema is immutable, the subschema (cells) cannot
 * be modified / deleted 
 *
 * procedures
 * startup
 *  +> ExMgr
 *     +> initialize()
 *        * init to true
 *        * status to INIT
 *     +> config()
 *        * status is INIT
 *        +> dsExists == true // note that ds and root get setup together
 *           +> rootExists == true // root can exist separate from app & cell
 *              +> appExists == true // note that app / cell will be all config'd together
 *                 * proceed "normal"
 *                   * status to READY
 *              +> appExists == false
 *                 * need cell information to proceed
 *                   * status to NEED_APP_CELL
 *           +> rootExists == false
 *              * this should never happen as ds and root get setup together
 *                * status to FAIL
 *       +> dsExists == false
 *          * need to setup system
 *            * status to NEED_CONFIG
 *
 * startup
 *  +> start ExMgr
 *     +> status == READY -> proceed normal -> flip buttons
 *     +> status == INIT -> should never happen -> report program failure - exit
 *     +> status == FAIL -> should never happen -> report program failure - exit
 *     +> status == NEED_APP_CELL -> flip buttons
 *     +> status == NEED_CONFIG -> flip buttons
 *
 *  buttons (on a dialog box)
 *                        button             
 *                        off unless         
 *                        status             
 *  * initialize          is NEED_CONFIG     
 *  * add cell			  is NEED_APP_CELL or
 *                        is READY
 *  * show root info	  is READY
 *  * show app info		  is READY
 *    * show cell info	  is READY
 *  * show all cell info  is READY
 *  * modify cell		  is READY
 *  * delete cell		  is READY
 *  * remove system		  is READY
 *
 *			
 *			
 * function							datastore (ds)
 *					init	confg	root	app		root		app	
 *					req'd	req'd	exists	exists	exists		exists	
 *
 * initialize		false	n/a		n/a		n/a		n/a			n/a
 * configure		true	false	n/a		n/a		n/a			n/a		tests ds / root / app exists
 *
 * Create ds		true	false	false	n/a		n/a			n/a
 *
 * Read root		n/a		n/a		true			n/a			n/a		true ds == init / config true
 * Read app			n/a		n/a		n/a				true		false	true root exist == init / confg / ds true
 * Read cells		n/a		n/a		n/a				true		true	true root exist == init / confg / ds true
 * write root		n/a		n/a		true			false		n/a		true ds == init / config true
 * write app +		n/a		n/a		n/a				true		n/a		true root exist == init / confg / ds true
 *	cells
 * update root		-- n/a -						 not permitted
 * update app +		n/a		n/a		n/a				true 		n/a		true root exist == init true
 * 	cells
 * del root			n/a		n/a		n/a				true		false	false app exist == false cell(s) exist / true root exists == init / confg / ds true
 * del app +		n/a		n/a		n/a				n/a			true	true app exists == init / confg / ds true
 *  cell(s)
 *
 * ds exists		true	false	false			n/a			n/a
 * root exists		na		n/a		true			false		n/a		
 * app exists		n/a		n/a		n/a				true		false
 *
 * **	cell(s) exist	n/a							n/a			true	** probably do not need
 */

namespace AOTools.Cells.ExStorage
{
	public class ExStoreMgr
	{
		private static int idx1;
		public int idx2x = 0;

		public static int idx
		{
			get
			{
				idx1 = (idx1 + 1) % 4;
				return idx1;
			}
		}

		private static string[] names = new [] {"alpha", "beta", "delta", "omega"};

		public static string ms => names[idx];
		public string m1 => names[idx];

		public int mi => idx1;

		public int idx2
		{
			get
			{
				idx2x = (idx2x + 1) % 4;
				return idx2x;
			}
		}

		public string mx => names[idx2];

		public ExStoreMgr()
		{
			ExStorageTests.location = 100;

			XRoot = ExStoreRoot.Instance();
			XApp = ExStoreApp.Instance();

			Initialize();
			Configure();
		}

		public static ExStoreMgr XsMgr { get; private set; } = new ExStoreMgr();

		// public DataStorageManager DsMgr;

		public bool Initialized { get; private set; }
		public bool Configured { get; private set; }

		// public bool[] DataExStoreExists { get; private set; }

		public bool RootExStorExists => xRoot.IsDefault == false;

		/*{ get; private set; }*/
		public bool AppExStorExists => xApp.IsDefault == false;
		/*{ get; private set; }*/

		public string OpDescription  { get; private set; }

		ExStorageTests xsTest = new ExStorageTests(null);
		private ExStoreHelper xsHlpr;

		// todo - move data stores to the datastoragemanager

		// private Entity[] dataStores = new Entity[APP_DATA_STORE + 1];

		private ExStoreRoot xRoot;
		private ExStoreApp xApp;
		private ExStoreCell xCell;

		public ExStoreRoot XRoot
		{
			get => xRoot;
			set => xRoot = value;
		}

		public ExStoreApp XApp
		{
			get => xApp;
			set => xApp = value;
		}

		public ExStoreCell XCell
		{
			get => xCell;
			set => xCell = value;
		}


	#region initialize

		private void Initialize()
		{
			ExStorageTests.location = 110;
			OpDescription = "Initialize ExStorage Manager";
			if (Initialized) return;

			xsHlpr = new ExStoreHelper();

			resetExStore();
			makeSchema();

			Initialized = true;
			ExStorageTests.location = 119;
		}

		private void resetExStore()
		{
			xRoot = ExStoreRoot.Instance();
			xApp = ExStoreApp.Instance();
			xCell = ExStoreCell.Instance();
		}

		public void makeSchema()
		{
			Schema s;

			s = xsHlpr.MakeRootSchema(xRoot);

			DsMgr[ROOT_DATA_STORE].Schema = s;


			// s = xsHlpr.MakeAppSchema(xApp, xCell);
			// DsMgr[APP_DATA_STORE_CURR].Schema = s;
			//
			// DsMgr[APP_DATA_STORE_NEW].Schema = null;
		}

		public bool Reset()
		{
			if (AppRibbon.App.Documents.Size > 1)
			{
				xsTest.taskDialogWarning_Ok("Cells",
					"It is Unsafe to Reset Now", "A reset can only be preformed when a single document "
					+ "is open because a reset would apply to all open documents");

				return false;
			}

			Configured = false;

			DsMgr.Reset();

			// this must follow the reset else schema's don't exist
			resetExStore();

			Initialized = true;

			return true;
		}

	#endregion

	#region configure

		public void Configure()
		{
			ExStorageTests.location = 120;
			OpDescription = "Configure ExStorage Manager";

			if (Configured) return;

			ExStoreRtnCodes result;

			// check if the root datastorage element exists
			// in the database
			// if not, create one using the default information
			ExStorageTests.location = 121;
			if (!DsMgr[ROOT_DATA_STORE].GotDataStorage)
			{
				ExStorageTests.location = 122;
				AppRibbon.msg += "don't got xroot\n";
				CheckRootDataStorExists();
				if (!DsMgr[ROOT_DATA_STORE].GotDataStorage) return;
			}

			AppRibbon.msg += "checking that root ex data exists\n";

			ExStorageTests.location = 123;
			CheckRootExStorExists();

			if (!RootExStorExists) return;

			// check if the app datastorage element exists
			// in the database
			ExStorageTests.location = 124;
			if (!DsMgr[APP_DATA_STORE_CURR].GotDataStorage)
			{
				ExStorageTests.location = 125;
				AppRibbon.msg += "don't got xapp\n";
				CheckAppDataStorExists();
			}

			ExStorageTests.location = 126;
			AppRibbon.msg += "checking that app ex data exists\n";
			CheckAppExStorExists();

			if (!AppExStorExists) return;

			Configured = true;
			ExStorageTests.location = 129;
		}

		private void CheckRootExStorExists()
		{
			ExStorageTests.location = 130;
			if (RootExStorExists) return;
			if (!DsMgr[ROOT_DATA_STORE].GotDataStorage) return;

			ExStoreRtnCodes result = readRoot();

			if (result != ExStoreRtnCodes.XRC_GOOD) return;

			ExStorageTests.location = 139;
		}

		private void CheckAppExStorExists()
		{
			ExStorageTests.location = 140;
			if (!RootExStorExists) return;
			if (AppExStorExists) return;
			if (!DsMgr[ROOT_DATA_STORE].GotDataStorage) return;

			ExStoreRtnCodes result = readApp();

			if (result != ExStoreRtnCodes.XRC_GOOD) return;

			ExStorageTests.location = 149;
		}

		public void CheckRootDataStorExists()
		{
			ExStorageTests.location = 150;
			if (!IsInit("Check data Store exists"))  return;
			if (Configured)  return;
			if (DsMgr[ROOT_DATA_STORE].GotDataStorage) return;

			Entity e;
			DataStorage ds;

			bool result = xsHlpr.GetRootEntity(xRoot, out e, out ds);

			if (result)
			{
				DsMgr[ROOT_DATA_STORE].DataStorage = ds;
				// DsMgr[ROOT_DATA_STORE].Entity = e;
				// DsMgr[ROOT_DATA_STORE].Schema = s;
			}
			ExStorageTests.location = 159;
		}

		public void CheckAppDataStorExists()
		{
			ExStorageTests.location = 160;
			if (DsMgr[APP_DATA_STORE_CURR].GotDataStorage) return;

			// ExStoreApp xApp = ExStoreApp.Instance();

			Entity e;
			DataStorage ds;

			bool result = xsHlpr.GetAppEntity(xApp, xCell, out e, out ds);

			if (result)
			{
				DsMgr[APP_DATA_STORE_CURR].DataStorage = ds;
				// DsMgr[APP_DATA_STORE].Entity = e;
			}
			ExStorageTests.location = 169;
		}

		private ExStoreRtnCodes CreateRootDataStore()
		{
			ExStoreRtnCodes result;

			result = DsMgr.CreateDataStorage(ROOT_DATA_STORE);
			if (result != ExStoreRtnCodes.XRC_GOOD) return result;

			result = WriteRoot();
			if (result != ExStoreRtnCodes.XRC_GOOD) return result;

			return ExStoreRtnCodes.XRC_GOOD;
		}

		private ExStoreRtnCodes CreateAppDataStore()
		{
			ExStoreRtnCodes result;

			result = DsMgr.CreateDataStorage(APP_DATA_STORE_CURR);
			if (result != ExStoreRtnCodes.XRC_GOOD) return result;

			result = WriteAppAndCells();
			if (result != ExStoreRtnCodes.XRC_GOOD) return result;

			return ExStoreRtnCodes.XRC_GOOD;
		}

	#endregion

	#region create

		public ExStoreRtnCodes MakeRoot()
		{
			OpDescription = "Create root datastore";
			if (!IsInit(OpDescription))  return ExStoreRtnCodes.XRC_NOT_INIT;
			if (Configured) return ExStoreRtnCodes.XRC_IS_CONFIG;
			if (DsMgr[ROOT_DATA_STORE].GotDataStorage) return ExStoreRtnCodes.XRC_DS_EXISTS;

			ExStoreRtnCodes result;

			// result = CreateRootDataStore();
			// if (result != ExStoreRtnCodes.XRC_GOOD) return result;

			result = DsMgr.CreateDataStorage(ROOT_DATA_STORE);
			if (result != ExStoreRtnCodes.XRC_GOOD) return result;

			result = WriteRoot();
			if (result != ExStoreRtnCodes.XRC_GOOD) return result;

			// Configure();

			return ExStoreRtnCodes.XRC_GOOD;
		}

		public ExStoreRtnCodes MakeApp()
		{
			OpDescription = "Create app datastore";
			if (!IsInit(OpDescription))  return ExStoreRtnCodes.XRC_NOT_INIT;
			if (Configured) return ExStoreRtnCodes.XRC_IS_CONFIG;
			if (!DsMgr[ROOT_DATA_STORE].GotDataStorage) return ExStoreRtnCodes.XRC_DS_NOT_EXIST;
			if (DsMgr[APP_DATA_STORE_CURR].GotDataStorage) return ExStoreRtnCodes.XRC_DS_EXISTS;

			ExStoreRtnCodes result;

			// result = CreateAppDataStore();
			// if (result != ExStoreRtnCodes.XRC_GOOD) return result;

			result = DsMgr.CreateDataStorage(APP_DATA_STORE_CURR);
			if (result != ExStoreRtnCodes.XRC_GOOD) return result;

			result = WriteAppAndCells();
			if (result != ExStoreRtnCodes.XRC_GOOD) return result;

			// Configure();

			return ExStoreRtnCodes.XRC_GOOD;
		}

		/// <summary>
		/// create the datastore that the ex data is attached to <br/>
		/// 
		/// </summary>
		/// <param name="xRoot"></param>
		/// <returns></returns>
		public ExStoreRtnCodes CreateRoot()
		{
			OpDescription = "Create root datastore";
			if (!IsInit(OpDescription))  return ExStoreRtnCodes.XRC_NOT_INIT;
			if (Configured) return ExStoreRtnCodes.XRC_IS_CONFIG;
			if (DsMgr[ROOT_DATA_STORE].GotDataStorage) return ExStoreRtnCodes.XRC_DS_EXISTS;

			ExStoreRtnCodes result;
			Entity e = null;

			Schema sRoot = xsHlpr.GetRootSchema();
			//MakeRootSchema(xRoot);

			result = DsMgr.CreateDataStorage(sRoot, out e);

			DsMgr[ROOT_DATA_STORE].Entity = e;

			if (result != ExStoreRtnCodes.XRC_GOOD) return result;

			result = WriteRoot();
			if (result != ExStoreRtnCodes.XRC_GOOD) return result;

			// Configure();

			return ExStoreRtnCodes.XRC_GOOD;
		}

		public void xxx()
		{
			Schema sApp = xsHlpr.MakeAppSchema(xApp, xCell);


			Debug.WriteLine($"got schema| ");
			Debug.WriteLine($"name| {sApp.SchemaName}");
			Debug.WriteLine($"desc| {sApp.Documentation}");
			Debug.WriteLine($"guid| {sApp.GUID.ToString()}");

			IList<Field> fields = sApp.ListFields();

			Debug.WriteLine($"# fields| {fields.Count}");

			foreach (Field f in fields)
			{
				Debug.WriteLine($"Field| names| {f.FieldName}"
					+ $"  type| {f.ValueType.Name}"
					+ $"  desc| {f.Documentation}"
					+ $"  sub-schema desc| {(f.SubSchema?.Documentation ?? "none")}");
			}

		}

		public ExStoreRtnCodes CreateAppDs()
		{
			OpDescription = "Create app datastore";
			if (!IsInit(OpDescription))  return ExStoreRtnCodes.XRC_NOT_INIT;
			if (Configured) return ExStoreRtnCodes.XRC_IS_CONFIG;
			if (!DsMgr[ROOT_DATA_STORE].GotDataStorage) return ExStoreRtnCodes.XRC_DS_NOT_EXIST;
			if (DsMgr[APP_DATA_STORE_CURR].GotDataStorage) return ExStoreRtnCodes.XRC_DS_EXISTS;

			ExStoreRtnCodes result;
			Entity e = null;

			Schema sApp = xsHlpr.MakeAppSchema(xApp, xCell);
			// GetAppSchemaCurr();

			result = DsMgr.CreateDataStorage(sApp, out e);

			DsMgr[APP_DATA_STORE_CURR].Entity = e;

			if (result != ExStoreRtnCodes.XRC_GOOD) return result;

			Configure();

			return ExStoreRtnCodes.XRC_GOOD;
		}

	#endregion

	#region read

		private ExStoreRtnCodes ReadRoot()
		{
			OpDescription = "Read Root Data";
			if (!DsMgr[ROOT_DATA_STORE].GotDataStorage) return ExStoreRtnCodes.XRC_DS_NOT_EXIST;
			// if (!IsInit(OpDescription))  return ExStoreRtnCodes.NOT_INIT;
			// if (!IsConfig(OpDescription))  return ExStoreRtnCodes.NOT_CONFIG;

			ExStoreRtnCodes result = readRoot();
			if (result != ExStoreRtnCodes.XRC_GOOD) return result;

			return ExStoreRtnCodes.XRC_GOOD;
		}

		private ExStoreRtnCodes ReadApp()
		{
			OpDescription = "Read App Data";
			if (!RootExStorExists) return ExStoreRtnCodes.XRC_ROOT_NOT_EXIST;

			ExStoreRtnCodes result = readApp();
			if (result != ExStoreRtnCodes.XRC_GOOD) return result;

			xApp = this.XApp;

			return ExStoreRtnCodes.XRC_GOOD;
		}

		private ExStoreRtnCodes ReadCells()
		{
			OpDescription = "Read Cells Data";
			if (!RootExStorExists) return ExStoreRtnCodes.XRC_ROOT_NOT_EXIST;
			if (!AppExStorExists) return ExStoreRtnCodes.XRC_APP_NOT_EXIST;

			// this.xCell = ExStoreCell.Instance(0);

			ExStoreRtnCodes result = xsHlpr.ReadCellData(ref this.xCell);
			if (result != ExStoreRtnCodes.XRC_GOOD) return result;

			xCell = XCell;

			return ExStoreRtnCodes.XRC_GOOD;
		}

	#endregion

	#region write

		// public ExStoreRtnCodes WriteRoot(ExStoreRoot xRoot)
		public ExStoreRtnCodes WriteRoot()
		{
			OpDescription = "Write Root Data";
			if (!DsMgr[ROOT_DATA_STORE].GotDataStorage)
				return ExStoreRtnCodes.XRC_DS_NOT_EXIST;
			// if (!IsInit("Save App Data"))  return ExStoreRtnCodes.NOT_INIT;
			// if (RootExStorExists) return ExStoreRtnCodes.ROOT_NOT_EXIST;

			// ExStoreRtnCodes result = xsHlpr.WriteRootData(xRoot, DsMgr.DataStoreEntity[ROOT_DATA_STORE]);
			ExStoreRtnCodes result = xsHlpr.WriteRootData(xRoot,
				DsMgr[ROOT_DATA_STORE].DataStorage);

			// RootExStorExists = true;

			// this.xRoot = xRoot;

			return result;
		}

		public ExStoreRtnCodes WriteAppAndCells( /*ExStoreApp xApp, ExStoreCell xCell*/)
		{
			OpDescription = "Write App and Cell Data";
			if (!DsMgr[APP_DATA_STORE_CURR].GotDataStorage)
				return ExStoreRtnCodes.XRC_ROOT_NOT_EXIST;

			ExStoreRtnCodes result = xsHlpr.WriteAppAndCellsData(xApp, xCell,
				DsMgr[APP_DATA_STORE_CURR].DataStorage);

			return result;
		}

	#endregion

	#region update

		public ExStoreRtnCodes UpdateApp()
		{
			ExStoreRtnCodes result;
			if (!AppExStorExists) return ExStoreRtnCodes.XRC_APP_NOT_EXIST;

			result = DeleteApp();
			if (result != ExStoreRtnCodes.XRC_GOOD) return result;

			// write the modified app info and the current cell info
			WriteAppAndCells( /*xApp, XCell*/);

			return result;
		}

		public ExStoreRtnCodes UpdateCells()
		{
			if (!AppExStorExists) return ExStoreRtnCodes.XRC_APP_NOT_EXIST;

			ExStoreRtnCodes result;

			result = xsHlpr.UpdateCellData(XApp, xCell, DsMgr[APP_DATA_STORE_CURR].DataStorage);

			return result;
		}

	#endregion

	#region delete

		public Tuple<int, int> DeleteByVendorId(string vendorId)
		{
			xsHlpr = new ExStoreHelper();
			return xsHlpr.DeleteSchemaPerVendorId(vendorId);
		}

		public ExStoreRtnCodes DeleteDs()
		{
			return ExStoreRtnCodes.XRC_GOOD;
		}

		public ExStoreRtnCodes DeleteRoot()
		{
			OpDescription = "Delete Root Schema";
			if (!RootExStorExists) return ExStoreRtnCodes.XRC_ROOT_NOT_EXIST;
			if (AppExStorExists) return ExStoreRtnCodes.XRC_APP_NOT_EXIST;

			ExStoreRtnCodes result = xsHlpr.DeleteRootSchema();

			if (result != ExStoreRtnCodes.XRC_GOOD) return result;

			// RootExStorExists = false;

			resetExStore();

			return ExStoreRtnCodes.XRC_GOOD;
		}

		public ExStoreRtnCodes DeleteApp()
		{
			OpDescription = "Delete App Schema";
			if (!AppExStorExists) return ExStoreRtnCodes.XRC_APP_NOT_EXIST;

			ExStoreRtnCodes result = xsHlpr.DeleteAppSchema();

			if (result != ExStoreRtnCodes.XRC_GOOD) return result;

			return ExStoreRtnCodes.XRC_GOOD;
		}

	#endregion

	#region support methods

		// public Schema MakeRootSchema(ExStoreRoot xRoot)
		// {
		// 	return xsHlpr.makeRootSchema(xRoot);
		// }

		// public Schema MakeAppSchema(ExStoreApp xApp, ExStoreCell xCell)
		// {
		// 	return xsHlpr.makeAppSchema(xApp, xCell);
		// }


		private ExStoreRtnCodes readRoot()
		{
			ExStoreRtnCodes result = xsHlpr.ReadRootData(ref this.xRoot);
			if (result != ExStoreRtnCodes.XRC_GOOD) return result;

			xRoot.IsDefault = false;
			SchemaGuidManager.AppGuidString = XRoot.Data[SchemaRootKey.RK_APP_GUID].Value;

			return ExStoreRtnCodes.XRC_GOOD;
		}

		private ExStoreRtnCodes readApp()
		{
			ExStoreRtnCodes result = xsHlpr.ReadAppData(ref this.xApp);

			if (result != ExStoreRtnCodes.XRC_GOOD) return result;

			xApp.IsDefault = false;
			xCell.IsDefault = false;

			return result;
		}

		private bool IsInit(string desc)
		{
			if (Initialized) return true;

			NotInitFail(desc, "Manager is not initialized");

			return false;
		}

		private bool IsConfig(string desc)
		{
			if (Configured) return true;

			NotInitFail(desc, "Manager is not configured");

			return false;
		}

		private void NotInitFail(string desc, string why)
		{
			TaskDialog td = new TaskDialog("AO Tools - " + desc);
			td.MainIcon = TaskDialogIcon.TaskDialogIconWarning;
			td.MainInstruction = desc + " - failed";
			td.MainContent = why;
			td.Show();
		}

		public void DeleteSchemaFail(string desc)
		{
			TaskDialog td = new TaskDialog("AO Tools - " + desc);
			td.MainIcon = TaskDialogIcon.TaskDialogIconWarning;
			td.MainInstruction = desc + " - failed";
			td.Show();
		}

		public void ReadSchemaFail(string desc)
		{
			TaskDialog td = new TaskDialog("AO Tools - " + desc);
			td.MainIcon = TaskDialogIcon.TaskDialogIconWarning;
			td.MainInstruction = desc + " - failed";
			td.Show();
		}

		public void ExStoreFail(string descTitle, string descMsg, string errorMsg)
		{
			TaskDialog td = new TaskDialog("AO Tools - " + descTitle);
			td.MainIcon = TaskDialogIcon.TaskDialogIconWarning;
			td.MainInstruction =
				"Process Failed| " + descMsg + "\n"
				+ "Error| " + errorMsg;
			td.Show();
		}

	#endregion
	}
}