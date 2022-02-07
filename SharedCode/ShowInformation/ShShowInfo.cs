#region using

using SharedCode.Fields.Testing;
using SharedCode.Windows;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using SharedCode.Fields.SchemaInfo.SchemaData.DataTemplates;
using SharedCode.Fields.SchemaInfo.SchemaFields.FieldsTemplates;
using SharedCode.Fields.SchemaInfo.SchemaData.DataTemplate;
using SharedCode.Fields.SchemaInfo.SchemaSupport;
using UtilityLibrary;
using static SharedCode.Windows.ColData;
using static SharedCode.Fields.SchemaInfo.SchemaSupport.FieldColumns;

#endregion

// username: jeffs
// created:  8/28/2021 7:48:35 PM


namespace SharedCode.ShowInformation
{
	public class ShShowInfo
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
		private string programName;

	#endregion

	#region ctor

		public ShShowInfo( AWindow w, string progName, string docName)
		{
			W = w;
			this.programName = progName;
			this.documentName = docName.IsVoid() ? "un-named" : docName;
		}

	#endregion

		private string leftMarginSpacer = "   ";
		private string[] flowEnter      = new [] { "", "->" };
		private string[] flowExit       = new [] { "", "<-" };
		private string[] flowLoopBeg    = new [] { "  ", "*>" };
		private string[] flowLoopEnd    = new [] { "  ", "<*" };
		private string[] flowCont       = new [] { "    ", "v" };


		// private const int leftMarginAdjustAmt = 2;
		private static int leftMarginWidth;
		private string leftMargin;

	#region show methods

	#endregion

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

				msgC = msgB.IsVoid() && msgC.IsVoid() && msgD.IsVoid()
					? null
					: $"| {msgB.PadRight(FLOW_WIDTH)}"
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

		private string formatFieldInfo(string name, string type, string value)
		{
			return $"name| {name.PadRight(NAME_WIDTH)} Type| {type.PadRight(TYPE_WIDTH)}  value| {value}";
		}

		private void debugMsg(string msgA, string msgB)
		{
			W.WriteLineDebugMsg(msgA, null, msgB,
				$"{msgB.PadRight(60)} | {(SampleData.TestIdx + 1)} : {SampleData.PrimeLoopIdx}", 40);

			// W.WriteLine($"{msgA} {msgB}");
			// Debug.WriteLine($"{msgA} {msgB.PadRight(60)} | {(SampleData.TestIdx + 1)} : {SampleData.PrimeLoopIdx}");
		}

		// good - old method
		public void ShowDataGeneric<TE>(ADataTempBase<TE> dataTempBaseDef) where TE : Enum, new()
		{
			W.WriteLineAligned("this is (prog name)| ", null, msg2: programName);
			W.WriteLineAligned("this is  (doc name)| ", null, msg2: $"{(documentName ?? "un-named")}");
			W.WriteLineAligned("Show Root data| ", null, msg2: "type 2");
			W.WriteLineAligned("Show Root data| ", null, msg2: $"schema name {dataTempBaseDef.SchemaName}");
			W.WriteLineAligned("Show Root data| ", null, msg2: $"schema desc {dataTempBaseDef.SchemaDesc}");
			W.WriteLineAligned("Show Root data| ", null, msg2: $"schema date {dataTempBaseDef.SchemaCreateDate}");
			W.WriteLineAligned("Show Root data| ", null, msg2: $"schema ver  {dataTempBaseDef.SchemaVersion}");
			W.WriteLineAligned("Show Root data| ", null, msg2: $"data lists| {dataTempBaseDef.DataIndexMaxAllowed}");
			W.WriteAligned("\n", null);

			string name;
			string value;
			string type;

			for (int i = 0; i < dataTempBaseDef.DataIndexMaxAllowed; i++)
			{
				W.WriteLineAligned($"datalist idx| ", null, msg2: $"{i}");

				foreach (TE key in dataTempBaseDef.FieldsData.FieldOrderDefault)
				{
					name =  dataTempBaseDef[i, key].AFieldsMembers.Name;
					value = dataTempBaseDef[i, key].ValueString;
					type =  dataTempBaseDef[i, key].ValueType.Name;

					W.WriteLineAligned($"key| {key}| ", null, msg2: formatFieldInfo(name, type, value));
				}

				W.WriteAligned("\n", null);
			}

			W.WriteAligned("\n", null);
			W.WriteLineAligned("Show Root data| ", null, msg2: "finished");
			W.WriteAligned("\n", null);

			W.ShowMsg();
		}

		// good - old method
		public void ShowSchemaFields<TE>(AFieldsTemp<TE> fields) where TE : Enum, new()
		{
			showFieldsHeader(fields.SchemaName);


			string name;
			string type;
			string value;

			foreach (TE key in fields.FieldOrderDefault)
			{
				AFieldsMembers<TE> f = fields[key];


				name  = f.Name;
				value = f.ValueString;
				type  = f.ValueType.Name;

				W.WriteLineAligned($"key| {key}| ", null, formatFieldInfo(name, type, value));
			}

			W.WriteAligned("\n", null);
			W.WriteLineAligned("Show Root Fields| ", null, "finished");
			W.WriteAligned("\n", null);

			W.ShowMsg();
		}

		// good - old method
		public void ShowSchemaField<TE>(AFieldsTemp<TE> fields, TE key) where TE : Enum, new()
		{
			showFieldsHeader(fields.SchemaName);

			W.WriteAligned("\n", null);
			W.WriteAligned("method 1", null);

			string name = fields[key].Name;
			string value = fields[key].ValueString;
			string type = fields[key].ValueType.Name;
			fields.GetValue<string>(key);

			W.WriteLineAligned($"key| {key}| ", null, msg2: formatFieldInfo(name, type, value));


			W.WriteAligned("\n", null);
			W.WriteAligned("method 2", null);

			AFieldsMembers<TE> f = fields[key];

			name = f.Name;
			value = f.ValueString;
			type = f.ValueType.Name;


			W.WriteLineAligned($"key| {key}| ", null, msg2: formatFieldInfo(name, type, value));

			W.WriteAligned("\n", null);
			W.WriteAligned("method 2", null);

			FieldsTemp<TE, string> fd = fields.GetField<string>(key);

			name = fd.Name;
			value = fd.ValueString;
			type = fd.ValueType.Name;


			W.WriteLineAligned($"key| {key}| ", null, msg2: formatFieldInfo(name, type, value));

			W.WriteAligned("\n", null);
			W.WriteLineAligned($"Show {fields.SchemaName}, single field| ", null, msg2: "finished");
			W.WriteAligned("\n", null);

			W.ShowMsg();
		}

		// show the data metadata - which is contained in the fields data
		public void ShowDataInfo(
			List<DataColumns> dataOrder,
			Dictionary<DataColumns, ColData> dataHdr,
			Dictionary<DataColumns, string> dataHdrInfo,
			List<List<Dictionary<DataColumns, string>>> dataInfo
			)
		{

			Dictionary<DataColumns, string> dataRowInfo;

			// dataTempBaseDef is AdataTempBase which is the control class
			// holds the data collection, fields, etc.

			foreach (List<Dictionary<DataColumns, string>> lists in dataInfo)
			{
				W.WriteNewLine();
				W.WriteRow(dataOrder, dataHdr, dataHdrInfo, DataTemplateMembers.MaxHdrRows, JustifyVertical.MIDDLE, true, true);

				foreach (Dictionary<DataColumns, string> info in lists)
				{

					W.WriteRow(dataOrder, dataHdr, info, DataTemplateMembers.MaxHdrRows, JustifyVertical.TOP, false, false);
				}
			}

			W.WriteNewLine();
			W.WriteLineAligned("Show Root data", "| ", "finished");
			W.WriteNewLine();

			W.ShowMsg();
		}

		public void ShowDataMembers<TSk>(ADataTempBase<TSk> data) where TSk : Enum, new ()
		{
			List<List<Dictionary<DataColumns, string>>> info = DataTemplateMembers.FormatData(data);

			showDataHeader(data);

			ShowDataInfo(
				DataTemplateMembers.DefaultDataOrder,
				DataTemplateMembers.DataHdr,
				DataTemplateMembers.DataHdrInfo,
				info);
		}


		// show the data metadata - which is contained in the fields data
		public void ShowFieldInfo(
			List<FieldColumns> dataOrder,
			Dictionary<FieldColumns, ColData> dataHdr,
			Dictionary<FieldColumns, string> dataHdrInfo,
			List<List<Dictionary<FieldColumns, string>>> dataInfo
			)
		{

			Dictionary<FieldColumns, string> dataRowInfo;

			// dataTempBaseDef is AdataTempBase which is the control class
			// holds the data collection, fields, etc.

			foreach (List<Dictionary<FieldColumns, string>> lists in dataInfo)
			{
				W.WriteNewLine();
				W.WriteRow(dataOrder, dataHdr, dataHdrInfo, DataTemplateMembers.MaxHdrRows, JustifyVertical.MIDDLE, true, true);

				foreach (Dictionary<FieldColumns, string> info in lists)
				{

					W.WriteRow(dataOrder, dataHdr, info, DataTemplateMembers.MaxHdrRows, JustifyVertical.TOP, false, false);
				}
			}

			W.WriteNewLine();
			W.WriteLineAligned("Show Root data", "| ", "finished");
			W.WriteNewLine();

			W.ShowMsg();
		}


		public void ShowFieldMembers<TSk>(AFieldsTemp<TSk> data) where TSk : Enum, new()
		{

		}





		/*

		// show the data metadata - which is contained in the fields data
		public void ShowDataInfo3<Tsk>(
			ADataTempBase<Tsk> data,
			List<DataColumns> dataOrder,
			Dictionary<DataColumns, ColData> dataHdr,
			Dictionary<DataColumns, string> dataHdrInfo
			) where Tsk : Enum, new()
		{
			showDataHeader(data);

			Dictionary<DataColumns, string> dataRowInfo;

			// dataTempBaseDef is AdataTempBase which is the control class
			// holds the data collection, fields, etc.
			for (int i = 0; i < data.DataIndexMaxAllowed; i++)
			{
				W.WriteLineAligned($"datalist idx", "| ", $"{i}");
				W.WriteNewLine();
				W.WriteRow(dataOrder, dataHdr, dataHdrInfo, DataTemplateMembers.MaxHdrRows, JustifyVertical.MIDDLE, true, true);

				data.DataIndex = i;

				foreach (KeyValuePair<Tsk, ADataMember<Tsk>> kvp in data.Data)
				{
					dataRowInfo = kvp.Value.DataRowInfo();

					W.WriteRow(dataOrder, dataHdr, dataRowInfo, DataTemplateMembers.MaxHdrRows, JustifyVertical.TOP, false, false);
				}
			}

			W.WriteNewLine();
			W.WriteLineAligned("Show Root data", "| ", "finished");
			W.WriteNewLine();

			W.ShowMsg();
		}



		private ColData[] ShowDataPrepHeader<TE>(ADataTempBase<TE> dataTemp) where TE : Enum, new()
		{
			ColData[] colData = new ColData[dataTemp.Fields.Count];

			int i = 0;

			foreach (KeyValuePair<TE, IFieldsTemp<TE>> kvp in dataTemp.Fields)
			{
				IFieldsTemp<TE> a = kvp.Value;
				// colData[i++] = new ColData( a.DisplayWidth, Justify.CENTER, Justify.LEFT, a.Name);
			}

			return colData;
		}


		private List<ColData[]> ShowDataPrepValues<TE>(ADataTempBase<TE> dataTemp) where TE : Enum, new()
		{
			int count = dataTemp.Fields.Count;

			ColData[] colData;

			List<ColData[]> colDataRows = new List<ColData[]>();

			for (int i = 0; i < dataTemp.DataIndexMaxAllowed; i++)
			{
				colData = new ColData[count];

				int j = 0;

				foreach (KeyValuePair<TE, ADataTemp<TE>> kvp in dataTemp.Data)
				{
					// colData[j++] = new ColData( 20, Justify.LEFT, Justify.LEFT, kvp.Value.Key.ToString());
				}

				colDataRows.Add(colData);
			}

			return colDataRows;
		}


		public void ShowDataGen<TE>(ADataTempBase<TE> dataTempBaseDef) where TE : Enum, new()
		{
			W.WriteLineAligned("this is (prog name)| ", programName);
			W.WriteLineAligned("this is  (doc name)| ", $"{(documentName ?? "un-named")}");
			W.WriteLineAligned("Show Base data| ", "type 2");
			W.WriteLineAligned("Show Base data| ", $"schema name {dataTempBaseDef.SchemaName}");
			W.WriteLineAligned("Show Base data| ", $"schema desc {dataTempBaseDef.SchemaDesc}");
			W.WriteLineAligned("Show Base data| ", $"schema date {dataTempBaseDef.SchemaCreateDate}");
			W.WriteLineAligned("Show Base data| ", $"schema ver  {dataTempBaseDef.SchemaVersion}");
			W.WriteLineAligned("Show Base data| ", $"data lists| {dataTempBaseDef.DataIndexMaxAllowed}");
			W.WriteAligned("\n");

			AWindow.ColData[] values;
			AWindow.ColData[] header;
			header = Mc(new []
			{
				Tx("Key", 25, Justify.LEFT),
				Tx("Name", 25, Justify.CENTER),
				Tx("Value", 16, Justify.CENTER),
				Tx("Type", 100, Justify.CENTER)
			});

			for (int i = 0; i < dataTempBaseDef.DataIndexMaxAllowed; i++)
			{
				W.WriteLineAligned($"datalist idx| ", $"{i}");

				W.WriteColumns(header, true);

				foreach (TE key in dataTempBaseDef.FieldsData.FieldOrderDefault)
				{
					values = Mc(new []
					{
						Tx(key.ToString(), header[0].Width, Justify.LEFT),
						Tx(dataTempBaseDef[i, key].FieldsTemp.Name, header[0].Width, Justify.LEFT),
						Tx(dataTempBaseDef[i, key].ValueString, header[1].Width, Justify.LEFT),
						Tx(dataTempBaseDef[i, key].ValueType.Name, header[2].Width, Justify.LEFT)
					});
					W.WriteColumns(values, true);
				}

				W.WriteAligned("\n");
			}

			W.WriteAligned("\n");
			W.WriteLineAligned("Show Root data| ", "finished");
			W.WriteAligned("\n");

			W.ShowMsg();
		}
		*/


	#region private methods

		private void showDataHeader<Tsk>(ADataTempBase<Tsk> data) where Tsk : Enum, new()
		{
			W.WriteLineAligned("this is (prog name)", "| ", programName);
			W.WriteLineAligned("this is  (doc name)", "| ", $"{(documentName ?? "un-named")}");
			W.WriteLineAligned("schema name", "| ", $"{data.SchemaName}");
			W.WriteLineAligned("schema desc", "| ", $"{data.SchemaDesc}");
			W.WriteLineAligned("schema date", "| ", $"{data.SchemaCreateDate}");
			W.WriteLineAligned("schema ver", "| " , $"{data.SchemaVersion}");
			W.WriteLineAligned("data lists", "| " , $"{data.DataIndexMaxAllowed}");
			W.WriteNewLine();
		}

		private void showFieldsHeader(string type)
		{
			showHeader();
			W.WriteLineAligned("Show Fields| ", null, msg2: $"Type| {type}");
			W.WriteAligned("\n", null);
		}

		private void showHeader()
		{
			W.WriteLineAligned("this is (prog name)| ", null, msg2: programName);
			W.WriteLineAligned("this is  (doc name)| ", null, msg2: $"{(documentName ?? "un-named")}");
		}

	#endregion


/*
		public void ShowRootFields(SchemaRootFields fields)
		{
			W.WriteLineAligned("this is (prog name)| ", "CSToolsDelux");
			W.WriteLineAligned("this is  (doc name)| ", $"{(documentName ?? "un-named")}");
			W.WriteLineAligned("Show Root Fields| ", "type 1");
			W.WriteAligned("\n");

			string name;
			string type;
			string value;

			foreach (SchemaRootKey key in fields.KeyOrder)
			{
				name = fields[key].Name;
				value = fields[key].ValueString;
				type = fields[key].ValueType.Name;

				W.WriteLineAligned($"key| {key}| ", formatFieldInfo(name, type, value));
			}

			W.WriteAligned("\n");
			W.WriteLineAligned("Show Root Fields| ", "finished");
			W.WriteAligned("\n");

			W.ShowMsg();
		}
		public void ShowCellFields(SchemaCellFields fields)
		{
			W.WriteLineAligned("this is (prog name)| ", "CSToolsDelux");
			W.WriteLineAligned("this is  (doc name)| ", $"{(documentName ?? "un-named")}");
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
		public void ShowLockFields(SchemaLockFields fields)
		{
			W.WriteLineAligned("this is (prog name)| ", "CSToolsDelux");
			W.WriteLineAligned("this is  (doc name)| ", $"{(documentName ?? "un-named")}");
			W.WriteLineAligned("Show lock Fields| ", "type 1");
			W.WriteAligned("\n");

			string name;
			string type;
			string value;

			foreach (SchemaLockKey key in fields.KeyOrder)
			{
				name = fields[key].Name;
				value = fields[key].ValueString;
				type = fields[key].ValueType.Name;

				W.WriteLineAligned($"key| {key}| ", formatFieldInfo(name, type, value));
			}

			W.WriteAligned("\n");
			W.WriteLineAligned("Show lock Fields| ", "finished");
			W.WriteAligned("\n");

			W.ShowMsg();
		}
	


		public void ShowRootData(SchemaRootFields rootFields, SchemaRootData rootData)
		{
			W.WriteLineAligned("this is (prog name)| ", "CSToolsDelux");
			W.WriteLineAligned("this is  (doc name)| ", $"{(documentName ?? "un-named")}");
			W.WriteLineAligned("Show Root data| ", "type 1");
			W.WriteAligned("\n");

			string name;
			string value;
			string type;

			foreach (SchemaRootKey key in rootFields.FieldOrderDefault)
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


		public void ShowCellData( SchemaCellFields fields, SchemaCellData data)
		{
			W.WriteLineAligned("this is (prog name)| ", "CSToolsDelux");
			W.WriteLineAligned("this is  (doc name)| ", $"{(documentName ?? "un-named")}");
			W.WriteLineAligned("Show Cell data| ", "type 1");
			W.WriteAligned("\n");

			string name;
			string type;
			string value;

			for (int i = 0; i < data.DataList.Count; i++)
			{
				foreach (SchemaCellKey key in fields.FieldOrderDefault)
				{
					data.Index = i;
					name = data.Data[key].FieldDef.Name;
					value = data.Data[key].ValueString;
					type = data.Data[key].ValueType.Name;

					W.WriteLineAligned($"key| {key}| ", formatFieldInfo(name, type, value));
				}
			}


			W.WriteAligned("\n");
			W.WriteLineAligned("Show Cell data| ", "finished");
			W.WriteAligned("\n");

			W.ShowMsg();
		}

		public void ShowLockData(SchemaLockFields rootFields, SchemaLockData rootData)
		{
			W.WriteLineAligned("this is (prog name)| ", "CSToolsDelux");
			W.WriteLineAligned("this is  (doc name)| ", $"{(documentName ?? "un-named")}");
			W.WriteLineAligned("Show Root-App data| ", "type 1");
			W.WriteAligned("\n");

			string name;
			string value;
			string type;

			foreach (SchemaLockKey key in rootFields.FieldOrderDefault)
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



		private void showData<TE, TD, TF>(TE[] keys,
			ISchemaData<TE, TD, TF> data)
			where TE : Enum
			where TD : SchemaDataDictionaryBase<TE>
			where TF : SchemaDictionaryBase<TE>
		{
			string name;
			string value;
			string type;

			foreach (TE k in keys)
			{
				name = data[k].FieldDef.Name;
				value = data[k].ValueString;
				type = data[k].ValueType.Name;

				W.WriteLineAligned($"key| {k}| ", formatFieldInfo(name, type, value));
			}
		}


		public void ShowSchemaFields<TE, TF, TD>(TF fields)
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

			foreach (TE key in fields.FieldOrderDefault)
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
*/

	#region system overrides

		public override string ToString()
		{
			return "this is ShowInfo";
		}

	#endregion
	}
}