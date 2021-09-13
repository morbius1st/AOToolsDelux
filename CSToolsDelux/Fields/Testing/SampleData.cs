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


#endregion

// user name: jeffs
// created:   8/29/2021 7:02:18 AM

namespace CSToolsDelux.Fields.Testing
{
	public static class SampleData
	{
		static SampleData() { }

		// public static void SampleAppData(SchemaAppData aData) 
		// {
		// 	aData.Add(SchemaAppKey.AK_NAME, "App Data Name");
		// 	aData.Add(SchemaAppKey.AK_DESCRIPTION, "App Data Description");
		// 	aData.AddDefault<string>(SchemaAppKey.AK_VERSION);
		// }
		//
		// public static void SampleFieldData01(SchemaRootFields rootFields, string appGuidStr)
		// {
		// 	rootFields.AppGuidField = appGuidStr;
		// 	rootFields.CreationField = DateTime.UtcNow.ToString();
		// }

		// public static void SampleData01(SchemaRootData rootData, string appGuidStr)
		// {
		// 	rootData.AddDefault<string>(RK_NAME);
		// 	rootData.AddDefault<string>(RK_DESCRIPTION);
		// 	rootData.AddDefault<string>(RK_DEVELOPER);
		// 	rootData.AddDefault<string>(RK_VERSION);
		// 	rootData.Add(RK_APP_GUID, appGuidStr);
		// 	rootData.Add(RK_CREATION, DateTime.UtcNow.ToString());
		// }
	}
}