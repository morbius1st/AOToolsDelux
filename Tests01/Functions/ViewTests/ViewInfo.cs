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

	public class ViewInfo
	{
		public bool Execute()
		{
			int a = RvtLibrary.SetWindowText(R.Uiapp.MainWindowHandle, "this is a message");

			View v = R.Uidoc.ActiveGraphicalView;

			RvtLibrary.VType vt = RvtLibrary.GetViewType(v);

			M.WriteLine(null, $"message set\n");
			M.WriteLine(null, $"\nview info");
			M.WriteLine(null, $"view name    | {v.Name}");
			M.WriteLine(null, $"view name    | {vt.VTName}");
			M.WriteLine(null, $"view cat     | {vt.VTCat}");
			M.WriteLine(null, $"view sub cat | {vt.VTSub}");
			M.WriteLine(null, $"view 3D      | {vt.VTSub == RvtLibrary.VTypeSub.D3_VIEW}");
			M.WriteLine(null, $"view plane ok| {RvtLibrary.IsPlaneOrientationOk(v)}");


			return true;
		}

		private bool isView3D(View v)
		{
			RvtLibrary.VType vt = RvtLibrary.GetViewType(v);

			bool planeOk = RvtLibrary.IsPlaneOrientationOk(v);
			bool is3d = vt.VTSub == RvtLibrary.VTypeSub.D3_VIEW;

			M.WriteLine(null, $"view is 3D   | {is3d}");
			M.WriteLine(null, $"view plane ok| {planeOk}");

			if (!planeOk || !is3d) return false;

			return true;
		}


		public override string ToString()
		{
			return $"this is {nameof(ViewInfo)}";
		}
	}
}