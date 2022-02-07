#region + Using Directives

#endregion

// user name: jeffs
// created:   7/3/2021 7:21:09 AM
using System.Collections.Generic;
using SharedCode.Windows;

namespace SharedCode.Fields.SchemaInfo.SchemaSupport
{
	public enum SchemaDataStorType
	{
		DT_ROOT,
		DT_CELL,
		DT_LOCK
	}

	public enum SchemaFieldDisplayLevel
	{
		DL_DEBUG			= -1,
		DL_BASIC			= 0,
		DL_MEDIUM			= 1,
		DL_ADVANCED			= 2
	}

	public static class SchemaConstants
	{
		// field definition type
		public static KeyValuePair<string, SchemaDataStorType> SchemaTypeRoot =
			new KeyValuePair<string, SchemaDataStorType>("Root Fields", SchemaDataStorType.DT_ROOT);

		public static KeyValuePair<string, SchemaDataStorType> SchemaTypeLock =
			new KeyValuePair<string, SchemaDataStorType>("Lock Fields", SchemaDataStorType.DT_LOCK);

		public static KeyValuePair<string, SchemaDataStorType> SchemaTypeCell =
			new KeyValuePair<string, SchemaDataStorType>("Cell Fields", SchemaDataStorType.DT_CELL);

		// standard / basic fields - ensure each uses the exact same
		public const int K_SCHEMA_NAME       = 0;
		public const int K_DESCRIPTION       = 1;
		public const int K_VERSION           = 2;
		public const int K_CREATE_DATE       = 3;

		// defined in AFieldsTemp
		public static ColData[] SchemaFieldsHeader;

		// defined in ADataTempBase
		public static ColData[] SchemaDataHeader;
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

	// schema data columns
	public enum DataColumns
	{
		KEY, NAME, VALUE_TYPE, VALUE_STR, VALUE, FIELDS_TEMP
	}
	// schema field columns
	public enum FieldColumns
	{
		TYPE, KEY, SEQUENCE, NAME, DESC, UNIT_TYPE, GUID, VALUE_TYPE, VALUE_STR, VALUE, DISP_LEVEL, DISP_ORDER, DISP_WIDTH
	}

	// schema field keys
	public enum SchemaRootKey
	{
		RK_SCHEMA_NAME       = SchemaConstants.K_SCHEMA_NAME,
		RK_DESCRIPTION       = SchemaConstants.K_DESCRIPTION,
		RK_VERSION           = SchemaConstants.K_VERSION,
		RK_CREATE_DATE       = SchemaConstants.K_CREATE_DATE,
		RK_DEVELOPER         ,
		RK_GUID
	}

	public enum SchemaCellKey
	{
		CK_SCHEMA_NAME       = SchemaConstants.K_SCHEMA_NAME,
		CK_DESCRIPTION       = SchemaConstants.K_DESCRIPTION,
		CK_VERSION           = SchemaConstants.K_VERSION,
		CK_CREATE_DATE       = SchemaConstants.K_CREATE_DATE,
		CK_SEQUENCE          ,
		CK_UPDATE_RULE       ,
		CK_CELL_FAMILY_NAME  ,
		CK_SKIP              ,
		CK_XL_FILE_PATH      ,
		CK_XL_WORKSHEET_NAME
	}

	public enum SchemaLockKey
	{
		LK_SCHEMA_NAME       = SchemaConstants.K_SCHEMA_NAME,
		LK_DESCRIPTION       = SchemaConstants.K_DESCRIPTION,
		LK_VERSION           = SchemaConstants.K_VERSION,
		LK_CREATE_DATE       = SchemaConstants.K_CREATE_DATE,
		LK_USER_NAME         ,
		LK_MACHINE_NAME      ,
		LK_GUID
	}

}