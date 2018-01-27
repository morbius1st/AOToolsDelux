#region Using directives

using System;
using System.Collections.Generic;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using UtilityLibrary;
using InvalidOperationException = Autodesk.Revit.Exceptions.InvalidOperationException;

using static AOTools.Settings.UnitSchema;

#endregion

// itemname:	ExtensibleStorageMgr
// username:	jeffs
// created:		1/7/2018 3:37:43 PM


namespace AOTools.Settings
{
	public class RevitSettings : ExtensibleStorageMgr
	{
		public static RevitSettings RSettings = new RevitSettings();
		public static List<SchemaDictionaryUnit> RSet = null;
		public static SchemaDictionaryBasic RBSet = null;

		public static bool RSCompare()
		{
			return RSet == UnitRevitSchemaFields;
		}

//		static RevitSettings()
//		{
//
//			RSet = ExtensibleStorageMgr.UnitRevitSchemaFields;
//			RBSet = ExtensibleStorageMgr.BasicRevitSchemaFields;
//		}

		public static void Init()
		{
			if (RSet == null)
			{
				ReadRevitSettings();
				RSet = UnitRevitSchemaFields;
				RBSet = BasicRevitSchemaFields;
				
			}
		}

	}


	public abstract class ExtensibleStorageMgr
	{
		public static bool initalized = false;

		protected static SchemaDictionaryBasic BasicRevitSchemaFields;
		public static List<SchemaDictionaryUnit> UnitRevitSchemaFields = new List<SchemaDictionaryUnit>(3);

		// ******************************
		// general routines
		// ******************************
		private static void InitRevitSettings()
		{
			if (initalized) return;

			initalized = true;

			SetDefaultFields();

		}

		public static bool ESCompare(List<SchemaDictionaryUnit> test)
		{
			return test == UnitRevitSchemaFields;
		}

		// delete thecurrent schema from the current model only
		public static bool DeleteCurrentSchema()
		{
			if (AppRibbon.App.Documents.Size != 1) { return false;}

			List<Schema> _subSchema = new List<Schema>(BasicSchema._basicSchemaFields[SBasicKey.COUNT].Value);

			Schema schema = Schema.Lookup(BasicSchema.SchemaGUID);

			if (schema != null)
			{
				InitRevitSettings();

				using (Transaction t = new Transaction(AppRibbon.Doc, "Delete old schema"))
				{
					t.Start();
				
					if (ReadAllRevitSettings() && _subSchema.Count > 0)
					{
						for (int i = 0; i < BasicRevitSchemaFields[SBasicKey.COUNT].Value; i++)
						{
							Schema.EraseSchemaAndAllEntities(_subSchema[i], false);
							_subSchema[i].Dispose();
						}
					}
					Schema.EraseSchemaAndAllEntities(schema, false);
					t.Commit();

				}
				schema.Dispose();
			}
			return true;
		}

		// update the schema with the current schema
		public static void UpdateRevitSettings()
		{
			ReadRevitSettings();

			DeleteCurrentSchema();

			SaveRevitSettings();
		}

		// reset the settings to their default values
		public static void ResetRevitSettings()
		{
			DeleteCurrentSchema();

			SetDefaultFields();

			SaveRevitSettings();
		}

		public static void SetDefaultFields()
		{
			BasicRevitSchemaFields = BasicSchema._basicSchemaFields.Clone();
			UnitRevitSchemaFields = GetUnitSchemaFields(BasicSchema._basicSchemaFields[SBasicKey.COUNT].Value);
		}

		// ******************************
		// save settings routines
		// ******************************

		// save the basic settings to the revit project base point
		// this saves both the basic and the unit styles
		public static bool SaveRevitSettings()
		{
			try
			{
				if (!initalized) { return false; }
				Element elem = Util.GetProjectBasepoint();

				SchemaBuilder sbld = CreateSchema(BasicSchema.SCHEMA_NAME, BasicSchema.SCHEMA_DESC, BasicSchema.SchemaGUID);

				// this makes the basic setting fields
				MakeFields(sbld, BasicRevitSchemaFields);

				// create and get the unit style schema fields
				// and then the sub-schemd (unit styles)
				Dictionary<string, string> subSchemaFields = 
					CreateUnitFields(sbld);

				// all fields created and added
				Schema schema = sbld.Finish();

				Entity entity = new Entity(schema);

				// set the basic fields
				SaveFieldValues(entity, schema, BasicRevitSchemaFields);

				SaveUnitSettings(entity, schema, subSchemaFields);

				using (Transaction t = new Transaction(AppRibbon.Doc, "Unit Style Settings"))
				{
					t.Start();
					elem.SetEntity(entity);
					t.Commit();
				}

				schema.Dispose();
			}
			catch (InvalidOperationException e)
			{
				return false;
			}

			return true;
		}

		// create the schema builder opject
		private static SchemaBuilder CreateSchema(string name, string description, Guid guid)
		{
			SchemaBuilder sbld = new SchemaBuilder(guid);

			sbld.SetReadAccessLevel(AccessLevel.Public);
			sbld.SetWriteAccessLevel(AccessLevel.Vendor);
			sbld.SetVendorId(Util.GetVendorId());
			sbld.SetSchemaName(name);
			sbld.SetDocumentation(description);

			return sbld;
		}

		// create the fields that hold the unit schemas
		private static Dictionary<string, string> CreateUnitFields(SchemaBuilder sbld)
		{
			Dictionary<string, string> subSchemaFields =
				new Dictionary<string, string>(BasicRevitSchemaFields[SBasicKey.COUNT].Value);

			// temp - test making ) unit subschemas
			for (int i = 0; i < BasicRevitSchemaFields[SBasicKey.COUNT].Value; i++)
			{
				string guid = string.Format(BasicSchema._subSchemaFieldInfo.Guid, i);   // + suffix;
				string fieldName =
					string.Format(BasicSchema._subSchemaFieldInfo.Name, i);
				FieldBuilder fbld =
					sbld.AddSimpleField(fieldName, typeof(Entity));
				fbld.SetDocumentation(BasicSchema._subSchemaFieldInfo.Desc);
				fbld.SetSubSchemaGUID(new Guid(guid));

				subSchemaFields.Add(fieldName, guid);
			}
			return subSchemaFields;
		}

		// save the settings held in the 
		private static void SaveFieldValues<T>(Entity entity, Schema schema,
			SchemaDictionaryBase<T> fieldList)
		{
			foreach (KeyValuePair<T, FieldInfo> kvp in fieldList)
			{
				Field field = schema.GetField(kvp.Value.Name);
				if (field == null || !field.IsValidObject) { continue; }

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

		private static void SaveUnitSettings(Entity entity, Schema schema, 
			Dictionary<string, string> subSchemaFields)
		{
			int j = 0;

			foreach (KeyValuePair<string, string> kvp in subSchemaFields)
			{
				Field field = schema.GetField(kvp.Key);
				if (field == null || !field.IsValidObject) { continue; }

				Entity subEntity =
					MakeUnitSchema(kvp.Value, UnitRevitSchemaFields[j++]);
				entity.Set(field, subEntity);
			}
		}

		private static void MakeFields<T>(SchemaBuilder sbld,
			SchemaDictionaryBase<T> fieldList)
		{
			foreach (KeyValuePair<T, FieldInfo> kvp in fieldList)
			{
				MakeField(sbld, kvp.Value);
			}
		}

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

		private static Entity MakeUnitSchema(string guid,
			SchemaDictionaryUnit unitSchemaFields)
		{
			SchemaBuilder sbld = CreateSchema(UnitSchema.UNIT_SCHEMA_NAME,
				UnitSchema.UNIT_SCHEMA_DESC, new Guid(guid));

			MakeFields(sbld, unitSchemaFields);

			Schema schema = sbld.Finish();

			Entity entity = new Entity(schema);

			SaveFieldValues(entity, schema, unitSchemaFields);

			return entity;
		}

		// ******************************
		// read setting routines
		// ******************************


		// routine to read the existing saved settings
		// if none exist, this saves the default settings, then
		// reads them back, and then flags that this is initalized
		public static bool ReadRevitSettings()
		{
			InitRevitSettings();

			if (!ReadAllRevitSettings())
			{
				SaveRevitSettings();

				if (ReadAllRevitSettings())
				{
					return false;
				}
			}

			return true;
		}

		// does the schema exist
		private static bool SettingsExist(out Schema schema, out Entity elemEntity)
		{
			elemEntity = null;

			schema = Schema.Lookup(BasicSchema.SchemaGUID);

			if (schema == null ||
				schema.IsValidObject == false) { return false; }

			Element elem = Util.GetProjectBasepoint();

			elemEntity = elem.GetEntity(schema);

			if (elemEntity?.Schema == null) { return false; }

			return true;
		}


		// general routine to read through a saved schema and 
		// get the value from each field 
		// this will work with any field list
		private static bool ReadAllRevitSettings()
		{
			Schema schema;
			Entity elemEntity;

			if (!SettingsExist(out schema, out elemEntity)) { return false; }

			ReadBasicRevitSettings(elemEntity, schema);

			if (!ReadRevitUnitStyles(elemEntity, schema))
			{
				return false;
			}

			schema.Dispose();

			return true;
		}

		private static void ReadBasicRevitSettings(Entity elemEntity, Schema schema)
		{
			foreach (KeyValuePair<SBasicKey, FieldInfo> kvp in BasicRevitSchemaFields)
			{
				Field field = schema.GetField(kvp.Value.Name);
				if (field == null || !field.IsValidObject) { continue; }

				kvp.Value.Value = kvp.Value.ExtractValue(elemEntity, field);
			}
		}

		// this reads through the basic fields associated with the unit style schema
		// it passes these down to the readsubentity method that then reads
		// through all of the fields in the subschema
		private static bool ReadRevitUnitStyles(Entity elemEntity, Schema schema)
		{
			List<Schema> _subSchema = new List<Schema>(BasicSchema._basicSchemaFields[SBasicKey.COUNT].Value);

			for (int i = 0; i < BasicRevitSchemaFields[SBasicKey.COUNT].Value; i++)
			{
				string subSchemaName = UnitSchema.GetSubSchemaName(i);

				Field field = schema.GetField(subSchemaName);
				if (field == null || !field.IsValidObject) { continue; }

				Entity subSchema = elemEntity.Get<Entity>(field);

				_subSchema.Add(field.SubSchema);

				if (subSchema == null || !subSchema.IsValidObject) { continue; }

				ReadSubSchema(subSchema, subSchema.Schema, UnitRevitSchemaFields[i]);
			}

			return true;
		}

		private static void ReadSubSchema(Entity subSchemaEntity, Schema schema,
			SchemaDictionaryUnit unitSchemaField)
		{
			foreach (KeyValuePair<SUnitKey, FieldInfo> kvp
				in unitSchemaField)
			{
				Field field = schema.GetField(kvp.Value.Name);
				if (field == null || !field.IsValidObject) { continue; }

				kvp.Value.Value =
					kvp.Value.ExtractValue(subSchemaEntity, field);
			}
		}

		// ******************************
		// listing routines
		// ******************************

		public static void ListFieldInfo(int count = 0)
		{
			MessageUtilities.logMsgDbLn2("basic", "settings");
			ListFieldInfo(BasicRevitSchemaFields);

			for (int i = 0; i < BasicRevitSchemaFields[SBasicKey.COUNT].Value; i++)
			{
				MessageUtilities.logMsg(MessageUtilities.nl);
				MessageUtilities.logMsgDbLn2("unit", "settings");
				ListFieldInfo(UnitRevitSchemaFields[i]);

				if (i == count) { return; }
			}
		}

		private static void ListFieldInfo<T>(SchemaDictionaryBase<T> fieldList)
		{
			int i = 0;

			foreach (KeyValuePair<T, FieldInfo> kvp in fieldList)
			{
				MessageUtilities.logMsgDbLn2("field #" + i++, kvp.Key.GetType().Name
					+ "  name| " + kvp.Value.Name
					+ "  value| " + kvp.Value.Value);
			}
		}

		private static void ListSchema()
		{
			IList<Schema> schemas = Schema.ListSchemas();
			MessageUtilities.logMsgDbLn2("number of schema found", schemas.Count.ToString());

			foreach (Schema schema in schemas)
			{
				MessageUtilities.logMsgDbLn2("schema name", schema.SchemaName + "  guid| " + schema.GUID);
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
		//		public static string ReadRevitSettings()
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
