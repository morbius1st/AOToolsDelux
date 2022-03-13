using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using DeluxMeasure.UnitsUtil;
using SettingsManager;

namespace DeluxMeasure.Windows
{
	/// <summary>
	/// Interaction logic for UnitStylesManager.xaml
	/// </summary>
	public partial class UnitStylesManager : Window, INotifyPropertyChanged
	{
	#region private fields

		private readonly string[] contentSelection = new [] { "Adjust Style Order", "Adjust Saved Styles" };

		private  int tempSelect;

		private string status;
		private ListCollectionView styles;

		private ListCollectionView ribbonStyles;
		private ListCollectionView dialogLeftStyles;
		private ListCollectionView dialogRightStyles;

		private bool initStatus = false;

	#endregion

	#region ctor
		public UnitStylesManager()
		{
			InitializeComponent();

			Init();

			status = "init";

			TempSel = 0;

		}

	#endregion

	#region public properties

		public ListCollectionView StylesView => styles;

		public int TempSel
		{
			get
			{
				return tempSelect;
			}
			set
			{
				tempSelect = value;
				OnPropertyChanged();
				OnPropertyChanged(nameof(ContentType));
			}
		}

		public string ContentType => contentSelection[tempSelect];


	#endregion

	#region private properties

		private List<UnitStyle> Styles => UnitsManager.Instance.StyleList;

	#endregion

	#region public methods

		public void Init()
		{
			initStatus = true;

			styles = (ListCollectionView) CollectionViewSource.GetDefaultView(Styles);
			styles.CurrentChanged += Styles_CurrentChanged;

			OnPropertyChanged("Styles");
			OnPropertyChanged("StylesView");

			initStatus = false;
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
			bool resultR = setStyleList(ribbonStyles, (int) UnitStyle.ListToShowIn.RIBBON);
			bool resultDl = setStyleList(dialogLeftStyles, (int) UnitStyle.ListToShowIn.DIALOG_LEFT);
			bool resultDr = setStyleList(dialogRightStyles, (int) UnitStyle.ListToShowIn.DIALOG_RIGHT);

		}



	#endregion

	#region private methods

		private bool setStyleList(ListCollectionView view, int thisList)
		{
			view = (ListCollectionView) CollectionViewSource.GetDefaultView(Styles);

			view.Filter = item =>
			{
				if (item == null) return false;
				UnitStyle us = (UnitStyle) item;

				return us.ShowIn(thisList);
			};

			return view.Count > 0;
		}



		// private bool setRibbonStyles()
		// {
		// 	ribbonStyles = (ListCollectionView) CollectionViewSource.GetDefaultView(Styles);
		// 	ribbonStyles.Filter = item =>
		// 	{
		// 		if (item == null) return false;
		// 		UnitStyle us = (UnitStyle) item;
		//
		// 		return us.ShowInRibbon;
		// 	};
		//
		// 	return ribbonStyles.Count > 0;
		// }

		// private bool setDialogLeftStyles()
		// {
		// 	dialogLeftStyles = (ListCollectionView) CollectionViewSource.GetDefaultView(Styles);
		// 	dialogLeftStyles.Filter = item =>
		// 	{
		// 		if (item == null) return false;
		// 		UnitStyle us = (UnitStyle) item;
		//
		// 		return us.ShowInDialogLeft;
		// 	};
		//
		// 	return dialogLeftStyles.Count > 0;
		// }
		//
		// private bool setDialogRghtStyles()
		// {
		// 	dialogRightStyles = (ListCollectionView) CollectionViewSource.GetDefaultView(Styles);
		// 	dialogRightStyles.Filter = item =>
		// 	{
		// 		if (item == null) return false;
		// 		UnitStyle us = (UnitStyle) item;
		// 		return us.ShowInDialogLeft;
		// 	};
		//
		// 	return dialogRightStyles.Count > 0;
		// }

	#endregion

	#region event consuming

		private void Styles_CurrentChanged(object sender, EventArgs e)
		{
			if (initStatus) return;

			status = "current changed";

			bool y = styles.IsEmpty;
		}

		private void WinUnitStyle_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			UserSettings.Data.WinPosUnitStyleMgr = new WindowLocation(this.Top, this.Left);
			UserSettings.Admin.Write();
		}

		private void BtnDone_OnClick(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		private void BtnChgTemplate_OnClick(object sender, RoutedEventArgs e)
		{
			TempSel = (tempSelect + 1) % 2;

			ProcessStyleChanges();

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
	}
}
