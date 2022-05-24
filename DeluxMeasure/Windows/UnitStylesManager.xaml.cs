using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Interop;
using System.Windows.Media;
using Autodesk.Revit.UI;
using DeluxMeasure.UnitsUtil;
using SettingsManager;
using UtilityLibrary;
using ComboBox = System.Windows.Controls.ComboBox;
using static DeluxMeasure.UnitsUtil.UnitsStdUStyles;

namespace DeluxMeasure.Windows
{
	/*

	UI adjustments:
	* Length Unit Settings
	 * change title to Unit Settings
	 * add subtitle to "Length"
	 * add source dropdown
	 * -> standard Style + {a standard style name}
	 * -> user Style
	 * * whichever style shown
	 *	-> duplicate as a new style / add to list
	 *  -> cannot duplicate "project" from standard styles

	* allow user to delete "feet and decimal inches"


	features / functions
	(in no order)

	 * startup
		-> as configured
		and
		-> add project as a place holder

	 * save
		-> save project as a place holder

	 * based on the current unit settings, create a new unit style
		-> get name (shown in lists) and description
		-> add to end or location noted
		* need to re-number
		* save (user styles) after creation
	
	 * select a "standard" style (from app styles) and add as a new style
		-> get name (shown in lists) and description - copy from standard style but adjust for duplicates
		-> add to end or location noted
		* need to re-number
		* save (user styles) after creation
	 
	 * change style name
		* check for duplicate name & disallow
	 
	 * change description
	 
	 * change order in list
		* cannot be moved above first
		* first cannot be moved
	 
	 * delete a style
		* cannot delete locked
	
	 * toggle which list
	 
	 * change sample value
	 
	*** change to list organization
		* allows the re-arrangement of the order in each list
		* does not allow editing
		* keep list simple: number, icon, name, description
	 *


	*/


	/// <summary>
	/// Interaction logic for UnitStylesManager.xaml
	/// </summary>
	/// 
	public partial class UnitStylesManager : Window, INotifyPropertyChanged
	{
	#region private fields

		private static bool winMoving = false;
		private const int WM_SETCURSOR = 32;
		private const int WM_NCLBUTTONDOWN = 0x00a1;

		private const string STR_SHOWSTYLE_NAME = null;
		private const string STR_SHOWSTYLE_DDNAME = "Show Selected User Style";

		//
		// private const int WM_NCMOUSEMOVE = 0x00a0;
		//
		// private const int WM_WINDOWPOSCHANGING = 0x0046;
		// private const int WM_WINDOWPOSCHANGED = 0x0047;
		//
		// private const int WM_SYSCOMMAND = 274;
		// private const int WM_NCHITTEST = 132;
		// private const int WM_CAPTURECHANGED = 533;
		// private const int WM_NCMOUSELEAVE = 674;


		private static UnitStylesManager me;


		private readonly string[] contentSelection = new [] { "Adjust Style Order", "Adjust Saved Styles" };
		private readonly string[] closeSelection = new [] { "Done", "Save and Done" };

		private string status;

		private ListCollectionView styles;

		private ListCollectionView ribbonStyles;
		private ListCollectionView dialogLeftStyles;
		private ListCollectionView dialogRightStyles;

		private bool initStatus = false;

		private bool showingSavedStyles = false;

		private UnitsDataR detailUnit;

		private UnitsManager uMgr = UnitsManager.Instance;

		private Image img;

		private int lbxSelIndex;
		private UnitsDataR lbxSelItem;


		private int cbxSelIndex;
		private	KeyValuePair<string, UnitsDataR> cbxSelItem;

		private ComboBox cbx;
		private List<KeyValuePair<string, string>> cbxList;
		private KeyValuePair<string, string> cbxSelectedItem;


		// not the one
		private int dialogIndex;

		private int hasChanges;

		private int insPosition;

		private string newName;
		private string newDesc;
		private bool? isNewNameOk = null;   // true: ok to use | false not ok to use | null neither / undefined
		private bool? isNewDescOk = null;   // true: ok to use | false not ok to use | null neither / undefined
		private bool? isInsPosOk = null;    // true: ok to use | false not ok to use | null neither / undefined
		private bool isInsPosValid = false; // true: value is OK | false: value is not ok
		private bool canAddBefore = false;
		private bool canAddAfter = false;

		private static bool winLocationChanging;

		private ListBox lbx;

	#endregion

	#region ctor

		public UnitStylesManager()
		{
		#if PATH
			MethodBase mb = new StackTrace().GetFrame(1).GetMethod();
			Debug.WriteLine($"@UnitStyleMgr: ctor: {(mb.ReflectedType?.FullName ?? "is null")} > {mb.Name}");
		#endif

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

				// insertPosition = insPosition.ToString();
				// OnPropertyChanged(nameof(InsertPosition));
			}
		}

		public int Count => styles.Count;

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

		// public KeyValuePair<string, UnitsDataR> CbxSelItem
		// {
		// 	get => cbxSelItem;
		// 	set
		// 	{
		// 		cbxSelItem = value;
		// 		OnPropertyChanged();
		//
		// 		showUnitStyleSettings(cbxSelItem.Value);
		// 	}
		// }

		public int CbxSelIndex
		{
			get => cbxSelIndex;
			set
			{
				cbxSelIndex = value;
				OnPropertyChanged();
			}
		}

		// not the one
		// public UnitsDataR CbxSelValue
		// {
		// 	get => cbxSelValue;
		// 	set
		// 	{
		// 		cbxSelValue = value;
		// 		OnPropertyChanged();
		// 	}
		// }

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

		/*
		public string InsertPosition 
		{
			get => insertPosition;
			set
			{
				if (value.Equals(insertPosition))
				{
					DetermineCanAdd();
					return;
				}

				insertPosition = value;
				OnPropertyChanged();

				int val;
				bool result = Int32.TryParse(value, out val);

				if (!result)
				{
					IsInsPosValid = false;
					return;
				}

				if (val == insPosition)
				{
					isInsPosValid = true;
					DetermineCanAdd();
					return;
				}

				result = ValidateInsPos(val);

				if (result)
				{
					IsInsPosValid = true;

					insPosition = val;

					LbxSelIndex = val - 1;
				}

				DetermineCanAdd();
			}
		}
		*/

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

		public bool IsModified => hasChanges > 0;

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


		// new name / new description properties

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
			// set
			// {
			// 	if (value.HasValue && value.Equals(cvrtToBool(isInsPosOk))) return;
			//
			// 	isInsPosOk = value;
			// 	OnPropertyChanged();
			//
			// 	DetermineCanAdd();
			// }
		}

		// public bool IsInsPosValid
		// {
		// 	get => isInsPosValid;
		// 	set
		// 	{
		// 		if (value == isInsPosValid) return;
		//
		// 		isInsPosValid = value;
		// 		OnPropertyChanged();
		//
		// 		DetermineCanAdd();
		// 	}
		// }

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

		// public bool CanEditStyle
		// {
		// 	get => canEditStyle;
		// 	set
		// 	{
		// 		canEditStyle = value;
		// 		OnPropertyChanged();
		// 	}
		// }

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

	#endregion

	#region public methods

		public bool ValidateInsPos(int pos)
		{
			// rules
			// cannot be 1 or less
			// cannot be greater than last number

			return  pos > 1 && pos <= Count;
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

			// CanEditStyle = false;
			//
			// if (udr.Ustyle.UnitClass == UnitClass.CL_ORDINARY)
			// {
			// 	CanEditStyle = true;
			// }
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
					childElement = (T)child;
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


			// removed - is a duplicate of the other change recordings - however, is more accurate
			// HasChanges += 1;
			//
			// Debug.WriteLine("got current changed");

			// OnPropertyChanged(nameof(Count));
		}

		private void WinUnitStyle_Loaded(object sender, RoutedEventArgs e)
		{
			LbxSelIndex = 3;

			CbxSelIndex = 0;

			// lbx = FindElementByName<ListBox>(this.WinGrid, "lbx");


			HwndSource source = HwndSource.FromHwnd(new WindowInteropHelper(this).Handle);
			source.AddHook(new HwndSourceHook(WndProc));
		}

		private void WinUnitStyle_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			UserSettings.Data.WinPosUnitStyleMgr = new WindowLocation(this.Top, this.Left);
			UserSettings.Admin.Write();
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

		#endregion


	}
}