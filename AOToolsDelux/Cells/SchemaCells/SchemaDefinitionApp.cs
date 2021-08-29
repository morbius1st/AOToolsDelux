#region + Using Directives

using System;
using AOTools.Cells.SchemaDefinition;
using static AOTools.Cells.SchemaDefinition.SchemaAppKey;

#endregion

// user name: jeffs
// created:   7/3/2021 10:48:37 PM

namespace AOTools.Cells.SchemaCells
{
	public class SchemaDefinitionApp : ASchemaDef<SchemaAppKey, SchemaDictionaryApp>
	{
		private static readonly Lazy<SchemaDefinitionApp> instance =
			new Lazy<SchemaDefinitionApp>(() => new SchemaDefinitionApp());

		public const string SCHEMA_NAME = "CellsAppData";
		public const string SCHEMA_DESC = "Excel Cells to Revit Exchange";
		public const string SCHEMA_VER = "0.1";

		private SchemaDefinitionApp()
		{
			defineFields();
		}

		public static SchemaDefinitionApp Instance => instance.Value;

		public override SchemaAppKey[] KeyOrder { get; set; }

		private void defineFields()
		{
			Fields = new SchemaDictionaryApp();

			KeyOrder = new SchemaAppKey[Enum.GetNames(typeof(SchemaAppKey)).Length];
			int idx = 0;

			KeyOrder[idx++] =
				defineField<string>(AK_NAME, "Name", "Name", SCHEMA_NAME );

			KeyOrder[idx++] =
				defineField<string>(AK_DESCRIPTION, "Description", "Description", SCHEMA_DESC );

			KeyOrder[idx++] =
				defineField<string>(AK_VERSION, "Version", "Cells Version", SCHEMA_VER );
		}


		// the guid for each sub-schema and the 
		// field that holds the sub-schema - both must match
		// the guid here is missing the last (2) digits.
		// fill in for each sub-schema 
		// unit type is number is a filler
		private static SchemaFieldDef<SchemaAppKey> SubSchemaFieldInfo { get; } =
			new SchemaFieldDef<SchemaAppKey>(AK_UNDEFINED, "RootCellsDefinition{0:D2}",
				"subschema for a cells definition", "");

		public static SchemaFieldDef<SchemaAppKey> GetSubSchemaDef(int id)
		{
			SchemaFieldDef<SchemaAppKey> subDef = new SchemaFieldDef<SchemaAppKey>();
			subDef.Sequence = SubSchemaFieldInfo.Sequence;
			subDef.Name = string.Format(SubSchemaFieldInfo.Name, id);
			subDef.Desc = SubSchemaFieldInfo.Desc;
			subDef.Guid = SchemaGuidManager.GetCellGuidString(id);

			return subDef;
		}

		// public SchemaDictionaryApp Fields { get; } =
		// 	new SchemaDictionaryApp
		// 	{
		// 		{
		// 			NAME,
		// 			new SchemaFieldDef<SchemaAppKey>(NAME, "Name",
		// 				"Name", SCHEMA_NAME)
		// 		},
		//
		// 		{
		// 			DESCRIPTION,
		// 			new SchemaFieldDef<SchemaAppKey>(DESCRIPTION, "Description",
		// 				"Description", SCHEMA_DESC)
		// 		},
		//
		// 		{
		// 			VERSION,
		// 			new SchemaFieldDef<SchemaAppKey>(VERSION, "Version",
		// 				"Cells Version", "1.0")
		// 		}
		// 	};
	}
}