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
using static CSToolsDelux.Fields.SchemaInfo.SchemaDefinitions.SchemaAppKey;

#endregion

// username: jeffs
// created:  8/28/2021 10:10:07 PM

namespace CSToolsDelux.Fields.SchemaInfo.SchemaData
{
	public class SchemaAppData
	{
	#region private fields

		private SchemaDataDictApp appDict;
		private SchemaAppFields appFields;

	#endregion

	#region ctor

		public SchemaAppData()
		{
			appDict = new SchemaDataDictApp();
			appFields = new SchemaAppFields();
		}

	#endregion

	#region public properties

		public SchemaDataDictApp AppDict => appDict;

		public ASchemaDataFieldDef<SchemaAppKey> this[SchemaAppKey key] => appDict[key];

		public TD GetValue<TD>(SchemaAppKey key)
		{
			return ((SchemaAppDataField<TD>) appDict[key]).Value;
		}

		public void SetValue<TD>(SchemaAppKey key, TD value)
		{
			((SchemaAppDataField<TD>) appDict[key]).Value = value;
		}

		public void Add<TD>(SchemaAppKey key, TD value)
		{
			AppDict.Add(key, 
				new SchemaAppDataField<TD>(value, appFields.GetField<TD>(key)));
		}

		public void AddDefault<TD>(SchemaAppKey key)
		{
			SchemaFieldDef<TD, SchemaAppKey> f = appFields.GetField<TD>(key);

			AppDict.Add(key, 
				new SchemaAppDataField<TD>(f.Value, f));
		}

	#endregion

	#region private properties

	#endregion

	#region public methods

		public void Configure(string name, string desc)
		{
			Add<string>(AK_NAME, name);
			Add<string>(AK_DESCRIPTION, desc);
			AddDefault<string>(AK_VERSION);
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