#region using

using Autodesk.Revit.DB;
using CSToolsDelux.ExStorage.Management;
using CSToolsDelux.Fields.Testing;
using SharedCode.Fields.SchemaInfo.SchemaSupport;
using SharedCode.Windows;
using SharedCode.Fields.ExStorage.ExStorManagement;
using SharedCode.Fields.SchemaInfo.SchemaData;
using SharedCode.Fields.SchemaInfo.SchemaFields;

#endregion

// username: jeffs
// created:  10/30/2021 7:43:47 AM

namespace CSToolsDelux.Fields.FieldsManagement
{
	public class FieldsManager2
	{
	#region private fields

		private ExStoreController exCtlr;

		// show information routines
		private ShowInfo show;
		// private FieldsStartProcedure fs;

		private AWindow W;

		private Document doc;

	#endregion

	#region ctor

		public FieldsManager2(AWindow w, Document doc)
		{
			this.doc = doc;
			W = w;

			show = new ShowInfo(w);

			RtFields = new FieldsRoot();
			ClFields = new FieldsCell();
			LkFields = new FieldsLock();

			IsConfigured = configCtlr(w, doc);
			configData();
		}

	#endregion

	#region public properties

		public ExStoreController ExStoreCtlr => exCtlr;

		public string DsKey => exCtlr?.DsKey ?? "undefined";

		public bool IsConfigured { get; private set; }

	#endregion

	#region private properties

		private void configData()
		{

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

		private bool configCtlr(AWindow w, Document doc)
		{
			ExStoreRtnCodes result;

			exCtlr = ExStoreController.Instance;

			result = exCtlr.Configure(w, doc);

			if (result != ExStoreRtnCodes.XRC_GOOD) return false;

			return true;
		}

	#endregion

	#region public methods

		public FieldsRoot RtFields { get; }
		public FieldsCell ClFields { get; }
		public FieldsLock LkFields { get; }

		// NEW system
		public DataRoot RtData { get; private set; }
		public DataCell ClData { get; private set; }
		public DataLock LkData { get; private set; }


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
			return "this is FieldsManager2";
		}

	#endregion
	}
}