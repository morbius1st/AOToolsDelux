#region + Using Directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion

// user name: jeffs
// created:   9/1/2021 6:38:01 AM

namespace AOToolsDelux.Cells.Tests
{
	public class StaticTest01
	{
		public string guid { get; set; } = null;

		public StaticTest01(string guid)
		{
			if (this.guid != null) return;

			this.guid = guid;
		}

	}
}
