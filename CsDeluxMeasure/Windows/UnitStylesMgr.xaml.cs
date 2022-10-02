using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interop;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using CsDeluxMeasure.Annotations;
using CsDeluxMeasure.UnitsUtil;
using CsDeluxMeasure.Windows.Support;
using SettingsManager;
using UtilityLibrary;
using static CsDeluxMeasure.Windows.Support.UnitStylesMgrWinData;
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

		private readonly string[] contentSelection = new [] { "Adjust Style Order", "Adjust Saved Styles" };
		private readonly string[] closeSelection = new [] { "Done", "Save and Done" };

	#endregion

	#region fields

		// to capture when the window moves
		private static bool winLocationChanging;
		private static bool winMoving = false;
		private bool winLostFocus;
		private const int WM_SETCURSOR = 32;
		private const int WM_NCLBUTTONDOWN = 0x00a1;

		private const string STR_SHOWSTYLE_NAME = null;
		private const string STR_SHOWSTYLE_DDNAME = "Show Selected User Style";

		// collections
		// private ListCollectionView wkgUserStyles;

		private ListCollectionView ribbonStyles;
		private ListCollectionView dialogLeftStyles;
		private ListCollectionView dialogRightStyles;

		private KeyValuePair<string, UnitsDataR> cbxSelItem;
		private List<KeyValuePair<string, string>> cbxList;
		private KeyValuePair<string, string> cbxSelectedItem;

		// objects + related
		private UnitsManager uMgr = UnitsManager.Instance;
		// private static UnitsSupport uSup;

		private UnitsDataR detailUnit;
		private UnitsDataR lbxSelItem;

		// status

		private int changesMade;
		private int[] orderChanges = new int[UnitData.INLIST_COUNT];

		private bool showingSavedStyles = false;

	#region for item list

		// control settings only for style list
		private bool isSelected = false;
		private bool? isEditing = false;
		private bool? isReadOnly = false;
		private bool? isLocked = false;

	#endregion

	#region for add style

		// new style

		private string validateFailDesc;

		private bool canAddBefore = false;
		private bool canAddAfter = false;
		private bool canStyleAdd = false;

		private string newName;
		private bool? isNewNameOk = null; // true: ok to use | false not ok to use | null neither / undefined
		private ValMsgNameId newNameMsgId;
		private string newNameToolTip;


		private string newDesc;
		private bool? isNewDescOk = null; // true: ok to use | false not ok to use | null neither / undefined
		private ValMsgDescId newDescMsgId;
		private string newDescToolTip;

		private int insPosition;
		private bool? isInsPosOk = null;    // true: ok to use | false not ok to use | null neither / undefined
		private bool isInsPosValid = false; // true: value is OK | false: value is not ok
		private string insPosToolTip;

		// control settings for add unit are

		private int priorValueIdx = -1;
		private string[] priorValue = new string[UnitStylesMgrWinData.POPUP_COUNT];

	#endregion

	#region for editing a style

		// editing style

		private string editNameToolTip;
		private string editDescToolTip;
		private string editSampleToolTip;

	#endregion

	#region for popup

		private Popup currInfoPopup;
		private Popup currEditPopup;

	#endregion

		private bool cbx1UnitSelect = false;


		// contained controls
		private static UnitStylesMgr me;
		private ListBox lbx;


		// indicies

		private int lbxSelIndex;
		private int cbxSelIndex;
		private int dialogIndex;

	#endregion

	#region ctor

		// static UnitStylesMgr() { uSup = new UnitsSupport();}

		public UnitStylesMgr()
		{
			me = this;

			InitializeComponent();

			init();
		}

	#endregion

	#region public properties

		public UnitsManager UnitsManager => uMgr;

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

		public Dictionary<string, UnitsDataR> StdStyles => uMgr.StdStyles;


		// this is the collection being displayed
		public ListCollectionView WkgUserStylesView => uMgr.WkgUserStylesView;

		public UnitsDataR LbxSelItem
		{
			get => lbxSelItem;
			set
			{
				lbxSelItem = value;

				OnPropertyChanged();

				// LbxSelItemCopy = lbxSelItem.Clone();

				showSavedStyle(lbxSelItem);
			}
		}

		public int LbxSelIndex
		{
			get => lbxSelIndex;
			set
			{
				if (value == lbxSelIndex || value < 0) return;
				lbxSelIndex = value;


				if (lbx != null)
				{
					UnitsDataR udr = (UnitsDataR) WkgUserStylesView.GetItemAt(lbxSelIndex);
					lbx.ScrollIntoView(udr);
				}

				OnPropertyChanged();
				InsPosition = value + 1;

				PopupEditOpsClose();
				PopupInfoClose();
			}
		}

		public int Count => WkgUserStylesView.Count;


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


		// new style information

		public string NewName
		{
			get => newName;
			set
			{
				string proposed = value.TrimEnd();

				string tooltip = "";
				bool? result = false;

				if (!proposed.IsVoid())
				{
					result = detailUnit.ValidateName(proposed, out newNameMsgId);
					tooltip = NameErrMsgs[(int) newNameMsgId];

					if (hasNameInUserStyles(proposed))
					{
						result = false;
						tooltip = NameErrMsgs[(int) ValMsgNameId.VN_MUST_BE_UNIQUE];
					}
				}

				IsNewNameOk = result;
				NewNameToolTip = tooltip;

				newName = proposed;
				OnPropertyChanged();

				DetermineCanAdd();
			}
		}

		public string NewDesc
		{
			get => newDesc;
			set
			{
				string proposed = value.TrimEnd();
				string tooltip = "";
				bool? result = false;

				if (!proposed.IsVoid())
				{
					result = detailUnit.ValidateDesc(proposed, out newDescMsgId);
					tooltip = DescErrMsgs[(int) newDescMsgId];
				}

				IsNewDescOk = result;
				NewDescToolTip = tooltip;

				newDesc = proposed;
				OnPropertyChanged();

				DetermineCanAdd();
			}
		}

		public int InsPosition
		{
			get => insPosition;
			set
			{
				insPosition = value;

				isInsPosValid = ValidateInsPos(value);
				InsPosToolTip = validateFailDesc;

				if (isInsPosValid)
				{
					lbxSelIndex = value - 1;
					OnPropertyChanged(nameof(LbxSelIndex));
				}

				OnPropertyChanged();

				DetermineCanAdd();
			}
		}


		public string NewNameToolTip
		{
			get => newNameToolTip;
			set
			{
				if (newNameToolTip?.Equals(value) ?? false) return;

				newNameToolTip = value;

				OnPropertyChanged();
			}
		}

		public string NewDescToolTip
		{
			get => newDescToolTip;
			set
			{
				if (newDescToolTip?.Equals(value) ?? false) return;

				newDescToolTip = value;

				OnPropertyChanged();
			}
		}

		public string InsPosToolTip
		{
			get => insPosToolTip;
			set
			{
				if (insPosToolTip?.Equals(value) ?? false) return;

				insPosToolTip = value;

				OnPropertyChanged();
			}
		}

		public string EditNameToolTip
		{
			get => editNameToolTip;
			set
			{
				if (editNameToolTip?.Equals(value) ?? false) return;

				editNameToolTip = value;

				OnPropertyChanged();
			}
		}

		public string EditDescToolTip
		{
			get => editDescToolTip;
			set
			{
				if (editDescToolTip?.Equals(value) ?? false) return;

				editDescToolTip = value;

				OnPropertyChanged();
			}
		}

		public string EditSampleToolTip
		{
			get => editSampleToolTip;
			set
			{
				if (editSampleToolTip?.Equals(value) ?? false) return;

				editSampleToolTip = value;

				OnPropertyChanged();
			}
		}


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


		// status 

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

		public bool HasStyleOrderChanges => hasStyleOrderChanges();


		public int[] StyleOrderChanges => orderChanges;

		public int ChangesMade
		{
			get => changesMade;
			set
			{
				if (value == changesMade) return;

				changesMade = value;
				OnPropertyChanged();
				OnPropertyChanged(nameof(HasChanges));
				OnPropertyChanged(nameof(HasNoChanges));
			}
		}

		public bool HasChanges => changesMade > 0;
		public bool HasNoChanges => changesMade == 0;

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
			set
			{
				if (isInsPosOk == value) return;

				isInsPosOk = value;

				OnPropertyChanged();
			}
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

	#endregion

	#region private properties

		private WindowLocation _Location { get; set; } = new WindowLocation(0, 0);

	#endregion

	#region public methods

		public void SetPosition(Window w)
		{
			double t = UserSettings.Data.WinPosUnitStyleMgr.Top;
			double l = UserSettings.Data.WinPosUnitStyleMgr.Left;
		
			this.Owner = w;
		
			this.Top = t > 0 ? t : w.Top;
			this.Left = l > 0 ? l : w.Left;
		}

	#endregion

	#region private methods

		// init setup
		private void init()
		{
			// uMgr.ReadUnitSettings();
			uMgr.UpdateProjectStyleSettings();
			uMgr.initUserStylesView();
			uMgr.BackupUserStyleList();

			OnPropertyChanged(nameof(WkgUserStylesView));

			initCbxList();

			DialogIndex = 0;

			ChangesMade = 0;

			Cbx1UnitSelect = true;

			UnitsDataR.OnNameChanging += OnEditedNameChanging;
			UnitsDataR.OnNameChanged += UnitsDataROnNameChanged;
			UnitsDataR.OnDescriptionChanged += UnitsDataROnDescriptionChanged;

			UStyle.ShowInChanged += OnShowInChanged;

		}

		private void initCbxList()
		{
			cbxList = new List<KeyValuePair<string, string>>();

			foreach (KeyValuePair<string, UnitsDataR> kvp in StdStyles)
			{
				cbxList.Add(new KeyValuePair<string, string>(kvp.Value.DropDownName, kvp.Value.Ustyle.Name));
			}

			cbxList.Add(new KeyValuePair<string, string>(STR_SHOWSTYLE_DDNAME, STR_SHOWSTYLE_NAME));

			showUnitStyleSettings(cbxList[0].Value);
		}

		// user style

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

		private void resetUserStyles()
		{
			int idx = lbxSelIndex;

			uMgr.ResetUserStyleList();
			uMgr.UpdateProjectStyleSettings();
			uMgr.initUserStylesView();
			uMgr.BackupUserStyleList();

			OnPropertyChanged(nameof(WkgUserStylesView));

			DialogIndex = 0;

			LbxSelIndex = idx >= Count ? Count - 1 : idx;

			ChangesMade = 0;
		}

		private void resetUserStylesToDefault()
		{
			uMgr.ResetUserStylesToDefault();
			uMgr.UnDelete();
			uMgr.UpdateProjectStyleSettings();
			uMgr.initUserStylesView();
			uMgr.BackupUserStyleList();

			OnPropertyChanged(nameof(WkgUserStylesView));

			DialogIndex = 0;

			LbxSelIndex = 2;
			ChangesMade = 1;
		}

		private bool setUserStyles(ListCollectionView view, int thisList)
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

		private void showSavedStyle(UnitsDataR udr)
		{
			if (!showingSavedStyles) return;

			DetailUnit = udr;
		}

		private void showUnitStyleSettings(string name)
		{
			if (name != null)
			{
				showingSavedStyles = false;
				CanStyleAdd = true;

				if (StdStyles[name].Ustyle.UnitClass == UnitClass.CL_PROJECT)
				{
					DetailUnit = uMgr.ProjectUnitStyle;
				}
				else
				{
					DetailUnit = StdStyles[name];
				}
			}
			else
			{
				showingSavedStyles = true;
				showSavedStyle(LbxSelItem);
				CanStyleAdd = false;
			}
		}

		private void resetNewStyleInfo()
		{
			// string oldName = newName;
			// string oldDesc = newDesc;

			// newName = "";
			// newDesc = "";
			insPosition = Count < 2 ? Count : 2;

			IsNewNameOk = false;
			NewNameToolTip = NameErrMsgs[(int) ValMsgNameId.VN_MUST_BE_UNIQUE];

			IsNewDescOk = true;

			IsInsPosOk = null;


			OnPropertyChanged(nameof(InsPosition));

			// NewName = oldName;
		}

		// tests

		private void DetermineCanAdd()
		{
			bool newNameOk = cvrtToBool(isNewNameOk);
			bool newDescOk = cvrtToBool(isNewDescOk);


			if (!isInsPosOk.HasValue)
			{
				if (newNameOk && newDescOk && isInsPosValid)
				{
					IsInsPosOk = true;
				}
				else
				{
					IsInsPosOk = null;
				}
			}
			else
			{
				if (newNameOk && newDescOk)
				{
					IsInsPosOk = true;
				}
				else
				{
					IsInsPosOk = null;
				}
			}

			bool insPosOk = cvrtToBool(isInsPosOk);

			CanAddBefore = newNameOk && newDescOk && insPosOk;
			CanAddAfter = newNameOk && newDescOk;
		}

		/*

		inspostiion:
		generally neutral / null

		 * if inspos is null => neutral / null / no color
		 *
		 * if name entered and is ok + inspos is ok => green
		 * and
		 * if desc entered and is ok + inspos is ok => green
		 * and
		 * if inspos not ok => red

		 after add:
			name has old value => red (already used)
			desc has old value => neutral / null / no color
			pos has a value => neutral / null / no color

		*/

		private bool isNotDeleted(object obj)
		{
			UnitsDataR udr = obj as UnitsUtil.UnitsDataR;

			return !udr.DeleteStyle;
		}


		// validate data

		private bool ValidateInsPos(int pos)
		{
			// rules
			// cannot be 1 or less
			// cannot be greater than last number

			validateFailDesc = null;

			if (pos < 2)
			{
				validateFailDesc = "Position cannot be 1 or less";
				return false;
			}

			if (pos > Count)
			{
				validateFailDesc = "Position is too large";
				return false;
			}

			return true;
		}

		private bool hasNameInUserStyles(string testName)
		{
			// return true if found / name already exists
			foreach (UnitsDataR udr in WkgUserStylesView)
			{
				if (udr.Ustyle.Name.Equals(testName)) return true;
			}

			return false;
		}

		private void OnEditedNameChanging(object sender, ChangeValueEventArgs<ValMsgNameId> e)
		{
			// the data class does the basic syntax validation
			// only need to verify that the name is unique

			if (!lbxSelItem.Ustyle.Name.Equals(e.Proposed) && hasNameInUserStyles(e.Proposed))
			{
				e.Response = ValMsgNameId.VN_MUST_BE_UNIQUE;
				e.Cancel = true;
			}
		}

		private void UnitsDataROnDescriptionChanged(object sender, EventArgs e)
		{
			ChangesMade += 1;
		}

		private void UnitsDataROnNameChanged(object sender, EventArgs e)
		{
			ChangesMade += 1;
		}

		// private void OnOnShowInChanged(object sender, CheckBoxChangedEventArgs e)
		// {
		// 	ChangesMade += 1;
		// }

		private void applyChanges()
		{
			uMgr.UlMgr.Current.InListViewRibbon.Refresh();

			uMgr.UlMgr.ApplyRibbonButtonChangesToCurrent();

			uMgr.WriteUserStyles();

			ChangesMade = 0;

			WkgUserStylesView.Refresh();
		}

		private string getDefaultNewStyleText()
		{
			if (priorValueIdx == UnitStylesMgrWinData.POPUP_NEW_STYLE_NAME)
			{
				return $"{detailUnit.Ustyle.Name} Copy";
			}

			return $"Based on: {detailUnit.Ustyle.Name}";
		}

		private string getEditStyleResetText()
		{
			if (priorValueIdx == POPUP_STYLE_NAME)
			{
				return lbxSelItem.Ustyle.Name;
			}
			else if (priorValueIdx == POPUP_STYLE_DESC)
			{
				return lbxSelItem.Ustyle.Description;
			}

			return "0.0";
		}

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

		private bool cvrtToBool(bool? b)
		{
			return b.HasValue ? b.Value : false;
		}

		private bool hasStyleOrderChanges()
		{
			return orderChanges[0] > 0 || orderChanges[1] > 0 || orderChanges[2] > 0;
		}

		private bool verifyDialogClose()
		{
			PopupEditOpsClose();
			PopupInfoClose();

			if (HasChanges)
			{
				TaskDialog td = new TaskDialog("Unit Styles");
				td.MainInstruction = "You have un-saved changes.";
				td.MainContent = "Select CANCEL to keep these changes\nSelect OK to proceed and lose these changes";
				td.CommonButtons = TaskDialogCommonButtons.Cancel | TaskDialogCommonButtons.Ok;
				td.DefaultButton = TaskDialogResult.Cancel;

				if (td.Show() == TaskDialogResult.Cancel)
				{
					return false;
				}
			}


			return true;
		}


		// debug 

		private void showList()
		{
			Debug.WriteLine("");
			Debug.WriteLine($"view count: {WkgUserStylesView.Count}");
			Debug.WriteLine($"list count: {uMgr.UsrStyleList.Count}");
			Debug.WriteLine($"this count: {Count}");
			Debug.WriteLine("");

			int i = 0;
			foreach (UnitsDataR udr in uMgr.UsrStyleList)
			{
				Debug.WriteLine($"idx: {i++:D2}| name: {udr.Ustyle.Name}| ");


				// string seq = udr.Sequence == 0 ? "  " : $"{(udr.Sequence + 1):D2}";
				// string iseq = udr.InitialSequence == 0 ? "  " : $"{(udr.Sequence + 1):D2}";

				// Debug.WriteLine($"idx: {i++:D2}  seq: {seq}  intseq:  {udr.InitialSequence:D2}  iseq: {udr.Sequence:D2}   isDel: {udr.DeleteStyle,6}  name: {udr.Ustyle.Name}");
			}

			Debug.WriteLine("");
		}

	#region defunct

		private void setUnitDisplay(string name)
		{
			UnitsDataR udr = getStdStyleFromName(name);

			if (udr != null)
			{
				DetailUnit = udr;
			}
		}

		//  Removed
		// private bool ValidateNewName(string testName)
		// {
		// 	// rules
		// 	// validation requirements
		// 	// min 4 characters
		// 	// must start with alphanumeric (uc or lc)
		// 	// middle is alphanumeric, space, dash, period
		// 	// must end with alphanumeric (no dash, no space, no period)
		// 	// name must be unique
		//
		// 	bool result = validateName(testName);
		//
		// 	validateFailDesc = null;
		//
		// 	
		// 	// validateFailDesc = uSup.CheckStyleNameSyntax(testName);
		// 	//
		// 	// if (validateFailDesc != null) return false;
		// 	//
		// 	// if (hasNameInUserStyles(testName))
		// 	// {
		// 	// 	validateFailDesc = "Name is already in use";
		// 	// 	return false;
		// 	// }
		//
		// 	return validateFailDesc == null;
		//
		//
		// 	// if (testName == null || testName.Length < 4)
		// 	// {
		// 	// 	validateFailDesc = "Name mist be a minimum of 4 characters";
		// 	// 	return false;
		// 	// }
		// 	//
		// 	// Regex r = new Regex("^[a-zA-Z0-9][a-zA-Z0-9\\. \\-]*[a-zA-Z0-9]{1}$");
		// 	//
		// 	// if (!r.IsMatch(testName))
		// 	// {
		// 	// 	validateFailDesc = "Name has invalid characters";
		// 	// 	return false;
		// 	// }
		// 	//
		// 	// if (hasNameInUserStyles(testName))
		// 	// {
		// 	// 	validateFailDesc = "Name is already in use";
		// 	// 	return false;
		// 	// }
		// 	//
		// 	// return true;
		// }


		// private bool ValidateNewDesc(string testName)
		// {
		// 	// rules
		// 	// validation requirements
		// 	// min 6 characters
		// 	// must start with alphanumeric (uc or lc)
		// 	// middle is alphanumeric, space, dash, or period
		// 	// must end with alphanumeric (no dash, no space, no period)
		//
		// 	// validateFailDesc = uMgr.ValidateStyleDesc(testName);
		// 	validateFailDesc = UnitsSupport.CheckStyleDescSyntax(testName);
		//
		// 	return validateFailDesc == null;
		//
		//
		// 	// if (testName == null || testName.Length < 6)
		// 	// {
		// 	// 	validateFailDesc = "Name mist be a minimum of 6 characters";
		// 	// 	return false;
		// 	// }
		// 	//
		// 	// Regex r = new Regex("^[a-zA-Z0-9][a-zA-Z0-9\\. \\-]*[a-zA-Z0-9]{1}$");
		// 	//
		// 	// if (!r.IsMatch(testName))
		// 	// {
		// 	// 	validateFailDesc = "Name has invalid characters";
		// 	// 	return false;
		// 	// }
		// 	//
		// 	// return true;
		// }

		// private bool validateName(string testName)
		// {
		// 	bool result;
		// 	ValMsgNameId msgId;
		//
		// 	result = UnitsSupport.CheckStyleNameSyntax(testName, out msgId);
		//
		// 	if (!result) return result;
		//
		// 	if (hasNameInUserStyles(testName))
		// 	{
		// 		result = false;
		// 		msgId = ValMsgNameId.VN_MUST_BE_UNIQUE;
		// 	}
		//
		// 	return result ;
		// }

		// private void OnEditedDescriptionChanging(object sender, ChangeValueEventArgs e)
		// {
		// 	string result = UnitsSupport.CheckStyleDescSyntax(e.Proposed);
		//
		// 	if (e.Proposed.Equals(""))
		// 	{
		// 		e.Cancel = false;
		// 	}
		// 	else
		// 	{
		// 		e.Cancel = result != null;
		// 	}
		//
		// 	lbxSelItem.IsDescOk = result == null;
		//
		// 	EditDescToolTip = result;
		// }


		// util

	#endregion

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

		private void OnStaticPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			// Debug.WriteLine($"@static prop changed| {e.PropertyName}");

			if (e.PropertyName == nameof(WinLocationChanging))
			{
				// Debug.WriteLine($"@static prop change| loc changing| {winLocationChanging} | win moving| {winMoving}");

				if (winLocationChanging)
				{
					PopupInfoHide();
					PopupEditOpsHide();
				}

				if (!winLocationChanging)
				{
					BeginInfoPopup();
					BeginEditOpsPopup();
				}
			}
		}

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is UnitStylesMgr";
		}

	#endregion

	#region event consuming

		// win

		private void UnitStylesMgr_Loaded(object sender, RoutedEventArgs e)
		{
			InsPosition = 2;

			_Location = UserSettings.Data.WinPosUnitStyleMgr;

			if (_Location.Top >= 0 && _Location.Left >= 0)
			{
				this.Top = _Location.Top;
				this.Left = _Location.Left;
			}

			HwndSource source = HwndSource.FromHwnd(new WindowInteropHelper(this).Handle);
			source.AddHook(new HwndSourceHook(WndProc));

			StaticPropertyChanged += OnStaticPropertyChanged;
		}

		private void UnitStylesMgr_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			// PopupEditOpsClose();
			// PopupInfoClose();
			//
			// if (HasChanges)
			// {
			// 	TaskDialog td = new TaskDialog("Unit Styles");
			// 	td.MainInstruction = "You have un-saved changes.";
			// 	td.MainContent = "Select CANCEL to keep these changes\nSelect OK to proceed and lose these changes";
			// 	td.CommonButtons = TaskDialogCommonButtons.Cancel | TaskDialogCommonButtons.Ok;
			// 	td.DefaultButton = TaskDialogResult.Cancel;
			//
			// 	if (td.Show() == TaskDialogResult.Cancel)
			// 	{
			// 		e.Cancel = true;
			// 		return;
			// 	}
			// }

			if (!verifyDialogClose())
			{
				e.Cancel = true;
				return;
			}

			UserSettings.Data.WinPosUnitStyleMgr = new WindowLocation(this.Top, this.Left);
			UserSettings.Admin.Write();
		}

		private void UnitStylesMgr_OnIsKeyboardFocusWithinChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			if ((bool) e.NewValue)
			{
				if (!winLostFocus) return;

				BeginInfoPopup();
				BeginEditOpsPopup();
			}
			else
			{
				PopupInfoHide();
				PopupEditOpsHide();

				winLostFocus = true;
			}
		}


		// controls

		private void OnShowInChanged(object sender, CheckBoxChangedEventArgs e)
		{
			if ((e.WhichCheckBox.HasValue))
			{
				int order = uMgr.GetMaxInListIdx(e.WhichCheckBox.Value);

				e.InListOrder = ++order;
			}
			else
			{
				e.InListOrder = UnitData.INLIST_UNDEFINED;
			}

			ChangesMade++;

		}

		private void TbxEditText_OnKeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
			{
				e.Handled = true;

				((TextBox)sender).MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
			}
		}

		private void lbx_Initialized(object sender, EventArgs e)
		{
			lbx = (ListBox) sender;
		}

		private void Lbx1_OnScrollChanged(object sender, ScrollChangedEventArgs e)
		{
			if (e.VerticalChange == 0) return;

			PopupEditOpsClose();
			FocusManager.SetFocusedElement(FocusManager.GetFocusScope(((ListBox)sender)), null);
			Keyboard.ClearFocus();
		}


		// private void TblkEditSampleText_OnKeyUp(object sender, KeyEventArgs e)
		// {
		// 	if (e.Key != Key.Enter) return;
		//
		// 	TextBox tbx = (TextBox)sender;
		//
		// 	e.Handled = true;
		// 	LbxSelItem.ProcessSample = tbx.Text;
		// 	tbx.CaretIndex = tbx.Text.Length;
		// }


		// dialog control
		private void BtnDebug_OnClick(object sender, RoutedEventArgs e)
		{
			


			uMgr.UlMgr.Rio.ListPbs();


			int a = 1;

			// IList<ForgeTypeId> ids = uMgr.GetSymIds(uMgr.ProjectUnitStyle);
			// UnitsInListMgr ul = uMgr.UlMgr;
			// SettingsMgr<AppSettingPath, AppSettingInfo<AppSettingDataFile>, AppSettingDataFile> a = AppSettings.Admin;
			// SettingsMgr<UserSettingPath, UserSettingInfo<UserSettingDataFile>, UserSettingDataFile> u = UserSettings.Admin;
			//
			// UnitsDataR r = lbxSelItem;

			// ListCollectionView vr = uMgr.InListViewRibbon;
			// ListCollectionView vlt = uMgr.InListViewDlgLeft;
			// ListCollectionView vrt = uMgr.InListViewDlgRight;

			// Dictionary<string, UStyle> us = UnitsStdUStyles.StdSysStyles;
			//
			// List<UnitsDataD> ud1 = UnitStdStylesD.ListDribbon;
			// List<UnitsDataD> ud2 = UnitStdStylesD.ListDleft;
			// List<UnitsDataD> ud3 = UnitStdStylesD.ListDright;

			// vr.Refresh();
			// t1.Refresh();
			// t2.Refresh();

			// lbxSelItem.Sequence = 0;
			//
			// int b = 1;

			// Debug.WriteLine("@debug| popup is| ");
			//
			// BeginEditOpsPopup();

			// if (currEditPopup == null)
			// {
			// 	Debug.WriteLine("null");
			// 	return;
			// }
			//
			// Debug.WriteLine("not null");
			//
			// currEditPopup.IsOpen = true;


			// Debug.WriteLine("\nStdStyles\n");
			// styles(StdStyles);
			//
			// Debug.WriteLine("\nUsrStyles");
			// showList();
			//
			// Debug.WriteLine("\nend of listings\n");

			// int idx = LbxSelIndex;
			//
			// UnitsDataR udr = DetailUnit;
			// UStyle us = udr.Ustyle;
			//
			// UnitsDataR udrl = lbxSelItem;
			// UStyle usl = udrl.Ustyle;
			//
			// KeyValuePair<string, UnitsDataR> kvp = cbxSelItem;
			// string kname = kvp.Value.Ustyle.Name;
			//
			// // not the one
			// // UnitsDataR udrv = cbxSelValue;
			// // UStyle usc = udrv.Ustyle;
			// // string name = usc.Name;
			//
			// NewName = udr.Ustyle.Name;
			// NewDesc = udr.Ustyle.Description;
			//
			// showList();
			//
			// Debug.WriteLine("@debug button");
		}

		private void BtnDone_OnClick(object sender, RoutedEventArgs e)
		{
			if (HasChanges)
			{
				applyChanges();
			}

			this.Close();
		}

		private void BtnApply_OnClick(object sender, RoutedEventArgs e)
		{
			applyChanges();
		}

		private void BtnCancel_OnClick(object sender, RoutedEventArgs e)
		{
			// if (HasChanges)
			// {
			// 	TaskDialog td = new TaskDialog("Unit Styles");
			// 	td.MainInstruction = "You have un-saved changes.";
			// 	td.MainContent = "Select CANCEL to keep these changes\nSelect OK to proceed and lose these changes";
			// 	td.CommonButtons = TaskDialogCommonButtons.Cancel | TaskDialogCommonButtons.Ok;
			// 	td.DefaultButton = TaskDialogResult.Cancel;
			//
			// 	TaskDialogResult result = td.Show();
			//
			// 	if (result == TaskDialogResult.Cancel)
			// 	{
			// 		
			// 		return;
			// 	}
			// }

			// pressed OK == ok to cancel these changes / go back to before the changes.
			// same as reset but closes dialog box.
			if (!verifyDialogClose())
			{
				e.Handled = true;
				return;
			}

			resetUserStyles();

			this.Close();
		}

		private void BtnReset_OnClick(object sender, RoutedEventArgs e)
		{
			if (HasChanges)
			{
				TaskDialog td = new TaskDialog("Unit Styles");
				td.MainInstruction = "You have un-saved changes.";
				td.MainContent = "Select CANCEL to keep these changes\nSelect OK to lose your changes";
				td.CommonButtons = TaskDialogCommonButtons.Cancel | TaskDialogCommonButtons.Ok;
				td.DefaultButton = TaskDialogResult.Cancel;

				TaskDialogResult result = td.Show();

				if (result == TaskDialogResult.Cancel) return;
			}

			resetUserStyles();
		}

		private void BtnUsResetToDefault_OnClick(object sender, RoutedEventArgs e)
		{
			TaskDialog td = new TaskDialog("Unit Styles");
			td.MainInstruction = "This will eliminate any changes and revert to the default settings.";
			td.MainContent = "Select CANCEL to keep all of your changes\nSelect OK to lose all of your changes";
			td.CommonButtons = TaskDialogCommonButtons.Cancel | TaskDialogCommonButtons.Ok;
			td.DefaultButton = TaskDialogResult.Cancel;

			TaskDialogResult result = td.Show();

			if (result == TaskDialogResult.Cancel) return;

			resetUserStylesToDefault();
		}

		private void BtnSoShowStyleOrder_OnClick(object sender, RoutedEventArgs e)
		{
			uMgr.ConfigWorkingInLists();

			UnitStyleOrder winOrder = new UnitStyleOrder(this);

			winOrder.SetPosition(this);

			winOrder.ShowDialog();
		}

		// list controls

		private void BtnUp_OnClick(object sender, RoutedEventArgs e)
		{
			if (lbxSelIndex <= 1) return;

			int atIdx = ((UnitsDataR) WkgUserStylesView.GetItemAt(lbxSelIndex)).Sequence;
			((UnitsDataR) WkgUserStylesView.GetItemAt(lbxSelIndex)).Sequence -= 1;

			int pastIdx = ((UnitsDataR) WkgUserStylesView.GetItemAt(lbxSelIndex - 1)).Sequence;
			((UnitsDataR) WkgUserStylesView.GetItemAt(lbxSelIndex - 1)).Sequence += 1;

			ChangesMade += 1;
		}

		private void BtnDn_OnClick(object sender, RoutedEventArgs e)
		{
			if (lbxSelIndex == Count - 1 || lbxSelIndex == 0) return;

			((UnitsDataR) WkgUserStylesView.GetItemAt(lbxSelIndex)).Sequence += 1;
			((UnitsDataR) WkgUserStylesView.GetItemAt(lbxSelIndex + 1)).Sequence -= 1;

			ChangesMade += 1;
		}

		private void BtnDelete_OnClick(object sender, RoutedEventArgs e)
		{
			if (lbxSelItem.Ustyle.IsLocked) return;
			int idx = lbxSelIndex;


			lbxSelItem.DeleteStyle = true;
			lbxSelItem.Sequence = 0;

			if (lbxSelIndex < Count - 1)
			{
				// resequence down (false)
				uMgr.ReSequenceStylesList(WkgUserStylesView, lbxSelIndex + 1, false);
			}

			ChangesMade += 1;

			WkgUserStylesView.Refresh();

			LbxSelIndex = idx <= WkgUserStylesView.Count - 1 ? idx : WkgUserStylesView.Count - 1;

			// showList();
		}


		// new style controls

		private void BtnAddBefore_OnClick(object sender, RoutedEventArgs e)
		{
			UnitsDataR udr = uMgr.NewUDR(detailUnit, newName, newDesc, lbxSelIndex);

			WkgUserStylesView.AddNewItem(udr);
			WkgUserStylesView.CommitNew();

			uMgr.ReSequenceStylesList(WkgUserStylesView, lbxSelIndex + 1, true);

			WkgUserStylesView.Refresh();

			OnPropertyChanged(nameof(WkgUserStylesView));

			ChangesMade += 1;

			resetNewStyleInfo();
		}

		private void BtnAddLast_OnClick(object sender, RoutedEventArgs e)
		{
			UnitsDataR udr = uMgr.NewUDR(detailUnit, newName, newDesc, WkgUserStylesView.Count);

			// wkgUserStyles.AddNew();
			WkgUserStylesView.AddNewItem(udr);
			WkgUserStylesView.CommitNew();

			ChangesMade += 1;

			resetNewStyleInfo();

			LbxSelIndex = WkgUserStylesView.Count - 1;
		}

	#endregion

	#region popup

		// info popup

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


		// edit ops popup


		private void Tbx_OnGotFocus(object sender, RoutedEventArgs e)
		{
			int idx = CustomProperties.GetGenericIntOne((TextBox) sender);

			if (idx != priorValueIdx)
			{
				PopupEditOpsClose();
			}

			currEditPopup = CustomProperties.GetGenericPopupOne((TextBox) sender);
			priorValueIdx = idx;
			priorValue[priorValueIdx] = ((TextBox) sender).Text;

			// if (priorValue[priorValueIdx].IsVoid()) ((TextBox) sender).Text = getDefaultNewStyleText();

			// sendTextToTbx(((TextBox) sender),  getDefaultNewStyleText());

			BeginEditOpsPopup();
		}

		private void Tbx_OnLostFocus(object sender, RoutedEventArgs e)
		{
			PopupEditOpsClose();
		}


		// private void TbxEditNameText_OnLostFocus(object sender, RoutedEventArgs e)
		// {
		// 	Debug.WriteLine($"TbxEditNameText_OnLostFocus| {((TextBox) sender).Text}");
		// 	// when a list item's textbox loses focus / is complete
		// 	// first close the popup
		// 	PopupEditOpsClose();
		//
		// 	// validate the information
		// }




		private void EditOpsPopup_Opened(object sender, EventArgs e)
		{
			Popup pop = (Popup)sender;
			ContentControl cc = (ContentControl) pop.Child;

			Border bdr = (Border) pop.PlacementTarget;
			// TextBox Tbx = (TextBox) CustomProperties.GetGenericObjectOne(pop);

			double popWidth = cc.ActualWidth;
			double tbxWidth = bdr.ActualWidth;

			pop.HorizontalOffset = tbxWidth - popWidth;
		}

		private void EditOpsPopup_OnClosed(object sender, EventArgs e)
		{
			// currEditPopup = null;
		}


		private void BeginEditOpsPopup()
		{
			PopupInfoClose();

			if (currEditPopup == null) return;

			PopupEditOpsOpen();
		}

		private void PopupEditOpsOpen()
		{
			// Debug.WriteLine($"at popup open|");
			if (!currEditPopup.IsOpen) currEditPopup.IsOpen = true;
		}

		private void PopupEditOpsClose()
		{
			// Debug.WriteLine($"at popup close|");
			if (currEditPopup == null) return;

			if (currEditPopup.IsOpen) currEditPopup.IsOpen = false;

			currEditPopup = null;
		}

		private void PopupEditOpsHide()
		{
			if (currEditPopup == null) return;

			if (currEditPopup.IsOpen) currEditPopup.IsOpen = false;
		}


		private void BtnEditOptsClearText_OnClick(object sender, RoutedEventArgs e)
		{
			TextBox tbx = getTextBox(sender);

			// priorValue[priorValueIdx] = "";
			sendTextToTbx(tbx, "");
		}

		private void BtnEditOptsReset_OnClick(object sender, RoutedEventArgs e)
		{
			TextBox tbx = getTextBox(sender);

			if (!priorValue[priorValueIdx].IsVoid())
			{
				sendTextToTbx(tbx, priorValue[priorValueIdx]);
			}
			else
			{
				string text;

				if (priorValueIdx <= POPUP_NEW_STYLE_LIMIT)
				{
					text = getDefaultNewStyleText();
					priorValue[priorValueIdx] = text;
				}
				else
				{
					text = getEditStyleResetText();
				}

				sendTextToTbx(tbx, text);
			}
		}

		private void BtnEditOptsCancel_OnClick(object sender, RoutedEventArgs e)
		{
			TextBox tbx = getTextBox(sender);

			if (!priorValue[priorValueIdx].IsVoid())
			{
				sendTextToTbx(tbx, priorValue[priorValueIdx]);
			}

			PopupEditOpsClose();

			FocusManager.SetFocusedElement(FocusManager.GetFocusScope(tbx), null);
			Keyboard.ClearFocus();
		}

		private void BtnEditOptsClose_OnClick(object sender, RoutedEventArgs e)
		{
			TextBox tbx = getTextBox(sender);

			PopupEditOpsClose();

			FocusManager.SetFocusedElement(FocusManager.GetFocusScope(tbx), null);
			Keyboard.ClearFocus();
		}


		[DebuggerStepThrough]
		private int getTextBoxIdx(TextBox tbx)
		{
			return (int) CustomProperties.GetGenericIntOne(tbx);
		}

		[DebuggerStepThrough]
		private TextBox getTextBox(object sender)
		{
			Popup pu = CustomProperties.GetGenericPopupOne((Button) sender);
			return (TextBox) CustomProperties.GetGenericObjectOne((Button) sender);
		}

		private void sendTextToTbx(TextBox tbx, string text)
		{
			int idx = getTextBoxIdx(tbx);

			switch (idx)
			{
			case UnitStylesMgrWinData.POPUP_NEW_STYLE_NAME:
				{
					NewName = text;
					break;
				}
			case UnitStylesMgrWinData.POPUP_NEW_STYLE_DESC:
				{
					NewDesc = text;
					break;
				}
			case UnitStylesMgrWinData.POPUP_STYLE_NAME:
				{
					lbxSelItem.Name = text;
					break;
				}
			case UnitStylesMgrWinData.POPUP_STYLE_DESC:
				{
					lbxSelItem.Description = text;
					break;
				}
			case UnitStylesMgrWinData.POPUP_STYLE_SAMPLE:
				{
					lbxSelItem.Sample = text;
					break;
				}
			}
		}

	#endregion

	#region un-categorized

	#if DEBUG

		private void styles(Dictionary<string, UnitsDataR> styles)
		{
			foreach (KeyValuePair<string, UnitsDataR> kvp in styles)
			{
				Debug.Write($"key| {kvp.Key,-30}");
				Debug.Write($"USystem| {kvp.Value.USystem,-15}");
				Debug.Write($"Id| {kvp.Value.Id.TypeId,-50}");
				Debug.Write($"Cat| {kvp.Value.Ustyle.UnitCat,-15}");
				Debug.Write($"Class| {kvp.Value.Ustyle.UnitClass,-15}");
				Debug.Write($"Sys| {kvp.Value.Ustyle.UnitSys,-15}");
				// Debug.Write($"Sym| >{kvp.Value.Ustyle.Symbol}<");
				Debug.WriteLine("");
			}
		}


	#endif

	#endregion

		/*
		to make the textbox editing work
		
		code behind routines are setup

		in the "UnitStylesMgrWinData" file, create an index value for each
		textbox / data field.  this index identifies the position in the prior value array
		which holds the "original" textbox value which allow "reset" to occur.


		for the textbox, include the two popups
		"edit ops" and "info" (help)"

		at each textbox
		@ code behind - validate the info.
		bind "cs:CustomProperties.GenericIntOne" to the popup index
		bind "cs:CustomProperties.GenericPopupOne" to the popup 
		include the following:
		GotFocus="Tbx_OnGotFocus"
		LostFocus="Tbx_OnLostFocus"
		ToolTip="{Binding NewDescToolTip, Mode=OneWay}"
		(and maybe)
		Style="{StaticResource TbxEditable}"

		for the edit ops popup
		bind 'cs:CustomProperties.GenericPopupOne' to the help popup
		bind 'cs:CustomProperties.GenericObjectOne' to the textbox
		set the 'PlacementTarget' to the surrounding border
		include the following
		Opened="EditOpsPopup_Opened"
		Closed="EditOpsPopup_OnClosed"
		Style="{StaticResource PuBase}"

		for the "info" (help) popup
		set 'cs:VisualStates.MainContent' to the content of the help box
		set 'cs:VisualStates.TitleText' to the title for the help box
		include the following:
		Button.Click="BtnPopupClose_OnClick"
		Style="{StaticResource PuBase}"

		 */
	}
}

/*
 dead removed code
		private void BtnEditOptsApply_OnClick(object sender, RoutedEventArgs e)
		{
			popupEditClose();
			this.Focus();
		}

		private void BtnChgTemplate_OnClick(object sender, RoutedEventArgs e)
		{
			ProcessStyleChanges();
		}

		private void MoveFocus(KeyEventArgs e)
		{
			// Creating a FocusNavigationDirection object and setting it to a
			// local field that contains the direction selected.
			FocusNavigationDirection focusDirection = FocusNavigationDirection.Next;

			// MoveFocus takes a TraveralReqest as its argument.
			TraversalRequest request = new TraversalRequest(focusDirection);

			// Gets the element with keyboard focus.
			UIElement elementWithFocus = Keyboard.FocusedElement as UIElement;

			// Change keyboard focus.
			if (elementWithFocus != null)
			{
				if (elementWithFocus.MoveFocus(request)) e.Handled = true;
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





*/

// private string status;
// private DispatcherTimer timer;
// private bool isChanged;
// private bool stylePanelActivated;
// private bool lv1ItemPanelActivated;

// private bool okToSave;
// private int saveCount;
// private Image img;
// private ComboBox cbx;

// public Image Imgs => img;
// private int isClosing = -1;
// private bool? isGoodOrBad = null;
// private bool overEditPopup;

// private bool popupInfoIsEntered;
//
// private DoubleAnimation d = new DoubleAnimation(1, new Duration(TimeSpan.FromSeconds(2)));
// private AnimationClock popupAniClock;
//
// private DispatcherTimer timer;
// private bool timerActive;


// private void wkgUserStylesCurrentChanged(object sender, EventArgs e)
// {
// 	if (initStatus) return;
// }

// public UnitsDataR LbxSelItemCopy { get; set; }


// public string ContentType => contentSelection[dialogIndex];
//
// public string CloseType => closeSelection[changesMade > 1 ? 1 : 0];


// public bool NotOkToSave => !okToSave;

// public bool OkToSave
// {
// 	get => okToSave;
//
// 	set
// 	{
// 		okToSave = value;
// 		OnPropertyChanged();
// 		OnPropertyChanged(nameof(NotOkToSave));
// 	}
// }


// private void showUnitStyleSettings(UnitsDataR udr)
// {
// 	if (udr.Ustyle.UnitClass == UnitClass.CL_CONTROL)
// 	{
// 		showingSavedStyles = true;
//
// 		showSavedStyle(LbxSelItem);
// 	}
// 	else
// 	{
// 		showingSavedStyles = false;
// 		// CanEditStyle = false;
// 		UnitsDataR ur = getStdStyleFromName(udr.Name);
//
// 		if (ur != null)
// 		{
// 			DetailUnit = ur;
// 		}
// 	}
// }


// private void CbxStdStyles_Initialized(object sender, EventArgs e)
// {
// 	cbx = (ComboBox) sender;
// 	// cbx.SelectedIndex = 1;
// }

// private int SaveCount
// {
// 	get => saveCount;
// 	set
// 	{
// 		saveCount = value;
// 		OkToSave = saveCount == 0;
// 	}
// }


// private void NameValidation_OnError(object sender, ValidationErrorEventArgs e)
// {
// 	// IsNameOk = (e.Action != ValidationErrorEventAction.Added);
//
// 	SaveCount += (e.Action != ValidationErrorEventAction.Added) ? 1 : -1;
// }


// public string ExistStyleName
// {
// 	get => lbxSelItem.Name;
//
// 	set
// 	{
// 		if (value.Equals(lbxSelItem.Name)) return;
//
//
// 	}
// }
//
// public bool IsNameOk
// {
// 	get => isNameOk;
// 	set
// 	{
// 		if (value == isNameOk) return;
//
// 		isNameOk = value;
// 		OnPropertyChanged();
// 	}
// }
//
// public bool IsDescOk
// {
// 	get => isDescOk;
// 	set
// 	{
// 		if (value == isDescOk) return;
//
// 		isDescOk = value;
// 		OnPropertyChanged();
// 	}
// }


// public bool StylePanelActivated
// {
// 	get => stylePanelActivated;
//
// 	set
// 	{
// 		if (value == stylePanelActivated ||
// 			lv1ItemPanelActivated) return;
//
// 		stylePanelActivated = value;
// 		OnPropertyChanged();
// 	}
// }
//
// public bool Lv1ItemPanelActivated
// {
// 	get => lv1ItemPanelActivated;
//
// 	set
// 	{
// 		if (value == lv1ItemPanelActivated || 
// 			stylePanelActivated) return;
//
// 		lv1ItemPanelActivated = value;
// 		OnPropertyChanged();
// 	}
// }


// private void PuEditOptsStyleName_OnClick(object sender, RoutedEventArgs e)
// {
// 	Popup pop = (Popup) sender;
// 	// TextBox Tbx = (TextBox) CustomProperties.GetGenericObjectOne(pop);
//
// 	Debug.WriteLine($"@button click| {((Button) e.OriginalSource).Name}"
// 		+ $" sender| {pop.Name} "
// 		+ $" border| {((Border) pop.PlacementTarget).Name} "
// 		// + $" textbox {Tbx.Name}")
// 		);
// }

// private void TbxStyleName_OnTextChanged(object sender, TextChangedEventArgs e)
// {
// 	Debug.WriteLine($"new name| textbox changed");
//
// 	// isChanged = true;
//
// 	// TextBox tbx = (TextBox)sender;
// 	//
// 	// VisualStates.SetIsModified(tbx, true);
// 	// VisualStates.SetIsModified(spNewStyle, true);
// }


// private void getAddStylePanel()
// {
// spNewStyle = FindElementByName<StackPanel>(CtMainContent, "SpAddStyleBegin");
// grdLv1 = FindElementByName<Grid>(CtMainContent, "GrdLv1Item");
// bdrId = FindElementByName<Border>(CtMainContent, "Id_BdrId");

// StackPanel sp1 = FindElementByName<StackPanel>(DpMain, "SpAddStyleBegin");
// sp1 = FindElementByName<StackPanel>(WinUnitStyle, "SpAddStyleBegin");
// }

// private void Popup_OnMouseEnter(object sender, RoutedEventArgs e)
// {
// 	Debug.WriteLine($"C) mouse enter| {currInfoPopup?.Name ?? "is null"}");
//
// 	popupInfoIsEntered = true;
//
// 	removeTimer();
// }
//
// private void Popup_OnMouseLeave(object sender, RoutedEventArgs e)
// {
// 	Debug.WriteLine($"D) mouse leave| {currInfoPopup?.Name ?? "is null"}");
//
// 	popupInfoIsEntered = false;
//
// 	removeTimer();
// 	startTimer();
// }
//
// private void popupTimerCompleted(object sender, EventArgs e)
// {
// 	Debug.WriteLine($"H) timer completed| is entered?| {popupInfoIsEntered}");
//
// 	removeTimer();
//
// 	if (popupInfoIsEntered)
// 	{
// 		return;
// 	}
//
// 	popupInfoClose();
// }
//
// private void removeTimer()
// {
// 	Debug.WriteLine($"I) Try remove timer| is active| {timerActive}");
//
// 	if (!timerActive) return;
//
// 	Debug.WriteLine("J) is active| removing timer");
//
// 	timer.Stop();
// 	timer = null;
//
// 	timerActive = false;
// }
//
// private void startTimer()
// {
// 	Debug.WriteLine($"K) start timer?| {!timerActive}");
//
// 	if (timerActive) return;
//
// 	Debug.WriteLine("L) starting timer");
//
// 	timer = new DispatcherTimer();
// 	timer.Interval = new TimeSpan(0, 0, 3);
//
// 	timerActive = true;
//
// 	timer.Tick += popupTimerCompleted;
//
// 	timer.Start();
// }


// private void PuEditOptsStyleName_OnMouseEnter(object sender, MouseEventArgs e)
// {
// 	Debug.WriteLine("@element| mouse enter|");
// 	overEditPopup = true;
// }
//
// private void PuEditOptsStyleName_OnMouseLeave(object sender, MouseEventArgs e)
// {
// 	Debug.WriteLine("@element| mouse leave|");
// 	overEditPopup = false;
// }

// private void tbx_OnLostFocus(object sender, RoutedEventArgs e)
// {
// 	Debug.Write("@textbox| lost focus|");
//
// 	if (isChanged)
// 	{
// 		Debug.WriteLine($"changed?| yes");
// 		// ((TextBox) sender).Focus();
// 	}
// 	else
// 	{
// 		Debug.WriteLine($"changed?| no");
// 		StylePanelActivated = false;
// 	}
//
// }