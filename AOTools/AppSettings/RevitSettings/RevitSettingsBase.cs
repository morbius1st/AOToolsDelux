#region Using directives

using System;
using System.Collections.Generic;
using AOTools.AppSettings.SchemaSettings;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using static Autodesk.Revit.DB.ExtensibleStorage.Schema;
using InvalidOperationException = Autodesk.Revit.Exceptions.InvalidOperationException;

using static AOTools.AppSettings.RevitSettings.RevitSettingsUnitApp;
using static AOTools.AppSettings.RevitSettings.RevitSettingsUnitUsr;
using AOTools.Utility;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.UI;
using static UtilityLibrary.MessageUtilities;

using static AOTools.AppSettings.RevitSettings.RevitSettingsBase.RevitSetgDelRetnCode;

#endregion

// itemname:	RevitSettingsBase
// username:	jeffs
// created:		1/30/2018 9:10:15 PM


namespace AOTools.AppSettings.RevitSettings
{
	class RevitSettingsBase
	{
		#region Read settings routines
		// ******************************
		// read setting routines
		// ******************************

		// return true if the app schema exists
		// and is good
		private static bool AppSchemaExist(out Schema schema)
		{
			schema = Lookup(RsuApp.SchemaGuid);

			return schema != null && schema.IsValidObject;
		}

		// does the schema exist
		private static bool GetAppSchemaAndElement(out Entity elemEntity, out Schema schema)
		{
			elemEntity = null;

			if (!AppSchemaExist(out schema)) { return false; }

			//			schema = Lookup(RsuApp.SchemaGuid);
			//			if (schema == null ||
			//				schema.IsValidObject == false) { return false; }

			Element elem = Util.GetProjectBasepoint();

			elemEntity = elem.GetEntity(schema);

			if (elemEntity?.Schema == null) { return false; }

			return true;
		}


		// general routine to read through a saved schema and 
		// get the value from each field 
		// this will work with any field list
		protected bool ReadAllRevitSettings()
		{
			Schema schema;
			Entity elemEntity;

			if (!GetAppSchemaAndElement(out elemEntity, out schema)) { return false; }

			ReadRevitAppSettings(elemEntity, schema);

			if (!ReadRevitUnitStyles(elemEntity, schema))
			{
				return false;
			}

			schema.Dispose();

			return true;
		}

		// read and store the revit app settings
		private static void ReadRevitAppSettings(Entity elemEntity, Schema schema)
		{
			foreach (KeyValuePair<SchemaAppKey, SchemaFieldUnit> kvp in RsuAppSetg)
			{
				Field field = schema.GetField(kvp.Value.Name);
				if (field == null || !field.IsValidObject) { continue; }

				kvp.Value.Value = kvp.Value.ExtractValue(elemEntity, field);
			}
		}

		// get a list of schema entities that are associated
		// with sub schemas
		private static List<Entity> GetSchemaEntities(Entity elemEntity, Schema schema)
		{
			List<Entity> entityList = new List<Entity>(4);

			// make the root schema element 0;
			entityList.Add(elemEntity);

			foreach (Field f in schema.ListFields())
			{
				if (f.SubSchema == null) { continue; }

				Field field = schema.GetField(f.FieldName);
				if (field == null || !field.IsValidObject) { break; }

				Entity subSchema = elemEntity.Get<Entity>(field);
				if (subSchema == null || !subSchema.IsValidObject) { break; }

				entityList.Add(subSchema);
			}
			return entityList;
		}


		// this reads through the fields associated with the unit style schema
		// it passes these down to the readsubentity method that then reads
		// through all of the fields in the subschema
		// currently always returns true
		private bool ReadRevitUnitStyles(Entity elemEntity, Schema schema)
		{
			// provide a default list to start with - this will be populated
			// per the below
			RsuUsr.Clear();

			// element 0 is the root schema
			// subschema's start at 1
			List<Entity> schemaList = GetSchemaEntities(elemEntity, schema);

			if (schemaList.Count <= 1) { return false;}

			for (int i = 1; i < schemaList.Count; i++)
			{
				RsuUsrSetg.Add(SchemaUnitUtil.DefaultSchemaUsr(i - 1));
				ReadSubSchema(schemaList[i], schemaList[i].Schema, RsuUsrSetg[i - 1]);
			}
			return true;
		}

		private void ReadSubSchema(Entity subSchemaEntity, Schema schema,
			SchemaDictionaryUsr usrSchemaField)
		{
			foreach (KeyValuePair<SchemaUsrKey, SchemaFieldUnit> kvp
				in usrSchemaField)
			{
				Field field = schema.GetField(kvp.Value.Name);
				if (field == null || !field.IsValidObject) { continue; }

				kvp.Value.Value =
					kvp.Value.ExtractValue(subSchemaEntity, field);
			}
		}
		#endregion

		#region Save Settings routines
		// ******************************
		// save settings routines
		// ******************************

		// save the basic settings to the revit project base point
		// this saves both the basic and the unit styles
		protected SaveRtnCodes SaveAllRevitSettings()
		{
			try
			{
				Element elem = Util.GetProjectBasepoint();

				SchemaBuilder sbld = CreateSchema(SchemaName, RevitSettingsUnitApp.SchemaDesc, RsuApp.SchemaGuid);

				// this makes the basic setting fields
				MakeFields(sbld, RsuAppSetg);

				// create and get the unit style schema fields
				// and then the sub-schemd (unit styles)
				Dictionary<string, string> subSchemaFields =
					CreateUnitFields(sbld);

				// all fields created and added
				Schema schema = sbld.Finish();

				Entity entity = new Entity(schema);

				// set the basic fields
				SaveFieldValues(entity, schema, RsuAppSetg);

				SaveUnitSettings(entity, schema, subSchemaFields);

				using (Transaction t = new Transaction(AppRibbon.Doc, "Unit Style Settings"))
				{
					t.Start();
					elem.SetEntity(entity);
					t.Commit();
				}

				schema.Dispose();
			}
			catch (InvalidOperationException ex)
			{
				if (ex.HResult == -2146233088)
				{
					logMsgDbLn2("schema", "duplicate - not saved");
					return SaveRtnCodes.DUPLICATE;
				}

				return SaveRtnCodes.FAIL;
			}
			return SaveRtnCodes.GOOD;
		}

		public enum SaveRtnCodes
		{
			DUPLICATE = -1,
			FAIL = 0,
			GOOD = 1

		}

		// create the schema builder opject
		// ReSharper disable once MemberCanBeMadeStatic.Local
		private SchemaBuilder CreateSchema(string name, string description, Guid guid)
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
		private Dictionary<string, string> CreateUnitFields(SchemaBuilder sbld)
		{
			Dictionary<string, string> subSchemaFields =
				new Dictionary<string, string>(RsuUsr.Count);

			// temp - test making ) unit subschemas
			for (int i = 0; i < RsuUsr.Count; i++)
			{
				string guid = String.Format(SchemaUnitUtil.SubSchemaFieldInfo.Guid, i);   // + suffix;
				string fieldName =
					String.Format(SchemaUnitUtil.SubSchemaFieldInfo.Name, i);
				FieldBuilder fbld =
					sbld.AddSimpleField(fieldName, typeof(Entity));
				fbld.SetDocumentation(SchemaUnitUtil.SubSchemaFieldInfo.Desc);
				fbld.SetSubSchemaGUID(new Guid(guid));

				subSchemaFields.Add(fieldName, guid);
			}
			return subSchemaFields;
		}

		// save the settings held in the 
		private void SaveFieldValues<T>(Entity entity, Schema schema,
			SchemaDictionaryBase<T> fieldList)
		{
			foreach (KeyValuePair<T, SchemaFieldUnit> kvp in fieldList)
			{
				Field field = schema.GetField(kvp.Value.Name);
				if (field == null || !field.IsValidObject) { continue; }

				if (kvp.Value.UnitType != RevitUnitType.UT_UNDEFINED)
				{
					entity.Set(field, kvp.Value.Value, DisplayUnitType.DUT_GENERAL);
				}
				else
				{
					entity.Set(field, kvp.Value.Value);
				}
			}
		}

		private void SaveUnitSettings(Entity entity, Schema schema,
			Dictionary<string, string> subSchemaFields)
		{
			int j = 0;

			foreach (KeyValuePair<string, string> kvp in subSchemaFields)
			{
				Field field = schema.GetField(kvp.Key);
				if (field == null || !field.IsValidObject) { continue; }

				Entity subEntity =
					MakeUnitSchema(kvp.Value,RsuUsrSetg[j++]);
				entity.Set(field, subEntity);
			}
		}

		private void MakeFields<T>(SchemaBuilder sbld,
			SchemaDictionaryBase<T> fieldList)
		{
			foreach (KeyValuePair<T, SchemaFieldUnit> kvp in fieldList)
			{
				MakeField(sbld, kvp.Value);
			}
		}

		private void MakeField(SchemaBuilder sbld, SchemaFieldUnit schemaFieldUnit)
		{
			FieldBuilder fbld = sbld.AddSimpleField(
					schemaFieldUnit.Name, schemaFieldUnit.Value.GetType());

			fbld.SetDocumentation(schemaFieldUnit.Desc);

			if (schemaFieldUnit.UnitType != RevitUnitType.UT_UNDEFINED)
			{
				fbld.SetUnitType((UnitType) (int) schemaFieldUnit.UnitType);
			}
		}

		private Entity MakeUnitSchema(string guid,
			SchemaDictionaryUsr usrSchemaFields)
		{
			SchemaBuilder sbld = CreateSchema(UnitSchemaName,
				RevitSettingsUnitUsr.SchemaDesc, new Guid(guid));

			MakeFields(sbld, usrSchemaFields);

			Schema schema = sbld.Finish();

			Entity entity = new Entity(schema);

			SaveFieldValues(entity, schema, usrSchemaFields);

			return entity;
		}
		#endregion

		#region Delete settings routines
		// ******************************
		// delete routines
		// ******************************
		public enum RevitSetgDelRetnCode
		{
			DELETE_SETTINGS_WORKED = 0,
			TOO_MANY_DOCUMENTS = -1,
			EXISTING_SCHEMA_NOT_FOUND = -2
		}

		protected RevitSetgDelRetnCode DeleteAllSchemas()
		{
			Application a = AppRibbon.App;
			DocumentSet d = a.Documents;

			// must be only one active document
			if (AppRibbon.App.Documents.Size != 1) { return TOO_MANY_DOCUMENTS; }

			List<Entity> Entities = GetEntities();

			if (Entities == null) { return EXISTING_SCHEMA_NOT_FOUND; }

			using (Transaction t = new Transaction(AppRibbon.Doc, "Delete Unit Styles"))
			{
				t.Start();

				// remove the sub-schema's first
				if (Entities.Count > 1)
				{
					for (int i = 1; i < Entities.Count; i++)
					{
						EraseSchemaAndAllEntities(Entities[i].Schema, false);
						Entities[i].Dispose();
					}
				}
				// remove the root schema laast
				EraseSchemaAndAllEntities(Entities[0].Schema, false);
				Entities[0].Dispose();

				t.Commit();
			}
			return DELETE_SETTINGS_WORKED;
		}

		public void DeleteTooManyDocumentsMsg(string from)
		{
			TaskDialog td = new TaskDialog("AO Tools");
			td.MainInstruction = "Cannot perform operation: " + nl 
				+ from + nl
				+ "There are other documents open." + nl
				+ "Please close all other documents and" + nl
				+ "try the operation again";
			td.MainIcon = TaskDialogIcon.TaskDialogIconWarning;
			td.CommonButtons = TaskDialogCommonButtons.Ok;

			td.Show();
		}

		public bool ChkDelRetnCode(RevitSetgDelRetnCode code, string from)
		{
			if (code != DELETE_SETTINGS_WORKED)
			{
				if (code == TOO_MANY_DOCUMENTS)
				{
					DeleteTooManyDocumentsMsg(from);
				}

				return false;
			}

			return true;
		}

		// get a list of revit entities that are associated
		// with subschema's
		protected List<Entity> GetEntities()
		{
			Schema schema;
			Entity elemEntity;

			if (!GetAppSchemaAndElement(out elemEntity, out schema)) { return null; }

			return GetSchemaEntities(elemEntity, schema);
		}

		// get a list of revit schema's that 
		// are subschemas
		public static List<Schema> GetSchemas()
		{
			Schema schema;
			Entity elemEntity;

			if (!GetAppSchemaAndElement(out elemEntity, out schema)) { return null; }

			return GetSubSchemas(elemEntity, schema);
		}

		// get a list of revit schema's that 
		// are subschemas
		private static List<Schema> GetSubSchemas(Entity elemEntity, Schema schema)
		{
			List<Schema> schemaList = new List<Schema>(4);

			// make the root schema element 0;
			schemaList.Add(schema);

			foreach (Field f in schema.ListFields())
			{
				if (f.SubSchema == null) { continue; }

				Field field = schema.GetField(f.FieldName);
				if (field == null || !field.IsValidObject) { break; }

				Entity subSchema = elemEntity.Get<Entity>(field);
				if (subSchema == null || !subSchema.IsValidObject) { break; }

				schemaList.Add(subSchema.Schema);
			}
			return schemaList;
		}

		#endregion

		#region Listing Routines
		// ******************************
		// listing routines
		// ******************************

		public static void ListRevitSettingInfo(int count = -1)
		{
			logMsgDbLn2("basic", "settings");

			SchemaUnitListing.ListFieldInfo(RsuAppSetg);
			logMsg("");

			foreach (SchemaDictionaryUsr unitStyle in RsuUsrSetg)
			{
				logMsgDbLn2("unit", "settings");
				SchemaUnitListing.ListFieldInfo(unitStyle, count);
				logMsg("");
			}
		}

		public static void ListRevitSchema()
		{
			IList<Schema> schemas = ListSchemas();
			logMsgDbLn2("number of schema found", schemas.Count.ToString());

			foreach (Schema schema in schemas)
			{
				logMsgDbLn2("schema name", schema.SchemaName + "  guid| " + schema.GUID);
			}
		}

		#endregion
	}
}
