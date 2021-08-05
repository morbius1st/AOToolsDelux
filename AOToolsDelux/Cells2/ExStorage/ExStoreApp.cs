#region using

using System;
using AOTools.Cells2.SchemaCells;
using AOTools.Cells2.SchemaDefinition;

#endregion

// username: jeffs
// created:  7/4/2021 3:54:15 PM

namespace AOTools.Cells2.ExStorage
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

		public string Name => SchemaDefApp.SCHEMA_NAME;
		public string Description => SchemaDefApp.SCHEMA_DESC;
		public string Developer => SchemaDefApp.DEVELOPER_NAME;
		public Guid ExStoreGuid => SchemaGuidManager.AppGuid;

		public SchemaDictionaryApp Data { get; private set; }

		public bool IsInitialized { get; private set; }

		public static SchemaDefApp SchemaDef { get; }  = new SchemaDefApp();

		public SchemaDictionaryApp FieldDefs => SchemaDef.DefaultFields;

		public Enum[] KeyOrder => SchemaDef.KeyOrderX;

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
			return SchemaDef.DefaultFields.Clone();
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