#region Using directives

using System;
using System.Collections.Generic;
using AOTools.AppSettings.Schema;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using UtilityLibrary;
using InvalidOperationException = Autodesk.Revit.Exceptions.InvalidOperationException;

#endregion

// itemname:	RevitSettingsBase
// username:	jeffs
// created:		1/30/2018 9:10:15 PM


namespace AOTools.AppSettings.RevitSettings
{
	internal class RevitSettingsBase
	{
		// ******************************
		// save settings routines
		// ******************************

		// save the basic settings to the revit project base point
		// this saves both the basic and the unit styles
		protected bool SaveAllRevitSettings()
		{
			try
			{
				Element elem = Util.GetProjectBasepoint();

				SchemaBuilder sbld = CreateSchema(RevitSettingsUnitApp.RsuApp.SchemaName, RevitSettingsUnitApp.RsuApp.SchemaDesc, RevitSettingsUnitApp.RsuApp.SchemaGuid);

				// this makes the basic setting fields
				MakeFields(sbld, RevitSettingsUnitApp.RsuApp.RsuAppSetg);

				// create and get the unit style schema fields
				// and then the sub-schemd (unit styles)
				Dictionary<string, string> subSchemaFields =
					CreateUnitFields(sbld);

				// all fields created and added
				Autodesk.Revit.DB.ExtensibleStorage.Schema schema = sbld.Finish();

				Entity entity = new Entity(schema);

				// set the basic fields
				SaveFieldValues(entity, schema, RevitSettingsUnitApp.RsuApp.RsuAppSetg);

				SaveUnitSettings(entity, schema, subSchemaFields);

				using (Transaction t = new Transaction(AppRibbon.Doc, "Unit Style Settings"))
				{
					t.Start();
					elem.SetEntity(entity);
					t.Commit();
				}

				schema.Dispose();
			}
			catch (InvalidOperationException)
			{
				return false;
			}
			return true;
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
				new Dictionary<string, string>(RevitSettingsUnitApp.RsuApp.RsuAppSetg[SchemaAppKey.COUNT].Value);

			// temp - test making ) unit subschemas
			for (int i = 0; i < RevitSettingsUnitApp.RsuApp.RsuAppSetg[SchemaAppKey.COUNT].Value; i++)
			{
				string guid = string.Format(SchemaUnitApp.SubSchemaFieldInfo.Guid, i);   // + suffix;
				string fieldName =
					string.Format(SchemaUnitApp.SubSchemaFieldInfo.Name, i);
				FieldBuilder fbld =
					sbld.AddSimpleField(fieldName, typeof(Entity));
				fbld.SetDocumentation(SchemaUnitApp.SubSchemaFieldInfo.Desc);
				fbld.SetSubSchemaGUID(new Guid(guid));

				subSchemaFields.Add(fieldName, guid);
			}
			return subSchemaFields;
		}

		// save the settings held in the 
		private void SaveFieldValues<T>(Entity entity, Autodesk.Revit.DB.ExtensibleStorage.Schema schema,
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

		private void SaveUnitSettings(Entity entity, Autodesk.Revit.DB.ExtensibleStorage.Schema schema,
			Dictionary<string, string> subSchemaFields)
		{
			int j = 0;

			foreach (KeyValuePair<string, string> kvp in subSchemaFields)
			{
				Field field = schema.GetField(kvp.Key);
				if (field == null || !field.IsValidObject) { continue; }

				Entity subEntity =
					MakeUnitSchema(kvp.Value, RevitSettingsUnitUsr.RsuUsr.RsuUsrSetg[j++]);
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
			SchemaBuilder sbld = CreateSchema(RevitSettingsUnitUsr.RsuUsr.UnitSchemaName,
				RevitSettingsUnitUsr.RsuUsr.SchemaDesc, new Guid(guid));

			MakeFields(sbld, usrSchemaFields);

			Autodesk.Revit.DB.ExtensibleStorage.Schema schema = sbld.Finish();

			Entity entity = new Entity(schema);

			SaveFieldValues(entity, schema, usrSchemaFields);

			return entity;
		}

		// ******************************
		// read setting routines
		// ******************************

		// does the schema exist
		private bool SettingsExist(out Autodesk.Revit.DB.ExtensibleStorage.Schema schema, out Entity elemEntity)
		{
			elemEntity = null;

			schema = Autodesk.Revit.DB.ExtensibleStorage.Schema.Lookup(RevitSettingsUnitApp.RsuApp.SchemaGuid);

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
		protected bool ReadAllRevitSettings()
		{
			Autodesk.Revit.DB.ExtensibleStorage.Schema schema;
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

		private void ReadBasicRevitSettings(Entity elemEntity, Autodesk.Revit.DB.ExtensibleStorage.Schema schema)
		{
			foreach (KeyValuePair<SchemaAppKey, SchemaFieldUnit> kvp in RevitSettingsUnitApp.RsuApp.RsuAppSetg)
			{
				Field field = schema.GetField(kvp.Value.Name);
				if (field == null || !field.IsValidObject) { continue; }

				kvp.Value.Value = kvp.Value.ExtractValue(elemEntity, field);
			}
		}

		// this reads through the basic fields associated with the unit style schema
		// it passes these down to the readsubentity method that then reads
		// through all of the fields in the subschema
		private bool ReadRevitUnitStyles(Entity elemEntity, Autodesk.Revit.DB.ExtensibleStorage.Schema schema)
		{
			// adjust the list based on the actual size
			RevitSettingsUnitUsr.RsuUsr.Resize(RevitSettingsUnitApp.RsuApp.RsuAppSetg[SchemaAppKey.COUNT].Value);

			for (int i = 0; i < RevitSettingsUnitApp.RsuApp.RsuAppSetg[SchemaAppKey.COUNT].Value; i++)
			{
				string subSchemaName = RevitSettingsUnitApp.RsuApp.GetSubSchemaName(i);

				Field field = schema.GetField(subSchemaName);
				if (field == null || !field.IsValidObject) { continue; }

				Entity subSchema = elemEntity.Get<Entity>(field);

				if (subSchema == null || !subSchema.IsValidObject) { continue; }

				ReadSubSchema(subSchema, subSchema.Schema, RevitSettingsUnitUsr.RsuUsr.RsuUsrSetg[i]);
			}

			return true;
		}

		private void ReadSubSchema(Entity subSchemaEntity, Autodesk.Revit.DB.ExtensibleStorage.Schema schema,
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

		// ******************************
		// listing routines
		// ******************************

		public static void ListRevitSettingInfo(int count = -1)
		{
			MessageUtilities.logMsgDbLn2("basic", "settings");

			SchemaUnitUtil.ListFieldInfo(RevitSettingsUnitApp.RsuApp.RsuAppSetg);
			MessageUtilities.logMsg("");

			for (int i = 0; i < RevitSettingsUnitApp.RsuApp.RsuAppSetg[SchemaAppKey.COUNT].Value; i++)
			{
				MessageUtilities.logMsgDbLn2("unit", "settings");
				SchemaUnitUtil.ListFieldInfo(RevitSettingsUnitUsr.RsuUsr.RsuUsrSetg[i], count);
				MessageUtilities.logMsg("");
			}
		}

		public void ListRevitSchema()
		{
			IList<Autodesk.Revit.DB.ExtensibleStorage.Schema> schemas = Autodesk.Revit.DB.ExtensibleStorage.Schema.ListSchemas();
			MessageUtilities.logMsgDbLn2("number of schema found", schemas.Count.ToString());

			foreach (Autodesk.Revit.DB.ExtensibleStorage.Schema schema in schemas)
			{
				MessageUtilities.logMsgDbLn2("schema name", schema.SchemaName + "  guid| " + schema.GUID);
			}
		}
	}
}
