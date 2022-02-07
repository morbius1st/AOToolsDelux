#region + Using Directives
using System;
using System.Text.RegularExpressions;
using Autodesk.Revit.DB.ExtensibleStorage;
using CSToolsDelux.Fields.SchemaInfo.SchemaData;
using CSToolsDelux.Utility;

#endregion

// user name: jeffs
// created:   9/11/2021 7:00:33 AM

namespace CSToolsDelux.Fields.ExStorage.ExStorageData
{

	// "static" class to hold all of
	// the data associated with the
	// document's datastorage
	public class ExStorData
	{
		public static ExStorData Instance => instance.Value;

		private static readonly Lazy<ExStorData> instance =
			new Lazy<ExStorData>(() => new ExStorData());

		private ExStorData() { }

		public string DsKey { get; set; }
		public static string VendorId { get; private set; }

		public DataStorage DataStorage { get; set; }
		public Entity Entity { get; set; }
		public Schema Schema { get; set; }
		public SchemaRootData RootData { get; set; }
		public SchemaCellData CellData { get; set; }

		public bool HasDsKey => DsKey != null;
		public bool HasDataStorage => DataStorage != null;
		public bool HasEntity => Entity != null;
		public bool HasSchema => Schema != null;
		public bool HasRoot => RootData != null;
		public bool HasCell => CellData != null;

		public bool HasRootData => RootData.Data != null;
		public bool HasCellData => CellData.Data != null;

		public void Config(string dsKey, DataStorage ds)
		{
			reset();

			VendorId = Util.GetVendorId().Replace('.','_');

			DsKey = dsKey;
			DataStorage = ds;

			RootData = new SchemaRootData();
			CellData = new SchemaCellData();
		}

		public bool MatchDsName(string testDsKey)
		{
			return (HasDataStorage &&
				DataStorage.Name.Equals(testDsKey));
		}

		public bool MatchDsKeyName(string testDsKey)
		{
			return (HasDsKey &&
				DsKey.Equals(testDsKey));
		}

		public bool MatchName(string testDsKey)
		{
			return (MatchDsName(testDsKey) && 
				MatchDsKeyName(testDsKey));
		}

		private void reset()
		{
			DsKey = null;
			DataStorage = null;
			Entity = null;
			Schema = null;
			RootData = null;
			CellData = null;
		}


		internal static string MakeKey(string documentName)
		{
			string vendId = VendorId;
			string docName = Regex.Replace(documentName, @"[^0-9a-zA-Z]", "");
			return vendId + "_" + docName;
		}

		// internal static string MakeVendIdPrefix()
		// {
		// 	return Util.GetVendorId().Replace('.','_');
		// }
	}
}
