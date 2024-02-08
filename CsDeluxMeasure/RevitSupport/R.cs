#region + Using Directives
using System.Windows;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using CsDeluxMeasure.RevitSupport;
using CsDeluxMeasure.RevitSupport.ExtEvents;
using CsDeluxMeasure.Windows;
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

				// UiDoc = uiapp.ActiveUIDocument;
				App = uiapp.Application;
				// Doc = UiDoc.Document;

			}
		}

		public static Window RvtWin { get; set; }

		public static UIControlledApplication UcApp { get; set; }

		public static UIDocument UiDoc => uiapp.ActiveUIDocument;
		public static Application App { get; set; }
		public static Document Doc => uiapp.ActiveUIDocument.Document;


		public static ExtEvttMake EeMaker {get; set; }
		public static ExtEvtHandler EeHandler { get; set; }
		public static ExternalEvent EeEvent {get; set; }

		public static MainWindow Mw { get; set; }
		public static MiniMain Mm {get; set; }
		public static DxMeasure Dx { get; set; }

		// public static void UpdateDoc()
		// {
		// 	if (uiapp == null) return;
		//
		// 	UiDoc = uiapp.ActiveUIDocument;
		// 	Doc = UiDoc.Document;
		// }

		public static void Shutdown()
		{
			if (Mw == null) return;

			// if (Mw != null && Mw.IsVisible) Mw.Close();
			// if (Mm != null && Mm.IsVisible) Mm.Close();

			EeEvent.Dispose();
			EeHandler = null;
			EeMaker = null;

			// Mw.ShutDown();
			// Mm.Shutdown();
			//
			// Mw = null;
			// Mm = null;
		}

		public static void Measure()
		{
			ExtEvttMake(ExtEvtId.EI_MEASURE);
		}

		private static void ExtEvttMake(ExtEvtId eeId)
		{
			EeHandler.Maker.Make(eeId);
			EeEvent.Raise();
		}

	}
}
