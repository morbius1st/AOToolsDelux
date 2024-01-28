#region + Using Directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests01.Functions.GetPoint;
using Tests01.Functions.ViewTests;
using Tests01.Functions.WorkPlane;

#endregion

// user name: jeffs
// created:   1/27/2024 6:58:13 AM

namespace Tests01.Functions
{
	public enum FunctionId
	{
		FID_VIEW_INFO,
		FID_VIEW_DATA,
		FID_GET_PT1,
		FID_WORKPLANE_INFO
	}

	public class FunctionHandler
	{

		public bool Execute(FunctionId fid)
		{
			bool result = false;
			ViewInfo vi = new ViewInfo();
			ViewData vd = new ViewData();
			GetPoint1 gp1 = new GetPoint1();
			WorkPlaneInfo wpi = new WorkPlaneInfo();

			switch (fid)
			{
			case FunctionId.FID_VIEW_INFO:
				{
					result = vi.Execute();
					break;
				}
			case FunctionId.FID_VIEW_DATA:
				{
					result = vd.Execute();
					break;
				}
			case FunctionId.FID_GET_PT1:
				{
					result = gp1.Execute();
					break;
				}
			case  FunctionId.FID_WORKPLANE_INFO:
				{
					result = wpi.Execute();
					break;
				}
			}

			return result;
		}


		public override string ToString()
		{
			return $"this is {nameof(FunctionHandler)}";
		}
	}
}
