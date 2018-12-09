#region Using directives

using System.Collections.Generic;
using System.Runtime.Serialization;
using static AOToolsVue.Settings.SKey;
using static AOToolsVue.Settings.SUnitKey;
using static AOToolsVue.Settings.SUKey;

#endregion

// itemname:	SchemaDefinitions
// username:	jeffs
// created:		1/14/2018 4:28:23 PM


namespace AOToolsVue.Settings
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


//	[CollectionDataContract(Name = "SchemaFields", KeyName = "SequenceKey", 
//		ValueName = "SchemaField", ItemName = "SchemaFieldItem")]
//	public class SchemaDictionary<T1, T2> : Dictionary<T1, T2>
//	{
//		public SchemaDictionary()
//		{
//		}
//
//		public SchemaDictionary(int capacity) : base(capacity)
//		{
//		}
//
//	}


	[CollectionDataContract(Name = "SchemaFields", KeyName = "SequenceKey",
		ValueName = "SchemaField", ItemName = "SchemaFieldItem")]
	public class SchemaDictionaryBase<T> : Dictionary<T, FieldInfo>
	{
		public SchemaDictionaryBase()
		{
		}

		public SchemaDictionaryBase(int capacity) : base(capacity)
		{
		}

		protected U Clone<U>(U original) where U : SchemaDictionaryBase<T>, new()
		{
			U copy = new U();

			foreach (KeyValuePair<T, FieldInfo> kvp in original)
			{
				copy.Add(kvp.Key, kvp.Value);
			}

			return copy;
		}

	}

	[CollectionDataContract(Name = "SchemaFields", KeyName = "SequenceKey", 
		ValueName = "SchemaField", ItemName = "SchemaFieldItem")]
	public class SchemaDictionary2 : SchemaDictionaryBase<SUKey>
	{
		public SchemaDictionary2()
		{
		}

		public SchemaDictionary2(int capacity) : base(capacity)
		{
		}

		public SchemaDictionary2 Clone()
		{
			return Clone(this);
		}
	}

//	[CollectionDataContract(Name = "UnitStyleList", ItemName = "itemName")]
//	public class SchemaList<T> : List<T> { }



//	// basic schema is only saved in the Revitfile
//	[DataContract]
//	public class BasicSchema
//	{
//		public const string SCHEMA_NAME = "UnitStyleSettings";
//		public const string SCHEMA_DESC = "unit style setings";
//
//		public static readonly Guid SchemaGUID = new Guid("B1788BC0-381E-4F4F-BE0B-93A93B9470FF");
//
//		public static readonly SchemaDictionary _schemaFields =
//			new SchemaDictionary<SBasicKey, FieldInfo>
//			{
//				{
//					(SBasicKey.CURRENT),
//					new FieldInfo(SBasicKey.CURRENT, "CurrentUnitStyle",
//						"number of the current style", 0)
//				},
//
//				{
//					(SBasicKey.COUNT),
//					new FieldInfo(SBasicKey.COUNT, "Count",
//						"number of unit styles", 3)
//				},
//
//				{
//					(SBasicKey.USE_OFFICE),
//					new FieldInfo(SBasicKey.USE_OFFICE, "UseOfficeUnitStyle",
//						"use the office standard style", true)
//				},
//
//				{
//					(SBasicKey.VERSION_BASIC),
//					new FieldInfo(SBasicKey.VERSION_BASIC, "version",
//						"version", "1.0")
//				},
//
//				{
//					(SBasicKey.AUTO_RESTORE),
//					new FieldInfo(SBasicKey.AUTO_RESTORE,
//						"AutoRestoreUnitStyle", "auto update to the selected unit style", true)
//				}
//			};
//
//		internal static SchemaDictionary<SBasicKey, FieldInfo> GetBasicSchemaFields()
//		{
//			return _schemaFields.Clone();
//		}

//		// the guid for each sub-schema and the 
//		// field that holds the sub-schema - both must match
//		// the guid here is missing the last (2) digits.
//		// fill in for each sub-schema 
//		// unit type is number is a filler
//		public static readonly FieldInfo _subSchemaFieldInfo =
//			new FieldInfo(SBasicKey.UNDEFINED, "LocalUnitStyle{0:D2}",
//				"subschema for the local unit style",
//				new Entity(), UnitType.UT_Number, "B2788BC0-381E-4F4F-BE0B-93A93B9470{0:x2}");
//
//		public static Guid GetSubSchemaGuid(int i)
//		{
//			return new Guid(string.Format(_subSchemaFieldInfo.Guid, i));
//		}
//	}


	// unit schema is saved in
	// the app settings as a list of office standard unit styles
	// the user settings for a list of their personal unit styles
	// in the revit files as a list of custom unit styles
	[DataContract]
	public class UnitSchema
	{
//		public static SchemaKey VERSION_UNIT = new SUnitKey(0);
//		public static SchemaKey STYLE_NAME = new SUnitKey(1);
//		public static SchemaKey STYLE_DESC = new SUnitKey(2);
//		public static SchemaKey CAN_BE_ERASED = new SUnitKey(3);

		public static SchemaDictionary2 Make(string name, string desc)
		{
			SchemaDictionary2 temp = _unitSchemaFields.Clone();
			
//			temp[SKey.Key2].Value = name;
//			temp[SKey.Key3].Value = desc;
			temp[eSTYLE_NAME].Value = name;
			temp[eSTYLE_DESC].Value = desc;

			SKey s = new SKey(1);

			return temp;
		}

		public const string UNIT_SCHEMA_NAME = "UnitStyleSchema";
		public const string UNIT_SCHEMA_DESC = "unit style sub schema";

		[DataMember]
		public static readonly SchemaDictionary2 _unitSchemaFields =
			new SchemaDictionary2
			{
				{
					(eVERSION_UNIT),
					new FieldInfo(eVERSION_UNIT,
						"version", "version", "1.0")
				},

				{
					(eSTYLE_NAME),
					new FieldInfo(eSTYLE_NAME,
						"UnitStyleName", "name of this unit style", "unit style {0:D2}")
				},

				{
					(eSTYLE_DESC),
					new FieldInfo(eSTYLE_DESC,
						"UnitStyleDesc", "description for this unit style", "unit style description")
				},

				{
					(eCAN_BE_ERASED),
					new FieldInfo(eCAN_BE_ERASED,
						"CanBeErased", "can this unit style be erased", false)
				},
				
			};

		internal static SchemaDictionary2[] GetUnitSchemaFields(int count)
		{
			SchemaDictionary2[] unitSchemaFields =
				new SchemaDictionary2[count];

			// create the UnitSchema's
			// personlize the sub schema's
			for (int i = 0; i < count; i++)
			{
				unitSchemaFields[i] = _unitSchemaFields.Clone();
				unitSchemaFields[i][eSTYLE_NAME].Value =
					string.Format(_unitSchemaFields[eSTYLE_NAME].Value, i);

			}

			return unitSchemaFields;
		}
//
//		internal static string GetSubSchemaName(int i)
//		{
//			return String.Format(BasicSchema._subSchemaFieldInfo.Name, i);
//		}
	}
}
