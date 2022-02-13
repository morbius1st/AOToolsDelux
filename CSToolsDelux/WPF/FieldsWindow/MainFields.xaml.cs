using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using Autodesk.Revit.DB;
using CSToolsDelux.Fields.FieldsManagement;
using CSToolsDelux.Fields.Testing;
using SharedCode.Fields.SchemaInfo.SchemaSupport;
using SharedCode.Windows;
using SharedCode.Fields.ExStorage.ExStorManagement;

using SharedCode.ShowInformation;
using UtilityLibrary;

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

		private FieldsManager2 fm2;
		private ShowInfo show;
		private ShShowInfo shShow;

		// original
		// private ExTests01 et;
		// private ExStorData exData;
		// private SchemaRootData raData;
		// private SchemaCellData cData;
		// /// <inheritdoc cref="FieldsManager"/>
		// private FieldsManager fm;
		private static string docName;
		private string dsKey;
		private SchemaDataStorType dsType;

		// generic
		private Document doc;

		private string myName = nameof(MainFields);

		private string textMsg01;

		private int marginSize = 0;

		private int marginSpaceSize = 2;

		private string location;

		private KeyValuePair<SchemaDataStorType, string> currentSchemaDataType;


	#endregion

	#region ctor

		public MainFields(Document doc)
		{
			ExStoreRtnCodes result;

			InitializeComponent();

			this.doc = doc;
			docName = doc.Title;

			fm2 = new FieldsManager2(this, doc);
			shShow = new ShShowInfo(this, CsUtilities.AssemblyName, doc.Title);
			// show = new ShowInfo(this);
/*
			et = new ExTests01(this, doc);
			exData = ExStorData.Instance;

			DsKey = ExStorData.MakeKey(docName);
			exData.DsKey = dsKey;
*/
			DsKey = fm2.DsKey;
			DsType = SchemaDataStorType.DT_ROOT;

			CurrentSchemaDataType = SchemaConstants.SchemaTypeRoot;
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

		public SchemaDataStorType DsType
		{
			get => dsType;
			set
			{
				dsType = value;
				OnPropertyChanged();
			}
		}

		public KeyValuePair<SchemaDataStorType, string> CurrentSchemaDataType
		{
			get => currentSchemaDataType;
			set
			{
				currentSchemaDataType = value;
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
		
		private void BtnClrTxBx_OnClick(object sender, RoutedEventArgs e)
		{
			MsgClr();
		}

	#endregion


	#region debug method

		private void BtnDebug_OnClick(object sender, RoutedEventArgs e)
		{
			Debug.WriteLine("@debug");
		}

	#endregion

	#region show

		private void BtnSetRoot_OnClick(object sender, RoutedEventArgs e)
		{
			DsType = SchemaDataStorType.DT_ROOT;
		}
		
		private void BtnSetCell_OnClick(object sender, RoutedEventArgs e)
		{
			DsType = SchemaDataStorType.DT_CELL;
		}
		
		private void BtnSetLock_OnClick(object sender, RoutedEventArgs e)
		{
			DsType = SchemaDataStorType.DT_LOCK;
		}

		private void BtnShowData_OnClick(object sender, RoutedEventArgs e)
		{
			this.MsgClr();

			switch (CurrentSchemaDataType.Key)
			{
			case SchemaDataStorType.DT_ROOT:
				{
					shShow.ShowData(fm2.RtData);
					break;
				}
			case SchemaDataStorType.DT_CELL:
				{
					shShow.ShowData(fm2.ClData);
					break;
				}
			case SchemaDataStorType.DT_LOCK:
				{
					shShow.ShowData(fm2.LkData);
					break;
				}
			}
		}

		private void BtnShowDataInfo_OnClick(object sender, RoutedEventArgs e)
		{
			this.MsgClr();

			switch (CurrentSchemaDataType.Key)
			{
			case SchemaDataStorType.DT_ROOT:
				{
					shShow.ShowDataMembers(fm2.RtData);
					break;
				}
			case SchemaDataStorType.DT_CELL:
				{
					shShow.ShowDataMembers(fm2.ClData);
					break;
				}
			case SchemaDataStorType.DT_LOCK:
				{
					shShow.ShowDataMembers(fm2.LkData);
					break;
				}
			}
		}

		private void BtnShowFieldsInfo_OnClick(object sender, RoutedEventArgs e)
		{
			this.MsgClr();

			switch (CurrentSchemaDataType.Key)
			{
			case SchemaDataStorType.DT_ROOT:
				{
					// shShow.ShowSchemaFields(fm.RtFields);
					shShow.ShowFieldMembers(fm2.RtFields);
					// fm.ShowRootFields();
					break;
				}
			case SchemaDataStorType.DT_CELL:
				{
					shShow.ShowFieldMembers(fm2.ClFields);
					// fm.ShowCellFields();
					break;
				}
			case SchemaDataStorType.DT_LOCK:
				{
					shShow.ShowFieldMembers(fm2.LkFields);
					// fm.ShowLockFields();
					break;
				}
			}
		}

		private void BtnShowDataTest_OnClick(object sender, RoutedEventArgs e)
		{
			this.MsgClr();

			switch (CurrentSchemaDataType.Key)
			{
			case SchemaDataStorType.DT_ROOT:
				{
					shShow.ShowTest(fm2.RtData, SchemaRootKey.RK_DESCRIPTION);
					break;
				}
			case SchemaDataStorType.DT_CELL:
				{
					shShow.ShowTest(fm2.ClData, SchemaCellKey.CK_DESCRIPTION);
					break;
				}
			case SchemaDataStorType.DT_LOCK:
				{
					shShow.ShowTest(fm2.LkData, SchemaLockKey.LK_DESCRIPTION);
					break;
				}
			}
		}

		private void BtnShowExStorInfo_OnClick(object sender, RoutedEventArgs e)
		{
			this.MsgClr();

			bool exsConfgd = fm2.ExStoreCtlr.IsConfigured;

			if (exsConfgd)
			{
				WriteLine2("ExStoreCtlr|isConfg'd", "| ", fm2.ExStoreCtlr.IsConfigured);
				WriteLine2("ExStoreCtlr|DsKey", "| ", fm2.ExStoreCtlr.DsKey);
				WriteLine2("ExSupport|DsKey", "| ", fm2.ExStoreCtlr.ExSupport.DsKey);
				WriteLine2("ExSupport|DocName", "| ", fm2.ExStoreCtlr.ExSupport.DocName);
				WriteLine2("ExSupport|DocumentName", "| ", fm2.ExStoreCtlr.ExSupport.DocumentName);
				WriteLine2("ExSupport|VendorId", "| ", fm2.ExStoreCtlr.ExSupport.VendorId);
				WriteLine2("ExSupport|IsDocValid", "| ", fm2.ExStoreCtlr.ExSupport.IsDocValid);
			}
			else
			{
				
			}



		}

	#endregion

		private void BtnFindAllDs_OnClick(object sender, RoutedEventArgs e)
		{
			
		}

	#region tests

	#endregion

	}
}


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

