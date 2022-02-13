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
using UtilityLibrary;

namespace SharedCode.Fields.SchemaInfo.SchemaData
{
	public class DataLock : ADataTempBase<SchemaLockKey>
	{
		public override KeyValuePair<SchemaDataStorType, string> DataStorType { get; } = SchemaConstants.SchemaTypeLock;

		public DataLock(
			AFieldsTemp<SchemaLockKey> fields, int idxCount = 1) : base(fields, idxCount) { }

		public override string SchemaName => ((DataMembers<SchemaLockKey, string>)ListOfDataDictionaries[0][SchemaLockKey.LK_SCHEMA_NAME]).Value;
		public override string SchemaDesc => ((DataMembers<SchemaLockKey, string>)ListOfDataDictionaries[0][SchemaLockKey.LK_DESCRIPTION]).Value;
		public override string SchemaVersion => ((DataMembers<SchemaLockKey, string>)ListOfDataDictionaries[0][SchemaLockKey.LK_VERSION]).Value;
		public override string SchemaCreateDate => ((DataMembers<SchemaLockKey, string>)ListOfDataDictionaries[0][SchemaLockKey.LK_CREATE_DATE]).Value;

		public override void Configure(string name)
		{
			Add(SchemaLockKey.LK_SCHEMA_NAME, name);
			AddDefault<string>(SchemaLockKey.LK_DESCRIPTION);
			AddDefault<string>(SchemaLockKey.LK_VERSION);
			Add(SchemaLockKey.LK_CREATE_DATE, DateTime.UtcNow.ToString());
			Add(SchemaLockKey.LK_USER_NAME, CsUtilities.UserName);
			Add(SchemaLockKey.LK_MACHINE_NAME, CsUtilities.MachineName);
			Add(SchemaLockKey.LK_GUID, Guid.Empty.ToString());
		}

	}
}