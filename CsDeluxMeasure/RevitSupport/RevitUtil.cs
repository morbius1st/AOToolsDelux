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
using CsDeluxMeasure.Windows;
using UtilityLibrary;

#endregion

// user name: jeffs
// created:   9/18/2022 2:10:29 PM

namespace CsDeluxMeasure.RevitSupport
{
	public class RevitUtil
	{
		internal const ObjectSnapTypes snaps = ObjectSnapTypes.Centers | ObjectSnapTypes.Endpoints | 
			ObjectSnapTypes.Intersections | ObjectSnapTypes.Midpoints |  ObjectSnapTypes.Perpendicular |
			ObjectSnapTypes.Quadrants; // | ObjectSnapTypes.Tangents;

		internal enum VTtypeCat
		{
			OTHER,
			D2_WITHPLANE,
			D2_WITHOUTPLANE,
			D2_NOPLANE,
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
				vtype = new VType(VTypeSub.D2_DRAFTING,
					VTtypeCat.D2_WITHOUTPLANE, "Detail View");
				break;
			case ViewType.Legend:
			case ViewType.DraftingView:
				vtype = new VType(VTypeSub.D2_DRAFTING,
					VTtypeCat.D2_NOPLANE, "Drafting View");
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
		public bool Is3D { get; set; }

		public string version;

		internal bool IsValid { get; private set; }
		internal bool IsVoid { get; private set; }

		internal XYZ P1 { get; }
		internal  XYZ P2 { get; }


		internal XYZ P1R { get; }
		internal  XYZ P2R { get; }

		private XYZ sqDelta;

		internal XYZ delta { get; }
		internal double dX => delta.X;
		internal double dY => delta.Y;
		internal double dZ => delta.Z;

		internal double Xy { get; }
		internal double Xz { get; }
		internal double Yz { get; }
		internal double Xyz { get; }

		internal double AngleXy { get; }
		internal double AngleXz { get; }
		internal double AngleYz { get; }
		internal double AngleXyz { get; }


		internal double Rotation { get; }

		private double area;

		internal double Area
		{
			get => area;
			private set => area = value;
		}

		public PointMeasurements(XYZ p1, XYZ p2, XYZ origin, double rotation, bool is3D = false)
		{
			version = "3.0";

			Is3D = is3D;

			P1 = p1 - origin;
			P2 = p2 - origin;

			Rotation = rotation;

			if (!double.IsNaN(rotation))
			{
				Transform t = Transform.CreateRotation(XYZ.BasisZ, rotation);

				P1R = t.OfPoint(P1);
				P2R = t.OfPoint(P2);
			}
			else
			{
				P1R = p1;
				P2R = p2;
			}

			delta = P2R - P1R;
			sqDelta = delta.Multiply(delta);

			Xy = Math.Sqrt(sqDelta.X + sqDelta.Y);
			Xz = Math.Sqrt(sqDelta.X + sqDelta.Z);
			Yz = Math.Sqrt(sqDelta.Y + sqDelta.Z);

			Xyz = Math.Sqrt(sqDelta.X + sqDelta.Y + sqDelta.Z);

			AngleXy = delta.Y != 0 ? Math.Atan(delta.X / delta.Y) : 0.0;
			AngleXz = delta.X != 0 ? Math.Atan(delta.Z / delta.X) : 0.0;
			AngleYz = delta.Y != 0 ? Math.Atan(delta.Z / delta.Y) : 0.0;
			AngleXyz = Xy != 0 ? Math.Atan(delta.Z / Xy) : 0.0;

			IsValid = true;
			IsVoid = false;

			// if (M.W != null) M.W.WriteLine1("calculating area");

			area = -1;
			area = setArea();

			// if (M.W != null) M.W.WriteLine1($"area calculated| {area} ");
		}

		public static PointMeasurements InValid()
		{
			PointMeasurements pm = new PointMeasurements(XYZ.Zero, XYZ.Zero, XYZ.Zero, 0);
			pm.IsValid = false;

			return pm;
		}

		public static  PointMeasurements SetVoid()
		{
			PointMeasurements pm = new PointMeasurements(XYZ.Zero, XYZ.Zero, XYZ.Zero, 0);
			pm.IsValid = true;
			pm.IsVoid = true;

			return pm;
		}

		public static PointMeasurements Zero()
		{
			return new PointMeasurements(XYZ.Zero, XYZ.Zero, XYZ.Zero, 0);
		}

		private double setArea()
		{
			if (dZ == 0)
			{
				return Math.Abs(dX * dY);
			}

			if (dY == 0)
			{
				return Math.Abs(dX * dZ);
			}

			if (dX == 0)
			{
				return Math.Abs(dY * dZ);
			}

			return -1.0;
		}
	}


	public class PointDistances // : INotifyPropertyChanged
	{
		public string version = "2.0";

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
			get
			{
				return points;
			}

			set
			{
				// M.W.WriteLine1($"points set| area| {points.Area}");
				
				points = value; 
				// OnPropertyChanged();
			}

		
	}

		public bool HasPoints => points.IsValid;


		public string P_x  => format(points.dX);
		public string P_y  => format(points.dY);
		public string P_z  => format(points.dZ);
		public string P_xy => format(points.Xy);
		public string P_xz => format(points.Xz);
		public string P_yz => format(points.Yz);
		public string P_xyz => format(points.Xyz);

		public string P1_x  => format(points.P1.X);
		public string P1_y  => format(points.P1.Y);
		public string P1_z  => format(points.P1.Z);

		public string P2_x  => format(points.P2.X);
		public string P2_y  => format(points.P2.Y);
		public string P2_z  => format(points.P2.Z);

		public string Area => formatArea(points.Area);

		public string Rotation => formatRotation(points.Rotation);

		public string A_XY => formatAngle(points.AngleXy);
		public string A_XZ => formatAngle(points.AngleXz);
		public string A_YZ => formatAngle(points.AngleYz);
		public string A_XYZ => formatAngle(points.AngleXyz);

		private string format(double? d)
		{
			if (udr == null) return "undefined";
			string formatted = UnitsSupport.FormatLength(udr, d, true);

			return formatted.IsVoid() ? "null" : formatted;
		}

		private string formatArea(double? d)
		{

			if (udr == null || d.Equals(-1.0))
			{
				// M.W.WriteLine1($"udr null? | d is {d}");

				return "undefined";
			}

			string formatted = UnitsSupport.FormatArea(udr, d);

			// M.W.WriteLine1($"formatting area| {formatted ?? "is null"}");

			return formatted.IsVoid() ? "null" : formatted;

		}

		private string formatRotation(double? d)
		{
			if (d==null || d.Value == 0) return "No View Rotation";

			double decRotation = d.Value * (180 / Math.PI);

			return $"{decRotation:##.00##°}";
		}

		private string formatAngle(double? d)
		{
			double decRotation = d.Value * (180 / Math.PI);

			return $"{decRotation:##.00##°}";
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