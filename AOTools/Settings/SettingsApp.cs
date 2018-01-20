#region Using directives

using System.Collections.Generic;
using UtilityLibrary;

using static AOTools.SBasicKey;
using static AOTools.SUnitKey;

#endregion

// itemname:	SettingsFileApp
// username:	jeffs
// created:		1/3/2018 8:05:02 PM


namespace AOTools
{
	// sample settings app
	public class AppSettings : SettingsPathFileAppBase
	{
		//		public int AppI { get; set; } = 0;
		//		public bool AppB { get; set; } = false;
		//		public double AppD { get; set; } = 0.0;
		//		public string AppS { get; set; } = "this is a App";
				public int[] AppIs { get; set; } = new[] {10, 20, 30 };
	}

}
