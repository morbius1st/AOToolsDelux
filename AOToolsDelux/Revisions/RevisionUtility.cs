using System;
using System.Collections.Generic;
using System.Text;
using Autodesk.Revit.DB;
using UtilityLibrary;

using static AOTools.Revisions.EItem;

//using static UtilityLibrary.MessageUtilities;
using static UtilityLibrary.MessageUtilities2;

namespace AOTools.Revisions
{

	public static class RevisionUtility
	{
		public static void ListRevInfo2_1(SortedList<string, RevDataItems2> revInfo2)
		{
			if (revInfo2 == null) return;

			logMsgLn2("item count", revInfo2.Count);

			RevDataDescription desc = RevDataDescription.GetInstance;

			foreach (KeyValuePair<string, RevDataItems2> kvp in revInfo2)
			{
				logMsg2(nl);

				logMsgLn2(desc[REV_KEY].Title,
					">" + kvp.Key + "<");

				for (int i = 0; i < (int) REV_ITEMS_LEN; i++)
				{
					logMsgLn2(desc[i].Title, kvp.Value[i] ?? "");
				}

			}
		}

		// these are for the new system

		// this lists each data description
		public static void ListDescriptions()
		{
			RevDataDescription dx = RevDataDescription.GetInstance;

			logMsg2($"{"EItem",-26}");
			logMsg2($"{"col"           ,-04} ");
			logMsg2($"{"source"        ,-22}");
			logMsg2($"{"type"          ,-12}");
			logMsg2($"{"usage"         ,-10}");
			logMsg2($"{"visible"       ,-07} ");
			logMsg2($"{"colw"          ,-04} ");
			logMsg2($"{"fmt str"       ,-10}");

			logMsg2(nl);


			foreach (KeyValuePair<EItem, DataDescription> kvp in dx)
			{
				logMsg2($"{kvp.Key                       ,-26}");
				logMsg2($"{kvp.Value.Column              , 04:D} ");
				logMsg2($"{kvp.Value.Source              ,-22}");
				logMsg2($"{kvp.Value.Type                ,-12}");
				logMsg2($"{kvp.Value.Useage              ,-10}");
				logMsg2($"{kvp.Value.Visible             ,-07} ");
				logMsg2($"{kvp.Value.Display.ColumnWidth , 04:D} ");
				logMsg2($"{kvp.Value.Display.FormatString,-10}");
				logMsg2(nl);
			}
		}

		private static string GetSortOrderCode(string revAltId, string revTypeCode,
			string revDisciplineCode, string shtNum )
		{
			string altId = revAltId;
			string num = $"{shtNum,20}";
			string sortOrderCode = altId + revTypeCode + revDisciplineCode + num;

			return sortOrderCode;
		}

//		public static void ListRevInfo4(SortedList<RevDataKey, RevDataItems> revInfo)
//		{
//			if (revInfo == null) return;
//
//			int i = 1;
//
//			logMsgLn2("ri count", revInfo.Count);
//
//			foreach (KeyValuePair<RevDataKey, RevDataItems> riKvp in revInfo)
//			{
//				logMsg2(nl);
//				logMsgLn2("revision info", "---| " + i + " |---");
//
//				string columnTitle = "";
//				string columnValue = "";
//
//
//				foreach (KeyValuePair<int, RevCol> rcKvp in RevCols)
//				{
//					if (rcKvp.Key >= MAX_FIELDS) break;
//
//					switch (rcKvp.Value.Source)
//					{
//					case DERIVED:
//						{
//							int a = (int) (ERevDataDerived) rcKvp.Value.Index;
//							columnTitle = RevColumns.Columns[a];
//							columnValue = "undefined";
//							break;
//						}
//					case KEY:
//						{
//							ERevDataKey a = (ERevDataKey) rcKvp.Value.Index;
//							columnTitle = RevDataKey.Columns[a];
//							columnValue = riKvp.Key[a];
//
//							break;
//						}
//					case DATA:
//						{
//							ERevDataItems a = (ERevDataItems) rcKvp.Value.Index;
//							columnTitle = RevDataItems.Columns[a];
//							columnValue = riKvp.Value[a];
//							break;
//						}
//					}
//
//					logMsgLn2(columnTitle, columnValue);
//				}
//				i++;
//			}
//		}

		public static void ListRevInfo5(SortedList<RevDataKey, RevDataItems> revInfo)
		{
								   //k0  //k1  //k2  //k3  //k4
			int[] paddingk = new [] {0,    0,    0,    16,   9};
									//vis //alt  //title //basis  //desc
			int[] paddingv = new [] {20,    4,     16,      10,       0};

			if (revInfo == null) return;

			int i = 1;

			// header
			logMsg2("ri count| " + revInfo.Count);
			logMsg2(nl);
			logMsg2("                        key                              key     key     key          key               key            item                item        item              item          item");
			logMsg2(nl);
			logMsg2("----  -- item --   ---- sort code --------------------  -alt    -type  - disc -  ---- delta -------  -- sheet --  ---- visibility ------  -rev -  ---- block -------  -- basis ---  -- description --------");
			logMsg2(nl);
			logMsg2("----  -- num ---   -----------------------------------  -num-   -code  - code -  ---- title -------  -- number -  ----------------------  -num--  ---- title -------  ------------  -----------------------");
			logMsg2(nl);

			foreach (KeyValuePair<RevDataKey, RevDataItems> riKvp in revInfo)
			{
				int pv = 0;
				int pk = 0;

				StringBuilder sb = new StringBuilder();
				StringBuilder sbk = new StringBuilder();

				sb.Append("item --| ").Append($"{i:D3}").Append("|-- ");
				sbk.Append("item  ");

				string SortOrderKey = GetSortOrderCode(
					riKvp.Key[ERevDataKey.REV_KEY_ALTID],
					riKvp.Key[ERevDataKey.REV_KEY_TYPE_CODE],
					riKvp.Key[ERevDataKey.REV_KEY_DISCIPLINE_CODE],
					riKvp.Key[ERevDataKey.REV_KEY_SHEETNUM]
				);

				sb.Append(" :: ").Append(SortOrderKey);
				sbk.Append("sort key  ");


				foreach (KeyValuePair<int, RevCol> rcKvp in RevCols)
				{
					if (rcKvp.Key >= MAX_FIELDS) break;

					sb.Append(" :: ");

					switch (rcKvp.Value.Source)
					{
					case DERIVED:
						{
							int a = (int) (ERevDataDerived) rcKvp.Value.Index;

							sb.Append("undefined");
							break;
						}
					case KEY:
						{
							ERevDataKey a = (ERevDataKey) rcKvp.Value.Index;

							sb.Append(riKvp.Key[a].PadRight(paddingk[pk++]));

							break;
						}
					case DATA:
						{
							ERevDataItems a = (ERevDataItems) rcKvp.Value.Index;

							sb.Append(riKvp.Value[a].PadRight(paddingv[pv++]));
							break;
						}
					}
				}

				logMsg2(sb.ToString() + nl);
				i++;
			}
		}
	}
}
