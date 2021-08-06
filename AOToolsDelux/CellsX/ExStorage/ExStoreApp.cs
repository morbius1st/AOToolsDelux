#region using

using System;
using AOTools.Cells.SchemaCells;
using AOTools.Cells.SchemaDefinition;

#endregion

// username: jeffs
// created:  7/4/2021 3:54:15 PM

namespace AOTools.Cells.ExStorage
{
	public class ExStoreApp : IExStore<SchemaAppKey, SchemaDictionaryApp>
	{
	#region private fields

		private static readonly Lazy<ExStoreApp> instance =
			new Lazy<ExStoreApp>(() => new ExStoreApp());

		public const string SCHEMA_NAME = "CellsAppData";
		public const string SCHEMA_DESC = "Excel Cells to Revit Exchange";
		public const string DEVELOPER_NAME = "CyberStudio";

	#endregion

	#region ctor

		private ExStoreApp() { }

	#endregion

	#region public properties

		public static ExStoreApp Instance => instance.Value;

		public string Name => SCHEMA_NAME;
		public string Description => SCHEMA_DESC;
		public string Developer => DEVELOPER_NAME;
		public Guid ExStoreGuid => SchemaGuidManager.AppGuid;

		// the schema fields
		public  SchemaDefinitionApp SchemaDefinition { get; }  = SchemaDefinitionApp.Instance;

		public SchemaDictionaryApp FieldDefs => SchemaDefinition.Fields;

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
			return "this is ExStoreApp";
		}

	#endregion
	}
}