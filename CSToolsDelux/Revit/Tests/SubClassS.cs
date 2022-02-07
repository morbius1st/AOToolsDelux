#region using directives

using System;

#endregion


// projname: $projectname$
// itemname: SubClassS
// username: jeffs
// created:  8/28/2021 4:49:49 PM

namespace CSToolsDelux.Revit.Tests
{
	public class SubClassS
	{
		private int tiS;

		private static readonly Lazy<SubClassS> instance =
			new Lazy<SubClassS>(() => new SubClassS());

		private SubClassS() { }

		public static SubClassS Instance => instance.Value;

		public int TestValS
		{
			get => tiS;
			set => tiS = value;
		}

		internal int tiS2 = 0;

		public int TestValS2
		{
			get => tiS2;
			set => tiS2 = value;
		}

		public override string ToString()
		{
			return "this is SubClassS";
		}

	}
}