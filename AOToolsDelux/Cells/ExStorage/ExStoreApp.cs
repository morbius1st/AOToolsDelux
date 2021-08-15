#region using
using System;
using AOTools.Cells.SchemaCells;
using AOTools.Cells.SchemaDefinition;

#endregion

// username: jeffs
// created:  7/4/2021 3:54:15 PM

namespace AOTools.Cells.ExStorage
{
	public class ExStoreApp : IExStore, IExStoreData<SchemaDictionaryApp, SchemaDictionaryApp> //: SchemaDefApp, 
	{
	#region private fields

	#endregion

	#region ctor

		private ExStoreApp()
		{
			Initialize();
		}

	#endregion

	#region public properties

		public string Name => SchemaDefinitionApp.SCHEMA_NAME;
		public string Description => SchemaDefinitionApp.SCHEMA_DESC;
		public Guid ExStoreGuid => SchemaGuidManager.AppGuid;

		public SchemaDictionaryApp Data { get; private set; }

		public bool IsInitialized { get; private set; }

		public SchemaDefinitionApp SchemaDefinition => SchemaDefinitionApp.Instance;

		public SchemaDictionaryApp FieldDefs => SchemaDefinition.Fields;

		public SchemaAppKey[] KeyOrder => SchemaDefinition.KeyOrder;

	#endregion

	#region private properties

	#endregion

	#region public methods

		public static ExStoreApp Instance()
		{
			return new ExStoreApp();
		}

		public void Initialize()
		{
			Data = DefaultValues();

			IsInitialized = true;
		}

		// set the default values
		// the default values are those used in the schema field
		// definition so only need to clone the schema field def
		public SchemaDictionaryApp DefaultValues()
		{
			return SchemaDefinition.Fields.Clone();
		}

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