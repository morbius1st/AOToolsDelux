#region using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

#endregion


// projname: $projectname$
// itemname: SubClassS
// username: jeffs
// created:  8/28/2021 4:49:49 PM

namespace CSToolsDelux.Revit.Tests
{
	public class SubClassS
	{
	#region private fields
		
		private int tiS;

		private static readonly Lazy<SubClassS> instance =
			new Lazy<SubClassS>(() => new SubClassS());

	#endregion

	#region ctor

		private SubClassS() { }

	#endregion

	#region public properties

		public static SubClassS Instance => instance.Value;

	#endregion

	#region private properties

	#endregion

	#region public methods

		public int TestValS
		{
			get
			{
				int a = tiS;
				tiS = ((tiS + 1) % 5);
				return a;
			}
		}

	#endregion

	#region private methods

	#endregion

	#region event consuming

	#endregion

	#region event publishing

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is SubClassS";
		}

	#endregion
	}
}