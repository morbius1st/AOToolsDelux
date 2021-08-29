#region using

#endregion

// username: jeffs
// created:  8/28/2021 7:48:35 PM

using System.Collections.Generic;
using CSToolsDelux.Fields.SchemaInfo.SchemaDefinitions;
using CSToolsDelux.Fields.SchemaInfo.SchemaFields;
using CSToolsDelux.WPF.FieldsWindow;

namespace CSToolsDelux.Fields.Testing
{
	public class ShowInfo
	{
	#region private fields

		private MainFields W;

	#endregion

	#region ctor

		public ShowInfo(MainFields w)
		{
			W = w;
		}

	#endregion

		public void ShowRootFields(SchemaFieldsRoot rootFields)
		{
			W.WriteLineMsg("Show Root Fields| ", "type 1");
			W.WriteMsg("\n");

			string name;
			string value;

			foreach (SchemaRootKey key in rootFields.KeyOrder)
			{
				name = rootFields[key].Name;
				value = rootFields[key].ValueString;

				W.WriteLineMsg($"key| {key}| ", $"name| {name}  value| {value}");
			}

			W.WriteMsg("\n");
			W.WriteLineMsg("Show Root Fields| ", "finished");
			W.WriteMsg("\n");

			W.ShowMsg();
		}



	#region system overrides

		public override string ToString()
		{
			return "this is ShowInfo";
		}

	#endregion
	}
}