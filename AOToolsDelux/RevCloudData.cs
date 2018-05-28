using System.Collections;
using System.Collections.Generic;

namespace AOTools
{
	public partial class RevCloudData : IEnumerable
	{
		#region + Data
		private static RevCloudData me = new RevCloudData();

		// this is the list of revision information
		// obtained from the revision coluds
		// this si the master list with all of the records
		// one master list to rule them all
		private static SortedList<RevDataKey, RevDataItems> RevCloudMasterList = 
			new SortedList<RevDataKey, RevDataItems>();

		// multiple selected lists as a sub-set of the master list
		private SortedList<RevDataKey, RevDataItems> RevCloudSelectedList =
			new SortedList<RevDataKey, RevDataItems>();

		#endregion

		#region + Class
		private RevCloudData() { }

		public static RevCloudData GetInstance()
		{
			// always re-read all of the data - make sure it is current
			Read();

			return me;
		}

		public IEnumerator GetEnumerator()
		{
			return RevCloudMasterList.GetEnumerator();
		}

		#endregion

		#region + Util

		private static void Read()
		{
			// initalize the master revision list
			RevData.Init();
			RevCloudMasterList = RevData.RevisionInfo;
		}

		#endregion

		#region + Master List

		public SortedList<RevDataKey, RevDataItems> MasterList => 
			RevCloudMasterList;

		public int MasterListCount => RevCloudMasterList.Count;

		#endregion

		#region + Selected List

		public SortedList<RevDataKey, RevDataItems> SelectedList =>
			RevCloudSelectedList;

		public int SelectedListCount => RevCloudSelectedList.Count;
		

		#endregion

	}
}
