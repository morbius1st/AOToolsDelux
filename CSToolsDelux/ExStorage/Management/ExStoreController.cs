#region using directives

using System;
using Autodesk.Revit.DB;
using CSToolsDelux.Fields.Testing;
using SharedCode.Windows;
using SharedCode.Fields.ExStorage.ExStorManagement;

#endregion


// projname: $projectname$
// itemname: ExStoreController
// username: jeffs
// created:  10/22/2021 6:03:00 AM

// master class:
// FieldsManagement -> this
// root class
// class management:
// configure()
// debug only
// ShowRootData()

// production items
// properties: 
// DsKey





namespace CSToolsDelux.ExStorage.Management
{
	/// <summary>
	/// Primary static class to control the ex storage / data store / schema<br/>
	/// includes<br/>
	/// DsKey| the primary key used to name the various parts saved to the model
	/// </summary>
	public class ExStoreController
	{
	#region private fields

		private static readonly Lazy<ExStoreController> instance =
			new Lazy<ExStoreController>(() => new ExStoreController());

		private AWindow w;

		private ExStoreSupport exSupport;
		private ExStoreDialogs exDlg;


	#if DEBUG
		private ShowInfo show;
	#endif

	#endregion

	#region ctor

		private ExStoreController()
		{
			exSupport = new ExStoreSupport("");
			exDlg = ExStoreDialogs.Instance;

		// #if DEBUG
		// 	show = new ShowInfo(w);
		// #endif
		}

	#endregion

	#region public properties

		public static ExStoreController Instance => instance.Value;

		public string DsKey { get; private set; }

		public ExStoreSupport ExSupport => exSupport;

		public bool IsConfigured => !string.IsNullOrWhiteSpace(DsKey);

	#endregion

	#region private properties

	#endregion

	#region public methods

		internal ExStoreRtnCodes Configure(AWindow w, Document doc)
		{
			this.w = w;

			if (doc == null)
			{
				exDlg.MsgDlgNoDocName();
				return ExStoreRtnCodes.XRC_FAIL;
			}

			exSupport = new ExStoreSupport(doc.Title);

			if (!exSupport.IsDocValid) return ExStoreRtnCodes.XRC_FAIL;

			DsKey = exSupport.DsKey;

			return ExStoreRtnCodes.XRC_GOOD;
		}


	#region debug public methods

	#if DEBUG

		public void ShowRootData() { }


	#endif

	#endregion

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
			return "this is ExStoreController";
		}

	#endregion
	}
}