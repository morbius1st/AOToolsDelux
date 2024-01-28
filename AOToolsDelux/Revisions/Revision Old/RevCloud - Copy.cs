#region Using directives

using System.Collections.Generic;
using System.Reflection;
using X = Microsoft.Office.Interop.Excel;


using Autodesk.Revit.Attributes;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

using static UtilityLibrary.MessageUtilities;

using static AOToolsDelux.RevInfoEnum;
using static AOToolsDelux.RevCloudData;


#endregion

// itemname:	RevCloud
// username:	jeffs
// created:		5/9/2018 6:14:10 PM


namespace AOToolsDelux
{

	[Transaction(TransactionMode.Manual)]
	public class RevCloud : IExternalCommand
	{
		public static UIDocument UiDoc;
		public static Document Doc;

		private static RevCloudData rcd = new RevCloudData();

		public Result Execute(
			ExternalCommandData commandData,
			ref string message, ElementSet elements)
		{
			UIApplication uiApp = commandData.Application;
			UiDoc = uiApp.ActiveUIDocument;
			Doc = UiDoc.Document;

			// this cleaned up the text display problem
			//			Application.SetCompatibleTextRenderingDefault(false);

			using (Transaction t = new Transaction(Doc, "revision cloud"))
			{
				t.Start();
				Process();
				t.Commit();
			}

			return Result.Succeeded;
		}


		internal bool Process()
		{
			int idx = 0;

			switch (idx)
			{
			case 0:
				{
					return ProcessRev();
				}
			case 1:
				{
					return ProcessFamily();
				}
			}

			return false;

		}

		internal bool ProcessFamily()
		{
			if (!Doc.IsFamilyDocument) return false;

			FamilyManager famMgr = Doc.FamilyManager;

			FamilyTypeSet famTypes = famMgr.Types;
			FamilyTypeSetIterator famTypeItor = famTypes.ForwardIterator();
			famTypeItor.Reset();
			while (famTypeItor.MoveNext())
			{
				FamilyType famType = famTypeItor.Current as FamilyType;
				logMsgDbLn2("fam type", famType.Name);
			}

			FamilyParameterSet famParas = famMgr.Parameters;
			FamilyParameterSetIterator famParaItor = famParas.ForwardIterator();
			famParaItor.Reset();
			while (famParaItor.MoveNext())
			{
				FamilyParameter famPara = famParaItor.Current as FamilyParameter;
				logMsgDbLn2("fam para", famPara.Definition.Name 
					+ "  :: " + famPara.Definition.ParameterGroup.ToString() 
					+ "  :: " + famPara.Definition.ParameterType.ToString());

			}

			famMgr.AddParameter("ASI", BuiltInParameterGroup.PG_IDENTITY_DATA, ParameterType.Text, true);

//			using (SubTransaction st = new SubTransaction(_doc))
//			{
//				st.Start();
//				famMgr.AddParameter("ASI", BuiltInParameterGroup.PG_IDENTITY_DATA, ParameterType.Text, true);
//				st.Commit();
//			}

			return true;
		}




		private bool ProcessRev()
		{
			ElementCategoryFilter filterRevClouds = new ElementCategoryFilter(BuiltInCategory.OST_RevisionClouds);
			ElementCategoryFilter filterRevTags = new ElementCategoryFilter(BuiltInCategory.OST_RevisionCloudTags);
			ElementCategoryFilter filterRevs = new ElementCategoryFilter(BuiltInCategory.OST_Revisions);

			FilteredElementCollector collectClouds = new FilteredElementCollector(Doc);
			FilteredElementCollector collectTags = new FilteredElementCollector(Doc);
			FilteredElementCollector collectRevs = new FilteredElementCollector(Doc);

			IList<Element> revClouds = collectClouds.WherePasses(filterRevClouds).
				WhereElementIsNotElementType().ToElements();
			IList<Element> revTags = collectTags.WherePasses(filterRevTags).
				WhereElementIsNotElementType().ToElements();
			IList<Element> revs = collectRevs.WherePasses(filterRevs).
				WhereElementIsNotElementType().ToElements();


			RevisionVisible = new bool[revs.Count + 1];
			foreach (Element e in revs)
			{
				Revision rev = e as Revision;
				RevisionVisible[rev.SequenceNumber] = rev.Visibility != RevisionVisibility.Hidden;
			}

			int opt = 5;

			switch (opt)
			{
			case 0:
				{
					ListRevisions1(revs);
					break;
				}
			case 1:
				{
					ListRevInfo1(revTags);
					break;
				}
			case 2:
				{
					ListRevInfo2(revTags);
					break;
				}
			case 3:
				{
					ListRevInfo3(revClouds);
					break;
				}
			case 4:
				{
					ListRevInfo4(GetRevInfo(revClouds, false));
					break;
				}
			case 5:
				{
					ExportToExcel(GetRevInfo(revClouds, false));
					break;
				}
			}

			return false;
		}

		private void ListRevisions1(IList<Element> revs)
		{
			int i = 0;

			logMsg(nl);
			logMsgDbLn2("revisions");

			foreach (Element e in revs)
			{
				Revision rev = e as Revision;
				i++;

				RevisionVisible[rev.SequenceNumber] = rev.Visibility != RevisionVisibility.Hidden;

				logMsgDbLn2("rev desc| " + i, rev.Description);
				logMsgDbLn2("rev seq num| " + i, rev.SequenceNumber.ToString());
				logMsgDbLn2("rev rev num| " + i, rev.RevisionNumber);
				logMsgDbLn2("rev rev date| " + i, rev.RevisionDate);
				logMsgDbLn2("rev rev visibility| " + i, rev.Visibility);
			}
		}


		


		private void ListRevInfo3(IList<Element> revClouds)
		{
			int i = 0;
			logMsg(nl);
			logMsgDbLn2("revision clouds");

			foreach (Element e in revClouds)
			{
				RevisionCloud revCloud = e as RevisionCloud;

				if (revCloud == null) continue;

				i++;

				Revision rev =
					Doc.GetElement(revCloud.get_Parameter(BuiltInParameter.REVISION_CLOUD_REVISION).AsElementId()) as Revision;

				if (rev == null) continue;

				int seq = rev.SequenceNumber;
				string shtNumber = GetSheetNumber(revCloud);

				if (RevisionVisible[seq])
				{
					string num = i.ToString();

					logMsg(nl);
					logMsgDbLn2("sheet number| " + num, shtNumber);
					logMsgDbLn2("revision seq| " + num, seq);

					logMsgDbLn2("revision title| " + num, revCloud.get_Parameter(BuiltInParameter.REVISION_CLOUD_REVISION_DESCRIPTION).AsString());
					logMsgDbLn2("revision id| " + num, revCloud.get_Parameter(BuiltInParameter.REVISION_CLOUD_REVISION_NUM).AsString());
					logMsgDbLn2("revision alt id| " + num, revCloud.get_Parameter(BuiltInParameter.REVISION_CLOUD_REVISION_ISSUED_BY).AsString());
					logMsgDbLn2("revision date| " + num, revCloud.get_Parameter(BuiltInParameter.REVISION_CLOUD_REVISION_DATE).AsString());

					logMsgDbLn2("revision desc| " + num, revCloud.get_Parameter(BuiltInParameter.ALL_MODEL_MARK)?.AsString() ?? "null");

					logMsgDbLn2("revision basis| " + num, revCloud.get_Parameter(BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS)?.AsString() ?? "null");
				}
			}
		}

		private void ListRevInfo4(SortedList<string, string[]> revInfo)
		{
			if (revInfo == null) return;

			foreach (KeyValuePair<string, string[]> riKvp in revInfo)
			{
				logMsg(nl);
				logMsgDbLn2("revision info");

				logMsgDbLn2("sort key", riKvp.Key);

				foreach (KeyValuePair<int, RevCol> rcKvp in RevCols)
				{
					logMsgDbLn2(rcKvp.Value.Title, riKvp.Value[rcKvp.Key]);
				}
			}
		}


		private SortedList<string, string[]> GetRevInfo(IList<Element> revClouds, bool all)
		{
			string CurrBulletin = "";

			logMsg(nl);
			logMsgDbLn2("revision clouds");

			SortedList<string, string[]> RevisionInfo = new SortedList<string, string[]>(revClouds.Count);

			foreach (Element e in revClouds)
			{
				RevisionCloud revCloud = e as RevisionCloud;

				if (revCloud == null) continue;

				Revision rev =
					Doc.GetElement(revCloud.get_Parameter(BuiltInParameter.REVISION_CLOUD_REVISION).AsElementId()) as Revision;

				if (rev == null) continue;

				int seq = rev.SequenceNumber;

				if (!all && !RevisionVisible[seq]) continue;

				string[] ri = new string[(int) REV_LEN];
				string Key = "";
				

				ri[(int) SEQ] = seq.ToString();
				ri[(int) REV_SHTNUM] = GetSheetNumber(revCloud);
				ri[(int) REV_TITLE] = revCloud.get_Parameter(BuiltInParameter.REVISION_CLOUD_REVISION_DESCRIPTION).AsString();
				ri[(int) REV_ID] = revCloud.get_Parameter(BuiltInParameter.REVISION_CLOUD_REVISION_NUM).AsString();
				ri[(int) REV_ALTID] = revCloud.get_Parameter(BuiltInParameter.REVISION_CLOUD_REVISION_ISSUED_BY).AsString();
				ri[(int) REV_NAME] = revCloud.get_Parameter(BuiltInParameter.REVISION_CLOUD_REVISION_ISSUED_TO).AsString();
				ri[(int) REV_DATE] = revCloud.get_Parameter(BuiltInParameter.REVISION_CLOUD_REVISION_DATE).AsString();
				ri[(int) REV_BASIS] = revCloud.get_Parameter(BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS).AsString();
				ri[(int) REV_DESC] = revCloud.get_Parameter(BuiltInParameter.ALL_MODEL_MARK)?.AsString();

				ri[(int) REV_ORDER] = SortKey.GetSortOrderCode(ri);

				// key = revision number (REV_ID) + (REV_SHTNUM) + (REV_TITLE) 
				Key = SortKey.GetSortKey(ri[(int) REV_ORDER], ri[(int) REV_TITLE], ri[(int) REV_SHTNUM]);

				ri[(int) REV_SORTSEQ] = Key;

				RevisionInfo.Add(Key, ri);

			}

			return RevisionInfo;
		}

		private string GetSheetNumber(RevisionCloud revCloud)
		{
			ISet<ElementId> s = revCloud.GetSheetIds();

			foreach (ElementId ex in s)
			{
				return ((ViewSheet) Doc.GetElement(ex)).SheetNumber;
			}
			return null;
		}


		private void ListRevInfo2(IList<Element> revTags)
		{
			int i = 0;
			logMsg(nl);
			logMsgDbLn2("revision tags");

			IList<string[]> RevisionInfo = new List<string[]>(revTags.Count);


			foreach (Element e in revTags)
			{
				string[] ri = new string[(int) REV_LEN];


				i++;
				IndependentTag revTag = e as IndependentTag;
				RevisionCloud revCloud = revTag.GetTaggedLocalElement() as RevisionCloud;
				Revision rev =
					Doc.GetElement(revCloud.get_Parameter(BuiltInParameter.REVISION_CLOUD_REVISION).AsElementId()) as Revision;

				// show only visible revisions
				if (rev != null && RevisionVisible[rev.SequenceNumber])
				{
//					ParameterSet psrt = revTag.Parameters;
//					ParameterSet ps = revCloud.Parameters;
//					IList<Parameter> po = revCloud.GetOrderedParameters();
					ISet<ElementId> s = revCloud.GetSheetIds();

					// get the sheet number
					foreach (ElementId ex in s)
					{
						//						ri[(int) REV_SHTNUM] = ((ViewSheet) _doc.GetElement(ex)).SheetNumber;
						logMsgDbLn2("sheet number", ((ViewSheet) Doc.GetElement(ex)).SheetNumber);
					}

					string num = i.ToString();


					logMsgDbLn2("tag rev seq| " + num, rev.SequenceNumber);

					logMsgDbLn2("revision title| " + num, revCloud.get_Parameter(BuiltInParameter.REVISION_CLOUD_REVISION_DESCRIPTION).AsString());
					logMsgDbLn2("revision id| " + num, revCloud.get_Parameter(BuiltInParameter.REVISION_CLOUD_REVISION_NUM).AsString());
					logMsgDbLn2("revision alt id| " + num, revCloud.get_Parameter(BuiltInParameter.REVISION_CLOUD_REVISION_ISSUED_BY).AsString());
					logMsgDbLn2("revision date| " + num, revCloud.get_Parameter(BuiltInParameter.REVISION_CLOUD_REVISION_DATE).AsString());

					logMsgDbLn2("revision desc 1?| " + num, revCloud.get_Parameter(BuiltInParameter.ALL_MODEL_MARK)?.AsString() ?? "null");

					logMsgDbLn2("revision basis 1?| " + num, revCloud.get_Parameter(BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS)?.AsString() ?? "null");
				}
			}
		}

			private void ListRevInfo1(IList<Element> revTags)
		{
			int i = 0;
			logMsg(nl);
			logMsgDbLn2("revision tags");
			foreach (Element e in revTags)
			{
				i++;
				IndependentTag revTag = e as IndependentTag;

				RevisionCloud revCloud = revTag.GetTaggedLocalElement() as RevisionCloud;
				Revision rev = Doc.GetElement(revCloud.get_Parameter(BuiltInParameter.REVISION_CLOUD_REVISION).AsElementId()) as Revision;

				// show only visible revisions
				if (rev != null && RevisionVisible[rev.SequenceNumber])
				{
					ParameterSet psrt = revTag.Parameters;

					ParameterSet ps = revCloud.Parameters;
					IList<Parameter> po = revCloud.GetOrderedParameters();
					ISet<ElementId> s = revCloud.GetSheetIds();

					// need to get:
					// rev number
					// mark
					// description
					// sheet number

					logMsg(nl);
					logMsgDbLn2("tag name| " + i, revTag.Name);
					logMsgDbLn2("tag text| " + i, revTag.TagText);

					logMsgDbLn2("tag rev desc| " + i, rev.Description);
					logMsgDbLn2("tag rev date| " + i, rev.RevisionDate);
					logMsgDbLn2("tag rev seq| " + i, rev.SequenceNumber);

					logMsgDbLn2("tag cloud num| " + i, revCloud.get_Parameter(BuiltInParameter.REVISION_CLOUD_REVISION_NUM).AsString());
					logMsgDbLn2("tag cloud date| " + i, revCloud.get_Parameter(BuiltInParameter.REVISION_CLOUD_REVISION_DATE).AsString());

					logMsgDbLn2("tag cloud desc| " + i, revCloud.Name);

					logMsgDbLn2("tag cloud para set count| " + i, ps.Size);
					logMsgDbLn2("tag cloud para ordered count| " + i, po.Count);

					// using ordered parameter list
					//					foreach (Parameter pm in p)
					//					{
					//						logMsgDbLn2("Tag cloud para| " + pm.Definition.Name, GetParameterInfo(pm));
					//					}

					// using all parameter list
//					foreach (Parameter pm in psrt)
//					{
//						logMsgDbLn2("Tag tag para| " + pm.Definition.Name, GetParameterInfo(pm) + " :: " + pm.GUID);
//					}

					// using all parameter list
					foreach (Parameter pm in ps)
					{
						logMsgDbLn2("Tag cloud para| " + pm.Definition.Name, GetParameterInfo(pm) + " :: " + pm.Id);
					}

					// using all parameter list
					foreach (ElementId ex in s)
					{
						logMsgDbLn2("sheet info", ((ViewSheet) Doc.GetElement(ex)).SheetNumber);
					}
				}


			}
		}

		private void ListRevClouds(IList<Element> revClouds)
		{
			int i = 0;
			
			logMsgDbLn2("revision clouds");
			foreach (RevisionCloud revCloud in revClouds)
			{
				i++;
			
				logMsgDbLn2("cloud name|" + i, revCloud.Name);
			
				logMsgDbLn2("cloud date|" + i, revCloud.get_Parameter(BuiltInParameter.REVISION_CLOUD_REVISION_DATE).AsString());
				logMsgDbLn2("cloud desc|" + i, revCloud.get_Parameter(BuiltInParameter.REVISION_CLOUD_REVISION_DESCRIPTION)?.AsString());
				logMsgDbLn2("cloud num|" + i, revCloud.get_Parameter(BuiltInParameter.REVISION_CLOUD_REVISION_NUM)?.AsString());
				logMsgDbLn2("cloud mark|" + i, revCloud.get_Parameter(BuiltInParameter.ALL_MODEL_MARK)?.AsString() ?? "no mark");
				logMsgDbLn2("cloud view specific|" + i, revCloud.ViewSpecific.ToString());
			}
		}



		private static string GetParameterInfo(Parameter p)
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

		private bool ExportToExcel(SortedList<string, string[]> revInfo)
		{
			X.Application excel = new X.Application();

			if (excel == null) return false;


			excel.Visible = true;

			X.Workbook workbook = excel.Workbooks.Add(Missing.Value);
			X.Worksheet ws;

			ws = workbook.Sheets.Item[1] as X.Worksheet;

			ws.Name = "Revisions";

			// add the header row in bold
			int row = 1;

			foreach (KeyValuePair<int, RevCol> kvp in RevCols)
			{
				// key = the order for the columns
				// value = the column data

				// if not export - skip this column
				if (!kvp.Value.Export || kvp.Value.Column < 0) continue;

				ws.Cells[1, kvp.Value.Column] = kvp.Value.Title;
			}

			var range = ws.get_Range("A1", "Z1");

			range.Font.Bold = true;
			

			row = 2;

			foreach (KeyValuePair<string, string[]> riKvp in revInfo)
			{
				foreach (KeyValuePair<int, RevCol> rcKvp in RevCols)
				{
					// key = the order for the columns
					// value = the column data

					// if not export - skip this column
					if (!rcKvp.Value.Export || rcKvp.Value.Column < 0) continue;

					ws.Cells[row, rcKvp.Value.Column] = riKvp.Value[rcKvp.Key];
				}
				row++;
			}

			range.EntireColumn.AutoFit();
			return true;
		}
	}
}
