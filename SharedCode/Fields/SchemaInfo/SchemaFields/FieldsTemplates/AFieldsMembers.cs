using System;
using System.Collections.Generic;
using SharedCode.Fields.SchemaInfo.SchemaSupport;
using SharedCode.Fields.SchemaInfo.SchemaDefinitions;
using SharedCode.Windows;

// Solution:     SharedCode
// Project:       SharedCode
// File:             ISchemaFieldDef.cs
// Created:      2022-01-23 (9:25 PM)

// defines the properties of a SchemaField
namespace SharedCode.Fields.SchemaInfo.SchemaFields.FieldsTemplates
{
	public abstract class AFieldsMembers<TE> where TE : Enum
	{
		public abstract KeyValuePair<string, int> Type { get; }
		public abstract TE Key { get; protected set; }
		public abstract int Sequence { get; protected set; }
		public abstract string Name { get; protected set; }
		public abstract string Desc { get; protected set; }
		public abstract FieldUnitType UnitType { get; protected set; }
		public abstract string Guid { get; protected set; }
		public abstract Type ValueType { get; protected set; }
		public abstract string ValueString { get;}
		public abstract SchemaFieldDisplayLevel DisplayLevel { get; protected set; }
		public abstract string DisplayOrder { get; protected set; }
		public abstract ColData ColDisplayData { get; protected set; }

		public abstract AFieldsMembers<TE> Clone();

		public string this[FieldColumns Id]
		{
			get
			{
				switch (Id)
				{
				case FieldColumns.TYPE:        return Type.ToString();
				case FieldColumns.KEY:         return Key.ToString();
				case FieldColumns.SEQUENCE:    return Sequence.ToString();
				case FieldColumns.NAME:        return Name;
				case FieldColumns.DESC:        return Desc;
				case FieldColumns.UNIT_TYPE:   return UnitType.ToString();
				case FieldColumns.GUID:        return Guid;
				case FieldColumns.VALUE_TYPE:  return ValueType.ToString();
				case FieldColumns.VALUE_STR:   return ValueString;
				case FieldColumns.DISP_LEVEL:  return DisplayLevel.ToString();
				case FieldColumns.DISP_ORDER:  return DisplayOrder;
				case FieldColumns.COL_WIDTH:   return ColDisplayData.ColWidth.ToString();
				case FieldColumns.TITLE_WIDTH: return ColDisplayData.TitleWidth.ToString();
				case FieldColumns.JUST_HDR:    return ColDisplayData.Just[0].ToString();
				case FieldColumns.JUST_VAL:    return ColDisplayData.Just[1].ToString();
				default:                       return null;
				}
			}
		}

		public Dictionary<FieldColumns, string> FieldsRowInfo()
		{
			Dictionary<FieldColumns, string> rowInfo = new Dictionary<FieldColumns, string>();

			foreach (FieldColumns key in FieldsTemplateMembers.DefaultFieldsOrder)
			{
				rowInfo.Add(key, this[key]);
			}

			return rowInfo;
		}

	}
}