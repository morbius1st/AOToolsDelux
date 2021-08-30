#region using

using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using CSToolsDelux.Fields.FieldsManagement;
using CSToolsDelux.WPF;
using UtilityLibrary;

#endregion

// projname: CSToolsStudies
// itemname: MainWindow
// username: jeffs
// created:  8/29/2021 2:19:26 PM

namespace CSToolsStudies.Windows
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : AWindow, INotifyPropertyChanged
	{

	#region private fields

		private string myName = nameof(MainWindow);
		private string textMsg01;

		private int marginSize = 0;
		private int marginSpaceSize = 2;

		private string location;

		private FieldsManager sMgr;

	#endregion

	#region ctor

		public MainWindow()
		{
			InitializeComponent();

			sMgr = new FieldsManager(this, null);
		}

	#endregion

	#region public properties

	#endregion

	#region private properties

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
			base.Close();
		}

		private void BtnShowRootFields_OnClick(object sender, RoutedEventArgs e)
		{
			sMgr.ShowRootFields();
		}

		private void BtnRootData_OnClick(object sender, RoutedEventArgs e)
		{
			sMgr.ShowRootData();
		}
	}
}