#region + Using Directives

using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AOTools.Cells.SchemaCells;
using AOTools.Cells.SchemaDefinition;
using AOTools.Utility;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;

#endregion

// user name: jeffs
// created:   7/4/2021 3:07:57 PM

namespace AOTools.Cells.ExStorage
{
	public enum ExStoreRtnCodes
	{
		DUPLICATE = -2,
		NOT_INIT  = -1,
		FAIL      = 0,
		GOOD      = 1,
	}

	public class ExStoreHelper
	{

	#region root element

		public Element AppRootElement { get; private set; }

		public ExStoreHelper()
		{
			SetExStorageRootToBasePt();
		}

		public void SetExStorageRootToElement(Element e)
		{
			AppRootElement = e;
		}

		public void SetExStorageRootToBasePt()
		{
			SetExStorageRootToElement(Util.GetProjectBasepoint());
		}

	#endregion

	#region read schema

		public ExStoreRtnCodes ReadRootExStorageData(ExStoreRoot xRoot)
		{
			Entity e;
			Schema s;

			ExStoreRtnCodes result = GetExStoreRootInfo(out e, out s);

			if (result != ExStoreRtnCodes.GOOD) return result;

			ReadData(e, s, xRoot);

			return ExStoreRtnCodes.GOOD;
		}


		private void ReadData<TT, TD>(Entity e, Schema s, IExStoreData<TT, TD> xStoreData)  
			where TT : SchemaDictionaryBase<string> where TD : SchemaDictionaryBase<string>
		{
			int j = 0;

			foreach (KeyValuePair<string, SchemaFieldDef> kvp in xStoreData.FieldDefs)
			{
				string fieldName = kvp.Value.FieldName;
				Field f = s.GetField(fieldName);
				Type t = f.ValueType;
				
				if (t?.Equals(typeof(string)) ?? false)
				{
					// xStoreData.Data[(TE) xStoreData.KeyOrder[j++]].Value =
					// 	e.Get<string>(fieldName);

					xStoreData.Data[fieldName].Value =
						e.Get<string>(fieldName);
				} 
				else if (t?.Equals(typeof(double)) ?? false)
				{
					// xStoreData.Data[(TE) xStoreData.KeyOrder[j++]].Value =
					// 	e.Get<double>(fieldName);

					xStoreData.Data[fieldName].Value =
						e.Get<double>(fieldName);
				}
			}
		}

		
		// private void ReadData2<TE, TT, TD>(Entity e, Schema s, IExStoreData<TT, TD> xStoreData)  
		// 	where TE : Enum where TT : SchemaDictionaryBase<TE> where TD : SchemaDictionaryBase<TE>
		// {
		// 	int j = 0;
		//
		// 	foreach (KeyValuePair<TE, SchemaFieldDef> kvp in xStoreData.FieldDefs)
		// 	{
		// 		string fieldName = kvp.Value.FieldName;
		// 		Field f = s.GetField(fieldName);
		// 		Type t = f.ValueType;
		//
		//
		// 		if (t?.Equals(typeof(string)) ?? false)
		// 		{
		// 			xStoreData.Data[(TE) xStoreData.KeyOrder[j++]].Value =
		// 				e.Get<string>(fieldName);
		// 		} 
		// 		else if (t?.Equals(typeof(double)) ?? false)
		// 		{
		// 			xStoreData.Data[(TE) xStoreData.KeyOrder[j++]].Value =
		// 				e.Get<double>(fieldName);
		// 		}
		// 	}
		// }







	#endregion

	#region update methods

		public ExStoreRtnCodes UpdateExStorageCellData(ExStoreCell xCell)
		{
			Transaction t = null;
			try
			{
				Entity e;
				Schema s;

				ExStoreRtnCodes result = GetExStoreRootInfo(out e, out s);

				if (result != ExStoreRtnCodes.GOOD) return result;

				// SaveAppData2(e, s, xApp);
				// SaveCellData2(e, s, xCell);

				List<Entity> subSchemaEnt = GetExistSubSchemaFields(e, s, xCell);

				if ((subSchemaEnt?.Count ?? 0) < 1) return ExStoreRtnCodes.FAIL;

				// using (t = new Transaction(AppRibbon.Doc, "Save Cells Default Config Info"))
				// {
				// 	t.Start();
				// 	AppRootElement.SetEntity(e);
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
					return ExStoreRtnCodes.DUPLICATE;
				}

				return ExStoreRtnCodes.FAIL;
			}

			return ExStoreRtnCodes.GOOD;
		}


		private List<Entity> GetExistSubSchemaFields(Entity entity, Schema schema, ExStoreCell xCell)
		{
			List<Entity> subSchemaEnt = new List<Entity>();

			IList<Field> fields = schema.ListFields();

			foreach (Field field in fields)
			{
				if (!field?.IsValidObject ?? true) continue;

				Type t1 = field.ValueType;
				Type t2 = typeof(Entity);

				bool b1 = t1.Equals(t2);

				if (field.ValueType != typeof(Entity)) continue;

				Entity e = entity.Get<Entity>(field);
				if (e == null || !e.IsValidObject) { continue; }

				subSchemaEnt.Add(e);
			}

			return subSchemaEnt;
		}

	#endregion

	#region root schema

		public ExStoreRtnCodes WriteRootExStorageData(ExStoreRoot xRoot)
		{
			SchemaGuidManager.SetUniqueAppGuidSubStr();
			// xRoot.Data[SchemaDefApp.Inst.AK_APP_GUID].Value = SchemaGuidManager.AppGuidUniqueString;
			string guid = SchemaGuidManager.AppGuidUniqueString;

			Transaction t = null;

			try
			{
				SchemaBuilder sb = new SchemaBuilder(xRoot.ExStoreGuid);

				MakeSchemaDef2(sb, xRoot.Name, xRoot.Description);

				MakeSchemaFields2(sb, xRoot.FieldDefs);

				Schema schema = sb.Finish();

				Entity e = new Entity(schema);

				SaveRootData2(e, schema, xRoot);

				using (t = new Transaction(AppRibbon.Doc, "Save Cells Default Config Info"))
				{
					t.Start();
					AppRootElement.SetEntity(e);
					t.Commit();
				}
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
					return ExStoreRtnCodes.DUPLICATE;
				}

				return ExStoreRtnCodes.FAIL;
			}

			return ExStoreRtnCodes.GOOD;
		}
		

	#endregion

	#region save methods

		public ExStoreRtnCodes SaveExStorageData(ExStoreApp xApp, ExStoreCell xCell)
		{
			Transaction t = null;

			try
			{
				SchemaBuilder sb = new SchemaBuilder(xApp.ExStoreGuid);

				MakeSchemaDef2(sb, xApp.Name, xApp.Description);

				MakeSchemaFields2(sb, xApp.FieldDefs);

				MakeSchemaSubSchemaFields2(sb, xCell);

				Schema schema = sb.Finish();

				Entity e = new Entity(schema);

				MakeSubSchemasFields2(e, schema, xCell);

				SaveAppData2(e, schema, xApp);
				SaveCellData2(e, schema, xCell);

				using (t = new Transaction(AppRibbon.Doc, "Save Cells Default Config Info"))
				{
					t.Start();
					AppRootElement.SetEntity(e);
					t.Commit();
				}
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
					return ExStoreRtnCodes.DUPLICATE;
				}

				return ExStoreRtnCodes.FAIL;
			}

			return ExStoreRtnCodes.GOOD;
		}

		
		private void SaveRootData2(Entity entity, Schema schema, ExStoreRoot xRoot)
		{
			SaveData2(entity, schema, xRoot.Data);
		}


		private void SaveAppData2(Entity entity, Schema schema, ExStoreApp xApp)
		{
			SaveData2(entity, schema, xApp.Data);
		}

		private void SaveCellData2(Entity entity, Schema schema, ExStoreCell xCell)
		{
			int j = 0;

			foreach (KeyValuePair<string, string> kvp in xCell.SubSchemaFields)
			{
				Field f = schema.GetField(kvp.Key);

				Entity subE = entity.Get<Entity>(kvp.Key);

				SaveData2(subE, subE.Schema, xCell.Data[j++]);

				entity.Set(f, subE);
			}
		}


		private void SaveData2(Entity entity, Schema schema, 
			SchemaDictionaryBase<string> data)
		{
			// for (int i = 0; i < keyOrder.Length; i++)
			// {
			// 	T key = keyOrder[i];
			// 	SchemaFieldDef< val = data[key];
			//
			// 	Field f = schema.GetField(val.Name);
			// 	if (f == null || !f.IsValidObject) continue;
			//
			// 	if (val.UnitType != RevitUnitType.UT_UNDEFINED)
			// 	{
			// 		entity.Set(f, val.Value, DisplayUnitType.DUT_GENERAL);
			// 	}
			// 	else
			// 	{
			// 		entity.Set(f, val.Value);
			// 	}
			// }


			foreach (KeyValuePair<string, SchemaFieldDef> kvp in data)
			{
				Field f = schema.GetField(kvp.Value.FieldName);
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

	#endregion

	#region make schema

		// private ExStoreRtnCodes MakeBaseSchema()
		// {
		//
		// }


		private void MakeSchemaDef2(SchemaBuilder sb, string name, string description)
		{
			sb.SetReadAccessLevel(AccessLevel.Public);
			sb.SetWriteAccessLevel(AccessLevel.Public);
			sb.SetVendorId(Util.GetVendorId());
			sb.SetSchemaName(name);
			sb.SetDocumentation(description);
		}

		private void MakeSchemaFields2(SchemaBuilder sbld, SchemaDictionaryBase<string> fieldList) 
		{
			foreach (KeyValuePair<string, SchemaFieldDef> kvp in fieldList)
			{
				MakeSchemaField2(sbld, kvp.Value);
			}
		}

		private void MakeSchemaField2(SchemaBuilder sbld, SchemaFieldDef fieldDef)
		{
			Type t = fieldDef.Value.GetType();

			FieldBuilder fbld = sbld.AddSimpleField(
				fieldDef.FieldName, fieldDef.Value.GetType());

			fbld.SetDocumentation(fieldDef.Desc);

			if (fieldDef.UnitType != RevitUnitType.UT_UNDEFINED)
			{
				fbld.SetUnitType((UnitType) (int) fieldDef.UnitType);
			}
		}

		private void MakeSchemaSubSchemaFields2(SchemaBuilder sb,  ExStoreCell xCell)
		{
			xCell.SubSchemaFields = new Dictionary<string, string>();

			for (int i = 0; i < xCell.Data.Count; i++)
			{
				string[] info = xCell.GetSubSchemaFieldInfo(i);

				FieldBuilder fb = sb.AddSimpleField(info[0], typeof(Entity));

				fb.SetDocumentation(info[2]);
				fb.SetSubSchemaGUID(new Guid(info[1]));

				xCell.SubSchemaFields.Add(info[0], info[1]);
			}
		}

		private void MakeSubSchemasFields2(Entity entity, Schema schema, ExStoreCell xCell)
		{
			foreach (KeyValuePair<string, string> kvp in xCell.SubSchemaFields)
			{
				Field f = schema.GetField(kvp.Key);

				Schema subSchema  = MakeSubSchema2(kvp.Value, xCell);

				Entity subE = new Entity(subSchema);

				entity.Set(f, subE);
			}
		}

		private Schema MakeSubSchema2(string guid, ExStoreCell xCell)
		{
			SchemaBuilder sb = new SchemaBuilder(new Guid(guid));

			MakeSchemaDef2(sb, xCell.Name, xCell.Description);

			MakeSchemaFields2(sb, xCell.FieldDefs);

			return sb.Finish();
		}

	#endregion


	#region utility methods

		private List<Tuple<Entity, Field>> GetSubSchemaEntities(Entity entity, Schema schema,  ExStoreCell xCell)
		{
			List<Tuple<Entity, Field>> subSchemaEntities = new List<Tuple<Entity, Field>>();

			foreach (Field field in schema.ListFields())
			{
				if (!field?.IsValidObject ?? true) continue;

				if (field.ValueType != typeof(Entity)) continue;

				Entity e = entity.Get<Entity>(field);

				if (!e?.IsValidObject ?? true) continue;

				subSchemaEntities.Add(new Tuple<Entity, Field>(e, field));
			}

			return subSchemaEntities;
		}


		private bool GetAppSchema(out Schema schema)
		{
			schema = null;

			schema = Schema.Lookup(SchemaGuidManager.RootGuid);

			if (schema == null || !schema.IsValidObject)
			{
				return false;
			}

			return true;
		}

		private ExStoreRtnCodes GetExStoreRootInfo(out Entity entity, out Schema schema)
		{
			entity = null;
			schema = Schema.Lookup(SchemaGuidManager.RootGuid);

			if (!schema?.IsValidObject ?? true) return ExStoreRtnCodes.NOT_INIT;

			if (!AppRootElement?.IsValidObject ?? true) return ExStoreRtnCodes.NOT_INIT;

			entity = AppRootElement.GetEntity(schema);

			if (!entity?.IsValidObject ?? true) return ExStoreRtnCodes.FAIL;

			return ExStoreRtnCodes.GOOD;
		}

	#endregion


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