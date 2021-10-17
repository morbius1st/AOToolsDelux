#region using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using CSToolsDelux.Fields.SchemaInfo.SchemaData.SchemaDataDefinitions;
using CSToolsDelux.Fields.SchemaInfo.SchemaDefinitions;
using CSToolsDelux.Fields.SchemaInfo.SchemaFields;
using CSToolsDelux.Fields.Testing;
using UtilityLibrary;
using static CSToolsDelux.Fields.SchemaInfo.SchemaDefinitions.SchemaLockKey;

#endregion

// username: jeffs
// created:  8/28/2021 10:10:07 PM

namespace CSToolsDelux.Fields.SchemaInfo.SchemaData
{
	// public abstract class SchemaData<TE, TD, TF> : 
	// 	ISchemaData<TE, TD, TF>
	// 	where TE : Enum
	// 	where TD : SchemaDataDictionaryBase<TE>
	// 	where TF : SchemaDictionaryBase<TE>
	// {
	// 	public void Add<TX>(TE key, TX value) { }
	// 	public void AddDefault<TX>(TE key) { }
	// 	public TD Data { get; }
	// 	public TF Fields { get; }
	// 	public TX GetValue<TX>(TE key)
	// 	{
	// 		return default;
	// 	}
	//
	// 	public void SetValue<TX>(TE key, TX value) { }
	// }


	public class SchemaLockData: 
		ISchemaData<SchemaLockKey, SchemaDataDictLock, SchemaDictionaryLock>
	{
	#region private fields

		private SchemaDataDictLock data;
		private SchemaLockFields appFields;

	#endregion

	#region ctor

		public SchemaLockData()
		{
			data = new SchemaDataDictLock();
			appFields = new SchemaLockFields();

			Configure();
		}

	#endregion

	#region public properties

		public string DocumentName { get; set; }
		public string DocKey { get; set; }

		public override SchemaDataDictLock Data 
		{
			get => data;
			protected set { }
		}

		public SchemaLockFields AppFields => appFields;

		public override SchemaDictionaryLock Fields => (SchemaDictionaryLock) appFields.Fields;

	#endregion

	#region private properties

	#endregion

	#region public methods

		public override TD GetValue<TD>(SchemaLockKey key)
		{
			return ((SchemaLockDataField<TD>) data[key]).Value;
		}

		public override void SetValue<TD>(SchemaLockKey key, TD value)
		{
			((SchemaLockDataField<TD>) data[key]).Value = value;
		}

		public override void Add<TD>(SchemaLockKey key, TD value)
		{
			Data.Add(key,
				new SchemaLockDataField<TD>(value, appFields.GetField<TD>(key)));
		}

		public override void AddDefault<TD>(SchemaLockKey key)
		{
			SchemaFieldDef<TD, SchemaLockKey> f = appFields.GetField<TD>(key);

			Data.Add(key,
				new SchemaLockDataField<TD>(f.Value, f));
		}

		private void Configure()
		{
			AddDefault<string>(LK_SCHEMA_NAME);
			AddDefault<string>(LK_DESCRIPTION);
			AddDefault<string>(LK_VERSION);
			AddDefault<string>(LK_USER_NAME);
			AddDefault<string>(LK_MACHINE_NAME);
			AddDefault<string>(LK_GUID);

			Add<string>(LK_CREATE_DATE, DateTime.UtcNow.ToString());

		}

	#endregion

	#region private methods

	#endregion

	#region event consuming

	#endregion

	#region event publishing

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is SchemaAppData";
		}

	#endregion
	}
}