// Solution:     SharedCode
// Project:     SharedCode
// File:             SchemaDictionaryBase.cs
// Created:      2021-07-03 (11:28 PM)

using System;
using System.Collections.Generic;

		
namespace SharedCode.Fields.SchemaInfo.SchemaFields.FieldsTemplates
{
	public class FieldsTempDictionary<TE> : Dictionary<TE, AFieldsMembers<TE>>  where TE : Enum
	{
		public TC Clone<TC>(TC original) where TC : FieldsTempDictionary<TE>, new()
		{
			TC copy = new TC();

			foreach (KeyValuePair<TE, AFieldsMembers<TE>> kvp in original)
			{
				copy.Add(kvp.Key, (AFieldsMembers<TE>) kvp.Value.Clone());
			}

			return copy;
		}
	}
}