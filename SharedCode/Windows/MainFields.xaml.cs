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
using CSToolsDelux.Fields.FieldsManagement;
using CSToolsDelux.Revit.Commands;
using UtilityLibrary;

namespace SharedCode.Windows
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

	#endregion

	#region ctor

		public MainFields()
		{
			InitializeComponent();

			SharedCode.Windows.AWindow aw = this;

			fm = new FieldsManager(aw, "");
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
	}
}