#region + Using Directives
using System;
using System.Collections.Generic;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

using static DluxMeasure.UnitStyles.UnitStyleType;

#endregion


// projname: AOTools.Units
// itemname: UnitStyleDefault
// username: jeffs
// created:  12/1/2018 8:04:05 AM


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

	[Transaction(TransactionMode.Manual)]
	public class UnitStyleFeetInchCmd : IExternalCommand
	{
		private UIDocument        _uiDoc;
		private Document          _doc;

		public Result Execute(
			ExternalCommandData commandData,
			ref string message,
			ElementSet elements)
		{
			UIApplication uiApp = commandData.Application;
			_uiDoc = uiApp.ActiveUIDocument;
			_doc   = _uiDoc.Document;

			using (Transaction tg = new Transaction(_doc, "Set Project Units to Feet-Inches"))
			{
				tg.Start();
				bool result = UnitStylesDefault.SetProjUnitsToStd(_doc, UnitStyleType.FEET_FRAC_IN);
				if (result)
				{
					tg.Commit();
				}
				else
				{
					tg.RollBack();
				}
			}

			return Result.Succeeded;
		}

	}
	
	[Transaction(TransactionMode.Manual)]
	public class UnitStyleFracInchCmd : IExternalCommand
	{
		private UIDocument        _uiDoc;
		private Document          _doc;

		public Result Execute(
			ExternalCommandData commandData,
			ref string message,
			ElementSet elements)
		{
			UIApplication uiApp = commandData.Application;
			_uiDoc = uiApp.ActiveUIDocument;
			_doc   = _uiDoc.Document;

			using (Transaction tg = new Transaction(_doc, "Set Project Units to Fractional Inches"))
			{
				tg.Start();
				bool result = UnitStylesDefault.SetProjUnitsToStd(_doc, UnitStyleType.FRAC_IN);
				if (result)
				{
					tg.Commit();
				}
				else
				{
					tg.RollBack();
				}
			}

			return Result.Succeeded;
		}

	}

	[Transaction(TransactionMode.Manual)]
	public class UnitStyleDecFeetCmd : IExternalCommand
	{
		private UIDocument _uiDoc;
		private Document _doc;

		public Result Execute(
			ExternalCommandData commandData,
			ref string message,
			ElementSet elements)
		{
			UIApplication uiApp = commandData.Application;
			_uiDoc = uiApp.ActiveUIDocument;
			_doc = _uiDoc.Document;

			using (Transaction tg = new Transaction(_doc, "Set Project Units to Decimal Feet"))
			{
				tg.Start();
				bool result = UnitStylesDefault.SetProjUnitsToStd(_doc, UnitStyleType.DEC_FT);
				if (result)
				{
					tg.Commit();
				}
				else
				{
					tg.RollBack();
				}
			}

			return Result.Succeeded;
		}
	}

	[Transaction(TransactionMode.Manual)]
	public class UnitStyleDecInchCmd : IExternalCommand
	{
		private UIDocument _uiDoc;
		private Document _doc;

		public Result Execute(
			ExternalCommandData commandData,
			ref string message,
			ElementSet elements)
		{
			UIApplication uiApp = commandData.Application;
			_uiDoc = uiApp.ActiveUIDocument;
			_doc = _uiDoc.Document;

			using (Transaction tg = new Transaction(_doc, "Set Project Units to Decimal Inches"))
			{
				tg.Start();
				bool result = UnitStylesDefault.SetProjUnitsToStd(_doc, UnitStyleType.DEC_IN);
				if (result)
				{
					tg.Commit();
				}
				else
				{
					tg.RollBack();
				}
			}

			return Result.Succeeded;
		}
	}


	public static class UnitStylesDefault
	{
		public static SortedDictionary<int, string> UnitStyleDescriptionList { get; set; } =
			new SortedDictionary<int, string>()
			{
				{(int) PROJECT, "Per Project Units" },
				{(int) FEET_FRAC_IN, "Feet and Fractional Inches" },
				{(int) FEET_DEC_IN, "Feet and Decimal Inches" },
				{(int) FRACT_FT, "Fractional Feet" },
				{(int) DEC_FT, "Decimal Feet" },
				{(int) FRAC_IN, "Fractional Inches" },
				{(int) DEC_IN, "Decimal Inches" }
			};

		public static bool SetProjUnitsToStd(Document _doc,
			UnitStyleType utStyle)
		{
			Units units = StandardUnitStyle(_doc, utStyle);

			if (units == null) return false;

			if (!SetProjUnits(_doc, units)) return false;

			return true;
		}

		public static bool SetProjUnits(Document _doc, Units units)
		{
			try
			{
				_doc.SetUnits(units);
			}
			catch
			{
				return false;
			}

			return true;
		}


		public static Units StandardUnitStyle(Document _doc, UnitStyleType utStyle)
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
