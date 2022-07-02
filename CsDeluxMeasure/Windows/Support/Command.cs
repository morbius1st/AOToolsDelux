#region using

using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

#endregion

// projname: CsDeluxMeasure
// itemname: Command
// username: jeffs
// created:  2/12/2022 8:46:31 AM

namespace CsDeluxMeasure.Windows.Support
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

			MainWindow main = new MainWindow();

			main.Show();

			return Result.Succeeded;

		}
	}

#endregion

#region public methods

#endregion

#region private methods

#endregion
}