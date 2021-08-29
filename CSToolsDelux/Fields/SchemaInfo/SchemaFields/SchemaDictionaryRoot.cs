#region + Using Directives

using CSToolsDelux.Fields.SchemaInfo.SchemaDefinitions;

#endregion

// user name: jeffs
// created:   7/3/2021 10:49:50 PM


namespace CSToolsDelux.Fields.SchemaInfo.SchemaFields
{
	public class SchemaDictionaryRoot : SchemaDictionaryBase<SchemaRootKey>
	{
		public SchemaDictionaryRoot Clone()
		{
			return Clone(this);
		}
	}

}
