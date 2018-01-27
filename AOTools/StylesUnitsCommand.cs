#region Using directives

using System.Collections.Generic;
using AOTools.Settings;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

using static AOTools.Util;
using static AOTools.Settings.ExtensibleStorageMgr;
using static AOTools.Settings.SBasicKey;
using static AOTools.Settings.SettingsUser;
using static AOTools.Settings.SUnitKey;
using static AOTools.Settings.ExtensibleStorageMgr;

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
		private const int testVal = 20;

		public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
		{
			output = outputLocation.debug;

			test5();

			return Result.Succeeded;
		}

		private void test6()
		{
			logMsgDbLn2("enum test");
			logMsgDbLn2("unit type", UnitType.UT_Undefined.ToString());

			UnitType u = (UnitType) (int) UnitType.UT_Undefined;

			logMsgDbLn2("unit type", u.ToString());
		}

		private void test5()
		{
			logMsg("");
			logMsgDbLn2("user settings file", "before");
			logMsg("");
			ListSettings();

			USet.FormMeasurePointsLocation = new System.Drawing.Point(100, 100);
			USet.MeasurePointsShowWorkplane = true;
			USet.UserUnitStyleSchemas[0][STYLE_NAME].Value = "Revised Style Name 0";
			USet.UserUnitStyleSchemas[1][STYLE_NAME].Value = "Revised Style Name 1";
			USet.UserUnitStyleSchemas[2][STYLE_NAME].Value = "Revised Style Name 2";

			USettings.Save();

			logMsgDbLn2("user settings file", "after");
			logMsg("");
			ListSettings();


		}

		private void ListSettings()
		{
			int j = 0;
			foreach (SchemaDictionaryUnit sd in USet.UserUnitStyleSchemas)
			{
				int i = 0;
				logMsgDbLn2("unit style #", j++.ToString());

				foreach (KeyValuePair<SUnitKey, FieldInfo> kvp in sd)
				{
					logMsgDbLn2("field #" + i++, kvp.Key
						+ "  name| " + kvp.Value.Name
						+ "  value| " + kvp.Value.Value);
				}
				logMsg("");
			}
		}

		private void test4()
		{
			// no app settings
//			ASet.AppIs[0] = 100;
//			ASet.AppIs[1] = 200;
//			ASet.AppIs[2] = 300;
//			ASettings.Save();
//
//			logMsgDbLn2("app settings file", ASettings.SettingsPathAndFile);

			USet.FormMeasurePointsLocation = new System.Drawing.Point(100, 100);
			USet.MeasurePointsShowWorkplane = true;
			USettings.Save();

			
			
			logMsgDbLn2("user Settings file", USettings.SettingsPathAndFile);
		}

		// test update settings
		private void test3()
		{
			// first read and display the current settings
			RSettings.ReadRevitSettings();

			logMsg("");
			logMsgDbLn2("revit settings", "before");
			RSettings.ListFieldInfo();
			logMsg("");

			RSettings.UpdateRevitSettings();

			logMsg("");
			logMsgDbLn2("revit settings", "after");
			RSettings.ListFieldInfo();
			logMsg("");
		}

		// test reset settings to default
		private void test2()
		{
			// first read and display the current settings
			RSettings.ReadRevitSettings();

			logMsg("");
			logMsgDbLn2("revit settings", "before");
			RSettings.ListFieldInfo();
			logMsg("");

			RBSet[VERSION_BASIC].Value = (testVal * 10.00).ToString();

			RBSet[AUTO_RESTORE].Value = (testVal / 10) % 2 == 0;

			RSet[0][VERSION_UNIT].Value = "sub version " + (testVal + 1);
			RSet[1][VERSION_UNIT].Value = "sub version " + (testVal + 2);
			RSet[2][VERSION_UNIT].Value = "sub version " + (testVal + 3);

			RSettings.ResetRevitSettings();

			logMsg("");
			logMsgDbLn2("settings", "after");
			RSettings.ListFieldInfo();
			logMsg("");
		}



		// this is just a basic read, modify, save, re-read test
		private void test1()
		{
			RSettings.InitalizeRevitSettings();

			if (!RSettings.ReadRevitSettings())
			{
				TaskDialog.Show("AO Tools", "Could not read Revit Settings or " + nl + "is not Initalized");
				return;
			}

			logMsg("");
			logMsgDbLn2("revit saved settings", "before");
			RSettings.ListFieldInfo(4);
			logMsg("");

			RBSet[VERSION_BASIC].Value = (testVal * 10.00).ToString();

			RBSet[AUTO_RESTORE].Value = (testVal / 10) % 2 == 0;

			RSet[0][VERSION_UNIT].Value = "sub version " + (testVal + 1);
			RSet[0][STYLE_NAME].Value = "style name " + (testVal + 1);
			RSet[0][STYLE_DESC].Value = "style description " + (testVal + 1);

			RSet[1][VERSION_UNIT].Value = "sub version " + (testVal + 2);
			RSet[1][STYLE_NAME].Value = "style name " + (testVal + 2);
			RSet[1][STYLE_DESC].Value = "style description " + (testVal + 2);

			RSet[2][VERSION_UNIT].Value = "sub version " + (testVal + 3);
			RSet[2][STYLE_NAME].Value = "style name " + (testVal + 3);
			RSet[2][STYLE_DESC].Value = "style description " + (testVal + 3);

			logMsg("");
			if (!RSettings.SaveRevitSettings())
			{
				logMsgDbLn2("revit save settings", "failed");
				return;
			}

			RSettings.ReadRevitSettings();

			logMsgDbLn2("revit saved settings", "after");
			RSettings.ListFieldInfo(4);
		}

	}

}