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
	 * is the primary task manager.  this will have a collection of
	 * tasks that need to be performed as directed by MainFields
	 *
	 * ExStoreManager
	 * is the primary action manager & performer
	 * this will have the individual task components that are
	 * needed by the task manager.
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
		private SchemaRootAppData raData;
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

	#region test - debug methods

		private void BtnDebug_OnClick(object sender, RoutedEventArgs e)
		{
			Debug.WriteLine("@debug");
		}

		private void BtnWriteData_OnClick(object sender, RoutedEventArgs e)
		{
			// test only
			// et.testNames();


			ExStoreRtnCodes result;

			// bogus data for testing
			raData = new SchemaRootAppData();
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

			result = fm.WriteRootApp(raData, cData);

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



		
		private void BtnFindRootDs_OnClick(object sender, RoutedEventArgs e)
		{
			ExStoreRtnCodes result;

			result = fm.FindRootDS();

			WriteLine("find root DS|", result.ToString());
			ShowMsg();
		}

		private void BtnFindSchemaAndDataStor_OnClick(object sender, RoutedEventArgs e)
		{
			// get the entity by using ELEMENT.GetEntity(Schema) - 
			// per the below, the DataStorage is the element 
			// asuming that a single schema / element get found

			WriteMsg("\n");

			IList<Schema> schemas;
			IList<DataStorage> dx;

			bool result = fm.GetRootAppSchemas(exData.DocKey, out schemas);

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

		private void BtnDelete_OnClick(object sender, RoutedEventArgs e)
		{
			using (Transaction T = new Transaction(AppRibbon.Doc, "fields"))
			{
				T.Start();

				ExStoreRtnCodes result = fm.DeleteRootApp(exData.DocKey);

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

		private void BtnRead_OnClick(object sender, RoutedEventArgs e)
		{

			using (Transaction T = new Transaction(AppRibbon.Doc, "fields| Read"))
			{
				T.Start();

				ExStoreRtnCodes result = et.StartProcess(exData.DocKey);

				if (result == ExStoreRtnCodes.XRC_GOOD)
				{
					T.Commit();
					WriteLineAligned($"read| ");
					WriteLineAligned($"date| {exData.RootAppData.GetValue<string>(SchemaRootAppKey.RAK_CREATE_DATE)}");
				}
				else
				{
					T.RollBack();

					WriteLineAligned($"not read| {result.ToString()}");
				}
			}

			ShowMsg();
		}



		private void BtnRootAppFields_OnClick(object sender, RoutedEventArgs e)
		{
			fm.ShowRootAppFields();
		}

		private void BtnRootAppData_OnClick(object sender, RoutedEventArgs e)
		{
			fm.ShowRootAppData();
		}

		private void BtnCellFields_OnClick(object sender, RoutedEventArgs e)
		{
			fm.ShowCellFields();
		}

		private void BtnCellData_OnClick(object sender, RoutedEventArgs e)
		{
			fm.ShowCellData();
		}

	#endregion

	}
}