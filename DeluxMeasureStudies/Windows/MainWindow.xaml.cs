#region using

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Documents;

#endregion

// projname: DeluxMeasureStudies
// itemname: MainWindow
// username: jeffs
// created:  6/12/2022 4:16:20 PM

namespace DeluxMeasureStudies.Windows
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window, INotifyPropertyChanged
	{
		public ObservableCollection<string> sub_list { get; set; }

		private structure temp_struct;

		public MainWindow()
		{
			InitializeComponent();
			//temp_struct = new structure("Bladder");

			// structure_info_list.ItemsSource = temp_struct.fx_list;
			//this.DataContext = temp_struct;
		}

		public List<string[]> HelpInfo => Windows.HelpInfoData.NameInfo;
		public List<HelpInfo> HelpData => Windows.HelpInfoData.NameInfoData;

		public structure Temp_Struct
		{
			get => temp_struct;
			set
			{
				temp_struct = value;
				OnPropertyChanged();
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}
	}

	public static class HelpInfoData
	{
		private static readonly Thickness MARG_HDR1 = new Thickness(0, 0, 0, 2);
		private static readonly Thickness MARG_HDR2 = new Thickness(0, 2, 0, 2);
		private static readonly Thickness MARG_HDR3 = new Thickness(0, 3, 0, 2);
		private static readonly Thickness MARG_LST1 = new Thickness(6, 1, 0, 1);

		private static readonly Tuple<int, string>[] FIRST_CHAR =
		{
			new Tuple<int, string>(2, "●"),
			new Tuple<int, string>(3, "Style name's first character must be alphanumeric")
		};

		private static readonly Tuple<int, string>[] LAST_CHAR =
		{
			new Tuple<int, string>(2, "●"),
			new Tuple<int, string>(3, "Style name's last character must be alphanumeric")
		};

		private static readonly Tuple<int, string>[] ANY_CHAR =
		{
			new Tuple<int, string>(2, "●"),
			new Tuple<int, string>(3, "Only alphanumeric, space, dash, and period may be used")
		};


		public static List<string[]> NameInfo { get; set; } = new List<string[]>();

		public static List<HelpInfo> NameInfoData { get; set; } = new List<HelpInfo>();

		static HelpInfoData()
		{
			SetNameInfo();
		}

		private static void SetNameInfo()
		{
			NameInfo = new List<string[]>();
			NameInfo.Add(new [] { "", "This name identifies the style and must be unique.", "", "", "", "" });
			NameInfo.Add(new [] { "The name must follow these syntax rules:", "", "", "", "", "" });
			NameInfo.Add(new [] { "", "", "●", "Style name must be at least 4 characters long", "", "" });
			NameInfo.Add(new [] { "", "", "●", "Style name's first character must be alphanumeric", "", "" });
			NameInfo.Add(new [] { "", "", "●", "Style name's last character must be alphanumeric", "", "" });
			NameInfo.Add(new [] { "", "", "●", "Only alphanumeric, space, dash, and period may be used", "", "" });
			NameInfo.Add(new [] { "", "", "Suggestion:", "", "", "For ribbon styles, keep the name short to keep the ribbon button narrow" });


			NameInfoData.Add(new HelpInfo(
				new [] { new Tuple<int, string>(0, "The name identifies the style and must be unique.") }, MARG_HDR1));
			NameInfoData.Add(new HelpInfo(
				new [] { new Tuple<int, string>(1, "The name must follow these syntax rules:") }, MARG_HDR2));
			NameInfoData.Add(new HelpInfo(
				new []
				{
					new Tuple<int, string>(2, "●"),
					new Tuple<int, string>(3, "Style name must be at least 4 characters long")
				}, MARG_LST1));
			NameInfoData.Add(new HelpInfo(FIRST_CHAR, MARG_LST1));
			NameInfoData.Add(new HelpInfo(LAST_CHAR, MARG_LST1));
			NameInfoData.Add(new HelpInfo(ANY_CHAR, MARG_LST1));
			NameInfoData.Add(new HelpInfo(
				new []
				{
					new Tuple<int, string>(1, "Note:"),
					new Tuple<int, string>(4, "For ribbon styles, keep the name short so that the ribbon button is narrow")
				}, MARG_HDR3));
		}
	}

	public struct HelpInfo
	{
		public Thickness Marg { get; private set; }
		public string[] HelpDesc { get; private set; }

		public HelpInfo(Tuple<int, string>[] info, Thickness marg = default(Thickness))
		{
			Marg = marg;

			HelpDesc = new [] { "", "", "", "", "" , "", "" };

			foreach (var tuple in info)
			{
				HelpDesc[tuple.Item1] = tuple.Item2;
			}
		}
	}


	public class structure : INotifyPropertyChanged
	{
		private ObservableCollection<fraction> fxlist = new ObservableCollection<fraction>();

		public structure(string name)
		{
			this.name = name;

			fx_list = new ObservableCollection<fraction>();
			fraction fx1 = new fraction(3);
			fraction fx2 = new fraction(4);
			fraction fx3 = new fraction(5);

			fx_list.Add(fx1);
			fx_list.Add(fx2);
			fx_list.Add(fx3);

			MessageBox.Show("Total: " + total_dose);
			OnPropertyChanged("total_dose");

			fx_list[0].fx_dose = 50;

			MessageBox.Show("Total: " + total_dose);
			OnPropertyChanged("total_dose");
		}

		private void fractions_changed_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.Action == NotifyCollectionChangedAction.Add)
			{
				foreach (var item in e.NewItems)
				{
					((fraction)item).PropertyChanged += fx_Changed;
				}
			}
			else if (e.Action == NotifyCollectionChangedAction.Remove)
			{
				foreach (var item in e.OldItems)
				{
					((fraction)item).PropertyChanged -= fx_Changed;
				}
			}
		}

		void fx_Changed(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == nameof(fraction.fx_dose))
			{
				OnPropertyChanged(nameof(fx_list));
			}
		}


		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged([CallerMemberName] string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
			MessageBox.Show("A property in the underlying list was changed: " + propertyName);
		}

		/*
		public void calc_total_sum()
		{
			int total_sum_temp = 0;
			
			foreach (fraction fx in fx_list)
			{
				total_sum_temp += fx.fx_dose;
			}
			total_sum = total_sum_temp;
		}
		*/
		public string name { get; set; }

		public ObservableCollection<fraction> fx_list
		{
			get => fxlist;
			set
			{
				fxlist = value;
				OnPropertyChanged();
			}
		}

		//public int total_sum { get; set; }
		public int total_dose
		{
			get { return fx_list.Sum(x => x.fx_dose); }
		}
	}

	public class fraction : INotifyPropertyChanged
	{
		private int _fx_dose;

		public int fx_dose
		{
			get { return _fx_dose; }
			set
			{
				_fx_dose = value;
				this.calc_eq();
				this.OnPropertyChanged("fx_dose");
				//MessageBox.Show("FX DOSE PROP");
			}
		}

		private int _eq;

		public int eq
		{
			get { return _eq; }
			set { _eq = value; }
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public fraction(int fx_dose)
		{
			this.fx_dose = fx_dose;
			this.eq = fx_dose * 2;
		}

		public void calc_eq()
		{
			this.eq = this.fx_dose * 2;
		}

		public void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			if (PropertyChanged != null)
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
			//PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}