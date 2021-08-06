#region using directives

using System;

#endregion


// projname: $projectname$
// itemname: SchemaGuidManager
// username: jeffs
// created:  7/4/2021 7:38:10 AM

namespace AOTools.Cells.SchemaDefinition
{
	// concepts here:
	/*
	 * the root guid is the same for every revit model
	 * and every revit model will have a root ex store schema
	 * that will provide the unique guid for the model
	 *
	 * the root guid 
	 * ROOT_GUID + uniqueBaseGuid + baseSuffixGuid
	 *  +-> provides the app uniqueAppGuid
	 *
	 * the app guid
	 * ROOT_GUID + uniqueAppGuid + baseSuffixGuid
	 *
	 * the cell guid
	 * ROOT_GUID + uniqueAppGuid + cellSuffix
	 *
	 * |<- always the same for apps  ->|
	 * B1788BC0-381E-4F4F-BE0B-    93A93            
	 * xxxxxxxx-xxxx-xxxx-xxxx- +  bbbbb          + aaaa           + sss
	 * ROOT_GUID                + uniqueBaseGuid  + uniqueAppGuid  + appSuffix
	 *
	 * 0         1         2         3   
	 * 012345678901234567890123456789012345
	 * 00000000-0000-0000-0000-000000000000
	 * xxxxxxxx-xxxx-xxxx-xxxx-bbbbbaaaasss
	 */


	public class SchemaGuidManager
	{
	#region private fields

		private static readonly Lazy<SchemaGuidManager> instance =
			new Lazy<SchemaGuidManager>(() => new SchemaGuidManager());

		private const string ROOT_GUID = "B1788BC0-381E-4F4F-BE0B-";
		private static string baseGuidUniqueStr = "93A93B94F";

		private static string baseSuffixGuid = "FFF";

		private static string appGuidUniqueStr = "93A93B947";

		private static string cellSuffixGuid = "{0:x3}";

	#endregion

	#region ctor

		static SchemaGuidManager()
		{
			RootGuidString = ROOT_GUID + baseGuidUniqueStr + baseSuffixGuid;
		}

	#endregion

	#region public properties

		public static SchemaGuidManager Instance => instance.Value;

		public static string RootGuidString { get; private set; }
		public static Guid RootGuid => new Guid(RootGuidString);

		public static string AppGuidWholeString => GetAppGuidString();
		public static Guid AppGuid => new Guid(AppGuidWholeString);

		public static string AppGuidUniqueString
		{
			get => appGuidUniqueStr;
			set
			{
				GotAppGuid = true;
				appGuidUniqueStr = value;
			}
		}

		public static bool GotAppGuid { get; private set; }

	#endregion

	#region private properties

	#endregion

	#region public methods

		public static string GetCellGuidString (int id)
		{
			return Guid.NewGuid().ToString();
			// return ROOT_GUID + appGuidUniqueStr + string.Format(cellSuffixGuid, id);
		}

		public static string GetAppGuidString()
		{
			return ROOT_GUID + appGuidUniqueStr  + baseSuffixGuid;
		}

		public static void SetUniqueAppGuidSubStr()
		{
			// 0         1         2         3   
			// 0123456789012345678901234567890123456789
			// 00000000-0000-0000-0000-000000000000
			string guid = Guid.NewGuid().ToString();
			appGuidUniqueStr = guid.Substring(24, appGuidUniqueStr.Length);
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
			return "this is SchemaGuidManager";
		}

	#endregion

	}
}