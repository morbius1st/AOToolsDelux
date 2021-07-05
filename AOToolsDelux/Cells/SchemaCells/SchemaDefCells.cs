#region + Using Directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AOTools.Cells.SchemaDefinition;
using static AOTools.Cells.SchemaDefinition.SchemaCellKey;
using static AOTools.Cells.SchemaDefinition.UpdateRules;

#endregion

// user name: jeffs
// created:   7/3/2021 10:48:37 PM

namespace AOTools.Cells.SchemaCells
{
	public class SchemaDefCells
	{
		protected const string SCHEMA_NAME = "CellDefaultDefinition";
		protected const string SCHEMA_DESC = "Default Root Cells Definition";
		protected const string NOTDEFINED = "<not defined>";

		public static SchemaDictionaryCell DefaultFields { get; } =
			new SchemaDictionaryCell
			{
				{
					NAME,
					new SchemaFieldDef(NAME, "Name",
						"Name", SCHEMA_NAME)
				},

				{
					VERSION,
					new SchemaFieldDef(VERSION, "Version",
						"Version", "1.0")
				},
				
				{
					DESCRIPTION,
					new SchemaFieldDef(DESCRIPTION, "Description",
						"Description", SCHEMA_DESC)
				},

				{
					SEQUENCE,
					new SchemaFieldDef(SEQUENCE, "Sequence",
						"Evaluation Sequence", 1.0)
				},
				
				{
					UPDATE_RULE,
					new SchemaFieldDef(UPDATE_RULE, "UpdateRule",
						"Update Rule", UPON_REQUEST)
				},

				{
					CELL_FAMILY_NAME,
					new SchemaFieldDef(CELL_FAMILY_NAME, "CellFamilyName",
						"Name of the Cells Family", NOTDEFINED)
				},
				
				{
					SKIP,
					new SchemaFieldDef(SKIP, "Skip",
						"Skip Updating", false)
				},

				{
					XL_FILE_PATH,
					new SchemaFieldDef(XL_FILE_PATH, "XlFilePath",
						"File Path to the Excel File", NOTDEFINED)
				},
				
				{
					XL_WORKSHEET_NAME,
					new SchemaFieldDef(XL_WORKSHEET_NAME, "XlWorksheet",
						"Name of the Excel Worksheet", NOTDEFINED)
				},
			};
	}
}