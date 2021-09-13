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

			public schemaItem(string docKey, int qtySubSchema) : this(docKey, qtySubSchema, null) { }

			public schemaItem(string docKey, int qtySubSchema, Schema schema)
			{
				DocumentKey = docKey;
				Schema = schema;
				SubSchemaFields = new Dictionary<string, Guid>(qtySubSchema);
				QtySubSchema = qtySubSchema;
			}
		}

		public class schemaList
		{
			private Dictionary<string, schemaItem> items;

			public schemaList()
			{
				if (items == null) items = new Dictionary<string, schemaItem>();
			}

			public Dictionary<string, schemaItem> Items => items;

			public schemaItem this[string docKey]
			{
				get
				{
					if (items.ContainsKey(docKey))
					{
						return items[docKey];
					}

					return null;
				}
			}

			public void AddNew(string docKey, int qtySubSchema)
			{
				schemaItem si = new schemaItem(docKey, qtySubSchema);

				items.Add(si.DocumentKey, si);
			}

			public Schema Find(string docKey)
			{
				if (!items.ContainsKey(docKey)) return null;

				return items[docKey].Schema;
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

	#region find schema

		public Schema FindSchema(string docKey)
		{
			bool result;
			IList<Schema> schemas;
			result = findSchemasFromDoc(docKey, out schemas);

			if (schemas.Count != 1) return null;

			return schemas[0];
		}

		public bool FindSchemas(string docKey, out IList<Schema> schemas)
		{
			bool result;
			schemas = new List<Schema>(1);

			// already got the schema?
			result = findSchemasFromList(docKey, out schemas);

			if (result) return true;

			// don't already got
			// check the document

			result = findSchemasFromDoc(docKey, out schemas);

			if (!result) return false;

			return true;
		}


		private bool findSchemasFromDoc(string docKey, out IList<Schema> schemaList)
		{
			bool result = false;
			schemaList = new List<Schema>(1);

			IList<Schema> schemas = Schema.ListSchemas();

			foreach (Schema s in schemas)
			{
				if (s.SchemaName.Equals(docKey))
				{
					schemaList.Add(s);

					result = true;
				}
			}

			return result;
		}

		private bool findSchemasFromList(string docKey, out IList<Schema> schemaList)
		{
			bool result = false;
			Schema schema = null;

			schemaList = new List<Schema>(1);

			schema = scList.Find(docKey);

			if (schema != null)
			{
				schemaList.Add(schema);
				result = true;
			}

			return result;
		}

	#endregion

	#region make schema

		public bool MakeRootAppSchema(string docKey, SchemaRootAppData raData, int QtySubSchema)
		{
			if (raData == null || docKey == null || QtySubSchema == 0) return false;

			scList.AddNew(docKey, QtySubSchema);

			Schema schema = null;

			try
			{
				SchemaBuilder sb = new SchemaBuilder(Guid.NewGuid());

				makeSchemaDef(ref sb, raData.GetValue<string>(SchemaRootAppKey.RAK_NAME),
					raData.GetValue<string>(SchemaRootAppKey.RAK_DESCRIPTION));

				makeSchemaFields(ref sb, raData.Fields);

				makeSchemaSubSchemaFields(docKey, ref sb, raData);

				schema = sb.Finish();
			}
			catch (Exception e)
			{
				string ex = e.Message;
				string iex = e?.InnerException.Message ?? "none";
			}

			scList[docKey].Schema = schema;

			return true;
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

		private void makeSchemaSubSchemaFields(string docKey, ref SchemaBuilder sb,  SchemaRootAppData raData)
		{
			int qty = scList[docKey].QtySubSchema;

			for (int i = 0; i < qty; i++)
			{
				Tuple<string, Guid> subS = raData.AppFields.SubSchemaField();

				FieldBuilder fb = sb.AddSimpleField(subS.Item1, typeof(Entity));

				fb.SetDocumentation(raData.GetValue<string>(SchemaRootAppKey.RAK_DESCRIPTION));
				fb.SetSubSchemaGUID(subS.Item2);

				scList[docKey].SubSchemaFields.Add(subS.Item1, subS.Item2);
			}
		}

		public void MakeSubSchemasFields(Entity entity, Schema schema, SchemaCellData cData)
		{
			int idx = 0;

			foreach (KeyValuePair<string, Guid> kvp in SchemaList[cData.DocKey].SubSchemaFields)
			{
				Field f = schema.GetField(kvp.Key);

				Schema subSchema  = makeSubSchema(idx, kvp.Value, cData);

				Entity subE = new Entity(subSchema);

				entity.Set(f, subE);

				idx++;
			}
		}

		private Schema makeSubSchema(int idx, Guid guid, SchemaCellData cData)
		{
			SchemaBuilder sb = new SchemaBuilder(guid);

			cData.Index = idx;

			makeSchemaDef(ref sb, cData.GetValue<string>( SchemaCellKey.CK_NAME),
				cData.GetValue<string>( SchemaCellKey.CK_DESCRIPTION));

			makeSchemaFields(ref sb, cData.Fields);

			return sb.Finish();
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