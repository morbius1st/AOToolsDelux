#region + Using Directives

using System.Collections.Generic;
using System.Windows.Data;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

using DeluxMeasure.UnitsUtil;

#endregion

// user name: jeffs
// created:   3/23/2022 10:12:17 PM


// this has the standard list of "UnitStyles"
// except this specifically avoids any Revit specific
// information so that it may also be used as
// sample data

namespace DeluxMeasure.UnitsUtil
{
	public struct STYLE_ID
	{
		public string TypeId { get; private set; }
		public string NameId { get; private set; }

		public STYLE_ID(string typeId, string nameId)
		{
			TypeId = typeId;
			NameId = nameId;
		}

		// cross reference between Revit ForgeTypeId name and standard style Name
		public static readonly STYLE_ID Invalid            = new STYLE_ID(null                  , null);
		public static readonly STYLE_ID Project            = new STYLE_ID("General"             , "Project");
		public static readonly STYLE_ID FtDecIn            = new STYLE_ID("Custom"              , "Feet and Decimal Inches");
		public static readonly STYLE_ID FtFracIn           = new STYLE_ID("FeetFractionalInches", "Feet and Fractional Inches");
		public static readonly STYLE_ID UsSurvey           = new STYLE_ID("UsSurveyFeet"        , "Survey Feet");
		public static readonly STYLE_ID Feet               = new STYLE_ID("Feet"                , "Decimal Feet");
		public static readonly STYLE_ID DecInches          = new STYLE_ID("Inches"              , "Decimal Inches");
		public static readonly STYLE_ID FracInches         = new STYLE_ID("FractionalInches"    , "Fractional Inches");
		public static readonly STYLE_ID Meters             = new STYLE_ID("Meters"              , "Meters"     );
		public static readonly STYLE_ID Decimeters         = new STYLE_ID("Decimeters"          , "Decimeters" );
		public static readonly STYLE_ID Centimeters        = new STYLE_ID("CentiMeters"         , "CentiMeters");
		public static readonly STYLE_ID Millimeters        = new STYLE_ID("MilliMeters"         , "MilliMeters");
		public static readonly STYLE_ID MetersCentimeters  = new STYLE_ID("MetersCentimeters"   , "Meters and Centimeters");
	}

	public class UnitsStdUStyles
	{
/*
		public const string STYLE_ID_FT_DEC_IN = "Custom";
		public const string STYLE_ID_PROJECT = "Project";
		public const string STYLE_ID_FT_FRAC_IN = "FeetFractionalInches";
		public const string STYLE_ID_US_SURVEY = "UsSurveyFeet";
		public const string STYLE_ID_FEET = "Feet";
		public const string STYLE_ID_DEC_INCHES = "Inches";
		public const string STYLE_ID_FRAC_INCHES = "FractionalInches";

		public const string STYLE_ID_METERS = "Meters";
		public const string STYLE_ID_DECIMETERS = "Decimeters";
		public const string STYLE_ID_CENTIMETERS = "CentiMeters";
		public const string STYLE_ID_MILLIMETERS = "MilliMeters";
		public const string STYLE_ID_METERS_CENTIMETERS = "MetersCentimeters";
*/

		static UnitsStdUStyles()
		{
			Initialize();
		}

		public static Dictionary<string, UStyle> StdStyles { get; private set; }

		public static void Initialize()
		{
			// the below also identifies whether a 
			// formatting option is allowed to be set
			// null = cannot be set

			string id;
			string name;

			StdStyles = new Dictionary<string, UStyle>(12);

			// 0
			// project style
			StdStyles.Add(STYLE_ID.Project.NameId, ProjStyle());

			//1
			// id = STYLE_ID_FT_FRAC_IN;
			name = "Feet and Fractional Inches";
			StdStyles.Add(STYLE_ID.FtFracIn.NameId, new UStyle(UnitClass.CL_ORDINARY, // locked
				STYLE_ID.FtFracIn.TypeId,
				STYLE_ID.FtFracIn.NameId,
				$"{name} Unit Style", 
				UnitsSupport.UnitCat.UC_FT_IN_FRAC, 
				UnitsSupport.UnitSys.US_IMPERIAL,
				1.0 / 64.0, 
				// nothing allowed
				null,
				null, true, null, true, true,
				-1, -1, -1, 
				11.2345,
				"Delux Measure ft-frac-in 32.png"));

			// 2
			// id = STYLE_ID_FT_DEC_IN;
			name = "Feet and Decimal Inches";
			StdStyles.Add(STYLE_ID.FtDecIn.NameId, new UStyle(UnitClass.CL_FT_DEC_IN, // locked
				STYLE_ID.FtDecIn.TypeId,
				STYLE_ID.FtDecIn.NameId,
				$"{name} Unit Style", 
				UnitsSupport.UnitCat.UC_FT_IN_DEC, 
				UnitsSupport.UnitSys.US_IMPERIAL,
				0.001, 
				// nothing allowed
				null,
				true, false, null, true, false,
				-1, -1, -1, 
				22.3455,
				"Delux Measure ft-dec-in 32.png"));

			// 3
			// same as decimal feet
			// id = STYLE_ID_US_SURVEY;
			name = "Survey Feet";
			StdStyles.Add(STYLE_ID.UsSurvey.NameId, new UStyle(UnitClass.CL_ORDINARY, 
				STYLE_ID.UsSurvey.TypeId,
				STYLE_ID.UsSurvey.NameId,
				$"{name} Unit Style", 
				UnitsSupport.UnitCat.UC_DECIMAL, 
				UnitsSupport.UnitSys.US_IMPERIAL,
				0.001, 
				// "USft" or "" (use "none")
				"none",
				true, null, null, true, null,
				-1, -1, -1, 
				33.4567,
				"Delux Measure us-ft 32.png"));

			// 4
			// decimal feet
			// id = STYLE_ID_FEET;
			name = "Decimal Feet";
			StdStyles.Add(STYLE_ID.Feet.NameId, new UStyle(UnitClass.CL_ORDINARY,
				STYLE_ID.Feet.TypeId,
				STYLE_ID.Feet.NameId,
				$"{name} Unit Style", 
				UnitsSupport.UnitCat.UC_DECIMAL, 
				UnitsSupport.UnitSys.US_IMPERIAL,
				0.0001, 
				// ' or ft or LF or "" (use "none")
				"'", 
				true, null, null, true, null,
				-1, -1, -1, 
				44.5678,
				"Delux Measure dec-ft 32.png"));

			// 5
			// decimal inches
			// id = STYLE_ID_DEC_INCHES;
			name = "Decimal Inches";
			StdStyles.Add(STYLE_ID.DecInches.NameId, new UStyle(UnitClass.CL_ORDINARY,
				STYLE_ID.DecInches.TypeId,
				STYLE_ID.DecInches.NameId,
				$"{name} Unit Style", 
				UnitsSupport.UnitCat.UC_IN_DEC, 
				UnitsSupport.UnitSys.US_IMPERIAL,
				0.0001, 
				// " or in or ""
				"\"",
				true, null, null, true, null,
				-1, -1, -1, 
				55.6789,
				"Delux Measure dec-in 32.png"));

			// 6
			// fractional inches
			// id = STYLE_ID_FRAC_INCHES;
			name = "Fractional Inches";
			StdStyles.Add(STYLE_ID.FracInches.NameId, new UStyle(UnitClass.CL_ORDINARY,
				STYLE_ID.FracInches.TypeId,
				STYLE_ID.FracInches.NameId,
				$"{name} Unit Style", 
				UnitsSupport.UnitCat.UC_IN_FRAC, 
				UnitsSupport.UnitSys.US_IMPERIAL,
				1.0/256, 
				// nothing allowed
				null,
				null, null, null, true, null,
				-1, -1, -1, 
				66.7891,
				"Delux Measure frac-in 32.png"));

			// METRIC styles

			// 7
			// meters
			// id = STYLE_ID_METERS;
			name = "Meters";
			StdStyles.Add(STYLE_ID.Meters.NameId, new UStyle(UnitClass.CL_ORDINARY,
				STYLE_ID.Meters.TypeId,
				STYLE_ID.Meters.NameId,
				$"{name} Unit Style", 
				UnitsSupport.UnitCat.UC_DECIMAL, 
				UnitsSupport.UnitSys.US_METRIC,
				2.0, 
				// "m" or ""
				"m",
				true, null, null, true, null,
				-1, -1, -1, 
				77.8912,
				"Delux Measure m 32.png"));

			// 8
			// decimeters
			// id = STYLE_ID_DECIMETERS;
			name = "Decimeters";
			StdStyles.Add(STYLE_ID.Decimeters.NameId, new UStyle(UnitClass.CL_ORDINARY,
				STYLE_ID.Decimeters.TypeId,
				STYLE_ID.Decimeters.NameId,
				$"{name} Unit Style", 
				UnitsSupport.UnitCat.UC_DECIMAL, 
				UnitsSupport.UnitSys.US_METRIC,
				10.0, 
				// "dm" or ""
				"dm",
				true, null, null, true, null,
				-1, -1, -1, 
				88.9123,
				"Delux Measure dm 32.png"));

			// 9
			// centimeters
			// id = STYLE_ID_CENTIMETERS;
			name = "Centimeters";
			StdStyles.Add(STYLE_ID.Centimeters.NameId, new UStyle(UnitClass.CL_ORDINARY,
				STYLE_ID.Centimeters.TypeId,
				STYLE_ID.Centimeters.NameId,
				$"{name} Unit Style", 
				UnitsSupport.UnitCat.UC_DECIMAL, 
				UnitsSupport.UnitSys.US_METRIC,
				0.1, 
				// "cm" or "" 
				"cm",
				true, null, null, true, null,
				-1, -1, -1, 
				99.1234,
				"Delux Measure cm 32.png"));

			// 10
			// millimeters
			// id = STYLE_ID_MILLIMETERS;
			name = "Millimeters";
			StdStyles.Add(STYLE_ID.Millimeters.NameId, new UStyle(UnitClass.CL_ORDINARY,
				STYLE_ID.Millimeters.TypeId,
				STYLE_ID.Millimeters.NameId,
				$"{name} Unit Style", 
				UnitsSupport.UnitCat.UC_DECIMAL, 
				UnitsSupport.UnitSys.US_METRIC,
				0.01, 
				// "mm" or ""
				"mm",
				true, null, null, true, null,
				-1, -1, -1, 
				110.2345,
				"Delux Measure mm 32.png"));

			// 11
			// Meters-Centimeters
			// id = STYLE_ID_METERS_CENTIMETERS;
			name = "Meters and Centimeters";
			StdStyles.Add(STYLE_ID.MetersCentimeters.NameId, new UStyle(UnitClass.CL_ORDINARY,
				STYLE_ID.MetersCentimeters.TypeId,
				STYLE_ID.MetersCentimeters.NameId,
				$"{name} Unit Style", 
				UnitsSupport.UnitCat.UC_METER_CM, 
				UnitsSupport.UnitSys.US_METRIC,
				0.001, 
				// nothing allowed
				null,
				null, null, null, true, null,
				-1, -1, -1, 
				111.3456,
				"Delux Measure m-cm 32.png"));
		}

		// public static UStyle ProjStyle()
		// {
		// 	return ProjStyle(STYLE_ID_PROJECT);
		// }

		// this is a place holder
		// the plan is the actual information will
		// be gotten from one of the standard styles
		// except
		public static UStyle ProjStyle()
		{
			return new UStyle(                      // which from this or reference
				UnitClass.CL_PROJECT,               // this
				STYLE_ID.Project.TypeId,            // ref
				STYLE_ID.Project.NameId,            // ref
				"Current Project Units",            // ref
				UnitsSupport.UnitCat.UC_FT_IN_FRAC, // ref
				UnitsSupport.UnitSys.US_IMPERIAL,   // ref
				1.0 / 64.0,                         // ref
				null,                               // ref
				null,                               // ref
				true,                               // ref
				null,                               // ref
				true,                               // ref
				true,                               // ref
				1,                                  // this
				1,                                  // this
				1,                                  // this
				112.4567,                           // this
				"Delux Measure dec-ft 32.png");
				// "Delux Measure ft-frac-in 32.png"); // ref
		}

		




	}
}