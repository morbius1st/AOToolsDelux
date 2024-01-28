#region + Using Directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using RevitLibrary;
using Tests01.RevitSupport;

#endregion

// user name: jeffs
// created:   1/27/2024 6:17:55 PM

namespace Tests01.Functions.WorkPlane
{
	public class WorkPlaneInfo
	{
		public bool Execute()
		{
			View v = R.Uidoc.ActiveGraphicalView;

			RvtLibrary.VType vt = RvtLibrary.GetViewType(v);

			return true;
		}


		public override string ToString()
		{
			return $"this is {nameof(WorkPlaneInfo)}";
		}
	}
}
