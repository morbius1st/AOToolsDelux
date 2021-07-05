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

	// primary scheme element 
	// defines the basic information
	public enum SchemaAppKey
	{
		UNDEFINED		  = -10,
		NAME              = 10,
		DESCRIPTION       = 20,
		VERSION           = 30,
		DEVELOPER         = 40,
	}

	public enum SchemaCellKey
	{
		NAME              = 10,
		VERSION           = 20,
		DESCRIPTION       = 30,
		SEQUENCE          = 40,
		UPDATE_RULE       = 50,
		CELL_FAMILY_NAME  = 60,
		SKIP              = 70,
		XL_FILE_PATH      = 80,
		XL_WORKSHEET_NAME = 90

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
		UT_UNDEFINED = UnitType.UT_Undefined,
		UT_NUMBER = UnitType.UT_Number
	}

}
