#region Using directives
using System;
using System.Collections.Generic;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using Autodesk.Revit.UI;

using static AOTools.Util;
using static AOTools.AppRibbon;

using static AOTools.ExtensibleStorageMgr;
using static AOTools.SBasicKey;
using static AOTools.SUnitKey;

using UtilityLibrary;
using static UtilityLibrary.MessageUtilities;

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
//			DeleteCurrentSchema();
//			return Result.Succeeded;

			ReadRevitSettings();

			logMsg("");
			logMsgDbLn2("settings", "before");
			ListFieldInfo();
			logMsg("");


			SchemaFields[VERSION_BASIC].Value = "50.00";
			
			SchemaFields[AUTO_RESTORE].Value = false;

			UnitSchemaFields[0][VERSION_UNIT].Value = "sub version 51";
			UnitSchemaFields[1][VERSION_UNIT].Value = "sub version 52";
			UnitSchemaFields[2][VERSION_UNIT].Value = "sub version 53";

			SaveRevitSettings();

			logMsgDbLn2("settings", "after 1");
			ListFieldInfo();
			logMsg("");

			ReadRevitSettings();

			logMsgDbLn2("settings", "after 2");
			ListFieldInfo();

			//			ExtensibleStorageMgr.SaveRevitSettings2();
			//			TaskDialog.Show(APP_NAME, "read message|\n" +
			//				ExtensibleStorageMgr.ReadRevitSettings2());

			//			ExtensibleStorageMgr.SaveRevitBasicSettings("the lazy brown dog... etc. for| " + Environment.UserName.ToLower());
			//			TaskDialog.Show(APP_NAME, "read message|\n" + 
			//				ExtensibleStorageMgr.ReadRevitSettings());



			return Result.Succeeded;
		}
//
//		private void test()
//		{
//			Dictionary<SchemaFldNum, string> t = new Dictionary<SchemaFldNum, string>()
//			{
//				{ SchemaFldNum.VERSION_BASIC, "version"},
//				{ SchemaFldNum.AUTO_RESTORE, "auto restore" },
//				{ SchemaFldNum.CURRENT, "current" },
//				{ SchemaFldNum.UNDEFINED, "undefined" },
//				{ SchemaFldNum.USE_OFFICE, "use office" }
//			};
//
//			int i = 0;
//
//			logMsg(Util.nl);
//			logMsgDbLn2("test using enum field as key");
//
//			foreach (KeyValuePair<SchemaFldNum, string> kvp in t)
//			{
//				logMsgDbLn2("item", i++.ToString("000") +
//					"key / value|  " + kvp.Key.ToString() 
//					+ "  value| " + kvp.Value);
//				
//			}
//		}
		
	}

}