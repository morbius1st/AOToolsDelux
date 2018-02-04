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
	public static class SettingsMgrUsr
	{
		public static readonly SettingsMgr<SettingsUsr> SmUsrSetg;

		private static SettingsUsr smUsr;

		public static SettingsUsr SmUsr {
			get
			{
				if (smUsr == null) { Initalize(); }
				return smUsr;
			}
		}

		static SettingsMgrUsr()
		{
			SmUsrSetg = new SettingsMgr<SettingsUsr>();
			Initalize();
//			SmUsr = SmUsrSetg.Settings;
//			SmUsr.Header = new Header(SettingsUsr.USERSETTINGFILEVERSION);
		}

		private static void Initalize()
		{
			try
			{
				smUsr = SmUsrSetg.Settings;
				smUsr.Header = new Header(SettingsUsr.USERSETTINGFILEVERSION);

			}
			catch
			{
				smUsr = null;
				throw;
			}
		}
	}


	// sample Settings User
	[DataContract]
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
