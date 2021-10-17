#region + Using Directives

using System;
using CSToolsDelux.Fields.SchemaInfo.SchemaDefinitions;
using UtilityLibrary;
using static CSToolsDelux.Fields.SchemaInfo.SchemaDefinitions.SchemaLockKey;
#endregion

// user name: jeffs
// created:   7/3/2021 10:48:37 PM

namespace CSToolsDelux.Fields.SchemaInfo.SchemaFields
{
	public class SchemaLockFields : ASchemaFields2<SchemaLockKey, SchemaDictionaryBase<SchemaLockKey>>
	{
		// public const string LF_SCHEMA_NAME = "FieldsLockSchema";
		public const string LF_SCHEMA_DESC = "Fields Lock DS";
		public const string LF_SCHEMA_VER = "0.1";

		public SchemaLockFields()
		{
			SchemaName = "Fields>FieldSchema>Lock";
			defineFields();
		}

		// public LockFields<TD> GetField<TD>(SchemaLockKey key)
		// {
		// 	return (LockFields<TD>) Fields[key];
		// }
		//
		// public TD GetValue<TD>(SchemaLockKey key)
		// {
		// 	return ((LockFields<TD>) Fields[key]).Value;
		// }
		//
		// public void SetValue<TD>(SchemaLockKey key, TD value)
		// {
		// 	((LockFields<TD>) Fields[key]).Value = value;
		// }

		protected override void defineFields()
		{
			Fields = new SchemaDictionaryLock();

			KeyOrder = new SchemaLockKey[Enum.GetNames(typeof(SchemaLockKey)).Length];
			int idx = 0;

			KeyOrder[idx++] =
				defineField<string>(LK_SCHEMA_NAME, "Name", "Name", SchemaName );

			KeyOrder[idx++] =
				defineField<string>(LK_DESCRIPTION, "Description", "Description", LF_SCHEMA_DESC );

			KeyOrder[idx++] =
				defineField<string>(LK_VERSION, "Version", "Cells Version", LF_SCHEMA_VER );

			KeyOrder[idx++] =
				defineField<string>(LK_CREATE_DATE, "CreationData", "Date and Time Created", DateTime.UtcNow.ToString());
			
			KeyOrder[idx++] =
				defineField<string>(LK_USER_NAME, "UserName", "Name of Lock Owner", CsUtilities.UserName);
			
			KeyOrder[idx++] =
				defineField<string>(LK_MACHINE_NAME, "MachineName", "Machine Lock Made", CsUtilities.MachineName);
			
			KeyOrder[idx++] =
				defineField<string>(LK_GUID, "AppGuidString", "App Guid String", Guid.NewGuid().ToString());

		}

	}
}