#region + Using Directives

using AOTools.Cells.SchemaDefinition;

#endregion

// user name: jeffs
// created:   7/3/2021 10:49:50 PM

namespace AOTools.Cells.SchemaCells
{
	public class SchemaDictionaryApp : SchemaDictionaryBase<string>
	{
		public SchemaDictionaryApp() { }
		public SchemaDictionaryApp(int capacity) :base(capacity) { }
		// public SchemaDictionaryApp Clone()
		// {
		// 	return Clone();
		// 	// return Clone(this);
		// }
	}

	public class SchemaDictionaryCell : SchemaDictionaryBase<string>
	{
		public SchemaDictionaryCell() { }
		public SchemaDictionaryCell(int capacity) :base(capacity) { }
		// public SchemaDictionaryCell Clone()
		// {
		// 	return Clone();
		// 	// return Clone(this);
		// }
	}
}
