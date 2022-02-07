#region + Using Directives

#endregion

// user name: jeffs
// created:   8/28/2021 9:54:13 PM

using SharedCode.Fields.SchemaInfo.SchemaSupport;
using SharedCode.Fields.SchemaInfo.SchemaData.DataTemplates;
using SharedCode.Fields.SchemaInfo.SchemaFields.FieldsTemplates;

namespace CSToolsDelux.Fields.SchemaInfo.SchemaData.SchemaDataDefinitions
{
	public class CellData<TD> : DataMembers<SchemaCellKey, TD>
	{
		public CellData(TD value, AFieldsMembers<SchemaCellKey> aFieldsMembers)
			: base(value, aFieldsMembers) { }
		// {
		// 	Value = value;
		// 	FieldDef = fieldDef;
		// 	ValueType = typeof(TD);
		// }
	}

	public class RootData<TD> : DataMembers<SchemaRootKey, TD>
	{
		public RootData(TD value, AFieldsMembers<SchemaRootKey> aFieldsMembers) 
			: base(value, aFieldsMembers) { }
		// {
		// 	Value = value;
		// 	FieldDef = fieldDef;
		// 	ValueType = typeof(TD);
		// }
	}
	
	public class LockData<TD> : DataMembers<SchemaLockKey, TD>
	{
		public LockData(TD value, AFieldsMembers<SchemaLockKey> aFieldsMembers)
			: base(value, aFieldsMembers) { }
		// {
		// 	Value = value;
		// 	FieldDef = fieldDef;
		// 	ValueType = typeof(TD);
		// }
	}
}
