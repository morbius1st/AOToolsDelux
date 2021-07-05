using System.Diagnostics;
using System.Windows.Forms;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.UI;
using DluxMeasure.Utility;
using Application = Autodesk.Revit.ApplicationServices.Application;
using View = Autodesk.Revit.DB.View;

using static DluxMeasure.Utility.Util;
using static DluxMeasure.AppSettings.ConfigSettings.SettingsUsr;

using RevitLibrary;

namespace DluxMeasure
{
	[Transaction(TransactionMode.Manual)]
	public class DxMeasure : IExternalCommand
	{
		public Result Execute(
			ExternalCommandData commandData,
			ref string message, ElementSet elements
			)
		{
			UIApplication uiApp = commandData.Application;
			Document _doc = uiApp.ActiveUIDocument.Document;

			DlxMeasure mx = DlxMeasure.Instance();

			// this cleaned up the text display problem
			//			Application.SetCompatibleTextRenderingDefault(false);
			using (TransactionGroup tg = new TransactionGroup(_doc, "AO delux measure"))
			{
				tg.Start();
				mx.Measure(uiApp);
				tg.RollBack();
			}

			return Result.Succeeded;
		}
	}


    public class DlxMeasure
    {
		private static FormDlxMeasure _form;

		internal static UIDocument _uiDoc;
		internal static Document _doc;

		internal static DlxMeasure instance { get; private set; } = null;

		internal static bool ShowWorkplane { get; set; } = false;


		private DlxMeasure()
		{
			SmUsrInit();
		}

		private void SetInfo(UIApplication uiApp)
		{
			_uiDoc =  uiApp.ActiveUIDocument;
			_doc =  _uiDoc.Document;
		}

		public static DlxMeasure Instance()
		{

			if (instance == null)
			{
				instance = new DlxMeasure();
			}

			return instance;
		}

	#region Public Face

		public bool Measure(UIApplication uiApp)
		{
			SetInfo(uiApp);

			_form = new FormDlxMeasure();

			View av = _doc.ActiveView;

			Util.VType vtype = GetViewType(av);

			if (vtype.VTCat == Util.VTtypeCat.OTHER)
			{
				return false;
			}

			Plane plane;

			// get the current sketch / work plane
			plane = av.SketchPlane?.GetPlane();

			if (plane == null && (vtype.VTCat == Util.VTtypeCat.D2_WITHPLANE ||
				vtype.VTCat == Util.VTtypeCat.D3_WITHPLANE))
			{
				using (Transaction t = new Transaction(_doc, "measure points"))
				{
					t.Start();
					plane = Plane.CreateByNormalAndOrigin(
						_doc.ActiveView.ViewDirection,
						new XYZ(0, 0, 0));

					SketchPlane sp = SketchPlane.Create(_doc, plane);

					av.SketchPlane = sp;

					t.Commit();
				}
			}

			MeasurePointsSetup(av, vtype);

			return true;
		}

		public bool ShowHideWorkplane()
		{
			View av = _doc.ActiveView;
			Plane p = av.SketchPlane?.GetPlane();

			if (p == null )
			{
				return false;
			}

			return ShowHideWorkplane(p, av);
		}

	#endregion

	#region Private Methods

		private static bool MeasurePointsSetup(View av, Util.VType vtype)
		{
			bool again = true;

			XYZ normal = XYZ.BasisZ;
			XYZ workingOrigin = XYZ.Zero;
			XYZ actualOrigin = XYZ.Zero;

			if (RvtLibrary.IsPlaneOrientationAcceptable(_uiDoc))
			{
				Plane p = av.SketchPlane?.GetPlane();
				string planeName = av.SketchPlane?.Name ?? "No Name";

				if (p != null)
				{
					normal = p.Normal;
					actualOrigin = p.Origin;

					if (vtype.VTSub != VTypeSub.D3_VIEW)
					{
						workingOrigin = p.Origin;
					}
				}

				return MeasurePoints(workingOrigin, vtype, planeName);
			}
			else
			{
				TaskDialog td = new TaskDialog("Workplane Orientation");
				td.MainIcon = TaskDialogIcon.TaskDialogIconWarning;
				td.MainInstruction = "The angle of the Worlplane is too steep in this view.  "
					+ "Please adjust the Workplane's orientation or "
					+ "make a different Workplane active in order to proceed";
				td.CommonButtons = TaskDialogCommonButtons.Cancel;
				td.DefaultButton = TaskDialogResult.Cancel;

				TaskDialogResult result = td.Show();

			}

			return false;
		}

		private static bool MeasurePoints(XYZ workingOrigin, Util.VType vtype, string planeName)
		{
			bool again = true;

			PointMeasurements? pm;

			while (again)
			{
				pm = GetPts(workingOrigin);

				if (pm != null)
				{
					_form.UpdatePoints(pm, vtype, planeName);

					DialogResult result = _form.ShowDialog();
					switch (result)
					{
					case DialogResult.Cancel:
						again = false;
						break;
					}
				}
				else
				{
					TaskDialog td = new TaskDialog("No Points Selected");
					td.MainIcon = TaskDialogIcon.TaskDialogIconWarning;
					td.MainInstruction = "Please select two points to proceed";
					td.CommonButtons = TaskDialogCommonButtons.Cancel | TaskDialogCommonButtons.Retry;
					td.DefaultButton = TaskDialogResult.Retry;

					TaskDialogResult result = td.Show();

					if (result == TaskDialogResult.Cancel)
					{
						break;
					}
				}
			}

			return true;
		}


		private static PointMeasurements? GetPts(XYZ workingOrigin)
		{
			_form.tbxMessage.ResetText();

			XYZ startPoint;
			XYZ endPoint;

			try
			{
				View avStart = _doc.ActiveView;

				startPoint = _uiDoc.Selection.PickPoint(snaps, "Select Point");
				if (startPoint == null) return null;

				View avEnd = _doc.ActiveView;

				// cannot change views between points
				if (avStart.Id.IntegerValue != avEnd.Id.IntegerValue)
				{
					return null;
				}

				endPoint = _uiDoc.Selection.PickPoint(snaps, "Select Point");
				if (endPoint == null) return null;
			}
			catch
			{
				return null;
			}
			return new PointMeasurements(startPoint, endPoint, workingOrigin);
		}

		private static bool ShowHideWorkplane(Plane p, View av)
		{
			if (p == null) { return false; }

			try
			{
				using (Transaction t = new Transaction(_doc, "measure points"))
				{
					t.Start();

					if (ShowWorkplane)
					{
						av.ShowActiveWorkPlane();
					}
					else
					{
						av.HideActiveWorkPlane();
					}

					t.Commit();
				}
			}
			catch
			{
				return false;
			}

			return true;
		}

	#endregion


    }
}
