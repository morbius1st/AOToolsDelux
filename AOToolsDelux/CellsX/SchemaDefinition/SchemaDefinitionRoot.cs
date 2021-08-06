#region + Using Directives

using System;
using AOTools.Cells.ExStorage;
using static AOTools.Cells.SchemaDefinition.SchemaRootKey;

#endregion

// user name: jeffs
// created:   7/3/2021 10:48:37 PM

namespace AOTools.Cells.SchemaDefinition
{
	public class SchemaDefinitionRoot : ASchemaDef<SchemaRootKey, SchemaDictionaryRoot>
	{
		private static readonly Lazy<SchemaDefinitionRoot> instance =
			new Lazy<SchemaDefinitionRoot>(() => new SchemaDefinitionRoot());

		private SchemaDefinitionRoot()
		{
			DefineFields();
		}

		public static SchemaDefinitionRoot Instance => instance.Value;

		private void DefineFields()
		{
			Fields = new SchemaDictionaryRoot();

			KeyOrder = new SchemaRootKey[Enum.GetNames(typeof(SchemaRootKey)).Length];
			int idx = 0;

			KeyOrder[idx++] =
				defineField<string>(NAME, "Name", "Name");
			
			KeyOrder[idx++] =
				defineField<string>(DESCRIPTION, "Description", "Description");
			
			KeyOrder[idx++] =
				defineField<string>(VERSION, "Version", "Cells Version");
			
			KeyOrder[idx++] =
				defineField<string>(DEVELOPER,"Developer", "Developer");
			
			KeyOrder[idx++] =
				defineField<string>(APP_GUID, "UniqueAppGuidString", 
					"Unique App Guid String" );
		}

		// private SchemaRootKey defineField<TD>(SchemaRootKey key, 
		// 	string name, string desc)
		// {
		// 	Fields.Add(key, 
		// 		new SchemaFieldDef<SchemaRootKey, TD>(key, name, desc));
		//
		// 	return key;
		// }
	}
}