#region + Using Directives
using System.Windows;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Application = Autodesk.Revit.ApplicationServices.Application;
#endregion

// user name: jeffs
// created:   1/27/2024 6:38:39 AM

namespace Tests01.RevitSupport
{
	public static class R
	{
		private static UIApplication uiapp;

		// static R() { }

		public static void ActivateRevit()
		{
			RvtWin.Activate();
		}

		public static UIApplication UiApp
		{
			get => uiapp;
			set
			{ 
				uiapp = value;
				RvtWin = RevitLibrary.RvtLibrary.WindowHandle(uiapp.MainWindowHandle);

				UiDoc = uiapp.ActiveUIDocument;
				App = uiapp.Application;
				Doc = UiDoc.Document;
			}
		}

		public static Window RvtWin { get; set; }

		public static UIControlledApplication UcApp { get; set; }


		public static UIDocument UiDoc { get; set; }
		public static Application App { get; set; }
		public static Document Doc { get; set; }
	}
}
