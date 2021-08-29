using System;
using CSToolsDelux.Fields.SchemaInfo.SchemaDefinitions;
using static CSToolsDelux.Fields.SchemaInfo.SchemaDefinitions.SchemaRootKey;

// user name: jeffs
// created:   7/3/2021 10:48:37 PM

namespace CSToolsDelux.Fields.SchemaInfo.SchemaFields
{
	public class SchemaFieldsRoot : ASchemaFieldsRoot
	{
		public const string ROOT_SCHEMA_NAME = "CellsAppRoot";
		public const string ROOT_SCHEMA_DESC = "Excel Cells to Revit Exchange";
		public const string ROOT_DEVELOPER_NAME = "CyberStudio";
		public const string ROOT_SCHEMA_VER = "0.1";

		public SchemaFieldsRoot()
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

		public SchemaFieldRoot<string> NameField => GetField<string>(RK_NAME);

		private void defineFields()
		{
			Fields = new SchemaDictionaryRoot();

			KeyOrder = new SchemaRootKey[(int) RK_COUNT];

			int idx = 0;

			KeyOrder[idx++] =
				defineField<string>(RK_NAME, "Name", "Name", ROOT_SCHEMA_NAME);

			KeyOrder[idx++] =
				defineField<string>(RK_DESCRIPTION, "Description", "Description", ROOT_SCHEMA_DESC);

			KeyOrder[idx++] =
				defineField<string>(RK_VERSION, "Version", "Cells Version", ROOT_SCHEMA_VER);

			KeyOrder[idx++] =
				defineField<string>(RK_DEVELOPER, "Developer", "Developer", ROOT_DEVELOPER_NAME);

			KeyOrder[idx++] =
				defineField<string>(RK_APP_GUID, "UniqueAppGuidString", "Unique App Guid String", "");

			KeyOrder[idx++] =
				defineField<string>(RK_CREATION, "CreationData", "Date and Time Created", "");
		}
	}
}