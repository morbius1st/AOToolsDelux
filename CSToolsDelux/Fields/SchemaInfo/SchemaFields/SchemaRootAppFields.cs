#region + Using Directives

using System;
using CSToolsDelux.Fields.SchemaInfo.SchemaDefinitions;
using static CSToolsDelux.Fields.SchemaInfo.SchemaDefinitions.SchemaRootAppKey;
#endregion

// user name: jeffs
// created:   7/3/2021 10:48:37 PM

namespace CSToolsDelux.Fields.SchemaInfo.SchemaFields
{
	public class SchemaRootAppFields : ASchemaFieldsRootApp
	{
		public const string RA_SCHEMA_NAME = "CellsRootAppSchema";
		public const string RA_SCHEMA_DESC = "Excel Cells to Revit Exchange";
		public const string RA_ROOT_DEVELOPER_NAME = "CyberStudio";
		public const string RA_SCHEMA_VER = "0.1";

		public SchemaRootAppFields()
		{
			defineFields();
		}

		public override SchemaRootAppKey[] KeyOrder { get; set; }

		public ISchemaFieldDef<SchemaRootAppKey> this[SchemaRootAppKey key] => Fields[key];

		public SchemaFieldRootApp<TD> GetField<TD>(SchemaRootAppKey key)
		{
			return (SchemaFieldRootApp<TD>) Fields[key];
		}

		public TD GetValue<TD>(SchemaRootAppKey key)
		{
			return ((SchemaFieldRootApp<TD>) Fields[key]).Value;
		}

		public void SetValue<TD>(SchemaRootAppKey key, TD value)
		{
			((SchemaFieldRootApp<TD>) Fields[key]).Value = value;
		}

		private void defineFields()
		{
			Fields = new SchemaDictionaryRootApp();

			KeyOrder = new SchemaRootAppKey[Enum.GetNames(typeof(SchemaRootAppKey)).Length];
			int idx = 0;

			KeyOrder[idx++] =
				defineField<string>(RAK_NAME, "Name", "Name", RA_SCHEMA_NAME );

			KeyOrder[idx++] =
				defineField<string>(RAK_DESCRIPTION, "Description", "Description", RA_SCHEMA_DESC );

			KeyOrder[idx++] =
				defineField<string>(RAK_VERSION, "Version", "Cells Version", RA_SCHEMA_VER );

			KeyOrder[idx++] =
				defineField<string>(RAK_DEVELOPER, "Developer", "Developer", RA_ROOT_DEVELOPER_NAME );

			KeyOrder[idx++] =
				defineField<string>(RAK_CREATE_DATE, "CreationData", "Date and Time Created", DateTime.UtcNow.ToString());
			//
			// KeyOrder[idx++] =
			// 	defineField<string>(RAK_APP_GUID, "AppGuidString", "App Guid String", Guid.Empty.ToString());

		}

		// the guid for each sub-schema and the 
		// field that holds the sub-schema - both must match
		// the guid here is missing the last (2) digits.
		// fill in for each sub-schema 
		// unit type is number is a filler
		// private static SchemaFieldDef<SchemaAppKey> SubSchemaFieldInfo { get; } =
		// 	new SchemaFieldDef<SchemaAppKey>(AK_UNDEFINED, "RootCellsDefinition{0:D2}",
		// 		"subschema for a cells definition", "");
		//
		// public static SchemaFieldDef<SchemaAppKey> GetSubSchemaDef(int id)
		// {
		// 	SchemaFieldDef<SchemaAppKey> subDef = new SchemaFieldDef<SchemaAppKey>();
		// 	subDef.Sequence = SubSchemaFieldInfo.Sequence;
		// 	subDef.Name = string.Format(SubSchemaFieldInfo.Name, id);
		// 	subDef.Desc = SubSchemaFieldInfo.Desc;
		// 	subDef.Guid = SchemaGuidManager.GetCellGuidString(id);
		//
		// 	return subDef;
		// }
	}
}