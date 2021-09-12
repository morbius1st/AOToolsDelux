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

	public class SubClassLazy
	{
		private static readonly Lazy<SubClassLazy> instance =
			new Lazy<SubClassLazy>(() => new SubClassLazy());

		public static SubClassLazy Instance => instance.Value;

		public static SubClass02 sc02Early { get; set; }  = new SubClass02();
		public static SubClass02 sc02Late { get; set; }

		private SubClassLazy()
		{
			sc02Late = new SubClass02();
		}

		public string DocName2 { get; set; }

	}
}
