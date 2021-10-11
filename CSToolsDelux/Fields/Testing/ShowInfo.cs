#region using
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Autodesk.Revit.DB.ExtensibleStorage;
using CSToolsDelux.Fields.SchemaInfo.SchemaData;
using CSToolsDelux.Fields.SchemaInfo.SchemaData.SchemaDataDefinitions;
using CSToolsDelux.Fields.SchemaInfo.SchemaDefinitions;
using CSToolsDelux.Fields.SchemaInfo.SchemaFields;
using CSToolsDelux.WPF;
using CSToolsDelux.WPF.FieldsWindow;
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
		private const int CODE_WIDTH = 20;
		private const int PROC_WIDTH = 30;
		private const int FLOW_WIDTH = 8;
		private const int INFO_WIDTH = 15;

		private AWindow W;
		private string documentName;

	#endregion

	#region ctor

		public ShowInfo(AWindow w)
		{
			W = w;

			string docName = MainFields.DocName;
				
			this.documentName = docName.IsVoid() ? "un-named" : docName;

		}

	#endregion

		private string leftMarginSpacer = "   ";
		private string[] flowEnter      = new [] {"", "->"};
		private string[] flowExit       = new [] {"", "<-"};
		private string[] flowLoopBeg    = new [] {"  ", "*>"};
		private string[] flowLoopEnd    = new [] {"  ", "<*"};
		private string[] flowCont       = new [] {"    ", "v"};


		// private const int leftMarginAdjustAmt = 2;
		private static int leftMarginWidth;
		private string leftMargin;

		private void incLeftMargin()
		{
			Debug.WriteLine("increment");
			leftMarginWidth += 1;
			leftMargin = leftMarginSpacer.Repeat(leftMarginWidth);
		}
		
		private void decLeftMargin()
		{
			Debug.WriteLine("decrement");
			setLeftMarginWidth(leftMarginWidth - 1);
			leftMargin = leftMarginSpacer.Repeat(leftMarginWidth);
		}

		private void setLeftMargin(int pos)
		{
			setLeftMarginWidth(pos);
			leftMargin = leftMarginSpacer.Repeat(leftMarginWidth);
		}

		private void setLeftMarginWidth(int pos)
		{
			leftMarginWidth = pos <= 0 ? 0 : pos;
		}


		public void informStartEnter(int idx, string info, string code = "")
		{
			informStartX(idx, info, $"{flowEnter[0]}{flowEnter[1]}", code);
			incLeftMargin();
		}

		
		public void informStartExit(int idx, string info, string code = "")
		{
			decLeftMargin();
			informStartX(idx, info, $"{flowExit[0]}{flowExit[1]}", code);
		}
		
		public void informStartLoopBeg(int idx, string info, string code = "")
		{
			informStartX(idx, info, $"{flowLoopBeg[0]}{flowLoopBeg[1]}", code, flowLoopBeg[1]);
			incLeftMargin();
		}

		
		public void informStartLoopEnd(int idx, string info, string code = "")
		{
			decLeftMargin();
			informStartX(idx, info, $"{flowLoopEnd[0]}{flowLoopEnd[1]}", code, flowLoopEnd[1]);
		}

				
		public void informStartCont(int idx, string code = "")
		{
			informStartX(idx, null, $"{flowCont[0]}{flowCont[1]}", code);
		}

		private void informStartX(int idx, string info, string flow, string code, string altProcName = null)
		{
			string p = null;
			string d = null;

			if (idx != SampleData.xxx)
			{
				if (altProcName != null)
				{
					p = altProcName;
				}
				else
				{
					p = $"{SampleData.ti[idx].routine}";
				}

				d = $"{SampleData.ti[idx].description}";
			}

			informStartY(p, info, flow, code, d);
		}


		private void informStartY(string procName, string info, string flow, string code, string desc)
		{
			string msgA = "v";
			string msgB;

			// bool b0 = procName == null;
			// bool b1 = info.IsVoid();
			// bool b2 = flow.IsVoid();
			// bool b3 = code.IsVoid();
			// bool bx = !(b0 && b1 && b2 && b3);
			//
			// bool b = !(procName == null && info.IsVoid() && flow.IsVoid() && code.IsVoid());


			if (!(procName == null && info.IsVoid() && flow.IsVoid() && code.IsVoid()))
			{
				msgB = $"{leftMargin}{procName}";
				msgA = $"{msgA} {msgB.PadRight(PROC_WIDTH)}";
			}

			informStart(msgA, info, flow, code, $"{desc}");

		}

		public void informStart(int idx, string info, string flow, string code = "")
		{
			string msgA = "D";

			// bool b0 = idx == SampleData.xxx;
			// bool b1 = info.IsVoid();
			// bool b2 = flow.IsVoid();
			// bool b3 = code.IsVoid();
			// bool bx = !(b0 && b1 && b2 && b3);
			//
			// bool b = !(idx == SampleData.xxx && info.IsVoid() && flow.IsVoid() && code.IsVoid());


			if (!(idx == SampleData.xxx && info.IsVoid() && flow.IsVoid() && code.IsVoid()))
			{
				msgA = $"{msgA} {SampleData.ti[idx].routine.PadRight(PROC_WIDTH)}";
			}

			informStart(msgA, info, flow, code, $"{SampleData.ti[idx].description}");
		}

		private void informStart(string msgA, string info, 
			string flow, string code, string desc)
		{
			string msgB;
			string msgC = "";
			string msgD;

			if (!(info.IsVoid() && flow.IsVoid() && code.IsVoid()))
			{
				msgB = flow.IsVoid() ? "" : $"{flow}"; 

				msgD = code.IsVoid() ? "" : $"{code.PadRight(CODE_WIDTH)}";

				msgC = info.IsVoid() ? "" : $"{info.PadRight(INFO_WIDTH)}";

				msgC = msgC.IsVoid() ? (desc.IsVoid() ? null : $"{" ".PadRight(INFO_WIDTH)} | ({desc})") : $"{msgC} | ({desc})";

				msgC = msgB.IsVoid() && msgC.IsVoid() && msgD.IsVoid() ? null : 
					$"| {msgB.PadRight(FLOW_WIDTH)}"
					+ $" {msgD.PadRight(CODE_WIDTH)}"
					+ $"| {msgC}";
			}

			debugMsg(msgA, msgC);
		}

		public void informStart(  int idx, int location, string flow, string code)
		{
			string msgA = $"B {SampleData.ti[idx].routine.PadRight(PROC_WIDTH)}";
		
			string msgB = $"{flow.PadRight(FLOW_WIDTH)} ";
			 msgB = $"{msgB}{code.PadRight(CODE_WIDTH)} | ({SampleData.ti[idx].description})";
		
			string msgC = $"location {SampleData.ti[location].routine} > {msgB}";
		
			// W.WriteLine(msgA, msgC);
			debugMsg(msgA, msgB);
		}


		private void debugMsg(string msgA, string msgB)
		{
			W.WriteDebugMsg(msgA, msgB, 
				$"{msgB.PadRight(60)} | {(SampleData.TestIdx + 1)} : {SampleData.PrimeLoopIdx}", "", 40);

			// W.WriteLine($"{msgA} {msgB}");
			// Debug.WriteLine($"{msgA} {msgB.PadRight(60)} | {(SampleData.TestIdx + 1)} : {SampleData.PrimeLoopIdx}");

		}

		public void ShowSchemas(IList<Schema> schemas)
		{
			if (schemas != null && schemas.Count > 0)
			{
				W.WriteLineAligned($"schema not found| {(schemas?.Count.ToString() ?? "is null")}");
			}

			foreach (Schema s in schemas)
			{
				W.WriteLineAligned($"schema|", $"name| {s.SchemaName.PadRight(35)}  vendor id| {s.VendorId.PadRight(20)}   guid| {s.GUID.ToString()}");
			}

			W.ShowMsg();
		}

		public void ShowSchema(Schema s)
		{
			W.WriteLine($"Show Schema|");
			W.WriteMsg("\n");
			W.WriteLineAligned("name| ", $"{s.SchemaName}");
			W.WriteLineAligned("desc| ", $"{s.Documentation}");
			W.WriteLineAligned("vendId| ", $"{s.VendorId}");
			W.WriteLineAligned("Guid| ", $"{s.GUID}");

			foreach (Field f in s.ListFields())
			{
				W.WriteLineAligned("field| ", $"name| {f.FieldName}  type| {f.ValueType.Name}"
					+ $"  desc| {f.Documentation}");
			}

			W.WriteMsg("\n");

			W.ShowMsg();
			;
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
		
		// public void ShowAppFields(SchemaAppFields fields)
		// {
		// 	W.WriteLineAligned("this is| ", "CSToolsDelux");
		// 	W.WriteLineAligned("this is| ", $"{documentName}");
		// 	W.WriteLineAligned("Show App Fields| ", "type 1");
		// 	W.WriteAligned("\n");
		//
		// 	string name;
		// 	string type;
		// 	string value;
		//
		// 	foreach (SchemaAppKey key in fields.KeyOrder)
		// 	{
		// 		name = fields[key].Name;
		// 		value = fields[key].ValueString;
		// 		type = fields[key].ValueType.Name;
		//
		// 		W.WriteLineAligned($"key| {key}| ", formatFieldInfo(name, type, value));
		// 	}
		//
		// 	W.WriteAligned("\n");
		// 	W.WriteLineAligned("Show App Fields| ", "finished");
		// 	W.WriteAligned("\n");
		//
		// 	W.ShowMsg();
		// }
		//
		// public void ShowAppData(SchemaAppFields fields,
		// 	SchemaAppData data)
		// {
		// 	W.WriteLineAligned("this is| ", "CSToolsDelux");
		// 	W.WriteLineAligned("this is| ", $"{documentName}");
		// 	W.WriteLineAligned("Show App data| ", "type 1");
		// 	W.WriteAligned("\n");
		//
		// 	string name;
		// 	string type;
		// 	string value;
		//
		// 	foreach (SchemaAppKey key in fields.KeyOrder)
		// 	{
		// 		name = data[key].FieldDef.Name;
		// 		value = data[key].ValueString;
		// 		type =  data[key].ValueType.Name;
		//
		// 		W.WriteLineAligned($"key| {key}| ", formatFieldInfo(name, type, value));
		// 	}
		//
		// 	W.WriteAligned("\n");
		// 	W.WriteLineAligned("Show App data| ", "finished");
		// 	W.WriteAligned("\n");
		//
		// 	W.ShowMsg();
		// }

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

			for (int i = 0; i < data.DataList.Count; i++)
			{
				foreach (SchemaCellKey key in fields.KeyOrder)
				{
					data.Index = i;
					name = data.Data[key].FieldDef.Name;
					value = data.Data[key].ValueString;
					type = data.Data[key].ValueType.Name;

					W.WriteLineAligned($"key| {key}| ",formatFieldInfo(name, type, value));
				}
			}


			W.WriteAligned("\n");
			W.WriteLineAligned("Show Cell data| ", "finished");
			W.WriteAligned("\n");

			W.ShowMsg();
		}

		private string formatFieldInfo(string name, string type, string value)
		{
			return $"name| {name.PadRight(NAME_WIDTH)} Type| {type.PadRight(TYPE_WIDTH)}  value| {value}";
		}

	#region system overrides

		public override string ToString()
		{
			return "this is ShowInfo";
		}

	#endregion
	}
}