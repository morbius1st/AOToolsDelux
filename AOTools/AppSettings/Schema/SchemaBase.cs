#region Using directives

using System;
using System.Collections.Generic;
using Autodesk.Revit.DB;
using UtilityLibrary;
//using Autodesk.Revit.DB;
//using Autodesk.Revit.DB.ExtensibleStorage;

#endregion

// itemname:	SchemaDefinitions
// username:	jeffs
// created:		1/14/2018 4:28:23 PM


namespace AOTools.AppSettings
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
					(SchemaAppKey.CURRENT),
					new SchemaFieldUnit(SchemaAppKey.CURRENT, "CurrentUnitStyle",
						"number of the current style", 0)
				},

				{
					(SchemaAppKey.COUNT),
					new SchemaFieldUnit(SchemaAppKey.COUNT, "Count",
						"number of unit styles", DEFAULT_COUNT)
				},

				{
					(SchemaAppKey.USE_OFFICE),
					new SchemaFieldUnit(SchemaAppKey.USE_OFFICE, "UseOfficeUnitStyle",
						"use the office standard style", true)
				},

				{
					(SchemaAppKey.VERSION_BASIC),
					new SchemaFieldUnit(SchemaAppKey.VERSION_BASIC, "version",
						"version", "1.0")
				},

				{
					(SchemaAppKey.AUTO_RESTORE),
					new SchemaFieldUnit(SchemaAppKey.AUTO_RESTORE,
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
			new SchemaFieldUnit(SchemaAppKey.UNDEFINED, "LocalUnitStyle{0:D2}",
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
					(SchemaUsrKey.VERSION_UNIT),
					new SchemaFieldUnit(SchemaUsrKey.VERSION_UNIT,
						"version", "version", "1.0")
				},

				{
					(SchemaUsrKey.STYLE_NAME),
					new SchemaFieldUnit(SchemaUsrKey.STYLE_NAME,
						"UnitStyleName", "name of this unit style", "unit style {0:D2}")
				},

				{
					(SchemaUsrKey.STYLE_DESC),
					new SchemaFieldUnit(SchemaUsrKey.STYLE_DESC,
						"UnitStyleDesc", "description for this unit style", "unit style description")
				},

				{
					(SchemaUsrKey.CAN_BE_ERASED),
					new SchemaFieldUnit(SchemaUsrKey.CAN_BE_ERASED,
						"CanBeErased", "can this unit style be erased", false)
				},

				{
					(SchemaUsrKey.UNIT_SYSTEM),
					new SchemaFieldUnit(SchemaUsrKey.UNIT_SYSTEM, "US", "unit system", 0)
				},

				{
					(SchemaUsrKey.UNIT_TYPE),
					new SchemaFieldUnit(SchemaUsrKey.UNIT_TYPE,
						"UnitType", "unit type", 0)
				},

				{
					(SchemaUsrKey.ACCURACY),
					new SchemaFieldUnit(SchemaUsrKey.ACCURACY,
						"Accuracy", "accuracy", 0.0, RevitUnitType.UT_NUMBER)
				},

				{
					(SchemaUsrKey.DUT),
					new SchemaFieldUnit(SchemaUsrKey.DUT, "DUT",
						"display unit type", 0)
				},

				{
					(SchemaUsrKey.UST),
					new SchemaFieldUnit(SchemaUsrKey.UST,
						"UST", "unit symbol type", 0)
				},

				{
					(SchemaUsrKey.SUP_SPACE),
					new SchemaFieldUnit(SchemaUsrKey.SUP_SPACE,
						"SuppressSpaces", "suppress spaces", (int) SchemaBoolOpts.YES)
				},

				{
					(SchemaUsrKey.SUP_LEAD_ZERO),
					new SchemaFieldUnit(SchemaUsrKey.SUP_LEAD_ZERO,
						"SuppressLeadZero", "suppress leading zero", (int) SchemaBoolOpts.NO)
				},

				{
					(SchemaUsrKey.SUP_TRAIL_ZERO),
					new SchemaFieldUnit(SchemaUsrKey.SUP_TRAIL_ZERO,
						"SuppressTrailZero", "suppress trailing zero", (int) SchemaBoolOpts.IGNORE)
				},

				{
					(SchemaUsrKey.USE_DIG_GRP),
					new SchemaFieldUnit(SchemaUsrKey.USE_DIG_GRP,
						"DigitGrouping", "digit grouping", (int) SchemaBoolOpts.YES)
				},

				{
					(SchemaUsrKey.USE_PLUS_PREFIX),
					new SchemaFieldUnit(SchemaUsrKey.USE_PLUS_PREFIX,
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

			def[SchemaUsrKey.STYLE_NAME].Value =
				string.Format(SchemaUnitUsr.SchemaUnitUsrDefault[SchemaUsrKey.STYLE_NAME].Value, itemNumber);

			def[SchemaUsrKey.UNIT_SYSTEM].Value = (int) UnitSystem.Imperial;
			def[SchemaUsrKey.UNIT_TYPE].Value = (int) UnitType.UT_Length;
			def[SchemaUsrKey.ACCURACY].Value = (1.0 / 12.0) / 16.0;
			def[SchemaUsrKey.DUT].Value = (int) DisplayUnitType.DUT_FEET_FRACTIONAL_INCHES;
			def[SchemaUsrKey.UST].Value = (int) UnitSymbolType.UST_NONE;

			return def;
		}



		public static void ListUnitDictionary(List<SchemaDictionaryUsr> u, int count = -1)
		{
			int j = 0;
			foreach (SchemaDictionaryUsr sd in u)
			{
				MessageUtilities.logMsgDbLn2("unit style #", j++.ToString());

				ListFieldInfo(sd, count);

				MessageUtilities.logMsg("");
			}
		}

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
