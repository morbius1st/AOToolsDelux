#region Using directives

using System;
using System.Collections.Generic;
using System.Xml;
using AOTools.AppSettings.ConfigSettings;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

using AOTools.AppSettings.RevitSettings;
using AOTools.AppSettings.SchemaSettings;
using Autodesk.Revit.UI.Selection;
using UtilityLibrary;

using static AOTools.AppSettings.SchemaSettings.SchemaAppKey;
using static AOTools.AppSettings.SchemaSettings.SchemaUsrKey;
using static AOTools.AppSettings.SettingUtil.SettingsListings;

using static AOTools.AppSettings.RevitSettings.RevitSettingsMgr;
using static AOTools.AppSettings.RevitSettings.RevitSettingsUnitUsr;
using static AOTools.AppSettings.RevitSettings.RevitSettingsUnitApp;

using static AOTools.AppSettings.ConfigSettings.SettingsApp;
using static AOTools.AppSettings.ConfigSettings.SettingsUsr;


using static UtilityLibrary.MessageUtilities;

#endregion

// itemname:	UnitStylesCommand
// username:	jeffs
// created:		1/6/2018 3:55:08 PM


namespace AOTools
{
	[Transaction(TransactionMode.Manual)]
	class SelectElement : IExternalCommand
	{

		public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
		{

			AppRibbon.UiApp = commandData.Application;
			AppRibbon.Uidoc = AppRibbon.UiApp.ActiveUIDocument;
			AppRibbon.App =  AppRibbon.UiApp.Application;
			AppRibbon.Doc =  AppRibbon.Uidoc.Document;

			OutLocation = OutputLocation.DEBUG;

			RevitSettingsUnitApp a = RsuApp;
			SchemaDictionaryApp b = RsuAppSetg;

			RevitSettingsUnitUsr c = RsuUsr;
			List<SchemaDictionaryUsr> d = RsuUsrSetg;

			RsMgr.Init();

			return Test01();
		}


		private Result Test01()
		{
			Element selElement;

			try
			{
				Reference eRef = AppRibbon.Uidoc.Selection.PickObject(ObjectType.Element, "Please pick an element.");

				if (eRef != null && eRef.ElementId != ElementId.InvalidElementId)
				{
					selElement = AppRibbon.Doc.GetElement(eRef);

					RsMgr.SetElement(selElement);
					RsMgr.Init();
					RsMgr.Save();
				}

			}
			catch (OperationCanceledException)
			{
				return Result.Failed;
			}


			return Result.Succeeded;
		}

	}

}