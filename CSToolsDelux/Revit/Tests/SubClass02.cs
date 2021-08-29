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

	public class SubClass02
	{

		private int ti02;

		public SubClass02()
		{
			ti02 = 0;
		}

		public int TestVal02
		{
			get
			{
				int a = ti02;
				ti02 = ((ti02 + 1) % 5) ;
				return a;
			}
		}

	}
}
