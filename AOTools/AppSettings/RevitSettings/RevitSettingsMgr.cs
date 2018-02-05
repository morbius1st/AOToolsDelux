#region Using directives

using System.Collections.Generic;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using static Autodesk.Revit.DB.ExtensibleStorage.Schema;

using AOTools.AppSettings.SchemaSettings;
using static AOTools.AppSettings.RevitSettings.RevitSettingsUnitApp;
using static AOTools.AppSettings.RevitSettings.RevitSettingsUnitUsr;
using static AOTools.AppSettings.RevitSettings.RevitSettingsMgr.RevitSettingDeleteReturnCode;

#endregion

// itemname:	RevitSettingsMgr
// username:	jeffs
// created:		1/7/2018 3:37:43 PM


namespace AOTools.AppSettings.RevitSettings
{
	internal class RevitSettingsMgr : RevitSettingsBase
	{
		public static readonly RevitSettingsMgr RsMgr = new RevitSettingsMgr();

		// ******************************
		// read setting
		// ******************************
		public bool Read()
		{
			if (!ReadAllRevitSettings())
			{
				Save();
			}

			return ReadAllRevitSettings();
		}

		// ******************************
		// save settings
		// ******************************
		public bool Save()
		{
			return SaveAllRevitSettings();
		}

		// ******************************
		// delete schema from revit document
		// ******************************

		public enum RevitSettingDeleteReturnCode
		{
			DELETE_SETTINGS_WORKED = 0,
			TOO_MANY_DOCUMENTS = -1,
			EXISTING_SCHEMA_NOT_FOUND = -2
		}

		// delete thecurrent schema from the current model only
		public RevitSettingDeleteReturnCode DeleteSchema()
		{
			// must be only one active document
			if (AppRibbon.App.Documents.Size != 1) { return TOO_MANY_DOCUMENTS; }

			List<Schema> schemas = GetSchemas();

			if (schemas == null) { return EXISTING_SCHEMA_NOT_FOUND; }

			using (Transaction t = new Transaction(AppRibbon.Doc, "Delete Unit Styles"))
			{
				t.Start();

				// remove the sub-schema's first
				if (schemas.Count > 1)
				{
					for (int i = 1; i < schemas.Count; i++)
					{ 
						EraseSchemaAndAllEntities(schemas[i], false);
						schemas[i].Dispose();
					}
				}
				// remove the root schema laast
				EraseSchemaAndAllEntities(schemas[0], false);
				schemas[0].Dispose();

				t.Commit();
			}

			return DELETE_SETTINGS_WORKED;
		}

		private List<Schema> GetSchemas()
		{
			Schema schema;
			Entity elemEntity;

			if (!GetAppSchemaAndElement(out schema, out elemEntity)) { return null; }

			return GetSubSchemas(elemEntity, schema);
		}

		private List<Schema> GetSubSchemas(Entity elemEntity, Schema schema)
		{
			List <Schema> schemaList = new List<Schema>(4);

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


		// ******************************
		// update settings
		// ******************************
		// update the schema with the current schema
		public void Update()
		{
			Read();

			DeleteSchema();

			Save();
		}

		// ******************************
		// reset settings
		// ******************************
		// reset the settings to their default values
		public void Reset()
		{
			DeleteSchema();

			Save();
		}



	}
}
