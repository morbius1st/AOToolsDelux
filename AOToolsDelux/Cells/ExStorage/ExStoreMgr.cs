#region + Using Directives

using AOTools.Cells.SchemaCells;
using static AOTools.Cells.ExStorage.ExStoreApp;
using static AOTools.Cells.ExStorage.ExStoreCell;

#endregion

// user name: jeffs
// created:   7/5/2021 6:55:42 AM

namespace AOTools.Cells.ExStorage
{
	public class ExStoreMgr
	{
		public static ExStoreMgr XsMgr { get; private set; } = new ExStoreMgr();
		public static bool Initialized { get; private set; }

		private ExStoreHelper XsHlpr;

		public ExStoreRtnCodes Save(ExStoreHelper XsHlpr, 
			ExStoreApp xApp, ExStoreCell xCell)
		{
			ExStoreRtnCodes result = XsHlpr.SaveExStorageData(xApp, xCell);

			return result;
		}


		public ExStoreRtnCodes UpdateCell(ExStoreHelper XsHlpr, ExStoreCell xCell)
		{
			ExStoreRtnCodes result = XsHlpr.UpdateExStorageCellData(xCell);

			return result;
		}


		// public void Reset()
		// {
		// 	Initialized = false;
		// 	Init();
		// }

		// public ExStoreRtnCodes Save(ExStoreApp xApp, ExStoreCell xCell)
		// {
		// 	if (!Initialized) return ExStoreRtnCodes.NOT_INIT;
		//
		// 	return XsHlpr.SaveExStorageData2(xApp, xCell);
		// }


	}
}
