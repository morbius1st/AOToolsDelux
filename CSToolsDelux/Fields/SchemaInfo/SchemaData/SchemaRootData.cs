#region using

using System;
using CSToolsDelux.Fields.SchemaInfo.SchemaData.SchemaDataDefinitions;
using SharedCode.Fields.SchemaInfo.SchemaSupport;
using SharedCode.Fields.SchemaInfo.SchemaFields.FieldsTemplates;
using SharedCode.Fields.SchemaInfo.SchemaFields;
using static SharedCode.Fields.SchemaInfo.SchemaSupport.SchemaRootKey;
#endregion

// username: jeffs
// created:  8/28/2021 10:10:07 PM

namespace CSToolsDelux.Fields.SchemaInfo.SchemaData
{
	public class SchemaRootData: 
		ISchemaData<SchemaRootKey, SchemaDataDictRoot, FieldsTempDictionary<SchemaRootKey>>
	{
	#region private fields

		private FieldsRoot fieldsRoot;

		private SchemaDataDictRoot data;

	#endregion

	#region ctor

		public SchemaRootData()
		{
			data = new SchemaDataDictRoot();
			fieldsRoot = new FieldsRoot();
		}

	#endregion

	#region public properties

		// public string DocumentName { get; set; }
		public string DsKey { get; set; }

		public override SchemaDataDictRoot Data {
			get => data;
			protected set {}
	}

		public FieldsRoot FieldsRoot => fieldsRoot;

		public override FieldsTempDictionary<SchemaRootKey> Fields => fieldsRoot.Fields;

	#endregion

	#region private properties

	#endregion

	#region public methods

		public override TD GetValue<TD>(SchemaRootKey key)
		{
			return ((RootData<TD>) data[key]).Value;
		}

		public override void SetValue<TD>(SchemaRootKey key, TD value)
		{
			((RootData<TD>) data[key]).Value = value;
		}

		public override void Add<TD>(SchemaRootKey key, TD value)
		{
			Data.Add(key,
				new RootData<TD>(value, fieldsRoot.GetField<TD>(key)));
		}

		public override void AddDefault<TD>(SchemaRootKey key)
		{
			FieldsTemp<SchemaRootKey, TD> f = fieldsRoot.GetField<TD>(key);

			Data.Add(key,
				new RootData<TD>(f.Value, f));
		}

		public void Configure(string name, string desc)
		{
			Add<string>(RK_SCHEMA_NAME, name);
			Add<string>(RK_DESCRIPTION, desc);
			AddDefault<string>(RK_VERSION);
			AddDefault<string>(RK_DEVELOPER);
			Add<string>(RK_CREATE_DATE, DateTime.UtcNow.ToString());
			Add<string>(RK_GUID, Guid.Empty.ToString());
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