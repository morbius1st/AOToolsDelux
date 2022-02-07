#region using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using SharedCode.Fields.SchemaInfo.SchemaSupport;
using CSToolsStudies.FieldsManagement;
using SharedCode.Windows;
using CSToolsStudies.Testing;
using SharedCode.Fields.SchemaInfo.SchemaData.DataTemplate;
using SharedCode.ShowInformation;
using UtilityLibrary;
using SharedCode.Fields.SchemaInfo.SchemaData.DataTemplates;

#endregion

// projname: CSToolsStudies
// itemname: MainWindow
// username: jeffs
// created:  8/29/2021 2:19:26 PM

namespace CSToolsStudies.Windows
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : AWindow, INotifyPropertyChanged
	{
	#region private fields

		public string VendorId = "PRO.CYBERSTUDIO";

		private string myName = nameof(MainWindow);
		private string textMsg01;

		private int marginSize = 0;
		private int marginSpaceSize = 2;

		private string location;

		private string dsKey;
		private SchemaDataStorType dsType;

		private KeyValuePair<string, SchemaDataStorType> currentSchemaDataType;

		private FieldsManager fm;
		private ShShowInfo shShow;

		private Tests01 test01;

	#endregion

	#region ctor

		public MainWindow()
		{
			InitializeComponent();

			fm = new FieldsManager(this);
			shShow = new ShShowInfo(this, CsUtilities.AssemblyName, "CSToolsStudies");
			test01 = new Tests01(this);

			// DsType = SchemaDataStorType.DT_ROOT;
			CurrentSchemaDataType = SchemaConstants.SchemaTypeRoot;
		}

	#endregion

	#region public properties

		public static string DsKey => "CSToolsStudies";


		// public DataStorType DsType
		// {
		// 	get => dsType;
		// 	set
		// 	{
		// 		dsType = value;
		// 		OnPropertyChanged();
		// 	}
		// }

		public KeyValuePair<string, SchemaDataStorType> CurrentSchemaDataType
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

	#region event consuming

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

		private void BtnExit_OnClick(object sender, RoutedEventArgs e)
		{
			base.Close();
		}

		private void BtnClrTxBx_OnClick(object sender, RoutedEventArgs e)
		{
			MsgClr();
		}


		// private void BtnShowRootFields_OnClick(object sender, RoutedEventArgs e)
		// {
		// 	fmMgr.ShowRootFields();
		// }
		//
		// private void BtnRootData_OnClick(object sender, RoutedEventArgs e)
		// {
		// 	fmMgr.ShowRootData();
		// }
		//
		// private void BtnAppFields_OnClick(object sender, RoutedEventArgs e)
		// {
		// 	fmMgr.ShowRootFields();
		// }
		//
		// private void BtnAppData_OnClick(object sender, RoutedEventArgs e)
		// {
		// 	fmMgr.ShowAppData();
		// }

		// private void BtnCellFields_OnClick(object sender, RoutedEventArgs e)
		// {
		// 	fm.ShowCellFields();
		// }

		// private void BtnRootAppFields_OnClick(object sender, RoutedEventArgs e)
		// {
		// 	fm.ShowRootFields();
		// }

		// private void BtnRootAppData_OnClick(object sender, RoutedEventArgs e)
		// {
		// 	fm.ShowRootData();
		// }

		// private void BtnShowLockFields_OnClick(object sender, RoutedEventArgs e)
		// {
		// 	fm.ShowLockFields();
		// }

		// private void BtnShowLockData_OnClick(object sender, RoutedEventArgs e)
		// {
		// 	fm.ShowLockData();
		// }


		// private void BtnTest02_OnClick(object sender, RoutedEventArgs e) 
		// {
		// 	string x = System.IO.Path.GetRandomFileName().Replace('.','_');
		//
		// 	WriteLineAligned("unique name| ", x);
		//
		// 	ShowMsg();
		// }

		// private void BtnTestStart_OnClick(object sender, RoutedEventArgs e)
		// {
		// 	test01.proc00();
		// }

	#region tests

		private void Btn_TstDivText_OnClick(object sender, RoutedEventArgs e)
		{
			// test01.TestSplitString5();
			test01.TestColumnSplit();
		}

		private void Btn_TstJustText_OnClick(object sender, RoutedEventArgs e)
		{
			test01.TestJustifyEllipsisString();
		}

	#endregion

	#region set which

		private void BtnSetRoot_OnClick(object sender, RoutedEventArgs e)
		{
			// DsType = DataStorType.DT_ROOT;
			CurrentSchemaDataType = SchemaConstants.SchemaTypeRoot;
		}

		private void BtnSetCell_OnClick(object sender, RoutedEventArgs e)
		{
			// DsType = DataStorType.DT_CELL;
			CurrentSchemaDataType = SchemaConstants.SchemaTypeCell;
		}

		private void BtnSetLock_OnClick(object sender, RoutedEventArgs e)
		{
			// DsType = DataStorType.DT_LOCK;
			CurrentSchemaDataType = SchemaConstants.SchemaTypeLock;
		}

	#endregion

	#region show

		private void BtnShowData_OnClick(object sender, RoutedEventArgs e)
		{
			this.MsgClr();

			switch (CurrentSchemaDataType.Value)
			{
			case SchemaDataStorType.DT_ROOT:
				{
					shShow.ShowDataMembers(fm.RtData);
					break;
				}
			case SchemaDataStorType.DT_CELL:
				{
					shShow.ShowDataMembers(fm.ClData);
					break;
				}
			case SchemaDataStorType.DT_LOCK:
				{
					shShow.ShowDataMembers(fm.LkData);
					break;
				}
			}
		}

		// private void ShowDataMembers<TSk>(ADataTempBase<TSk> data) where TSk : Enum, new ()
		// {
		// 	List<List<Dictionary<DataColumns, string>>> info = DataTemplateMembers.FormatData(data);
		//
		// 	shShow.ShowDataInfo(
		// 		data,
		// 		DataTemplateMembers.DefaultDataOrder,
		// 		DataTemplateMembers.DataHdr,
		// 		DataTemplateMembers.DataHdrInfo,
		// 		info);
		// }
		//

		private void BtnShowFields_OnClick(object sender, RoutedEventArgs e)
		{
			this.MsgClr();

			switch (CurrentSchemaDataType.Value)
			{
			case SchemaDataStorType.DT_ROOT:
				{
					shShow.ShowSchemaFields(fm.RtFields);
					// fm.ShowRootFields();
					break;
				}
			case SchemaDataStorType.DT_CELL:
				{
					fm.ShowCellFields();
					break;
				}
			case SchemaDataStorType.DT_LOCK:
				{
					fm.ShowLockFields();
					break;
				}
			}
		}

	#endregion
	}
}