#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using AOToolsDelux.Cells.SchemaDefinition;
using static UtilityLibrary.MessageUtilities;

using AOToolsDelux.Cells.ExStorage;
using AOToolsDelux.Cells.Tests;
using Autodesk.Revit.DB.ExtensibleStorage;
using static AOToolsDelux.Cells.ExStorage.ExStoreMgr;

#endregion

// itemname:	UnitStylesCommand
// username:	jeffs
// created:		1/6/2018 3:55:08 PM


namespace AOToolsDelux
{

	[Transaction(TransactionMode.Manual)]
	class ReadAppExStore : IExternalCommand
	{
		// public static Schema SchemaUnit;
		// public static Entity EntityUnit;
		//
		// public static Schema SchemaDS;
		// public static Entity EntityDS;



		public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
		{

			AppRibbon.UiApp = commandData.Application;
			AppRibbon.Uidoc = AppRibbon.UiApp.ActiveUIDocument;
			AppRibbon.App =  AppRibbon.UiApp.Application;
			AppRibbon.Doc =  AppRibbon.Uidoc.Document;

			OutLocation = OutputLocation.DEBUG;

			return Test01();
		}

		private Result Test01()
		{
			ExStoreHelper xsHlpr = new ExStoreHelper();

			// ExStoreRtnCodes result = ReadRootExStore(xsHlpr);
			ExStoreRtnCodes result = XsMgr.RootExStorExists ? 
				ExStoreRtnCodes.XRC_GOOD : ExStoreRtnCodes.XRC_ROOT_NOT_EXIST;

			if (result != ExStoreRtnCodes.XRC_GOOD)
			{
				XsMgr.ReadSchemaFail(XsMgr.OpDescription);
				return Result.Failed;
			}

			// ExStoreApp xApp = ExStoreApp.Instance();

			try
			{
				// result = XsMgr.ReadApp();
				result = XsMgr.AppExStorExists ? 
					ExStoreRtnCodes.XRC_GOOD : ExStoreRtnCodes.XRC_APP_NOT_EXIST;

				// result = xsHlpr.ReadAppData(xApp);

				if (result != ExStoreRtnCodes.XRC_GOOD)
				{
					XsMgr.ReadSchemaFail(XsMgr.OpDescription);
					return Result.Failed;
				}
			}
			catch (OperationCanceledException)
			{
				return Result.Failed;
			}

			ExStorageTests.ShowDataApp(XsMgr.XApp);

			return Result.Succeeded;
		}

		// private ExStoreRtnCodes ReadRootExStore(ExStoreHelper xsHlpr)
		// {
		// 	// ExStoreRoot xRoot = ExStoreRoot.Instance();
		//
		// 	try
		// 	{
		// 		ExStoreRtnCodes result = XsMgr.ReadRoot(/*ref xRoot*/);
		//
		// 		if (result != ExStoreRtnCodes.XRC_GOOD)
		// 		{
		// 			Debug.WriteLine("initial save failed");
		// 			return ExStoreRtnCodes.XRC_FAIL;
		// 		}
		// 	}
		// 	catch (OperationCanceledException)
		// 	{
		// 		return ExStoreRtnCodes.XRC_FAIL;
		// 	}
		//
		// 	return ExStoreRtnCodes.XRC_GOOD;
		// }

		// private void ShowDataApp(ExStoreApp xApp)
		// {
		// 	TaskDialog td = new TaskDialog("Ex Storage App Data");
		//
		// 	td.MainInstruction = "App Schema was read successfully\ncontents:";
		//
		// 	StringBuilder sb = new StringBuilder();
		//
		// 	foreach (KeyValuePair<SchemaAppKey, SchemaFieldDef<SchemaAppKey>> kvp in xApp.Data)
		// 	{
		// 		string name = xApp.Data[kvp.Key].Name;
		// 		string value = xApp.Data[kvp.Key].Value;
		//
		// 		sb.Append(name).Append("| ").AppendLine(value);
		// 	}
		//
		// 	td.MainContent = sb.ToString();
		// 	td.MainIcon = TaskDialogIcon.TaskDialogIconNone;
		//
		// 	td.Show();
		//
		// }
		//


	}

}