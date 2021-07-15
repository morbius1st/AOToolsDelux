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
using Autodesk.Revit.DB.ExtensibleStorage;

#endregion

// username: jeffs
// created:  7/4/2021 3:54:32 PM

namespace AOTools.Cells.ExStorage
{
	public class ExStoreCell : IExStore, 
		IExStoreData<SchemaDictionaryCell, List<SchemaDictionaryCell>> //: SchemaDefCells, 
	{
	#region private fields

	#endregion

	#region ctor

		private ExStoreCell(int count)
		{
			Initialize(count);
		}

	#endregion

	#region public properties

		public List<SchemaDictionaryCell> Data { get; private set; }

		public Guid ExStoreGuid => Guid.Empty;

		public string Name => SchemaDefCells.SCHEMA_NAME;
		public string Description => SchemaDefCells.SCHEMA_DESC;

		public bool IsInitialized { get; private set; }

		public Dictionary<string, string> SubSchemaFields { get; set; }


		public static SchemaDefCells SchemaDef { get; } = SchemaDefCells.Inst;

		public SchemaDictionaryCell FieldDefs => (SchemaDictionaryCell) SchemaDef.DefaultFields;


	#endregion

	#region private properties

	#endregion

	#region public methods

		public static ExStoreCell Instance(int count)
		{
			return new ExStoreCell(count);
		}

		public void Initialize(int count)
		{
			if (IsInitialized) return;

			initData(count);

			IsInitialized = true;
		}

		// set the default values
		// the default values are those used in the schema field
		// definition so only need to clone the schema field def
		public SchemaDictionaryCell DefaultValues()
		{
			return FieldDefs.Clone<SchemaDictionaryCell>();
		}

		public void AddDefault()
		{
			Data.Add(DefaultValues());
		}

		
		public string[] GetSubSchemaFieldInfo(int i)
		{
			string[] info =
			{
				$"RootCellsDefinition{i:D3}",
				SchemaGuidManager.GetCellGuidString(i),
				"subschema for a cells definition"
			};

			return info;
		}


	#endregion

	#region private methods

		private void initData(int count)
		{
			Data = new List<SchemaDictionaryCell>();

			for (int i = 0; i < count; i++)
			{
				AddDefault();
			}
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