#region using
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.UI;

using System;
using System.Diagnostics;
using System.Reflection;
using CSToolsDelux.Revit.Commands;
using UtilityLibrary;

#endregion

// projname: CSToolsDelux
// itemname: App
// username: jeffs
// created:  8/28/2021 8:49:24 AM

namespace CSToolsDelux
{
	class AppRibbon : IExternalApplication
	{
		public Result OnShutdown(UIControlledApplication a)
		{
			return Result.Succeeded;
		}
		// application: launch with revit - setup interface elements
		// display information

		private const string NAMESPACE_PREFIX = "CSToolsDelux.Resources";

		internal const string APP_NAME = "CSToolsDelux";
		private const string PANEL_NAME = "CS Tools Delux";
		private const string TAB_NAME = "AO Tools";

		private const string BUTTON_NAME_01 = "CSToolsDelux";
		private const string BUTTON_TEXT_01 = "Test 01";
		private const string BUTTON_TOOL_TIP_01 = "Run Test 01";
		private const string COMMAND_CLASS_NAME_01 = nameof(Test01);


		private static string AddInPath = typeof(AppRibbon).Assembly.Location;
		private const string CLASSPATH = "CSToolsDelux.Revit.Commands.";

		private const string SMALLICON = "information16.png";
		private const string LARGEICON = "information32.png";

		private static UIControlledApplication _uiCtrlApp;
		internal static UIApplication UiApp;
		internal static UIDocument Uidoc;
		internal static Application App;
		internal static Document Doc;

		internal static string docName;

		public Result OnStartup(UIControlledApplication app)
		{
			try
			{
				// uiCtrlApp = app;

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

/*
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
*/

				ribbonPanel = app.CreateRibbonPanel(tabName, panelName);

				ribbonPanel.AddItem(
					createButton(BUTTON_NAME_01, BUTTON_TEXT_01, COMMAND_CLASS_NAME_01,
						BUTTON_TOOL_TIP_01, SMALLICON, LARGEICON));


				//				// example 1
				//				//add a pull down button to the panel
				//				if (!AddPullDownButton(ribbonPanel))
				//				{
				//					// create the split button failed
				//					MessageBox.Show("Failed to Add the Pull Down Button!", "Information",
				//						MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
				//					return Result.Failed;
				//				}
				//
				//				// example 2
				//				//add a stacked pair of push buttons to the panel
				//				if (!AddStackedPushButtons(ribbonPanel))
				//				{
				//					// create the split button failed
				//					MessageBox.Show("Failed to Add the Stacked Push Buttons!", "Information",
				//						MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
				//					return Result.Failed;
				//				}
				//
				//				// example 3
				//				//add a stacked pair of push buttons and a text box to the panel
				//				if (!AddStackedPushButtonsAndTextBox(ribbonPanel))
				//				{
				//					// create the split button failed
				//					MessageBox.Show("Failed to Add the Stacked Push Buttons and TextBox!", "Information",
				//						MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
				//					return Result.Failed;
				//				}

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

			UiApp = new UIApplication(app);
		}

		private PushButtonData createButton(string ButtonName, string ButtonText,
			string className, string ToolTip, string smallIcon, string largeIcon)
		{
			PushButtonData pbd;

			string dllPath = Assembly.GetExecutingAssembly().Location;

			try
			{
				pbd = new PushButtonData(ButtonName, ButtonText,
					AddInPath, string.Concat(CLASSPATH, className));
				// {
				// 	Image = CsUtilitiesMedia.GetBitmapImage(smallIcon, NAMESPACE_PREFIX),
				// 	LargeImage = CsUtilitiesMedia.GetBitmapImage(largeIcon, NAMESPACE_PREFIX),
				// 	ToolTip = ToolTip
				// };

				pbd.Image = CsUtilitiesMedia.GetBitmapImage(smallIcon, NAMESPACE_PREFIX);
				pbd.LargeImage = CsUtilitiesMedia.GetBitmapImage(largeIcon, NAMESPACE_PREFIX);
				pbd.ToolTip = ToolTip;

			}
			catch (Exception e)
			{
				TaskDialog td = new TaskDialog($"CS Tools - Make Button");
				td.MainIcon = TaskDialogIcon.TaskDialogIconWarning;
				td.MainInstruction = "Failed to create the button";

				td.MainContent = $"class path| {CLASSPATH}\n";
				td.MainContent += $"class| {className}\n";
				td.MainContent += $"add in path| {AddInPath}\n";
				td.MainContent += $"dll path| {dllPath}\n";

				td.MainContent += "\nexception|\n"
					+ $"{e.Message}";

				if (e.InnerException != null)
				{
					td.MainContent += "\n\ninner exception|\n"
						+ $"{e.InnerException.Message}";
				}


				td.Show();


				return null;
			}

			return pbd;
		}



		/*        
				private bool AddSplitButton(RibbonPanel rp)
				{
					try
					{
						SplitButtonData sbData = new SplitButtonData("splitButton01", "function select");
						sbData.Image = RibbonUtil.GetBitmapImage(LARGEICON);
						sbData.LargeImage = RibbonUtil.GetBitmapImage(LARGEICON);

						SplitButton sb = rp.AddItem(sbData) as SplitButton;

						PushButtonData pbd;

						pbd = createButton("SplitBtn01", "Proper\nCascade", "OrganizeProperCascade",
							"Organize Revit Windows by Proper Cascade", SMALLICON, LARGEICON);
						sb.AddPushButton(pbd);

						pbd = createButton("SplitBtn02", "Window's\nCascade", "OrganizeWindowsCascade",
							"Organize Revit Windows by Windows Cascade", SMALLICON, LARGEICON);
						sb.AddPushButton(pbd);

						pbd = createButton("SplitBtn03", "Active on\nLeft Tiled", "OrganizeLeft",
							"Place the Active Window on the Left", SMALLICON, LARGEICON);
						sb.AddPushButton(pbd);

						pbd = createButton("SplitBtn04", "Active on\nTop Tiled", "OrganizeTop",
							"Place the Active Window on the Top", SMALLICON, LARGEICON);
						sb.AddPushButton(pbd);

						pbd = createButton("SplitBtn05", "Active on\nRight Tiled", "OrganizeRight",
							"Place the Active Window on the Right", SMALLICON, LARGEICON);
						sb.AddPushButton(pbd);

						pbd = createButton("SplitBtn06", "Active on\nBottom Tiled", "OrganizeBottom",
							"Place the Active Window on the Bottom", SMALLICON, LARGEICON);
						sb.AddPushButton(pbd);

						pbd = createButton("SplitBtn07", "Active to\nLeft Stacked", "OrganizeLeftOverlapped",
							"Place the Active Window on the Left\nand Stack Remaining Windows", SMALLICON, LARGEICON);
						sb.AddPushButton(pbd);
					}
					catch
					{
						return false;
					}

					return true;
				}
		*/

		/* 
						private bool AddStackedPullDownhButtons(RibbonPanel rp)
				{
					SplitButton sb0;
					SplitButton sb1;


					SplitButtonData sbData0 = new SplitButtonData("pullDownButton0", "function select");
					sbData0.Image = RibbonUtil.GetBitmapImage(SMALLICON);

					SplitButtonData sbData1 = new SplitButtonData("pullDownButton1", "auto activate");
					sbData1.Image = RibbonUtil.GetBitmapImage(SMALLICON);

					PushButtonData pbData0 = createButton("pushButton0", "Auto Update: On - Turn Off", "ToggAutoActivate", 
						"Revit Windows Settings", SMALLICON, LARGEICON);

					PushButtonData pbData1 = createButton("pushButton1", "Settings", "SettingsFormShow", 
						"Revit Windows Settings", SMALLICON, LARGEICON);

					IList<RibbonItem> ris = rp.AddStackedItems(sbData0, pbData0, pbData1);

					sb0 = ris[0] as SplitButton;
		//			sb1 = ris[1] as SplitButton;
					pb01 = ris[1] as PushButton;

					PushButtonData pbd;

					// pull down button 0
					pbd = createButton("button00", "Side Views Larger ", "IncreaseSideViewSize",
						"Make the Active View Larger", SMALLICON, LARGEICON);
					sb0.AddPushButton(pbd);

					pbd = createButton("button01", "Side Views Smaller", "DecreaseSideViewSize",
						"Make the Active View Smaller", SMALLICON, LARGEICON);
					sb0.AddPushButton(pbd);

		//			// pull down button 1
		//			pbd = createButton("button10", "Activate Auto On", "AutoActivateOn",
		//				"Turn on AutoActivate", SMALLICON, LARGEICON);
		//			sb1.AddPushButton(pbd);
		//
		//			pbd = createButton("button11", "Activate Auto Off", "AutoActivateOff",
		//				"Turn off AutoActivate", SMALLICON, LARGEICON);
		//			sb1.AddPushButton(pbd);
		//
					return true;
				}
		 */

		/*
				 * examples of how to add various buttons
				 *
				 */
		//				//add a split pull down button to the panel
		//				if (!AddStackedComboBoxes(ribbonPanel))
		//				{
		//					TaskDialog td = new TaskDialog("Revit Windows");
		//					td.TitleAutoPrefix = false;
		//					td.MainInstruction = "Failed to Add the Stacked ComboBoxes!";
		//					td.MainIcon = TaskDialogIcon.TaskDialogIconWarning;
		//					td.CommonButtons = TaskDialogCommonButtons.Ok;
		//
		//					td.Show();
		//
		//					// create the split button failed
		//					MessageBox.Show("Failed to Add the Stacked ComboBoxes!", "Information",
		//						MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
		//					return Result.Failed;
		//				}
		//
		//				// example
		//				// add a button to the panel
		//				ribbonPanel.AddItem(
		//					createButton("ModifyPoints1", "Modify\nPoints", "ModifyPoints",
		//						"Modify the points of a topography surface", SMALLICON, LARGEICON));
		//
		//				// example 1
		//				//add a split pull down button to the panel
		//				if (!AddPullDownButton(ribbonPanel))
		//				{
		//					// create the split button failed
		//					MessageBox.Show("Failed to Add the Pull Down Button!", "Information",
		//						MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
		//					return Result.Failed;
		//				}
		//
		//				// example 2
		//				//add a stacked pair of push buttons to the panel
		//				if (!AddStackedPushButtons(ribbonPanel))
		//				{
		//					// create the split button failed
		//					MessageBox.Show("Failed to Add the Stacked Push Buttons!", "Information",
		//						MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
		//					return Result.Failed;
		//				}
		//
		//				// example 3
		//				//add a stacked pair of push buttons and a text box to the panel
		//				if (!AddStackedPushButtonsAndTextBox(ribbonPanel))
		//				{
		//					// create the split button failed
		//					MessageBox.Show("Failed to Add the Stacked Push Buttons and TextBox!", "Information",
		//						MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
		//					return Result.Failed;
		//				}


		//
		//		// add a pair of combo boxes - first is the function selection and 
		//		// the second is basically a check box replacement since revit
		//		// does not have a check box and I don't feel like making one at the moment
		//		private bool AddStackedComboBoxes(RibbonPanel rp)
		//		{
		//			try
		//			{
		//				ComboBoxData cbxData1 = new ComboBoxData("functions");
		//				ComboBoxData cbxData2 = new ComboBoxData("autoactivate");
		//
		//				IList<RibbonItem> ris = rp.AddStackedItems(cbxData1, cbxData2);
		//
		//				ComboBox cbx0 = ris[0] as ComboBox;
		//				ComboBox cbx1 = ris[1] as ComboBox;
		//
		//				cbx0.ItemText = "combobox 0";
		//				cbx0.ToolTip = "select a function";
		//				cbx0.LongDescription = "select a window organize function";
		//
		//				cbx1.ItemText = "combobox 1";
		//				cbx1.ToolTip = "toggle auto activate";
		//				cbx1.LongDescription = " toggle auto activate on or off";
		//
		//				CreateFunctionsCbx(ref cbx0);
		//				CreateAutoActivateCbx(ref cbx1);
		//
		//				cbx0.DropDownClosed += Cbx0_DropDownClosed;
		//				cbx1.DropDownClosed += Cbx1_DropDownClosed;
		//
		//			}
		//			catch
		//			{
		//				return false;
		//			}
		//			return true;
		//		}
		//
		//		private void Cbx1_DropDownClosed(object sender, 
		//			Autodesk.Revit.UI.Events.ComboBoxDropDownClosedEventArgs e)
		//		{
		//			TaskDialog.Show("Auto Activate", "this is a test " + ((ComboBox) sender).Current.ItemText
		//				+ " name (" + ((ComboBox) sender).Current.Name + ")");
		//		}
		//
		//		private void Cbx0_DropDownClosed(object sender, 
		//			Autodesk.Revit.UI.Events.ComboBoxDropDownClosedEventArgs e)
		//		{
		//			TaskDialog.Show("Auto Activate", "this is a test " + ((ComboBox) sender).Current.ItemText
		//				+ " name (" + ((ComboBox) sender).Current.Name + ")");
		//		}
		//
		//
		//		private void CreateFunctionsCbx(ref ComboBox cbx)
		//		{
		//			cbx.AddItem(createCbxMemberData("A", "Proper Cascade", SMALLICON));
		//			cbx.AddItem(createCbxMemberData("B", "Window Cascade", SMALLICON));
		//			cbx.AddItem(createCbxMemberData("C", "Active at Right", SMALLICON));
		//			cbx.AddItem(createCbxMemberData("D", "Active at Top", SMALLICON));
		//			cbx.AddItem(createCbxMemberData("E", "Active at Left", SMALLICON));
		//			cbx.AddItem(createCbxMemberData("F", "Active at Bottom", SMALLICON));
		//		}
		//
		//		private void CreateAutoActivateCbx(ref ComboBox cbx)
		//		{
		//			cbx.AddItem(createCbxMemberData("cx0", "Auto Activate On", SMALLICON));
		//			cbx.AddItem(createCbxMemberData("cx1", "Auto Activate Off", SMALLICON));
		//		}
		//
		//		private ComboBoxMemberData createCbxMemberData(string internalName, 
		//			string visibleName, string smallIcon)
		//		{
		//			ComboBoxMemberData cbxd = new ComboBoxMemberData(internalName, visibleName);
		//
		//			cbxd.Image = RibbonUtil.GetBitmapImage(smallIcon);
		//
		//			return cbxd;
		//		}
		//
		//		
		//		private bool AddStackedPushButtonsAndTextBox(RibbonPanel rp)
		//		{
		//			TextBoxData tbd = new TextBoxData("TopoSurfaceName");
		//			PushButtonData[] pbd = new PushButtonData[1];
		//
		//			pbd[0] = createButton("RaiseLowerPoints", "Raise\nLower\nPoints", "RaiseLowerPoints", 
		//				"Raise or Lower points by a fixed amount", SMALLICON, LARGEICON);
		//
		//			IList<RibbonItem> ribbonItems = rp.AddStackedItems(tbd, pbd[0]);
		//
		//			TopoName = ribbonItems[0] as Autodesk.Revit.UI.TextBox;
		//			TopoName.Value = "";
		//			TopoName.ToolTip = "Current Topo Surface Name";
		//			TopoName.Width = 200.0;
		//			TopoName.Enabled = false;
		//
		//			return true;
		//		}
		//
		//		private void SetTextBoxValue(object sender, TextBoxEnterPressedEventArgs args)
		//		{
		//			Units units = new Units(UnitSystem.Imperial);
		//			double length = 0;
		//			bool result = UnitFormatUtils.TryParse(units, UnitType.UT_Length, ElevChange.Value.ToString(), out length);
		//
		//			if (result)
		//			{
		//				elevChangeValue = length;
		//
		//				FormatOptions fOpt = new FormatOptions(DisplayUnitType.DUT_DECIMAL_FEET, 0.001);
		//				fOpt.SuppressTrailingZeros = true;
		//
		//
		//				FormatValueOptions opt = new FormatValueOptions();
		//				opt.AppendUnitSymbol = true;
		//				opt.SetFormatOptions(fOpt);
		//				ElevChange.Value = UnitFormatUtils.Format(units, UnitType.UT_Length, length, false, true, opt);
		//			}
		//			else
		//			{
		//				ElevChange.Value = "invalid";
		////				TaskDialog.Show("Parse", "Worked!", TaskDialogCommonButtons.Ok);
		//				MessageBox.Show("Elevation Change Value", "Amount is not a real distance", MessageBoxButtons.OK, MessageBoxIcon.Error);
		//			}
		//		}
		//
		//
		//		private bool AddStackedPushButtons(RibbonPanel rp)
		//		{
		//			PushButtonData[] pbd = new PushButtonData[2];
		//
		//			pbd[0] = createButton("RaisePoints2", "Raise\nPoints", "RaisePoints", 
		//				"Raise points by a fixed amount", SMALLICON, LARGEICON);
		//
		//			pbd[1] = createButton("OffsetPoints2", "Offset\nPoints", "OffsetPoints", 
		//				"Move points by a fixed amount", SMALLICON, LARGEICON);
		//
		//			IList<RibbonItem> ribbonItems = rp.AddStackedItems(pbd[0], pbd[1]);
		//
		//			return true;
		//		}
		//
		//
		//		// add a set of pull down buttons (3)
		//		private bool AddPullDownButton(RibbonPanel ribbonPanel)
		//		{
		//			PulldownButton pb;
		//
		//			PulldownButtonData pdData = new PulldownButtonData("pullDownButton1", "Edit Points");
		//			pdData.Image = RibbonUtil.GetBitmapImage(SMALLICON);
		//
		//			pb = ribbonPanel.AddItem(pdData) as PulldownButton;
		//
		//			PushButtonData pbd;
		//
		//			pbd = createButton("RaisePoints1", "Raise Points", "RaisePoints", 
		//				"Raise points by a fixed amount", SMALLICON, LARGEICON);
		//			pb.AddPushButton(pbd);
		//
		//			pbd = createButton("OffsetPoints1", "Offset Points", "OffsetPoints", 
		//				"Move points by a fixed amount", SMALLICON, LARGEICON);
		//			pb.AddPushButton(pbd);
		//
		//			return true;
		//		}

	}
}
