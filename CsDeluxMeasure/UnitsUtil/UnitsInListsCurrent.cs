#region using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Autodesk.Revit.UI;

#endregion

// username: jeffs
// created:  9/2/2022 8:24:50 PM

// these are the lists accessed by the measure dialog and by the ribbon command

namespace CsDeluxMeasure.UnitsUtil
{
	public class UnitsInListsCurrent : INotifyPropertyChanged
	{
		private readonly string[] IN_LISTS_NAMES = new []
		{
			nameof(InListViewRibbon),
			nameof(InListViewDlgLeft),
			nameof(InListViewDlgRight),
		};

	#region private fields

		private ListCollectionView[] inListViews;

	#endregion

	#region ctor

		public UnitsInListsCurrent()
		{
			inListViews = new ListCollectionView[UnitData.INLIST_COUNT];
		}

	#endregion

	#region public properties

		public ListCollectionView[] InListViews => inListViews;
		
		// these are the current / saved name lists
		public ListCollectionView InListViewRibbon => inListViews[(int) InList.RIBBON];
		public ListCollectionView InListViewDlgLeft => inListViews[(int) InList.DIALOG_LEFT];
		public ListCollectionView InListViewDlgRight => inListViews[(int) InList.DIALOG_RIGHT];

	#endregion

	#region private properties

	#endregion

	#region public methods


		public void ConfigInLists(List<UnitsDataR> UsrStyleList)
		{
			// makeTestLists();

			foreach (InList which in Enum.GetValues(typeof(InList)))
			{
				ConfigInListsView(which, UsrStyleList);

			}
		}

		public void ConfigInListsView(InList which, List<UnitsDataR> UsrStyleList)
		{
			configInListsViews(which, UsrStyleList);

			OnPropertyChanged(IN_LISTS_NAMES[(int) which]);
		}

	#endregion

	#region private methods

		private void configInListsViews(InList which, List<UnitsDataR> UsrStyleList)
		{
			if (UsrStyleList == null || UsrStyleList.Count == 0) return;
		
			int currList = (int) which;
		
			// liev views
		
			inListViews[currList] = new ListCollectionView(UsrStyleList);
		
			inListViews[currList].SortDescriptions.Clear();
			inListViews[currList].SortDescriptions.Add(
				new SortDescription(UStyle.INLIST_PROP_NAMES[currList], ListSortDirection.Ascending));
		
			inListViews[currList].Filter = o =>
			{
				return o is UnitsDataR udr && udr.Ustyle.ShowIn(currList) && !udr.DeleteStyle;
			};
		
		
		}

	#endregion

	#region event consuming

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
			return $"this is UnitsInListsCurrent| counts| ribbon| {InListViewRibbon.Count} | "
				+ $"dlg left| {InListViewDlgLeft.Count} | dlg right| {InListViewDlgRight.Count}";
		}

	#endregion
	}
}