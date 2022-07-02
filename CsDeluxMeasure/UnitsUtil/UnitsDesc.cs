#region + Using Directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion

// user name: jeffs
// created:   3/25/2022 10:32:35 PM

namespace CsDeluxMeasure.UnitsUtil
{
	public struct UnitDesc
	{
		public string DataId { get; }
		public String Name { get; }
		public string Desc { get; }
		public string IconFile { get; }
		public UnitStyles.UnitCat UCat { get; }

		public UnitDesc(string dataId, string name, string desc, string iconFile, UnitStyles.UnitCat uCat)
		{
			DataId = dataId;
			Name = name;
			Desc = desc;
			IconFile = iconFile;
			UCat = uCat;
		}
	}

	public class UnitsDesc
	{
		public static Dictionary<string, UnitDesc> UnitDescTable { get; private set; }

		static UnitsDesc()
		{
			assignUnitData();
		}
		
		private static void assignUnitData()
		{
			UnitsDesc.UnitDescTable = new Dictionary<string, UnitDesc>(12);

			string dataId;

			dataId = "General";
			UnitsDesc.UnitDescTable.Add(dataId, new UnitDesc(dataId, "Project", "Current Project Units", "information32.png", UnitStyles.UnitCat.DECIMAL));
			
			dataId = "FeetFractionalInches";
			UnitsDesc.UnitDescTable.Add(dataId,
				new UnitDesc(dataId , "Std Feet+Frac In", "Feet and Fractional Inches", "Delux Measure ft-frac-in 32.png", UnitStyles.UnitCat.FT_IN_FRAC));

			dataId = "Custom";
			UnitsDesc.UnitDescTable.Add(dataId,
				new UnitDesc(dataId , "Std Feet+Dec In", "Feet and Decimal Inches", "Delux Measure ft-frac-in 32.png", UnitStyles.UnitCat.FT_IN_DEC));

			dataId = "UsSurveyFeet";
			UnitsDesc.UnitDescTable.Add(dataId,
				new UnitDesc(dataId , "Std Feet Us"    , "Survey Feet"               , "Delux Measure us-ft 32.png", UnitStyles.UnitCat.DECIMAL));

			dataId = "Feet";
			UnitsDesc.UnitDescTable.Add(dataId,
				new UnitDesc(dataId , "Std Dec Feet"   , "Decimal Feet"              , "Delux Measure dec-ft 32.png", UnitStyles.UnitCat.DECIMAL));

			dataId = "Inches";
			UnitsDesc.UnitDescTable.Add(dataId,
				new UnitDesc(dataId , "Std Dec In"     , "Decimal Inches"            , "Delux Measure dec-in 32.png", UnitStyles.UnitCat.DECIMAL));
			dataId = "FractionalInches";
			UnitsDesc.UnitDescTable.Add(dataId,
				new UnitDesc(dataId , "Std Frac In"    , "Fractional Inches"         , "Delux Measure frac-in 32.png", UnitStyles.UnitCat.IN_FRAC));





			dataId = "Meters";
			UnitsDesc.UnitDescTable.Add(dataId,
				new UnitDesc(dataId , "Std Meters"     , "Meters"                    , "Delux Measure m 32.png", UnitStyles.UnitCat.DECIMAL));

			dataId = "Decimeters";
			UnitsDesc.UnitDescTable.Add(dataId,
				new UnitDesc(dataId , "Std Decimeters" , "Decimeters"                , "Delux Measure dm 32.png", UnitStyles.UnitCat.DECIMAL));

			dataId = "Centimeters";
			UnitsDesc.UnitDescTable.Add(dataId,
				new UnitDesc(dataId , "Std Centimeters", "Centimeters"               , "Delux Measure cm 32.png", UnitStyles.UnitCat.DECIMAL));

			dataId = "Millimeters";
			UnitsDesc.UnitDescTable.Add(dataId,
				new UnitDesc(dataId , "Std Millimeters", "Millimeters"               , "Delux Measure mm 32.png", UnitStyles.UnitCat.DECIMAL));

			dataId = "MetersCentimeters";
			UnitsDesc.UnitDescTable.Add(dataId,
				new UnitDesc(dataId , "Std Meters+cm"  , "Meters and Centimeters"    , "Delux Measure m-cm 32.png", UnitStyles.UnitCat.METER_CM));
				
		}

	}
}
