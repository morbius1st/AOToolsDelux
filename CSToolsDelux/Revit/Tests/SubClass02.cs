#region + Using Directives

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

		public string DocName { get; set; }

		public int TestVal02
		{
			get => ti02;
			set => ti02 = value;
		}

		internal int ti22 = 0;

		public int TestVal22
		{
			get => ti22;
			set => ti22 = value;
		}

	}
}
