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

		private ExStoreMgr exMgr;

		private ShowInfo show;

		private AWindow W;

	#endregion

	#region ctor

		static FieldsManager()
		{
			rFields = new SchemaRootFields();
			aFields = new SchemaAppFields();
			raFields = new SchemaRootAppFields();
			cFields = new SchemaCellFields();
		}

		public FieldsManager(AWindow w)
		{
			
			W = w;
			show = new ShowInfo(w);
						
			rData = new SchemaRootData();
			rData.Configure(SchemaGuidManager.GetNewAppGuidString());

			aData = new SchemaAppData();
			aData.Configure("App Data Name", "App Data Description");

			raData = new SchemaRootAppData();
			raData.Configure("Root-App Data Name", "Root-App Data Description");

			cData = new SchemaCellData();
			cData.Configure("new name", "A1", UpdateRules.UR_AS_NEEDED, "cell Family", false, "xl file path", "worksheet name");
			cData.Configure("Another new name", "B2", UpdateRules.UR_AS_NEEDED, "second cell Family", false, "second xl file path", "second worksheet name");
		}

	#endregion

	#region public properties

		public SchemaRootFields SchemaRootFields => rFields;

		public SchemaRootData rData { get; private set; }
		public SchemaAppData aData { get; private set; }
		public SchemaRootAppData raData { get; private set; }
		public SchemaCellData cData { get; private set; }

		public static SchemaRootFields rFields { get; }
		public static SchemaAppFields aFields { get; }
		public static SchemaRootAppFields raFields { get; }
		public static SchemaCellFields cFields { get; }

	#endregion

	#region private properties

	#endregion

	#region public methods
		
		public void GetDataStorage()
		{
			exMgr.GetDataStorage();
		}

		public void ShowRootAppFields()
		{
			show.ShowRootAppFields(raFields);
		}
		
		public void ShowRootAppData()
		{
			show.ShowRootAppData(raFields, raData);
		}

		public void ShowRootFields()
		{
			show.ShowRootFields(rFields);
		}

		public void ShowRootData()
		{
			show.ShowRootData(rFields, rData);
		}
		
		public void ShowAppFields()
		{
			show.ShowAppFields(aFields);
		}

		public void ShowAppData()
		{
			show.ShowAppData(aFields, aData);
		}
				
		public void ShowCellFields()
		{
			show.ShowCellFields(cFields);
		}

		public void ShowCellData()
		{
			show.ShowCellData(cData, cFields);
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