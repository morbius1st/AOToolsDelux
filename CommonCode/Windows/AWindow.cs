﻿#region + Using Directives

using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using UtilityLibrary;

#endregion

// user name: jeffs
// created:   8/29/2021 5:20:59 PM

namespace CommonCode.Windows
{
	public abstract class AWindow : Window
	{
		public string textMsg01 { get; set; }
		
		private int marginSize = 0;
		private int marginSpaceSize = 2;
		private string location;

		public AWindow() {}

	#region public methods

		public int ColumnWidth { get; set; } = 30;

		public string MessageBoxText
		{
			get => textMsg01;
			set
			{
				textMsg01 = value;
				OnPropertyChanged();
			}
		}

		public void MsgClr()
		{
			textMsg01 = "";
			ShowMsg();
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
			writeMsg(msg1, msg2, loc, spacer);
		}

		public void WriteLineAligned(string msg1, string msg2 = "", string loc = "", string spacer = " ")
		{
			writeMsg(msg1, msg2, loc, spacer);
			WriteNewLine();
		}

		public void WriteMsg(string msg1, string msg2 = "", string loc = "")
		{
			writeMsg(msg1, msg2, loc);

		}
		
		public void WriteLine(string msg1, string msg2 = "", string loc = "")
		{
			writeMsg(msg1, msg2, loc);
			WriteNewLine();
		}

		public void WriteLineDebugMsg(string msgA, string msgB, string msgD, string loc = "", int colWidth = -1)
		{

			writeMsg(msgA, msgB, loc, colWidth);
			WriteNewLine();
			Debug.WriteLine(fmtMsg(msgA, msgD));

		}

		public void WriteNewLine()
		{
			textMsg01 += "\n";
		}


		public void ShowMsg()
		{
			OnPropertyChanged("MessageBoxText");
		}

	#endregion

	#region private methods

		public string margin(string spacer)
		{
			if (marginSize == 0) return "";

			return spacer.Repeat(marginSize);
		}

		public string fmtMsg(string msg1, string msg2, int colWidth = -1)
		{
			string partA = msg1.IsVoid() ? msg1 : msg1.PadRight(colWidth == -1 ? ColumnWidth : colWidth);
			string partB = msg2.IsVoid() ? msg2 : " " + msg2;

			return partA + partB;
		}

		public void writeMsg(string msg1, string msg2, string loc, string spacer, int colWidth = -1)
		{
			location = loc;

			textMsg01 += margin(spacer) + fmtMsg(msg1, msg2, colWidth);
		}

		public void writeMsg(string msg1, string msg2, string loc, int colWidth = -1)
		{
			location = loc;

			textMsg01 += fmtMsg(msg1, msg2, colWidth);
		}

	#endregion

		protected abstract void OnPropertyChanged([CallerMemberName] string memberName = "");
	}
}