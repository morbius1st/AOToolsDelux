#region using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.UI;
using SettingsManager;
using Autodesk.Revit.DB;

#endregion

// username: jeffs
// created:  2/26/2022 7:39:36 AM

// IO for getting / saving persistent unit settings

namespace DeluxMeasure.UnitsUtil
{
	public class UnitsSettings
	{
	#region private fields

		// private UnitsManager uMgr;
		private UnitStdStylesR uStdR;

	#endregion

	#region ctor

		public UnitsSettings()
		{
			// uMgr = UnitsManager.Instance;
			uStdR = new UnitStdStylesR();
		}

	#endregion

	#region public properties

	#endregion

	#region private properties

	#endregion

	#region public methods

		// Configuration: 
		// two tracks,
		// -> user styles (uMgr / StyleList) and
		// -> default styles (uMgr / StdStyles)
		//
		// look for user styles -> fail ->
		//   look for app styles -> 
		//      succeed
		//        user styles <= app styles
		//        write user styles
		//      fail -> 
		//        app styles <= default styles
		//        write app styles
		//        user styles <= app styles
		//        write user styles
		// use the user settings for all operations
		//
		// second / subsequent times
		// look for user styles -> succeed ->
		//   look for app styles ->
		//      succeed cont
		//      fail -> 
		//         app styles <= default styles
		//         write app styles
		// use the user settings for all operations

		public void ReadStyles()
		{
		#if PATH
			MethodBase mb = new StackTrace().GetFrame(1).GetMethod();
			Debug.WriteLine($"@UnitsSettings: ReadStyles: {(mb.ReflectedType?.FullName ?? "is null")} > {mb.Name}");
		#endif

			if (!getUserSettings())
			{
				// user settings does not exist

				if (getAppSettings())
				{
					// app settings found
					// set user settings to the default
					// app styles
					setUserStyles();
				}
				else
				{
					setAppStyles();
					setUserStyles();
				}
			}
			else
			{
				// user settings exist
				// user settings read/set (assume that styles exist too)

				// double check - just to be for sure, for sure
				if (!getAppSettings())
				{
					setAppStyles();
				}
			}
		}

		private void setAppStyles()
		{
			AppSettings.Data.AppStyles = new Dictionary<string, UnitsDataR>(uStdR.StdStyles);

			// int i = 1;
			//
			// foreach (KeyValuePair<ForgeTypeId, UnitsDataR> kvp in AppSettings.Data.AppStyles)
			// {
			// 	kvp.Value.Sequence = i++;
			// }

			// AppSettings.Data.UpdateAppStyles(uStdR.StdStyles);
			// int c = AppSettings.Data.AppStyles.Count;
			AppSettings.Admin.Write();
		}

		public static void SetUserStyles()
		{
		#if PATH
			MethodBase mb = new StackTrace().GetFrame(1).GetMethod();
			Debug.WriteLine($"@UnitsSettings: SetUserStyles: {(mb.ReflectedType?.FullName ?? "is null")} > {mb.Name}");
		#endif

			UserSettings.Data.UserStyles = 
				new List<UnitsDataR>(AppSettings.Data.AppStyles.Values);
		}

		private void setUserStyles()
		{
			UserSettings.Data.UserStyles = 
				new List<UnitsDataR>(AppSettings.Data.AppStyles.Values);

			// int i = 1;
			//
			// foreach (UnitsDataR kvp in UserSettings.Data.UserStyles)
			// {
			// 	kvp.Sequence = i++;
			// }

			UserSettings.Admin.Write();
		}

		private bool getUserSettings()
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

		private bool getAppSettings()
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