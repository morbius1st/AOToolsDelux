#region + Using Directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

#endregion

// user name: jeffs
// created:   1/28/2024 7:51:35 AM

namespace CsDeluxMeasure.RevitSupport.ExtEvents
{

	public class ExtEvttMake
	{
		private int eeId = (int) ExtEvtId.EI_NONE;

		public ExtEvtId Take()
		{
			return (ExtEvtId) Interlocked.Exchange(ref eeId, (int) ExtEvtId.EI_NONE);
		}

		public void Make(ExtEvtId eid)
		{
			Interlocked.Exchange(ref eeId, (int) eid);
		}

	}
}
