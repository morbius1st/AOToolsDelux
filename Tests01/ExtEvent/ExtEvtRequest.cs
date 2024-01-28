#region + Using Directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

#endregion

// user name: jeffs
// created:   1/27/2024 3:05:38 PM

namespace Tests01.ExtEvent
{
	public enum EeIId
	{
		EID_NONE,
		EID_SKETCH_PLANE,
	}


	public class ExtEvtRequest
	{
		private int eEid = (int) EeIId.EID_NONE;

		public EeIId Take()
		{
			return (EeIId) Interlocked.Exchange(ref eEid, (int) EeIId.EID_NONE);
		}

		public void Make(EeIId eid)
		{
			Interlocked.Exchange(ref eEid, (int) eid);
		}

	}
}
