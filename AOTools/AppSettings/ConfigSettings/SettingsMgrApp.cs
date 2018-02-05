#region Using directives

using System.Runtime.Serialization;
using static AOTools.AppSettings.RevitSettings.RevitSettingsUnitApp;
#endregion

using AOTools.AppSettings.SchemaSettings;
using UtilityLibrary;

// itemname:	SettingsFileApp
// username:	jeffs
// created:		1/3/2018 8:05:02 PM

namespace AOTools.AppSettings.ConfigSettings
{
	// no app settings yet.
	public static class SettingsMgrApp
	{
		public static SettingsMgr<SettingsApp> SmAppMgr { get; private set; }

		public static SettingsApp SmAppSetg { get; private set; }

		public static void SmAppInit()
		{
			SmAppMgr = new SettingsMgr<SettingsApp>();
			SmAppSetg = SmAppMgr.Settings;
			SmAppSetg.Header = new Header(SettingsApp.APPSETTINGFILEVERSION);
		}

		public static bool IsAppSetgValid()
		{
			return SmAppMgr != null;
		}
	}

	[DataContract(Namespace = "")]
	public class SettingsApp : SettingsPathFileAppBase
	{
		public const string APPSETTINGFILEVERSION = "1.0";
		
		[DataMember]
		public int[] AppIs { get; set; } = new[] {10, 20, 30 };

		[DataMember] public SchemaDictionaryApp SettingsAppData = RsuApp.DefAppSchema;
//					SchemaUnitApp.SchemaUnitAppDefault.Clone();
	}
}
