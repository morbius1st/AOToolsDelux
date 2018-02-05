#region Using directives

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using AOTools.AppSettings.RevitSettings;
using AOTools.AppSettings.SchemaSettings;
using UtilityLibrary;
using Point = System.Drawing.Point;

#endregion

// itemname:	SettingsMgrUsr
// username:	jeffs
// created:		1/3/2018 8:04:40 PM


namespace AOTools.AppSettings.ConfigSettings
{
	[DataContract]
	public static class SettingsMgrUsr
	{
		public static SettingsMgr<SettingsUsr> SmUsrMgr { get; private set; }

		public static SettingsUsr SmUsrSetg { get; private set; }

		public static void SmUsrInit()
		{
			SmUsrMgr = new SettingsMgr<SettingsUsr>();
			SmUsrSetg = SmUsrMgr.Settings;
			SmUsrSetg.Header = new Header(SettingsUsr.USERSETTINGFILEVERSION);
		}

		public static bool IsValid()
		{
			return SmUsrMgr != null;
		}
	}


	// sample Settings User
	[DataContract(Namespace = "")]
	public class SettingsUsr : SettingsPathFileUserBase
	{
		public int Count => UnitStylesList.Count;

		public const string USERSETTINGFILEVERSION = "1.0";

		[DataMember]
		public Point FormMeasurePointsLocation = new Point(0, 0);
		[DataMember]
		public bool MeasurePointsShowWorkplane = false;

		[DataMember]
		public List<SchemaDictionaryUsr> UnitStylesList = RevitSettingsUnitUsr.RsuUsr.RsuUsrSetg;
		
	}

}
