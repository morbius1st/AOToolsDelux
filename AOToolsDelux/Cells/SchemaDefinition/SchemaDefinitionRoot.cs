#region + Using Directives

using System;
using AOTools.Cells.ExStorage;
using static AOTools.Cells.SchemaDefinition.SchemaRootKey;
#endregion

// user name: jeffs
// created:   7/3/2021 10:48:37 PM

namespace AOTools.Cells.SchemaDefinition
{

	public class SchemaDefinitionRoot : ASchemaDef<SchemaRootKey, SchemaDictionaryRoot>
	{
		public const string ROOT_SCHEMA_NAME = "CellsAppRoot";
		public const string ROOT_SCHEMA_DESC = "Excel Cells to Revit Exchange";
		public const string ROOT_DEVELOPER_NAME = "CyberStudio";

		// public override SchemaRootKey[] KeyOrder { get; set; }

		private static readonly Lazy<SchemaDefinitionRoot> instance =
			new Lazy<SchemaDefinitionRoot>(() => new SchemaDefinitionRoot());

		private SchemaDefinitionRoot()
		{
			DefineFields();
		}

		public static SchemaDefinitionRoot Instance => instance.Value;

		private void DefineFields()
		{
			Fields = new SchemaDictionaryRoot();

			KeyOrder = new SchemaRootKey[Enum.GetNames(typeof(SchemaRootKey)).Length];
			int idx = 0;

			KeyOrder[idx++] =
				defineField<string>(NAME, "Name", "Name", 
					new ExData<string>(ROOT_SCHEMA_NAME));
			
			KeyOrder[idx++] =
				defineField<string>(DESCRIPTION, "Description", "Description",
					new ExData<string>(ROOT_SCHEMA_DESC));
			
			KeyOrder[idx++] =
				defineField<string>(VERSION, "Version", "Cells Version",
					new ExData<string>("1.0"));
			
			KeyOrder[idx++] =
				defineField<string>(DEVELOPER,"Developer", "Developer",
					new ExData<string>(ROOT_DEVELOPER_NAME));
			
			KeyOrder[idx++] =
				defineField<string>(APP_GUID, "UniqueAppGuidString", 
					"Unique App Guid String",
					new ExData<string>(SchemaGuidManager.AppGuidUniqueString));

			KeyOrder[idx++] =
				defineField<int>(UNDEFINED, "UniqueAppGuidString", 
					"Unique App Guid String",
					new ExData<int>(10));


		}

		// public override SchemaDictionaryRoot DefaultFields { get; } =
		// 	new SchemaDictionaryRoot
		// 	{
		// 		{
		// 			NAME,
		// 			new SchemaFieldDef<SchemaRootKey>(NAME, "Name",
		// 				"Name", ROOT_SCHEMA_NAME)
		// 		},
		//
		// 		{
		// 			DESCRIPTION,
		// 			new SchemaFieldDef<SchemaRootKey>(DESCRIPTION, "Description",
		// 				"Description", ROOT_SCHEMA_DESC)
		// 		},
		//
		// 		{
		// 			VERSION,
		// 			new SchemaFieldDef<SchemaRootKey>(VERSION, "Version",
		// 				"Cells Version", "1.0")
		// 		},
		//
		// 		{
		// 			DEVELOPER,
		// 			new SchemaFieldDef<SchemaRootKey>(DEVELOPER, "Developer",
		// 				"Developer", ROOT_DEVELOPER_NAME)
		// 		},
		//
		// 		{
		// 			APP_GUID,
		// 			new SchemaFieldDef<SchemaRootKey>(APP_GUID, "UniqueAppGuidString",
		// 				"Unique App Guid String", SchemaGuidManager.AppGuidUniqueString)
		// 		},
		// 	};

	}
}