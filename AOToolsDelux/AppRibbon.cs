#region Namespaces

using System;
using System.Collections.Generic;
using System.Reflection;
using AOTools.Cells.ExDataStorage;
using AOTools.Cells.ExStorage;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.DB.ExtensibleStorage;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Events;
using Application = Autodesk.Revit.ApplicationServices.Application;
using UtilityLibrary;
using static UtilityLibrary.MessageUtilities;
using static AOTools.AppSettings.ConfigSettings.SettingsUsr;

#endregion

namespace AOTools
{


	class AppRibbon : IExternalApplication
	{
		public static ExStoreMgr xm; // = ExStoreMgr.XsMgr;
		public static DataStorageManager dm;// = DataStorageManager.DsMgr;
		public static ExStoreRoot xr => xm?.XRoot ?? null;
		public static ExStoreApp xa => xm?.XApp ?? null;
		public static ExStoreCell xc  => xm?.XCell ?? null;

		public static bool xr_isdefault => xr?.IsDefault ?? true;
		public static bool xa_isdefault => xa?.IsDefault ?? true;

		public static bool xmInit => xm?.Initialized ?? false;
		public static bool xmCfg => xm?.Configured ?? false;

		public static DataStorage ds0 => dm?[DataStoreIdx.ROOT_DATA_STORE].DataStorage ?? null;
		public static DataStorage ds1 => dm?[DataStoreIdx.APP_DATA_STORE_CURR].DataStorage ?? null;

		public static string msg { get; set; }


		internal const string APP_NAME = "AOTools";
		private const string TAB_NAME = "AO Tools";

		private const string NAMESPACE_PREFIX = "AOTools.Resources";
		private const string AO_TOOLS_PANEL_NAME = "AO Tools";
		private const string UNITS_PANEL_NAME = "Project Units";
		private const string EX_STORE_PANEL_NAME = "Ex Store";

		private const string BUTTON_UNITSTYLES = "Unit\nStyles";
		private const string BUTTON_UNITSTYLEDELETE = "Delete\nStyles";

		private const string BUTTON_SELECT = "Select\nElement";

		private const string BUTTON_ROOT_EX_STORE = "Make Root\nEx Storage";
		private const string BUTTON_DATA_STORAGE = "Make App\n& Cell";
		private const string BUTTON_READ_ROOT_EX_STORE = "Read Root\nEx Storage";
		private const string BUTTON_READ_APP_EX_STORE = "Read App\nEx Storage";
		private const string BUTTON_READ_CELL_EX_STORE = "Read Cell\nEx Storage";
		private const string BUTTON_READ_SCHEMA = "Read\nSchema";
		private const string BUTTON_READ_APPRIBBON_INFO = "Read App-\nRibbon Info";

		private const string BUTTON_TEST0 = "RESET";
		private const string BUTTON_TEST1 = "TEST 1";
		private const string BUTTON_TEST2 = "TEST 2";
		private const string BUTTON_TEST3 = "TEST 3";

		private const string BTN_EX_STOR_DEL_ROOT = "Delete\nRoot Schema";
		private const string BTN_EX_STOR_DEL_APP = "Delete\nApp Schema";
		private const string BTN_EX_STOR_MOD_CELLS = "Modify\nCells";
		private const string BTN_EX_STOR_MOD_APP = "Modify\nApp";


//
//		private const string BUTTON_UNIT_FTIN_NAME   = "Units\nto Feet-In";
//		private const string BUTTON_UNIT_FRACIN_NAME = "Units\nto Frac In";
//		private const string BUTTON_UNIT_DECFT_NAME  = "Units\nto Dec Ft ";
//		private const string BUTTON_UNIT_DECIN_NAME  = "Units\nto Dec In ";

//		private static bool _eventsRegistered = false;
//		private static bool _unitsConfigured = false;
//
//		private static bool _familyDocumentCreated = false;

		private static UIControlledApplication _uiCtrlApp;
		internal static UIApplication UiApp;
		internal static UIDocument Uidoc;
		internal static Application App;
		internal static Document Doc;

		private static int idx1 = 0;

		public static int idx
		{
			get
			{
				idx1 = (idx1 + 1) % 4;
				return idx1;
			}

		}

		private static string[] names = new [] {"alpha", "beta", "delta", "omega"};

		public string testMessage1 => names[idx];
		public static string testMessageS  => names[idx];

		public static AppRibbon appR { get; set; }

		public static string getTestMsg1s()
		{
			return appR.testMessage1;
		}

		public string getTestMsg1()
		{
			return appR.testMessage1;
		}


		public Result OnStartup(UIControlledApplication app)
		{
			appR = this;

			_uiCtrlApp = app;

			// clearConsole();

			try
			{
				// _uiCtrlApp.Idling += OnIdling;

				// create the ribbon tab first - this is the top level
				// UI item, below this will be the panel that is "on" the tab
				// and below this will be a button that is "on" the panel
				// give the tab a name
				string m_tabName = TAB_NAME;

				// first create the tab
				try
				{
					// try to create the ribbon panel
					app.CreateRibbonTab(m_tabName);
				}
				catch (Exception)
				{
					// might already exist - do nothing
				}

				// got the tab now


/*
				// create the ribbon panel if needed
				// give the panel a name
				string m_panelName = UNITS_PANEL_NAME;

				RibbonPanel ribbonPanel = null;

				// check to see if the panel alrady exists
				// get the Panel within the tab by name
				List<RibbonPanel> m_RP = new List<RibbonPanel>();

				m_RP = app.GetRibbonPanels(m_tabName);

				foreach (RibbonPanel xRP in m_RP)
				{
					if (xRP.Name.ToUpper().Equals(m_panelName.ToUpper()))
					{
						ribbonPanel = xRP;
						break;
					}
				}

				// add the panel if it does not exist
				if (ribbonPanel == null)
				{
					// create the ribbon panel on the tab given the tab's name
					// FYI - leave off the ribbon panel's name to put onto the "add-in" tab
					ribbonPanel = app.CreateRibbonPanel(m_tabName, m_panelName);
				}


				if (AddButtons(ribbonPanel) == Result.Failed)
				{
					return Result.Failed;
				}
*/

				RibbonPanel ribbonPanel =
					AddRibbonPanel(UNITS_PANEL_NAME, m_tabName);

				if (AddButtons1(ribbonPanel) == Result.Failed)
				{
					return Result.Failed;
				}

				ribbonPanel =
					AddRibbonPanel(EX_STORE_PANEL_NAME, m_tabName);

				if (AddButtons2(ribbonPanel) == Result.Failed)
				{
					return Result.Failed;
				}
				
				if (!AddSplitButtonRead(ribbonPanel))
				{
					return Result.Failed;
				}
				//
				//
				// if (!AddSplitButtonDelete(ribbonPanel))
				// {
				// 	return Result.Failed;
				// }
				//
				// if (!AddSplitButtonModify(ribbonPanel))
				// {
				// 	return Result.Failed;
				// }

				SmUsrInit();

				// ExStoreMgr.XsMgr.Initialize();

				return Result.Succeeded;
			}
			catch
			{
				return Result.Failed;
			}
		} // end OnStartup


		// required
		public Result OnShutdown(UIControlledApplication a)
		{
			try
			{
				// begin code here
				return Result.Succeeded;
			}
			catch
			{
				return Result.Failed;
			}
		} // end OnShutdown


		private RibbonPanel AddRibbonPanel(string panelName, string m_tabName)
		{
			string m_panelName = panelName;

			RibbonPanel ribbonPanel = null;

			// check to see if the panel alrady exists
			// get the Panel within the tab by name
			List<RibbonPanel> m_RP = new List<RibbonPanel>();

			m_RP = _uiCtrlApp.GetRibbonPanels(m_tabName);

			foreach (RibbonPanel xRP in m_RP)
			{
				if (xRP.Name.ToUpper().Equals(m_panelName.ToUpper()))
				{
					ribbonPanel = xRP;
					break;
				}
			}

			// add the panel if it does not exist
			if (ribbonPanel == null)
			{
				// create the ribbon panel on the tab given the tab's name
				// FYI - leave off the ribbon panel's name to put onto the "add-in" tab
				ribbonPanel = _uiCtrlApp.CreateRibbonPanel(m_tabName, m_panelName);
			}

			// if (!AddSplitButtons(ribbonPanel))
			// {
			// 	return Result.Failed;
			// }

			return ribbonPanel;
		}


/*		private void OnIdling(object sender,
			IdlingEventArgs e
			)
		{
			UiApp             =  sender as UIApplication;
			Uidoc             =  UiApp.ActiveUIDocument;

			if (Uidoc == null) return;

			_uiCtrlApp.Idling -= OnIdling;

			App               =  UiApp.Application;
			Doc               =  Uidoc.Document;
		}
*/

		private bool AddSplitButtonRead(RibbonPanel ribbonPanel)
		{
			SplitButtonData sbData = new SplitButtonData("splitButton2", "Split");
			SplitButton sb = ribbonPanel.AddItem(sbData) as SplitButton;
		
			bool result;
		
			result = CreateSplitButton(sb, "ReadCellExStore",
				BUTTON_READ_CELL_EX_STORE, "Read Ex Storage Cell Data");
		
			result = CreateSplitButton(sb, "ReadAppExStore",
				BUTTON_READ_APP_EX_STORE, "Read Ex Storage App Data");
		
			result = CreateSplitButton(sb, "ReadRootExStore",
				BUTTON_READ_ROOT_EX_STORE, "Read Ex Storage Root Data");

			result = CreateSplitButton(sb, "ReadSchema",
				BUTTON_READ_SCHEMA, "Read Schema");

			result = CreateSplitButton(sb, "ReadAppRibbonInfo",
				BUTTON_READ_APPRIBBON_INFO, "Read App-Ribbon Info");
		
			return true;
		}


		// private bool AddSplitButtonDelete(RibbonPanel ribbonPanel)
		// {
		// 	SplitButtonData sbData = new SplitButtonData("splitButton1", "Split");
		// 	SplitButton sb = ribbonPanel.AddItem(sbData) as SplitButton;
		//
		// 	bool result;
		//
		// 	result = CreateSplitButton(sb, "DelRootExStore",
		// 		BTN_EX_STOR_DEL_ROOT, "Delete Ex Storage Root Entity");
		//
		// 	result = CreateSplitButton(sb, "DelAppExStore",
		// 		BTN_EX_STOR_DEL_APP, "Delete Ex Storage App Entity");
		//
		// 	// result = CreateSplitButton(sb, "DelSubExStor",
		// 	// 	BTN_EX_STOR_DEL_SUB, "Delete Ex Storage Sub-Entities");
		//
		// 	return true;
		// }


		// private bool AddSplitButtonModify(RibbonPanel ribbonPanel)
		// {
		// 	SplitButtonData sbData = new SplitButtonData("splitButton3", "Split");
		// 	SplitButton sb = ribbonPanel.AddItem(sbData) as SplitButton;
		//
		// 	bool result;
		//
		// 	result = CreateSplitButton(sb, "ModCellExData",
		// 		BTN_EX_STOR_MOD_CELLS, "Modify Cells Entity");
		//
		// 	result = CreateSplitButton(sb, "ModAppExData",
		// 		BTN_EX_STOR_MOD_APP, "Modify App Entity");
		//
		// 	// result = CreateSplitButton(sb, "DelSubExStor",
		// 	// 	BTN_EX_STOR_DEL_SUB, "Delete Ex Storage Sub-Entities");
		//
		// 	return true;
		// }

		private bool CreateSplitButton(SplitButton sb,
			string identifier, string title, string tootTip)
		{
			PushButtonData pbd;

			pbd = CreateButton(identifier, title,
				"information16.png", "information16.png",
				Assembly.GetExecutingAssembly().Location,
				"AOTools." + identifier, tootTip);

			if (pbd == null)
			{
				CreateButtonFail(identifier);
				return false;
			}

			sb.AddPushButton(pbd);

			return true;
		}

		private void CreateButtonFail(string whichButton)
		{
			// creating the pushbutton failed
			TaskDialog td = new TaskDialog("AO Tools - " + whichButton);
			td.MainIcon = TaskDialogIcon.TaskDialogIconWarning;
			td.MainContent = String.Format(Properties.Resources.ButtonCreateFail,
				whichButton);
			td.Show();
		}



		private Result AddButtons1(RibbonPanel ribbonPanel)
		{
			if (AddUnitStylesButton(ribbonPanel) != Result.Succeeded)
				return Result.Failed;

			if (AddUnitStyleDeleteButton(ribbonPanel) != Result.Succeeded)
				return Result.Failed;

			return Result.Succeeded;
		}


		private Result AddButtons2(RibbonPanel ribbonPanel)
		{
			if (TestButton0(ribbonPanel) != Result.Succeeded)
				return Result.Failed;
			
			if (TestButton1(ribbonPanel) != Result.Succeeded)
				return Result.Failed;
			
			if (TestButton2(ribbonPanel) != Result.Succeeded)
				return Result.Failed;
			
			if (TestButton3(ribbonPanel) != Result.Succeeded)
				return Result.Failed;


			// if (AddSelectButton(ribbonPanel) != Result.Succeeded)
			// 	return Result.Failed;
			//
			// if (MakeRootDataStoreButton(ribbonPanel) != Result.Succeeded)
			// 	return Result.Failed;
			//
			// if (MakeAppAndCellsStoreButton(ribbonPanel) != Result.Succeeded)
			// 	return Result.Failed;



			return Result.Succeeded;
		}

		private Result TestButton0(RibbonPanel ribbonPanel)
		{
			return makePushButton(ribbonPanel, BUTTON_TEST0, nameof(TestExStore0), 
				"Test Reset", 
				"Test Ex Storage");
		}
		
		private Result TestButton1(RibbonPanel ribbonPanel)
		{
			return makePushButton(ribbonPanel, BUTTON_TEST1, nameof(TestExStore1), 
				"Test 1", 
				"Test Ex Storage 1");
		}
		
		private Result TestButton2(RibbonPanel ribbonPanel)
		{
			return makePushButton(ribbonPanel, BUTTON_TEST2, nameof(TestExStore2), 
				"Test 2", 
				"Test Ex Storage 2");
		}
		
		private Result TestButton3(RibbonPanel ribbonPanel)
		{
			return makePushButton(ribbonPanel, BUTTON_TEST3, nameof(TestExStore3), 
				"Test 3", 
				"Test Ex Storage 3");
		}


		private string exCmdName;

		private Result makePushButton(RibbonPanel ribbonPanel, string btnText,
			string cmdName, string toolTip, string desc)
		{
			if (!AddPushButton(ribbonPanel, cmdName, btnText,
				"information16.png",
				"information32.png",
				Assembly.GetExecutingAssembly().Location, $"AOTools.{cmdName}",
				toolTip))

			{
				TaskDialog td = new TaskDialog($"AO Tools - {desc}");
				td.MainIcon = TaskDialogIcon.TaskDialogIconWarning;
				td.MainInstruction = String.Format(Properties.Resources.ButtonCreateFail,
					$"AOTools {desc}");
				td.Show();

				return Result.Failed;
			}

			return Result.Succeeded;
		}


		private Result AddUnitStylesButton(RibbonPanel ribbonPanel)
		{
			return makePushButton(ribbonPanel, BUTTON_UNITSTYLES, nameof(UnitStylesCommand),
				"Create and Modify Unit Styles", 
				"Create and Modify Unit Styles");
		}

		private Result AddUnitStyleDeleteButton(RibbonPanel ribbonPanel)
		{
			return makePushButton(ribbonPanel, BUTTON_UNITSTYLEDELETE, nameof(UnitStylesDelete),
				"Unit Styles Delete", 
				"Unit Styles Delete");
		}









		// private Result ReadAppDataStoreButton(RibbonPanel ribbonPanel)
		// {
		// 	
		// 	return makePushButton(ribbonPanel, BUTTON_READ_APP_EX_STORE, nameof(ReadAppExStore), 
		// 		"Read App Extension Storage", 
		// 		"Read App Ex Storage");
		// }
		//
		// private Result ReadRootDataStoreButton(RibbonPanel ribbonPanel)
		// {
		// 	return makePushButton(ribbonPanel, BUTTON_READ_ROOT_EX_STORE, nameof(ReadRootExStore),
		// 		"Read Root Extension Storage", 
		// 		"Read Root Ex Storage");
		// }
		//
		// private Result MakeRootDataStoreButton(RibbonPanel ribbonPanel)
		// {
		// 	return makePushButton(ribbonPanel, BUTTON_ROOT_EX_STORE, nameof(MakeRootExStore),
		// 		"Write Root Extension Storage", 
		// 		"Write Root Ex Storage");
		// }
		//
		// private Result MakeAppAndCellsStoreButton(RibbonPanel ribbonPanel)
		// {
		// 	return makePushButton(ribbonPanel, BUTTON_DATA_STORAGE, nameof(MakeAppAndDataStore),
		// 		"Add Data Storage", 
		// 		"Make Data Storage");
		// }
		//
		// private Result AddSelectButton(RibbonPanel ribbonPanel)
		// {
		// 	return makePushButton(ribbonPanel, BUTTON_SELECT, nameof(SelectElement),
		// 		"Select Element", 
		// 		"Select Element");
		// }

		// method to add a pushbutton to the ribbon
		private bool AddPushButton(RibbonPanel Panel, string ButtonName,
			string ButtonText, string Image16, string Image32,
			string dllPath, string dllClass, string ToolTip)
		{
			try
			{
				PushButtonData m_pdData = CreateButton(ButtonName, ButtonText, Image16, Image32,
					dllPath, dllClass, ToolTip);

				// add it to the panel
				PushButton m_pb = Panel.AddItem(m_pdData) as PushButton;

				return true;
			}
			catch
			{
				return false;
			}
		}

		private PushButtonData CreateButton(string ButtonName,
			string ButtonText,
			string Image16,
			string Image32,
			string dllPath,
			string dllClass,
			string ToolTip)
		{
			PushButtonData pdData;

			try
			{
				pdData = new PushButtonData(ButtonName,
					ButtonText, dllPath, dllClass);
				// if we have a path for a small image, try to load the image
				if (Image16.Length != 0)
				{
					try
					{
						// load the image
						pdData.Image = CsUtilitiesMedia.GetBitmapImage(Image16, NAMESPACE_PREFIX);
					}
					catch
					{
						// could not locate the image
					}
				}

				// if have a path for a large image, try to load the image
				if (Image32.Length != 0)
				{
					try
					{
						// load the image
						pdData.LargeImage = CsUtilitiesMedia.GetBitmapImage(Image32, NAMESPACE_PREFIX);
					}
					catch
					{
						// could not locate the image
					}
				}

				// set the tooltip
				pdData.ToolTip = ToolTip;
			}
			catch
			{
				return null;
			}

			return pdData;
		}




//		private Result AddUnitStyleFtInButton(RibbonPanel ribbonPanel)
//		{
//			// create a button for the 'copy sheet' command
//			if (!AddPushButton(ribbonPanel, "UnitStyleFtIn", BUTTON_UNIT_FTIN_NAME,
//				"information16.png",
//				"information32.png",
//				Assembly.GetExecutingAssembly().Location, "AOTools.UnitStyles.UnitStyleFeetInchCmd",
//					"Set Project Units to Standard Feet & Inches"))
//			{
//				// creating the pushbutton failed
//				TaskDialog td = new TaskDialog("AO Tools - Unit Style FtIn");
//				td.MainIcon = TaskDialogIcon.TaskDialogIconWarning;
//				td.MainContent = String.Format(Properties.Resources.ButtonCreateFail,
//					Properties.Resources.R_ButtonStyleFtInName);
//				td.Show();
//
//				return Result.Failed;
//			}
//			return Result.Succeeded;
//		}
//		
//		private Result AddUnitStyleFracInButton(RibbonPanel ribbonPanel)
//		{
//			// create a button for the 'copy sheet' command
//			if (!AddPushButton(ribbonPanel, "UnitStyleFracIn", BUTTON_UNIT_FRACIN_NAME,
//				"information16.png",
//				"information32.png",
//				Assembly.GetExecutingAssembly().Location, "AOTools.UnitStyles.UnitStyleFracInchCmd",
//					"Set Project Units to Standard Fractional Inches"))
//			{
//				// creating the pushbutton failed
//				TaskDialog td = new TaskDialog("AO Tools - Unit Style Frac In");
//				td.MainIcon = TaskDialogIcon.TaskDialogIconWarning;
//				td.MainContent = String.Format(Properties.Resources.ButtonCreateFail,
//					Properties.Resources.R_ButtonStyleFracInName);
//				td.Show();
//
//				return Result.Failed;
//			}
//			return Result.Succeeded;
//		}
//		
//		private Result AddUnitStyleDecInchButton(RibbonPanel ribbonPanel)
//		{
//			// create a button for the 'copy sheet' command
//			if (!AddPushButton(ribbonPanel, "UnitStyleDecInch", BUTTON_UNIT_DECIN_NAME,
//				"information16.png",
//				"information32.png",
//				Assembly.GetExecutingAssembly().Location, "AOTools.UnitStyles.UnitStyleDecInchCmd",
//					"Set Project Units to Standard Decimal Inches"))
//			{
//				// creating the pushbutton failed
//				TaskDialog td = new TaskDialog("AO Tools - Unit Style Dec In");
//				td.MainIcon = TaskDialogIcon.TaskDialogIconWarning;
//				td.MainContent = String.Format(Properties.Resources.ButtonCreateFail,
//					Properties.Resources.R_ButtonStyleDecInchName);
//				td.Show();
//
//				return Result.Failed;
//			}
//			return Result.Succeeded;
//		}
//		
//		private Result AddUnitStyleDecFeetButton(RibbonPanel ribbonPanel)
//		{
//			// create a button for the 'copy sheet' command
//			if (!AddPushButton(ribbonPanel, "UnitStyleDecFeet", BUTTON_UNIT_DECFT_NAME,
//				"information16.png",
//				"information32.png",
//				Assembly.GetExecutingAssembly().Location, "AOTools.UnitStyles.UnitStyleDecFeetCmd",
//					"Set Project Units to Standard Decimal Feet - Unit Style Dec Ft"))
//			{
//				// creating the pushbutton failed
//				TaskDialog td = new TaskDialog("AO Tools");
//				td.MainIcon = TaskDialogIcon.TaskDialogIconWarning;
//				td.MainContent = String.Format(Properties.Resources.ButtonCreateFail,
//					Properties.Resources.R_ButtonStyleDecFeetName);
//				td.Show();
//
//				return Result.Failed;
//			}
//			return Result.Succeeded;
//		}


		//		private void AppClosing(object sender, ApplicationClosingEventArgs args)
		//		{
		//			//			UiApp.ViewActivated -= ViewActivated;
		//
		//			App.DocumentOpened -= DocOpenEvent;
		//
		//			UiApp.ApplicationClosing -= AppClosing;
		//		}
		//
		//
		//		private void OnIdling(object sender, IdlingEventArgs e)
		//		{
		//			_uiCtrlApp.Idling -= OnIdling;
		//			UiApp = sender as UIApplication;
		//			Uidoc = UiApp.ActiveUIDocument;
		//			App = UiApp.Application;
		//
		//			RegisterDocEvents();
		//		}
		//
		//		private bool RegisterDocEvents()
		//		{
		////			logMsgDbLn2("registering events", "0");
		//
		//			if (_eventsRegistered) return true;
		//			_eventsRegistered = true;
		//
		//			try
		//			{
		////				UiApp.ViewActivated += ViewActivated;
		////				UiApp.ViewActivated += ViewActivated;
		//				App.DocumentOpened += DocOpenEvent;
		////				App.DocumentCreated += new EventHandler<DocumentCreatedEventArgs>(DocCreatedEvent);
		//
		//				App.DocumentCreated += DocumentCreated;
		//
		//				UiApp.ApplicationClosing += AppClosing;
		//			}
		//			catch (Exception)
		//			{
		//				return false;
		//			}
		//
		//			return true;
		//		}
		//
		//		private void DocumentCreated(object sender,
		//			DocumentCreatedEventArgs args)
		//		{
		//			if (args.Document.IsFamilyDocument)
		//			{
		//				_familyDocumentCreated = true;
		//			}
		//
		//			SetUnits();
		//		}
		//
		//		private void DocOpenEvent(object sender, DocumentOpenedEventArgs args)
		//		{
		//			Doc = args.Document;
		//
		//			SetUnits();
		//
		//		}
		//
		//		private void ConfigNewDocUnits(Document doc)
		//		{
		//			
		//		}
		//
		//		private bool SetUnits()
		//		{
		//			Units u = Doc.GetUnits();
		//
		//			double accuracy = (1.0 / 12.0) / 16.0;
		//
		//			try
		//			{
		//				Units units = new Units(UnitSystem.Imperial);
		//				FormatOptions fmtOps = 
		//					new FormatOptions(DisplayUnitType.DUT_FEET_FRACTIONAL_INCHES, 
		//						UnitSymbolType.UST_NONE, accuracy);
		//				
		//				fmtOps.SuppressSpaces = true;
		//				fmtOps.SuppressLeadingZeros = true;
		//				fmtOps.UseDigitGrouping = true;
		//
		//				units.SetFormatOptions(UnitType.UT_Length, fmtOps);
		//
		//				using (Transaction t = new Transaction(Doc, "Update Units"))
		//				{
		//					t.Start();
		//					Doc.SetUnits(units);
		//					t.Commit();
		//				}
		//			}
		//			catch (Exception e)
		//			{
		//				logMsgDbLn2("set units: Exception|", e.Message);
		//				throw;
		//			}
		//			_unitsConfigured = false;
		//
		//			return _unitsConfigured;
		//		}
	}
}