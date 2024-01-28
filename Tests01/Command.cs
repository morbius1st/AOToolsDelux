#region using

using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Tests01.ExtEvent;
using Tests01.RevitSupport;
using Tests01.Windows;

#endregion

// projname: Tests01
// itemname: Command
// username: jeffs
// created:  1/27/2024 12:19:21 AM

namespace Tests01
{
	[Transaction(TransactionMode.Manual)]
	public class Command : IExternalCommand
	{
	#region fields

		private const string ROOT_TRANSACTION_NAME = "Transaction Name";

	#endregion

	#region entry point: Execute

		public Result Execute(
			ExternalCommandData commandData, ref string message, ElementSet elements)
		{
			// UIApplication uiapp = commandData.Application;
			// UIDocument uidoc = uiapp.ActiveUIDocument;
			// Application app = uiapp.Application;
			// Document doc = uidoc.Document;

			R.Uiapp = commandData.Application;
			R.Uidoc = R.Uiapp.ActiveUIDocument;
			R.App = R.Uiapp.Application;
			R.Doc = R.Uidoc.Document;

			/*
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
			*/

			// Modify document within a transaction
			// using (Transaction tx = new Transaction(R.Doc))
			// {
			// 	tx.Start(ROOT_TRANSACTION_NAME);
			//
			//
			// 	tx.Commit();
			// }

			start();

			return Result.Succeeded;
		}

	#endregion

	#region public methods

	#endregion

	#region private methods

		private void start()
		{
			ExtEvtHandler eEh = new ExtEvtHandler();
			 ExternalEvent eEv = ExternalEvent.Create(eEh);

			MainWindow mw = new MainWindow(eEh, eEv);
			mw.Owner = R.RevitWindow;
			mw.Show();
		}

	#endregion
	}
}