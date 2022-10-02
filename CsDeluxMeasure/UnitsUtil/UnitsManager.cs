#region using

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using System.Windows.Media.Effects;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
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

in-lists needs
* 1. used to determine ribbon button descriptions / icon / button name / tooltip
* 2. need to update ribbon button due to changes
* 3. units manager holds the data but unit style manager directs usage and updates
* 4. thinking - 
* 5.  "current" list is the in-lists when dialog opens
* 6.  "working" that has the changes due to edits from unit style manager and style order manager
* 7. upon close of unit style manager, updates occur
* 8. "working" in-list created upon open order dialog - null otherwise
* 9. order dlg: reset means re-create lists from the live data
* 10. order dlg: cancel means, do not process the changes & set in-lists to null
* 11. order dlg: apply means
*		a. update live data with order changes
*		b. re-create "current" based on "working"
*		c. "working" to null
*		d. update ribbon button
* 12. need "current" to be an array list based on the determined order and only the items selected
* 13. "working" can be a sorted dictionary
* 14. "current" list should be just a list of UDR's
* 15. "working" list needs to be a sort index + the udr
* 16. sequence user applies for this sequence
*		start -> units manager reads data -> main dlg open -> main dlg adds project units -> create "current" in-lists
*			-> button to change order -> create "working" lists -> order dlg opens
*			-> user changes order -> dictionary key adjusted -> user applies -> flag "apply" with return value
*			-> at main dlg: -> update live data -> update ribbon button
*
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
		private List<UnitsDataR> usrStyleListBackup;

		private ListCollectionView wkgUserStyles;

		// private ListCollectionView[] inListViews;
		// private List<UnitsDataR>[] inLists;

		// private SortedDictionary<int, UnitsDataR>[] inListSortedDic;

		// private int[] InListMaxIdx;
		//
		// private UnitsDataR projUdr;

		private UnitsInListMgr ulMgr;

	#endregion

	#region ctor

		public UnitsManager()
		{
			// Debug.WriteLine("got unitsmgr ctor");


		#if PATH
			MethodBase mb = new StackTrace().GetFrame(1).GetMethod();
			Debug.WriteLine($"@UnitsManager: ctor: {(mb.ReflectedType?.FullName ?? "is null")} > {mb.Name}");
		#endif

			uSup = new UnitsSupport();
			uStg = new UnitsSettings();
			ulMgr = new UnitsInListMgr();

			// UStyle.ShowInChanged += OnShowInChanged;
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

		public ListCollectionView WkgUserStylesView => wkgUserStyles;

		public UnitsInListMgr UlMgr => ulMgr;

		public static Document Doc { get; set ; }

		public UnitsDataR ProjectUnitStyle => uSup.GetProjectUnitData(Doc);

		public int ProjectStyleIdx { get; set; }

	#endregion

	#region private properties

	#endregion

	#region public methods

		public void Config()
		{
			ReadUnitSettings();

			// initUserStylesView();
			// BackupUserStyleList();
		}

		public void initUserStylesView()
		{
			SetInitialSequence();

			wkgUserStyles = (ListCollectionView) CollectionViewSource.GetDefaultView(UsrStyleList);

			wkgUserStyles.SortDescriptions.Add(
				new SortDescription("Sequence", ListSortDirection.Ascending));
			wkgUserStyles.Filter = isNotDeleted;

			wkgUserStyles.IsLiveSorting = true;

			OnPropertyChanged(nameof(WkgUserStylesView));
		}

		// main list processing

		public void ReadUnitSettings()
		{
		#if PATH
			MethodBase mb = new StackTrace().GetFrame(1).GetMethod();
			Debug.WriteLine($"@UnitsManager: ReadUnitSettings: {(mb.ReflectedType?.FullName ?? "is null")} > {mb.Name}");
		#endif

			uStg.ReadStyles();
		}

		public void WriteUserStyles()
		{
			uSup.RemoveDeleted(UsrStyleList);

			UserSettings.Admin.Write();
		}

		public void BackupUserStyleList()
		{
			usrStyleListBackup = UnitsSupport.UnitsDataRListClone(UsrStyleList);
		}

		public void ResetUserStyleList()
		{
			UserSettings.Data.UserStyles = usrStyleListBackup;
			usrStyleListBackup = null;
		}

		public void ResetUserStylesToDefault()
		{
			UnitsSettings.SetUserStyles();
			usrStyleListBackup = null;
		}

		public void SetInitialSequence()
		{
			uSup.SetInitialSequence(UsrStyleList);
		}

		public void ReSequenceStylesList(ListCollectionView styles, int start, bool increase)
		{
			if (start == styles.Count) return;

			uSup.ReSequence(styles, start, increase);
		}

		public bool SetUnit( /*Document doc, */ UnitsDataR style)
		{
			Units units = UnitsSupport.makeStdLengthUnit(style);

			if (units == null) return false;

			if (uSup.setUnit(Doc, units)) return true;

			return false;
		}

		public UnitsDataR NewUDR(UnitsDataR orig, string name, string desc, int seq)
		{
			UnitsDataR udr = uSup.UDRClone(orig, name, desc, seq);
			udr.Ustyle.UnitClass = UnitClass.CL_ORDINARY;

			return udr;
		}

		public bool HasNameUserList(string name)
		{
			foreach (UnitsDataR udr in UsrStyleList)
			{
				if (udr.Ustyle.Name.Equals(name)) return true;
			}

			return false;
		}

		private bool isNotDeleted(object obj)
		{
			UnitsDataR udr = obj as UnitsUtil.UnitsDataR;

			return !udr.DeleteStyle;
		}

		public void ResetInitialSequence()
		{
			uSup.ResetInitialSequence(UsrStyleList);
		}

		public void UnDelete()
		{
			uSup.UnDelete(UsrStyleList);
		}

		public void UpdateProjectStyleSettings()
		{
			for (var i = 0; i < UsrStyleList.Count; i++)
			{
				if (UsrStyleList[i].Ustyle.UnitClass != UnitClass.CL_PROJECT) continue;

				UsrStyleList[i] = ProjectUnitStyle;
				ProjectStyleIdx = i;

				// there can be only one
				break;
			}
		}

		public bool DoesPropertyMatch(UStyle current, string propName)
		{
			UnitsDataR backup = usrStyleListBackup.Find(r => r.Ustyle.Name.Equals(current.Name));

			if (backup == null) return false;

			return backup.GetType().GetMember(propName).GetValue(0).Equals(
				current.GetType().GetMember(propName).GetValue(0));
		}


		// in-list processing

		public void ConfigWorkingInLists()
		{
			ulMgr.WkgUserStylesView = wkgUserStyles;
			ulMgr.ConfigWorkingInLists();
		}

		public void ConfigCurrentInList()
		{
			if (UsrStyleList == null) throw new NullReferenceException();

			ulMgr.UsrStyleList = UsrStyleList;
			ulMgr.ConfigCurrentInLists();
		}

		public void ApplyStyleOrderChange(InList which)
		{
			// 1. take the changes in the working inlist[] and apply to UserStyles
			// 2. notify that user styles has been updated
			// 3. update current inlist / notify changed
			// 4. (reserved)
			// 5. update working inlist / notify changed
			// 6. (reserved)
			// 7. update style order status values


			// 1.
			ulMgr.ApplyChanges(which, UsrStyleList);

			OnPropertyChanged(nameof(UsrStyleList));
			OnPropertyChanged(nameof(WkgUserStylesView));

			// 2. 
			// OnPropertyChanged(nameof(UsrStyleList));
		}

		public void ApplyStyleOrderChanges()
		{
			foreach (InList which in Enum.GetValues(typeof(InList)))
			{
				ulMgr.ApplyChanges(which, UsrStyleList);
			}

			OnPropertyChanged(nameof(UsrStyleList));
			OnPropertyChanged(nameof(WkgUserStylesView));
		}

		public int GetMaxInListIdx(InList list)
		{
			return uSup.GetMaxInListIdx(UsrStyleList, list);
		}

		// tests 

	#endregion

	#region private methods

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