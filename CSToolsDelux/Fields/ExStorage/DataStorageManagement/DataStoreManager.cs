#region using directives
using System.Collections.Generic;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using CSToolsDelux.Fields.ExStorage.ExStorageData;
using SharedCode.Fields.ExStorage.ExStorManagement;
#endregion


// projname: $projectname$
// itemname: DataStorageManager
// username: jeffs
// created:  8/15/2021 6:39:48 PM



namespace CSToolsDelux.Fields.ExStorage.DataStorageManagement
{
	/// <summary>
	/// <c>DataStorageExists</c> | Determine if the DataStorage Exists<br/>
	/// <c>CreateDataStorage</c> | Create a DataStorage element (no info)<br/>
	/// <c>CreateDataStorage</c> | Create a DataStorage element (with info)<br/>
	/// <c>FindDataStorages</c>  | Provide a list of existing DataStorages per DsKey<br/>
	/// <c>FindDataStorages</c>  | Provide a list of existing DataStorages<br/>
	/// </summary>
	public class DataStoreManager
	{

	#region private fields


		private Document doc;
		private ExStorData exData;

		private List<string> oldDataStorages;

	#endregion

	#region ctor

		public DataStoreManager(Document doc)
		{
			this.doc = doc;
			exData = ExStorData.Instance;

			LockStatus = ExStorDsLockStatus.XLK_UNKNOWN;
		}

	#endregion

	#region public properties

		public List<string> OldDataStorageList => oldDataStorages;

		public static ExStorDsLockStatus LockStatus { get; set; }


	#endregion

	#region private properties

	#endregion

	#region public methods


		/// <summary>
		/// Determine if the DataStorage exists - either already read<br/>
		/// or in the active document
		/// </summary>
		/// <param name="dsKey"></param>
		/// <returns></returns>
		public ExStoreRtnCodes DataStorageExists(string dsKey)
		{
			// step 1
			// already got?
			if (ExStorData.Instance.MatchName(dsKey)) return ExStoreRtnCodes.XRC_GOOD;

			ExStoreStartRtnCodes result;
			IList<DataStorage> dsList;

			// step 2
			// see if the document has the datastorage
			result = FindDataStorages(dsKey, out dsList);

			if ((dsList?.Count ?? 0) == 1)
			{
				exData.Config(dsKey, dsList[0]);

				// found and saved
				return ExStoreRtnCodes.XRC_GOOD;
			}

			// not found
			return ExStoreRtnCodes.XRC_DS_NOT_FOUND;
		}

		public ExStoreRtnCodes CreateDataStorage(string dsKey)
		{
			DataStorage ds = null;
			Transaction T;

			try
			{
				ds = DataStorage.Create(doc);
				ds.Name = dsKey;
			}
			catch
			{
				return ExStoreRtnCodes.XRC_FAIL;
			}

			exData.Config(dsKey, ds);

			return ExStoreRtnCodes.XRC_GOOD;
		}


		/// <summary>
		/// Search for 
		/// </summary>
		/// <param name="dsKey"></param>
		/// <param name="dx"></param>
		/// <returns></returns>
		public ExStoreStartRtnCodes FindDataStorages(string dsKey, out IList<DataStorage> dx)
		{
			ExStoreStartRtnCodes answer;

			ExStoreRtnCodes result;
			string vendIdPrefix = ExStorData.VendorId;

			dx = new List<DataStorage>(1);
			oldDataStorages = new List<string>();

			IList<DataStorage> dataStorList;

			result = FindDataStorages(out dataStorList);

			foreach (Element ds in dataStorList)
			{
				if (ds.Name.StartsWith(dsKey))
				{
					dx.Add((DataStorage) ds);
				} 
				else if (ds.Name.StartsWith(vendIdPrefix))
				{
					oldDataStorages.Add(ds.Name);
				}
			}

			if (dx.Count > 0)
			{
				if (oldDataStorages.Count == 0)
				{
					answer = ExStoreStartRtnCodes.XSC_YES;
				}
				else
				{
					answer = ExStoreStartRtnCodes.XSC_YES_WITH_PRIOR;
				}
			}
			else
			{
				if (oldDataStorages.Count == 0)
				{
					answer = ExStoreStartRtnCodes.XSC_NO;
				}
				else
				{
					answer = ExStoreStartRtnCodes.XSC_NO_WITH_PRIOR;
				}
			}

			return answer;
		}

		// find all DataStorages
		public ExStoreRtnCodes FindDataStorages(out IList<DataStorage> dx)
		{
			ExStoreRtnCodes result = ExStoreRtnCodes.XRC_DS_NOT_FOUND;
			dx = new List<DataStorage>(1);

			FilteredElementCollector collector = new FilteredElementCollector(doc);

			FilteredElementCollector dataStorages =
				collector.OfClass(typeof(DataStorage));
			if (dataStorages == null) return ExStoreRtnCodes.XRC_DS_NOT_FOUND;

			foreach (Element ds in dataStorages)
			{
				dx.Add((DataStorage) ds);
				result = ExStoreRtnCodes.XRC_GOOD;
			}

			return result;
		}


	#endregion

	#region private methods

	#endregion

	#region event consuming

	#endregion

	#region event publishing

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is DataStorageManager";
		}

	#endregion

	}
}