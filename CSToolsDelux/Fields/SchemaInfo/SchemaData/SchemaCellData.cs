#region using

using System;
using System.Collections.Generic;
using Autodesk.Revit.DB.Analysis;
using CSToolsDelux.Fields.SchemaInfo.SchemaData.SchemaDataDefinitions;
using CSToolsDelux.Fields.SchemaInfo.SchemaDefinitions;
using CSToolsDelux.Fields.SchemaInfo.SchemaFields;
using static CSToolsDelux.Fields.SchemaInfo.SchemaDefinitions.SchemaCellKey;

#endregion

// username: jeffs
// created:  8/28/2021 10:10:07 PM

namespace CSToolsDelux.Fields.SchemaInfo.SchemaData
{
	public class SchemaCellData
	{
	#region private fields

		private SchemaCellFields cellFields;

	#endregion

	#region ctor

		public SchemaCellData()
		{
			Initialize();
		}

	#endregion

	#region public properties

		public SchemaCellFields FieldsDefinition => cellFields;

		public SchemaDictionaryCell Fields => (SchemaDictionaryCell) cellFields.Fields;

		public List<SchemaDataDictCell> Data { get; private set; }

		public Guid ExStorCellGuid => Guid.Empty;

		public bool IsInitialized { get; private set; }


		public TD GetValue<TD>(int dataSet, SchemaCellKey key)
		{
			return ((SchemaCellDataField<TD>) Data[dataSet][key]).Value;
		}
		
		public void SetValue<TD>(int dataSet, SchemaCellKey key, TD value)
		{
			((SchemaCellDataField<TD>) Data[dataSet][key]).Value = value;
		}

		public void Add<TD>(int dataSet, SchemaCellKey key, TD value)
		{
			Data[dataSet].Add(key, 
				new SchemaCellDataField<TD>(value, cellFields.GetField<TD>(key)));
		}

		public SchemaCellDataField<TD> GetDefaultData<TD>( SchemaCellKey key)
		{
			SchemaCellDataField<TD> data = new SchemaCellDataField<TD>(cellFields.GetField<TD>(key).Value,
				cellFields.GetField<TD>(key));

			return data;
		}

	#endregion

	#region private properties

	#endregion

	#region public methods

		public void Initialize()
		{
			if (IsInitialized) return;

			cellFields = new SchemaCellFields();

			Data = new List<SchemaDataDictCell>();

			IsInitialized = true;
		}

		public void Configure(string name, string seq, UpdateRules ur, 
			string cellFamName, bool skip, string xlFilePath, string xlWrkShtName)
		{
			int dataSet = Data.Count;

			Data.Add(MakeDefaultCellData());

			SetValue(dataSet, CK_NAME, name);
			SetValue(dataSet, CK_SEQUENCE, seq);
			SetValue(dataSet, CK_UPDATE_RULE, (int) ur);
			SetValue(dataSet, CK_CELL_FAMILY_NAME, cellFamName);
			SetValue(dataSet, CK_SKIP, skip);
			SetValue(dataSet, CK_XL_FILE_PATH, xlFilePath);
			SetValue(dataSet, CK_XL_WORKSHEET_NAME, xlWrkShtName);
		}

		public SchemaDictionaryCell DefValues()
		{
			return Fields.Clone();
		}

		public void AddDefaultData(int qty)
		{
			for (int i = Data.Count; i < qty; i++)
			{
				Data.Add(new SchemaDataDictCell());

				Data[i] = MakeDefaultCellData();
			}
		}

		private SchemaDataDictCell MakeDefaultCellData()
		{
			SchemaDataDictCell data = new SchemaDataDictCell();

			for (int j = 0; j < cellFields.KeyOrder.Length; j++)
			{
				SchemaCellKey key = cellFields.KeyOrder[j];

				Type t = cellFields[key].ValueType;

				if (t == typeof(string))
				{
					data.Add(key, GetDefaultData<string>(key));
				}
				else if (t == typeof(double))
				{
					data.Add(key, GetDefaultData<double>(key));
				}
				else if (t == typeof(bool))
				{
					data.Add(key, GetDefaultData<bool>(key));
				}
				else if (t == typeof(int))
				{
					data.Add(key, GetDefaultData<int>(key));
				}
			}
			return data;
		} 

	#endregion

	#region private methods

		private List<SchemaDataDictCell> cloneData()
		{
			List<SchemaDataDictCell> copy = 
				new List<SchemaDataDictCell>(Data?.Count ?? 1);

			foreach (SchemaDataDictCell data in Data)
			{
				
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
			return "this is SchemaCellData";
		}

	#endregion
	}
}