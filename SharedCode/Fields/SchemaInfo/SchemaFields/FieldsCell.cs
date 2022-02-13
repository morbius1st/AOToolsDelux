#region + Using Directives

using System;
using System.Collections.Generic;
using SharedCode.Fields.SchemaInfo.SchemaSupport;
using SharedCode.Fields.SchemaInfo.SchemaFields.FieldsTemplates;
using SharedCode.Windows;
using static SharedCode.Fields.SchemaInfo.SchemaSupport.SchemaCellKey;
using static SharedCode.Fields.SchemaInfo.SchemaSupport.UpdateRules;
using static SharedCode.Fields.SchemaInfo.SchemaSupport.SchemaFieldDisplayLevel;

using static SharedCode.Windows.ColData.JustifyHoriz;

#endregion

// user name: jeffs
// created:   7/3/2021 10:48:37 PM

namespace SharedCode.Fields.SchemaInfo.SchemaFields
{
	public class FieldsCell : AFieldsTemp<SchemaCellKey>
	{
		// public const string SCHEMA_NAME = "CellDefaultDefinition";
		public const string SCHEMA_DESC = "Default Root Cells Definition";
		public const string NOTDEFINED = "<not defined>";

		public override KeyValuePair<SchemaDataStorType, string> FieldStorType { get; } = SchemaConstants.SchemaTypeCell;

		public FieldsCell()
		{
			SchemaName = "Fields>FieldSchema>Cell";
			defineFields();
		}

		protected override void defineFields()
		{
			Fields = new FieldsTempDictionary<SchemaCellKey>();

			FieldOrderDefault = new SchemaCellKey[Enum.GetNames(typeof(SchemaCellKey)).Length];

			int idx = 0;

			// KeyOrder is an array of keys in the standard order
			FieldOrderDefault[idx++] =
				defineField<string>(CK_SCHEMA_NAME      , "Name", "Name" , SchemaName, DL_BASIC, "A1", 16, 14, CENTER, LEFT);

			FieldOrderDefault[idx++] =
				defineField<string>(CK_DESCRIPTION      , "Description", "Description", SCHEMA_DESC, DL_BASIC, "A2", 26, 24, CENTER, LEFT );

			FieldOrderDefault[idx++] =
				defineField<string>(CK_VERSION          , "Version", "Cells Version", "0.1", DL_MEDIUM, "A3", 10, 10, CENTER, LEFT );
			
			FieldOrderDefault[idx++] =
				defineField<string>(CK_CREATE_DATE      , "CreationDate", "Date and Time Created", DateTime.UtcNow.ToString(), DL_MEDIUM, "A4", 16, 14, CENTER, LEFT);

			FieldOrderDefault[idx++] =
				defineField<string>(CK_SEQUENCE         , "Sequence", "Evaluation Sequence",  "1.0", DL_BASIC, "A5", 12, 10, CENTER, LEFT);

			FieldOrderDefault[idx++] =
				defineField<int>   (CK_UPDATE_RULE      , "UpdateRule", "Update Rule",  (int) UR_UPON_REQUEST, DL_BASIC, "A6", 16, 14, CENTER, LEFT);

			FieldOrderDefault[idx++] =
				defineField<string>(CK_CELL_FAMILY_NAME , "CellFamilyName", "Name of the Cells Family", NOTDEFINED, DL_BASIC, "A7", 26, 24, CENTER, LEFT);

			FieldOrderDefault[idx++] =
				defineField<bool>  (CK_SKIP             , "Skip", "Skip Updating", false, DL_BASIC, "A8", 10, 8, CENTER, LEFT);

			FieldOrderDefault[idx++] =
				defineField<string>(CK_XL_WORKSHEET_NAME, "XlWorksheet", "Name of the Excel Worksheet", NOTDEFINED, DL_BASIC, "A9", 16, 14, CENTER, LEFT);

			FieldOrderDefault[idx++] =
				defineField<string>(CK_XL_FILE_PATH     , "XlFilePath", "File Path to the Excel File", NOTDEFINED, DL_BASIC, "A10", 60, 60, CENTER, LEFT);

		}
	}
}