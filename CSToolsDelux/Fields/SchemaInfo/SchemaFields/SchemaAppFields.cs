#region + Using Directives

using System;
using CSToolsDelux.Fields.SchemaInfo.SchemaDefinitions;
using static CSToolsDelux.Fields.SchemaInfo.SchemaDefinitions.SchemaAppKey;
#endregion

// user name: jeffs
// created:   7/3/2021 10:48:37 PM

namespace CSToolsDelux.Fields.SchemaInfo.SchemaFields
{
	public class SchemaAppFields : ASchemaFieldsApp
	{
		public const string SCHEMA_NAME = "CellsAppData";
		public const string SCHEMA_DESC = "Excel Cells to Revit Exchange";
		public const string SCHEMA_VER = "0.1";

		public SchemaAppFields()
		{
			defineFields();
		}

		public override SchemaAppKey[] KeyOrder { get; set; }

		public ISchemaFieldDef<SchemaAppKey> this[SchemaAppKey key] => Fields[key];

		public SchemaFieldApp<TD> GetField<TD>(SchemaAppKey key)
		{
			return (SchemaFieldApp<TD>) Fields[key];
		}

		public TD GetValue<TD>(SchemaAppKey key)
		{
			return ((SchemaFieldApp<TD>) Fields[key]).Value;
		}

		public void SetValue<TD>(SchemaAppKey key, TD value)
		{
			((SchemaFieldApp<TD>) Fields[key]).Value = value;
		}

		private void defineFields()
		{
			Fields = new SchemaDictionaryApp();

			KeyOrder = new SchemaAppKey[(int) AK_COUNT];
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
		// private static SchemaFieldDef<SchemaAppKey> SubSchemaFieldInfo { get; } =
		// 	new SchemaFieldDef<SchemaAppKey>(AK_UNDEFINED, "RootCellsDefinition{0:D2}",
		// 		"subschema for a cells definition", "");
		//
		// public static SchemaFieldDef<SchemaAppKey> GetSubSchemaDef(int id)
		// {
		// 	SchemaFieldDef<SchemaAppKey> subDef = new SchemaFieldDef<SchemaAppKey>();
		// 	subDef.Sequence = SubSchemaFieldInfo.Sequence;
		// 	subDef.Name = string.Format(SubSchemaFieldInfo.Name, id);
		// 	subDef.Desc = SubSchemaFieldInfo.Desc;
		// 	subDef.Guid = SchemaGuidManager.GetCellGuidString(id);
		//
		// 	return subDef;
		// }
	}
}