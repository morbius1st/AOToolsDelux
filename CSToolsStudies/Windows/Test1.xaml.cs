using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using CSToolsStudies.Annotations;
using UtilityLibrary;

namespace CSToolsStudies.Windows
{
	/// <summary>
	/// Interaction logic for Test1.xaml
	/// </summary>
	public partial class Test1 : Window, INotifyPropertyChanged
	{
		public const int RIBBONFAV = 1;

		public static int RIBBONFAVORITE = 1;

		public static int RIBBONFAVS => 1;

		private bool expandWindow = true;


	#region for item list

		// control settings only for style list
		private int isClosing = -1;
		private bool isSelected = false;
		private bool? isEditing = false;
		private bool? isReadOnly = false;
		private bool? isLocked = false;
		
	#endregion

	#region for add unit

		// control settings for add unit area
		private bool addUnitEnabled = false;
		private bool addUnitSelected = false;
		private bool addUnitReadOnly = false;
		private bool addUnitIsLocked = false;
		private bool? isGoodOrBad = null;

	#endregion


		private bool winLocationChanged = false;

		private int currPopupIdx = -1;
		private Popup[] popups = new Popup[10];
		private byte canEditPopup = 0;
		private byte action1Popup = 1;

		private bool popupIsEntered;
		private DoubleAnimation d = new DoubleAnimation(1, new Duration(TimeSpan.FromSeconds(2)));
		private AnimationClock popupAniClock;

		private DispatcherTimer timer;
		private bool timerActive;

		private WinLocation location { get; set; }

		// private Popup canEditPopup;

		public Test1(WinLocation location)
		{
			this.location = location;

			InitializeComponent();

			IsClosing = 1;
			IsGoodOrBad = null;

		}

		public bool ExpandWindow
		{

			get => expandWindow;

			set
			{
				if (value == expandWindow) return;
				expandWindow = value;
				OnPropertyChanged();
			}
		}


		public int IsClosing
		{

			get => isClosing;

			set
			{
				if (value == isClosing) return;
				isClosing = value;
				OnPropertyChanged();
			}
		}

		public bool? IsGoodOrBad
		{
			[DebuggerStepThrough]
			get => isGoodOrBad;
			[DebuggerStepThrough]
			set
			{
				if (value == isGoodOrBad) return;
				isGoodOrBad = value;
				OnPropertyChanged();
			}
		}

		public bool AddUnitEnabled
		{
			get => addUnitEnabled;

			set
			{
				if (value == addUnitEnabled) return;
				addUnitEnabled = value;
				OnPropertyChanged();
			}
		}

		public bool AddUnitSelected
		{

			get => addUnitSelected;

			set
			{
				if (value == addUnitSelected) return;
				addUnitSelected = value;
				OnPropertyChanged();
			}
		}

		public bool AddUnitReadOnly
		{
			[DebuggerStepThrough]
			get => addUnitReadOnly;
			[DebuggerStepThrough]
			set
			{
				if (value == addUnitReadOnly) return;
				addUnitReadOnly = value;
				OnPropertyChanged();
			}
		}

		public bool AddUnitIsLocked
		{
			[DebuggerStepThrough]
			get => addUnitIsLocked;
			[DebuggerStepThrough]
			set
			{
				if (value == addUnitIsLocked) return;
				addUnitIsLocked = value;
				OnPropertyChanged();
			}
		}

		public bool IsSelected
		{
			[DebuggerStepThrough]
			get => isSelected;

			set
			{

				if (value == isSelected) return;
				isSelected = value;

				if (isSelected == false)
				{
					IsEditing = false;
					IsReadOnly = false;
				}

				OnPropertyChanged();
			}
		}

		public bool? IsEditing
		{
			[DebuggerStepThrough]
			get => isEditing;

			set
			{
				if (value == isEditing) return;

				if (isLocked.HasValue && isLocked.Value ||
					!isSelected)
				{
					isEditing = false;
				}
				else
				{
					isEditing = value;

					if (isEditing.HasValue && isEditing.Value)
					{
						isReadOnly = false;
					}
				}

				OnPropertyChanged();
				OnPropertyChanged(nameof(IsReadOnly));
			}
		}

		public bool? IsReadOnly
		{
			[DebuggerStepThrough]
			get => isReadOnly;

			set
			{
				if (value == isReadOnly) return;

				if (isLocked.HasValue && isLocked.Value
					|| !isSelected)
				{
					isReadOnly = false;
				}
				else
				{
					isReadOnly = value;

					if (isReadOnly.HasValue && isReadOnly.Value)
					{
						isEditing = false;
					}
				}

				OnPropertyChanged();
				OnPropertyChanged(nameof(IsEditing));
			}
		}

		public bool? IsLocked
		{
			[DebuggerStepThrough]
			get => isLocked;

			set
			{
				if (value == isLocked) return;
				isLocked = value;

				if (isLocked.HasValue == true && isLocked.Value)
				{
					IsEditing = false;
					IsReadOnly = false;
				}

				OnPropertyChanged();
			}
		}

		public bool WinLocationChanged
		{
			get => winLocationChanged;
			set
			{
				winLocationChanged = value;
				OnPropertyChanged();

			}
		}




		public event PropertyChangedEventHandler PropertyChanged;
		
		[NotifyPropertyChangedInvocator]
		[DebuggerStepThrough]
		private void OnPropertyChanged([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}


		private void win_Loaded(object sender, RoutedEventArgs e)
		{
			popups[canEditPopup] = CsWpfUtilities.FindElementByName<Popup>(this, "PuCanEdit");
			popups[action1Popup] = CsWpfUtilities.FindElementByName<Popup>(this, "PuAction1");


			if (location.Top >= 0 && location.left >= 0)
			{
				this.Top = location.Top;
				this.Left = location.left;
			}

			IsClosing = 2;
		}

		private void win_LocationChanged(object sender, EventArgs e)
		{
			WinLocationChanged = true;

			popupClose();

		}


		
		private void BtnChangeSkin_OnClick(object sender, RoutedEventArgs e)
		{
			

			if ( AppRibbon.Skin == Skin.Jeff)
			{
				((AppRibbon) Application.Current).ChangeSkin( Skin.Fluent);
			}
			else
			{
				((AppRibbon) Application.Current).ChangeSkin( Skin.Jeff);
			}

			IsClosing = 3;

			Tag = new WinLocation(this.Top, this.Left);

			DialogResult = false;

			this.Close();
		}

		private void BtnDone_OnClick(object sender, RoutedEventArgs e)
		{
			DialogResult = true;
			Tag = "closing now";

			this.Close();
		}

		private void BtnCanEditClear_OnClick(object sender, RoutedEventArgs e)
		{
			Debug.WriteLine("@EditClear");
		}



		private void Action1Info_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (currPopupIdx >= 0)
			{
				return;
			}

			popupOpen(action1Popup);

			popupIsEntered = false;

			startTimer();
		}

		private void CanEditInfo_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (currPopupIdx >= 0)
			{
				return;
			}

			popupOpen(canEditPopup);

			popupIsEntered = false;

			startTimer();
		}


		private void Popup_OnMouseEnter(object sender, RoutedEventArgs e)
		{
			popupIsEntered = true;

			removeTimer();
		}

		private void Popup_OnMouseLeave(object sender, RoutedEventArgs e)
		{
			popupIsEntered = false;

			startTimer();
		}

		private void BtnPopupClose_OnClick(object sender, RoutedEventArgs e)
		{
			popupClose();
		}


		private void popupOpen(byte idx)
		{
			currPopupIdx = idx;

			if (!popups[currPopupIdx].IsOpen) popups[currPopupIdx].IsOpen = true;
		}

		private void popupClose()
		{
			if (currPopupIdx < 0)
			{
				return;
			}

			if (popups[currPopupIdx].IsOpen) popups[currPopupIdx].IsOpen = false;

			removeTimer();

			currPopupIdx = -1;
		}


		private void popupTimerCompleted(object sender, EventArgs e)
		{
			removeTimer();

			if (popupIsEntered)
			{
				return;
			}

			popupClose();
		}

		private void removeTimer()
		{
			if (!timerActive) return;

			timer.Stop();

			timerActive = false;
		}

		private void startTimer()
		{
			if (timerActive) return;

			timer = new DispatcherTimer();
			timer.Interval = new TimeSpan(0, 0, 3);

			timerActive = true;

			timer.Tick += popupTimerCompleted;

			timer.Start();
		}




		private Skin skin { get; set; } = AppRibbon.Skin;

		private void TbxGetStyleName1_GotFocus(object sender, RoutedEventArgs e)
		{
			AddUnitSelected = true;
		}

		private void CbxUnitSelect2_DropDownOpened(object sender, EventArgs e)
		{
			AddUnitSelected = true;
		}
	}
}
