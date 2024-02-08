#region + Using Directives
using Autodesk.Revit.UI;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using SettingsManager;
using Tests01.RevitSupport;

#endregion

// user name: jeffs
// created:   1/28/2024 7:52:01 AM

namespace CsDeluxMeasure.RevitSupport.ExtEvents
{
	public class ExtEvtHandler : IExternalEventHandler
	{
		private ExtEvttMake maker = new ExtEvttMake();

		public ExtEvttMake Maker => maker;

		public string GetName()
		{
			return "Ext Event Handler";
		}

		public void Execute(UIApplication app)
		{
			try
			{
				switch (Maker.Take())
				{
				case ExtEvtId.EI_NONE:
					{
						return;
					}
				case ExtEvtId.EI_MEASURE:
					{
						R.Dx.MeasurePoints();
						break;
					}
				}
			}
			catch (Exception e)
			{
				// Debug.WriteLine(e);
				// throw;
			}
		}

	}
}
