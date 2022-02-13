// Solution:     SharedCode
// Project:     SharedCode
// File:             ASchemaData.cs
// Created:      2021-10-19 (6:13 AM)

using System;
using System.Collections.Generic;
using SharedCode.Fields.SchemaInfo.SchemaData.DataTemplate;
using SharedCode.Fields.SchemaInfo.SchemaFields.FieldsTemplates;
using SharedCode.Fields.SchemaInfo.SchemaSupport;


namespace SharedCode.Fields.SchemaInfo.SchemaData.DataTemplates
{
	public abstract class ADataTempBase<TSk>
		where TSk : Enum, new()
	{
		protected int dataIndexMaxAllowed = 0;
		protected int dataIndexCurrent = 0;

		public ADataTempBase(AFieldsTemp<TSk> fields, int maxIndex = 1)
		{
			this.FieldsData = fields;

			ListOfDataDictionaries = new List<Dictionary<TSk, ADataMembers<TSk>>>(1);

			ListAdd(maxIndex);
		}

		public abstract KeyValuePair<SchemaDataStorType, string> DataStorType { get; }

		public int DataIndex
		{
			get => dataIndexCurrent;
			set
			{
				if (value > dataIndexMaxAllowed) return;
				dataIndexCurrent = value;
			}
		}
		public int DataIndexMaxAllowed => dataIndexMaxAllowed;
		public abstract string SchemaName { get; }
		public abstract string SchemaDesc { get; }
		public abstract string SchemaVersion { get; }
		public abstract string SchemaCreateDate { get; }

		// data (not fields)
		public List<Dictionary<TSk, ADataMembers<TSk>>> ListOfDataDictionaries { get; set; }

		public Dictionary<TSk, ADataMembers<TSk>> Data
		{
			get => ListOfDataDictionaries[DataIndex];
			protected set => ListOfDataDictionaries[DataIndex] = value;
		}

		// fields (not data)
		public AFieldsTemp<TSk> FieldsData { get; }

		public Dictionary<TSk, AFieldsMembers<TSk>> Fields => FieldsData.Fields;

		public AFieldsMembers<TSk> GetField(TSk key)
		{
			return ListOfDataDictionaries[DataIndex][key].AFieldsMembers;
		}

		public AFieldsMembers<TSk> GetField(int idx, TSk key)
		{
			return ListOfDataDictionaries[idx][key].AFieldsMembers;
		}

		public abstract void Configure(string name = null);

		public ADataMembers<TSk> this[TSk key]
		{
			get
			{
				if (!ListOfDataDictionaries[DataIndex].ContainsKey(key)) return null;
				return ListOfDataDictionaries[DataIndex][key];
			}
		}

		public ADataMembers<TSk> this[int idx, TSk key]
		{
			get
			{
				if (!ListOfDataDictionaries[idx].ContainsKey(key)) return null;
				return ListOfDataDictionaries[idx][key];
			}
		}

		public void ListAdd(int qty)
		{
			if (qty < 1) return;

			for (int i = 0; i < qty; i++)
			{
				ListOfDataDictionaries.Add(new Dictionary<TSk, ADataMembers<TSk>>());
				dataIndexMaxAllowed += 1;
			}
		}

		// needs methods to adjust collection?
		// maybe remove from abstract class and move to concrete class
		// as this applies only to the cell class

		public void ListRemoveAt(int idx = 0)
		{
			if (idx == 0) return;

			ListOfDataDictionaries.RemoveAt(idx);
		}

		public void ListRemoveLast() { }

		public Type GetValueType(TSk key)
		{
			if (!ListOfDataDictionaries[DataIndex].ContainsKey(key)) return null;
			return ListOfDataDictionaries[DataIndex][key].ValueType;
		}

		public Type GetValueType(TSk key, int idx)
		{
			if (!ListOfDataDictionaries[idx].ContainsKey(key)) return null;
			return ListOfDataDictionaries[idx][key].ValueType;
		}

		public T GetValue<T>(TSk key)
		{
			return ((DataMembers<TSk, T>) ListOfDataDictionaries[dataIndexCurrent][key]).Value;
		}

		public T GetValue<T>(TSk key, int idx)
		{
			return ((DataMembers<TSk, T>) ListOfDataDictionaries[idx][key]).Value;
		}

		public void SetValue<T>(TSk key, T value)
		{
			((DataMembers<TSk, T>) ListOfDataDictionaries[dataIndexCurrent][key]).Value = value;
		}

		public void SetValue<T>(TSk key, T value, int idx)
		{
			((DataMembers<TSk, T>) ListOfDataDictionaries[idx][key]).Value = value;
		}

		public void Add<T>(TSk key, T value)
		{
			ListOfDataDictionaries[dataIndexCurrent].Add(key,
				new DataMembers<TSk, T>(value, FieldsData.GetField<T>(key)));
		}

		public void Add<T>(TSk key, T value, int idx)
		{
			ListOfDataDictionaries[idx].Add(key,
				new DataMembers<TSk, T>(value, FieldsData.GetField<T>(key)));
		}

		public void AddDefault<T>(TSk key)
		{
			FieldsTemp<TSk, T> f = FieldsData.GetField<T>(key);

			ListOfDataDictionaries[dataIndexCurrent].Add(key,
				new DataMembers<TSk, T>(f.Value, f));
		}

		public void AddDefault<T>(TSk key, int idx)
		{
			FieldsTemp<TSk, T> f = FieldsData.GetField<T>(key);

			ListOfDataDictionaries[idx].Add(key,
				new DataMembers<TSk, T>(f.Value, f));
		}

		public DataMembers<TSk, T> GetDefaultData<T>(TSk key)
		{
			DataMembers<TSk, T> d =
				new DataMembers<TSk, T>(FieldsData.GetField<T>(key).Value,
					FieldsData.GetField<T>(key));

			return d;
		}


	}
}