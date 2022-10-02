#region using

using System;
using System.Windows;
using System.Windows.Interop;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using CsDeluxMeasure.Windows;
using Application = Autodesk.Revit.ApplicationServices.Application;
using Visibility = System.Windows.Visibility;

#endregion

// projname: CsDeluxMeasure
// itemname: Command
// username: jeffs
// created:  2/12/2022 8:46:31 AM

namespace CsDeluxMeasure.RevitSupport
{
	[Transaction(TransactionMode.Manual)]
	public class MainWindowCommand : IExternalCommand
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

			main = new MainWindow(uidoc);
			// main.Visibility = Visibility.Collapsed;
			main.Name = "Main01";

			SetWinPos(main);

			// WindowInteropHelper helper = new WindowInteropHelper(main);
			// helper.Owner = uiapp.MainWindowHandle;

			start();

			return Result.Succeeded;
		}

	#endregion

	#region public methods

	#endregion

	#region private methods

		private void start()
		{
			bool result;

			using (TransactionGroup tg = new TransactionGroup(doc, "Delux Measure"))
			{
				tg.Start();
				{
					do
					{
						result = main.DxMeasure();

						if (result)
						{
							main = new MainWindow(uidoc);
							main.SetPointsToZero();
							SetWinPos(main);
						}
					}
					while (result);

					// main.Show();
					// main.Visibility=Visibility.Visible;
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
				main.Visibility = Visibility.Collapsed;
			}

			main.Name = "Main01";

			SetWinPos(main);

			start();

			return Result.Succeeded;
		}

	#endregion

	#region public methods

	#endregion

	#region private methods

		private void start()
		{
			bool result = true;

			using (TransactionGroup tg = new TransactionGroup(doc, "Delux Measure"))
			{
				tg.Start();
				{
					main.SetPointsToZero();
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

	#endregion
	}
}