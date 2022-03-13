using System.Collections.Generic;
using System.Runtime.Serialization;
using DeluxMeasure.UnitsUtil;

// User settings (per user) 
//  - user's settings for a specific app
//	- located in the user's app data folder / app name


// ReSharper disable once CheckNamespace


namespace SettingsManager
{
	public struct WindowLocation
	{
		public double Top { get; set; }
		public double Left { get; set; }

		public WindowLocation(double top, double left)
		{
			Top = top;
			Left = left;
		}
	}

	#region user data class

	// this is the actual data set saved to the user's configuration file
	// this is unique for each program
	[DataContract(Namespace = "")]
	public class UserSettingDataFile : IDataFile
	{
		[IgnoreDataMember]
		private List<UnitStyle> userStyles;

		[IgnoreDataMember]
		public string DataFileVersion => "user 0.1";

		[IgnoreDataMember]
		public string DataFileDescription => "user setting file";

		[IgnoreDataMember]
		public string DataFileNotes => "user settings for DeluxMeasure";

		[DataMember(Order = 1)]
		public int UserSettingsValue { get; set; } = 7;

		[DataMember(Order = 2)]
		public List<UnitStyle> UserStyles {
			get
			{
				return userStyles;
			}
			set
			{
				userStyles = value;
			}
		}

		[DataMember]
		public WindowLocation WinPosUnitStyleMgr { get; set; }


	}

	#endregion
}


// , APP_SETTINGS, SUITE_SETTINGS, MACH_SETTINGS, SITE_SETTINGS