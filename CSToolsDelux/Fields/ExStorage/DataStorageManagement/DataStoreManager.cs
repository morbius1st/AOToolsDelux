#region using directives

using System.Collections.Generic;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using CSToolsDelux.Fields.ExStorage.ExStorageData;
using CSToolsDelux.Fields.ExStorage.ExStorManagement;
using UtilityLibrary;

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
	/// <c>FindDataStorages</c>  | Provide a list of existing DataStorages per DocKey<br/>
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
		}

	#endregion

	#region public properties

		public List<string> OldDataStorageList => oldDataStorages;

	#endregion

	#region private properties

	#endregion

	#region public methods


		/// <summary>
		/// Determine if the DataStorage exists - either already read<br/>
		/// or in the active document
		/// </summary>
		/// <param name="docKey"></param>
		/// <returns></returns>
		public ExStoreRtnCodes DataStorageExists(string docKey)
		{
			// step 1
			// already got?
			if (ExStorData.Instance.MatchName(docKey)) return ExStoreRtnCodes.XRC_GOOD;

			ExStoreRtnCodes result;
			IList<DataStorage> dsList;

			// step 2
			// see if the document has the datastorage
			result = FindDataStorages(docKey, out dsList);

			if ((dsList?.Count ?? 0) == 1)
			{
				exData.Config(docKey, dsList[0]);

				// found and saved
				return ExStoreRtnCodes.XRC_GOOD;
			}

			// not found
			return ExStoreRtnCodes.XRC_DS_NOT_EXIST;
		}

		public ExStoreRtnCodes CreateDataStorage(string docKey)
		{
			DataStorage ds = null;
			Transaction T;

			try
			{
				ds = DataStorage.Create(doc);
				ds.Name = docKey;
			}
			catch
			{
				return ExStoreRtnCodes.XRC_FAIL;
			}

			exData.Config(docKey, ds);

			return ExStoreRtnCodes.XRC_GOOD;
		}

		public ExStoreRtnCodes FindDataStorages(string docKey, out IList<DataStorage> dx)
		{
			ExStoreRtnCodes result;
			string vendIdPrefix = ExStorData.MakeVendIdPrefix();

			dx = new List<DataStorage>(1);
			oldDataStorages = new List<string>();

			IList<DataStorage> dataStorList;
			result = FindDataStorages(out dataStorList);

			foreach (Element ds in dataStorList)
			{
				if (ds.Name.StartsWith(docKey))
				{
					dx.Add((DataStorage) ds);
				} 
				else if (ds.Name.StartsWith(vendIdPrefix))
				{
					oldDataStorages.Add(ds.Name);
				}
			}

			if (oldDataStorages.Count > 0)
			{
				result = ExStoreRtnCodes.XRC_SEARCH_FOUND_PRIOR;

				if (dx.Count > 0)
				{
					result = ExStoreRtnCodes.XRC_SEARCH_FOUND_PRIOR_AND_NEW;
				}
			}
			else if (dx.Count > 0)
			{
				result = ExStoreRtnCodes.XRC_GOOD;
			}

			return result;
		}

		// find all DataStorages
		public ExStoreRtnCodes FindDataStorages(out IList<DataStorage> dx)
		{
			ExStoreRtnCodes result = ExStoreRtnCodes.XRC_DS_NOT_EXIST;
			dx = new List<DataStorage>(1);

			FilteredElementCollector collector = new FilteredElementCollector(doc);

			FilteredElementCollector dataStorages =
				collector.OfClass(typeof(DataStorage));
			if (dataStorages == null) return ExStoreRtnCodes.XRC_DS_NOT_EXIST;

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