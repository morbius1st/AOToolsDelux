// Solution:     SharedCode
// Project:     SharedCode
// File:             ASchemaDataFieldDef.cs
// Created:      2022-01-25 (10:37 PM)

using System;
using System.Collections.Generic;
using SharedCode.Fields.SchemaInfo.SchemaSupport;
using static SharedCode.Windows.ColData;
using SharedCode.Fields.SchemaInfo.SchemaData.DataTemplates;
using SharedCode.Fields.SchemaInfo.SchemaFields.FieldsTemplates;
using SharedCode.Windows;

namespace SharedCode.Fields.SchemaInfo.SchemaData.DataTemplate
{
	public abstract class ADataMember<TE> where TE : Enum
	{
	#region data fields

		public abstract TE Key                     { get; protected set; }
		public abstract string ValueString         { get; }
		public abstract Type ValueType             { get; protected set; }
		public abstract IFieldsTemp<TE> FieldsTemp { get; protected set; }

	#endregion

	#region system overrides

		public abstract ADataMember<TE> Clone();

	#endregion

	#region public methods


		// provide access to the data field via a key
		// key is DataColumns.{enum}
		/// <summary>
		/// Provide access to the data fields via a key<br/>
		/// key is <code>DataColumns.{enum}</code>
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>
		public string this[DataColumns Id]
		{
			get
			{
				switch (Id)
				{
				case DataColumns.KEY:          { return Key.ToString();}
				case DataColumns.NAME:         { return FieldsTemp.Name;}
				case DataColumns.VALUE_STR :   { return ValueString;}
				case DataColumns.VALUE_TYPE :  { return ValueType.ToString();}
				case DataColumns.FIELDS_TEMP : { return FieldsTemp.ToString();}
				}
				return null;
			}
		}

		/// <summary>
		/// Return all data fields as a dictionary <code>&lt;DataColumns.{enum}, string&gt;</code><br/>
		/// Fields 
		/// </summary>
		/// <param name="dataOrder"></param>
		/// <returns></returns>
		public Dictionary<DataColumns, string> DataRowInfo()
		{
			Dictionary<DataColumns, string> rowInfo = new Dictionary<DataColumns, string>();

			foreach (DataColumns key in DataTemplateMembers.DefaultDataOrder)
			{
				rowInfo.Add(key, this[key]);
			}

			return rowInfo;
		}
		

	#endregion


/*

		public string AsString()
		{
			return As<string>();
		}

		public double AsDouble()
		{
			return As<double>();
		}

		public int AsInteger()
		{
			return As<int>();
		}
				
		public bool AsBoolean()
		{
			return As<bool>();
		}


		public TT As<TT>()
		{
			return ((DataTemp<TE, TT>) this).Value;
		}

		public static TT As<TT>(ADataTemp<TE> id)
		{
			return ((DataTemp<TE, TT>) id).Value;
		}

		public static string AsS(ADataTemp<TE> id)
		{
			return ((DataTemp<TE, string>) id).Value;
		}

		public static double AsD(ADataTemp<TE> id)
		{
			return ((DataTemp<TE, double>) id).Value;
		}
		
		public static int AsI(ADataTemp<TE> id)
		{
			return ((DataTemp<TE, int>) id).Value;
		}

		public static bool AsB(ADataTemp<TE> id)
		{
			return ((DataTemp<TE, bool>) id).Value;
		}
		*/

	}
}