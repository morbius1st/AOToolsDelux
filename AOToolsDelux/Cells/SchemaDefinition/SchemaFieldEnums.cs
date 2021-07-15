#region + Using Directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

#endregion

// user name: jeffs
// created:   7/3/2021 7:21:09 AM

namespace AOTools.Cells.SchemaDefinition
{

	public enum SchemaRootKey
	{
		RK_NAME           = 0,
		RK_DESCRIPTION    = 1,
		RK_VERSION        = 2,
		RK_DEVELOPER      = 3,
		RK_APP_GUID       = 4,
	}

	// primary scheme element 
	// defines the basic information
	public enum SchemaAppKey
	{
		AK_NAME              = 10,
		AK_DESCRIPTION       = 20,
		AK_VERSION           = 30,
	}

	public enum SchemaCellKey
	{
		CK_NAME              = 0,
		CK_VERSION           = 1,
		CK_DESCRIPTION       = 2,
		CK_SEQUENCE          = 3,
		CK_UPDATE_RULE       = 4,
		CK_CELL_FAMILY_NAME  = 5,
		CK_SKIP              = 6,
		CK_XL_FILE_PATH      = 7,
		CK_XL_WORKSHEET_NAME = 8,

	}

	public enum SchemaBoolOpts
	{
		NO				= 0,
		YES				= 1,
		IGNORE			= -1
	}

	public enum UpdateRules
	{
		NEVER,
		AS_NEEDED,
		UPON_REQUEST,
	}



	// defined here as this file will be revit specific
	public enum RevitUnitType
	{
		UT_CUSTOM = UnitType.UT_Custom,
		UT_UNDEFINED = UnitType.UT_Undefined,
		UT_NUMBER = UnitType.UT_Number
	}

}
