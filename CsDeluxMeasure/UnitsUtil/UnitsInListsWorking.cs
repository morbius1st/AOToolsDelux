#region using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Documents;

#endregion

// username: jeffs
// created:  9/2/2022 8:44:47 PM


// this contains the working versions of the in-lists
// these are the versions actually modified by the order dialog


namespace CsDeluxMeasure.UnitsUtil
{

	public class WkgInListItem : INotifyPropertyChanged
	{
		#region WkgInListItem

		private bool isModified = false;
		private int proposedOrder;

		public int ProposedOrder
		{
			get => proposedOrder;
			set
			{
				proposedOrder = value;
				OnPropertyChanged();
				OnPropertyChanged(nameof(IsModified));
			}
		}

		public int CurrentOrder { get; set; }

		public bool IsModified => ProposedOrder != CurrentOrder;

		public UnitsDataR Data { get; set; }

		public WkgInListItem(int proposedOrder, int currentOrder, UnitsDataR data)
		{
			CurrentOrder = currentOrder;
			ProposedOrder = proposedOrder;
			Data = data;
		}

		public override string ToString()
		{
			return $"WkgList| name| {Data.Name}| ProposedOrder| {ProposedOrder}| CurrentOrdeer| {CurrentOrder}";
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

		#endregion
	}

	public class InListOrderComparer
	{
		#region InListOrderComparer

		public static int Compare(WkgInListItem x, WkgInListItem y)
		{
			if (x.ProposedOrder > y.ProposedOrder) { return 1; }

			if (x.ProposedOrder < y.ProposedOrder) { return -1; }

			return 0;
		}

		#endregion
	}


	public class UnitsInListsWorking : INotifyPropertyChanged
	{
		private readonly string[] IN_LISTS_NAMES = new []
		{
			nameof(InListsRibbon),
			nameof(InListsDlgLeft),
			nameof(InListsDlgRight),
		};

	#region private fields

		private List<WkgInListItem>[] inLists;

		private bool[] hasChanges;

		private int[] changeCount;

		private ListCollectionView wkgUserStylesView;

	#endregion

	#region ctor

		public UnitsInListsWorking()
		{

			hasChanges = new bool[UnitData.INLIST_COUNT];

			changeCount = new int[UnitData.INLIST_COUNT];

			inLists = new List<WkgInListItem>[UnitData.INLIST_COUNT];

			initInLists();
		}

	#endregion

	#region public properties

		public List<WkgInListItem>[] InLists => inLists;

		public List<WkgInListItem> InListsRibbon => inLists[(int) InList.RIBBON];
		public List<WkgInListItem> InListsDlgLeft => inLists[(int) InList.DIALOG_LEFT];
		public List<WkgInListItem> InListsDlgRight => inLists[(int) InList.DIALOG_RIGHT];


		public int[] ChangeCount => changeCount;


		public int ChgCntRibbon
		{
			get => changeCount[(int) InList.RIBBON];
			set
			{
				if (value < 0) return;

				changeCount[(int) InList.RIBBON] = value;
				OnPropertyChanged();
				OnPropertyChanged(nameof(ChangeCount));

				HasChgsRibbon = changeCount[(int) InList.RIBBON] > 0;
			}
		}

		public int ChgCntDlgLeft
		{
			get => changeCount[(int) InList.DIALOG_LEFT];
			set
			{
				if (value < 0) return;

				changeCount[(int) InList.DIALOG_LEFT] = value;
				OnPropertyChanged();
				OnPropertyChanged(nameof(ChangeCount));

				HasChgsDlgLeft = changeCount[(int) InList.DIALOG_LEFT] > 0;
			}
		}

		public int ChgCntDlgRight
		{
			get => changeCount[(int) InList.DIALOG_RIGHT];
			set
			{
				if (value < 0) return;

				changeCount[(int) InList.DIALOG_RIGHT] = value;
				OnPropertyChanged();
				OnPropertyChanged(nameof(ChangeCount));

				HasChgsDlgRight = changeCount[(int) InList.DIALOG_RIGHT] > 0;
			}
		}

		public bool NoChanges => !HasChanges;
		public bool HasChanges => HasChgsRibbon || HasChgsDlgLeft || HasChgsDlgRight;

		public bool[] HasChgs => hasChanges;

		public bool HasChgsRibbon
		{
			get => hasChanges[(int) InList.RIBBON];
			set
			{
				hasChanges[(int) InList.RIBBON] = value;
				OnPropertyChanged();
				OnPropertyChanged(nameof(HasChanges));
				OnPropertyChanged(nameof(NoChanges));
				OnPropertyChanged(nameof(HasChgs));
			}
		}

		public bool HasChgsDlgLeft
		{
			get => hasChanges[(int) InList.DIALOG_LEFT];
			set
			{
				hasChanges[(int) InList.DIALOG_LEFT] = value;
				OnPropertyChanged();
				OnPropertyChanged(nameof(HasChanges));
				OnPropertyChanged(nameof(NoChanges));
				OnPropertyChanged(nameof(HasChgs));
			}
		}

		public bool HasChgsDlgRight
		{
			get => hasChanges[(int) InList.DIALOG_RIGHT];
			set
			{
				hasChanges[(int) InList.DIALOG_RIGHT] = value;
				OnPropertyChanged();
				OnPropertyChanged(nameof(HasChanges));
				OnPropertyChanged(nameof(NoChanges));
				OnPropertyChanged(nameof(HasChgs));
			}
		}


		public ListCollectionView WkgUserStylesView
		{
			get => wkgUserStylesView;
			set => wkgUserStylesView = value;
		}

	#endregion

	#region private properties

	#endregion

	#region public methods

		public void ApplyChanges(InList list, List<UnitsDataR> usrStylesList)
		{
			UnitsDataR udr;
			string find;
			int which = (int) list;

			for (var i = 0; i < InLists[which].Count; i++)
			{
				find = InLists[which][i].Data.Ustyle.Name;
				udr = usrStylesList.Find(r => r.Ustyle.Name.Equals(find));

				if (udr != null)
				{
					udr.Ustyle.OrderInList[which] = InLists[which][i].ProposedOrder;
					InLists[which][i].CurrentOrder = InLists[which][i].ProposedOrder;
				}
			}

			OnPropertyChanged(IN_LISTS_NAMES[which]);
			hasChanges[which] = false;
			NotifyPropertyChanges(list);
		}

		public void ResetAllInList()
		{
			foreach (InList list in Enum.GetValues(typeof(InList)))
			{
				configtInList(list);
				changeCnt(list, 0);
			}
		}

		public void ResetInList(InList list)
		{
			configtInList(list);
			changeCnt(list, 0);
		}

		public void SortList(InList list)
		{
			int currList = (int) list;

			inLists[currList].Sort(delegate(WkgInListItem x, WkgInListItem y)
			{
				return InListOrderComparer.Compare(x, y);
			});
		}

		// up as in closer to front of list
		public void SwapProposedOrderUp(InList list, int idxSelElem)
		{
			if (idxSelElem <= 0) return;

			// DebugList("before", idxSelElem, idxSelElem-1);

			int i = (int) list;

			int currentOrder = InLists[i][idxSelElem - 1].ProposedOrder + 1;

			InLists[i][idxSelElem].ProposedOrder = InLists[i][idxSelElem - 1].ProposedOrder;

			InLists[i][idxSelElem - 1].ProposedOrder = currentOrder;

			changeCnt(list, countChanges(list));

			// DebugList("after", idxSelElem, idxSelElem-1);
		}

		// dn as in closer to the end of the list
		public void SwapProposedOrderDn(InList list, int idxSelElem)
		{
			int i = (int) list;

			// DebugList("before", idxSelElem, idxSelElem+1);

			if (idxSelElem == InLists[i].Count - 1) return;

			int currentOrder = InLists[i][idxSelElem].ProposedOrder + 1;

			InLists[i][idxSelElem + 1].ProposedOrder = InLists[i][idxSelElem].ProposedOrder;

			InLists[i][idxSelElem].ProposedOrder = currentOrder;

			changeCnt(list, countChanges(list));

			// DebugList("after", idxSelElem, idxSelElem+1);
		}

		public int GetIndexFromProposedOrder(InList list, int test)
		{
			return inLists[(int) list].FindIndex(x => x.ProposedOrder == test);
		}

		public void MoveByProposedOrder(InList list, int idxSelElem, int value)
		{
			if (!validateAgainstMinProposed(list, value)) return;

			int newIdx = GetIndexFromProposedOrder(list, value);

			if (newIdx >= 0)
			{
				MoveProposedOrder(list, newIdx, value);
			}

			InLists[(int) list][idxSelElem].ProposedOrder = value;

			SortList(list);
			changeCnt(list, countChanges(list));
			NotifyPropertyChanges(list);
		}
		
		public void ConfigInLists()
		{

			// called from in list manager (list was called from units manager)
			foreach (InList list in Enum.GetValues(typeof(InList)))
			{
				configtInList(list);

				OnPropertyChanged(IN_LISTS_NAMES[(int) list]);
			}
		}

		public void ConfigInList(InList list)
		{
			configtInList(list);

			OnPropertyChanged(IN_LISTS_NAMES[(int) list]);
		}


		public void DebugList(string title, int selItem, int swaped)
		{


			Debug.WriteLine($"{title}");
			Debug.WriteLine($"swapping {selItem} with {swaped}");

			DebugListItem("right dialog, selected", 2, selItem);
			DebugListItem("right dialog,  swapped", 2, swaped);
			// DebugListItem("right dialog", 2, 2);

			Debug.WriteLine($"right  has changes| {HasChgsDlgRight}");
			Debug.WriteLine($"right change count| {ChgCntDlgRight}");

		}

		public void DebugListItem(string name, int which, int item)
		{
			Debug.Write($"{name}, item| {item}|");
			Debug.Write($" pro| {InLists[which][item].ProposedOrder}");
			Debug.Write($" cur| {InLists[which][item].CurrentOrder}");
			Debug.Write($" mod| {InLists[which][item].IsModified}");
			Debug.Write("\n");

		}



	#endregion

	#region private methods

		private void initInLists()
		{
			foreach (InList inListEnum in Enum.GetValues(typeof(InList)))
			{
				inLists[(int) inListEnum] = new List<WkgInListItem>(5);
			}
		}

		private void configtInList(InList which)
		{
			if (WkgUserStylesView == null || WkgUserStylesView.Count == 0) return;

			int currList = (int) which;

			inLists[currList].Clear();

			foreach (UnitsDataR udr in WkgUserStylesView)
			{
				if (udr.DeleteStyle || !udr.Ustyle.ShowIn(currList)) continue;

				inLists[currList].Add(new WkgInListItem(
					udr.Ustyle.OrderInList[currList],
					udr.Ustyle.OrderInList[currList],
					udr));
			}

			SortList(which);
		}

		private bool validateAgainstMinProposed(InList list, int value)
		{
			return list == (int) InList.RIBBON ? value >= UnitsInListMgr.MIN_INLIST_VALUE_RIBBON : value >= UnitsInListMgr.MIN_INLIST_VALUE_DIALOG;
		}

		private void changeCnt(InList list, int chgAmt)
		{
			switch (list)
			{
			case InList.RIBBON:
				{
					ChgCntRibbon = chgAmt;
					break;
				}
			case InList.DIALOG_LEFT:
				{
					ChgCntDlgLeft = chgAmt;
					break;
				}
			case InList.DIALOG_RIGHT:
				{
					ChgCntDlgRight = chgAmt;
					break;
				}
			}
		}

		private void MoveProposedOrder(InList list, int idxSelElem, int value)
		{
			int which = (int) list;
			// list is list array item to process
			// idxSelElem is the index for the current item to check
			// value is the new proposed
			// if the current proposed == value, Move the next item
			// else found the end and begin processing the changes

			if (idxSelElem != InLists[which].Count - 1 &&
				InLists[which][idxSelElem].ProposedOrder == value)
			{
				MoveProposedOrder(list, idxSelElem + 1, value + 1);
			}

			InLists[which][idxSelElem].ProposedOrder = value + 1;

			changeCnt(list, countChanges(list));
		}

		private void NotifyPropertyChanges(InList list)
		{
			OnPropertyChanged(nameof(ChangeCount));
			OnPropertyChanged(nameof(HasChanges));
			OnPropertyChanged(nameof(NoChanges));

			switch (list)
			{
			case InList.RIBBON:
				{
					OnPropertyChanged(nameof(ChgCntRibbon));
					OnPropertyChanged(nameof(HasChgsRibbon));
					break;
				}
			case InList.DIALOG_LEFT:
				{
					OnPropertyChanged(nameof(ChgCntDlgLeft));
					OnPropertyChanged(nameof(HasChgsDlgLeft));
					break;
				}
			case InList.DIALOG_RIGHT:
				{
					OnPropertyChanged(nameof(ChgCntDlgRight));
					OnPropertyChanged(nameof(HasChgsDlgRight));
					break;
				}
			}
		}

		private int countChanges(InList list)
		{
			int count = 0;

			foreach (WkgInListItem item in InLists[(int) list])
			{
				count += item.IsModified ? 1 : 0;
			}

			return count;
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
			return 
				$"this is UnitsInListsWorking| "
				+ $"counts| ribbon| {InListsRibbon.Count} | "
				+ $"dlg left| {InListsDlgLeft.Count} | dlg right| {InListsDlgRight.Count}";
		}

	#endregion
	}
}