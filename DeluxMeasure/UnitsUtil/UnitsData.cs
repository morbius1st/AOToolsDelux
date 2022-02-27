#region using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using EnvDTE;
using Autodesk.Revit.DB;
using static Autodesk.Revit.DB.UnitSystem;
using static DeluxMeasure.UnitsUtil.UnitStyles;

#endregion

// username: jeffs
// created:  2/19/2022 10:42:55 PM

namespace DeluxMeasure.UnitsUtil
{
	public class UnitsData
	{
		public static Dictionary<ForgeTypeId, UnitInfo> UnitTypes { get; private set; }

		public struct UnitInfo
		{
			public UnitId Id { get; }
			public String Title { get; }
			public string Desc { get; }
			public string IconFile { get; }

			public UnitSystem USys => Id.USys;
			public UnitCat UCat => Id.UCat;

			public UnitInfo(  UnitId id,
				string title, string desc, string iconFile)
			{
				Id = id;
				Title = title;
				Desc = desc;
				IconFile = iconFile;
			}
		}

		public struct UnitId
		{
			public ForgeTypeId Id { get; }
			public UnitSystem USys { get; }
			public UnitCat UCat { get; }

			public UnitId(ForgeTypeId id, UnitSystem uSys, UnitCat uCat)
			{
				USys = uSys;
				Id = id;
				UCat = uCat;
			}
		}

	#region private fields

		private static Dictionary<string, double>[] precisions;
		private Dictionary<string, double> precDecimal;
		private Dictionary<string, double> precInFrac;
		private Dictionary<string, double> precFtFrac;
		private Dictionary<string, double> precMeterCm;

	#endregion

	#region ctor

		public UnitsData()
		{
			init();
		}

	#endregion

	#region public properties

	#endregion

	#region private properties

	#endregion

	#region public methods

		public UnitInfo GetInfo(ForgeTypeId id)
		{
			return UnitTypes[id];
		}

		public string GetIcon(ForgeTypeId id)
		{
			return UnitTypes[id].IconFile;
		}

		public List<UnitInfo> GetImperial()
		{
			return getUnitsBySys(UnitSystem.Imperial);
		}

		public List<UnitInfo> GetMetric()
		{
			return getUnitsBySys(UnitSystem.Metric);
		}

		public static string GetPrecString(UnitCat uc, double prec)
		{
			return getPrecString(precisions[(int) uc], prec);
		}
		
		public static double GetPrec(UnitCat uc, string prec)
		{
			return getPrec(precisions[(int) uc], prec);
		}

	#endregion

	#region private methods

		private List<UnitInfo> getUnitsBySys(UnitSystem uSys)
		{
			List<UnitInfo> result = new List<UnitInfo>();

			foreach (KeyValuePair<ForgeTypeId, UnitInfo> kvp in UnitTypes)
			{
				if (kvp.Value.Id.USys == uSys)
				{
					result.Add(kvp.Value);
				}
			}

			return result;
		}

		private void init()
		{
			assignUnitInfo();

			assignPrecStrings();
		}

		private void assignUnitInfo()
		{
			UnitTypes = new Dictionary<ForgeTypeId, UnitInfo>();
			//																											  
			addUnitType( UnitTypeId.FeetFractionalInches, Imperial, UnitCat.FT_FRAC, 
				"Feet+Frac In"       , "Feet and Fractional Inches", "Delux Measure ft-frac-in 32.png");
			addUnitType( UnitTypeId.UsSurveyFeet        , Imperial, UnitCat.DECIMAL, 
				"Feet Us"            , "Survey Feet"               , "Delux Measure us-ft 32.png");
			addUnitType( UnitTypeId.Feet                , Imperial, UnitCat.DECIMAL, 
				"Dec Feet"           , "Decimal Feet"              , "Delux Measure dec-ft 32.png");
			addUnitType( UnitTypeId.Inches              , Imperial, UnitCat.DECIMAL, 
				"Dec In"             , "Decimal Inches"            , "Delux Measure dec-in 32.png");
			addUnitType( UnitTypeId.FractionalInches    , Imperial, UnitCat.IN_FRAC, 
				"Frac In"            , "Fractional Inches"         , "Delux Measure frac-in 32.png");
			addUnitType( UnitTypeId.Meters              , Metric, UnitCat.DECIMAL  , 
				"Meters"             , "Meters"                    , "Delux Measure m 32.png");
			addUnitType( UnitTypeId.Decimeters          , Metric, UnitCat.DECIMAL  , 
				"Decimeters"         , "Decimeters"                , "Delux Measure dm 32.png");
			addUnitType( UnitTypeId.Centimeters         , Metric, UnitCat.DECIMAL  , 
				"Centimeters"        , "Centimeters"               , "Delux Measure cm 32.png");
			addUnitType( UnitTypeId.Millimeters         , Metric, UnitCat.DECIMAL  , 
				"Millimeters"        , "Millimeters"               , "Delux Measure mm 32.png");
			addUnitType( UnitTypeId.MetersCentimeters   , Metric, UnitCat.METER_CM  , 
				"Meters+cm"          , "Meters and Centimeters"    , "Delux Measure m-cm 32.png");
		}

		private void addUnitType(  ForgeTypeId id, UnitSystem us,
			UnitCat uc, string title, string desc, string icon)
		{
			UnitsData.UnitId ui = new UnitsData.UnitId( id, us, uc);

			UnitTypes.Add(id, new UnitsData.UnitInfo(ui, title, desc, icon));
		}


		private static double getPrec(Dictionary<string, double> data, string prec)
		{
			foreach (KeyValuePair<string, double> kvp in data)
			{
				if (kvp.Key.Equals(prec)) return kvp.Value;
			}

			return -1.0;
		}

		private static string getPrecString(Dictionary<string, double> data, double prec)
		{
			string result = "custom";

			foreach (KeyValuePair<string, double> kvp in data)
			{
				if (kvp.Value.Equals(prec)) return kvp.Key;
			}

			return "custom";
		}

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


		private void assignPrecStrings()
		{

			precisions = new Dictionary<string, double>[4];

			precInFrac = new Dictionary<string, double>()
			{
				{ "1/256\"", 1.0 / 256 }, { "1/128\"", 1.0 / 128 }, { "1/64\"", 1.0 / 64 }, { "1/32\"", 1.0 / 32 },
				{ "1/16\"", 1.0 / 16 }, { "1/8\"", 0.125 }, { "1/4\"", 0.25 }, { "1/2\"", 0.5 }, { "1\"", 1.0 }
			};
			precisions[(int) UnitCat.IN_FRAC] = precInFrac;


			precFtFrac = new Dictionary<string, double>()
			{
				{ "1/256\"", (1.0 / 256) / 12.0 }, { "1/128\"", (1.0 / 128) / 12.0 }, { "1/64\"", (1.0 / 64) / 12.0 }, { "1/32\"", (1.0 / 32) / 12.0 },
				{ "1/16\"", (1.0 / 16) / 12.0 }, { "1/8\"", 0.125 / 12.0 }, { "1/4\"", 0.25 / 12.0 }, { "1/2\"", 0.5 / 12.0 },
				{ "1\"", 1.0 / 12.0 }, { "6\"", 0.5 }, { "1\'", 1.0 },
			};
			precisions[(int) UnitCat.FT_FRAC] = precFtFrac;


			precDecimal = new Dictionary<string, double>()
			{
				{ "1", 1.0 }, { "10", 10.0 }, { "100", 100.0 }, { "1000", 1000.0 },
				{ "1 decimal place", 0.1 }, { "2 decimal place", 0.01 }, { "3 decimal place", 0.001 }
			};
			precisions[(int) UnitCat.DECIMAL] = precDecimal;


			precMeterCm = new Dictionary<string, double>()
			{
				{ "1 cm", 0.01 }, { "5 mm", 0.005 }, { "2.5 mm", 0.0025 }, { "1 mm", 0.001 }, { "0.1 mm", 0.0001 }
			};
			precisions[(int) UnitCat.METER_CM] = precMeterCm;
		}

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
	}
}