#region using
using System;
using System.Collections.Generic;
using CSToolsDelux.Fields.SchemaInfo.SchemaData;
using CSToolsDelux.Fields.SchemaInfo.SchemaData.SchemaDataDefinitions;
using CSToolsDelux.Fields.SchemaInfo.SchemaDefinitions;
using CSToolsDelux.Fields.SchemaInfo.SchemaFields;
using CSToolsDelux.WPF;

using UtilityLibrary;
#endregion

// username: jeffs
// created:  8/28/2021 7:48:35 PM



namespace CSToolsDelux.Fields.Testing
{
	public class ShowInfo
	{
	#region private fields

		private const int NAME_WIDTH = 20;
		private const int TYPE_WIDTH = 15;



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

		private string formatFieldInfo(string name, string type, string value)
		{
			return $"name| {name.PadRight(NAME_WIDTH)} Type| {type.PadRight(TYPE_WIDTH)}  value| {value}";
		}

		public void ShowRootFields(SchemaRootFields rootFields)
		{
			W.WriteLineAligned("this is| ", "CSToolsDelux");
			W.WriteLineAligned("this is| ", $"{documentName}");
			W.WriteLineAligned("Show Root Fields| ", "type 1");
			W.WriteAligned("\n");

			string name;
			string value;
			string type;

			foreach (SchemaRootKey key in rootFields.KeyOrder)
			{
				name = rootFields[key].Name;
				value = rootFields[key].ValueString;
				type =  rootFields[key].ValueType.Name;

				W.WriteLineAligned($"key| {key}| ", formatFieldInfo(name, type, value));
			}

			W.WriteAligned("\n");
			W.WriteLineAligned("Show Root Fields| ", "finished");
			W.WriteAligned("\n");

			W.ShowMsg();
		}

		public void ShowRootData(SchemaRootFields rootFields,
			SchemaRootData rootData)
		{
			W.WriteLineAligned("this is| ", "CSToolsDelux");
			W.WriteLineAligned("this is| ", $"{documentName}");
			W.WriteLineAligned("Show Root data| ", "type 1");
			W.WriteAligned("\n");

			string name;
			string type;
			string value;

			foreach (SchemaRootKey key in rootFields.KeyOrder)
			{
				name = rootData[key].FieldDef.Name;
				value = rootData[key].ValueString;
				type =  rootData[key].ValueType.Name;

				W.WriteLineAligned($"key| {key}| ", formatFieldInfo(name, type, value));
			}

			W.WriteAligned("\n");
			W.WriteLineAligned("Show Root data| ", "finished");
			W.WriteAligned("\n");

			W.ShowMsg();
		}
		

		
		public void ShowRootAppData(SchemaRootAppFields rootFields,
			SchemaRootAppData rootData)
		{
			W.WriteLineAligned("this is| ", "CSToolsDelux");
			W.WriteLineAligned("this is| ", $"{documentName}");
			W.WriteLineAligned("Show Root-App data| ", "type 1");
			W.WriteAligned("\n");

			string name;
			string type;
			string value;

			foreach (SchemaRootAppKey key in rootFields.KeyOrder)
			{
				name = rootData[key].FieldDef.Name;
				value = rootData[key].ValueString;
				type =  rootData[key].ValueType.Name;

				W.WriteLineAligned($"key| {key}| ", formatFieldInfo(name, type, value));
			}

			W.WriteAligned("\n");
			W.WriteLineAligned("Show Root-App data| ", "finished");
			W.WriteAligned("\n");

			W.ShowMsg();
		}
		


		public void ShowRootAppFields(SchemaRootAppFields fields)
		{
			W.WriteLineAligned("this is| ", "CSToolsDelux");
			W.WriteLineAligned("this is| ", $"{documentName}");
			W.WriteLineAligned("Show Root-App Fields| ", "type 1");
			W.WriteAligned("\n");

			string name;
			string type;
			string value;

			foreach (SchemaRootAppKey key in fields.KeyOrder)
			{
				name = fields[key].Name;
				value = fields[key].ValueString;
				type = fields[key].ValueType.Name;

				W.WriteLineAligned($"key| {key}| ", formatFieldInfo(name, type, value));
			}

			W.WriteAligned("\n");
			W.WriteLineAligned("Show Root-App Fields| ", "finished");
			W.WriteAligned("\n");

			W.ShowMsg();
		}
		
		public void ShowAppFields(SchemaAppFields fields)
		{
			W.WriteLineAligned("this is| ", "CSToolsDelux");
			W.WriteLineAligned("this is| ", $"{documentName}");
			W.WriteLineAligned("Show App Fields| ", "type 1");
			W.WriteAligned("\n");

			string name;
			string type;
			string value;

			foreach (SchemaAppKey key in fields.KeyOrder)
			{
				name = fields[key].Name;
				value = fields[key].ValueString;
				type = fields[key].ValueType.Name;

				W.WriteLineAligned($"key| {key}| ", formatFieldInfo(name, type, value));
			}

			W.WriteAligned("\n");
			W.WriteLineAligned("Show App Fields| ", "finished");
			W.WriteAligned("\n");

			W.ShowMsg();
		}

		public void ShowAppData(SchemaAppFields fields,
			SchemaAppData data)
		{
			W.WriteLineAligned("this is| ", "CSToolsDelux");
			W.WriteLineAligned("this is| ", $"{documentName}");
			W.WriteLineAligned("Show App data| ", "type 1");
			W.WriteAligned("\n");

			string name;
			string type;
			string value;

			foreach (SchemaAppKey key in fields.KeyOrder)
			{
				name = data[key].FieldDef.Name;
				value = data[key].ValueString;
				type =  data[key].ValueType.Name;

				W.WriteLineAligned($"key| {key}| ", formatFieldInfo(name, type, value));
			}

			W.WriteAligned("\n");
			W.WriteLineAligned("Show App data| ", "finished");
			W.WriteAligned("\n");

			W.ShowMsg();
		}

		
		public void ShowCellFields(SchemaCellFields fields)
		{
			W.WriteLineAligned("this is| ", "CSToolsDelux");
			W.WriteLineAligned("this is| ", $"{documentName}");
			W.WriteLineAligned("Show Cell Fields| ", "type 1");
			W.WriteAligned("\n");

			string name;
			string type;
			string value;

			foreach (SchemaCellKey key in fields.KeyOrder)
			{
				name = fields[key].Name;
				value = fields[key].ValueString;
				type = fields[key].ValueType.Name;

				W.WriteLineAligned($"key| {key}| ", formatFieldInfo(name, type, value));
			}

			W.WriteAligned("\n");
			W.WriteLineAligned("Show Cell Fields| ", "finished");
			W.WriteAligned("\n");

			W.ShowMsg();
		}
		
		public void ShowFields<TE, TF, TD>(TF fields)
			where TE : Enum
			where TF : ASchemaFields<TE, TD>
			where TD : SchemaDictionaryBase<TE>, new()
		{
			W.WriteLineAligned("this is| ", "CSToolsDelux");
			W.WriteLineAligned("this is| ", $"{documentName}");
			W.WriteLineAligned("Show Cell Fields| ", "type 1");
			W.WriteAligned("\n");

			string name;
			string type;
			string value;

			foreach (TE key in fields.KeyOrder)
			{
				name = fields[key].Name;
				value = fields[key].ValueString;
				type = fields[key].ValueType.Name;

				W.WriteLineAligned($"key| {key}| ", formatFieldInfo(name, type, value));
			}

			W.WriteAligned("\n");
			W.WriteLineAligned("Show Cell Fields| ", "finished");
			W.WriteAligned("\n");

			W.ShowMsg();
		}

		public void ShowCellData(SchemaCellData data, SchemaCellFields fields)
		{
			W.WriteLineAligned("this is| ", "CSToolsDelux");
			W.WriteLineAligned("this is| ", $"{documentName}");
			W.WriteLineAligned("Show Cell data| ", "type 1");
			W.WriteAligned("\n");

			string name;
			string type;
			string value;

			for (int i = 0; i < data.Data.Count; i++)
			{
				foreach (SchemaCellKey key in fields.KeyOrder)
				{
					name = data.Data[i][key].FieldDef.Name;
					value = data.Data[i][key].ValueString;
					type = data.Data[i][key].ValueType.Name;

					W.WriteLineAligned($"key| {key}| ",formatFieldInfo(name, type, value));
				}
			}


			W.WriteAligned("\n");
			W.WriteLineAligned("Show Cell data| ", "finished");
			W.WriteAligned("\n");

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