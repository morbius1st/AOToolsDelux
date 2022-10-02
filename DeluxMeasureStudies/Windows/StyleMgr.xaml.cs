using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using UtilityLibrary;

namespace DeluxMeasureStudies.Windows
{
	public struct UStyle
	{
		public bool IsLocked { get; set; }
		public string Name { get; set; }

		public UStyle(bool isLocked, string name)
		{
			IsLocked = isLocked;
			Name = name;
		}
	}

	public struct LbxData
	{
		public string SeqFormatted { get; set; }
		public UStyle Ustyle { get; set; }
		public string Key { get; set; }
		public ImageSource Ux { get; set; }

		public LbxData(string seqFormatted, UStyle ustyle)
		{
			SeqFormatted = seqFormatted;
			Key = seqFormatted;
			Ustyle = ustyle;
			Ux = null;
		}
	}


	/// <summary>
	/// Interaction logic for StyleMgr.xaml
	/// </summary>
	public partial class StyleMgr : Window , INotifyPropertyChanged
	{
		private int showTab;

		private List<LbxData> cbxList;


		public StyleMgr()
		{
			InitializeComponent();

			ShowTab = 0;
			initList();

		}

		public int ShowTab
		{
			get => showTab;
			set
			{
				showTab = value;
				OnPropertyChanged();
			}

		}

		public List<LbxData> CbxList => cbxList;

		public string CbxSelectedItem { get; set; }
		public bool CanStyleAdd { get; set; } = true;
		public bool CanAddBefore { get; set; } = true;
		public int InsPosition { get; set; } = 0;
		public bool CanAddAfter { get; set; } = true;


		private void initList()
		{
			cbxList = new List<LbxData>();

			cbxList.Add(new LbxData("01", new UStyle(false, "Name 1")));
			cbxList.Add(new LbxData("02", new UStyle(false, "Name 2")));
			cbxList.Add(new LbxData("03", new UStyle(false, "Name 3")));
		}



		private void StyleMgr_OnLoaded(object sender, RoutedEventArgs e)
		{
			// listLogTree();
			// listVisTree();

			findElement();

			int a = 1;
		}

		private void listLogTree()
		{
			Debug.WriteLine("\n\nListLogicalTree");
			CsWpfUtilities.ListLogicalTree(this);
			Debug.WriteLine("End ListLogicalTree");
		}

		private void listVisTree()
		{
			Debug.WriteLine($"\n\nListVisualTree");
			CsWpfUtilities.ListVisualTree(this);
			Debug.WriteLine($"end ListVisualTree");
		}

		private void findElement()
		{
			string name = "Lbx1";

			FrameworkElement fe = CsWpfUtilities.FindElementByName<ListBox>(this, name);

			
			if (fe != null)
			{
				Debug.WriteLine($"^^^ {name} FOUND| {fe.Name}");
			}
			else
			{
				Debug.WriteLine($"vvv {name} NOT found");
			}
		}


		private void findFrameworkElement(string name, bool dbug)
		{
			Debug.WriteLine($"\n\nFind name object| {name}| {FindName(name)}");

			FrameworkElement fe;
			fe = CsWpfUtilities.FindElementByName<ListBox>(this, name, dbug);


			if (fe != null)
			{
				Debug.WriteLine($"^^^ {name} FOUND| {fe.Name}");
			}
			else
			{
				Debug.WriteLine($"vvv {name} NOT found");
			}
		}

		private void LbxRibbon_OnInitialized(object sender, EventArgs e)
		{
			Debug.WriteLine("LbxRibbon_OnInitializede");
		}

		private void LbxRibbon_OnLoaded(object sender, RoutedEventArgs e)
		{
			Debug.WriteLine("LbxRibbon_OnLoaded");
		}

		private void LbxLeft_Loaded(object sender, RoutedEventArgs e)
		{
			Debug.WriteLine("LbxLeft_Loaded");
		}

		private void LbxLeft_Initialized(object sender, EventArgs e)
		{
			Debug.WriteLine("LbxLeft_Initialized");
		}

		private void LbxRight_Initialized(object sender, EventArgs e)
		{
			Debug.WriteLine("LbxRight_Initialized");
		}

		private void LbxRight_Loaded(object sender, RoutedEventArgs e)
		{
			Debug.WriteLine("LbxRight_Loaded");
		}


		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

		private void BtnSoShowStyleOrder_OnClick(object sender, RoutedEventArgs e)
		{
			ShowTab = showTab == 0 ? 1 : 0;
		}
	}
}
