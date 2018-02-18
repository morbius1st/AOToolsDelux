#region Using directives

using System;
using System.Collections.Generic;
using AOTools.AppSettings.SchemaSettings;
using Autodesk.Revit.DB;
using static AOTools.AppSettings.RevitSettings.RevitSettingsUnitApp;
using static AOTools.AppSettings.SchemaSettings.SchemaUnitUtil;

#endregion

// itemname:	RevitUserSettingsMgr
// username:	jeffs
// created:		1/27/2018 10:26:23 AM


namespace AOTools.AppSettings.RevitSettings
{
	public class RevitSettingsUnitApp : SchemaUnitApp
	{
		public static string SchemaName => SCHEMA_NAME;
		public static string SchemaDesc => SCHEMA_DESC;
		public Guid SchemaGuid => Schemaguid;

		public static readonly RevitSettingsUnitApp RsuApp = new RevitSettingsUnitApp();
		public static SchemaDictionaryApp RsuAppSetg { get; private set; }

		public SchemaDictionaryApp DefAppSchema => SchemaUnitAppDefault;

		public bool IsInitalized { get; }

		private RevitSettingsUnitApp()
		{
			Initalize();

			IsInitalized = true;
		}

		private void Initalize()
		{
			RsuAppSetg = GetSchemaUnitAppDefault();
		}
	}

	public class RevitSettingsUnitUsr : SchemaUnitUsr
	{
		public static string UnitSchemaName => SCHEMA_NAME;
		public static string SchemaDesc => SCHEMA_DESC;

		public static readonly RevitSettingsUnitUsr RsuUsr = new RevitSettingsUnitUsr();
		public static List<SchemaDictionaryUsr> RsuUsrSetg { get; private set; }

		private RevitSettingsUnitUsr()
		{
			if (RsuApp.IsInitalized == false)
			{
				throw new Exception("Basic Manager Must be Initalized First");
			}

			Initalize();
		}

		public void Initalize()
		{
			RsuUsrSetg = DefaultSchemaListUsr(1);
		}

		public void Clear()
		{
			RsuUsrSetg = new List<SchemaDictionaryUsr>(1);
		}

		public int Count => RsuUsrSetg.Count;
	}

	// defined here as this file will be revit specific
	public enum RevitUnitType
	{
		UT_UNDEFINED = UnitType.UT_Undefined,
		UT_NUMBER = UnitType.UT_Number
	}

}
