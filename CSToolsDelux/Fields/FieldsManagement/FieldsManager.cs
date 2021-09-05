#region using
using System;
using System.Collections.Generic;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using CSToolsDelux.Fields.ExStorage.ExDataStorage;
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
	public class FieldsManager
	{
	#region private fields

		private DataStorageManager dsMgr;

		private ExStoreMgr exMgr;
		private SchemaManager scMgr;
		private ShowInfo show;

		private AWindow W;

		private Document doc;

		// ********* //
		private string docName;

	#endregion

	#region ctor

		static FieldsManager()
		{
			
			rFields = new SchemaRootFields();
			aFields = new SchemaAppFields();
			raFields = new SchemaRootAppFields();
			cFields = new SchemaCellFields();
		}

		public FieldsManager(AWindow w, Document doc, string documentName)
		{
			this.doc = doc;
			docName = documentName;

			W = w;
			scMgr = SchemaManager.Instance;
			exMgr = new ExStoreMgr(w, doc);
			dsMgr = new DataStorageManager(doc);
			show = new ShowInfo(w, docName);
			
			rData = new SchemaRootData();
			rData.Configure(SchemaGuidManager.GetNewAppGuidString());

			aData = new SchemaAppData();
			aData.Configure("App Data Name", "App Data Description");

			raData = new SchemaRootAppData();
			raData.Configure("Root-App Data Name", "Root-App Data Description");

			cData = new SchemaCellData();
			cData.Configure("new name", "A1", UpdateRules.UR_AS_NEEDED, "cell Family", false, "xl file path", "worksheet name");
		}

	#endregion

	#region public properties

		public SchemaRootFields SchemaRootFields => rFields;

		public SchemaRootData rData { get; private set; }
		public SchemaAppData aData { get; private set; }
		public SchemaRootAppData raData { get; private set; }
		public SchemaCellData cData { get; private set; }

		public static SchemaRootFields rFields { get; }
		public static SchemaAppFields aFields { get; }
		public static SchemaRootAppFields raFields { get; }
		public static SchemaCellFields cFields { get; }

	#endregion

	#region private properties

	#endregion

	#region public methods

		public bool MakeRootAppSchema(out Schema schema)
		{
			bool result = false;

			string key = scMgr.makeKey(docName);

			raFields.SetValue(SchemaRootAppKey.RAK_NAME, key);

			result = scMgr.MakeRootAppSchema(key, raFields, cData.Data.Count);

			schema = scMgr.SchemaList[key].Schema;

			if (result) show.ShowSchema(schema);

			return true;
		}

		public bool GetRootDataStorages(out IList<DataStorage> dx)
		{
			bool result = false;

			result = dsMgr.FindDataStorage(docName, out dx);

			return result;
		}

		public bool GetRootSchema(out IList<Schema> schemas)
		{
			schemas = null;

			bool result = scMgr.Find(docName, out schemas);

			return result;
		}

		public void GetDataStorage()
		{
			exMgr.GetDataStorage();
		}

		public void ShowRootAppFields()
		{
			show.ShowRootAppFields(raFields);
		}
		
		public void ShowRootAppData()
		{
			show.ShowRootAppData(raFields, raData);
		}

		public void ShowRootFields()
		{
			show.ShowRootFields(rFields);
		}

		public void ShowRootData()
		{
			show.ShowRootData(rFields, rData);
		}
		
		public void ShowAppFields()
		{
			show.ShowAppFields(aFields);
		}

		public void ShowAppData()
		{
			show.ShowAppData(aFields, aData);
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

	}
}