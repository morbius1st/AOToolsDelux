using System;
using System.Collections.Generic;
using AOToolsVue.Settings;
using UtilityLibrary;
using static UtilityLibrary.MessageUtilities;

using static AOToolsVue.Settings.SUnitKey;
using static AOToolsVue.Settings.SUKey;

using static AOToolsVue.Settings.SettingsUser;

namespace AOToolsVue
{
	class Program
	{
		static void Main(string[] args)
		{
			MessageUtilities.OutLocation = OutputLocation.CONSOLE;

			//			MessageUtilities.output = MessageUtilities.outputLocation.console;

			logMsgDbLn2("settings test| ", "begin");

			ProcessUserSettings();

			Wait();
		}
		
		private static void Wait()
		{
			logMsg(MessageUtilities.nl);
			 
			logMsg("waiting ... ");
			System.Console.ReadKey();

			Environment.Exit(0);
		}

		private static void ProcessUserSettings()
		{
			logMsgDbLn2("settings test user| ", "begin");
			logMsgDbLn2("file location| ", USettings.SettingsPathAndFile);

			USet.UserUnitStyleSchemas[0][eSTYLE_NAME].Name = "Style0Name";
			USet.UserUnitStyleSchemas[0][eVERSION_UNIT].Value = "1.0";

			USet.UserUnitStyleSchemas[1][eSTYLE_NAME].Name = "Style1Name";
			USet.UserUnitStyleSchemas[1][eVERSION_UNIT].Value = "1.1";

			USet.UserUnitStyleSchemas[2][eSTYLE_NAME].Name = "Style2Name";
			USet.UserUnitStyleSchemas[2][eVERSION_UNIT].Value = "1.2";

			USettings.Save();

			DisplayUserData();
		}

		private static void DisplayUserData()
		{
			logMsgDbLn2("point| ", USet.FormMeasurePointsLocation);
			logMsgDbLn2("name| ", USet.UserUnitStyleSchemas[0][eSTYLE_NAME].Name);

			int i = 0;

			foreach (SchemaDictionary2 sd in USet.UserUnitStyleSchemas)
			{
				logMsgDbLn2("");
				logMsgDbLn2("item| ", i++);


				foreach (FieldInfo fi in sd.Values)
				{
					logMsgDbLn2("");
					logMsgDbLn2("name| ", fi.Name);
					logMsgDbLn2("desc| ", fi.Desc);
					logMsgDbLn2("unit type| ", fi.UnitType);
					logMsgDbLn2("value| ", fi.Value);
					logMsgDbLn2("value type| ", fi.Value.GetType().ToString());
				}
			}
		}

	}
}
