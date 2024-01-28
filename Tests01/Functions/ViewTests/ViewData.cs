#region + Using Directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using RevitLibrary;
using Tests01.RevitSupport;
using UtilityLibrary;


#endregion

// user name: jeffs
// created:   1/27/2024 6:59:08 AM

namespace Tests01.Functions.ViewTests
{

	public class ViewData
	{
		public static View3D V3D { get; set; }
		public static bool Result { get; set; }

		public ViewData()
		{
			Result = false;
		}

		public bool Execute()
		{
			View v = R.Uidoc.ActiveGraphicalView;

			bool result = RevitUtils.isView3D(v);

			if (!result) return false;

			M.WriteLine(null, $"\nregular view data");
			showViewInfo(v);

			XYZ origin = v.Origin;
			XYZ right = v.RightDirection;
			XYZ up = v.UpDirection;
			XYZ dir = v.ViewDirection;

			Plane p1 = Plane.CreateByNormalAndOrigin(dir, origin);
			Plane p2 = Plane.CreateByOriginAndBasis(origin, right, up);


			M.WriteLine(null, $"\nplane data");
			M.WriteLine(null, $"\np1");
			M.WriteLine(null, $"origin    | {RvtLibrary.XyzToString(p1.Origin)}");
			M.WriteLine(null, $"normal    | {RvtLibrary.XyzToString(p1.Normal)}");


			M.WriteLine(null, $"\np2");
			M.WriteLine(null, $"origin    | {RvtLibrary.XyzToString(p2.Origin)}");
			M.WriteLine(null, $"normal    | {RvtLibrary.XyzToString(p2.Normal)}");


			if (!MakeAndSetSketchPlane(v, true)) return false;

			R.ActivateRevit();

			View3D v3d = v as View3D;

			M.WriteLine(null, $"\n3D view data");
			showViewInfo(v3d);

			return true;
		}

		private void showViewInfo(View v)
		{
			XYZ origin = v.Origin;
			XYZ right = v.RightDirection;
			XYZ up = v.UpDirection;
			XYZ dir = v.ViewDirection;

			M.WriteLine(null, $"origin    | {origin.X}, {origin.Y}, {origin.Z}");
			M.WriteLine(null, $"right     | {right.X}, {right.Y}, {right.Z}");
			M.WriteLine(null, $"up        | {up.X}, {up.Y}, {up.Z}");
			M.WriteLine(null, $"direction | {dir.X}, {dir.Y}, {dir.Z}");
		}


		public void MakeAndSetSp()
		{
			Result = MakeAndSetSketchPlane(V3D, true);
		}

		public bool MakeAndSetSketchPlane(View v, bool showWorkPlane)
		{
			bool result = true;

			using (Transaction t = new Transaction(R.Doc, "Make a sketch plane"))
			{
				try
				{
					t.Start();

					{
						SketchPlane p3 = makeSketchPlane(v);

						if (p3 != null)
						{
							v.SketchPlane = p3;

							if (showWorkPlane) v.ShowActiveWorkPlane();

							M.WriteLine(null, $"\np3");
							M.WriteLine(null, $"origin    | {RvtLibrary.XyzToString(p3.GetPlane().Origin)}");
							M.WriteLine(null, $"normal    | {RvtLibrary.XyzToString(p3.GetPlane().Origin)}");
						}
						else
						{
							result = false;
						}
					}

					t.Commit();
				}
				catch
				{
					result = false;
					t.RollBack();
				}
			}

			return result;
		}

		private SketchPlane makeSketchPlane(View v)
		{

			Plane plane = Plane.CreateByNormalAndOrigin(
				R.Doc.ActiveView.ViewDirection,
				v.Origin);

			SketchPlane sp = SketchPlane.Create(R.Doc, plane);

			return sp;
		}

		public override string ToString()
		{
			return $"this is {nameof(ViewInfo)}";
		}
	}
}