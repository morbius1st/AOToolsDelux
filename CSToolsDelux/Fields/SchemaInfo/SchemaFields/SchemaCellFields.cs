#region + Using Directives

using System;
using CSToolsDelux.Fields.SchemaInfo.SchemaDefinitions;
using static CSToolsDelux.Fields.SchemaInfo.SchemaDefinitions.SchemaCellKey;
using static CSToolsDelux.Fields.SchemaInfo.SchemaDefinitions.UpdateRules;

#endregion

// user name: jeffs
// created:   7/3/2021 10:48:37 PM

namespace CSToolsDelux.Fields.SchemaInfo.SchemaFields
{
	public class SchemaCellFields : ASchemaFieldsCell
	{
		public const string SCHEMA_NAME = "CellDefaultDefinition";
		public const string SCHEMA_DESC = "Default Root Cells Definition";
		public const string NOTDEFINED = "<not defined>";

		public SchemaCellFields()
		{
			defineFields();
		}

		public override SchemaCellKey[] KeyOrder { get; set; }

		public SchemaFieldCell<TD> GetField<TD>(SchemaCellKey key)
		{
			return (SchemaFieldCell<TD>) Fields[key];
		}

		public TD GetValue<TD>(SchemaCellKey key)
		{
			return ((SchemaFieldCell<TD>) Fields[key]).Value;
		}

		public void SetValue<TD>(SchemaCellKey key, TD value)
		{
			((SchemaFieldCell<TD>) Fields[key]).Value = value;
		}

		public ASchemaFieldsCell GetValue2<TD>(SchemaCellKey key)
		{
			return (ASchemaFieldsCell) Fields[key];
		}

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
				defineField<string>(CK_SEQUENCE, "Sequence", "Evaluation Sequence",  "1.0");

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
	}
}