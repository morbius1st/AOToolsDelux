#region + Using Directives

using System.Runtime.CompilerServices;
using System.Windows;
using UtilityLibrary;

#endregion

// user name: jeffs
// created:   8/29/2021 5:20:59 PM

namespace SharedCode.Windows
{
	public abstract class AWindow : Window
	{
		private string textMsg01;
		
		private int marginSize = 0;
		private int marginSpaceSize = 2;
		private string location;

		public AWindow() {}

	#region public methods

		public int ColumnWidth { get; set; } = 24;

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

		public void WriteMsg(string msg1, string msg2 = "", string loc = "", string spacer = " ")
		{
			writeMsg(msg1, msg2, loc, spacer);
		}

		public void WriteLineMsg(string msg1, string msg2 = "", string loc = "", string spacer = " ")
		{
			writeMsg(msg1, msg2 + "\n", loc, spacer);
		}

		public void ShowMsg()
		{
			OnPropertyChanged("MessageBoxText");
		}

	#endregion

	#region private methods

		private string margin(string spacer)
		{
			if (marginSize == 0) return "";

			return spacer.Repeat(marginSize);
		}

		private void writeMsg(string msg1, string msg2, string loc, string spacer)
		{
			location = loc;

			string partA = msg1.IsVoid() ? msg1 : msg1.PadRight(ColumnWidth);
			string partB = msg2.IsVoid() ? "" : " " + msg2;

			textMsg01 += margin(spacer) + partA + partB;
		}

	#endregion

		protected abstract void OnPropertyChanged([CallerMemberName] string memberName = "");
	}
}