#region + Using Directives

#endregion

// user name: jeffs
// created:   9/27/2021 6:30:34 AM

namespace CSToolsDelux.Utility
{
	public struct ProcedureInfo
	{
		public string routine { get; set; }
		public string description { get; set; }

		public ProcedureInfo(string routine, string description)
		{
			this.routine = routine;
			this.description = description;
		}
	}
}
