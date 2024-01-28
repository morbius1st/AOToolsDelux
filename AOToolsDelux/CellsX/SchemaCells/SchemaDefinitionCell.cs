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

	public class SchemaDefinitionCell : ASchemaDef<SchemaCellKey, SchemaDictionaryCell>
	{
		private static readonly Lazy<SchemaDefinitionCell> instance =
			new Lazy<SchemaDefinitionCell>(() => new SchemaDefinitionCell());

		private SchemaDefinitionCell()
		{
			DefineFields();
		}

		public static SchemaDefinitionCell Instance => instance.Value;


		private void DefineFields()
		{
			Fields = new SchemaDictionaryCell();

			KeyOrder = new SchemaCellKey[Enum.GetNames(typeof(SchemaCellKey)).Length];
			int idx = 0;

			KeyOrder[idx++] =
				defineField<string>(NAME, "Name", "Name" );

			KeyOrder[idx++] =
				defineField<string>(DESCRIPTION, "Description", "Description" );

			KeyOrder[idx++] =
				defineField<string>(VERSION, "Version", "Cells Version" );

			KeyOrder[idx++] =
				defineField<int>(SEQUENCE, "Sequence", "Evaluation Sequence",  
					RevitUnitType.UT_NUMBER );

			KeyOrder[idx++] =
				defineField<int>(UPDATE_RULE, "UpdateRule", "Update Rule",  
					RevitUnitType.UT_NUMBER);

			KeyOrder[idx++] =
				defineField<string>(CELL_FAMILY_NAME, "CellFamilyName", "Name of the Cells Family");
			
			KeyOrder[idx++] =
				defineField<bool>(SKIP, "Skip", "Skip Updating");

			KeyOrder[idx++] =
				defineField<string>(XL_FILE_PATH, "XlFilePath", "File Path to the Excel File");
			
			KeyOrder[idx++] =
				defineField<string>(XL_WORKSHEET_NAME, "XlWorksheet", "Name of the Excel Worksheet");
		}

		// private SchemaCellKey defineField<TD>(SchemaCellKey key, 
		// 	string name, string desc, RevitUnitType unitType = RevitUnitType.UT_UNDEFINED)
		// {
		// 	Fields.Add(key, 
		// 		new SchemaFieldDef<SchemaCellKey, TD>(key, name, desc, unitType));
		//
		// 	return key;
		// }

		//
		// public override SchemaDictionaryCell DefaultFields { get; } =
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