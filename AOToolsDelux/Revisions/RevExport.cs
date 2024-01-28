using System.Collections.Generic;
using System.Reflection;

using X = Microsoft.Office.Interop.Excel;


namespace AOToolsDelux.Revisions
{
	class RevExport
	{
		private bool ExportToExcel(SortedList<string, string[]> revInfo)
		{
//			X.Application excel = new X.Application();
//
//			if (excel == null) return false;
//
//
//			excel.Visible = true;
//
//
//
//			X.Workbook workbook = excel.Workbooks.Add(Missing.Value);
//			X.Worksheet ws;
//
//			ws = workbook.Sheets.Item[1] as X.Worksheet;
//
//			ws.Name = "Revisions";
//
//			// add the header row in bold
//			int row = 1;
//
//			foreach (KeyValuePair<int, RevCol> kvp in RevCols)
//			{
//				// key = the order for the columns
//				// value = the column data
//
//				// if not export - skip this column
//				if (!kvp.Value.Export || kvp.Value.Column < 0) continue;
//
//				ws.Cells[1, kvp.Value.Column] = kvp.Value.Title;
//			}
//
//			var range = ws.get_Range("A1", "Z1");
//
//			range.Font.Bold = true;
//
//
//			row = 2;
//
//			foreach (KeyValuePair<string, string[]> riKvp in revInfo)
//			{
//				foreach (KeyValuePair<int, RevCol> rcKvp in RevCols)
//				{
//					// key = the order for the columns
//					// value = the column data
//
//					// if not export - skip this column
//					if (!rcKvp.Value.Export || rcKvp.Value.Column < 0) continue;
//
//					ws.Cells[row, rcKvp.Value.Column] = riKvp.Value[rcKvp.Key];
//				}
//				row++;
//			}
//
//			range.EntireColumn.AutoFit();
//
			return true;
		}
	}
}
