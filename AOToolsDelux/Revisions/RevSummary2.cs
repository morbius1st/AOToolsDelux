using System.Collections;
using System.Collections.Generic;

using static AOToolsDelux.Revisions.RevSummary2.EListSubject;

namespace AOToolsDelux.Revisions
{
	// keep track of some of th columns to allow the 
	// user to select what they want
	public class RevSummary2 : IEnumerable<KeyValuePair<int,RevSummary2.ListData>>
	{
		// need to be a bit complex as we need to present only
		// true available choices - this means that most of the lists are blank
		// until an upper level choice is made
		private const string ANY = "any";

		// these are in order of precidence
		public enum EListSubject
		{
			LIST_SEQUENCE = 0,
			LIST_REVALTID = 1,
			LIST_SHTNUM = 2,
			LIST_DELTATITLE = 3,  // probably only one of titles will get used at a time
			LIST_BLOCKTITLE = 4,  // probably only one of titles will get used at a time
			LIST_BASIS = 5,
			LIST_DESC = 6,
			LIST_COUNT
		}

		public class ListData
		{
			public string Choice = "";
			public List<string> Summary = new List<string>(10);
		}

		private SortedList<int, ListData> _sumMastList = new SortedList<int, ListData>((int) LIST_COUNT);

		private SortedList<string, RevDataItems2> revInfo;

		public RevSummary2(SortedList<string, RevDataItems2> revisionInfo)
		{
			ListsCreate();

			Reset(revisionInfo);
		}

		// reset all data to the default values
		public void Reset(SortedList<string, RevDataItems2> revisionInfo)
		{
			if (revisionInfo != null) revInfo = revisionInfo;

			// indcate that any value is good
			InitLists(true, true);
			InitChoices(true, true);

			foreach (KeyValuePair<string, RevDataItems2> kvp in revInfo)
			{
				AddToLists(kvp.Value);
			}

			ListsSort();
		}

		

		#region + List Mgmt

		// initalize all lists to empty status - but save the choice data
		private void ListsCreate()
		{
			_sumMastList = new SortedList<int, ListData>((int) LIST_COUNT);

			// create the empty lists for the first time
			for (int i = 0; i < (int) LIST_COUNT; i++)
			{
				_sumMastList.Add(i, new ListData()
				{
					Choice = ANY,
					Summary = new List<string>(10)
				});
			}
		}

		private void InitChoices(bool master, bool selected)
		{
			for (int i = 0; i < (int) LIST_COUNT; i++)
			{
				if (master) _sumMastList[i].Choice = ANY;
//				if (selected) _summarySelectedLists[i].Choice = ANY;
			}
		}

		private void InitLists(bool master, bool selected)
		{
			for (int i = 0; i < (int) LIST_COUNT; i++)
			{
				if (master) _sumMastList[i].Summary = new List<string>(10);
//				if (selected) _summarySelectedLists[i].Summary = new List<string>(10);
			}
		}

		private void ListsSort()
		{
			for (int i = 0; i < (int) LIST_COUNT; i++)
			{
				_sumMastList[i].Summary.Sort();
			}
		}

		private void AddToLists(RevDataItems2 value)
		{
			AddToList(LIST_SEQUENCE, value.Sequence.ToString());
			AddToList(LIST_REVALTID, value.AltId);
			AddToList(LIST_DELTATITLE, value.DeltaTitle);
			AddToList(LIST_SHTNUM, value.ShtNum);
			AddToList(LIST_BLOCKTITLE, value.BlockTitle);
			AddToList(LIST_BASIS, value.Basis);
			AddToList(LIST_DESC, value.Description);
		}

		private void AddToList(EListSubject Elist, string value)
		{
			if (!_sumMastList[(int)Elist].Summary.Contains(value))
			{
				_sumMastList[(int)Elist].Summary.Add(value);
			}
		}
		
		#endregion

		// set a value in the current sequence list
		public string Sequence
		{
			get => _sumMastList[(int) LIST_SEQUENCE].Choice;
			set => _sumMastList[(int) LIST_SEQUENCE].Choice = value;
		}

		public List<string> SequenceList
		{
			get => _sumMastList[(int) LIST_SEQUENCE].Summary;
		}

		// set a value in the current sequence list
		public string AlternateId
		{
			get => _sumMastList[(int) LIST_REVALTID].Choice;
			set => _sumMastList[(int) LIST_REVALTID].Choice = value;
		}

		public List<string> AlternateIdList
		{
			get => _sumMastList[(int) LIST_REVALTID].Summary;
		}

		// set a value in the current sequence list
		public string SheetNumber
		{
			get => _sumMastList[(int) LIST_SHTNUM].Choice;
			set => _sumMastList[(int) LIST_SHTNUM].Choice = value;
		}

		public List<string> SheetNumberList
		{
			get => _sumMastList[(int) LIST_SHTNUM].Summary;
		}


		// conceptually, the plan is to create the sub-lists based on the lists above
		public void UpdateLists()
		{
			// clear all of the current lists
			InitLists(true, false);

			string[] chkList = new string[(int) LIST_COUNT];

			// read through each item and create the lists based on the search criteria
			// list is sorted and provided in sequence order
			foreach (KeyValuePair<string, RevDataItems2> kvp in revInfo)
			{
				chkList[(int) LIST_SEQUENCE] = kvp.Value.Sequence.ToString();
				chkList[(int) LIST_REVALTID] = kvp.Value.AltId.Trim();
				chkList[(int) LIST_SHTNUM] = kvp.Value.ShtNum;
				chkList[(int) LIST_DELTATITLE] = kvp.Value.DeltaTitle;
				chkList[(int) LIST_BLOCKTITLE] = kvp.Value.BlockTitle;
				chkList[(int) LIST_BASIS] = kvp.Value.Basis;
				chkList[(int) LIST_DESC] = kvp.Value.Description;

				bool result = true;

				for (int i = 0; i < (int) LIST_COUNT; i++)
				{
					if (!_sumMastList[i].Choice.Equals(ANY) &&
						!_sumMastList[i].Choice.Equals(chkList[i]))
					{
						result = false;
						break;
					}
				}

				if (!result) continue;

				// found a match - add the items
				for (int i = 0; i < (int) LIST_COUNT; i++)
				{
					if (!_sumMastList[i].Summary.Contains(chkList[i]))
					{
						_sumMastList[i].Summary.Add(chkList[i]);
					}

				}
			}
		}

		public IEnumerator<KeyValuePair<int,RevSummary2.ListData>> GetEnumerator()
		{
			return _sumMastList.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
