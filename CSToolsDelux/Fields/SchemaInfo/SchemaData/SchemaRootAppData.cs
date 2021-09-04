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
using static CSToolsDelux.Fields.SchemaInfo.SchemaDefinitions.SchemaRootAppKey;

#endregion

// username: jeffs
// created:  8/28/2021 10:10:07 PM

namespace CSToolsDelux.Fields.SchemaInfo.SchemaData
{
	public class SchemaRootAppData
	{
	#region private fields

		private SchemaDataDictRootApp appDict;
		private SchemaRootAppFields appFields;

	#endregion

	#region ctor

		public SchemaRootAppData()
		{
			appDict = new SchemaDataDictRootApp();
			appFields = new SchemaRootAppFields();
		}

	#endregion

	#region public properties

		public SchemaDataDictRootApp AppDict => appDict;

		public ASchemaDataFieldDef<SchemaRootAppKey> this[SchemaRootAppKey key] => appDict[key];

		public TD GetValue<TD>(SchemaRootAppKey key)
		{
			return ((SchemaRootAppDataField<TD>) appDict[key]).Value;
		}

		public void SetValue<TD>(SchemaRootAppKey key, TD value)
		{
			((SchemaRootAppDataField<TD>) appDict[key]).Value = value;
		}

		public void Add<TD>(SchemaRootAppKey key, TD value)
		{
			AppDict.Add(key, 
				new SchemaRootAppDataField<TD>(value, appFields.GetField<TD>(key)));
		}

		public void AddDefault<TD>(SchemaRootAppKey key)
		{
			SchemaFieldDef<TD, SchemaRootAppKey> f = appFields.GetField<TD>(key);

			AppDict.Add(key, 
				new SchemaRootAppDataField<TD>(f.Value, f));
		}

	#endregion

	#region private properties

	#endregion

	#region public methods

		public void Configure(string name, string desc)
		{
			Add<string>(RAK_NAME, name);
			Add<string>(RAK_DESCRIPTION, desc);
			AddDefault<string>(RAK_VERSION);
			AddDefault<string>(RAK_DEVELOPER);
			Add<string>(RAK_CREATE_DATE, DateTime.UtcNow.ToString());
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