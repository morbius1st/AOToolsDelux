using System;
using System.Collections.Generic;
using static AOTools.RevDataKey.ERevDataKey;
using static AOTools.RevDataKey;
using static AOTools.RevDataItems.ERevDataItems;
using static AOTools.RevDataItems;

using static AOTools.RevColumns.EDataSource;
using static AOTools.RevColumns.ERevDataDerived;
using static AOTools.RevColumns;

namespace AOTools
{
	class RevColumns
	{
		public enum EDataSource
		{
			UNASSIGNED = -1,
			DERIVED = 0,
			KEY = 10,
			DATA = 30
		}

		public enum ERevDataDerived
		{
			REV_DERIVED_KEY,
			REV_DERIVED_LEN
		}

		public struct ColumnKey
		{
			public Enum[] key { get; private set; }

			public ColumnKey(EDataSource source, Enum value)
			{
				key = new [] {source, value};
			}
		}

		private static int HiddenColumnCount = 0;

		public static RevDataDerivedColumn Columns = new RevDataDerivedColumn();

		public const int MAX_FIELDS = 100;

		// note that key == 0 is unused
		public static SortedList<int, RevCol> RevCols { get; private set; }
		
		// represents the columns, their titles, there order, and wheather to export
		// this will allow the column order to be changed
		public static void AssignColumns()
		{
			RevCols = new SortedList<int, RevCol>();

			int i = 1;  // first column is 1

			AssignColumnKey(RevCols,  -1, DERIVED, REV_DERIVED_KEY, true);

			AssignColumnKey(RevCols,  -1, DATA, SEQ, true);

			AssignColumnKey(RevCols, i++, KEY, REV_KEY_ALTID, true);
			AssignColumnKey(RevCols, i++, KEY, REV_KEY_TYPE_CODE, true);
			AssignColumnKey(RevCols, i++, KEY, REV_KEY_DISCIPLINE_CODE, true);
			AssignColumnKey(RevCols, i++, KEY, REV_KEY_DELTA_TITLE, true);
			AssignColumnKey(RevCols, i++, KEY, REV_KEY_SHTNUM, true);

			AssignColumnKey(RevCols, i++, DATA, REV_ITEM_VISIBLE, true);
			AssignColumnKey(RevCols, i++, DATA, REV_ITEM_REVID, true);
			AssignColumnKey(RevCols, i++, DATA, REV_ITEM_BLOCK_TITLE, true);
			AssignColumnKey(RevCols, i++, DATA, REV_ITEM_BASIS, true);
			AssignColumnKey(RevCols, i++, DATA, REV_ITEM_DESC, true);
			AssignColumnKey(RevCols,  -1, DATA, REV_ITEM_DATE, true);
		}

		// non-printing fields >= HiddenColumnCount 
		private static void AssignColumnKey(SortedList<int, RevCol> revCols,
			int column,EDataSource source, Enum field, bool export)
		{
			if (column < 0)
			{
				column = MAX_FIELDS + HiddenColumnCount++;
			}

			revCols.Add(column, new RevCol(source, field, export));
		}

		public class RevDataDerivedColumn
		{
			private readonly string[] Cols = new string[(int) REV_DERIVED_LEN];

			public RevDataDerivedColumn()
			{
				Cols[(int) REV_DERIVED_KEY] = "Main Sort Key";
			}

			public string this[int idx] => Cols[idx];
			public string this[ERevDataDerived idx] => Cols[(int) idx];
		}

		public class RevCol
		{
			public EDataSource Source { get; private set; }
			private Enum index;
			public bool Export { get; private set; }

			public RevCol()
			{
				Source = UNASSIGNED;
				index = null;
				Export = true;
			}

			public RevCol(EDataSource source, Enum index, bool export)
			{
				Source = source;
				this.index = index;
				Export = export;
			}

			public Enum Index
			{
				get
				{
					switch (Source)
					{
					case DERIVED:
						{
							return (EDataSource) index;
						}
					case KEY:
						{
							return (ERevDataKey) index;
						}
					case DATA:
						{
							return (ERevDataItems) index;
							break;
						}
					}

					return UNASSIGNED;
				}
			}
		}
	}
}
