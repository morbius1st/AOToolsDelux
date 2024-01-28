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
		public static Window RevitWindow { get; set; }

		static R() { }

		public static void ActivateRevit()
		{
			RevitWindow.Activate();
		}

		public static UIApplication Uiapp
		{
			get => uiapp;
			set
			{ 
				uiapp = value;
				RevitWindow = RevitLibrary.RvtLibrary.WindowHandle(uiapp.MainWindowHandle);
			}
		}
		public static UIDocument Uidoc { get; set; }
		public static Application App { get; set; }
		public static Document Doc { get; set; }
	}
}
