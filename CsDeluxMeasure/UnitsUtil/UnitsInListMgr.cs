#region using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using CsDeluxMeasure.RevitSupport;

#endregion

// username: jeffs
// created:  9/2/2022 8:55:19 PM

namespace CsDeluxMeasure.UnitsUtil
{
	public class UnitsInListMgr : INotifyPropertyChanged
	{
		public const int MIN_INLIST_VALUE_RIBBON = 101;
		public const int MIN_INLIST_VALUE_DIALOG = 101;
		public const int FIXED_INLIST_VALUE_DIALOG = 100;


	#region private fields

		private UnitsInListsCurrent current;
		private UnitsInListsWorking working;

		// private UnitsSupport uSup;

		// private List<PushButton> pbList;
		//
		// private SplitButton sb;

		private RevitInterOp rio = new RevitInterOp();

	#endregion

	#region ctor

		public UnitsInListMgr()
		{
			// uSup = new UnitsSupport();
			//
			// pbList = new List<PushButton>(5);

			resetCurrent();
			resetWorking();
		}

	#endregion

	#region public properties

		public UnitsInListsCurrent Current => current;
		public UnitsInListsWorking Working => working;

		public ListCollectionView WkgUserStylesView
		{
			get => working.WkgUserStylesView;
			set => working.WkgUserStylesView = value;
		}
		public List<UnitsDataR> UsrStyleList { get; set; }

		// public SplitButton SB {
		// 	get => sb;
		// 	set => sb = value;
		// }

		public RevitInterOp Rio => rio;

		
	#endregion

	#region private properties

	#endregion

	#region public methods

		public void ConfigWorkingInLists()
		{
			working.ConfigInLists();
		}

		public void ConfigCurrentInLists()
		{
			// called from units manager
			current.ConfigInLists(UsrStyleList);
		}

		// public void AddPb(PushButton pb)
		// {
		// 	pbList.Add(pb);
		// }
		//
		// public void SetPbTitle(int idx, string title)
		// {
		// 	// pbdList[idx].Name = title;
		// 	pbList[idx].ItemText = title;
		// }

		public void SortWorkingList(InList which)
		{
			working.SortList(which);
		}

		public void MoveByProposedOrder(InList list, int idx, int newProposed)
		{
			working.MoveByProposedOrder(list, idx, newProposed);
		}

		public void SwapProposedOrderUp(InList list, int selIdx)
		{
			working.SwapProposedOrderUp(list, selIdx);
		}

		public void SwapProposedOrderDn(InList list, int selIdx)
		{
			working.SwapProposedOrderDn(list, selIdx);
		}

		public void ResetListAll()
		{
			working.ResetAllInList();
		}

		public void ResetList(InList list)
		{
			working.ResetInList(list);
		}

		public void ApplyChanges(InList list, List<UnitsDataR> UsrStyleList)
		{
			// apply the changes in the working list to the master list
			working.ApplyChanges(list, UsrStyleList);
			current.InListViews[(int) list].Refresh();
			;

			// update the current list
			// current.ConfigInListsView(which, UsrStyleList);

			// re-update the working list

		}

		public void ApplyRibbonButtonChangesToCurrent()
		{

			rio.UpdateRibbonButton(Current.InListViewRibbon);

			// ListCollectionView c = Current.InListViewRibbon;
			//
			// UnitsDataR udr;
			//
			// for (var i = 0; i < c.Count; i++)
			// {
			// 	udr = c.GetItemAt(i) as UnitsDataR;
			//
			// 	rio.SetPbTitle(i, udr.Name);
			// }
		}

	#endregion

	#region private methods

		private void resetCurrent()
		{
			current = null;
			current = new UnitsInListsCurrent();
		}

		private void resetWorking()
		{
			working = null;
			working = new UnitsInListsWorking();
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
			return "this is UnitsInListMgr";
		}

	#endregion
	}
}