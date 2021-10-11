#region + Using Directives
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

using CSToolsDelux.Fields.ExStorage.ExStorManagement;
using CSToolsDelux.Fields.FieldsManagement;
using CSToolsDelux.Fields.Testing;
using CSToolsDelux.WPF;
using CSToolsStudies.FieldsManagement;
using UtilityLibrary;
using FieldsStartProcedure = CSToolsStudies.FieldsManagement.FieldsStartProcedure;

#endregion

// user name: jeffs
// created:   9/18/2021 10:35:20 AM

namespace CSToolsStudies.Testing
{

	public class FieldStartProcedure
	{
		private FieldsStartProcedure fs;
		private ShowInfo show;
		private AWindow W;

		public FieldStartProcedure(AWindow w)
		{
			W = w;
			SampleData.W = w;
			fs = new FieldsStartProcedure(w);
			show = new ShowInfo(w);
		}

		// get data / show data
		// proc00
		public ExStoreRtnCodes proc00()
		{
			string procName = "proc00";
			int op = SampleData.p00;

			ExStoreRtnCodes result = ExStoreRtnCodes.XRC_GOOD;

			for (int i = 0; i < SampleData.tests; i++)
			{
				SampleData.TestIdx = i;

				if (i + 1 < SampleData.firstTest)
				{
					show.informStart(op, $"skipping test| {SampleData.TestNames[i]}", "");
					continue;
				}

				show.informStart(SampleData.xxx, "", "");
				show.informStartEnter(op, $"entering start| {SampleData.TestNames[i]}");

				result = fs.DoesDataStoreExist();

				show.informStartExit(op,"start complete", result.ToString());


				show.informStart(SampleData.xxx, "", "");

				W.ShowMsg();
			}
			
			return result;
		}

		// private void showInfo(string msgA, string msgB = null)
		// {
		// 	string msg = msgB.IsVoid() ? "" : $"| {msgB}";
		//
		// 	msg = $"A at {msgA.PadRight(ShowInfo.PROC_WIDTH)} {msg}";
		//
		// 	W.WriteLineAligned(msg);
		// 	Debug.WriteLine(msg);
		// }
	}
}
