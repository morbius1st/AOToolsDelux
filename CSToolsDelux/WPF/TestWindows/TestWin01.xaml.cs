using System;
using System.Collections;
using System.Threading;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
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
using Autodesk.Revit.UI;
using CSToolsDelux.Revit.Commands;
using CSToolsDelux.Revit.Tests;

namespace CSToolsDelux.WPF.TestWindows
{
	/// <summary>
	/// Interaction logic for TestWin01.xaml
	/// </summary>
	public partial class TestWin01 : Window, INotifyPropertyChanged
	{
		internal static int int01 = 0;
		internal static string docName;

		private string textMsg01;

		private SubClass01 sc01;

		private SubClass01 sc01x= new SubClass01(null);
		private static SubClass02 sc02;
		private SubClassS scS;

		private SubClassLazy sclEarly = SubClassLazy.Instance;
		private SubClassLazy sclLate;

		private Singleton single;

		private int a01 = 0;
		private int a02 = 0;
		private int aS = 0;

		private int tv = 0;

		private int test01;

		public TestWin01()
		{
			InitializeComponent();

			sc01 = new SubClass01(AppRibbon.Doc.Title);
			SubClass01.sc02After2 = new SubClass02();
			SubClass01.sc02After2.DocName = AppRibbon.Doc.Title;

			sc02 = new SubClass02();
			scS = SubClassS.Instance;

			SubClass01.StaticDocName = AppRibbon.Doc.Title;

			single = Singleton.Get(AppRibbon.Doc.Title);

			docName = AppRibbon.Doc.Title;

			AppRibbon.docName = AppRibbon.Doc.Title;

			Test01.sc02 = new SubClass02();

			Test01.sc01.TestVal12 = 0;
			Test01.sc02.TestVal22 = 0;

			sc01x.TestVal12 = 0;

			SubClassLazy.sc02Early.DocName = docName;
			SubClassLazy.sc02Late = new SubClass02();
			SubClassLazy.sc02Late.DocName = docName;

			sclLate = SubClassLazy.Instance;

			sclEarly.DocName2 = docName;
			sclLate.DocName2 = docName;


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
				sc01.TestVal01 += 1;
				sc02.TestVal02 += 1;
				scS.TestValS += 1;
				tv = TestVal;

				int01 += i;

				single.D1 += 1;
				single.I1 += 1;


				sc01.TestVal12 += 1;
				sc02.TestVal22 += 1;
				scS.TestValS2 += 1;
				sc01x.TestVal01 += 1;
				sc01x.TestVal12 += 1;

				int01 += 1;

				Test01.sc01.TestVal01 += 1;
				Test01.sc01.TestVal12 += 1;

				Test01.sc02.TestVal02 += 1;
				Test01.sc02.TestVal22 += 1;

				SubClassPerDoc.pd01 += 1;
				Test01.pd01.pd11 += 1;

			}

			show2();
		}

		private void clear()
		{
			AppDomain b = AppDomain.CurrentDomain;
			Application a = System.Windows.Application.Current;
			// IDictionary a = System.Windows.Application.Current.Properties;

			textMsg01 = "";
			WriteLineMsg($"file name| {AppRibbon.Doc.Title}");
		}

		private void show()
		{
			WriteLineMsg($"testVal| {tv}");
			WriteLineMsg($"sc01| {a01}");
			WriteLineMsg($"sc02| {a02}");
			WriteLineMsg($"scS| {aS}");
			WriteLineMsg($"single (D1)| {single.D1}");
			WriteLineMsg($"single (I1)| {single.I1}");
			WriteLineMsg($"single (doc)| {single.DocName}");
			WriteLineMsg($"single (doc)| {single.docName}");

			WriteLineMsg("");
		}

		private void show2()
		{
			// the below comments are based on window being shown as a 
			WriteLineMsg($"name| {docName}");        // not shared
			WriteLineMsg($"sc01| {sc01.TestVal01}"); // not shared
			WriteLineMsg($"sc11| {sc01.TestVal12}"); // not shared
			WriteLineMsg($"x.sc01| {sc01x.TestVal01}");  // not shared - reset in its constructor
			WriteLineMsg($"x.sc11| {sc01x.TestVal12}");  // not shared - reset in the above constructor


			WriteLineMsg($"sc02| {sc02.TestVal02}"); // not shared
			WriteLineMsg($"sc22| {sc02.TestVal22}"); // not shared
			WriteLineMsg($" scS| {scS.TestValS}");   // not shared

			// local object
			WriteLineMsg($"scS2| {scS.TestValS2}");    // shared

			// a local static variable
			WriteLineMsg($"int01| {int01}");         // shared
			WriteLineMsg("");
			WriteLineMsg($"test01.sc01| {Test01.sc01.TestVal01}");  // shared
			WriteLineMsg($"test01.sc12| {Test01.sc01.TestVal12}");  // not shared
			WriteLineMsg(""); 
			WriteLineMsg($"test01.sc02| {Test01.sc02.TestVal02}");  // not shared
			WriteLineMsg($"test01.sc22| {Test01.sc02.TestVal22}");  // not shared
			WriteLineMsg(""); 
			WriteLineMsg($"perdoc pd01| {SubClassPerDoc.pd01}");    // shared
			WriteLineMsg($"test01.pd01.pd11| {Test01.pd01.pd11}");  // not shared

			WriteLineMsg("");
			WriteLineMsg($"single (D1)| {single.D1}");        // all singletons are saved
			WriteLineMsg($"single (I1)| {single.I1}");
			WriteLineMsg($"single (doc)| {single.DocName}");
			WriteLineMsg($"single (doc)| {single.docName}");
			WriteLineMsg("");
			WriteLineMsg($"doc name (doc.title)| {AppRibbon.Doc.Title}");  // not shared
			WriteLineMsg($"doc name (appribbon)| {AppRibbon.docName}");    // not shared
			WriteLineMsg($"doc name (uiApp.doc.title)| {AppRibbon.UiApp.ActiveUIDocument.Document.Title}"); // not shared

			WriteLineMsg($"doc name (test doc)| {Test01.doc.Title}");    // not shared

			WriteLineMsg($"doc name (test01.doc)| {Test01.docName}");    // not shared

			WriteLineMsg($"doc name (local static doc)| {docName}"); // not shared

			WriteLineMsg($"static doc name (sub01.doc)| {SubClass01.StaticDocName}"); // not shared
			WriteLineMsg($"static early bound| {SubClass01.sc02Early.DocName}");      // not shared
			WriteLineMsg($"static late  bound| {SubClass01.sc02Late.DocName}");       // not shared
			WriteLineMsg($"static after  bound| {SubClass01.sc02After2.DocName}");    // not shared
			WriteLineMsg("");
			WriteLineMsg($"lazy early bound| {SubClassLazy.sc02Early.DocName}"); // not shared
			WriteLineMsg($"lazy late bound| {SubClassLazy.sc02Late.DocName}");   // not shared
			WriteLineMsg("");                                                    // not shared
			WriteLineMsg($"lazy inst early bound| {sclEarly.DocName2}");         // not shared
			WriteLineMsg($"lazy inst late bound| {sclLate.DocName2}");           // not shared

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
			WriteLineMsg("show info only");

			show2();

		}

		private void BtnClr_OnClick(object sender, RoutedEventArgs e)
		{
			clear();
			show2();
		}
	}
}
