#region using

using System.Collections.Generic;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using CSToolsDelux.Fields.ExStorage.DataStorageManagement;
using CSToolsDelux.Fields.ExStorage.ExStorageData;
using CSToolsDelux.Fields.ExStorage.ExStorManagement;
using CSToolsDelux.Fields.SchemaInfo.SchemaData;
using CSToolsDelux.Fields.SchemaInfo.SchemaManagement;
using CSToolsDelux.WPF.FieldsWindow;
using SharedCode.Fields.SchemaInfo.SchemaSupport;
using SharedCode.Windows;
using SharedCode.Fields.SchemaInfo.SchemaData;
using SharedCode.Fields.SchemaInfo.SchemaFields;

using SharedCode.Fields.ExStorage.ExStorManagement;
using SharedCode.Fields.Testing;
using UtilityLibrary;

#endregion

// username: jeffs
// created:  8/28/2021 8:58:30 PM

namespace CSToolsDelux.Fields.FieldsManagement
{
	/// <summary>
	/// <c>DataStorExist</c> | Determine if DataStor(docName) Exists<br/>
	/// </summary>
	public class FieldsManager
	{
	#region private fields

		// basic datastorage routines - create, find, etc.  Also has a list
		// to save datastorages in order to use the revit model less - eliminate this part
		private DataStoreManager dsMgr;

		// ex storage routines - create, find, etc. - needs to be refined
		private ExStoreManager exMgr;

		// the static data class to hold the information for the document's datastore
		private ExStorData exData;

		// static schema routines - create & find
		// has a list of schema
		private SchemaManager scMgr;

		// show information routines
		private SharedCode.ShowInformation.ShShowInfo show;

		// private FieldsStartProcedure fs;

		private AWindow W;

		private Document doc;

	#endregion

	#region ctor

		static FieldsManager()
		{
			R = new FieldsRoot();
			C = new FieldsCell();
			L = new FieldsLock();
		}

		public FieldsManager(AWindow w, Document doc)
		{
			this.doc = doc;

			W = w;
			scMgr = SchemaManager.Instance;
			exMgr = new ExStoreManager(w, doc);
			dsMgr = new DataStoreManager(doc);
			show = new SharedCode.ShowInformation.ShShowInfo(w, CsUtilities.AssemblyName, MainFields.DocName);
			exData = ExStorData.Instance;
			// fs = new FieldsStartProcedure(w);

			rData = new SchemaRootData();
			rData.Configure("Root Data Name", "Root Data Description");

			cData = new SchemaCellData();
			cData.Configure("new name", "A1", UpdateRules.UR_AS_NEEDED,
				"cell Family", false, "xl file path", "worksheet name");

			RtData = new DataRoot(R);
			RtData.Configure("Root Data Name - Generic");

		}

	#endregion

	#region public properties

		public SchemaRootData rData { get; private set; }
		public SchemaCellData cData { get; private set; }
		public SchemaLockData lData { get; private set; }

		public static FieldsRoot R { get; }
		public static FieldsCell C { get; }
		public static FieldsLock L { get; }

		public DataRoot RtData { get; private set; }


	#endregion

	#region private properties

		public List<string> OldDataStorageList => dsMgr.OldDataStorageList;

	#endregion

	#region process procedures

	#region start procedure


		/// <summary>
		/// initial procedure that runs when first started<br/>
		/// this finds DataStore and Loads the data<br/>
		/// This
		/// </summary>
		/// <returns></returns>
		// proc00
		public ExStoreRtnCodes StartProcess()
		{
			int loc = SampleData.p00;

			ExStoreRtnCodes result = ExStoreRtnCodes.XRC_FAIL;





			return  result;
		}


		

	#endregion




	#endregion

	#region find

		public ExStoreRtnCodes DataStorExist(string docKey)
		{
			return dsMgr.DataStorageExists(docKey);
		}

		public ExStoreRtnCodes FindAppEntity(string docName, out Entity entity, out Schema schema)
		{
			ExStoreRtnCodes result;

			result = exMgr.FindRootEntity(docName, out entity, out schema);

			return result;
		}

	#endregion

	#region get

		public bool GetAppSchemas(string docKey, out IList<Schema> schemas)
		{
			schemas = null;

			bool result = scMgr.FindSchemas(docKey, out schemas);

			return result;
		}

	#endregion

	#region read

		public ExStoreRtnCodes ReadData()
		{
			ExStoreRtnCodes result;

			result = exMgr.ReadData();
			if (result != ExStoreRtnCodes.XRC_GOOD) return ExStoreRtnCodes.XRC_DATA_NOT_FOUND;

			return result;
		}

	#endregion

	#region write

		public ExStoreRtnCodes WriteRoot(SchemaRootData raData, SchemaCellData cData)
		{
			Transaction T;
			ExStoreRtnCodes result;

			using (T = new Transaction(AppRibbon.Doc, "fields"))
			{
				T.Start();
				result = exMgr.WriteRootData(raData, cData, exData.DataStorage);
				if (result == ExStoreRtnCodes.XRC_GOOD)
				{
					T.Commit();
				}
				else
				{
					T.RollBack();
				}
			}

			return result;
		}

	#endregion

	#region create

		public ExStoreRtnCodes CreateDataStorage(string docKey)
		{
			Transaction T;
			ExStoreRtnCodes result;

			using (T = new Transaction(AppRibbon.Doc, "fields"))
			{
				T.Start();
				result = dsMgr.CreateDataStorage(docKey);

				if (result != ExStoreRtnCodes.XRC_GOOD) return ExStoreRtnCodes.XRC_FAIL;

				T.Commit();
			}

			return result;
		}

	#endregion

	#region delete

		public ExStoreRtnCodes DeleteRoot(string docKey)
		{
			Entity entity;
			Schema schema;

			ExStoreRtnCodes result = FindAppEntity(docKey, out entity, out schema);
			if (result != ExStoreRtnCodes.XRC_GOOD) return result;

			result = exMgr.EraseRoot(entity, schema);
			if (result != ExStoreRtnCodes.XRC_GOOD) return result;

			return ExStoreRtnCodes.XRC_GOOD;
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
			return "this is FieldsManager";
		}

	#endregion

	#region test - debug methods

		public ExStoreRtnCodes FindRootDS()
		{
			ExStoreRtnCodes result;
			result = DataStorExist(exData.DsKey);
			if (result == ExStoreRtnCodes.XRC_DS_NOT_FOUND) return result;

			W.WriteLineAligned("fm| find root DS", $"found| {(exData.DataStorage?.Name ?? "null")}");

			return ExStoreRtnCodes.XRC_GOOD;
		}

		public ExStoreStartRtnCodes GetRootDataStorages(string docKey, out IList<DataStorage> dx)
		{
			ExStoreStartRtnCodes result;

			result = dsMgr.FindDataStorages(docKey, out dx);

			return result;
		}

	#endregion

	#region public show methods


		public void ShowLockData()
		{
			lData = new SchemaLockData();

			// show.ShowLockData(lFields, lData);
		}

	#endregion
	}
}