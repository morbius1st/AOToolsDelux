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

		private SchemaFieldDef<SchemaAppKey, string> subSchemaFieldInfo;

		private SchemaDefinitionApp()
		{
			subSchemaFieldInfo = new SchemaFieldDef<SchemaAppKey, string>(UNDEFINED, "RootCellsDefinition{0:D2}",
				"subschema for a cells definition");

			DefineFields();
		}

		public static SchemaDefinitionApp Instance => instance.Value;

		// the guid for each sub-schema and the 
		// field that holds the sub-schema - both must match
		// the guid here is missing the last (2) digits.
		// fill in for each sub-schema 
		// unit type is number is a filler
		private SchemaFieldDef<SchemaAppKey, string> SubSchemaFieldInfo => subSchemaFieldInfo;

		public SchemaFieldDef<SchemaAppKey, string> GetSubSchemaDef(int id)
		{
			SchemaFieldDef<SchemaAppKey, string> subDef = new SchemaFieldDef<SchemaAppKey, string>();
			subDef.Sequence = SubSchemaFieldInfo.Sequence;
			subDef.Name = string.Format(SubSchemaFieldInfo.Name, id);
			subDef.Desc = SubSchemaFieldInfo.Desc;
			subDef.Guid = SchemaGuidManager.GetCellGuidString(id);

			return subDef;
		}

		private void DefineFields()
		{
			Fields = new SchemaDictionaryApp();

			KeyOrder = new SchemaAppKey[Enum.GetNames(typeof(SchemaAppKey)).Length];
			int idx = 0;

			KeyOrder[idx++] =
				defineField<string>(NAME, "Name", "Name" );
			
			KeyOrder[idx++] =
				defineField<string>(DESCRIPTION, "Description", "Description" );
			
			KeyOrder[idx++] =
				defineField<string>(VERSION, "Version", "Cells Version" );
		}

		// private SchemaAppKey defineField<TD>(SchemaAppKey key, 
		// 	string name, string desc)
		// {
		// 	Fields.Add(key, 
		// 		new SchemaFieldDef<SchemaAppKey, TD>(key, name, desc));
		//
		// 	return key;
		// }
	}
}
