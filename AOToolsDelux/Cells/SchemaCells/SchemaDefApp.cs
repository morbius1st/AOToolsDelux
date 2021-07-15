#region + Using Directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AOTools.Cells.SchemaDefinition;
using static AOTools.Cells.SchemaDefinition.SchemaAppKey;

#endregion

// user name: jeffs
// created:   7/3/2021 10:48:37 PM

namespace AOTools.Cells.SchemaCells
{
	public class SchemaDefApp : ASchemaDef<SchemaAppKey>
	{
		public const string SCHEMA_NAME = "CellsAppData";
		public const string SCHEMA_DESC = "Excel Cells to Revit Exchange";
		public const string DEVELOPER_NAME = "CyberStudio";

		// public string[] FIELD_NAMES;

		private SchemaDefApp()
		{
			FIELD_NAMES = new string[3];
			FIELD_NAMES[(int) AK_NAME]        = "Name";
			FIELD_NAMES[(int) AK_DESCRIPTION] = "Description";
			FIELD_NAMES[(int) AK_VERSION]     = "Version";
		}

		public static SchemaDefApp Inst { get; } = new SchemaDefApp();

		public override string this[SchemaAppKey key]
		{
			get
			{
				return FIELD_NAMES[(int) key];
			}
		}

		// // the guid for each sub-schema and the 
		// // field that holds the sub-schema - both must match
		// // the guid here is missing the last (2) digits.
		// // fill in for each sub-schema 
		// // unit type is number is a filler
		// private static SchemaFieldDef<SchemaAppKey> SubSchemaFieldInfo { get; } =
		// 	new SchemaFieldDef<SchemaAppKey>(UNDEFINED, "RootCellsDefinition{0:D2}",
		// 		"subschema for a cells definition", "");
		//
		// public static SchemaFieldDef<SchemaAppKey> GetSubSchemaDef(int id)
		// {
		// 	SchemaFieldDef<SchemaAppKey> subDef = new SchemaFieldDef<SchemaAppKey>();
		// 	subDef.Sequence = SubSchemaFieldInfo.Sequence;
		// 	subDef.FieldName = string.Format(SubSchemaFieldInfo.FieldName, id);
		// 	subDef.Desc = SubSchemaFieldInfo.Desc;
		// 	subDef.Guid = SchemaGuidManager.GetCellGuidString(id);
		//
		// 	return subDef;
		// }

		public override SchemaDictionaryBase<string> DefaultFields { get; } =
			new SchemaDictionaryApp
			{
				{
					Inst[AK_NAME],
					new SchemaFieldDef(Inst[AK_NAME],
						"Name", SCHEMA_NAME)
				},

				{
					Inst[AK_DESCRIPTION],
					new SchemaFieldDef(Inst[AK_DESCRIPTION],
						"Description", SCHEMA_DESC)
				},

				{
					Inst.FIELD_NAMES[(int) AK_VERSION],
					new SchemaFieldDef(Inst[AK_VERSION],
						"Cells Version", "1.0")
				}
			};
	}
}
