#region + Using Directives

using Autodesk.Revit.DB;

#endregion

// user name: jeffs
// created:   7/3/2021 7:21:09 AM

namespace AOToolsDelux.Cells.SchemaDefinition
{
	public enum SchemaRootKey
	{
		RK_UNDEFINED		  = -10,
		RK_NAME              = 10,
		RK_DESCRIPTION       = 20,
		RK_VERSION           = 30,
		RK_DEVELOPER         = 40,
		RK_APP_GUID          = 50,
	}

	// primary scheme element 
	// defines the basic information
	public enum SchemaAppKey
	{
		AK_UNDEFINED		  = -10,
		AK_NAME              = 10,
		AK_DESCRIPTION       = 20,
		AK_VERSION           = 30,
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
		CK_XL_WORKSHEET_NAME = 90

	}

	public enum SchemaBoolOpts
	{
		NO				= 0,
		YES				= 1,
		IGNORE			= -1
	}

	public enum UpdateRules
	{
		UR_NEVER,
		UR_AS_NEEDED,
		UR_UPON_REQUEST,
	}



	// defined here as this file will be revit specific
	public enum RevitUnitType
	{
		UT_CUSTOM = UnitType.UT_Custom,
		UT_UNDEFINED = UnitType.UT_Undefined,
		UT_NUMBER = UnitType.UT_Number
	}

}
