#region Using directives

using System;
using System.Diagnostics;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using AOTools.Cells.SchemaDefinition;
using static UtilityLibrary.MessageUtilities;

using AOTools.Cells.ExStorage;
using Autodesk.Revit.DB.ExtensibleStorage;
using static AOTools.Cells.ExStorage.ExStoreMgr;

#endregion

// itemname:	UnitStylesCommand
// username:	jeffs
// created:		1/6/2018 3:55:08 PM


namespace AOTools
{
	[Transaction(TransactionMode.Manual)]
	class RootExStore : IExternalCommand
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
			if (AppRibbon.Doc.IsDetached) return Result.Cancelled;

			ExStoreHelper xsHlpr = new ExStoreHelper();

			ExStoreRoot xRoot = ExStoreRoot.Instance();

			xRoot.Data[SchemaRootKey.NAME].Value
				= "RootEx4"+AppRibbon.Doc.Title;

			xRoot.Data[SchemaRootKey.DESCRIPTION].Value
				= "Root Ex Storage Data for| "+AppRibbon.Doc.Title;

			try
			{
				xsHlpr.WriteRootExStorageData(xRoot);
			}
			catch (OperationCanceledException)
			{
				return Result.Failed;
			}

			// Schema schemaUnit= SchemaUnit;
			// Entity entityUnit= EntityUnit;
			// 				   
			// Schema schemaDS =  SchemaDS;
			// Entity entityDS =  EntityDS;

			return Result.Succeeded;
		}
		

	}

}