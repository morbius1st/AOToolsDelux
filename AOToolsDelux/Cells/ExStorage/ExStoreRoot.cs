#region using
using System;
using AOTools.Cells.SchemaDefinition;

#endregion

// username: jeffs
// created:  7/4/2021 3:54:15 PM

namespace AOTools.Cells.ExStorage
{
	public class ExStoreRoot : IExStore, IExStoreData<SchemaDictionaryRoot, SchemaDictionaryRoot>
	{
	#region private fields

		private Guid exStoreGuid = SchemaGuidManager.RootGuid;

	#endregion

	#region ctor

		private ExStoreRoot()
		{
			Initialize();
		}

	#endregion

	#region public properties

		public string Name => Data?[SchemaRootKey.RK_NAME]?.Value ?? SchemaDefinitionRoot.ROOT_SCHEMA_NAME;
		public string Description => Data?[SchemaRootKey.RK_DESCRIPTION]?.Value ??SchemaDefinitionRoot.ROOT_SCHEMA_DESC;
		public string Developer => Data?[SchemaRootKey.RK_DEVELOPER]?.Value ??SchemaDefinitionRoot.ROOT_DEVELOPER_NAME;
		public string Version => Data?[SchemaRootKey.RK_VERSION]?.Value ??SchemaDefinitionRoot.ROOT_SCHEMA_VER;
		public Guid AppExStoreGuid => Data[SchemaRootKey.RK_APP_GUID].Value;
		public Guid ExStoreGuid
		{
			get => exStoreGuid;
			private set => exStoreGuid = value;
		}

		public SchemaDictionaryRoot Data => SchemaDefinition.Fields;

		public SchemaDefinitionRoot SchemaDefinition => SchemaDefinitionRoot.Instance;


		public bool IsDefault { get; set; }
		public bool IsInitialized { get; private set; }

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
			Data[SchemaRootKey.RK_APP_GUID].Value = SchemaGuidManager.AppGuidString;

			IsDefault = true;
			IsInitialized = true;
		}

		// set the default values
		// the default values are those used in the schema field
		// definition so only need to clone the schema field def
		public SchemaDictionaryRoot DefaultValues()
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
			return "this is ExStoreRoot";
		}

	#endregion
	}
}