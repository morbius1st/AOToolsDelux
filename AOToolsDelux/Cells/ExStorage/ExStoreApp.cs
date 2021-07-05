#region using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using AOTools.Cells.SchemaCells;
using AOTools.Cells.SchemaDefinition;

#endregion

// username: jeffs
// created:  7/4/2021 3:54:15 PM

namespace AOTools.Cells.ExStorage
{
	public class ExStoreApp : SchemaDefApp, IExStoreData<SchemaAppKey, SchemaDictionaryApp>
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

		public static ExStoreApp AppConfig { get; } = new ExStoreApp();

		public string Name => SCHEMA_NAME;
		public string Description => SCHEMA_DESC;
		public string Developer => DEVELOPER_NAME;

		public SchemaDictionaryBase<SchemaAppKey> SchemaFields => DefaultFields;
		public SchemaDictionaryApp Data { get; private set; }

		public bool IsInitialized { get; private set; }

	#endregion

	#region private properties

	#endregion

	#region public methods

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
			return (SchemaDictionaryApp) SchemaFields.Clone();
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