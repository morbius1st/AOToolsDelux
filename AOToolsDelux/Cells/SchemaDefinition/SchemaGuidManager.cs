#region using directives

using System;

#endregion


// projname: $projectname$
// itemname: SchemaGuidManager
// username: jeffs
// created:  7/4/2021 7:38:10 AM

namespace AOToolsDelux.Cells.SchemaDefinition
{

/*
	// this concept was revised to use a pre-defined GUID for
	// root and a unique GUID for app and for cells
	// this will prevent duplicate app or cell GUID's

		// concepts here:
		*//*
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
		   *
*/

	public class SchemaGuidManager
	{
	#region private fields

		private static readonly Lazy<SchemaGuidManager> instance =
			new Lazy<SchemaGuidManager>(() => new SchemaGuidManager());

		private const string ROOT_GUID = "B1788BC0-381E-4F4F-BE0B-93A93B94FFFF";

		private static string appGuidUniqueStr /* = "93A93B947"*/;

	#endregion

	#region ctor

		static SchemaGuidManager()
		{
			appGuidUniqueStr = GetAppGuidString();
		}

	#endregion

	#region public properties

		public static SchemaGuidManager Instance => instance.Value;

		public static string RootGuidString => ROOT_GUID;
		public static Guid RootGuid => new Guid(RootGuidString);

		public static string AppGuidString {
			get => appGuidUniqueStr;
			set
			{
				GotAppGuid = true;
				appGuidUniqueStr = value;
			}
	}
		public static Guid AppGuid => new Guid(AppGuidString);

		public static bool GotAppGuid { get; private set; }

	#endregion

	#region private properties

	#endregion

	#region public methods

		public static string GetCellGuidString (int id)
		{
			return Guid.NewGuid().ToString();
		}

		private static string GetAppGuidString()
		{
			return Guid.NewGuid().ToString();
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
			return $"root GUID| {RootGuidString}" ;
		}

	#endregion

	}
}