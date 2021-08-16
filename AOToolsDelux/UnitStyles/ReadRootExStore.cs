#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using AOTools.Cells.SchemaDefinition;
using static UtilityLibrary.MessageUtilities;

using AOTools.Cells.ExStorage;
using AOTools.Cells.SchemaDefinition2;
using Autodesk.Revit.DB.ExtensibleStorage;
using static AOTools.Cells.ExStorage.ExStoreMgr;

#endregion

// itemname:	UnitStylesCommand
// username:	jeffs
// created:		1/6/2018 3:55:08 PM


namespace AOTools
{

	[Transaction(TransactionMode.Manual)]
	class ReadRootExStore : IExternalCommand
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

			// ExStoreRoot xRoot = ExStoreRoot.Instance();

			try
			{
				ExStoreRtnCodes result = XsMgr.ReadRoot(/*ref xRoot*/);

				if (result != ExStoreRtnCodes.GOOD)
				{
					Debug.WriteLine("initial save failed");
					return Result.Failed;
				}

				// SchemaGuidManager.AppGuidUniqueString = xRoot.Data[SchemaRootKey.APP_GUID].Value;
			}
			catch (OperationCanceledException)
			{
				return Result.Failed;
			}

			ShowData(XsMgr.XRoot);

			return Result.Succeeded;
		}

		private void ShowData(ExStoreRoot xRoot)
		{
			TaskDialog td = new TaskDialog("Ex Storage Root Data");

			td.MainInstruction = "Root Schema was read successfully\ncontents:";

			StringBuilder sb = new StringBuilder();

			foreach (KeyValuePair<SchemaRootKey, ISchemaFieldDef2<SchemaRootKey>> kvp in xRoot.Data)
			{
				string name = xRoot.Name;
				string value = xRoot.Data[kvp.Key].AsString();

				sb.Append(name).Append("| ").AppendLine(value);
			}

			td.MainContent = sb.ToString();
			td.MainIcon = TaskDialogIcon.TaskDialogIconNone;

			td.Show();

		}
		

	}

}