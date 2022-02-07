#region using

using SharedCode.Fields.SchemaInfo.SchemaSupport;
using SharedCode.Fields.SchemaInfo.SchemaData;
using SharedCode.Fields.SchemaInfo.SchemaFields;
using SharedCode.Windows;
using SharedCode.ShowInformation;
using UtilityLibrary;
// using CSToolsDelux.Fields.SchemaInfo.SchemaData;
// using CSToolsDelux.Fields.SchemaInfo.SchemaFields;

#endregion

// username: jeffs
// created:  9/4/2021 11:41:42 AM

namespace CSToolsStudies.FieldsManagement
{
	public class FieldsManager
	{
	#region private fields

		// private ExStoreManager exMgr;

		private ShShowInfo shShow;

		private AWindow W;

	#endregion

	#region ctor

		static FieldsManager()
		{
			// NEW and OLD system
			
			ClFields = new FieldsCell();
			LkFields = new FieldsLock();
		}

		public FieldsManager(AWindow w)
		{
			W = w;
			shShow = new ShShowInfo(w, CsUtilities.AssemblyName, "CsToolsStudies");

			// // OLD system
			// rData = new SchemaRootData();
			// rData.Configure("Root Data Name", "Root Data Description");
			//
			// cData = new SchemaCellData();
			// cData.Configure("new name", "A1", UpdateRules.UR_AS_NEEDED, "cell Family", false, "xl file path", "worksheet name");
			// cData.Configure("Another new name", "B2", UpdateRules.UR_AS_NEEDED, "second cell Family", false, "second xl file path", "second worksheet name");


			RtFields = new FieldsRoot();

			// NEW class design
			// root data
			RtData = new DataRoot(RtFields);
			RtData.Configure("New Root Data Name");

			// lock data
			LkData = new DataLock(LkFields);
			LkData.Configure("New Lock Data Name");

			// cell data
			ClData = new DataCell(ClFields, 2);
			ClData.DataIndex = 0;
			ClData.Configure("New cell Data Name 0");

			ClData.DataIndex = 1;
			ClData.Configure("New cell Data Name 1");

			ClData.SetValue(SchemaCellKey.CK_XL_WORKSHEET_NAME, "This is worksheet 0", 0);
			ClData.SetValue(SchemaCellKey.CK_XL_WORKSHEET_NAME, "This is worksheet 1", 1);

			ClData.SetValue(SchemaCellKey.CK_CELL_FAMILY_NAME, "Family name 0", 0);
			ClData.SetValue(SchemaCellKey.CK_CELL_FAMILY_NAME, "Family name 1", 1);
		}

	#endregion

	#region public properties

		// OLD system
		// public SchemaRootData rData { get; private set; }
		// public SchemaCellData cData { get; private set; }
		// public SchemaLockData lData { get; private set; }

		// NEW and OLD system
		public FieldsRoot RtFields { get; }
		public static FieldsCell ClFields { get; }
		public static FieldsLock LkFields { get; }

		// NEW system
		public DataRoot RtData { get; private set; }
		public DataCell ClData { get; private set; }
		public DataLock LkData { get; private set; }

	#endregion

	#region private properties

	#endregion

	#region public methods




		public void ShowRootFields()
		{
			// show.ShowRootFields(rtFields);

			shShow.ShowSchemaFields(RtFields);
		}

		// public void ShowRootData()
		// {
		// 	shShow.ShowRootData(rFields, rData);
		//
		// 	shShow.ShowDataGeneric(rtData);
		// }

		public void ShowCellFields()
		{
			// show.ShowCellFields(clFields);
			shShow.ShowSchemaFields(ClFields);
		}

		// public void ShowCellData()
		// {
		// 	shShow.ShowCellData( cFields, cData);
		//
		// 	shShow.ShowDataGeneric(clData);
		// }

		public void ShowLockFields()
		{
			// show.ShowLockFields(lkFields);
			shShow.ShowSchemaFields(LkFields);
		}

		// public void ShowLockData()
		// {
		// 	lData = new SchemaLockData();
		//
		// 	shShow.ShowLockData( lFields, lData);
		//
		// 	shShow.ShowDataGeneric(lkData);
		// }

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