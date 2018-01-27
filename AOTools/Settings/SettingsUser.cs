#region Using directives

using System.Collections.Generic;
using System.Runtime.Serialization;
using UtilityLibrary;
using Point = System.Drawing.Point;

using static AOTools.Settings.SUnitKey;
using static AOTools.Settings.SBasicKey;
using static AOTools.Settings.UnitSchema;
using static AOTools.Settings.BasicSchema;

#endregion

// itemname:	SettingsUser
// username:	jeffs
// created:		1/3/2018 8:04:40 PM


namespace AOTools.Settings
{
	public static class SettingsUser
	{
		public static readonly SettingsBase<UserSettings> USettings;

		public static readonly UserSettings USet;

		static SettingsUser()
		{
			USettings = new SettingsBase<UserSettings>();
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

		[DataMember]
		public List<SchemaDictionaryUnit> UserUnitStyleSchemas =
			GetUnitSchemaFields(_basicSchemaFieldsDefault[COUNT].Value);
	}

}
