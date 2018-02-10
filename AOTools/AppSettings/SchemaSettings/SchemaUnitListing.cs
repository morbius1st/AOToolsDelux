using System;
using System.Collections.Generic;

using Autodesk.Revit.DB.ExtensibleStorage;

using static AOTools.AppSettings.RevitSettings.RevitSettingsUnitApp;
using static AOTools.AppSettings.RevitSettings.RevitSettingsUnitUsr;

using UtilityLibrary;
using static UtilityLibrary.MessageUtilities;

namespace AOTools.AppSettings.SchemaSettings
{
	public static class SchemaUnitListing
	{
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

//		public static void ListUnitDictionary(List<SchemaDictionaryUsr> u, int count = -1)
//		{
//			int j = 0;
//			foreach (SchemaDictionaryUsr sd in u)
//			{
//				MessageUtilities.logMsgDbLn2("unit style #", j++.ToString());
//
//				ListFieldInfo(sd, count);
//
//				MessageUtilities.logMsg("");
//			}
//		}

		public static void ListFieldInfo<T>(SchemaDictionaryBase<T> fieldList, int count = -1)
		{
			int i = 0;

			foreach (KeyValuePair<T, SchemaFieldUnit> kvp in fieldList)
			{
				if (i == count) return;

				MessageUtilities.logMsgDbLn2("field #" + i++,
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



	}
}