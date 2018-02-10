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
	public static class SettingsApp
	{
		public static SettingsMgr<SettingsAppBase> SmApp { get; private set; }

		public static SettingsAppBase SmAppSetg { get; private set; }

		public static void SmAppInit()
		{
			SmApp = new SettingsMgr<SettingsAppBase>();
			SmAppSetg = SmApp.Settings;
			SmAppSetg.Header = new Header(SettingsAppBase.APPSETTINGFILEVERSION);
		}

		public static bool IsAppSetgValid()
		{
			return SmApp != null;
		}
	}

	[DataContract(Namespace = "")]
	public class SettingsAppBase : SettingsPathFileAppBase
	{
		public const string APPSETTINGFILEVERSION = "1.0";
		
		[DataMember]
		public int[] AppIs { get; set; } = new[] {10, 20, 30 };

		[DataMember]
		public SchemaDictionaryApp SettingsAppData = RsuApp.DefAppSchema;
	}
}
