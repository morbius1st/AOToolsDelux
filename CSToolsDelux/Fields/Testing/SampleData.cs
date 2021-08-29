#region + Using Directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSToolsDelux.Fields.SchemaInfo.SchemaData;
using CSToolsDelux.Fields.SchemaInfo.SchemaData.SchemaDataDefinitions;
using CSToolsDelux.Fields.SchemaInfo.SchemaDefinitions;
using CSToolsDelux.Fields.SchemaInfo.SchemaFields;
using static CSToolsDelux.Fields.SchemaInfo.SchemaDefinitions.SchemaRootKey;

#endregion

// user name: jeffs
// created:   8/29/2021 7:02:18 AM

namespace CSToolsDelux.Fields.Testing
{
	public static class SampleData
	{
		static SampleData() { }

		public static void SampleData01(SchemaRootData rootData, string appGuidStr)
		{
			rootData.AddDefault<string>(RK_NAME);
			rootData.AddDefault<string>(RK_DESCRIPTION);
			rootData.AddDefault<string>(RK_DEVELOPER);
			rootData.AddDefault<string>(RK_VERSION);
			rootData.Add(RK_APP_GUID, appGuidStr);
			rootData.Add(RK_CREATION, DateTime.UtcNow.ToString());
		}
	}
}