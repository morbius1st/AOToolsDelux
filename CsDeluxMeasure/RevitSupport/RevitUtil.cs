#region + Using Directives

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using CsDeluxMeasure.Annotations;
using CsDeluxMeasure.UnitsUtil;
using UtilityLibrary;

#endregion

// user name: jeffs
// created:   9/18/2022 2:10:29 PM

namespace CsDeluxMeasure.RevitSupport
{
	public class RevitUtil
	{
		internal const ObjectSnapTypes snaps =
			ObjectSnapTypes.Centers | ObjectSnapTypes.Endpoints | ObjectSnapTypes.Intersections |
			ObjectSnapTypes.Midpoints | ObjectSnapTypes.Nearest | ObjectSnapTypes.Perpendicular |
			ObjectSnapTypes.Quadrants | ObjectSnapTypes.Tangents;

		internal enum VTtypeCat
		{
			OTHER,
			D2_WITHPLANE,
			D2_WITHOUTPLANE,
			D3_WITHPLANE
		}

		internal enum VTypeSub
		{
			OTHER,
			D2_HORIZONTAL,
			D2_VERTICAL,
			D2_DRAFTING,
			D2_SHEET,
			D3_VIEW
		}

		internal struct VType
		{
			internal VTypeSub VTSub { get; }
			internal VTtypeCat VTCat { get; }
			internal string VTName { get; }

			internal VType(VTypeSub VTSub, VTtypeCat VTCat, string VTName )
			{
				this.VTSub = VTSub;
				this.VTCat = VTCat;
				this.VTName = VTName;
			}
		}

		internal static bool IsViewTypeOther(View v)
		{
			return GetViewType(v).VTCat == VTtypeCat.OTHER;
		}

		internal static VType GetViewType(Autodesk.Revit.DB.View v)
		{
			VType vtype = new VType(VTypeSub.OTHER, VTtypeCat.OTHER, "Other View Type");

			switch (v.ViewType)
			{
			case ViewType.AreaPlan:
			case ViewType.CeilingPlan:
			case ViewType.EngineeringPlan:
			case ViewType.FloorPlan:
				vtype = new VType(VTypeSub.D2_HORIZONTAL,
					VTtypeCat.D2_WITHPLANE, "Plan 2D View");
				break;
			case ViewType.Elevation:
			case ViewType.Section:
				vtype = new VType(VTypeSub.D2_VERTICAL,
					VTtypeCat.D2_WITHPLANE, "Vertical 2D View");
				break;
			case ViewType.ThreeD:
				vtype = new VType(VTypeSub.D3_VIEW,
					VTtypeCat.D3_WITHPLANE, "3D View");
				break;
			case ViewType.Detail:
			case ViewType.DraftingView:
				vtype = new VType(VTypeSub.D2_DRAFTING,
					VTtypeCat.D2_WITHOUTPLANE, "Drafting View");
				break;
			case ViewType.DrawingSheet:
				vtype = new VType(VTypeSub.D2_SHEET,
					VTtypeCat.D2_WITHOUTPLANE, "Sheet View");
				break;
			}

			return vtype;
		}

		public static bool IsPlaneOrientationAcceptable(UIDocument uiDoc)
		{
			View v = uiDoc.ActiveGraphicalView;
			SketchPlane sp = uiDoc.ActiveGraphicalView.SketchPlane;
			Plane p = sp?.GetPlane();

			if (p == null) { return false; }

			double dp = Math.Abs(v.ViewDirection.DotProduct(p.Normal));

			if (dp < 0.05) { return false; }

			return true;
		}
	}

	public struct PointMeasurements
	{
		internal bool IsValid { get; private set; }

		internal XYZ P1 { get; }
		internal  XYZ P2 { get; }

		private XYZ sqDelta;

		internal XYZ delta { get; }
		internal double dX => delta.X;
		internal double dY => delta.Y;
		internal double dZ => delta.Z;

		internal double Xy { get; }
		internal double Xz { get; }
		internal double Yz { get; }
		internal double Xyz { get; }

		public PointMeasurements(XYZ p1, XYZ p2, XYZ origin)
		{
			P1 = p1 - origin;
			P2 = p2 - origin;

			delta = p2 - p1;
			sqDelta = delta.Multiply(delta);

			Xy = Math.Sqrt(sqDelta.X + sqDelta.Y);
			Xz = Math.Sqrt(sqDelta.X + sqDelta.Z);
			Yz = Math.Sqrt(sqDelta.Y + sqDelta.Z);

			Xyz = Math.Sqrt(sqDelta.X + sqDelta.Y + sqDelta.Z);

			IsValid = true;
		}

		public static PointMeasurements InValid()
		{
			PointMeasurements pm = new PointMeasurements(XYZ.Zero, XYZ.Zero, XYZ.Zero);
			pm.IsValid = false;

			return pm;
		}

		public static PointMeasurements Zero()
		{
			return new PointMeasurements(XYZ.Zero, XYZ.Zero, XYZ.Zero);
		}
	}


	public class PointDistances : INotifyPropertyChanged
	{
		private PointMeasurements points;
		private UnitsDataR udr;

		public PointDistances()
		{
			points = PointMeasurements.InValid();
		}

		public UnitsDataR UnitStyle
		{
			get => udr;
			set => udr = value;
		}

		public PointMeasurements Points
		{
			// get
			// {
			// 	return points;
			// }

			set
			{
				points = value;
			}
		}

		public bool HasPoints => points.IsValid;


		public string P_x  =>format(points.dX);
		public string P_y  =>format(points.dY);
		public string P_z  =>format(points.dZ);
		public string P_xy =>format(points.Xy);
		public string P_xz =>format(points.Xz);
		public string P_yz =>format(points.Yz);
		public string P_xyz=>format(points.Xyz);

		public string P1_x  =>format(points.P1.X);
		public string P1_y  =>format(points.P1.Y);
		public string P1_z  =>format(points.P1.Z);

		public string P2_x  =>format(points.P2.X);
		public string P2_y  =>format(points.P2.Y);
		public string P2_z  =>format(points.P2.Z);



		private string format(double? d)
		{
			if (udr == null) return "undefined";
			string formatted = UnitsSupport.FormatLength(udr, d, true);

			return formatted.IsVoid() ? "null" : formatted;
		}


		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		[DebuggerStepThrough]
		private void OnPropertyChanged([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}
	}

	static class XYZExtensions
	{
		public static XYZ Multiply(this XYZ point, XYZ multiplier)
		{
			return new XYZ(point.X * multiplier.X, point.Y * multiplier.Y, point.Z * multiplier.Z);
		}
	}
}