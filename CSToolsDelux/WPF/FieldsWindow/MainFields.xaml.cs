using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using Autodesk.Revit.UI;
using CSToolsDelux.Fields.FieldsManagement;
using CSToolsDelux.WPF;
using CSToolsDelux.Revit.Commands;
using CSToolsDelux.Utility;
using UtilityLibrary;

namespace CSToolsDelux.WPF.FieldsWindow
{
	/// <summary>
	/// Interaction logic for MainFields.xaml
	/// </summary>
	public partial class MainFields : AWindow, INotifyPropertyChanged
	{
	#region private fields

		private string myName = nameof(MainFields);
		private string textMsg01;

		private int marginSize = 0;

		private int marginSpaceSize = 2;

		private string location;

		private FieldsManager fm;
		


		private Document doc;

	#endregion

	#region ctor

		public MainFields(Document doc)
		{
			
			this.doc = doc;

			InitializeComponent();

			fm = new FieldsManager(this, doc, doc.Title);
		}

	#endregion

	#region public properties
/*
		public string MessageBoxText
		{
			get => textMsg01;
			set
			{
				textMsg01 = value;
				OnPropertyChanged();
			}
		}
*/
	#endregion

	#region private properties

	#endregion

	#region public methods
/*
		public void MsgClr()
		{
			textMsg01 = "";
			ShowMsg();
		}

		public void MarginClr()
		{
			marginSize = 0;
		}

		public void MarginUp()
		{
			marginSize += marginSpaceSize;
		}

		public void MarginDn()
		{
			marginSize -= marginSpaceSize;

			if (marginSize < 0) marginSize = 0;
		}

		public void WriteMsg(string msg1, string msg2 = "", string loc = "", string spacer = " ")
		{
			writeMsg(msg1, msg2, loc, spacer);
		}

		public void WriteLineMsg(string msg1, string msg2 = "", string loc = "", string spacer = " ")
		{
			writeMsg(msg1, msg2 + "\n", loc, spacer);
		}

		public void ShowMsg()
		{
			OnPropertyChanged("MessageBoxText");
		}
*/
	#endregion

	#region private methods
/*
		private string margin(string spacer)
		{
			if (marginSize == 0) return "";

			return spacer.Repeat(marginSize);
		}

		private void writeMsg(string msg1, string msg2, string loc, string spacer)
		{
			location = loc;

			textMsg01 += margin(spacer) + msg1 + " " + msg2;
		}
*/
	#endregion

	#region event consuming

	#endregion

	#region event publishing

		public event PropertyChangedEventHandler PropertyChanged;

		protected override void OnPropertyChanged([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

	#endregion

	#region system overrides

		public string ToString()
		{
			return $"this is| {myName}" ;
		}

	#endregion


		private void BtnExit_OnClick(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private void BtnRootAppFields_OnClick(object sender, RoutedEventArgs e)
		{
			fm.ShowRootAppFields();
		}
		
		private void BtnRootAppData_OnClick(object sender, RoutedEventArgs e)
		{
			fm.ShowRootAppData();
		}
		
		private void BtnShowRootFields_OnClick(object sender, RoutedEventArgs e)
		{
			fm.ShowRootFields();
		}

		private void BtnRootData_OnClick(object sender, RoutedEventArgs e)
		{
			fm.ShowRootData();
		}

		private void BtnAppFields_OnClick(object sender, RoutedEventArgs e)
		{
			fm.ShowAppFields();
		}

		private void BtnAppData_OnClick(object sender, RoutedEventArgs e)
		{
			fm.ShowAppData();
		}

		private void BtnCellFields_OnClick(object sender, RoutedEventArgs e)
		{
			fm.ShowCellFields();
		}

		private void BtnCellData_OnClick(object sender, RoutedEventArgs e)
		{
			fm.ShowCellData();
		}

		private void BtnFindRootDs_OnClick(object sender, RoutedEventArgs e)
		{
			fm.GetDataStorage();
		}

		private void BtnMakeSchema_OnClick(object sender, RoutedEventArgs e)
		{
			Schema schema;
			fm.MakeRootAppSchema(out schema);

			WriteLineAligned($"schema made| {schema.SchemaName}");

			ShowMsg();


		}

		private void testNames()
		{
			string test;
			string testA;
			string testB;

			SchemaBuilder sb = new SchemaBuilder(Guid.NewGuid());

			// failed - the "." is no good
			testA = Util.GetVendorId();

			WriteLineAligned($"is ok?| {testA}| ", $"{sb.AcceptableName(testA)}");

			// worked
			testA = Util.GetVendorId().Replace(".", "_");

			WriteLineAligned($"is ok?| {testA}| ", $"{sb.AcceptableName(testA)}");

			// this worked (but see below)
			testB = AppRibbon.Doc.Title;
			test = (testA + "_" + testB);

			WriteLineAligned($"is ok?| {test}| ", $"{sb.AcceptableName(test)}");

			// this worked (but see below)
			testB = AppRibbon.Doc.Title.Replace(' ','_');
			test = (testA + "_" + testB);

			WriteLineAligned($"is ok?| {test}| ", $"{sb.AcceptableName(test)}");

			// this worked (but see below)
			testB = AppRibbon.Doc.Title.Replace(" ",null);
			test = (testA + "_" + testB);

			WriteLineAligned($"is ok?| {test}| ", $"{sb.AcceptableName(test)}");
			
			// this failed because the spaces are no-good
			// the above titles worked because the model's title already has no spaces
			testB = "this is a test";
			test = (testA + "_" + testB);

			WriteLineAligned($"is ok?| {test}| ", $"{sb.AcceptableName(test)}");
			
			// worked (eliminated the spaces)
			testB = "this is a test".Replace(" ",null);
			test = (testA + "_" + testB);

			WriteLineAligned($"is ok?| {test}| ", $"{sb.AcceptableName(test)}");
			
			// worked (eliminate the spaces)
			testB = "this is a test".Replace(" ","");
			test = (testA + "_" + testB);

			WriteLineAligned($"is ok?| {test}| ", $"{sb.AcceptableName(test)}");
			
			testB = "this is a test TEST+123-456&789 =0";
			testB = Regex.Replace(testB, @"[^0-9a-zA-Z]", "");
			test = (testA + "_" + testB);

			WriteLineAligned($"is ok?| {test}| ", $"{sb.AcceptableName(test)}");



			ShowMsg();
			
		}


		private void BtnFindSchemaAndDataStor_OnClick(object sender, RoutedEventArgs e)
		{
			// get the entity by using ELEMENT.GetEntity(Schema) - 
			// per the below, the DataStorage is the element 
			// asuming that a single schema / element get found


			IList<Schema> schemas;
			IList<DataStorage> dx;

			bool result = fm.GetRootSchema(out schemas);

			WriteAligned($"schema (in memory) found?| {result.ToString()}", $"quantity| {schemas.Count}");

			if (schemas.Count > 0)
			{
				WriteMsg("\n");

				foreach (Schema s in schemas)
				{
					WriteAligned($"schema info| {s.SchemaName}  {s.VendorId}  {s.Documentation}");
				}
			}


			result = fm.GetRootDataStorages(out dx);
			WriteMsg("\n");
			WriteAligned($"datastorage found?| {result.ToString()}", $"quantity| {dx.Count}");

			if (dx.Count > 0)
			{
				WriteMsg("\n");

				foreach (DataStorage ds in dx)
				{
					WriteLineAligned($"datastorage info| {ds.Name}", $"valid?| {ds.IsValidObject}");
				}
			}

			ShowMsg();
		}
	}
}