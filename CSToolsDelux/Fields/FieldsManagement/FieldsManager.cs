#region using

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using Autodesk.Revit.UI;
using CSToolsDelux.Fields.ExStorage.DataStorageManagement;
using CSToolsDelux.Fields.ExStorage.ExStorageData;
using CSToolsDelux.Fields.ExStorage.ExStorManagement;
using CSToolsDelux.Fields.SchemaInfo.SchemaData;
using CSToolsDelux.Fields.SchemaInfo.SchemaDefinitions;
using CSToolsDelux.Fields.SchemaInfo.SchemaFields;
using CSToolsDelux.Fields.SchemaInfo.SchemaManagement;
using CSToolsDelux.Fields.Testing;
using CSToolsDelux.WPF;

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

		private DataStoreManager dsMgr;
		private ExStorData exData;
		private ExStoreManager exMgr;
		private SchemaManager scMgr;
		private ShowInfo show;
		// private FieldsStartProcedure fs;

		private AWindow W;

		private Document doc;

	#endregion

	#region ctor

		static FieldsManager()
		{
			raFields = new SchemaRootAppFields();
			cFields = new SchemaCellFields();
		}

		public FieldsManager(AWindow w, Document doc)
		{
			this.doc = doc;

			W = w;
			scMgr = SchemaManager.Instance;
			exMgr = new ExStoreManager(w, doc);
			dsMgr = new DataStoreManager(doc);
			show = new ShowInfo(w);
			exData = ExStorData.Instance;
			// fs = new FieldsStartProcedure(w);

			raData = new SchemaRootAppData();
			raData.Configure("Root-App Data Name", "Root-App Data Description");

			cData = new SchemaCellData();
			cData.Configure("new name", "A1", UpdateRules.UR_AS_NEEDED,
				"cell Family", false, "xl file path", "worksheet name");
		}

	#endregion

	#region public properties

		public SchemaRootAppData raData { get; private set; }
		public SchemaCellData cData { get; private set; }

		public static SchemaRootAppFields raFields { get; }
		public static SchemaCellFields cFields { get; }

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

		public ExStoreRtnCodes FindRootAppEntity(string docName, out Entity entity, out Schema schema)
		{
			ExStoreRtnCodes result;

			result = exMgr.FindRootAppEntity(docName, out entity, out schema);

			return result;
		}

	#endregion

	#region get

		public bool GetRootAppSchemas(string docKey, out IList<Schema> schemas)
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

		public ExStoreRtnCodes WriteRootApp(SchemaRootAppData raData, SchemaCellData cData)
		{
			Transaction T;
			ExStoreRtnCodes result;

			using (T = new Transaction(AppRibbon.Doc, "fields"))
			{
				T.Start();
				result = exMgr.WriteRootAppData(raData, cData, exData.DataStorage);
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

		public ExStoreRtnCodes DeleteRootApp(string docKey)
		{
			Entity entity;
			Schema schema;

			ExStoreRtnCodes result = FindRootAppEntity(docKey, out entity, out schema);
			if (result != ExStoreRtnCodes.XRC_GOOD) return result;

			result = exMgr.EraseRootApp(entity, schema);
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
			result = DataStorExist(exData.DocKey);
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

		public void ShowRootAppFields()
		{
			show.ShowRootAppFields(raFields);
		}

		public void ShowRootAppData()
		{
			show.ShowRootAppData(raFields, raData);
		}

		public void ShowCellFields()
		{
			show.ShowCellFields(cFields);
		}

		public void ShowCellData()
		{
			show.ShowCellData(cData, cFields);
		}

	#endregion
	}
}