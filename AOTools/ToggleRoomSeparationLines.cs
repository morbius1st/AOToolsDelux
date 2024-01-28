#region Namespaces
using System;
using System.Runtime.InteropServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

#endregion

namespace AOTools
{
	[Transaction(TransactionMode.Manual)]
	public class ToggleRoomSeparationLines : IExternalCommand
	{
		public Result Execute(
		  ExternalCommandData commandData,
		  ref string message,
		  ElementSet elements)
		{
			UIDocument uiDoc = commandData.Application.ActiveUIDocument;
			Document doc = uiDoc.Document;

			View av = doc.ActiveView;

			Category toggCategory = Category.GetCategory(doc, BuiltInCategory.OST_RoomSeparationLines);

			bool isVisible = toggCategory.get_Visible(av);

			if (av.CanCategoryBeHidden(toggCategory.Id))
			{
				using (Transaction t = new Transaction(doc, "Toggle Room Separation Line Visibility"))
				{
					t.Start();
					toggCategory.set_Visible(av, !isVisible);
					t.Commit();
				}
			}
			else
			{
				User32.SetStatusText("View template prevents toggling Room Separation Line's visibility");
			}

			return Result.Succeeded;

		}

	}

}
