// Solution:     AOToolsDelux
// Project:       CsDeluxMeasure
// File:             CheckBoxChangedEventArgs.cs
// Created:      2023-08-27 (9:35 PM)

using System.ComponentModel;

namespace CsDeluxMeasure.UnitsUtil
{
	public class CheckBoxChangedEventArgs : CancelEventArgs
	{
		public InList? WhichCheckBox { get; }
		public int InListOrder { get; set; }

		public CheckBoxChangedEventArgs(InList? whichCheckBox)
		{
			WhichCheckBox = whichCheckBox;
			InListOrder = -1;
		}
	}
}