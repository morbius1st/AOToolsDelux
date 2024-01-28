#region + Using Directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using RevitLibrary;
using UtilityLibrary;

#endregion

// user name: jeffs
// created:   1/27/2024 9:26:45 AM

namespace Tests01.RevitSupport
{
	public class RevitUtils
	{

		public static bool isView3D(View v)
		{
			RvtLibrary.VType vt = RvtLibrary.GetViewType(v);

			bool planeOk = RvtLibrary.IsPlaneOrientationOk(v);
			bool is3d = vt.VTSub == RvtLibrary.VTypeSub.D3_VIEW;

			M.WriteLine(null, $"view is 3D   | {is3d}");
			M.WriteLine(null, $"view plane ok| {planeOk}");

			if (!planeOk || !is3d) return false;

			return true;
		}


	}
}
