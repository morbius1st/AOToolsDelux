#region + Using Directives

using System.Collections.Generic;
using System.Windows.Data;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

using static CsDeluxMeasure.UnitsUtil.UnitData;

#endregion

// user name: jeffs
// created:   3/23/2022 10:12:17 PM


// this has the standard list of "UnitStyles"
// except this specifically avoids any Revit specific
// information so that it may also be used as
// sample data

namespace CsDeluxMeasure.UnitsUtil
{
	public struct STYLE_DATA
	{
		public string TypeId { get; private set; }
		public string NameId { get; private set; }
		public int InRibbonPos { get; }
		public int InDlgLeftPos { get; }
		public int InDlgRightPos { get; }

		// static STYLE_DATA()
		// {
		// 	STYLE_INFO = new STYLE_DATA[12];
		// 	config();
		// }

		public STYLE_DATA(string typeId, string nameId, int inRibbonPos, int inDlgLeftPos, int inDlgRightPos)
		{
			TypeId = typeId;
			NameId = nameId;
			InRibbonPos = inRibbonPos;
			InDlgLeftPos = inDlgLeftPos;
			InDlgRightPos = inDlgRightPos;
		}

		// cross reference between Revit ForgeTypeId name and standard style Name
		public static readonly STYLE_DATA Invalid            = new STYLE_DATA(null                  , null                        , INLIST_UNDEFINED, INLIST_UNDEFINED, INLIST_UNDEFINED);
		// public static readonly STYLE_DATA Control01          = new STYLE_DATA("Control_01"          , "Control_01"                , INLIST_UNDEFINED, INLIST_UNDEFINED, INLIST_UNDEFINED);
		public static readonly STYLE_DATA Project            = new STYLE_DATA("General"             , "Project"                   , INLIST_DISABLED,  0,  0);
		public static readonly STYLE_DATA FtDecIn            = new STYLE_DATA("Custom"              , "Feet and Decimal Inches"   , INLIST_DISABLED,  2,  3);
		public static readonly STYLE_DATA FtFracIn           = new STYLE_DATA("FeetFractionalInches", "Feet and Fractional Inches",  2,  3,  4);
		public static readonly STYLE_DATA UsSurvey           = new STYLE_DATA("UsSurveyFeet"        , "Survey Feet"               , INLIST_UNDEFINED, INLIST_UNDEFINED, INLIST_UNDEFINED);
		public static readonly STYLE_DATA Feet               = new STYLE_DATA("Feet"                , "Decimal Feet"              ,  3,  4,  1);
		public static readonly STYLE_DATA DecInches          = new STYLE_DATA("Inches"              , "Decimal Inches"            ,  1,  1,  2);
		public static readonly STYLE_DATA FracInches         = new STYLE_DATA("FractionalInches"    , "Fractional Inches"         ,  0, INLIST_UNDEFINED, INLIST_UNDEFINED);
		public static readonly STYLE_DATA Meters             = new STYLE_DATA("Meters"              , "Meters"                    , INLIST_UNDEFINED, INLIST_UNDEFINED, INLIST_UNDEFINED);
		public static readonly STYLE_DATA Decimeters         = new STYLE_DATA("Decimeters"          , "Decimeters"                , INLIST_UNDEFINED, INLIST_UNDEFINED, INLIST_UNDEFINED);
		public static readonly STYLE_DATA Centimeters        = new STYLE_DATA("CentiMeters"         , "CentiMeters"               , INLIST_UNDEFINED, INLIST_UNDEFINED, INLIST_UNDEFINED);
		public static readonly STYLE_DATA Millimeters        = new STYLE_DATA("MilliMeters"         , "MilliMeters"               , INLIST_UNDEFINED, INLIST_UNDEFINED, INLIST_UNDEFINED);
		public static readonly STYLE_DATA MetersCentimeters  = new STYLE_DATA("MetersCentimeters"   , "Meters and Centimeters"    , INLIST_UNDEFINED, INLIST_UNDEFINED, INLIST_UNDEFINED);


		// public static readonly STYLE_DATA[] STYLE_INFO;
		//
		// private static void config()
		// {
		// 	STYLE_INFO[(int) UnitType.UT_INVALID] = new STYLE_DATA("asf", "asdf", -1, -1, -1);
		// }


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

		public static Dictionary<string, UStyle> StdSysStyles { get; private set; }

		public static void Initialize()
		{

			StdSysStyles = new Dictionary<string, UStyle>();

			// 0
			// project style
			StdSysStyles.Add(STYLE_DATA.Project.NameId, ProjStyle());

			addStandard();

			// UStyle us = CtrlStyle(STYLE_DATA.Control01.NameId, "Show Selected Saved Style", -1);

			// StdSysStyles.Add(us.Name, us);
		}

		// Show Selected Saved Style 
		// Show System Style| {}


		private static void addStandard()
		{
			// the below also identifies whether a 
			// formatting option is allowed to be set
			// null = cannot be set

			string id;
			string name;

			// 1
			// id = STYLE_ID_FT_DEC_IN;
			name = "Feet and Decimal Inches";
			StdSysStyles.Add(STYLE_DATA.FtDecIn.NameId, 
				new UStyle(UnitClass.CL_FT_DEC_IN, // locked
				STYLE_DATA.FtDecIn.TypeId,
				STYLE_DATA.FtDecIn.NameId,
				$"{name} Unit Style", 
				UnitCat.UC_FT_IN_DEC, 
				UnitSys.US_IMPERIAL,
				0.001, 
				// nothing allowed
				null,
				true, false, null, true, false,
				STYLE_DATA.FtDecIn.InRibbonPos, 
				STYLE_DATA.FtDecIn.InDlgLeftPos, 
				STYLE_DATA.FtDecIn.InDlgRightPos, 
				22.3455,
				"Delux Measure ft-dec-in 32.png"));


			// 2
			// id = STYLE_ID_FT_FRAC_IN;
			name = "Feet and Fractional Inches";
			StdSysStyles.Add(STYLE_DATA.FtFracIn.NameId, new UStyle(UnitClass.CL_ORDINARY, // locked
				STYLE_DATA.FtFracIn.TypeId,
				STYLE_DATA.FtFracIn.NameId,
				$"{name} Unit Style", 
				UnitCat.UC_FT_IN_FRAC, 
				UnitSys.US_IMPERIAL,
				1.0 / 64.0, 
				// nothing allowed
				null,
				null, true, null, true, true,
				STYLE_DATA.FtFracIn.InRibbonPos, 
				STYLE_DATA.FtFracIn.InDlgLeftPos, 
				STYLE_DATA.FtFracIn.InDlgRightPos, 
				11.2345,
				"Delux Measure ft-frac-in 32.png"));


			// 3
			// same as decimal feet
			// id = STYLE_ID_US_SURVEY;
			name = "Survey Feet";
			StdSysStyles.Add(STYLE_DATA.UsSurvey.NameId, 
				new UStyle(UnitClass.CL_ORDINARY, 
				STYLE_DATA.UsSurvey.TypeId,
				STYLE_DATA.UsSurvey.NameId,
				$"{name} Unit Style", 
				UnitCat.UC_DECIMAL, 
				UnitSys.US_IMPERIAL,
				0.001, 
				// "USft" or "" (use "none")
				"none",
				true, null, null, true, null,
				STYLE_DATA.UsSurvey.InRibbonPos, 
				STYLE_DATA.UsSurvey.InDlgLeftPos, 
				STYLE_DATA.UsSurvey.InDlgRightPos, 
				33.4567,
				"Delux Measure us-ft 32.png"));

			// 4
			// decimal feet
			// id = STYLE_ID_FEET;
			name = "Decimal Feet";
			StdSysStyles.Add(STYLE_DATA.Feet.NameId, new UStyle(UnitClass.CL_ORDINARY,
				STYLE_DATA.Feet.TypeId,
				STYLE_DATA.Feet.NameId,
				$"{name} Unit Style", 
				UnitCat.UC_DECIMAL, 
				UnitSys.US_IMPERIAL,
				0.0001, 
				// ' or ft or LF or "" (use "none")
				"'", 
				true, null, null, true, null,
				STYLE_DATA.Feet.InRibbonPos, 
				STYLE_DATA.Feet.InDlgLeftPos, 
				STYLE_DATA.Feet.InDlgRightPos, 
				44.5678,
				"Delux Measure dec-ft 32.png"));

			// 5
			// decimal inches
			// id = STYLE_ID_DEC_INCHES;
			name = "Decimal Inches";
			StdSysStyles.Add(STYLE_DATA.DecInches.NameId, new UStyle(UnitClass.CL_ORDINARY,
				STYLE_DATA.DecInches.TypeId,
				STYLE_DATA.DecInches.NameId,
				$"{name} Unit Style", 
				UnitCat.UC_IN_DEC, 
				UnitSys.US_IMPERIAL,
				0.0001, 
				// " or in or ""
				"\"",
				true, null, null, true, null,
				STYLE_DATA.DecInches.InRibbonPos, 
				STYLE_DATA.DecInches.InDlgLeftPos, 
				STYLE_DATA.DecInches.InDlgRightPos, 
				55.6789,
				"Delux Measure dec-in 32.png"));

			// 6
			// fractional inches
			// id = STYLE_ID_FRAC_INCHES;
			name = "Fractional Inches";
			StdSysStyles.Add(STYLE_DATA.FracInches.NameId, new UStyle(UnitClass.CL_ORDINARY,
				STYLE_DATA.FracInches.TypeId,
				STYLE_DATA.FracInches.NameId,
				$"{name} Unit Style", 
				UnitCat.UC_IN_FRAC, 
				UnitSys.US_IMPERIAL,
				1.0/256, 
				// nothing allowed
				null,
				null, null, null, true, null,
				STYLE_DATA.FracInches.InRibbonPos, 
				STYLE_DATA.FracInches.InDlgLeftPos, 
				STYLE_DATA.FracInches.InDlgRightPos, 
				66.7891,
				"Delux Measure frac-in 32.png"));

			// METRIC styles

			// 7
			// meters
			// id = STYLE_ID_METERS;
			name = "Meters";
			StdSysStyles.Add(STYLE_DATA.Meters.NameId, new UStyle(UnitClass.CL_ORDINARY,
				STYLE_DATA.Meters.TypeId,
				STYLE_DATA.Meters.NameId,
				$"{name} Unit Style", 
				UnitCat.UC_DECIMAL, 
				UnitSys.US_METRIC,
				2.0, 
				// "m" or ""
				"m",
				true, null, null, true, null,
				STYLE_DATA.Meters.InRibbonPos, 
				STYLE_DATA.Meters.InDlgLeftPos, 
				STYLE_DATA.Meters.InDlgRightPos, 
				77.8912,
				"Delux Measure m 32.png"));

			// 8
			// decimeters
			// id = STYLE_ID_DECIMETERS;
			name = "Decimeters";
			StdSysStyles.Add(STYLE_DATA.Decimeters.NameId, new UStyle(UnitClass.CL_ORDINARY,
				STYLE_DATA.Decimeters.TypeId,
				STYLE_DATA.Decimeters.NameId,
				$"{name} Unit Style", 
				UnitCat.UC_DECIMAL, 
				UnitSys.US_METRIC,
				10.0, 
				// "dm" or ""
				"dm",
				true, null, null, true, null,
				STYLE_DATA.Decimeters.InRibbonPos, 
				STYLE_DATA.Decimeters.InDlgLeftPos, 
				STYLE_DATA.Decimeters.InDlgRightPos, 
				88.9123,
				"Delux Measure dm 32.png"));

			// 9
			// centimeters
			// id = STYLE_ID_CENTIMETERS;
			name = "Centimeters";
			StdSysStyles.Add(STYLE_DATA.Centimeters.NameId, new UStyle(UnitClass.CL_ORDINARY,
				STYLE_DATA.Centimeters.TypeId,
				STYLE_DATA.Centimeters.NameId,
				$"{name} Unit Style", 
				UnitCat.UC_DECIMAL, 
				UnitSys.US_METRIC,
				0.1, 
				// "cm" or "" 
				"cm",
				true, null, null, true, null,
				STYLE_DATA.Centimeters.InRibbonPos, 
				STYLE_DATA.Centimeters.InDlgLeftPos, 
				STYLE_DATA.Centimeters.InDlgRightPos, 
				99.1234,
				"Delux Measure cm 32.png"));

			// 10
			// millimeters
			// id = STYLE_ID_MILLIMETERS;
			name = "Millimeters";
			StdSysStyles.Add(STYLE_DATA.Millimeters.NameId, new UStyle(UnitClass.CL_ORDINARY,
				STYLE_DATA.Millimeters.TypeId,
				STYLE_DATA.Millimeters.NameId,
				$"{name} Unit Style", 
				UnitCat.UC_DECIMAL, 
				UnitSys.US_METRIC,
				0.01, 
				// "mm" or ""
				"mm",
				true, null, null, true, null,
				STYLE_DATA.Millimeters.InRibbonPos, 
				STYLE_DATA.Millimeters.InDlgLeftPos, 
				STYLE_DATA.Millimeters.InDlgRightPos, 
				110.2345,
				"Delux Measure mm 32.png"));

			// 11
			// Meters-Centimeters
			// id = STYLE_ID_METERS_CENTIMETERS;
			name = "Meters and Centimeters";
			StdSysStyles.Add(STYLE_DATA.MetersCentimeters.NameId, new UStyle(UnitClass.CL_ORDINARY,
				STYLE_DATA.MetersCentimeters.TypeId,
				STYLE_DATA.MetersCentimeters.NameId,
				$"{name} Unit Style", 
				UnitCat.UC_METER_CM, 
				UnitSys.US_METRIC,
				0.001, 
				// nothing allowed
				null,
				null, null, null, true, null,
				STYLE_DATA.MetersCentimeters.InRibbonPos, 
				STYLE_DATA.MetersCentimeters.InDlgLeftPos, 
				STYLE_DATA.MetersCentimeters.InDlgRightPos, 
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
				STYLE_DATA.Project.TypeId,            // ref
				STYLE_DATA.Project.NameId,            // ref
				"Current Project Units",            // ref
				UnitCat.UC_FT_IN_FRAC, // ref
				UnitSys.US_IMPERIAL,   // ref
				1.0 / 64.0,                         // ref
				null,                               // ref
				null,                               // ref
				true,                               // ref
				null,                               // ref
				true,                               // ref
				true,                               // ref
				STYLE_DATA.Project.InRibbonPos, 
				STYLE_DATA.Project.InDlgLeftPos, 
				STYLE_DATA.Project.InDlgRightPos, 
				112.4567,                           // this
				"Delux Measure dec-ft 32.png");
				
		}
		
		// public static UStyle CtrlStyle(string name, string desc, int order)
		// {
		// 	return new UStyle(                      // which from this or reference
		// 		UnitClass.CL_CONTROL,               // this
		// 		STYLE_DATA.Control01.TypeId,          // ref
		// 		name,                               // ref
		// 		desc,                               // ref
		// 		UnitCat.UC_NONE,                    // ref
		// 		UnitSys.US_IMPERIAL,                // ref
		// 		1,                                  // ref
		// 		null,                               // ref
		// 		null,                               // ref
		// 		null,                               // ref
		// 		null,                               // ref
		// 		null,                               // ref
		// 		null,                               // ref
		// 		order,                              // this
		// 		-1,                                 // this
		// 		-1,                                 // this
		// 		1,                                  // this
		// 		null);
		// 		
		// }

		




	}
}