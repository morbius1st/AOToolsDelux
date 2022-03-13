#region using

using System.Reflection;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Windows.Controls;
using Autodesk.Revit.DB;
using DeluxMeasure.UnitsUtil;
using DeluxMeasure.Windows.Support;
using UtilityLibrary;

#endregion

// projname: DeluxMeasure
// itemname: App
// username: jeffs
// created:  2/12/2022 8:46:31 AM

namespace DeluxMeasure
{
	internal class AppRibbon : IExternalApplication
	{

		public Result OnShutdown(UIControlledApplication a)
		{
			return Result.Succeeded;
		}
		// application: launch with revit - setup interface elements
		// display information

		internal const string APP_NAME = "Delux Measure";

		private const string NAMESPACE_PREFIX_RESOURCES = "DeluxMeasure.Resources";

		private const string PANEL_NAME = "DeluxMeasure";
		private const string TAB_NAME = "AO Tools";

		private const string BUTTON_NAME = "UnitStyles";
		private const string BUTTON_TEXT = "UnitStyles";

		private const string COMMAND_CLASS_NAME = "Command";

		private static string AddInPath = typeof(AppRibbon).Assembly.Location;
		private const string CLASSPATH = "DeluxMeasure.Windows.Support.";

		private const string SMALLICON = "information16.png";
		private const string LARGEICON = "information32.png";


		public const string BTN_NAME_DIVIDER = "?";
		private string BTN_NAME = $"UnitStyle{BTN_NAME_DIVIDER}";

		private UnitStyles us;
		private UnitsManager uMgr;

		public static SplitButton sb { get; set; }

		internal UIApplication uiApp;

		//		internal UIControlledApplication uiCtrlApp;

		//		public static PulldownButton pb;
		//		public static SplitButton sb;


		public Result OnStartup(UIControlledApplication app)
		{

			try
			{
				uMgr = UnitsManager.Instance;
				// us = UnitStyles.Instance;
				//				uiCtrlApp = app;

				UnitsSettings usx = new UnitsSettings();

				uMgr.StyleList = usx.GetStyles();

				app.ControlledApplication.ApplicationInitialized += OnAppInitalized;

				// create the ribbon tab first - this is the top level
				// ui item.  below this will be the panel that is "on" the tab
				// and below this will be a pull down or split button that is "on" the panel;

				// give the tab a name;
				string tabName = TAB_NAME;
				// give the panel a name
				string panelName = PANEL_NAME;

				// first try to create the tab
				try
				{
					app.CreateRibbonTab(tabName);
				}
				catch (Exception)
				{
					// might already exist - do nothing
				}

				// tab created or exists

				// create the ribbon panel if needed
				RibbonPanel ribbonPanel = null;

				// check to see if the panel already exists
				// get the Panel within the tab name
				List<RibbonPanel> rp = new List<RibbonPanel>();

				rp = app.GetRibbonPanels(tabName);

				foreach (RibbonPanel rpx in rp)
				{
					if (rpx.Name.ToUpper().Equals(panelName.ToUpper()))
					{
						ribbonPanel = rpx;
						break;
					}
				}

				// if panel not found
				// add the panel if it does not exist
				if (ribbonPanel == null)
				{
					// create the ribbon panel on the tab given the tab's name
					// FYI - leave off the ribbon panel's name to put onto the "add-in" tab
					ribbonPanel = app.CreateRibbonPanel(tabName, panelName);
				}

				ribbonPanel.AddItem(
					CreateButton(
						BUTTON_NAME, BUTTON_TEXT,
						SMALLICON, LARGEICON,
						AddInPath,
						CLASSPATH + COMMAND_CLASS_NAME,
						"Set Model Units to a Style"));

				if (!addSplitButtons(ribbonPanel))
				{
					return Result.Failed;
				}

				if (addDropPanel(ribbonPanel))
				{
					return Result.Failed;
				}
			}
			catch (Exception e)
			{
				Debug.WriteLine("exception " + e.Message);
				return Result.Failed;
			}



			return Result.Succeeded;
		}

		private void OnAppInitalized(object sender, ApplicationInitializedEventArgs e)
		{
			Application app = sender as Application;

			uiApp = new UIApplication(app);
		}

		private void CreateButtonFail(string whichButton)
		{
			// creating the pushbutton failed
			TaskDialog td = new TaskDialog(APP_NAME + " - " + whichButton);
			td.MainIcon = TaskDialogIcon.TaskDialogIconWarning;
			td.MainContent = String.Format($"Failed to create the {0} button",
				whichButton);
			td.Show();
		}

		private PushButtonData CreateButton(string ButtonName,
			string ButtonText,
			string Image16,
			string Image32,
			string dllPath,
			string dllClass,
			string ToolTip
			)
		{
			PushButtonData pdData;

			try
			{
				pdData = new PushButtonData(
					ButtonName,
					ButtonText,
					dllPath,
					dllClass);
				// if we have a path for a small image, try to load the image
				if (Image16.Length != 0)
				{
					try
					{
						// load the image
						pdData.Image = CsUtilitiesMedia.GetBitmapImage(Image16, NAMESPACE_PREFIX_RESOURCES);
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
						pdData.LargeImage = CsUtilitiesMedia.GetBitmapImage(Image32, NAMESPACE_PREFIX_RESOURCES);
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

		private bool addSplitButtons(RibbonPanel ribbonPanel)
		{
			SplitButtonData sbData = new SplitButtonData("splitButton1", "Split");
			sb = ribbonPanel.AddItem(sbData) as SplitButton;

			PushButtonData pbd;
			UnitsData.UnitInfo ui;

			for (int i = 0; 
				i < (uMgr.StyleList.Count > UnitStyleCmd.MAX_STYLE_CMDS ? UnitStyleCmd.MAX_STYLE_CMDS : uMgr.StyleList.Count)
				; i++)
			{
				
				ui = uMgr.UnitData.GetInfo(uMgr.StyleList[i].Id);

				string cmdName = $"UnitStyleCmd{i}";
				string btnName = $"{BTN_NAME}{i:D3}";
				string btnTitle = $"Units to\n" + ui.Title;
				string btnToolTip = $"Set Model Units to " + ui.Desc;
				string classPath = $"{CLASSPATH}{cmdName}";
				string btnFailText = $"{ui.Title} Button";


				pbd = CreateButton(
					btnName, btnTitle, ui.IconFile, ui.IconFile, AddInPath, classPath, btnToolTip);

				if (pbd == null)
				{
					CreateButtonFail(btnFailText);
					return false;
				}

				sb.AddPushButton(pbd);
			}

			sb.IsSynchronizedWithCurrentItem = true;

			return true;

		}

		private bool addDropPanel(RibbonPanel ribbonPanel)
		{
			ribbonPanel.AddSlideOut();
			
			PushButtonData pbd;

			pbd = CreateButton(
				"UnitStyleUtil",
				"Unit Style Mgr",
				SMALLICON,
				LARGEICON,
				AddInPath,
				$"{CLASSPATH}UnitStyleMgr",
				"Opens the Unit Style Manager");

			ribbonPanel.AddItem(pbd);

			return true;
		}

	}

}