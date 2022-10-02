#region using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using CsDeluxMeasure.RevitSupport;
using CsDeluxMeasure.UnitsUtil;
using SettingsManager;
using UtilityLibrary;
using static CsDeluxMeasure.RevitSupport.RevitUtil;
using Visibility = System.Windows.Visibility;

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
	public partial class MainWindow : INotifyPropertyChanged
	{
	#region private fields

		private UnitsManager uMgr = UnitsManager.Instance;

		private UnitsInListsCurrent uCurr = new UnitsInListsCurrent();

		private Window revitWindow;

		private MainWindow mainWin;

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

	#endregion

	#region ctor

		public MainWindow(UIDocument uiDoc)
		{
			InitializeComponent();

			this.uiDoc = uiDoc;

			distancesPrime = new PointDistances();
			distancesSecond = new PointDistances();

			// BitmapImage b = new BitmapImage(new Uri("pack://application:,,,/CsDeluxMeasure;component/Resources/CyberStudio Icon.png"));
		}

	#endregion

	#region public properties

		public string MessageBoxText
		{
			get => textMsg01;
			set { textMsg01 = value; }
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

		public PointMeasurements? MeasuredPoints => points;

		public PointDistances DistancesPrime
		{
			get => distancesPrime;
			set
			{
				distancesPrime = value;
				OnPropertyChanged();
			}
		}

		public PointDistances DistancesSecond
		{
			get => distancesSecond;
			set
			{
				distancesSecond = value;
				OnPropertyChanged();
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

		public bool HasChanges
		{
			get => hasChanges;
			set => hasChanges = value;
		}

	#endregion

	#region private properties

		private UIDocument _uiDoc => uiDoc;
		private Document _doc => uiDoc.Document;

		private MainWindow Main => mainWin;

		private WindowLocation _Location { get; set; } = new WindowLocation(0, 0);

	#endregion

	#region public methods

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

		public bool ShowHideWorkplane()
		{
			View av = _doc.ActiveView;
			Plane p = av.SketchPlane?.GetPlane();

			showHideWorkplane(p, av);

			return true;
		}

		public void SetPosition(Window parentWindow)
		{
			revitWindow = parentWindow;

			double t = UserSettings.Data.WinPosDeluxMeasure.Top;
			double l = UserSettings.Data.WinPosDeluxMeasure.Left;

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
			UnitsManager.Doc = _doc;

			readSettings();

			string left = UserSettings.Data.DialogLeftSelItemName;
			string right = UserSettings.Data.DialogRightSelItemName;

			createStyleViews();
			getSavedSelected(left, right);
		}

		private void readSettings()
		{
			UserSettings.Admin.Read();
		}

		private void createStyleViews()
		{
			uMgr.UpdateProjectStyleSettings();

			uCurr.ConfigInListsView(InList.DIALOG_LEFT, UsrStylesList);
			uCurr.ConfigInListsView(InList.DIALOG_RIGHT, UsrStylesList);

			OnPropertyChanged(nameof(DialogLeftList));
			OnPropertyChanged(nameof(DialogRightList));
		}

		private void getSavedSelected2(string leftKey, string rightKey)
		{
			// must occur after the views are set
			distancesPrime.UnitStyle = getSavedSelStyle(DialogLeftList, leftKey);

			SelIdxLeft = DialogLeftList.IndexOf(distancesPrime.UnitStyle);

			distancesSecond.UnitStyle = getSavedSelStyle(DialogRightList, rightKey);

			SelIdxRight = DialogRightList.IndexOf(distancesSecond.UnitStyle);
		}

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
			int result;

			result = getSavedSelStyleIdx(DialogLeftList, leftKey);
			SelIdxLeft = result;

			result = getSavedSelStyleIdx(DialogRightList, rightKey);
			SelIdxRight = result;
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

			UserSettings.Data.DialogLeftSelItemName = udr.Name;

			hasChanges = true;
		}

		private void updateRight(UnitsDataR udr)
		{
			distancesSecond.UnitStyle = udr;
			OnPropertyChanged(nameof(DistancesSecond));

			UserSettings.Data.DialogRightSelItemName = udr.Name;

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


		// measure points


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

		private bool MeasurePoints(XYZ workingOrigin, RevitUtil.VType vtype, string planeName)
		{
			bool again = true;
			bool? result;

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

		private PointMeasurements GetPts(XYZ workingOrigin)
		{
			XYZ startPoint;
			XYZ endPoint;

			revitWindow.Focus();

			try
			{
				View avStart = _doc.ActiveView;

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
			}
			catch
			{
				return PointMeasurements.InValid();
			}

			return new PointMeasurements(startPoint, endPoint, workingOrigin);
		}

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
			_Location = UserSettings.Data.WinPosDeluxMeasure;

			if (_Location.Top >= 0 && _Location.Left >= 0)
			{
				this.Top = _Location.Top;
				this.Left = _Location.Left;
			}

			init();
		}

		private void MainWindow_OnClosing(object sender, CancelEventArgs e)
		{
			UserSettings.Data.WinPosDeluxMeasure = new WindowLocation(this.Top, this.Left);
			UserSettings.Admin.Write();

			// if (!this.DialogResult.HasValue) DialogResult = true;
		}

		private void MainWin_ProcessF1(object sender, KeyEventArgs e)
		{
			e.Handled = true;
		}

		// controls

		private void BtnDone_OnClick(object sender, RoutedEventArgs e)
		{
			// this.DialogResult = false;
			this.Close();
		}

		private void Btn_SelPoints_OnClick(object sender, RoutedEventArgs e)
		{
			// this.Visibility = Visibility.Collapsed;
			// DxMeasure();
			// this.Visibility = Visibility.Visible;

			DialogResult = true;
			this.Close();
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

		public void SetPointsToZero()
		{
			DistancesPrime.Points = PointMeasurements.Zero();
			DistancesSecond.Points = PointMeasurements.Zero();
		}

		public bool DxMeasure()
		{
			bool result;

			View av = _doc.ActiveView;
			RevitUtil.VType vtype = RevitUtil.GetViewType(av);

			dxPrepPlane(av, vtype);
			result = dxMeasurePoints(av, vtype);

			if (result) result = this.ShowDialog() ?? false;

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

			if (points.IsValid)
			{
				DistancesPrime.Points = points;
				DistancesSecond.Points = points;
				OnPropertyChanged(nameof(DistancesPrime));
				OnPropertyChanged(nameof(DistancesSecond));
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

			return true;
		}

	#endregion

		private void Btn_Debug_OnClick(object sender, RoutedEventArgs e)
		{
			UserSettingDataFile d = UserSettings.Data;


			string p = distancesPrime.UnitStyle.Name ;
			string s = distancesSecond.UnitStyle.Name;


			int a = 1;
		}

	}
}