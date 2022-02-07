using Autodesk.Revit.DB; 


// Solution:     AOToolsDelux
// Project:       CSToolsDelux
// File:             SchemaFieldUnitTypeEnum.cs
// Created:      2022-01-22 (9:12 AM)

// namespace CSToolsDelux.Fields.SchemaInfo.SchemaDefinitions
namespace SharedCode.Fields.SchemaInfo.SchemaDefinitions
{
	public enum FieldUnitType
	{
		UT_CUSTOM = UnitType.UT_Custom,
		UT_UNDEFINED = UnitType.UT_Undefined,
		UT_NUMBER = UnitType.UT_Number
	}
}