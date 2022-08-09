#region using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using Autodesk.Revit.DB;
using CsDeluxMeasure.Annotations;
using static Autodesk.Revit.DB.FormatOptions;
using SettingsManager;

#endregion

// username: jeffs
// created:  2/26/2022 9:26:35 AM

/*

data collections

StdStyles  (AppStyles)		(Dictionary<ForgeTypeId, UnitsDataR>)  to / from app setting file
StyleList  (UserStyles)		(List<UnitsDataR>)  to / from user setting file



data map
** data
UnitsDataR		a standard styles that includes Revit specific information
				+ holds a pointer to a UnitUStyle
UnitsDataD		same as UnitsDataR except does not have any Revit specific information
				only intended for debugging
				+ holds a pointer to a UnitUStyle

UnitStdStylesR	collection of UnitsDataR (standard styles) and includes Revit specific information
UnitStdStylesD	same as UnitStdStylesR except does not have any Revit specific information
				only intended for debugging
	
UnitsManager.StyleList (working styles | specific to a user)
UnitsManager.StdStyles (working styles | specific to the app / starts out as the system dafault)

** base
UnitUStyle		base unit style | specifically does not include any Revit specific information (which allows this to be used as d:DataContext
UnitsStdUStyles	collection of standard styles - one per unit display system.

** support
UnitSupport		(parts static) data / settings for units | conversion routines

UnitSettings	(not static) I/O for writing / reading data to user or app setting file (front end for SettingsManager)

UnitsManager	(is static) manager if the information
				+ holds "UnitsUStyles"
				+ holds "UnitSupport"
				+ manages the working lists
					-> StyleList (access point to the data saved in the user's setting file)
					-> StdStyles (access point to the data saved in the app setting file)
				+ provides access to the current project unit style

** UI
UnitStylesManager	manager for the interface

** usage

allow user to create and organize unit styles
* organize
	+ determine which list a style appears
	+ determine the order in each list a style appears
* create
	+ make a new style from the current project unit settings
	+ allow to only change
		-> name
		-> description
		-> sample
	+ cannot change any other unit settings / done only from revit

*/


namespace CsDeluxMeasure.UnitsUtil
{
	public class UnitsManager : INotifyPropertyChanged
	{
	#region private fields

		// private Dictionary<ForgeTypeId, UnitsDataR> styleList = null;
		// private Dictionary<ForgeTypeId, UnitsDataR> stdStyles = null;

		private static readonly Lazy<UnitsManager> instance =
			new Lazy<UnitsManager>(() => new UnitsManager());

		private UnitsSettings uStg;
		private UnitsSupport uSup;

		// current / saved lists
		private ListCollectionView[] inListViews;
		private List<UnitsDataR> usrStyleListCopy;

		// working lists
		// private ListCollectionView wkgUserStyles;
		// private ListCollectionView[] wkgInListViews;

	#endregion

	#region ctor

		public UnitsManager()
		{
		#if PATH
			MethodBase mb = new StackTrace().GetFrame(1).GetMethod();
			Debug.WriteLine($"@UnitsManager: ctor: {(mb.ReflectedType?.FullName ?? "is null")} > {mb.Name}");
		#endif

			uSup = new UnitsSupport();
			uStg = new UnitsSettings();

			inListViews = new ListCollectionView[UnitData.INLIST_COUNT];
			// wkgInListViews = new ListCollectionView[UnitData.INLIST_COUNT];
		}

	#endregion

	#region public properties

		public static UnitsManager Instance
		{
			get
			{
			#if PATH
				MethodBase mb = new StackTrace().GetFrame(1).GetMethod();
				Debug.WriteLine($"@UnitsManager: Instance: {(mb.ReflectedType?.FullName ?? "is null")} > {mb.Name}");
			#endif

				return instance.Value;
			}
		}

		// these are the current / saved name lists
		public ListCollectionView InListViewRibbon => inListViews[(int) InList.RIBBON];
		public ListCollectionView InListViewDlgLeft => inListViews[(int) InList.DIALOG_LEFT];
		public ListCollectionView InListViewDlgRight => inListViews[(int) InList.DIALOG_RIGHT];

		// the current / saved list of user styles
		public List<UnitsDataR> UsrStyleList
		{
			get
			{
			#if PATH
				MethodBase mb = new StackTrace().GetFrame(1).GetMethod();
				Debug.WriteLine($"@UnitsManager: StyleList: {(mb.ReflectedType?.FullName ?? "is null")} > {mb.Name}");
			#endif

				return UserSettings.Data.UserStyles;
			}
			// set => SettingsManager.UserSettings.Data.UserStyles = value;
		}

		// list of standard unit styles
		public Dictionary<string, UnitsDataR> StdStyles
		{
			get => SettingsManager.AppSettings.Data.AppStyles;
			// set => SettingsManager.AppSettings.Data.AppStyles = value;
		}

		public static Document Doc { get; set ; }

		public UnitsDataR ProjectUnitStyle => uSup.GetProjectUnitData(Doc);


		// // working data
		// public ListCollectionView WkgUserStyles => wkgUserStyles;
		//
		// // these are the current / saved name lists
		// public ListCollectionView WkgInListViewsRibbon => wkgInListViews[(int) InList.RIBBON];
		// public ListCollectionView WkgInListViewsDlgLeft => wkgInListViews[(int) InList.DIALOG_LEFT];
		// public ListCollectionView WkgInListViewsDlgRight => wkgInListViews[(int) InList.DIALOG_RIGHT];


	#endregion

	#region private properties

	#endregion

	#region public methods

		public void Config()
		{
			ReadUnitSettings();
			UpdateNameLists();
			// ConfigUserStyleView();
		}

		public void BackupUserStyleList()
		{
			usrStyleListCopy = uSup.UnitsDataRListClone(UsrStyleList);
		}

		public void ResetUserStyleList()
		{
			UserSettings.Data.UserStyles = usrStyleListCopy;
			usrStyleListCopy = null;
		}

		private bool isNotDeleted(object obj)
		{
			UnitsDataR udr = obj as UnitsUtil.UnitsDataR;

			return !udr.DeleteStyle;
		}


		public void SetInitialSequence()
		{
			uSup.SetInitialSequence(UsrStyleList);
		}

		public void ResetInitialSequence()
		{
			uSup.ResetInitialSequence(UsrStyleList);
		}

		public void UnDelete()
		{
			uSup.UnDelete(UsrStyleList);
		}

		public void WriteUser()
		{
			uSup.RemoveDeleted(UsrStyleList);

			UserSettings.Admin.Write();
		}

		public void ReSequenceStylesList(ListCollectionView styles, int start, bool increase)
		{
			if (start == styles.Count - 1) return;

			uSup.ReSequence(styles, start, increase);
		}

		public void ReadUnitSettings()
		{
		#if PATH
			MethodBase mb = new StackTrace().GetFrame(1).GetMethod();
			Debug.WriteLine($"@UnitsManager: ReadUnitSettings: {(mb.ReflectedType?.FullName ?? "is null")} > {mb.Name}");
		#endif

			uStg.ReadStyles();
		}

		public bool SetUnit( /*Document doc, */ UnitsDataR style)
		{
			Units units = makeStdLengthUnit( style);

			if (units == null) return false;

			if (setUnit(Doc, units)) return true;

			return false;
		}

		public UnitsDataR NewUDR(UnitsDataR orig, string name, string desc, int seq)
		{
			UnitsDataR udr = uSup.UDRClone(orig, name, desc, seq);
			udr.Ustyle.UnitClass = UnitClass.CL_ORDINARY;
			
			return udr;
		}

		public string FormatLength(double value, UnitsDataR style)
		{
			Units units = makeStdLengthUnit(style);

			if (units == null) return "N/A";

			string result =  UnitFormatUtils.Format(units, SpecTypeId.Length, value, false);
			return result;
		}

		public bool HasNameUserList(string name)
		{
			foreach (UnitsDataR udr in UsrStyleList)
			{
				if (udr.Ustyle.Name.Equals(name)) return true;
			}

			return false;
		}

		// public string ValidateStyleName(string testName)
		// {
		// 	return uSup.CheckStyleNameSyntax(testName);
		// }
		//
		// public string ValidateStyleDesc(string testName)
		// {
		//
		// 	return uSup.CheckStyleDescSyntax(testName);
		// }

		
		public double ValidateStyleSample(string testValue)
		{
			double value;

			bool result = double.TryParse(testValue, out value);

			return result ? value : double.NaN;
		}

	#endregion

	#region private methods

		private List<string> initList(int qty)
		{
			List<string> list = new List<string>(qty);

			return list;
		}

		private bool setFmtOpt(bool? opt)
		{
			if (opt == null) return false;

			return opt.Value;
		}

		private bool setUnit(Document doc, Units unit)
		{
			try { doc.SetUnits(unit); }
			catch (Exception e)
			{
				return false;
			}

			return true;
		}

		private Units makeStdLengthUnit(UnitsDataR udr)
		{
			// if (udr.Ustyle.IsLocked == null) return null;

			Units units;
			FormatOptions fmtOpts;

			try
			{
				fmtOpts = GetFormatOptions(udr);
			}
			catch (Exception e)
			{
				return null;
			}

			units = new Units(udr.USystem);
			units.SetFormatOptions(SpecTypeId.Length, fmtOpts);

			return units;
		}

		private void UpdateNameLists()
		{
			foreach (InList inListEnum in Enum.GetValues(typeof(InList)))
			{
				InitInRibbonNameList(inListEnum);
			}
		}

		// create the ListCollectionViews of each dialog list
		private void InitInRibbonNameList(InList which)
		{
			int currList = (int) which;

			// liev views
			if (UsrStyleList == null || UsrStyleList.Count == 0) return;

			inListViews[currList] =new ListCollectionView(UsrStyleList);

			inListViews[currList].SortDescriptions.Clear();
			inListViews[currList].SortDescriptions.Add(
				new SortDescription(UStyle.INLIST_PROP_NAMES[currList], ListSortDirection.Ascending));
				// new SortDescription("Ustyle.OrderInRibbon", ListSortDirection.Ascending));

			inListViews[currList].Filter = o =>
			{
				return o is UnitsDataR udr && udr.Ustyle.ShowIn(currList);
			};

			inListViews[currList].IsLiveSorting = true;

			// // working views
			// wkgInListViews[currList] =new ListCollectionView(UsrStyleList);
			//
			// wkgInListViews[currList].SortDescriptions.Clear();
			// wkgInListViews[currList].SortDescriptions.Add(
			// 	new SortDescription(UStyle.INLIST_PROP_NAMES[currList], ListSortDirection.Ascending));
			// // new SortDescription("Ustyle.OrderInRibbon", ListSortDirection.Ascending));
			//
			// wkgInListViews[currList].Filter = o =>
			// {
			// 	return o is UnitsDataR udr && udr.Ustyle.ShowIn(currList);
			// };
			//
			// wkgInListViews[currList].IsLiveSorting = true;

		}
		//
		// private void ConfigUserStyleView()
		// {
		// 	SetInitialSequence();
		//
		// 	wkgUserStyles = (ListCollectionView) CollectionViewSource.GetDefaultView(UsrStyleList);
		//
		// 	wkgUserStyles.SortDescriptions.Add(
		// 		new SortDescription("Sequence", ListSortDirection.Ascending));
		// 	wkgUserStyles.Filter = isNotDeleted;
		//
		// 	wkgUserStyles.IsLiveSorting = true;
		//
		// 	OnPropertyChanged(nameof(WkgUserStyles));
		// }

		private FormatOptions GetFormatOptions(UnitsDataR style)
		{
			FormatOptions fmtOpts;
			UStyle us = style.Ustyle;

			try
			{
				fmtOpts = new FormatOptions(style.Id);
				fmtOpts.Accuracy = us.Precision;

				if (CanHaveSymbol(style.Id))
				{
					fmtOpts.SetSymbolTypeId(style.Symbol);
				}

				if (CanSuppressLeadingZeros(style.Id))
				{
					fmtOpts.SuppressLeadingZeros = setFmtOpt(us.SuppressLeadZeros);
				}

				if (CanSuppressTrailingZeros(style.Id))
				{
					fmtOpts.SuppressTrailingZeros = setFmtOpt(us.SuppressTrailZeros);
				}

				if (CanSuppressSpaces(style.Id))
				{
					fmtOpts.SuppressSpaces = setFmtOpt(us.SuppressSpaces);
				}

				if (CanUsePlusPrefix(style.Id))
				{
					fmtOpts.UsePlusPrefix = setFmtOpt(us.UsePlusPrefix);
				}
			}
			catch (Exception e)
			{
				return null;
			}

			return fmtOpts;
		}


		// private void populateDefaultStyleList()
		// {
		// 	styleList = new List<UnitStyle>();
		//
		// 	foreach (UnitStyle unitStyle in UnitStyles.StdStyles)
		// 	{
		// 		styleList.Add(unitStyle);
		// 	}
		// }

	#endregion

	#region event consuming

	#endregion

	#region event publishing

		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		[DebuggerStepThrough]
		private void OnPropertyChanged([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is UnitUtils";
		}

	#endregion
	}
}