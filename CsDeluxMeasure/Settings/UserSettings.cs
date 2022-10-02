using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.Serialization;
using CsDeluxMeasure.UnitsUtil;
using Autodesk.Revit.DB;

// User settings (per user) 
//  - user's settings for a specific app
//	- located in the user's app data folder / app name


// ReSharper disable once CheckNamespace


namespace SettingsManager
{
	public struct WindowLocation
	{
		[DataMember]
		public int top;

		[DataMember]
		public int left;

		[IgnoreDataMember]
		public double Top
		{
			get => top;
			set => top = (int) value;
		}

		[IgnoreDataMember]
		public double Left
		{
			get => left;
			set => left = (int) value;
		}

		public WindowLocation(double top, double left)
		{
			this.top = (int) top;
			this.left = (int) left;
		}
	}

	#region user data class

	// this is the actual data set saved to the user's configuration file
	// this is unique for each program
	[DataContract(Namespace = "")]
	public class UserSettingDataFile : IDataFile
	{
		[IgnoreDataMember]
		private List<UnitsDataR> userStyles;

		[IgnoreDataMember]
		public string DataFileVersion => "user 0.1";

		[IgnoreDataMember]
		public string DataFileDescription => "user setting file";

		[IgnoreDataMember]
		public string DataFileNotes => "user settings for CsDeluxMeasure";

		[DataMember(Order = 1)]
		public int UserSettingsValue { get; set; } = 7;

		[DataMember(Order = 2)]
		public string DialogLeftSelItemName { get; set; }

		[DataMember(Order = 3)]
		public string DialogRightSelItemName { get; set; }

		[DataMember(Order = 3)]
		public List<UnitsDataR> UserStyles {
			get
			{
			#if PATH
				MethodBase mb = new StackTrace().GetFrame(1).GetMethod();

				Debug.WriteLine($"@UserStyles: Get: {(mb.ReflectedType?.FullName ?? "is null")} > {mb.Name}");
			#endif

				return userStyles;
			}
			set
			{
			#if PATH
				MethodBase mb = new StackTrace().GetFrame(1).GetMethod();

				Debug.WriteLine($"@UserStyles: Set: {(mb.ReflectedType?.FullName ?? "is null")} > {mb.Name}");
				#endif

				userStyles = value;
			}
		}
		
		[DataMember]
		public WindowLocation WinPosUnitStyleMgr { get; set; }

		[DataMember]
		public WindowLocation WinPosStyleOrder { get; set; }
		
		[DataMember]
		public WindowLocation WinPosDeluxMeasure { get; set; }


	}

	#endregion
}


// , APP_SETTINGS, SUITE_SETTINGS, MACH_SETTINGS, SITE_SETTINGS