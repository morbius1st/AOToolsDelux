using System;
using System.Collections.Generic;
using AOToolsVue.Settings;
using UtilityLibrary;
using static UtilityLibrary.MessageUtilities;

using static AOToolsVue.Settings.SUnitKey;
using static AOToolsVue.Settings.SUKey;

namespace AOToolsVue
{
	class Program
	{
		static void Main(string[] args)
		{
//			MessageUtilities.output = MessageUtilities.outputLocation.console;

			logMsgFmtln("settings test| ", "begin");

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
			logMsgFmtln("settings test user| ", "begin");
			logMsgFmtln("file location| ", SettingsUser.USettings.SettingsPathAndFile);

			SettingsUser.USet.UserUnitStyleSchemas[0][eSTYLE_NAME].Name = "Style0Name";

			SettingsUser.USettings.Save();

			DisplayUserData();
		}

		private static void DisplayUserData()
		{
			logMsgFmtln("point| ", SettingsUser.USet.FormMeasurePointsLocation);
			logMsgFmtln("name| ", SettingsUser.USet.UserUnitStyleSchemas[0][eSTYLE_NAME].Name);
		}

	}
}
