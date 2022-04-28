using System;
using System.Collections.Generic;
using System.ComponentModel;

using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Windows.Data;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Autodesk.Revit.DB;
using UtilityLibrary;
using static DeluxMeasure.UnitsUtil.UnitsStdUStyles;

// Solution:     AOToolsDelux
// Project:       DeluxMeasure
// File:             UnitStdStyles.cs
// Created:      2022-03-27 (7:23 AM)

namespace DeluxMeasure.UnitsUtil
{
#region data types


	[DataContract(Namespace = "")]
	public abstract class AUnitsData<T, U> : INotifyPropertyChanged
	{
		private bool unitLeadZeroEnable;
		private bool unitTrailZeroEnable;
		private bool unitExtraSpacesEnable;
		private bool unitShowPlusEnable;
		private bool unitDigitGrpEnable;

		private int sequence;

		private bool deleteStyle = false;

		[DataMember(Order = 1)]
		public int Sequence
		{
			get => sequence;
			set
			{
				sequence = value;
				OnPropertyChanged();
				OnPropertyChanged(nameof(SeqFormatted));
			}
		}

		[IgnoreDataMember]
		public int InitialSequence { get; set; }

			[DataMember(Order = 2)]
		public abstract T Id { get; set; }

		[DataMember(Order = 4)]
		public abstract T Symbol { get; set;  }

		[IgnoreDataMember]
		public abstract U USystem { get; }

		[DataMember(Order = 8)]
		public abstract UStyle Ustyle { get; set; }

		[IgnoreDataMember]
		public bool DeleteStyle
		{
			get => deleteStyle;
			set
			{
				if (deleteStyle == value) return;
				deleteStyle = value;
				OnPropertyChanged();
			}
		}

		// public string UnitSystem      => "* Imperial";

		// public string UnitPrecision   => Ustyle.Precision.ToString();

		public string UnitLeadZero
		{
			get
			{
				bool status = true;
				string result = convertFmtOpt(Ustyle.SuppressLeadZeros ,
					"Yes", "No", "Not Available", ref status);
				UnitLeadZeroEnable = status;
				return result;
			}
		}

		public bool UnitLeadZeroEnable
		{
			get => unitLeadZeroEnable;
			set
			{
				unitLeadZeroEnable = value;
				OnPropertyChanged();
			}
		}

		public string UnitTrailZero
		{
			get
			{
				bool status = true;
				string result = convertFmtOpt(Ustyle.SuppressTrailZeros ,
					"Yes", "No",  "Not Available", ref status);
				UnitTrailZeroEnable = status;
				return result;
			}
		}

		public bool UnitTrailZeroEnable
		{
			get => unitTrailZeroEnable;
			set
			{
				unitTrailZeroEnable = value;
				OnPropertyChanged();
			}
		}

		public string UnitExtraSpaces
		{
			get
			{
				bool status = true;
				string result = convertFmtOpt(Ustyle.SuppressSpaces ,
					"Yes", "No",  "Not Applicable", ref status);
				UnitExtraSpacesEnable = status;
				return result;
			}
		}

		public bool UnitExtraSpacesEnable
		{
			get => unitExtraSpacesEnable;
			set
			{
				unitExtraSpacesEnable = value;
				OnPropertyChanged();
			}
		}

		public string UnitShowPlus
		{
			get
			{
				bool status = true;
				string result = convertFmtOpt(Ustyle.UsePlusPrefix ,
					"Yes", "No", "Not Applicable", ref status);
				UnitShowPlusEnable = status;
				return result;
			}
		}

		public bool UnitShowPlusEnable
		{
			get => unitShowPlusEnable;
			set
			{
				unitShowPlusEnable = value;
				OnPropertyChanged();
			}
		}

		public string UnitDigitGrp
		{
			get
			{
				bool status = true;
				string result = convertFmtOpt(Ustyle.UseDigitGrouping ,
					"Yes", "No",  "Not Available", ref status);
				UnitDigitGrpEnable = status;
				return result;
			}
		}

		public bool UnitDigitGrpEnable
		{
			get => unitDigitGrpEnable;
			set
			{
				unitDigitGrpEnable = value;
				OnPropertyChanged();
			}
		}

		public bool Skip()
		{
			return deleteStyle;
		}


		public string SeqFormatted => (sequence + 1).ToString("00");

		public string UnitSymbolFormated => formatSymbol();
		protected abstract string formatSample();

		public string UnitPrecisionFormatted => formatPrecision();
		protected abstract string formatPrecision();

		public string UnitSampleFormatted => formatSample();
		protected abstract string formatSymbol();

		private string convertFmtOpt(bool? opt, string whenTrue,
			string whenFalse, string whenNull, ref bool enable)
		{
			if (!opt.HasValue)
			{
				enable = false;
				return whenNull;
			}

			return opt.Value ? whenTrue : whenFalse;
		}

		public void UpdateProperties()
		{
			Ustyle.UpdateProperties();

			OnPropertyChanged(nameof(UnitLeadZero));
			OnPropertyChanged(nameof(UnitTrailZero));
			OnPropertyChanged(nameof(UnitShowPlus));
			OnPropertyChanged(nameof(UnitShowPlus));
			OnPropertyChanged(nameof(UnitDigitGrp));
			OnPropertyChanged(nameof(UnitExtraSpaces));
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChanged([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}
	}

	// this is the data map for any type fo sample
	// debug version only  (i.e. ...D
	public class UnitsDataD : AUnitsData<string, string>
	{

		public UnitsDataD(string id, string sample, UStyle us)
		{
			Id = id;
			Sample = sample;
			Ustyle = us;
		}

		[IgnoreDataMember]
		public BitmapImage Ux => CsUtilitiesMedia.GetBitmapImage(Ustyle.IconId, "DeluxMeasure.Resources");

		public override string Id { get; set; }

		public override string Symbol { get; set; }

		private string Sample { get; set; }

		public override string USystem => Ustyle.UnitSys.ToString();

		public override UStyle Ustyle { get; set; }


		protected override string formatSymbol()
		{
			string s = fmtSymbol();

			if (string.IsNullOrWhiteSpace(s)) return "none";

			return s;
		}

		protected string fmtSymbol()
		{
			if (Ustyle.Symbol != null) return Ustyle.Symbol;

			string s = UnitsSupport.GetSymbol(Ustyle.UnitCat);

			return s;
		}

		protected override string formatPrecision()
		{
			string uSym = null;

			if (Ustyle.Precision < 0) return "*invalid";

			if (Ustyle.UnitCat == UnitsSupport.UnitCat.UC_DECIMAL)
			{
				uSym = USystem.Equals("US_METRIC") ? " " : "";
				uSym += fmtSymbol();
			}

			string s = 
				UnitsSupport.GetPrecString(Ustyle.UnitCat, 
					Ustyle.Precision, uSym);

			return s;
		}

		protected override string formatSample()
		{
			return Sample;
		}
	}

	// this is the data map for any type of style
	// release version  (i.e. ...R)
	[DataContract(Namespace = "")]
	public class UnitsDataR : AUnitsData<ForgeTypeId, UnitSystem> , IEquatable<UnitsDataR>
	{
		private ForgeTypeId id;
		private ForgeTypeId symbol;
		private UStyle ustyle;

		private BitmapImage bx;

		public UnitsDataR(ForgeTypeId id,
			ForgeTypeId symbol,
			UStyle us)
		{
			Id = id;
			Symbol = symbol;
			Ustyle = us;

			// bx = new BitmapImage();
			bx = CsUtilitiesMedia.GetBitmapImage(Ustyle.IconId, "DeluxMeasure.Resources");
			// bx.BeginInit();
			// //"pack://application:,,,/AssemblyName;component/Resources/logo.png")
			// bx.UriSource = new Uri("/DeluxMeasure;component/Resources/Delux Measure cm 32.png", UriKind.Absolute);
			// bx.UriSource = new Uri("pack://application:,,,/DeluxMeasure;component/DeluxMeasure/Resources/Delux Measure cm 32.png", UriKind.RelativeOrAbsolute);
			// bx.EndInit();
		}

		[IgnoreDataMember]
		public BitmapImage Ux => CsUtilitiesMedia.GetBitmapImage(Ustyle.IconId, "DeluxMeasure.Resources");

		[DataMember(Order = 2)]
		public override ForgeTypeId Id
		{
			get => id;
			set
			{
				id = value;
				OnPropertyChanged();
			}
		}

		[DataMember(Order = 4)]
		public override ForgeTypeId Symbol
		{
			get => symbol;
			set
			{
				symbol = value;
				OnPropertyChanged();
			}
		}

		[IgnoreDataMember]
		public override UnitSystem USystem => (UnitSystem) (int) Ustyle.UnitSys;

		[DataMember(Order = 20)]
		public override UStyle Ustyle

		{
			get => ustyle;
			set
			{
				ustyle = value;
				OnPropertyChanged();
			}
		}


		protected override string formatSymbol()
		{
			return UnitsSupport.GetSymbol(Symbol, Ustyle.UnitCat);
		}

		protected override string formatPrecision()
		{
			string uSym = null;

			if (Ustyle.Precision < 0) return "*invalid";

			if (Ustyle.UnitCat == UnitsSupport.UnitCat.UC_DECIMAL)
			{
				uSym = USystem == UnitSystem.Metric ? " " : "";
				uSym += formatSymbol();
			}

			string s = 
				UnitsSupport.GetPrecString(Ustyle.UnitCat, 
					Ustyle.Precision, uSym);

			return s;
		}

		protected override string formatSample()
		{
			if (!Ustyle.Sample.HasValue)
			{
				return "Not Set";
				
			}

			string formatted = UnitsSupport.GetSampleFormatted(this, Ustyle.Sample.Value);

			return $"{formatted}  ({Ustyle.Sample.Value:G})";

		}

		public bool Equals(UnitsDataR other)
		{
			if (other == null) return false;
			return  Id.Equals(other.Id);
		}
	}

#endregion

#region data containers

	[DataContract(Namespace = "")]
	public abstract class AUnitsStdStyles<T, U> : INotifyPropertyChanged 
	{
		protected ICollectionView list;
		public abstract Dictionary<T, U> StdStyles { get; protected set; }
		public ICollectionView List { get; set; }

		public static event PropertyChangedEventHandler PropertyChanged_S;

		protected static void OnPropertyChanged_S([CallerMemberName] string memberName = "")
		{
			PropertyChanged_S?.Invoke(null, new PropertyChangedEventArgs(memberName));
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChanged([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}
	}

	[DataContract(Namespace = "")]
	public class UnitStdStylesD : AUnitsStdStyles<string, UnitsDataD>, IEqualityComparer<UnitsDataD>
	{
		static UnitStdStylesD()
		{
			initialize();
		}

		// private new static ICollectionView list;
		private new static List<UnitsDataD> listD;

		public static Dictionary<string, UnitsDataD> SStdStyles { get; protected set; }
		public override Dictionary<string, UnitsDataD> StdStyles { get; protected set; }

		public static List<UnitsDataD> ListD
		{
			get => listD;
			set
			{
				listD = value;
				OnPropertyChanged_S();
			}
		}

		// private void assignSamples()
		// {
		// string[] samples = new []
		// {
		// 	"120'-11 248/255\"", // 0
		// 	"120'-11.987\"",     // 1
		// 	"120.1234'",         // 2
		// 	"120.1234\"",        // 3
		// 	"120 248/255\"",     // 4
		// 	"120.1234m",         // 5
		// 	"120.1234dm",        // 6
		// 	"120.1234cm",        // 7
		// 	"120.1234mm",        // 8
		// 	"120m 1234cm"        // 9
		// };
		// }

		public bool Equals(UnitsDataD x, UnitsDataD y)
		{
			if (x == null || y == null) return false;
			return x.Id.Equals(y.Id);
		}

		public int GetHashCode(UnitsDataD obj)
		{
			return 0;
		}

		private static void addStyleItem(int i, STYLE_ID sid, string sample)
		{
			UnitsDataD udd;

			udd = new UnitsDataD(
				sid.TypeId, sample,
				UnitsStdUStyles.StdStyles[sid.NameId]);
			udd.Sequence = i;
			ListD.Add(udd);
			SStdStyles.Add(udd.Ustyle.Name, udd);
		}

		private static void initialize()
		{
			SStdStyles = new Dictionary<string, UnitsDataD>(12);
			ListD = new List<UnitsDataD>();

			int i = 0;

			addStyleItem(i++, STYLE_ID.Project, "1,123'-11 255/256\"");
			// udd = new UnitsDataD(
			// 	STYLE_ID_PROJECT, "1,123'-11 255/256\"",
			// 	UnitsStdUStyles.StdStyles[STYLE_ID_PROJECT]);
			// udd.Sequence = i++;
			// ListD.Add(udd);
			// SStdStyles.Add(udd.Ustyle.Name, udd);


			addStyleItem(i++, STYLE_ID.FtFracIn, "123'-4 5/8\"");
			// udd = new UnitsDataD(
			// 	STYLE_ID_FT_FRAC_IN, "123'-4 5/8\"",
			// 	UnitsStdUStyles.StdStyles[STYLE_ID_FT_FRAC_IN]);
			// udd.Sequence = i++;
			// ListD.Add(udd);
			//  SStdStyles.Add(udd.Ustyle.Name, udd);


			addStyleItem(i++, STYLE_ID.FtDecIn, "123'-4.625\"");
			// udd = new UnitsDataD(
			// 	STYLE_ID_FT_DEC_IN, "123'-4.625\"",
			// 	UnitsStdUStyles.StdStyles[STYLE_ID_FT_DEC_IN]);
			// udd.Sequence = i++;
			// ListD.Add(udd);
			//  SStdStyles.Add(udd.Ustyle.Name, udd);


			addStyleItem(i++, STYLE_ID.UsSurvey, "123'-4 5/8\"");
			// udd = new UnitsDataD(
			// 	STYLE_ID_US_SURVEY, "123'-4 5/8\"",
			// 	UnitsStdUStyles.StdStyles[STYLE_ID_US_SURVEY]);
			// udd.Sequence = i++;
			//  SStdStyles.Add(udd.Ustyle.Name, udd);


			addStyleItem(i++, STYLE_ID.Feet, "1,234.567'");
			// udd = new UnitsDataD(
			// 	STYLE_ID_FEET, "1,234.567'",
			// 	UnitsStdUStyles.StdStyles[STYLE_ID_FEET]);
			// udd.Sequence = i++;
			// ListD.Add(udd);
			//  SStdStyles.Add(udd.Ustyle.Name, udd);


			addStyleItem(i++, STYLE_ID.DecInches, "123.456\"");
			// udd = new UnitsDataD(
			// 	STYLE_ID_DEC_INCHES, "123.456\"",
			// 	UnitsStdUStyles.StdStyles[STYLE_ID_DEC_INCHES]);
			// udd.Sequence = i++;
			// ListD.Add(udd);
			//  SStdStyles.Add(udd.Ustyle.Name, udd);


			addStyleItem(i++, STYLE_ID.FracInches, "14 129/256\"");
			// udd = new UnitsDataD(
			// 	STYLE_ID_FRAC_INCHES, "14 129/256\"",
			// 	UnitsStdUStyles.StdStyles[STYLE_ID_FRAC_INCHES]);
			// udd.Sequence = i++;
			// ListD.Add(udd);
			//  SStdStyles.Add(udd.Ustyle.Name, udd);


			addStyleItem(i++, STYLE_ID.Meters, "123.456m");
			// udd = new UnitsDataD(
			// 	STYLE_ID_METERS, "123.456m",
			// 	UnitsStdUStyles.StdStyles[STYLE_ID_METERS]);
			// udd.Sequence = i++;
			// ListD.Add(udd);
			//  SStdStyles.Add(udd.Ustyle.Name, udd);


			addStyleItem(i++, STYLE_ID.Decimeters, "123.456dm");
			// udd = new UnitsDataD(
			// 	STYLE_ID_DECIMETERS, "123.456dm",
			// 	UnitsStdUStyles.StdStyles[STYLE_ID_DECIMETERS]);
			// udd.Sequence = i++;
			// ListD.Add(udd);
			//  SStdStyles.Add(udd.Ustyle.Name, udd);


			addStyleItem(i++, STYLE_ID.Centimeters, "123.456cm");
			// udd = new UnitsDataD(
			// 	STYLE_ID_CENTIMETERS, "123.456cm",
			// 	UnitsStdUStyles.StdStyles[STYLE_ID_CENTIMETERS]);
			// udd.Sequence = i++;
			// ListD.Add(udd);
			//  SStdStyles.Add(udd.Ustyle.Name, udd);


			addStyleItem(i++, STYLE_ID.Millimeters, "123.456mm");
			// udd = new UnitsDataD(
			// 	STYLE_ID_MILLIMETERS, "123.456mm",
			// 	UnitsStdUStyles.StdStyles[STYLE_ID_MILLIMETERS]);
			// udd.Sequence = i++;
			// ListD.Add(udd);
			//  SStdStyles.Add(udd.Ustyle.Name, udd);


			addStyleItem(i++, STYLE_ID.MetersCentimeters, "123m 456cm");
			// udd = new UnitsDataD(
			// 	STYLE_ID_METERS_CENTIMETERS, "123m 456cm",
			// 	UnitsStdUStyles.StdStyles[STYLE_ID_METERS_CENTIMETERS]);
			// udd.Sequence = i++;
			// ListD.Add(udd);
			//  SStdStyles.Add(udd.Ustyle.Name, udd);

			// ListD = new List<UnitsDataD>(SStdStyles.Values);

		}

	}

	public class UnitStdStylesR : AUnitsStdStyles<string, UnitsDataR>
	{
		public UnitStdStylesR()
		{
			initialize();
		}

		private new ICollectionView list;

		[DataMember(Order = 2)]
		public override Dictionary<string, UnitsDataR> StdStyles { get; protected set; }
		// public Dictionary<ForgeTypeId, UnitsDataR> StdStyles { get; protected set; }

		[IgnoreDataMember]
		public new ICollectionView List
		{
			get => list;
			set
			{
				list = value;
				OnPropertyChanged_S();
			}
		}

		private void addStyleItem(int i, ForgeTypeId uid, ForgeTypeId sid,	string name) 
		{
			UnitsDataR udr = new UnitsDataR(uid, sid, UnitsStdUStyles.StdStyles[name]);
			udr.Sequence = i++;
			StdStyles.Add(udr.Ustyle.Name, udr);
		}

		private void initialize()
		{
			StdStyles = new Dictionary<string, UnitsDataR>(12);

			int i = 0;

			UnitsDataR udr;

			addStyleItem(i++, UnitTypeId.General, null, STYLE_ID.Project.NameId);


			// udr = new UnitsDataR(UnitTypeId.General, null, UnitsStdUStyles.StdStyles[STYLE_ID_PROJECT]);
			// udr.Sequence = i++;
			// StdStyles.Add(udr.Ustyle.Name, udr);
			//
			addStyleItem(i++, UnitTypeId.FeetFractionalInches, null, STYLE_ID.FtFracIn.NameId);
			//
			// udr = new UnitsDataR(
			// 	UnitTypeId.FeetFractionalInches, null,
			// 	UnitsStdUStyles.StdStyles[STYLE_ID_FT_FRAC_IN]);
			// udr.Sequence = i++;
			// StdStyles.Add(STYLE_ID_FT_FRAC_IN, udr);
			//
			addStyleItem(i++, UnitTypeId.Custom, null, STYLE_ID.FtDecIn.NameId);
			//
			// udr = new UnitsDataR(
			// 	UnitTypeId.Custom, null,
			// 	UnitsStdUStyles.StdStyles[STYLE_ID_FT_DEC_IN]);
			// udr.Sequence = i++;
			// StdStyles.Add(STYLE_ID_FT_DEC_IN, udr);
			//
			addStyleItem(i++, UnitTypeId.UsSurveyFeet, SymbolTypeId.Usft, STYLE_ID.UsSurvey.NameId);
			//
			// udr = new UnitsDataR(
			// 	UnitTypeId.UsSurveyFeet, SymbolTypeId.Usft,
			// 	UnitsStdUStyles.StdStyles[STYLE_ID_US_SURVEY]);
			// udr.Sequence = i++;
			// StdStyles.Add(STYLE_ID_US_SURVEY, udr);
			//
			addStyleItem(i++, UnitTypeId.Feet, SymbolTypeId.Ft, STYLE_ID.Feet.NameId);
			//
			// udr = new UnitsDataR(
			// 	UnitTypeId.Feet, SymbolTypeId.Ft,
			// 	UnitsStdUStyles.StdStyles[STYLE_ID_FEET]);
			// udr.Sequence = i++;
			// StdStyles.Add(STYLE_ID_FEET, udr);
			//
			addStyleItem(i++, UnitTypeId.Inches, SymbolTypeId.InchDoubleQuote, STYLE_ID.DecInches.NameId);
			//
			// udr = new UnitsDataR(
			// 	UnitTypeId.Inches, SymbolTypeId.InchDoubleQuote,
			// 	UnitsStdUStyles.StdStyles[STYLE_ID_DEC_INCHES]);
			// udr.Sequence = i++;
			// StdStyles.Add(STYLE_ID_DEC_INCHES, udr);
			//
			addStyleItem(i++, UnitTypeId.FractionalInches, null, STYLE_ID.FracInches.NameId);
			//
			// udr = new UnitsDataR(
			// 	UnitTypeId.FractionalInches, null,
			// 	UnitsStdUStyles.StdStyles[STYLE_ID_FRAC_INCHES]);
			// udr.Sequence = i++;
			// StdStyles.Add(STYLE_ID_FRAC_INCHES, udr);
			//
			addStyleItem(i++, UnitTypeId.Meters, SymbolTypeId.Meter, STYLE_ID.Meters.NameId);
			//
			// udr = new UnitsDataR(
			// 	UnitTypeId.Meters, SymbolTypeId.Meter,
			// 	UnitsStdUStyles.StdStyles[STYLE_ID_METERS]);
			// udr.Sequence = i++;
			// StdStyles.Add(STYLE_ID_METERS, udr);
			//
			addStyleItem(i++, UnitTypeId.Decimeters, SymbolTypeId.Dm, STYLE_ID.Decimeters.NameId);
			//
			// udr = new UnitsDataR(
			// 	UnitTypeId.Decimeters, SymbolTypeId.Dm,
			// 	UnitsStdUStyles.StdStyles[STYLE_ID_DECIMETERS]);
			// udr.Sequence = i++;
			// StdStyles.Add(STYLE_ID_DECIMETERS, udr);
			//
			addStyleItem(i++, UnitTypeId.Centimeters, SymbolTypeId.Cm, STYLE_ID.Centimeters.NameId);
			//
			// udr = new UnitsDataR(
			// 	UnitTypeId.Centimeters, SymbolTypeId.Cm,
			// 	UnitsStdUStyles.StdStyles[STYLE_ID_CENTIMETERS]);
			// udr.Sequence = i++;
			// StdStyles.Add(STYLE_ID_CENTIMETERS, udr);
			//
			addStyleItem(i++, UnitTypeId.Millimeters, SymbolTypeId.Mm, STYLE_ID.Millimeters.NameId);
			//
			// udr = new UnitsDataR(
			// 	UnitTypeId.Millimeters, SymbolTypeId.Mm,
			// 	UnitsStdUStyles.StdStyles[STYLE_ID_MILLIMETERS]);
			// udr.Sequence = i++;
			// StdStyles.Add(STYLE_ID_MILLIMETERS, udr);
			//
			//
			addStyleItem(i++, UnitTypeId.MetersCentimeters, null, STYLE_ID.MetersCentimeters.NameId);
			//
			// udr = new UnitsDataR(
			// 	UnitTypeId.MetersCentimeters, null,
			// 	UnitsStdUStyles.StdStyles[STYLE_ID_METERS_CENTIMETERS]);
			// udr.Sequence = i++;
			// StdStyles.Add(STYLE_ID_METERS_CENTIMETERS, udr);

			List = CollectionViewSource.GetDefaultView(StdStyles);

			// ICollectionView a = CollectionViewSource.GetDefaultView(StdStyles);
		}
	}

#endregion
}