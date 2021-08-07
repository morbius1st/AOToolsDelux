#region using
using System;
using System.Collections.Generic;
using AOTools.Cells.SchemaDefinition;

#endregion

// username: jeffs
// created:  7/4/2021 3:54:15 PM

namespace AOTools.Cells.ExStorage
{
	public class ExStoreRoot2 : IExStore2<SchemaRootKey, ISchemaFieldDef<SchemaRootKey>>
	{
	#region private fields

		private static readonly Lazy<ExStoreRoot2> instance =
			new Lazy<ExStoreRoot2>(() => new ExStoreRoot2());

	#endregion

	#region ctor

		private ExStoreRoot2()
		{
			Initialize();
		}

	#endregion

	#region public properties

		public static ExStoreRoot2 Instance => instance.Value;

		public string Name => SchemaDefinitionRoot.ROOT_SCHEMA_NAME;
		public string Description => SchemaDefinitionRoot.ROOT_SCHEMA_DESC;
		public string Developer => SchemaDefinitionRoot.ROOT_DEVELOPER_NAME;
		public Guid ExStoreGuid => SchemaGuidManager.RootGuid;

		public Dictionary<SchemaRootKey, SchemaFieldDef<SchemaRootKey>> Data { get; private set; }

		public bool IsInitialized { get; private set; }

		public static SchemaDefinitionRoot SchemaDefinition { get; }  = SchemaDefinitionRoot.Instance;

		public Dictionary<SchemaRootKey, ISchemaFieldDef<SchemaRootKey>> FieldDefs => SchemaDefinition.Fields;

	#endregion

	#region private properties

	#endregion

	#region public methods

		public ExData<T> GetVal<T>(SchemaRootKey idx)
		{
			SchemaFieldDef<SchemaRootKey> z = Data[idx];
			IExData a = z.ExValue;
			ExData<T> b = (ExData<T>) a;

			return ((ExData<T>) Data[idx].ExValue);
		}
		
		// public static ExStoreRoot2 Instance()
		// {
		// 	return new ExStoreRoot2();
		// }

		public void Initialize()
		{
			Data = DefaultValues();

			IsInitialized = true;
		}

		// set the default values
		// the default values are those used in the schema field
		// definition so only need to clone the schema field def
		public Dictionary<SchemaRootKey, SchemaFieldDef<SchemaRootKey>> DefaultValues()
		{
			return copy(FieldDefs);
		}

	#endregion

	#region private methods

		private Dictionary<SchemaRootKey, SchemaFieldDef<SchemaRootKey>>
			copy(Dictionary<SchemaRootKey, ISchemaFieldDef<SchemaRootKey>> fd)
		{
			Dictionary<SchemaRootKey, SchemaFieldDef<SchemaRootKey>> 
				copy = new Dictionary<SchemaRootKey, SchemaFieldDef<SchemaRootKey>>();

			foreach (KeyValuePair<SchemaRootKey, ISchemaFieldDef<SchemaRootKey>> kvp in fd)
			{
				copy.Add(kvp.Key, 
					((SchemaFieldDef<SchemaRootKey>) kvp.Value).Copy());
			}

			return copy;
		}

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




	public class ExStoreRoot : IExStore<SchemaRootKey, SchemaDictionaryRoot>
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

		public string Name => SchemaDefinitionRoot.ROOT_SCHEMA_NAME;
		public string Description => SchemaDefinitionRoot.ROOT_SCHEMA_DESC;
		public string Developer => SchemaDefinitionRoot.ROOT_DEVELOPER_NAME;
		public Guid ExStoreGuid => SchemaGuidManager.RootGuid;

		public SchemaDictionaryRoot Data { get; private set; }

		public bool IsInitialized { get; private set; }

		public static SchemaDefinitionRoot SchemaDefinition { get; }  = SchemaDefinitionRoot.Instance;

		public SchemaDictionaryRoot FieldDefs => SchemaDefinition.Fields;

		public SchemaFieldDef<SchemaRootKey> test => (SchemaFieldDef<SchemaRootKey>) Data[SchemaRootKey.NAME];

		public SchemaFieldDef<SchemaRootKey> this[SchemaRootKey idx] => (((SchemaFieldDef<SchemaRootKey>) Data[idx]));

	#endregion

	#region private properties

	#endregion

	#region public methods

		public ExData<T> GetVal<T>(SchemaRootKey idx)
		{
			return ((ExData<T>)((SchemaFieldDef<SchemaRootKey>) Data[idx]).ExValue);
		}
		
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