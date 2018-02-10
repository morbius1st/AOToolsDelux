using System;
using System.Collections.Generic;
using static UtilityLibrary.MessageUtilities;

using static AOTools.AppSettings.RevitSettings.RevitSettingsUnitUsr;
using static AOTools.AppSettings.ConfigSettings.SettingsUsr;
using AOTools.AppSettings.ConfigSettings;
using AOTools.AppSettings.RevitSettings;
using AOTools.AppSettings.SchemaSettings;

namespace AOTools.AppSettings.Util
{
	public static class SettingsListings
	{
		public static void ListRevitSettings()
		{
			logMsgDbLn2("revit app settings");
			ListRevitAppSettings();
			logMsg("");
			logMsgDbLn2("revit usr settings");
			ListUnitDictionary<SchemaDictionaryUsr, SchemaUsrKey>(RsuUsrSetg, 4);
			logMsg("");
		}

		public static void ListConfigSettings()
		{
			logMsgDbLn2("config app settings");
			ListUserAppSettings();
			logMsg("");
			logMsgDbLn2("config user settings");
			ListUnitDictionary<SchemaDictionaryUsr, SchemaUsrKey>(SmuUsrSetg, 4);
			logMsg("");
		}


		public static void ListUnitDictionary<TU, T>(List<TU> u, int count = -1) where TU : SchemaDictionaryBase<T>
		{
			int j = 0;
			foreach (TU sd in u)
			{
				logMsgDbLn2("unit style #", j++.ToString());
				ListFieldInfo(sd, count);
				logMsg("");
			}
		}

		public static void ListFieldInfo<T>(SchemaDictionaryBase<T> fieldList, int count = -1)
		{
			int i = 0;

			foreach (KeyValuePair<T, SchemaFieldUnit> kvp in fieldList)
			{
				if (i == count) return;

				logMsgDbLn2("field #" + i++,
					FormatFieldInfo(kvp.Key as Enum, kvp.Value));
			}
		}

		private static string FormatFieldInfo(Enum key, SchemaFieldUnit fi)
		{
			int len = 28;
			string keyDesc = key?.ToString() ?? "undefined";
			string valueDesc = fi.Value.ToString().PadRight(len).Substring(0, len);
			return $"key| {keyDesc,-20}  name| {fi.Name,-20} value| {valueDesc,-30} unit type| {fi.UnitType}";
		}

		public static void ListRevitAppSettings()
		{
			logMsgDbLn2("data in dictionary");
			foreach (KeyValuePair<SchemaAppKey, SchemaFieldUnit> kvp in RevitSettingsUnitApp.RsuAppSetg)
			{
				logMsgDbLn2("data", "key| " + kvp.Key + "  name| " + kvp.Value.Name + "  value| " + kvp.Value.Value);
			}

			logMsg("");
		}

		public static void ListUserAppSettings()
		{
			logMsgDbLn2("app inits", SettingsApp.SmAppSetg.AppIs[0].ToString()
				+ "  " + SettingsApp.SmAppSetg.AppIs[1].ToString() + "  " + SettingsApp.SmAppSetg.AppIs[2].ToString());


			logMsgDbLn2("data in dictionary");
			foreach (KeyValuePair<SchemaAppKey, SchemaFieldUnit> kvp in SettingsApp.SmAppSetg.SettingsAppData)
			{
				logMsgDbLn2("data", "key| " + kvp.Key + "  name| " + kvp.Value.Name + "  value| " + kvp.Value.Value);
			}

			logMsg("");
		}



	}
}