#region + Using Directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion

// user name: jeffs
// created:   8/28/2021 4:41:03 PM

namespace CSToolsDelux.Revit.Tests
{

	public class SubClass01
	{

		private int ti01;

		public SubClass01()
		{
			ti01 = 0;
		}

		public int TestVal01
		{
			get
			{
				int a = ti01;
				ti01 = ((ti01 + 1) % 5);
				return a;
			}
		}

	}
}
