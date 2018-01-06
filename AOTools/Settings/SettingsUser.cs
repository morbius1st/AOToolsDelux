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
	public class UserSettings : SettingsPathFileUserBase
	{
		public Point FormMeasurePointsLocation = new Point(0, 0);

		public bool MeasurePointsShowWorkplane = false;

	}

}
