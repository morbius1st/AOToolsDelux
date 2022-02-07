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
			new Tuple<FieldColumns, int, int, JustifyHoriz, JustifyHoriz>(KEY       , 16, 16, CENTER, LEFT),
			new Tuple<FieldColumns, int, int, JustifyHoriz, JustifyHoriz>(NAME      , 16, 16, CENTER, LEFT),
			new Tuple<FieldColumns, int, int, JustifyHoriz, JustifyHoriz>(DESC      , 16, 16, CENTER, LEFT),
			new Tuple<FieldColumns, int, int, JustifyHoriz, JustifyHoriz>(VALUE_STR , 16, 16, CENTER, LEFT),
			new Tuple<FieldColumns, int, int, JustifyHoriz, JustifyHoriz>(DISP_LEVEL, 16, 16, CENTER, LEFT),
			new Tuple<FieldColumns, int, int, JustifyHoriz, JustifyHoriz>(DISP_ORDER, 16, 16, CENTER, LEFT),
			new Tuple<FieldColumns, int, int, JustifyHoriz, JustifyHoriz>(DISP_WIDTH, 16, 16, CENTER, LEFT),
			new Tuple<FieldColumns, int, int, JustifyHoriz, JustifyHoriz>(UNIT_TYPE , 16, 16, CENTER, LEFT));

		public static Dictionary<FieldColumns, string[]> fieldsHdrInfo =
			new Dictionary<FieldColumns, string[]>()
			{
				{ KEY       , new [] { "Key", null, null } },
				{ NAME      , new [] { "Name", null, null } },
				{ DESC      , new [] { "Description", null, null } },
				{ VALUE_STR , new [] { "String", "Value", null } },
				{ DISP_LEVEL, new [] { "Level", "Display", null } },
				{ DISP_ORDER, new [] { "Order", "Display", null } },
				{ DISP_WIDTH, new [] { "Width", "Display", null } },
				{ UNIT_TYPE , new [] { "Type", "Unit", null } }
			};

		public static List<FieldColumns> fieldOrder = new List<FieldColumns>()
		{
			KEY       ,
			NAME      ,
			DESC      ,
			VALUE_STR ,
			DISP_LEVEL,
			DISP_ORDER,
			DISP_WIDTH,
			UNIT_TYPE
		};
	}
}