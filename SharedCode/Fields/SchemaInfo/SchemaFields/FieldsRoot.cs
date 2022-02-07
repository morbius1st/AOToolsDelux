#region + Using Directives

using System;
using System.Collections.Generic;
using SharedCode.Fields.SchemaInfo.SchemaSupport;
using SharedCode.Fields.SchemaInfo.SchemaFields.FieldsTemplates;
using static SharedCode.Fields.SchemaInfo.SchemaSupport.SchemaRootKey;
using static SharedCode.Fields.SchemaInfo.SchemaSupport.SchemaFieldDisplayLevel;
#endregion

// user name: jeffs
// created:   7/3/2021 10:48:37 PM

namespace SharedCode.Fields.SchemaInfo.SchemaFields
{
	// public class SchemaRootFields : ASchemaRootFields
	public class FieldsRoot : AFieldsTemp<SchemaRootKey>
	{
		// public const string RF_SCHEMA_NAME = "FieldsRootSchema";
		public const string RF_SUBSCHEMA_NAME = "FieldsSubSchemaName_";
		public const string RF_SCHEMA_DESC = "Revit Fields";
		public const string RF_ROOT_DEVELOPER_NAME = "CyberStudio";
		public const string RF_SCHEMA_VER = "0.1";

		public KeyValuePair<string, SchemaDataStorType> Type { get; } = SchemaConstants.SchemaTypeRoot;

		public FieldsRoot()
		{
			SchemaName = "Fields>FieldSchema>Root";
			defineFields();
		}

		protected override void defineFields()
		{
			Fields = new FieldsTempDictionary<SchemaRootKey>();

			FieldOrderDefault = new SchemaRootKey[Enum.GetNames(typeof(SchemaRootKey)).Length];
			int idx = 0;

			FieldOrderDefault[idx++] =
				defineField<string>(RK_SCHEMA_NAME, "Name", "Name", SchemaName, DL_BASIC, "A1", 16 );

			FieldOrderDefault[idx++] =
				defineField<string>(RK_DESCRIPTION, "Description", "Description", RF_SCHEMA_DESC, DL_BASIC, "A2", 16 );

			FieldOrderDefault[idx++] =
				defineField<string>(RK_VERSION    , "Version", "Cells Version", RF_SCHEMA_VER, DL_MEDIUM, "A3", 16 );

			FieldOrderDefault[idx++] =
				defineField<string>(RK_CREATE_DATE, "CreationData", "Date and Time Created", DateTime.UtcNow.ToString(), DL_MEDIUM, "A5", 16);
			
			FieldOrderDefault[idx++] =
				defineField<string>(RK_DEVELOPER  , "Developer", "Developer", RF_ROOT_DEVELOPER_NAME, DL_ADVANCED, "A4", 16 );

			FieldOrderDefault[idx++] =
				defineField<string>(RK_GUID       , "AppGuidString", "App Guid String", Guid.NewGuid().ToString(), DL_DEBUG, "A6", 16);

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