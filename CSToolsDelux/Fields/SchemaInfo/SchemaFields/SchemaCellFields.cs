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
	public class SchemaCellFields : ASchemaFields2<SchemaCellKey, SchemaDictionaryBase<SchemaCellKey>>
	{
		// public const string SCHEMA_NAME = "CellDefaultDefinition";
		public const string SCHEMA_DESC = "Default Root Cells Definition";
		public const string NOTDEFINED = "<not defined>";

		public SchemaCellFields()
		{
			SchemaName = "Fields>FieldSchema>Cell";
			defineFields();
		}

		// public CellFields<TD> GetField<TD>(SchemaCellKey key)
		// {
		// 	return (CellFields<TD>) Fields[key];
		// }
		//
		// public TD GetValue<TD>(SchemaCellKey key)
		// {
		// 	return ((CellFields<TD>) Fields[key]).Value;
		// }
		//
		// public void SetValue<TD>(SchemaCellKey key, TD value)
		// {
		// 	((CellFields<TD>) Fields[key]).Value = value;
		// }
		//
		// public ASchemaCellFields GetValue2<TD>(SchemaCellKey key)
		// {
		// 	return (ASchemaCellFields) Fields[key];
		// }

		protected override void defineFields()
		{
			Fields = new SchemaDictionaryCell();

			KeyOrder = new SchemaCellKey[Enum.GetNames(typeof(SchemaCellKey)).Length];

			int idx = 0;

			KeyOrder[idx++] =
				defineField<string>(CK_SCHEMA_NAME, "Name", "Name" , SchemaName);

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