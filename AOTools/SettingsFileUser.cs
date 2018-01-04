#region Using directives

using System.Xml.Serialization;
using UtilityLibrary;


using Point = System.Drawing.Point;

#endregion

// itemname:	SettingsUser
// username:	jeffs
// created:		1/3/2018 8:04:40 PM


namespace AOTools
{
	// sample Settings User
	[XmlRoot("UserSettings")]
	public class SettingsFileUser : SettingsFileBase
	{
		public Point FormMeasurePointsLocation = new Point(0, 0);

		public bool MeasurePointsShowWorkplane = false;


		// read only property does not get serialized
		[XmlIgnore]
		public override SettingsPathBase SettingsPath { get; } = new SettingsPathUserDefault();
		public override string SETTINGFILEVERSION { get; } = "0.0.0.1";
	}

}
