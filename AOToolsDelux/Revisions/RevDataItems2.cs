using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Autodesk.Revit.DB;
using static AOToolsDelux.Revisions.EItem;
using static AOToolsDelux.Revisions.EItemType;
using static AOToolsDelux.Revisions.EItemSource;
using static AOToolsDelux.Revisions.EItemUsage;


namespace AOToolsDelux.Revisions
{
	// this represents a one tag / cloud
	public class RevDataItems2 : IEnumerable
	{
		// these fields represent the data items - this represents their
		// array position

		private dynamic[] _revDataItems2;

		public RevDataItems2()
		{
			_revDataItems2 = new dynamic[(int) REV_ITEMS_LEN];
		}

		public IEnumerator GetEnumerator()
		{
			return _revDataItems2.GetEnumerator();
		}

		public dynamic this[int idx]
		{
			get => _revDataItems2[idx];
			set => _revDataItems2[idx] = value;
		}

		public dynamic this[EItem idx]
		{
			get => _revDataItems2[(int) idx];
			set => _revDataItems2[(int) idx] = value;
		}

		public int? AsInt(EItem idx)
		{
			if (RevDataDescription.GetInstance[idx].Type != INT) return null;

			return (int) _revDataItems2[(int) idx];
		}

		public bool? AsBool(EItem idx)
		{
			if (RevDataDescription.GetInstance[idx].Type != BOOL) return null;

			return (bool) _revDataItems2[(int) idx];
		}

		public ElementId AsElementId(EItem idx)
		{
			if (RevDataDescription.GetInstance[idx].Type != ELEMENTID) return null;

			return (ElementId) _revDataItems2[(int) idx];
		}

		public RevisionVisibility? AsRevVisibility(EItem idx)
		{
			if (RevDataDescription.GetInstance[idx].Type != VISIBILITY) return null;

			return (RevisionVisibility) _revDataItems2[(int) idx];
		}

		public String AsString(EItem idx)
		{
			if (RevDataDescription.GetInstance[idx].Type != STRING) return null;

			return (string) _revDataItems2[(int) idx];
		}

		public bool Selected
		{
			get => (bool) _revDataItems2[(int) REV_SELECTED];
			set => _revDataItems2[(int) REV_SELECTED] = value;
		}
		// read only
		public int Sequence
		{
			get => (int) _revDataItems2[(int) REV_SEQ];
			set => _revDataItems2[(int) REV_SEQ] = value;
		}

		public string AltId
		{
			get => (string) _revDataItems2[(int) REV_KEY_ALTID];
			set => _revDataItems2[(int) REV_KEY_ALTID] = value;
		}

		public string TypeCode
		{
			get => (string) _revDataItems2[(int) REV_KEY_TYPE_CODE];
			set => _revDataItems2[(int) REV_KEY_TYPE_CODE] = value;
		}

		public string DisciplineCode
		{
			get => (string) _revDataItems2[(int) REV_KEY_DISCIPLINE_CODE];
			set => _revDataItems2[(int) REV_KEY_DISCIPLINE_CODE] = value;
		}

		public string DeltaTitle
		{
			get => (string) _revDataItems2[(int) REV_KEY_DELTA_TITLE];
			set => _revDataItems2[(int) REV_KEY_DELTA_TITLE] = value;
		}

		public string ShtNum
		{
			get => (string) _revDataItems2[(int) REV_KEY_SHEETNUM];
			set => _revDataItems2[(int) REV_KEY_SHEETNUM] = value;
		}

		public RevisionVisibility Visibility
		{
			get => (RevisionVisibility) _revDataItems2[(int) REV_ITEM_VISIBLE];
			set => _revDataItems2[(int) REV_ITEM_VISIBLE] = value;
		}

		public string RevisionId
		{
			get => (string) _revDataItems2[(int) REV_ITEM_REVID];
			set => _revDataItems2[(int) REV_ITEM_REVID] = value;
		}

		public  string BlockTitle
		{
			get => (string) _revDataItems2[(int) REV_ITEM_BLOCK_TITLE];
			set => _revDataItems2[(int) REV_ITEM_BLOCK_TITLE] = value;
		}

		public  string RevisionDate
		{
			get => (string) _revDataItems2[(int) REV_ITEM_DATE];
			set => _revDataItems2[(int) REV_ITEM_DATE] = value;
		}

		public  string Basis
		{
			get => (string) _revDataItems2[(int) REV_ITEM_BASIS];
			set => _revDataItems2[(int) REV_ITEM_BASIS] = value;
		}

		public  ElementId TagElemId
		{
			get => (ElementId) _revDataItems2[(int) REV_TAG_ELEM_ID];
			set => _revDataItems2[(int) REV_TAG_ELEM_ID] = value;
		}

		public  ElementId CloudElemId
		{
			get => (ElementId) _revDataItems2[(int) REV_CLOUD_ELEM_ID];
			set => _revDataItems2[(int) REV_CLOUD_ELEM_ID] = value;
		}
		public  string Description
		{
			get => (string) _revDataItems2[(int) REV_ITEM_DESC];
			set => _revDataItems2[(int) REV_ITEM_DESC] = value;
		}


	}
}
