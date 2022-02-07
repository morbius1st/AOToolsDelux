#region + Using Directives

using System.Collections.Generic;
using System.Windows;
using SharedCode.Fields.ExStorage.ExStorManagement;
using SharedCode.Fields.Testing;
using SharedCode.ShowInformation;
using UtilityLibrary;
using FieldsStartProcedure = CSToolsStudies.FieldsManagement.FieldsStartProcedure;
using SharedCode.Windows;

// using CSToolsDelux.Fields.FieldsManagement;
// using CSToolsDelux.Fields.Testing;

#endregion

// user name: jeffs
// created:   9/18/2021 10:35:20 AM

namespace CSToolsStudies.Testing
{
	public class Tests01
	{
		private FieldsStartProcedure fs;
		private ShShowInfo show;
		private AWindow W;

		private Window wx;

		public Tests01(AWindow w)
		{
			W = w;
			SampleData.W = w;
			fs = new FieldsStartProcedure(w);
			show = new ShShowInfo(w, CsUtilities.AssemblyName, "CSToolsStudies");
		}

		// get data / show data
		// proc00
		public ExStoreRtnCodes proc00()
		{
			string procName = "proc00";
			int op = SampleData.p00;

			ExStoreRtnCodes result = ExStoreRtnCodes.XRC_GOOD;

			for (int i = 0; i < SampleData.tests; i++)
			{
				SampleData.TestIdx = i;

				if (i + 1 < SampleData.firstTest)
				{
					show.informStart(op, $"skipping test| {SampleData.TestNames[i]}", "");
					continue;
				}

				show.informStart(SampleData.xxx, "", "");
				show.informStartEnter(op, $"entering start| {SampleData.TestNames[i]}");

				result = fs.DoesDataStoreExist();

				show.informStartExit(op, "start complete", result.ToString());


				show.informStart(SampleData.xxx, "", "");

				W.ShowMsg();
			}

			return result;
		}

		public void TestJustifyEllipsisString()
		{
			string[] sample = new []
			{
				//0        1
				//1234567890123
				"  now is the time  longword  a verylongwordword  all good men  ",
				"  this is a   short   line of text ", // 3 lines
				"  this    is title ",                 // 2 lines
				"today Satuday",                       // one line (widht == 13
				"today Satday",                        // one line (widht == 12
				"today Saday",                         // one line (widht == 11
				"  today   is ",                       // one line (widht == 13
				"  today  is ",                        // one line (widht == 12
				"  today is ",                         // one line (widht == 11
				" abc ",
				"ab",
				"a",
				null
			};

			int iMax = 2; // inner most loop == ellipsis
			int jMax = 4; // middle loop == justify
			int kMax = 3; // outter loop == trim
			int lMax = 2;


			string result1;
			string result2;

			string iMsg = "";
			string jMsg = "";
			string kMsg = "";
			string lMsg = "";


			int colWidth = 12;
			bool? trim = null;
			bool doEllipsis = false;
			ColData.JustifyHoriz justifyHoriz = ColData.JustifyHoriz.UNSPECIFIED;

			int maxLines = 0;
			bool truncate = false;

			W.WriteLine2("----------", null, "--------------");
			W.WriteLine1("JustifyEllipsis string tests");

			// outter loop = trim
			for (int k = 0; k < kMax; k++)
			{
				if (k == 0)
				{
					trim = false;
					kMsg = "do not trim text";
				}
				else if (k == 1)
				{
					trim = true;
					kMsg = "trim text";
				}
				else
				{
					break;
				}

				W.WriteLine1(k == 0 ? $"----- start k loop ({kMsg}) ----" : $"----- next k loop ({kMsg}) ----");
				W.WriteNewLine();
				W.ShowMsg();

				// middle loop == justify
				for (int j = 0; j < jMax; j++)
				{
					if (j == 0)
					{
						jMsg = "left justify";
						justifyHoriz = ColData.JustifyHoriz.LEFT;
					}
					else if (j == 1)
					{
						jMsg = "right justify";
						justifyHoriz = ColData.JustifyHoriz.RIGHT;
					}
					else if (j == 2)
					{
						jMsg = "center justify";
						justifyHoriz = ColData.JustifyHoriz.CENTER;
					}
					else if (j == 3)
					{
						jMsg = "unset / ignore justify";
						justifyHoriz = ColData.JustifyHoriz.UNSPECIFIED;
					}

					W.WriteLine1(j == 0 ? $"----- start j loop ({jMsg}) ----" : $"----- next j loop ({jMsg}) ----");
					W.WriteNewLine();
					W.ShowMsg();

					// inner most loop == ellipsis
					for (int i = 0; i < iMax; i++)
					{
						if (i == 0)
						{
							doEllipsis = false;
							iMsg = "do not ellipsisify";
						}
						else if (i == 1)
						{
							doEllipsis = true;
							iMsg = "ellipsisify";
						}

						W.WriteLine2("loop #| ", null, $"i:({i}) j:({j}) k:({k})");
						W.WriteLine1(i == 0 ? $"----- start i loop ({iMsg}) ----" : $"----- next i loop ({iMsg}) ----");
						W.WriteNewLine();

						W.WriteLine2("    k message| ", null, kMsg);
						W.WriteLine2("    j message| ", null, jMsg);
						W.WriteLine2("    i message| ", null, iMsg);
						W.WriteLine2("max col width| ", null, colWidth);
						W.WriteLine2("   doEllipsis| ", null, doEllipsis);
						W.WriteLine2("      justify| ", null, justifyHoriz);
						W.WriteLine2("         trim| ", null, trim, whenNull1: "", whenNull2: "always trim");
						W.WriteNewLine();

						foreach (string s in sample)
						{
							W.WriteLine2($"string| len ({s?.Length ?? -1})", null, $">{s}<");

							// do (2) methods:
							// trim - ellipsisify - justify
							// - or -
							// ellipsisify - trim - justify


							// final answer is this: T-E-J
							result1 = AWindow.TejString(s, justifyHoriz, colWidth, doEllipsis, trim);
							// result1 = AWindow.TrimString(s, justify, trim);
							// result1 = doEllipsis ? AWindow.ellipsisify(result1, justify, colWidth) : result1;
							// result1 = AWindow.justifyString2(result1, justify, colWidth);

							W.WriteLine2("T-E-J| >" + result1 + "<", null, $" len| {result1.Length.ToString()}");

							/*
							result2 = doEllipsis ? AWindow.ellipsisify(s, justify, colWidth) : s;
							result2 = AWindow.TrimString(result2, justify, trim);
							result2 = AWindow.justifyString2(result2, justify, colWidth);

							W.WriteLine2("E-T-J| >" + result2 + "<", $" len| {result2.Length.ToString()}");
							*/

							W.WriteNewLine();
							W.WriteNewLine();
							W.ShowMsg();
						}
					} // end i for loop
				}     // end j loop
			}         // end k loop
		}

		public void TestColumnSplit()
		{
			string[] sample = new []
			{
				"now is the time  longword  a verylongwordword  all good men",
				// "this is a   short   line of text", // 3 lines
				// "this    is title", // 2 lines
				// "today is", // one line
			};

			bool loop = true;

			int iMax = 3;
			int jMax = 4;
			int kMax = 2;
			int lMax = 2;


			List<string> result;

			string iMsg = "";
			string jMsg = "";
			string kMsg = "";
			string lMsg = "";


			int colWidth = 12;
			int maxLines = 0;
			bool truncate = false;
			bool? trim = null;
			bool doEllipsis = false;
			ColData.JustifyHoriz justifyHoriz = ColData.JustifyHoriz.LEFT;


			W.WriteLine2("----------", null, "--------------");
			W.WriteLine1("divide string tests");

			// for (int k = 0; k < kMax; k++)
			// {
			// 	if (k == 0)
			// 	{
			// 		kMsg = "do not truncate to column width";
			// 		truncate = false;
			// 	}
			// 	else if (k == 1)
			// 	{
			// 		kMsg = "truncate to column width";
			// 		truncate = true;
			// 	}
			// 	else
			// 	{
			// 		break;
			// 	}
			//
			// 	W.WriteLine1(k == 0 ? $"----- start k loop ({kMsg}) ----" : $"----- next k loop ({kMsg}) ----");
			// 	W.WriteNewLine();
			// 	W.ShowMsg();

				for (int j = 0; j < jMax; j++)
				{
					if (j == 0)
					{
						jMsg = "left justify";
						justifyHoriz = ColData.JustifyHoriz.LEFT;
					}
					else if (j == 1)
					{
						jMsg = "right justify";
						justifyHoriz = ColData.JustifyHoriz.RIGHT;
					}
					else if (j == 2)
					{
						jMsg = "center justify";
						justifyHoriz = ColData.JustifyHoriz.CENTER;
					}
					else if (j == 3)
					{
						jMsg = "unset / ignore justify";
						justifyHoriz = ColData.JustifyHoriz.UNSPECIFIED;
					}

					W.WriteLine1(j == 0 ? $"----- start j loop ({jMsg}) ----" : $"----- next j loop ({jMsg}) ----");
					W.WriteNewLine();
					W.ShowMsg();

					for (int l = 0; l < lMax; l++)
					{
						if (l == 0)
						{
							doEllipsis = false;
							lMsg = "do not ellipsisify";
						}
						else
						{
							doEllipsis = true;
							lMsg = "ellipsisify";
						}

						W.WriteLine1(l == 0 ? $"----- start l loop ({lMsg}) ----" : $"----- next l loop ({lMsg}) ----");
						W.WriteNewLine();
						W.ShowMsg();

						for (int i = 0; i < iMax; i++)
						{
							if (i == 0)
							{
								trim = null;
								iMsg = "null trim| full trim";
							}
							else if (i == 1)
							{
								iMsg = "trim is true";
								trim = true;
							}
							else if (i == 2)
							{
								iMsg = "trim is false";
								trim = false;
							}
							
							W.WriteLine1(i == 0 ? $"----- start i loop ({iMsg}) ----" : $"----- next i loop ({iMsg}) ----");
							W.WriteNewLine();
							W.ShowMsg();

							W.WriteLine2("loop #| ", null, $"i:({i}) j:({j})");

							W.WriteLine2("    k message| ", null, kMsg);
							W.WriteLine2("    j message| ", null, jMsg);
							W.WriteLine2("    i message| ", null, iMsg);
							W.WriteLine2("max col width| ", null, colWidth);
							W.WriteLine2("    max lines| ", null, maxLines);
							W.WriteLine2("         trim| ", null, trim, whenNull1: null, whenNull2: "always trim");
							W.WriteLine2("      justify| ", null, justifyHoriz.ToString());
							W.WriteLine2("   doEllipsis| ", null, doEllipsis.ToString());
							W.WriteNewLine();

							foreach (string s in sample)
							{
								result = AWindow.ColumnifyString(s, colWidth, colWidth, maxLines, justifyHoriz,ColData.JustifyVertical.UNSPECIFIED ,doEllipsis, trim);

								if (result != null && result.Count > 0)
								{
									W.WriteLine2($"columnify string| ({s.Length})", null, $">{s}<");

									foreach (string s1 in result)
									{
										if (s1 == null) continue;
										W.WriteLine2(">" + s1 + "<", null, $"len| {s1.Length.ToString()}");
									}
								}
								else
								{
									W.WriteLine2("dvide failed| ", null,
										result == null ? "is null" : $"lines| {result.Count}");
								}

								W.WriteNewLine();
								W.WriteNewLine();
								W.ShowMsg();
							}
						} // end i for loop
					}
				} // end j loop
			// } // end k loop
		}


/*
		public void TestSplitString4()
		{
			string[] sample = new []
			{
				"now is the time  longword  a verylongwordword  all good men",
				// "this is a   short   line of text", // 3 lines
				// "this    is title", // 2 lines
				// "today is", // one line
			};

			bool loop = true;

			int iMax = 3;
			int jMax = 4;
			int kMax = 2;
			int lMax = 2;


			string[] result;

			string iMsg = "";
			string jMsg = "";
			string kMsg = "";
			string lMsg = "";


			int colWidth = 12;
			int maxLines = 0;
			bool truncate = false;
			bool? trim = null;
			bool doEllipsis = false;
			ColData.Justify justify = ColData.Justify.LEFT;


			W.WriteLine2("----------", "--------------");
			W.WriteLine1("divide string tests");

			for (int k = 0; k < kMax; k++)
			{
				if (k == 0)
				{
					kMsg = "do not truncate to column width";
					truncate = false;
				}
				else if (k == 1)
				{
					kMsg = "truncate to column width";
					truncate = true;
				}
				else
				{
					break;
				}

				W.WriteLine1(k == 0 ? $"----- start k loop ({kMsg}) ----" : $"----- next k loop ({kMsg}) ----");
				W.WriteNewLine();
				W.ShowMsg();

				for (int j = 0; j < jMax; j++)
				{
					if (j == 0)
					{
						jMsg = "left justify";
						justify = ColData.Justify.LEFT;
					}
					else if (j == 1)
					{
						jMsg = "right justify";
						justify = ColData.Justify.RIGHT;
					}
					else if (j == 2)
					{
						jMsg = "center justify";
						justify = ColData.Justify.CENTER;
					}
					else if (j == 3)
					{
						jMsg = "unset / ignore justify";
						justify = ColData.Justify.UNSPECIFIED;
					}

					W.WriteLine1(j == 0 ? $"----- start j loop ({jMsg}) ----" : $"----- next j loop ({jMsg}) ----");
					W.WriteNewLine();
					W.ShowMsg();

					for (int l = 0; l < lMax; l++)
					{
						if (l == 0)
						{
							doEllipsis = false;
							lMsg = "do not ellipsisify";
						}
						else
						{
							doEllipsis = true;
							lMsg = "ellipsisify";
						}

						W.WriteLine1(l == 0 ? $"----- start l loop ({lMsg}) ----" : $"----- next l loop ({lMsg}) ----");
						W.WriteNewLine();
						W.ShowMsg();

						for (int i = 0; i < iMax; i++)
						{
							if (i == 0)
							{
								trim = null;
								iMsg = "null trim| full trim";
							}
							else if (i == 1)
							{
								iMsg = "trim is true";
								trim = true;
							}
							else if (i == 2)
							{
								iMsg = "trim is false";
								trim = false;
							}

							W.WriteLine2("loop #| ", $"i:({i}) j:({j}) k:({k})");

							W.WriteLine2("    k message| ", kMsg);
							W.WriteLine2("    j message| ", jMsg);
							W.WriteLine2("    i message| ", iMsg);
							W.WriteLine2("max col width| ", colWidth);
							W.WriteLine2("    max lines| ", maxLines);
							W.WriteLine2("     truncate| ", truncate.ToString());
							W.WriteLine2("         trim| ", trim, null, "always trim");
							W.WriteLine2("      justify| ", justify.ToString());
							W.WriteLine2("   doEllipsis| ", doEllipsis.ToString());
							W.WriteNewLine();

							foreach (string s in sample)
							{
								result = AWindow.stringDivide4(s, new [] { ' ' }, colWidth, maxLines, justify, truncate, trim, doEllipsis);

								if (result != null && result.Length > 0)
								{
									W.WriteLine2("divide string| ", s);

									foreach (string s1 in result)
									{
										if (s1 == null) continue;
										W.WriteLine2(">" + s1 + "<", $"len| {s1.Length.ToString()}");
									}
								}
								else
								{
									W.WriteLine2("dvide failed| ",
										result == null ? "is null" : $"lines| {result.Length}");
								}

								W.WriteNewLine();
								W.WriteNewLine();
								W.ShowMsg();
							}
						} // end i for loop
					}
				} // end j loop
			}     // end k loop
		}

		public void TestSplitString5()
		{
			string[] sample = new []
			{
				//0        1
				//1234567890123
				"  now is the time  longword  a verylongwordword  all good men  ",
				"  this is a   short   line of text ", // 3 lines
				"  this    is title ",                 // 2 lines
				"today Satuday",                       // one line (widht == 13
				"today Satday",                        // one line (widht == 12
				"today Saday",                         // one line (widht == 11
				"  today   is ",                       // one line (widht == 13
				"  today  is ",                        // one line (widht == 12
				"  today is ",                         // one line (widht == 11
				" abc ",
				"ab",
				"a",
				null
			};

			bool loop = true;

			int jMax = 4;
			int kMax = 2;


			string[] result;

			string jMsg = "";
			string kMsg = "";

			int lMax = 4;
			string lMsg = "";

			int iMax = 3;
			string iMsg = "";


			int maxStringLength = 12;
			int maxLines = 0;
			ColData.Justify justify = ColData.Justify.LEFT;


			// bool truncate = false;
			// bool? trim = null;
			// bool doEllipsis = false;


			W.WriteLine2("----------", "--------------");
			W.WriteLine1("divide string tests");

			// for (int k = 0; k < kMax; k++)
			// {
			// 	if (k == 0)
			// 	{
			// 		kMsg = "do not truncate to column width";
			// 		truncate = false;
			// 	}
			// 	else if (k == 1)
			// 	{
			// 		kMsg = "truncate to column width";
			// 		truncate = true;
			// 	}
			// 	else
			// 	{
			// 		break;
			// 	}
			//
			// 	W.WriteLine1(k == 0 ? $"----- start k loop ({kMsg}) ----" : $"----- next k loop ({kMsg}) ----");
			// 	W.WriteNewLine();
			// 	W.ShowMsg();

			// for (int j = 0; j < jMax; j++)
			// {
			// 	if (j == 0)
			// 	{
			// 		jMsg = "left justify";
			// 		justify = ColData.Justify.LEFT;
			// 	}
			// 	else if (j == 1)
			// 	{
			// 		jMsg = "right justify";
			// 		justify = ColData.Justify.RIGHT;
			// 	}
			// 	else if (j == 2)
			// 	{
			// 		jMsg = "center justify";
			// 		justify = ColData.Justify.CENTER;
			// 	}
			// 	else if (j == 3)
			// 	{
			// 		jMsg = "unset / ignore justify";
			// 		justify = ColData.Justify.UNSPECIFIED;
			// 	}
			//
			// 	W.WriteLine1(j == 0 ? $"----- start j loop ({jMsg}) ----" : $"----- next j loop ({jMsg}) ----");
			// 	W.WriteNewLine();
			// 	W.ShowMsg();

			for (int l = 0; l < lMax; l++)
			{
				if (l == 0)
				{
					maxStringLength = 12;
					lMsg = "max string len = 12 (@ word split";
				}
				else if (l == 1)
				{
					maxStringLength = -12;
					lMsg = "max string len = -12 (@ character split";
				}
				else if (l == 2)
				{
					maxStringLength = 3;
					lMsg = "max string len = 3 (@ character split";
				}
				else if (l == 3)
				{
					maxStringLength = -3;
					lMsg = "max string len = -3 (@ character split";
				}

				W.WriteLine1(l == 0 ? $"----- start l loop ({lMsg}) ----" : $"----- next l loop ({lMsg}) ----");
				W.WriteNewLine();
				W.ShowMsg();

				for (int i = 0; i < iMax; i++)
				{
					if (i == 0)
					{
						maxLines = 0;
						iMsg = "maxLines = 0";
					}
					else if (i == 1)
					{
						maxLines = 3;
						iMsg = "maxLines = 3";
					}
					else if (i == 2)
					{
						maxLines = 10;
						iMsg = "maxLines = 10";
					}


					W.WriteLine1(i == 0 ? $"----- start i loop ({iMsg}) ----" : $"----- next i loop ({iMsg}) ----");
					W.WriteNewLine();
					W.ShowMsg();

					// W.WriteLine2("    k message| ", kMsg);
					// W.WriteLine2("    j message| ", jMsg);
					W.WriteLine2("    l message| ", lMsg);
					W.WriteLine2("    i message| ", iMsg);
					W.WriteLine2("max col width| ", maxStringLength);
					W.WriteLine2("    max lines| ", maxLines);

					// W.WriteLine2("      justify| ", justify.ToString());
					// W.WriteLine2("     truncate| ", truncate.ToString());
					// W.WriteLine2("         trim| ", trim, null, "always trim");
					// W.WriteLine2("   doEllipsis| ", doEllipsis.ToString());
					W.WriteNewLine();

					foreach (string s in sample)
					{
						result = AWindow.StringDivide(s, new [] { ' ' }, maxStringLength, maxLines);

						if (result != null && result.Length > 0)
						{
							W.WriteLine2($"string| ({s?.Length ?? -1})", $">{s ?? "is null"}<");

							foreach (string s1 in result)
							{
								if (s1 == null) continue;
								W.WriteLine2(">" + s1 + "<", $"len| {s1.Length.ToString()}");
							}
						}
						else
						{
							W.WriteLine2("dvide failed| ",
								result == null ? "is null" : $"lines| {result.Length}");
						}

						W.WriteNewLine();
						W.WriteNewLine();
						W.ShowMsg();
					}
				} // end i for loop
			} // end l loop
			// } // end j loop
			// } // end k loop
		}
*/



	}
}