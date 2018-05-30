using System;
using System.Collections.Generic;
using System.Text;
using Autodesk.Revit.DB;
using UtilityLibrary;
using static AOTools.RevDataKey.ERevDataKey;
using static AOTools.RevDataKey;
using static AOTools.RevDataItems;

using static AOTools.RevColumns.EDataSource;
using static AOTools.RevColumns;

using static AOTools.SelectCriteria.ECompare;

using static AOTools.ERevDataItems2;

//using static UtilityLibrary.MessageUtilities;
using static UtilityLibrary.MessageUtilities2;

namespace AOTools
{

	class RevCloudUtil
	{
		public static void ListRevInfo4(SortedList<RevDataKey, RevDataItems> revInfo)
		{
			if (revInfo == null) return;

			int i = 1;

			logMsgLn2("ri count", revInfo.Count);

			foreach (KeyValuePair<RevDataKey, RevDataItems> riKvp in revInfo)
			{
				logMsg2(nl);
				logMsgLn2("revision info", "---| " + i + " |---");

				string columnTitle = "";
				string columnValue = "";


				foreach (KeyValuePair<int, RevCol> rcKvp in RevCols)
				{
					if (rcKvp.Key >= MAX_FIELDS) break;

					switch (rcKvp.Value.Source)
					{
					case DERIVED:
						{
							int a = (int) (ERevDataDerived) rcKvp.Value.Index;
							columnTitle = RevColumns.Columns[a];
							columnValue = "undefined";
							break;
						}
					case KEY:
						{
							ERevDataKey a = (ERevDataKey) rcKvp.Value.Index;
							columnTitle = RevDataKey.Columns[a];
							columnValue = riKvp.Key[a];

							break;
						}
					case DATA:
						{
							ERevDataItems a = (ERevDataItems) rcKvp.Value.Index;
							columnTitle = RevDataItems.Columns[a];
							columnValue = riKvp.Value[a];
							break;
						}
					}

					logMsgLn2(columnTitle, columnValue);
				}
				i++;
			}
		}

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
					riKvp.Key[ERevDataKey.REV_KEY_SHTNUM]
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

		public static void ListRevInfo2_1(SortedList<string, RevDataItems2> revInfo2)
		{
			if (revInfo2 == null) return;

			logMsgLn2("item count", revInfo2.Count);

			RevDataDescription desc = RevDataDescription.GetInstance;

			foreach (KeyValuePair<string, RevDataItems2> kvp in revInfo2)
			{
				logMsg2(nl);

				logMsgLn2(desc[REV_KEY].Title, 
					">" + kvp.Key +"<");

				for (int i = 0; i < (int) REV_ITEMS_LEN; i++)
				{
					logMsgLn2(desc[i].Title, kvp.Value[i]?? "");
				}
				
			}
		}

		// these are for the new system

		// this lists each data description
		public static void ListDescriptions()
		{
			RevDataDescription dx = RevDataDescription.GetInstance;

			logMsg2($"{"ERevDataItems2",-26}");
			logMsg2($"{"col"           ,-04} ");
			logMsg2($"{"source"        ,-22}");
			logMsg2($"{"type"          ,-12}");
			logMsg2($"{"usage"         ,-10}");
			logMsg2($"{"visible"       ,-07} ");
			logMsg2($"{"colw"          ,-04} ");
			logMsg2($"{"fmt str"       ,-10}");

			logMsg2(nl);


			foreach (KeyValuePair<ERevDataItems2, DataDescription> kvp in dx)
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

		private void test2()
		{
			RevDataKey test1 = new RevDataKey("a1", 
				"a2", "a3", "a4", "a5");

			int i = 0;

			foreach (string s in test1)
			{
				logMsgLn2("item " + i++, s);
			}

			for (i = 0; i < 5; i++)
			{
				logMsgLn2("item " + i, test1[i]);
			}
		}

		private void test1()
		{
			SortedList<string[], string[]> test = 
				new SortedList<string[], string[]>(new StringArrayCompare());

			string[] key;
			string[] value;

			key = new [] {"k1", "k3", "k4"};
			value = new [] {"v1", "v3", "v4"};

			test.Add(key, value);

			key = new [] {"k1", "k2", "k4"};
			value = new [] {"v1", "v2", "v4"};

			test.Add(key, value);

			key = new [] {"k1", "k2", "k3"};
			value = new [] {"v1", "v2", "v3"};

			test.Add(key, value);

			foreach (KeyValuePair<string[], string[]> kvp in test)
			{
				logMsg2(nl);
				logMsgLn2("key", kvp.Key[0]  
					+ " :: " + kvp.Key[1]
					+ " :: " + kvp.Key[2]
					);

				logMsgLn2("value", kvp.Value[0]  
					+ " :: " + kvp.Value[1]
					+ " :: " + kvp.Value[2]
					);
			}
		}

		private void test3 ()
		{
			string[,] items = new string[,]
			{
				// equal
				// less than or equal
				// greater than or equal
				{"alpha", "alpha"},

				// less than
				// less than or equal
				// not equal
				{"beta", "alpha"},

				// greater than
				// greater than or equal
				// not equal
				{"alpha", "beta"}, // greater than
			};


			for (int i = 0; i < items.Length / 2; i++)
			{	
				
				foreach (SelectCriteria.ECompare c 
					in Enum.GetValues(typeof(SelectCriteria.ECompare)))
				{
					logMsg2(items[i,0].PadLeft(7));	
					logMsg2(" <-" + c.ToString().PadCenter(25) + "->  ");	
					logMsg2(items[i,1].PadRight(7));	
					logMsg2(" " 
						+ Select(items[i,0], c, items[i,1]));	
					logMsg2(nl);
				}
				logMsg2(nl);
			}
		}

		private bool Select(string original, 
			SelectCriteria.ECompare c, string test)
		{
			bool result = true;

			int f = (int) c;

			if (c != ANY)
			{
				int compare = original.CompareTo(test);

				logMsg2("*| " + compare.ToString().PadLeft(7) + " |* ");	

				if ( c == EQUAL ||
						c == GREATER_THEN_OR_EQUAL ||
						c == LESS_THEN_OR_EQUAL )
				{
					result = compare == 0;
				} 

				if (c == GREATER_THEN_OR_EQUAL ||
					c == GREATER_THEN )
				{
					result = compare > 0;
				}
				else if (c == LESS_THEN_OR_EQUAL ||
					c == LESS_THEN)
				{
					result = compare < 0;
				} 
				else if (c == NOT_EQUAL)
				{
					result =  original.Equals(c);
				}
			}

			return result;
		}
	}

	public class StringArrayCompare : IComparer<string[]>
	{
		public int Compare(string[] first, string[] second)
		{
			int compare = 0;

			if (first.Length == second.Length)
			{
				for (int i = 0; i < first.Length; i++)
				{
					compare = first[i].CompareTo(second[i]);

					if (compare != 0) break;
				}
			}
			else
			{ 
				compare = first.Length - second.Length;
			}
			return compare;
		}

		public static string GetParameterInfo(Document Doc, Parameter p)
		{
			string result = "";

			switch (p.StorageType)
			{
			case StorageType.None:
				{
					result += p.AsString();
					break;
				}
			case StorageType.Double:
				//covert the number into Metric 
				result += " : " + p.AsValueString();
				break;
			case StorageType.ElementId:
				//find out the name of the element 
				ElementId id = p.AsElementId();
				if (id.IntegerValue >= 0)
				{
					result += " : " + Doc.GetElement(id).Name;
				}
				else
				{
					result += " : " + id.IntegerValue.ToString();
				}
				break;
			case StorageType.Integer:
				if (ParameterType.YesNo == p.Definition.ParameterType)
				{
					if (p.AsInteger() == 0)
					{
						result += " : " + "False";
					}
					else
					{
						result += " : " + "True";
					}
				}
				else
				{
					result += " : " + p.AsInteger().ToString();
				}
				break;
			case StorageType.String:
				result += " : " + p.AsString();
				break;
			default:
				result = "Unexposed parameter.";
				break;
			}

			return result;
		}
	}
}
