
using System;
using System.Collections;
using System.Windows.Forms;
using Autodesk.Revit.DB;
using static AOToolsDelux.RevDataItems.ERevDataItems;

using static AOToolsDelux.RevColumns;


namespace AOToolsDelux
{
	public class RevDataItems : IEnumerable
	{
		public enum ERevDataItems
		{
			// these fields represent the data items in the string array
			SEQ = 1,              // (from sequence) revision sequence number - for ordering only
			REV_ITEM_VISIBLE,     // (calculated) is this item visible
			REV_ITEM_REVID,       // (from revision) revision id number or alphanumeric
			REV_ITEM_BLOCK_TITLE, // (from revision description) title for this issuance
			REV_ITEM_DATE,        // (from revision date) the date assigned to the revision
			REV_ITEM_BASIS,       // (from comment) the reason for the revision
			REV_ITEM_DESC,        // (from mark) the description of the revision
			REV_ITEM_LEN,         // the total number of items in the string array
		}

		public static RevDataItemsColumn Columns = new RevDataItemsColumn();

		// primary data container
		private string[] _revDataItems;

		private enum ESelected
		{
			SELECTED_IGNORE = -1,
			SELECTED_FALSE = 0,
			SELECTED_TRUE = 1
		}

		

		public RevDataItems()
		{
			_revDataItems = new string[(int) REV_ITEM_LEN];
		}

		public RevDataItems(
			string seq, 
			string visible,
			string revId, 
			string blockTitle, 
			string date,
			string basis,
			string desc
			)
		{
			_revDataItems = new string[(int) REV_ITEM_LEN];

			_revDataItems[(int) SEQ]                  = seq;
			_revDataItems[(int) REV_ITEM_VISIBLE]     = visible;
			_revDataItems[(int) REV_ITEM_REVID]       = revId;
			_revDataItems[(int) REV_ITEM_BLOCK_TITLE] = blockTitle;
			_revDataItems[(int) REV_ITEM_DATE]        = date;
			_revDataItems[(int) REV_ITEM_BASIS]       = basis;
			_revDataItems[(int) REV_ITEM_DESC]        = desc;

			Selected = ESelected.SELECTED_IGNORE;
		}

		public IEnumerator GetEnumerator()
		{
			return _revDataItems.GetEnumerator();
		}

		public string this[int idx] // indexer declaration
		{
			get
			{
				if (idx == 0) { throw new IndexOutOfRangeException(); }
				return _revDataItems[idx];
			}
			set
			{
				if (idx == 0 ) { throw new IndexOutOfRangeException(); }
				_revDataItems[idx] = value;
			}
		}

		public string this[ERevDataItems idx]
		{
			get
			{
				if (idx == 0) { throw new IndexOutOfRangeException(); }
				return _revDataItems[(int) idx];
			}
			set
			{
				if (idx == 0) { throw new IndexOutOfRangeException(); }

				_revDataItems[(int) idx] = value;
			}
		}

		public string Sequence
		{
			get => _revDataItems[(int) SEQ];
			set => _revDataItems[(int) SEQ] = value;
		}

		private ESelected Selected { get; set; }
		
		public string RevId
		{
			get => _revDataItems[(int) REV_ITEM_REVID];
			set => _revDataItems[(int) REV_ITEM_REVID] = value;
		}

		public string RevBlockTitle
		{
			get => _revDataItems[(int) REV_ITEM_BLOCK_TITLE];
			set => _revDataItems[(int) REV_ITEM_BLOCK_TITLE] = value;
		}

		public string RevDate
		{
			get => _revDataItems[(int) REV_ITEM_DATE];
			set => _revDataItems[(int) REV_ITEM_DATE] = value;
		}

		public string RevBasis
		{
			get => _revDataItems[(int) REV_ITEM_BASIS];
			set => _revDataItems[(int) REV_ITEM_BASIS] = value;
		}

		public string RevDescription
		{
			get => _revDataItems[(int) REV_ITEM_DESC];
			set => _revDataItems[(int) REV_ITEM_DESC] = value;
		}
		
		public RevisionVisibility RevVisible
		{
			get => GetVisibility(_revDataItems[(int) REV_ITEM_VISIBLE]); 
			set => _revDataItems[(int) REV_ITEM_VISIBLE] = SetVisibility(value);
		}
//
//		private readonly string[] _visibility = new []
//		{
//			RevisionVisibility.Hidden.ToString(),
//			RevisionVisibility.CloudAndTagVisible.ToString(),
//			RevisionVisibility.TagVisible.ToString(),
//		};

//		public const string VISIBLE_HIDDEN = "hidden";
//		public const string VISIBLE_TAGS_CLOUDS = "tags and clouds";
//		public const string VISIBLE_TAGS = "tags";

		private string SetVisibility(RevisionVisibility visibility)
		{
			return visibility.ToString();
		}

		private RevisionVisibility GetVisibility(string visibility)
		{
			RevisionVisibility result;

			if (Enum.TryParse(visibility, out result)) return result;
		
			return RevisionVisibility.Hidden;
		}

		public class RevDataItemsColumn
		{
			private readonly string[] Cols = new string[(int) REV_ITEM_LEN];

			public RevDataItemsColumn()
			{
				Cols[0]                          = "Not Used";
				Cols[(int) SEQ]                  = "Revision Sequence";
				Cols[(int) REV_ITEM_VISIBLE]     = "Revision Visible";
				Cols[(int) REV_ITEM_REVID]       = "Revision Number";
				Cols[(int) REV_ITEM_BLOCK_TITLE] = "Title Block Description";
				Cols[(int) REV_ITEM_DATE]        = "Revision Date";
				Cols[(int) REV_ITEM_BASIS]       = "Revision Basis";
				Cols[(int) REV_ITEM_DESC]        = "Revision Description";
			}

			public string this[int idx]
			{
				get
				{
					if (idx == 0) { throw new IndexOutOfRangeException(); }
					return Cols[idx];
				}
			}

			public string this[ERevDataItems idx]
			{
				get
				{
					if ((int) idx == 0) { throw new IndexOutOfRangeException(); }
					return Cols[(int) idx];
				}
			}

		}
	}
}