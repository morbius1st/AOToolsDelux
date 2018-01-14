#region Using directives
using System;
using System.Collections.Generic;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;

using static AOTools.AppRibbon;
using static AOTools.Util;
using static AOTools.ExtensibleStorageMgr.SBasicKey;
using static AOTools.ExtensibleStorageMgr.SUnitKey;
using SchemaKey = AOTools.ExtensibleStorageMgr.SchemaKey;
using FieldInfo = AOTools.ExtensibleStorageMgr.FieldInfo;

using static UtilityLibrary.MessageUtilities;

#endregion

// itemname:	ExtensibleStorageMgr
// username:	jeffs
// created:		1/7/2018 3:37:43 PM


namespace AOTools
{

	public static class Extensions
	{
		public static FieldInfo Clone(this FieldInfo fi)
		{
			return new FieldInfo(fi.Key, fi.Name,fi.Desc, fi.Value, 
				fi.UnitType, fi.Guid);
		}

		public static Dictionary<T, FieldInfo> 
			Clone<T>(this Dictionary<T, FieldInfo> d) where T : SchemaKey

		{
			Dictionary<T, FieldInfo> copy = 
				new Dictionary<T, FieldInfo>(d.Count);

			foreach (KeyValuePair<T, FieldInfo> kvp in d)
			{
				copy.Add(kvp.Key, new FieldInfo(kvp.Value));
			}

			return copy;
		}
	}

	public class ExtensibleStorageMgr
	{
		public const string KEY_FMT_STR = "0000";

		private static bool initalized = false;


		public class FieldInfo
		{
			public SchemaKey Key { get; }
			public string Name { get; set; }
			public string Desc { get; }
			public UnitType UnitType { get; }
			public string Guid { get; }
			public dynamic Value;


			public FieldInfo(SchemaKey key, string name, string desc, dynamic val, 
				UnitType unitType = UnitType.UT_Undefined, string guid = "")
			{
				Key = key;
				Name = name;
				Desc = desc;
				Value = val;
				UnitType = unitType;
				Guid = guid;
			}

			public FieldInfo(FieldInfo fi)
			{
				Key = fi.Key;
				Name = fi.Name;
				Desc = fi.Desc;
				Value = fi.Value;
				UnitType = fi.UnitType;
				Guid = fi.Guid;
			}
//
//			public FieldInfo(SubScmaFldNum key, string name, string desc, dynamic val,
//							UnitType unitType = UnitType.UT_Undefined, string guid = "")
//			{
//				Key = ((int) key).ToString(KEY_FMT_STR);
//				Name = name;
//				Desc = desc;
//				Value = val;
//				UnitType = unitType;
//				Guid = guid;
//			}
//
//			public FieldInfo(string key, string name, string desc, dynamic val,
//							UnitType unitType = UnitType.UT_Undefined, string guid = "")
//			{
//				Key = key;
//				Name = name;
//				Desc = desc;
//				Value = val;
//				UnitType = unitType;
//				Guid = guid;
//			}

			public dynamic ExtractValue(Entity e, Field f)
			{
				return ExtractValue(Value, e, f);
			}

			private Entity ExtractValue(Entity key, Entity e, Field f)
			{
				return e.Get<Entity>(f);
			}

			private string ExtractValue(string key, Entity e, Field f)
			{
				return e.Get<string>(f);
			}
			
			private int ExtractValue(int key, Entity e, Field f)
			{
				return e.Get<int>(f);
			}
			
			private bool ExtractValue(bool key, Entity e, Field f)
			{
				return e.Get<bool>(f);
			}
			
			private double ExtractValue(double key, Entity e, Field f)
			{
				return e.Get<double>(f);
			}
		}

		enum FmtOpt
		{
			NO = 0,
			YES = 1,
			IGNORE = -1
		}

		public abstract class SchemaKey
		{
			public abstract int Value { get; }
		}

		public class SBasicKey : SchemaKey
		{
			public SBasicKey(int value)
			{
				this.Value = value;
			}

			public override int Value { get; }

			public static readonly SBasicKey UNDEFINED = new SBasicKey(-1);
			public static readonly SBasicKey VERSION_BASIC = new SBasicKey(0);
			public static readonly SBasicKey USE_OFFICE = new SBasicKey(1);
			public static readonly SBasicKey AUTO_RESTORE = new SBasicKey(2);
			public static readonly SBasicKey COUNT = new SBasicKey(3);
			public static readonly SBasicKey CURRENT = new SBasicKey(4);
		}

		public class SUnitKey : SchemaKey
		{
			public SUnitKey(int value)
			{
				this.Value = value;
			}

			public override int Value { get; }

			public static readonly SUnitKey VERSION_UNIT = new SUnitKey(0);
			public static readonly SUnitKey STYLE_NAME = new SUnitKey(1);
			public static readonly SUnitKey CAN_BE_ERASED = new SUnitKey(2);
			public static readonly SUnitKey UNIT_SYSTEM = new SUnitKey(3);
			public static readonly SUnitKey UNIT_TYPE = new SUnitKey(4);
			public static readonly SUnitKey ACCURACY = new SUnitKey(5);
			public static readonly SUnitKey DUT = new SUnitKey(6);
			public static readonly SUnitKey UST = new SUnitKey(7);
			public static readonly SUnitKey SUP_SPACE = new SUnitKey(8);
			public static readonly SUnitKey SUP_LEAD_ZERO = new SUnitKey(9);
			public static readonly SUnitKey SUP_TRAIL_ZERO = new SUnitKey(10);
			public static readonly SUnitKey USE_DIG_GRP = new SUnitKey(11);
			public static readonly SUnitKey USE_PLUS_PREFIX = new SUnitKey(12);
		}

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
		

		

		private const string SCHEMA_NAME = "UnitStyleSettings";
		private const string SCHEMA_DESC = "unit style setings";
		public static readonly Guid SchemaGUID = new Guid("B1788BC0-381E-4F4F-BE0B-93A93B9470FF");

		public static Dictionary<SBasicKey, FieldInfo> SchemaFields;
		private static readonly Dictionary<SBasicKey, FieldInfo> _schemaFields = 
			new Dictionary<SBasicKey, FieldInfo>()
		{
			{	(CURRENT), 
					new FieldInfo(CURRENT, "CurrentUnitStyle", 
						"number of the current style", 0)
			},
			
			{	(COUNT), 
					new FieldInfo(COUNT, "Count", 
						"number of unit styles", 3)
			},

			{	(USE_OFFICE),
					new FieldInfo(USE_OFFICE, "UseOfficeUnitStyle", 
						"use the office standard style", true)
			},

			{	(VERSION_BASIC),
					new FieldInfo(VERSION_BASIC, "version", 
						"version", "1.0")
			},

			{	(AUTO_RESTORE),
					new FieldInfo(AUTO_RESTORE, 
						"AutoRestoreUnitStyle", "auto update to the selected unit style", true)
			}
		};

		// the guid for each sub-schema and the 
		// field that holds the sub-schema - both must match
		// the guid here is missing the last (2) digits.
		// fill in for each sub-schema 
		// unit type is number is a filler
		static readonly FieldInfo _subSchemaFieldInfo = 
			new FieldInfo(UNDEFINED, "LocalUnitStyle{0:D2}",
				"subschema for the local unit style",
				new Entity(), UnitType.UT_Number, "B2788BC0-381E-4F4F-BE0B-93A93B9470{0:x2}");


		// unit style sub-schema information
		//
		private const string UNIT_SCHEMA_NAME = "UnitStyleSchema";
		private const string UNIT_SCHEMA_DESC = "unit style sub schema";
//		private static readonly Guid SubSchemaGuid = 
//			new Guid(UNIT_SCHEMA_GUID + "99");

		public static Dictionary<SUnitKey, FieldInfo>[] UnitSchemaFields;
		private static readonly Dictionary<SUnitKey, FieldInfo> _unitSchemaFields = 
			new Dictionary<SUnitKey, FieldInfo>()
		{
			{   (VERSION_UNIT),
				new FieldInfo(VERSION_UNIT,
					"version", "version", "1.0")
			},

			{   (STYLE_NAME),
				new FieldInfo(STYLE_NAME, 
					"UnitStyle{0:D2}", "name of this unit style", "unit style {0:D2}")
			},

			{   (CAN_BE_ERASED),
				new FieldInfo(CAN_BE_ERASED, 
					"CanBeErased", "can this unit style be erased", false)
			},

			{   (UNIT_SYSTEM),
				new FieldInfo(UNIT_SYSTEM,
					"US", "unit system", (int) UnitSystem.Imperial)
			},

			{   (UNIT_TYPE),
				new FieldInfo(UNIT_TYPE, 
					"UnitType", "unit type", (int) UnitType.UT_Length)
			},

			{   (ACCURACY),
				new FieldInfo(ACCURACY, 
					"Accuracy", "accuracy", (1.0 / 12.0) / 16.0, UnitType.UT_Number)
			},

			{   (DUT),
				new FieldInfo(DUT, "DUT",
					"display unit type", (int) DisplayUnitType.DUT_FEET_FRACTIONAL_INCHES)
			},

			{   (UST),
				new FieldInfo(UST, 
					"UST","unit symbol type", (int) UnitSymbolType.UST_NONE)
			},

			{   (SUP_SPACE),
				new FieldInfo(SUP_SPACE, 
					"SuppressSpaces", "suppress spaces", (int) FmtOpt.YES)
			},

			{   (SUP_LEAD_ZERO),
				new FieldInfo(SUP_LEAD_ZERO,
					"SuppressLeadZero", "suppress leading zero", (int) FmtOpt.NO)
			},

			{   (SUP_TRAIL_ZERO),
				new FieldInfo(SUP_TRAIL_ZERO, 
					"SuppressTrailZero", "suppress trailing zero", (int) FmtOpt.IGNORE)
			},

			{   (USE_DIG_GRP),
				new FieldInfo(USE_DIG_GRP, 
					"DigitGrouping", "digit grouping", (int) FmtOpt.YES)
			},

			{	(USE_PLUS_PREFIX),
				new FieldInfo(USE_PLUS_PREFIX, 
					"PlusPrefix", "plus prefix", (int) FmtOpt.NO)
			}
		};

		// ******************************
		// general routines
		// ******************************
		private static void Init()
		{

			if (initalized) return;

			initalized = true;

//			DeleteCurrentSchema();

			SchemaFields = new Dictionary<SBasicKey, FieldInfo>(_schemaFields);

			InitUnitSchema(_schemaFields[COUNT].Value);
		}

		public static void DeleteCurrentSchema()
		{
			Schema schema = Schema.Lookup(SchemaGUID);
			if (schema != null)
			{
				Element elem = GetProjectBasepoint();
				logMsgDbLn2("delete current schema", elem.DeleteEntity(schema) ? "worked" : "failed");
				Schema.EraseSchemaAndAllEntities(schema, false);
				schema.Dispose();
			}
		}

		public static void InitUnitSchema(int count)
		{
			UnitSchemaFields = new Dictionary<SUnitKey, FieldInfo>
				[count];

			// create the UnitSchema's
			// personlize the sub schema's
			for (int i = 0; i < count; i++)
			{
				UnitSchemaFields[i] = _unitSchemaFields.Clone();
				UnitSchemaFields[i][STYLE_NAME].Name =
					string.Format(_unitSchemaFields[STYLE_NAME].Name, i);
				UnitSchemaFields[i][STYLE_NAME].Value =
					string.Format(_unitSchemaFields[STYLE_NAME].Value, i);
			}
		}

		// ******************************
		// save settings routines
		// ******************************

		// save the basic settings if initalized
		public static bool SaveRevitBasicSettings()
		{
			if (!initalized) { return false; }

			return SaveRevitSettings();
		}

		// save the basic settings to the revit project base point
		// this saves both the basic and the unit styles
		private static bool SaveRevitSettings()
		{
			try
			{
				Element elem = GetProjectBasepoint();

				SchemaBuilder sbld = new SchemaBuilder(SchemaGUID);

				sbld.SetReadAccessLevel(AccessLevel.Public);
				sbld.SetWriteAccessLevel(AccessLevel.Vendor);
				sbld.SetVendorId(Util.GetVendorId());
				sbld.SetSchemaName(SCHEMA_NAME);
				sbld.SetDocumentation(SCHEMA_DESC);

				// this makes the basic setting fields
				MakeFields(sbld, SchemaFields);

				Dictionary<string, string> subSchemaFields = 
					new Dictionary<string, string>(SchemaFields[COUNT].Value);

				// temp - test making ) unit subschemas
				for (int i = 0; i < SchemaFields[COUNT].Value; i++)
				{
					string guid = string.Format(_subSchemaFieldInfo.Guid, i);	// + suffix;
					string fieldName =
						string.Format(_subSchemaFieldInfo.Name, i);
					FieldBuilder fbld =
						sbld.AddSimpleField(fieldName, typeof(Entity));
					fbld.SetDocumentation(_subSchemaFieldInfo.Desc);
					fbld.SetSubSchemaGUID(new Guid(guid));

					subSchemaFields.Add(fieldName, guid);
				}

				Schema schema = sbld.Finish();

				Entity entity = new Entity(schema);

				// set the basic fields
				SaveFieldValues(schema, entity, SchemaFields);

				int j = 0;

				foreach (KeyValuePair<string, string> kvp in subSchemaFields)
				{
					Field f = schema.GetField(kvp.Key);
					Entity subEntity = 
						MakeUnitSchema(kvp.Value, UnitSchemaFields[j]);
					entity.Set(f, subEntity);
				}

				using (Transaction t = new Transaction(Doc, "Unit Style Settings"))
				{
					t.Start();
					elem.SetEntity(entity);
					t.Commit();
				}

			}
			catch
			{
				return false;
			}
			return true;
		}


		/// <summary>
		/// general routine to make one schema field for<para/>
		/// each item in a field info list
		/// </summary>
		/// <param name="sbld"></param>
		/// <param name="fieldList"></param>
		private static void MakeFields<T>(SchemaBuilder sbld, 
			Dictionary<T, FieldInfo> fieldList)
		{
			foreach (KeyValuePair<T, FieldInfo> kvp in fieldList)
			{
				MakeField(sbld, kvp.Value);
			}
		}

		/// <summary>
		/// general routine to make a single schema field
		/// </summary>
		/// <param name="sbld"></param>
		/// <param name="fieldInfo"></param>
		private static void MakeField(SchemaBuilder sbld, FieldInfo fieldInfo)
		{
			FieldBuilder fbld = sbld.AddSimpleField(
					fieldInfo.Name, fieldInfo.Value.GetType());

			fbld.SetDocumentation(fieldInfo.Desc);

			if (fieldInfo.UnitType != UnitType.UT_Undefined)
			{
				fbld.SetUnitType(fieldInfo.UnitType);
			}
		}

		/// <summary>
		/// routine to make a unit schema and its fields
		/// </summary>
		/// <param name="guid"></param>
		/// <param name="unitSchemaFields"></param>
		/// <returns>SchemaBuilder</returns>
		private static Entity MakeUnitSchema(string guid, 
			Dictionary<SUnitKey, FieldInfo> unitSchemaFields)
		{
			SchemaBuilder sbld = new SchemaBuilder(new Guid(guid));

			sbld.SetReadAccessLevel(AccessLevel.Public);
			sbld.SetWriteAccessLevel(AccessLevel.Vendor);
			sbld.SetVendorId(Util.GetVendorId());
			sbld.SetSchemaName(UNIT_SCHEMA_NAME);
			sbld.SetDocumentation(UNIT_SCHEMA_DESC);

			MakeFields(sbld, unitSchemaFields);

			Schema schema = sbld.Finish();

			Entity entity = new Entity(schema);

			SaveFieldValues(schema, entity, unitSchemaFields);

			return entity;
		}

		private static void SaveFieldValues<T>(Schema schema, Entity entity,
			Dictionary<T, FieldInfo> fieldList)
		{
			foreach (KeyValuePair<T, FieldInfo> kvp in fieldList) {
				Field field = schema.GetField(kvp.Value.Name);

				if (kvp.Value.UnitType != UnitType.UT_Undefined)
				{
					entity.Set(field, kvp.Value.Value, DisplayUnitType.DUT_GENERAL);
				}
				else
				{
					entity.Set(field, kvp.Value.Value);
				}
			}
		}

		// ******************************
		// read setting routines
		// ******************************


		// routine to read the existing saved settings
		// if none exist, this saves the default settings, then
		// reads them back, and then flags that this is initalized
		public static bool ReadRevitBasicSettings()
		{
			Init();

			if (!ReadRevitSettings(SchemaFields, SchemaGUID))
			{
				SaveRevitBasicSettings();

				if (ReadRevitSettings(SchemaFields, SchemaGUID))
				{
					return false;
				}
			}

			return true;
		}


		// general routine to read through a saved schema and 
		// get the value from each field 
		// this will work with any field list
		private static bool ReadRevitSettings<T>(Dictionary<T, FieldInfo> fieldList, 
			Guid guid)
		{
			try
			{
				Schema schema = Schema.Lookup(guid);

				if (!schema?.IsValidObject != true) { return false; }

				Element elem = GetProjectBasepoint();

				Entity elemEntity = elem.GetEntity(schema);

				if (elemEntity?.Schema == null) { return false; }

				foreach (KeyValuePair<T, FieldInfo> kvp in fieldList)
				{
					kvp.Value.Value = GetFieldValue(elemEntity, schema, kvp.Value);
				}

				if (!ReadRevitUnitStyles(elemEntity, schema, UnitSchemaFields))
				{
					return false;
				}

			}
			catch { throw; }

			return true;
		}

		private static bool ReadRevitUnitStyles(Entity elemEntity, Schema schema,
			Dictionary<SUnitKey, FieldInfo>[] unitSchemaFields)
		{
			try
			{
				for (int i = 0; i < SchemaFields[COUNT].Value; i++)
				{
					FieldInfo fi = new FieldInfo(_subSchemaFieldInfo);

					string subSchemaName = string.Format(_subSchemaFieldInfo.Name, i);

//					Entity subEntity = GetFieldValue(elem, schema, fi);
					Entity subSchema = elemEntity.Get<Entity>(subSchemaName);

					ReadSubEntity(subSchema, schema, UnitSchemaFields[i]);
				}
			}
			catch { throw; }

			return true;
		}

		// general routine to extract the value from a field
		// this will work with any schema
		private static dynamic GetFieldValue(Entity elemEntity, 
			Schema schema, FieldInfo fi)
		{
//			Entity entity = elem.GetEntity(schema);

			Field f = schema.GetField(fi.Name);

			return fi.ExtractValue(elemEntity, f);
		}

		private static void ReadSubEntity(Entity subSchema, Schema schema,
			Dictionary<SUnitKey, FieldInfo> unitSchemaField)
		{
			foreach (KeyValuePair<SUnitKey, FieldInfo> kvp 
				in unitSchemaField)
			{
				Field f = schema.GetField(kvp.Value.Name);
				kvp.Value.Value =
					kvp.Value.ExtractValue(subSchema, f);
			}
		}

		// ******************************
		// listing routines
		// ******************************

		public static void ListFieldInfo()
		{
			logMsgDbLn2("basic", "settings");
			ListFieldInfo(SchemaFields);

			for (int i = 0; i < SchemaFields[COUNT].Value; i++)
			{
				logMsg(nl);
				logMsgDbLn2("unit", "settings");
				ListFieldInfo(UnitSchemaFields[i]);
			}
		}

		private static void ListFieldInfo<T>(Dictionary<T, FieldInfo> fieldList)
		{
			int i = 0;

			foreach (KeyValuePair<T, FieldInfo> kvp in fieldList)
			{
				logMsgDbLn2("field #" + i++, kvp.Key.GetType().Name
					+ "  name| " + kvp.Value.Name
					+ "  value| " + kvp.Value.Value);
			}
		}

		//		private static Entity MakeSubSchema()
		//		{
		//			SchemaBuilder sbld = new SchemaBuilder(SubSchemaGuid);
		//
		//			sbld.SetReadAccessLevel(AccessLevel.Public);
		//			sbld.SetWriteAccessLevel(AccessLevel.Vendor);
		//			sbld.SetVendorId(Util.GetVendorId());
		//			sbld.SetDocumentation(SCHEMA_DESC);
		//			sbld.SetSchemaName("subSchema" + seq.ToString("0000"));
		//		}
		//
		//		private const string guid = "59338959-393B-4064-9EE0-ADE4599D";
		//
		//		public static bool SaveRevitSettings2()
		//		{
		//			if (VendorId == null) Initalize();
		//
		//			ListSchemaInMemory();
		//
		//
		////
		////			IDictionary<string, Entity> testDict = new Dictionary<string, Entity>(3);
		////
		////			testDict.Add("name1", MakeSubSchema("a1", 101, 201.0, 1));
		////			testDict.Add("name2", MakeSubSchema("a2", 102, 202.0, 2));
		////			testDict.Add("name3", MakeSubSchema("a3", 103, 203.0, 3));
		//
		//			if (VendorId == null) Initalize();
		//
		//			Element elem = GetProjectBasepoint();
		//
		//			SchemaBuilder sbld = new SchemaBuilder(SchemaGUID);
		//
		//			sbld.SetReadAccessLevel(AccessLevel.Public);
		//			sbld.SetWriteAccessLevel(AccessLevel.Vendor);
		//			sbld.SetVendorId(VendorId);
		//			sbld.SetDocumentation(SCHEMA_DESC);
		//			sbld.SetSchemaName(SCHEMA1_NAME);
		//
		////			FieldBuilder fbld = sbld.AddMapField("dictionary", typeof(string), typeof(Entity));
		////			fbld.SetSubSchemaGUID(new Guid(guid + "0000"));
		//
		//			FieldBuilder fbld1 = sbld.AddSimpleField("field1", typeof(Entity));
		//			fbld1.SetSubSchemaGUID(new Guid(guid + "0001"));
		//
		//			FieldBuilder fbld2 = sbld.AddSimpleField("field2", typeof(Entity));
		//			fbld2.SetSubSchemaGUID(new Guid(guid + "0002"));
		//
		//			FieldBuilder fbld3 = sbld.AddSimpleField("field3", typeof(Entity));
		//			fbld3.SetSubSchemaGUID(new Guid(guid + "0003"));
		//
		//			Schema schema = sbld.Finish();
		//
		//			Entity entity = new Entity(schema);
		//
		//			entity.Set("field1", MakeSubSchema("a1", 101, 201.0, 1));
		//			entity.Set("field2", MakeSubSchema("a2", 102, 202.0, 2));
		//			entity.Set("field3", MakeSubSchema("a3", 103, 203.0, 3));
		//
		//			using (Transaction t = new Transaction(Doc, "unit style test"))
		//			{
		//				t.Start();
		//				elem.SetEntity(entity);
		//				t.Commit();
		//			}
		//
		//			return true;
		//		}
		//
		//		private static Entity MakeSubSchema(string testStr, int testInt, double testDbl, int seq)
		//		{
		//			string g = guid + seq.ToString("0000");
		//			
		//			SchemaBuilder sbld = new SchemaBuilder(new Guid(g));
		//
		//			sbld.SetReadAccessLevel(AccessLevel.Public);
		//			sbld.SetWriteAccessLevel(AccessLevel.Vendor);
		//			sbld.SetVendorId(VendorId);
		//			sbld.SetDocumentation(SCHEMA_DESC);
		//			sbld.SetSchemaName("subSchema" + seq.ToString("0000"));
		//
		//			FieldBuilder fbldStr = sbld.AddSimpleField("fieldStr", typeof(string));
		//			FieldBuilder fbldInt = sbld.AddSimpleField("fieldInt", typeof(int));
		//			FieldBuilder fbldDbl = sbld.AddSimpleField("fieldDbl", typeof(double));
		//			fbldDbl.SetUnitType(UnitType.UT_Number);
		//
		//			Schema schema = sbld.Finish();
		//
		//			Entity entity = new Entity(schema);
		//
		//			Field fldStr = schema.GetField("fieldStr");
		//			Field fldInt = schema.GetField("fieldInt");
		//			Field fldDbl = schema.GetField("fieldDbl");
		//
		//			entity.Set(fldStr, testStr);
		//			entity.Set(fldInt, testInt);
		//			entity.Set(fldDbl, testDbl, DisplayUnitType.DUT_GENERAL);
		//
		//			return entity;
		//		}
		//
		//		public static string ReadRevitBasicSettings()
		//		{
		//
		//			try
		//			{
		//				Element elem = GetProjectBasepoint();
		//
		//				Schema schema = Schema.Lookup(SchemaGUID);
		//
		//				if (schema == null) return null;
		//
		//				Field fld = schema.GetField(FIELD1_NAME);
		//
		//				Entity entity = elem.GetEntity(schema);
		//
		//				string test = entity.Get<string>(fld);
		//
		//				return test;
		//
		//			}
		//			catch { }
		//
		//			return null;
		//		}
		//
		//		public static string ReadRevitSettings2()
		//		{
		//			StringBuilder sb = new StringBuilder();
		//
		//			try
		//			{
		//				Element elem = GetProjectBasepoint();
		//
		//				Schema schema = Schema.Lookup(SchemaGUID);
		//
		//				ListSchemaFields(schema);
		//
		//				if (schema == null) return null;
		//
		//				Entity entElem = elem.GetEntity(schema);
		//
		//				Entity entity;
		//
		//				entity = entElem.Get<Entity>("field1");
		//				sb.Append(ReadSubEntity(entity));
		//
		//				entity = entElem.Get<Entity>("field2");
		//				sb.Append(ReadSubEntity(entity));
		//
		//				entity = entElem.Get<Entity>("field3");
		//				sb.Append(ReadSubEntity(entity));
		//
		//				return sb.ToString();
		//
		//			}
		//			catch { }
		//
		//			return null;
		//		}
		//
		//		private static string ReadSubEntity(Entity entity)
		//		{
		//			StringBuilder sb = new StringBuilder();
		//
		//			string s = entity.Get<string>("fieldStr");
		//			int i = entity.Get<int>("fieldInt");
		//			double d = entity.Get<double>("fieldDbl", DisplayUnitType.DUT_GENERAL);
		//
		//			sb.Append("string is| ").AppendLine(s);
		//			sb.Append("int is| ").AppendLine(i.ToString());
		//			sb.Append("double is| ").AppendLine(d.ToString());
		//
		//			return sb.ToString();
		//		}
		//
		//		private static void ListSchemaFields(Schema schema)
		//		{
		//			foreach (Field  fld in schema.ListFields())
		//			{
		//				logMsgDbLn2("field", fld.FieldName + "  type| " + fld.ValueType);
		//			}
		//		}
		//
		//		private static void ListSchemaInMemory()
		//		{
		//			foreach (Schema schema in Schema.ListSchemas())
		//			{
		//				logMsgDbLn2("schema", schema.SchemaName);
		//			}
		//		}



	}
}
