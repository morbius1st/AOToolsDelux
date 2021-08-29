#region using

using System.Diagnostics;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
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

		public static UIApplication uiapp;
		public static UIDocument uidoc;
		public static Application app;
		public static Document doc;

	#region entry point: Execute

		public Result Execute(
			ExternalCommandData commandData, ref string message, ElementSet elements)
		{
			uiapp = commandData.Application;
			uidoc = uiapp.ActiveUIDocument;
			app = uiapp.Application;
			doc = uidoc.Document;

			showMainFields();

			return Result.Succeeded;
		}

		private void showMainFields()
		{
			MainFields mf = new MainFields();

			mf.Show();
		}

		private void showTestWin()
		{
			TestWin01 testWin = new TestWin01();

			testWin.Show();
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
			Selection sel = uidoc.Selection;

			// Retrieve elements from database
			FilteredElementCollector col
				= new FilteredElementCollector(doc)
				.WhereElementIsNotElementType()
				.OfCategory(BuiltInCategory.INVALID)
				.OfClass(typeof(Wall));

			// Filtered element collector is iterable
			foreach (Element e in col)
			{
				Debug.Print(e.Name);
			}

			// Modify document within a transaction
			using (Transaction tx = new Transaction(doc))
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