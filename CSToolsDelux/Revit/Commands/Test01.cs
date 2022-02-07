#region using

using System.Diagnostics;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using CSToolsDelux.Revit.Tests;
using CSToolsDelux.WPF.FieldsWindow;
using CSToolsDelux.WPF.TestWindows;

#endregion

// projname: CSToolsDelux
// itemname: Command
// username: jeffs
// created:  8/28/2021 8:49:24 AM

namespace CSToolsDelux.Revit.Commands
{
	[Transaction(TransactionMode.Manual)]
	public class Test01 : IExternalCommand
	{
	#region fields

		private const string ROOT_TRANSACTION_NAME = "Transaction Name";

	#endregion

		internal static UIApplication uiApp;
		internal static UIDocument uiDoc;
		internal static Application app;
		internal static Document doc;

		public static string docName;

		public static SubClass01 sc01 = new SubClass01(null);
		public static SubClass02 sc02;
		public static SubClassPerDoc pd01;

	#region entry point: Execute

		public Result Execute(
			ExternalCommandData commandData, ref string message, ElementSet elements)
		{
			AppRibbon.UiApp = commandData.Application;
			AppRibbon.Uidoc = AppRibbon.UiApp.ActiveUIDocument;
			AppRibbon.App = AppRibbon.UiApp.Application;
			AppRibbon.Doc = AppRibbon.Uidoc.Document;

			uiApp = commandData.Application;
			uiDoc = AppRibbon.UiApp.ActiveUIDocument;
			app = AppRibbon.UiApp.Application;
			doc = AppRibbon.Uidoc.Document;

			docName = doc.Title;

			pd01 = new SubClassPerDoc();

			showMainFields(commandData);

			return Result.Succeeded;
		}

		private Result showMainFields(ExternalCommandData cmdData)
		{
			Result result = Result.Cancelled;

			int key = 0;

			switch (key)
			{
			case 0:
				{
					result = showFieldsWin();
					break;
				}
			case 1:
				{
					result = showTestWin(cmdData);
					break;
				}
			}

			return result;
		}

		private Result showFieldsWin()
		{
			MainFields mf = new MainFields(AppRibbon.Doc);
			bool? result = mf.ShowDialog();

			if (result.HasValue && result.Value) return Result.Succeeded;

			return Result.Cancelled;
		}

		private Result showTestWin(ExternalCommandData cmdData)
		{
			TestWin01 testWin = new TestWin01();

			bool? result = testWin.ShowDialog();

			if (result.HasValue && result.Value) return Result.Succeeded;

			return Result.Cancelled;
		}


		private void showTD()
		{
			TaskDialog td = new TaskDialog("CS Tools Delux");

			td.MainInstruction = "It Worked";
			td.MainContent = "The command ran";

			td.Show();
		}


		private void getSelected()
		{
			// Access current selection
			Selection sel = AppRibbon.Uidoc.Selection;

			// Retrieve elements from database
			FilteredElementCollector col
				= new FilteredElementCollector(AppRibbon.Doc)
				.WhereElementIsNotElementType()
				.OfCategory(BuiltInCategory.INVALID)
				.OfClass(typeof(Wall));

			// Filtered element collector is iterable
			foreach (Element e in col)
			{
				Debug.Print(e.Name);
			}

			// Modify document within a transaction
			using (Transaction tx = new Transaction(AppRibbon.Doc))
			{
				tx.Start(ROOT_TRANSACTION_NAME);
				tx.Commit();
			}
		}
	}

#endregion

#region public methods

#endregion

#region private methods

#endregion
}