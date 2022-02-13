// Solution:     SharedCode
// Project:     SharedCode
// File:             SchemaRootData2.cs
// Created:      2021-10-19 (6:13 AM)

using System;
using System.Collections.Generic;
using SharedCode.Fields.SchemaInfo.SchemaData.DataTemplate;
using SharedCode.Fields.SchemaInfo.SchemaSupport;
using SharedCode.Fields.SchemaInfo.SchemaData.DataTemplates;
using SharedCode.Fields.SchemaInfo.SchemaFields.FieldsTemplates;

namespace SharedCode.Fields.SchemaInfo.SchemaData
{
	public class DataRoot : ADataTempBase<SchemaRootKey>
	{
		public override KeyValuePair<SchemaDataStorType, string> DataStorType { get; } = SchemaConstants.SchemaTypeRoot;

		public DataRoot(
			AFieldsTemp<SchemaRootKey> fields, int idxCount = 1) : base(fields, idxCount) { }

		
		public override string SchemaName => ((DataMembers<SchemaRootKey, string>)ListOfDataDictionaries[0][SchemaRootKey.RK_SCHEMA_NAME]).Value;
		public override string SchemaDesc => ((DataMembers<SchemaRootKey, string>)ListOfDataDictionaries[0][SchemaRootKey.RK_DESCRIPTION]).Value;
		public override string SchemaVersion => ((DataMembers<SchemaRootKey, string>)ListOfDataDictionaries[0][SchemaRootKey.RK_VERSION]).Value;
		public override string SchemaCreateDate => ((DataMembers<SchemaRootKey, string>)ListOfDataDictionaries[0][SchemaRootKey.RK_CREATE_DATE]).Value;

		public override void Configure(string name)
		{
			Add(SchemaRootKey.RK_SCHEMA_NAME, name);
			AddDefault<string>(SchemaRootKey.RK_DESCRIPTION);
			AddDefault<string>(SchemaRootKey.RK_VERSION);
			AddDefault<string>(SchemaRootKey.RK_DEVELOPER);
			Add(SchemaRootKey.RK_CREATE_DATE, DateTime.UtcNow.ToString());
			Add(SchemaRootKey.RK_GUID, Guid.Empty.ToString());
		}

	}
}