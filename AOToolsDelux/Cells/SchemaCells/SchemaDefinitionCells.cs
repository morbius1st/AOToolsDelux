#region + Using Directives
using System;
using AOToolsDelux.Cells.SchemaDefinition;
using static AOToolsDelux.Cells.SchemaDefinition.SchemaCellKey;
using static AOToolsDelux.Cells.SchemaDefinition.UpdateRules;
#endregion

// user name: jeffs
// created:   7/3/2021 10:48:37 PM

namespace AOToolsDelux.Cells.SchemaCells
{
	public class SchemaDefinitionCells : ASchemaDef<SchemaCellKey, SchemaDictionaryCell>
	{
		private static readonly Lazy<SchemaDefinitionCells> instance =
			new Lazy<SchemaDefinitionCells>(() => new SchemaDefinitionCells());

		public const string SCHEMA_NAME = "CellDefaultDefinition";
		public const string SCHEMA_DESC = "Default Root Cells Definition";
		public const string NOTDEFINED = "<not defined>";

		private SchemaDefinitionCells()
		{
			defineFields();
		}

		public static SchemaDefinitionCells Instance => instance.Value;

		public override SchemaCellKey[] KeyOrder { get; set; }

		private void defineFields()
		{
			Fields = new SchemaDictionaryCell();

			KeyOrder = new SchemaCellKey[Enum.GetNames(typeof(SchemaCellKey)).Length];
			int idx = 0;

			KeyOrder[idx++] =
				defineField<string>(CK_NAME, "Name", "Name" , SCHEMA_NAME);

			KeyOrder[idx++] =
				defineField<string>(CK_DESCRIPTION, "Description", "Description", SCHEMA_DESC );

			KeyOrder[idx++] =
				defineField<string>(CK_VERSION, "Version", "Cells Version", "0.1" );

			KeyOrder[idx++] =
				defineField<int>(CK_SEQUENCE, "Sequence", "Evaluation Sequence",  1.0,
					RevitUnitType.UT_NUMBER );

			KeyOrder[idx++] =
				defineField<int>(CK_UPDATE_RULE, "UpdateRule", "Update Rule",  (int) UR_UPON_REQUEST);

			KeyOrder[idx++] =
				defineField<string>(CK_CELL_FAMILY_NAME, "CellFamilyName", "Name of the Cells Family", NOTDEFINED);
			
			KeyOrder[idx++] =
				defineField<bool>(CK_SKIP, "Skip", "Skip Updating", false);

			KeyOrder[idx++] =
				defineField<string>(CK_XL_FILE_PATH, "XlFilePath", "File Path to the Excel File", NOTDEFINED);
			
			KeyOrder[idx++] =
				defineField<string>(CK_XL_WORKSHEET_NAME, "XlWorksheet", "Name of the Excel Worksheet", NOTDEFINED);
		}

		// public Enum[] k { get; } = { NAME, VERSION};
		//
		// public SchemaDictionaryCell Fields { get; } =
		// 	new SchemaDictionaryCell
		// 	{
		// 		{
		// 			NAME,
		// 			new SchemaFieldDef<SchemaCellKey>(NAME, "Name",
		// 				"Name", SCHEMA_NAME)
		// 		},
		//
		// 		{
		// 			VERSION,
		// 			new SchemaFieldDef<SchemaCellKey>(VERSION, "Version",
		// 				"Version", "1.0")
		// 		},
		// 		
		// 		{
		// 			DESCRIPTION,
		// 			new SchemaFieldDef<SchemaCellKey>(DESCRIPTION, "Description",
		// 				"Description", SCHEMA_DESC)
		// 		},
		//
		// 		{
		// 			SEQUENCE,
		// 			new SchemaFieldDef<SchemaCellKey>(SEQUENCE, "Sequence",
		// 				"Evaluation Sequence", 1.0 , RevitUnitType.UT_NUMBER)
		// 		},
		// 		
		// 		{
		// 			UPDATE_RULE,
		// 			new SchemaFieldDef<SchemaCellKey>(UPDATE_RULE, "UpdateRule",
		// 				"Update Rule", (int) UPON_REQUEST)
		// 		},
		//
		// 		{
		// 			CELL_FAMILY_NAME,
		// 			new SchemaFieldDef<SchemaCellKey>(CELL_FAMILY_NAME, "CellFamilyName",
		// 				"Name of the Cells Family", NOTDEFINED)
		// 		},
		// 		
		// 		{
		// 			SKIP,
		// 			new SchemaFieldDef<SchemaCellKey>(SKIP, "Skip",
		// 				"Skip Updating", false)
		// 		},
		//
		// 		{
		// 			XL_FILE_PATH,
		// 			new SchemaFieldDef<SchemaCellKey>(XL_FILE_PATH, "XlFilePath",
		// 				"File Path to the Excel File", NOTDEFINED)
		// 		},
		// 		
		// 		{
		// 			XL_WORKSHEET_NAME,
		// 			new SchemaFieldDef<SchemaCellKey>(XL_WORKSHEET_NAME, "XlWorksheet",
		// 				"Name of the Excel Worksheet", NOTDEFINED)
		// 		},
		// 	};

	}
}