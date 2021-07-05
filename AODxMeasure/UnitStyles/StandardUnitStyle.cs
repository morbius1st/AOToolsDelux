// Solution:     AOToolsDelux
// Project:       AODxMeasure
// File:             StandardUnitStyle.cs
// Created:      -- ()

using Autodesk.Revit.DB;

namespace DluxMeasure.UnitStyles
{
	public enum UnitStyleType
	{
		PROJECT,
		FEET_FRAC_IN,
		FEET_DEC_IN,
		FRACT_FT,
		DEC_FT,
		FRAC_IN,
		DEC_IN,
		Count
	}

	internal static class StandardUnitStyle
	{
		internal static Units GetStdUnitStyle(Document _doc, UnitStyleType utStyle)
		{
			FormatOptions fmtOpts = null;

			if (utStyle == UnitStyleType.PROJECT)
			{
				return _doc.GetUnits();
			}

			switch (utStyle)
			{
			case UnitStyleType.FEET_FRAC_IN:
				{
					fmtOpts = new FormatOptions(DisplayUnitType.DUT_FEET_FRACTIONAL_INCHES,
						UnitSymbolType.UST_NONE, (1.0 / 64.0) / 12.0);
					fmtOpts.SuppressSpaces = true;
					fmtOpts.SuppressLeadingZeros = true;

					break;
				}
			case UnitStyleType.FRAC_IN:
				{
					fmtOpts = new FormatOptions(DisplayUnitType.DUT_FRACTIONAL_INCHES,
						UnitSymbolType.UST_NONE, 1.0/ 64.0);
					break;
				}
			case UnitStyleType.DEC_FT:
				{
					fmtOpts = new FormatOptions(DisplayUnitType.DUT_DECIMAL_FEET,
						UnitSymbolType.UST_FOOT_SINGLE_QUOTE, 0.001);
					fmtOpts.SuppressTrailingZeros = true;
					break;
				}
			case UnitStyleType.DEC_IN:
				{
					fmtOpts = new FormatOptions(DisplayUnitType.DUT_DECIMAL_INCHES,
						UnitSymbolType.UST_INCH_DOUBLE_QUOTE, 0.0001);
					fmtOpts.SuppressTrailingZeros = true;
					break;
				}
			case UnitStyleType.FRACT_FT:
				{
					fmtOpts = new FormatOptions(DisplayUnitType.DUT_FRACTIONAL_INCHES,
						UnitSymbolType.UST_NONE, 1.0 / 64.0);
					break;
				}
			}

			if (fmtOpts == null) return null;

			fmtOpts.UseDigitGrouping = true;

			Units units = new Units(UnitSystem.Imperial);

			units.SetFormatOptions(UnitType.UT_Length, fmtOpts);

			return units;
		}
	}
}