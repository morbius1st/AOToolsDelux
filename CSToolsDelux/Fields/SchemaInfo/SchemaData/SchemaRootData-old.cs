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
using static CSToolsDelux.Fields.SchemaInfo.SchemaDefinitions.SchemaRootKey;

#endregion

// username: jeffs
// created:  8/28/2021 10:10:07 PM

namespace CSToolsDelux.Fields.SchemaInfo.SchemaData
{
	public class SchemaRootData
	{
	#region private fields

		private SchemaDataDictRoot rootDict;
		private SchemaRootFields rootFields;

	#endregion

	#region ctor

		public SchemaRootData()
		{
			rootDict = new SchemaDataDictRoot();
			rootFields = new SchemaRootFields();
		}

	#endregion

	#region public properties

		public SchemaDataDictRoot RootDict => rootDict;

		public ASchemaDataFieldDef<SchemaRootKey> this[SchemaRootKey key] => rootDict[key];

		public TD GetValue<TD>(SchemaRootKey key)
		{
			return ((SchemaRootDataField<TD>) rootDict[key]).Value;
		}

		public void SetValue<TD>(SchemaRootKey key, TD value)
		{
			((SchemaRootDataField<TD>) rootDict[key]).Value = value;
		}

		public void Add<TD>(SchemaRootKey key, TD value)
		{
			rootDict.Add(key, 
				new SchemaRootDataField<TD>(value, rootFields.GetField<TD>(key)));
		}

		public void AddDefault<TD>(SchemaRootKey key)
		{
			SchemaFieldDef<TD, SchemaRootKey> f = rootFields.GetField<TD>(key);

			rootDict.Add(key, 
				new SchemaRootDataField<TD>(f.Value, f));
		}

	#endregion

	#region private properties

	#endregion

	#region public methods

		public void Configure(string appGuidStr)
		{
			AddDefault<string>(RK_NAME);
			AddDefault<string>(RK_DESCRIPTION);
			AddDefault<string>(RK_DEVELOPER);
			AddDefault<string>(RK_VERSION);
			Add(RK_APP_GUID, appGuidStr);
			Add(RK_CREATION, DateTime.UtcNow.ToString());
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
			return "this is SchemaRootData";
		}

	#endregion
	}
}