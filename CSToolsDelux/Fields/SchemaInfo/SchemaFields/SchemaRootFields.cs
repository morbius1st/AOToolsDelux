#region + Using Directives

using System;
using CSToolsDelux.Fields.SchemaInfo.SchemaDefinitions;
using static CSToolsDelux.Fields.SchemaInfo.SchemaDefinitions.SchemaRootKey;
#endregion

// user name: jeffs
// created:   7/3/2021 10:48:37 PM

namespace CSToolsDelux.Fields.SchemaInfo.SchemaFields
{
	// public class SchemaRootFields : ASchemaRootFields
	public class SchemaRootFields : ASchemaFields2<SchemaRootKey, SchemaDictionaryBase<SchemaRootKey>>
	{
		// public const string RF_SCHEMA_NAME = "FieldsRootSchema";
		public const string RF_SUBSCHEMA_NAME = "FieldsSubSchemaName_";
		public const string RF_SCHEMA_DESC = "Revit Fields";
		public const string RF_ROOT_DEVELOPER_NAME = "CyberStudio";
		public const string RF_SCHEMA_VER = "0.1";

		public SchemaRootFields()
		{
			SchemaName = "Fields>FieldSchema>Root";
			defineFields();
		}

		// public RootFields<TD> GetField<TD>(SchemaRootKey key)
		// {
		// 	return (RootFields<TD>) Fields[key];
		// }
		//
		// public TD GetValue<TD>(SchemaRootKey key)
		// {
		// 	return ((RootFields<TD>) Fields[key]).Value;
		// }
		//
		// public void SetValue<TD>(SchemaRootKey key, TD value)
		// {
		// 	((RootFields<TD>) Fields[key]).Value = value;
		// }

		protected override void defineFields()
		{
			Fields = new SchemaDictionaryBase<SchemaRootKey>();

			KeyOrder = new SchemaRootKey[Enum.GetNames(typeof(SchemaRootKey)).Length];
			int idx = 0;

			KeyOrder[idx++] =
				defineField<string>(RK_SCHEMA_NAME, "Name", "Name", SchemaName );

			KeyOrder[idx++] =
				defineField<string>(RK_DESCRIPTION, "Description", "Description", RF_SCHEMA_DESC );

			KeyOrder[idx++] =
				defineField<string>(RK_VERSION, "Version", "Cells Version", RF_SCHEMA_VER );

			KeyOrder[idx++] =
				defineField<string>(RK_DEVELOPER, "Developer", "Developer", RF_ROOT_DEVELOPER_NAME );

			KeyOrder[idx++] =
				defineField<string>(RK_CREATE_DATE, "CreationData", "Date and Time Created", DateTime.UtcNow.ToString());
			
			KeyOrder[idx++] =
				defineField<string>(RK_GUID, "AppGuidString", "App Guid String", Guid.NewGuid().ToString());

		}

		public Tuple<string, Guid> SubSchemaField()
		{
			string uniqueName = RF_SUBSCHEMA_NAME+ System.IO.Path.GetRandomFileName().Replace('.', '_');

			return new Tuple<string, Guid>(uniqueName, Guid.NewGuid());
		}

/*
		public SchemaRootFields SubSchemaFields()
		{
			SchemaRootFields subS = new SchemaRootFields();

			string uniqueName = RF_SUBSCHEMA_NAME+ System.IO.Path.GetRandomFileName().Replace('.', '_');

			subS.SetValue(RK_SCHEMA_NAME, uniqueName);

			return subS;
		}
*/
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