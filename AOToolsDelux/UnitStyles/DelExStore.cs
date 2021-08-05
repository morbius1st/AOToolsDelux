#region Using directives

using System;
using System.Diagnostics;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using AOTools.Cells2.SchemaDefinition;
using static UtilityLibrary.MessageUtilities;

using AOTools.Cells.ExStorage;
using Autodesk.Revit.DB.ExtensibleStorage;
using InvalidOperationException = Autodesk.Revit.Exceptions.InvalidOperationException;
// using static AOTools.Cells.ExStorage.ExStoreMgr;

#endregion

// itemname:	UnitStylesCommand
// username:	jeffs
// created:		1/6/2018 3:55:08 PM


namespace AOTools
{
/*
	[Transaction(TransactionMode.Manual)]
	class DelRootExStore : IExternalCommand
	{
		public Result Execute(ExternalCommandData commandData, 
			ref string message, ElementSet elements)
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
			// XsMgr.Initialize();

			ExStoreRtnCodes result = XsMgr.DeleteRoot();

			if (result != ExStoreRtnCodes.GOOD)
			{
				XsMgr.DeleteSchemaFail(XsMgr.OpDescription);
				return Result.Failed;
			}

			return Result.Succeeded;
		}
	}

	[Transaction(TransactionMode.Manual)]
	class DelAppExStore : IExternalCommand
	{
		public Result Execute(ExternalCommandData commandData, 
			ref string message, ElementSet elements)
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
			ExStoreRtnCodes result = XsMgr.DeleteApp();

			if (result != ExStoreRtnCodes.GOOD)
			{
				XsMgr.DeleteSchemaFail(XsMgr.OpDescription);
				return Result.Failed;
			}

			return Result.Succeeded;
		}
	}
	
	// [Transaction(TransactionMode.Manual)]
	// class DelSubExStor : IExternalCommand
	// {
	// 	public Result Execute(ExternalCommandData commandData, 
	// 		ref string message, ElementSet elements)
	// 	{
	// 		AppRibbon.UiApp = commandData.Application;
	// 		AppRibbon.Uidoc = AppRibbon.UiApp.ActiveUIDocument;
	// 		AppRibbon.App =  AppRibbon.UiApp.Application;
	// 		AppRibbon.Doc =  AppRibbon.Uidoc.Document;
	//
	// 		OutLocation = OutputLocation.DEBUG;
	//
	// 		return Test01();
	// 	}
	//
	// 	private Result Test01()
	// 	{
	// 		ExStoreRtnCodes result = XsMgr.DeleteCells();
	//
	// 		if (result != ExStoreRtnCodes.GOOD)
	// 		{
	// 			XsMgr.DeleteSchemaFail(XsMgr.OpDescription);
	// 			return Result.Failed;
	// 		}
	//
	// 		return Result.Succeeded;
	// 	}
	// }
*/
}