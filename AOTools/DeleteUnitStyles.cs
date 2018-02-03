#region Using directives

using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

using static AOTools.Settings.RevitSettingsMgr;

#endregion

// itemname:	DeleteUnitStyles
// username:	jeffs
// created:		1/14/2018 7:19:43 PM


namespace AOTools
{
	[Transaction(TransactionMode.Manual)]
	class DeleteUnitStyles : IExternalCommand
	{
		public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
		{
			RsMgr.DeleteSchema();

			return Result.Succeeded;
		}
	}
}
