#region using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using AOTools.Cells.ExStorage;
using AOTools.Cells.Tests;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using static AOTools.Cells.ExDataStorage.DataStoreIdx;

#endregion


// projname: $projectname$
// itemname: DataStorageManager
// username: jeffs
// created:  8/15/2021 6:39:48 PM

namespace AOTools.Cells.ExDataStorage
{
	public enum DataStoreIdx
	{
		ROOT_DATA_STORE = 0,
		APP_DATA_STORE_CURR = 1,
		APP_DATA_STORE_NEW = 2
	}


	public class DataStorageManager
	{
		private readonly int dataStorCount = (int) APP_DATA_STORE_NEW + 1;

		public class DataStor
		{
			public Entity Entity { get; set; }
			public Schema Schema { get; 
				set; }
			public DataStorage DataStorage { get; set; }
			public string Name { get; }

			public DataStor(Entity entity, Schema schema,
				DataStorage dataStorage, string name)
			{
				Entity = entity;
				Schema = schema;
				DataStorage = dataStorage;
				Name = name;
			}

			public DataStor(string name)
			{
				Entity = null;
				Schema = null;
				DataStorage = null;
				Name = name;
			}

			// public bool GotEntity => Entity != null && Entity.IsValid();
			// public bool GotSchema => Schema != null;
			public bool GotDataStorage => DataStorage != null;
		}


	#region private fields

		private readonly string[] DATA_STORE_NAMES =
			{"Cells│ Root Data Storage", "Cells│ App Data Storage", "Cells│ Cell Info not used"};

		private static readonly Lazy<DataStorageManager> instance =
			new Lazy<DataStorageManager>(() => new DataStorageManager());

		private Document doc = AppRibbon.Doc;

		private DataStor[] DataStore;

		// private Entity[] dataStoreEntity;
		// private bool[] dataStoreExists;
		// private DataStorage[] dataStorage;

	#endregion

	#region ctor

		private DataStorageManager()
		{
			DataStore = new DataStor[dataStorCount];

			Reset();

			// dataStoreEntity = new Entity[(int) APP_DATA_STORE + 1];
			// dataStorage = new DataStorage[(int) APP_DATA_STORE + 1];

			// Entity e = DataStore[APP_DATA_STORE];
		}

	#endregion

	#region public properties

		public static DataStorageManager DsMgr => instance.Value;

		public DataStor this[DataStoreIdx idx]
		{
			get => DataStore[(int) idx];
		}

	#endregion

	#region private properties

	#endregion

	#region public methods

		/// <summary>
		/// Wipe out datastorage references.
		/// </summary>
		public void Reset()
		{
			for (int i = 0; i < dataStorCount; i++)
			{
				DataStore[i] = new DataStor(DATA_STORE_NAMES[i]);
			}
		}

		/// <summary>
		/// Wipe out data storage references and delete the datastorage elements.
		/// </summary>
		public void Restart()
		{
			for (int i = 0; i < dataStorCount; i++)
			{
				if (DataStore[i] != null && DataStore[i].GotDataStorage)
				{
					doc.Delete(DataStore[i].DataStorage.Id);
				}

				DataStore[i] = new DataStor(DATA_STORE_NAMES[i]);
			}
		}

		/// <summary>
		/// create a datastorage element but do not assign an entity
		/// </summary>
		/// <param name="idx"></param>
		/// <returns></returns>
		public ExStoreRtnCodes CreateDataStorage(DataStoreIdx idx)
		{
			try
			{
				DataStorage ds = DataStorage.Create(doc);
				ds.Name = DataStore[(int) idx].Name;
				DataStore[(int) idx].DataStorage = ds;
			}
			catch
			{
				return ExStoreRtnCodes.XRC_FAIL;
			}

			return ExStoreRtnCodes.XRC_GOOD;
		}

		/// <summary>
		/// create a datastorage element and assign the entity
		/// </summary>
		/// <param name="schema"></param>
		/// <param name="e"></param>
		/// <returns></returns>
		public ExStoreRtnCodes CreateDataStorage(Schema schema, out Entity e)
		{
			e = null;
			Transaction t = null;

			try
			{
				DataStorage ds = DataStorage.Create(doc);

				e = new Entity(schema);

				ds.SetEntity(e);
			}
			catch
			{
				return ExStoreRtnCodes.XRC_FAIL;
			}

			return ExStoreRtnCodes.XRC_GOOD;
		}

		/// <summary>
		/// find an existing datastorage element
		/// </summary>
		/// <param name="schema"></param>
		/// <param name="ex"></param>
		/// <param name="dx"></param>
		/// <returns></returns>
		public bool GetDataStorage(Schema schema, out Entity ex, out DataStorage dx)
		{
			ExStorageTests.location = 360;
			ex = null;
			dx = null;

			FilteredElementCollector collector = new FilteredElementCollector(AppRibbon.Doc);

			FilteredElementCollector dataStorages =
				collector.OfClass(typeof(DataStorage));

			if (dataStorages == null) return false;

			foreach (DataStorage ds in dataStorages)
			{
				Entity e = ds.GetEntity(schema);

				if (!e.IsValid()) continue;

				ex = e;
				dx = ds;
				ExStorageTests.location = 367;
				return true;
			}

			ExStorageTests.location = 369;
			return false;
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