// Solution:     AOToolsDelux
// Project:       DeluxMeasure
// File:             UnitStyle.cs
// Created:      2022-03-10 (10:48 PM)

using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using Autodesk.Revit.DB;

namespace DeluxMeasure.UnitsUtil
{
	/*
	 revit unit type information
	ForgeTypeId  
	is of (3) types:
	UnitTypeId
	SymbolTypeId
	SpecTypeId

	SpecTypeId:
	unit category such as
	Length, Area, Energy, etc.

	UnitTypeId
	is the unit type such as
	Feet, Inches, Millimeters, etc.



	*/

	/*
	* style locked (cannot delete)
	 rules
	locked is true: == project units: locked / cannot delete / not saved
	locked is null: == project units: "locked" but does not have real information / pseudo entry;
	locked is false: can delete
	 

	 ribbon styles
	* order # & -1 == hide
	 
	Dialog Left
	* order & -1 == hide
	
	Dialog Right
	* order & -1 ==  hide

	*/
/*
	[DataContract(Namespace = "")]
	public class UnitStyle : INotifyPropertyChanged
	{

		private static int ribbonCounter = -4;
		private static int dialogLeftCounter = -3;
		private static int dialogRightCounter = -2;

		private double sample;
		private bool deleteStyle;

		[DataMember(Order = 0)]
		public bool? Locked { get; set; }

		[DataMember(Order = 1)]
		public ForgeTypeId Id { get; set; }

		[DataMember(Order = 2)]
		public string Name { get; set; }

		[DataMember(Order = 3)]
		public string Desc { get; set; }

		[DataMember(Order = 3)]
		public ForgeTypeId Symbol { get; set; }

		[DataMember(Order = 4)]
		public double Precision { get; set; }

		[DataMember(Order = 5)]
		public bool? SuppressLeadZero { get; set; }

		[DataMember(Order = 5)]
		public bool? SuppressTrailZero { get; set; }

		[DataMember(Order = 5)]
		public bool? SuppressSpaces { get; set; }

		[DataMember(Order = 5)]
		public bool? UsePlusPrefix { get; set; }

		[DataMember(Order = 5)]
		public bool? UseDigitGrouping { get; set; }

		[DataMember(Order = 5)]
		public UnitsData.UnitCat UCat { get; set; }

		[DataMember(Order = 5)]
		public UnitsData.UnitSys USys { get; set; }

		[DataMember(Order = 7)]
		public string IconId { get; set; }

		[DataMember(Order = 10)]
		int[] Order { get; set; }

		[IgnoreDataMember]
		public int OrderInRibbon
		{
			get => Order[(int) UnitsData.ListToShowIn.RIBBON];
			set => Order[(int) UnitsData.ListToShowIn.RIBBON] = value;
		}

		[IgnoreDataMember]
		public int OrderInDialogLeft
		{
			get => Order[(int) UnitsData.ListToShowIn.DIALOG_LEFT];
			set => Order[(int) UnitsData.ListToShowIn.DIALOG_LEFT] = value;
		}

		[IgnoreDataMember]
		public int OrderInDialogRight
		{
			get => Order[(int) UnitsData.ListToShowIn.DIALOG_LEFT];
			set => Order[(int) UnitsData.ListToShowIn.DIALOG_RIGHT] = value;
		}


		[DataMember(Order = 12)]
		public double Sample
		{
			get => sample;
			set
			{
				if (sample == value) return;
				sample = value;
				OnPropertyChanged();
			}
		}

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

		public UnitStyle(   ForgeTypeId id, string name, string desc, string precision,
			UnitsData.UnitCat uCat,
			ForgeTypeId symbol, bool suppressTrailZero,
			bool suppressLeadZero, bool usePlusPrefix, bool useDigitGrouping, bool suppressSpaces,
			int orderInRibbon, int orderInDialogLeft, int orderInDialogRight, string iconId)
		{
			double prec = UnitsData.GetPrec(UnitsData.UnitTypes[id].UnitDesc.UCat, precision);
			assign(id, name, desc, UnitsData.UnitCat.DECIMAL, prec, symbol, 
				suppressTrailZero, suppressLeadZero,
				usePlusPrefix, useDigitGrouping, suppressSpaces, 
				orderInRibbon, orderInDialogLeft, orderInDialogRight, iconId);
		}

		public UnitStyle(  ForgeTypeId id, string name, string desc, double precision,
			UnitsData.UnitCat uCat,
			ForgeTypeId symbol, bool suppressTrailZero,
			bool suppressLeadZero, bool usePlusPrefix, bool useDigitGrouping,
			bool suppressSpaces, int orderInRibbon,
			int orderInDialogLeft, int orderInDialogRight, string iconId)
		{
			assign(id, name, desc, UnitsData.UnitCat.DECIMAL, precision, 
				symbol, suppressTrailZero, suppressLeadZero, 
				usePlusPrefix, useDigitGrouping, suppressSpaces, 
				orderInRibbon, orderInDialogLeft, orderInDialogRight, 
				iconId);
		}

		private void assign(ForgeTypeId id, string name, string desc,
			UnitsData.UnitCat uCat, double precision,
			ForgeTypeId symbol, bool suppressTrailZero, bool suppressLeadZero, 
			bool usePlusPrefix, bool useDigitGrouping, bool suppressSpaces, 
			int orderInRibbon, int orderInDialogLeft, int orderInDialogRight,
			string iconId)
		{
			Locked = false;
			Name = name;
			Id = id;
			Desc = desc;
			Symbol = symbol;
			Precision = precision;
			SuppressLeadZero = suppressLeadZero;
			SuppressTrailZero = suppressTrailZero;
			SuppressSpaces = suppressSpaces;
			UsePlusPrefix = usePlusPrefix;
			UseDigitGrouping = useDigitGrouping;
			IconId = iconId;

			Order = new int[3];

			OrderInRibbon = orderInRibbon;
			OrderInDialogLeft = orderInDialogLeft;
			OrderInDialogRight = orderInDialogRight;

			// OrderInRibbon=ribbonCounter++;
			// OrderInDialogLeft=dialogLeftCounter++;
			// OrderInDialogRight=dialogRightCounter++;

			DeleteStyle = false;

			Sample = 10.123456789;
		}

		public UnitStyle() { }

		public UnitStyle(Units u)
		{
			FormatOptions fo = u.GetFormatOptions(SpecTypeId.Length);

			ForgeTypeId utId = fo.GetUnitTypeId();
			string name = UnitsData.UnitTypes[utId].UnitDesc.Name;
			string desc = UnitsData.UnitTypes[utId].UnitDesc.Desc;
			UnitsData.UnitCat uCat = UnitsData.UnitTypes[utId].UnitDesc.UCat;
			ForgeTypeId symbol = fo.GetSymbolTypeId();
			bool supLeadZero = fo.SuppressLeadingZeros;
			bool supTrailZero = fo.SuppressTrailingZeros;
			bool supSpaces = fo.SuppressSpaces;
			bool plusPrefix = fo.UsePlusPrefix;
			bool digitGrp = fo.UseDigitGrouping;

			assign(utId, name, desc, uCat, fo.Accuracy, symbol,
				supTrailZero, supLeadZero, plusPrefix, digitGrp, 
				supSpaces, -1, -1, -1, "information32.png");
		}

		public static UnitStyle GetProjectUnitStyle(Document doc)
		{
			UnitStyle us;

			if (doc == null)
			{
				us = new UnitStyle();
				us.Locked = null;
				us.Id = UnitTypeId.General;
				us.Name = UnitsData.UnitTypes[us.Id].UnitDesc.Name;
				us.Order = new [] { -1, -1, -1 };
			}
			else
			{
				us = new UnitStyle(doc.GetUnits());
				us.Locked = true;
			}

			// us.Name = "Current Project Units";

			return us;
		}
		
		private string GetSymbol()
		{
			if (Symbol == null) return "is null";

			if (Symbol.Empty()) return "None";

			ForgeTypeId id = Symbol;

			string result = LabelUtils.GetLabelForSymbol(id);

			return result;
		}

		
		public string UnitDescription => UnitsData.UnitTypes[Id].UnitDesc.Desc + "(" + LabelUtils.GetLabelForUnit(Id) + ")";
		public string UnitSystem => USys.ToString(); // + "(" + LabelUtils.GetLabelForSpec(Id) + ")";
		public string UnitSymbol => Symbol.ToString() + "(" + GetSymbol() + ")";
		public string UnitPrecision => Precision.ToString();
		public string UnitLeadZero => SuppressLeadZero ? "Suppress" : "Not Suppressed";
		public string UnitTrailZero => SuppressTrailZero ? "Suppress" : "Not Suppressed";
		public string UnitExtraSpaces => SuppressSpaces ? "Show Spaces" : "Hide Spaces";
		public string UnitShowPlus => UsePlusPrefix ? "Yes" : "No";
		public string UnitDigitGrp => UseDigitGrouping ? "Yes" : "No";

		public UnitSystem USystem => (UnitSystem) (int) USys;

		public string SampleFormatted => UnitsManager.Instance.FormatLength(Sample, this);

		public bool ShowInRibbon
		{
			get => OrderInRibbon >= 0;
			set
			{
				OrderInRibbon = value ? 100 : -1;
				OnPropertyChanged();
			}
		}

		public bool ShowInDialogLeft
		{
			get => OrderInDialogLeft >= 0;
			set
			{
				OrderInDialogLeft = value ? 100 : -1;
				OnPropertyChanged();
			}
		}

		public bool ShowInDialogRight
		{
			get => OrderInDialogRight >= 0;
			set
			{
				OrderInDialogRight = value ? 100 : -1;
				OnPropertyChanged();
			}
		}


		public bool ShowIn(int which) => Order[which] >= 0;

		public void UpdateProperties()
		{
			OnPropertyChanged(nameof(UnitDescription));
			OnPropertyChanged(nameof(UnitSymbol));
			OnPropertyChanged(nameof(UnitPrecision));
			OnPropertyChanged(nameof(UnitLeadZero));
			OnPropertyChanged(nameof(UnitTrailZero));
			OnPropertyChanged(nameof(UnitShowPlus));
			OnPropertyChanged(nameof(UnitShowPlus));
			OnPropertyChanged(nameof(UnitDigitGrp));
			OnPropertyChanged(nameof(ShowInRibbon));
			OnPropertyChanged(nameof(ShowInDialogLeft));
			OnPropertyChanged(nameof(ShowInDialogRight));
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}
	}

	*/
}