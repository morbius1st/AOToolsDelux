#region + Using Directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using Autodesk.Revit.DB.ExtensibleStorage;
using CSToolsDelux.Fields.SchemaInfo.SchemaData;
using CSToolsDelux.Fields.SchemaInfo.SchemaData.SchemaDataDefinitions;
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

		public string DocKey { get; set; }

		public DataStorage DataStorage { get; set; }
		public Entity Entity { get; set; }
		public Schema Schema { get; set; }
		public SchemaRootAppData RootAppData { get; set; }
		public SchemaCellData CellData { get; set; }

		public bool HasDocKey => DocKey != null;
		public bool HasDataStorage => DataStorage != null;
		public bool HasEntity => Entity != null;
		public bool HasSchema => Schema != null;
		public bool HasRootApp => RootAppData != null;
		public bool HasCell => CellData != null;

		public bool HasRootAppData => RootAppData.Data != null;
		public bool HasCellData => CellData.Data != null;

		public void Config(string docKey, DataStorage ds)
		{
			reset();

			DocKey = docKey;
			DataStorage = ds;

			RootAppData = new SchemaRootAppData();
			CellData = new SchemaCellData();
		}

		public bool MatchDsName(string testDocKey)
		{
			return (HasDataStorage &&
				DataStorage.Name.Equals(testDocKey));
		}

		public bool MatchDocKeyName(string testDocKey)
		{
			return (HasDocKey &&
				DocKey.Equals(testDocKey));
		}

		public bool MatchName(string testDocKey)
		{
			return (MatchDsName(testDocKey) && 
				MatchDocKeyName(testDocKey));
		}

		private void reset()
		{
			DocKey = null;
			DataStorage = null;
			Entity = null;
			Schema = null;
			RootAppData = null;
			CellData = null;
		}


		internal static string MakeKey(string documentName)
		{
			string vendId = MakeVendIdPrefix();
			string docName = Regex.Replace(documentName, @"[^0-9a-zA-Z]", "");
			return vendId + "_" + docName;
		}

		internal static string MakeVendIdPrefix()
		{
			return Util.GetVendorId().Replace('.','_');
		}
	}
}
