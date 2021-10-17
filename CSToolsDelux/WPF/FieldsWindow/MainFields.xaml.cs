using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using CSToolsDelux.Fields.ExStorage.ExStorageData;
using CSToolsDelux.Fields.ExStorage.ExStorManagement;
using CSToolsDelux.Fields.FieldsManagement;
using CSToolsDelux.Fields.SchemaInfo.SchemaData;
using CSToolsDelux.Fields.SchemaInfo.SchemaDefinitions;
using CSToolsDelux.Fields.Testing;

namespace CSToolsDelux.WPF.FieldsWindow
{

	/*
	 * my concept
	 * MainFields is the conductor that collects and directs the tasks
	 * in general, it does not perform any tasks
	 *
	 * FieldsManager
	 * is the primary process manager.  this will have a collection of
	 * processes that can be performed as directed by MainFields
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
	/// Interaction logic for MainFields.xaml
	/// </summary>
	public partial class MainFields : AWindow, INotifyPropertyChanged
	{
	#region private fields

		private ExTests01 et;
		private ExStorData exData;
		private SchemaRootData raData;
		private SchemaCellData cData;

		private Document doc;

		/// <inheritdoc cref="FieldsManager"/>
		private FieldsManager fm;

		private string myName = nameof(MainFields);
		private string textMsg01;

		private int marginSize = 0;

		private int marginSpaceSize = 2;

		private string location;

		private static string docName;
		private string docKey;

	#endregion

	#region ctor

		public MainFields(Document doc)
		{
			this.doc = doc;

			InitializeComponent();

			fm = new FieldsManager(this, doc);
			et = new ExTests01(this, doc);
			exData = ExStorData.Instance;

			docName = doc.Title;

			// temp to just creata a bogus old DS entry and name
			// docName = "HasDataStorage X";

			// save a local copy
			DocKey = ExStorData.MakeKey(docName);
			exData.DocKey = docKey;
		}

	#endregion

	#region public properties

		public static string DocName => docName;

		public string DocKey
		{
			get => docKey ?? "undefined";
			set
			{
				docKey = value;
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

		private void MainFields_Loaded(object sender, RoutedEventArgs e)
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

		}
		
		private void BtnSetCell_OnClick(object sender, RoutedEventArgs e)
		{

		}
		
		private void BtnSetLock_OnClick(object sender, RoutedEventArgs e)
		{

		}

		private void BtnMakeDs_OnClick(object sender, RoutedEventArgs e)
		{

		}
		
		private void BtnFindDs_OnClick(object sender, RoutedEventArgs e)
		{

		}


	#endregion

	#region debug method

		private void BtnDebug_OnClick(object sender, RoutedEventArgs e)
		{
			Debug.WriteLine("@debug");
		}

	#endregion

	#region show - debug methods

		// show

		private void BtnShowRootData_OnClick(object sender, RoutedEventArgs e)
		{
			fm.ShowRootData();
		}

		private void BtnShowRootFields_OnClick(object sender, RoutedEventArgs e)
		{
			fm.ShowRootFields();
		}

		private void BtnShowCellFields_OnClick(object sender, RoutedEventArgs e)
		{
			fm.ShowCellFields();
		}

		private void BtnShowCellData_OnClick(object sender, RoutedEventArgs e)
		{
			fm.ShowCellData();
		}

		private void BtnShowLockFields_OnClick(object sender, RoutedEventArgs e)
		{
			fm.ShowLockFields();
		}

		private void BtnShowLockData_OnClick(object sender, RoutedEventArgs e)
		{
			fm.ShowLockData();
		}

	#endregion

	#region old - debug methods

		// "old"

		private void BtnStartProcess_OnClick(object sender, RoutedEventArgs e)
		{

			using (Transaction T = new Transaction(AppRibbon.Doc, "fields| Read"))
			{
				T.Start();

				ExStoreRtnCodes result = et.StartProcess(exData.DocKey);

				if (result == ExStoreRtnCodes.XRC_GOOD)
				{
					T.Commit();
					WriteLineAligned($"read| ");
					WriteLineAligned($"date| {exData.RootData.GetValue<string>(SchemaRootKey.RK_CREATE_DATE)}");
				}
				else
				{
					T.RollBack();

					WriteLineAligned($"not read| {result.ToString()}");
				}
			}

			ShowMsg();
		}

		private void BtnDeleteRoot_OnClick(object sender, RoutedEventArgs e)
		{
			using (Transaction T = new Transaction(AppRibbon.Doc, "fields"))
			{
				T.Start();

				ExStoreRtnCodes result = fm.DeleteRoot(exData.DocKey);

				if (result == ExStoreRtnCodes.XRC_GOOD)
				{
					T.Commit();
					WriteLineAligned($"erased| ");
				}
				else
				{
					T.RollBack();

					WriteLineAligned($"entity NOT found| ");
				}
			}

			ShowMsg();
		}

		private void BtnWriteRoot_OnClick(object sender, RoutedEventArgs e)
		{
			// test only
			// et.testNames();


			ExStoreRtnCodes result;

			// bogus data for testing
			raData = new SchemaRootData();
			raData.Configure("Root_App_Data_Name", "Root-App Data Description");
			raData.DocumentName = docName;
			raData.DocKey = exData.DocKey;

			cData = new SchemaCellData();
			cData.Configure("Cell_Data_Name", "A1",
				UpdateRules.UR_AS_NEEDED, "cell Family", false, "xl file path", "worksheet name");
			cData.DocumentName = docName;
			cData.DocKey = exData.DocKey;


			result = fm.DataStorExist(exData.DocKey);
			if (result == ExStoreRtnCodes.XRC_DS_NOT_FOUND)
			{
				result = fm.CreateDataStorage(exData.DocKey);
				if (result != ExStoreRtnCodes.XRC_GOOD) return;
			}

			result = fm.WriteRoot(raData, cData);

			if (result == ExStoreRtnCodes.XRC_GOOD)
			{
				WriteLineAligned($"Data storage made and written!\n");
			}
			else
			{
				WriteLineAligned($"Data Storage failed!\n");
			}

			ShowMsg();
		}

		private void BtnFindRoot_OnClick(object sender, RoutedEventArgs e)
		{
			// get the entity by using ELEMENT.GetEntity(Schema) - 
			// per the below, the DataStorage is the element 
			// asuming that a single schema / element get found

			WriteMsg("\n");

			IList<Schema> schemas;
			IList<DataStorage> dx;

			bool result = fm.GetAppSchemas(exData.DocKey, out schemas);

			WriteLineAligned($"schema (in memory) found?| {result.ToString()}", $"quantity| {schemas.Count}");

			if (schemas.Count > 0)
			{
				WriteMsg("\n");

				foreach (Schema s in schemas)
				{
					WriteLineAligned($"schema info| {s.SchemaName}  {s.VendorId}  {s.Documentation}");
				}
			}

			ExStoreStartRtnCodes answer = fm.GetRootDataStorages(exData.DocKey, out dx);

			WriteMsg("\n");
			WriteLineAligned($"datastorage found?| {answer.ToString()}", $"quantity| {dx.Count}");

			if (dx.Count > 0)
			{
				WriteMsg("\n");

				foreach (DataStorage ds in dx)
				{
					WriteLineAligned($"datastorage info| {ds.Name}", $"valid?| {ds.IsValidObject}");
				}
			}

			ShowMsg();
		}

		// probably n/a
		private void BtnFindRootDs_OnClick(object sender, RoutedEventArgs e)
		{
			ExStoreRtnCodes result;

			result = fm.FindRootDS();

			WriteLine("find root DS|", result.ToString());
			ShowMsg();
		}


	#endregion

	}
}