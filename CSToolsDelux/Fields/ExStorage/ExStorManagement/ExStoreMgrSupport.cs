#region + Using Directives

using System;
using System.Collections.Generic;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using CSToolsDelux.Fields.SchemaInfo.SchemaDefinitions;
using CSToolsDelux.Fields.SchemaInfo.SchemaFields;
using CSToolsDelux.Utility;
using static Autodesk.Revit.DB.ExtensibleStorage.Schema;

#endregion

// user name: jeffs
// created:   7/4/2021 3:07:57 PM

namespace CSToolsDelux.Fields.ExStorage.ExStorManagement
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

	public class ExStoreMgrSupport
	{
		public string OpDescription { get; private set; }

	#region root element

		// public Element AppRootElement { get; private set; }

		public ExStoreMgrSupport()
		{
			
		}

	#endregion










	#region schema

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
			foreach (KeyValuePair<T, ISchemaFieldDef<T>> kvp in fieldList)
			{
				makeSchemaField(ref sbld, kvp.Value);
			}
		}

		private void makeSchemaField<T>(ref SchemaBuilder sbld, 
			ISchemaFieldDef<T> fieldDef) where T : Enum
		{
			Type t = fieldDef.ValueType;

			FieldBuilder fbld = sbld.AddSimpleField(
				fieldDef.Name, fieldDef.ValueType);

			fbld.SetDocumentation(fieldDef.Desc);

			if (fieldDef.UnitType != RevitUnitType.UT_UNDEFINED)
			{
				fbld.SetUnitType((UnitType) (int) fieldDef.UnitType);
			}
		}
/*
		private void makeSchemaSubSchemaFields(ref SchemaBuilder sb,  SchemaCellFields xCell)
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

*/


	#endregion




	//
	// 	public void SetExStorageRootToElement(Element e)
	// 	{
	// 		AppRootElement = e;
	// 	}
	//
	// 	public void SetExStorageRootToBasePt()
	// 	{
	// 		SetExStorageRootToElement(Util.GetProjectBasepoint());
	// 	}
	// 	
	// 	public bool GetRootEntity(ExStoreRoot xRoot, out Entity e, out DataStorage ds)
	// 	{
	// 		Schema schema = GetRootSchema();
	// 			// makeRootSchema(xRoot);
	// 	
	// 		return DsMgr.GetDataStorage(schema, out e, out ds);
	// 	}
	// 	
	// 	public bool GetAppEntity(ExStoreApp xApp, ExStoreCell xCell, out Entity e, out DataStorage ds)
	// 	{
	// 		Schema schema =
	// 			MakeAppSchema(xApp, xCell);
	// 			// GetAppSchemaCurr();
	// 	
	// 		return DsMgr.GetDataStorage(schema, out e, out ds);
	// 	}
	//
	//
	//
	// #region read schema
	//
	// #endregion
	//
	// 	
	// 		public ExStoreRtnCodes ReadRootData(ref ExStoreRoot xRoot)
	// 		{
	// 			Entity e;
	// 			Schema s;
	// 	
	// 			ExStoreRtnCodes result = getRootSchemaAndEntity(out e, out s);
	// 	
	// 			if (result != ExStoreRtnCodes.XRC_GOOD) return result;
	// 	
	// 			result = ReadData<SchemaRootKey, SchemaDictionaryRoot, SchemaDictionaryRoot>(e, s, xRoot);
	// 	
	// 			if (result != ExStoreRtnCodes.XRC_GOOD) return result;
	// 	
	// 			return ExStoreRtnCodes.XRC_GOOD;
	// 		}
	// 	
	// 		public ExStoreRtnCodes ReadAppData(ref ExStoreApp xApp)
	// 		{
	// 			Entity e;
	// 			Schema s;
	// 	
	// 			ExStoreRtnCodes result = getAppSchemaAndEntity(out e, out s);
	// 	
	// 			if (result != ExStoreRtnCodes.XRC_GOOD) return result;
	// 	
	// 			result = ReadData<SchemaAppKey, SchemaDictionaryApp, SchemaDictionaryApp>(e, s, xApp);
	// 	
	// 			if (result != ExStoreRtnCodes.XRC_GOOD) return result;
	// 	
	// 			return ExStoreRtnCodes.XRC_GOOD;
	// 		}
	// 	
	// 		public ExStoreRtnCodes ReadCellData(ref ExStoreCell xCell)
	// 		{
	// 			OpDescription = "Read Cells SubSchema";
	// 			Entity eApp;
	// 			Schema sApp;
	// 			ExStoreRtnCodes result;
	// 	
	// 			List<Entity> subEntities;
	// 	
	// 			result = getAppSchemaAndEntity(out eApp, out sApp);
	// 	
	// 			if (result != ExStoreRtnCodes.XRC_GOOD) return result;
	// 	
	// 			result = getSubEntities(eApp, sApp, out subEntities);
	// 	
	// 			if (subEntities.Count > 0)
	// 			{
	// 				result = readCellData(ref xCell, subEntities);
	// 	
	// 				if (result != ExStoreRtnCodes.XRC_GOOD) return ExStoreRtnCodes.XRC_FAIL;
	// 			}
	// 	
	// 			return ExStoreRtnCodes.XRC_GOOD;
	// 		}
	// 	
	// 		private ExStoreRtnCodes readCellData(ref ExStoreCell xCell, List<Entity> subE)
	// 		{
	// 			ExStoreRtnCodes result;
	// 	
	// 			xCell = ExStoreCell.Instance();
	// 	
	// 			for (var i = 0; i < subE.Count; i++)
	// 			{
	// 				// xCell.Data[i] = xCell.DefaultValues();
	// 				xCell.AddDefault();
	// 	
	// 				foreach (KeyValuePair<SchemaCellKey, SchemaFieldDef<SchemaCellKey>> kvp in xCell.Fields)
	// 				{
	// 					SchemaCellKey key = kvp.Value.Key;
	// 					string fieldName = kvp.Value.Name;
	// 					Field f = subE[i].Schema.GetField(fieldName);
	// 					if (f == null) return ExStoreRtnCodes.XRC_FAIL;
	// 	
	// 					Type t = f.ValueType;
	// 					if (t == null) return ExStoreRtnCodes.XRC_FAIL;
	// 	
	// 	
	// 					if (t?.Equals(typeof(string)) ?? false)
	// 					{
	// 						xCell.Data[i][key].Value = subE[i].Get<string>(fieldName);
	// 					}
	// 					else if (t?.Equals(typeof(double)) ?? false)
	// 					{
	// 						// int i1 = subE[i].Get<int>(fieldName);
	// 						double d2 = subE[i].Get<double>(fieldName, DisplayUnitType.DUT_GENERAL);
	// 	
	// 						xCell.Data[i][key].Value = subE[i].Get<double>(fieldName, DisplayUnitType.DUT_GENERAL);
	// 					}
	// 					else if (t?.Equals(typeof(bool)) ?? false)
	// 					{
	// 						xCell.Data[i][key].Value = subE[i].Get<bool>(fieldName);
	// 					}
	// 				}
	// 			}
	// 	
	// 			return ExStoreRtnCodes.XRC_GOOD;
	// 		}
	// 	
	// 		private ExStoreRtnCodes ReadData<TE, TT, TD>(Entity e, Schema s, IExStoreData<TT, TD> xStoreData)
	// 			where TE : Enum where TT : SchemaDictionaryBase<TE>  where TD : SchemaDictionaryBase<TE>
	// 		{
	// 			foreach (KeyValuePair<TE, SchemaFieldDef<TE>> kvp in xStoreData.Data)
	// 			{
	// 				TE key = kvp.Value.Key;
	// 				string fieldName = kvp.Value.Name;
	// 				Field f = s.GetField(fieldName);
	// 				if (f == null) return ExStoreRtnCodes.XRC_FAIL;
	// 	
	// 				Type t = f.ValueType;
	// 				if (t == null) return ExStoreRtnCodes.XRC_FAIL;
	// 	
	// 				if (t?.Equals(typeof(string)) ?? false)
	// 				{
	// 					xStoreData.Data[key].Value = e.Get<string>(fieldName);
	// 				}
	// 				else if (t?.Equals(typeof(double)) ?? false)
	// 				{
	// 					xStoreData.Data[key].Value = e.Get<double>(fieldName);
	// 				}
	// 				else if (t?.Equals(typeof(bool)) ?? false)
	// 				{
	// 					xStoreData.Data[key].Value = e.Get<bool>(fieldName);
	// 				}
	// 			}
	// 	
	// 			xStoreData.IsDefault = false;
	// 	
	// 			return ExStoreRtnCodes.XRC_GOOD;
	// 		}
	// 	
	// 	#endregion
	// 	
	// 	#region update methods
	// 	
	// 		 /*
	// 		 * update method
	// 		 * get new data
	// 		 * erase schema
	// 		 * save new data
	// 		 */ 
	// 	
	// 		public ExStoreRtnCodes UpdateCellData(ExStoreApp xApp, ExStoreCell xCell, DataStorage ds)
	// 		{
	// 			ExStoreRtnCodes result;
	// 	
	// 			result = DeleteAppSchema();
	// 	
	// 			if (result != ExStoreRtnCodes.XRC_GOOD) return result;
	// 	
	// 			result = WriteAppAndCellsData(xApp, xCell, ds);
	// 	
	// 			if (result != ExStoreRtnCodes.XRC_GOOD) return result;
	// 	
	// 			return ExStoreRtnCodes.XRC_GOOD;
	// 		}
	// 	
	// 	#endregion
	// 	
	// 	#region save methods
	// 	
	// 		/// <summary>
	// 		/// Write the data into the entity per the schema <br/>
	// 		/// 
	// 		/// </summary>
	// 		/// <param name="xRoot"></param>
	// 		/// <returns></returns>
	// 		// public ExStoreRtnCodes WriteRootData(ExStoreRoot xRoot, Entity e)
	// 		public ExStoreRtnCodes WriteRootData(ExStoreRoot xRoot, DataStorage ds)
	// 		{
	// 			// SchemaGuidManager.SetUniqueAppGuidSubStr();
	// 			// xRoot.Data[SchemaRootKey.APP_GUID].Value = SchemaGuidManager.AppGuidString;
	// 	
	// 			Transaction t = null;
	// 	
	// 			try
	// 			{
	// 				// SchemaBuilder sb = new SchemaBuilder(xRoot.ExStoreGuid);
	// 				//
	// 				// makeSchemaDef(sb, xRoot.Name, xRoot.Description);
	// 				//
	// 				// makeSchemaFields(sb, xRoot.Data);
	// 				//
	// 				// Schema schema = sb.Finish();
	// 	
	// 				Schema schema = GetRootSchema();
	// 					// makeRootSchema(xRoot);
	// 				
	// 				Entity e = new Entity(schema);
	// 	
	// 				writeData(e, e.Schema, xRoot.Data);
	// 	
	// 				// using (t = new Transaction(AppRibbon.Doc, "Save Cells Default Config Info"))
	// 				// {
	// 				// 	t.Start();
	// 					ds.SetEntity(e);
	// 				// 	t.Commit();
	// 				// }
	// 			}
	// 			catch (InvalidOperationException ex)
	// 			{
	// 				if (t != null && t.HasStarted())
	// 				{
	// 					t.RollBack();
	// 					t.Dispose();
	// 				}
	// 	
	// 				if (ex.HResult == -2146233088)
	// 				{
	// 					return ExStoreRtnCodes.XRC_DUPLICATE;
	// 				}
	// 	
	// 				return ExStoreRtnCodes.XRC_FAIL;
	// 			}
	// 	
	// 			return ExStoreRtnCodes.XRC_GOOD;
	// 		}
	// 	
	// 		public ExStoreRtnCodes WriteAppAndCellsData(ExStoreApp xApp, ExStoreCell xCell,  DataStorage ds)
	// 		{
	// 			Transaction t = null;
	// 	
	// 			try
	// 			{
	// 				// SchemaBuilder sb = new SchemaBuilder(xApp.ExStoreGuid);
	// 				//
	// 				// makeSchemaDef(sb, xApp.Name, xApp.Description);
	// 				//
	// 				// makeSchemaFields(sb, xApp.Data);
	// 				//
	// 				// makeSchemaSubSchemaFields(sb, xCell);
	// 	
	// 				// Schema schema = sb.Finish();
	// 				Schema schema = 
	// 					MakeAppSchema(xApp, xCell);
	// 					// GetAppSchemaCurr();
	// 	
	// 				Entity e = new Entity(schema);
	// 	
	// 				makeSubSchemasFields(e, schema, xCell);
	// 	
	// 				writeData(e, schema, xApp.Data);
	// 				writeCellData(e, schema, xCell);
	// 	
	// 				// using (t = new Transaction(AppRibbon.Doc, "Save Cells Default Config Info"))
	// 				// {
	// 				// 	t.Start();
	// 					ds.SetEntity(e);
	// 				// 	t.Commit();
	// 				// }
	// 			}
	// 			catch (InvalidOperationException ex)
	// 			{
	// 				if (t != null && t.HasStarted())
	// 				{
	// 					t.RollBack();
	// 					t.Dispose();
	// 				}
	// 	
	// 				if (ex.HResult == -2146233088)
	// 				{
	// 					return ExStoreRtnCodes.XRC_DUPLICATE;
	// 				}
	// 	
	// 				return ExStoreRtnCodes.XRC_FAIL;
	// 			}
	// 	
	// 			return ExStoreRtnCodes.XRC_GOOD;
	// 		}
	// 	
	// 		/// <summary>
	// 		/// write the data into the entity based on the schema <br/>
	// 		/// the schema has already been added to the entity
	// 		/// </summary>
	// 		/// <typeparam name="T"></typeparam>
	// 		/// <param name="entity"></param>
	// 		/// <param name="schema"></param>
	// 		/// <param name="data"></param>
	// 		private void writeData<T>(Entity entity, Schema schema, SchemaDictionaryBase<T> data) where T : Enum
	// 		{
	// 			foreach (KeyValuePair<T, SchemaFieldDef<T>> kvp in data)
	// 			{
	// 				Field f = schema.GetField(kvp.Value.Name);
	// 				if (f == null || !f.IsValidObject) continue;
	// 	
	// 				if (kvp.Value.UnitType != RevitUnitType.UT_UNDEFINED)
	// 				{
	// 					entity.Set(f, kvp.Value.Value, DisplayUnitType.DUT_GENERAL);
	// 				}
	// 				else
	// 				{
	// 					entity.Set(f, kvp.Value.Value);
	// 				}
	// 			}
	// 		}
	// 	
	// 		private void writeCellData(Entity entity, Schema schema, ExStoreCell xCell)
	// 		{
	// 			int j = 0;
	// 	
	// 			foreach (KeyValuePair<string, string> kvp in xCell.SubSchemaFields)
	// 			{
	// 				Field f = schema.GetField(kvp.Key);
	// 	
	// 				Entity subE = entity.Get<Entity>(kvp.Key);
	// 	
	// 				writeData(subE, subE.Schema, xCell.Data[j++]);
	// 	
	// 				entity.Set(f, subE);
	// 			}
	// 		}
	// 	
	// 		public Schema MakeRootSchema(ExStoreRoot xRoot)
	// 		{
	// 			SchemaBuilder sb = new SchemaBuilder(xRoot.ExStoreGuid);
	// 	
	// 			makeSchemaDef(ref sb, xRoot.Name, xRoot.Description);
	// 	
	// 			makeSchemaFields(ref sb, xRoot.Data);
	// 	
	// 			Schema schema = sb.Finish();
	// 	
	// 			return schema;
	// 		}
	// 	
	// 		public Schema MakeAppSchema(ExStoreApp xApp, ExStoreCell xCell)
	// 		{
	// 			Schema schema;
	// 	
	// 			// schema = Schema.Lookup(xApp.ExStoreGuid);
	// 			//
	// 			// if (schema != null) return schema;
	// 	
	// 			SchemaBuilder sb = new SchemaBuilder(xApp.ExStoreGuid);
	// 	
	// 			makeSchemaDef(ref sb, xApp.Name, xApp.Description);
	// 	
	// 			makeSchemaFields(ref sb, xApp.Data);
	// 	
	// 			if (xCell != null)
	// 			{
	// 				makeSchemaSubSchemaFields(ref sb, xCell);
	// 			}
	// 	
	// 			schema = sb.Finish();
	// 	
	// 			return schema;
	// 		}
	// 	
	// 		public Schema GetRootSchema()
	// 		{
	// 			return DsMgr[DataStoreIdx.ROOT_DATA_STORE].Schema;
	// 		}
	// 	
	// 		// public Schema GetAppSchemaCurr()
	// 		// {
	// 		// 	return DsMgr[DataStoreIdx.APP_DATA_STORE_CURR].Schema;
	// 		// }
	// 	
	// 	#endregion
	// 	
	// 	
	// 	#region make schema
	// 	
	// 		private void makeSchemaDef(ref SchemaBuilder sb, string name, string description)
	// 		{
	// 			sb.SetReadAccessLevel(AccessLevel.Public);
	// 			sb.SetWriteAccessLevel(AccessLevel.Public);
	// 			sb.SetVendorId(Util.GetVendorId());
	// 			sb.SetSchemaName(name);
	// 			sb.SetDocumentation(description);
	// 		}
	// 	
	// 		private void makeSchemaFields<T>(ref SchemaBuilder sbld, SchemaDictionaryBase<T> fieldList) where T : Enum
	// 		{
	// 			foreach (KeyValuePair<T, SchemaFieldDef<T>> kvp in fieldList)
	// 			{
	// 				makeSchemaField(ref sbld, kvp.Value);
	// 			}
	// 		}
	// 	
	// 		private void makeSchemaField<T>(ref SchemaBuilder sbld, SchemaFieldDef<T> fieldDef) where T : Enum
	// 		{
	// 			Type t = fieldDef.Value.GetType();
	// 	
	// 			FieldBuilder fbld = sbld.AddSimpleField(
	// 				fieldDef.Name, fieldDef.Value.GetType());
	// 	
	// 			fbld.SetDocumentation(fieldDef.Desc);
	// 	
	// 			if (fieldDef.UnitType != RevitUnitType.UT_UNDEFINED)
	// 			{
	// 				fbld.SetUnitType((UnitType) (int) fieldDef.UnitType);
	// 			}
	// 		}
	// 	
	// 		private void makeSchemaSubSchemaFields(ref SchemaBuilder sb,  ExStoreCell xCell)
	// 		{
	// 			xCell.SubSchemaFields = new Dictionary<string, string>();
	// 	
	// 			int count = xCell.Data.Count;
	// 			count = count == 0 ? 3 : count;
	// 	
	// 	
	// 			for (int i = 0; i < xCell.Data.Count; i++)
	// 			{
	// 				SchemaFieldDef<SchemaAppKey> subS = SchemaDefinitionApp.GetSubSchemaDef(i);
	// 	
	// 				string guid = subS.Guid;
	// 				string name = subS.Name;
	// 	
	// 				FieldBuilder fb = sb.AddSimpleField(name, typeof(Entity));
	// 	
	// 				fb.SetDocumentation(subS.Desc);
	// 				fb.SetSubSchemaGUID(new Guid(guid));
	// 	
	// 				xCell.SubSchemaFields.Add(name, guid);
	// 			}
	// 		}
	// 	
	// 		private void makeSubSchemasFields(Entity entity, Schema schema, ExStoreCell xCell)
	// 		{
	// 			foreach (KeyValuePair<string, string> kvp in xCell.SubSchemaFields)
	// 			{
	// 				Field f = schema.GetField(kvp.Key);
	// 	
	// 				Schema subSchema  = makeSubSchema(kvp.Value, xCell);
	// 	
	// 				Entity subE = new Entity(subSchema);
	// 	
	// 				entity.Set(f, subE);
	// 			}
	// 		}
	// 	
	// 		private Schema makeSubSchema(string guid, ExStoreCell xCell)
	// 		{
	// 			SchemaBuilder sb = new SchemaBuilder(new Guid(guid));
	// 	
	// 			makeSchemaDef(ref sb, xCell.Name, xCell.Description);
	// 	
	// 			makeSchemaFields(ref sb, xCell.Fields);
	// 	
	// 			return sb.Finish();
	// 		}
	// 	
	// 	#endregion
	// 	
	// 	#region delete methods
	// 	
	// 	
	// 		// app entity / schema is unique per model
	// 		/// <summary>
	// 		/// Delete the Root entity / schema<br/>
	// 		/// Whether the app entity / schema exists
	// 		/// </summary>
	// 		/// <param name="eRoot"></param>
	// 		/// <param name="sRoot"></param>
	// 		/// <returns></returns>
	// 		public ExStoreRtnCodes DeleteRootSchema()
	// 		{
	// 			if (AppRibbon.App.Documents.Size != 1) return ExStoreRtnCodes.XRC_TOO_MANY_OPEN_DOCS;
	// 	
	// 			OpDescription = "Delete Cells Root Schema";
	// 			Entity eRoot;
	// 			Schema sRoot;
	// 			ExStoreRtnCodes result;
	// 	
	// 			result = getRootSchemaAndEntity(out eRoot, out sRoot);
	// 	
	// 			if (result != ExStoreRtnCodes.XRC_GOOD) return result;
	// 	
	// 			// removed the check for existence of
	// 			// app entity / schema - check before using
	// 			// this method
	// 	
	// 			// Entity eApp;
	// 			// Schema sApp;
	// 			//
	// 			// // this cannot be found - it must be deleted first
	// 			// result = GetAppSchemaAndEntity(out eApp, out sApp);
	// 			//
	// 			// if (result == ExStoreRtnCodes.GOOD) return ExStoreRtnCodes.FAIL;
	// 	
	// 			result = EraseSchema(eRoot, sRoot);
	// 	
	// 			return result;
	// 		}
	// 	
	// 		// app entity / schema is unique per model
	// 		/// <summary>
	// 		/// Delete the App entity / schema <br/>
	// 		/// Verifies that there are no app subentities / subschema
	// 		/// </summary>
	// 		/// <param name="eApp"></param>
	// 		/// <param name="sApp"></param>
	// 		/// <returns></returns>
	// 		public ExStoreRtnCodes DeleteAppSchema()
	// 		{
	// 			OpDescription = "Delete Cells App Schema";
	// 			Entity eApp;
	// 			Schema sApp;
	// 			ExStoreRtnCodes result;
	// 	
	// 			result = getAppSchemaAndEntity(out eApp, out sApp);
	// 	
	// 			if (result != ExStoreRtnCodes.XRC_GOOD) return result;
	// 	
	// 			List<Entity> subEntities;
	// 	
	// 			result = EraseSchema(eApp, sApp);
	// 	
	// 			return result;
	// 		}
	// 	
	// 		
	// 		// common method to remove a schema using
	// 		// a transaction
	// 		private ExStoreRtnCodes EraseSchema(Entity e, Schema s)
	// 		{
	// 			Transaction t = null;
	// 			ExStoreRtnCodes result;
	// 	
	// 			try
	// 			{
	// 				// using (t = new Transaction(AppRibbon.Doc, OpDescription))
	// 				// {
	// 				// 	t.Start();
	// 	
	// 					EraseSchemaAndAllEntities(s, false);
	// 	
	// 					e.Dispose();
	// 				// }
	// 			}
	// 			catch
	// 			{
	// 				// if (t != null && t.HasStarted())
	// 				// {
	// 				// 	t.RollBack();
	// 				// 	t.Dispose();
	// 				// }
	// 	
	// 				return ExStoreRtnCodes.XRC_FAIL;
	// 			}
	// 	
	// 			return ExStoreRtnCodes.XRC_GOOD;
	// 		}
	// 	
	// 	#endregion
	//
	// #region utility methods
	//

	//
	//
	//
	// 		private ExStoreRtnCodes getRootSchemaAndEntity(out Entity eRoot, out Schema sRoot)
	// 		{
	// 			eRoot = null;
	// 			sRoot = Schema.Lookup(SchemaGuidManager.RootGuid);
	// 	
	// 			// if (!sRoot?.IsValidObject ?? true) return ExStoreRtnCodes.XRC_NOT_FOUND;
	// 			//
	// 			// if (!DsMgr[DataStoreIdx.ROOT_DATA_STORE].GotDataStorage) return ExStoreRtnCodes.XRC_NOT_INIT;
	// 	
	// 			eRoot = DsMgr[DataStoreIdx.ROOT_DATA_STORE].DataStorage.GetEntity(sRoot);
	// 	
	// 			if (!eRoot?.IsValidObject ?? true) return ExStoreRtnCodes.XRC_FAIL;
	// 	
	// 			return ExStoreRtnCodes.XRC_GOOD;
	// 		}
	// 	
	// 		private ExStoreRtnCodes getAppSchemaAndEntity(out Entity eApp, out Schema sApp)
	// 		{
	// 			eApp = null;
	// 			sApp = Schema.Lookup(SchemaGuidManager.AppGuid);
	// 	
	// 			// if (!(sApp?.IsValidObject ?? false)) return ExStoreRtnCodes.XRC_NOT_FOUND;
	// 			//
	// 			// if (!DsMgr[DataStoreIdx.APP_DATA_STORE].GotDataStorage) return ExStoreRtnCodes.XRC_NOT_INIT;
	// 	
	// 			eApp = DsMgr[DataStoreIdx.APP_DATA_STORE_CURR].DataStorage.GetEntity(sApp);
	// 	
	// 			if (!(eApp?.IsValidObject ?? false)) return ExStoreRtnCodes.XRC_FAIL;
	// 	
	// 			return ExStoreRtnCodes.XRC_GOOD;
	// 		}
	// 	
	// 		// get a list of entities that are associated to the provided entity based 
	// 		// on the schema provided
	// 		private ExStoreRtnCodes getSubEntities(Entity eApp, Schema sApp, out List<Entity> list)
	// 		{
	// 			list = new List<Entity>();
	// 	
	// 			if (eApp == null || sApp == null) return ExStoreRtnCodes.XRC_FAIL;
	// 	
	// 			foreach (Field field in sApp.ListFields())
	// 			{
	// 				
	// 				if (field.SubSchema == null) continue;
	// 	
	// 				Field f = sApp.GetField(field.FieldName);
	// 				if (!f?.IsValidObject ?? true) continue;
	// 				
	// 				Entity ent = eApp.Get<Entity>(f);
	// 				if (!ent?.IsValidObject ?? true) continue;
	// 	
	// 				list.Add(ent);
	// 			}
	// 	
	// 			return ExStoreRtnCodes.XRC_GOOD;
	// 		}
	//
	// #endregion
	}
}