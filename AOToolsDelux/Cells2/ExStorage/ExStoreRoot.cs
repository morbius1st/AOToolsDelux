#region using

using System;
using AOTools.Cells2.SchemaCells;
using AOTools.Cells2.ExStorage;
using AOTools.Cells2.SchemaDefinition;

#endregion

// username: jeffs
// created:  7/4/2021 3:54:15 PM

namespace AOTools.Cells2.ExStorage
{
	public class ExStoreRoot : IExStore, IExStoreData<SchemaDictionaryRoot, SchemaDictionaryRoot>
	{
	#region private fields

	#endregion

	#region ctor

		private ExStoreRoot()
		{
			Initialize();
		}

	#endregion

	#region public properties

		public string Name => SchemaDefRoot.ROOT_SCHEMA_NAME;
		public string Description => SchemaDefRoot.ROOT_SCHEMA_DESC;
		public string Developer => SchemaDefRoot.ROOT_DEVELOPER_NAME;
		public Guid ExStoreGuid => SchemaGuidManager.RootGuid;

		public SchemaDictionaryRoot Data { get; private set; }

		public bool IsInitialized { get; private set; }

		public static SchemaDefRoot SchemaDef { get; }  = new SchemaDefRoot();

		public SchemaDictionaryRoot FieldDefs => SchemaDef.DefaultFields;

		public Enum[] KeyOrder => SchemaDef.KeyOrderX;

	#endregion

	#region private properties

	#endregion

	#region public methods

		public static ExStoreRoot Instance()
		{
			return new ExStoreRoot();
		}

		public void Initialize()
		{
			Data = DefaultValues();

			IsInitialized = true;
		}

		// set the default values
		// the default values are those used in the schema field
		// definition so only need to clone the schema field def
		public SchemaDictionaryRoot DefaultValues()
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
			return "this is ExStoreRoot";
		}

	#endregion
	}
}