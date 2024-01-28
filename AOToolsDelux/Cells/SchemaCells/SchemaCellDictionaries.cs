#region + Using Directives

#endregion

// user name: jeffs
// created:   7/3/2021 10:49:50 PM

using AOToolsDelux.Cells.SchemaDefinition;

namespace AOToolsDelux.Cells.SchemaCells
{
	public class SchemaDictionaryApp : SchemaDictionaryBase<SchemaAppKey>
	{
		public SchemaDictionaryApp Clone()
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
