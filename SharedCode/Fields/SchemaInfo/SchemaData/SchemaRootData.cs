#region using

using static SharedCode.Fields.SchemaInfo.SchemaDefinitions.SchemaRootKey;

#endregion

// username: jeffs
// created:  8/28/2021 10:10:07 PM

namespace CSToolsDelux.Fields.SchemaInfo.SchemaData
{
	public class SchemaRootData: 
		ISchemaData<SchemaRootKey, SchemaDataDictRoot, SchemaDictionaryRoot>
	{
	#region private fields

		private SchemaRootFields rootFields;

		private SchemaDataDictRoot data;

	#endregion

	#region ctor

		public SchemaRootData()
		{
			data = new SchemaDataDictRoot();
			rootFields = new SchemaRootFields();
		}

	#endregion

	#region public properties

		// public string DocumentName { get; set; }
		public string DsKey { get; set; }

		public override SchemaDataDictRoot Data {
			get => data;
			protected set {}
	}

		public SchemaRootFields RootFields => rootFields;

		public override SchemaDictionaryRoot Fields => (SchemaDictionaryRoot) rootFields.Fields;

	#endregion

	#region private properties

	#endregion

	#region public methods

		public override TD GetValue<TD>(SchemaRootKey key)
		{
			return ((SchemaRootDataField<TD>) data[key]).Value;
		}

		public override void SetValue<TD>(SchemaRootKey key, TD value)
		{
			((SchemaRootDataField<TD>) data[key]).Value = value;
		}

		public override void Add<TD>(SchemaRootKey key, TD value)
		{
			Data.Add(key,
				new SchemaRootDataField<TD>(value, rootFields.GetField<TD>(key)));
		}

		public override void AddDefault<TD>(SchemaRootKey key)
		{
			SchemaFieldDef<SchemaRootKey, TD> f = rootFields.GetField<TD>(key);

			Data.Add(key,
				new SchemaRootDataField<TD>(f.Value, f));
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