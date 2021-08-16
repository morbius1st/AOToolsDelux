#region using
using System;
using AOTools.Cells.ExStorage;
using AOTools.Cells.SchemaDefinition;
using AOTools.Cells.SchemaDefinition2;
using SchemaDefinitionRoot = AOTools.Cells.SchemaDefinition2.SchemaDefinitionRoot;

#endregion

// username: jeffs
// created:  7/4/2021 3:54:15 PM

namespace AOTools.Cells.ExStorage
{
	public class ExStoreRoot : IExStore, IExStoreData<SchemaDictionaryRoot2, SchemaDictionaryRoot2>
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

		public string Name
		{
			get => Data?[SchemaRootKey.NAME]?.AsString() ?? SchemaDefinitionRoot.ROOT_SCHEMA_NAME;
			set => Data[SchemaRootKey.NAME].Set(value);
				// ((SchemaFieldDef2<SchemaRootKey, string>) Data[SchemaRootKey.NAME]).Value = value;

		}

		public string Description
		{
			get => Data?[SchemaRootKey.DESCRIPTION]?.AsString() ?? SchemaDefinitionRoot.ROOT_SCHEMA_DESC;
			set => Data[SchemaRootKey.DESCRIPTION].Set(value);
			
		}

		public string Developer => Data?[SchemaRootKey.DEVELOPER]?.AsString() ??SchemaDefinitionRoot.ROOT_DEVELOPER_NAME;
		public string Version => Data?[SchemaRootKey.VERSION]?.AsString() ??SchemaDefinitionRoot.ROOT_SCHEMA_VER;
		public Guid ExStoreGuid => SchemaGuidManager.RootGuid;

		public SchemaDictionaryRoot2 Data => SchemaDefinition.Fields;

		public bool IsInitialized { get; private set; }

		public SchemaDefinitionRoot SchemaDefinition => SchemaDefinitionRoot.Instance;


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
			// Data = DefaultValues();

			IsInitialized = true;
		}

		// set the default values
		// the default values are those used in the schema field
		// definition so only need to clone the schema field def
		public SchemaDictionaryRoot2 DefaultValues()
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