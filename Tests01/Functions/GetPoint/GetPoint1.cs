#region + Using Directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Events;
using RevitLibrary;
using Tests01.ExtEvent;
using Tests01.Functions.ViewTests;
using Tests01.RevitSupport;
using UtilityLibrary;

#endregion

// user name: jeffs
// created:   1/27/2024 1:39:29 PM

namespace Tests01.Functions.GetPoint
{
	public class GetPoint1
	{
		private ViewData vd;
		private XYZ viewDir;

		public bool Execute()
		{
			vd=new ViewData();

			bool result = true;
			SketchPlane origSp;
			XYZ startPt;
			XYZ foundPt;

			View v = R.Uidoc.ActiveGraphicalView;

			origSp = v.SketchPlane;

			result = RevitUtils.isView3D(v);

			View3D v3d = v as View3D;

			viewDir = v.ViewDirection;

			if (result)
			{
				ViewData.V3D = v3d;
				
				if (vd.MakeAndSetSketchPlane(v3d, true))
				{
					getPoints(v3d);
				}
				else
				{
					result = false;
				}
			}

			return result;
		}


		private void getPoints(View3D v3d)
		{
			bool repeat = true;

			do
			{
				try
				{
					R.ActivateRevit();
					XYZ tstPt = R.Uidoc.Selection.PickPoint(RvtLibrary.snaps, "select a point");

					XYZ foundPt = FindRefPoint(v3d, tstPt);

					if (foundPt != null)
					{
						M.WriteLine(null, $"found point| {RvtLibrary.XyzToString(foundPt)}");
					}
					else
					{
						M.WriteLine(null,"point is null");

						viewDir = viewDir.Negate();
					}
				}
				catch 
				{
					M.WriteLine(null, "done with getting points");
					repeat = false;
				}
			}
			while (repeat);
		}

		// private bool makeSketchPlane(View v)
		// {
		// 	return vd.MakeAndSetSketchPlane(v, true);
		// }

		public override string ToString()
		{
			return $"this is {nameof(GetPoint1)}";
		}

		private bool HasViewChanged(View v)
		{
			if (viewDir != null &&
				viewDir.IsAlmostEqualTo(v.ViewDirection)) return false;

			viewDir = v.ViewDirection;

			return true;
		}


		private ReferenceIntersector ri;

		public XYZ FindRefPoint(View3D v3d, XYZ origin)
		{
			XYZ pt = XYZ.Zero;

			if (ri == null)
			{
				ri = new ReferenceIntersector(v3d);
				ri.TargetType = FindReferenceTarget.All;
			}

			ReferenceWithContext ric = ri.FindNearest(origin, viewDir);

			if (ric == null) return null;

			Reference rf = ric.GetReference();

			pt = rf.GlobalPoint;

			return pt;
		}
	}
}