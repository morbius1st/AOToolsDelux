#region using

#endregion

// username: jeffs
// created:  8/28/2021 7:48:35 PM

using System;
using System.Collections.Generic;
using CSToolsDelux.Fields.SchemaInfo.SchemaData;
using CSToolsDelux.Fields.SchemaInfo.SchemaDefinitions;
using CSToolsDelux.Fields.SchemaInfo.SchemaFields;
using CSToolsDelux.WPF;
using CSToolsDelux.WPF.FieldsWindow;
using UtilityLibrary;

namespace CSToolsDelux.Fields.Testing
{
	public class ShowInfo
	{
	#region private fields

		private AWindow W;
		private string documentName;

	#endregion

	#region ctor

		public ShowInfo(AWindow w, string documentName)
		{
			W = w;

			this.documentName = documentName.IsVoid() ? "un-named" : documentName;
		}

	#endregion

		public void ShowRootFields(SchemaRootFields rootFields)
		{
			W.WriteLineMsg("this is| ", "CSToolsDelux");
			W.WriteLineMsg("this is| ", $"{documentName}");
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

		public void ShowRootData(SchemaRootFields rootFields,
			SchemaRootData rootData)
		{
			W.WriteLineMsg("this is| ", "CSToolsDelux");
			W.WriteLineMsg("this is| ", $"{documentName}");
			W.WriteLineMsg("Show Root data| ", "type 1");
			W.WriteMsg("\n");

			string name;
			string type;
			string value;

			foreach (SchemaRootKey key in rootFields.KeyOrder)
			{
				name = rootData[key].FieldDef.Name;
				value = rootData[key].ValueString;
				type =  rootData[key].ValueType.Name;

				W.WriteLineMsg($"key| {key}| ", $"name| {name}  type| {type}  value| {value}");
			}

			W.WriteMsg("\n");
			W.WriteLineMsg("Show Root data| ", "finished");
			W.WriteMsg("\n");

			W.ShowMsg();
		}
		


		public void ShowAppFields(SchemaAppFields fields)
		{
			W.WriteLineMsg("this is| ", "CSToolsDelux");
			W.WriteLineMsg("this is| ", $"{documentName}");
			W.WriteLineMsg("Show App Fields| ", "type 1");
			W.WriteMsg("\n");

			string name;
			string value;

			foreach (SchemaAppKey key in fields.KeyOrder)
			{
				name = fields[key].Name;
				value = fields[key].ValueString;

				W.WriteLineMsg($"key| {key}| ", $"name| {name}  value| {value}");
			}

			W.WriteMsg("\n");
			W.WriteLineMsg("Show App Fields| ", "finished");
			W.WriteMsg("\n");

			W.ShowMsg();
		}

		public void ShowAppData(SchemaAppFields fields,
			SchemaAppData data)
		{
			W.WriteLineMsg("this is| ", "CSToolsDelux");
			W.WriteLineMsg("this is| ", $"{documentName}");
			W.WriteLineMsg("Show App data| ", "type 1");
			W.WriteMsg("\n");

			string name;
			string type;
			string value;

			foreach (SchemaAppKey key in fields.KeyOrder)
			{
				name = data[key].FieldDef.Name;
				value = data[key].ValueString;
				type =  data[key].ValueType.Name;

				W.WriteLineMsg($"key| {key}| ", $"name| {name}  type| {type}  value| {value}");
			}

			W.WriteMsg("\n");
			W.WriteLineMsg("Show App data| ", "finished");
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