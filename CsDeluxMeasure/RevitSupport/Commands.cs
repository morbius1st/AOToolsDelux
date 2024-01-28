#region using

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using CsDeluxMeasure.Windows;
using CsDeluxMeasure.Windows.Support;
using SettingsManager;
using Application = Autodesk.Revit.ApplicationServices.Application;
using Visibility = System.Windows.Visibility;
using Autodesk.Revit.ApplicationServices;
using Autodesk.RevitAddIns;
using CsDeluxMeasure.RevitSupport.ExtEvents;
using RevitLibrary;
using Tests01.RevitSupport;

#endregion

// projname: CsDeluxMeasure
// itemname: Command
// username: jeffs
// created:  2/12/2022 8:46:31 AM

namespace CsDeluxMeasure.RevitSupport
{
	[Transaction(TransactionMode.Manual)]
	public class MainWindowCommand : IExternalCommand, INotifyPropertyChanged
	{
	#region fields

		// private const string ROOT_TRANSACTION_NAME = "Transaction Name";

		public static UIApplication UiApp;
		public static UIDocument UiDoc ;
		public static Application App;
		public static Document Doc;

		public static MainWindow Mw;
		public static MiniMain Mm;
		public static DxMeasure Dx;

		public static MainWindowCommand me;

		public static Window W;

		// public static Prime prime;
		// private string commandString = "b9890444-dd63-400c-b8c8-303b44730830";
		// public static RevitCommandId commandId;
		//
		// private static bool miniVisibility;
		// private static bool mainVisibility;
		// private static bool dialogNotVisible;

		static MainWindowCommand() { }


	#region entry point: Execute

		public Result Execute(
			ExternalCommandData commandData, ref string message, ElementSet elements)
		{
			UiApp = commandData.Application;
			UiDoc = UiApp.ActiveUIDocument;
			App = UiApp.Application;
			Doc = UiDoc.Document;



			// commandId  = RevitCommandId.LookupCommandId("b9890444-dd63-400c-b8c8-303b44730830");
			// commandId  = RevitCommandId.LookupCommandId( uiapp.ActiveAddInId.GetGUID().ToString());
			// AddInId ai = new AddInId(new Guid("b9890444-dd63-400c-b8c8-303b44730830"));

			if (W==null)
			{
				// handle to the revit window
				W = RvtLibrary.WindowHandle(UiApp.MainWindowHandle);
			}

			if (Mw==null)
			{
				Mw = new MainWindow(UiDoc);
				

				// mw.Visibility = Visibility.Visible;
				Mw.Owner = W;
			}

			AppRibbon.Mw = Mw;

			if (Mm==null)
			{

				Mm = new MiniMain(UiDoc);
				// mm.Visibility = Visibility.Visible;
				Mm.Owner = W;
			}
			// Command_HideMain(UserSettings.Data.HideMain);
			// Command_ShowMiniWin(UserSettings.Data.ShowMiniWin);

			if (Dx==null)
			{
				Dx = new DxMeasure(UiDoc);
			}

			config(commandData.Application);

			start();

			return Result.Succeeded;
		}

	#endregion

		private void config(UIApplication uiApp)
		{
			if (Mw != null) return;

			R.UiApp = uiApp;

			ExtEvtHandler handler = new ExtEvtHandler();
			ExternalEvent eEvent = ExternalEvent.Create(handler);

			AppRibbon.Mw = new MainWindow(R.UiDoc, handler, eEvent);
			AppRibbon.Mm = new MiniMain(R.UiDoc, handler, eEvent);
			AppRibbon.Dx = new DxMeasure(R.UiDoc);

		}





		// private static void measure()
		// {
		// 	bool repeat = true;
		//
		// 	do
		// 	{
		// 		Debug.WriteLine("top of loop");
		// 		try
		// 		{
		// 			repeat = start();
		// 		}
		// 		catch (Exception e)
		// 		{
		// 			Debug.WriteLine(e);
		// 			repeat = false;
		// 		}
		//
		// 		Debug.WriteLine("bottom of loop\n");
		// 	}
		// 	while (repeat);
		// }


		/*
		 * need to revise to put the point request portion in an external event which
		 * will be async
		 * so, must be broken into parts
		 * 1. the "command" must be placed someplace and static so that it can be
		 *		accessed by the async routines - revit suggests at AppRibbon
		 * 2. command start needs to make the ext evt request but no other processing.
		 * 3. ext event will process the function, get the points, and package the points
		 * 4. ext event will then call main window's select points completion routine to
		 *		use the information and display the information
		 * ** that is, start() must be divided into (2) parts
		 */

		private static bool start()
		{
			bool result = true;

			// Debug.WriteLine("begin start");

			// M.W.WriteLine1("\nStart-begin");

			// Debug.WriteLine("B revit being activated");
			W.Activate();

			using (TransactionGroup tg = new TransactionGroup(Doc, "Delux Measure"))
			{
				tg.Start();
				{
					// get the points
					result = Dx.Measure();

				}
				tg.RollBack();
			}

			if (!result) return result;

			UserSettings.Admin.Read();

			// M.W.WriteLine1($"command| points read| area| {dx.Points.Area}");

			Mw.Points = Dx.Points;
			Mw.init();
			Mm.Points = Dx.Points;

			Dlg_HideMain(UserSettings.Data.HideMain);
			Dlg_ShowMini(UserSettings.Data.ShowMiniWin);

			// Debug.WriteLine($"start| hidemain| {UserSettings.Data.HideMain}");
			// Debug.WriteLine($"start| showmini| {UserSettings.Data.ShowMiniWin}");

			// Debug.WriteLine("end start");

			// M.W.WriteLine1("Start-end\n");

			// Debug.WriteLine("C revit being activated");
			W.Activate();

			return result;
		}

		public static MainWindowCommand GetMe
		{
			get
			{
				if (me == null)
				{
					me = new MainWindowCommand();
				}

				return me;
			}
		}

		
		public static void SelectPoints()
		{
			// uidoc.Application.PostCommand(MainWindowCommand.commandId);

			start();

		}

		public static void UpdatePoints()
		{
			Mw.Points = Dx.Points;
			Mw.init();
			Mm.Points = Dx.Points;
		}


		public static void Dlg_ShowMini(bool value)
		{
			Mm.ShowMini(value);

			if (!value && UserSettings.Data.HideMain)
			{
				Mw.UpdateCkBxShowMiniWin(true);
				Mw.ShowMe();
			}
		}

		public static void Dlg_HideMain(bool value)
		{
			if (value && !UserSettings.Data.ShowMiniWin)
			{
				Mw.UpdateCkBxHideMain(false);
				Mm.ShowMe();
			}

			Mm.MainHidden = value;

			
			Mw.HideMain(value);
		}

		public static void Mini_TempShowDialog()
		{
			Mw.ShowMe();
		}

		// value: true - visible / false - not visible
		public static void Dlg_WinVisibilityChanged(bool value)
		{
			Mm.MainHidden = !value;
		}

		public static void UpdateHideMain(bool value)
		{
			// Debug.WriteLine($"command| UpdateHideMain| {value}");

			UserSettings.Data.HideMain = value;
			UserSettings.Admin.Write();

		}

		public static void UpdateShowMini(bool value)
		{
			// Debug.WriteLine($"command| UpdateShowMini| {value}");

			UserSettings.Data.ShowMiniWin = value;
			UserSettings.Admin.Write();

		}




		// public static void Dlg_ShowMiniWin(bool value)
		// {
		// 	mm.ShowMini = value;
		// }
		//
		// public static void Dlg_HideMain(bool? value)
		// {
		// 	mm.HideMain = value.HasValue ? value.Value : true;
		//
		// 	CoordShowWindows();
		// }
		//
		// public static void Mini_ShowMiniWin(bool value)
		// {
		// 	mw.ShowMini = value;
		// }
		//
		// public static void Mini_HideMain(bool? value)
		// {
		// 	if (!value.HasValue)
		// 	{
		// 		HideMainTemp();
		// 		return;
		// 	}
		//
		// 	mw.HideMain = value.Value;
		//
		// 	CoordShowWindows();
		// }
		//
		//
		// public static void Command_ShowMiniWin(bool value)
		// {
		// 	Dlg_ShowMiniWin(value);
		// 	Mini_ShowMiniWin(value);
		// }
		//
		// public static void Command_HideMain(bool value)
		// {
		// 	Dlg_HideMain(value);
		// 	Mini_HideMain(value);
		// }
		//
		//
		// public static void HideMainTemp()
		// {
		// 	mw.ShowMe();
		// }

		// public static bool DialogNotVisible
		// {
		// 	get => dialogNotVisible;
		// 	set
		// 	{ 
		// 		Debug.WriteLine($"command| dialognotvisible changed| {!value}");
		//
		// 		dialogNotVisible = !value;
		// 		OnPropertyChangedStatic();
		//
		// 	}
		// }

		// public static void DialogNotVisible(bool value)
		// {
		// 	Debug.WriteLine($"command| dialognotvisible changed| {!value}");
		//
		// 	mm.DialogNotVisible = !value;
		// }
		//
		//
		// private static void CoordShowWindows()
		// {
		// 	if (!mw.HideMain) Command_ShowMiniWin(true);
		// }




		// public static bool MainVisibility
		// {
		// 	get => mainVisibility;
		// 	set
		// 	{
		// 		mainVisibility = value;
		// 		OnPropertyChangedStatic();
		//
		// 		if (value)
		// 		{
		// 			mw.ShowMe();
		// 		}
		// 		else
		// 		{
		// 			mw.HideMe();
		// 		}
		// 	}
		// }
		//
		// public static bool MiniVisibility
		// {
		// 	get => miniVisibility;
		// 	set
		// 	{
		// 		miniVisibility = value;
		// 		OnPropertyChangedStatic();
		//
		// 		if (value)
		// 		{
		// 			mm.ShowMe();
		// 		}
		// 		else
		// 		{
		// 			mm.HideMe();
		// 		}
		// 	}
		// }
		//
		//
		// public static bool ShowMini
		// {
		// 	get => showMini;
		// 	set
		// 	{
		// 		if (showMini == value) return;
		//
		// 		if (!value) HideMain = true;
		//
		// 		showMini = value;
		// 		OnPropertyChangedStatic();
		//
		// 		UserSettings.Data.ShowMiniWin = value;
		// 		UserSettings.Admin.Write();
		// 	}
		// }
		//
		//
		// public static bool? HideMain
		// {
		// 	get => hideMain;
		// 	set
		// 	{
		// 		if (!value.HasValue)
		// 		{
		// 			// is null
		// 			ShowDlgTemp = true;
		// 			value = false;
		// 		}
		//
		// 		if (!value.Value) ShowMini = true;
		//
		// 		hideMain = value;
		// 		OnPropertyChangedStatic();
		//
		// 		UserSettings.Data.HideMain = value.Value;
		// 		UserSettings.Admin.Write();
		// 	}
		// }
		//
		// public static bool ShowDlgTemp
		// {
		// 	get => showDlgTemp;
		// 	set
		// 	{
		// 		if (hideMain.HasValue && hideMain.Value) return;
		//
		// 		showDlgTemp = value;
		// 		OnPropertyChangedStatic();
		// 	}
		// }
		//
		//
		// public static bool ShowMini2
		// {
		// 	get => showMini;
		// 	set
		// 	{
		// 		if (showMini == value) return;
		//
		// 		showMini = value;
		//
		// 		// false - hide
		// 		if (!value)
		// 		{
		// 			HideMain = true;
		// 			if (mm.Visibility == Visibility.Hidden) return;
		// 			mm.Visibility = Visibility.Hidden;
		// 			UserSettings.Data.ShowMiniWin = false;
		// 		}
		// 		else
		// 		{
		// 			if (mm.Visibility == Visibility.Visible) return;
		// 			mm.Visibility = Visibility.Visible;
		// 			UserSettings.Data.ShowMiniWin = true;
		// 		}
		//
		// 		UserSettings.Admin.Write();
		//
		// 		OnPropertyChangedStatic();
		// 	}
		// }
		//
		//
		// // true == always show main
		// // false == always don't show main
		// // null -> already true - ignore
		// //		-> is false - set to null
		// public static bool? HideMain2
		// {
		// 	get => hideMain;
		// 	set
		// 	{
		// 		
		// 		if (!value.HasValue)
		// 		{
		// 			// is null
		// 			// if false
		// 			if (!hideMain.Value)
		// 			{
		// 				mw.OneTimeView = true;
		// 				if (mw.Visibility == Visibility.Visible) return;
		// 				mw.Visibility = Visibility.Visible;
		// 				UserSettings.Data.HideMain = false;
		// 			}
		// 		}
		// 		// if true
		// 		else if	(value.Value)
		// 		{
		// 			hideMain = true;
		// 			if (mw.Visibility == Visibility.Visible) return;
		// 			mw.Visibility = Visibility.Visible;
		// 			UserSettings.Data.HideMain = false;
		// 		} 
		// 		// if false
		// 		else 
		// 		{
		// 			hideMain = value;
		// 			if (mw.Visibility == Visibility.Hidden) return;
		// 			mw.Visibility = Visibility.Hidden;
		// 			UserSettings.Data.HideMain=true;
		//
		// 			ShowMini = true;
		// 		} 
		//
		// 		UserSettings.Admin.Write();
		//
		// 		OnPropertyChangedStatic();
		// 		OnPropertyChangedStatic(nameof(DontHideMain));
		// 	}
		// }
		//
		// public static bool DontHideMain
		// {
		// 	set
		// 	{
		// 		hideMain = !value;
		// 	}
		// 	get
		// 	{
		// 		return hideMain.HasValue ? !hideMain.Value : false;
		// 	}
		// }

	#endregion


	#region public methods

	#endregion

	#region private methods

		/*
		private void start()
		{
			bool result;

			using (TransactionGroup tg = new TransactionGroup(doc, "Delux Measure"))
			{
				tg.Start();
				{
					do
					{
						// if (mm!= null) mm.Close();

						result = mw.DxMeasure();

						if (result)
						{
							mw = new MainWindow(uidoc);
							mw.SetPointsToZero();
							SetWinPos();
						}
					}
					while (result);

					// main.Show();
					// main.Visibility=Visibility.Visible;
				}
				tg.RollBack();
			}
		}
		*/


		private void SetWinPos()
		{
			W = RevitLibrary.RvtLibrary.WindowHandle(UiApp.MainWindowHandle);
		}


		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

		public static event PropertyChangedEventHandler PropertyChangedStatic;

		private static void OnPropertyChangedStatic([CallerMemberName] string memberName = "")
		{
			PropertyChangedStatic?.Invoke(GetMe, new PropertyChangedEventArgs(memberName));
		}

	#endregion
	}

	[Transaction(TransactionMode.Manual)]
	public class MainWindowCmdWin : IExternalCommand
	{
	#region fields

		private const string ROOT_TRANSACTION_NAME = "Transaction Name";

		public static UIApplication uiapp;
		public static UIDocument uidoc ;
		public static Application app;
		public static Document doc;

		public static MainWindow main;

	#endregion

	#region entry point: Execute

		public Result Execute(
			ExternalCommandData commandData, ref string message, ElementSet elements)
		{
			uiapp = commandData.Application;
			uidoc = uiapp.ActiveUIDocument;
			app = uiapp.Application;
			doc = uidoc.Document;

			if (main == null)
			{
				main = new MainWindow(uidoc);
				main.Visibility = Visibility.Hidden;
			}

			main.Name = "Main01";

			// SetWinPos(main);
			//
			// start();

			return Result.Succeeded;
		}

	#endregion

	#region public methods

	#endregion

	#region private methods

		/*
		private void start()
		{
			bool result = true;

			using (TransactionGroup tg = new TransactionGroup(doc, "Delux Measure"))
			{
				tg.Start();
				{
					// main.SetPointsToZero();
					main.Show();
					main.Visibility = Visibility.Visible;
				}
				tg.RollBack();
			}
		}

		private void SetWinPos(MainWindow main)
		{
			IntPtr h = uiapp.MainWindowHandle;

			Window w = RevitLibrary.RvtLibrary.WindowHandle(h);

			main.SetPosition(w);
		}
		*/

	#endregion
	}
}