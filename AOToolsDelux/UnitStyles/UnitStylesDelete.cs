#region Using directives

using AOToolsDelux.AppSettings.RevitSettings;
using AOToolsDelux.Utility;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

using static AOToolsDelux.AppSettings.RevitSettings.RevitSettingsMgr;
using static AOToolsDelux.AppSettings.RevitSettings.RevitSettingsBase.RevitSetgDelRetnCode;
using static AOToolsDelux.AppSettings.SettingUtil.SettingsListings;

using static UtilityLibrary.MessageUtilities;

#endregion

// itemname:	UnitStylesDelete
// username:	jeffs
// created:		1/14/2018 7:19:43 PM


namespace AOToolsDelux
{
	[Transaction(TransactionMode.Manual)]
	class UnitStylesDelete : IExternalCommand
	{
		public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
		{
			AppRibbon.UiApp = commandData.Application;
			AppRibbon.Uidoc = AppRibbon.UiApp.ActiveUIDocument;
			AppRibbon.App =  AppRibbon.UiApp.Application;
			AppRibbon.Doc =  AppRibbon.Uidoc.Document;

			OutLocation = OutputLocation.DEBUG;

			logMsgDbLn2("delete unit styles", "before");
			RevitSettingsBase.ListRevitSchema();

			RsMgr.Init();
			RsMgr.SetElementBasePoint();

			if (!RsMgr.DeleteSchema())
			{
				return Result.Failed;
			}

			logMsg("");
			logMsgDbLn2("delete unit styles", "after");
			RevitSettingsBase.ListRevitSchema();

			logMsg("");
			return Result.Succeeded;
		}
	}
}
