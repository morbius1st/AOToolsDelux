#region Namespaces
using System;
using System.Collections.Generic;
using System.Reflection;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
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
		internal const string APP_NAME = "AOTools";

		private const string NAMESPACE_PREFIX = "AOTools.Resources";

		private const string TAB_NAME = "AO Tools";
		private const string AO_TOOLS_PANEL_NAME = "AO Tools";
		private const string UNITS_PANEL_NAME = "Project Units";

		private const string BUTTON_UNITSTYLES = "Unit\nStyles";
		private const string BUTTON_UNITSTYLEDELETE = "Delete\nStyles";

		private const string BUTTON_UNIT_FTIN_NAME   = "Units\nto Feet-In";
		private const string BUTTON_UNIT_FRACIN_NAME = "Units\nto Frac In";
		private const string BUTTON_UNIT_DECFT_NAME  = "Units\nto Dec Ft ";
		private const string BUTTON_UNIT_DECIN_NAME  = "Units\nto Dec In ";

//		private static bool _eventsRegistered = false;
//		private static bool _unitsConfigured = false;
//
//		private static bool _familyDocumentCreated = false;

		private static UIControlledApplication _uiCtrlApp;
		internal static UIApplication UiApp;
		internal static UIDocument Uidoc;
		internal static Application App;
		internal static Document Doc;


		public Result OnStartup(UIControlledApplication app)
		{
			_uiCtrlApp = app;

//			clearConsole();

			try
			{
				_uiCtrlApp.Idling += OnIdling;

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

				if (!AddSplitButtons(ribbonPanel))
				{
					return Result.Failed;
				}

				SmUsrInit();

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


		private bool AddSplitButtons(RibbonPanel ribbonPanel)
		{ 
			SplitButtonData sbData = new SplitButtonData("splitButton1", "Split");
			SplitButton sb = ribbonPanel.AddItem(sbData) as SplitButton;

			PushButtonData pbd;

			pbd = CreateButton("UnitStyleFtIn", BUTTON_UNIT_FTIN_NAME,
				"Delux Measure Ft-In 16.png",
				"Delux Measure Ft-In 32.png",
				Assembly.GetExecutingAssembly().Location, "AOTools.UnitStyles.UnitStyleFeetInchCmd",
				"Set Project Units to Standard Feet & Inches");

			if (pbd == null)
			{
				CreateButtonFail(Properties.Resources.R_ButtonStyleFtInName);
				return false;
			}

			sb.AddPushButton(pbd);

			pbd = CreateButton("UnitStyleFracIn", BUTTON_UNIT_FRACIN_NAME,
				"Delux Measure Frac-In 16.png",
				"Delux Measure Frac-In 32.png",
				Assembly.GetExecutingAssembly().Location, "AOTools.UnitStyles.UnitStyleFracInchCmd",
					"Set Project Units to Standard Fractional Inches");

			if (pbd == null)
			{
				CreateButtonFail(Properties.Resources.R_ButtonStyleFracInName);
				return false;
			}

			sb.AddPushButton(pbd);

			pbd = CreateButton("UnitStyleDecInch", BUTTON_UNIT_DECIN_NAME,
				"Delux Measure Dec-In 16.png",
				"Delux Measure Dec-In 32.png",
				Assembly.GetExecutingAssembly().Location, "AOTools.UnitStyles.UnitStyleDecInchCmd",
					"Set Project Units to Standard Decimal Inches");

			if (pbd == null)
			{
				CreateButtonFail(Properties.Resources.R_ButtonStyleDecInchName);
				return false;
			}

			sb.AddPushButton(pbd);

			pbd = CreateButton("UnitStyleDecFeet", BUTTON_UNIT_DECFT_NAME,
				"Delux Measure Dec-Ft 16.png",
				"Delux Measure Dec-Ft 32.png",
				Assembly.GetExecutingAssembly().Location, "AOTools.UnitStyles.UnitStyleDecFeetCmd",
					"Set Project Units to Standard Decimal Feet");

			if (pbd == null)
			{
				CreateButtonFail(Properties.Resources.R_ButtonStyleDecFeetName);
				return false;
			}

			sb.AddPushButton(pbd);

			return true;
		}

		private void CreateButtonFail(string whichButton)
		{
			// creating the pushbutton failed
			TaskDialog td = new TaskDialog("AO Tools");
			td.MainIcon = TaskDialogIcon.TaskDialogIconWarning;
			td.MainContent = String.Format(Properties.Resources.ButtonCreateFail,
				whichButton);
			td.Show();
		}

		private Result AddButtons(RibbonPanel ribbonPanel)
		{
//			if (AddUnitStylesButton(ribbonPanel) != Result.Succeeded) 
//				return Result.Failed;
//
//			if (AddUnitStyleDeleteButton(ribbonPanel) != Result.Succeeded) 
//				return Result.Failed;

			if (AddUnitStyleFtInButton(ribbonPanel) != Result.Succeeded) 
				return Result.Failed;

			if (AddUnitStyleFracInButton(ribbonPanel) != Result.Succeeded) 
				return Result.Failed;

			if (AddUnitStyleDecInchButton(ribbonPanel) != Result.Succeeded) 
				return Result.Failed;

			if (AddUnitStyleDecFeetButton(ribbonPanel) != Result.Succeeded) 
				return Result.Failed;

			return Result.Succeeded;
		}

		private Result AddUnitStyleFtInButton(RibbonPanel ribbonPanel)
		{
			// create a button for the 'copy sheet' command
			if (!AddPushButton(ribbonPanel, "UnitStyleFtIn", BUTTON_UNIT_FTIN_NAME,
				"information16.png",
				"information32.png",
				Assembly.GetExecutingAssembly().Location, "AOTools.UnitStyles.UnitStyleFeetInchCmd",
					"Set Project Units to Standard Feet & Inches"))
			{
				// creating the pushbutton failed
				TaskDialog td = new TaskDialog("AO Tools");
				td.MainIcon = TaskDialogIcon.TaskDialogIconWarning;
				td.MainContent = String.Format(Properties.Resources.ButtonCreateFail,
					Properties.Resources.R_ButtonStyleFtInName);
				td.Show();

				return Result.Failed;
			}
			return Result.Succeeded;
		}
		
		private Result AddUnitStyleFracInButton(RibbonPanel ribbonPanel)
		{
			// create a button for the 'copy sheet' command
			if (!AddPushButton(ribbonPanel, "UnitStyleFracIn", BUTTON_UNIT_FRACIN_NAME,
				"information16.png",
				"information32.png",
				Assembly.GetExecutingAssembly().Location, "AOTools.UnitStyles.UnitStyleFracInchCmd",
					"Set Project Units to Standard Fractional Inches"))
			{
				// creating the pushbutton failed
				TaskDialog td = new TaskDialog("AO Tools");
				td.MainIcon = TaskDialogIcon.TaskDialogIconWarning;
				td.MainContent = String.Format(Properties.Resources.ButtonCreateFail,
					Properties.Resources.R_ButtonStyleFracInName);
				td.Show();

				return Result.Failed;
			}
			return Result.Succeeded;
		}
		
		private Result AddUnitStyleDecInchButton(RibbonPanel ribbonPanel)
		{
			// create a button for the 'copy sheet' command
			if (!AddPushButton(ribbonPanel, "UnitStyleDecInch", BUTTON_UNIT_DECIN_NAME,
				"information16.png",
				"information32.png",
				Assembly.GetExecutingAssembly().Location, "AOTools.UnitStyles.UnitStyleDecInchCmd",
					"Set Project Units to Standard Decimal Inches"))
			{
				// creating the pushbutton failed
				TaskDialog td = new TaskDialog("AO Tools");
				td.MainIcon = TaskDialogIcon.TaskDialogIconWarning;
				td.MainContent = String.Format(Properties.Resources.ButtonCreateFail,
					Properties.Resources.R_ButtonStyleDecInchName);
				td.Show();

				return Result.Failed;
			}
			return Result.Succeeded;
		}
		
		private Result AddUnitStyleDecFeetButton(RibbonPanel ribbonPanel)
		{
			// create a button for the 'copy sheet' command
			if (!AddPushButton(ribbonPanel, "UnitStyleDecFeet", BUTTON_UNIT_DECFT_NAME,
				"information16.png",
				"information32.png",
				Assembly.GetExecutingAssembly().Location, "AOTools.UnitStyles.UnitStyleDecFeetCmd",
					"Set Project Units to Standard Decimal Feet"))
			{
				// creating the pushbutton failed
				TaskDialog td = new TaskDialog("AO Tools");
				td.MainIcon = TaskDialogIcon.TaskDialogIconWarning;
				td.MainContent = String.Format(Properties.Resources.ButtonCreateFail,
					Properties.Resources.R_ButtonStyleDecFeetName);
				td.Show();

				return Result.Failed;
			}
			return Result.Succeeded;
		}


		private Result AddUnitStylesButton(RibbonPanel ribbonPanel)
		{
			// create a button for the 'copy sheet' command
			if (!AddPushButton(ribbonPanel, "UnitStyles", BUTTON_UNITSTYLES,
				"information16.png",
				"information32.png",
				Assembly.GetExecutingAssembly().Location, "AOTools.UnitStylesCommand",
					"Create and Modify Unit Styles"))

			{
				// creating the pushbutton failed
				TaskDialog td = new TaskDialog("AO Tools");
				td.MainIcon = TaskDialogIcon.TaskDialogIconWarning;
				td.MainContent = String.Format(Properties.Resources.ButtonCreateFail,
					Properties.Resources.UnitStyleButtonText);
				td.Show();

				return Result.Failed;
			}
			return Result.Succeeded;
		}

		private Result AddUnitStyleDeleteButton(RibbonPanel ribbonPanel)
		{
			// create a button for the 'copy sheet' command
			if (!AddPushButton(ribbonPanel, "UnitStylesDelete", BUTTON_UNITSTYLEDELETE,
				"information16.png",
				"information32.png",
				Assembly.GetExecutingAssembly().Location, "AOTools.UnitStylesDelete",
					"Create and Modify Unit Styles"))

			{
				// creating the pushbutton failed
				TaskDialog td = new TaskDialog("AO Tools");
				td.MainIcon = TaskDialogIcon.TaskDialogIconWarning;
				td.MainContent = "failed to create the delete unit styles button";
				td.Show();

				return Result.Failed;
			}
			return Result.Succeeded;
		}

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

		private void OnIdling(object sender,
			IdlingEventArgs e)
		{
			_uiCtrlApp.Idling -= OnIdling;
			UiApp             =  sender as UIApplication;
			Uidoc             =  UiApp.ActiveUIDocument;
			App               =  UiApp.Application;
			Doc               =  Uidoc.Document;
		}



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
