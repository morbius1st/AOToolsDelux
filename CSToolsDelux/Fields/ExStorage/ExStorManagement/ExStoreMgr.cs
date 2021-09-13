#region + Using Directives

using System;
using System.Collections.Generic;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using Autodesk.Revit.UI;
using CSToolsDelux.Fields.ExStorage.DataStorageManagement;
using CSToolsDelux.Fields.ExStorage.ExStorageData;
using CSToolsDelux.Fields.SchemaInfo.SchemaData;
using CSToolsDelux.Fields.SchemaInfo.SchemaData.SchemaDataDefinitions;
using CSToolsDelux.Fields.SchemaInfo.SchemaDefinitions;
using CSToolsDelux.Fields.SchemaInfo.SchemaFields;
using CSToolsDelux.Fields.SchemaInfo.SchemaManagement;
using CSToolsDelux.Fields.Testing;
using CSToolsDelux.WPF;
using static Autodesk.Revit.DB.ExtensibleStorage.Schema;
using InvalidOperationException = Autodesk.Revit.Exceptions.InvalidOperationException;

#endregion

// user name: jeffs
// created:   7/5/2021 6:55:42 AM

/*
 * process:
 *  config
 *   +-> finds info -> read info.
 *   +-> not find info -> create info -> write info
 *
 * make root path
 * makeroot()-> DsMgr.CreateDataStorage<-
 *           +> WriteRoot-> xsHlpr.WriteRootData -> makeRootSchema -> makeSchemaDef <-
 *                                                                 +> makeSchemaFields -> makeSchemaField <-
 *                                               +> writeData <-
 *
 *
 *
 * note: since a schema is immutable, the subschema (cells) cannot
 * be modified / deleted 
 *
 * procedures
 * startup
 *  +> ExMgr
 *     +> initialize()
 *        * init to true
 *        * status to INIT
 *     +> config()
 *        * status is INIT
 *        +> dsExists == true // note that ds and root get setup together
 *           +> rootExists == true // root can exist separate from app & cell
 *              +> appExists == true // note that app / cell will be all config'd together
 *                 * proceed "normal"
 *                   * status to READY
 *              +> appExists == false
 *                 * need cell information to proceed
 *                   * status to NEED_APP_CELL
 *           +> rootExists == false
 *              * this should never happen as ds and root get setup together
 *                * status to FAIL
 *       +> dsExists == false
 *          * need to setup system
 *            * status to NEED_CONFIG
 *
 * startup
 *  +> start ExMgr
 *     +> status == READY -> proceed normal -> flip buttons
 *     +> status == INIT -> should never happen -> report program failure - exit
 *     +> status == FAIL -> should never happen -> report program failure - exit
 *     +> status == NEED_APP_CELL -> flip buttons
 *     +> status == NEED_CONFIG -> flip buttons
 *
 *  buttons (on a dialog box)
 *                        button             
 *                        off unless         
 *                        status             
 *  * initialize          is NEED_CONFIG     
 *  * add cell			  is NEED_APP_CELL or
 *                        is READY
 *  * show root info	  is READY
 *  * show app info		  is READY
 *    * show cell info	  is READY
 *  * show all cell info  is READY
 *  * modify cell		  is READY
 *  * delete cell		  is READY
 *  * remove system		  is READY
 *
 *			
 *			
 * function							datastore (ds)
 *					init	confg	root	app		root		app	
 *					req'd	req'd	exists	exists	exists		exists	
 *
 * initialize		false	n/a		n/a		n/a		n/a			n/a
 * configure		true	false	n/a		n/a		n/a			n/a		tests ds / root / app exists
 *
 * Create ds		true	false	false	n/a		n/a			n/a
 *
 * Read root		n/a		n/a		true			n/a			n/a		true ds == init / config true
 * Read app			n/a		n/a		n/a				true		false	true root exist == init / confg / ds true
 * Read cells		n/a		n/a		n/a				true		true	true root exist == init / confg / ds true
 * write root		n/a		n/a		true			false		n/a		true ds == init / config true
 * write app +		n/a		n/a		n/a				true		n/a		true root exist == init / confg / ds true
 *	cells
 * update root		-- n/a -						 not permitted
 * update app +		n/a		n/a		n/a				true 		n/a		true root exist == init true
 * 	cells
 * del root			n/a		n/a		n/a				true		false	false app exist == false cell(s) exist / true root exists == init / confg / ds true
 * del app +		n/a		n/a		n/a				n/a			true	true app exists == init / confg / ds true
 *  cell(s)
 *
 * ds exists		true	false	false			n/a			n/a
 * root exists		na		n/a		true			false		n/a		
 * app exists		n/a		n/a		n/a				true		false
 *
 * **	cell(s) exist	n/a							n/a			true	** probably do not need
 */

namespace CSToolsDelux.Fields.ExStorage.ExStorManagement
{
	public class ExStoreMgr
	{
		private Document doc;
		private AWindow W;

		private ExStorData exData;
		private DataStoreManager dsMgr;
		private SchemaManager scMgr;
		private ShowInfo show;


		public ExStoreMgr(AWindow w, Document doc)
		{
			this.doc = doc;
			W = w;

			dsMgr = new DataStoreManager(doc);
			scMgr = SchemaManager.Instance;
			show = new ShowInfo(w);
			exData = ExStorData.Instance;
		}

		public bool Initialized { get; private set; }

		public bool Configured { get; private set; }

		public string OpDescription  { get; private set; }


	#region find

		public ExStoreRtnCodes FindRootAppEntity(string docKey, out Entity entity, out Schema schema)
		{
			entity = null;
			schema = null;

			IList<Schema> schemas;
			IList<DataStorage> dx;
			bool result;

			result = scMgr.FindSchemas(docKey, out schemas);

			
			if (!result || schemas.Count > 1)
			{
				return ExStoreRtnCodes.XRC_FAIL;
			}
			else
			{
				show.ShowSchemas(schemas);

				ExStoreRtnCodes answer = dsMgr.FindDataStorages(docKey, out dx);

				if (answer != ExStoreRtnCodes.XRC_GOOD) return answer;
			}

			// got a schema and a datastorage - get the entity
			schema = schemas[0];
			entity = dx[0].GetEntity(schemas[0]);

			if (entity == null) return ExStoreRtnCodes.XRC_FAIL;

			return ExStoreRtnCodes.XRC_GOOD;
		}

	#endregion


	#region delete

		public ExStoreRtnCodes EraseRootApp(Entity e, Schema s)
		{
			List<Entity> entities;
			int i;
			try
			{
				entities = GetSchemaSubEntities(e, s);

				for (i = entities.Count - 1; i >= 0 ; i--)
				{
					EraseSchemaAndAllEntities(entities[i].Schema, false);

					entities[i].Dispose();
				}
			}
			catch
			{
				return ExStoreRtnCodes.XRC_FAIL;
			}

			return ExStoreRtnCodes.XRC_GOOD;
		}

		private List<Entity> GetSchemaSubEntities(Entity entity, Schema schema)
		{
			List<Entity> entityList = new List<Entity>();

			// add the parent schema as element 0;
			entityList.Add(entity);

			foreach (Field f in schema.ListFields())
			{
				if (f.SubSchema == null) continue;

				Field field = schema.GetField(f.FieldName);
				if (field == null || !field.IsValidObject) break;

				Entity subSchema = entity.Get<Entity>(field);
				if (subSchema == null || !subSchema.IsValidObject) break;

				entityList.Add(subSchema);
			}

			return entityList;
		}

	#endregion


	#region read

		public ExStoreRtnCodes ReadData()
		{
			ExStoreRtnCodes result;

			if (!exData.HasDataStorage || !exData.HasDocKey) 
				return ExStoreRtnCodes.XRC_DS_NOT_EXIST;

			if (!exData.HasEntity || !exData.HasSchema)
			{
				result = getRootAppEntityAndSchema();
				if (result != ExStoreRtnCodes.XRC_GOOD) return result;
			}

			result = readRootAppData();
			if (result != ExStoreRtnCodes.XRC_GOOD) return result;

			result = readCellData();
			if (result != ExStoreRtnCodes.XRC_GOOD) return result;

			return ExStoreRtnCodes.XRC_GOOD;
		}

		private ExStoreRtnCodes readRootAppData()
		{
			ExStoreRtnCodes result;

			SchemaDictionaryRootApp a =
				exData.RootAppData.Fields;

			result = readData(exData.RootAppData);

			// result = ReadData(exData.RootAppData.Data, exData.RootAppData.Fields);
			if (result != ExStoreRtnCodes.XRC_GOOD) return result;

			return ExStoreRtnCodes.XRC_GOOD;
		}

		private ExStoreRtnCodes readCellData()
		{
			ExStoreRtnCodes result;

			List<Entity> subEntities = GetSchemaSubEntities(exData.Entity, exData.Schema);

			if (subEntities.Count > 0)
			{
				for (int i = 0; i < subEntities.Count; i++)
				{
					exData.CellData.DataList.Add(new SchemaDataDictCell());

					exData.CellData.Index = i;

					result = readData(exData.CellData);

					// result = ReadData(exData.CellData.Data, exData.CellData.Fields);
					if (result != ExStoreRtnCodes.XRC_GOOD) return ExStoreRtnCodes.XRC_FAIL;
				}
			}

			return ExStoreRtnCodes.XRC_GOOD;
		}

		private ExStoreRtnCodes readData<TE, TD, TF>(ISchemaData<TE, TD, TF> schemaData) 
			where TE : Enum 
			where TD : SchemaDataDictionaryBase<TE> 
			where TF : SchemaDictionaryBase<TE>
		{
			foreach (KeyValuePair<TE, ISchemaFieldDef<TE>> kvp in schemaData.Fields)
			{
				TE key = kvp.Value.Key;
				string fieldName = kvp.Value.Name;

				Field f = exData.Schema.GetField(fieldName);
				if (f == null) return ExStoreRtnCodes.XRC_FAIL;

				Type t = f.ValueType;
				if (t == null) return ExStoreRtnCodes.XRC_FAIL;


				if (t?.Equals(typeof(string)) ?? false)
				{
					schemaData.Add(key, exData.Entity.Get<string>(fieldName));
				}
				else if (t?.Equals(typeof(double)) ?? false)
				{
					schemaData.Add(key, exData.Entity.Get<double>(fieldName));
				}
				else if (t?.Equals(typeof(bool)) ?? false)
				{
					schemaData.Add(key, exData.Entity.Get<bool>(fieldName));
				}
				else
				{
					schemaData.Add(key, exData.Entity.Get<string>(fieldName));
				}
			}

			return ExStoreRtnCodes.XRC_GOOD;
		}

	#endregion

	#region create


	#endregion


	#region write

		public ExStoreRtnCodes WriteRootAppData(SchemaRootAppData raData, SchemaCellData cData, 
			DataStorage ds)
		{
			Transaction t = null;
			string docKey = exData.DocKey;
			ExStoreRtnCodes result = ExStoreRtnCodes.XRC_FAIL;

			raData.SetValue(SchemaRootAppKey.RAK_NAME, docKey);

			try
			{
				bool answer =
					scMgr.MakeRootAppSchema(docKey, raData, cData.DataList.Count);
				if (!answer) return ExStoreRtnCodes.XRC_FAIL;

				Schema schema = scMgr.SchemaList[docKey].Schema;

				Entity e = new Entity(schema);

				scMgr.MakeSubSchemasFields(e, schema, cData);

				writeData(e, schema, raData.Data);
				writeCellData(e, schema, cData);

				// using (t = new Transaction(AppRibbon.Doc, "Save Cells Default Config Info"))
				// {
				// 	t.Start();
				ds.SetEntity(e);
				// 	t.Commit();
				// }
			}
			catch (InvalidOperationException ex)
			{
				if (t != null && t.HasStarted())
				{
					t.RollBack();
					t.Dispose();
				}

				if (ex.HResult == -2146233088)
				{
					return ExStoreRtnCodes.XRC_DUPLICATE;
				}

				return ExStoreRtnCodes.XRC_FAIL;
			}

			return ExStoreRtnCodes.XRC_GOOD;
		}

		private void writeData<T>(Entity entity, Schema schema, SchemaDataDictionaryBase<T> data) where T : Enum
		{
			foreach (KeyValuePair<T, ASchemaDataFieldDef<T>> kvp in data)
			{
				Field f = schema.GetField(kvp.Value.FieldDef.Name);
				if (f == null || !f.IsValidObject) continue;


				if (kvp.Value.FieldDef.UnitType != RevitUnitType.UT_UNDEFINED)
				{
					entity.Set(f, kvp.Value.AsDouble(), DisplayUnitType.DUT_GENERAL);
				}
				else
				{
					if (kvp.Value.ValueType == typeof(string))
					{
						entity.Set(f, kvp.Value.AsString());
					}
					else if (kvp.Value.ValueType == typeof(double))
					{
						entity.Set(f, kvp.Value.AsDouble());
					}
					else if (kvp.Value.ValueType == typeof(int))
					{
						entity.Set(f, kvp.Value.AsInteger());
					}
				}
			}
		}

		private void writeCellData(Entity entity, Schema schema, SchemaCellData cData)
		{
			int j = 0;

			foreach (KeyValuePair<string, Guid> kvp in
				scMgr.SchemaList[cData.DocKey].SubSchemaFields)
			{
				Field f = schema.GetField(kvp.Key);

				Entity subE = entity.Get<Entity>(kvp.Key);

				cData.Index = j++;
				writeData(subE, subE.Schema, cData.Data);

				entity.Set(f, subE);
			}
		}

	#endregion


	#region get

		public IList<Schema> GetSchemas()
		{
			IList<Schema> schemas = ListSchemas();

			return schemas;
		}

		public void GetDataStorage()
		{
			DataStorage ds;

			string result = dsMgr.FindDataStorage(out ds);

			if (result != null)
			{
				W.WriteLineAligned($"DataStorages found|\n", result);
			}
			else
			{
				W.WriteLineAligned($"DataStorages not found|", result);
			}

			IList<Schema> schemas = GetSchemas();

			W.WriteAligned("\n");
			W.WriteAligned($"List of schema|");

			if (schemas != null)
			{
				W.WriteLineMsg($"found| {schemas.Count}");

				foreach (Schema s in schemas)
				{
					W.WriteLineAligned($"schema|", $"name| {s.SchemaName.PadRight(35)}  vendor id| {s.VendorId.PadRight(20)}   guid| {s.GUID.ToString()}");
				}
			}
			else
			{
				W.WriteLineAligned($"none found");
			}

			W.ShowMsg();
		}

		private ExStoreRtnCodes getRootAppEntityAndSchema()
		{
			ExStoreRtnCodes result;
			Entity e = null;
			Schema s = null;

			if (!exData.HasDataStorage || !exData.HasDocKey) 
				return ExStoreRtnCodes.XRC_DS_NOT_EXIST;

			s = scMgr.FindSchema(exData.DocKey);
			if (s == null) return ExStoreRtnCodes.XRC_SCHEMA_NOT_FOUND;

			exData.Schema = s;

			e = exData.DataStorage.GetEntity(s);
			if (e == null) return ExStoreRtnCodes.XRC_ENTITY_NOT_FOUND;

			exData.Entity = e;

			return ExStoreRtnCodes.XRC_GOOD;
		}

	#endregion



		// private ExStoreRtnCodes ReadData<TE>(SchemaDataDictionaryBase<TE> data,
		// 	SchemaDictionaryBase<TE> fields ) where TE : Enum
		// {
		// 	foreach (KeyValuePair<TE, ISchemaFieldDef<TE>> kvp in fields)
		// 	{
		// 		TE key = kvp.Value.Key;
		// 		string fieldName = kvp.Value.Name;
		// 		// string fieldName = kvp.Value.FieldDef.Name;
		// 		Field f = exData.Schema.GetField(fieldName);
		// 		if (f == null) return ExStoreRtnCodes.XRC_FAIL;
		//
		// 		Type t = f.ValueType;
		// 		if (t == null) return ExStoreRtnCodes.XRC_FAIL;
		//
		// 		if (t?.Equals(typeof(string)) ?? false)
		// 		{
		// 			((SchemaDataFieldDef<TE, string>) data[key]).Value = 
		// 				exData.Entity.Get<string>(fieldName);
		// 		}
		// 		else if (t?.Equals(typeof(double)) ?? false)
		// 		{
		// 			((SchemaDataFieldDef<TE, double>) data[key]).Value = 
		// 				exData.Entity.Get<double>(fieldName);
		// 		}
		// 		else if (t?.Equals(typeof(bool)) ?? false)
		// 		{
		// 			((SchemaDataFieldDef<TE, bool>) data[key]).Value = 
		// 				exData.Entity.Get<bool>(fieldName);
		// 		}
		// 	}
		//
		// 	return ExStoreRtnCodes.XRC_GOOD;
		// }

		// public ExStoreRtnCodes CreateRootDataStor(Schema schema)
		// {
		// 	ExStoreRtnCodes result;
		//
		// 	DataStorage ds = null;
		// 	Entity e;
		//
		// 	result = dsMgr.CreateDataStorage(schema, out e, out ds);
		//
		// 	return ExStoreRtnCodes.XRC_GOOD;
		// }

		// public ExStoreRtnCodes WriteRootAppData(SchemaRootAppData raData, SchemaCellData cData,
		// 	DataStorage ds)
		// {
		// 	ExStoreRtnCodes result;
		//
		// 	result = writeRootAppData(raData, cData, ds);
		// 	if (result != ExStoreRtnCodes.XRC_GOOD) return result;
		//
		// 	return ExStoreRtnCodes.XRC_GOOD;
		// }

		// write the data to datastorage
		// datastorage must already exist

		//
		// 	ExStorageTests xsTest = new ExStorageTests();
		// 	private ExStoreHelper xsHlpr;
		//
		// 	// todo - move data stores to the datastoragemanager
		//
		// 	// private Entity[] dataStores = new Entity[APP_DATA_STORE + 1];
		//
		// 	private ExStoreRoot xRoot;
		// 	private ExStoreApp xApp;
		// 	private ExStoreCell xCell;
		//
		// 	public ExStoreRoot XRoot
		// 	{
		// 		get => xRoot;
		// 		set => xRoot = value;
		// 	}
		//
		// 	public ExStoreApp XApp
		// 	{
		// 		get => xApp;
		// 		set => xApp = value;
		// 	}
		//
		// 	public ExStoreCell XCell
		// 	{
		// 		get => xCell;
		// 		set => xCell = value;
		// 	}
		//
		//
		// #region initialize
		//
		// 	private void Initialize()
		// 	{
		// 		OpDescription = "Initialize ExStorage Manager";
		// 		if (Initialized) return;
		//
		// 		xsHlpr = new ExStoreHelper();
		//
		// 		resetExStore();
		// 		makeSchema();
		//
		// 		Initialized = true;
		// 	}
		//
		// 	private void resetExStore()
		// 	{
		// 		xRoot = ExStoreRoot.Instance();
		// 		xApp = ExStoreApp.Instance();
		// 		xCell = ExStoreCell.Instance();
		// 	}
		//
		// 	public void makeSchema()
		// 	{
		// 		Schema s;
		//
		// 		s = xsHlpr.MakeRootSchema(xRoot);
		//
		// 		DsMgr[ROOT_DATA_STORE].Schema = s;
		//
		//
		// 		// s = xsHlpr.MakeAppSchema(xApp, xCell);
		// 		// DsMgr[APP_DATA_STORE_CURR].Schema = s;
		// 		//
		// 		// DsMgr[APP_DATA_STORE_NEW].Schema = null;
		// 	}
		//
		// 	public bool Reset()
		// 	{
		// 		if (AppRibbon.App.Documents.Size > 1)
		// 		{
		// 			xsTest.taskDialogWarning_Ok("Cells",
		// 				"It is Unsafe to Reset Now", "A reset can only be preformed when a single document "
		// 				+ "is open because a reset would apply to all open documents");
		//
		// 			return false;
		// 		}
		//
		// 		Configured = false;
		//
		// 		DsMgr.Reset();
		//
		// 		// this must follow the reset else schema's don't exist
		// 		resetExStore();
		//
		// 		Initialized = true;
		//
		// 		return true;
		// 	}
		//
		// #endregion
		//
		// #region configure
		//
		// 	public void Configure()
		// 	{
		// 		OpDescription = "Configure ExStorage Manager";
		//
		// 		if (Configured) return;
		//
		// 		ExStoreRtnCodes result;
		//
		// 		// check if the root datastorage element exists
		// 		// in the database
		// 		// if not, create one using the default information
		// 		if (!DsMgr[ROOT_DATA_STORE].GotDataStorage)
		// 		{
		// 			AppRibbon.msg += "don't got xroot\n";
		// 			CheckRootDataStorExists();
		// 			if (!DsMgr[ROOT_DATA_STORE].GotDataStorage) return;
		// 		}
		//
		// 		AppRibbon.msg += "checking that root ex data exists\n";
		//
		// 		CheckRootExStorExists();
		//
		// 		if (!RootExStorExists) return;
		//
		// 		// check if the app datastorage element exists
		// 		// in the database
		// 		if (!DsMgr[APP_DATA_STORE_CURR].GotDataStorage)
		// 		{
		// 			AppRibbon.msg += "don't got xapp\n";
		// 			CheckAppDataStorExists();
		// 		}
		//
		// 		AppRibbon.msg += "checking that app ex data exists\n";
		// 		CheckAppExStorExists();
		//
		// 		if (!AppExStorExists) return;
		//
		// 		Configured = true;
		// 	}
		//
		// 	private void CheckRootExStorExists()
		// 	{
		// 		if (RootExStorExists) return;
		// 		if (!DsMgr[ROOT_DATA_STORE].GotDataStorage) return;
		//
		// 		ExStoreRtnCodes result = readRoot();
		//
		// 		if (result != ExStoreRtnCodes.XRC_GOOD) return;
		//
		// 		// RootExStorExists = true;
		// 	}
		//
		// 	private void CheckAppExStorExists()
		// 	{
		// 		if (!RootExStorExists) return;
		// 		if (AppExStorExists) return;
		// 		if (!DsMgr[ROOT_DATA_STORE].GotDataStorage) return;
		//
		// 		ExStoreRtnCodes result = readApp();
		//
		// 		if (result != ExStoreRtnCodes.XRC_GOOD) return;
		//
		// 		// AppExStorExists = true;
		// 	}
		//
		// 	public void CheckRootDataStorExists()
		// 	{
		// 		if (!IsInit("Check data Store exists"))  return;
		// 		if (Configured)  return;
		// 		if (DsMgr[ROOT_DATA_STORE].GotDataStorage) return;
		//
		// 		Entity e;
		// 		DataStorage ds;
		//
		// 		bool result = xsHlpr.GetRootEntity(xRoot, out e, out ds);
		//
		// 		if (result)
		// 		{
		// 			DsMgr[ROOT_DATA_STORE].DataStorage = ds;
		// 			// DsMgr[ROOT_DATA_STORE].Entity = e;
		// 			// DsMgr[ROOT_DATA_STORE].Schema = s;
		// 		}
		// 	}
		//
		// 	public void CheckAppDataStorExists()
		// 	{
		// 		if (DsMgr[APP_DATA_STORE_CURR].GotDataStorage) return;
		//
		// 		// ExStoreApp xApp = ExStoreApp.Instance();
		//
		// 		Entity e;
		// 		DataStorage ds;
		//
		// 		bool result = xsHlpr.GetAppEntity(xApp, xCell, out e, out ds);
		//
		// 		if (result)
		// 		{
		// 			DsMgr[APP_DATA_STORE_CURR].DataStorage = ds;
		// 			// DsMgr[APP_DATA_STORE].Entity = e;
		// 		}
		// 	}
		//
		// 	private ExStoreRtnCodes CreateRootDataStore()
		// 	{
		// 		ExStoreRtnCodes result;
		//
		// 		result = DsMgr.CreateDataStorage(ROOT_DATA_STORE);
		// 		if (result != ExStoreRtnCodes.XRC_GOOD) return result;
		//
		// 		result = WriteRoot();
		// 		if (result != ExStoreRtnCodes.XRC_GOOD) return result;
		//
		// 		return ExStoreRtnCodes.XRC_GOOD;
		// 	}
		//
		// 	private ExStoreRtnCodes CreateAppDataStore()
		// 	{
		// 		ExStoreRtnCodes result;
		//
		// 		result = DsMgr.CreateDataStorage(APP_DATA_STORE_CURR);
		// 		if (result != ExStoreRtnCodes.XRC_GOOD) return result;
		//
		// 		result = WriteAppAndCells();
		// 		if (result != ExStoreRtnCodes.XRC_GOOD) return result;
		//
		// 		return ExStoreRtnCodes.XRC_GOOD;
		// 	}
		//
		// #endregion
		//
		// #region create
		//
		// 	public ExStoreRtnCodes MakeRoot()
		// 	{
		// 		OpDescription = "Create root datastore";
		// 		if (!IsInit(OpDescription))  return ExStoreRtnCodes.XRC_NOT_INIT;
		// 		if (Configured) return ExStoreRtnCodes.XRC_IS_CONFIG;
		// 		if (DsMgr[ROOT_DATA_STORE].GotDataStorage) return ExStoreRtnCodes.XRC_DS_EXISTS;
		//
		// 		ExStoreRtnCodes result;
		//
		// 		// result = CreateRootDataStore();
		// 		// if (result != ExStoreRtnCodes.XRC_GOOD) return result;
		//
		// 		result = DsMgr.CreateDataStorage(ROOT_DATA_STORE);
		// 		if (result != ExStoreRtnCodes.XRC_GOOD) return result;
		//
		// 		result = WriteRoot();
		// 		if (result != ExStoreRtnCodes.XRC_GOOD) return result;
		//
		// 		// Configure();
		//
		// 		return ExStoreRtnCodes.XRC_GOOD;
		// 	}
		//
		// 	public ExStoreRtnCodes MakeApp()
		// 	{
		// 		OpDescription = "Create app datastore";
		// 		if (!IsInit(OpDescription))  return ExStoreRtnCodes.XRC_NOT_INIT;
		// 		if (Configured) return ExStoreRtnCodes.XRC_IS_CONFIG;
		// 		if (!DsMgr[ROOT_DATA_STORE].GotDataStorage) return ExStoreRtnCodes.XRC_DS_NOT_EXIST;
		// 		if (DsMgr[APP_DATA_STORE_CURR].GotDataStorage) return ExStoreRtnCodes.XRC_DS_EXISTS;
		//
		// 		ExStoreRtnCodes result;
		//
		// 		// result = CreateAppDataStore();
		// 		// if (result != ExStoreRtnCodes.XRC_GOOD) return result;
		//
		// 		result = DsMgr.CreateDataStorage(APP_DATA_STORE_CURR);
		// 		if (result != ExStoreRtnCodes.XRC_GOOD) return result;
		//
		// 		result = WriteAppAndCells();
		// 		if (result != ExStoreRtnCodes.XRC_GOOD) return result;
		//
		// 		// Configure();
		//
		// 		return ExStoreRtnCodes.XRC_GOOD;
		// 	}
		//
		// 	/// <summary>
		// 	/// create the datastore that the ex data is attached to <br/>
		// 	/// 
		// 	/// </summary>
		// 	/// <param name="xRoot"></param>
		// 	/// <returns></returns>
		// 	public ExStoreRtnCodes CreateRoot()
		// 	{
		// 		OpDescription = "Create root datastore";
		// 		if (!IsInit(OpDescription))  return ExStoreRtnCodes.XRC_NOT_INIT;
		// 		if (Configured) return ExStoreRtnCodes.XRC_IS_CONFIG;
		// 		if (DsMgr[ROOT_DATA_STORE].GotDataStorage) return ExStoreRtnCodes.XRC_DS_EXISTS;
		//
		// 		ExStoreRtnCodes result;
		// 		Entity e = null;
		//
		// 		Schema sRoot = xsHlpr.GetRootSchema();
		// 		//MakeRootSchema(xRoot);
		//
		// 		result = DsMgr.CreateDataStorage(sRoot, out e);
		//
		// 		DsMgr[ROOT_DATA_STORE].Entity = e;
		//
		// 		if (result != ExStoreRtnCodes.XRC_GOOD) return result;
		//
		// 		result = WriteRoot();
		// 		if (result != ExStoreRtnCodes.XRC_GOOD) return result;
		//
		// 		Configure();
		//
		// 		return ExStoreRtnCodes.XRC_GOOD;
		// 	}
		//
		// 	public ExStoreRtnCodes CreateAppDs()
		// 	{
		// 		OpDescription = "Create app datastore";
		// 		if (!IsInit(OpDescription))  return ExStoreRtnCodes.XRC_NOT_INIT;
		// 		if (Configured) return ExStoreRtnCodes.XRC_IS_CONFIG;
		// 		if (!DsMgr[ROOT_DATA_STORE].GotDataStorage) return ExStoreRtnCodes.XRC_DS_NOT_EXIST;
		// 		if (DsMgr[APP_DATA_STORE_CURR].GotDataStorage) return ExStoreRtnCodes.XRC_DS_EXISTS;
		//
		// 		ExStoreRtnCodes result;
		// 		Entity e = null;
		//
		// 		Schema sApp = xsHlpr.MakeAppSchema(xApp, xCell);
		// 		// GetAppSchemaCurr();
		//
		// 		result = DsMgr.CreateDataStorage(sApp, out e);
		//
		// 		DsMgr[APP_DATA_STORE_CURR].Entity = e;
		//
		// 		if (result != ExStoreRtnCodes.XRC_GOOD) return result;
		//
		// 		Configure();
		//
		// 		return ExStoreRtnCodes.XRC_GOOD;
		// 	}
		//
		// #endregion
		//
		// #region read
		//
		// 	private ExStoreRtnCodes ReadRoot()
		// 	{
		// 		OpDescription = "Read Root Data";
		// 		if (!DsMgr[ROOT_DATA_STORE].GotDataStorage) return ExStoreRtnCodes.XRC_DS_NOT_EXIST;
		// 		// if (!IsInit(OpDescription))  return ExStoreRtnCodes.NOT_INIT;
		// 		// if (!IsConfig(OpDescription))  return ExStoreRtnCodes.NOT_CONFIG;
		//
		// 		ExStoreRtnCodes result = readRoot();
		// 		if (result != ExStoreRtnCodes.XRC_GOOD) return result;
		//
		// 		return ExStoreRtnCodes.XRC_GOOD;
		// 	}
		//
		// 	private ExStoreRtnCodes ReadApp()
		// 	{
		// 		OpDescription = "Read App Data";
		// 		if (!RootExStorExists) return ExStoreRtnCodes.XRC_ROOT_NOT_EXIST;
		//
		// 		ExStoreRtnCodes result = readApp();
		// 		if (result != ExStoreRtnCodes.XRC_GOOD) return result;
		//
		// 		xApp = this.XApp;
		//
		// 		return ExStoreRtnCodes.XRC_GOOD;
		// 	}
		//
		// 	private ExStoreRtnCodes ReadCells()
		// 	{
		// 		OpDescription = "Read Cells Data";
		// 		if (!RootExStorExists) return ExStoreRtnCodes.XRC_ROOT_NOT_EXIST;
		// 		if (!AppExStorExists) return ExStoreRtnCodes.XRC_APP_NOT_EXIST;
		//
		// 		// this.xCell = ExStoreCell.Instance(0);
		//
		// 		ExStoreRtnCodes result = xsHlpr.ReadCellData(ref this.xCell);
		// 		if (result != ExStoreRtnCodes.XRC_GOOD) return result;
		//
		// 		xCell = XCell;
		//
		// 		return ExStoreRtnCodes.XRC_GOOD;
		// 	}
		//
		// #endregion
		//
		// #region write
		//
		// 	// public ExStoreRtnCodes WriteRoot(ExStoreRoot xRoot)
		// 	public ExStoreRtnCodes WriteRoot()
		// 	{
		// 		OpDescription = "Write Root Data";
		// 		if (!DsMgr[ROOT_DATA_STORE].GotDataStorage)
		// 			return ExStoreRtnCodes.XRC_DS_NOT_EXIST;
		// 		// if (!IsInit("Save App Data"))  return ExStoreRtnCodes.NOT_INIT;
		// 		// if (RootExStorExists) return ExStoreRtnCodes.ROOT_NOT_EXIST;
		//
		// 		// ExStoreRtnCodes result = xsHlpr.WriteRootData(xRoot, DsMgr.DataStoreEntity[ROOT_DATA_STORE]);
		// 		ExStoreRtnCodes result = xsHlpr.WriteRootData(xRoot,
		// 			DsMgr[ROOT_DATA_STORE].DataStorage);
		//
		// 		// RootExStorExists = true;
		//
		// 		// this.xRoot = xRoot;
		//
		// 		return result;
		// 	}
		//
		// 	public ExStoreRtnCodes WriteAppAndCells( /*ExStoreApp xApp, ExStoreCell xCell*/)
		// 	{
		// 		OpDescription = "Write App and Cell Data";
		// 		if (!DsMgr[APP_DATA_STORE_CURR].GotDataStorage)
		// 			return ExStoreRtnCodes.XRC_ROOT_NOT_EXIST;
		//
		// 		ExStoreRtnCodes result = xsHlpr.WriteAppAndCellsData(xApp, xCell,
		// 			DsMgr[APP_DATA_STORE_CURR].DataStorage);
		//
		// 		return result;
		// 	}
		//
		// #endregion
		//
		// #region update
		//
		// 	public ExStoreRtnCodes UpdateApp()
		// 	{
		// 		ExStoreRtnCodes result;
		// 		if (!AppExStorExists) return ExStoreRtnCodes.XRC_APP_NOT_EXIST;
		//
		// 		result = DeleteApp();
		// 		if (result != ExStoreRtnCodes.XRC_GOOD) return result;
		//
		// 		// write the modified app info and the current cell info
		// 		WriteAppAndCells( /*xApp, XCell*/);
		//
		// 		return result;
		// 	}
		//
		// 	public ExStoreRtnCodes UpdateCells()
		// 	{
		// 		if (!AppExStorExists) return ExStoreRtnCodes.XRC_APP_NOT_EXIST;
		//
		// 		ExStoreRtnCodes result;
		//
		// 		result = xsHlpr.UpdateCellData(XApp, xCell, DsMgr[APP_DATA_STORE_CURR].DataStorage);
		//
		// 		return result;
		// 	}
		//
		// #endregion
		//
		// #region delete
		//
		// 	public ExStoreRtnCodes DeleteDs()
		// 	{
		// 		return ExStoreRtnCodes.XRC_GOOD;
		// 	}
		//
		// 	public ExStoreRtnCodes DeleteRoot()
		// 	{
		// 		OpDescription = "Delete Root Schema";
		// 		if (!RootExStorExists) return ExStoreRtnCodes.XRC_ROOT_NOT_EXIST;
		// 		if (AppExStorExists) return ExStoreRtnCodes.XRC_APP_NOT_EXIST;
		//
		// 		ExStoreRtnCodes result = xsHlpr.DeleteRootSchema();
		//
		// 		if (result != ExStoreRtnCodes.XRC_GOOD) return result;
		//
		// 		// RootExStorExists = false;
		//
		// 		resetExStore();
		//
		// 		return ExStoreRtnCodes.XRC_GOOD;
		// 	}
		//
		// 	public ExStoreRtnCodes DeleteApp()
		// 	{
		// 		OpDescription = "Delete App Schema";
		// 		if (!AppExStorExists) return ExStoreRtnCodes.XRC_APP_NOT_EXIST;
		//
		// 		ExStoreRtnCodes result = xsHlpr.DeleteAppSchema();
		//
		// 		if (result != ExStoreRtnCodes.XRC_GOOD) return result;
		//
		// 		return ExStoreRtnCodes.XRC_GOOD;
		// 	}
		//
		// #endregion
		//
		//
		//
		// 	// public Schema MakeRootSchema(ExStoreRoot xRoot)
		// 	// {
		// 	// 	return xsHlpr.makeRootSchema(xRoot);
		// 	// }
		//
		// 	// public Schema MakeAppSchema(ExStoreApp xApp, ExStoreCell xCell)
		// 	// {
		// 	// 	return xsHlpr.makeAppSchema(xApp, xCell);
		// 	// }
		//
		//
		// 	private ExStoreRtnCodes readRoot()
		// 	{
		// 		ExStoreRtnCodes result = xsHlpr.ReadRootData(ref this.xRoot);
		// 		if (result != ExStoreRtnCodes.XRC_GOOD) return result;
		//
		// 		xRoot.IsDefault = false;
		// 		SchemaGuidManager.AppGuidString = XRoot.Data[SchemaRootKey.RK_APP_GUID].Value;
		//
		// 		return ExStoreRtnCodes.XRC_GOOD;
		// 	}
		//
		// 	private ExStoreRtnCodes readApp()
		// 	{
		// 		ExStoreRtnCodes result = xsHlpr.ReadAppData(ref this.xApp);
		//
		// 		if (result != ExStoreRtnCodes.XRC_GOOD) return result;
		//
		// 		xApp.IsDefault = false;
		// 		xCell.IsDefault = false;
		//
		// 		return result;
		// 	}
		//
		// #region support methods
		//
		// 	private bool IsInit(string desc)
		// 	{
		// 		if (Initialized) return true;
		//
		// 		NotInitFail(desc, "Manager is not initialized");
		//
		// 		return false;
		// 	}
		//
		// 	private bool IsConfig(string desc)
		// 	{
		// 		if (Configured) return true;
		//
		// 		NotInitFail(desc, "Manager is not configured");
		//
		// 		return false;
		// 	}
		//
		// 	private void NotInitFail(string desc, string why)
		// 	{
		// 		TaskDialog td = new TaskDialog("AO Tools - " + desc);
		// 		td.MainIcon = TaskDialogIcon.TaskDialogIconWarning;
		// 		td.MainInstruction = desc + " - failed";
		// 		td.MainContent = why;
		// 		td.Show();
		// 	}
		//
		// 	public void DeleteSchemaFail(string desc)
		// 	{
		// 		TaskDialog td = new TaskDialog("AO Tools - " + desc);
		// 		td.MainIcon = TaskDialogIcon.TaskDialogIconWarning;
		// 		td.MainInstruction = desc + " - failed";
		// 		td.Show();
		// 	}
		//
		// 	public void ReadSchemaFail(string desc)
		// 	{
		// 		TaskDialog td = new TaskDialog("AO Tools - " + desc);
		// 		td.MainIcon = TaskDialogIcon.TaskDialogIconWarning;
		// 		td.MainInstruction = desc + " - failed";
		// 		td.Show();
		// 	}
		//
		// 	public void ExStoreFail(string descTitle, string descMsg, string errorMsg)
		// 	{
		// 		TaskDialog td = new TaskDialog("AO Tools - " + descTitle);
		// 		td.MainIcon = TaskDialogIcon.TaskDialogIconWarning;
		// 		td.MainInstruction =
		// 			"Process Failed| " + descMsg + "\n"
		// 			+ "Error| " + errorMsg;
		// 		td.Show();
		// 	}
		//
		// #endregion
		//
	}
}