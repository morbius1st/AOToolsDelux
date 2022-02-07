#region using directives

using System;
using System.Collections.Generic;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using CSToolsDelux.Fields.SchemaInfo.SchemaData;
using CSToolsDelux.Utility;
using SharedCode.Fields.SchemaInfo.SchemaSupport;
using SharedCode.Fields.SchemaInfo.SchemaFields.FieldsTemplates;

using SharedCode.Fields.SchemaInfo.SchemaDefinitions;

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

			public schemaItem(string dsKey, int qtySubSchema) : this(dsKey, qtySubSchema, null) { }

			public schemaItem(string dsKey, int qtySubSchema, Schema schema)
			{
				DocumentKey = dsKey;
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

			public schemaItem this[string dsKey]
			{
				get
				{
					if (items.ContainsKey(dsKey))
					{
						return items[dsKey];
					}

					return null;
				}
			}

			public void AddNew(string dsKey, int qtySubSchema)
			{
				schemaItem si = new schemaItem(dsKey, qtySubSchema);

				items.Add(si.DocumentKey, si);
			}

			public Schema Find(string dsKey)
			{
				if (!items.ContainsKey(dsKey)) return null;

				return items[dsKey].Schema;
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

		public Schema FindSchema(string dsKey)
		{
			bool result;
			IList<Schema> schemas;
			result = findSchemasFromDoc(dsKey, out schemas);

			if (schemas.Count != 1) return null;

			return schemas[0];
		}

		/// <summary>
		/// get the list of schemas that match the dskey
		/// </summary>
		/// <param name="dsKey"></param>
		/// <param name="schemas"></param>
		/// <returns></returns>
		public bool FindSchemas(string dsKey, out IList<Schema> schemas)
		{
			bool result;
			schemas = new List<Schema>(1);

			// already got the schema?
			result = findSchemasFromList(dsKey, out schemas);

			if (result) return true;

			// don't already got
			// check the document

			result = findSchemasFromDoc(dsKey, out schemas);

			if (!result) return false;

			return true;
		}

		private bool findSchemasFromDoc(string dsKey, out IList<Schema> schemaList)
		{
			bool result = false;
			schemaList = new List<Schema>(1);

			IList<Schema> schemas = Schema.ListSchemas();

			foreach (Schema s in schemas)
			{
				if (s.SchemaName.Equals(dsKey))
				{
					schemaList.Add(s);

					result = true;
				}
			}

			return result;
		}

		private bool findSchemasFromList(string dsKey, out IList<Schema> schemaList)
		{
			bool result = false;
			Schema schema = null;

			schemaList = new List<Schema>(1);

			schema = scList.Find(dsKey);

			if (schema != null)
			{
				schemaList.Add(schema);
				result = true;
			}

			return result;
		}

	#endregion

	#region make schema

		public bool MakeRootSchema(string dsKey, SchemaRootData raData, int QtySubSchema)
		{
			if (raData == null || dsKey == null || QtySubSchema == 0) return false;

			scList.AddNew(dsKey, QtySubSchema);

			Schema schema = null;

			try
			{
				SchemaBuilder sb = new SchemaBuilder(Guid.NewGuid());

				makeSchemaDef(ref sb, raData.GetValue<string>(SchemaRootKey.RK_SCHEMA_NAME),
					raData.GetValue<string>(SchemaRootKey.RK_DESCRIPTION));

				makeSchemaFields(ref sb, raData.Fields);

				makeSchemaSubSchemaFields(dsKey, ref sb, raData);

				schema = sb.Finish();
			}
			catch (Exception e)
			{
				string ex = e.Message;
				string iex = e?.InnerException.Message ?? "none";
			}

			scList[dsKey].Schema = schema;

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

		private void makeSchemaFields<T>(ref SchemaBuilder sbld, FieldsTempDictionary<T> fieldList) where T : Enum
		{
			foreach (KeyValuePair<T, AFieldsMembers<T>> kvp in fieldList)
			{
				makeSchemaField(ref sbld, kvp.Value);
			}
		}

		private void makeSchemaField<T>(ref SchemaBuilder sbld, AFieldsMembers<T> aFieldsMembers) where T : Enum
		{
			Type t = aFieldsMembers.ValueType;

			FieldBuilder fb = sbld.AddSimpleField(aFieldsMembers.Name, aFieldsMembers.ValueType);

			fb.SetDocumentation(aFieldsMembers.Desc);

			if (aFieldsMembers.UnitType != FieldUnitType.UT_UNDEFINED)
			{
				fb.SetUnitType((UnitType) (int) aFieldsMembers.UnitType);
			}
		}

		private void makeSchemaSubSchemaFields(string dsKey, ref SchemaBuilder sb,  SchemaRootData raData)
		{
			int qty = scList[dsKey].QtySubSchema;

			for (int i = 0; i < qty; i++)
			{
				Tuple<string, Guid> subS = raData.FieldsRoot.SubSchemaField();

				FieldBuilder fb = sb.AddSimpleField(subS.Item1, typeof(Entity));

				fb.SetDocumentation(raData.GetValue<string>(SchemaRootKey.RK_DESCRIPTION));
				fb.SetSubSchemaGUID(subS.Item2);

				scList[dsKey].SubSchemaFields.Add(subS.Item1, subS.Item2);
			}
		}

		public void MakeSubSchemasFields(Entity entity, Schema schema, SchemaCellData cData)
		{
			int idx = 0;

			foreach (KeyValuePair<string, Guid> kvp in SchemaList[cData.DsKey].SubSchemaFields)
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

			makeSchemaDef(ref sb, cData.GetValue<string>( SchemaCellKey.CK_SCHEMA_NAME),
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