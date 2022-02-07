using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
// using CSToolsStudies.ExStorage.Management;
// using CSToolsStudies.Fields.ExStorage.ExStorageData;
// using CSToolsStudies.Fields.ExStorage.ExStorManagement;
// using CSToolsStudies.Fields.FieldsManagement;
// using CSToolsStudies.Fields.SchemaInfo.SchemaData;
// using CSToolsStudies.Fields.SchemaInfo.SchemaDefinitions;
// using CSToolsStudies.Fields.Testing;

using SharedCode.Windows;

namespace CSToolsStudies.Windows
{

	/*
	 * my concept
	 * MainWindow is the conductor that collects and directs the tasks
	 * in general, it does not perform any tasks
	 *
	 * FieldsManager
	 * is the primary process manager.  this will have a collection of
	 * processes that can be performed as directed by MainWindow
	 * e.g StartProcess (e.g. proc00)
	 *
	 * ExStoreManager
	 * is the primary action / task performer / manager
	 * this will have the individual task components (procedures / methods) that are
	 * needed by the task manager.
	 * e.g. get data on startup (e.g. proc01)
	 * e.g. check lock status (e.g. procLx401)
	 *
	 * ExStoreManager will utilize the sub-managers / data objects
	 * SchemaManager
	 * ExStorData
	 * DataStoreManager
	 *
	 * to perform its tasks
	 *
	 */



	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : AWindow, INotifyPropertyChanged
	{
	#region private fields

		public enum DataStorType
		{
			DT_ROOT,
			DT_CELL,
			DT_LOCK
		}

		private FieldsManager2 fm2;
		private ShowInfo show;

		// original
		// private ExTests01 et;
		// private ExStorData exData;
		// private SchemaRootData raData;
		// private SchemaCellData cData;
		// /// <inheritdoc cref="FieldsManager"/>
		// private FieldsManager fm;
		private static string docName;
		private string dsKey;
		private DataStorType dsType;

		// generic
		private Document doc;

		private string myName = nameof(MainWindow);

		private string textMsg01;

		private int marginSize = 0;

		private int marginSpaceSize = 2;

		private string location;


	#endregion

	#region ctor

		public MainWindow(Document doc)
		{
			ExStoreRtnCodes result;

			InitializeComponent();

			this.doc = doc;
			docName = doc.Title;

			fm2 = new FieldsManager2(this, doc);
			show = new ShowInfo(this);
/*
			et = new ExTests01(this, doc);
			exData = ExStorData.Instance;

			DsKey = ExStorData.MakeKey(docName);
			exData.DsKey = dsKey;
*/
			DsKey = fm2.DsKey;
			DsType = DataStorType.DT_ROOT;
		}

	#endregion

	#region public properties

		public static string DocName => docName;

		public string DsKey
		{
			get => dsKey ?? "undefined";
			set
			{
				dsKey = value;
				OnPropertyChanged();
			}
		}

		public DataStorType DsType
		{
			get => dsType;
			set
			{
				dsType = value;
				OnPropertyChanged();
			}
		}

	#endregion

	#region private properties

	#endregion

	#region public methods

	#endregion

	#region private methods

		// initial starting point when window opens
		private bool startProcess()
		{
			ExStoreRtnCodes result;
			// result = fm.proc00();

			// if (result != ExStoreRtnCodes.XRC_GOOD) return false;

			WriteLineAligned("start process", "good");

			Show();

			return true;
		}

	#endregion

	#region event consuming

		private void MainWindow_Loaded(object sender, RoutedEventArgs e)
		{
			bool result = startProcess();

			if (!result)
			{
				DialogResult = false;
				Close();
			}
		}

	#endregion

	#region event publishing

		public event PropertyChangedEventHandler PropertyChanged;

		protected override void OnPropertyChanged([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

	#endregion

	#region system overrides

		public string ToString()
		{
			return $"this is| {myName}" ;
		}

	#endregion

	#region button event consumers

		private void BtnExit_OnClick(object sender, RoutedEventArgs e)
		{
			Close();
		}

	#endregion

	#region new - debug methods

		private void BtnSetRoot_OnClick(object sender, RoutedEventArgs e)
		{
			DsType = DataStorType.DT_ROOT;
		}
		
		private void BtnSetCell_OnClick(object sender, RoutedEventArgs e)
		{
			DsType = DataStorType.DT_CELL;
		}
		
		private void BtnSetLock_OnClick(object sender, RoutedEventArgs e)
		{
			DsType = DataStorType.DT_LOCK;
		}

		private void BtnMakeDs_OnClick(object sender, RoutedEventArgs e)
		{

		}
		
		private void BtnFindDs_OnClick(object sender, RoutedEventArgs e)
		{

		}



		// show info
		private void BtnShowData_OnClick(object sender, RoutedEventArgs e)
		{
			this.MsgClr();


			switch (DsType)
			{
			case DataStorType.DT_ROOT:
				{
					show.ShowDataGeneric(fm2.rtData);
					break;
				}
			case DataStorType.DT_CELL:
				{
					show.ShowDataGeneric(fm2.clData);
					break;
				}
			case DataStorType.DT_LOCK:
				{
					show.ShowDataGeneric(fm2.lkData);
					break;
				}
			}

		}


	#endregion

	#region debug method

		private void BtnDebug_OnClick(object sender, RoutedEventArgs e)
		{
			Debug.WriteLine("@debug");
		}

	#endregion

	#region show - debug methods

	#endregion

	#region old - debug methods
		//
		// // "old"
		//
		// // show
		//
		// private void BtnShowRootData_OnClick(object sender, RoutedEventArgs e)
		// {
		// 	fm.ShowRootData();
		// }
		//
		// private void BtnShowRootFields_OnClick(object sender, RoutedEventArgs e)
		// {
		// 	fm.ShowRootFields();
		// }
		//
		// private void BtnShowCellFields_OnClick(object sender, RoutedEventArgs e)
		// {
		// 	fm.ShowCellFields();
		// }
		//
		// private void BtnShowCellData_OnClick(object sender, RoutedEventArgs e)
		// {
		// 	fm.ShowCellData();
		// }
		//
		// private void BtnShowLockFields_OnClick(object sender, RoutedEventArgs e)
		// {
		// 	fm.ShowLockFields();
		// }
		//
		// private void BtnShowLockData_OnClick(object sender, RoutedEventArgs e)
		// {
		// 	fm.ShowLockData();
		// }
		//
		// private void BtnStartProcess_OnClick(object sender, RoutedEventArgs e)
		// {
		//
		// 	using (Transaction T = new Transaction(AppRibbon.Doc, "fields| Read"))
		// 	{
		// 		T.Start();
		//
		// 		ExStoreRtnCodes result = et.StartProcess(exData.DsKey);
		//
		// 		if (result == ExStoreRtnCodes.XRC_GOOD)
		// 		{
		// 			T.Commit();
		// 			WriteLineAligned($"read| ");
		// 			WriteLineAligned($"date| {exData.RootData.GetValue<string>(SchemaRootKey.RK_CREATE_DATE)}");
		// 		}
		// 		else
		// 		{
		// 			T.RollBack();
		//
		// 			WriteLineAligned($"not read| {result.ToString()}");
		// 		}
		// 	}
		//
		// 	ShowMsg();
		// }
		//
		// private void BtnDeleteRoot_OnClick(object sender, RoutedEventArgs e)
		// {
		// 	using (Transaction T = new Transaction(AppRibbon.Doc, "fields"))
		// 	{
		// 		T.Start();
		//
		// 		ExStoreRtnCodes result = fm.DeleteRoot(exData.DsKey);
		//
		// 		if (result == ExStoreRtnCodes.XRC_GOOD)
		// 		{
		// 			T.Commit();
		// 			WriteLineAligned($"erased| ");
		// 		}
		// 		else
		// 		{
		// 			T.RollBack();
		//
		// 			WriteLineAligned($"entity NOT found| ");
		// 		}
		// 	}
		//
		// 	ShowMsg();
		// }
		//
		// private void BtnWriteRoot_OnClick(object sender, RoutedEventArgs e)
		// {
		// 	// test only
		// 	// et.testNames();
		//
		//
		// 	ExStoreRtnCodes result;
		//
		// 	// bogus data for testing
		// 	raData = new SchemaRootData();
		// 	raData.Configure("Root_App_Data_Name", "Root-App Data Description");
		// 	// raData.DocumentName = docName;
		// 	raData.DsKey = exData.DsKey;
		//
		// 	cData = new SchemaCellData();
		// 	cData.Configure("Cell_Data_Name", "A1",
		// 		UpdateRules.UR_AS_NEEDED, "cell Family", false, "xl file path", "worksheet name");
		// 	cData.DocumentName = docName;
		// 	cData.DsKey = exData.DsKey;
		//
		//
		// 	result = fm.DataStorExist(exData.DsKey);
		// 	if (result == ExStoreRtnCodes.XRC_DS_NOT_FOUND)
		// 	{
		// 		result = fm.CreateDataStorage(exData.DsKey);
		// 		if (result != ExStoreRtnCodes.XRC_GOOD) return;
		// 	}
		//
		// 	result = fm.WriteRoot(raData, cData);
		//
		// 	if (result == ExStoreRtnCodes.XRC_GOOD)
		// 	{
		// 		WriteLineAligned($"Data storage made and written!\n");
		// 	}
		// 	else
		// 	{
		// 		WriteLineAligned($"Data Storage failed!\n");
		// 	}
		//
		// 	ShowMsg();
		// }
		//
		// private void BtnFindRoot_OnClick(object sender, RoutedEventArgs e)
		// {
		// 	// get the entity by using ELEMENT.GetEntity(Schema) - 
		// 	// per the below, the DataStorage is the element 
		// 	// asuming that a single schema / element get found
		//
		// 	WriteMsg("\n");
		//
		// 	IList<Schema> schemas;
		// 	IList<DataStorage> dx;
		//
		// 	bool result = fm.GetAppSchemas(exData.DsKey, out schemas);
		//
		// 	WriteLineAligned($"schema (in memory) found?| {result.ToString()}", $"quantity| {schemas.Count}");
		//
		// 	if (schemas.Count > 0)
		// 	{
		// 		WriteMsg("\n");
		//
		// 		foreach (Schema s in schemas)
		// 		{
		// 			WriteLineAligned($"schema info| {s.SchemaName}  {s.VendorId}  {s.Documentation}");
		// 		}
		// 	}
		//
		// 	ExStoreStartRtnCodes answer = fm.GetRootDataStorages(exData.DsKey, out dx);
		//
		// 	WriteMsg("\n");
		// 	WriteLineAligned($"datastorage found?| {answer.ToString()}", $"quantity| {dx.Count}");
		//
		// 	if (dx.Count > 0)
		// 	{
		// 		WriteMsg("\n");
		//
		// 		foreach (DataStorage ds in dx)
		// 		{
		// 			WriteLineAligned($"datastorage info| {ds.Name}", $"valid?| {ds.IsValidObject}");
		// 		}
		// 	}
		//
		// 	ShowMsg();
		// }
		//
		// // probably n/a
		// private void BtnFindRootDs_OnClick(object sender, RoutedEventArgs e)
		// {
		// 	ExStoreRtnCodes result;
		//
		// 	result = fm.FindRootDS();
		//
		// 	WriteLine("find root DS|", result.ToString());
		// 	ShowMsg();
		// }
		//

	#endregion

	}
}