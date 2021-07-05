#region Using directives

using System.Collections.Generic;
using AOTools.AppSettings.SchemaSettings;
using static AOTools.AppSettings.RevitSettings.RevitSettingsBase.RevitSetgDelRetnCode;
using static AOTools.AppSettings.RevitSettings.RevitSettingsBase;
using static UtilityLibrary.MessageUtilities;
using static AOTools.AppSettings.RevitSettings.RevitSettingsUnitUsr;

#endregion

// itemname:	RevitSettingsMgr
// username:	jeffs
// created:		1/7/2018 3:37:43 PM


namespace AOTools.AppSettings.RevitSettings
{
	internal class RevitSettingsMgr : RevitSettingsBase
	{
		public static readonly RevitSettingsMgr RsMgr = new RevitSettingsMgr();

		public static bool RvtSetgInitalized { get; private set; } = false;

	#region Read Settings

		// ******************************
		// read setting
		// ******************************

		public SaveRtnCodes Read()
		{
			if (!RvtSetgInitalized)
			{
				return SaveRtnCodes.NOT_INIT;
			}
		#if DEBUG
			logMsgDbLn2("revit settings", "read");
		#endif

			return ReadAllRevitSettings() ? SaveRtnCodes.GOOD : SaveRtnCodes.FAIL;
		}

	#endregion

	#region Save Settings

		// ******************************
		// save settings
		// ******************************

		// save the settings - if the active schema is
		// is not valid, delete the current schema and
		// then save

		private SaveRtnCodes Save(bool byPass)
		{
		#if DEBUG
			logMsgDbLn2("revit settings", "save");
		#endif
			if (!byPass && !RvtSetgInitalized)
			{
				return SaveRtnCodes.NOT_INIT;
			}

			return SaveAllRevitSettings();
		}

		public bool Save()
		{
			return Save(false) == SaveRtnCodes.GOOD;
		}

	#endregion

	#region Delete settings

		// ******************************
		// delete schema from revit document
		// ******************************

		// delete thecurrent schema from the current model only
		public bool DeleteSchema()
		{
		#if DEBUG
			logMsgDbLn2("revit settings", "delete schema");
		#endif
			return ChkDelRetnCode(DeleteAllSchemas(), "Delete Settings");
		}

	#endregion

	#region Update settings

		// ******************************
		// update settings
		// ******************************

		// update the schema with the current schema
		public bool Update()
		{
		#if DEBUG
			logMsgDbLn2("revit settings", "update");
		#endif
			if (!RvtSetgInitalized) return false;

			if (!ChkDelRetnCode(DeleteAllSchemas(),
				"Updating Settings")) { return false; }

			return Save();
		}

	#endregion

	#region Reset Settings

		// ******************************
		// reset settings
		// ******************************

		// reset the settings to their default values
		public bool Reset()
		{
		#if DEBUG
			logMsgDbLn2("revit settings", "reset");
		#endif
			DeleteAllSchemas();

			RsuUsr.Initialize();

			RvtSetgInitalized = true;

			return Save();
		}

	#endregion

	#region Init Settings

		// ******************************
		// reset settings
		// ******************************

		// reset the settings to their default values
		public void Init()
		{
		#if DEBUG
			logMsgDbLn2("revit settings", "init");
		#endif
			// DeleteAllSchemas();

			RsuUsr.Initialize();

			RvtSetgInitalized = true;
		}

	#endregion
	}
}