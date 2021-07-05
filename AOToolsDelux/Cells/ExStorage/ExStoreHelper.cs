#region + Using Directives
using System;
using System.Collections.Generic;
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
	public enum SaveRtnCodes
	{
		DUPLICATE = -2,
		NOT_INIT  = -1,
		FAIL      = 0,
		GOOD      = 1,
	}

	public class ExStoreHelper
	{
	#region root element

		public Element ExStorageRoot { get; private set; }

		public ExStoreHelper()
		{
			SetExStorageRootToBasePt();
		}

		public void SetExStorageRootToElement(Element e)
		{
			ExStorageRoot = e;
		}

		public void SetExStorageRootToBasePt()
		{
			SetExStorageRootToElement(Util.GetProjectBasepoint());
		}

	#endregion

	#region save methods

		public SaveRtnCodes SaveExStorageData(ExStoreApp xApp, ExStoreCell xCell)
		{
			Transaction t = null;

			try
			{
				Entity e = MakeSchemaEntity(xApp, xCell);

				SaveData(e, xApp, xCell);

				
				using (t = new Transaction(AppRibbon.Doc, "Unit Style Settings"))
				{
					t.Start();
					ExStorageRoot.SetEntity(e);
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
					return SaveRtnCodes.DUPLICATE;
				}

				return SaveRtnCodes.FAIL;
			}

			return SaveRtnCodes.GOOD;
		}

		private void SaveData(Entity e, ExStoreApp xApp, ExStoreCell xCell)
		{
			SaveData(e, xApp.SchemaFields);

			foreach (SchemaDictionaryCell schemaDictionaryCell in xCell.Data)
			{
				SaveData(e, schemaDictionaryCell);
			}
		}

		private Entity MakeSchemaEntity(ExStoreApp xApp, ExStoreCell xCell)
		{
			Element element = ExStorageRoot;

			SchemaBuilder sb = CreateSchema(xApp.Name, xApp.Description, xApp.AppSchemaGuid);

			MakeSchemaFields(sb, xApp.SchemaFields);

			Dictionary<string, string> subSchemaFields = MakeSubSchemaFields(sb, xCell.Data.Count);

			Schema schema = sb.Finish();

			return new Entity(schema);
		}


		private void SaveData<TT>(Entity entity, SchemaDictionaryBase<TT> data)
		{
			Schema s = entity.Schema;

			foreach (KeyValuePair<TT, SchemaFieldDef> kvp in data)
			{
				Field f = s.GetField(kvp.Value.Name);
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

		// private void SaveData<TE, TT, TD>(Entity entity, TE exStorData) 
		// 	where TE : IExStoreData<TT, TD>
		// 	where TD : SchemaDictionaryBase<TT>
		// {
		// 	Schema s = entity.Schema;
		//
		// 	foreach (KeyValuePair<TT, SchemaFieldDef> kvp in exStorData.Data)
		// 	{
		// 		Field f = s.GetField(kvp.Value.Name);
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

		// private Entity MakeSchemaEntity<TT, TD>(string name, string desc, string guid, 
		// 	TD schemaFields) where TD : SchemaDictionaryBase<TT>
		// {
		// 	SchemaBuilder sbld = CreateSchema(name, desc, new Guid(guid));
		//
		// 	MakeSchemaFields(sbld, schemaFields);
		//
		// 	Schema schema = sbld.Finish();
		//
		// 	return new Entity(schema);
		// 	//
		// 	// SaveFieldValues(entity, schema, schemaFields);
		// 	//
		// 	// return entity;
		// }

		private void MakeSchemaFields<T>(SchemaBuilder sbld, SchemaDictionaryBase<T> fieldList)
		{
			foreach (KeyValuePair<T, SchemaFieldDef> kvp in fieldList)
			{
				MakeSchemaField(sbld, kvp.Value);
			}
		}

		private void MakeSchemaField(SchemaBuilder sbld, SchemaFieldDef fieldDef)
		{
			FieldBuilder fbld = sbld.AddSimpleField(
				fieldDef.Name, fieldDef.Value.GetType());

			fbld.SetDocumentation(fieldDef.Desc);

			if (fieldDef.UnitType != RevitUnitType.UT_UNDEFINED)
			{
				fbld.SetUnitType((UnitType) (int) fieldDef.UnitType);
			}
		}

		private SchemaBuilder CreateSchema(string name, string description, Guid guid)
		{
			SchemaBuilder sbld = new SchemaBuilder(guid);

			sbld.SetReadAccessLevel(AccessLevel.Public);
			sbld.SetWriteAccessLevel(AccessLevel.Vendor);
			sbld.SetVendorId(Util.GetVendorId());
			sbld.SetSchemaName(name);
			sbld.SetDocumentation(description);

			return sbld;
		}

		private Dictionary<string, string> MakeSubSchemaFields(SchemaBuilder sb, int count)
		{
			Dictionary<string, string> subSchemaFields = new Dictionary<string, string>();

			for (int i = 0; i < count; i++)
			{
				SchemaFieldDef subS = SchemaDefApp.GetSubSchemaDef(i);

				string guid = subS.Guid;
				string name = subS.Name;

				FieldBuilder fb = sb.AddSimpleField(name, typeof(Entity));

				fb.SetDocumentation(subS.Desc);
				fb.SetSubSchemaGUID(new Guid(guid));

				subSchemaFields.Add(name, guid);
			}

			return subSchemaFields;
		}
		

	#endregion







	}
}
