#region + Using Directives

using System;
using AOTools.Cells.SchemaDefinition;
using AOTools.Cells.SchemaDefinition2;
using static AOTools.Cells.SchemaDefinition.SchemaRootKey;
#endregion

// user name: jeffs
// created:   7/3/2021 10:48:37 PM

namespace AOTools.Cells.SchemaDefinition2
{
	public class SchemaDefinitionRoot : ASchemaDef2<SchemaRootKey, SchemaDictionaryRoot2>
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

		// public override SchemaRootKey[] KeyOrder { get; set; }

		private void defineFields()
		{
			Fields = new SchemaDictionaryRoot2();

			KeyOrder = new SchemaRootKey[Enum.GetNames(typeof(SchemaRootKey)).Length];
			int idx = 0;

			KeyOrder[idx++] =
				defineField(NAME, "Name", "Name", ROOT_SCHEMA_NAME);
			
			KeyOrder[idx++] =
				defineField(DESCRIPTION, "Description", "Description", ROOT_SCHEMA_DESC);
			
			KeyOrder[idx++] =
				defineField(VERSION, "Version", "Cells Version", ROOT_SCHEMA_VER);
			
			KeyOrder[idx++] =
				defineField(DEVELOPER,"Developer", "Developer", ROOT_DEVELOPER_NAME);
			
			KeyOrder[idx++] =
				defineField(APP_GUID, "UniqueAppGuidString", "Unique App Guid String", "" );
		}
		
	}
}