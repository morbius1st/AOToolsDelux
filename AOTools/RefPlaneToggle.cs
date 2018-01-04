#region Namespaces
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
#endregion

namespace AOTools
{
	[Transaction(TransactionMode.Manual)]
	public class RefPlaneToggle : IExternalCommand
	{
		public Result Execute(
		  ExternalCommandData commandData,
		  ref string message,
		  ElementSet elements)
		{
			UIDocument uiDoc = commandData.Application.ActiveUIDocument;
			Document doc = uiDoc.Document;

			View av = doc.ActiveView;

			Category refPlanes = Category.GetCategory(doc, BuiltInCategory.OST_CLines);

			bool isVisible = refPlanes.get_Visible(av);

			if (av.CanCategoryBeHidden(refPlanes.Id))
			{
				using (Transaction t = new Transaction(doc, "Toggle Ref Plane Visibility"))
				{
					t.Start();
					refPlanes.set_Visible(av, !isVisible);
					t.Commit();
				}
			}
			else
			{
				SetStatusText("View template prevents toggling Reference Plane visibility");
			}

			return Result.Succeeded;

		}

		[DllImport("user32.dll",
			SetLastError = true,
			CharSet = CharSet.Auto)]
		static extern int SetWindowText(
			IntPtr hWnd,
			string lpString);

		[DllImport("user32.dll",
			SetLastError = true)]
		static extern IntPtr FindWindowEx(
			IntPtr hwndParent,
			IntPtr hwndChildAfter,
			string lpszClass,
			string lpszWindow);

		static void SetStatusText(string text)
		{
			IntPtr statusBar = FindWindowEx(
				GetWinHandle(), IntPtr.Zero,
				"msctls_statusbar32", "");

			if (statusBar != IntPtr.Zero)
			{
				SetWindowText(statusBar, text);
			}
		}

		static IntPtr GetWinHandle()
		{
			return System.Diagnostics.Process
				.GetCurrentProcess().MainWindowHandle;
		}
	}

}
