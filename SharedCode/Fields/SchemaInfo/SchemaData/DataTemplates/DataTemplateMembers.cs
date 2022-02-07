// Solution:     AOToolsDelux
// Project:       CSToolsDelux
// File:             DataTemplateMembers.cs
// Created:      2022-01-30 (9:12 PM)

using System;
using System.Collections.Generic;
using SharedCode.Fields.SchemaInfo.SchemaData.DataTemplate;
using SharedCode.Fields.SchemaInfo.SchemaSupport;
using SharedCode.Windows;

using static SharedCode.Windows.ColData;
using static SharedCode.Windows.ColData.JustifyHoriz;

using static SharedCode.Fields.SchemaInfo.SchemaSupport.DataColumns;

namespace SharedCode.Fields.SchemaInfo.SchemaData.DataTemplates
{
	public static class DataTemplateMembers
	{
		public const int MaxHdrRows = 4;

		public static readonly Dictionary<DataColumns, ColData> DataHdr = Mz(
			//                                                                        col   title hdr     row
			//                                                            key         width width just    just
			new Tuple<DataColumns, int, int, JustifyHoriz, JustifyHoriz>(KEY        , 20,   18,   CENTER, LEFT),
			new Tuple<DataColumns, int, int, JustifyHoriz, JustifyHoriz>(NAME       , 16,   12,   CENTER, LEFT),
			new Tuple<DataColumns, int, int, JustifyHoriz, JustifyHoriz>(VALUE_STR  , 30,   28,   CENTER, LEFT),
			new Tuple<DataColumns, int, int, JustifyHoriz, JustifyHoriz>(VALUE_TYPE , 16,   12,   CENTER, LEFT),
			new Tuple<DataColumns, int, int, JustifyHoriz, JustifyHoriz>(FIELDS_TEMP, 48,   44,   CENTER, LEFT));

		public static readonly Dictionary<DataColumns, string> DataHdrInfo =
			new Dictionary<DataColumns, string>()
			{
				{ KEY        , "Key" },
				{ NAME       , "Name"},
				{ VALUE_STR  , "Value String"},
				{ VALUE_TYPE , "Value Type"},
				{ FIELDS_TEMP, "Fields Template"}
			};

		public static readonly List<DataColumns> DefaultDataOrder = new List<DataColumns>()
		{
			KEY       , 
			NAME      ,
			VALUE_STR ,
			VALUE_TYPE, 
			FIELDS_TEMP
		};

		public static List<List<Dictionary<DataColumns, string>>> FormatData<TSk>(ADataTempBase<TSk> data) where TSk : Enum, new ()
		{
			List<List<Dictionary<DataColumns, string>>> infoLists = new List<List<Dictionary<DataColumns, string>>>();

			foreach (Dictionary<TSk, ADataMembers<TSk>> dataDict in data.ListOfDataDictionaries)
			{
				List<Dictionary<DataColumns, string>> infoList = new List<Dictionary<DataColumns, string>>();

				foreach (KeyValuePair<TSk, ADataMembers<TSk>> kvp in dataDict)
				{
					infoList.Add(kvp.Value.DataRowInfo());
				}

				infoLists.Add(infoList);
			}

			return infoLists;
		}
	}
}