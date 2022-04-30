using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
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

		private readonly string[] contentSelection = new [] { "Adjust Style Order", "Adjust Saved Styles" };
		private readonly string[] closeSelection = new [] { "Done", "Save and Done" };

		private string status;

		private ListCollectionView styles;

		private ListCollectionView ribbonStyles;
		private ListCollectionView dialogLeftStyles;
		private ListCollectionView dialogRightStyles;

		private bool initStatus = false;

		private UnitsDataR detailUnit;

		private UnitsManager uMgr = UnitsManager.Instance;

		private Image img;

		private int lv1SelIndex;
		private UnitsDataR lv1SelItem;

		private int cbxSelIndex;
		private	KeyValuePair<string, UnitsDataR> cbxSelItem;

		private ComboBox cbx;

		// not the one
		// private UnitsDataR cbxSelValue;

		private int dialogIndex;

		private int hasChanges;

		private int insertPosition;
		private string newName;
		private string newDesc;
		private bool hasNewName = false;
		private bool hasNewDesc = false;

	#endregion

	#region ctor

		public UnitStylesManager()
		{
		#if PATH
			MethodBase mb = new StackTrace().GetFrame(1).GetMethod();
			Debug.WriteLine($"@UnitStyleMgr: ctor: {(mb.ReflectedType?.FullName ?? "is null")} > {mb.Name}");
		#endif

			InitializeComponent();

			uMgr.ReadUnitSettings();

			OnPropertyChanged(nameof(StylesView));

			Init();

			status = "init";
		}

	#endregion

	#region public properties

		public Image Imgs => img;

		// the current displayed unit information
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

		public List<string> InRibbonList => uMgr.InRibbonList;

		public UnitsDataR Lv1SelItem
		{
			set { lv1SelItem = value; }
		}

		public KeyValuePair<string, UnitsDataR> CbxSelItem
		{
			get => cbxSelItem;
			set
			{
				cbxSelItem = value;
				OnPropertyChanged();

				setUnitDisplay(cbxSelItem.Value.Ustyle.Name);
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

		public string InsertPosition
		{
			get => insertPosition.ToString();
			set
			{
				int val;
				bool result = Int32.TryParse(value, out val);

				if (!result || val < 2 || val > Count || val == insertPosition) return;

				insertPosition = val;
				// SelIndex = val;
				OnPropertyChanged();
			}
		}

		public string NewName
		{
			get => newName;
			set
			{
				if (value.Equals(newName)) return;

				hasNewName = !value.IsVoid();

				newName = value;
				OnPropertyChanged();
				OnPropertyChanged(nameof(CanAddNew));

			}
		}

		public string NewDesc
		{
			get => newDesc;
			set
			{
				if (value.Equals(newDesc)) return;

				hasNewDesc = !value.IsVoid();

				newDesc = value;
				OnPropertyChanged();
				OnPropertyChanged(nameof(CanAddNew));
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

		public int Lv1SelIndex
		{
			get => lv1SelIndex;
			set
			{
				if (value == lv1SelIndex) return;
				lv1SelIndex = value;
				OnPropertyChanged();
			}
		}

		public int Count => styles.Count;

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


		public bool CanAddNew
		{
			get { return true; }
		}

	#endregion

	#region private properties

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

		public void ProcessStyleChanges()
		{
			bool resultR = setStyleList(ribbonStyles, (int) UnitsSupport.ListToShowIn.RIBBON);
			bool resultDl = setStyleList(dialogLeftStyles, (int) UnitsSupport.ListToShowIn.DIALOG_LEFT);
			bool resultDr = setStyleList(dialogRightStyles, (int) UnitsSupport.ListToShowIn.DIALOG_RIGHT);
		}

	#endregion

	#region private methods

		private void Init()
		{
			initStatus = true;

			HasChanges = 0;

			configView();

			DialogIndex = 0;

			insertPosition = 0;

			uMgr.SetInRibbonList();

			OnPropertyChanged(nameof(StylesView));
			OnPropertyChanged(nameof(InsertPosition));
			OnPropertyChanged(nameof(InRibbonList));

			initStatus = false;
		}

		private void configView()
		{
			uMgr.SetInitialSequence();
			
			CollectionViewSource x =  CollectionViewSource.GetDefaultView(uMgr.StyleList);

			styles = (ListCollectionView) CollectionViewSource.GetDefaultView(uMgr.StyleList);

			styles.CurrentChanged += Styles_CurrentChanged;

			styles.SortDescriptions.Add(
				new SortDescription("Sequence", ListSortDirection.Ascending));
			styles.Filter = isNotDeleted;

			styles.IsLiveSorting = true;
		}


		private bool canAddNew()
		{
			// 


			return true;
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

		private void setInitialSequence() { }

		private bool isNotDeleted(object obj)
		{
			UnitsDataR udr = obj as UnitsUtil.UnitsDataR;

			return !udr.DeleteStyle;
		}

		private bool setStyleList(ListCollectionView view, int thisList)
		{
			view = (ListCollectionView) CollectionViewSource.GetDefaultView(uMgr.StyleList);

			view.Filter = item =>
			{
				if (item == null) return false;
				UnitsDataR udr = (UnitsDataR) item;

				return udr.Ustyle.ShowIn(thisList);
			};

			return view.Count > 0;
		}

	#endregion

	#region event publishing

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is class1";
		}

	#endregion


	#region event consuming

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
			Lv1SelIndex = 3;

			CbxSelIndex = 0;
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
			if (lv1SelIndex <= 1) return;

			int atIdx = ((UnitsDataR) styles.GetItemAt(lv1SelIndex)).Sequence;
			((UnitsDataR) styles.GetItemAt(lv1SelIndex)).Sequence -= 1;

			int pastIdx = ((UnitsDataR) styles.GetItemAt(lv1SelIndex - 1)).Sequence;
			((UnitsDataR) styles.GetItemAt(lv1SelIndex - 1)).Sequence += 1;

			HasChanges += 1;
			Debug.WriteLine("got move up");
		}

		private void BtnDn_OnClick(object sender, RoutedEventArgs e)
		{
			if (lv1SelIndex == Count - 1 || lv1SelIndex == 0) return;

			((UnitsDataR) styles.GetItemAt(lv1SelIndex)).Sequence += 1;
			((UnitsDataR) styles.GetItemAt(lv1SelIndex + 1)).Sequence -= 1;

			HasChanges += 1;
			Debug.WriteLine("got move down");
		}

		private void BtnDelete_OnClick(object sender, RoutedEventArgs e)
		{
			// Debug.WriteLine("");
			// Debug.WriteLine($"selIdx: {SelIndex:D2}  name: {selItem.Ustyle.Name}");
			// Debug.WriteLine($"   pos: {styles.CurrentPosition:D2}");
			// Debug.WriteLine("");

			if (lv1SelItem.Ustyle.IsLocked) return;

			lv1SelItem.DeleteStyle = true;
			lv1SelItem.Sequence = 0;

			int idx = lv1SelIndex;
			int count = styles.Count;

			if (lv1SelIndex < Count - 1)
			{
				uMgr.ReSequenceStylesList(styles, lv1SelIndex + 1);
			}

			HasChanges += 1;
			// Debug.WriteLine("got delete");

			styles.Refresh();

			Lv1SelIndex = idx <= styles.Count - 1 ? idx : styles.Count - 1;

			// showList();
		}

		private void showList()
		{
			Debug.WriteLine("");
			Debug.WriteLine($"view count: {styles.Count}");
			Debug.WriteLine($"list count: {uMgr.StyleList.Count}");
			Debug.WriteLine($"this count: {Count}");
			Debug.WriteLine("");

			int i = 0;
			foreach (KeyValuePair<string, UnitsDataR> kvp in uMgr.StyleList)
			{
				string seq = kvp.Value.Sequence == 0 ? "  " : $"{(kvp.Value.Sequence + 1):D2}";
				string iseq = kvp.Value.InitialSequence == 0 ? "  " : $"{(kvp.Value.Sequence + 1):D2}";


				Debug.WriteLine($"idx: {i++:D2}  seq: {seq}  intseq:  {kvp.Value.InitialSequence:D2}  seq: {kvp.Value.Sequence:D2}   isDel: {kvp.Value.DeleteStyle,6}  name: {kvp.Value.Ustyle.Name}");
			}

			Debug.WriteLine("");
			Debug.WriteLine("");
		}


		private void BtnDebug_OnClick(object sender, RoutedEventArgs e)
		{
			int idx = Lv1SelIndex;

			UnitsDataR udr = DetailUnit;
			UStyle us = udr.Ustyle;

			UnitsDataR udrl = lv1SelItem;
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
			int idx = lv1SelIndex;

			// showList();

			uMgr.UnDelete();
			uMgr.ResetInitialSequence();

			HasChanges = 0;

			styles.Refresh();

			// showList();

			Lv1SelIndex = idx;
		}

		private void BtnAddBefore_OnClick(object sender, RoutedEventArgs e) { }

		private void BtnAddLast_OnClick(object sender, RoutedEventArgs e) { }

	#endregion
	}
}