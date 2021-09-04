#region using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB.ExtensibleStorage;
using Autodesk.Revit.DB.Macros;

#endregion


// projname: $projectname$
// itemname: SchemaManager
// username: jeffs
// created:  9/4/2021 12:59:44 PM

namespace CSToolsDelux.Fields.SchemaInfo.SchemaManagement
{
	
	public class SchemaManager
	{
		

		private class schemaItem
		{
			private SchemaManager scMgr { get; set; }

			public string DocumentName { get; set; }
			public Schema Schema { get; set; }

			public schemaItem(string documentName, Schema schema)
			{
				DocumentName = documentName;
				Schema = schema;
			}
		}

		private class schemaList
		{
			private SchemaManager scMgr { get; set; }

			private Dictionary<string, schemaItem> items;

			public schemaList()
			{
				if (items == null) items = new Dictionary<string, schemaItem>();
			}

			public Dictionary<string, schemaItem> Items => items;

			public Schema Find(string key)
			{
				bool result = items.ContainsKey(key);

				if (result) return items[key].Schema;

				return null;
			}
		}



	#region private fields

		private static readonly Lazy<SchemaManager> instance =
			new Lazy<SchemaManager>(() => new SchemaManager());

		private schemaList scList;

	#endregion

	#region ctor

		private SchemaManager()
		{
			scList = new schemaList();

		}

	#endregion

	#region public properties

		public static SchemaManager Instance => instance.Value;

	#endregion

	#region private properties

	#endregion

	#region public methods

		public bool Find(string docName, out IList<Schema> schemas)
		{
			bool result;
			string key = makeKey(docName);
			schemas = new List<Schema>(1);

			// already got the schema?
			result = findFromList(key, out schemas);

			if (result) return true;

			// don't already got
			// check the document

			result = findFromDoc(key, out schemas);

			if (!result) return false;

			return true;
		}


		public string VendorId()
		{
			return "PRO.CYBERSTUDIO";
		}

	#endregion

	#region private methods

		private bool findFromDoc(string key, out IList<Schema> schemaList)
		{
			bool result = false;
			schemaList = new List<Schema>(1);

			IList<Schema> schemas = Schema.ListSchemas();

			foreach (Schema s in schemas)
			{
				if (s.SchemaName.Equals(key))
				{
					schemaList.Add(s);

					result = true;
				}
			}
			return result;
		}


		private bool findFromList(string key, out IList<Schema> schemaList)
		{
			bool result = false;
			Schema schema = null;

			schemaList = new List<Schema>(1);

			schema = scList.Find(key);

			if (schema != null)
			{
				schemaList.Add(scList.Find(key));
				result = true;
			}

			return result;
		}

		protected string makeKey(string documentName)
		{
			return VendorId() + "::" + documentName;
		}

	#endregion

	#region event consuming

	#endregion

	#region event publishing

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is SchemaManager";
		}

	#endregion
	}
}