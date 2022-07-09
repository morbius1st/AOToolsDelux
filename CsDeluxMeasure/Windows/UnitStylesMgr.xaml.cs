using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using Autodesk.Revit.UI;
using CsDeluxMeasure.Annotations;
using CsDeluxMeasure.UnitsUtil;
using SettingsManager;
using UtilityLibrary;
using ComboBox = System.Windows.Controls.ComboBox;
using TextBox = System.Windows.Controls.TextBox;

namespace CsDeluxMeasure.Windows
{
	/// <summary>
	/// Interaction logic for UnitStylesMgr.xaml
	/// </summary>
	public partial class UnitStylesMgr : Window, INotifyPropertyChanged
	{
	#region fixed

		public const int RIBBONFAV = 1;

		public static int RIBBONFAVORITE = 1;

		public static int RIBBONFAVS => 1;

		public static string Test = "Name";

		private readonly string[] contentSelection = new [] { "Adjust Style Order", "Adjust Saved Styles" };
		private readonly string[] closeSelection = new [] { "Done", "Save and Done" };

	#endregion

	#region fields

		// to capture when the window moves
		private static bool winLocationChanging;
		private static bool winMoving = false;
		private const int WM_SETCURSOR = 32;
		private const int WM_NCLBUTTONDOWN = 0x00a1;

		private const string STR_SHOWSTYLE_NAME = null;
		private const string STR_SHOWSTYLE_DDNAME = "Show Selected User Style";

		// collections
		private ListCollectionView styles;

		private ListCollectionView ribbonStyles;
		private ListCollectionView dialogLeftStyles;
		private ListCollectionView dialogRightStyles;

		private KeyValuePair<string, UnitsDataR> cbxSelItem;
		private List<KeyValuePair<string, string>> cbxList;
		private KeyValuePair<string, string> cbxSelectedItem;

		// objects + related
		private UnitsDataR detailUnit;
		private UnitsManager uMgr = UnitsManager.Instance;
		private UnitsDataR lbxSelItem;

		// status
		private string status;
		private bool initStatus = false;
		private int hasChanges;

		private bool showingSavedStyles = false;

		private bool? isNewNameOk = null;   // true: ok to use | false not ok to use | null neither / undefined
		private bool? isNewDescOk = null;   // true: ok to use | false not ok to use | null neither / undefined
		private bool? isInsPosOk = null;    // true: ok to use | false not ok to use | null neither / undefined
		private bool isInsPosValid = false; // true: value is OK | false: value is not ok
		private bool canAddBefore = false;
		private bool canAddAfter = false;
		private bool canStyleAdd = false;

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

	#region for popup


		// private TextBox popupTargetTbxNewName;
		// private TextBox popupTargetTbxNewDesc;

		private TextBox popupTargetTbxEditName;
		private TextBox popupTargetTbxEditDesc;
		private TextBox popupTargetTbxEditSample;
		
		private CheckBox popupTargetCkbxRibbonFavs;
		private CheckBox popupTargetCkbxDialogLeft;
		private CheckBox popupTargetCkbxDialogRight;

		private int currPopupIdx = -1;
		private Popup[] popups = new Popup[20];
		private byte editNamePopup = 0;
		private byte editDescPopup = 1;
		private byte editSamplePopup = 2;

		private byte popupRibbonFavs = 3;


		private bool popupIsEntered;
		private DoubleAnimation d = new DoubleAnimation(1, new Duration(TimeSpan.FromSeconds(2)));
		private AnimationClock popupAniClock;

		private DispatcherTimer timer;
		private bool timerActive;

	#endregion

		private bool cbx1UnitSelect = false;

		private int insPosition;

		// contained controls
		private static UnitStylesMgr me;
		private ListBox lbx;
		private ComboBox cbx;

		// indicies
		private int lbxSelIndex;
		private int cbxSelIndex;
		private int dialogIndex;

		// properties
		private Image img;
		private string newName;
		private string newDesc;


	#endregion

	#region ctor

		public UnitStylesMgr()
		{
			me = this;

			InitializeComponent();

			uMgr.ReadUnitSettings();

			OnPropertyChanged(nameof(StylesView));

			Init();

			status = "init";
		}

	#endregion

	#region public properties

		public Image Imgs => img;

		// the current displayed unit setting information
		public UnitsDataR DetailUnit
		{
			get => detailUnit;
			set
			{
				detailUnit = value;
				if (detailUnit != null) detailUnit.UpdateProperties();

				OnPropertyChanged(nameof(DetailUnit));
			}
		}

		// this is the collection being displayed
		public ListCollectionView StylesView => styles;

		public Dictionary<string, UnitsDataR> StdStyles => uMgr.StdStyles;

		public UnitsDataR LbxSelItem
		{
			get => lbxSelItem;
			set
			{
				lbxSelItem = value;
				OnPropertyChanged();

				showSavedStyle(lbxSelItem);
			}
		}

		public List<KeyValuePair<string, string>> CbxList => cbxList;

		public KeyValuePair<string, string> CbxSelectedItem
		{
			get => cbxSelectedItem;
			set
			{
				cbxSelectedItem = value;
				OnPropertyChanged();

				showUnitStyleSettings(value.Value);
			}
		}


		public int Count => styles.Count;

		public string NewName
		{
			get => newName;
			set
			{
				string newValue = null;

				if (!value.IsVoid())
				{
					newValue = value.TrimEnd();

					if (newValue.Equals(newName)) return;

					IsNewNameOk = ValidateNewName(newValue);

					// if (!isNewNameOk.Value) return;
				}
				else
				{
					IsNewNameOk = null;
				}

				newName = newValue;

				OnPropertyChanged();

				DetermineCanAdd();
			}
		}

		public string NewDesc
		{
			get => newDesc;
			set
			{
				string newValue = null;

				if (!value.IsVoid())
				{
					newValue = value.TrimEnd();

					if (newValue.Equals(newDesc)) return;

					IsNewDescOk = ValidateNewDesc(newValue);

					// if (!isNewNameOk.Value) return;
				}
				else
				{
					IsNewDescOk = null;
				}

				newDesc = newValue;

				OnPropertyChanged();

				DetermineCanAdd();
			}
		}

		public string ContentType => contentSelection[dialogIndex];

		public string CloseType => closeSelection[hasChanges > 1 ? 1 : 0];


		// new name / new description properties

		public static bool WinLocationChanging
		{
			get => winLocationChanging;
			set
			{
				winLocationChanging = value;

				OnStaticPropertyChanged();
			}
		}


		// status 

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


		public bool Cbx1UnitSelect
		{
			get => cbx1UnitSelect;

			set
			{
				if (value == cbx1UnitSelect) return;
				cbx1UnitSelect = value;
				OnPropertyChanged();
			}
		}

		public int HasChanges
		{
			get => hasChanges;
			set
			{
				if (value == hasChanges) return;

				hasChanges = value;
				OnPropertyChanged();
				OnPropertyChanged(nameof(CloseType));
			}
		}

		public bool IsModified => hasChanges > 0;

		public bool? IsNewNameOk
		{
			get => isNewNameOk;
			set
			{
				// if (value.HasValue && value.Equals(cvrtToBool(isNewNameOk))) return;

				isNewNameOk = value;
				OnPropertyChanged();
			}
		}

		public bool? IsNewDescOk
		{
			get => isNewDescOk;
			set
			{
				// if (value.HasValue && value.Equals(cvrtToBool(isNewDescOk))) return;

				isNewDescOk = value;
				OnPropertyChanged();

				DetermineCanAdd();
			}
		}

		public bool? IsInsPosOk
		{
			get => isInsPosOk;
		}

		public bool CanAddBefore
		{
			get => canAddBefore;
			set
			{
				if (value == canAddBefore) return;

				canAddBefore = value;

				OnPropertyChanged();
			}
		}

		public bool CanAddAfter
		{
			get => canAddAfter;
			set
			{
				if (value == canAddAfter) return;

				canAddAfter = value;

				OnPropertyChanged();
			}
		}

		public bool CanStyleAdd
		{
			get => canStyleAdd;

			set
			{
				if (value == canStyleAdd) return;
				canStyleAdd = value;
				OnPropertyChanged();
			}
		}

		// indexes and positions

		public int DialogIndex
		{
			get => dialogIndex;
			set
			{
				if (value == dialogIndex) return;
				dialogIndex = value;
				OnPropertyChanged();
			}
		}

		public int CbxSelIndex
		{
			get => cbxSelIndex;
			set
			{
				cbxSelIndex = value;
				OnPropertyChanged();
			}
		}

		public int InsPosition
		{
			get => insPosition;
			set
			{
				if (value == insPosition) return;

				insPosition = value;
				OnPropertyChanged();

				bool result = ValidateInsPos(value);

				if (result)
				{
					isInsPosValid = true;

					LbxSelIndex = value - 1;
				}
				else
				{
					isInsPosValid = false;
				}

				DetermineCanAdd();
			}
		}

		public int LbxSelIndex
		{
			get => lbxSelIndex;
			set
			{
				if (value == lbxSelIndex) return;
				lbxSelIndex = value;
				OnPropertyChanged();

				if (lbx != null)
				{
					UnitsDataR udr = (UnitsDataR) styles.GetItemAt(lbxSelIndex);
					lbx.ScrollIntoView(udr);
				}

				insPosition = value + 1;
				OnPropertyChanged(nameof(InsPosition));
			}
		}



		public TextBox PopupTargetTbxEditName
		{
			get => popupTargetTbxEditName;
			set
			{
				popupTargetTbxEditName = value;
				OnPropertyChanged();
			}
		}

		public TextBox PopupTargetTbxEditDesc
		{
			get => popupTargetTbxEditDesc;
			set
			{
				popupTargetTbxEditDesc = value;
				OnPropertyChanged();
			}
		}

		public TextBox PopupTargetTbxEditSample
		{
			get => popupTargetTbxEditSample;
			set
			{
				popupTargetTbxEditSample = value;
				OnPropertyChanged();
			}
		}

		public CheckBox PopupTargetCkbxRibbonFavs
		{
			get => popupTargetCkbxRibbonFavs;
			set
			{
				popupTargetCkbxRibbonFavs = value;
				OnPropertyChanged();
			}
		}

	#endregion

	#region private properties

	#endregion

	#region public methods

		public bool ValidateInsPos(int pos)
		{
			// rules
			// cannot be 1 or less
			// cannot be greater than last number

			return pos > 1 && pos <= Count;
		}

		public bool ValidateNewName(string newName)
		{
			// rules
			// cannot stat with a number
			// minimum of 5 characters

			if (newName.Length < 5 || newName[0] == ' ' ||
				(newName[0] >= '0' && newName[0] <= '9')) return false;

			return uMgr.UsrStyleList.FindIndex(x => x.Ustyle.Name.Equals(newName)) == -1;
		}

		public bool ValidateNewDesc(string newName)
		{
			return newName.Length > 9 && newName[0] != ' ';
		}

		public void SetPosition(Window w)
		{
			double t = UserSettings.Data.WinPosUnitStyleMgr.Top;
			double l = UserSettings.Data.WinPosUnitStyleMgr.Left;

			this.Owner = w;

			this.Top = t > 0 ? t : w.Top;
			this.Left = l > 0 ? l : w.Left;
		}

		public void ProcessStyleChanges()
		{
			bool resultR = setStyleList(ribbonStyles, (int) InList.RIBBON);
			bool resultDl = setStyleList(dialogLeftStyles, (int) InList.DIALOG_LEFT);
			bool resultDr = setStyleList(dialogRightStyles, (int) InList.DIALOG_RIGHT);
		}

	#endregion

	#region private methods

		private void Init()
		{
			initStatus = true;

			HasChanges = 0;

			configView();
			configCbxList();

			DialogIndex = 0;

			InsPosition = 2;

			LbxSelIndex = 3;
			Cbx1UnitSelect = true;

			OnPropertyChanged(nameof(StylesView));

			initStatus = false;
		}

		private void configView()
		{
			uMgr.SetInitialSequence();

			styles = (ListCollectionView) CollectionViewSource.GetDefaultView(uMgr.UsrStyleList);
			styles.CurrentChanged += Styles_CurrentChanged;

			styles.SortDescriptions.Add(
				new SortDescription("Sequence", ListSortDirection.Ascending));
			styles.Filter = isNotDeleted;

			styles.IsLiveSorting = true;
		}

		private void configCbxList()
		{
			cbxList = new List<KeyValuePair<string, string>>();

			foreach (KeyValuePair<string, UnitsDataR> kvp in StdStyles)
			{
				cbxList.Add(new KeyValuePair<string, string>(kvp.Value.DropDownName, kvp.Value.Name));
			}


			cbxList.Add(new KeyValuePair<string, string>(STR_SHOWSTYLE_DDNAME, STR_SHOWSTYLE_NAME));
		}

		private bool cvrtToBool(bool? b)
		{
			return b.HasValue ? b.Value : false;
		}

		private void DetermineCanAdd()
		{
			bool newName = cvrtToBool(isNewNameOk);
			bool newDesc = cvrtToBool(isNewDescOk);

			isInsPosOk = (isNewNameOk & isInsPosValid) | (isNewDescOk & isInsPosValid);

			OnPropertyChanged(nameof(IsInsPosOk));

			CanAddBefore = newName && newDesc && isInsPosValid;
			CanAddAfter = newName && newDesc;
		}

		private void showUnitStyleSettings(string name)
		{
			if (name != null)
			{
				showingSavedStyles = false;
				DetailUnit = StdStyles[name];
			}
			else
			{
				showingSavedStyles = true;
				showSavedStyle(LbxSelItem);
			}
		}

		private void showUnitStyleSettings(UnitsDataR udr)
		{
			if (udr.Ustyle.UnitClass == UnitClass.CL_CONTROL)
			{
				showingSavedStyles = true;

				showSavedStyle(LbxSelItem);
			}
			else
			{
				showingSavedStyles = false;
				// CanEditStyle = false;
				UnitsDataR ur = getStdStyleFromName(udr.Name);

				if (ur != null)
				{
					DetailUnit = ur;
				}
			}
		}

		private void showSavedStyle(UnitsDataR udr)
		{
			if (!showingSavedStyles) return;

			DetailUnit = udr;
		}

		private void setUnitDisplay(string name)
		{
			UnitsDataR udr = getStdStyleFromName(name);

			if (udr != null)
			{
				DetailUnit = udr;
			}
		}

		private UnitsDataR getStdStyleFromName(string name)
		{
			UnitsDataR udr = null;

			if (StdStyles.ContainsKey(name))
			{
				udr = StdStyles[name];

				if (udr.Ustyle.UnitClass == UnitClass.CL_PROJECT)
				{
					return uMgr.ProjectUnitStyle;
				}
			}

			return udr;
		}

		private T FindElementByName<T>(FrameworkElement element, string sChildName) where T : FrameworkElement
		{
			T childElement = null;
			var nChildCount = VisualTreeHelper.GetChildrenCount(element);
			for (int i = 0; i < nChildCount; i++)
			{
				FrameworkElement child = VisualTreeHelper.GetChild(element, i) as FrameworkElement;

				if (child == null)
					continue;

				if (child is T && child.Name.Equals(sChildName))
				{
					childElement = (T) child;
					break;
				}

				childElement = FindElementByName<T>(child, sChildName);

				if (childElement != null)
					break;
			}

			return childElement;
		}

		private void setInitialSequence() { }

		private bool isNotDeleted(object obj)
		{
			UnitsDataR udr = obj as UnitsUtil.UnitsDataR;

			return !udr.DeleteStyle;
		}

		private bool setStyleList(ListCollectionView view, int thisList)
		{
			view = (ListCollectionView) CollectionViewSource.GetDefaultView(uMgr.UsrStyleList);

			view.Filter = item =>
			{
				if (item == null) return false;
				UnitsDataR udr = (UnitsDataR) item;

				return udr.Ustyle.ShowIn(thisList);
			};

			return view.Count > 0;
		}

		private static IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			// Debug.WriteLine($"msg is| {msg}");
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

		[NotifyPropertyChangedInvocator]
		[DebuggerStepThrough]
		private void OnPropertyChanged([CallerMemberName] string memberName = "")
		{
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
			return "this is class1";
		}

	#endregion

	#region event consuming

		private void WinUnitStyle_Loaded(object sender, RoutedEventArgs e)
		{
			popups[editNamePopup] = CsWpfUtilities.FindElementByName<Popup>(this, "PuEditName");
			popups[editDescPopup] = CsWpfUtilities.FindElementByName<Popup>(this, "PuEditDesc");
			popups[editSamplePopup] = CsWpfUtilities.FindElementByName<Popup>(this, "PuEditSample");
			popups[popupRibbonFavs] = CsWpfUtilities.FindElementByName<Popup>(this, "PuRibbonFavs");


			HwndSource source = HwndSource.FromHwnd(new WindowInteropHelper(this).Handle);
			source.AddHook(new HwndSourceHook(WndProc));
		}

		private void WinUnitStyle_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			UserSettings.Data.WinPosUnitStyleMgr = new WindowLocation(this.Top, this.Left);
			UserSettings.Admin.Write();
		}


		private void lbx_Initialized(object sender, EventArgs e)
		{
			lbx = (ListBox) sender;
		}

		private void CbxStdStyles_Initialized(object sender, EventArgs e)
		{
			cbx = (ComboBox) sender;
			// cbx.SelectedIndex = 1;
		}

		private void Styles_CurrentChanged(object sender, EventArgs e)
		{
			if (initStatus) return;

			status = "current changed";
		}


		private void BtnChgTemplate_OnClick(object sender, RoutedEventArgs e)
		{
			ProcessStyleChanges();
		}

		private void BtnUp_OnClick(object sender, RoutedEventArgs e)
		{
			if (lbxSelIndex <= 1) return;

			int atIdx = ((UnitsDataR) styles.GetItemAt(lbxSelIndex)).Sequence;
			((UnitsDataR) styles.GetItemAt(lbxSelIndex)).Sequence -= 1;

			int pastIdx = ((UnitsDataR) styles.GetItemAt(lbxSelIndex - 1)).Sequence;
			((UnitsDataR) styles.GetItemAt(lbxSelIndex - 1)).Sequence += 1;

			HasChanges += 1;
			Debug.WriteLine("got move up");
		}

		private void BtnDn_OnClick(object sender, RoutedEventArgs e)
		{
			if (lbxSelIndex == Count - 1 || lbxSelIndex == 0) return;

			((UnitsDataR) styles.GetItemAt(lbxSelIndex)).Sequence += 1;
			((UnitsDataR) styles.GetItemAt(lbxSelIndex + 1)).Sequence -= 1;

			HasChanges += 1;
			Debug.WriteLine("got move down");
		}

		private void BtnDelete_OnClick(object sender, RoutedEventArgs e)
		{
			// Debug.WriteLine("");
			// Debug.WriteLine($"selIdx: {SelIndex:D2}  name: {selItem.Ustyle.Name}");
			// Debug.WriteLine($"   pos: {styles.CurrentPosition:D2}");
			// Debug.WriteLine("");

			if (lbxSelItem.Ustyle.IsLocked) return;

			lbxSelItem.DeleteStyle = true;
			lbxSelItem.Sequence = 0;

			int idx = lbxSelIndex;
			int count = styles.Count;

			if (lbxSelIndex < Count - 1)
			{
				uMgr.ReSequenceStylesList(styles, lbxSelIndex + 1);
			}

			HasChanges += 1;
			// Debug.WriteLine("got delete");

			styles.Refresh();

			LbxSelIndex = idx <= styles.Count - 1 ? idx : styles.Count - 1;

			// showList();
		}

		private void showList()
		{
			Debug.WriteLine("");
			Debug.WriteLine($"view count: {styles.Count}");
			Debug.WriteLine($"list count: {uMgr.UsrStyleList.Count}");
			Debug.WriteLine($"this count: {Count}");
			Debug.WriteLine("");

			int i = 0;
			foreach (UnitsDataR udr in uMgr.UsrStyleList)
			{
				string seq = udr.Sequence == 0 ? "  " : $"{(udr.Sequence + 1):D2}";
				string iseq = udr.InitialSequence == 0 ? "  " : $"{(udr.Sequence + 1):D2}";


				Debug.WriteLine($"idx: {i++:D2}  seq: {seq}  intseq:  {udr.InitialSequence:D2}  seq: {udr.Sequence:D2}   isDel: {udr.DeleteStyle,6}  name: {udr.Ustyle.Name}");
			}

			Debug.WriteLine("");
			Debug.WriteLine("");
		}

		private void BtnDebug_OnClick(object sender, RoutedEventArgs e)
		{
			int idx = LbxSelIndex;

			UnitsDataR udr = DetailUnit;
			UStyle us = udr.Ustyle;

			UnitsDataR udrl = lbxSelItem;
			UStyle usl = udrl.Ustyle;

			KeyValuePair<string, UnitsDataR> kvp = cbxSelItem;
			string kname = kvp.Value.Ustyle.Name;

			// not the one
			// UnitsDataR udrv = cbxSelValue;
			// UStyle usc = udrv.Ustyle;
			// string name = usc.Name;

			NewName = udr.Ustyle.Name;
			NewDesc = udr.Ustyle.Description;

			Debug.WriteLine("@debug button");
		}

		private void BtnDone_OnClick(object sender, RoutedEventArgs e)
		{
			if (IsModified)
			{
				applyChanges();
			}

			this.Close();
		}

		private void applyChanges()
		{
			uMgr.WriteUser();

			HasChanges = 0;

			styles.Refresh();
		}

		private void BtnApply_OnClick(object sender, RoutedEventArgs e)
		{
			applyChanges();
		}

		private void BtnCancel_OnClick(object sender, RoutedEventArgs e)
		{
			if (IsModified)
			{
				TaskDialog td = new TaskDialog("Delux Measure");
				td.MainInstruction = "You have un-saved changes.";
				td.MainContent = "Select CANCEL to return\nSelect OK to proceed and lose these changes";
				td.CommonButtons = TaskDialogCommonButtons.Cancel | TaskDialogCommonButtons.Ok;
				td.DefaultButton = TaskDialogResult.Cancel;

				TaskDialogResult result = td.Show();

				if (result == TaskDialogResult.Cancel) return;
			}

			this.Close();
		}

		private void BtnReset_OnClick(object sender, RoutedEventArgs e)
		{
			int idx = lbxSelIndex;

			// showList();

			uMgr.UnDelete();
			uMgr.ResetInitialSequence();

			HasChanges = 0;

			styles.Refresh();

			// showList();

			LbxSelIndex = idx;
		}

		private void BtnAddBefore_OnClick(object sender, RoutedEventArgs e) { }

		private void BtnAddLast_OnClick(object sender, RoutedEventArgs e)
		{
			UnitsDataR udr = uMgr.NewUDR(detailUnit, newName, newDesc, uMgr.UsrStyleList.Count);


			// styles.AddNew();
			styles.AddNewItem(udr);
			styles.CommitNew();
		}

		private void Btn_NameEntryClear_OnClick(object sender, RoutedEventArgs e)
		{
			NewName = null;
		}

		private void Btn_DescEntryClear_OnClick(object sender, RoutedEventArgs e)
		{
			NewDesc = null;
		}

		private void BtnFavCkbxStatus_OnClick(object sender, RoutedEventArgs e)
		{
			int favButtonIdx;
			bool result = Int32.TryParse((string) ((Button) sender).Tag, out favButtonIdx);

			if (!result) return;

			LbxSelItem.ActiveElement = favButtonIdx;

			TaskDialog td = new TaskDialog("title");

			td.MainIcon = TaskDialogIcon.TaskDialogIconInformation;

			td.MainInstruction = $"button {favButtonIdx} pressed";

			td.Show();

			LbxSelItem.ActiveElement = -1;
		}

	#endregion

	#region popup

		private void BtnRibbonFavs_OnClick(object sender, RoutedEventArgs e)
		{
			PopupTargetCkbxRibbonFavs = (CheckBox) ((Button) sender).Tag;

			beginPopup(popupRibbonFavs);
		}

		private void BtnEditName_OnClick(object sender, RoutedEventArgs e)
		{
			PopupTargetTbxEditName = (TextBox) ((Button) sender).Tag;

			beginPopup(editNamePopup);
		}

		private void BtnEditDesc_OnClick(object sender, RoutedEventArgs e)
		{
			PopupTargetTbxEditDesc = (TextBox) ((Button) sender).Tag;

			beginPopup(editDescPopup);
		}

		private void BtnEditSample_OnClick(object sender, RoutedEventArgs e)
		{
			PopupTargetTbxEditSample = (TextBox) ((Button) sender).Tag;

			beginPopup(editSamplePopup);
		}

		private void beginPopup(byte popupId)
		{
			if (currPopupIdx >= 0)
			{
				return;
			}

			popupOpen(popupId);

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

		public void popupClose()
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

	#endregion
	}
}