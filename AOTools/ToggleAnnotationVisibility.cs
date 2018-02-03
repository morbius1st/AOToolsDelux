#region Namespaces

using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

#endregion

namespace AOTools
{
	[Transaction(TransactionMode.Manual)]
	public class ToggleAnnotationVisibility : IExternalCommand
	{
		public Result Execute(
		  ExternalCommandData commandData,
		  ref string message,
		  ElementSet elements)
		{
			Document doc = commandData.Application.ActiveUIDocument.Document;

			View av = doc.ActiveView;

			using (Transaction t = new Transaction(doc, "Toggle Annotation Visibility"))
			{
				t.Start();
				av.AreAnnotationCategoriesHidden = !av.AreAnnotationCategoriesHidden;
				t.Commit();
			}

			return Result.Succeeded;

		}
	}
}
