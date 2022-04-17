#region using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Documents;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using DeluxMeasure.Windows.Support;
using static Autodesk.Revit.DB.FormatOptions;

#endregion

// username: jeffs
// created:  2/20/2022 9:23:19 AM

namespace DeluxMeasure.UnitsUtil
{

	//
	// public partial class UnitStyles
	// {
	//
	//
	// #region private fields
	//
	// #endregion
	//
	// #region ctor
	//
	// 	public UnitStyles()
	// 	{
	// 		init();
	// 	}
	//
	// #endregion
	//
	// #region public properties
	//
	// 	// public static UnitStyles Instance => instance.Value;
	//
	// 	public List<UnitStyle> StdStyles { get; private set; }
	//
	// 	// public UnitsData UnitsData { get; }
	//
	// 	// public List<UnitStyle> StyleList { get; set; }
	//
	// #if DEBUGUNITPREC
	// 	public List<UnitStyle> TestStyles { get; private set; }
	// #endif
	//
	// #endregion
	//
	// #region private properties
	//
	// #endregion
	//
	// #region public methods
	// 	
	// #endregion
	//
	// #region private methods
	//
	// 	private void init()
	// 	{
	// 		makeStdStyles();
	// 	}
	//
	// 	private void makeStdStyles()
	// 	{
	// 		StdStyles = new List<UnitStyle>();
	//
	// 		ForgeTypeId uType;
	//
	// 		// IMPERIAL STD STYLES
	// 		// feet+frac inches @ 1/64
	// 		// frac in @ 1/64
	// 		// dec ft @ 0.001
	// 		// dec in @ 0.0001
	//
	// 		StdStyles.Add(UnitStyle.GetProjectUnitStyle(null));
	//
	// 		// std style: feet+frac Inches
	// 		uType = UnitTypeId.FeetFractionalInches;
	// 		StdStyles.Add(
	// 			new UnitStyle(
	// 				uType,
	// 				"Std " + UnitsData.UnitTypes[uType].UnitDesc.Name, // name
	// 				UnitsData.UnitTypes[uType].UnitDesc.Desc,          // desc
	// 				"1/64\"", UnitsData.UnitCat.DECIMAL,              // precision
	// 				null,                                              // lead zero
	// 				true,                                              // trail zero
	// 				true,                                              // spaces
	// 				false,                                             // plus
	// 				true, true, -1, -1, -1, TODO)                      // grouping
	// 			);
	//
	// 		// std style: frac Inches
	// 		uType = UnitTypeId.FractionalInches;
	// 		StdStyles.Add(
	// 			new UnitStyle(uType,
	// 				"Std " + UnitsData.UnitTypes[uType].UnitDesc.Name, // name
	// 				UnitsData.UnitTypes[uType].UnitDesc.Desc,          // desc
	// 				(1.0 / 64.0), UnitsData.UnitCat.DECIMAL,          // precision
	// 				SymbolTypeId.In,                                   // lead zero
	// 				true,                                              // trail zero
	// 				true,                                              // spaces
	// 				false,                                             // plus
	// 				true, true, -1, -1, -1, TODO)                      // grouping
	// 			);
	//
	// 		// std style: dec feet
	// 		uType = UnitTypeId.Feet;
	// 		StdStyles.Add(
	// 			new UnitStyle(uType,
	// 				"Std " + UnitsData.UnitTypes[uType].UnitDesc.Name, // name
	// 				UnitsData.UnitTypes[uType].UnitDesc.Desc,          // desc
	// 				0.0001, UnitsData.UnitCat.DECIMAL,                // precision
	// 				SymbolTypeId.Ft,                                   // lead zero
	// 				true,                                              // trail zero
	// 				true,                                              // spaces
	// 				false,                                             // plus
	// 				true, true, -1, -1, -1, TODO)                      // grouping
	// 			);
	//
	// 		// std style: dec inches
	// 		uType = UnitTypeId.Inches;
	// 		StdStyles.Add(
	// 			new UnitStyle(uType,
	// 				"Std " + UnitsData.UnitTypes[uType].UnitDesc.Name, // name
	// 				UnitsData.UnitTypes[uType].UnitDesc.Desc,          // desc
	// 				0.0001, UnitsData.UnitCat.DECIMAL,                // precision
	// 				SymbolTypeId.In,                                   // lead zero
	// 				true,                                              // trail zero
	// 				true,                                              // spaces
	// 				false,                                             // plus
	// 				true, true, -1, -1, -1, TODO)                      // grouping
	// 			);
	//
	//
	// 		// METRIC STD STYLES
	//
	// 		// std style: meters+centimeters
	// 		uType = UnitTypeId.MetersCentimeters;
	// 		StdStyles.Add(
	// 			new UnitStyle(uType,
	// 				"Std " + UnitsData.UnitTypes[uType].UnitDesc.Name, // name
	// 				UnitsData.UnitTypes[uType].UnitDesc.Desc,          // desc
	// 				0.0001, UnitsData.UnitCat.DECIMAL,                // precision
	// 				null,                                              // lead zero
	// 				true,                                              // trail zero
	// 				false,                                             // spaces
	// 				false,                                             // plus
	// 				true, false, -1, -1, -1, TODO)                     // grouping
	// 			);
	//
	// 		// std style: Meters
	// 		uType = UnitTypeId.Meters;
	// 		StdStyles.Add(
	// 			new UnitStyle(uType,
	// 				"Std " + UnitsData.UnitTypes[uType].UnitDesc.Name, // name
	// 				UnitsData.UnitTypes[uType].UnitDesc.Desc,          // desc
	// 				0.01, UnitsData.UnitCat.DECIMAL,                  // precision
	// 				SymbolTypeId.Meter,                                // lead zero
	// 				true,                                              // trail zero
	// 				false,                                             // spaces
	// 				false,                                             // plus
	// 				true, false, -1, -1, -1, TODO)                     // grouping
	// 			);
	//
	// 		// std style: centimeters
	// 		uType = UnitTypeId.Centimeters;
	// 		StdStyles.Add(
	// 			new UnitStyle(uType,
	// 				"Std " + UnitsData.UnitTypes[uType].UnitDesc.Name, // name
	// 				UnitsData.UnitTypes[uType].UnitDesc.Desc,          // desc
	// 				0.01, UnitsData.UnitCat.DECIMAL,                  // precision
	// 				SymbolTypeId.Cm,                                   // lead zero
	// 				true,                                              // trail zero
	// 				false,                                             // spaces
	// 				false,                                             // plus
	// 				true, false, -1, -1, -1, TODO)                     // grouping
	// 			);
	//
	// 		// std style: millimeters
	// 		uType = UnitTypeId.Millimeters;
	// 		StdStyles.Add(
	// 			new UnitStyle(uType,
	// 				"Std " + UnitsData.UnitTypes[uType].UnitDesc.Name, // name
	// 				UnitsData.UnitTypes[uType].UnitDesc.Desc,          // desc
	// 				0.01, UnitsData.UnitCat.DECIMAL,                  // precision
	// 				SymbolTypeId.Mm,                                   // lead zero
	// 				true,                                              // trail zero
	// 				false,                                             // spaces
	// 				false,                                             // plus
	// 				true, false, -1, -1, -1, TODO)                     // grouping
	// 			);
	// 		
	// 	}
	//
	// #endregion
	//
	// #region event consuming
	//
	// #endregion
	//
	// #region event publishing
	//
	// #endregion
	//
	// #region system overrides
	//
	// 	public override string ToString()
	// 	{
	// 		return "this is UnitStyles";
	// 	}
	//
	// #endregion
	//
	//
	// #if DEBUGUNITPREC
	//
	// 	private void makeTestStyles()
	// 	{
	// 		int count = 0;
	//
	// 		ForgeTypeId uType;
	//
	// 		TestStyles = new List<UnitStyle>();
	//
	// 		// IMPERIAL STD STYLES
	//
	// 											
	// 		uType = UnitTypeId.FeetFractionalInches;
	// 		
	// 		TestStyles.Add(
	// 			new UnitStyle(uType,
	// 				$"Std ({count++})  {UnitsData.UnitTypes[uType].Title} to 1'", 
	// 				"1\'", // precision (good)
	// 				null,  // symbol
	// 				true,  // lead zero (n/a)
	// 				true,  // trail zero
	// 				true,  // spaces (n/a)
	// 				false, // plus (n/a)
	// 				true)  // grouping
	// 			);
	//
	//
	// 		// 0
	// 		// std style: feet+frac Inches
	// 		uType = UnitTypeId.FeetFractionalInches;
	// 		TestStyles.Add(
	// 			new UnitStyle(uType,
	// 				"Std " + UnitsData.UnitTypes[uType].Title + " to 1/64\"", 
	// 				"1/64\"",			// precision (good)
	// 				null,				// symbol
	// 				true,				// lead zero (n/a)
	// 				true,				// trail zero
	// 				true,				// spaces (n/a)
	// 				false,				// plus (n/a)
	// 				true)				// grouping
	// 			);
	// 		
	// 		/*
	// 		uType = UnitTypeId.FeetFractionalInches;
	// 		TestStyles.Add(
	// 			new UnitStyle(uType,
	// 				"Std " + UnitsData.UnitTypes[uType].Title + " to 1/8\"", 
	// 				"1/8\"",			// precision (good)
	// 				null,				// symbol
	// 				true,				// lead zero (n/a)
	// 				true,				// trail zero
	// 				true,				// spaces (n/a)
	// 				false,				// plus (n/a)
	// 				true)				// grouping
	// 			);
	// 		
	// 					
	// 		uType = UnitTypeId.FeetFractionalInches;
	// 		TestStyles.Add(
	// 			new UnitStyle(uType,
	// 				"Std " + UnitsData.UnitTypes[uType].Title + " to 1/2\"", 
	// 				"1/2\"",			// precision (good)
	// 				null,				// symbol
	// 				true,				// lead zero (n/a)
	// 				true,				// trail zero
	// 				true,				// spaces (n/a)
	// 				false,				// plus (n/a)
	// 				true)				// grouping
	// 			);
	// 		
	// 								
	// 		uType = UnitTypeId.FeetFractionalInches;
	// 		TestStyles.Add(
	// 			new UnitStyle(uType,
	// 				"Std " + UnitsData.UnitTypes[uType].Title + " to 1\"", 
	// 				"1\"",			// precision (good)
	// 				null,				// symbol
	// 				true,				// lead zero (n/a)
	// 				true,				// trail zero
	// 				true,				// spaces (n/a)
	// 				false,				// plus (n/a)
	// 				true)				// grouping
	// 			);
	// 		
	// 					
	// 								
	// 		uType = UnitTypeId.FeetFractionalInches;
	// 		TestStyles.Add(
	// 			new UnitStyle(uType,
	// 				"Std " + UnitsData.UnitTypes[uType].Title + " to 6\"", 
	// 				"6\"",			// precision (good)
	// 				null,				// symbol
	// 				true,				// lead zero (n/a)
	// 				true,				// trail zero
	// 				true,				// spaces (n/a)
	// 				false,				// plus (n/a)
	// 				true)				// grouping
	// 			);
	// 		
	// 		
	// 		// 1
	// 		// std style: feet+frac Inches
	// 		uType = UnitTypeId.FeetFractionalInches;
	// 		TestStyles.Add(
	// 			new UnitStyle(uType,
	// 				"Std " + UnitsData.UnitTypes[uType].Title + " to 1\'", 
	// 				"1\'",				// precision (failed)
	// 				null,               // symbol
	// 				true,               // lead zero (n/a)
	// 				true,               // trail zero
	// 				true,               // spaces (n/a)
	// 				false, 				// plus (n/a)
	// 				true)               // grouping
	// 			);
	// 		*/
	//
	// 		// 2
	// 		// std style: frac Inches
	// 		uType = UnitTypeId.FractionalInches;
	// 		TestStyles.Add(
	// 			new UnitStyle(uType,
	// 				$"Std ({count++})  {UnitsData.UnitTypes[uType].Title} to 1/64\"",  
	// 				"1/64\"",            // precision  (good)
	// 				SymbolTypeId.In,     // symbol
	// 				true,                // lead zero (n/a)
	// 				true,                // trail zero
	// 				true,                // spaces (n/a)
	// 				false, 				 // plus (n/a)
	// 				true)                // grouping
	// 			);
	//
	// 		// 3
	// 		// std style: frac Inches
	// 		uType = UnitTypeId.FractionalInches;
	// 		TestStyles.Add(
	// 			new UnitStyle(uType,
	// 				$"Std ({count++})  {UnitsData.UnitTypes[uType].Title} to 1\"", 
	// 				"1\"",            // precision  (failed)
	// 				SymbolTypeId.In,  // symbol
	// 				true,             // lead zero (n/a)
	// 				true,             // trail zero
	// 				true,             // spaces (n/a)
	// 				false, 			  // plus (n/a)
	// 				true)             // grouping
	// 			);
	//
	//
	// 		// 4
	// 		// std style: dec feet
	// 		uType = UnitTypeId.Feet;
	// 		TestStyles.Add(
	// 			new UnitStyle(uType,
	// 				$"Std ({count++})  {UnitsData.UnitTypes[uType].Title} to 0.0001'", 
	// 				0.0001,           // precision  (good)
	// 				SymbolTypeId.Ft,  // symbol
	// 				true,             // lead zero (n/a)
	// 				true,             // trail zero
	// 				true,             // spaces (n/a)
	// 				false, 			  // plus (n/a)
	// 				true)             // grouping
	// 			);
	//
	// 		// 5
	// 		// std style: dec inches
	// 		uType = UnitTypeId.Inches;
	// 		TestStyles.Add(
	// 			new UnitStyle(uType,
	// 				$"Std ({count++})  {UnitsData.UnitTypes[uType].Title} to 0.0001\"", 
	// 				0.0001,           // precision  (good)
	// 				SymbolTypeId.In,  // symbol
	// 				true,             // lead zero (n/a)
	// 				true,             // trail zero
	// 				true,             // spaces (n/a)
	// 				false, 			  // plus (n/a)
	// 				true)             // grouping
	// 			);
	//
	// 		// METRIC STD STYLES
	// 		// 6
	// 		// std style: Meters
	// 		uType = UnitTypeId.Meters;
	// 		TestStyles.Add(
	// 			new UnitStyle(uType,
	// 				$"Std ({count++})  {UnitsData.UnitTypes[uType].Title} to 0.001", 
	// 				0.001,              // precision  (good)
	// 				SymbolTypeId.Meter, // symbol
	// 				false,              // lead zero (n/a)
	// 				true,               // trail zero
	// 				false,              // spaces (n/a)
	// 				false, 				// plus (n/a)
	// 				true)               // grouping
	// 			);
	// 		// 7
	// 		// std style: centimeters
	// 		uType = UnitTypeId.Centimeters;
	// 		TestStyles.Add(
	// 			new UnitStyle(uType,
	// 				$"Std ({count++})  {UnitsData.UnitTypes[uType].Title} to 0.01", 
	// 				0.01,            // precision  (good)
	// 				SymbolTypeId.Cm, // symbol
	// 				false,           // lead zero (n/a)
	// 				true,            // trail zero
	// 				false,           // spaces (n/a)
	// 				false, 			 // plus (n/a)
	// 				true)            // grouping
	// 			);
	// 		// 8
	// 		// std style: millimeters
	// 		uType = UnitTypeId.Millimeters;
	// 		TestStyles.Add(
	// 			new UnitStyle(uType,
	// 				$"Std ({count++})  {UnitsData.UnitTypes[uType].Title} to 0.01", 
	// 				0.01,             // precision  (good)
	// 				SymbolTypeId.Mm,  // symbol
	// 				false,            // lead zero (n/a)
	// 				true,             // trail zero
	// 				false,            // spaces (n/a)
	// 				false, 			  // plus (n/a)
	// 				true)             // grouping
	// 			);
	//
	// 		// 9
	// 		// std style: millimeters
	// 		uType = UnitTypeId.Millimeters;
	// 		TestStyles.Add(
	// 			new UnitStyle(uType,
	// 				$"Std ({count++})  {UnitsData.UnitTypes[uType].Title} to 0.025", 
	// 				0.025,           // precision (2.5 mm) (fail)
	// 				SymbolTypeId.Mm,  // symbol
	// 				false,            // lead zero (n/a)
	// 				true,             // trail zero
	// 				false,            // spaces (n/a)
	// 				false, 			  // plus (n/a)
	// 				true)             // grouping
	// 			);
	//
	//
	//
	// 		// 10
	// 		// std style: meters+centimeters
	// 		uType = UnitTypeId.MetersCentimeters;
	// 		TestStyles.Add(
	// 			new UnitStyle(uType,
	// 				$"Std ({count++})  {UnitsData.UnitTypes[uType].Title} to 0.1 mm", 
	// 				0.0001,				// precision (0.1 mm) (worked)
	// 				null,				// symbol
	// 				false,				// lead zero (n/a)
	// 				true,				// trail zero
	// 				false,				// spaces (n/a)
	// 				false, 				// plus (n/a)
	// 				true)				// grouping
	// 			);
	//
	// 		// 11
	// 		// std style: meters+centimeters
	// 		uType = UnitTypeId.MetersCentimeters;
	// 		TestStyles.Add(
	// 			new UnitStyle(uType,
	// 				$"Std ({count++})  {UnitsData.UnitTypes[uType].Title} to 1 cm", 
	// 				0.01,			// precision
	// 				null,			// symbol
	// 				false,			// lead zero (n/a)
	// 				true,			// trail zero
	// 				false,			// spaces (n/a)
	// 				false, 			// plus (n/a)
	// 				true)			// grouping
	// 			);
	//
	// 		UnitStyleCmd0.idxMax = count;
	//
	// 	}
	//
	// #endif
	// }

}