#region using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using CSToolsDelux.Fields.ExStorage.ExStorManagement;
using CSToolsDelux.Fields.SchemaInfo.SchemaData;
using CSToolsDelux.Fields.SchemaInfo.SchemaDefinitions;
using CSToolsDelux.Fields.SchemaInfo.SchemaFields;
using CSToolsDelux.Fields.Testing;
using CSToolsDelux.WPF;

#endregion

// username: jeffs
// created:  9/4/2021 11:41:42 AM

namespace CSToolsStudies.FieldsManagement
{
	public class FieldsManager
	{
	#region private fields

		private ExStoreManager exMgr;

		private ShowInfo show;

		private AWindow W;

	#endregion

	#region ctor

		static FieldsManager()
		{
			raFields = new SchemaRootFields();
			cFields = new SchemaCellFields();
			lFields = new SchemaLockFields();
		}

		public FieldsManager(AWindow w)
		{
			W = w;
			show = new ShowInfo(w);

			rData = new SchemaRootData();
			rData.Configure("Root Data Name", "Root Data Description");

			cData = new SchemaCellData();
			cData.Configure("new name", "A1", UpdateRules.UR_AS_NEEDED, "cell Family", false, "xl file path", "worksheet name");
			cData.Configure("Another new name", "B2", UpdateRules.UR_AS_NEEDED, "second cell Family", false, "second xl file path", "second worksheet name");
		}

	#endregion

	#region public properties

		public SchemaRootData rData { get; private set; }
		public SchemaCellData cData { get; private set; }
		public SchemaLockData lData { get; private set; }

		public static SchemaRootFields raFields { get; }
		public static SchemaCellFields cFields { get; }
		public static SchemaLockFields lFields { get; }

	#endregion

	#region private properties

	#endregion

	#region public methods

		public void ShowRootFields()
		{
			// show.ShowRootFields(raFields);

			show.ShowSchemaFields(raFields);
		}
		
		public void ShowRootData()
		{
			show.ShowRootData(raFields, rData);
		}

		public void ShowCellFields()
		{
			// show.ShowCellFields(cFields);
			show.ShowSchemaFields(cFields);
		}

		public void ShowCellData()
		{
			show.ShowCellData( cFields, cData);
		}
		
		public void ShowLockFields()
		{
			// show.ShowLockFields(lFields);
			show.ShowSchemaFields(lFields);
		}

		public void ShowLockData()
		{
			lData = new SchemaLockData();

			show.ShowLockData( lFields, lData);
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