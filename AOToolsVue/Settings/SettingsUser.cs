#region Using directives

using System.Collections.Generic;
using System.Runtime.Serialization;
using UtilityLibrary;
using Point = System.Drawing.Point;

//using static AOTools.Settings.SUnitKey;

#endregion

// itemname:	SettingsUser
// username:	jeffs
// created:		1/3/2018 8:04:40 PM


namespace AOToolsVue.Settings
{
	public static class SettingsUser
	{
		public static readonly SettingsMgr<UserSettings> USettings;

		public static readonly UserSettings USet;

		static SettingsUser()
		{
			USettings = new SettingsMgr<UserSettings>();
			USet = USettings.Settings;
			USet.Header = new Header(UserSettings.USERSETTINGFILEVERSION);
		}
	}


	// sample Settings User
	[DataContract]
	public class UserSettings : SettingsPathFileUserBase
	{
		public const string USERSETTINGFILEVERSION = "1.0";

		[DataMember]
		public Point FormMeasurePointsLocation = new Point(0, 0);
		[DataMember]
		public bool MeasurePointsShowWorkplane = false;

		[DataMember(Name = "UnitStyleList")]
		public List<SchemaDictionary2> UserUnitStyleSchemas =
			new List<SchemaDictionary2>()
			{
				UnitSchema.Make("User Unit Style 01", "User unit style desc 01"),
				UnitSchema.Make("User Unit Style 02", "User unit style desc 02"),
				UnitSchema.Make("User Unit Style 03", "User unit style desc 03")
			};
	}

}
