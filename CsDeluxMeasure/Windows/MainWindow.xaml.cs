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
using Tests01.RevitSupport;
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

		private const string OrthogonalView = "Orthogonal to View";
		private const string OrthogonalModel = "Orthogonal to Model";

	#region private fields

		private UnitsManager uMgr = UnitsManager.Instance;

		private UnitsInListsCurrent uCurr = new UnitsInListsCurrent();

		private MainWindow mainWin;

		private ExtEvtHandler exHandler;
		private ExternalEvent exEvent;

		private bool showWorkPlane;

		// private PointMeasurements points;

		private PointDistances distancesPrime;
		private PointDistances distancesSecond;

		// private UnitsDataR primeUnit;
		private int selIdxLeft;
		private UnitsDataR selItemLeft;

		// private UnitsDataR secondUnit;
		private int selIdxRight;
		private UnitsDataR selItemRight;

		private bool hasChanges;

		// private UIDocument uiDoc;

		private bool hideMain;
		private bool showMini;

		private bool unitsDialogBoxDisplayed;
		private bool gotMouse = false;

		private string message;
		private string msg;
		private string textMsg02 = String.Empty;

		private bool showMiniMainEnable = true;

	#endregion

	#region ctor

		// public MainWindow(UIDocument uiDoc, ExtEvtHandler exHandler, ExternalEvent exEvent)
		public MainWindow()

		{
			// BindingErrorTraceListener.SetTrace();

			InitializeComponent();

			// M.W = new AWindow(this);

			// this.uiDoc = uiDoc;
			// this.exHandler = exHandler;
			// this.exEvent = exEvent;

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

		public bool                     UnitsDialogBoxDisplayed
		{
			get =>                      unitsDialogBoxDisplayed;
			set
			{
				unitsDialogBoxDisplayed = value;

				R.Dx.UpdatePoints();
			}
		}


		public string                   textMsg01
		{
			get =>                      msg;
			set
			{
				msg                     = value;
				OnPropertyChanged(nameof(MessageBoxText));
			}
		}

		public string                   MessageBoxText
		{
			get =>                      textMsg01;
			set
			{
				textMsg01               = value;
				OnPropertyChanged();
			}
		}

		public string                   MessageBoxText2
		{
			get =>                      textMsg02;
			set
			{
				textMsg02               = value;
				OnPropertyChanged();
			}
		}

		public List<UnitsDataR>         UsrStylesList => UserSettings.Data.UserStyles;

		public ListCollectionView       DialogLeftList => uCurr.InListViewDlgLeft;
		public ListCollectionView       DialogRightList => uCurr.InListViewDlgRight;

		public bool                     ShowWorkplane
		{
			get =>                      showWorkPlane;
			set
			{
				showWorkPlane           = value;
				ShowHideWorkplane();
			}
		}

		public string                   PrimeArea => DistancesPrime?.Area ?? "null";

		public string                   SecondArea => DistancesSecond?.Area ?? "null";

		public PointMeasurements        Points
		{
			set
			{
				DistancesPrime.Points   = value;
				DistancesSecond.Points  = value;
			}
		}

		public PointDistances           DistancesPrime
		{
			get =>                      distancesPrime;
			set
			{
				distancesPrime          = value;
				OnPropertyChanged();
				OnPropertyChanged(nameof(PrimeArea));
				OnPropertyChanged(nameof(DistancesSubTitle));
			}
		}

		public PointDistances           DistancesSecond
		{
			get =>                      distancesSecond;
			set
			{
				distancesSecond         = value;
				OnPropertyChanged();
				OnPropertyChanged(nameof(SecondArea));
				OnPropertyChanged(nameof(DistancesSubTitle));
			}
		}

		// ui data
		public string                   DistancesSubTitle
		{
			get
			{
				if                      ((DistancesPrime?.Points.Rotation ?? 1.0) > 0.0)
				{
					return                        OrthogonalView;
				}

				return                  OrthogonalModel;
			}
		}

		public string                   SelItemLeftName => SelItemLeft?.Name ?? "Undefined";

		public UnitsDataR               SelItemLeft
		{
			get =>                      selItemLeft;
			set
			{
				if                      (value == null) return;

				selItemLeft             = value;
				OnPropertyChanged();

				updateLeft(selItemLeft);

				OnPropertyChanged(nameof(SelItemLeftName));
			}
		}

		public int                      SelIdxLeft
		{
			get =>                      selIdxLeft;
			set
			{
				selIdxLeft              = value;
				OnPropertyChanged();
			}
		}

		public string                   SelItemRightName => SelItemRight?.Name ?? "Undefined";

		public UnitsDataR               SelItemRight
		{
			get =>                      selItemRight;
			set
			{
				if                      (value == null) return;

				selItemRight            = value;
				OnPropertyChanged();

				updateRight(selItemRight);

				OnPropertyChanged(nameof(SelItemRightName));
			}
		}

		public int                      SelIdxRight
		{
			get =>                      selIdxRight;
			set
			{
				selIdxRight             = value;
				OnPropertyChanged();
			}
		}

		public bool                     OneTimeView { get; set; }

		public bool ShowMiniMainEnable
		{
			get => showMiniMainEnable;
			set
			{
				if (value == showMiniMainEnable) return;

				showMiniMainEnable = value;

				OnPropertyChanged();
			}
		}

		// only use mini results window - invert of showmain
		public bool? CkBxOnlyUseMini
		{
			get => UserSettings.Data.OnlyUseMini;
			set
			{
				MainWindowCommand.UpdateOnlyUseMini( value.HasValue ? value.Value : true);

				OnPropertyChanged();

				MainWindowCommand.Dlg_OnlyUseMini(value.Value);

				ShowMiniMainEnable = !UserSettings.Data.OnlyUseMini;
			}
		}

		// show mini results window
		public bool CkBxShowMiniWin
		{
			get => UserSettings.Data.ShowMiniWin;
			set
			{
				// Debug.WriteLine($"main| CkBxShowMiniWin| {value}");

				MainWindowCommand.UpdateShowMini(value);

				OnPropertyChanged();

				MainWindowCommand.Dlg_ShowMini(value);
			}
		}

	#endregion

	#region private properties

		// private UIDocument _uiDoc => uiDoc;
		// private Document _doc => uiDoc.Document;
		private MainWindow Main => mainWin;

		private WindowLocation _Location { get; set; } = new WindowLocation(0, 0);

	#endregion

	#region public methods

		public void init()
		{
			// M.W.WriteLine1("\ninit-begin");
			UnitsManager.Doc = R.Doc;

			UserSettings.Admin.Read();

			string left = UserSettings.Data.DialogLeftSelItemName;
			string right = UserSettings.Data.DialogRightSelItemName;

			createStyleViews();
			getSavedSelected(left, right);

			updateProperties();

			// Debug.WriteLine("mw B");
		}

		public void updateProperties()
		{
			OnPropertyChanged(nameof(PrimeArea));
			OnPropertyChanged(nameof(SecondArea));
			OnPropertyChanged(nameof(DistancesPrime));
			OnPropertyChanged(nameof(DistancesSecond));
			OnPropertyChanged(nameof(DistancesSubTitle));
		}

		public bool ShowHideWorkplane()
		{
			View av = R.Doc.ActiveView;
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

			// Debug.WriteLine("mw A");
		}

		public void UpdateCkBxHideMain(bool value)
		{
			MainWindowCommand.UpdateOnlyUseMini(value);
			OnPropertyChanged(nameof(CkBxOnlyUseMini));
		}

		public void UpdateCkBxShowMiniWin(bool value)
		{
			MainWindowCommand.UpdateShowMini(value);
			OnPropertyChanged(nameof(CkBxOnlyUseMini));
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

		public void ShutDown()
		{
			this.Closing -= MainWindow_OnClosing;
			this.IsVisibleChanged -= MainWindow_OnIsVisibleChanged;
			this.Loaded -= MainWindow_OnLoaded;
			this.PreviewKeyDown -= MainWindow_OnPreviewKeyUp;
			this.KeyDown -= MainWin_ProcessF1;
			this.KeyUp -= MainWin_ProcessF1;
			this.MouseEnter -= MainWindow_OnMouseEnter;
			this.MouseLeave -= MainWindow_OnMouseLeave;
			this.Activated -= MainWindow_OnActivated;
		}

	#endregion

	#region private methods

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

			R.Mm.Prime = distancesPrime;

			UserSettings.Data.DialogLeftSelItemName = udr.Name;
			UserSettings.Admin.Write();

			hasChanges = true;
		}

		private void updateRight(UnitsDataR udr)
		{
			distancesSecond.UnitStyle = udr;
			OnPropertyChanged(nameof(DistancesSecond));

			R.Mm.Second = distancesSecond;

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
				R.Mm.Visibility = Visibility.Hidden;
				return;
			}

			// MainWindowCommand.mm.Prime = distancesPrime;
			// MainWindowCommand.mm.Second = distancesSecond;

			R.Mm.Visibility = Visibility.Visible;
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
				using (Transaction t = new Transaction(R.Doc, "measure points"))
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

		private void showHelp()
		{
			uMgr.UlMgr.Rio.SB.GetContextualHelp().Launch();
		}

	#endregion

	#region event consuming

		// window

		private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
		{
			SetPosition(this.Owner);

			R.ActivateRevit();

			// Debug.WriteLine("mw A loaded");
		}

		private void MainWindow_OnActivated(object sender, EventArgs e)
		{
			if (!gotMouse)
			{
				Owner.Activate();
			}
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
		}

		private void MainWin_ProcessF1(object sender, KeyEventArgs e)
		{
			e.Handled = true;
			showHelp();
		}

		private void MainWindow_OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			MainWindowCommand.Dlg_WinVisibilityChanged(IsVisible);

			// Debug.WriteLine($"mw visibility changed| {e.NewValue}");
		}

		private void MainWindow_OnMouseEnter(object sender, MouseEventArgs e)
		{
			// Debug.WriteLine("mw mouse enter");

			gotMouse = true;
		}

		private void MainWindow_OnMouseLeave(object sender, MouseEventArgs e)
		{
			// Debug.WriteLine("mw mouse leave");

			gotMouse = false;
		}

		private void MainWindow_OnPreviewKeyUp(object sender, KeyEventArgs e)
		{
			e.Handled = true;
			base.OnPreviewKeyDown(e);
		}

		// controls

		private void Btn_SelPoints_OnClick(object sender, RoutedEventArgs e)
		{
			R.Measure();
		}

		private void BtnDone_OnClick(object sender, RoutedEventArgs e)
		{
			UserSettings.Data.WinPosDeluxMeasure = new WindowLocation(this.Top, this.Left);
			UserSettings.Admin.Write();

			this.Hide();
			Owner.Activate();
			ApiCalls.SendKeyCode(0x01);
		}

		private void BtnHelp_OnClick(object sender, RoutedEventArgs e)
		{
			showHelp();
		}

		private void BtnAbout_OnClick(object sender, RoutedEventArgs e)
		{
			About a = new About(this);
			a.ShowDialog();
		}

		// debug

		private void Btn_Debug_OnClick(object sender, RoutedEventArgs e)
		{
			UserSettingDataFile d = UserSettings.Data;


			string p = distancesPrime.UnitStyle.Name ;
			string s = distancesSecond.UnitStyle.Name;


			int a = 1;
		}


		// on property changed

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

		// private void MainWindow_OnGotFocus(object sender, RoutedEventArgs e)
		// {
		// 	Debug.WriteLine("mw got focus");
		// }
		//
		// private void MainWindow_OnPreviewGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
		// {
		// 	Debug.WriteLine("mw got preview keyboard focus");
		// }
		//
		// private void MainWindow_OnGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
		// {
		// 	Debug.WriteLine("mw got keyboard focus");
		//
		// 	// if (Owner.IsActive) return;
		// 	//
		// 	// Owner.Activate();
		// }

		// private void BtnDone_PreviewMouseDown(object sender, MouseButtonEventArgs e)
		// {
		// 	Debug.WriteLine("mw - button - got preview mouse down");
		// }

		// private void WinMain_PreviewKeyDown(object sender, KeyEventArgs e)
		// {
		// 	Owner.Activate();
		//
		// 	e.Handled = false;
		// }
		//
		// private void WinMain_ContentRendered(object sender, EventArgs e)
		// {
		// 	Debug.WriteLine("mw rendered");
		// }
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
}