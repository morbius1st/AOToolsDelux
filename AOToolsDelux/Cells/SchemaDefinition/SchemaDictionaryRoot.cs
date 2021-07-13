#region + Using Directives

using AOTools.Cells.SchemaDefinition;

#endregion

// user name: jeffs
// created:   7/3/2021 10:49:50 PM

namespace AOTools.Cells.SchemaDefinition
{
	public class SchemaDictionaryRoot : SchemaDictionaryBase<SchemaRootKey>
	{
		public SchemaDictionaryRoot() { }
		public SchemaDictionaryRoot(int capacity) :base(capacity) { }
		public SchemaDictionaryRoot Clone()
		{
			return Clone(this);
		}
	}

}
