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
	public abstract class ADataTempBase<TE>
		where TE : Enum, new()
	{
		protected int dataIndexMaxAllowed = 0;
		protected int dataIndexCurrent = 0;

		public ADataTempBase(AFieldsTemp<TE> fields, int maxIndex = 1)
		{
			// dataIndexMaxAllowed = maxIndex;

			this.FieldsData = fields;

			ListOfDataDictionaries = new List<Dictionary<TE, ADataMembers<TE>>>(1);

			ListAdd(maxIndex);
		}

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
		public List<Dictionary<TE, ADataMembers<TE>>> ListOfDataDictionaries { get; set; }

		public Dictionary<TE, ADataMembers<TE>> Data
		{
			get => ListOfDataDictionaries[DataIndex];
			protected set => ListOfDataDictionaries[DataIndex] = value;
		}

		// fields (not data)
		public AFieldsTemp<TE> FieldsData { get; }
		public Dictionary<TE, AFieldsMembers<TE>> Fields => FieldsData.Fields;

		public AFieldsMembers<TE> GetField(TE key)
		{
			return ListOfDataDictionaries[DataIndex][key].AFieldsMembers;
		}

		public AFieldsMembers<TE> GetField(int idx, TE key)
		{
			return ListOfDataDictionaries[idx][key].AFieldsMembers;
		}

		public abstract void Configure(string name = null);

		public ADataMembers<TE> this[TE key]
		{
			get
			{
				if (!ListOfDataDictionaries[DataIndex].ContainsKey(key)) return null;
				return ListOfDataDictionaries[DataIndex][key];
			}
		}

		public ADataMembers<TE> this[int idx, TE key]
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
				ListOfDataDictionaries.Add(new Dictionary<TE, ADataMembers<TE>>());
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

		public Type GetValueType(TE key)
		{
			if (!ListOfDataDictionaries[DataIndex].ContainsKey(key)) return null;
			return ListOfDataDictionaries[DataIndex][key].ValueType;
		}

		public Type GetValueType(TE key, int idx)
		{
			if (!ListOfDataDictionaries[idx].ContainsKey(key)) return null;
			return ListOfDataDictionaries[idx][key].ValueType;
		}

		public T GetValue<T>(TE key)
		{
			return ((DataMembers<TE, T>) ListOfDataDictionaries[dataIndexCurrent][key]).Value;
		}

		public T GetValue<T>(TE key, int idx)
		{
			return ((DataMembers<TE, T>) ListOfDataDictionaries[idx][key]).Value;
		}

		public void SetValue<T>(TE key, T value)
		{
			((DataMembers<TE, T>) ListOfDataDictionaries[dataIndexCurrent][key]).Value = value;
		}

		public void SetValue<T>(TE key, T value, int idx)
		{
			((DataMembers<TE, T>) ListOfDataDictionaries[idx][key]).Value = value;
		}

		public void Add<T>(TE key, T value)
		{
			ListOfDataDictionaries[dataIndexCurrent].Add(key,
				new DataMembers<TE, T>(value, FieldsData.GetField<T>(key)));
		}

		public void Add<T>(TE key, T value, int idx)
		{
			ListOfDataDictionaries[idx].Add(key,
				new DataMembers<TE, T>(value, FieldsData.GetField<T>(key)));
		}

		public void AddDefault<T>(TE key)
		{
			FieldsTemp<TE, T> f = FieldsData.GetField<T>(key);

			ListOfDataDictionaries[dataIndexCurrent].Add(key,
				new DataMembers<TE, T>(f.Value, f));
		}

		public void AddDefault<T>(TE key, int idx)
		{
			FieldsTemp<TE, T> f = FieldsData.GetField<T>(key);

			ListOfDataDictionaries[idx].Add(key,
				new DataMembers<TE, T>(f.Value, f));
		}

		public DataMembers<TE, T> GetDefaultData<T>(TE key)
		{
			DataMembers<TE, T> d =
				new DataMembers<TE, T>(FieldsData.GetField<T>(key).Value,
					FieldsData.GetField<T>(key));

			return d;
		}


	}
}