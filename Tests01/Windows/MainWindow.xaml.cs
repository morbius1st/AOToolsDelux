#region using

using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using RevitLibrary;

using Tests01.Annotations;
using Tests01.ExtEvent;
using Tests01.Functions;
using Tests01.RevitSupport;
using UtilityLibrary;

#endregion

// projname: Tests01
// itemname: MainWindow
// username: jeffs
// created:  1/27/2024 12:19:21 AM

namespace Tests01.Windows
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window, INotifyPropertyChanged, IWin
	{
		private string messageBox;

	#region private fields

		// objects
		private FunctionHandler fh;


		private ExtEvtHandler eEh;
		private ExternalEvent eEv;

	#endregion

	#region ctor

		public MainWindow(ExtEvtHandler eeh, ExternalEvent eee)
		{
			InitializeComponent();

			fh = new FunctionHandler();

			eEh = eeh;
			eEv = eee;

			M.Win = this;
		}

	#endregion

	#region public properties

		public string MessageBox
		{
			get => messageBox;
			set
			{
				if (value == messageBox) return;
				messageBox = value;
				OnPropertyChanged();
			}
		}

	#endregion

	#region private properties

	#endregion

	#region public methods

	#endregion

	#region private methods

		private void MakeRequest(EeIId eeid)
		{
			eEh.EeRequest.Make(eeid);
			eEv.Raise();
		}

	#endregion

	#region event publishing

		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}		

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is MainWindow";
		}

	#endregion

	#region event consuming

		private void BtnViewInfo_OnClick(object sender, RoutedEventArgs e)
		{
			fh.Execute(FunctionId.FID_VIEW_INFO);
		}

	#endregion

		private void BtnViewData_OnClick(object sender, RoutedEventArgs e)
		{
			fh.Execute(FunctionId.FID_VIEW_DATA);
		}

		private void BtnMsgClr_OnClick(object sender, RoutedEventArgs e)
		{
			M.Clr(null);
		}

		private void BtnGetPt1_OnClick(object sender, RoutedEventArgs e)
		{
			// fh.Execute(FunctionId.FID_GET_PT1);

			MakeRequest(EeIId.EID_SKETCH_PLANE);
		}

		private void BtnWorkPlaneInfo_OnClick(object sender, RoutedEventArgs e)
		{
			fh.Execute(FunctionId.FID_WORKPLANE_INFO);
		}
	}
}