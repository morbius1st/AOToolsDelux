using System.Collections;
using System.Collections.Generic;

namespace AOToolsDelux.Revisions
{
	public partial class RevCloudData2 : IEnumerable
	{
		#region + Data
		private static RevCloudData2 me = new RevCloudData2();

		// this is the list of revision information
		// obtained from the revision coluds
		// this si the master list with all of the records
		// one master list to rule them all
		private static SortedList<string, RevDataItems2> RevCloudMasterList2 = 
			new SortedList<string, RevDataItems2>();

		// multiple selected lists as a sub-set of the master list
		private SortedList<string, RevDataItems2> RevCloudSelectedList2 =
			new SortedList<string, RevDataItems2>();

		#endregion

		#region + Class
		private RevCloudData2() { }

		public static RevCloudData2 GetInstance()
		{
			// always re-read all of the data - make sure it is current
			Read();

			return me;
		}

		public IEnumerator GetEnumerator()
		{
			return RevCloudMasterList2.GetEnumerator();
		}

		#endregion

		#region + Util

		private static void Read()
		{
			// initalize the master revision list
			RevData2.Init();
			RevCloudMasterList2 = RevData2.RevisionInfo;
		}

		#endregion

		#region + Master List

		public SortedList<string, RevDataItems2> MasterList => 
			RevCloudMasterList2;

		public int MasterListCount => RevCloudMasterList2.Count;

		#endregion

		#region + Selected List

		public SortedList<string, RevDataItems2> SelectedList =>
			RevCloudSelectedList2;

		public int SelectedListCount => RevCloudSelectedList2.Count;
		

		#endregion

	}
}
