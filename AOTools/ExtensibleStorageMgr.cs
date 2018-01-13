#region Using directives
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Media.Animation;
using System.Xml.Serialization;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using Autodesk.Revit.UI;

using static AOTools.AppRibbon;
using static AOTools.Util;
using FieldInfo = AOTools.ExtensibleStorageMgr.FieldInfo;

using static UtilityLibrary.MessageUtilities;

#endregion

// itemname:	ExtensibleStorageMgr
// username:	jeffs
// created:		1/7/2018 3:37:43 PM


namespace AOTools
{

	public static class FieldInfoExtensions
	{
		public static FieldInfo Clone(this FieldInfo fi)
		{
			return new FieldInfo(fi.FieldNumber, fi.Name,
					fi.Desc, fi.Value, fi.UnitType, fi.Guid);
		}
	}

	public class ExtensibleStorageMgr
	{
		private static bool initalized = false;


		public class FieldInfo
		{
			public int FieldNumber { get;}
			public string Name { get; }
			public string Desc { get; }
			public UnitType UnitType { get; }
			public string Guid { get; }
			public dynamic Value;


			public FieldInfo(int fieldNumber, string name, string desc, dynamic val, 
				UnitType unitType = UnitType.UT_Undefined, string guid = "")
			{
				FieldNumber = fieldNumber;
				Name = name;
				Desc = desc;
				Value = val;
				UnitType = unitType;
				Guid = guid;
			}
//
//			public static FieldInfo Duplicate(FieldInfo fi)
//			{
//				return new FieldInfo(fi.FieldNumber, fi.Name, 
//					fi.Desc, fi.Value, fi.UnitType, fi.Guid);
//			}


			public dynamic ExtractValue(Entity e, Field f)
			{
				return ExtractValue(Value, e, f);
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

		public enum SchemaFldNum 
		{
			VERSION = 0,
			USE_OFFICE = 1,
			AUTO_RESTORE = 2,
			CURRENT = 3
		}


		enum SubScmaFldNum
		{
			VERSION = 0,
			STYLE_NAME = 1,
			CAN_BE_ERASED = 2,
			UNIT_SYSTEM = 3,
			UNIT_TYPE = 4,
			ACCURACY = 5,
			DUT = 6,
			UST = 7,
			SUP_SPACE = 8,
			SUP_LEAD_ZERO = 9,
			SUP_TRAIL_ZERO = 10,
			USE_DIG_GRP = 11,
			USE_PLUS_PREFIX = 12,
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
		

		// this is the guid for each sub-schema and the 
		// field that holds the sub-schema - both must match
		// missing the last (2) digits - fill in for each sub-schema
		private const string UNIT_SCHEMA_GUID = "B2788BC0-381E-4F4F-BE0B-93A93B9470";


		private const string SCHEMA_NAME = "UnitStyleSettings";
		private const string SCHEMA_DESC = "unit style setings";
		public static readonly Guid SchemaGUID = new Guid("B1788BC0-381E-4F4F-BE0B-93A93B947000");

		public static List<FieldInfo> SchemaFields;
		private static List<FieldInfo> _schemaFields = new List<FieldInfo>()
		{
			new FieldInfo((int) SchemaFldNum.CURRENT, 
				"CurrentUnitStyle", "number of the current style", 0),

			new FieldInfo((int) SchemaFldNum.VERSION, 
				"version", "version", "1.0"),

			new FieldInfo((int) SchemaFldNum.USE_OFFICE, 
				"UseOfficeUnitStyle", "use the office standard style", true),

			new FieldInfo((int) SchemaFldNum.AUTO_RESTORE, 
				"AutoRestoreUnitStyle", "auto update to the selected unit style", true),
		};

		static FieldInfo _subSchemaFieldInfo = 
			new FieldInfo(-1, "LocalUnitStyle{0:00}",
				"subschema for the local unit style", 
				null, UnitType.UT_Number, UNIT_SCHEMA_GUID);



		// unit style sub-schema information
		//
		private const string UNIT_SCHEMA_NAME = "UnitStyleSchema";
		private const string UNIT_SCHEMA_DESC = "unit style sub schema";
		private static readonly Guid SubSchemaGuid = 
			new Guid(UNIT_SCHEMA_GUID + "99");

		public static List<FieldInfo> UnitSchemaFields;
		private static List<FieldInfo> _unitSchemaFields = new List<FieldInfo>()
		{
			new FieldInfo((int) SubScmaFldNum.VERSION,
				"version", "version", "1.0"),

			new FieldInfo((int) SubScmaFldNum.STYLE_NAME, 
				"UnitStyle", "name of this unit style", "unit style {0:00}"),

			new FieldInfo((int) SubScmaFldNum.CAN_BE_ERASED, 
				"CanBeErased", "can this unit style be erased", false),

			new FieldInfo((int) SubScmaFldNum.UNIT_SYSTEM,
				"US", "unit system", (int) UnitSystem.Imperial),

			new FieldInfo((int) SubScmaFldNum.UNIT_TYPE, 
				"UnitType", "unit type", (int) UnitType.UT_Length),

			new FieldInfo((int) SubScmaFldNum.ACCURACY, 
				"Accuracy", "accuracy", (1.0 / 12.0) / 16.0),

			new FieldInfo((int) SubScmaFldNum.DUT, "DUT",
				"display unit type", (int) DisplayUnitType.DUT_FEET_FRACTIONAL_INCHES),

			new FieldInfo((int) SubScmaFldNum.UST, 
				"UST","unit symbol type", (int) UnitSymbolType.UST_NONE),

			new FieldInfo((int) SubScmaFldNum.SUP_SPACE, 
				"SuppressSpaces", "suppress spaces", (int) FmtOpt.YES),

			new FieldInfo((int) SubScmaFldNum.SUP_LEAD_ZERO,
				"SuppressLeadZero", "suppress leading zero", (int) FmtOpt.NO),

			new FieldInfo((int) SubScmaFldNum.SUP_TRAIL_ZERO, 
				"SuppressTrailZero", "suppress trailing zero", (int) FmtOpt.IGNORE),

			new FieldInfo((int) SubScmaFldNum.USE_DIG_GRP, 
				"DigitGrouping", "digit grouping", (int) FmtOpt.YES),

			new FieldInfo((int) SubScmaFldNum.USE_PLUS_PREFIX, 
				"PlusPrefix", "plus prefix", (int) FmtOpt.NO),
		};

		// ******************************
		// general routines
		// ******************************
		private static void Init()
		{
			if (initalized) return;

			initalized = true;

			SchemaFields = new List<FieldInfo>(_schemaFields);
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
			SchemaFields.Sort((a, b) => (a.FieldNumber.CompareTo(b.FieldNumber)));
//			UnitSchemaFields.Sort((a, b) => (a.FieldNumber.CompareTo(b.FieldNumber)));

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

				Schema schema = sbld.Finish();

				Entity entity = new Entity(schema);

				// set the basic fields
				SaveFieldValues(schema, entity, SchemaFields);

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
		private static void MakeFields(SchemaBuilder sbld, List<FieldInfo> fieldList)
		{
			foreach (FieldInfo fieldInfo in fieldList)
			{
				MakeField(sbld, fieldInfo);
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
		/// <returns>SchemaBuilder</returns>
		private static Entity MakeUnitSchema(string guid)
		{
			// create a copy of the template
			UnitSchemaFields = new List<FieldInfo>(_unitSchemaFields);
			// sort to insure everythins is in the correct order
			UnitSchemaFields.Sort((a, b) => 
					a.FieldNumber.CompareTo(b.FieldNumber));



			SchemaBuilder sbld = new SchemaBuilder(new Guid(guid));

			sbld.SetReadAccessLevel(AccessLevel.Public);
			sbld.SetWriteAccessLevel(AccessLevel.Vendor);
			sbld.SetVendorId(Util.GetVendorId());
			sbld.SetSchemaName(UNIT_SCHEMA_NAME);
			sbld.SetDocumentation(UNIT_SCHEMA_DESC);

			MakeFields(sbld, _unitSchemaFields);

			Schema schema = sbld.Finish();

			Entity entity = new Entity(schema);

			SaveFieldValues(schema, entity, _unitSchemaFields);

			return entity;
		}

		private static void SaveFieldValues(Schema schema, Entity entity, List<FieldInfo> fieldList)
		{
			foreach (FieldInfo fieldInfo in fieldList) {
				Field field = schema.GetField(fieldInfo.Name);
				entity.Set(field, fieldInfo.Value);
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
			if (ReadRevitSettings(SchemaFields, SchemaGUID))
			{
				SaveRevitBasicSettings();

				if (ReadRevitSettings(SchemaFields, SchemaGUID))
				{
					return false;
				}
			}

			Init();

			return true;
		}


		// general routine to read through a saved schema and 
		// get the value from each field 
		// this will work with any field list
		private static bool ReadRevitSettings(List<FieldInfo> fieldList, Guid guid)
		{
			try
			{
				Schema schema = Schema.Lookup(guid);

				if (schema == null) { return false; }

				Element elem = GetProjectBasepoint();

				foreach (FieldInfo fieldInfo in fieldList)
				{
					fieldInfo.Value = GetFieldValue(elem, schema, fieldInfo);
				}
			}
			catch { throw; }

			return true;
		}

		// general routine to extract the value from a field
		// this will work with any schema
		private static dynamic GetFieldValue(Element elem,
			Schema schema, FieldInfo fi)
		{
			Field f = schema.GetField(fi.Name);
			Entity entity = elem.GetEntity(schema);

			return fi.ExtractValue(entity, f);
		}

		// ******************************
		// listing routines
		// ******************************

		public static void ListBasicFieldInfo()
		{
			ListFieldInfo(SchemaFields);
		}

		private static void ListFieldInfo(List<FieldInfo> fieldList)
		{
			for (int i = 0; i < fieldList.Count; i++)
			{
				logMsgDbLn2("field #" + i, fieldList[i].FieldNumber
					+ "  name| " + fieldList[i].Name
					+ "  value| " + fieldList[i].Value);
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
