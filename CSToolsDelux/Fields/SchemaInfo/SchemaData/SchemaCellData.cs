#region using

using System;
using System.Collections.Generic;
using CSToolsDelux.Fields.SchemaInfo.SchemaData.SchemaDataDefinitions;
using SharedCode.Fields.SchemaInfo.SchemaSupport;
using SharedCode.Fields.SchemaInfo.SchemaFields.FieldsTemplates;
using SharedCode.Fields.SchemaInfo.SchemaData.DataTemplate;
using SharedCode.Fields.SchemaInfo.SchemaFields;
using static SharedCode.Fields.SchemaInfo.SchemaSupport.SchemaCellKey;

#endregion

// username: jeffs
// created:  8/28/2021 10:10:07 PM

namespace CSToolsDelux.Fields.SchemaInfo.SchemaData
{
	public class SchemaCellData :
		ISchemaData<SchemaCellKey, SchemaDataDictCell,  FieldsTempDictionary<SchemaCellKey>>
	{
	#region private fields

		private static FieldsCell fieldsCell;

	#endregion

	#region ctor

		public SchemaCellData()
		{
			Initialize();
		}

	#endregion

	#region public properties

		public string DocumentName { get; set; }
		public string DsKey { get; set; }
		public int Index { get; set; } = 0;

		// public SchemaCellFields FieldsDefinition => cellFields;

		public override FieldsTempDictionary<SchemaCellKey> Fields =>  fieldsCell.Fields;

		public List<SchemaDataDictCell> DataList { get; set; }

		public override SchemaDataDictCell Data
		{
			get => DataList[Index];
			protected set => DataList[Index] = value; 
		}

		public Guid ExStorCellGuid => Guid.Empty;

		public bool IsInitialized { get; private set; }

	#endregion

	#region private properties

	#endregion

	#region public methods

		public void Initialize()
		{
			if (IsInitialized) return;

			fieldsCell = new FieldsCell();

			DataList = new List<SchemaDataDictCell>();

			IsInitialized = true;
		}

		public void Configure(string name, string seq, UpdateRules ur,
			string cellFamName, bool skip, string xlFilePath, string xlWrkShtName)
		{
			int Index = 0;

			DataList.Add(MakeDefaultCellData());

			SetValue( CK_SCHEMA_NAME, name);
			SetValue( CK_CREATE_DATE, DateTime.UtcNow.ToString());
			SetValue( CK_SEQUENCE, seq);
			SetValue( CK_UPDATE_RULE, (int) ur);
			SetValue( CK_CELL_FAMILY_NAME, cellFamName);
			SetValue( CK_SKIP, skip);
			SetValue( CK_XL_FILE_PATH, xlFilePath);
			SetValue( CK_XL_WORKSHEET_NAME, xlWrkShtName);
		}


		public override TD GetValue<TD>( SchemaCellKey key)
		{
			return ((CellData<TD>) Data[key]).Value;
		}

		public override void SetValue<TD>(  SchemaCellKey key, TD value)
		{
			((CellData<TD>) Data[key]).Value = value;
		}

		public override void Add<TD>(SchemaCellKey key, TD value)
		{
			Data.Add(key,
				new CellData<TD>(value, fieldsCell.GetField<TD>(key)));
		}

		public override void AddDefault<TD>(SchemaCellKey key)
		{
			FieldsTemp< SchemaCellKey,TD> f = fieldsCell.GetField<TD>(key);

			Data.Add(key,
				new CellData<TD>(f.Value, f));
		}

		public CellData<TD> GetDefaultData<TD>( SchemaCellKey key)
		{
			CellData<TD> data = new CellData<TD>(fieldsCell.GetField<TD>(key).Value,
				fieldsCell.GetField<TD>(key));

			return data;
		}

		// public SchemaDictionaryCell DefValues()
		// {
		// 	return Fields.Clone();
		// }

		public void AddDefaultData(int qty)
		{
			for (int i = Data.Count; i < qty; i++)
			{
				DataList.Add(new SchemaDataDictCell());
				Index = i;
				Data = MakeDefaultCellData();
			}
		}

		private SchemaDataDictCell MakeDefaultCellData()
		{
			SchemaDataDictCell data = new SchemaDataDictCell();

			for (int j = 0; j < fieldsCell.FieldOrderDefault.Length; j++)
			{
				SchemaCellKey key = fieldsCell.FieldOrderDefault[j];

				Type t = fieldsCell[key].ValueType;

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
			int count = DataList?.Count ?? 1;
			if (count < 1) return null;

			List<SchemaDataDictCell> copy =
				new List<SchemaDataDictCell>(count);

			for (int i = 0; i < count; i++)
			{
				copy.Add(new SchemaDataDictCell());

				foreach (KeyValuePair<SchemaCellKey, 
					ADataMembers<SchemaCellKey>> kvp in DataList[i])
				{
					if (kvp.Value.ValueType == typeof(string))
					{
						copy[i].Add(kvp.Key, new CellData<SchemaCellKey>(((CellData<SchemaCellKey>)kvp.Value).Value, kvp.Value.AFieldsMembers));
					}
				}
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