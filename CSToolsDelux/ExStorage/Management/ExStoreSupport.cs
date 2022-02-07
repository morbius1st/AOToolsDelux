#region using

using System.Text.RegularExpressions;
using CSToolsDelux.Utility;
using UtilityLibrary;

#endregion

// username: jeffs
// created:  10/22/2021 6:23:06 AM

namespace CSToolsDelux.ExStorage.Management
{
	/// <summary>
	/// Support routines for ExStoreController
	/// </summary>
	public class ExStoreSupport
	{
	#region private fields

	#endregion

	#region ctor

		public ExStoreSupport(string documentName)
		{
			IsDocValid = true;
			VendorId = Util.GetVendorId().Replace('.','_');
			DocumentName = documentName;

			if (documentName.IsVoid())
			{
				IsDocValid = false;
				return;
			}

			DocName = ConvertDocName(documentName);
			DsKey = VendorId + "_" + DocName;
		}

	#endregion

	#region public properties

		public bool IsDocValid { get; private set; }
		public string VendorId { get; private set; }
		public string DocumentName { get; private set; }
		public string DocName { get; private set; }
		public string DsKey { get; private set; }

	#endregion

	#region private properties

	#endregion

	#region public methods

		internal string ConvertDocName(string docName)
		{
			return Regex.Replace(docName, @"[^0-9a-zA-Z]", "");
		}

		internal bool MatchVendorId(string key)
		{
			string test = key.Substring(VendorId.Length);

			return test.Equals(VendorId);
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
			return "this is ExStoreSupport";
		}

	#endregion
	}
}