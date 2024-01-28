#region + Using Directives
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using UtilityLibrary;

#endregion

// user name: jeffs
// created:   1/6/2024 3:34:45 PM

// namespace JackRvtTst01.Windows.Support
namespace UtilityLibrary
{
	public static class M
	{
		private static string deferred;

		private static int marginSize = 0;
		private static int marginSpaceSize = 2;
		private static IWin win;

		private static Stack<IWin> winStack = new Stack<IWin>();

		public static IWin Win
		{
			get => win;
			set
			{
				win = value;
		
				if (!deferred.IsVoid())
				{
					sendMessage(win, deferred);
					deferred = string.Empty;
				}
			}
		}

		public static void Push(IWin  w)
		{
			Win = w;
			winStack.Push(w);
		}

		public static void Pop()
		{
			if (winStack.Count == 1) return;
			Win = winStack.Pop();
		}

		public static void Reset(IWin w)
		{
			winStack = new Stack<IWin>();
			Push(w);
		}


		public static string NL => Environment.NewLine;

	#region public methods

		public static int ColumnWidth { get; set; } = 30;

		public static void Clr(IWin win)
		{
			(win ?? Win).MessageBox = "";
		}

		public static void MarginClr()
		{
			marginSize = 0;
		}

		public static void MarginUp()
		{
			marginSize += marginSpaceSize;
		}

		public static void MarginDn()
		{
			marginSize -= marginSpaceSize;

			if (marginSize < 0) marginSize = 0;
		}

		public static void WriteAligned(IWin winName, string msg1, 
			string msg2 = "", string loc = "", string spacer = " ")
		{
			writeMsg(winName, msg1, msg2, spacer);
		}

		public static void WriteLineAligned(IWin winName, string msg1, 
			string msg2 = "", string loc = "", string spacer = " ")
		{
			writeMsg(winName, msg1, msg2 + "\n", spacer);
		}

		public static void Write(IWin winName, string msg1, 
			string msg2 = "", string loc = "")
		{
			writeMsg(winName, msg1, msg2);
		}
		
		public static void WriteLine(IWin winName, string msg1, 
			string msg2 = "", string loc = "")
		{
			writeMsg(winName, msg1, msg2 + "\n");
		}

		public static void WriteLineDebug(IWin winName, string msgA, 
			string msgB, string msgD, string loc = "", int colWidth = -1)
		{
			writeMsg(winName, msgA, msgB, colWidth);
			Debug.WriteLine(fmtMsg(msgA, msgD));

		}

	#endregion

	#region private methods

		
		private static void sendMessage(IWin win, string msg)
		{
			if (win != null || Win!=null)
			{
				(win ?? Win).MessageBox += msg;
			}
			else
			{
				deferred += msg;
			}
		}


		private static string margin(string spacer)
		{
			if (marginSize == 0) return "";

			return spacer.Repeat(marginSize);
		}

		private static string fmtMsg(string msg1, string msg2, int colWidth = -1)
		{
			string partA = msg1.IsVoid() ? msg1 : msg1.PadRight(colWidth == -1 ? ColumnWidth : colWidth);
			string partB = msg2.IsVoid() ? msg2 : " " + msg2;

			return partA + partB;
		}

		private static void writeMsg(IWin winName, string msg1, string msg2, string spacer, int colWidth = -1)
		{
			sendMessage(winName, margin(spacer) + fmtMsg(msg1, msg2, colWidth));
		}

		private static void writeMsg(IWin winName, string msg1, string msg2, int colWidth = -1)
		{
			sendMessage(winName, fmtMsg(msg1, msg2, colWidth));
		}

	#endregion

	}


		public class Mx
	{
		private int marginSize = 0;
		private int marginSpaceSize = 2;

		public  IWin Win { get; set; }

		public string NL => Environment.NewLine;

	#region public methods

		public int ColumnWidth { get; set; } = 30;


		public void Clr()
		{
			Win.MessageBox = "";
		}

		public void MarginClr()
		{
			marginSize = 0;
		}

		public void MarginUp()
		{
			marginSize += marginSpaceSize;
		}

		public void MarginDn()
		{
			marginSize -= marginSpaceSize;

			if (marginSize < 0) marginSize = 0;
		}

		public void WriteAligned(string msg1, string msg2 = "", string loc = "", string spacer = " ")
		{
			writeMsg(msg1, msg2, spacer);
		}

		public void WriteLineAligned(string msg1, string msg2 = "", string loc = "", string spacer = " ")
		{
			writeMsg(msg1, msg2 + "\n", spacer);
		}

		public void Write(string msg1, string msg2 = "", string loc = "")
		{
			writeMsg(msg1, msg2);
		}
		
		public void WriteLine(string msg1, string msg2 = "", string loc = "")
		{
			writeMsg(msg1, msg2 + "\n");
		}

		// public void ShowMsg()
		// {
		// 	OnPropertyChanged("MessageBoxText");
		// }

		public void WriteLineDebug(string msgA, string msgB, string msgD, string loc = "", int colWidth = -1)
		{
			writeMsg(msgA, msgB, colWidth);
			Debug.WriteLine(fmtMsg(msgA, msgD));

		}

	#endregion

	#region private methods


		private string margin(string spacer)
		{
			if (marginSize == 0) return "";

			return spacer.Repeat(marginSize);
		}

		private string fmtMsg(string msg1, string msg2, int colWidth = -1)
		{
			string partA = msg1.IsVoid() ? msg1 : msg1.PadRight(colWidth == -1 ? ColumnWidth : colWidth);
			string partB = msg2.IsVoid() ? msg2 : " " + msg2;

			return partA + partB;
		}

		private void writeMsg(    string msg1, string msg2, string spacer, int colWidth = -1)
		{
			Win.MessageBox += margin(spacer) + fmtMsg(msg1, msg2, colWidth);
		}

		private void writeMsg(   string msg1, string msg2, int colWidth = -1)
		{
			Win.MessageBox += fmtMsg(msg1, msg2, colWidth);
		}

	#endregion

	}
}
