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
		public static SettingsMgr<UserSettings> USettings;

		public static UserSettings USet;

		static SettingsUser()
		{
			USettings = new SettingsMgr<UserSettings>(ResetData);
			USet = USettings.Settings;
//			USet.Header = new Header(UserSettings.USERSETTINGFILEVERSION);
		}

		public static void ResetData()
		{
			USettings = new SettingsMgr<UserSettings>(ResetData);
			USet = USettings.Settings;
		}
	}


	// sample Settings User
	[DataContract]
	public class UserSettings : SettingsPathFileUserBase
	{
		public sealed override string FileVersion { get; set; } = "2.0";

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
