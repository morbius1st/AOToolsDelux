#region Using directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UtilityLibrary;

#endregion

// itemname:	SettingObjects
// username:	jeffs
// created:		1/3/2018 8:18:48 PM


namespace AOTools
{
	public static class UserSettings
	{
		public static Settings<SettingsFileUser> _us =
			UserSettings.GetInstance();

		public static SettingsFileUser _uSet = _us.Setting;

		private static Settings<SettingsFileUser> settingUser;

		public static Settings<SettingsFileUser> GetInstance()
		{
			if (settingUser == null)
			{
				settingUser = new Settings<SettingsFileUser>();
			}

			return settingUser;
		}
	}

	public static class AppSettings
	{
		private static Settings<SettingsFileApp> settingApp;

		public static Settings<SettingsFileApp> GetInstance()
		{
			if (settingApp == null)
			{
				settingApp = new Settings<SettingsFileApp>();
			}

			return settingApp;
		}
	}
}
