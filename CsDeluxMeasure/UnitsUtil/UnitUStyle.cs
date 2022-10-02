using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using static CsDeluxMeasure.UnitsUtil.UnitData;
using CsDeluxMeasure.UnitsUtil;

// Solution:     AOToolsDelux
// Project:       CsDeluxMeasure
// File:             UnitUStyle.cs
// Created:      2022-03-27 (7:21 AM)


namespace CsDeluxMeasure.UnitsUtil
{
	[DataContract(Namespace = "")]
	public class UStyle : INotifyPropertyChanged
	{
		public static int[] InListMaxIdx { get; set; } = new [] { 100, 100, 100 };

		public static readonly string[] INLIST_PROP_NAMES = new []
		{
			$"Ustyle.{nameof(OrderInRibbon)}",
			$"Ustyle.{nameof(OrderInDialogLeft)}",
			$"Ustyle.{nameof(OrderInDialogRight)}",
		};

		private string name;
		private string desc;
		private double? sample;
		private bool modified;

		protected UStyle() {}

		public UStyle(
			UnitClass uClass,
			string id,
			string name,
			string description,
			UnitCat uCat,
			UnitSys uSys,
			double precision, 
			// string symbol,
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
			// Symbol = symbol;

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

			modified = false;
		}

		[OnDeserialized]
		void OnDeserialized(StreamingContext context)
		{
			modified = false;
		}

		[DataMember(Order = 2)]
		public UnitClass UnitClass { get; set; }

		[DataMember(Order = 4)]
		public string Id { get; set; }

		[DataMember(Order = 6)]
		public string Name
		{
			get => name;
			set
			{
				if (value == name) return;
				name = value;

				OnPropertyChanged();
			}
		}

		[DataMember(Order = 8)]
		public string Description
		{
			get => desc;
			set
			{
				if (value == desc) return;
				desc = value;
				OnPropertyChanged();
			}
		}
		
		[DataMember(Order = 14)]
		public UnitCat UnitCat { get; set; }

		[DataMember(Order = 14)]
		public UnitSys UnitSys { get; set; }

		// [DataMember(Order = 16)]
		// public string Symbol { get; set; }

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
		public int[] OrderInList { get; set; }

		
		// [IgnoreDataMember]
		// public bool IsModified
		// {
		// 	// not utilized at this point - very complicated to implement
		//  // thing is, would need to be a count of the changes in order to
		//  // properly determine if the record turely has no changes
		// 	get => modified;
		// 	set
		// 	{
		// 		if (modified == value) return;
		//
		// 		modified = value;
		// 		OnPropertyChanged();
		// 	}
		// }

		[IgnoreDataMember]
		public bool IsLocked => (UnitClass < UnitClass.CL_ORDINARY);

		[IgnoreDataMember]
		public bool IsControl => UnitClass.Equals(UnitClass.CL_CONTROL);
		
		[IgnoreDataMember]
		public bool IsFtDecInch => UnitClass.Equals(UnitClass.CL_FT_DEC_IN);

		[IgnoreDataMember]
		public int OrderInRibbon
		{
			get => OrderInList[(int) InList.RIBBON];
			set
			{
				OrderInList[(int) InList.RIBBON] = value;
			}
		}

		[IgnoreDataMember]
		public int OrderInDialogLeft
		{
			get => OrderInList[(int) InList.DIALOG_LEFT];
			set
			{
				OrderInList[(int) InList.DIALOG_LEFT] = value;
				// OnChkBxPropertyChanged();
			}
		}

		[IgnoreDataMember]
		public int OrderInDialogRight
		{
			get => OrderInList[(int) InList.DIALOG_RIGHT];
			set
			{
				OrderInList[(int) InList.DIALOG_RIGHT] = value;
				// OnChkBxPropertyChanged();
			}
		}

		[IgnoreDataMember]
		public bool ShowInRibbon
		{
			get => OrderInRibbon > INLIST_UNDEFINED;
			set
			{
				if (value == ShowInRibbon) return;

				CheckBoxChangedEventArgs e;

				if (value)
				{
					e = new CheckBoxChangedEventArgs(InList.RIBBON);
				}
				else
				{
					e = new CheckBoxChangedEventArgs(null);
				}

				RaiseShowInChangedEvent(e);

				OrderInRibbon = e.InListOrder;

				OnPropertyChanged();
			}
		}

		[IgnoreDataMember]
		public bool ShowInDialogLeft
		{
			get => OrderInDialogLeft > INLIST_UNDEFINED;
			set
			{
				if (value == ShowInDialogLeft) return;

				CheckBoxChangedEventArgs e;

				if (value)
				{
					e = new CheckBoxChangedEventArgs(InList.DIALOG_LEFT);
				}
				else
				{
					e = new CheckBoxChangedEventArgs(null);
				}

				RaiseShowInChangedEvent(e);

				OrderInDialogLeft = e.InListOrder;

				OnPropertyChanged();

			}
		}

		[IgnoreDataMember]
		public bool ShowInDialogRight
		{
			get => OrderInDialogRight > INLIST_UNDEFINED;
			set
			{
				if (value == ShowInDialogRight) return;

				CheckBoxChangedEventArgs e;

				if (value)
				{
					e = new CheckBoxChangedEventArgs(InList.DIALOG_LEFT);
				}
				else
				{
					e = new CheckBoxChangedEventArgs(null);
				}

				RaiseShowInChangedEvent(e);

				OrderInDialogRight = e.InListOrder;

				OnPropertyChanged();

			}
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

		public string FormatOrderValue(int which)
		{
			return $"{OrderInList[which]:D10}";
		}

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
			us.OrderInRibbon = OrderInRibbon;
			us.OrderInDialogLeft = OrderInDialogLeft;
			us.OrderInDialogRight = OrderInDialogRight;
			us.Sample = Sample;
			us.IconId = IconId;

			return us;
		}


		[DebuggerStepThrough]
		private void OnPropertyChanged([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

		public event PropertyChangedEventHandler PropertyChanged;


		[DebuggerStepThrough]
		protected virtual void RaiseShowInChangedEvent(CheckBoxChangedEventArgs e)
		{
			ShowInChanged?.Invoke(this, e);
		}

		public static event UStyle.ShowInChangedEventHandler ShowInChanged;

		public delegate void ShowInChangedEventHandler(object sender, CheckBoxChangedEventArgs e);

	}
}