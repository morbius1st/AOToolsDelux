using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOToolsDelux.Revisions
{
	public partial class RevCloudData2
	{

		#region + Select Sort Filter

		// select records from the master list
		public int Select(SelectCriteria2 sc2)
		{
			if (RevCloudMasterList2.Count == 0) return -1;

			// start with clean list
			RevCloudSelectedList2 =
				new SortedList<string, RevDataItems2>();
	
			int i = 0;

			foreach (KeyValuePair<string, RevDataItems2> kvp in MasterList)
			{
				if (sc2.Match(kvp.Key, kvp.Value))
				{
					i++;
					RevCloudSelectedList2.Add(kvp.Key, kvp.Value);
				}
			}
			return i;
		}

		#endregion


	}
}
