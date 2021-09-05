#region using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using Autodesk.Revit.DB.Macros;
using CSToolsDelux.Fields.SchemaInfo.SchemaData;
using CSToolsDelux.Fields.SchemaInfo.SchemaDefinitions;
using CSToolsDelux.Fields.SchemaInfo.SchemaFields;
using CSToolsDelux.Utility;

#endregion


// projname: $projectname$
// itemname: SchemaManager
// username: jeffs
// created:  9/4/2021 12:59:44 PM

namespace CSToolsDelux.Fields.SchemaInfo.SchemaManagement
{
	
	public class SchemaManager
	{
		

		public class schemaItem
		{
			private SchemaManager scMgr { get; set; }

			public string DocumentKey { get; set; }
			public Schema Schema { get; set; }
			public Dictionary<string, Guid> SubSchemaFields { get; set; }
			public int QtySubSchema { get; private set; }

			public schemaItem(string documentKey, int qtySubSchema) : this(documentKey, qtySubSchema, null) {}

			public schemaItem(string documentKey, int qtySubSchema, Schema schema)
			{
				DocumentKey = documentKey;
				Schema = schema;
				SubSchemaFields = new Dictionary<string, Guid>(qtySubSchema);
				QtySubSchema = qtySubSchema;
			}
		}

		public class schemaList
		{
			private SchemaManager scMgr { get; set; }

			private Dictionary<string, schemaItem> items;

			public schemaList()
			{
				if (items == null) items = new Dictionary<string, schemaItem>();
			}

			public Dictionary<string, schemaItem> Items => items;

			public schemaItem this[string idx] => items[idx];

			public void AddNew(string key, int qtySubSchema)
			{
				schemaItem si = new schemaItem(key, qtySubSchema);

				items.Add(si.DocumentKey, si);
			}

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

		public schemaList SchemaList => scList;

	#endregion

	#region private properties

	#endregion

	#region public methods

		public string makeKey(string documentName)
		{
			string vendId = Util.GetVendorId().Replace('.','_');
			string docName = Regex.Replace(documentName, @"[^0-9a-zA-Z]", "");
			return vendId + "_" + docName;
		}

		public bool Find(string docName, out IList<Schema> schemas)
		{
			bool result;
			schemas = new List<Schema>(1);

			// already got the schema?
			result = FindSchemasFromList(docName, out schemas);

			if (result) return true;

			// don't already got
			// check the document

			result = FindSchemasFromDoc(docName, out schemas);

			if (!result) return false;

			return true;
		}

		public bool MakeRootAppSchema(string key, SchemaRootAppFields raFields, int QtySubSchema)
		{
			if (raFields == null || key == null || QtySubSchema == 0) return false;

			scList.AddNew(key, QtySubSchema);

			Schema schema = null;

			Dictionary<string, Guid> subSchemaFields;

			try
			{
				SchemaBuilder sb = new SchemaBuilder(Guid.NewGuid());

				makeSchemaDef(ref sb, raFields.GetValue<string>(SchemaRootAppKey.RAK_NAME), 
					raFields.GetValue<string>(SchemaRootAppKey.RAK_DESCRIPTION));

				makeSchemaFields(ref sb, raFields.Fields);

				makeSchemaSubSchemaFields(key, ref sb, raFields);

				schema = sb.Finish();

			}
			catch (Exception e)
			{
				string ex = e.Message;
				string iex = e?.InnerException.Message ?? "none";
			}

			scList[key].Schema = schema;

			return true;
		}

	#endregion

	#region private methods

		private bool FindSchemasFromDoc(string docName, out IList<Schema> schemaList)
		{
			bool result = false;
			schemaList = new List<Schema>(1);
			string key = makeKey(docName);

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

		private bool FindSchemasFromList(string docName, out IList<Schema> schemaList)
		{
			bool result = false;
			Schema schema = null;
			string key = makeKey(docName);

			schemaList = new List<Schema>(1);

			schema = scList.Find(key);

			if (schema != null)
			{
				schemaList.Add(scList.Find(key));
				result = true;
			}

			return result;
		}

		private void makeSchemaDef(ref SchemaBuilder sb, string name, string description)
		{
			sb.SetReadAccessLevel(AccessLevel.Public);
			sb.SetWriteAccessLevel(AccessLevel.Public);
			sb.SetVendorId(Util.GetVendorId());
			sb.SetSchemaName(name);
			sb.SetDocumentation(description);
		}

		private void makeSchemaFields<T>(ref SchemaBuilder sbld, SchemaDictionaryBase<T> fieldList) where T : Enum
		{
			foreach (KeyValuePair<T, ISchemaFieldDef<T>> kvp in fieldList)
			{
				makeSchemaField(ref sbld, kvp.Value);
			}
		}

		private void makeSchemaField<T>(ref SchemaBuilder sbld, ISchemaFieldDef<T> fieldDef) where T : Enum
		{
			Type t = fieldDef.ValueType;

			FieldBuilder fb = sbld.AddSimpleField(fieldDef.Name, fieldDef.ValueType);

			fb.SetDocumentation(fieldDef.Desc);

			if (fieldDef.UnitType != RevitUnitType.UT_UNDEFINED)
			{
				fb.SetUnitType((UnitType) (int) fieldDef.UnitType);
			}
		}

		private void makeSchemaSubSchemaFields(string key, ref SchemaBuilder sb,  SchemaRootAppFields raFields)
		{

			int qty = scList[key].QtySubSchema;

			for (int i = 0; i < qty; i++)
			{
				Tuple<string, Guid> subS = raFields.SubSchemaField();

				FieldBuilder fb = sb.AddSimpleField(subS.Item1, typeof(Entity));

				fb.SetDocumentation(raFields.GetValue<string>(SchemaRootAppKey.RAK_DESCRIPTION));
				fb.SetSubSchemaGUID(subS.Item2);

				scList[key].SubSchemaFields.Add(subS.Item1, subS.Item2);
			}
		}

/*	
		private void makeSubSchemasFields(Entity entity, Schema schema, SchemaCellFields cFields)
		{
			foreach (KeyValuePair<string, string> kvp in xCell.SubSchemaFields)
			{
				Field f = schema.GetField(kvp.Key);

				Schema subSchema  = makeSubSchema(kvp.Value, xCell);

				Entity subE = new Entity(subSchema);

				entity.Set(f, subE);
			}
		}

	private Schema makeSubSchema(string guid, SchemaCellFields cFields)
		{
			SchemaBuilder sb = new SchemaBuilder(new Guid(guid));

			makeSchemaDef(ref sb, xCell.Name, xCell.Description);

			makeSchemaFields(ref sb, xCell.Fields);

			return sb.Finish();
		}
*/

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