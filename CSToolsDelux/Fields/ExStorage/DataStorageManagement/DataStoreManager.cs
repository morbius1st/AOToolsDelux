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

		
		
		// /// <summary>
		// /// create a datastorage element and assign the entity
		// /// </summary>
		// /// <param name="schema"></param>
		// /// <param name="e"></param>
		// /// <param name="ds"></param>
		// /// <returns></returns>
		// public ExStoreRtnCodes CreateDataStorage(Schema schema,
		// 	out Entity e, out DataStorage ds)
		// {
		// 	e = null;
		// 	ds = null;
		//
		// 	try
		// 	{
		// 		ds = DataStorage.Create(doc);
		//
		// 		e = new Entity(schema);
		//
		// 		ds.SetEntity(e);
		// 	}
		// 	catch
		// 	{
		// 		return ExStoreRtnCodes.XRC_FAIL;
		// 	}
		//
		// 	return ExStoreRtnCodes.XRC_GOOD;
		// }

		// /// <summary>
		// /// find an existing datastorage element
		// /// </summary>
		// /// <param name="schema"></param>
		// /// <param name="ex"></param>
		// /// <param name="dx"></param>
		// /// <returns></returns>
		// public bool GetDataStorage(Schema schema, out Entity ex, out DataStorage dx)
		// {
		// 	ex = null;
		// 	dx = null;
		//
		// 	FilteredElementCollector collector = new FilteredElementCollector(doc);
		//
		// 	FilteredElementCollector dataStorages =
		// 		collector.OfClass(typeof(DataStorage));
		//
		// 	if (dataStorages == null) return false;
		//
		// 	foreach (DataStorage ds in dataStorages)
		// 	{
		// 		Entity e = ds.GetEntity(schema);
		//
		// 		if (!e.IsValid()) continue;
		//
		// 		ex = e;
		// 		dx = ds;
		//
		// 		return true;
		// 	}
		//
		// 	return false;
		// }


		// find DS for cells
		// public string FindDataStorage(out DataStorage dx)
		// {
		// 	string result = null;
		// 	string name = "Cells";
		// 	dx = null;
		//
		// 	FilteredElementCollector collector = new FilteredElementCollector(doc);
		//
		// 	FilteredElementCollector dataStorages =
		// 		collector.OfClass(typeof(DataStorage));
		//
		// 	if (dataStorages == null) return null;
		//
		// 	foreach (Element ds in dataStorages)
		// 	{
		// 		if (!ds.Name.IsVoid() && ds.Name.StartsWith(name))
		// 		{
		// 			result += ds.Name + " \n";
		// 		}
		// 	}
		//
		// 	return result;
		// }

		// private Dictionary<string, ExStorData> exStoreList;

		// private Entity[] dataStoreEntity;
		// private bool[] dataStoreExists;
		// private DataStorage[] dataStorage;

		// public class ExStorData
		// {
		// 	public Entity Entity { get; set; }
		// 	public Schema Schema { get; set; }
		// 	public DataStorage DataStorage { get; set; }
		// 	public string Name { get; }
		//
		// 	public ExStorData( string name,
		// 		DataStorage dataStorage,
		// 		Entity entity = null, Schema schema = null)
		// 	{
		// 		Entity = entity;
		// 		Schema = schema;
		// 		DataStorage = dataStorage;
		// 		Name = name;
		// 	}
		//
		// 	// public ExStorData(string name) : this(name, null) { }
		//
		// 	// public bool GotEntity => Entity != null && Entity.IsValid();
		// 	// public bool GotSchema => Schema != null;
		// 	public bool GotDataStorage => DataStorage != null;
		// }

		// private readonly string[] DATA_STORE_NAMES =
		// {
		// 	"Cells│ Root Data Storage",
		// 	"Cells│ App Data Storage",
		// 	"Cells│ Cell Info not used",
		// 	"Cells| Root-App Data Storage"
		// };


		// /// <summary>
		// /// Wipe out data storage references and delete the datastorage elements.
		// /// </summary>
		// public void Restart()
		// {
		// 	exData.Config(null, null);
		// }

		// public ExStoreRtnCodes GetDataStorage(string docKey)
		// {
		// 	ExStoreRtnCodes result;
		//
		// 	IList<DataStorage> dsList;
		//
		// 	// check in (3) places,
		// 	// saved in list.
		// 	// exists in document,
		// 	// make new
		//
		// 	// part 1
		// 	if (exStoreList.ContainsKey(docKey)) return ExStoreRtnCodes.XRC_GOOD;
		//
		// 	DataStorage ds;
		//
		// 	// part 2
		// 	result = FindDataStorages(docKey, out dsList);
		//
		// 	if ((dsList?.Count ?? 0) == 1)
		// 	{
		// 		ds = dsList[0];
		// 	} 
		// 	else if ((dsList?.Count ?? 0) == 0)
		// 	{
		// 		// part 3
		// 		result = CreateDataStorage(docKey, out ds);
		// 		if (ds == null) return ExStoreRtnCodes.XRC_FAIL;
		// 	}
		// 	else
		// 	{
		// 		return ExStoreRtnCodes.XRC_FAIL;
		// 	}
		//
		// 	exStoreList.Add(docKey, new ExStorData(docKey, ds));
		// 	return ExStoreRtnCodes.XRC_GOOD;
		// }

		
		// /// <summary>
		// /// create a datastorage element but do not assign an entity<br/>
		// /// must be within a transaction
		// /// </summary>
		// /// <param name="idx"></param>
		// /// <returns></returns>
		// public ExStoreRtnCodes CreateDataStorage(DataStoreIdx idx)
		// {
		// 	try
		// 	{
		// 		DataStorage ds = DataStorage.Create(doc);
		// 		ds.Name = exStoreList[(int) idx].Name;
		// 		exStoreList[(int) idx].DataStorage = ds;
		// 	}
		// 	catch
		// 	{
		// 		return ExStoreRtnCodes.XRC_FAIL;
		// 	}
		//
		// 	return ExStoreRtnCodes.XRC_GOOD;
		// }


	}
}