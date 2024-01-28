#region Namespaces
using System;
using System.Runtime.InteropServices;

#endregion

namespace AOTools
{
	public static class User32 
	{
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

		public static void SetStatusText(string text)
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
