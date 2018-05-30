#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

using static AOTools.RevCloudUtil;

using static UtilityLibrary.MessageUtilities2;

using static AOTools.SelectCriteria.ECompare;
using static AOTools.SelectCriteria.EVisibility;


using UtilityLibrary;


#endregion

// itemname:	RevCloud
// username:	jeffs
// created:		5/9/2018 6:14:10 PM


namespace AOTools
{

	[Transaction(TransactionMode.Manual)]
	public class RevCloud : IExternalCommand
	{
		public static UIDocument UiDoc;
		public static Document Doc;

		public RevSummary rs;

		public Result Execute(
			ExternalCommandData commandData,
			ref string message, ElementSet elements)
		{
			UIApplication uiApp = commandData.Application;
			UiDoc = uiApp.ActiveUIDocument;
			Doc = UiDoc.Document;

			using (Transaction t = new Transaction(Doc, "revision cloud"))
			{
				t.Start();
				

				Process();
//				test1();
//				test2();
//				test3();

				t.Commit();
			}

			return Result.Succeeded;
		}

		internal bool Process()
		{
			RevColumns.AssignColumns();

			// initalize and read all of the revision data
//			RevCloudData rcd = RevCloudData.GetInstance();

			#region + Original Tests

//			ListRevInfo5(rcd.MasterList);

			// these all relate to the original system
//			SelectAll(rcd);
//			SelectTest1(rcd);
//			SelectTest2(rcd);
//			SelectTest3(rcd);
//			SelectTest4(rcd);
//			test5(rcd.MasterList);
//			test6(rcd.MasterList);

			#endregion

			// these are for the new system
			RevCloudData2 rcd2 = RevCloudData2.GetInstance();
			ListDescriptions();
			SelectAll2(rcd2);

			return true;
		}

		private void SelectAll2(RevCloudData2 rcd2)
		{
			SelectCriteria sc = new SelectCriteria(VISIBILITY_ALL);

			int count = rcd2.Select(sc);

			logMsg2("count| " + count);
			logMsg2(nl);
			ListRevInfo2_1(rcd2.SelectedList);
			logMsg2(nl);
		}

		private void ListSummary2(RevSummary rs)
		{
			string[] names = Enum.GetNames(typeof(RevSummary2.EListSubject));

			int i = 0;

			// scan through each of the lists and list its values
			foreach (KeyValuePair<int, RevSummary.ListData> kvp in rs)
			{
				logMsgLn2("listing for", names[i++]);
				logMsgLn2("choice is", ">" + kvp.Value.Choice + "<");
				logMsgLn2("count", kvp.Value.Summary.Count);

				int j = 0;

				foreach (string s in kvp.Value.Summary)
				{
					logMsgLn2("item " + j, s);
				}
			}
		}

		#region + Original Tests

		// these are for the old system
		private void test5(SortedList<RevDataKey, RevDataItems> rcd)
		{
			rs = new RevSummary(rcd);
			rs.UpdateLists();

			ListSummary(rs);
		}

		private void test6(SortedList<RevDataKey, RevDataItems> rcd)
		{
			rs = new RevSummary(rcd);
			rs.AlternateId = "3";
			rs.UpdateLists();

			ListSummary(rs);
		}

		private void SelectAll(RevCloudData rcd)
		{
			SelectCriteria sc = new SelectCriteria(VISIBILITY_ALL);

			int count = rcd.Select(sc);

			logMsg2("count| " + count);
			logMsg2(nl);
			ListRevInfo5(rcd.SelectedList);
			logMsg2(nl);

		}
		private void SelectTest1(RevCloudData rcd)
		{
			SelectCriteria sc = new SelectCriteria(VISIBILITY_CLOUDANDTAG);
			sc.Basis(EQUAL, "pcc");

			int count = rcd.Select(sc);

			logMsg2("count| " + count);
			logMsg2(nl);
			ListRevInfo5(rcd.SelectedList);
			logMsg2(nl);
		}
		private void SelectTest2(RevCloudData rcd)
		{
			SelectCriteria sc = new SelectCriteria(VISIBILITY_TAGONLY);
			sc.Basis(EQUAL, "rfi");

			int count = rcd.Select(sc);

			logMsg2("count| " + count);
			logMsg2(nl);
			ListRevInfo5(rcd.SelectedList);
			logMsg2(nl);
		}
		private void SelectTest3(RevCloudData rcd)
		{
			SelectCriteria sc = new SelectCriteria(VISIBILITY_ALL);
			sc.Basis(GREATER_THEN_OR_EQUAL, "pcc");

			int count = rcd.Select(sc);

			logMsg2("count| " + count);
			logMsg2(nl);
			ListRevInfo5(rcd.SelectedList);
			logMsg2(nl);
		}
		private void SelectTest4(RevCloudData rcd)
		{
			SelectCriteria sc = new SelectCriteria(VISIBILITY_HIDDEN);
			sc.DeltaTitle(EQUAL, "rfi 301");

			int count = rcd.Select(sc);

			logMsg2("count| " + count);
			logMsg2(nl);
			ListRevInfo5(rcd.SelectedList);
			logMsg2(nl);
		}


		private void ListSummary(RevSummary rs)
		{
			string[] names = Enum.GetNames(typeof(RevSummary.EListSubject));

			int i = 0;

			// scan through each of the lists and list its values
			foreach (KeyValuePair<int, RevSummary.ListData> kvp in rs)
			{
				logMsgLn2("listing for", names[i++]);
				logMsgLn2("choice is", ">" + kvp.Value.Choice + "<");
				logMsgLn2("count", kvp.Value.Summary.Count);

				int j = 0;

				foreach (string s in kvp.Value.Summary)
				{
					logMsgLn2("item " + j, s);
				}
			}
		}

		#endregion
	}
}
