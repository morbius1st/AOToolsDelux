#region + Using Directives

#endregion

// user name: jeffs
// created:   7/3/2021 10:49:50 PM

using AOTools.Cells.SchemaDefinition;

namespace AOTools.Cells.SchemaDefinition2
{
	public class SchemaDictionaryRoot2 : SchemaDictionaryBase2<SchemaRootKey>
	{
		public SchemaDictionaryRoot2 Clone()
		{
			return Clone(this);
		}
	}

}
