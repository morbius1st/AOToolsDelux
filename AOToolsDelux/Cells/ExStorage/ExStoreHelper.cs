#region + Using Directives

using AOToolsDelux.Cells.ExDataStorage;
using AOToolsDelux.Cells.SchemaCells;
using AOToolsDelux.Cells.SchemaDefinition;
using AOToolsDelux.Cells.Tests;
using AOToolsDelux.Utility;

using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;

using System;
using System.Collections.Generic;

using static AOToolsDelux.Cells.ExDataStorage.DataStorageManager;
using static Autodesk.Revit.DB.ExtensibleStorage.Schema;

#endregion

// user name: jeffs
// created:   7/4/2021 3:07:57 PM

namespace AOToolsDelux.Cells.ExStorage
{
	public enum ExStoreRtnCodes
	{
		XRC_NOT_CONFIG       = -17,
		XRC_IS_CONFIG           = -16,
		XRC_DS_NOT_EXIST		= -15,
		XRC_DS_EXISTS           = -10,
		XRC_APP_NOT_EXIST       = -9,
		XRC_ROOT_NOT_EXIST      = -8,
		XRC_EX_STORE_NOT_EXISTS	= -7,
		XRC_EX_STORE_EXISTS	    = -6,
		XRC_NOT_FOUND           = -5,
		XRC_TOO_MANY_OPEN_DOCS  = -4,
		XRC_DUPLICATE           = -3,
		XRC_NOT_INIT            = -2,
		XRC_FAIL                = -1,
		XRC_UNDEFINED			= 0,
		XRC_GOOD                = 1,
	}

	public class ExStoreHelper
	{
		public string OpDescription { get; private set; }

	#region root element

		// public Element AppRootElement { get; private set; }

		public ExStoreHelper()
		{
			// SetExStorageRootToBasePt();
		}

		// public void SetExStorageRootToElement(Element e)
		// {
		// 	AppRootElement = e;
		// }

		// public void SetExStorageRootToBasePt()
		// {
		// 	SetExStorageRootToElement(Util.GetProjectBasepoint());
		// }

		public bool GetRootEntity(ExStoreRoot xRoot, out Entity e, out DataStorage ds)
		{
			Schema schema = GetRootSchema();
			// makeRootSchema(xRoot);

			return DsMgr.GetDataStorage(schema, out e, out ds);
		}

		public bool GetAppEntity(ExStoreApp xApp, ExStoreCell xCell, out Entity e, out DataStorage ds)
		{
			ExStorageTests.location = 260;
			e = null;
			ds = null;

			Schema schema =
				MakeAppSchema(xApp, xCell);

			if (schema == null) return false;

			ExStorageTests.location = 265;
			return DsMgr.GetDataStorage(schema, out e, out ds);
		}

	#endregion

	#region read schema

		public ExStoreRtnCodes ReadRootData(ref ExStoreRoot xRoot)
		{
			Entity e;
			Schema s;

			ExStoreRtnCodes result = getRootSchemaAndEntity(out e, out s);

			if (result != ExStoreRtnCodes.XRC_GOOD) return result;

			result = ReadData<SchemaRootKey, SchemaDictionaryRoot, SchemaDictionaryRoot>(e, s, xRoot);

			if (result != ExStoreRtnCodes.XRC_GOOD) return result;

			return ExStoreRtnCodes.XRC_GOOD;
		}

		public ExStoreRtnCodes ReadAppData(ref ExStoreApp xApp)
		{
			Entity e;
			Schema s;

			ExStoreRtnCodes result = getAppSchemaAndEntity(out e, out s);

			if (result != ExStoreRtnCodes.XRC_GOOD) return result;

			result = ReadData<SchemaAppKey, SchemaDictionaryApp, SchemaDictionaryApp>(e, s, xApp);

			if (result != ExStoreRtnCodes.XRC_GOOD) return result;

			return ExStoreRtnCodes.XRC_GOOD;
		}

		public ExStoreRtnCodes ReadCellData(ref ExStoreCell xCell)
		{
			OpDescription = "Read Cells SubSchema";
			Entity eApp;
			Schema sApp;
			ExStoreRtnCodes result;

			List<Entity> subEntities;

			result = getAppSchemaAndEntity(out eApp, out sApp);

			if (result != ExStoreRtnCodes.XRC_GOOD) return result;

			result = getSubEntities(eApp, sApp, out subEntities);

			if (subEntities.Count > 0)
			{
				result = readCellData(ref xCell, subEntities);

				if (result != ExStoreRtnCodes.XRC_GOOD) return ExStoreRtnCodes.XRC_FAIL;
			}

			return ExStoreRtnCodes.XRC_GOOD;
		}

		private ExStoreRtnCodes readCellData(ref ExStoreCell xCell, List<Entity> subE)
		{
			ExStoreRtnCodes result;

			xCell = ExStoreCell.Instance();

			for (var i = 0; i < subE.Count; i++)
			{
				// xCell.Data[i] = xCell.DefaultValues();
				xCell.AddDefault();

				foreach (KeyValuePair<SchemaCellKey, SchemaFieldDef<SchemaCellKey>> kvp in xCell.Fields)
				{
					SchemaCellKey key = kvp.Value.Key;
					string fieldName = kvp.Value.Name;
					Field f = subE[i].Schema.GetField(fieldName);
					if (f == null) return ExStoreRtnCodes.XRC_FAIL;

					Type t = f.ValueType;
					if (t == null) return ExStoreRtnCodes.XRC_FAIL;


					if (t?.Equals(typeof(string)) ?? false)
					{
						xCell.Data[i][key].Value = subE[i].Get<string>(fieldName);
					}
					else if (t?.Equals(typeof(double)) ?? false)
					{
						// int i1 = subE[i].Get<int>(fieldName);
						double d2 = subE[i].Get<double>(fieldName, DisplayUnitType.DUT_GENERAL);

						xCell.Data[i][key].Value = subE[i].Get<double>(fieldName, DisplayUnitType.DUT_GENERAL);
					}
					else if (t?.Equals(typeof(bool)) ?? false)
					{
						xCell.Data[i][key].Value = subE[i].Get<bool>(fieldName);
					}
				}
			}

			return ExStoreRtnCodes.XRC_GOOD;
		}

		private ExStoreRtnCodes ReadData<TE, TT, TD>(Entity e, Schema s, IExStoreData<TT, TD> xStoreData)
			where TE : Enum where TT : SchemaDictionaryBase<TE>  where TD : SchemaDictionaryBase<TE>
		{
			foreach (KeyValuePair<TE, SchemaFieldDef<TE>> kvp in xStoreData.Data)
			{
				TE key = kvp.Value.Key;
				string fieldName = kvp.Value.Name;
				Field f = s.GetField(fieldName);
				if (f == null) return ExStoreRtnCodes.XRC_FAIL;

				Type t = f.ValueType;
				if (t == null) return ExStoreRtnCodes.XRC_FAIL;

				if (t?.Equals(typeof(string)) ?? false)
				{
					xStoreData.Data[key].Value = e.Get<string>(fieldName);
				}
				else if (t?.Equals(typeof(double)) ?? false)
				{
					xStoreData.Data[key].Value = e.Get<double>(fieldName);
				}
				else if (t?.Equals(typeof(bool)) ?? false)
				{
					xStoreData.Data[key].Value = e.Get<bool>(fieldName);
				}
			}

			xStoreData.IsDefault = false;

			return ExStoreRtnCodes.XRC_GOOD;
		}

	#endregion

	#region update methods

		/*
		 * update method
		 * get new data
		 * erase schema
		 * save new data
		 */

		public ExStoreRtnCodes UpdateCellData(ExStoreApp xApp, ExStoreCell xCell, DataStorage ds)
		{
			ExStoreRtnCodes result;

			result = DeleteAppSchema();

			if (result != ExStoreRtnCodes.XRC_GOOD) return result;

			result = WriteAppAndCellsData(xApp, xCell, ds);

			if (result != ExStoreRtnCodes.XRC_GOOD) return result;

			return ExStoreRtnCodes.XRC_GOOD;
		}

	#endregion

	#region save methods

		/// <summary>
		/// Write the data into the entity per the schema <br/>
		/// 
		/// </summary>
		/// <param name="xRoot"></param>
		/// <returns></returns>
		// public ExStoreRtnCodes WriteRootData(ExStoreRoot xRoot, Entity e)
		public ExStoreRtnCodes WriteRootData(ExStoreRoot xRoot, DataStorage ds)
		{
			// SchemaGuidManager.SetUniqueAppGuidSubStr();
			// xRoot.Data[SchemaRootKey.APP_GUID].Value = SchemaGuidManager.AppGuidString;

			Transaction t = null;

			try
			{
				// SchemaBuilder sb = new SchemaBuilder(xRoot.ExStoreGuid);
				//
				// makeSchemaDef(sb, xRoot.Name, xRoot.Description);
				//
				// makeSchemaFields(sb, xRoot.Data);
				//
				// Schema schema = sb.Finish();

				Schema schema = GetRootSchema();
				// makeRootSchema(xRoot);

				Entity e = new Entity(schema);

				writeData(e, e.Schema, xRoot.Data);

				// using (t = new Transaction(AppRibbon.Doc, "Save Cells Default Config Info"))
				// {
				// 	t.Start();
				ds.SetEntity(e);
				// 	t.Commit();
				// }
			}
			catch (InvalidOperationException ex)
			{
				if (t != null && t.HasStarted())
				{
					t.RollBack();
					t.Dispose();
				}

				if (ex.HResult == -2146233088)
				{
					return ExStoreRtnCodes.XRC_DUPLICATE;
				}

				return ExStoreRtnCodes.XRC_FAIL;
			}

			return ExStoreRtnCodes.XRC_GOOD;
		}

		public ExStoreRtnCodes WriteAppAndCellsData(ExStoreApp xApp, ExStoreCell xCell,  DataStorage ds)
		{
			Transaction t = null;

			try
			{
				// SchemaBuilder sb = new SchemaBuilder(xApp.ExStoreGuid);
				//
				// makeSchemaDef(sb, xApp.Name, xApp.Description);
				//
				// makeSchemaFields(sb, xApp.Data);
				//
				// makeSchemaSubSchemaFields(sb, xCell);

				// Schema schema = sb.Finish();
				Schema schema =
					MakeAppSchema(xApp, xCell);
				// GetAppSchemaCurr();

				Entity e = new Entity(schema);

				makeSubSchemasFields(e, schema, xCell);

				writeData(e, schema, xApp.Data);
				writeCellData(e, schema, xCell);

				// using (t = new Transaction(AppRibbon.Doc, "Save Cells Default Config Info"))
				// {
				// 	t.Start();
				ds.SetEntity(e);
				// 	t.Commit();
				// }
			}
			catch (InvalidOperationException ex)
			{
				if (t != null && t.HasStarted())
				{
					t.RollBack();
					t.Dispose();
				}

				if (ex.HResult == -2146233088)
				{
					return ExStoreRtnCodes.XRC_DUPLICATE;
				}

				return ExStoreRtnCodes.XRC_FAIL;
			}

			return ExStoreRtnCodes.XRC_GOOD;
		}

		/// <summary>
		/// write the data into the entity based on the schema <br/>
		/// the schema has already been added to the entity
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="entity"></param>
		/// <param name="schema"></param>
		/// <param name="data"></param>
		private void writeData<T>(Entity entity, Schema schema, SchemaDictionaryBase<T> data) where T : Enum
		{
			foreach (KeyValuePair<T, SchemaFieldDef<T>> kvp in data)
			{
				Field f = schema.GetField(kvp.Value.Name);
				if (f == null || !f.IsValidObject) continue;

				if (kvp.Value.UnitType != RevitUnitType.UT_UNDEFINED)
				{
					entity.Set(f, kvp.Value.Value, DisplayUnitType.DUT_GENERAL);
				}
				else
				{
					entity.Set(f, kvp.Value.Value);
				}
			}
		}

		private void writeCellData(Entity entity, Schema schema, ExStoreCell xCell)
		{
			int j = 0;

			foreach (KeyValuePair<string, string> kvp in xCell.SubSchemaFields)
			{
				Field f = schema.GetField(kvp.Key);

				Entity subE = entity.Get<Entity>(kvp.Key);

				writeData(subE, subE.Schema, xCell.Data[j++]);

				entity.Set(f, subE);
			}
		}

		public Schema MakeRootSchema(ExStoreRoot xRoot)
		{
			SchemaBuilder sb = new SchemaBuilder(xRoot.ExStoreGuid);

			makeSchemaDef(ref sb, xRoot.Name, xRoot.Description);

			makeSchemaFields(ref sb, xRoot.Data);

			Schema schema = sb.Finish();

			return schema;
		}

		public Schema MakeAppSchema(ExStoreApp xApp, ExStoreCell xCell)
		{

			ExStorageTests.location = 270;
			Schema schema = null;

			try
			{
				// schema = Schema.Lookup(xApp.ExStoreGuid);
				//
				// if (schema != null) return schema;

				SchemaBuilder sb = new SchemaBuilder(xApp.ExStoreGuid);

				ExStorageTests.location = 273;
				makeSchemaDef(ref sb, xApp.Name, xApp.Description);

				ExStorageTests.location = 275;
				makeSchemaFields(ref sb, xApp.Data);

				if (xCell != null)
				{
					ExStorageTests.location = 277;
					makeSchemaSubSchemaFields(ref sb, xCell);
				}

				ExStorageTests.location = 278;
				schema = sb.Finish();

			}
			catch (Exception e)
			{
				string ex = e.Message;
				string iex = e?.InnerException.Message ?? "none";
				
				return null;
			}

			ExStorageTests.location = 279;
			return schema;
		}

		public Schema GetRootSchema()
		{
			return DsMgr[DataStoreIdx.ROOT_DATA_STORE].Schema;
		}

		// public Schema GetAppSchemaCurr()
		// {
		// 	return DsMgr[DataStoreIdx.APP_DATA_STORE_CURR].Schema;
		// }

	#endregion


	#region make schema

		private void makeSchemaDef(ref SchemaBuilder sb, string name, string description)
		{
			sb.SetReadAccessLevel(AccessLevel.Public);
			sb.SetWriteAccessLevel(AccessLevel.Public);
			sb.SetVendorId(Util.GetVendorId());
			sb.SetSchemaName(name);
			sb.SetDocumentation(description);
		}

		private void makeSchemaFields<T>(ref SchemaBuilder sbld, 
			SchemaDictionaryBase<T> fieldList) where T : Enum
		{
			foreach (KeyValuePair<T, SchemaFieldDef<T>> kvp in fieldList)
			{
				makeSchemaField(ref sbld, kvp.Value);
			}
		}

		private void makeSchemaField<T>(ref SchemaBuilder sbld, SchemaFieldDef<T> fieldDef) where T : Enum
		{
			Type t = fieldDef.Value.GetType();

			FieldBuilder fbld = sbld.AddSimpleField(
				fieldDef.Name, fieldDef.Value.GetType());

			fbld.SetDocumentation(fieldDef.Desc);

			if (fieldDef.UnitType != RevitUnitType.UT_UNDEFINED)
			{
				fbld.SetUnitType((UnitType) (int) fieldDef.UnitType);
			}
		}

		private void makeSchemaSubSchemaFields(ref SchemaBuilder sb,  ExStoreCell xCell)
		{
			xCell.SubSchemaFields = new Dictionary<string, string>();

			int count = xCell.Data.Count;
			count = count == 0 ? 3 : count;


			for (int i = 0; i < xCell.Data.Count; i++)
			{
				SchemaFieldDef<SchemaAppKey> subS = SchemaDefinitionApp.GetSubSchemaDef(i);

				string guid = subS.Guid;
				string name = subS.Name;

				FieldBuilder fb = sb.AddSimpleField(name, typeof(Entity));

				fb.SetDocumentation(subS.Desc);
				fb.SetSubSchemaGUID(new Guid(guid));

				xCell.SubSchemaFields.Add(name, guid);
			}
		}

		private void makeSubSchemasFields(Entity entity, Schema schema, ExStoreCell xCell)
		{
			foreach (KeyValuePair<string, string> kvp in xCell.SubSchemaFields)
			{
				Field f = schema.GetField(kvp.Key);

				Schema subSchema  = makeSubSchema(kvp.Value, xCell);

				Entity subE = new Entity(subSchema);

				entity.Set(f, subE);
			}
		}

		private Schema makeSubSchema(string guid, ExStoreCell xCell)
		{
			SchemaBuilder sb = new SchemaBuilder(new Guid(guid));

			makeSchemaDef(ref sb, xCell.Name, xCell.Description);

			makeSchemaFields(ref sb, xCell.Fields);

			return sb.Finish();
		}

	#endregion

	#region delete methods

		public Tuple<int, int> DeleteSchemaPerVendorId(string vendorId)
		{
			IList<Schema> schemas = Schema.ListSchemas();

			int found = schemas.Count;
			int erased = 0;

			foreach (Schema s in schemas)
			{
				if (s.VendorId.Equals(vendorId))
				{
					EraseSchemaAndAllEntities(s, false);
					erased++;
				}
			}

			return new Tuple<int, int>(found, erased);

		}


		// app entity / schema is unique per model
		/// <summary>
		/// Delete the Root entity / schema<br/>
		/// Whether the app entity / schema exists
		/// </summary>
		/// <param name="eRoot"></param>
		/// <param name="sRoot"></param>
		/// <returns></returns>
		public ExStoreRtnCodes DeleteRootSchema()
		{
			if (AppRibbon.App.Documents.Size != 1) return ExStoreRtnCodes.XRC_TOO_MANY_OPEN_DOCS;

			OpDescription = "Delete Cells Root Schema";
			Entity eRoot;
			Schema sRoot;
			ExStoreRtnCodes result;

			result = getRootSchemaAndEntity(out eRoot, out sRoot);

			if (result != ExStoreRtnCodes.XRC_GOOD) return result;

			// removed the check for existence of
			// app entity / schema - check before using
			// this method

			// Entity eApp;
			// Schema sApp;
			//
			// // this cannot be found - it must be deleted first
			// result = GetAppSchemaAndEntity(out eApp, out sApp);
			//
			// if (result == ExStoreRtnCodes.GOOD) return ExStoreRtnCodes.FAIL;

			result = EraseSchema(eRoot, sRoot);

			return result;
		}

		// app entity / schema is unique per model
		/// <summary>
		/// Delete the App entity / schema <br/>
		/// Verifies that there are no app subentities / subschema
		/// </summary>
		/// <param name="eApp"></param>
		/// <param name="sApp"></param>
		/// <returns></returns>
		public ExStoreRtnCodes DeleteAppSchema()
		{
			OpDescription = "Delete Cells App Schema";
			Entity eApp;
			Schema sApp;
			ExStoreRtnCodes result;

			result = getAppSchemaAndEntity(out eApp, out sApp);

			if (result != ExStoreRtnCodes.XRC_GOOD) return result;

			List<Entity> subEntities;

			result = EraseSchema(eApp, sApp);

			return result;
		}


		// common method to remove a schema using
		// a transaction
		private ExStoreRtnCodes EraseSchema(Entity e, Schema s)
		{
			Transaction t = null;
			ExStoreRtnCodes result;

			try
			{
				// using (t = new Transaction(AppRibbon.Doc, OpDescription))
				// {
				// 	t.Start();

				EraseSchemaAndAllEntities(s, false);

				e.Dispose();
				// }
			}
			catch
			{
				// if (t != null && t.HasStarted())
				// {
				// 	t.RollBack();
				// 	t.Dispose();
				// }

				return ExStoreRtnCodes.XRC_FAIL;
			}

			return ExStoreRtnCodes.XRC_GOOD;
		}

	#endregion

	#region utility methods

		private ExStoreRtnCodes getRootSchemaAndEntity(out Entity eRoot, out Schema sRoot)
		{
			eRoot = null;
			sRoot = Schema.Lookup(SchemaGuidManager.RootGuid);

			// if (!sRoot?.IsValidObject ?? true) return ExStoreRtnCodes.XRC_NOT_FOUND;
			//
			// if (!DsMgr[DataStoreIdx.ROOT_DATA_STORE].GotDataStorage) return ExStoreRtnCodes.XRC_NOT_INIT;

			eRoot = DsMgr[DataStoreIdx.ROOT_DATA_STORE].DataStorage.GetEntity(sRoot);

			if (!eRoot?.IsValidObject ?? true) return ExStoreRtnCodes.XRC_FAIL;

			return ExStoreRtnCodes.XRC_GOOD;
		}

		private ExStoreRtnCodes getAppSchemaAndEntity(out Entity eApp, out Schema sApp)
		{
			eApp = null;
			sApp = Schema.Lookup(SchemaGuidManager.AppGuid);

			// if (!(sApp?.IsValidObject ?? false)) return ExStoreRtnCodes.XRC_NOT_FOUND;
			//
			// if (!DsMgr[DataStoreIdx.APP_DATA_STORE].GotDataStorage) return ExStoreRtnCodes.XRC_NOT_INIT;

			eApp = DsMgr[DataStoreIdx.APP_DATA_STORE_CURR].DataStorage.GetEntity(sApp);

			if (!(eApp?.IsValidObject ?? false)) return ExStoreRtnCodes.XRC_FAIL;

			return ExStoreRtnCodes.XRC_GOOD;
		}

		// get a list of entities that are associated to the provided entity based 
		// on the schema provided
		private ExStoreRtnCodes getSubEntities(Entity eApp, Schema sApp, out List<Entity> list)
		{
			list = new List<Entity>();

			if (eApp == null || sApp == null) return ExStoreRtnCodes.XRC_FAIL;

			foreach (Field field in sApp.ListFields())
			{
				if (field.SubSchema == null) continue;

				Field f = sApp.GetField(field.FieldName);
				if (!f?.IsValidObject ?? true) continue;

				Entity ent = eApp.Get<Entity>(f);
				if (!ent?.IsValidObject ?? true) continue;

				list.Add(ent);
			}

			return ExStoreRtnCodes.XRC_GOOD;
		}

	#endregion

		// public ExStoreRtnCodes UpdateExStorageCellData(ExStoreCell xCell)
		// {
		// 	Transaction t = null;
		// 	try
		// 	{
		// 		Entity e;
		// 		Schema s;
		//
		// 		ExStoreRtnCodes result = GetRootSchemaAndEntity(out e, out s);
		//
		// 		if (result != ExStoreRtnCodes.GOOD) return result;
		//
		// 		// SaveAppData2(e, s, xApp);
		// 		// SaveCellData2(e, s, xCell);
		//
		// 		List<Entity> subSchemaEnt = GetExistSubSchemaFields(e, s, xCell);
		//
		// 		if ((subSchemaEnt?.Count ?? 0) < 1) return ExStoreRtnCodes.FAIL;
		//
		// 		// using (t = new Transaction(AppRibbon.Doc, "Save Cells Default Config Info"))
		// 		// {
		// 		// 	t.Start();
		// 		// 	AppRootElement.SetEntity(e);
		// 		// 	t.Commit();
		// 		// }
		// 	}
		// 	catch (InvalidOperationException ex)
		// 	{
		// 		if (t != null && t.HasStarted())
		// 		{
		// 			t.RollBack();
		// 			t.Dispose();
		// 		}
		//
		// 		if (ex.HResult == -2146233088)
		// 		{
		// 			return ExStoreRtnCodes.DUPLICATE;
		// 		}
		//
		// 		return ExStoreRtnCodes.FAIL;
		// 	}
		//
		// 	return ExStoreRtnCodes.GOOD;
		// }
		//
		//
		// private List<Entity> GetExistSubSchemaFields(Entity entity, Schema schema, ExStoreCell xCell)
		// {
		// 	List<Entity> subSchemaEnt = new List<Entity>();
		//
		// 	IList<Field> fields = schema.ListFields();
		//
		// 	foreach (Field field in fields)
		// 	{
		// 		if (!field?.IsValidObject ?? true) continue;
		//
		// 		Type t1 = field.ValueType;
		// 		Type t2 = typeof(Entity);
		//
		// 		bool b1 = t1.Equals(t2);
		//
		// 		if (field.ValueType != typeof(Entity)) continue;
		//
		// 		Entity e = entity.Get<Entity>(field);
		// 		if (e == null || !e.IsValidObject) { continue; }
		//
		// 		subSchemaEnt.Add(e);
		// 	}
		//
		// 	return subSchemaEnt;
		// }


		// since cannot delete cell schema independent of the app schema
		// don't need to get subschema fields

		// // get a list of entities that are associated to the provided entity based 
		// // on the schema provided
		// public ExStoreRtnCodes GetSubSchemaFields(Entity eApp, Schema sApp, out List<Field> list)
		// {
		// 	list = new List<Field>();
		//
		// 	if (eApp == null || sApp == null) return ExStoreRtnCodes.FAIL;
		//
		// 	IList<Field> fx = sApp.ListFields();
		//
		// 	foreach (Field field in sApp.ListFields())
		// 	{
		// 		if (field.SubSchema == null) continue;
		//
		// 		Field f = sApp.GetField(field.FieldName);
		// 		if (!f?.IsValidObject ?? true) continue;
		// 		
		// 		list.Add(f);
		// 	}
		//
		// 	return ExStoreRtnCodes.GOOD;
		// }


		// private bool GetAppSchema(out Schema schema)
		// {
		// 	schema = null;
		//
		// 	schema = Schema.Lookup(SchemaGuidManager.RootGuid);
		//
		// 	if (schema == null || !schema.IsValidObject)
		// 	{
		// 		return false;
		// 	}
		//
		// 	return true;
		// }

		/*
		 * delete types
		 * delete root schema
		 * delete app schema
		 * delete a cell schema
		 * delete all cell schema
		 *
		 * to delete root schema, cannot have an app schema (& should be the only open document - by others)
		 * to delete app schema, cannot have any cell schema
		 *
		 * to delete cell schema, get a list of associated entities
		 */

		// public ExStoreRtnCodes DeleteSchema(Entity e, Schema s,
		// 	DelExStoreOpCode op, string desc)
		// {
		// 	Transaction t = null;
		// 	ExStoreRtnCodes result;
		//
		// 	try
		// 	{
		// 		using (t = new Transaction(AppRibbon.Doc, desc))
		// 		{
		// 			t.Start();
		//
		// 			switch (op)
		// 			{
		// 			// case DelExStoreOpCode.DEL_OP_ROOT:
		// 			// 	{
		// 			// 		result = DeleteRootSchema(e, s);
		// 			// 		break;
		// 			// 	}
		// 			case DelExStoreOpCode.DEL_OP_APP:
		// 				{
		// 					result = DeleteAppSchema(e, s);
		// 					break;
		// 				}
		// 			case DelExStoreOpCode.DEL_OP_SUB:
		// 				{
		// 					result = DeleteAppSubSchema(e, s);
		// 					break;
		// 				}
		// 			}
		//
		// 			t.Commit();
		// 		}
		// 	}
		// 	catch
		// 	{
		// 		if (t != null && t.HasStarted())
		// 		{
		// 			t.RollBack();
		// 			t.Dispose();
		// 		}
		//
		// 		return ExStoreRtnCodes.FAIL;
		// 	}
		//
		// 	return ExStoreRtnCodes.GOOD;
		// }


		// cannot delete subscheme independant of the app schema

		// /// <summary>
		// /// Delete a list of entities and their associated schema
		// /// </summary>
		// /// <param name="eApp">App Entity</param>
		// /// <param name="sApp">App Schema</param>
		// /// <returns></returns>
		// // private ExStoreRtnCodes DeleteAppSubSchema(Entity eApp, Schema sApp)
		// public ExStoreRtnCodes DeleteAppSubSchema()
		// {
		// 	OpDescription = "Delete Cells SubSchema";
		// 	Entity eApp;
		// 	Schema sApp;
		// 	ExStoreRtnCodes result;
		//
		// 	List<Field> subSchemaFields;
		// 	List<Entity> es;
		//
		// 	result = GetAppSchemaAndEntity(out eApp, out sApp);
		//
		// 	if (result != ExStoreRtnCodes.GOOD) return result;
		//
		// 	result = GetSubSchemaFields(eApp, sApp, out subSchemaFields);
		// 	result = GetSubEntities(eApp, sApp, out es);
		//
		// 	if (subSchemaFields.Count > 0)
		// 	{
		// 		Field f = subSchemaFields[0];
		// 		Schema s = subSchemaFields[0].Schema;
		// 		Schema ss = subSchemaFields[0].SubSchema;
		//
		// 		Entity e = es[0];
		// 		Schema s2 = e.Schema;
		//
		// 		result = EraseSchema(eApp, subSchemaFields[0].SubSchema);
		//
		// 		if (result != ExStoreRtnCodes.GOOD) return ExStoreRtnCodes.FAIL;
		// 	}
		//
		// 	return ExStoreRtnCodes.GOOD;
		// }
		//
		// // delete a list of entities and their associated schema
		// private ExStoreRtnCodes DeleteSchemaEntities(List<Entity> entities)
		// {
		// 	ExStoreRtnCodes result;
		//
		// 	
		//
		// 	for (int i = entities.Count - 1; i >= 0 ; i--)
		// 	{
		//
		// 		// result = EraseSchema(entities[i], entities[i].Schema);
		//
		// 		// if (result != ExStoreRtnCodes.GOOD) return result;
		// 	}
		//
		// 	return ExStoreRtnCodes.GOOD;
		// }


		//
		// private void MakeSubSchemas3(Entity entity, Schema schema, ExStoreCell xCell)
		// {
		// 	// foreach (KeyValuePair<string, string> kvp in xCell.SubSchemaFields)
		// 	// {
		// 		KeyValuePair<string, string> kvp2 = xCell.SubSchemaFields.First();
		//
		// 		Field f = schema.GetField(kvp2.Key);
		//
		// 		Schema subSchema  = MakeSubSchema2(kvp2.Value, xCell);
		//
		// 		Entity subE = new Entity(subSchema);
		//
		// 		entity.Set(f, subE);
		// 	// }
		// }


		// #region save methods
		//
		// 	public SaveRtnCodes SaveExStorageData2(ExStoreApp xApp, ExStoreCell xCell)
		// 	{
		// 		Transaction t = null;
		//
		// 		try
		// 		{
		// 			Schema schema = MakeRootSchema(xApp, xCell);
		//
		// 			Entity e = new Entity(schema);
		//
		// 			// DataStore.SchemaDS = schema;
		// 			// DataStore.EntityDS = e;
		//
		// 			SaveAppData(e, schema, xApp);
		// 			SaveCellData(e, schema, xCell);
		//
		// 			using (t = new Transaction(AppRibbon.Doc, "Unit Style Settings"))
		// 			{
		// 				t.Start();
		// 				ExStorageRoot.SetEntity(e);
		// 				t.Commit();
		// 			}
		// 		}
		// 		catch (InvalidOperationException ex)
		// 		{
		// 			if (t != null && t.HasStarted())
		// 			{
		// 				t.RollBack();
		// 				t.Dispose();
		// 			}
		//
		// 			if (ex.HResult == -2146233088)
		// 			{
		// 				return SaveRtnCodes.DUPLICATE;
		// 			}
		//
		// 			return SaveRtnCodes.FAIL;
		// 		}
		//
		// 		return SaveRtnCodes.GOOD;
		// 	}
		//
		//
		// 	private Schema MakeRootSchema(ExStoreApp xApp, ExStoreCell xCell)
		// 	{
		// 		SchemaBuilder sb = MakeSchema(xApp.Name, xApp.Description, xApp.AppSchemaGuid);
		//
		// 		MakeSchemaFields(sb, xApp.FieldDefs);
		//
		// 		xCell.SubSchemaFields = MakeSubSchemaFields(sb, xCell.Data.Count);
		//
		// 		return sb.Finish();
		// 		//
		// 		// return new Entity(schema);
		// 	}
		//
		// 	private Schema MakeCellSchema(string guid, ExStoreCell xCell, int idx)
		// 	{
		// 		SchemaBuilder sb = MakeSchema(xCell.Name, xCell.Description, new Guid(guid));
		//
		// 		MakeSchemaFields(sb, xCell.Data[idx]);
		//
		// 		return sb.Finish();
		// 	}
		//
		//
		// 	private SchemaBuilder MakeSchema(string name, string description, Guid guid)
		// 	{
		// 		SchemaBuilder sbld = new SchemaBuilder(guid);
		//
		// 		sbld.SetReadAccessLevel(AccessLevel.Public);
		// 		sbld.SetWriteAccessLevel(AccessLevel.Vendor);
		// 		sbld.SetVendorId(Util.GetVendorId());
		// 		sbld.SetSchemaName(name);
		// 		sbld.SetDocumentation(description);
		//
		// 		return sbld;
		// 	}
		//
		// 	private void SaveAppData(Entity entity, Schema schema, ExStoreApp xApp)
		// 	{
		// 		SaveData(entity, schema, xApp.Data);
		// 	}
		//
		// 	private void SaveCellData(Entity entity, Schema schema, ExStoreCell xCell)
		// 	{
		// 		int j = 0;
		//
		// 		foreach (KeyValuePair<string, string> kvp in xCell.SubSchemaFields)
		// 		{
		// 			Field f = schema.GetField(kvp.Key);
		//
		// 			Schema subSchema  = MakeCellSchema(kvp.Value, xCell, j);
		//
		// 			Entity subE = new Entity(subSchema);
		//
		// 			SaveData(subE, subSchema, xCell.Data[j]);
		//
		// 			entity.Set(f, subE);
		//
		// 			j++;
		//
		// 		}
		// 	}
		//
		// 	private void SaveData<TT>(Entity entity, Schema schema, SchemaDictionaryBase<TT> data)
		// 	{
		// 		// Schema s = entity.Schema;
		//
		// 		foreach (KeyValuePair<TT, SchemaFieldDef> kvp in data)
		// 		{
		// 			Field f = schema.GetField(kvp.Value.Name);
		// 			if (f == null || !f.IsValidObject) continue;
		//
		// 			if (kvp.Value.UnitType != RevitUnitType.UT_UNDEFINED)
		// 			{
		// 				entity.Set(f, kvp.Value.Value, DisplayUnitType.DUT_GENERAL);
		// 			}
		// 			else
		// 			{
		// 				entity.Set(f, kvp.Value.Value);
		// 			}
		// 		}
		// 	}
		//
		//
		//
		//
		//
		//
		// 	private void MakeSchemaFields<T>(SchemaBuilder sbld, SchemaDictionaryBase<T> fieldList)
		// 	{
		// 		foreach (KeyValuePair<T, SchemaFieldDef> kvp in fieldList)
		// 		{
		// 			MakeSchemaField(sbld, kvp.Value);
		// 		}
		// 	}
		//
		// 	private void MakeSchemaField(SchemaBuilder sbld, SchemaFieldDef fieldDef)
		// 	{
		// 		FieldBuilder fbld = sbld.AddSimpleField(
		// 			fieldDef.Name, fieldDef.Value.GetType());
		//
		// 		fbld.SetDocumentation(fieldDef.Desc);
		//
		// 		if (fieldDef.UnitType != RevitUnitType.UT_UNDEFINED)
		// 		{
		// 			fbld.SetUnitType((UnitType) (int) fieldDef.UnitType);
		// 		}
		// 	}
		//
		//
		// 	private Dictionary<string, string> MakeSubSchemaFields(SchemaBuilder sb, int count)
		// 	{
		// 		Dictionary<string, string> subSchemaFields = new Dictionary<string, string>();
		//
		// 		for (int i = 0; i < count; i++)
		// 		{
		// 			SchemaFieldDef subS = SchemaDefApp.GetSubSchemaDef(i);
		//
		// 			string guid = subS.Guid;
		// 			string name = subS.Name;
		//
		// 			FieldBuilder fb = sb.AddSimpleField(name, typeof(Entity));
		//
		// 			fb.SetDocumentation(subS.Desc);
		// 			fb.SetSubSchemaGUID(new Guid(guid));
		//
		// 			subSchemaFields.Add(name, guid);
		// 		}
		//
		// 		return subSchemaFields;
		// 	}
		// 	
		//
		// #endregion


		//
		// public ExStoreRtnCodes SaveExStorageData2(ExStoreApp xApp, ExStoreCell xCell)
		// {
		// 	Transaction t = null;
		//
		// 	Schema schema = null;
		//
		// 	if (!GetAppSchema(out schema))
		// 	{
		// 		return ExStoreRtnCodes.FAIL;
		// 	}
		//
		// 	Entity e = AppRootElement.GetEntity(schema);
		//
		// 	try
		// 	{
		// 		SaveAppData2(e, schema, xApp);
		// 		SaveCellData2(e, schema, xCell);
		//
		//
		// 		using (t = new Transaction(AppRibbon.Doc, "Save Cells Config Info"))
		// 		{
		// 			t.Start();
		// 			AppRootElement.SetEntity(e);
		// 			t.Commit();
		// 		}
		// 	}
		// 	catch 
		// 	{
		// 		if (t != null && t.HasStarted())
		// 		{
		// 			t.RollBack();
		// 			t.Dispose();
		// 		}
		//
		// 		return ExStoreRtnCodes.FAIL;
		// 	}
		// 	return ExStoreRtnCodes.GOOD;
		// }


		//
		// public ExStoreRtnCodes AddSubExStorage(string name, string desc, string guid,
		// 	SchemaDictionaryCell xCellFields)
		// {
		// 	Entity e;
		// 	Schema s;
		//
		// 	ExStoreRtnCodes result = GetExStoreRoot(out e, out s);
		//
		// 	if (result != ExStoreRtnCodes.GOOD) return result;
		//
		//
		//
		//
		//
		// 	return ExStoreRtnCodes.GOOD;
		// }
		//
		//
		// public ExStoreRtnCodes ConfigAppExStorage3(string name, string desc, 
		// 	SchemaDictionaryApp xAppFields)
		// {
		// 	Transaction t = null;
		//
		// 	try
		// 	{
		// 		SchemaBuilder sb = MakeSchema3(name, desc, SchemaGuidManager.AppGuid);
		//
		// 		MakeSchemaFields3(sb, xAppFields);
		//
		// 		// Tuple<string, string, string> subSchemaFields = MakeSubSchemaFields3(sb);
		//
		// 		Schema schema = sb.Finish();
		//
		// 		Entity e = new Entity(schema);
		//
		// 		SaveData3(e, schema, xAppFields);
		//
		// 		// MakeSubSchema3(e, schema, xCellFields, subSchemaFields);
		// 		//
		// 		// SaveAppData2(e, schema, xApp);
		// 		// SaveCellData2(e, schema, xCell);
		//
		// 		using (t = new Transaction(AppRibbon.Doc, "Save Cells Default Config Info"))
		// 		{
		// 			t.Start();
		// 			AppRootElement.SetEntity(e);
		// 			t.Commit();
		// 		}
		// 	}
		// 	catch (InvalidOperationException ex)
		// 	{
		// 		if (t != null && t.HasStarted())
		// 		{
		// 			t.RollBack();
		// 			t.Dispose();
		// 		}
		//
		// 		if (ex.HResult == -2146233088)
		// 		{
		// 			return ExStoreRtnCodes.DUPLICATE;
		// 		}
		//
		// 		return ExStoreRtnCodes.FAIL;
		// 	}
		//
		// 	return ExStoreRtnCodes.GOOD;
		// }
		//
		// private SchemaBuilder MakeSchema3(string name, string description, Guid guid)
		// {
		// 	SchemaBuilder sb = new SchemaBuilder(guid);
		//
		// 	sb.SetReadAccessLevel(AccessLevel.Public);
		// 	sb.SetWriteAccessLevel(AccessLevel.Public);
		// 	sb.SetVendorId(Util.GetVendorId());
		// 	sb.SetSchemaName(name);
		// 	sb.SetDocumentation(description);
		//
		// 	return sb;
		// }
		//
		//
		// private Tuple<string, string, string> MakeSubSchemaFields3(SchemaBuilder sb)
		// {
		// 	SchemaFieldDef subS = SchemaDefApp.GetSubSchemaDef(0);
		//
		// 	string guid = subS.Guid;
		// 	string name = subS.Name;
		// 	string desc = subS.Desc;
		//
		// 	FieldBuilder fb = sb.AddSimpleField(name, typeof(Entity));
		//
		// 	fb.SetDocumentation(subS.Desc);
		// 	fb.SetSubSchemaGUID(new Guid(guid));
		//
		// 	return new Tuple<string, string, string>(name, desc, guid);;
		// }
		//
		//
		// private void  MakeSubSchema3<T>(Entity entity, Schema schema, SchemaDictionaryBase<T> fieldList,
		// 	Tuple<string, string, string> subSchemaFields)
		// {
		// 	Field f = schema.GetField(subSchemaFields.Item1);
		//
		// 	if (f == null || !f.IsValidObject) return;
		//
		// 	Schema subSchema  = MakeCellSchema3(subSchemaFields.Item3, fieldList, subSchemaFields);
		//
		// 	Entity subE = new Entity(subSchema);
		//
		// 	entity.Set(f,subE);
		// }
		//
		//
		// private Schema MakeCellSchema3<T>(string guid, SchemaDictionaryBase<T> fieldList, 
		// 	Tuple<string, string, string> subSchemaFields)
		// {
		// 	SchemaBuilder sb = MakeSchema3(subSchemaFields.Item1, subSchemaFields.Item2, new Guid(guid));
		//
		// 	MakeSchemaFields3(sb, fieldList);
		//
		// 	return sb.Finish();
		// }
		//
		//
		// private void MakeSchemaFields3<T>(SchemaBuilder sbld, SchemaDictionaryBase<T> fieldList)
		// {
		// 	foreach (KeyValuePair<T, SchemaFieldDef> kvp in fieldList)
		// 	{
		// 		MakeSchemaField3(sbld, kvp.Value);
		// 	}
		// }
		//
		// private void MakeSchemaField3(SchemaBuilder sbld, SchemaFieldDef fieldDef)
		// {
		// 	Type t = fieldDef.Value.GetType();
		//
		// 	FieldBuilder fbld = sbld.AddSimpleField(
		// 		fieldDef.Name, fieldDef.Value.GetType());
		//
		// 	fbld.SetDocumentation(fieldDef.Desc);
		//
		// 	if (fieldDef.UnitType != RevitUnitType.UT_UNDEFINED)
		// 	{
		// 		fbld.SetUnitType((UnitType) (int) fieldDef.UnitType);
		// 	}
		// }
		//
		//
		// private void SaveData3<TT>(Entity entity, Schema schema, SchemaDictionaryBase<TT> data)
		// {
		// 	// Schema s = entity.Schema;
		//
		// 	foreach (KeyValuePair<TT, SchemaFieldDef> kvp in data)
		// 	{
		// 		Field f = schema.GetField(kvp.Value.Name);
		// 		if (f == null || !f.IsValidObject) continue;
		//
		// 		if (kvp.Value.UnitType != RevitUnitType.UT_UNDEFINED)
		// 		{
		// 			entity.Set(f, kvp.Value.Value, DisplayUnitType.DUT_GENERAL);
		// 		}
		// 		else
		// 		{
		// 			entity.Set(f, kvp.Value.Value);
		// 		}
		// 	}
		// }
		//
		//
		//
		// private void SaveAppData3(Entity entity, Schema schema, ExStoreApp xApp)
		// {
		// 	SaveData3(entity, schema, xApp.Data);
		// }
		//
		// private void SaveCellData3(Entity entity, Schema schema, ExStoreCell xCell)
		// {
		// 	int j = 0;
		//
		// 	foreach (KeyValuePair<string, string> kvp in xCell.SubSchemaFields)
		// 	{
		// 		Field f = schema.GetField(kvp.Key);
		//
		// 		Entity subE = entity.Get<Entity>(kvp.Key);
		//
		// 		SaveData3(subE, subE.Schema, xCell.Data[j++]);
		//
		// 		entity.Set(f, subE);
		// 	}
		// }
		//
	}
}