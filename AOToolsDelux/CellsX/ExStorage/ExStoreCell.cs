#region using

using System;
using System.Collections.Generic;
using AOToolsDelux.Cells.SchemaCells;
using AOToolsDelux.Cells.SchemaDefinition;

#endregion

// username: jeffs
// created:  7/4/2021 3:54:32 PM

namespace AOToolsDelux.Cells.ExStorage
{

	public class ExStoreCell : IExStore<SchemaCellKey, SchemaDictionaryCell>
	{
	#region private fields

		private static readonly Lazy<ExStoreCell> instance =
			new Lazy<ExStoreCell>(() => new ExStoreCell());

		public const string SCHEMA_NAME = "CellDefaultDefinition";
		public const string SCHEMA_DESC = "Default Root Cells Definition";
		public const string NOTDEFINED = "<not defined>";

	#endregion

	#region ctor

		private ExStoreCell()
		{
			IsInitialized = true;
			// Initialize();
		}

	#endregion

	#region public properties

		public static ExStoreCell Instance => instance.Value;

		public string Name => SCHEMA_NAME;
		public string Description => SCHEMA_DESC;

		// this is the schema definition and fields
		public SchemaDefinitionCell SchemaDefinition { get; }  = SchemaDefinitionCell.Instance;

		// // this is the list of cell data
		// public List<SchemaDictionaryCell> Data { get; private set; }

		// public Dictionary<string, string> SubSchemaFields { get; set; }

		public SchemaDictionaryCell FieldDefs => SchemaDefinition.Fields;

		public Guid ExStoreGuid => Guid.Empty;

		public bool IsInitialized { get; private set; }

	#endregion

	#region private properties

	#endregion

	#region public methods

		public void Initialize(int count)
		{
			if (IsInitialized) return;

			// initData(count);

			IsInitialized = true;
		}

		// set the default values
		// the default values are those used in the schema field
		// definition so only need to clone the schema field def
		// public SchemaDictionaryCell DefaultValues()
		// {
		// 	return FieldDefs.Clone();
		// }
		//
		// public void AddDefault()
		// {
		// 	Data.Add(DefaultValues());
		// }

	#endregion

	#region private methods

		// private void initData(int count)
		// {
		// 	Data = new List<SchemaDictionaryCell>();
		//
		// 	for (int i = 0; i < count; i++)
		// 	{
		// 		AddDefault();
		// 	}
		// }

	#endregion

	#region event consuming

	#endregion

	#region event publishing

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is ExStoreCell";
		}

	#endregion
	}
}