using System;

// Solution:     AOToolsDelux
// Project:       AOToolsDelux
// File:             ISchemaDef.cs
// Created:      2021-07-11 (2:40 PM)


namespace CSToolsDelux.Fields.SchemaInfo.SchemaDefinitions
{

	public abstract class ASchemaFields<TE, TD>
		where TE : Enum  where TD : SchemaDictionaryBase<TE>, new()
		
	{
		public TE[] KeyOrder { get; set; }

		public TD Fields { get; protected set; }

		public ISchemaFieldDef<TE> this[TE key] => Fields[key];

	}

	public abstract class ASchemaFields2<TE, TD>
		where TE : Enum  
		where TD : SchemaDictionaryBase<TE>, new()
	{
		public string SchemaName { get; protected set; }

		public TE[] KeyOrder { get; set; }

		public TD Fields { get; protected set; }

		public ISchemaFieldDef<TE> this[TE key] => Fields[key];

		public SchemaFieldDef<T, TE> GetField<T>(TE key)
		{
			return (SchemaFieldDef<T, TE>) Fields[key];
		}

		public T GetValue<T>(TE key)
		{
			return ((SchemaFieldDef<T, TE>) Fields[key]).Value;
		}

		public void SetValue<T>(TE key, T value)
		{
			((SchemaFieldDef<T, TE>) Fields[key]).Value = value;
		}

		protected TE defineField<T>(TE key, string name, string desc, T val,
			FieldUnitType unittype = FieldUnitType.UT_UNDEFINED)
		{
			Fields.Add(key, new SchemaFieldDef<T,TE>(key, name, desc, val, unittype));

			return key;
		}

		protected abstract void defineFields();

	}







	public abstract class ASchemaRootFields : ASchemaFields<SchemaRootKey, SchemaDictionaryBase<SchemaRootKey>>
	{
		protected SchemaRootKey defineField<TT>(SchemaRootKey key, string name,
			string desc, TT val,
			FieldUnitType unittype = FieldUnitType.UT_UNDEFINED)
		{
			Fields.Add(key, 
				new RootFields<TT>(key, name, desc, val, unittype));

			return key;
		}
	}

	public abstract class ASchemaCellFields : ASchemaFields<SchemaCellKey, SchemaDictionaryBase<SchemaCellKey>>
	{
		protected SchemaCellKey defineField<TT>(SchemaCellKey key, string name,
			string desc, TT val,
			FieldUnitType unittype = FieldUnitType.UT_UNDEFINED)
		{
			Fields.Add(key, 
				new CellFields<TT>(key, name, desc, val, unittype));

			return key;
		}
	}

	public abstract class ASchemaLockFields : ASchemaFields<SchemaLockKey, SchemaDictionaryBase<SchemaLockKey>>
	{
		protected SchemaLockKey defineField<TT>(SchemaLockKey key, string name,
			string desc, TT val,
			FieldUnitType unittype = FieldUnitType.UT_UNDEFINED)
		{
			Fields.Add(key, 
				new LockFields<TT>(key, name, desc, val, unittype));

			return key;
		}
	}




}