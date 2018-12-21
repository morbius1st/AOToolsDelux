#region + Using Directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

using static UtilityLibrary.MessageUtilities;

#endregion


// projname: AOToolsParameterVue
// itemname: ParamView
// username: jeffs
// created:  12/8/2018 5:34:04 PM


namespace AOToolsParameterVue
{

	[Transaction(TransactionMode.Manual)]
	public class ParamView : IExternalCommand
	{
		private const int PAD_RIGHT = 18;

		private const int FIRSTPASS_MAX = 19;
		private int firstPassItem = 0;
		private bool[] firstPass = new bool[FIRSTPASS_MAX + 1];

		private static  UIDocument    _uiDoc;
		internal static Document      _doc;

		private readonly ParamViewMsg _form = new ParamViewMsg(); 

		public Result Execute(
			ExternalCommandData commandData,
			ref string message,
			ElementSet elements)
		{
			UIApplication uiApp = commandData.Application;
			_uiDoc = uiApp.ActiveUIDocument;
			_doc   = _uiDoc.Document;

			_form.message.HorizontalScrollBarVisibility = ScrollBarVisibility.Visible;


			// this cleaned up the text display problem
			//			Application.SetCompatibleTextRenderingDefault(false);

			using (Transaction tg = new Transaction(_doc, "AO Modify Family Parameters"))
			{
				tg.Start();
				Process();
				tg.Commit();
			}


			return Result.Succeeded;
		}


		private void Process()
		{
			logMsgDbLn2("@ParamView|", "Process");

//			GetAllFamilies();

//			GetAllElements1();

//			GetAllElements2();

			GetAllElements3();

		}

		private void GetAllElements3()
		{
			string name;

			StringBuilder sb = new StringBuilder();

			FilteredElementCollector elemCollector =
				new FilteredElementCollector(_doc).OfClass(typeof(TextNoteType));

			ICollection<Element> elements = elemCollector.ToElements();

			foreach (Element el in elements)
			{
				firstPassItem = 0;

				sb.AppendLine(ListParameters(el).ToString());
			}

			_form.message.Text = sb.ToString();

			_form.ShowDialog();

		}

		private StringBuilder ListParameters(Element el)
		{
			StringBuilder sb = new StringBuilder();

			// this provies some extra and un-needed parameters
			ParameterSet ps = el.Parameters;

			// both are basically the same
			ParameterMap pm = el.ParametersMap;
			IList<Parameter> ipm = el.GetOrderedParameters();


			sb.AppendLine(logMsgDbS("name", el.Name));

			// no good - this is null
//			sb.AppendLine(logMsgDbS("category", el.Category.Name));

			string header = "value".PadRight(PAD_RIGHT);
			header = "storage type".PadRight(PAD_RIGHT);
//			header += " :: " + "read-only".PadRight(PAD_RIGHT);
//			header += " :: " + "user-modifiable".PadRight(PAD_RIGHT);
//			header += " :: " + "has value".PadRight(PAD_RIGHT);
			header += " :: " + "param type".PadRight(PAD_RIGHT);
			header += " :: " + "unit type".PadRight(PAD_RIGHT);
			header += " :: " + "disp unit type".PadRight(PAD_RIGHT);

			sb.AppendLine(logMsgDbS("definition name", header));

			foreach (Parameter p in ipm)
			{
				sb.AppendLine(logMsgDbS(p.Definition.Name, ParameterValue(p)));

//				if (p.Definition.Name.Equals("Text Size"))
//				{
//					p.Set(UnitUtils.ConvertToInternalUnits(0.25, p.DisplayUnitType));
//				}
			}

			return sb;
		}


		private string ParameterValue(Parameter p)
		{
			string storageType = p.StorageType.ToString();
			string result = (p.AsValueString() ?? "").Trim();

			if (string.IsNullOrWhiteSpace(result))
			{
				switch (p.StorageType)
				{
				case StorageType.Double:
					{
						storageType = "double";
						result = p.AsValueString();
						break;
					}
				case StorageType.ElementId:
					{
						storageType = "element id";
						ElementId id = new ElementId(p.AsElementId().IntegerValue);
						Element   e  = _doc.GetElement(id);

						result = e?.Name ?? "Null Element";
						break;
					}
				case StorageType.Integer:
					{
						storageType = "integer";
						result = p.AsInteger().ToString();
						break;
					}
				case StorageType.None:
					{
						storageType = "none";
						result = p.AsValueString();
						break;
					}
				case StorageType.String:
					{
						storageType = "string";
						result = p.AsString();
						break;
					}
				}
			}


			if (p.StorageType == StorageType.ElementId && !firstPass[firstPassItem])
			{

				ElementArray ea = GetSimilarForElement(p.AsElementId());

				ElementType et = _doc.GetElement(p.AsElementId()) as ElementType;

				if (ea != null)
				{
					firstPass[firstPassItem++] = true;

					logMsgDbLn2("getting similar", et.Name + " :: " + et.FamilyName);

					foreach (Element e in ea)
					{
						logMsgDbLn2("type", e.Name);
					}
				}
			}



			result = result.PadRight(18);
			result += " :: " + storageType.ToString().PadRight(PAD_RIGHT);


			// not valid info
			//			result += " :: " + p.IsReadOnly.ToString().PadRight(PAD_RIGHT);

			// not valid info
			//			result += " :: " + p.UserModifiable.ToString().PadRight(PAD_RIGHT);

			// valid info
			//			result += " :: " + p.HasValue.ToString().PadRight(PAD_RIGHT);

			result += " :: " + p.Definition.ParameterType.ToString().PadRight(PAD_RIGHT);
			result += " :: " + p.Definition.UnitType.ToString().PadRight(PAD_RIGHT);

			// does not provide much useful info
//			result += " :: " + p.Definition.ParameterGroup.ToString().PadRight(18);


			try
			{
				result += " :: " + p.DisplayUnitType.ToString().PadRight(PAD_RIGHT);
			}
			catch
			{
				result += " :: (no unit type)".PadRight(PAD_RIGHT);
			}

			return result;
		}


		private ElementArray GetSimilarForElement(ElementId elementId)
		{
			ElementArray ea = null;

			if (elementId != null)
			{
				ElementType et = _doc.GetElement(elementId) as ElementType;

				if (et != null)
				{
					ea = new ElementArray();

					foreach (ElementId eid in et.GetSimilarTypes())
					{
						ea.Append(_doc.GetElement(eid));
					}
				}
			}

			return ea;
		}




		private void GetAllElements2()
		{
			string typeName;

			StringBuilder sb = new StringBuilder();

			FilteredElementCollector elemCollector =
				new FilteredElementCollector(_doc).WhereElementIsElementType();
			FilteredElementCollector notElemCollector =
				new FilteredElementCollector(_doc).WhereElementIsNotElementType();

			FilteredElementCollector allElemCollector =
				elemCollector.UnionWith(notElemCollector);

			ICollection<Element> elements = allElemCollector.ToElements();

			SortedSet<string> elems = new SortedSet<string>();

			foreach (Element el in elements)
			{
				typeName = el.GetType().Name;

				if (!elems.Contains(typeName))
				{
					elems.Add(typeName);
				}
			}

			foreach (string name in elems)
			{
				sb.AppendLine(name);
			}

			_form.message.Text = sb.ToString();

			_form.ShowDialog();

		}


		private void GetAllElements1()
		{
			StringBuilder sb = new StringBuilder();

			FilteredElementCollector collector =
				new FilteredElementCollector(_doc).WhereElementIsNotElementType();

			IList<string> Cats = new List<string>();

			foreach (Element el in collector)
			{
				if (el.Category != null)
				{
					if (!Cats.Contains(el.Category.Name))
					{
						Cats.Add(el.Category.Name);
					}
				}
				else
				{
					logMsgDbLn2("null category", el.Name ?? "name is null too");
				}
			}

			if (Cats.Count > 0)
			{
				foreach (string cat in Cats)
				{
					sb.AppendLine(cat);
				}

				TaskDialog.Show("Categories", sb.ToString());
			}
			else
			{
				TaskDialog.Show("Categories", "none");
			}

		}


		private void GetAllFamilies()
		{
			StringBuilder sb = new StringBuilder();

			FilteredElementCollector collector =
				new FilteredElementCollector(_doc);

			ICollection<Element> elements =
				collector.OfClass(typeof(Family)).ToElements();

			foreach (Element element in elements)
			{
				sb.AppendLine(element.Name);
			}

			TaskDialog.Show("Parameter Vue", sb.ToString());
		}
	}

}
