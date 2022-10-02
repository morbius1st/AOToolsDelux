using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interop;
using Autodesk.Revit.UI;
using CsDeluxMeasure.UnitsUtil;
using CsDeluxMeasure.Windows.Support;
using SettingsManager;
using UtilityLibrary;
using TextBox = System.Windows.Controls.TextBox;

namespace CsDeluxMeasure.Windows
{
	public partial class UnitStyleOrder : Window, INotifyPropertyChanged
	{
		private readonly string[] LISTBX_NAMES = new [] { "LbxRibbon", "LbxLeft", "LbxRight" };
		private readonly string[] LISTBX_SEL_IDX_PROP_NAME = new [] { nameof(SelIdxRibbon) , nameof(SelIdxDlgLeft), nameof(SelIdxDlgRight) };


	#region private fields

		private UnitStylesMgr usMgr;

		private UnitsManager uMgr = UnitsManager.Instance;

		private InList selectedTabIdx;

		private WkgInListItem[] selectedItem;

		private int[] selIdx = new int[UnitData.INLIST_COUNT];
		private ListBox[] Lbx = new ListBox[UnitData.INLIST_COUNT];

	#region popups

		private Popup currInfoPopup;

	#endregion

		// to capture when the window moves
		private static bool winLocationChanging;
		private static bool winMoving = false;
		private bool winLostFocus;
		private const int WM_SETCURSOR = 32;
		private const int WM_NCLBUTTONDOWN = 0x00a1;

	#endregion

	#region ctor

		public UnitStyleOrder(UnitStylesMgr um)
		{
			usMgr = um;

			InitializeComponent();

			init();
		}

	#endregion

	#region public properties

		// public List<WkgInListItem> WkgListRibbon => uMgr.UlMgr.Working.InListsRibbon;
		// public List<WkgInListItem> WkgListDlgLeft => uMgr.UlMgr.Working.InListsDlgLeft;
		// public List<WkgInListItem> WkgListDlgRight => uMgr.UlMgr.Working.InListsDlgRight;

		public UnitsInListsWorking Working => uMgr.UlMgr.Working;

		public UnitsManager UnitMgr => uMgr;

		// public WkgInListItem SelItem
		// {
		// 	get => selectedItem[(int) selectedTabIdx];
		// 	set
		// 	{
		// 		// Debug.WriteLine($"SelItem| proposed order| {value.ProposedOrder}");
		// 		selectedItem[(int) selectedTabIdx] = value; 
		// 		OnPropertyChanged();
		// 		OnPropertyChanged(nameof(PropOrder));
		// 	}
		// }

		public WkgInListItem SelItemRibbon
		{
			get => selectedItem[(int) InList.RIBBON];
			set
			{
				// Debug.WriteLine($"SelItemRibbon| proposed order| {value.ProposedOrder}");
				selectedItem[(int) InList.RIBBON] = value;
				OnPropertyChanged();
				OnPropertyChanged(nameof(PropOrder));
			}
		}

		public WkgInListItem SelItemDlgLeft
		{
			get => selectedItem[(int) InList.DIALOG_LEFT];
			set
			{
				selectedItem[(int) InList.DIALOG_LEFT] = value;
				OnPropertyChanged();
				OnPropertyChanged(nameof(PropOrder));
			}
		}

		public WkgInListItem SelItemDlgRight
		{
			get => selectedItem[(int) InList.DIALOG_RIGHT];
			set
			{
				selectedItem[(int) InList.DIALOG_RIGHT] = value;
				OnPropertyChanged();
				OnPropertyChanged(nameof(PropOrder));
			}
		}

		public InList SelectedTabIdx
		{
			get => selectedTabIdx;
			set
			{
				selectedTabIdx = value;
				OnPropertyChanged();
				OnPropertyChanged(nameof(InListName));
				OnPropertyChanged(nameof(TabHasChanges));
				OnPropertyChanged(nameof(PropOrder));
			}
		}

		public int SelIdx => selIdx[(int) selectedTabIdx];

		public int SelIdxRibbon
		{
			get => selIdx[(int) InList.RIBBON];
			set
			{
				selIdx[(int) InList.RIBBON] = value;
				OnPropertyChanged(nameof(PropOrder));
				OnPropertyChanged();
			}
		}

		public int SelIdxDlgLeft
		{
			get => selIdx[(int) InList.DIALOG_LEFT];
			set
			{
				selIdx[(int) InList.DIALOG_LEFT] = value;
				OnPropertyChanged(nameof(PropOrder));
				OnPropertyChanged();
			}
		}

		public int SelIdxDlgRight
		{
			get => selIdx[(int) InList.DIALOG_RIGHT];
			set
			{
				selIdx[(int) InList.DIALOG_RIGHT] = value;
				OnPropertyChanged(nameof(PropOrder));
				OnPropertyChanged();
			}
		}


		public int PropOrder
		{
			get =>  selectedItem[(int) selectedTabIdx].ProposedOrder;
			set
			{
				// Debug.WriteLine($"PropOrder| order| {value}");
				switch (selectedTabIdx)
				{
				case InList.RIBBON:
					{
						PropOrderRibbon = value;
						break;
					}
				case InList.DIALOG_LEFT:
					{
						PropOrderDlgLeft = value;
						break;
					}
				case InList.DIALOG_RIGHT:
					{
						PropOrderDlgRight = value;
						break;
					}
				}
			}
		}

		public int PropOrderRibbon
		{
			get => SelItemRibbon.ProposedOrder;
			set
			{
				moveItem(SelIdxRibbon, value);
				OnPropertyChanged();
			}
		}

		public int PropOrderDlgLeft
		{
			get => SelItemDlgLeft.ProposedOrder;
			set
			{
				moveItem(SelIdxDlgLeft, value);
				OnPropertyChanged();
			}
		}

		public int PropOrderDlgRight
		{
			get => SelItemDlgRight.ProposedOrder;
			set
			{
				moveItem(SelIdxDlgRight, value);
				OnPropertyChanged();
			}
		}

		public ListBox[] LBX => Lbx;

		public string InListName => UnitData.IN_LIST_NAMES[(int) selectedTabIdx];

		public bool TabHasChanges => uMgr.UlMgr.Working.HasChgs[(int) selectedTabIdx];


		// window location

		public static bool WinLocationChanging
		{
			get => winLocationChanging;
			set
			{
				winLocationChanging = value;

				OnStaticPropertyChanged();
			}
		}

	#endregion

	#region private properties

		private WindowLocation _Location { get; set; } = new WindowLocation(0, 0);

	#endregion

	#region public methods

		public void SetPosition(Window parentWindow)
		{
			double t = UserSettings.Data.WinPosStyleOrder.Top;
			double l = UserSettings.Data.WinPosStyleOrder.Left;

			this.Owner = parentWindow;

			// this.Top = t > 0 ? t : parentWindow.Top;
			// this.Left = l > 0 ? l : parentWindow.Left;

			this.Top = t > 0 ? t : this.Top;
			this.Left = l > 0 ? l : this.Left;

		}

	#endregion

	#region private methods

		private void init()
		{
			selectedItem = new WkgInListItem[UnitData.INLIST_COUNT];

			selectedTabIdx = InList.DIALOG_LEFT;
			SelItemDlgLeft = uMgr.UlMgr.Working.InListsDlgLeft[0];

			selectedTabIdx = InList.DIALOG_RIGHT;
			SelItemDlgRight = uMgr.UlMgr.Working.InListsDlgRight[0];

			SelectedTabIdx = InList.RIBBON;
			SelItemRibbon = uMgr.UlMgr.Working.InListsRibbon[0];
		}

		private void moveItem(int idx, int newProposed)
		{
			uMgr.UlMgr.MoveByProposedOrder(selectedTabIdx, idx, newProposed);

			Lbx[(int) selectedTabIdx].Items.Refresh();

			OnPropertyChanged(nameof(TabHasChanges));
		}

		private void RefreshListAll()
		{
			foreach (InList list in Enum.GetValues(typeof(InList)))
			{
				if (Lbx[(int) list] != null)
				{
					refreshList(list);
				}
			}

			OnPropertyChanged(nameof(TabHasChanges));
		}

		private void refreshList(InList which)
		{
			Lbx[(int) which].Items.Refresh();
			selIdx[(int) which] = 0;
			OnPropertyChanged(LISTBX_SEL_IDX_PROP_NAME[(int) which]);
			OnPropertyChanged(nameof(PropOrder));
		}

	#endregion

	#region event consuming

		// overall events

		private void UnitStyleOrder_OnLoaded(object sender, RoutedEventArgs e)
		{
			_Location = UserSettings.Data.WinPosStyleOrder;

			if (_Location.Top >= 0 && _Location.Left >= 0)
			{
				this.Top = _Location.Top;
				this.Left = _Location.Left;
			}

			HwndSource source = HwndSource.FromHwnd(new WindowInteropHelper(this).Handle);
			source.AddHook(new HwndSourceHook(WndProc));

			StaticPropertyChanged += OnStaticPropertyChanged;
		}

		private void UnitStyleOrder_OnIsKeyboardFocusWithinChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			if ((bool) e.NewValue)
			{
				if (!winLostFocus) return;

				BeginInfoPopup();
			}
			else
			{
				PopupInfoHide();

				winLostFocus = true;
			}
		}
		
		private void OnStaticPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			// Debug.WriteLine($"@static prop changed| {e.PropertyName}");

			if (e.PropertyName == nameof(WinLocationChanging))
			{
				if (winLocationChanging)
				{
					PopupInfoHide();
				}

				if (!winLocationChanging)
				{
					BeginInfoPopup();
				}
			}
		}


		// tab header buttons

		private void UnitStyleOrder_OnClosing(object sender, CancelEventArgs e)
		{
			if (TabHasChanges)
			{
				TaskDialog td = new TaskDialog("Style Order");
				td.MainInstruction = "You have un-saved changes.";
				td.MainContent = "Select CANCEL to keep these changes\nSelect OK to lose your changes";
				td.CommonButtons = TaskDialogCommonButtons.Cancel | TaskDialogCommonButtons.Ok;
				td.DefaultButton = TaskDialogResult.Cancel;

				TaskDialogResult result = td.Show();

				if (result == TaskDialogResult.Cancel)
				{
					e.Cancel = true;
					return;
				}

				uMgr.UlMgr.ResetListAll();
			}


			UserSettings.Data.WinPosStyleOrder = new WindowLocation(this.Top, this.Left);

		}

		private void TiRibbonButton_OnSelected(object sender, RoutedEventArgs e)
		{
			SelectedTabIdx = InList.RIBBON;
		}

		private void TiDlgLeft_OnSelected(object sender, RoutedEventArgs e)
		{
			SelectedTabIdx = InList.DIALOG_LEFT;
		}

		private void TiDlgRight_OnSelected(object sender, RoutedEventArgs e)
		{
			SelectedTabIdx = InList.DIALOG_RIGHT;
			OnPropertyChanged(nameof(PropOrder));
		}


		// list boxes

		private void LbxRibbon_Initialized(object sender, EventArgs e)
		{
			Lbx[(int) InList.RIBBON] = sender as ListBox;
			SelIdxRibbon = 0;
		}

		private void LbxLeft_Initialized(object sender, EventArgs e)
		{
			Lbx[(int) InList.DIALOG_LEFT] = sender as ListBox;
			SelIdxDlgLeft = 0;
		}

		private void LbxRight_Initialized(object sender, EventArgs e)
		{
			Lbx[(int) InList.DIALOG_RIGHT] = sender as ListBox;
			SelIdxDlgRight = 0;
		}


		// list controls

		private void TbxEditPosition_OnKeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
			{
				e.Handled = true;

				((TextBox) sender).MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
			}
		}

		private void BtnUp_OnClick(object sender, RoutedEventArgs e)
		{
			if (SelIdx <= 0) return;

			if (uMgr.UlMgr.Working.InLists[(int) selectedTabIdx][selIdx[(int) selectedTabIdx]-1].CurrentOrder == UnitsInListMgr.FIXED_INLIST_VALUE_DIALOG) return;

			uMgr.UlMgr.SwapProposedOrderUp(selectedTabIdx, SelIdx);

			uMgr.UlMgr.SortWorkingList(selectedTabIdx);

			Lbx[(int) selectedTabIdx].Items.Refresh();

			OnPropertyChanged(nameof(TabHasChanges));
		}

		private void BtnDn_OnClick(object sender, RoutedEventArgs e)
		{
			if (SelIdx < 0) return;

			if (selectedItem[(int) selectedTabIdx].CurrentOrder == UnitsInListMgr.FIXED_INLIST_VALUE_DIALOG) return;

			uMgr.UlMgr.SwapProposedOrderDn(selectedTabIdx, SelIdx);

			uMgr.UlMgr.SortWorkingList(selectedTabIdx);

			Lbx[(int) selectedTabIdx].Items.Refresh();

			OnPropertyChanged(nameof(TabHasChanges));
		}

		private void BtnListReset_OnClick(object sender, RoutedEventArgs e)
		{
			if (TabHasChanges)
			{
				TaskDialog td = new TaskDialog("Style Order");
				td.MainInstruction = "You have un-saved changes.";
				td.MainContent = "Select CANCEL to keep these changes\nSelect OK to lose your changes";
				td.CommonButtons = TaskDialogCommonButtons.Cancel | TaskDialogCommonButtons.Ok;
				td.DefaultButton = TaskDialogResult.Cancel;

				TaskDialogResult result = td.Show();

				if (result == TaskDialogResult.Cancel)
				{
					return;
				}
			}


			uMgr.UlMgr.ResetList(selectedTabIdx);

			refreshList(selectedTabIdx);
		}

		private void BtnListApply_OnClick(object sender, RoutedEventArgs e)
		{
			// apply one tab

			// 1. take the changes in the working inlist[] and apply to UserStyles
			// 2. notify that user styles has been updated
			// 3. update current inlist / notify changed
			// 4. (reserved)
			// 5. update working inlist / notify changed
			// 6. (reserved)
			// 7. update style order status values

			// begin here
			// get UnitStylesMgr to process
			// 

			uMgr.ApplyStyleOrderChange(selectedTabIdx);
			Lbx[(int) selectedTabIdx].Items.Refresh();

			usMgr.ChangesMade += 1;
			usMgr.StyleOrderChanges[(int) selectedTabIdx] += 1;

			OnPropertyChanged(nameof(TabHasChanges));
		}

		// dialog control buttons

		private void BtnSoCancel_OnClick(object sender, RoutedEventArgs e)
		{
			TaskDialog td = new TaskDialog("Style Order");
			td.MainInstruction = "You have un-saved changes.";
			td.MainContent = "Select CANCEL to keep these changes\nSelect OK to lose your changes";
			td.CommonButtons = TaskDialogCommonButtons.Cancel | TaskDialogCommonButtons.Ok;
			td.DefaultButton = TaskDialogResult.Cancel;

			TaskDialogResult result = td.Show();

			if (result == TaskDialogResult.Cancel) return;

			uMgr.UlMgr.ResetListAll();

			this.Close();
		}

		private void BtnSoApply_OnClick(object sender, RoutedEventArgs e)
		{
			foreach (int i in Enum.GetValues(typeof(InList)))
			{
				if (uMgr.UlMgr.Working.HasChgs[i])
				{
					usMgr.ChangesMade++;
					usMgr.StyleOrderChanges[i]++;
				}
			}

			uMgr.ApplyStyleOrderChanges();

			RefreshListAll();
		}

		private void BtnSoReset_OnClick(object sender, RoutedEventArgs e)
		{
			TaskDialog td = new TaskDialog("Style Order");
			td.MainInstruction = "You have un-saved changes.";
			td.MainContent = "Select CANCEL to keep these changes\nSelect OK to lose your changes";
			td.CommonButtons = TaskDialogCommonButtons.Cancel | TaskDialogCommonButtons.Ok;
			td.DefaultButton = TaskDialogResult.Cancel;

			TaskDialogResult result = td.Show();

			if (result == TaskDialogResult.Cancel) return;

			uMgr.UlMgr.ResetListAll();
			RefreshListAll();
		}

		private void BtnSoDone_OnClick(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		// popups

		private void BtnPopupInfoStart_OnClick(object sender, RoutedEventArgs e)
		{
			

			if (currInfoPopup!= null && currInfoPopup.Equals(CustomProperties.GetGenericPopupOne((Button) sender)))
			{
				PopupInfoClose();
				return;
			}

			PopupInfoClose();

			currInfoPopup = CustomProperties.GetGenericPopupOne((Button) sender);

			BeginInfoPopup();
		}

		private void BtnPopupClose_OnClick(object sender, RoutedEventArgs e)
		{
			PopupInfoClose();
		}


		private void BeginInfoPopup()
		{
			if (currInfoPopup == null) return;

			// Debug.WriteLine($"B) begin popup| {currInfoPopup.Name}");

			PopupInfoOpen();

			// popupInfoIsEntered = false;

			// startTimer();
		}

		private void PopupInfoOpen()
		{
			// Debug.WriteLine($"E) open popup| {currInfoPopup.Name}");

			if (!currInfoPopup.IsOpen) currInfoPopup.IsOpen = true;
		}

		private void PopupInfoClose()
		{
			if (currInfoPopup == null) return;

			// Debug.WriteLine($"F) close popup| {currInfoPopup.Name}");

			if (currInfoPopup.IsOpen) currInfoPopup.IsOpen = false;

			// Debug.WriteLine($"G) popup closed?| {!currInfoPopup.IsOpen}");

			// removeTimer();

			currInfoPopup = null;
		}

		private void PopupInfoHide()
		{
			if (currInfoPopup == null) return;

			if (currInfoPopup.IsOpen) currInfoPopup.IsOpen = false;
		}


		// window location
				
		[DebuggerStepThrough]
		private static IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			// Debug.WriteLine($"WndProc| msg is| {msg}");
			switch (msg)
			{
			case WM_NCLBUTTONDOWN:
				{
					winMoving = true;

					// Debug.WriteLine("mouse down - begin win move");

					WinLocationChanging = true;

					break;
				}
			case WM_SETCURSOR:
				{
					if (winMoving)
					{
						winMoving = false;
						// Debug.WriteLine("SETCURSOR - end win move");
						WinLocationChanging = false;
					}

					break;
				}
			}

			return IntPtr.Zero;
		}


	#endregion

	#region event publishing

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged([CallerMemberName] string memberName = "")
		{
			// Debug.WriteLine($"OnPropertyChanged| name| {memberName}");
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

		public static event PropertyChangedEventHandler StaticPropertyChanged;

		private static void OnStaticPropertyChanged([CallerMemberName] string memberName = "")
		{
			StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(memberName));
		}



	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is UnitStyleOrder";
		}

	#endregion

	}
}