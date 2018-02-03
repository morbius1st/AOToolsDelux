#region Using directives

using System.Collections.Generic;
using AOTools.AppSettings.Schema;
using Autodesk.Revit.DB;

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
		// delete thecurrent schema from the current model only
		public bool DeleteSchema()
		{
			if (AppRibbon.App.Documents.Size != 1) { return false;}

			// allocate subSchema and make sure not null
			List<Autodesk.Revit.DB.ExtensibleStorage.Schema> subSchema = 
				new List<Autodesk.Revit.DB.ExtensibleStorage.Schema>(RevitSettingsUnitApp.RsuApp.DefAppSchema[SchemaAppKey.COUNT].Value);

			Autodesk.Revit.DB.ExtensibleStorage.Schema schema = Autodesk.Revit.DB.ExtensibleStorage.Schema.Lookup(RevitSettingsUnitApp.RsuApp.SchemaGuid);

			if (schema != null)
			{
				using (Transaction t = new Transaction(AppRibbon.Doc, "Delete old schema"))
				{
					t.Start();
				
					if (ReadAllRevitSettings() && subSchema.Count > 0)
					{
						for (int i = 0; i < RevitSettingsUnitApp.RsuApp.RsuAppSetg[SchemaAppKey.COUNT].Value; i++)
						{
							Autodesk.Revit.DB.ExtensibleStorage.Schema.EraseSchemaAndAllEntities(subSchema[i], false);
							subSchema[i].Dispose();
						}
					}
					Autodesk.Revit.DB.ExtensibleStorage.Schema.EraseSchemaAndAllEntities(schema, false);
					t.Commit();

				}
				schema.Dispose();
			}

			return true;
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
