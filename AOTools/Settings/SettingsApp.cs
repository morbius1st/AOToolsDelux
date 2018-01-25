#region Using directives

using System.Runtime.Serialization;
using UtilityLibrary;

using static AOTools.Settings.BasicSchema;

#endregion

// itemname:	SettingsFileApp
// username:	jeffs
// created:		1/3/2018 8:05:02 PM


namespace AOTools.Settings
{
	public static class SettingsApp
	{
		public static readonly SettingsBase<AppSettings> ASettings;

		public static readonly AppSettings ASet;

		static SettingsApp()
		{
			ASettings = new SettingsBase<AppSettings>();
			ASet = ASettings.Settings;
			ASet.Header = new Header(AppSettings.APPSETTINGFILEVERSION);
		}
	}

	[DataContract]
	public class AppSettings : SettingsPathFileAppBase
	{
		
		public const string APPSETTINGFILEVERSION = "1.0";

		//		public int AppI { get; set; } = 0;
		//		public bool AppB { get; set; } = false;
		//		public double AppD { get; set; } = 0.0;
		//		public string AppS { get; set; } = "this is a App";
		[DataMember]
		public int[] AppIs { get; set; } = new[] {10, 20, 30 };

		[DataMember]
		public SchemaDictionaryBasic AppUnitStyleSchema = 
			_schemaFields.Clone();


	}


}
