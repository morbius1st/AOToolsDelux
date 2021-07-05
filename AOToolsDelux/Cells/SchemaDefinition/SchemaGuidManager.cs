#region using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

#endregion


// projname: $projectname$
// itemname: SchemaGuidManager
// username: jeffs
// created:  7/4/2021 7:38:10 AM

namespace AOTools.Cells.SchemaDefinition
{
	public class SchemaGuidManager
	{
	#region private fields

		private static readonly Lazy<SchemaGuidManager> instance =
			new Lazy<SchemaGuidManager>(() => new SchemaGuidManager());

		private const string ROOT_GUID = "B1788BC0-381E-4F4F-BE0B-";
		private static string uniqueRootGuid = "93A93B947"; // + "FFF"
		private static string uniqueAppGuid = "FFF";
		private static string uniqueCellGuid = "0:x3";

	#endregion

	#region ctor

		private SchemaGuidManager()
		{
			uniqueAppGuid = uniqueRootGuid + "FFF";

			AppGuidString = ROOT_GUID + uniqueRootGuid + uniqueAppGuid;
		}

	#endregion

	#region public properties

		public static SchemaGuidManager Instance => instance.Value;

		public static string AppGuidString { get; private set; }

		public static Guid AppGuid => new Guid(AppGuidString);

		public static string RootGuid
		{
			get => uniqueRootGuid;
			set => uniqueRootGuid = value;
		}

	#endregion

	#region private properties

	#endregion

	#region public methods

		public static string GetCellGuid (int id)
		{
			return ROOT_GUID + uniqueRootGuid + string.Format(uniqueCellGuid, id);
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