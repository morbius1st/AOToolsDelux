#region + Using Directives
using Autodesk.Revit.UI;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion

// user name: jeffs
// created:   1/28/2024 7:52:01 AM

namespace CsDeluxMeasure.RevitSupport.ExtEvents
{
	public class ExtEvtHandler : IExternalEventHandler
	{
		public void Execute(UIApplication app) { }
		public string GetName()
		{
			return null;
		}
	}
}
