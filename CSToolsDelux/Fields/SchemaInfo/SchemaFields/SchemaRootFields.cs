using System;
using CSToolsDelux.Fields.SchemaInfo.SchemaDefinitions;
using static CSToolsDelux.Fields.SchemaInfo.SchemaDefinitions.SchemaRootKey;

// user name: jeffs
// created:   7/3/2021 10:48:37 PM

namespace CSToolsDelux.Fields.SchemaInfo.SchemaFields
{
	public class SchemaRootFields : ASchemaFieldsRoot
	{
		public const string ROOT_SCHEMA_NAME = "CellsAppRoot";
		public const string ROOT_SCHEMA_DESC = "Excel Cells to Revit Exchange";
		public const string ROOT_DEVELOPER_NAME = "CyberStudio";
		public const string ROOT_SCHEMA_VER = "0.1";

		public SchemaRootFields()
		{
			defineFields();

			// ISchemaFieldDef<SchemaRootKey> a = Fields[RK_NAME];
		}

		public override SchemaRootKey[] KeyOrder { get; set; }

		public ISchemaFieldDef<SchemaRootKey> this[SchemaRootKey key] => Fields[key];

		public SchemaFieldRoot<TD> GetField<TD>(SchemaRootKey key)
		{
			return (SchemaFieldRoot<TD>) Fields[key];
		}

		public TD GetValue<TD>(SchemaRootKey key)
		{
			return ((SchemaFieldRoot<TD>) Fields[key]).Value;
		}

		public void SetValue<TD>(SchemaRootKey key, TD value)
		{
			((SchemaFieldRoot<TD>) Fields[key]).Value = value;
		}

		public string AppGuidField
		{
			get => GetField<string>(RK_APP_GUID).Value;
			set { ((SchemaFieldRoot<string>) Fields[RK_APP_GUID]).Value = value; }
		}

		public string CreationField
		{
			get => GetField<string>(RK_CREATION).Value;
			set { ((SchemaFieldRoot<string>) Fields[RK_CREATION]).Value = value; }
		}

		private void defineFields()
		{
			Fields = new SchemaDictionaryRoot();

			KeyOrder = new SchemaRootKey[(int) RK_COUNT];

			int idx = 0;

			// these fields define the schema which defines the data file

			// the saved name - equals App Preface code (e.g. "Cells" or "Fields" 
			// and the name of the associated revit drawing
			// this will be used to locate the correct data file
			KeyOrder[idx++] =
				defineField<string>(RK_NAME, "Name", "Name", ROOT_SCHEMA_NAME);

			// generic description
			KeyOrder[idx++] =
				defineField<string>(RK_DESCRIPTION, "Description", "Description", ROOT_SCHEMA_DESC);

			// system version
			KeyOrder[idx++] =
				defineField<string>(RK_VERSION, "Version", "Cells Version", ROOT_SCHEMA_VER);

			// name of developer
			KeyOrder[idx++] =
				defineField<string>(RK_DEVELOPER, "Developer", "Developer", ROOT_DEVELOPER_NAME);

			// guid string of the app used to access / modify this
			// why - to help tie the data file to the unique schema
			KeyOrder[idx++] =
				defineField<string>(RK_APP_GUID, "AppGuidString", "App Guid String", Guid.Empty.ToString());

			// creation data
			KeyOrder[idx++] =
				defineField<string>(RK_CREATION, "CreationData", "Date and Time Created", DateTime.UtcNow.ToString());
		}
	}
}