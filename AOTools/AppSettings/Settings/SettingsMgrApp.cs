#region Using directives

using System.Runtime.Serialization;
using static AOTools.AppSettings.RevitSettings.RevitSettingsUnitApp;
#endregion

using AOTools.AppSettings.Schema;
using UtilityLibrary;

// itemname:	SettingsFileApp
// username:	jeffs
// created:		1/3/2018 8:05:02 PM

namespace AOTools.AppSettings.Settings
{
	// no app settings yet.
	public static class SettingsMgrApp
	{
				public static readonly SettingsMgr<SettingsApp> SmAppSetg;
		
				public static readonly SettingsApp SmApp;
		
				static SettingsMgrApp()
				{
					SmAppSetg = new SettingsMgr<SettingsApp>();
					SmApp = SmAppSetg.Settings;
					SmApp.Header = new Header(SettingsApp.APPSETTINGFILEVERSION);
				}
			}
		
			[DataContract]
			public class SettingsApp : SettingsPathFileAppBase
			{
				public const string APPSETTINGFILEVERSION = "1.0";
		
				[DataMember]
				public int[] AppIs { get; set; } = new[] {10, 20, 30 };

				[DataMember] public SchemaDictionaryApp SettingsAppData = RsuApp.DefAppSchema;
//					SchemaUnitApp.SchemaUnitAppDefault.Clone();
	}
}
