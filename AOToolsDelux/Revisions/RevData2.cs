using System;
using System.Collections;
using System.Collections.Generic;
using Autodesk.Revit.DB;

//using static AOTools.RevDataKey.ERevDataKey;
//using static AOTools.RevDataItems.ERevDataItems;

using static AOTools.SortKey;

using static  UtilityLibrary.MessageUtilities2;


namespace AOTools
{
	// this represents a collection of tag / clouds
	public class RevData2 : IEnumerable<KeyValuePair<string, RevDataItems2>>
	{
		private static SortedList<string, RevDataItems2> _revisionInfo2;

		public static SortedList<string, RevDataItems2> RevisionInfo
		{
			get
			{
				if (_revisionInfo2 == null ||
					_revisionInfo2.Count == 0) Init();

				return _revisionInfo2;
			}
		}

		public static void Init()
		{
			GetRevInfo(GetRevClouds());
		}

			private static IList<Element> GetRevClouds()
		{
			ElementCategoryFilter filterRevClouds = new ElementCategoryFilter(BuiltInCategory.OST_RevisionClouds);
			FilteredElementCollector collectClouds = new FilteredElementCollector(Revision.Doc);

			return collectClouds.WherePasses(filterRevClouds).
				WhereElementIsNotElementType().ToElements();
		}

		
		public IEnumerator<KeyValuePair<string, RevDataItems2>> GetEnumerator()
		{
			return  _revisionInfo2.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public static void GetRevisions(Document Doc)
		{
			IList<ElementId> revIds = Autodesk.Revit.DB.Revision.GetAllRevisionIds(Doc);

			foreach (ElementId e in revIds)
			{
				Autodesk.Revit.DB.Revision rev = Doc.GetElement(e) as Autodesk.Revit.DB.Revision;

				logMsgLn2("SequenceNumber"              , rev.SequenceNumber);
				logMsgLn2("Revision Id (RevisionNumber)", rev.RevisionNumber);
				logMsgLn2("Alt Id (IssuedBy)"           , rev.IssuedBy);
				logMsgLn2("Issued"                      , rev.Issued);
				logMsgLn2("Block Title (Description)"   , rev.Description);
				logMsgLn2("Delta Title (IssuedTo)"      , rev.IssuedTo);
				logMsgLn2("RevisionDate"                , rev.RevisionDate);
				logMsgLn2("NumberType"                  , rev.NumberType);
				logMsgLn2("Visibility"                  , rev.Visibility);
			}
		}



		private static void GetRevInfo(IList<Element> revClouds)
		{
			_revisionInfo2 =
				new SortedList<string, RevDataItems2>(revClouds.Count);

			foreach (Element e in revClouds)
			{
				if (!(e is RevisionCloud revCloud)) continue;

				ElementId cloudId = revCloud.get_Parameter(
					BuiltInParameter.REVISION_CLOUD_REVISION).AsElementId();

				if (!(Revision.Doc.GetElement(cloudId) is Autodesk.Revit.DB.Revision rev))
				{
					continue;
				}

				// create the item list
				RevDataItems2 item = new RevDataItems2();

				// start storing the information in the data list
				item.Selected        = false;
				item.Sequence        = rev.SequenceNumber;
				item.DeltaTitle		 = revCloud.get_Parameter(BuiltInParameter.REVISION_CLOUD_REVISION_ISSUED_TO).AsString();
				item.AltId           = revCloud.get_Parameter(BuiltInParameter.REVISION_CLOUD_REVISION_ISSUED_BY).AsString();
				item.ShtNum	 = GetSheetNumber(revCloud);
				item.TypeCode        = GetTypeSortCode(item.DeltaTitle);
				item.DisciplineCode	 = GetDisciplineSortCode(item.ShtNum);
				item.Visibility		 = rev.Visibility;
				item.RevisionId		 = revCloud.get_Parameter(BuiltInParameter.REVISION_CLOUD_REVISION_NUM).AsString();
				item.BlockTitle		 = revCloud.get_Parameter(BuiltInParameter.REVISION_CLOUD_REVISION_DESCRIPTION).AsString();
				item.RevisionDate	 = revCloud.get_Parameter(BuiltInParameter.REVISION_CLOUD_REVISION_DATE).AsString();
				item.Basis			 = revCloud.get_Parameter(BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS).AsString();
				item.Description	 = revCloud.get_Parameter(BuiltInParameter.ALL_MODEL_MARK)?.AsString();
				item.TagElemId		 = ElementId.InvalidElementId;
				item.CloudElemId	 = cloudId??ElementId.InvalidElementId;

				string key = GetSortKey(item.AltId, item.TypeCode,
					item.DisciplineCode, item.DeltaTitle, item.ShtNum);

				_revisionInfo2.Add(key, item);
			}
		}

		private static string GetSheetNumber(RevisionCloud revCloud)
		{
			ISet<ElementId> s = revCloud.GetSheetIds();

			foreach (ElementId ex in s)
			{
				return ((ViewSheet) Revision.Doc.GetElement(ex)).SheetNumber;
			}
			return null;
		}

	}
}
