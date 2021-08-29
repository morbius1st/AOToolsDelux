#region + Using Directives

using System;
using static AOTools.Cells.SchemaDefinition.SchemaRootKey;
#endregion

// user name: jeffs
// created:   7/3/2021 10:48:37 PM

namespace AOTools.Cells.SchemaDefinition
{
	public class SchemaDefinitionRoot : ASchemaDef<SchemaRootKey, SchemaDictionaryRoot>
	{
		private static readonly Lazy<SchemaDefinitionRoot> instance =
			new Lazy<SchemaDefinitionRoot>(() => new SchemaDefinitionRoot());

		public const string ROOT_SCHEMA_NAME = "CellsAppRoot";
		public const string ROOT_SCHEMA_DESC = "Excel Cells to Revit Exchange";
		public const string ROOT_DEVELOPER_NAME = "CyberStudio";
		public const string ROOT_SCHEMA_VER = "0.1";

		private SchemaDefinitionRoot()
		{
			defineFields();
		}

		public static SchemaDefinitionRoot Instance => instance.Value;

		public override SchemaRootKey[] KeyOrder { get; set; }

		private void defineFields()
		{
			Fields = new SchemaDictionaryRoot();

			KeyOrder = new SchemaRootKey[Enum.GetNames(typeof(SchemaRootKey)).Length];
			int idx = 0;

			KeyOrder[idx++] =
				defineField<string>(RK_NAME, "Name", "Name", ROOT_SCHEMA_NAME);
			
			KeyOrder[idx++] =
				defineField<string>(RK_DESCRIPTION, "Description", "Description", ROOT_SCHEMA_DESC);
			
			KeyOrder[idx++] =
				defineField<string>(RK_VERSION, "Version", "Cells Version", ROOT_SCHEMA_VER);
			
			KeyOrder[idx++] =
				defineField<string>(RK_DEVELOPER,"Developer", "Developer", ROOT_DEVELOPER_NAME);

			KeyOrder[idx++] =
				defineField<string>(RK_APP_GUID, "UniqueAppGuidString", "Unique App Guid String", "" );

		}
		
	}
}