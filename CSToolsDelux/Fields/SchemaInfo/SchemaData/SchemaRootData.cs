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
		private SchemaFieldsRoot fields;

	#endregion

	#region ctor

		public SchemaRootData()
		{
			rootDict = new SchemaDataDictRoot();
			fields = new SchemaFieldsRoot();
		}

	#endregion

	#region public properties

		public SchemaDataDictRoot RootDict => rootDict;

		public void Add<TD>(SchemaRootKey key, TD value)
		{
			rootDict.Add(key, 
				new SchemaRootField<TD>(value, fields.GetField<TD>(key)));
		}

		public void AddDefault<TD>(SchemaRootKey key)
		{
			SchemaFieldDef<TD, SchemaRootKey> f = fields.GetField<TD>(key);

			rootDict.Add(key, 
				new SchemaRootField<TD>(f.Value, f));
		}

	#endregion

	#region private properties

	#endregion

	#region public methods

		public void Configure()
		{
			SampleData.SampleData01(this, SchemaGuidManager.RootGuidString);
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