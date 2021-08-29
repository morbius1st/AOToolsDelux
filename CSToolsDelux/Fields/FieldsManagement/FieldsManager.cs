#region using

#endregion

// username: jeffs
// created:  8/28/2021 8:58:30 PM

using CSToolsDelux.Fields.SchemaInfo.SchemaData;
using CSToolsDelux.Fields.SchemaInfo.SchemaData.SchemaDataDefinitions;
using CSToolsDelux.Fields.SchemaInfo.SchemaDefinitions;
using CSToolsDelux.Fields.SchemaInfo.SchemaFields;
using CSToolsDelux.Fields.Testing;
using CSToolsDelux.WPF.FieldsWindow;

namespace CSToolsDelux.Fields.FieldsManagement
{
	public class FieldsManager
	{
	#region static properties

		public static SchemaFieldsRoot fRoot { get; }
		public SchemaRootData rData { get; private set; }


	#endregion

	#region private fields

		private ShowInfo show;

		private MainFields W;

	#endregion

	#region ctor

		static FieldsManager()
		{
			fRoot = new SchemaFieldsRoot();
		}

		public FieldsManager(MainFields w)
		{
			W = w;
			show = new ShowInfo(w);
			rData = new SchemaRootData();
			rData.Configure();
		}


	#endregion

	#region public properties

		public SchemaFieldsRoot SchemaFieldsRoot => fRoot;

	#endregion

	#region private properties

	#endregion

	#region public methods

		public void ShowFields()
		{
			show.ShowRootFields(fRoot);
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