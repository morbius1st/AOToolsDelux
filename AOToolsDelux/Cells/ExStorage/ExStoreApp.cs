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
		
		public string Name => Data?[SchemaAppKey.AK_NAME]?.Value ?? SchemaDefinitionApp.SCHEMA_NAME;
		public string Description => Data?[SchemaAppKey.AK_DESCRIPTION]?.Value ?? SchemaDefinitionApp.SCHEMA_DESC;
		public string Version => Data?[SchemaAppKey.AK_VERSION]?.Value ?? SchemaDefinitionApp.SCHEMA_VER;
		public Guid ExStoreGuid => SchemaGuidManager.AppGuid;
		public SchemaDefinitionApp SchemaDefinition => SchemaDefinitionApp.Instance;

		public SchemaDictionaryApp Data => SchemaDefinition.Fields;

		public SchemaAppKey[] KeyOrder => SchemaDefinition.KeyOrder;

		public bool IsDefault { get; set; }
		public bool IsInitialized { get; private set; }


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
			IsDefault = true;
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