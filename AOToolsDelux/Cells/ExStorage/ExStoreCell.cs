#region using
using System;
using System.Collections.Generic;
using AOTools.Cells.SchemaCells;

#endregion

// username: jeffs
// created:  7/4/2021 3:54:32 PM

namespace AOTools.Cells.ExStorage
{
	public class ExStoreCell : IExStore, IExStoreData<SchemaDictionaryCell, List<SchemaDictionaryCell>> //: SchemaDefCells, 
	{
	#region private fields

	#endregion

	#region ctor

		private ExStoreCell(/*int count*/)
		{
			Initialize(/*count*/);
		}

	#endregion

	#region public properties

		public string Name => SchemaDefinitionCells.SCHEMA_NAME;
		public string Description => SchemaDefinitionCells.SCHEMA_DESC;

		// this is the schema definition and fields
		public SchemaDefinitionCells SchemaDefinition => SchemaDefinitionCells.Instance;

		// this is the list of cell data
		public List<SchemaDictionaryCell> Data { get; private set; }

		public Dictionary<string, string> SubSchemaFields { get; set; }

		public SchemaDictionaryCell Fields => SchemaDefinition.Fields;

		public Guid ExStoreGuid => Guid.Empty;

		public bool IsDefault { get; set; }
		public bool IsInitialized { get; private set; }

	#endregion

	#region private properties

	#endregion

	#region public methods

		public static ExStoreCell Instance(/*int count*/)
		{
			return new ExStoreCell(/*count*/);
		}

		public void Initialize(/*int count*/)
		{
			if (IsInitialized) return;

			// initData(/*count*/);

			Data = new List<SchemaDictionaryCell>();

			IsDefault = true;
			IsInitialized = true;
		}

		// set the default values
		// the default values are those used in the schema field
		// definition so only need to clone the schema field def
		public SchemaDictionaryCell DefaultValues()
		{
			return Fields.Clone();
		}

		public void AddDefault()
		{
			Data.Add(DefaultValues());
		}

		public void Add(int qty)
		{
			for (int i = 0; i < qty; i++)
			{
				AddDefault();
			}
		}

		public ExStoreCell Clone()
		{
			ExStoreCell copy = new ExStoreCell(/*Data.Count*/);

			copy.Data = cloneData();
			copy.IsInitialized = IsInitialized;

			return copy;
		}

	#endregion

	#region private methods

/*
		private void initData(*//*int count*//*)
		{
			Data = new List<SchemaDictionaryCell>();

			for (int i = 0; i < count; i++)
			{
				AddDefault();
			}
		}
*/
		private List<SchemaDictionaryCell> cloneData()
		{
			List<SchemaDictionaryCell> copy = 
				new List<SchemaDictionaryCell>(Data.Count);

			foreach (SchemaDictionaryCell data in Data)
			{
				copy.Add(data.Clone());
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
			return "this is ExStoreCell";
		}

	#endregion
	}
}