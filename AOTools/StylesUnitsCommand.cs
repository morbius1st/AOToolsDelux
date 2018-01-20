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
using static UtilityLibrary.SettingsApp;

#endregion

// itemname:	StylesUnitsCommand
// username:	jeffs
// created:		1/6/2018 3:55:08 PM


namespace AOTools
{
	[Transaction(TransactionMode.Manual)]
	class StylesUnitsCommand : IExternalCommand
	{
		private const int testVal = 70;

		public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
		{
			test1();

			return Result.Succeeded;
		}

		private void test4()
		{
			ASet.AppIs[0] = 100;
			ASet.AppIs[1] = 200;
			ASet.AppIs[2] = 300;
			ASetting.Save();

			logMsgDbLn2("app settings file", ASetting.SettingsPathAndFile);
		}

		// test update settings
		private void test3()
		{
			// first read and display the current settings
			ReadRevitSettings();

			logMsg("");
			logMsgDbLn2("settings", "before");
			ListFieldInfo();
			logMsg("");

			UpdateSettings();

			logMsg("");
			logMsgDbLn2("settings", "after");
			ListFieldInfo();
			logMsg("");
		}

		// test reset settings to default
		private void test2()
		{
			// first read and display the current settings
			ReadRevitSettings();

			logMsg("");
			logMsgDbLn2("settings", "before");
			ListFieldInfo();
			logMsg("");

			SchemaFields[VERSION_BASIC].Value = (testVal * 10.00).ToString();

			SchemaFields[AUTO_RESTORE].Value = (testVal / 10) % 2 == 0;

			UnitSchemaFields[0][VERSION_UNIT].Value = "sub version " + (testVal + 1);
			UnitSchemaFields[1][VERSION_UNIT].Value = "sub version " + (testVal + 2);
			UnitSchemaFields[2][VERSION_UNIT].Value = "sub version " + (testVal + 3);

			ResetSettings();

			logMsg("");
			logMsgDbLn2("settings", "after");
			ListFieldInfo();
			logMsg("");
		}



		// this is just a basic read, modify, save, re-read test
		private void test1()
		{
			ReadRevitSettings();

			logMsg("");
			logMsgDbLn2("settings", "before");
			ListFieldInfo();
			logMsg("");

			SchemaFields[VERSION_BASIC].Value = (testVal * 10.00).ToString();

			SchemaFields[AUTO_RESTORE].Value = (testVal / 10) % 2 == 0;

			UnitSchemaFields[0][VERSION_UNIT].Value = "sub version " + (testVal + 1);
			UnitSchemaFields[1][VERSION_UNIT].Value = "sub version " + (testVal + 2);
			UnitSchemaFields[2][VERSION_UNIT].Value = "sub version " + (testVal + 3);

			if (!SaveRevitSettings())
			{
				logMsgDbLn2("save settings", "failed");
			}
			
			ReadRevitSettings();

			logMsgDbLn2("settings", "after 2");
			ListFieldInfo();
		}

	}

}