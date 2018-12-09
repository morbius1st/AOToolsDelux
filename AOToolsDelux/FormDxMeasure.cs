using System;
using System.Windows.Forms;
using AOTools.Utility;
using Autodesk.Revit.DB;
using Form = System.Windows.Forms.Form;
using Point = System.Drawing.Point;

using static AOTools.Utility.Util;
using static UtilityLibrary.MessageUtilities2;

using static AOTools.AppSettings.ConfigSettings.SettingsUsr;

using AOTools.UnitStyles;

namespace AOTools
{

	public partial class FormDxMeasure : Form
	{
		private UnitStyleType _utStyle = UnitStyleType.PROJECT;

		private Units units;

		private PointMeasurements? _pm;

		public FormDxMeasure()
		{
			InitializeComponent();
			LoadSettings();
			tbxMessage.Text = "";
		}

		private void FormDxMeasure_FormClosing(object sender,
			FormClosingEventArgs e)
		{
			SmUsrSetg.FormMeasurePointsLocation  = this.Location;
			SmUsrSetg.MeasurePointsShowWorkplane = DxMeasure.ShowWorkplane;
			SmUsrSetg.DxMeasureUnitStyle = _utStyle;
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

			ConfigUnitRadioButtons();

			SetUnits(_utStyle);
		}


		private void cbxWpOnOff_CheckedChanged(object sender,
			EventArgs e)
		{
			DxMeasure.ShowWorkplane = cbxWpOnOff.Checked;

			DxMeasure.ShowHideWorkplane();
		}


		private void rbUnit_Click(object sender, EventArgs e)
		{
			RadioButton rb = (RadioButton) sender;

			SetUnits((UnitStyleType) rb.Tag);
		}

		private void SetUnits(UnitStyleType utStyle)
		{
			_utStyle = utStyle;

			if (_utStyle != UnitStyleType.FEET_DEC_IN)
			{
				units = UnitStylesDefault.StandardUnitStyle(DxMeasure._doc, _utStyle);
			}

			UpdatePoints();
		}

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
			if (_pm == null || (units == null && _utStyle != UnitStyleType.FEET_DEC_IN) ) return;

			lblP1X.Text = FormatLength(_pm.Value.P1.X);
			lblP1Y.Text = FormatLength(_pm.Value.P1.Y);
			lblP1Z.Text = FormatLength(_pm.Value.P1.Z);

			lblP2X.Text = FormatLength(_pm.Value.P2.X);
			lblP2Y.Text = FormatLength(_pm.Value.P2.Y);
			lblP2Z.Text = FormatLength(_pm.Value.P2.Z);

			lblDistX.Text = FormatLength(_pm.Value.delta.X);
			lblDistY.Text = FormatLength(_pm.Value.delta.Y);
			lblDistZ.Text = FormatLength(_pm.Value.delta.Z);

			lblDistXY.Text = FormatLength(_pm.Value.distanceXY);
			lblDistXZ.Text = FormatLength(_pm.Value.distanceXZ);
			lblDistYZ.Text = FormatLength(_pm.Value.distanceYZ);

			lblDistXYZ.Text = FormatLength(_pm.Value.distanceXYZ);

//
//			string test = UtilityLibrary.CsConversions.FromDoubleFeet.
//					ToFeetAndDecimalInches(0.5, 0.001, true);
//
//			test = UtilityLibrary.CsConversions.FromDoubleFeet.ToFeetAndDecimalInches(6.1 / 12.0, 0.00001, true);
//
//			test = UtilityLibrary.CsConversions.FromDoubleFeet.ToFeetAndDecimalInches(0.51111, 0.001, true);
//			
//			test = UtilityLibrary.CsConversions.FromDoubleFeet.ToFeetAndDecimalInches(0.5, 0.001, false);
//
//			test = UtilityLibrary.CsConversions.FromDoubleFeet.ToFeetAndDecimalInches(0.51111, 0.001, false);
//
//			test = UtilityLibrary.CsConversions.FromDoubleFeet.ToFeetAndDecimalInches(6.1 / 12, 0.001, false);



		}

		private string FormatLength(double length)
		{
			if (_utStyle == UnitStyleType.FEET_DEC_IN)
			{
				return UtilityLibrary.CsConversions.FromDoubleFeet.ToFeetAndDecimalInches(length, 0.0001, false);
			}
			else if (_utStyle == UnitStyleType.FRACT_FT)
			{

				return UnitFormatUtils.Format(units, UnitType.UT_Length, length / 12, false, false).Replace('\"', '\'');
			}

			return UnitFormatUtils.Format(units, UnitType.UT_Length, length, false, false);
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

			lblDistXY.Text = "";
			lblDistXZ.Text = "";
			lblDistYZ.Text = "";

			lblDistXYZ.Text = "";
		}

		private void ConfigUnitRadioButtons()
		{
			rbUnitsProj.Tag = UnitStyleType.PROJECT;
			rbUnitsProj.Checked = _utStyle == UnitStyleType.PROJECT;

			rbUnitDecFt.Tag = UnitStyleType.DEC_FT;
			rbUnitDecFt.Checked = _utStyle == UnitStyleType.DEC_FT;

			rbUnitDecIn.Tag = UnitStyleType.DEC_IN;
			rbUnitDecIn.Checked = _utStyle == UnitStyleType.DEC_IN;

			rbUnitFracIn.Tag = UnitStyleType.FRAC_IN;
			rbUnitFracIn.Checked = _utStyle == UnitStyleType.FRAC_IN;

			rbUnitFtFracIn.Tag = UnitStyleType.FEET_FRAC_IN;
			rbUnitFtFracIn.Checked = _utStyle == UnitStyleType.FEET_FRAC_IN;

			rbUnitFractFt.Tag = UnitStyleType.FRACT_FT;
			rbUnitFractFt.Checked = _utStyle == UnitStyleType.FRACT_FT;

			rbUnitFtDecIn.Tag = UnitStyleType.FEET_DEC_IN;
			rbUnitFtDecIn.Checked = _utStyle == UnitStyleType.FEET_DEC_IN;
		}

	}
}
