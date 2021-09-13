#region using

using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

using CSToolsStudies.FieldsManagement;
using CSToolsDelux.WPF;
using UtilityLibrary;
using Autodesk.Revit.DB;

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

		public string VendorId = "PRO.CYBERSTUDIO";

		private string myName = nameof(MainWindow);
		private string textMsg01;

		private int marginSize = 0;
		private int marginSpaceSize = 2;

		private string location;

		private FieldsManager fmMgr;

	#endregion

	#region ctor

		public MainWindow()
		{
			InitializeComponent();

			fmMgr = new FieldsManager(this);
		}

	#endregion

	#region public properties

		public static string DocKey => "CSToolsStudies";

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

		// private void BtnShowRootFields_OnClick(object sender, RoutedEventArgs e)
		// {
		// 	fmMgr.ShowRootFields();
		// }
		//
		// private void BtnRootData_OnClick(object sender, RoutedEventArgs e)
		// {
		// 	fmMgr.ShowRootData();
		// }
		//
		// private void BtnAppFields_OnClick(object sender, RoutedEventArgs e)
		// {
		// 	fmMgr.ShowAppFields();
		// }
		//
		// private void BtnAppData_OnClick(object sender, RoutedEventArgs e)
		// {
		// 	fmMgr.ShowAppData();
		// }

		private void BtnCellFields_OnClick(object sender, RoutedEventArgs e)
		{
			fmMgr.ShowCellFields();
		}

		private void BtnCellData_OnClick(object sender, RoutedEventArgs e)
		{
			fmMgr.ShowCellData();
		}

		private void BtnRootAppFields_OnClick(object sender, RoutedEventArgs e)
		{
			fmMgr.ShowRootAppFields();
		}

		private void BtnRootAppData_OnClick(object sender, RoutedEventArgs e)
		{
			fmMgr.ShowRootAppData();
		}

		private void BtnTest02_OnClick(object sender, RoutedEventArgs e) 
		{
			string x = System.IO.Path.GetRandomFileName().Replace('.','_');

			WriteLineAligned("unique name| ", x);

			ShowMsg();
		}

		
	}
}