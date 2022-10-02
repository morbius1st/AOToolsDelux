// Solution:     AOToolsDelux
// Project:       CsDeluxMeasure
// File:             UnitData.cs
// Created:      2022-04-30 (1:20 PM)

using System;

namespace CsDeluxMeasure.UnitsUtil
{
	public static class UnitData
	{
		public const int INLIST_UNDEFINED = -1;
		public const int INLIST_DISABLED = -2;

		public static readonly int INLIST_COUNT = Enum.GetValues(typeof(InList)).Length;
		public static string[] IN_LIST_NAMES = new [] { "Ribbon Button", "Dialog Left", "Dialog Right" };

		public static readonly int UCAT_COUNT = Enum.GetValues(typeof(UnitCat)).Length;
		public static readonly int UTYPE_COUNT = Enum.GetValues(typeof(UnitType)).Length;
	}

	public enum UnitSys
	{
		US_METRIC = 0,
		US_IMPERIAL = 1
	}
	public enum UnitClass
	{
		CL_CONTROL,   // special & locked
		CL_PROJECT,   // special & locked
		CL_ORDINARY,  // not special & not locked
		CL_FT_DEC_IN, // special & not-locked
	}

	public enum UnitCat
	{
		UC_METER_CM,
		UC_FT_IN_FRAC,
		UC_FT_IN_DEC,
		UC_DECIMAL,
		UC_IN_DEC,
		UC_IN_FRAC,
		UC_FT_FRAC,
		UC_NONE
	}

	public enum UnitType
	{
		UT_INVALID = 0, 
		UT_CONTROL,
		UT_PROJECT,
		UT_FT_IN_DEC,
		UT_FT_IN_FRAC,
		UT_US_SURVEY,
		UT_FT_DEC,
		UT_IN_DEC,
		UT_IN_FRAC,
		UT_METER,
		UT_DECIMETER,
		UT_CENTIMETER,
		UT_MILLIMETER,
		UT_METER_CENTIMETER
	}

	public enum InList
	{
		RIBBON = 0,
		DIALOG_LEFT = 1,
		DIALOG_RIGHT = 2,
	}

	

	public enum PrecXref
	{
		XR_FRAC_IN ,
		XR_FRAC_FT,
		XR_DEC,
		XR_M_CM
	}
}