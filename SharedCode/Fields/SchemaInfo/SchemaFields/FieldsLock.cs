#region + Using Directives

using System;
using System.Collections.Generic;
using SharedCode.Fields.SchemaInfo.SchemaSupport;
using SharedCode.Fields.SchemaInfo.SchemaFields.FieldsTemplates;
using UtilityLibrary;
using static SharedCode.Fields.SchemaInfo.SchemaSupport.SchemaLockKey;
using static SharedCode.Fields.SchemaInfo.SchemaSupport.SchemaFieldDisplayLevel;
#endregion

// user name: jeffs
// created:   7/3/2021 10:48:37 PM

namespace SharedCode.Fields.SchemaInfo.SchemaFields
{
	public class FieldsLock : AFieldsTemp<SchemaLockKey>
	{
		// public const string LF_SCHEMA_NAME = "FieldsLockSchema";
		public const string LF_SCHEMA_DESC = "Fields Lock DS";
		public const string LF_SCHEMA_VER = "0.1";

		public KeyValuePair<string, SchemaDataStorType> Type { get; } = SchemaConstants.SchemaTypeLock;

		public FieldsLock()
		{
			SchemaName = "Fields>FieldSchema>Lock";
			defineFields();
		}

		protected override void defineFields()
		{
			Fields = new FieldsTempDictionary<SchemaLockKey>();

			FieldOrderDefault = new SchemaLockKey[Enum.GetNames(typeof(SchemaLockKey)).Length];
			int idx = 0;

			FieldOrderDefault[idx++] =
				defineField<string>(LK_SCHEMA_NAME , "Name", "Name", SchemaName, DL_BASIC, "A1", 16 );

			FieldOrderDefault[idx++] =
				defineField<string>(LK_DESCRIPTION , "Description", "Description", LF_SCHEMA_DESC, DL_BASIC, "A2", 26 );

			FieldOrderDefault[idx++] =
				defineField<string>(LK_VERSION     , "Version", "Cells Version", LF_SCHEMA_VER, DL_MEDIUM, "A3", 10 );

			FieldOrderDefault[idx++] =
				defineField<string>(LK_CREATE_DATE , "CreationData", "Date and Time Created", DateTime.UtcNow.ToString(), DL_MEDIUM, "A4", 16);
			
			FieldOrderDefault[idx++] =
				defineField<string>(LK_USER_NAME   , "UserName", "Name of Lock Owner", CsUtilities.UserName, DL_ADVANCED, "A5", 16);
			
			FieldOrderDefault[idx++] =
				defineField<string>(LK_MACHINE_NAME, "MachineName", "Machine Lock Made", CsUtilities.MachineName, DL_ADVANCED, "A6", 16);
			
			FieldOrderDefault[idx++] =
				defineField<string>(LK_GUID        , "LockGuidString", "Lock Guid String", Guid.NewGuid().ToString(), DL_DEBUG, "A7", 32);

		}

	}
}