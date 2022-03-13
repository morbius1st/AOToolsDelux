#region using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.UI;
using SettingsManager;

#endregion

// username: jeffs
// created:  2/26/2022 7:39:36 AM

// IO for getting / saving persistent unit settings

namespace DeluxMeasure.UnitsUtil
{
	public class UnitsSettings
	{
	#region private fields

		private UnitsManager uMgr;

	#endregion

	#region ctor

		public UnitsSettings()
		{
			uMgr = UnitsManager.Instance;
		}

	#endregion

	#region public properties

	#endregion

	#region private properties

	#endregion

	#region public methods


		public List<UnitStyle> GetStyles()
		{
			if (!getUserStyles())
			{
				if (!getAppStyles())
				{
					AppSettings.Data.AppStyles = uMgr.StdStyles;;

					AppSettings.Admin.Write();

				}

				UserSettings.Data.UserStyles = AppSettings.Data.AppStyles;

				UserSettings.Admin.Write();
			}

			return UserSettings.Data.UserStyles;
		}

		public void ReadUnitImperialAppSettings()
		{
			if (AppSettings.Path.SettingFileExists)
			{
				AppSettings.Admin.Read();
			}
			else
			{
				WriteUnitImperialAppSettings();
			}

			// TaskDialog td = new TaskDialog("Read app settings");
			// td.MainInstruction = $"app settings read\n"
			// 	+ $"info (int)| {AppSettings.Data.AppSettingsValue}"
			// 	+ $"path| {AppSettings.Path.SettingFilePath}";
			// td.Show();
		}

		public void WriteUnitImperialAppSettings()
		{
			AppSettings.Admin.Write();
		}

	#endregion

	#region private methods

		


		private bool getUserStyles()
		{
			if (UserSettings.Path.SettingFileExists)
			{
				if (UserSettings.Data.UserStyles == null)
				{
					UserSettings.Admin.Read();
				}
			}

			if (UserSettings.Data.UserStyles == null) return false;

			return true;
		}

		private bool getAppStyles()
		{
			if (!AppSettings.Path.SettingFileExists)
			{
				if (AppSettings.Data.AppStyles == null)
				{
					AppSettings.Admin.Read();
				}
			}

			if (AppSettings.Data.AppStyles == null) return false;

			return true;
		}

	#endregion

	#region event consuming

	#endregion

	#region event publishing

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is UnitsSettings";
		}

	#endregion
	}
}