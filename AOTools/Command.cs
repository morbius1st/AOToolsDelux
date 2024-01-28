#region using
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

using System;
using System.Collections.Generic;
using System.Diagnostics;

#endregion

// projname: AOTools
// itemname: Command
// username: jeffs
// created:  10/4/2022 6:27:27 PM

namespace AOTools
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
			UIApplication uiapp = commandData.Application;
			UIDocument uidoc = uiapp.ActiveUIDocument;
			Application app = uiapp.Application;
			Document doc = uidoc.Document;

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

			return Result.Succeeded;
		}
	}

	#endregion

	#region public methods

	#endregion

	#region private methods

	#endregion
}
