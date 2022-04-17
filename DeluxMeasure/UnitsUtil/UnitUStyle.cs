// Solution:     AOToolsDelux
// Project:       DeluxMeasure
// File:             UnitUStyle.cs
// Created:      2022-03-27 (7:21 AM)

using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace DeluxMeasure.UnitsUtil
{
	[DataContract(Namespace = "")]
	public class UStyle : INotifyPropertyChanged
	{
		private double? sample;

		public UStyle() {}

		public UStyle(
			UnitClass uClass,
			string id,
			string name,
			string description,
			UnitsSupport.UnitCat uCat,
			UnitsSupport.UnitSys uSys,
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

			Order = new int[3];
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
		public UnitsSupport.UnitCat UnitCat { get; set; }

		[DataMember(Order = 14)]
		public UnitsSupport.UnitSys UnitSys { get; set; }

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
		int[] Order { get; set; }


		[IgnoreDataMember]
		public bool IsLocked => (UnitClass != UnitClass.CL_ORDINARY);


		[IgnoreDataMember]
		public int OrderInRibbon
		{
			get => Order[(int) UnitsSupport.ListToShowIn.RIBBON];
			set => Order[(int) UnitsSupport.ListToShowIn.RIBBON] = value;
		}

		[IgnoreDataMember]
		public int OrderInDialogLeft
		{
			get => Order[(int) UnitsSupport.ListToShowIn.DIALOG_LEFT];
			set => Order[(int) UnitsSupport.ListToShowIn.DIALOG_LEFT] = value;
		}

		[IgnoreDataMember]
		public int OrderInDialogRight
		{
			get => Order[(int) UnitsSupport.ListToShowIn.DIALOG_RIGHT];
			set => Order[(int) UnitsSupport.ListToShowIn.DIALOG_RIGHT] = value;
		}

		[IgnoreDataMember]
		public bool ShowInRibbon
		{
			get => OrderInRibbon >= 0;
			set => OrderInRibbon = value ? 100 : -1;
		}

		[IgnoreDataMember]
		public bool ShowInDialogLeft
		{
			get => OrderInDialogLeft >= 0;
			set => OrderInDialogLeft = value ? 100 : -1;
		}

		[IgnoreDataMember]
		public bool ShowInDialogRight
		{
			get => OrderInDialogRight >= 0;
			set => OrderInDialogRight = value ? 100 : -1;
		}

		public bool ShowIn(int which) => Order[which] >= 0;

		public void UpdateProperties()
		{
			OnPropertyChanged(nameof(Description));
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
}