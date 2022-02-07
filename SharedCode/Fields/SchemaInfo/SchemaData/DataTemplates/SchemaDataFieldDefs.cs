#region + Using Directives

#endregion

// user name: jeffs
// created:   8/28/2021 9:54:13 PM

using SharedCode.Fields.SchemaInfo.SchemaData.SchemaDataDefinitions;
using SharedCode.Fields.SchemaInfo.SchemaDefinitions;

namespace CSToolsDelux.Fields.SchemaInfo.SchemaData.SchemaDataDefinitions
{
	public class SchemaCellDataField<TD> : SchemaDataFieldDef<SchemaCellKey, TD>
	{
		public SchemaCellDataField(TD value, ISchemaFieldDef<SchemaCellKey> fieldDef)
			: base(value, fieldDef) { }
		// {
		// 	Value = value;
		// 	FieldDef = fieldDef;
		// 	ValueType = typeof(TD);
		// }
	}

	public class SchemaRootDataField<TD> : SchemaDataFieldDef<SchemaRootKey, TD>
	{
		public SchemaRootDataField(TD value, ISchemaFieldDef<SchemaRootKey> fieldDef) 
			: base(value, fieldDef) { }
		// {
		// 	Value = value;
		// 	FieldDef = fieldDef;
		// 	ValueType = typeof(TD);
		// }
	}
	
	public class SchemaLockDataField<TD> : SchemaDataFieldDef<SchemaLockKey, TD>
	{
		public SchemaLockDataField(TD value, ISchemaFieldDef<SchemaLockKey> fieldDef)
			: base(value, fieldDef) { }
		// {
		// 	Value = value;
		// 	FieldDef = fieldDef;
		// 	ValueType = typeof(TD);
		// }
	}
}
