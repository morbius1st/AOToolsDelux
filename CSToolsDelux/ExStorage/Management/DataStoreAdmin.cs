#region using

using System.Collections.Generic;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using SharedCode.Fields.ExStorage.ExStorManagement;
using UtilityLibrary;

#endregion

// username: jeffs
// created:  10/22/2021 6:12:42 AM

namespace CSToolsDelux.ExStorage.Management
{
	/// <summary>
	/// Data Storage routines - find and create, etc.<br/>
	/// FindAllDs() - get a list of all DS / 'good', some found else, 'fail'
	/// FilterDs() - process list of DS for a prefix / 'good' some found else, 'fail'
	/// </summary>
	public class DataStoreAdmin
	{
	#region private fields

		private Document doc;


	#endregion

	#region ctor

		public DataStoreAdmin(Document doc)
		{
			this.doc = doc;

			AllDs = new List<DataStorage>(1);
			FoundDs = new List<DataStorage>(1);
			PriorDs = new List<DataStorage>(1);
		}

	#endregion

	#region public properties

		public List<DataStorage> AllDs { get; private set; }
		public List<DataStorage> FoundDs { get; private set; }
		public List<DataStorage> PriorDs { get; private set; }

	#endregion

	#region private properties

	#endregion

	#region public methods

		// find all DataStorages
		// return good if some found
		// return not found if none found
		// fn11
		public ExStoreRtnCodes FindAllDs()
		{
			ExStoreRtnCodes result = ExStoreRtnCodes.XRC_FAIL;

			AllDs = new List<DataStorage>(1);

			FilteredElementCollector collector = new FilteredElementCollector(doc);

			FilteredElementCollector dataStorages =
				collector.OfClass(typeof(DataStorage));

			if (dataStorages != null)
			{
				foreach (Element ds in dataStorages)
				{
					AllDs.Add((DataStorage) ds);
					result = ExStoreRtnCodes.XRC_GOOD;
				}
			}
			
			return result;
		}

		// filter found ds to have a specific preface
		// fn12
		public ExStoreRtnCodes FilterDs(string foundPreface, string priorPrefix, List<DataStorage> dsList)
		{
			FoundDs = new List<DataStorage>(1);

			ExStoreRtnCodes result = ExStoreRtnCodes.XRC_FAIL;

			if (foundPreface.IsVoid() || dsList == null || dsList.Count < 1) return result;

			foreach (DataStorage ds in dsList)
			{
				if (ds.Name.StartsWith(foundPreface))
				{
					FoundDs.Add(ds);
				} 
				else if (ds.Name.StartsWith(priorPrefix))
				{
					PriorDs.Add(ds);
				}
			}

			if (FoundDs.Count > 0 || PriorDs.Count > 0) result = ExStoreRtnCodes.XRC_GOOD;

			return result;
		}

		// process DS list to determine what was found - or not
		// fn13
		public ExStoreStartRtnCodes ProcessDsLists()
		{
			ExStoreStartRtnCodes result;

			if (FoundDs.Count > 0)
			{
				if (PriorDs.Count > 0)
				{
					result = ExStoreStartRtnCodes.XSC_YES_WITH_PRIOR;
				}
				else
				{
					result = ExStoreStartRtnCodes.XSC_YES;
				}
			}
			else
			{
				if (PriorDs.Count > 0)
				{
					result = ExStoreStartRtnCodes.XSC_NO_WITH_PRIOR;
				}
				else
				{
					result = ExStoreStartRtnCodes.XSC_NO;
				}
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
			return $"this is DataStoreAdmin| AllDs| {AllDs.Count}  FoundDs| {FoundDs.Count}";
		}

	#endregion
	}
}