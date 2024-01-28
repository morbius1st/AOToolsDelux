using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOToolsDelux
{
	public partial class RevCloudData
	{

		#region + Select Sort Filter

		// select records from the master list
		public int Select(SelectCriteria sc)
		{
			if (RevCloudMasterList.Count == 0) return -1;

			// start with clean list
			RevCloudSelectedList =
				new SortedList<RevDataKey, RevDataItems>();
	
			int i = 0;

			foreach (KeyValuePair<RevDataKey, RevDataItems> kvp in MasterList)
			{
				if (sc.Match(kvp.Key, kvp.Value))
				{
					i++;
					RevCloudSelectedList.Add(kvp.Key, kvp.Value);
				}
			}
			return i;
		}

		#endregion


	}
}
