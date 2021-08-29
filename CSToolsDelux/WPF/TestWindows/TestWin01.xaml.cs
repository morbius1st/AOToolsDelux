using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using CSToolsDelux.Revit.Commands;
using CSToolsDelux.Revit.Tests;

namespace CSToolsDelux.WPF.TestWindows
{
	/// <summary>
	/// Interaction logic for TestWin01.xaml
	/// </summary>
	public partial class TestWin01 : Window, INotifyPropertyChanged
	{
		private string textMsg01;
		private SubClass01 sc01;
		private static SubClass02 sc02;
		private SubClassS scS;

		private int a01 = 0;
		private int a02 = 0;
		private int aS = 0;

		private int tv = 0;

		private int test01;

		public TestWin01()
		{
			InitializeComponent();

			sc01 = new SubClass01();
			sc02 = new SubClass02();
			scS = SubClassS.Instance;

			clear();
		}

		public int TestVal
		{
			get
			{
				int a = test01;
				test01 = ((test01 + 1) % 5);
				return a;
			}
		}

		public string TextMsg01
		{
			get => textMsg01;
			set
			{
				textMsg01 = value;
				OnPropertyChanged();
			}
		}

		public void WriteMsg(string msg)
		{
			TextMsg01 += msg;
		}

		public void WriteLineMsg(string msg)
		{
			WriteMsg(msg + "\n");
		}

		public void test(int qty)
		{

			for (int i = 0; i < qty; i++)
			{
				a01 = sc01.TestVal01;
				a02 = sc02.TestVal02;
				aS =  scS.TestValS;
				tv = TestVal;
			}

			show();
		}

		private void clear()
		{
			textMsg01 = "";
			WriteLineMsg($"file name| {Test01.doc.Title}");
		}

		private void show()
		{
			WriteLineMsg($"testVal| {tv}");
			WriteLineMsg($"sc01| {a01}");
			WriteLineMsg($"sc02| {a02}");
			WriteLineMsg($"scS| {aS}");
			WriteLineMsg("");
		}



		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

		private void BtnQ1_OnClick(object sender, RoutedEventArgs e)
		{
			WriteLineMsg("process qty| 1");
			test(1);
		}

		private void BtnQ2_OnClick(object sender, RoutedEventArgs e)
		{
			WriteLineMsg("process qty| 2");
			test(2);
		}

		private void BtnClr_OnClick(object sender, RoutedEventArgs e)
		{
			clear();
			show();
		}
	}
}
