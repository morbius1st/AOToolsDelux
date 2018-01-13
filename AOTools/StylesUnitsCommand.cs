#region Using directives
using System;
using System.Collections.Generic;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using Autodesk.Revit.UI;

using static AOTools.Util;
using static AOTools.AppRibbon;

using UtilityLibrary;
using static UtilityLibrary.MessageUtilities;

using static AOTools.ExtensibleStorageMgr;

#endregion

// itemname:	StylesUnitsCommand
// username:	jeffs
// created:		1/6/2018 3:55:08 PM


namespace AOTools
{
	[Transaction(TransactionMode.Manual)]
	class StylesUnitsCommand : IExternalCommand
	{
		public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
		{

			ReadRevitBasicSettings();

			logMsgDbLn2("settings", "before");
			ListBasicFieldInfo();

			_schemaFields[(int) SchemaFldNum.VERSION].Value = "100.00";

			SaveRevitBasicSettings();

			logMsgDbLn2("settings", "after 1");
			ListBasicFieldInfo();

			ReadRevitBasicSettings();

			logMsgDbLn2("settings", "after 2");
			ListBasicFieldInfo();

			//			ExtensibleStorageMgr.SaveRevitSettings2();
			//			TaskDialog.Show(APP_NAME, "read message|\n" +
			//				ExtensibleStorageMgr.ReadRevitSettings2());

			//			ExtensibleStorageMgr.SaveRevitBasicSettings("the lazy brown dog... etc. for| " + Environment.UserName.ToLower());
			//			TaskDialog.Show(APP_NAME, "read message|\n" + 
			//				ExtensibleStorageMgr.ReadRevitBasicSettings());



			return Result.Succeeded;
		}
		
	}

}