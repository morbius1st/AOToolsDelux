#region + Using Directives

#endregion

// user name: jeffs
// created:   8/28/2021 4:41:03 PM

namespace CSToolsDelux.Revit.Tests
{

	public class SubClass01
	{
		private int ti01;

		public static string StaticDocName { get; set; }

		public static SubClass02 sc02Early { get; set; }  = new SubClass02();
		public static SubClass02 sc02Late { get; set; }
		public static SubClass02 sc02After1 { get; set; }
		public static SubClass02 sc02After2 { get; set; }

		public SubClass01(string docName)
		{
			ti01 = 0;

			sc02Late = new SubClass02();

			sc02Early.DocName = docName;
			sc02Late.DocName = docName;

		}

		public int TestVal01
		{
			get => ti01;
			set => ti01 = value;
		}

		internal int ti12 = 0;

		public int TestVal12
		{
			get => ti12;
			set => ti12 = value;
		}

	}
}
