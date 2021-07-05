#region + Using Directives

using static AOTools.Cells.ExStorage.ExStoreApp;
using static AOTools.Cells.ExStorage.ExStoreCell;

#endregion

// user name: jeffs
// created:   7/5/2021 6:55:42 AM

namespace AOTools.Cells.ExStorage
{
	public class ExStoreMgr
	{
		public static ExStoreMgr XsMgr { get; private set; }
		public static bool Initialized { get; private set; }

		private ExStoreHelper XsHlpr;

		public void Init()
		{
			if (Initialized) return;
			XsMgr = new ExStoreMgr();
			XsHlpr = new ExStoreHelper();

			Initialized = true;
		}

		public void Reset()
		{
			Initialized = false;
			Init();
		}

		public SaveRtnCodes Save(ExStoreApp xApp, ExStoreCell xCell)
		{
			if (!Initialized) return SaveRtnCodes.NOT_INIT;

			return XsHlpr.SaveExStorageData(xApp, xCell);
		}


	}
}
