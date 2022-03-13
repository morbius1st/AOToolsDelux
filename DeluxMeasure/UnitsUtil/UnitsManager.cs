#region using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using static Autodesk.Revit.DB.FormatOptions;

using DeluxMeasure.Windows.Support;

#endregion

// username: jeffs
// created:  2/26/2022 9:26:35 AM

namespace DeluxMeasure.UnitsUtil
{
	public class UnitsManager
	{
		private static readonly Lazy<UnitsManager> instance =
			new Lazy<UnitsManager>(() => new UnitsManager());

		private List<UnitStyle> styleList = null;
		private List<UnitStyle> stdStyles = null;

		public static UnitsManager Instance => instance.Value;

		public UnitsManager()
		{
			UnitData = new UnitsData();
			UnitStyles = new UnitStyles();

			populateDefaultStyleList();
			stdStyles = UnitStyles.StdStyles;
		}

		public UnitStyles UnitStyles { get; }

		public UnitsData UnitData { get; }

		public List<UnitStyle> StyleList
		{
			get => styleList;
			set => styleList = value;
		}

		public List<UnitStyle> StdStyles
		{
			get => stdStyles;
			set
			{
				if (value != null) stdStyles = value;
			}
		}

		public bool SetUnit(Document doc, UnitStyle style)
		{
			Units units = makeStdLengthUnit( style);

			if (units == null) return false;

			if (SetUnit(doc, units)) return true;

			return false;
		}

		public string FormatLength(double value, UnitStyle style)
		{
			Units units = makeStdLengthUnit(style);
			string result =  UnitFormatUtils.Format(units, SpecTypeId.Length, value, false);
			return result;
		}

		public FormatOptions GetFormatOptions(UnitStyle style)
		{
			FormatOptions fmtOpts;

			try
			{
				fmtOpts = new FormatOptions(style.Id);
				fmtOpts.Accuracy = style.Precision;

				if (CanHaveSymbol(style.Id))
				{
					fmtOpts.SetSymbolTypeId(style.Symbol);
				}

				if (CanSuppressLeadingZeros(style.Id))
				{
					fmtOpts.SuppressLeadingZeros = style.SuppressLeadZero;
				}

				if (CanSuppressTrailingZeros(style.Id))
				{
					fmtOpts.SuppressTrailingZeros = style.SuppressTrailZero;
				}

				if (CanSuppressSpaces(style.Id))
				{
					fmtOpts.SuppressSpaces = style.SuppressSpaces;
				}

				if (CanUsePlusPrefix(style.Id))
				{
					fmtOpts.UsePlusPrefix = style.UsePlusPrefix;
				}
			}
			catch (Exception e)
			{
				return null;
			}

			return fmtOpts;
		}

		public bool SetUnit(Document doc, Units unit)
		{
			try { doc.SetUnits(unit); }
			catch (Exception e)
			{
				return false;
			}

			return true;
		}

		public Units makeStdLengthUnit(UnitStyle style)
		{
			Units units;
			FormatOptions fmtOpts;

			try
			{
				fmtOpts = GetFormatOptions(style);
			}
			catch (Exception e)
			{
				return null;
			}

			units = new Units(style.USys);
			units.SetFormatOptions(SpecTypeId.Length, fmtOpts);
			
			return units;
		}

		private void populateDefaultStyleList()
		{
			styleList = new List<UnitStyle>();

			foreach (UnitStyle unitStyle in UnitStyles.StdStyles)
			{
				styleList.Add(unitStyle);
			}
		}


	#region system overrides

		public override string ToString()
		{
			return "this is UnitUtils";
		}

	#endregion
	}
}