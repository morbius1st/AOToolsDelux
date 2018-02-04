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

#endregion

namespace AOTools
{
	class AppRibbon : IExternalApplication
	{
		internal const string APP_NAME = "AOTools";

		private const string NAMESPACE_PREFIX = "AOTools.Resources";

		private const string BUTTON_NAME1 = "Unit\nStyles";
		private const string BUTTON_NAME2 = "Delete\nStyles";
		private const string PANEL_NAME = "AO Tools";
		private const string TAB_NAME = "AO Tools";

		private static bool _eventsRegistered = false;
		private static bool _unitsConfigured = false;

		private static UIControlledApplication _uiCtrlApp;
		internal static UIApplication UiApp;
		internal static UIDocument Uidoc;
		internal static Application App;
		internal static Document Doc;


		public Result OnStartup(UIControlledApplication app)
		{
			_uiCtrlApp = app;

			clearConsole();

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
				string m_panelName = PANEL_NAME;

				RibbonPanel m_RibbonPanel = null;

				// check to see if the panel alrady exists
				// get the Panel within the tab by name
				List<RibbonPanel> m_RP = new List<RibbonPanel>();

				m_RP = app.GetRibbonPanels(m_tabName);

				foreach (RibbonPanel xRP in m_RP)
				{
					if (xRP.Name.ToUpper().Equals(m_panelName.ToUpper()))
					{
						m_RibbonPanel = xRP;
						break;
					}
				}

				// if
				// add the panel if it does not exist
				if (m_RibbonPanel == null)
				{
					// create the ribbon panel on the tab given the tab's name
					// FYI - leave off the ribbon panel's name to put onto the "add-in" tab
					m_RibbonPanel = app.CreateRibbonPanel(m_tabName, m_panelName);
				}

				// create a button for the 'copy sheet' command
				if (!AddPushButton(m_RibbonPanel, "UnitStyles", BUTTON_NAME1,
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

				// create a button for the 'copy sheet' command
				if (!AddPushButton(m_RibbonPanel, "UnitStylesDelete", BUTTON_NAME2,
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

				//UnitStylesDelete

				return Result.Succeeded;
			}
			catch
			{
				return Result.Failed;
			}
		} // end OnStartup



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

		// method to add a pushbutton to the ribbon
		private bool AddPushButton(RibbonPanel Panel, string ButtonName,
			string ButtonText, string Image16, string Image32,
			string dllPath, string dllClass, string ToolTip)
		{
			try
			{
				PushButtonData m_pdData = new PushButtonData(ButtonName,
					ButtonText, dllPath, dllClass);
				// if we have a path for a small image, try to load the image
				if (Image16.Length != 0)
				{
					try
					{
						// load the image
						m_pdData.Image = CsUtilities.GetBitmapImage(Image16, NAMESPACE_PREFIX);
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
						m_pdData.LargeImage = CsUtilities.GetBitmapImage(Image32, NAMESPACE_PREFIX);
					}
					catch
					{
						// could not locate the image
					}
				}
				// set the tooltip
				m_pdData.ToolTip = ToolTip;

				// add it to the panel
				PushButton m_pb = Panel.AddItem(m_pdData) as PushButton;

				return true;
			}
			catch
			{
				return false;
			}
		}

		private void AppClosing(object sender, ApplicationClosingEventArgs args)
		{
			//			UiApp.ViewActivated -= ViewActivated;

			App.DocumentOpened += DocOpenEvent;
			App.DocumentCreated += DocCreateEvent;

			UiApp.ApplicationClosing -= AppClosing;
		}


		private void OnIdling(object sender, IdlingEventArgs e)
		{
			_uiCtrlApp.Idling -= OnIdling;
			UiApp = sender as UIApplication;
			Uidoc = UiApp.ActiveUIDocument;
			App = UiApp.Application;

			RegisterDocEvents();
		}

		private bool RegisterDocEvents()
		{
			logMsgDbLn2("registering events", "0");

			if (_eventsRegistered) return true;
			_eventsRegistered = true;

			try
			{
//				UiApp.ViewActivated += ViewActivated;
//				UiApp.ViewActivated += ViewActivated;
				App.DocumentOpened += new EventHandler<DocumentOpenedEventArgs>(DocOpenEvent);
				App.DocumentCreated += new EventHandler<DocumentCreatedEventArgs>(DocCreateEvent);

				UiApp.ApplicationClosing += AppClosing;
			}
			catch (Exception)
			{
				return false;
			}

			return true;
		}

		private void DocOpenEvent(object sender, DocumentOpenedEventArgs args)
		{
			Doc = args.Document;

			logMsgDbLn2("doc open event", "0");

			ReadUnits();

			logMsgDbLn2("doc open event", "1");

//			AddInfoToProjectBasepoint();

//			logMsgDbLn2("read message", ReadRevitSettings2());


		}

		private void DocCreateEvent(object sender, DocumentCreatedEventArgs args)
		{
			Doc = args.Document;

			ReadUnits();
		}

		private bool ReadUnits()
		{
			logMsgDbLn2("read units", "0");

			Units u = Doc.GetUnits();

//			FormatOptions f = u.GetFormatOptions(UnitType.UT_Length);
//
//			logMsgDbLn2("read units| accuracy", f.Accuracy.ToString());

			double accuracy = (1.0 / 12.0) / 16.0;

			try
			{
				Units units = new Units(UnitSystem.Imperial);
//				logMsgDbLn2("read units", "1");
				FormatOptions fmtOps = 
					new FormatOptions(DisplayUnitType.DUT_FEET_FRACTIONAL_INCHES, 
						UnitSymbolType.UST_NONE, accuracy);
//				logMsgDbLn2("read units", "2");

				fmtOps.SuppressSpaces = true;
				fmtOps.SuppressLeadingZeros = false;
				fmtOps.UseDigitGrouping = true;

//				logMsgDbLn2("read units", "3");

				units.SetFormatOptions(UnitType.UT_Length, fmtOps);

//				logMsgDbLn2("read units", "5");

				using (Transaction t = new Transaction(Doc, "Update Units"))
				{
					t.Start();
					Doc.SetUnits(units);
					t.Commit();
				}


			}
			catch (Exception e)
			{
				logMsgDbLn2("read units: Exception|", e.Message);
				throw;
			}
			_unitsConfigured = false;

			logMsgDbLn2("read units", "9");

			return _unitsConfigured;
		}

	}
}
