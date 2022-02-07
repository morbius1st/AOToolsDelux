// Solution:     SharedCode
// Project:     SharedCode
// File:             SchemaRootData2.cs
// Created:      2021-10-19 (6:13 AM)

using System;
using SharedCode.Fields.SchemaInfo.SchemaData.DataTemplates;
using SharedCode.Fields.SchemaInfo.SchemaFields.FieldsTemplates;
using SharedCode.Fields.SchemaInfo.SchemaSupport;

namespace SharedCode.Fields.SchemaInfo.SchemaData
{
	public class DataCell : ADataTempBase<SchemaCellKey>
	{
		public DataCell(
			AFieldsTemp<SchemaCellKey> fields, int idxCount = 1) : base(fields, idxCount) { }

		public override string SchemaName => ((DataMembers<SchemaCellKey, string>)ListOfDataDictionaries[0][SchemaCellKey.CK_SCHEMA_NAME]).Value;
		public override string SchemaDesc => ((DataMembers<SchemaCellKey, string>)ListOfDataDictionaries[0][SchemaCellKey.CK_DESCRIPTION]).Value;
		public override string SchemaVersion => ((DataMembers<SchemaCellKey, string>)ListOfDataDictionaries[0][SchemaCellKey.CK_VERSION]).Value;
		public override string SchemaCreateDate => ((DataMembers<SchemaCellKey, string>)ListOfDataDictionaries[0][SchemaCellKey.CK_CREATE_DATE]).Value;

		public override void Configure(string name = null)
		{
			if (name == null)
			{
				AddDefault<string>(SchemaCellKey.CK_SCHEMA_NAME);
			}
			else
			{
				Add(SchemaCellKey.CK_SCHEMA_NAME, name);
			}

			AddDefault<string>(SchemaCellKey.CK_DESCRIPTION);
			AddDefault<string>(SchemaCellKey.CK_VERSION);
			Add(SchemaCellKey.CK_CREATE_DATE, DateTime.UtcNow.ToString());

			AddDefault<string>(SchemaCellKey.CK_SEQUENCE);
			AddDefault<int>(SchemaCellKey.CK_UPDATE_RULE);
			AddDefault<string>(SchemaCellKey.CK_CELL_FAMILY_NAME);
			AddDefault<bool>(SchemaCellKey.CK_SKIP);
			AddDefault<string>(SchemaCellKey.CK_XL_FILE_PATH);
			AddDefault<string>(SchemaCellKey.CK_XL_WORKSHEET_NAME);


		}



	}
}