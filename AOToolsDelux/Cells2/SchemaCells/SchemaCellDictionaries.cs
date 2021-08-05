#region + Using Directives

using AOTools.Cells2.SchemaDefinition;

#endregion

// user name: jeffs
// created:   7/3/2021 10:49:50 PM

namespace AOTools.Cells2.SchemaCells
{
	public class SchemaDictionaryApp : SchemaDictionaryBase<SchemaAppKey>
	{
		public SchemaDictionaryApp() { }
		public SchemaDictionaryApp(int capacity) :base(capacity) { }
		public SchemaDictionaryApp Clone()
		{
			return Clone(this);
		}
	}

	public class SchemaDictionaryCell : SchemaDictionaryBase<SchemaCellKey>
	{
		public SchemaDictionaryCell() { }
		public SchemaDictionaryCell(int capacity) :base(capacity) { }
		public SchemaDictionaryCell Clone()
		{
			return Clone(this);
		}
	}
}
