using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using static CsDeluxMeasure.UnitsUtil.UnitData;


// Solution:     AOToolsDelux
// Project:       CsDeluxMeasure
// File:             UnitUStyle.cs
// Created:      2022-03-27 (7:21 AM)


namespace CsDeluxMeasure.UnitsUtil
{
	[DataContract(Namespace = "")]
	public class UStyle : INotifyPropertyChanged
	{
		public static readonly string[] INLIST_PROP_NAMES = new []
		{
			$"Ustyle.{nameof(OrderInRibbon)}",
			$"Ustyle.{nameof(OrderInDialogLeft)}",
			$"Ustyle.{nameof(OrderInDialogRight)}",
		};

		private double? sample;

		protected UStyle() {}

		public UStyle(
			UnitClass uClass,
			string id,
			string name,
			string description,
			UnitCat uCat,
			UnitSys uSys,
			double precision, string symbol,
			bool? suppressTrailZeros,
			bool? suppressLeadZeros,
			bool? usePlusPrefix,
			bool? useDigitGrouping,
			bool? suppressSpaces,
			int orderInRibbon,
			int orderInDialogLeft,
			int orderInDialogRight,
			double? sample,
			string iconId)
		{
			UnitClass = uClass;
			Id = id;
			UnitSys = uSys;
			Symbol = symbol;

			Name = name;
			Description = description;
			UnitCat = uCat;
			Precision = precision;
			SuppressTrailZeros = suppressTrailZeros;
			SuppressLeadZeros = suppressLeadZeros;
			SuppressSpaces = suppressSpaces;
			UsePlusPrefix = usePlusPrefix;
			UseDigitGrouping = useDigitGrouping;
			IconId = iconId;

			OrderInList = new int[3];
			OrderInRibbon = orderInRibbon;
			OrderInDialogLeft = orderInDialogLeft;
			OrderInDialogRight = orderInDialogRight;

			Sample = sample;
		}

		[DataMember(Order = 2)]
		public UnitClass UnitClass { get; set; }

		[DataMember(Order = 4)]
		public string Id { get; set; }

		[DataMember(Order = 6)]
		public string Name { get; set; }

		[DataMember(Order = 8)]
		public string Description { get; set; }
		
		[DataMember(Order = 14)]
		public UnitCat UnitCat { get; set; }

		[DataMember(Order = 14)]
		public UnitSys UnitSys { get; set; }

		[DataMember(Order = 16)]
		public string Symbol { get; set; }

		[DataMember(Order = 18)]
		public double Precision { get; set; }

		[DataMember(Order = 20)]
		public bool? SuppressTrailZeros { get; set; }

		[DataMember(Order = 20)]
		public bool? SuppressLeadZeros { get; set; }

		[DataMember(Order = 20)]
		public bool? UsePlusPrefix { get; set; }

		[DataMember(Order = 20)]
		public bool? UseDigitGrouping { get; set; }

		[DataMember(Order = 20)]
		public bool? SuppressSpaces { get; set; }

		[DataMember(Order = 20)]
		public double? Sample
		{
			get => sample;
			set
			{
				if (sample.Equals(value)) return;
				sample = value;
				OnPropertyChanged();
			}
		}

		[DataMember(Order = 30)]
		public string IconId { get; set; }

		[DataMember(Order = 30)]
		int[] OrderInList { get; set; }




		[IgnoreDataMember]
		public bool IsLocked => (UnitClass < UnitClass.CL_ORDINARY);

		[IgnoreDataMember]
		public bool IsControl => UnitClass.Equals(UnitClass.CL_CONTROL);

		[IgnoreDataMember]
		public int OrderInRibbon
		{
			get => OrderInList[(int) InList.RIBBON];
			set => OrderInList[(int) InList.RIBBON] = value;
		}

		[IgnoreDataMember]
		public int OrderInDialogLeft
		{
			get => OrderInList[(int) InList.DIALOG_LEFT];
			set => OrderInList[(int) InList.DIALOG_LEFT] = value;
		}

		[IgnoreDataMember]
		public int OrderInDialogRight
		{
			get => OrderInList[(int) InList.DIALOG_RIGHT];
			set => OrderInList[(int) InList.DIALOG_RIGHT] = value;
		}

		[IgnoreDataMember]
		public bool ShowInRibbon
		{
			get => OrderInRibbon > INLIST_UNDEFINED;
			set => OrderInRibbon = value ? 100 : INLIST_UNDEFINED;
		}

		[IgnoreDataMember]
		public bool ShowInDialogLeft
		{
			get => OrderInDialogLeft > INLIST_UNDEFINED;
			set => OrderInDialogLeft = value ? 100 : INLIST_UNDEFINED;
		}

		[IgnoreDataMember]
		public bool ShowInDialogRight
		{
			get => OrderInDialogRight > INLIST_UNDEFINED;
			set => OrderInDialogRight = value ? 100 : INLIST_UNDEFINED;
		}

		[IgnoreDataMember]
		public bool InRibbonEnabled
		{
			get => OrderInRibbon != INLIST_DISABLED;
		}

		[IgnoreDataMember]
		public bool InDialogLeftEnabled
		{
			get => OrderInDialogLeft != INLIST_DISABLED;
		}

		[IgnoreDataMember]
		public bool InDialogRightEnabled
		{
			get => OrderInDialogRight != INLIST_DISABLED;
		}

		public bool ShowIn(int which) => OrderInList[which] >= 0;

		public void UpdateProperties()
		{
			OnPropertyChanged(nameof(Description));
			OnPropertyChanged(nameof(ShowInRibbon));
			OnPropertyChanged(nameof(ShowInDialogLeft));
			OnPropertyChanged(nameof(ShowInDialogRight));
			OnPropertyChanged(nameof(InRibbonEnabled));
			OnPropertyChanged(nameof(InDialogLeftEnabled));
			OnPropertyChanged(nameof(InDialogRightEnabled));
		}



		public UStyle Clone()
		{
			UStyle us = new UStyle();
			us.UnitClass = UnitClass;
			us.Id = Id;
			us.Name = Name;
			us.Description = Description;
			us.UnitCat = UnitCat;
			us.UnitSys = UnitSys;
			us.Precision = Precision;
			us.SuppressTrailZeros = SuppressTrailZeros;
			us.SuppressLeadZeros = SuppressLeadZeros;
			us.UsePlusPrefix = UsePlusPrefix;
			us.UseDigitGrouping = UseDigitGrouping;
			us.SuppressSpaces = SuppressSpaces;
			us.OrderInList = new int[3];
			us.OrderInRibbon = -1;
			us.OrderInDialogLeft = -1;
			us.OrderInDialogRight = -1;
			us.Sample = Sample;
			us.IconId = IconId;

			return us;
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}
	}
}