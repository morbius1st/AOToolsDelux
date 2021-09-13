#region + Using Directives

using Autodesk.Revit.DB;

#endregion

// user name: jeffs
// created:   7/3/2021 7:21:09 AM

namespace CSToolsDelux.Fields.SchemaInfo.SchemaDefinitions
{
	public enum SchemaRootAppKey
	{
		RAK_NAME              = 10,
		RAK_DESCRIPTION       = 20,
		RAK_VERSION           = 30,
		RAK_DEVELOPER         = 40,
		RAK_CREATE_DATE       = 60,
		RAK_APP_GUID          = 70,
	}

	public enum SchemaCellKey
	{
		CK_NAME              = 10,
		CK_VERSION           = 20,
		CK_DESCRIPTION       = 30,
		CK_SEQUENCE          = 40,
		CK_UPDATE_RULE       = 50,
		CK_CELL_FAMILY_NAME  = 60,
		CK_SKIP              = 70,
		CK_XL_FILE_PATH      = 80,
		CK_XL_WORKSHEET_NAME = 90,
	}

	public enum SchemaBoolOpts
	{
		NO				= 0,
		YES				= 1,
		IGNORE			= -1
	}

	public enum UpdateRules
	{
		UR_NEVER        = 0,
		UR_AS_NEEDED    = 1,
		UR_UPON_REQUEST = 2,
		UR_COUNT        = 3
	}



	// defined here as this file will be revit specific
	public enum RevitUnitType
	{
		UT_CUSTOM = UnitType.UT_Custom,
		UT_UNDEFINED = UnitType.UT_Undefined,
		UT_NUMBER = UnitType.UT_Number
	}

}
