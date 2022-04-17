using System.Collections.Generic;
using System.Runtime.Serialization;
using DeluxMeasure.UnitsUtil;
using Autodesk.Revit.DB;

// App settings (per user)
//	- applies to a specific app in the suite
//	- holds information specific to the app
//	- located in the user's app data folder / app name / AppSettings


// ReSharper disable once CheckNamespace

namespace SettingsManager
{
	#region app data class

	// this is the actual data set saved to the user's configuration file
	// this is unique for each program
	[DataContract(Namespace = "")]
	public class AppSettingDataFile : IDataFile
	{
		[IgnoreDataMember]
		private Dictionary<string, UnitsDataR> appStyles;

		[IgnoreDataMember]
		public string DataFileVersion => "dxm 0.1";

		[IgnoreDataMember]
		public string DataFileDescription => "app setting file for DeluxMeasure";

		[IgnoreDataMember]
		public string DataFileNotes => "Settings include the default unit styles";

		[DataMember(Order = 1)]
		public int AppSettingsValue { get; set; } = 7;

		[DataMember(Order = 2)]
		public Dictionary<string, UnitsDataR> AppStyles {
			get
			{
				// return appStyles;
				return appStyles;
			}
			set
			{
				appStyles = value;
			}
		}


		// public void UpdateAppStyles(Dictionary<ForgeTypeId, UnitsDataR> original)
		// {
		// 	appStyles = new Dictionary<ForgeTypeId, UnitsDataR>(original);
		// }
	}

	#endregion
}