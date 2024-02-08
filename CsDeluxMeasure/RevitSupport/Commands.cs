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
	public class MainWindowCommand : IExternalCommand  //, INotifyPropertyChanged
	{
	#region fields

		// private const string ROOT_TRANSACTION_NAME = "Transaction Name";

		// public static UIApplication UiApp;
		// public static UIDocument UiDoc ;
		// public static Application App;
		// public static Document Doc;
		//
		// public static MainWindow Mw;
		// public static MiniMain Mm;
		// public static DxMeasure Dx;
		//
		// public static MainWindowCommand me;
		//
		// public static Window W;

		// public static Prime prime;
		// private string commandString = "b9890444-dd63-400c-b8c8-303b44730830";
		// public static RevitCommandId commandId;
		//
		// private static bool miniVisibility;
		// private static bool mainVisibility;
		// private static bool dialogNotVisible;

		// static MainWindowCommand() { }


	#region entry point: Execute

		public Result Execute(
			ExternalCommandData commandData, ref string message, ElementSet elements)
		{
			if (R.UiApp==null) R.UiApp = commandData.Application;

			if (R.Mw == null) config(commandData.Application);


			start();

			// Debug.WriteLine("start done");

			R.ActivateRevit();

			return Result.Succeeded;
		}

	#endregion

		private void config(UIApplication uiApp)
		{

			R.EeHandler = new ExtEvtHandler();
			R.EeEvent = ExternalEvent.Create(R.EeHandler);

			R.Mw = new MainWindow();
			R.Mw.Owner = R.RvtWin;
			
			R.Mm = new MiniMain();
			R.Mm.Owner = R.RvtWin;

			R.Dx = new DxMeasure();
		}

		private bool start()
		{
			bool result = R.Dx.MeasurePoints();

			if (!result) return result;

			Dlg_OnlyUseMini(UserSettings.Data.OnlyUseMini);
			Dlg_ShowMini(UserSettings.Data.ShowMiniWin);

			return result;
		}

		private static bool start2()
		{
			bool result = true;

			// Debug.WriteLine("begin start");

			// M.W.WriteLine1("\nStart-begin");

			// Debug.WriteLine("B revit being activated");
			R.ActivateRevit();

			using (TransactionGroup tg = new TransactionGroup(R.Doc, "Delux Measure"))
			{
				tg.Start();
				{
					// get the points
					result = R.Dx.Measure();

				}
				tg.RollBack();
			}

			if (!result) return result;

			UserSettings.Admin.Read();

			// M.W.WriteLine1($"command| points read| area| {dx.Points.Area}");

			R.Mw.Points = R.Dx.Points;
			R.Mw.init();
			R.Mm.Points = R.Dx.Points;

			Dlg_OnlyUseMini(UserSettings.Data.OnlyUseMini);
			Dlg_ShowMini(UserSettings.Data.ShowMiniWin);

			// Debug.WriteLine($"start| hidemain| {UserSettings.Data.HideMain}");
			// Debug.WriteLine($"start| showmini| {UserSettings.Data.ShowMiniWin}");

			// Debug.WriteLine("end start");

			// M.W.WriteLine1("Start-end\n");

			// Debug.WriteLine("C revit being activated");
			R.ActivateRevit();

			return result;
		}

		public static void Dlg_ShowMini(bool value)
		{
			R.Mm.ShowMini(value);

			if (!value && UserSettings.Data.OnlyUseMini)
			{
				R.Mw.UpdateCkBxShowMiniWin(true);
				R.Mw.ShowMe();
			}
		}

		public static void Dlg_OnlyUseMini(bool value)
		{
			if (value && !UserSettings.Data.ShowMiniWin)
			{
				R.Mw.UpdateCkBxHideMain(false);
				R.Mm.ShowMe();
			}

			R.Mm.MainHidden = value;

			R.Mw.HideMain(value);
		}

		public static void Mini_TempShowDialog()
		{
			R.Mw.ShowMe();
		}

		// value: true - visible / false - not visible
		public static void Dlg_WinVisibilityChanged(bool value)
		{
			R.Mm.MainHidden = !value;
		}

		public static void UpdateOnlyUseMini(bool value)
		{
			// Debug.WriteLine($"command| UpdateHideMain| {value}");

			UserSettings.Data.OnlyUseMini = value;
			UserSettings.Admin.Write();

		}

		public static void UpdateShowMini(bool value)
		{
			// Debug.WriteLine($"command| UpdateShowMini| {value}");

			UserSettings.Data.ShowMiniWin = value;
			UserSettings.Admin.Write();

		}


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


		// private void SetWinPos()
		// {
		// 	W = RevitLibrary.RvtLibrary.WindowHandle(UiApp.MainWindowHandle);
		// }


		// public event PropertyChangedEventHandler PropertyChanged;
		//
		// private void OnPropertyChanged([CallerMemberName] string memberName = "")
		// {
		// 	PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		// }
		//
		// public static event PropertyChangedEventHandler PropertyChangedStatic;
		//
		// private static void OnPropertyChangedStatic([CallerMemberName] string memberName = "")
		// {
		// 	PropertyChangedStatic?.Invoke(GetMe, new PropertyChangedEventArgs(memberName));
		// }

	#endregion

	}

 	/*
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

 		
// 		private void start()
// 		{
// 			bool result = true;
//
// 			using (TransactionGroup tg = new TransactionGroup(doc, "Delux Measure"))
// 			{
// 				tg.Start();
// 				{
// 					// main.SetPointsToZero();
// 					main.Show();
// 					main.Visibility = Visibility.Visible;
// 				}
// 				tg.RollBack();
// 			}
// 		}
//
// 		private void SetWinPos(MainWindow main)
// 		{
// 			IntPtr h = uiapp.MainWindowHandle;
//
// 			Window w = RevitLibrary.RvtLibrary.WindowHandle(h);
//
// 			main.SetPosition(w);
// 		}
		

	#endregion
	}

	*/

}