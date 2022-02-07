using System;

// Solution:     AOToolsDelux
// Project:       AOToolsDelux
// File:             ISchemaDef.cs
// Created:      2021-07-11 (2:40 PM)


// defines the class for a collection of fields

namespace SharedCode.Fields.SchemaInfo.SchemaDefinitions
{

	// schema fields (not data fields)
	public abstract class ASchemaFields4<TE, TD>
		where TE : Enum  
		where TD : SchemaDictionaryBase<TE>, new()
	{
		public string SchemaName { get; protected set; }

		public TE[] FieldOrderDefault { get; set; }

		public TD Fields { get; protected set; }

		public ISchemaFieldDef<TE> this[TE key] => Fields[key];

		public SchemaFieldDef< TE,T> GetField<T>(TE key)
		{
			return (SchemaFieldDef< TE,T>) Fields[key];
		}

		public T GetValue<T>(TE key)
		{
			return ((SchemaFieldDef< TE, T>) Fields[key]).Value;
		}

		public void SetValue<T>(TE key, T value)
		{
			((SchemaFieldDef< TE,T>) Fields[key]).Value = value;
		}

		protected TE defineField<T>(TE key,
			string name,
			string desc,
			T val,
			SchemaFieldDisplayLevel dispLvl,
			string dispOrder,
			int dispWidth,
			FieldUnitType unittype = FieldUnitType.UT_UNDEFINED)
		{
			Fields.Add(key, new SchemaFieldDef<TE,T>(key, name, desc, val, dispLvl, dispOrder, dispWidth, unittype));

			return key;
		}

		protected abstract void defineFields();

	}

	/*

		public abstract class ASchemaFields<TE, TD>
		where TE : Enum  where TD : SchemaDictionaryBase<TE>, new()
		
	{
		public TE[] FieldOrderDefault { get; set; }
	
		public TD Fields { get; protected set; }
	
		public ISchemaFieldDef<TE> this[TE key] => Fields[key];
	}


	public abstract class ASchemaRootFields : ASchemaFields<SchemaRootKey, SchemaDictionaryBase<SchemaRootKey>>
	{
		protected SchemaRootKey defineField<TT>(SchemaRootKey key,
			string name,
			string desc,
			TT val,
			string dispOrder, 
			int dispWidth,
			FieldUnitType unittype = FieldUnitType.UT_UNDEFINED)
		{
			Fields.Add(key, 
				new RootFields<TT>(key, name, desc, val, dispOrder, dispWidth, unittype));

			return key;
		}
	}

	public abstract class ASchemaCellFields : ASchemaFields<SchemaCellKey, SchemaDictionaryBase<SchemaCellKey>>
	{
		protected SchemaCellKey defineField<TT>(
			SchemaCellKey key, 
			string name,
			string desc, 
			TT val,
			string dispOrder, 
			int dispWidth,
			FieldUnitType unittype = FieldUnitType.UT_UNDEFINED)
		{
			Fields.Add(key, 
				new CellFields<TT>(key, name, desc, val, dispOrder, dispWidth, unittype));

			return key;
		}
	}

	public abstract class ASchemaLockFields : ASchemaFields<SchemaLockKey, SchemaDictionaryBase<SchemaLockKey>>
	{
		protected SchemaLockKey defineField<TT>(
			SchemaLockKey key, 
			string name,
			string desc, 
			TT val,
			string dispOrder, 
			int dispWidth,
			FieldUnitType unittype = FieldUnitType.UT_UNDEFINED)
		{
			Fields.Add(key, 
				new LockFields<TT>(key, name, desc, val, dispOrder, dispWidth, unittype));

			return key;
		}
	}
	*/

}