#region using

#endregion

// username: jeffs
// created:  8/28/2021 8:58:30 PM

using System;
using CSToolsDelux.Fields.SchemaInfo.SchemaData;
using CSToolsDelux.Fields.SchemaInfo.SchemaDefinitions;
using CSToolsDelux.Fields.SchemaInfo.SchemaFields;
using CSToolsDelux.Fields.Testing;
using CSToolsDelux.WPF;
using CSToolsDelux.WPF.FieldsWindow;

namespace CSToolsDelux.Fields.FieldsManagement
{
	public class FieldsManager
	{
	#region private fields

		private ShowInfo show;

		private AWindow W;

	#endregion

	#region ctor

		static FieldsManager()
		{
			rFields = new SchemaRootFields();
		}

		public FieldsManager(AWindow w, string documentName)
		{
			W = w;
			show = new ShowInfo(w, documentName);

			rData = new SchemaRootData();
			rData.Configure(SchemaGuidManager.GetNewAppGuidString());

			aData = new SchemaAppData();
			aData.Configure("App Data Name", "App Data Description");
		}

	#endregion

	#region public properties

		public SchemaRootFields SchemaRootFields => rFields;

		public SchemaRootData rData { get; private set; }
		public SchemaAppData aData { get; private set; }

		public static SchemaRootFields rFields { get; }
		public static SchemaAppFields aFields { get; }

	#endregion

	#region private properties

	#endregion

	#region public methods

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