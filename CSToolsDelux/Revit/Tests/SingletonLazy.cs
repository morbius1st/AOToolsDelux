#region + Using Directives
using System;

#endregion

// user name: jeffs
// created:   8/28/2021 4:41:03 PM

namespace CSToolsDelux.Revit.Tests
{

	public class SingletonLazy
	{
		private static readonly Lazy<SingletonLazy> instance =
			new Lazy<SingletonLazy>(() => new SingletonLazy());

		public static SingletonLazy Instance => instance.Value;

		public static SubClass02 sc02Early { get; set; }  = new SubClass02();
		public static SubClass02 sc02Late { get; set; }

		private SingletonLazy()
		{
			sc02Late = new SubClass02();
		}

		public string DocName2 { get; set; }

		public int I1 { get; set; }

	}
}
