#region + Using Directives
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows;

using static CsDeluxMeasure.RevitSupport.RevitUtil;
using CsDeluxMeasure.Windows;
using UtilityLibrary;

#endregion

// user name: jeffs
// created:   9/17/2023 10:39:21 PM

namespace CsDeluxMeasure.RevitSupport
{
	public class DxMeasure
	{
			#region private fields

		private UIDocument uiDoc;

		private Window revitWindow;

		private PointMeasurements points;

	#endregion

	#region ctor

		public DxMeasure(UIDocument uiDoc)  //  , Window revitWindow)
		{
			this.uiDoc = uiDoc;

			revitWindow = RevitLibrary.RvtLibrary.WindowHandle(uiDoc.Application.MainWindowHandle);

			// this.revitWindow = revitWindow;
		}

	#endregion

	#region public properties

		public PointMeasurements Points => points;

	#endregion

	#region private properties

		private UIDocument _uiDoc => uiDoc;
		private Document _doc => uiDoc.Document;

	#endregion

	#region public methods

		public void SetPointsToZero()
		{
			points = PointMeasurements.Zero();

		}

		public bool Measure()
		{
			// M.W.WriteLine1("\nmeasure-begin");

			bool result;

			View av = _doc.ActiveView;
			RevitUtil.VType vtype = RevitUtil.GetViewType(av);

			dxPrepPlane(av, vtype);
			result = dxMeasurePoints(av, vtype);

			// Debug.WriteLine("end measure");

			// M.W.WriteLine1("measure-end\n");
			return result;
		}

	#endregion

	#region private methods

		// view does not have a sketchplane
		private void dxPrepPlane(View av, RevitUtil.VType vtype)
		{

			Plane plane = av.SketchPlane?.GetPlane();

			if (plane == null && (vtype.VTCat == RevitUtil.VTtypeCat.D2_WITHPLANE
				|| vtype.VTCat == RevitUtil.VTtypeCat.D3_WITHPLANE))
			{
				// cannot use the current workplane - make a good one
				using (Transaction t = new Transaction(_doc, "Delux Measure Points"))
				{
					t.Start();
					{
						plane = Plane.CreateByNormalAndOrigin(
							_doc.ActiveView.ViewDirection,
							new XYZ(0, 0, 0));

						SketchPlane sp = SketchPlane.Create(_doc, plane);

						av.SketchPlane = sp;

						// av.ShowActiveWorkPlane();
					}
					t.Commit();
				}
			}

			// Debug.WriteLine("end prep plane");
		}

		private bool dxMeasurePoints(View av, RevitUtil.VType vtype)
		{
			// Debug.WriteLine("begin measure points");

			// XYZ normal = XYZ.BasisZ;
			// XYZ actualOrigin = XYZ.Zero;
			XYZ workingOrigin = XYZ.Zero;

			if (RevitUtil.IsPlaneOrientationAcceptable(_uiDoc))
			{
				Plane p = av.SketchPlane?.GetPlane();
				string planeName = av.SketchPlane?.Name ?? "No Name";

				if (p != null)
				{
					// normal = p.Normal;
					// actualOrigin = p.Origin;

					if (vtype.VTSub != VTypeSub.D3_VIEW)
					{
						workingOrigin = p.Origin;
					}
				}

				// Debug.WriteLine("end 1 measure points");

				return dxGetPoints(workingOrigin);
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

			// Debug.WriteLine("end 2 measure points");

			return false;
		}

		private bool dxGetPoints(XYZ workingOrigin)
		{
			// Debug.WriteLine("begin get points");

			points = GetPts(workingOrigin);

			if (points.IsVoid)
			{
				// Debug.WriteLine("end 3 get points");
				return false;
			}
			else
			if (!points.IsValid)
			{
				return false;


				/*
				TaskDialog td = new TaskDialog("Invalid Points Selected");
				td.MainIcon = TaskDialogIcon.TaskDialogIconWarning;
				td.MainInstruction = "Please select two points to proceed";
				td.CommonButtons = TaskDialogCommonButtons.Cancel | TaskDialogCommonButtons.Retry;
				td.DefaultButton = TaskDialogResult.Retry;

				TaskDialogResult tdResult = td.Show();

				if (tdResult == TaskDialogResult.Cancel)
				{
					// Debug.WriteLine("end 2 get points");

					return false;
				}
				*/

			}

			// Debug.WriteLine("end 1 get points");

			return true;
		}

		private PointMeasurements GetPts(XYZ workingOrigin)
		{
			// Debug.WriteLine("begin getPts");

			XYZ startPoint;
			XYZ endPoint;
			double rotation;
			bool atPtOne = true;


			try
			{
				View avStart = _doc.ActiveView;

				rotation = Math.Atan(avStart.UpDirection.X / avStart.UpDirection.Y);

				// Transform t = Transform.CreateRotation(XYZ.BasisZ, rotation);
				
				startPoint = _uiDoc.Selection.PickPoint(snaps, "Select Point");
				// startPoint = _uiDoc.Selection.PickPoint("Select Point");

				atPtOne = false;

				if (startPoint == null) return PointMeasurements.InValid();
				View avEnd = _doc.ActiveView;

				// cannot change views between points
				if (avStart.Id.IntegerValue != avEnd.Id.IntegerValue)
				{
					return PointMeasurements.InValid();
				}
				// endPoint = _uiDoc.Selection.PickPoint(snaps, "Select Point");
				endPoint = _uiDoc.Selection.PickPoint("Select Point");

				if (endPoint == null) return PointMeasurements.InValid();

			}
			catch
			{
				if (atPtOne)
				{
					return PointMeasurements.SetVoid();
				}

				revitWindow.Activate();

				User32dll.SendKeyCode(0x01);

				return PointMeasurements.SetVoid();

				// return PointMeasurements.InValid();
			}

			// Debug.WriteLine("end getPts");

			return new PointMeasurements(startPoint, endPoint, workingOrigin, rotation);
		}

	#endregion

	#region event consuming

	#endregion

	#region event publishing

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

	#endregion


		public override string ToString()
		{
			return $"this is {nameof(DxMeasure)}";
		}
	}
}
