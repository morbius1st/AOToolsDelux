#region + Using Directives

using Autodesk.Revit.DB;

#endregion

// user name: jeffs
// created:   7/3/2021 7:21:09 AM

namespace CSToolsDelux.Fields.SchemaInfo.SchemaDefinitions
{
	public enum SchemaRootKey
	{
		RK_SCHEMA_NAME       ,
		RK_DESCRIPTION       ,
		RK_VERSION           ,
		RK_DEVELOPER         ,
		RK_CREATE_DATE       ,
		RK_GUID          ,
	}

	public enum SchemaCellKey
	{
		CK_SCHEMA_NAME       ,
		CK_DESCRIPTION       ,
		CK_VERSION           ,
		CK_SEQUENCE          ,
		CK_UPDATE_RULE       ,
		CK_CELL_FAMILY_NAME  ,
		CK_SKIP              ,
		CK_XL_FILE_PATH      ,
		CK_XL_WORKSHEET_NAME ,
	}

	public enum SchemaLockKey
	{
		LK_SCHEMA_NAME       ,
		LK_DESCRIPTION       ,
		LK_VERSION           ,
		LK_CREATE_DATE       ,
		LK_USER_NAME         ,
		LK_MACHINE_NAME      ,
		LK_GUID              ,
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
	public enum FieldUnitType
	{
		UT_CUSTOM = UnitType.UT_Custom,
		UT_UNDEFINED = UnitType.UT_Undefined,
		UT_NUMBER = UnitType.UT_Number
	}

}
