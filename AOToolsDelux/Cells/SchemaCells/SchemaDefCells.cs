#region + Using Directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AOTools.Cells.SchemaDefinition;
using Autodesk.Revit.DB;
using static AOTools.Cells.SchemaDefinition.SchemaCellKey;
using static AOTools.Cells.SchemaDefinition.UpdateRules;

#endregion

// user name: jeffs
// created:   7/3/2021 10:48:37 PM

namespace AOTools.Cells.SchemaCells
{
	public class SchemaDefCells : ASchemaDef<SchemaCellKey>
	{
		public const string SCHEMA_NAME = "CellDefaultDefinition";
		public const string SCHEMA_DESC = "Default Root Cells Definition";
		public const string NOTDEFINED = "<not defined>";

		// public static string[] FIELD_NAMES;

		private SchemaDefCells()
		{
			FIELD_NAMES = new string[9];
			FIELD_NAMES[(int) CK_NAME]              = "Name";
			FIELD_NAMES[(int) CK_DESCRIPTION]       = "Description";
			FIELD_NAMES[(int) CK_VERSION]           = "Version";
			FIELD_NAMES[(int) CK_SEQUENCE]          = "Sequence";
			FIELD_NAMES[(int) CK_UPDATE_RULE]       = "UpdateRule";
			FIELD_NAMES[(int) CK_CELL_FAMILY_NAME]  = "CellFamilyName";
			FIELD_NAMES[(int) CK_SKIP]              = "Skip";
			FIELD_NAMES[(int) CK_XL_FILE_PATH]      = "XlFilePath";
			FIELD_NAMES[(int) CK_XL_WORKSHEET_NAME] = "XlWorksheet";

		}

		public static SchemaDefCells Inst { get; } = new SchemaDefCells();

		public override string this[SchemaCellKey key]
		{
			get
			{
				return FIELD_NAMES[(int) key];
			}
		}

		public override SchemaDictionaryBase<string> DefaultFields { get; } =
			new SchemaDictionaryCell
			{
				{
					Inst[CK_NAME],
					new SchemaFieldDef(Inst[CK_NAME],
						"Name", SCHEMA_NAME)
				},
				
				{
					Inst[CK_DESCRIPTION],
					new SchemaFieldDef(Inst[CK_DESCRIPTION],
						"Description", SCHEMA_DESC)
				},

				{
					Inst[CK_VERSION],
					new SchemaFieldDef(Inst[CK_VERSION],
						"Version", "1.0")
				},

				{
					Inst[CK_SEQUENCE],
					new SchemaFieldDef(Inst[CK_SEQUENCE],
						"Evaluation Sequence", 1.0 , RevitUnitType.UT_NUMBER)
				},
				
				{
					Inst[CK_UPDATE_RULE],
					new SchemaFieldDef(Inst[CK_UPDATE_RULE],
						"Update Rule", (int) UPON_REQUEST)
				},

				{
					Inst[CK_CELL_FAMILY_NAME],
					new SchemaFieldDef(Inst[CK_CELL_FAMILY_NAME],
						"Name of the Cells Family", NOTDEFINED)
				},
				
				{
					Inst[CK_SKIP],
					new SchemaFieldDef(Inst[CK_SKIP],
						"Skip Updating", false)
				},

				{
					Inst[CK_XL_FILE_PATH],
					new SchemaFieldDef(Inst[CK_XL_FILE_PATH],
						"File Path to the Excel File", NOTDEFINED)
				},
				
				{
					Inst[CK_XL_WORKSHEET_NAME],
					new SchemaFieldDef(Inst[CK_XL_WORKSHEET_NAME],
						"Name of the Excel Worksheet", NOTDEFINED)
				},
			};

	}
}