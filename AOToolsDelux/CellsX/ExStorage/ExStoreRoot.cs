#region using

using System;
using System.Linq.Expressions;
using AOToolsDelux.Cells.SchemaDefinition;

#endregion

// username: jeffs
// created:  7/4/2021 3:54:15 PM

namespace AOToolsDelux.Cells.ExStorage
{
	public class ExStoreRoot :
			IExStore<SchemaRootKey, SchemaDictionaryRoot>
	{
	#region private fields

		private static readonly Lazy<ExStoreRoot> instance =
			new Lazy<ExStoreRoot>(() => new ExStoreRoot());

		public const string ROOT_SCHEMA_NAME = "CellsAppRoot";
		public const string ROOT_SCHEMA_DESC = "Excel Cells to Revit Exchange";
		public const string ROOT_DEVELOPER_NAME = "CyberStudio";

	#endregion

	#region ctor

		private ExStoreRoot() { }

	#endregion

	#region public properties

		public static ExStoreRoot Instance => instance.Value;

		public string Name => ROOT_SCHEMA_NAME;
		public string Description => ROOT_SCHEMA_DESC;
		public string Developer => ROOT_DEVELOPER_NAME;
		public Guid ExStoreGuid => SchemaGuidManager.RootGuid;

		// the schema fields
		public  SchemaDefinitionRoot SchemaDefinition { get; }  = SchemaDefinitionRoot.Instance;

		public SchemaDictionaryRoot FieldDefs => SchemaDefinition.Fields;

	#endregion

	#region private properties

	#endregion

	#region public methods

	#endregion

	#region private methods

	#endregion

	#region event consuming

	#endregion

	#region event publishing

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is ExStoreRoot";
		}

	#endregion
	}
}