// Solution:     AOToolsDelux
// Project:       DeluxMeasure
// File:             ColData.cs
// Created:      2022-01-29 (6:32 PM)

using System;
using System.Collections.Generic;

namespace DeluxMeasure.Windows
{
	public class ColData
	{
		public enum JustifyHoriz
		{
			UNSPECIFIED,
			LEFT,
			CENTER,
			RIGHT
		}

		public enum JustifyVertical
		{
			UNSPECIFIED,
			TOP,
			MIDDLE,
			BOTTOM
		}



		public int ColWidth { get; set; }
		public int TitleWidth { get; set; }

		public JustifyHoriz[] Just { get; set; }  = new JustifyHoriz[2]; // 0 == values, 1 == header
		// public Justify HeaderJustify { get; set; }
		// public Justify ValueJustify { get; set; }

		public string[] Text { get; set; } = new string[3];

		public ColData(int colWidth, int titleWidth, JustifyHoriz hj, JustifyHoriz vj)
		{
			ColWidth = colWidth;
			TitleWidth = titleWidth;
			Just[0] = hj;
			Just[1] = vj;
		}


		public static Dictionary<TE, string[]> Vz<TE>(params Tuple<TE, string, string, string>[] p)
		{
			Dictionary<TE, string[]> vz = null;

			if (p.Length > 0)
			{
				vz = new Dictionary<TE, string[]>();

				for (int i = 0; i < p.Length; i++)
				{
					vz.Add(p[i].Item1, new [] { p[i].Item2, p[i].Item3, p[i].Item4 });
				}
			}

			return vz;
		}


		public static Dictionary<TE, ColData>
			Mz<TE>(params Tuple<TE, int, int, JustifyHoriz, JustifyHoriz>[] p)  where TE : System.Enum
		{
			Dictionary<TE, ColData> cd = new Dictionary<TE, ColData>(5);

			if (p.Length > 0)
			{
				for (int i = 0; i < p.Length; i++)
				{
					cd.Add(p[i].Item1, new ColData(
						p[i].Item2,		// column width
						p[i].Item3,		// title width
						p[i].Item4,     // header justify
						p[i].Item5      // value justify
						));
				}
			}

			return cd;
		}
	}
}