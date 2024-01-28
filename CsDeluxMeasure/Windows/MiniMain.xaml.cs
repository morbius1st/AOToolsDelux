using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
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
using CsDeluxMeasure.RevitSupport;
using CsDeluxMeasure.RevitSupport.ExtEvents;
using SettingsManager;
using UtilityLibrary;
using CsDeluxMeasure.Windows.Support;
// using Rectangle = Autodesk.Revit.DB.Rectangle;
using static CsDeluxMeasure.Windows.Support.WindowApiUtilities;
using Rectangle = System.Drawing.Rectangle;

namespace CsDeluxMeasure.Windows
{
	/// <summary>
	/// Interaction logic for MiniMain.xaml
	/// </summary>
	public partial class MiniMain : INotifyPropertyChanged, IWin
	{
		private const string DialogToWatch = "Dialog_Revit_Units";

		private PointDistances distancesPrime = new PointDistances();
		private PointDistances distancesSecond = new PointDistances();

		private UIDocument uiDoc;

		private ExtEvtHandler exHandler;
		private ExternalEvent exEvent;

		private bool unitsDialogBoxDisplayed;

		// private bool dialogClosed;
		private bool mainHidden;

		private bool doingMove = false;
		private bool doingSlider = false;

		public MiniMain(UIDocument uiDoc, ExtEvtHandler exHandler, ExternalEvent exEvent)
		{
			InitializeComponent();

			this.uiDoc = uiDoc;
			this.exHandler = exHandler;
			this.exEvent = exEvent;

			uiDoc.Application.DialogBoxShowing += Application_DialogBoxShowing;
			uiDoc.Application.ViewActivating += Application_ViewActivating;

		}

		private void Application_ViewActivating(object sender, Autodesk.Revit.UI.Events.ViewActivatingEventArgs e)
		{
			if (!unitsDialogBoxDisplayed) return;

			UnitsDialogBoxDisplayed = false;
		}

		private void Application_DialogBoxShowing(object sender, Autodesk.Revit.UI.Events.DialogBoxShowingEventArgs e)
		{
			if (e.DialogId.Equals(DialogToWatch))
			{
				unitsDialogBoxDisplayed=true;
			}
		}

		private string msg;

		public string textMsg01
		{
			get => msg;
			set
			{
				msg = value;
				OnPropertyChanged(nameof(MessageBoxText));
			}
		}

		public string MessageBoxText
		{
			get => textMsg01;
			set
			{
				textMsg01 = value;
				OnPropertyChanged();
			}
		}

		public bool UnitsDialogBoxDisplayed
		{
			get => unitsDialogBoxDisplayed;
			set
			{
				unitsDialogBoxDisplayed = value;

				MainWindowCommand.UpdatePoints();
			}
		}


		public PointDistances Prime
		{
			get => distancesPrime;
			set
			{
				distancesPrime = value;
				OnPropertyChanged();
			}
		}

		public PointDistances Second
		{
			get => distancesSecond;
			set
			{
				distancesSecond = value;
				OnPropertyChanged();
			}
		}

		public PointMeasurements Points
		{
			set
			{
				Prime.Points = value;
				Second.Points = value;

				OnPropertyChanged(nameof(Prime));
				OnPropertyChanged(nameof(Second));

				MainWindowCommand.W.Activate();
			}
		}


		private void Btn_Pick_OnClick(object sender, RoutedEventArgs e)
		{
			this.MoveFocus(new TraversalRequest(FocusNavigationDirection.First));

			MainWindowCommand.SelectPoints();

		}

		private void Btn_Dialog_OnClick(object sender, RoutedEventArgs e)
		{
			MainWindowCommand.Mini_TempShowDialog();
		}

		public bool MainHidden
		{
			get => mainHidden;
			set
			{
				// Debug.WriteLine($"mini| MainHidden| {value}");
				mainHidden = value;
				OnPropertyChanged();
			}
		}

		public bool DoingMove
		{
			get => doingMove;
			set
			{
				doingMove = value;
				// Debug.WriteLine($"drag move set| {value}");
			}
		}

		public bool DoingSlider
		{
			get => doingSlider;
			set => doingSlider = value;
		}

		public void ShowMe()
		{
			if (this.Visibility == Visibility.Visible) return;
			this.Visibility = Visibility.Visible;
		}

		public void HideMe()
		{
			if (this.Visibility == Visibility.Hidden) return;
			this.Visibility = Visibility.Hidden;
		}

		public void ShowMini(bool value)
		{
			if (value)
			{
				ShowMe();
			}
			else
			{
				HideMe();
			}
		}



		// public bool DialogNotVisible
		// {
		// 	get => dialogNotVisible;
		// 	set
		// 	{
		// 		Debug.WriteLine($"mini| DialogNotVisible| {value}");
		// 		dialogNotVisible = value;
		// 		OnPropertyChanged();
		// 	}
		// }
		//
		//
		// public bool HideMain
		// {
		// 	get => hideMain;
		// 	set
		// 	{
		// 		Debug.WriteLine($"mini| HideMain| {value}");
		//
		// 		hideMain = value;
		// 		OnPropertyChanged();
		// 	}
		// }
		//
		// public bool ShowMini
		// {
		// 	get => showMini;
		// 	set
		// 	{
		// 		Debug.WriteLine($"mini| showMini| {value}");
		//
		// 		showMini = value;
		// 		OnPropertyChanged();
		//
		// 		if (value)
		// 		{ 
		// 			ShowMe();
		// 		}
		// 		else
		// 		{
		// 			HideMe();
		// 		}
		// 	}
		// }





		public void SetPosition(Window parentWindow)
		{
			double t = UserSettings.Data.WinPosMiniWin.Top;
			double l = UserSettings.Data.WinPosMiniWin.Left;

			if (t <= 0 && l <= 0)
			{
				Rectangle r = WindowUtilities.GetWinPositionWinOffset(this, parentWindow, 100, 150);

				t = r.Top;
				l = r.Left;
			}

			this.Top = t;
			this.Left = l;
		}

		private void Btn_Exit_OnClick(object sender, RoutedEventArgs e)
		{
			UserSettings.Data.WinPosMiniWin = new WindowLocation(this.Top, this.Left);
			UserSettings.Admin.Write();

			// this.Visibility = Visibility.Hidden;

			this.Hide();
			Owner.Activate();

			User32dll.SendKeyCode(0x01);
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}


		private void MiniMain_OnClosing(object sender, CancelEventArgs e)
		{
			UserSettings.Data.WinPosMiniWin = new WindowLocation(this.Top, this.Left);
			UserSettings.Admin.Write();

			uiDoc.Application.DialogBoxShowing -= Application_DialogBoxShowing;
			uiDoc.Application.ViewActivating -= Application_ViewActivating;
		}

		private void MiniMain_OnMouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ChangedButton == MouseButton.Left)
			{
				// Debug.WriteLine("doing drag move");
				this.DragMove();
			}
		}

		private void MiniMain_OnLoaded(object sender, RoutedEventArgs e)
		{
			SetPosition(this.Owner);

			OnPropertyChanged(nameof(Prime));
			OnPropertyChanged(nameof(Second));

			// Debug.WriteLine("mm A revit being activated");
		}


		private void Window_Activated(object sender, EventArgs e)
		{
			// Debug.WriteLine("mm B revit has been activated");
			if (!DoingMove && !DoingSlider) Owner.Activate();
        }

		private void Window_MouseEnter(object sender, MouseEventArgs e)
		{
			// Debug.WriteLine("mm - mouse enter");
			DoingMove = true;
		}

		private void Window_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			// Debug.WriteLine("mm - mouse left up");
			DoingMove = false;

			// Debug.WriteLine("mm C revit has been activated");
			Owner.Activate();
		}

		

		private void slider_MouseEnter(object sender, MouseEventArgs e)
		{
			// Debug.WriteLine("slider - mouse enter");
			DoingSlider = true;
			
		}

		private void slider_MouseLeave(object sender, MouseEventArgs e)
		{
			// Debug.WriteLine("slider - mouse leave");
			DoingSlider = false;
		}


		// private void Window_MouseLeave(object sender, MouseEventArgs e)
		// {
		// 	Debug.WriteLine("mm - mouse leave");
		// }
		//
		// private void Window_MouseUp(object sender, MouseButtonEventArgs e)
		// {
		// 	Debug.WriteLine("mm - mouse up");
		// }
		//
		//
		// private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		// {
		// 	Debug.WriteLine("mm - mouse left down");
		// }





		// private void ParentWin_Closed(object sender, EventArgs e)
		// {
		// 	DialogClosed = true;
		// }


		// public bool DialogClosed
		// {
		// 	get => dialogClosed;
		// 	set
		// 	{
		// 		dialogClosed = value;
		// 		OnPropertyChanged();
		// 		OnPropertyChanged(nameof(DialogOpen));
		// 	}
		// }
		//
		// public bool DialogOpen => !DialogClosed;

		// public bool ShowDialog
		// {
		// 	get => showDialog;
		// 	set
		// 	{
		// 		showDialog = value;
		// 		OnPropertyChanged();
		// 	}
		// }


		// static WindowLocation _Location { get; set; } //= new WindowLocation(0, 0);
	}
}