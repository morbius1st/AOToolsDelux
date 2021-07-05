using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Autodesk.Revit.DB;
using Form = System.Windows.Forms.Form;
using Point = System.Drawing.Point;

using DluxMeasure.Utility;
using DluxMeasure.UnitStyles;
using static DluxMeasure.Utility.Util;
using static DluxMeasure.AppSettings.ConfigSettings.SettingsUsr;


namespace DluxMeasure
{


	public partial class FormDlxMeasure : Form
	{
		private const string primeUnitsName = "PrimeUnits";
		private const string secondUnitsName = "SecondUnits";


		private UnitStyleType _utStyle = UnitStyleType.PROJECT;

		private UnitStyleType _utStyleAlt = UnitStyleType.PROJECT;

		private Units units;
		private Units unitsAlt;

		private UnitDisplay[] utPrime = new UnitDisplay[7];
		
		private UnitDisplay[] utSecond = new UnitDisplay[7];



		private PointMeasurements? _pm;

		public string Message
		{
			get => tbxMessage.Text;

			set
			{
				if (value == null)
				{
					tbxMessage.Text = string.Empty;
				}
				else
				{
					tbxMessage.Text = value;
				}
			}
		}

		public FormDlxMeasure()
		{
			InitializeComponent();
			configureUnitLists();
			LoadSettings();
			tbxMessage.Text = "";
		}

		private void FormDxMeasure_FormClosing(object sender,
			FormClosingEventArgs e)
		{
			SmUsrSetg.FormMeasurePointsLocation  = this.Location;
			SmUsrSetg.MeasurePointsShowWorkplane = DlxMeasure.ShowWorkplane;
			SmUsrSetg.DxMeasureUnitStyle = _utStyle;
			SmUsrSetg.DxMeasureUnitStyleAlt = _utStyleAlt;
			SmUsr.Save();
		}

		private void FormDxMeasure_Load(object sender,
			EventArgs e)
		{
			LoadSettings();
		}

		private void LoadSettings()
		{
			if (SmUsrSetg.FormMeasurePointsLocation.Equals(new Point(0, 0)))
			{
				CenterToParent();
			}
			else
			{
				this.Location = SmUsrSetg.FormMeasurePointsLocation;
			}

			cbxWpOnOff.Checked = SmUsrSetg.MeasurePointsShowWorkplane;

			_utStyle = SmUsrSetg.DxMeasureUnitStyle;
			_utStyleAlt = SmUsrSetg.DxMeasureUnitStyleAlt;

			lbxPrimeUnits.SelectedIndex = (int) _utStyle;
			lbxSecondUnits.SelectedIndex = (int) _utStyleAlt;

			SetUnits(_utStyle);
			SetUnitsAlt(_utStyleAlt);
		}


		private void cbxWpOnOff_CheckedChanged(object sender, EventArgs e)
		{
			DlxMeasure.ShowWorkplane = cbxWpOnOff.Checked;

			if (!DlxMeasure.instance.ShowHideWorkplane())
			{
				Message = "No work plane to show";
			}
		}

		private void lbxSelectedIndexChanged(object sender, EventArgs e)
		{
			ListBox lb = (ListBox) sender;

			UnitDisplay ud = (UnitDisplay) lb.Items[lb.SelectedIndex];

			if (lb.Tag.Equals(primeUnitsName))
			{
				_utStyle = ud.unitType;
				SetUnits(ud.unitType);

				SmUsrSetg.DxMeasureUnitStyle = _utStyle;
			}
			else
			{
				_utStyleAlt = ud.unitType;
				SetUnitsAlt(ud.unitType);

				SmUsrSetg.DxMeasureUnitStyleAlt = _utStyleAlt;
			}

			SmUsr.Save();

//			UpdatePoints();
		}

		private void SetUnits(UnitStyleType utStyle)
		{
			_utStyle = utStyle;

			if (_utStyle != UnitStyleType.FEET_DEC_IN)
			{
				units = UnitStylesDefault.StandardUnitStyle(DlxMeasure._doc, _utStyle);
			}

			UpdatePoints();
		}
		
		private void SetUnitsAlt(UnitStyleType utStyle)
		{
			_utStyleAlt = utStyle;

			if (_utStyleAlt != UnitStyleType.FEET_DEC_IN)
			{
				unitsAlt = UnitStylesDefault.StandardUnitStyle(DlxMeasure._doc, _utStyleAlt);
			}

			UpdatePoints();
		}

		internal void UpdatePoints(PointMeasurements? pm,
			Util.VType vtype,
			string planeName)
		{
			_pm = pm;

			if (pm == null)
			{
				ClearText();

				tbxMessage.Text = "Invalid points selected.  Please pick Two Points, in the same view, to Measure";

				return;
			}

			Message = "View is a " + vtype.VTName;

			if (planeName != null)
			{
				Message += nl + "plane name:" + nl + planeName;
			}

			UpdatePoints();
		}

		private void UpdatePoints()
		{
			if (_pm == null ||
				(units == null && _utStyle != UnitStyleType.FEET_DEC_IN) ||
				(unitsAlt == null && _utStyleAlt != UnitStyleType.FEET_DEC_IN) 
				) return;

			lblP1X.Text     = FormatLength(_pm.Value.P1.X);
			lblP1Y.Text     = FormatLength(_pm.Value.P1.Y);
			lblP1Z.Text     = FormatLength(_pm.Value.P1.Z);
			lblP2X.Text     = FormatLength(_pm.Value.P2.X);
			lblP2Y.Text     = FormatLength(_pm.Value.P2.Y);
			lblP2Z.Text     = FormatLength(_pm.Value.P2.Z);

			lblDistX.Text   = FormatLength(_pm.Value.delta.X);
			lblDistY.Text   = FormatLength(_pm.Value.delta.Y);
			lblDistZ.Text   = FormatLength(_pm.Value.delta.Z);

			tbxDistX2.Text  = FormatLengthAlt(_pm.Value.delta.X);
			tbxDistY2.Text  = FormatLengthAlt(_pm.Value.delta.Y);
			tbxDistZ2.Text  = FormatLengthAlt(_pm.Value.delta.Z);

			lblDistXY.Text  = FormatLength(_pm.Value.distanceXY);
			lblDistXZ.Text  = FormatLength(_pm.Value.distanceXZ);
			lblDistYZ.Text  = FormatLength(_pm.Value.distanceYZ);
			lblDistXYZ.Text = FormatLength(_pm.Value.distanceXYZ);
		}

		private string FormatLength(double length)
		{
			if (_utStyle == UnitStyleType.FEET_DEC_IN)
			{
				return UtilityLibrary.CsConversions.FromDoubleFeet.ToFeetAndDecimalInches(length, 0.0001, false, true);
			}
			else if (_utStyle == UnitStyleType.FRACT_FT)
			{
		
				return UnitFormatUtils.Format(units, UnitType.UT_Length, length / 12, false, false).Replace('\"', '\'');
			}

			return UnitFormatUtils.Format(units, UnitType.UT_Length, length, false, false);
		}

		private string FormatLengthAlt(double length)
		{
			if (_utStyleAlt == UnitStyleType.FEET_DEC_IN)
			{
				return UtilityLibrary.CsConversions.FromDoubleFeet.ToFeetAndDecimalInches(length, 0.0001, false, true);
			}
			else if (_utStyleAlt == UnitStyleType.FRACT_FT)
			{

				return UnitFormatUtils.Format(unitsAlt, UnitType.UT_Length, length / 12, false, false).Replace('\"', '\'');
			}

			return UnitFormatUtils.Format(unitsAlt, UnitType.UT_Length, length, false, false);
		}


		internal void ClearText()
		{

			tbxMessage.Text = "";

			lblP1X.Text = "";
			lblP1Y.Text = "";
			lblP1Z.Text = "";

			lblP2X.Text = "";
			lblP2Y.Text = "";
			lblP2Z.Text = "";

			lblDistX.Text = "";
			lblDistY.Text = "";
			lblDistZ.Text = "";

			tbxDistX2.Text = "";
			tbxDistY2.Text = "";
			tbxDistZ2.Text = "";

			lblDistXY.Text = "";
			lblDistXZ.Text = "";
			lblDistYZ.Text = "";

			lblDistXYZ.Text = "";
		}

		class UnitDisplay
		{
			public string description;
			public UnitStyleType unitType;

			public UnitDisplay(string desc,
				UnitStyleType ut)
			{
				description = desc;
				unitType = ut;
			}

			public override string ToString()
			{
				return description;
			}
		}



		private void configureUnitLists()
		{
//			configUnitListItem("Per Project Units"          , UnitStyleType.PROJECT);
//			configUnitListItem("Feet and Fractional Inches" , UnitStyleType.FEET_FRAC_IN);
//			configUnitListItem("Feet and Decimal Inches"    , UnitStyleType.FEET_DEC_IN);
//			configUnitListItem("Fractional Feet"            , UnitStyleType.FRACT_FT);
//			configUnitListItem("Decimal Feet"               , UnitStyleType.DEC_FT);
//			configUnitListItem("Fractional Inches"          , UnitStyleType.FRAC_IN);
//			configUnitListItem("Decimal Inches"			    , UnitStyleType.DEC_IN);

			foreach (KeyValuePair<int, string> kvp in UnitStylesDefault.UnitStyleDescriptionList)
			{
				configUnitListItem(kvp.Value, (UnitStyleType) kvp.Key);
			}

			lbxPrimeUnits.Tag = primeUnitsName;
			lbxPrimeUnits.SelectedIndexChanged += lbxSelectedIndexChanged;

			lbxSecondUnits.Tag = secondUnitsName;
			lbxSecondUnits.SelectedIndexChanged += lbxSelectedIndexChanged;
		}

		private void configUnitListItem(string desc,
			UnitStyleType ut)
		{
			utPrime[(int) ut] = new UnitDisplay(desc, ut);
			lbxPrimeUnits.Items.Add(utPrime[(int) ut]);

			utSecond[(int) ut] = new UnitDisplay(desc, ut);
			lbxSecondUnits.Items.Add(utPrime[(int) ut]);
		}

	}
}
