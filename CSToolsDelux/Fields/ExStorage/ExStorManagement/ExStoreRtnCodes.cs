// Solution:     AOToolsDelux
// Project:       CSToolsDelux
// File:             ExStoreRtnCodes.cs
// Created:      2021-09-12 (7:17 PM)

namespace CSToolsDelux.Fields.ExStorage.ExStorManagement
{
	public enum ExStoreRtnCodes
	{
		XRC_ENTITY_NOT_FOUND    = -40,
		XRC_SCHEMA_NOT_FOUND    = -35,
		XRC_NOT_CONFIG          = -30,
		XRC_IS_CONFIG           = -25,
		XRC_DS_SINGLE_NOT_FOUND = -20,
		XRC_DS_NOT_EXIST		= -15,
		XRC_DS_EXISTS           = -10,
		XRC_APP_NOT_EXIST       = -9,
		XRC_ROOT_NOT_EXIST      = -8,
		XRC_EX_STORE_NOT_EXISTS	= -7,
		XRC_EX_STORE_EXISTS	    = -6,
		XRC_NOT_FOUND           = -5,
		XRC_TOO_MANY_OPEN_DOCS  = -4,
		XRC_NOT_INIT            = -3,
		XRC_DUPLICATE           = -2,
		XRC_FAIL                = -1,
		XRC_GOOD                = 0,
		XRC_PROCEED_GET_DATA    = 10,
		XRC_SEARCH_FOR_PRIOR    = 20,
		XRC_SEARCH_FOUND_PRIOR  = 21,
		XRC_SEARCH_FOUND_PRIOR_AND_NEW  = 23,
	}
}