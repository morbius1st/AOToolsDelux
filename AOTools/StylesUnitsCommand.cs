#region Using directives

using AOTools.Settings;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using static AOTools.Settings.SettingsMgrUsr;
using static AOTools.Settings.SchemaAppKey;
using static AOTools.Settings.SchemaUsrKey;
using static AOTools.Settings.RevitSettingsMgr;
using static AOTools.Settings.RevitSettingsUnitUsr;
using static AOTools.Settings.RevitSettingsUnitApp;

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
		private const int testVal = 30;

		public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
		{
			output = outputLocation.debug;

			test101();

			return Result.Succeeded;
		}

		// test transfer settings from user to revit and visa versa
		private void test101()
		{
			logMsgDbLn2("Settings", "before");
			logMsg("");
			logMsgDbLn2("user Settings", "before");
			SchemaUnitUtil.ListUnitDictionary(SmUsr.UnitStylesList,4);
			logMsg("");
			logMsgDbLn2("revit Settings", "before");
			RevitSettingsBase.ListRevitSettingInfo(4);
		}

		// enum test
		private void test22()
		{
			logMsgDbLn2("enum test");
			logMsgDbLn2("unit type", UnitType.UT_Undefined.ToString());

			UnitType u = (UnitType) (int) UnitType.UT_Undefined;

			logMsgDbLn2("unit type", u.ToString());
		}

		// test user settings
		private void test21()
		{
			logMsg("");
			logMsgDbLn2("user settings file", "before");
			logMsg("");
			SchemaUnitUtil.ListUnitDictionary(SmUsr.UnitStylesList, 4);

			SmUsr.FormMeasurePointsLocation = new System.Drawing.Point(100, 100);
			SmUsr.MeasurePointsShowWorkplane = true;

			for (int i = 0; i < SmUsr.Count; i++)
			{
				SmUsr.UnitStylesList[i][STYLE_NAME].Value = "Revised Style Name " + i;
			}

			SmUsrSetg.Save();

			logMsgDbLn2("user settings file", "after");
			logMsg("");
			SchemaUnitUtil.ListUnitDictionary(SmUsr.UnitStylesList, 4);
		}

		// test update settings
		private void test3()
		{
			// first read and display the current settings
			RsMgr.Read();

			logMsg("");
			logMsgDbLn2("revit settings", "before");
			RevitSettingsBase.ListRevitSettingInfo();
			logMsg("");

			RsMgr.Update();

			logMsg("");
			logMsgDbLn2("revit settings", "after");
			RevitSettingsBase.ListRevitSettingInfo();
			logMsg("");
		}

		// test reset settings to default
		private void test2()
		{
//			RsMgr.InitalizeRevitSettings();

			// first read and display the current settings
			RsMgr.Read();

			int a = RsuApp.RsuAppSetg[COUNT].Value;

			logMsg("");
			logMsgDbLn2("revit settings", "before");
			RevitSettingsBase.ListRevitSettingInfo();
			logMsg("");
			
			RsuApp.RsuAppSetg[VERSION_BASIC].Value = (testVal * 10.00).ToString();

			RsuApp.RsuAppSetg[AUTO_RESTORE].Value = (testVal / 10) % 2 == 0;

			for (int i = 0; i < RsuApp.RsuAppSetg[COUNT].Value; i++)
			{
				RsuUsr.RsuUsrSetg[i][VERSION_UNIT].Value = "sub version " + (testVal + i + 1);
			}

			RsMgr.Reset();

			logMsg("");
			logMsgDbLn2("settings", "after");
			RevitSettingsBase.ListRevitSettingInfo();
			logMsg("");
		}

		// this is just a basic read, modify, save, re-read test
		private void test1()
		{
			if (!RsMgr.Read())
			{
				TaskDialog.Show("AO Tools", "Could not read Revit Settings or " + nl + "is not Initalized");
				return;
			}

			logMsg("");
			logMsgDbLn2("revit saved settings", "before");
			RevitSettingsBase.ListRevitSettingInfo(4);
			logMsg("");

			RsuApp.RsuAppSetg[VERSION_BASIC].Value = (testVal * 10.00).ToString();

			RsuApp.RsuAppSetg[AUTO_RESTORE].Value = (testVal / 10) % 2 == 0;

			RsuUsr.RsuUsrSetg[0][VERSION_UNIT].Value = "sub version " + (testVal + 1);
			RsuUsr.RsuUsrSetg[0][STYLE_NAME].Value = "style name " + (testVal + 1);
			RsuUsr.RsuUsrSetg[0][STYLE_DESC].Value = "style description " + (testVal + 1);

			RsuUsr.RsuUsrSetg[1][VERSION_UNIT].Value = "sub version " + (testVal + 2);
			RsuUsr.RsuUsrSetg[1][STYLE_NAME].Value = "style name " + (testVal + 2);
			RsuUsr.RsuUsrSetg[1][STYLE_DESC].Value = "style description " + (testVal + 2);

			RsuUsr.RsuUsrSetg[2][VERSION_UNIT].Value = "sub version " + (testVal + 3);
			RsuUsr.RsuUsrSetg[2][STYLE_NAME].Value = "style name " + (testVal + 3);
			RsuUsr.RsuUsrSetg[2][STYLE_DESC].Value = "style description " + (testVal + 3);

			logMsg("");
			if (!RsMgr.Save())
			{
				logMsgDbLn2("revit save settings", "failed");
				return;
			}

			RsMgr.Read();

			logMsgDbLn2("revit saved settings", "after");
			RevitSettingsBase.ListRevitSettingInfo(4);
		}

		// simple test - read and list revit settings
		private void test0()
		{
			if (!RsMgr.Read())
			{
				TaskDialog.Show("AO Tools", "Could not read Revit Settings or " + nl + "is not Initalized");
				return;
			}

			logMsg("");
			logMsgDbLn2("revit saved settings");
			RevitSettingsBase.ListRevitSettingInfo();
		}

	}

}