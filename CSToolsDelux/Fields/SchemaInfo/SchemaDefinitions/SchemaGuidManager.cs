#region using directives

using System;

#endregion


// projname: $projectname$
// itemname: SchemaGuidManager
// username: jeffs
// created:  7/4/2021 7:38:10 AM

namespace CSToolsDelux.Fields.SchemaInfo.SchemaDefinitions
{
	public class SchemaGuidManager
	{
	#region private fields

		private const string ROOT_GUID = "B1788BC0-381E-4F4F-BE0B-91A92B96FFFF";

		private string appGuidStr;

	#endregion

	#region ctor

		public SchemaGuidManager()
		{
			// set to an "empty" guid string
			appGuidStr = Guid.Empty.ToString();
		}

	#endregion

	#region public properties

		public static string RootGuidString => ROOT_GUID;
		public static Guid RootGuid => new Guid(RootGuidString);

		public string AppGuidString {
			get => appGuidStr;
			set
			{
				GotAppGuid = true;
				appGuidStr = value;
			}
	}
		public Guid AppGuid => new Guid(AppGuidString);

		public bool GotAppGuid { get; private set; }

	#endregion

	#region private properties

	#endregion

	#region public methods

		public static string GetNewCellGuidString (int id)
		{
			return Guid.NewGuid().ToString();
		}

		public static string GetNewAppGuidString()
		{
			return Guid.NewGuid().ToString();
		}

	#endregion

	#region private methods

	#endregion

	#region system overrides

		public override string ToString()
		{
			return $"root GUID| {RootGuidString}";
		}

	#endregion

	}
}