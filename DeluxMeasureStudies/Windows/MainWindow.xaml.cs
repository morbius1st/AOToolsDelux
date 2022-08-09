#region using

using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;

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
		temp_struct = new structure("Bladder");

		// structure_info_list.ItemsSource = temp_struct.fx_list;
		this.DataContext = temp_struct;

	}

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