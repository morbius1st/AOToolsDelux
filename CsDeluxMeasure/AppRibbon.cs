#region using

using System.Reflection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Windows;
using System.Windows.Controls;

using CsDeluxMeasure.RevitSupport;
using CsDeluxMeasure.UnitsUtil;
using CsDeluxMeasure.Windows;
using CsDeluxMeasure.Windows.Support;

using UtilityLibrary;

using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using CsDeluxMeasure.RevitSupport.ExtEvents;
using Tests01.RevitSupport;
using Application = Autodesk.Revit.ApplicationServices.Application;

#endregion

// projname: CsDeluxMeasure
// itemname: App
// username: jeffs
// created:  2/12/2022 8:46:31 AM

namespace CsDeluxMeasure
{
	internal class AppRibbon : IExternalApplication
	{
		public Result OnShutdown(UIControlledApplication a)
		{
			if (Mw != null && Mw.IsVisible) Mw.Close();

			return Result.Succeeded;
		}

		internal const string APP_NAME = "Delux Measure";
		internal const int MAX_RIBBON_NAME_WIDTH = 20;
		internal const int MAX_RIBBON_TOOLTIP_WIDTH = 36;

		internal const string NAMESPACE_PREFIX_RESOURCES = "CsDeluxMeasure.Resources";

		private const string PANEL_NAME = "DeluxMeasure";
		private const string TAB_NAME = "AO Tools";

		private const string BUTTON_NAME = "UnitStyles";
		private const string BUTTON_TEXT = "UnitStyles";

		private const string COMMAND_CLASS_NAME = "Command";

		private static string AddInPath = typeof(AppRibbon).Assembly.Location;
		private const string CLASSPATH_REVITSUPPORT = "CsDeluxMeasure.RevitSupport.";
		private const string CLASSPATH_SUPPORT = "CsDeluxMeasure.Windows.Support.";

		private const string SMALLICON = "information16.png";
		private const string LARGEICON = "information32.png";

		private const string SMALLICON_AP = "Gear32.png";
		private const string LARGEICON_AP = "Gear32.png";

		
		private const string SMALLICON_DM = "Tape Measure32.png";
		private const string LARGEICON_DM = "Tape Measure32.png";

		public const string ICON_FOLDER = "/resources";

		private const string MAIN_WIN_HELP_FILE = "Delux Measure Intro Help.htm";

		public const string BTN_NAME_DIVIDER = "?";
		internal static string BTN_NAME = $"UnitStyle{BTN_NAME_DIVIDER}";


		// internal UIApplication uiApp;

		internal static string AddInLocation { get; set; }

		internal string AddInResourcesLocation => $"{AddInLocation}\\Resources";

		internal string AddinMainWinHelpFile => $"{AddInResourcesLocation}\\{MAIN_WIN_HELP_FILE}";
		internal string AddinUnitStylesHelpFile => $"{AddInResourcesLocation}\\{MAIN_WIN_HELP_FILE}";
		internal string AddinUnitStyleOrderHelpFile => $"{AddInResourcesLocation}\\{MAIN_WIN_HELP_FILE}";

		private bool unitsDialogDisplayed = false;
		

		/* static */

		// will be the revit window
		public static Window W;

		public static SplitButton sb { get; set; }

		public static UIApplication UiApp;
		public static UIDocument UiDoc ;
		public static Application App;
		public static Document Doc;

		
		/* objects  */

		public static AppRibbon Me;

		public static MainWindow Mw;
		public static MiniMain Mm;
		public static DxMeasure Dx;

		// private UnitStyles us;
		private UnitsManager uMgr;

		public Result OnStartup(UIControlledApplication app)
		{
			// ControlledApplication ctrldApp = app.ControlledApplication;
			// AddInLocation = ctrldApp.CurrentUserAddinsLocation;

			R.UcApp = app;

			app.DialogBoxShowing += App_DialogBoxShowing;
			app.Idling += App_Idling;

			Me = this;

			try
			{
				uMgr = UnitsManager.Instance;

				uMgr.Config();
				uMgr.ConfigCurrentInList();

				// app.ControlledApplication.ApplicationInitialized += OnAppInitalized;

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


				if (!addSplitButtons(ribbonPanel))
				{
					return Result.Failed;
				}

				if (!addDropPanel(ribbonPanel))
				{
					return Result.Failed;
				}
			}
			catch (Exception e)
			{
				// Debug.WriteLine("exception " + e.Message);
				return Result.Failed;
			}


			return Result.Succeeded;
		}
		
		private void App_Idling(object sender, Autodesk.Revit.UI.Events.IdlingEventArgs e)
		{
			if (!unitsDialogDisplayed) { return; }

			unitsDialogDisplayed = false;

			if (Mw==null) { return; }

			Mw.UnitsDialogBoxDisplayed = true;

			Debug.WriteLine("Now Idling");
		}

		private void App_DialogBoxShowing(object sender, Autodesk.Revit.UI.Events.DialogBoxShowingEventArgs e)
		{
			string dialogToWatch = "Dialog_Revit_Units";
			// Debug.WriteLine($"DialogShowing| {e.DialogId}");

			if (e.DialogId.Equals(dialogToWatch))
			{
				unitsDialogDisplayed=true;
			}
		}

		// private void OnAppInitalized(object sender, ApplicationInitializedEventArgs e)
		// {
		// 	Application app = sender as Application;
		//
		// 	// uiApp = new UIApplication(app);
		// }

		private void CreateButtonFail(string whichButton)
		{
			// creating the pushbutton failed
			TaskDialog td = new TaskDialog(APP_NAME + " - " + whichButton);
			td.MainIcon = TaskDialogIcon.TaskDialogIconWarning;
			td.MainContent = String.Format($"Failed to create the {0} button",
				whichButton);
			td.Show();
		}

		private PushButtonData CreateButtonData(string ButtonName,
			string ButtonText, string imageFolder, string Image16, string Image32,
			string dllPath, string dllClass, string ToolTip)
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
				if ((Image16?.Length ?? 0) != 0)
				{
					try
					{
						// load the image
						// pdData.Image = CsUtilitiesMedia.GetBitmapImage(Image16, NAMESPACE_PREFIX_RESOURCES);
						pdData.Image = CsUtilitiesMedia.GetBitmapImageResource($"{imageFolder}/{Image16}");
					}
					catch
					{
						// could not locate the image
					}
				}

				// if have a path for a large image, try to load the image
				if ((Image32?.Length ?? 0) != 0)
				{
					try
					{
						// load the image
						// pdData.LargeImage = CsUtilitiesMedia.GetBitmapImage(Image32, NAMESPACE_PREFIX_RESOURCES);
						pdData.LargeImage = CsUtilitiesMedia.GetBitmapImageResource($"{imageFolder}/{Image32}");
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

			ContextualHelp ch = new ContextualHelp(ContextualHelpType.Url, AddinMainWinHelpFile);
			sb.SetContextualHelp(ch);

			uMgr.UlMgr.Rio.SB = sb;

			PushButtonData pbd;


			int max = uMgr.UsrStyleList.Count > UnitStyleCmd.MAX_STYLE_CMDS ? UnitStyleCmd.MAX_STYLE_CMDS : uMgr.UsrStyleList.Count;
			int i = 0;

			// foreach (UnitsDataR udr in uMgr.InListViewRibbon)
			foreach (UnitsDataR udr in uMgr.UlMgr.Current.InListViewRibbon)
			{
				// List<string> lines = CsStringUtil.StringDivide(us.Name, new [] { ' ' }, MAX_RIBBON_NAME_WIDTH, 0);
				//
				// string result = CsStringUtil.MakeMultiLineString(lines, MAX_RIBBON_NAME_WIDTH);
				//
				// string cmdName = $"UnitStyleCmd{i}";
				// string btnName = $"{BTN_NAME}{i:D2}";
				// string btnTitle = result;
				// string btnToolTip = $"Set Project Units to " + us.Description;
				// string classPath = $"{CLASSPATH}{cmdName}";
				// string btnFailText = $"{us.Name} Button";

				// pbd = CreateButton(
				// 	btnName, btnTitle, us.IconId, us.IconId, AddInPath, classPath, btnToolTip);

				pbd = getPushButtonData(udr.Ustyle, i);

				if (pbd == null)
				{
					CreateButtonFail($"{udr.Ustyle.Name} Button");
					return false;
				}

				PushButton pb = sb.AddPushButton(pbd);
				pb.SetContextualHelp(ch);

				uMgr.UlMgr.Rio.AddPbToList(pb);

				i++;
				if (i == max) break;
			}

			for (int j = i; j < max; j++)
			{
				// string cmdName = $"UnitStyleCmd{j}";
				// string btnName = $"{BTN_NAME}{j:D2}";
				// string btnTitle = $"{BTN_NAME}{j:D2}";
				// string btnToolTip = $"Set Project Units to " + $"{BTN_NAME}{j:D2}";
				// string classPath = $"{CLASSPATH}{cmdName}";
				// string btnFailText = $"{BTN_NAME}{j:D2} Button";
				//
				// pbd = CreateButton(
				// 	btnName, btnTitle, null, null, AddInPath, classPath, btnToolTip);

				pbd = getPushButtonData(null, j);


				if (pbd == null)
				{
					CreateButtonFail($"{BTN_NAME}{j:D2} Button");
					return false;
				}

				PushButton pb = sb.AddPushButton(pbd);
				pb.Visible = false;
				pb.SetContextualHelp(ch);

				uMgr.UlMgr.Rio.AddPbToList(pb);
			}

			sb.IsSynchronizedWithCurrentItem = true;

			
			return true;
		}

		private PushButtonData getPushButtonData(UStyle us, int idx)
		{
			string name = uMgr.UlMgr.Rio.MakePbName(idx, BTN_NAME);

			string title;
			string icId;
			string tTip;

			if (us != null)
			{
				title = uMgr.UlMgr.Rio.MakePbTitle(us.Name);
				icId = us.IconId;
				tTip = uMgr.UlMgr.Rio.MakePbToolTip(us.Description);
			}
			else
			{
				title = uMgr.UlMgr.Rio.MakePbHiddenTitle(idx, name);
				icId = null;
				tTip=uMgr.UlMgr.Rio.MakePbToolTip(name);
			}

			PushButtonData pbd;

			string btnName = name;
			string cmdName = $"UnitStyleCmd{idx}";
			string btnTitle = title;
			string btnToolTip = tTip;
			string classPath = $"{CLASSPATH_REVITSUPPORT}{cmdName}";

			pbd = CreateButtonData(
				btnName, btnTitle, ICON_FOLDER, icId, icId, AddInPath, classPath, btnToolTip);

			return pbd;
		}

		private bool addDropPanel(RibbonPanel ribbonPanel)
		{
			ribbonPanel.AddSlideOut();

			PushButtonData pbd;

			ContextualHelp ch = new ContextualHelp(ContextualHelpType.Url, AddinUnitStylesHelpFile);

			pbd = CreateButtonData(
				"UnitStyleUtil",
				"Unit Style Mgr", 
				ICON_FOLDER,
				SMALLICON_AP,
				LARGEICON_AP,
				AddInPath,
				$"{CLASSPATH_REVITSUPPORT}UnitStyleMgr",
				"Opens the Unit Style Manager");

			RibbonItem ri = ribbonPanel.AddItem(pbd);

			ri.SetContextualHelp(ch);


			ch = new ContextualHelp(ContextualHelpType.Url, AddinUnitStylesHelpFile);

			pbd = CreateButtonData(
				"DeluxMeasure",
				"Delux Measure", 
				ICON_FOLDER,
				SMALLICON_DM,
				LARGEICON_DM,
				AddInPath,
				$"{CLASSPATH_REVITSUPPORT}MainWindowCommand",
				"Begin Measuring Between Points");

			// pbd.Image = CsUtilitiesMedia.GetBitmapImageResource($"{ICON_FOLDER}/{SMALLICON_DM}");
			// pbd.LargeImage = CsUtilitiesMedia.GetBitmapImageResource($"{ICON_FOLDER}/{LARGEICON_DM}");

			ri = ribbonPanel.AddItem(pbd);

			ri.SetContextualHelp(ch);

			return true;
		}

	}
}