using System;
using System.Collections.Generic;
using Autodesk.Revit.DB;

using static AOToolsDelux.RevDataKey.ERevDataKey;
using static AOToolsDelux.RevDataItems.ERevDataItems;

using static AOToolsDelux.SortKey;


// read through the database for the revision information and
// place into a sorted list
namespace AOToolsDelux
{

	public static class RevData
	{

		private static SortedList<RevDataKey, RevDataItems> _revisionInfo;
//		private static bool[] _revisionVisible;

		public static SortedList<RevDataKey, RevDataItems> RevisionInfo
		{
			get
			{
				if (_revisionInfo == null || _revisionInfo.Count == 0) Init();

				return _revisionInfo;
			}

			private set => _revisionInfo = value;
		}

		public static void Init()
		{
//			GetRevVisible();

			GetRevInfo(GetRevClouds());
		}

		private static IList<Element> GetRevClouds()
		{
			ElementCategoryFilter filterRevClouds = new ElementCategoryFilter(BuiltInCategory.OST_RevisionClouds);
			FilteredElementCollector collectClouds = new FilteredElementCollector(RevCloud.Doc);

			return collectClouds.WherePasses(filterRevClouds).
				WhereElementIsNotElementType().ToElements();
		}

		private static void GetRevInfo(IList<Element> revClouds)
		{
			_revisionInfo =
				new SortedList<RevDataKey, RevDataItems>(revClouds.Count);

			foreach (Element e in revClouds)
			{
				if (!(e is RevisionCloud revCloud)) continue;

				if (!(RevCloud.Doc.GetElement(
					revCloud.get_Parameter(
						BuiltInParameter.REVISION_CLOUD_REVISION).AsElementId())
					is Revision rev)) continue;

				int seq = rev.SequenceNumber;

				RevDataKey key = new RevDataKey();
				RevDataItems items = new RevDataItems();

				// from revision
				items.Sequence            = seq.ToString();
				items.RevVisible          = rev.Visibility;

				// item data from rev cloud
				items.RevBlockTitle       = revCloud.get_Parameter(BuiltInParameter.REVISION_CLOUD_REVISION_DESCRIPTION).AsString();
				items.RevId               = revCloud.get_Parameter(BuiltInParameter.REVISION_CLOUD_REVISION_NUM).AsString();
				items.RevDate             = revCloud.get_Parameter(BuiltInParameter.REVISION_CLOUD_REVISION_DATE).AsString();
				items.RevBasis            = revCloud.get_Parameter(BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS).AsString();
				items.RevDescription      = revCloud.get_Parameter(BuiltInParameter.ALL_MODEL_MARK)?.AsString();

				// key data
				key.RevDeltaTitle         = revCloud.get_Parameter(BuiltInParameter.REVISION_CLOUD_REVISION_ISSUED_TO).AsString();
				key.RevShtNumber          = GetSheetNumber(revCloud);

				key.RevAltId              = $"{revCloud.get_Parameter(BuiltInParameter.REVISION_CLOUD_REVISION_ISSUED_BY).AsString(),4}";
				key.RevTypeCode           = GetTypeSortCode(key.RevDeltaTitle);
				key.RevDisciplineCode     = GetDisciplineSortCode(key.RevShtNumber);

				_revisionInfo.Add(key, items);
			}
		}


		private static string GetSheetNumber(RevisionCloud revCloud)
		{
			ISet<ElementId> s = revCloud.GetSheetIds();

			foreach (ElementId ex in s)
			{
				return ((ViewSheet) RevCloud.Doc.GetElement(ex)).SheetNumber;
			}
			return null;
		}
	}

	//
	//		public static bool IsVisible(this Revision rev)
	//		{
	//			if (_revisionVisible == null || _revisionVisible.Length == 0) return false;
	//
	//			return _revisionVisible[rev.SequenceNumber];
	//		}
	//
	//		private static void GetRevVisible()
	//		{
	//			ElementCategoryFilter filterRevs = new ElementCategoryFilter(BuiltInCategory.OST_Revisions);
	//			FilteredElementCollector collectRevs = new FilteredElementCollector(RevCloud.Doc);
	//
	//			IList<Element> revs = collectRevs.WherePasses(filterRevs).
	//				WhereElementIsNotElementType().ToElements();
	//
	//			_revisionVisible = new bool[revs.Count + 1];
	//
	//			foreach (Element e in revs)
	//			{
	//				Revision rev = e as Revision;
	//				_revisionVisible[rev.SequenceNumber] = rev.Visibility != RevisionVisibility.Hidden;
	//			}
	//		}


	//
	//		private static void GetRevInfo(IList<Element> revClouds)
	//		{
	//			_revisionInfo = 
	//				new SortedList<string, string[]>(revClouds.Count);
	//
	//			foreach (Element e in revClouds)
	//			{
	//				if (!(e is RevisionCloud revCloud)) continue;
	//
	//				if (!(RevCloud.Doc.GetElement(
	//					revCloud.get_Parameter(
	//						BuiltInParameter.REVISION_CLOUD_REVISION).AsElementId())
	//					is Revision rev)) continue;
	//				
	//				int seq = rev.SequenceNumber;
	//
	//				string[] ri = new string[(int) REV_LEN];
	//
	//				// from revision
	//				ri[(int) SEQ] = seq.ToString();
	//				
	//				// from rev cloud
	//				ri[(int) REV_BLOCK_TITLE] = revCloud.get_Parameter(BuiltInParameter.REVISION_CLOUD_REVISION_DESCRIPTION).AsString();
	//				ri[(int) REV_DELTA_TITLE] = revCloud.get_Parameter(BuiltInParameter.REVISION_CLOUD_REVISION_ISSUED_TO).AsString();
	//				ri[(int) REV_REVID] = revCloud.get_Parameter(BuiltInParameter.REVISION_CLOUD_REVISION_NUM).AsString();
	//				ri[(int) REV_ALTID] = revCloud.get_Parameter(BuiltInParameter.REVISION_CLOUD_REVISION_ISSUED_BY).AsString();
	//				ri[(int) REV_DATE] = revCloud.get_Parameter(BuiltInParameter.REVISION_CLOUD_REVISION_DATE).AsString();
	//				ri[(int) REV_BASIS] = revCloud.get_Parameter(BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS).AsString();
	//				ri[(int) REV_DESC] = revCloud.get_Parameter(BuiltInParameter.ALL_MODEL_MARK)?.AsString();
	//
	//				// calculated
	//				ri[(int) REV_SHTNUM] = GetSheetNumber(revCloud);
	//				ri[(int) REV_ORDER] = SortKey.GetSortOrderCode2(ri);
	//
	//				// key = revision number (REV_REVID) + (REV_SHTNUM) + (REV_BLOCK_TITLE) [all formatted]
	//				string key = SortKey.GetSortKey(ri[(int) REV_ORDER], 
	//					ri[(int) REV_BLOCK_TITLE], ri[(int) REV_SHTNUM]);
	//
	//				_revisionInfo.Add(key, ri);
	//			}
	//		}

	//
	//	public enum RevCloudKeyEnum
	//	{
	//		REV_ORDER_ALTID,
	//		REV_ORDER_TYPE_CODE,
	//		REV_ORDER_DISCIPLINE_CODE,
	//		REV_ORDER_DELTA_TITLE,
	//		REV_ORDER_SHT_NUM,
	//		REV_ORDER_UNIQUE
	//	}
	//
	//	// this is the list of fields held in the string array
	//	public enum RevCloudDataEnum
	//	{
	//		// these fields represent the data items in the string array
	//		SEQ = 1,									   // (from sequence) revision sequence number - for ordering only
	//		REV_VISIBLE,								   // (calculated) is this item visible
	//		REV_REVID,									   // (from revision) revision id number or alphanumeric
	//		REV_BLOCK_TITLE,							   // (from revision description) title for this issuance
	//		REV_DATE,									   // (from revision date) the date assigned to the revision
	//		REV_BASIS,									   // (from comment) the reason for the revision
	//		REV_DESC,									   // (from mark) the description of the revision
	//		REV_LEN,									   // the total number of items in the string array
	//
	//		REV_KEY_START = 20,							   // the start of the key information
	//		REV_ALTID = 
	//			REV_KEY_START + REV_ORDER_ALTID,           // (from issued by) a cross-reference to the REV_REVID associated with this item
	//		REV_TYPE_CODE = 
	//			REV_KEY_START + REV_ORDER_TYPE_CODE,       // code based on the document type
	//		REV_DISCIPLINE_CODE = 
	//			REV_KEY_START + REV_ORDER_DISCIPLINE_CODE, // code based on the discipline
	//		REV_DELTA_TITLE = 
	//			REV_KEY_START + REV_ORDER_DELTA_TITLE,     // (from issued to) simple name for this issuance (goes below the delta)
	//		REV_SHTNUM = 
	//			REV_KEY_START + REV_ORDER_SHT_NUM,         // (calculated) sheet number of this tag
	//		REV_UNIQUE_CODE = 
	//			REV_KEY_START + REV_ORDER_UNIQUE,          // unique code to prevent duplicate keys
	//
	//		REV_KEY_END,								   // the end of the key & the total number of items + placeholders
	//	}
	//
	//	public class RevDataKey2 : IComparable<RevDataKey2>
	//	{
	//		public RevOrderCode2 RevOrder { get; set; }
	//		public string RevDeltaTitle { get; set; }
	//		public string RevShtNumber { get; set; }
	//
	//		private static int UniqueCode = 0;
	//
	//		public RevDataKey2() : this(new RevOrderCode2(), "", "") { }
	//
	//		public RevDataKey2(RevOrderCode2 revOrder, 
	//			string revDeltaTitle = "", string revShtNumber = "")
	//		{
	//			RevOrder = revOrder;
	//			RevDeltaTitle = revDeltaTitle;
	//			RevShtNumber = revShtNumber;
	//
	//			UniqueCode++;
	//		}
	//
	//		public int CompareTo(RevDataKey2 other)
	//		{
	//			int a = RevOrder.CompareTo(other.RevOrder);
	//
	//			if (a != 0) return a;
	//
	//			int b = RevDeltaTitle.CompareTo(other.RevDeltaTitle);
	//
	//			if (b != 0) return b;
	//
	//			int c = RevShtNumber.CompareTo(other.RevShtNumber);
	//
	//			if (c == 0) c = 1;
	//
	//			return c;
	//		}
	//	}
	//
	//	public class RevOrderCode2 : IComparable<RevOrderCode2>
	//	{
	//		public string RevAltId { get; set; }
	//		public string RevTypeCode { get; set; }
	//		public string RevDisciplineCode { get; set; }
	//
	//		public RevOrderCode2() { }
	//
	//		public RevOrderCode2(string id, string revTypeCode,
	//			string revDisciplineCode)
	//		{
	//			RevAltId = id;
	//			RevTypeCode = revTypeCode;
	//			RevDisciplineCode = revDisciplineCode;
	//		}
	//
	//		public int CompareTo(RevOrderCode2 other)
	//		{
	//			int a = RevAltId.CompareTo(other.RevAltId);
	//
	//			if (a != 0) return a;
	//
	//			int b = RevTypeCode.CompareTo(other.RevTypeCode);
	//
	//			if (b != 0) return b;
	//
	//			int C = RevDisciplineCode.CompareTo(other.RevDisciplineCode);
	//
	//			return C;
	//		}
	//	}

}