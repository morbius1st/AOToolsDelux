#region using

using System;
using CSToolsDelux.Fields.SchemaInfo.SchemaData.SchemaDataDefinitions;
using SharedCode.Fields.SchemaInfo.SchemaSupport;
using SharedCode.Fields.SchemaInfo.SchemaFields.FieldsTemplates;
using SharedCode.Fields.SchemaInfo.SchemaFields;
using static SharedCode.Fields.SchemaInfo.SchemaSupport.SchemaLockKey;
#endregion

// username: jeffs
// created:  8/28/2021 10:10:07 PM

namespace CSToolsDelux.Fields.SchemaInfo.SchemaData
{
	public class SchemaLockData: 
		ISchemaData<SchemaLockKey, SchemaDataDictLock, FieldsTempDictionary<SchemaLockKey>>
	{
	#region private fields

		private FieldsLock fieldsLock;

		private SchemaDataDictLock data;

	#endregion

	#region ctor

		public SchemaLockData()
		{
			data = new SchemaDataDictLock();
			fieldsLock = new FieldsLock();

			Configure();
		}

	#endregion

	#region public properties

		public string DocumentName { get; set; }
		public string DsKey { get; set; }

		public override SchemaDataDictLock Data 
		{
			get => data;
			protected set { }
		}

		// public SchemaLockFields LockFields => lockFields;

		public override FieldsTempDictionary<SchemaLockKey> Fields => fieldsLock.Fields;

	#endregion

	#region private properties

	#endregion

	#region public methods

		public override TD GetValue<TD>(SchemaLockKey key)
		{
			return ((LockData<TD>) data[key]).Value;
		}

		public override void SetValue<TD>(SchemaLockKey key, TD value)
		{
			((LockData<TD>) data[key]).Value = value;
		}

		public override void Add<TD>(SchemaLockKey key, TD value)
		{
			Data.Add(key,
				new LockData<TD>(value, fieldsLock.GetField<TD>(key)));
		}

		public override void AddDefault<TD>(SchemaLockKey key)
		{
			FieldsTemp< SchemaLockKey,TD> f = fieldsLock.GetField<TD>(key);

			Data.Add(key,
				new LockData<TD>(f.Value, f));
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