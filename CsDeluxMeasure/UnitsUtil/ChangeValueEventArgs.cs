// Solution:     AOToolsDelux
// Project:       CsDeluxMeasure
// File:             ChangeValueEventArgs.cs
// Created:      2023-08-27 (9:35 PM)

using System;
using System.ComponentModel;

namespace CsDeluxMeasure.UnitsUtil
{
	public class ChangeValueEventArgs<TE> : CancelEventArgs where TE : Enum
	{
		public string Proposed { get; }
		public TE Response { get; set; }

		public ChangeValueEventArgs(string proposed, TE def)
		{
			Cancel = false;
			Proposed = proposed;
			Response = def;
		}
	}
}