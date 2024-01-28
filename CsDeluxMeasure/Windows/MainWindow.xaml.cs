#region using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using CsDeluxMeasure.RevitSupport;
using CsDeluxMeasure.RevitSupport.ExtEvents;
using CsDeluxMeasure.UnitsUtil;
using CsDeluxMeasure.Windows.Support;
using SettingsManager;
using UtilityLibrary;
using static CsDeluxMeasure.RevitSupport.RevitUtil;
using Visibility = System.Windows.Visibility;
using Rectangle = System.Drawing.Rectangle;

#endregion

// projname: CsDeluxMeasure
// itemname: MainWindow
// username: jeffs
// created:  2/12/2022 8:46:31 AM

namespace CsDeluxMeasure.Windows
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : INotifyPropertyChanged, IWin
	{
		private const string DialogToWatch = "Dialog_Revit_Units";

	#region private fields

		private UnitsManager uMgr = UnitsManager.Instance;

		private UnitsInListsCurrent uCurr = new UnitsInListsCurrent();

		private MainWindow mainWin;

		private ExtEvtHandler exHandler;
		private ExternalEvent exEvent;

		private bool showWorkPlane;

		private PointMeasurements points;

		private PointDistances distancesPrime;
		private PointDistances distancesSecond;

		// private UnitsDataR primeUnit;
		private int selIdxLeft;
		private UnitsDataR selItemLeft;

		// private UnitsDataR secondUnit;
		private int selIdxRight;
		private UnitsDataR selItemRight;

		private bool hasChanges;

		private UIDocument uiDoc;

		private bool hideMain;
		private bool showMini;
		private bool unitsDialogBoxDisplayed;

		private string message;
		private string msg;
		private string textMsg02 = String.Empty;


	#endregion

	#region ctor

		public MainWindow(UIDocument uiDoc, ExtEvtHandler exHandler, ExternalEvent exEvent)
		{

			// BindingErrorTraceListener.SetTrace();

			InitializeComponent();

			// M.W = new AWindow(this);

			this.uiDoc = uiDoc;
			this.exHandler = exHandler;
			this.exEvent = exEvent;

			distancesPrime = new PointDistances();
			distancesSecond = new PointDistances();

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
				unitsDialogBoxDisplayed = true;
			}
		}

	#endregion

	#region public properties

		public bool UnitsDialogBoxDisplayed
		{
			get => unitsDialogBoxDisplayed;
			set
			{
				unitsDialogBoxDisplayed = value;

				MainWindowCommand.UpdatePoints();
			}
		}


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
		
		public string MessageBoxText2
		{
			get => textMsg02;
			set
			{
				textMsg02 = value; 
				OnPropertyChanged();
			}
		}

		public List<UnitsDataR> UsrStylesList => UserSettings.Data.UserStyles;

		public ListCollectionView DialogLeftList => uCurr.InListViewDlgLeft;
		public ListCollectionView DialogRightList => uCurr.InListViewDlgRight;

		public bool ShowWorkplane
		{
			get => showWorkPlane;
			set
			{
				showWorkPlane = value;
				ShowHideWorkplane();
			}
		}

		public string PrimeArea
		{
			get
			{
				// if (M.W != null) M.W.WriteLine1($"get prime area| {DistancesPrime?.Area ?? "null"}");

				return DistancesPrime?.Area ?? "null";
			}
		}

		public string SecondArea => DistancesSecond?.Area ?? "null";


		public PointMeasurements Points
		{
			set
			{
				// M.W.WriteLine1("\nset points-begin");
				DistancesPrime.Points = value;
				DistancesSecond.Points = value;

				// OnPropertyChanged(nameof(DistancesPrime));
				// OnPropertyChanged(nameof(DistancesPrime.Area));
				// OnPropertyChanged(nameof(DistancesSecond));
				// OnPropertyChanged(nameof(DistancesSecond.Area));

				// OnPropertyChanged(nameof(PrimeArea));
				// OnPropertyChanged(nameof(SecondArea));

				// M.W.WriteLine1("set points-end\n");
			}
		}

		public PointDistances DistancesPrime
		{
			get => distancesPrime;
			set
			{
				distancesPrime = value;
				OnPropertyChanged();
				OnPropertyChanged(nameof(PrimeArea));
			}
		}

		public PointDistances DistancesSecond
		{
			get => distancesSecond;
			set
			{
				distancesSecond = value;
				OnPropertyChanged();
				OnPropertyChanged(nameof(SecondArea));
			}
		}

		public string SelItemLeftName => SelItemLeft?.Name ?? "Undefined";

		public UnitsDataR SelItemLeft
		{
			get => selItemLeft;
			set
			{
				if (value == null) return;

				selItemLeft = value;
				OnPropertyChanged();

				updateLeft(selItemLeft);

				OnPropertyChanged(nameof(SelItemLeftName));
			}
		}

		public int SelIdxLeft
		{
			get => selIdxLeft;
			set
			{
				selIdxLeft = value;
				OnPropertyChanged();
			}
		}

		public string SelItemRightName => SelItemRight?.Name ?? "Undefined";

		public UnitsDataR SelItemRight
		{
			get => selItemRight;
			set
			{
				if (value == null) return;

				selItemRight = value;
				OnPropertyChanged();

				updateRight(selItemRight);

				OnPropertyChanged(nameof(SelItemRightName));
			}
		}

		public int SelIdxRight
		{
			get => selIdxRight;
			set
			{
				selIdxRight = value;
				OnPropertyChanged();
			}
		}

		// public bool UseMiniWin2
		// {
		// 	get => UserSettings.Data.UseMiniWin;
		// 	set
		// 	{
		// 		if (value)
		// 		{
		// 			UserSettings.Data.ShowMiniWin = true;
		// 		}
		//
		// 		UserSettings.Data.UseMiniWin = value;
		// 		UserSettings.Admin.Write();
		// 		
		// 		OnPropertyChanged();
		//
		// 		DisplayMiniWin();
		// 	}
		// }
		//
		// public bool UseMiniWin
		// {
		// 	get	=> MainWindowCommand.DontHideMain;
		// 	set => MainWindowCommand.DontHideMain = value;
		// }


		public bool OneTimeView { get; set; }

		public bool HasChanges
		{
			get => hasChanges;
			set => hasChanges = value;
		}

		public PointMeasurements? MeasuredPoints => points;

	#endregion

	#region private properties

		private UIDocument _uiDoc => uiDoc;
		private Document _doc => uiDoc.Document;
		private MainWindow Main => mainWin;

		private WindowLocation _Location { get; set; } = new WindowLocation(0, 0);

	#endregion

	#region public methods

		/*
		public void TestAppSettings()
		{
			IList<ForgeTypeId> validUnits = UnitUtils.GetValidUnits(SpecTypeId.Length);

			Tuple<ForgeTypeId, bool, IList<ForgeTypeId>>[] answers =
				new Tuple<ForgeTypeId, bool, IList<ForgeTypeId>>[validUnits.Count];

			for (int i = 0; i < validUnits.Count; i++)
			{
				bool result = FormatOptions.CanHaveSymbol(validUnits[i]);
				IList<ForgeTypeId> symbols = null;

				if (result)
				{
					symbols = FormatOptions.GetValidSymbols(validUnits[i]);
				}

				answers[i] = new Tuple<ForgeTypeId, bool, IList<ForgeTypeId>>(validUnits[i], result, symbols);
			}


			WriteNewLine();
			WriteLine2("status", "| ", AppSettings.Admin.Status);
			WriteLine2("path", "| ", AppSettings.Admin.Path);
			WriteLine2("status", "| ", "reading");
			AppSettings.Admin.Read();
			WriteLine2("test value", "| ", AppSettings.Data.AppSettingsValue);
			WriteLine2("change value", "| ", "to += 1");

			AppSettings.Data.AppSettingsValue += 1;

			AppSettings.Admin.Write();

			WriteLine2("app settings", "| ", "written");
			AppSettings.Data.AppSettingsValue = 0;
			WriteLine2("change value", "| ", $"to {AppSettings.Data.AppSettingsValue}");

			ShowMsg();

			MessageBox.Show(this, textMsg01);
		}

		
		public bool Measure(MainWindow main)
		{
			mainWin = main;

			View av = _doc.ActiveView;

			RevitUtil.VType vtype = RevitUtil.GetViewType(av);

			if (vtype.VTCat == RevitUtil.VTtypeCat.OTHER) return false;

			Plane plane = av.SketchPlane?.GetPlane();

			if (plane == null && (vtype.VTCat == RevitUtil.VTtypeCat.D2_WITHPLANE
				|| vtype.VTCat == RevitUtil.VTtypeCat.D3_WITHPLANE))
			{
				// cannot use the current workplane - make a good one
				using (Transaction t = new Transaction(_doc, "Delux Measure Points"))
				{
					t.Start();
					{
						plane = Plane.CreateByNormalAndOrigin(
							_doc.ActiveView.ViewDirection,
							new XYZ(0, 0, 0));

						SketchPlane sp = SketchPlane.Create(_doc, plane);

						av.SketchPlane = sp;
					}
					t.Commit();
				}
			}

			measurePointsConfig(av, vtype);

			return true;
		}

		*/

		public bool ShowHideWorkplane()
		{
			View av = _doc.ActiveView;
			Plane p = av.SketchPlane?.GetPlane();

			showHideWorkplane(p, av);

			return true;
		}

		public void SetPosition(Window parentWindow)
		{
			// revitWindow = parentWindow;
			//
			// double t = UserSettings.Data.WinPosDeluxMeasure.Top;
			// double l = UserSettings.Data.WinPosDeluxMeasure.Left;
			//
			// this.Owner = parentWindow;
			//
			// // this.Top = t > 0 ? t : parentWindow.Top;
			// // this.Left = l > 0 ? l : parentWindow.Left;
			//
			// this.Top = t > 0 ? t : this.Top;
			// this.Left = l > 0 ? l : this.Left;

			double t = UserSettings.Data.WinPosDeluxMeasure.Top;
			double l = UserSettings.Data.WinPosDeluxMeasure.Left;

			if (t <= 0 && l <= 0)
			{
				Rectangle r = WindowUtilities.GetWinPositionWinOffset(this, parentWindow, 100, 250);

				t = r.Top;
				l = r.Left;
			}

			this.Top = t;
			this.Left = l;
		}

	#endregion

	#region private methods

		public void init()
		{
			// M.W.WriteLine1("\ninit-begin");
			UnitsManager.Doc = _doc;

			UserSettings.Admin.Read();

			string left = UserSettings.Data.DialogLeftSelItemName;
			string right = UserSettings.Data.DialogRightSelItemName;

			createStyleViews();
			getSavedSelected(left, right);


			// M.W.WriteLine1("init-update-props");

			OnPropertyChanged(nameof(PrimeArea));
			OnPropertyChanged(nameof(SecondArea));
			OnPropertyChanged(nameof(DistancesPrime));
			OnPropertyChanged(nameof(DistancesSecond));


			// MainWindowCommand.mm.Prime.Points = points;
			// MainWindowCommand.mm.Second.Points = points;
			//
			// MainWindowCommand.mm.UpdatePrime();
			// MainWindowCommand.mm.UpdateSecond();

			// MainWindowCommand.mm.ParentWin = this;

			// DisplayMiniWin();

			// M.W.WriteLine1("init-end\n");
		}

		private void createStyleViews()
		{
			// M.W.WriteLine1("\ncreate-styles-begin");

			uMgr.UpdateProjectStyleSettings();

			uCurr.ConfigInListsView(InList.DIALOG_LEFT, UsrStylesList);
			uCurr.ConfigInListsView(InList.DIALOG_RIGHT, UsrStylesList);

			OnPropertyChanged(nameof(DialogLeftList));
			OnPropertyChanged(nameof(DialogRightList));

			// M.W.WriteLine1("create-styles-end\n");
		}

		// private void getSavedSelected2(string leftKey, string rightKey)
		// {
		// 	// must occur after the views are set
		// 	distancesPrime.UnitStyle = getSavedSelStyle(DialogLeftList, leftKey);
		//
		// 	SelIdxLeft = DialogLeftList.IndexOf(distancesPrime.UnitStyle);
		//
		// 	distancesSecond.UnitStyle = getSavedSelStyle(DialogRightList, rightKey);
		//
		// 	SelIdxRight = DialogRightList.IndexOf(distancesSecond.UnitStyle);
		// }

		private UnitsDataR getSavedSelStyle(ListCollectionView view, string key)
		{
			UnitsDataR udr = UsrStylesList[uMgr.ProjectStyleIdx];

			if (key.IsVoid())
			{
				return udr;
			}

			foreach (UnitsDataR o in view)
			{
				if (o.Name.Equals(key))
				{
					udr = o;
					break;
				}
			}

			return udr;
		}

		private void getSavedSelected(string leftKey, string rightKey)
		{
			// M.W.WriteLine1("\nsaved-styles-begin");
			int result;

			result = getSavedSelStyleIdx(DialogLeftList, leftKey);
			// M.W.WriteLine1("Idx-left-pre-set");
			SelIdxLeft = result;
			// M.W.WriteLine1("Idx-left-post-set");

			result = getSavedSelStyleIdx(DialogRightList, rightKey);
			SelIdxRight = result;
			// M.W.WriteLine1("saved-styles-end\n");
		}

		private int getSavedSelStyleIdx(ListCollectionView view, string key)
		{
			if (key.IsVoid())
			{
				return 0;
			}

			int i = 0;

			foreach (UnitsDataR o in view)
			{
				if (o.Name.Equals(key))
				{
					return i;
				}

				i++;
			}

			return 0;
		}

		// private UnitsDataR getSavedSelStyleIdx(ListCollectionView view, string key, out int idx)
		// {
		// 	idx = -1;
		//
		// 	UnitsDataR udr = UsrStylesList[uMgr.ProjectStyleIdx];
		//
		// 	if (key.IsVoid())
		// 	{
		// 		idx = -1;
		// 		return udr;
		// 	}
		//
		// 	int i = -1;
		//
		// 	foreach (UnitsDataR o in view)
		// 	{
		// 		i++;
		// 		if (o.Name.Equals(key))
		// 		{
		// 			udr = o;
		// 			idx = i;
		// 			break;
		// 		}
		// 	}
		//
		// 	return udr;
		// }

		private void updateLeft(UnitsDataR udr)
		{
			distancesPrime.UnitStyle = udr;
			OnPropertyChanged(nameof(DistancesPrime));
			OnPropertyChanged(nameof(PrimeArea));

			MainWindowCommand.Mm.Prime = distancesPrime;

			UserSettings.Data.DialogLeftSelItemName = udr.Name;
			UserSettings.Admin.Write();

			hasChanges = true;
		}

		private void updateRight(UnitsDataR udr)
		{
			distancesSecond.UnitStyle = udr;
			OnPropertyChanged(nameof(DistancesSecond));

			MainWindowCommand.Mm.Second = distancesSecond;

			UserSettings.Data.DialogRightSelItemName = udr.Name;
			UserSettings.Admin.Write();

			hasChanges = true;
		}

		internal static void DisplayFile(string location)
		{
			System.Diagnostics.Process process = new System.Diagnostics.Process();

			try
			{
				process.StartInfo.FileName = "explorer";
				process.StartInfo.Arguments = location;
				process.StartInfo.CreateNoWindow = true;
				process.StartInfo.UseShellExecute = false;
				process.Start();
			}
			catch (Exception e)
			{
				Debug.Print(e.Message);
			}
		}


		internal void DisplayMiniWin()
		{
			if (!CkBxShowMiniWin)
			{
				MainWindowCommand.Mm.Visibility = Visibility.Hidden;
				return;
			}

			// MainWindowCommand.mm.Prime = distancesPrime;
			// MainWindowCommand.mm.Second = distancesSecond;

			MainWindowCommand.Mm.Visibility = Visibility.Visible;
		}

		// measure points

		/*
		private bool measurePointsConfig(View av, RevitUtil.VType vtype)
		{
			XYZ normal = XYZ.BasisZ;
			XYZ workingOrigin = XYZ.Zero;
			XYZ actualOrigin = XYZ.Zero;


			if (RevitUtil.IsPlaneOrientationAcceptable(_uiDoc))
			{
				Plane p = av.SketchPlane?.GetPlane();
				string planeName = av.SketchPlane?.Name ?? "No Name";

				if (p != null)
				{
					normal = p.Normal;
					actualOrigin = p.Origin;

					if (vtype.VTSub != VTypeSub.D3_VIEW)
					{
						workingOrigin = p.Origin;
					}
				}

				return MeasurePoints(workingOrigin, vtype, planeName);
			}
			else
			{
				TaskDialog td = new TaskDialog("Workplane Orientation");
				td.MainIcon = TaskDialogIcon.TaskDialogIconWarning;
				td.MainInstruction = "The angle of the Worlplane is too steep in this view.  "
					+ "Please adjust the Workplane's orientation or "
					+ "make a different Workplane active in order to proceed";
				td.CommonButtons = TaskDialogCommonButtons.Cancel;
				td.DefaultButton = TaskDialogResult.Cancel;

				TaskDialogResult result = td.Show();
			}

			return false;
		}
		*/

		/*
		private bool MeasurePoints(XYZ workingOrigin, RevitUtil.VType vtype, string planeName)
		{
			// bool again = true;
			// bool? result;

			// PointMeasurements? pm;

			// do
			// {
			points = GetPts(workingOrigin);

			if (points.IsValid)
			{
				DistancesPrime.Points = points;
				DistancesSecond.Points = points;

				// result = Main.ShowDialog();
				//
				// if (result.HasValue && !result.Value)
				// {
				// 	again = false;
				// 	break;
				// }
			}
			else
			{
				TaskDialog td = new TaskDialog("No Points Selected");
				td.MainIcon = TaskDialogIcon.TaskDialogIconWarning;
				td.MainInstruction = "Please select two points to proceed";
				td.CommonButtons = TaskDialogCommonButtons.Cancel | TaskDialogCommonButtons.Retry;
				td.DefaultButton = TaskDialogResult.Retry;

				TaskDialogResult tdResult = td.Show();

				if (tdResult == TaskDialogResult.Cancel)
				{
					return false;
				}
			}
			// }
			// while (again);

			return true;
		}
		*/


		// workplane

		private bool showHideWorkplane(Plane p, View av)
		{
			if (p == null) { return false; }

			try
			{
				using (Transaction t = new Transaction(_doc, "measure points"))
				{
					t.Start();

					if (ShowWorkplane)
					{
						av.ShowActiveWorkPlane();
					}
					else
					{
						av.HideActiveWorkPlane();
					}

					t.Commit();
				}
			}
			catch
			{
				return false;
			}

			return true;
		}

	#endregion

	#region event consuming

		// window

		private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
		{
			// _Location = UserSettings.Data.WinPosDeluxMeasure;
			//
			// if (_Location.Top > 0 && _Location.Left > 0)
			// {
			// 	this.Top = _Location.Top;
			// 	this.Left = _Location.Left;
			// }

			SetPosition(this.Owner);

			// init();
		}

		private void MainWindow_OnClosing(object sender, CancelEventArgs e)
		{
			UserSettings.Data.WinPosDeluxMeasure = new WindowLocation(this.Top, this.Left);
			UserSettings.Admin.Write();

			if (OneTimeView)
			{
				this.Visibility = Visibility.Hidden;
				OneTimeView = false;
			}

			// uiDoc.Application.DialogBoxShowing -= Application_DialogBoxShowing;
			// uiDoc.Application.ViewActivating -= Application_ViewActivating;
		}

		private void MainWin_ProcessF1(object sender, KeyEventArgs e)
		{
			e.Handled = true;
		}

		// controls


		// public bool HideMain
		// {
		// 	get => !MainWindowCommand.HideMain.HasValue || MainWindowCommand.HideMain.Value;
		// 	set
		// 	{
		// 		if (value == MainWindowCommand.HideMain) return;
		//
		// 		MainWindowCommand.HideMain = value;
		//
		// 		OnPropertyChanged();
		// 	}
		// }


		private void Btn_SelPoints_OnClick(object sender, RoutedEventArgs e)
		{
			// OnPropertyChanged(nameof(PrimeArea));

			
			// M.W.WriteLine1($"points measure version 1| {points.version}");
			// M.W.WriteLine1($"points measure version 2| {distancesPrime.Points.version}");
			// M.W.WriteLine1($"points dist version 1   | {distancesPrime.version}");
			// M.W.WriteLine1($"points area             | {distancesPrime.Points.Area}");


			// M.W.WriteLine1($"dist area  | {distancesPrime.Area}");
			// M.W.WriteLine1($"prime area | {PrimeArea}");
			
			MainWindowCommand.SelectPoints();
		}


		public void UpdateCkBxHideMain(bool value)
		{
			MainWindowCommand.UpdateHideMain(value);
			OnPropertyChanged(nameof(CkBxHideMain));
		}

		// only use mini results window - invert of showmain
		public bool? CkBxHideMain
		{
			get => UserSettings.Data.HideMain;
			set
			{
				// Debug.WriteLine($"main| CkBxHideMain| {value}");

				MainWindowCommand.UpdateHideMain( value.HasValue ? value.Value : true);

				OnPropertyChanged();

				MainWindowCommand.Dlg_HideMain(value.Value);
			}
		}

		public void UpdateCkBxShowMiniWin(bool value)
		{
			MainWindowCommand.UpdateShowMini(value);
			OnPropertyChanged(nameof(CkBxHideMain));
		}

		// show mini results window
		public bool CkBxShowMiniWin
		{
			get => UserSettings.Data.ShowMiniWin;
			set
			{
				Debug.WriteLine($"main| CkBxShowMiniWin| {value}");

				MainWindowCommand.UpdateShowMini(value);

				OnPropertyChanged();

				MainWindowCommand.Dlg_ShowMini(value);
			}
		}

		private void MainWindow_OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			MainWindowCommand.Dlg_WinVisibilityChanged(IsVisible);
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

		public void HideMain(bool value)
		{
			if (value)
			{
				HideMe();
			}
			else
			{
				ShowMe();
			}
		}

		
		public event PropertyChangedEventHandler PropertyChanged;

		// [DebuggerStepThrough]
		protected void OnPropertyChanged([CallerMemberName] string memberName = "")
		{
			message = $"on prop| member| {memberName}\n";

			if (!memberName.Equals(nameof(MessageBoxText)))
			{
				MessageBoxText += message;
			}


			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

		private void BtnDone_OnClick(object sender, RoutedEventArgs e)
		{
			UserSettings.Data.WinPosDeluxMeasure = new WindowLocation(this.Top, this.Left);
			UserSettings.Admin.Write();

			// if (OneTimeView)
			// {
			// 	this.Visibility = Visibility.Hidden;
			// 	OneTimeView = false;
			// }

			// HideMe();
			// this.Visibility = Visibility.Hidden;

			this.Hide();
			Owner.Activate();
			User32dll.SendKeyCode(0x01);
		}

		private void BtnHelp_OnClick(object sender, RoutedEventArgs e)
		{
			uMgr.UlMgr.Rio.SB.GetContextualHelp().Launch();
		}

		private void BtnAbout_OnClick(object sender, RoutedEventArgs e)
		{
			About a = new About(this);
			a.ShowDialog();
		}

	#endregion

	#region event publishing

		// public event PropertyChangedEventHandler PropertyChanged;
		//
		// protected override void OnPropertyChange([CallerMemberName] string memberName = "")
		// {
		// 	PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		// }

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is MainWindow";
		}

	#endregion

	#region basic system to get points

/*
		public void SetPointsToZero()
		{
			DistancesPrime.Points = PointMeasurements.Zero();
			DistancesSecond.Points = PointMeasurements.Zero();
		}

		public bool DxMeasure()
		{
			bool result;

			MainWindowCommand.mm.Owner = this.Owner;

			View av = _doc.ActiveView;
			RevitUtil.VType vtype = RevitUtil.GetViewType(av);

			dxPrepPlane(av, vtype);
			result = dxMeasurePoints(av, vtype);

			if (!result) return false;

			init();

			if (UseMiniWin)
			{
				result = false;
			}
			else
			{
				result = this.ShowDialog() ?? false;
			}
			
			return result;
		}

		private void dxPrepPlane(View av, RevitUtil.VType vtype)
		{
			Plane plane = av.SketchPlane?.GetPlane();

			if (plane == null && (vtype.VTCat == RevitUtil.VTtypeCat.D2_WITHPLANE
				|| vtype.VTCat == RevitUtil.VTtypeCat.D3_WITHPLANE))
			{
				// cannot use the current workplane - make a good one
				using (Transaction t = new Transaction(_doc, "Delux Measure Points"))
				{
					t.Start();
					{
						plane = Plane.CreateByNormalAndOrigin(
							_doc.ActiveView.ViewDirection,
							new XYZ(0, 0, 0));

						SketchPlane sp = SketchPlane.Create(_doc, plane);

						av.SketchPlane = sp;
					}
					t.Commit();
				}
			}
		}

		private bool dxMeasurePoints(View av, RevitUtil.VType vtype)
		{
			// XYZ normal = XYZ.BasisZ;
			// XYZ actualOrigin = XYZ.Zero;
			XYZ workingOrigin = XYZ.Zero;

			if (RevitUtil.IsPlaneOrientationAcceptable(_uiDoc))
			{
				Plane p = av.SketchPlane?.GetPlane();
				string planeName = av.SketchPlane?.Name ?? "No Name";

				if (p != null)
				{
					// normal = p.Normal;
					// actualOrigin = p.Origin;

					if (vtype.VTSub != VTypeSub.D3_VIEW)
					{
						workingOrigin = p.Origin;
					}
				}

				return dxGetPoints(workingOrigin);
			}
			else
			{
				TaskDialog td = new TaskDialog("Workplane Orientation");
				td.MainIcon = TaskDialogIcon.TaskDialogIconWarning;
				td.MainInstruction = "The angle of the Worlplane is too steep in this view.  "
					+ "Please adjust the Workplane's orientation or "
					+ "make a different Workplane active in order to proceed";
				td.CommonButtons = TaskDialogCommonButtons.Cancel;
				td.DefaultButton = TaskDialogResult.Cancel;

				TaskDialogResult result = td.Show();
			}

			return false;
		}

		private bool dxGetPoints(XYZ workingOrigin)
		{
			bool result;

			points = GetPts(workingOrigin);

			if (!points.IsValid)
			// {
			// 	
			// 	DistancesPrime.Points = points;
			// 	DistancesSecond.Points = points;
			// 	// OnPropertyChanged(nameof(DistancesPrime));
			// 	// OnPropertyChanged(nameof(DistancesSecond));
			//
			// 	// MainWindowCommand.mm.Prime.Points = points;
			// 	// MainWindowCommand.mm.Second.Points = points;
			// 	//
			// 	// MainWindowCommand.mm.UpdatePrime();
			// 	// MainWindowCommand.mm.UpdateSecond();
			//
			// }
			// else
			{
				TaskDialog td = new TaskDialog("No Points Selected");
				td.MainIcon = TaskDialogIcon.TaskDialogIconWarning;
				td.MainInstruction = "Please select two points to proceed";
				td.CommonButtons = TaskDialogCommonButtons.Cancel | TaskDialogCommonButtons.Retry;
				td.DefaultButton = TaskDialogResult.Retry;

				TaskDialogResult tdResult = td.Show();

				if (tdResult == TaskDialogResult.Cancel)
				{
					return false;
				}
			}

			return true;
		}

		private PointMeasurements GetPts(XYZ workingOrigin)
		{
			XYZ startPoint;
			XYZ endPoint;
			double rotation;

			// XYZ startPointRotated;
			// XYZ endPointRotated;
			//
			// double xLen;
			// double yLen;
			//
			// double xLenR;
			// double yLenR;

			Owner.Focus();

			try
			{
				View avStart = _doc.ActiveView;

				rotation = Math.Atan(avStart.UpDirection.X / avStart.UpDirection.Y);

				// Transform t = Transform.CreateRotation(XYZ.BasisZ, rotation);

				startPoint = _uiDoc.Selection.PickPoint(snaps, "Select Point");
				if (startPoint == null) return PointMeasurements.InValid();

				View avEnd = _doc.ActiveView;

				// cannot change views between points
				if (avStart.Id.IntegerValue != avEnd.Id.IntegerValue)
				{
					return PointMeasurements.InValid();
				}

				endPoint = _uiDoc.Selection.PickPoint(snaps, "Select Point");
				if (endPoint == null) return PointMeasurements.InValid();

				// startPointRotated = t.OfPoint(startPoint);
				// endPointRotated = t.OfPoint(endPoint);
				//
				// xLen = endPoint.X-startPoint.X ;
				// yLen = endPoint.Y-startPoint.Y ;
				//
				// xLenR = endPointRotated.X-startPointRotated.X ;
				// yLenR = endPointRotated.Y-startPointRotated.Y ;
				//
				// Debug.WriteLine($"rotation| {rotation}");
				//
				// Debug.WriteLine($"original start| {startPoint.X} , {startPoint.Y}");
				// Debug.WriteLine($"original   end| {endPoint.X} , {endPoint.Y}");
				// Debug.WriteLine($"original x len| {xLen}   yLen| {yLen}");
				//
				// Debug.WriteLine($"rotated  start| {startPointRotated.X} , {startPointRotated.Y}");
				// Debug.WriteLine($"rotated    end| {endPointRotated.X} , {endPointRotated.Y}");
				// Debug.WriteLine($"rotated  x len| {xLenR}   yLen| {yLenR}");


			}
			catch
			{
				return PointMeasurements.InValid();
			}

			return new PointMeasurements(startPoint, endPoint, workingOrigin, rotation);
		}
*/

	#endregion

		private void Btn_Debug_OnClick(object sender, RoutedEventArgs e)
		{
			UserSettingDataFile d = UserSettings.Data;


			string p = distancesPrime.UnitStyle.Name ;
			string s = distancesSecond.UnitStyle.Name;


			int a = 1;
		}

	}

	public class BindingErrorTraceListener : DefaultTraceListener
	{
		//http://www.switchonthecode.com/tutorials/wpf-snippet-detecting-binding-errors
		private static BindingErrorTraceListener _Listener;

		public static void SetTrace()
		{
			SetTrace(SourceLevels.Error, TraceOptions.None);
		}

		public static void SetTrace(SourceLevels level, TraceOptions options)
		{
			if (_Listener == null)
			{
				_Listener = new BindingErrorTraceListener();
				PresentationTraceSources.DataBindingSource.Listeners.Add(_Listener);
			}

			_Listener.TraceOutputOptions = options;
			PresentationTraceSources.DataBindingSource.Switch.Level = level;
		}

		public static void CloseTrace()
		{
			if (_Listener == null)
			{
				return;
			}

			_Listener.Flush();
			_Listener.Close();
			PresentationTraceSources.DataBindingSource.Listeners.Remove(_Listener);
			_Listener = null;
		}

		private StringBuilder _Message = new StringBuilder();
		private BindingErrorTraceListener() { }

		public override void Write(string message)
		{
			_Message.Append(message);
		}

		public override void WriteLine(string message)
		{
			_Message.Append(message);

			var final = _Message.ToString();
			_Message.Length = 0;

			MessageBox.Show(final, "Binding Error", MessageBoxButton.OK,
				MessageBoxImage.Error);
		}
	}


	//
	// public void ShowMain(bool value)
	// {
	// 	UpdateCkBxHideMain(value);
	//
	// 	if (!value)
	// 	{
	// 		ShowMe();
	// 	}
	// 	else
	// 	{
	// 		HideMe();
	// 	}
	//
	//
	// }

	//
	// public bool HideMain
	// {
	// 	get => hideMain;
	// 	set
	// 	{
	// 		Debug.WriteLine($"main| showmain| {value}");
	//
	// 		hideMain = value;
	// 		OnPropertyChanged();
	//
	// 		CkBxHideMain = !value;
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
	//
	// public bool ShowMini
	// {
	// 	get => showMini;
	// 	set
	// 	{
	// 		Debug.WriteLine($"main| showmini| {value}");
	// 		showMini = value;
	// 		OnPropertyChanged();
	//
	// 		CkBxShowMiniWin = value;
	// 	}
	// }


	// private void MainWindow_OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
	// {
	// 	Debug.WriteLine($"main| visibility changed| {e.NewValue} | {(((bool) e.NewValue) ? "visible" : "hide" )}");
	//
	// 	MainWindowCommand.DialogNotVisible(((bool) e.NewValue));
	// }
}