#region + Using Directives

using CSToolsDelux.Fields.SchemaInfo.SchemaDefinitions;

#endregion

// user name: jeffs
// created:   7/3/2021 10:49:50 PM


namespace CSToolsDelux.Fields.SchemaInfo.SchemaFields
{
	
	public class SchemaDictionaryRootApp : SchemaDictionaryBase<SchemaRootAppKey>
	{
		public SchemaDictionaryRootApp Clone()
		{
			return Clone(this);
		}
	}


	public class SchemaDictionaryCell : SchemaDictionaryBase<SchemaCellKey>
	{
		public SchemaDictionaryCell Clone()
		{
			return Clone(this);
		}
	}

}
