// Solution:     AOToolsDelux
// Project:       CSToolsDelux
// File:             FieldsTempMembers.cs
// Created:      2022-01-30 (9:13 PM)

using System;
using System.Collections.Generic;
using SharedCode.Windows;
using static SharedCode.Windows.ColData;
using static SharedCode.Windows.ColData.JustifyHoriz;
using SharedCode.Fields.SchemaInfo.SchemaSupport;
using static SharedCode.Fields.SchemaInfo.SchemaSupport.FieldColumns;

namespace SharedCode.Fields.SchemaInfo.SchemaFields.FieldsTemplates
{
	public class FieldsTemplateMembers
	{
		public static Dictionary<FieldColumns, ColData> fieldHdr = Mz(
			new Tuple<FieldColumns, int, int, JustifyHoriz, JustifyHoriz>(KEY       , 20, 18, CENTER, LEFT),
			new Tuple<FieldColumns, int, int, JustifyHoriz, JustifyHoriz>(NAME      , 16, 12, CENTER, LEFT),
			new Tuple<FieldColumns, int, int, JustifyHoriz, JustifyHoriz>(DESC      , 30, 28, CENTER, LEFT),
			new Tuple<FieldColumns, int, int, JustifyHoriz, JustifyHoriz>(VALUE_STR , 30, 28, CENTER, LEFT),
			new Tuple<FieldColumns, int, int, JustifyHoriz, JustifyHoriz>(DISP_LEVEL, 12, 10, CENTER, LEFT),
			new Tuple<FieldColumns, int, int, JustifyHoriz, JustifyHoriz>(DISP_ORDER, 10, 8, CENTER, CENTER),
			new Tuple<FieldColumns, int, int, JustifyHoriz, JustifyHoriz>(COL_WIDTH, 10, 8, CENTER, CENTER),
			new Tuple<FieldColumns, int, int, JustifyHoriz, JustifyHoriz>(UNIT_TYPE , 12, 10, CENTER, LEFT));

		public static Dictionary<FieldColumns, string> fieldsHdrInfo =
			new Dictionary<FieldColumns, string>()
			{
				{ KEY       , "Key" },
				{ NAME      , "Name" },
				{ DESC      , "Description" },
				{ VALUE_STR , "Value String" },
				{ DISP_LEVEL, "Display Level" },
				{ DISP_ORDER, "Display Order" },
				{ COL_WIDTH , "Display Width" },
				{ UNIT_TYPE , "Unit Type" }
			};

		public static List<FieldColumns> DefaultFieldsOrder = new List<FieldColumns>()
		{
			KEY       ,
			NAME      ,
			DESC      ,
			VALUE_STR ,
			DISP_LEVEL,
			DISP_ORDER,
			COL_WIDTH,
			UNIT_TYPE
		};

		public static List<List<Dictionary<FieldColumns, string>>> FormatData<TSk>(AFieldsTemp<TSk> data) where TSk : Enum, new()
		{
			List<List<Dictionary<FieldColumns, string>>> infoLists = new List<List<Dictionary<FieldColumns, string>>>();

			List<Dictionary<FieldColumns, string>> infoList = new List<Dictionary<FieldColumns, string>>();

			foreach (KeyValuePair<TSk, AFieldsMembers<TSk>> kvp in data.Fields)
			{
				infoList.Add(kvp.Value.FieldsRowInfo());
			}

			infoLists.Add(infoList);

			return infoLists;
		}

	}
}