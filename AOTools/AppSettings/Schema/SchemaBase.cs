#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Autodesk.Revit.DB;
//using Autodesk.Revit.DB;
//using Autodesk.Revit.DB.ExtensibleStorage;
using static AOTools.Settings.SchemaAppKey;
using static AOTools.Settings.SchemaUsrKey;
using static UtilityLibrary.MessageUtilities;

#endregion

// itemname:	SchemaDefinitions
// username:	jeffs
// created:		1/14/2018 4:28:23 PM


namespace AOTools.Settings
{
	// master schema
	//  field 0 = version
	//	field 1 = bool : UseOfficeUnitStyle
	//	field 2 = bool : AutoRestoreUnitStyle
	//	field 3 = int  : CurrentUnitStyle
	//	field 10+ = Entity : schema : a unit style

	// unit sub schema  (always UnitType = UT_Length
	//	field 0 = verson
	//	field 1 = string : unit style name
	//	field 2 = bool   : CanBeErased
	//	field 3 = int    : UnitSystem
	//	field 4 = int    : UnitType
	//	field 5 = double : accuracy
	//	field 6 = int    : display unit type
	//	fleid 7 = int    : unit symbol type
	//	field 8 = int    : fmt op: suppress spaces			// 0 = false; 1 = true; -1 = ignore
	//	field 9 = int    : fmt op: suppress leading zeros	// 0 = false; 1 = true; -1 = ignore
	//	field 10= int    : fmt op: suppress trailing zeros	// 0 = false; 1 = true; -1 = ignore
	//	field 11= int    : fmt op: use digit grouping		// 0 = false; 1 = true; -1 = ignore
	//	field 12= int    : fmt op: use plus prefix			// 0 = false; 1 = true; -1 = ignore

	// basic schema is only saved in the Revitfile
	public class SchemaUnitApp
	{
		private const int DEFAULT_COUNT = 3;
		protected const string SCHEMA_NAME = "UnitStyleSettings";
		protected const string SCHEMA_DESC = "unit style setings";

		protected readonly Guid Schemaguid = new Guid("B1788BC0-381E-4F4F-BE0B-93A93B9470FF");

		protected SchemaDictionaryApp SchemaUnitAppDefault { get; } =
			new SchemaDictionaryApp
			{
				{
					(CURRENT),
					new SchemaFieldUnit(CURRENT, "CurrentUnitStyle",
						"number of the current style", 0)
				},

				{
					(COUNT),
					new SchemaFieldUnit(COUNT, "Count",
						"number of unit styles", DEFAULT_COUNT)
				},

				{
					(USE_OFFICE),
					new SchemaFieldUnit(USE_OFFICE, "UseOfficeUnitStyle",
						"use the office standard style", true)
				},

				{
					(VERSION_BASIC),
					new SchemaFieldUnit(VERSION_BASIC, "version",
						"version", "1.0")
				},

				{
					(AUTO_RESTORE),
					new SchemaFieldUnit(AUTO_RESTORE,
						"AutoRestoreUnitStyle", "auto update to the selected unit style", true)
				}
			};

		protected SchemaDictionaryApp GetSchemaUnitAppDefault()
		{
			return SchemaUnitAppDefault.Clone();
		}

		public string GetSubSchemaName(int i)
		{
			return string.Format(SubSchemaFieldInfo.Name, i);
		}

		public static Guid GetSubSchemaGuid(int i)
		{
			return new Guid(string.Format(SubSchemaFieldInfo.Guid, i));
		}

		// the guid for each sub-schema and the 
		// field that holds the sub-schema - both must match
		// the guid here is missing the last (2) digits.
		// fill in for each sub-schema 
		// unit type is number is a filler
		public static readonly SchemaFieldUnit SubSchemaFieldInfo =
			new SchemaFieldUnit(UNDEFINED, "LocalUnitStyle{0:D2}",
				"subschema for the local unit style",
				null, RevitUnitType.UT_UNDEFINED, "B2788BC0-381E-4F4F-BE0B-93A93B9470{0:x2}");
	}

	// unit schema is saved in
	// the app settings as a list of office standard unit styles
	// the user settings for a list of their personal unit styles
	// in the revit files as a list of custom unit styles
	//	[DataContract]
	public class SchemaUnitUsr 
	{
		protected const string SCHEMA_NAME = "UnitStyleSchema";
		protected const string SCHEMA_DESC = "unit style sub schema";

		public static SchemaDictionaryUsr SchemaUnitUsrDefault { get; } =
			new SchemaDictionaryUsr
			{
				{
					(VERSION_UNIT),
					new SchemaFieldUnit(VERSION_UNIT,
						"version", "version", "1.0")
				},

				{
					(STYLE_NAME),
					new SchemaFieldUnit(STYLE_NAME,
						"UnitStyleName", "name of this unit style", "unit style {0:D2}")
				},

				{
					(STYLE_DESC),
					new SchemaFieldUnit(STYLE_DESC,
						"UnitStyleDesc", "description for this unit style", "unit style description")
				},

				{
					(CAN_BE_ERASED),
					new SchemaFieldUnit(CAN_BE_ERASED,
						"CanBeErased", "can this unit style be erased", false)
				},

				{
					(UNIT_SYSTEM),
					new SchemaFieldUnit(UNIT_SYSTEM, "US", "unit system", 0)
				},

				{
					(UNIT_TYPE),
					new SchemaFieldUnit(UNIT_TYPE,
						"UnitType", "unit type", 0)
				},

				{
					(ACCURACY),
					new SchemaFieldUnit(ACCURACY,
						"Accuracy", "accuracy", 0.0, RevitUnitType.UT_NUMBER)
				},

				{
					(DUT),
					new SchemaFieldUnit(DUT, "DUT",
						"display unit type", 0)
				},

				{
					(UST),
					new SchemaFieldUnit(UST,
						"UST", "unit symbol type", 0)
				},

				{
					(SUP_SPACE),
					new SchemaFieldUnit(SUP_SPACE,
						"SuppressSpaces", "suppress spaces", (int) SchemaBoolOpts.YES)
				},

				{
					(SUP_LEAD_ZERO),
					new SchemaFieldUnit(SUP_LEAD_ZERO,
						"SuppressLeadZero", "suppress leading zero", (int) SchemaBoolOpts.NO)
				},

				{
					(SUP_TRAIL_ZERO),
					new SchemaFieldUnit(SUP_TRAIL_ZERO,
						"SuppressTrailZero", "suppress trailing zero", (int) SchemaBoolOpts.IGNORE)
				},

				{
					(USE_DIG_GRP),
					new SchemaFieldUnit(USE_DIG_GRP,
						"DigitGrouping", "digit grouping", (int) SchemaBoolOpts.YES)
				},

				{
					(USE_PLUS_PREFIX),
					new SchemaFieldUnit(USE_PLUS_PREFIX,
						"PlusPrefix", "plus prefix", (int) SchemaBoolOpts.NO)
				}
			};
	}

	public static class SchemaUnitUtil
	{

		public static List<SchemaDictionaryUsr> CreateDefaultSchemaList(int quantity)
		{
			List<SchemaDictionaryUsr> SettingList = new List<SchemaDictionaryUsr>(quantity);

			for (int i = 0; i < quantity; i++)
			{
				SettingList.Add(CreateDefaultSchema(i));
			}

			return SettingList;
		}

		private static SchemaDictionaryUsr CreateDefaultSchema(int itemNumber)
		{
			SchemaDictionaryUsr def = SchemaUnitUsr.SchemaUnitUsrDefault.Clone();

			def[STYLE_NAME].Value =
				string.Format(SchemaUnitUsr.SchemaUnitUsrDefault[STYLE_NAME].Value, itemNumber);

			def[UNIT_SYSTEM].Value = (int) UnitSystem.Imperial;
			def[UNIT_TYPE].Value = (int) UnitType.UT_Length;
			def[ACCURACY].Value = (1.0 / 12.0) / 16.0;
			def[DUT].Value = (int) DisplayUnitType.DUT_FEET_FRACTIONAL_INCHES;
			def[UST].Value = (int) UnitSymbolType.UST_NONE;

			return def;
		}



		public static void ListUnitDictionary(List<SchemaDictionaryUsr> u, int count = -1)
		{
			int j = 0;
			foreach (SchemaDictionaryUsr sd in u)
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
	}
}
