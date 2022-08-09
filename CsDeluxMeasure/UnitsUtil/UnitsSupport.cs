#region using

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows.Data;

using Autodesk.Revit.DB;
using static Autodesk.Revit.DB.FormatOptions;
using static CsDeluxMeasure.UnitsUtil.UnitsStdUStyles;
#endregion

// username: jeffs
// created:  2/19/2022 10:42:55 PM

namespace CsDeluxMeasure.UnitsUtil
{
	public partial class UnitsSupport
	{
		public const string DEFAULT_UNIT_ICON_NAME = "information32.png";

	#region private fields

		private static Dictionary<ForgeTypeId, STYLE_DATA> UnitTypeToString;
		private static Dictionary<STYLE_DATA, ForgeTypeId> StringToUnitType;

		private static Dictionary<string, double>[] precisions;
		private static Dictionary<string, double> precDecimal;
		private static Dictionary<string, double> precInFrac;
		private static Dictionary<string, double> precFtFrac;
		private static Dictionary<string, double> precMeterCm;


		private static Dictionary<UnitCat, int> pricXref;
		private static Dictionary<double, string>[] precisions2;
		private static Dictionary<double, string> precDecimal2;
		private static Dictionary<double, string> precInFrac2;
		private static Dictionary<double, string> precFtFrac2;
		private static Dictionary<double, string> precMeterCm2;

		public static string[][] symbolStrings;

	#endregion

	#region ctor

		static UnitsSupport()
		{
			try
			{
				init();
			}
			catch (Exception e)
			{

				Debug.WriteLine($"try error| {e.Message}");
			}
		}

		public UnitsSupport()
		{
			assignUnitTypesAndStrings();
		}

	#endregion

	#region public properties

	#endregion

	#region private properties

	#endregion

	#region public static methods



	#endregion
		
	#region public methods

		public void SetInitialSequence(List<UnitsDataR> styleList)
		{
			foreach (UnitsDataR udr in styleList)
			{
				udr.InitialSequence = udr.Sequence;
			}
		}

		public void ResetInitialSequence(List<UnitsDataR> styleList)
		{
			foreach (UnitsDataR udr in styleList)
			{
				udr.Sequence = udr.InitialSequence;
			}
		}

		public void UnDelete(List<UnitsDataR> styleList)
		{
			for (int i = styleList.Count - 1; i >= 0 ; i--)
			{
				styleList[i].DeleteStyle = false;

			}
		}

		public void RemoveDeleted(List<UnitsDataR> styleList)
		{
			for (int i = styleList.Count - 1; i >= 0 ; i--)
			{
				if (styleList[i].DeleteStyle)
				{
					styleList.RemoveAt(i);
				}
			}
		}

		public void ReSequence(ListCollectionView styles, int start, bool increase)
		{
			int amt = -1;
			if (increase) amt = 1;

			int idx = start;

			for (int i = start; i < styles.Count; i++)
			{
				UnitsDataR udr = styles.GetItemAt(i) as UnitsDataR;


				// Debug.Write($"checking: {udr.Ustyle.Name,30}   ");

				if (udr.Skip())
				{
					// Debug.WriteLine($"  skip? {udr.Skip(),5}  del: {udr.DeleteStyle,5}");
					continue;
				}

				// Debug.Write($"  seq before: {udr.Sequence:D2}");

				udr.Sequence += amt;

				// Debug.WriteLine($"  seq after: {udr.Sequence:D2}");
			}
		}

		public static string GetSampleFormatted(UnitsDataR udr, double sample)
		{
			Units units = makeStdLengthUnit(udr);

			if (units == null) return "N/A";

			string result =  UnitFormatUtils.Format(units, SpecTypeId.Length, sample, false);
			return result;
		}

		public static string GetPrecString(UnitCat uc, double prec, string uSym)
		{

			int table = pricXref[uc];

			return getPrecString2(precisions2[table], prec, uSym);
		}

		public static string GetTypeIdAsString(ForgeTypeId key)
		{
			if (!UnitTypeToString.ContainsKey(key)) return null;
		
			return UnitTypeToString[key].NameId;
		}

		public static STYLE_DATA GetTypeIdAsStyleId(ForgeTypeId key)
		{
			if (!UnitTypeToString.ContainsKey(key)) return STYLE_DATA.Invalid;
		
			return UnitTypeToString[key];
		}

		public static string GetSymbol(ForgeTypeId symbol, UnitCat uCat)
		{
			if (symbol == null) return GetSymbol(uCat);

			if (symbol.Empty()) return "None";

			ForgeTypeId id = symbol;

			string result = LabelUtils.GetLabelForSymbol(id);

			return result;
		}

		// only when symbol is null
		public static string GetSymbol(UnitCat uCat)
		{
			string[] s = symbolStrings[(int) uCat];

			string result = $"{s[0]}";

			result += s.Length > 1 ? $" & {s[1]}" : null;

			return result;
		}

		public List<UnitsDataR> UnitsDataRListClone(List<UnitsDataR> styleList)
		{
			List<UnitsDataR> styleListCopy = new List<UnitsDataR>(styleList.Count);
			UnitsDataR udrCopy;

			foreach (UnitsDataR udr in styleList)
			{
				udrCopy = udr.Clone();
				styleListCopy.Add(udrCopy);
			}

			return styleListCopy;
		}

		public UnitsDataR GetProjectUnitData(Document doc)
		{
			UnitsDataR udr;

			if (doc == null)
			{
				return UnitsManager.Instance.StdStyles[STYLE_DATA.Project.NameId];
				// return UnitsManager.Instance.StdStyles[
				// 	StringToUnitType[UnitsStdUStyles.STYLE_ID_PROJECT]];
			}
			else
			{
				udr = UDRFromRevitUnits(doc.GetUnits());
				udr.Ustyle.UnitClass = UnitClass.CL_PROJECT;
			}

			return udr;
		}

		public UnitsDataR UDRFromRevitUnits(Units u)
		{
			FormatOptions fo = u.GetFormatOptions(SpecTypeId.Length);

			UStyle us = USFromRevitFO(fo);

			ForgeTypeId id = fo.GetUnitTypeId();
			ForgeTypeId symbol = fo.GetSymbolTypeId();
			UnitSystem usys = (UnitSystem) (int) us.UnitSys;

			UnitsDataR udr = new UnitsDataR(id, symbol, us);
			return udr;
		}

		public UStyle USFromRevitUnits(Units u)
		{
			FormatOptions fo = u.GetFormatOptions(SpecTypeId.Length);

			return USFromRevitFO(fo);
		}

		public UStyle USFromRevitFO(FormatOptions fo)
		{
			ForgeTypeId id = fo.GetUnitTypeId();
			ForgeTypeId symbol = fo.GetSymbolTypeId();

			STYLE_DATA sid =GetTypeIdAsStyleId(id);

			UStyle baseUs = UnitsManager.Instance.StdStyles[sid.NameId].Ustyle;

			UStyle us 
				= new UStyle(UnitClass.CL_ORDINARY,
				sid.TypeId,  // only for this the project style
				sid.NameId,
				baseUs.Description,
				baseUs.UnitCat,
				baseUs.UnitSys,
				fo.Accuracy,
				GetSymbol(symbol, baseUs.UnitCat),
				baseUs.SuppressTrailZeros.HasValue ? (bool?) fo.SuppressTrailingZeros : null,
				baseUs.SuppressLeadZeros.HasValue ? (bool?) fo.SuppressLeadingZeros : null,
				baseUs.UsePlusPrefix.HasValue ? (bool?)	fo.UsePlusPrefix : null,
				baseUs.UseDigitGrouping.HasValue ? (bool?) fo.UseDigitGrouping : null,
				baseUs.SuppressSpaces.HasValue ? (bool?) fo.SuppressSpaces : null,
				-1, -1, -1,
				baseUs.Sample,
				baseUs.IconId);

			return us;
		}

		private bool? getBool(bool? baseUs, bool? fo)
		{
			return baseUs.HasValue ? fo : null;
		}

		public UnitsDataR UDRClone(UnitsDataR orig, 
			string name, string desc, int seq)
		{
			UnitsDataR udr = new UnitsDataR(orig.Id, orig.Symbol, orig.Ustyle.Clone());

			udr.Ustyle.Name = name;
			udr.Ustyle.Description = desc;
			udr.Sequence = seq;

			return udr;
		}


		public static string CheckStyleNameSyntax(string testName)
		{
			// rules
			// validation requirements
			// min 4 characters
			// must start with alphanumeric (uc or lc)
			// middle is alphanumeric, space, dash, period
			// must end with alphanumeric (no dash, no space, no period)
			// name must be unique
			if (testName == null || testName.Length < 4)
			{
				return "Name mist be a minimum of 4 characters";
			}

			Regex r = new Regex("^[a-zA-Z0-9][a-zA-Z0-9\\. \\-]*[a-zA-Z0-9]{1}$");

			if (!r.IsMatch(testName))
			{
				return "Name has invalid characters";
			}

			return null;
		}

		public static string CheckStyleDescSyntax(string newName)
		{

			if (newName == null || newName.Length < 6)
			{
				return "Name mist be a minimum of 6 characters";
			}

			Regex r = new Regex("^[a-zA-Z0-9][a-zA-Z0-9\\. \\-]*[a-zA-Z0-9]{1}$");

			if (!r.IsMatch(newName))
			{
				return "Name has invalid characters";
			}

			return null;
		}


	#endregion
		
	#region private static methods


		private static void init()
		{
			// assignUnitInfo();

			assignSymbols();

			assignPrecStrings();
			assignPrecStrings2();

			assignPricTableXref();
		}

		private static Units makeStdLengthUnit(UnitsDataR udr)
		{
			// if (udr.Ustyle.IsLocked == null) return null;

			Units units;
			FormatOptions fmtOpts;

			try
			{
				fmtOpts = getFormatOptions(udr);
			}
			catch (Exception e)
			{
				fmtOpts = null;
			}

			if (fmtOpts == null) return null;

			units = new Units(udr.USystem);
			units.SetFormatOptions(SpecTypeId.Length, fmtOpts);

			return units;
		}

		private static FormatOptions getFormatOptions(UnitsDataR style)
		{
			FormatOptions fmtOpts;
			UStyle us = style.Ustyle;

			try
			{
				fmtOpts = new FormatOptions(style.Id);
				fmtOpts.Accuracy = us.Precision;

				if (CanHaveSymbol(style.Id))
				{
					fmtOpts.SetSymbolTypeId(style.Symbol);
				}

				if (CanSuppressLeadingZeros(style.Id))
				{
					fmtOpts.SuppressLeadingZeros = setFmtOpt(us.SuppressLeadZeros);
				}

				if (CanSuppressTrailingZeros(style.Id))
				{
					fmtOpts.SuppressTrailingZeros = setFmtOpt(us.SuppressTrailZeros);
				}

				if (CanSuppressSpaces(style.Id))
				{
					fmtOpts.SuppressSpaces = setFmtOpt(us.SuppressSpaces);
				}

				if (CanUsePlusPrefix(style.Id))
				{
					fmtOpts.UsePlusPrefix = setFmtOpt(us.UsePlusPrefix);
				}
			}
			catch (Exception e)
			{
				return null;
			}

			return fmtOpts;
		}

		private static bool setFmtOpt(bool? opt)
		{
			if (opt == null) return false;

			return opt.Value;
		}

		private static string getPrecString2(Dictionary<double, string> data, 
			double precision, string uSym)
		{
			

			if (data.ContainsKey(precision))
			{
				return data[precision];
			}


			if (data.ContainsKey(-1.0)) return "*Invalid*";

			string s = String.Format(data[0.0], precision, uSym);

			return s;

			return $"{data[0.0]} ({precision:G})";
		}

		private static void assignSymbols()
		{
			symbolStrings = new string[UnitData.UCAT_COUNT][];

			symbolStrings[(int) UnitCat.UC_METER_CM] = new string[2];
			symbolStrings[(int) UnitCat.UC_METER_CM][0] = "m";
			symbolStrings[(int) UnitCat.UC_METER_CM][1] = "cm";

			symbolStrings[(int) UnitCat.UC_FT_IN_FRAC] = new string[2];
			symbolStrings[(int) UnitCat.UC_FT_IN_FRAC][0] = "'";
			symbolStrings[(int) UnitCat.UC_FT_IN_FRAC][1] = "\"";

			symbolStrings[(int) UnitCat.UC_FT_IN_DEC] = new string[2];
			symbolStrings[(int) UnitCat.UC_FT_IN_DEC][0] = "'";
			symbolStrings[(int) UnitCat.UC_FT_IN_DEC][1] = "\"";

			symbolStrings[(int) UnitCat.UC_IN_FRAC] = new string[1];
			symbolStrings[(int) UnitCat.UC_IN_FRAC][0] = "\"";
		}

		private static void assignUnitTypesAndStrings()
		{
			UnitTypeToString = new Dictionary<ForgeTypeId, STYLE_DATA>(12);
		
			StringToUnitType = new Dictionary<STYLE_DATA, ForgeTypeId>(12);
		
			ForgeTypeId fid;
			STYLE_DATA sid;
		
			fid = UnitTypeId.Custom;
			sid = STYLE_DATA.FtDecIn;
			UnitTypeToString.Add(fid, sid);
			StringToUnitType.Add(sid, fid);
		
			fid = UnitTypeId.General;
			sid = STYLE_DATA.Project;
			UnitTypeToString.Add(fid, sid);
			StringToUnitType.Add(sid, fid);
		
			fid = UnitTypeId.FeetFractionalInches;
			sid = STYLE_DATA.FtFracIn;
			UnitTypeToString.Add(fid, sid);
			StringToUnitType.Add(sid, fid);
		
			fid = UnitTypeId.UsSurveyFeet;
			sid = STYLE_DATA.UsSurvey;
			UnitTypeToString.Add(fid, sid);
			StringToUnitType.Add(sid, fid);
			
			fid = UnitTypeId.Feet;
			sid = STYLE_DATA.Feet;
			UnitTypeToString.Add(fid, sid);
			StringToUnitType.Add(sid, fid);

			fid = UnitTypeId.Inches;
			sid = STYLE_DATA.DecInches;
			UnitTypeToString.Add(fid, sid);
			StringToUnitType.Add(sid, fid);
			
			fid = UnitTypeId.FractionalInches;
			sid = STYLE_DATA.FracInches;
			UnitTypeToString.Add(fid, sid);
			StringToUnitType.Add(sid, fid);
			


			fid = UnitTypeId.Meters;
			sid = STYLE_DATA.Meters;
			UnitTypeToString.Add(fid, sid);
			StringToUnitType.Add(sid, fid);
			
			fid = UnitTypeId.Decimeters;
			sid = STYLE_DATA.Decimeters;
			UnitTypeToString.Add(fid, sid);
			StringToUnitType.Add(sid, fid);
			
			fid = UnitTypeId.Centimeters;
			sid = STYLE_DATA.Centimeters;
			UnitTypeToString.Add(fid, sid);
			StringToUnitType.Add(sid, fid);
			
			fid = UnitTypeId.Millimeters;
			sid = STYLE_DATA.Millimeters;
			UnitTypeToString.Add(fid, sid);
			StringToUnitType.Add(sid, fid);
		
			fid = UnitTypeId.MetersCentimeters;
			sid = STYLE_DATA.MetersCentimeters;
			UnitTypeToString.Add(fid, sid);
			StringToUnitType.Add(sid, fid);
		}

		private static void assignPricTableXref()
		{
			pricXref = new Dictionary<UnitCat, int>(UnitData.UCAT_COUNT);

			// cross ref if uCat is this      | then use this table
			pricXref.Add(UnitCat.UC_METER_CM  , (int) PrecXref.XR_M_CM);
			pricXref.Add(UnitCat.UC_FT_IN_FRAC, (int) PrecXref.XR_FRAC_IN);
			pricXref.Add(UnitCat.UC_FT_IN_DEC , (int) PrecXref.XR_DEC);
			pricXref.Add(UnitCat.UC_DECIMAL   , (int) PrecXref.XR_DEC);
			pricXref.Add(UnitCat.UC_IN_DEC    , (int) PrecXref.XR_DEC);
			pricXref.Add(UnitCat.UC_IN_FRAC   , (int) PrecXref.XR_FRAC_IN);
			pricXref.Add(UnitCat.UC_FT_FRAC   , (int) PrecXref.XR_FRAC_FT);
		}

		private static void assignPrecStrings()
		{
		// unit
		// for m+cm  1 cm to 0.1 mm

		// for m  1000 to .001 & custom
		// for cm  same
		// for dm  same
		// for mm  same
		// for ft  same
		// for usft  same
		// for in  same
		// for frac in  1" to 1/256"
		// for ft+ frac in  1' to 1/256"  (but divided by 12)

			precisions = new Dictionary<string, double>[UnitData.UCAT_COUNT];

			precInFrac = new Dictionary<string, double>()
			{
				{ "1/256\"", 1.0 / 256 }, { "1/128\"", 1.0 / 128 }, { "1/64\"", 1.0 / 64 }, { "1/32\"", 1.0 / 32 },
				{ "1/16\"", 1.0 / 16 }, { "1/8\"", 0.125 }, { "1/4\"", 0.25 }, { "1/2\"", 0.5 }, { "1\"", 1.0 }
			};
			precisions[(int) UnitCat.UC_IN_FRAC] = precInFrac;
			// precisions[(int) PrecXref.FRAC_IN] = precInFrac;


			precFtFrac = new Dictionary<string, double>()
			{
				{ "1/256\"", (1.0 / 256) / 12.0 }, { "1/128\"", (1.0 / 128) / 12.0 }, { "1/64\"", (1.0 / 64) / 12.0 }, { "1/32\"", (1.0 / 32) / 12.0 },
				{ "1/16\"", (1.0 / 16) / 12.0 }, { "1/8\"", 0.125 / 12.0 }, { "1/4\"", 0.25 / 12.0 }, { "1/2\"", 0.5 / 12.0 },
				{ "1\"", 1.0 / 12.0 }, { "6\"", 0.5 }, { "1\'", 1.0 },
			};
			precisions[(int) UnitCat.UC_FT_FRAC] = precFtFrac;
			// precisions[(int) PrecXref.FRAC_FT] = precFtFrac;


			precDecimal = new Dictionary<string, double>()
			{
				{ "To the Nearest 1", 1.0 }, { "To the Nearest 10", 10.0 }, { "To the Nearest 100", 100.0 }, { "To the Nearest 1000", 1000.0 },
				{ "1 decimal place", 0.1 }, { "2 decimal place", 0.01 }, { "3 decimal place", 0.001 }
			};
			precisions[(int) UnitCat.UC_DECIMAL] = precDecimal;
			// precisions[(int) PrecXref.DEC] = precDecimal;


			precMeterCm = new Dictionary<string, double>()
			{
				{ "To the Nearest 1 cm", 0.01 }, { "To the Nearest 5 mm", 0.005 }, { "To the Nearest 2.5 mm", 0.0025 }, { "To the Nearest 1 mm", 0.001 },
				{ "To the Nearest 0.1 mm", 0.0001 }
			};
			precisions[(int) UnitCat.UC_METER_CM] = precMeterCm;
			// precisions[(int) PrecXref.M_CM] = precMeterCm;
		}

		private static void assignPrecStrings2()
		{
			precisions2 = new Dictionary<double, string>[UnitData.UCAT_COUNT];

			precInFrac2 = new Dictionary<double, string>()
			{
				{ -1.0     , "no custom" },
				{ 1.0 / 256, "1/256\""  },
				{ 1.0 / 128, "1/128\""  },
				{ 1.0 / 64 , "1/64\""   },
				{ 1.0 / 32 , "1/32\""   },
				{ 1.0 / 16 , "1/16\""   },
				{ 0.125    , "1/8\""    },
				{ 0.25     , "1/4\""    },
				{ 0.5      , "1/2\""    },
				{ 1.0      , "1\""      }
			};
			// precisions2[(int) UnitCat.IN_FRAC] = precInFrac2;
			precisions2[(int) PrecXref.XR_FRAC_IN] = precInFrac2;

// todo: no such thing as fractional feet
			precFtFrac2 = new Dictionary<double, string>()
			{
				{ -1.0              , "no custom" },
				{ (1.0 / 256) / 12.0, "1/256\""  },
				{ (1.0 / 128) / 12.0, "1/128\""  },
				{ (1.0 / 64) / 12.0 , "1/64\""   },
				{ (1.0 / 32) / 12.0 , "1/32\""   },
				{ (1.0 / 16) / 12.0 , "1/16\""   },
				{ 0.125 / 12.0      , "1/8\""    },
				{ 0.25 / 12.0       , "1/4\""    },
				{ 0.5 / 12.0        , "1/2\""    },
				{ 1.0 / 12.0        , "1\""      },
				{ 0.5               , "6\""      },
				{ 1.0               , "1\'"      },
			};
			// precisions2[(int) UnitCat.FT_FRAC] = precFtFrac2;
			precisions2[(int) PrecXref.XR_FRAC_FT] = precFtFrac2;


			precDecimal2 = new Dictionary<double, string>()
			{
				{ 0.0   ,  "custom ({0:G}{1})" },
				{ 1.0   ,  "To the Nearest 1"     },
				{ 10.0  ,  "To the Nearest 10"    },
				{ 100.0 ,  "To the Nearest 100"   },
				{ 1000.0,  "To the Nearest 1000"  },
				{ 0.1   ,  "1 decimal place (0.1)"  },
				{ 0.01  ,  "2 decimal place (0.01)" },
				{ 0.001 ,  "3 decimal place (0.001)" }
			};
			// precisions2[(int) UnitCat.DECIMAL] = precDecimal2;
			precisions2[(int) PrecXref.XR_DEC] = precDecimal2;


			precMeterCm2 = new Dictionary<double, string>()
			{
				{ -1.0  ,  "no custom"    },
				{ 0.01  ,  "To the Nearest 1 cm"   },
				{ 0.005 ,  "To the Nearest 5 mm"   },
				{ 0.0025,  "To the Nearest 2.5 mm" },
				{ 0.001 ,  "To the Nearest 1 mm"   },
				{ 0.0001,  "To the Nearest 0.1 mm" }
			};
			// precisions2[(int) UnitCat.METER_CM] = precMeterCm2;
			precisions2[(int) PrecXref.XR_M_CM] = precMeterCm2;
		}

	#endregion

	#region private methods
		
	#endregion

	#region event consuming

	#endregion

	#region event publishing

	#endregion

	#region system overrides

		public new static string ToString()
		{
			return "this is UnitsManager";
		}

	#endregion


		// public UnitInfo GetInfo(ForgeTypeId id)
		// {
		// 	return UnitTypes[id];
		// }

		// public string GetIcon(ForgeTypeId id)
		// {
		// 	string key = id.ToString();
		//
		// 	try
		// 	{
		// 		UStyle us = UnitsStdUStyles.StdStyles[key];
		// 		return us.IconId;
		//
		// 	}
		// 	catch (Exception e)
		// 	{
		// 		return DEFAULT_UNIT_ICON_NAME;
		// 	}
		// }

		// public List<UnitInfo> GetImperial()
		// {
		// 	return getUnitsBySys(UnitSystem.Imperial);
		// }
		//
		// public List<UnitInfo> GetMetric()
		// {
		// 	return getUnitsBySys(UnitSystem.Metric);
		// }


		// private List<UnitInfo> getUnitsBySys(UnitSystem uSys)
		// {
		// 	List<UnitInfo> result = new List<UnitInfo>();
		//
		// 	foreach (KeyValuePair<ForgeTypeId, UnitInfo> kvp in UnitTypes)
		// 	{
		// 		if (kvp.Value.USys == uSys)
		// 		{
		// 			result.Add(kvp.Value);
		// 		}
		// 	}
		//
		// 	return result;
		// }

		//
		// private void assignUnitInfo()
		// {
		// 	UnitTypes = new Dictionary<ForgeTypeId, UnitInfo>(12);
		//
		// 	addUnitInfo(UnitTypeId.General);
		//
		//
		//
		// 	
		// 	UnitTypes.Add(UnitTypeId.General             , new UnitInfo(UnitTypeId.General             , Imperial, "General             "));
		// 	UnitTypes.Add(UnitTypeId.FeetFractionalInches, new UnitInfo(UnitTypeId.FeetFractionalInches, Imperial, "FeetFractionalInches"));
		// 	UnitTypes.Add(UnitTypeId.Custom              , new UnitInfo(UnitTypeId.Custom              , Imperial, "Custom              "));
		// 	UnitTypes.Add(UnitTypeId.UsSurveyFeet        , new UnitInfo(UnitTypeId.UsSurveyFeet        , Imperial, "UsSurveyFeet        "));
		// 	UnitTypes.Add(UnitTypeId.Feet                , new UnitInfo(UnitTypeId.Feet                , Imperial, "Feet                "));
		// 	UnitTypes.Add(UnitTypeId.Inches              , new UnitInfo(UnitTypeId.Inches              , Imperial, "Inches              "));
		// 	UnitTypes.Add(UnitTypeId.FractionalInches    , new UnitInfo(UnitTypeId.FractionalInches    , Imperial, "FractionalInches    "));
		// 	UnitTypes.Add(UnitTypeId.Meters              , new UnitInfo(UnitTypeId.Meters              , Metric  , "Meters              "));
		// 	UnitTypes.Add(UnitTypeId.Decimeters          , new UnitInfo(UnitTypeId.Decimeters          , Metric  , "Decimeters          "));
		// 	UnitTypes.Add(UnitTypeId.Centimeters         , new UnitInfo(UnitTypeId.Centimeters         , Metric  , "Centimeters         "));
		// 	UnitTypes.Add(UnitTypeId.Millimeters         , new UnitInfo(UnitTypeId.Millimeters         , Metric  , "Millimeters         "));
		// 	UnitTypes.Add(UnitTypeId.MetersCentimeters   , new UnitInfo(UnitTypeId.MetersCentimeters   , Metric  , "MetersCentimeters   "));
		//
		// }
		//
		// private void addUnitInfo(ForgeTypeId key)
		// {
		// 	string id = key.ToString();
		// 	UStyle us = UnitsStandard.StdStyles[id];
		// 	UnitSystem uSys = (UnitSystem) (int) us.UnitSys;
		//
		// 	UnitInfo ui = new UnitInfo(key, uSys, us);
		//
		// 	UnitTypes.Add(key, ui);
		// }
	}


}