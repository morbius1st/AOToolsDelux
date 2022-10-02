#region + Using Directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using UtilityLibrary;
// using static UtilityLibrary.MessageException;

#endregion

// user name: jeffs
// created:   8/29/2021 5:20:59 PM

namespace SharedCode.Windows
{
	public abstract class AWindow : Window
	{
		public string textMsg01 { get; set; }

		private int marginSize = 0;
		private int marginSpaceSize = 2;
		private string location;

		private static AWindow W;

		public AWindow()
		{
			W = this;
		}


		private const int IS_HDR = 0;
		private const int IS_ROW = 1;
		private const int IS_DIV = 2;

		private const int TBL_BDR_BEG = 0;
		private const int TBL_BDR_MID = 1;
		private const int TBL_BDR_END = 2;


		private string[][] tblBorder = new []
		{
			new [] { "> ", " < > ", " <" }, // header
			new [] { "| ", "  |  ", " |" }, // rows
			new [] { "+ ", "- + -", " +" }  // divider
		};

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

		public void MsgClr()
		{
			textMsg01 = "";
			ShowMsg();
		}


		public void WriteAligned(   string msg1, string divString = null, string msg2 = "", string spacer = " ")
		{
			writeMsg2(msg1, msg2, spacer, -1, null, null, divString);
		}

		public void WriteLineAligned(string msg1, string divString = null, string msg2 = "", string spacer = " ")
		{
			writeMsg2(msg1, msg2, spacer, -1, null, null, divString);
			WriteNewLine();
		}

		public void WriteMsg<T1, T2>( T1 msg1, string divString, T2 msg2,
			string whenNull1 = "", string whenNull2 = "",
			int colWidth = -1)
		{
			writeMsg2(msg1, msg2, colWidth, whenNull1, whenNull2, divString);
		}

		public void WriteLine2<T1, T2>( T1 msg1, string divString, T2 msg2,
			string whenNull1 = "", string whenNull2 = "",
			int colWidth = -1)
		{
			writeMsg2(msg1, msg2, colWidth, whenNull1, whenNull2, divString);
			WriteNewLine();
		}

		public void WriteLine1<T1>( T1 msg1,
			string whenNull1 = "",
			string divString = null,
			int colWidth = -1)
		{
			writeMsg1(msg1, colWidth, whenNull1, divString);
			WriteNewLine();
		}

		public void WriteLineDebugMsg(string msgA, string divString, string msgB, string msgD, int colWidth = -1)
		{
			writeMsg2(msgA, msgB, colWidth, null, null, divString);
			WriteNewLine();
			Debug.WriteLine(fmtMsg(msgA, msgD, null, null, divString));
		}

		public void WriteNewLine()
		{
			textMsg01 += "\n";
		}

		public void ShowMsg()
		{
			OnPropertyChanged("MessageBoxText");
		}


		public void WriteRowDivider<TE>(List<TE> order,
			Dictionary<TE, ColData> hdrData)
		{
			int width = tblBorder[IS_HDR][TBL_BDR_BEG].Length;
			int divWidth = tblBorder[IS_HDR][TBL_BDR_MID].Length;
			int i;

			for (i = 0; i < hdrData.Count - 1; i++)
			{
				width += hdrData[order[i]].ColWidth + divWidth;
			}

			width += hdrData[order[i]].ColWidth + tblBorder[IS_HDR][TBL_BDR_END].Length;

			WriteLine1("-".Repeat(width));

		}

		public void WriteRow<TE>( List<TE> order,
			Dictionary<TE, ColData> hdrData,
			Dictionary<TE, string> colInfo, int maxLines,
			ColData.JustifyVertical jv,
			bool isHeader, bool lastColAlign)
		{
			int rowType = isHeader ? IS_HDR : IS_ROW;

			StringBuilder[] sb = initTblRow(maxLines, tblBorder[rowType][TBL_BDR_BEG]);
			bool[] hasRow = new bool[maxLines];

			// ColData.JustifyVertical jv = ColData.JustifyVertical.MIDDLE;

			TE key;     //= order[0];
			ColData cd; //= hdrData[key];

			int titleWidth; //= isHeader ? cd.TitleWidth : cd.ColWidth;

			// break up the header text into individual lines of header text
			List<string> hdrTxt; //= ColumnifyString(colInfo[key], cd.ColWidth, titleWidth, maxLines, cd.Just[0], jv, true, true);

			int i;

			for (i = 0; i < order.Count - 1; i++)
			{
				key = order[i];
				cd = hdrData[key];
				titleWidth = isHeader ? cd.TitleWidth : cd.ColWidth;
				// break up the header text into individual lines of header text
				hdrTxt = ColumnifyString(colInfo[key], cd.ColWidth, titleWidth, maxLines, cd.Just[rowType], jv, true, true);

				appendInfo2(ref sb, hdrTxt, tblBorder[rowType][TBL_BDR_MID], ref hasRow);
			}

			key = order[i];
			cd = hdrData[key];
			titleWidth = isHeader ? cd.TitleWidth : lastColAlign ? cd.ColWidth : colInfo[key].Length;
			int colWidth = lastColAlign ? cd.ColWidth : colInfo[key].Length;

			// break up the header text into individual lines of header text
			hdrTxt = ColumnifyString(colInfo[key], colWidth, titleWidth, maxLines, cd.Just[rowType], jv, true, true);

			appendInfo2(ref sb, hdrTxt, tblBorder[rowType][TBL_BDR_END], ref hasRow);

			for (i =  maxLines - 1; i >= 0; i--)
			{
				if (hasRow[i])
				{
					writeMsg1(sb[i].ToString(), -1, "null?", null);
					WriteNewLine();
				}
			}
		}

		private void appendInfo2(ref StringBuilder[] sb, List<string> s, string colDiv, ref bool[] hasRow)
		{
			for (int i = 0; i < s.Count; i++)
			{
				hasRow[i] = hasRow[i] || !string.IsNullOrWhiteSpace(s[i]);

				sb[i].Append(s[i]).Append(colDiv);
			}
		}

		private StringBuilder[] initTblRow(int maxLines, string preface)
		{
			StringBuilder[] sb = new StringBuilder[maxLines];

			for (int i = 0; i < maxLines; i++)
			{
				sb[i] = new StringBuilder(preface);
			}

			return sb;
		}

		// format a text string into a column of text
		// colWidth: expected width of each row of text (with exceptions)
		// maxNumber of rows of text 
		// justify: how each row of text is justified
		// doEllipsis: do or do not ellipsisify rows longer than the maximum
		// trim: remove leading and / or trailing blank spaces;
		public static List<string> ColumnifyString(string s,
			int colWidth,
			int titleWidth,
			int maxLines,
			ColData.JustifyHoriz justifyHoriz,
			ColData.JustifyVertical justifyVert,
			bool doEllpisis,
			bool? trim)
		{
			List<string> result = new List<string>();

			List<string> final = new List<string>();

			result = StringDivide(s, new [] { ' ' }, titleWidth, maxLines);

			int[] lines = calcLines(maxLines, result.Count, justifyVert);

			int i;

			for (i = 0; i < lines[0]; i++)
			{
				final.Add(JustifyString(null, ColData.JustifyHoriz.RIGHT, colWidth));
			}

			for (i = result.Count - 1; i >= 0 ; i--)
			{
				final.Add(TejString(result[i], justifyHoriz, colWidth, doEllpisis, trim));
			}

			for (i = 0; i < lines[1]; i++)
			{
				final.Add(JustifyString(null, ColData.JustifyHoriz.RIGHT, colWidth));
			}

			return final;
		}

		private static int[] calcLines(int maxLines, int resultLines, ColData.JustifyVertical jv)
		{
			int[] lines = new int[2]; // before, middle, after

			lines[0] = 0;
			lines[1] = 0;

			if (maxLines == resultLines) return lines;

			switch (jv)
			{
			case ColData.JustifyVertical.BOTTOM:
				{
					break;
				}
			case ColData.JustifyVertical.MIDDLE:
				{
					lines[0] = (int) ((maxLines - resultLines) / 2 - 0.1);
					break;
				}
			// covers unspecified and bottom
			default:
				{
					lines[0] = maxLines - resultLines;
					break;
				}
			}

			lines[1] = maxLines - resultLines - lines[0];
			return lines;
		}


		// divide a string into sub-strings of maxLength size and a maximum
		// of maxLines.  Last line has the overflow if any.
		// maxLength > 0 means split on Word boundaries
		// < 0 means split on character boundaries (exact maxLength)
		// when maxLength > 0 the returned line can exceed maxLength
		public static List<string> StringDivide(string text,
			char[] splitanyOf,
			int maxLength,
			int maxLines)
		{
			text = text ?? "";

			bool splitMidWord = false;

			if ( maxLength < 0)
			{
				splitMidWord = true;
				maxLength *= -1;
			}

			List<string> result = new List<string>();

			string final;

			// result.Add("");

			int index = 0;
			int loop = 0;

			while (text.Length > 0)
			{
				int splitIdx;

				if (maxLength + 1 <= text.Length)
				{
					splitIdx = text.Substring(0, maxLength - 1).LastIndexOfAny(splitanyOf) + 1;

					if (!splitMidWord)
					{
						if ((splitIdx == 0 || splitIdx == -1))
						{
							splitIdx = text.IndexOfAny(splitanyOf, maxLength);
						}
					}
				}
				else
				{
					splitIdx = text.Length - index;
				}

				if (splitIdx == -1 || splitIdx == 0)
				{
					splitIdx = maxLength;
				}

				if (loop + 1 == maxLines)
				{
					final = text;
					splitIdx = text.Length;
				}
				else
				{
					final = text.Substring(0, splitIdx);
				}

				result.Add(final);

				if (text.Length > splitIdx)
				{
					text = text.Substring(splitIdx);
				}
				else
				{
					text = string.Empty;
				}

				loop++;
			}

			return result;
		}


		// combines TrimString + Ellipsisify + JustifyString
		// and has them in the correct order
		public static string TejString(string s, ColData.JustifyHoriz justifyHoriz, int maxLength,
			bool doEllipsis, bool? trim)
		{
			string result = TrimString(s, justifyHoriz, trim);
			result = doEllipsis ? EllipsisifyString(result, justifyHoriz, maxLength) : result;
			result = JustifyString(result, justifyHoriz, maxLength);

			return result;
		}

		// trim & pad but no ellipsisify
		// trim == null : full trim
		// trim == true : trim per justify
		//		justify == right : trim right side
		//		justify == left : trim left side
		//		justify == center or undefined : trim bith sides
		// trim == false : do nothing
		public static string TrimString(string s, ColData.JustifyHoriz justifyHoriz, bool? trim)
		{
			string result = s.IsVoid() ? "" : s;

			if (trim.HasValue)
			{
				if (trim.Value)
				{
					switch (justifyHoriz)
					{
					case ColData.JustifyHoriz.RIGHT:
						{
							result = result.TrimEnd();
							break;
						}
					case ColData.JustifyHoriz.LEFT:
						{
							result = result.TrimStart();
							break;
						}
					default:
						{
							result = result.Trim();
							break;
						}
					}
				}
			}
			else
			{
				result = result.Trim();
			}

			return result;
		}

		// ellipsisify - do not trim
		public static string EllipsisifyString(string s, ColData.JustifyHoriz j, int maxLength)
		{
			string msg = s ?? "";
			int beg = 0;
			int end = 0;
			int len = msg.Length;

			if (maxLength >= len || maxLength < 2) return s;

			//                     L    C    R
			string[] e = new [] { "…", "…", "…" };

			switch (j)
			{
			case ColData.JustifyHoriz.RIGHT:
				{
					// beg = 0;
					// ellipsis in left
					end = len - (maxLength - e[2].Length);
					msg = e[2] + s.Substring(end);
					break;
				}
			case ColData.JustifyHoriz.LEFT:
				{
					// ellipsis on right
					beg = maxLength - e[0].Length;
					msg = s.Substring(0, beg) + e[0];
					break;
				}
			default:
				{
					// ellipsis in center
					beg = (int) (((maxLength - e[1].Length) / 2) + 0.5);
					end = len - (maxLength - (beg + e[1].Length));

					msg = s.Substring(0, beg) + e[1] + s.Substring(end);
					break;
				}
			}


			return msg;
		}

		// justify the string within the provided colWidth
		public static string JustifyString(string s, ColData.JustifyHoriz j, int maxLength)
		{
			if (maxLength == 0) return s;

			string msg = s.IsVoid() ? "" : s;

			switch (j)
			{
			case ColData.JustifyHoriz.RIGHT:
				{
					msg = msg.PadLeft(maxLength);
					break;
				}
			case ColData.JustifyHoriz.CENTER:
				{
					msg = msg.PadCenter(maxLength);
					break;
				}
			default:
				{
					msg = msg.PadRight(maxLength);
					break;
				}
			}


			return msg;
		}


/*

		
		// private void appendInfo(
		// 	ref StringBuilder[] sb, string[] s, ColData.JustifyHoriz j,
		// 	int width , string colDiv, ref bool[] hasRow, bool isHeader = false)
		// {
		// 	for (int i = s.GetLength(0) - 1; i >= 0; i--)
		// 	{
		// 		hasRow[i] = hasRow[i] || !string.IsNullOrWhiteSpace(s[i]);
		//
		// 		// sb[i].Append(justifyString(s[i], j, width));
		//
		// 		sb[i].Append(TejString(s[i], j, width, false, false));
		// 		sb[i].Append(colDiv);
		// 	}
		// }

		public void WriteColumnRow<TE>(List<TE> order,
			Dictionary<TE, ColData> hdrData,
			Dictionary<TE, string> colInfo, int maxLines)
		{
			StringBuilder[] sb = initTblRow(maxLines, tblBorder[IS_ROW][TBL_BDR_BEG]);
			bool[] hasRow = new bool[maxLines];

			ColData.JustifyVertical jv = ColData.JustifyVertical.TOP;

			TE key = order[0];
			ColData cd = hdrData[key];
			// break up the header text into individual lines of header text
			List<string> hdrTxt = ColumnifyString(colInfo[key], cd.ColWidth, cd.ColWidth, maxLines, cd.Just[1], jv, true, true);

			int i;

			for (i = 1; i < order.Count - 1; i++)
			{
				appendInfo2(ref sb, hdrTxt, tblBorder[IS_ROW][TBL_BDR_MID], ref hasRow);

				key = order[i];
				cd = hdrData[key];
				// break up the header text into individual lines of header text
				hdrTxt = ColumnifyString(colInfo[key], cd.ColWidth, cd.ColWidth, maxLines, cd.Just[1], jv, true, true);
			}

			key = order[i];
			cd = hdrData[key];
			// break up the header text into individual lines of header text
			hdrTxt = ColumnifyString(colInfo[key], cd.ColWidth, cd.ColWidth, maxLines, cd.Just[1], jv, true, true);

			appendInfo2(ref sb, hdrTxt, tblBorder[IS_ROW][TBL_BDR_END], ref hasRow);

			for (i =  maxLines - 1; i >= 0; i--)
			{
				if (hasRow[i])
				{
					writeMsg1(sb[i].ToString(), -1, "null?", null);
					WriteNewLine();
				}
			}
		}


		// divide a string into sub-strings of maxLength size and a maximum
		// of maxLines.  Last line has the overflow if any.
		// maxLength > 0 means split on Word boundaries
		// < 0 means split on character boundaries (exact maxLength)
		// when maxLength > 0 the returned line can exceed maxLength
		public static string[] StringDivide2(string text,
			char[] splitanyOf,
			int maxLength,
			int maxLines)
		{
			text = text ?? "";

			bool splitMidWord = false;

			if ( maxLength < 0)
			{
				splitMidWord = true;
				maxLength *= -1;
			}

			string[] result = new string[2 * (text.Length / maxLength) + 2];
			string final;

			result[0] = "";

			int index = 0;
			int loop = 0;

			while (text.Length > 0)
			{
				int splitIdx;

				if (maxLength + 1 <= text.Length)
				{
					splitIdx = text.Substring(0, maxLength - 1).LastIndexOfAny(splitanyOf) + 1;

					if (!splitMidWord)
					{
						if ((splitIdx == 0 || splitIdx == -1))
						{
							splitIdx = text.IndexOfAny(splitanyOf, maxLength);
						}
					}
				}
				else
				{
					splitIdx = text.Length - index;
				}

				if (splitIdx == -1 || splitIdx == 0)
				{
					splitIdx = maxLength;
				}

				if (loop + 1 == maxLines)
				{
					final = text;
					splitIdx = text.Length;
				}
				else
				{
					final = text.Substring(0, splitIdx);
				}

				result[loop] = final;

				if (text.Length > splitIdx)
				{
					text = text.Substring(splitIdx);
				}
				else
				{
					text = string.Empty;
				}

				loop++;

				result[loop] = null;
			}

			return result;
		}



		public void WriteColumns<TE>( List<TE> order,
			Dictionary<TE, ColData> hdrData,
			Dictionary<TE, string> colInfo,
			bool isHeader, string loc = "") where TE : Enum, new()
		{
			if (hdrData == null || hdrData.Count == 0 || order == null || order.Count < 0)
			{
				writeMsg1("Write columns format error\n", -1, null);
				return;
			}

			string[][] colBdr = new []
			{
				new [] { "| ", "  |  ", " |" }, // rows
				new [] { "> ", " < > ", " <" }, // header
				new [] { "+ ", "- + -", " +" }  // divider
			};

			int isHdrCode = isHeader ? 1 : 0; // == 1 if header;  == 0 if row

			StringBuilder[] sb = new StringBuilder[ColData.MaxHdrRows];

			for (int i = 0; i < ColData.MaxHdrRows; i++)
			{
				sb[i] = new StringBuilder(colBdr[isHdrCode][0]);
			}

			bool[] hasRow = new bool[ColData.MaxHdrRows];

			TE key = order[0];
			ColData cd = hdrData[key];
			string s = colInfo[key];

			for (int j = 1; j < order.Count; j++)
			{
				appendInfo(ref sb, s, cd.Just[isHdrCode], cd.Width, colBdr[isHdrCode][1], ref hasRow, isHeader);

				key = order[j];
				cd = hdrData[key];
				s = colInfo[key];
			}

			appendInfo(ref sb, s, cd.Just[isHdrCode], cd.Width * isHdrCode, colBdr[isHdrCode][2], ref hasRow, isHeader);

			for (int i = ColData.MaxHdrRows - 1; i >= 0; i--)
			{
				if (hasRow[i])
				{
					writeMsg1(sb[i].ToString(), -1, null);
					WriteNewLine();
				}
			}

			if (isHeader)
			{
				sb[0] = new StringBuilder(colBdr[2][0]);
				int j;
				for (j = 0; j < order.Count - 1; j++)
				{
					key = order[j];
					cd = hdrData[key];
					s = new [] { "-".Repeat(cd.Width), null, null };
					appendInfo(ref sb, s, cd.Just[isHdrCode], cd.Width, colBdr[2][1], ref hasRow, true);
				}

				key = order[j];
				cd = hdrData[key];
				s = new [] { "-".Repeat(cd.Width), null, null };

				appendInfo(ref sb, s, cd.Just[isHdrCode], cd.Width, colBdr[2][2], ref hasRow, true);

				writeMsg1(sb[0].ToString(), -1, null);
				WriteNewLine();
			}
		}

		public static string[] stringDivide4(string text,
			char[] splitanyOf,
			int maxStringLength,
			int maxLines,
			ColData.Justify justify = ColData.Justify.UNSPECIFIED,
			bool truncate = false,
			bool? trim = null,
			bool doEllipsis = false)
		{
			string[] result = new string[2 * (text.Length / maxStringLength) + 2];
			string final;

			result[0] = "";

			int index = 0;
			int loop = 0;

			while (text.Length > 0)
			{
				int splitIdx;

				if (maxStringLength + 1 <= text.Length)
				{
					splitIdx = text.Substring(0, maxStringLength - 1).LastIndexOfAny(splitanyOf) + 1;
					// splitIdx += justify == ColData.Justify.RIGHT ? 1 : 0;


					if ((splitIdx == 0 || splitIdx == -1) && !truncate)
					{
						splitIdx = text.IndexOfAny(splitanyOf, maxStringLength);
					}
				}
				else
				{
					splitIdx = text.Length - index;
				}

				if (splitIdx == -1 || splitIdx == 0)
				{
					splitIdx = maxStringLength;
				}


				if (loop + 1 == maxLines)
				{
					final = text;
					splitIdx = text.Length;
				}
				else
				{
					final = text.Substring(0, splitIdx);
				}





				// add result to collection & increment index
				// 3 opions for trim
				// false = don't trim
				// true = standard trim per justification
				// null = both ends
				if (trim.HasValue)
				{
					if (trim.Value)
					{
						switch (justify)
						{
						case ColData.Justify.RIGHT:
							{
								final = final.TrimEnd();
								break;
							}
						case ColData.Justify.LEFT:
							{
								final = final.TrimStart();
								break;
							}
						default:
							{
								final = final.Trim();
								break;
							}
						}
					}
				}
				else
				{
					final = final.Trim();
				}

				if (doEllipsis && final.Length > maxStringLength)
				{
					final = EllipsisifyString(final, justify, maxStringLength);
				}

				result[loop] = final;

				if (text.Length > splitIdx)
				{
					text = text.Substring(splitIdx);
				}
				else
				{
					text = string.Empty;
				}

				loop++;

				result[loop] = null;
			}

			return result;
		}



		public static string[] stringDivide3(string s, int maxWidth)
		{
			List<string> result = stringDivide2(s, maxWidth);

			string[] rows = new string[ColData.MaxHdrRows];

			if (result.Count <= ColData.MaxHdrRows)
			{
				for (int i = result.Count - 1; i >= 0 ; i--)
				{
					rows[i] = result[result.Count - i - 1];
				}
			}
			else
			{
				rows[2] = result[0];
				rows[1] = result[1];
				rows[0] = result[2];

				for (int i = 3; i < result.Count; i++)
				{
					rows[0] += " " + result[i];
				}
			}

			return rows;
		}

		public static List<string> stringDivide2(string data, int length)
		{
			List<string> result = new List<string>();

			int lastSpace = 0;
			int currentSpace = 0;
			int newLinePos = 0;

			for (int i = 0; i < data.Length; i++)
			{
				if (data.Length - newLinePos <= length)
				{
					result.Add(data.Substring(newLinePos, data.Length - newLinePos));
					break;
				}

				if (data[i] == ' ')
				{
					lastSpace = currentSpace;
					currentSpace = i;
					if (currentSpace - newLinePos > length)
					{
						result.Add(data.Substring(newLinePos, lastSpace - newLinePos));
						newLinePos = lastSpace + 1;
					}
				}
			}

			return result;
		}

		public static string[] stringDivide(string s, int colWidth)
		{
			string[] msg = new string[ColData.MaxHdrRows];
			msg[0] = "";
			msg[1] = "";
			msg[2] = "";

			int len = s.Length;

			if (len > colWidth * ColData.MaxHdrRows)
			{
				// divide with ellipsis

				msg[0] = "… " + s.Substring(len - (colWidth - 2));
				msg[2] = s.Substring(0, colWidth);
				msg[1] = s.Substring(colWidth + 1, colWidth);
			}
			else
			{
				// divide btw rows, no ellipsis

				msg[0] = s.Substring(len - colWidth);
				len -= colWidth;

				if (len > colWidth)
				{
					msg[1] = s.Substring(len - colWidth, colWidth);
					len -= colWidth;
					msg[2] = s.Substring(0, len);
				}
				else
				{
					msg[1] = s.Substring(0, len);
				}
			}

			return msg;
		}

		public static string justifyString(string s, ColData.Justify j, int colWidth)
		{
			if (colWidth == 0) return s;

			string msg = s.IsVoid() ? "" : s;

			if (msg.Length > colWidth && colWidth > 4)
			{
				msg = ellipsisify(s, j, colWidth);
			}

			switch (j)
			{
			case ColData.Justify.LEFT:
				{
					msg = msg.PadRight(colWidth);
					break;
				}
			case ColData.Justify.CENTER:
				{
					msg = msg.PadCenter(colWidth);
					break;
				}
			default:
				{
					msg = msg.PadLeft(colWidth);
					break;
				}
			}


			return msg;
		}
		*/

	#endregion

	#region private methods

		private string margin(string spacer)
		{
			if (marginSize == 0) return "";

			return spacer.Repeat(marginSize);
		}

		private void writeMsg2<T1, T2>(   T1 msg1, T2 msg2,
			string spacer, int colWidth, string whenNull1, string whenNull2, string divString)
		{
			textMsg01 += margin(spacer) + fmtMsg(msg1, msg2, whenNull1, whenNull2, divString, colWidth);
		}

		private void writeMsg2<T1, T2>(    T1 msg1, T2 msg2, int colWidth, string whenNull1, string whenNull2, string divString)
		{
			textMsg01 += fmtMsg(msg1, msg2, whenNull1, whenNull2, divString, colWidth);
		}

		private void writeMsg1<T1>(  T1 msg1, int colWidth, string whenNull1, string divString)
		{
			textMsg01 += fmtMsg(msg1, "", whenNull1, null, divString, colWidth);
		}

		private string fmtMsg<T1, T2> (    T1 msg1, T2 msg2, string whenNull1, string whenNull2, string divString, int colWidth = -1)
		{
			string A;
			string B;

			if (msg1 is int)
			{
				A = fmtInt(Convert.ToInt32(msg1));
			}
			else
			{
				A = msg1?.ToString();

				if (A == null)
				{
					A = whenNull1 ?? "";
				}
			}

			if (msg2 is int)
			{
				B = fmtInt(Convert.ToInt32(msg2));
			}
			else
			{
				B = msg2?.ToString();

				if (B == null)
				{
					B = whenNull2 ?? "";
				}
			}

			return A.PadRight(colWidth == -1 ? ColumnWidth : colWidth) + divString + B;
		}

		public string fmtInt(int i)
		{
			return $"{i,-4}";
		}

	#endregion

		protected abstract void OnPropertyChanged([CallerMemberName] string memberName = "");
	}
}