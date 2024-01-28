#region + Using Directives
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.UI;

using Tests01.Functions;
using Tests01.Functions.ViewTests;

#endregion

// user name: jeffs
// created:   1/27/2024 3:12:57 PM

namespace Tests01.ExtEvent
{
	public class ExtEvtHandler : IExternalEventHandler
	{
		public string GetName()
		{
			return "event handler";
		}

		public ExtEvtRequest eeRequest = new ExtEvtRequest();

		public ExtEvtRequest EeRequest => eeRequest;


		public void Execute(UIApplication app)
		{

			try
			{
				switch (EeRequest.Take())
				{
				case EeIId.EID_SKETCH_PLANE:
					{
						getPoint();
						break;
					}
				}
			}
			catch
			{
				
			}


		}


		private void getPoint()
		{
			FunctionHandler fh = new FunctionHandler();

			fh.Execute(FunctionId.FID_GET_PT1);
		}


	}
}
