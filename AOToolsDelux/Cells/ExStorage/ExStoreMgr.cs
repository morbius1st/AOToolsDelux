#region + Using Directives
using AOTools.Cells.SchemaDefinition;
using Autodesk.Revit.UI;

#endregion

// user name: jeffs
// created:   7/5/2021 6:55:42 AM

/*
 *
 *
 * note: since a schema is immutable, the subschema (cells) cannot
 * be modified / deleted 
 *
 *
 *									config		app
 *									to root		read
 * function							read stat	stat
 *									req'd		req'd
 *					
 *					init	confg	root		app	
 *					req'd	req'd	exists		exists	
 *
 * configure		true	false	false		n/a		
 * Read root		true	true	n/a			n/a		
 * Read app			n/a		n/a		true		false	true root exist == init true / confg true
 * Read cells		n/a		n/a		true		true	true root exist == init true / confg true
 * write root		n/a		true	false		n/a		true config == init true
 * write app +		n/a				true		n/a		true root exist == init true / config true
 *	cells
 * update root		-- n/a -		 not permitted
 * update app +		n/a		n/a		n/a			true	true app exist == init true
 * 	cells
 * del root			n/a		n/a		true		false	false app exist == false cell(s) exist / true root exists == init true / confg true
 * del app +		n/a		n/a		n/a			true	true app exists == true root exist / true app exists = true config
 *  cell(s)
 *
 * root exists		true			false		n/a		
 * app exists		n/a				true		false
 *
 * **	cell(s) exist	n/a		n/a			true	** probably do not need
 */


namespace AOTools.Cells.ExStorage
{
	public class ExStoreMgr
	{
		private ExStoreRoot xRoot;
		private ExStoreApp xApp;
		private ExStoreCell xCell;

		private ExStoreMgr()
		{
			xRoot = ExStoreRoot.Instance();
			xApp = ExStoreApp.Instance();

			Initialize();
			Configure();
		}

		public static ExStoreMgr XsMgr { get; private set; } = new ExStoreMgr();
		public static bool Initialized { get; private set; }
		public static bool Configured { get; private set; }

		public bool RootExStorExists { get; private set; }
		public bool AppExStorExists { get; private set; }

		private ExStoreHelper xsHlpr;

		public string OpDescription  { get; private set; }

		public ExStoreRoot XRoot => xRoot;
		public ExStoreApp XApp => xApp;
		public ExStoreCell XCell => xCell;

	#region initialize

		private void Initialize()
		{
			OpDescription = "Initialize ExStorage Manager";
			if (Initialized) return;

			xsHlpr = new ExStoreHelper();

			Initialized = true;
		}

	#endregion

	#region configure

		private void Configure()
		{
			OpDescription = "Configure ExStorage Manager";

			if (Configured) return;

			ExStoreRtnCodes result;

			CheckRootExStorExists();

			CheckAppExStorExists();

			Configured = true;
		}

		private void CheckRootExStorExists()
		{
			if (!IsInit("Configure Manager"))  return;
			if (RootExStorExists) return;

			ExStoreRtnCodes result = readRoot();

			if (result != ExStoreRtnCodes.GOOD) return;

			RootExStorExists = true;
		}

		private void CheckAppExStorExists()
		{
			if (!RootExStorExists) return;
			if (AppExStorExists) return;

			ExStoreRtnCodes result = readApp();

			if (result != ExStoreRtnCodes.GOOD) return;

			AppExStorExists = true;
		}

	#endregion

		public ExStoreRtnCodes UpdateCells(ExStoreCell xCell)
		{
			if (!AppExStorExists) return ExStoreRtnCodes.APP_NOT_EXIST;

			ExStoreRtnCodes result;

			result = xsHlpr.UpdateCellData(xApp, xCell);

			return result;
		}

	#region read

		public ExStoreRtnCodes ReadRoot(ref ExStoreRoot xRoot)
		{
			OpDescription = "Read Root Data";
			if (!IsInit("Read Root Data"))  return ExStoreRtnCodes.NOT_INIT;
			if (!IsConfig("Read root schema"))  return ExStoreRtnCodes.NOT_CONFIG;

			ExStoreRtnCodes result = readRoot();
			if (result != ExStoreRtnCodes.GOOD) return result;

			xRoot = this.xRoot;

			return ExStoreRtnCodes.GOOD;
		}

		public ExStoreRtnCodes ReadApp(ref ExStoreApp xApp)
		{
			OpDescription = "Read App Data";
			if (!RootExStorExists) return ExStoreRtnCodes.ROOT_NOT_EXIST;

			ExStoreRtnCodes result = readApp();
			if (result != ExStoreRtnCodes.GOOD) return result;

			xApp = this.xApp;

			return ExStoreRtnCodes.GOOD;
		}

		public ExStoreRtnCodes ReadCells(ref ExStoreCell xCell)
		{
			OpDescription = "Read Cells Data";
			if (!RootExStorExists) return ExStoreRtnCodes.ROOT_NOT_EXIST;
			if (!AppExStorExists) return ExStoreRtnCodes.APP_NOT_EXIST;

			// this.xCell = ExStoreCell.Instance(0);

			ExStoreRtnCodes result = xsHlpr.ReadCellData(ref this.xCell);
			if (result != ExStoreRtnCodes.GOOD) return result;

			xCell = this.xCell;

			return ExStoreRtnCodes.GOOD;
		}

	#endregion

	#region write

		public ExStoreRtnCodes WriteRoot(ExStoreRoot xRoot)
		{
			OpDescription = "Write Root Data";
			if (!IsInit("Save App Data"))  return ExStoreRtnCodes.NOT_INIT;
			if (RootExStorExists) return ExStoreRtnCodes.ROOT_NOT_EXIST;

			ExStoreRtnCodes result = xsHlpr.WriteRootData(xRoot);

			RootExStorExists = true;

			return result;
		}


		public ExStoreRtnCodes WriteAppAndCells(ExStoreApp xApp, ExStoreCell xCell)
		{
			OpDescription = "Write App and Cell Data";
			if (!RootExStorExists) return ExStoreRtnCodes.ROOT_NOT_EXIST;

			ExStoreRtnCodes result = xsHlpr.WriteAppAndCellsData(xApp, xCell);

			AppExStorExists = true;

			return result;
		}

	#endregion

	#region delete

		public ExStoreRtnCodes DeleteRoot()
		{
			OpDescription = "Delete Root Schema";
			if (!RootExStorExists) return ExStoreRtnCodes.ROOT_NOT_EXIST;
			if (AppExStorExists) return ExStoreRtnCodes.APP_NOT_EXIST;

			ExStoreRtnCodes result = xsHlpr.DeleteRootSchema();

			if (result != ExStoreRtnCodes.GOOD) return result;

			RootExStorExists = false;

			return ExStoreRtnCodes.GOOD;
		}

		public ExStoreRtnCodes DeleteApp()
		{
			OpDescription = "Delete App Schema";
			if (!AppExStorExists) return ExStoreRtnCodes.APP_NOT_EXIST;

			ExStoreRtnCodes result = xsHlpr.DeleteAppSchema();

			if (result != ExStoreRtnCodes.GOOD) return result;

			AppExStorExists = false;

			return ExStoreRtnCodes.GOOD;
		}

	#endregion


	#region support methods

		private ExStoreRtnCodes readRoot()
		{
			ExStoreRtnCodes result = xsHlpr.ReadRootData(ref this.xRoot);
			if (result != ExStoreRtnCodes.GOOD) return result;

			SchemaGuidManager.AppGuidUniqueString = xRoot.Data[SchemaRootKey.APP_GUID].Value;

			return ExStoreRtnCodes.GOOD;
		}

		private ExStoreRtnCodes readApp()
		{
			ExStoreRtnCodes result = xsHlpr.ReadAppData(ref this.xApp);

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