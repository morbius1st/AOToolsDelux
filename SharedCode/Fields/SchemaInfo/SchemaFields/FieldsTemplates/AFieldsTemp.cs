using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using SharedCode.Fields.SchemaInfo.SchemaSupport;
using SharedCode.Fields.SchemaInfo.SchemaDefinitions;
using SharedCode.Windows;
using static SharedCode.Windows.ColData;

using SharedCode.Fields.SchemaInfo.SchemaFields.FieldsTemplates;

// Solution:     SharedCode
// Project:     SharedCode
// File:          ISchemaDef.cs
// Created:      2021-07-11 (2:40 PM)


// defines the class for a collection of fields

namespace SharedCode.Fields.SchemaInfo.SchemaFields.FieldsTemplates
{

	// schema fields (not data fields)
	public abstract class AFieldsTemp<TE>
		where TE : Enum, new()
	{
		public abstract KeyValuePair<SchemaDataStorType, string> FieldStorType { get; }

		public string SchemaName { get; protected set; }

		public TE[] FieldOrderDefault { get; set; }

		public FieldsTempDictionary<TE> Fields { get; protected set; }

		public AFieldsMembers<TE> this[TE key] => Fields[key];

		// public  FieldsTemp<TE, dynamic> this[TE key] => (FieldsTemp<TE, dynamic>) Fields[key];

		public FieldsTemp<TE, TD> GetField<TD>(TE key)
		{
			return (FieldsTemp<TE, TD>) Fields[key];
		}

		public T GetValue<T>(TE key)
		{
			return ((FieldsTemp<TE, T>) Fields[key]).Value;
		}

		public void SetValue<TD>(TE key, TD value)
		{
			((FieldsTemp<TE, TD>) Fields[key]).Value = value;
		}

		protected TE defineField<TD>(TE key,
			string name,
			string desc,
			TD val,
			SchemaFieldDisplayLevel dispLvl,
			string dispOrder,
			int colWidth,
			int ttlWidth,
			JustifyHoriz jh,
			JustifyHoriz jv,
			FieldUnitType unittype = FieldUnitType.UT_UNDEFINED)
		{
			Fields.Add(key, new FieldsTemp<TE, TD>(key, name, desc, val, dispLvl, dispOrder, 
				new ColData(colWidth, ttlWidth, jh, jv), unittype));

			return key;
		}

		protected abstract void defineFields();

	}
}