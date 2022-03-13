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
	* style locked (cannot delete)

	 ribbon styles
	* order # & -1 == hide
	 
	Dialog Left
	* order & -1 == hide
	
	Dialog Right
	* order & -1 ==  hide

	*/

	[DataContract(Namespace = "")]
	public class UnitStyle : INotifyPropertyChanged
	{
		private static int ribbonCounter = -4;
		private static int dialogLeftCounter = -3;
		private static int dialogRightCounter = -2;


		public enum ListToShowIn
		{
			RIBBON = 0,
			DIALOG_LEFT = 1,
			DIALOG_RIGHT = 2,
		}

		private double sample;
		private bool deleteStyle;

		[DataMember(Order = 1)]
		public string Name { get; set; }
		[DataMember(Order = 2)]
		public ForgeTypeId Id { get; set; }
		[DataMember(Order = 3)]
		public ForgeTypeId Symbol { get; set; }
		[DataMember(Order = 4)]
		public double Precision { get; set; }
		[DataMember(Order = 5)]
		public bool SuppressLeadZero { get; set; }
		[DataMember(Order = 5)]
		public bool SuppressTrailZero { get; set; }
		[DataMember(Order = 5)]
		public bool SuppressSpaces { get; set; }
		[DataMember(Order = 5)]
		public bool UsePlusPrefix { get; set; }
		[DataMember(Order = 5)]
		public bool UseDigitGrouping { get; set; }

		[DataMember(Order = 6)]
		int[] Order { get; set; }

		[IgnoreDataMember]
		public int OrderInRibbon
		{
			get => Order[(int) ListToShowIn.RIBBON];
			set => Order[(int) ListToShowIn.RIBBON] = value;
		}

		[IgnoreDataMember]
		public int OrderInDialogLeft 
		{
			get => Order[(int) ListToShowIn.DIALOG_LEFT];
			set => Order[(int) ListToShowIn.DIALOG_LEFT] = value;
		}

		[IgnoreDataMember]
		public int OrderInDialogRight		
		{
			get => Order[(int) ListToShowIn.DIALOG_LEFT];
			set => Order[(int) ListToShowIn.DIALOG_RIGHT] = value;
		}


		[DataMember(Order = 5)]
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

		public UnitStyle(  ForgeTypeId id, string name, string precision,
			ForgeTypeId symbol, bool suppressLeadZero, bool suppressTrailZero,
			bool suppressSpaces, bool usePlusPrefix, bool useDigitGrouping, int orderInRibbon, int orderInDialogLeft, int orderInDialogRight) 
		{
			double prec = UnitsData.GetPrec(UnitsData.UnitTypes[id].UCat, precision);
			assign(id, name, prec, symbol, suppressLeadZero, suppressTrailZero, suppressSpaces, 
				usePlusPrefix, useDigitGrouping, orderInRibbon, orderInDialogLeft, orderInDialogRight);
		}

		public UnitStyle(ForgeTypeId id, string name, double precision,
			ForgeTypeId symbol, bool suppressLeadZero, bool suppressTrailZero,
			bool suppressSpaces, bool usePlusPrefix, bool useDigitGrouping, int orderInRibbon, int orderInDialogLeft, int orderInDialogRight) 
		{
			assign(id, name, precision, symbol, suppressLeadZero, suppressTrailZero, 
				suppressSpaces, usePlusPrefix, useDigitGrouping, orderInRibbon, orderInDialogLeft, orderInDialogRight);
		}

		private void assign(  ForgeTypeId id, string name, double precision,
			ForgeTypeId symbol, bool suppressLeadZero, bool suppressTrailZero,
			bool suppressSpaces, bool usePlusPrefix, bool useDigitGrouping, int orderInRibbon, int orderInDialogLeft, int orderInDialogRight)
		{

			Name = name;
			Id = id;
			Symbol = symbol;
			Precision = precision;
			SuppressLeadZero = suppressLeadZero;
			SuppressTrailZero = suppressTrailZero;
			SuppressSpaces = suppressSpaces;
			UsePlusPrefix = usePlusPrefix;
			UseDigitGrouping = useDigitGrouping;

			Order = new int[3];

			OrderInRibbon=orderInRibbon;
			OrderInDialogLeft=orderInDialogLeft;
			OrderInDialogRight=orderInDialogRight;

			OrderInRibbon=ribbonCounter++;
			OrderInDialogLeft=dialogLeftCounter++;
			OrderInDialogRight=dialogRightCounter++;

			DeleteStyle = false;

			Sample = 10.123456789;
		}

		public string UnitDescription => UnitsData.UnitTypes[Id].Desc;
		public string SampleFormatted => UnitsManager.Instance.FormatLength(Sample, this);
		public UnitSystem USys => UnitsData.UnitTypes[Id].USys;
		public UnitStyles.UnitCat UCat => UnitsData.UnitTypes[Id].UCat;

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

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}
	}
}