using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using static AOTools.ERevDataItems2;
using static AOTools.EDataType;
using static AOTools.ERevSource;
using static AOTools.EDataUsage;



namespace AOTools
{
	public class RevDataDescription : IEnumerable<KeyValuePair<ERevDataItems2, DataDescription>>
	{
		public SortedList<ERevDataItems2, DataDescription> DataDesc;

		private static RevDataDescription _revDataDesc;

		// note - the index is the data item
		private RevDataDescription()
		{
			Reset();
		}

		public static RevDataDescription GetInstance => 
			_revDataDesc ?? (_revDataDesc = new RevDataDescription());


		public IEnumerator<KeyValuePair<ERevDataItems2, DataDescription>> GetEnumerator()
		{
			return DataDesc.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		// set everything back to detault
		public void Reset()
		{
			DataDesc = new SortedList<ERevDataItems2, DataDescription>((int) REV_ITEMS_LEN);
			AssignDataDescriptions();
		}

		// get the descriptions in column order
		// the index is the column to retreive
		public DataDescription this[int idx]
		{
			get
			{
				if (idx < 0 || idx > (int) REV_ITEMS_LEN) throw new IndexOutOfRangeException();

				ERevDataItems2 result = FindByColumn(idx);

				if (result == REV_CTRL_INVALID) return null;

				return DataDesc[result];
			}

			set
			{
				if (idx < 0 || idx > (int) REV_ITEMS_LEN) throw new IndexOutOfRangeException();

				ERevDataItems2 result = FindByColumn(idx);

				if (result == REV_CTRL_INVALID) return;

				DataDesc[result] = value;
			}

//			get => DataDesc[(ERevDataItems2) idx];
//			set => DataDesc[(ERevDataItems2) idx] = value;
		}

		private ERevDataItems2 FindByColumn(int column)
		{
			foreach (KeyValuePair<ERevDataItems2, DataDescription> kvp in DataDesc)
				{
					if (kvp.Value.Column == column)
					{
						return kvp.Key;
					}
				}

			return REV_CTRL_INVALID;
		}

		// get the data description in the enum order
		public DataDescription this[ERevDataItems2 idx]
		{
			get => DataDesc[idx];
			set => DataDesc[idx] = value;
		}

		public int Count => DataDesc.Count;

		private void AssignDataDescriptions()
		{
			// this defines information about the collection's data fields
			// the key is the enum for the field
			// the first field is the column number which is set in the
			// enum value order to start with
			// flag: this item is / is not selected
			DataDesc.Add( REV_SELECTED,
				new DataDescription(REV_SELECTED, REV_SOURCE_DERIVED,
					BOOL, NONE, "Selected?", true,
					new DataDescription.DataDisplay(7, "5", null)));
			// revision sequence number
			DataDesc.Add( REV_SEQ,
				new DataDescription(REV_SEQ, REV_SOURCE_DERIVED,
					INT, INFO, "Sequence", true,
					new DataDescription.DataDisplay(6, "-4:D", null)));
			// the alt id
			DataDesc.Add( REV_KEY_ALTID,
				new DataDescription(REV_KEY_ALTID, REV_SOURCE_CLOUD,
					STRING, KEY, "Xref Id", true,
					new DataDescription.DataDisplay(6, "-4:D", null)));
			// the document type code - bulletin versus asi versus rfi
			DataDesc.Add( REV_KEY_TYPE_CODE,
				new DataDescription(REV_KEY_TYPE_CODE, REV_SOURCE_DERIVED,
					STRING, KEY, "Type Code", true,
					new DataDescription.DataDisplay(6, "-4", null)));
			// the discipline type code - general versus title, architectural, etc
			DataDesc.Add( REV_KEY_DISCIPLINE_CODE,
				new DataDescription(REV_KEY_DISCIPLINE_CODE, REV_SOURCE_DERIVED,
					STRING, KEY, "Discipline Code", true,
					new DataDescription.DataDisplay(10, "6", null)));
			// the delta title
			DataDesc.Add( REV_KEY_DELTA_TITLE,
				new DataDescription(REV_KEY_DELTA_TITLE, REV_SOURCE_CLOUD,
					STRING, KEY, "Delta Title", true,
					new DataDescription.DataDisplay(16, "14", null)));
			// the sheet number
			DataDesc.Add( REV_KEY_SHTNUM,
				new DataDescription(REV_KEY_SHTNUM, REV_SOURCE_DERIVED,
					STRING, KEY, "Sheet Number", true,
					new DataDescription.DataDisplay(14, "12", null)));
			// the cloud / tag visibility
			DataDesc.Add( REV_ITEM_VISIBLE,
				new DataDescription(REV_ITEM_VISIBLE, REV_SOURCE_DERIVED,
					STRING, KEY, "Visibility", true,
					new DataDescription.DataDisplay(18, "16", null)));
			// the revision number
			DataDesc.Add( REV_ITEM_REVID,
				new DataDescription(REV_ITEM_REVID, REV_SOURCE_CLOUD,
					STRING, INFO, "Revision ID", true,
					new DataDescription.DataDisplay(6, "-4", null)));
			// the revision number
			DataDesc.Add( REV_ITEM_BLOCK_TITLE,
				new DataDescription(REV_ITEM_BLOCK_TITLE, REV_SOURCE_CLOUD,
					STRING, INFO, "Title Block Title", true,
					new DataDescription.DataDisplay(16, "14", null)));
			// the revision date
			DataDesc.Add( REV_ITEM_DATE,
				new DataDescription(REV_ITEM_DATE, REV_SOURCE_CLOUD,
					STRING, INFO, "Date", true,
					new DataDescription.DataDisplay(12, "10", null)));
			// the revision basis
			DataDesc.Add( REV_ITEM_BASIS,
				new DataDescription(REV_ITEM_BASIS, REV_SOURCE_CLOUD,
					STRING, INFO, "Basis", true,
					new DataDescription.DataDisplay(10, "8", null)));
			// the revision description
			DataDesc.Add( REV_ITEM_DESC,
				new DataDescription(REV_ITEM_DESC, REV_SOURCE_CLOUD,
					STRING, INFO, "Description", true,
					new DataDescription.DataDisplay(66, "64", null)));
			// the tag element id
			DataDesc.Add( REV_TAG_ELEM_ID,
				new DataDescription(REV_TAG_ELEM_ID, REV_SOURCE_DERIVED,
					ELEMENTID, NONE, "Tag Elem ID", true,
					new DataDescription.DataDisplay(14, "12", null)));
			// the cloud element id
			DataDesc.Add( REV_CLOUD_ELEM_ID,
				new DataDescription(REV_CLOUD_ELEM_ID, REV_SOURCE_DERIVED,
					ELEMENTID, NONE, "Cloud Elem ID", true,
					new DataDescription.DataDisplay(14, "12", null)));
						// this represents the default order

			// the below are "management" fields that may or may not
			// exist in the data collection
			// this is the
			DataDesc.Add( REV_MGMT_COLUMN,
				new DataDescription(REV_MGMT_COLUMN, REV_SOURCE_DERIVED,
					INT, NONE, "Column", true,
					new DataDescription.DataDisplay(5, "4", null)));
		}

	}

	public class DataDescription
	{
		public int Column { get; private set; }
		public ERevSource Source { get; private set; }
		public EDataType Type { get; private set; }
		public EDataUsage Useage { get; private set; }
		public string Title  { get; set; }
		public bool Visible  { get; set; }			// whether this column is displayed
		public DataDisplay Display { get; set; }

		public DataDescription()
		{
			Display = new DataDisplay();
		}

		public DataDescription(ERevDataItems2 column, ERevSource src,
			EDataType type, EDataUsage usage, string title, bool visible,
			DataDisplay disp)
		{
			Column = (int) column;
			Source = src;
			Type = type;
			Useage = usage;
			Title = title;
			Visible = visible;
			Display = disp;
		}


		public class DataDisplay
		{
			public int ColumnWidth { get; set; }       // the size of the column in which to place the data
			public string FormatString  { get; set; }  // the format string in which to format the data
			public Font Font  { get; set; }            // the font in which to format the data (not used)

			public DataDisplay()
			{
			}

			public DataDisplay(int colWidth, string formatString, Font font)
			{
				ColumnWidth = colWidth;
				FormatString = formatString;
				Font = font;
			}
		}
	}
}