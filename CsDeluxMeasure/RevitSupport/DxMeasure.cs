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
using SettingsManager;
using Tests01.RevitSupport;
using UtilityLibrary;

#endregion

// user name: jeffs
// created:   9/17/2023 10:39:21 PM

namespace CsDeluxMeasure.RevitSupport
{
	public class DxMeasure
	{
			#region private fields

		// private UIDocument uiDoc;

		// private Window revitWindow;

		private PointMeasurements points;

		private ReferenceIntersector ri;

		private XYZ viewDir;
		private View3D v3d;

	#endregion

	#region ctor

		public DxMeasure()  //  , Window revitWindow)
		{
			// this.uiDoc = uiDoc;
			//
			// revitWindow = RevitLibrary.RvtLibrary.WindowHandle(uiDoc.Application.MainWindowHandle);

			// this.revitWindow = revitWindow;
		}

	#endregion

	#region public properties

		public PointMeasurements Points => points;

	#endregion

	#region private properties

		// private UIDocument _uiDoc => uiDoc;
		// private Document _doc => uiDoc.Document;

	#endregion

	#region public methods

		public bool MeasurePoints()
		{
			bool result;

			R.ActivateRevit();

			/*
			using (TransactionGroup tg = new TransactionGroup(R.Doc, "Delux Measure"))
			{
				tg.Start();
				{
					// get the points
				}
				tg.RollBack();
			}
			*/

			result = Measure();

			if (!result) return result;

			UserSettings.Admin.Read();

			UpdatePoints();

			return result;
		}

		public void UpdatePoints()
		{
			R.Mw.Points = R.Dx.Points;
			R.Mw.init();
			R.Mm.Points = R.Dx.Points;
		}


		public void SetPointsToZero()
		{
			points = PointMeasurements.Zero();

		}

		public bool Measure()
		{
			
			// R.UpdateDoc();

			bool result;

			View av = R.Doc.ActiveView;

			View av2 = R.UiDoc.ActiveGraphicalView;
			bool b1 = R.Doc.IsFamilyDocument;

			RevitUtil.VType vtype = RevitUtil.GetViewType(av);

			using (Transaction t = new Transaction(R.Doc, "Delux Measure Points"))
			{
				t.Start();
				{
					v3d = av as View3D;

					if (!dxPrepPlane(av, vtype)) return false;

					result = dxMeasurePoints(av, vtype);
				}

				t.RollBack();
			}

			return result;
		}

	#endregion

	#region private methods

		// does view have a viable work plane (or does not need one)
		private bool dxPrepPlane(View av, RevitUtil.VType vtype)
		{
			Plane plane = av.SketchPlane?.GetPlane();

			viewDir = av.ViewDirection.Negate();

			try
			{
				if ((plane == null && (
					vtype.VTCat == VTtypeCat.D2_WITHPLANE || 
					vtype.VTCat == VTtypeCat.D2_WITHOUTPLANE))
					|| vtype.VTCat == VTtypeCat.D3_WITHPLANE)
				{
					plane = Plane.CreateByNormalAndOrigin(
						R.Doc.ActiveView.ViewDirection,
						av.Origin);
						// new XYZ(0, 0, 0));

					SketchPlane sp = SketchPlane.Create(R.Doc, plane);

					av.SketchPlane = sp;
					
				}
			}
			catch 
			{
				return false;
			}

			return true;
		}

		private bool dxMeasurePoints(View av, VType vtype)
		{
			// Debug.WriteLine("begin measure points");

			// XYZ normal = XYZ.BasisZ;
			// XYZ actualOrigin = XYZ.Zero;
			XYZ workingOrigin = XYZ.Zero;

			// bool b1 = RevitUtil.IsPlaneOrientationAcceptable(R.UiDoc);
			// bool b2 = vtype.VTCat == VTtypeCat.D2_WITHOUTPLANE;
			// bool b3 = b1 || b2;


			if (RevitUtil.IsPlaneOrientationAcceptable(R.UiDoc)
				// || vtype.VTCat == VTtypeCat.D2_WITHOUTPLANE
				|| vtype.VTCat == VTtypeCat.D2_NOPLANE
				)
			{
				if (vtype.VTCat != VTtypeCat.D2_WITHOUTPLANE)
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
					
				}
				// Debug.WriteLine("end 1 measure points");

				return dxGetPoints(workingOrigin, vtype.VTCat == VTtypeCat.D3_WITHPLANE);
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

		private bool dxGetPoints(XYZ workingOrigin, bool is3D)
		{
			// Debug.WriteLine("begin get points");

			points = GetPts(workingOrigin, is3D);

			if (points.IsVoid)
			{
				return false;
			}
			else
			if (!points.IsValid)
			{
				return false;
			}

			return true;
		}

		private PointMeasurements GetPts(XYZ workingOrigin, bool is3D)
		{
			// Debug.WriteLine("begin getPts");

			XYZ startPoint;
			XYZ endPoint;
			double rotation;
			bool atPtOne = true;

			try
			{
				View avStart = R.Doc.ActiveView;

				rotation = Math.Atan(avStart.UpDirection.X / avStart.UpDirection.Y);

				// Transform t = Transform.CreateRotation(XYZ.BasisZ, rotation);
				
				startPoint = R.UiDoc.Selection.PickPoint(snaps, "Select Point");
				// startPoint = _uiDoc.Selection.PickPoint("Select Point");

				atPtOne = false;

				if (startPoint == null) return PointMeasurements.InValid();
				View avEnd = R.Doc.ActiveView;

				// cannot change views between points
				if (avStart.Id.IntegerValue != avEnd.Id.IntegerValue)
				{
					return PointMeasurements.InValid();
				}
				// endPoint = _uiDoc.Selection.PickPoint(snaps, "Select Point");
				endPoint = R.UiDoc.Selection.PickPoint("Select Point");

				if (endPoint == null) return PointMeasurements.InValid();

				if (is3D)
				{
					startPoint = find3Dpoint(startPoint);
					endPoint = find3Dpoint(endPoint);
					rotation = 0;
				}

			}
			catch
			{
				if (atPtOne)
				{
					return PointMeasurements.SetVoid();
				}

				R.ActivateRevit();

				ApiCalls.SendKeyCode(0x01);

				return PointMeasurements.SetVoid();

				// return PointMeasurements.InValid();
			}

			// Debug.WriteLine("end getPts");

			return new PointMeasurements(startPoint, endPoint, workingOrigin, rotation, is3D);
		}

		private XYZ find3Dpoint(XYZ origin)
		{
			if (ri == null)
			{
				ri = new ReferenceIntersector(v3d);
				ri.TargetType = FindReferenceTarget.All;
			}

			ReferenceWithContext rc = ri.FindNearest(origin, viewDir);

			if (rc == null) return origin;

			Reference rf = rc.GetReference();

			return rf.GlobalPoint;
		}


	#endregion

	#region event consuming

	#endregion

	#region event publishing

		private event PropertyChangedEventHandler PropertyChanged;

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
