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
			logMsgDbLn2("file location| ", SettingsUser.USettings.SettingsPathAndFile);

			SettingsUser.USet.UserUnitStyleSchemas[0][eSTYLE_NAME].Name = "Style0Name";

			SettingsUser.USettings.Save();

			DisplayUserData();
		}

		private static void DisplayUserData()
		{
			logMsgDbLn2("point| ", SettingsUser.USet.FormMeasurePointsLocation);
			logMsgDbLn2("name| ", SettingsUser.USet.UserUnitStyleSchemas[0][eSTYLE_NAME].Name);
		}

	}
}
