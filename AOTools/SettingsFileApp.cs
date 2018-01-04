#region Using directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using UtilityLibrary;

#endregion

// itemname:	SettingsFileApp
// username:	jeffs
// created:		1/3/2018 8:05:02 PM


namespace AOTools
{
	// sample settings app
	[XmlRoot("AppSettings")]
	public class SettingsFileApp : SettingsFileBase
	{
//		public int AppI { get; set; } = 0;
//		public bool AppB { get; set; } = false;
//		public double AppD { get; set; } = 0.0;
//		public string AppS { get; set; } = "this is a App";
//		public int[] AppIs { get; set; } = new[] { 20, 30 };
	
		// read only properties are not serialized
		[XmlIgnore]
		public override SettingsPathBase SettingsPath { get; } = new SettingsPathAppDefault();
		public override string SETTINGFILEVERSION { get; } = "0.0.0.1";
	
	}
}
