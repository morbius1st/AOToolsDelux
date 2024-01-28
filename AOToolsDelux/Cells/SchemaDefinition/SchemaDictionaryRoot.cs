#region + Using Directives

#endregion

// user name: jeffs
// created:   7/3/2021 10:49:50 PM

namespace AOToolsDelux.Cells.SchemaDefinition
{
	public class SchemaDictionaryRoot : SchemaDictionaryBase<SchemaRootKey>
	{
		public SchemaDictionaryRoot Clone()
		{
			return Clone(this);
		}
	}

}
