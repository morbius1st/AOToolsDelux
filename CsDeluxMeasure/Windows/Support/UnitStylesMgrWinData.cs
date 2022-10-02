#region + Using Directives

using System;
using System.Collections.Generic;
using System.Windows;

#endregion

// user name: jeffs
// created:   7/29/2022 7:08:21 PM

namespace CsDeluxMeasure.Windows.Support
{
	/// <summary>
	/// structure the provides the help information
	/// Help string is placed in 1 of 7 fields.
	/// the int determines which field the help string will be placed.
	/// this allows text to stack across a single lines (basically acts as a tab)
	/// the higher the number the further to the right the text is placed
	/// </summary>
	public struct HelpInfo
	{
		public Thickness Marg { get; private set; }
		public string[] HelpDesc { get; private set; }

		public HelpInfo(Tuple<int, string>[] info, Thickness marg = default(Thickness))
		{
			Marg = marg;

			HelpDesc = new [] { "", "", "", "", "" , "", "" };

			foreach (var tuple in info)
			{
				HelpDesc[tuple.Item1] = tuple.Item2;
			}
		}
	}


	public static class UnitStylesMgrWinData
	{
		private static readonly Thickness MARG_HDR1 = new Thickness(0, 0, 0, 2);
		private static readonly Thickness MARG_HDR2 = new Thickness(0, 2, 0, 2);
		private static readonly Thickness MARG_HDR3 = new Thickness(0, 3, 0, 2);
		private static readonly Thickness MARG_LST1 = new Thickness(6, 1, 0, 1);
		private static readonly Thickness MARG_LST2A = new Thickness(6, 1, 0, 0);
		private static readonly Thickness MARG_LST2B = new Thickness(6, 0, 0, 1);

		private static readonly Tuple<int, string>[] FIRST_CHAR =
		{
			new Tuple<int, string>(2, "●"),
			new Tuple<int, string>(3, "First character must be alphanumeric")
		};

		private static readonly Tuple<int, string>[] LAST_CHAR =
		{
			new Tuple<int, string>(2, "●"),
			new Tuple<int, string>(3, "Last character must be alphanumeric")
		};

		private static readonly Tuple<int, string>[] ANY_CHAR =
		{
			new Tuple<int, string>(2, "●"),
			new Tuple<int, string>(3, "Only alphanumeric, space, dash, and period may be used")
		};

		public const int POPUP_NEW_STYLE_NAME = 0;
		public const int POPUP_NEW_STYLE_DESC = 1;
		public const int POPUP_NEW_STYLE_LIMIT = 1;

		public const int POPUP_STYLE_NAME = 2;
		public const int POPUP_STYLE_DESC = 3;
		public const int POPUP_STYLE_SAMPLE = 4;

		public const int POPUP_COUNT = POPUP_STYLE_SAMPLE + 1;

		static UnitStylesMgrWinData()
		{
			configNameMsgs();
			configDescMsgs();
			configSampleMsgs();
			configRibbonFavsMsgs();
			configLeftListMsgs();
			configRightListMsgs();
			configNewNameMsgs();
			configNewDescMsgs();
			configPosIdxMsgs();
			configCtrlsLstMsgs();
			configCtrlsDlgMsgs();
			configSoCtrlsDlgMsgs();
			configSoCtrlsPosMsgs();
			configSoCtrlsLstMsgs();

			configSoMsgs();
		}

		public enum ValMsgNameId
		{
			VN_GOOD,
			VN_TOOSHORT,
			VN_BEG_ALPHANUM_REQD,
			VN_END_ALPHANUM_REQD,
			VN_DISALLOWED_CHARS,
			VN_MUST_BE_UNIQUE
		}

		public static int ValMsgNameIdCount { get; private set; }
		public static string NameHelpInfoTitle { get; private set; }
		public static string NameHelpInfoHeader { get; private set; }
		public static string[] NameErrMsgs { get; private set; }

		public static List<HelpInfo> NameHelpInfo { get; set; } = new List<HelpInfo>();

		private static void configNameMsgs()
		{
			ValMsgNameIdCount = Enum.GetNames(typeof(ValMsgNameId)).Length;

			NameErrMsgs = new string[ValMsgNameIdCount];

			NameErrMsgs[(int) ValMsgNameId.VN_GOOD] = "Name is OK";
			NameErrMsgs[(int) ValMsgNameId.VN_TOOSHORT] = "Name is too short";
			NameErrMsgs[(int) ValMsgNameId.VN_BEG_ALPHANUM_REQD] = "Not allowed First character";
			NameErrMsgs[(int) ValMsgNameId.VN_END_ALPHANUM_REQD] = "Not allowed last character";
			NameErrMsgs[(int) ValMsgNameId.VN_DISALLOWED_CHARS] = "Not allowed character used";
			NameErrMsgs[(int) ValMsgNameId.VN_MUST_BE_UNIQUE] = "Name is not unique";

			NameHelpInfoTitle = "Edit Style Name";
			NameHelpInfoHeader =
				"Style Name Information";

			NameHelpInfo.Add(new HelpInfo(
				new [] { new Tuple<int, string>(0, "The name identifies the style and must be unique.") }, MARG_HDR1));
			NameHelpInfo.Add(new HelpInfo(
				new [] { new Tuple<int, string>(1, "The name must follow these rules:") }, MARG_HDR2));
			NameHelpInfo.Add(new HelpInfo(
				new [] { new Tuple<int, string>(2, "●"), new Tuple<int, string>(3, "Style name must be at least 4 characters long") }, MARG_LST1));
			NameHelpInfo.Add(new HelpInfo(FIRST_CHAR, MARG_LST1));
			NameHelpInfo.Add(new HelpInfo(LAST_CHAR, MARG_LST1));
			NameHelpInfo.Add(new HelpInfo(ANY_CHAR, MARG_LST1));
			NameHelpInfo.Add(new HelpInfo(
				new [] { new Tuple<int, string>(1, "Note:"), new Tuple<int, string>(4, "For ribbon styles, keep the name short so that the ribbon button is narrow") }, MARG_HDR3));
		}

		public enum ValMsgDescId
		{
			VD_GOOD,
			VD_TOOSHORT,
			VD_BEG_ALPHANUM_REQD,
			VD_END_ALPHANUM_REQD,
			VD_DISALLOWED_CHARS
		}

		public static int ValMsgDescIdCount { get; private set; }
		public static string DescHelpInfoTitle { get; private set; }
		public static string DescHelpInfoHeader { get; private set; }
		public static string[] DescErrMsgs { get; private set; }

		public static List<HelpInfo> DescHelpInfo { get; set; } = new List<HelpInfo>();

		private static void configDescMsgs()
		{
			ValMsgDescIdCount = Enum.GetNames(typeof(ValMsgDescId)).Length;

			DescErrMsgs = new string[ValMsgDescIdCount];
			DescErrMsgs[(int) ValMsgDescId.VD_GOOD] =  "Description is OK";
			DescErrMsgs[(int) ValMsgDescId.VD_TOOSHORT] =  "Description is too short";
			DescErrMsgs[(int) ValMsgDescId.VD_BEG_ALPHANUM_REQD] =  "Not allowed First character";
			DescErrMsgs[(int) ValMsgDescId.VD_END_ALPHANUM_REQD] =  "Not allowed last character";
			DescErrMsgs[(int) ValMsgDescId.VD_DISALLOWED_CHARS] =  "Not allowed character used";

			DescHelpInfoTitle = "Edit Style Description";
			DescHelpInfoHeader =
				"Style description information";

			DescHelpInfo.Add(new HelpInfo(
				new [] { new Tuple<int, string>(0, "The description helps you remember the styles purpose") }, MARG_HDR1));
			DescHelpInfo.Add(new HelpInfo(
				new [] { new Tuple<int, string>(1, "The description must follow these rules:") }, MARG_HDR2));
			DescHelpInfo.Add(new HelpInfo(
				new [] { new Tuple<int, string>(2, "●"), new Tuple<int, string>(3, "Style description must be at least 6 characters long") }, MARG_LST1));
			DescHelpInfo.Add(new HelpInfo(FIRST_CHAR, MARG_LST1));
			DescHelpInfo.Add(new HelpInfo(LAST_CHAR, MARG_LST1));
			DescHelpInfo.Add(new HelpInfo(ANY_CHAR, MARG_LST1));
		}


		public static string SampleHelpInfoTitle { get; private set; }
		public static string SampleHelpInfoHeader { get; private set; }
		public static List<HelpInfo> SampleHelpInfo { get; set; } = new List<HelpInfo>();

		private static void configSampleMsgs()
		{
			SampleHelpInfoTitle = "Style Sample Information";
			SampleHelpInfoHeader = "The sample shows an actual usage of this style";

			SampleHelpInfo.Add(new HelpInfo(
				new [] { new Tuple<int, string>(0, "The sample is just a number but note the following when entering a number:") }, MARG_HDR1 ));
			SampleHelpInfo.Add(new HelpInfo(
				new [] { new Tuple<int, string>(2, "●"), new Tuple<int, string>(3, "You can enter a number without units. This is considered a length in the current units of the model") }, MARG_LST1 ));
			SampleHelpInfo.Add(new HelpInfo(
				new [] { new Tuple<int, string>(2, "●"), new Tuple<int, string>(3, "You can enter a number with the unit symbology for the style") }, MARG_LST1 ));
			SampleHelpInfo.Add(new HelpInfo(
				new [] { new Tuple<int, string>(2, "●"), new Tuple<int, string>(3, "You cannot use any other unit symbology") }, MARG_LST1));
			SampleHelpInfo.Add(new HelpInfo(
				new [] { new Tuple<int, string>(1, "Note:"), new Tuple<int, string>(4, "The number, within parentheses, is the length without units") }, MARG_HDR3));
		}


		public static string RibbonFavsHelpInfoTitle { get; private set; }
		public static string RibbonFavsHelpInfoHeader { get; private set; }
		public static List<HelpInfo> RibbonFavsHelpInfo { get; set; } = new List<HelpInfo>();

		public static List<HelpInfo> RibbonFavsHelpInfoFtDecIn { get; set; } = new List<HelpInfo>();

		private static void configRibbonFavsMsgs()
		{
			RibbonFavsHelpInfoTitle = "Ribbon Fav Checkbox";
			RibbonFavsHelpInfoHeader = "Set style for the ribbon favorites";

			RibbonFavsHelpInfo.Add(new HelpInfo(
				new [] { new Tuple<int, string>(0, "Checked off styles will be included as a ribbon favorite.") }, MARG_HDR1 ));
			RibbonFavsHelpInfo.Add(new HelpInfo(
				new [] { new Tuple<int, string>(1, "Suggestion:") }, MARG_HDR3));
			RibbonFavsHelpInfo.Add(new HelpInfo(
				new [] {new Tuple<int, string>(2, "Keep the number of ribbon favorites to around 4 or 5 maximum for ease of use") }, MARG_HDR3));


			RibbonFavsHelpInfoFtDecIn.Add(RibbonFavsHelpInfo[0]);
			RibbonFavsHelpInfoFtDecIn.Add(
				new HelpInfo(
				new []
				{
					new Tuple<int, string>(2, "●"), 
					new Tuple<int, string>(3, "This Unit Style is custom and cannot be include as a ribbon favorite")
				}, MARG_LST1 ));
			RibbonFavsHelpInfoFtDecIn.Add(RibbonFavsHelpInfo[1]);
			RibbonFavsHelpInfoFtDecIn.Add(RibbonFavsHelpInfo[2]);
		}


		public static string LeftListHelpInfoTitle { get; private set; }
		public static string LeftListHelpInfoHeader { get; private set; }
		public static List<HelpInfo> LeftListHelpInfo { get; set; } = new List<HelpInfo>();

		private static void configLeftListMsgs()
		{
			LeftListHelpInfoTitle = "Left Unit Style List Checkbox";
			LeftListHelpInfoHeader = "Set style for the left list in the Delux Measure app";

			LeftListHelpInfo.Add(new HelpInfo(
				new [] { new Tuple<int, string>(0, "Checked off styles will be included in the left unit style list in the Delux Measure app.") }, MARG_HDR1 ));
			LeftListHelpInfo.Add(new HelpInfo(
				new [] { new Tuple<int, string>(1, "Suggestion:")}, MARG_HDR3));

			LeftListHelpInfo.Add(new HelpInfo(
				new [] {new Tuple<int, string>(2, "Keep the number to less than 10 for ease of use") }, MARG_HDR3));
		}


		public static string RightListHelpInfoTitle { get; private set; }
		public static string RightListHelpInfoHeader { get; private set; }
		public static List<HelpInfo> RightListHelpInfo { get; set; } = new List<HelpInfo>();

		private static void configRightListMsgs()
		{
			RightListHelpInfoTitle = "Right Unit Style List Checkbox ";
			RightListHelpInfoHeader = "Set style for the right list in the Delux Measure app";

			RightListHelpInfo.Add(new HelpInfo(
				new [] { new Tuple<int, string>(0, "Checked off styles will be included in the right unit style list in the Delux Measure app.") }, MARG_HDR1 ));
			RightListHelpInfo.Add(new HelpInfo(
				new [] { new Tuple<int, string>(1, "Suggestion:")}, MARG_HDR3));

			RightListHelpInfo.Add(new HelpInfo(
				new [] {new Tuple<int, string>(2, "Keep the number to less than 10 for ease of use") }, MARG_HDR3));
		}

		public static string NewNameHelpInfoTitle { get; private set; }
		public static string NewNameHelpInfoHeader { get; private set; }
		public static List<HelpInfo> NewNameHelpInfo { get; set; } = new List<HelpInfo>();

		private static void configNewNameMsgs()
		{
			NewNameHelpInfoTitle = "Provide a Name for the New Style";
			NewNameHelpInfoHeader = "Style Name Information";

			NewNameHelpInfo.Add(new HelpInfo(
				new [] { new Tuple<int, string>(0, "The name identifies the style and must be unique.") }, MARG_HDR1));
			NewNameHelpInfo.Add(new HelpInfo(
				new [] { new Tuple<int, string>(1, "The name must follow these rules:") }, MARG_HDR2));
			NewNameHelpInfo.Add(new HelpInfo(
				new [] { new Tuple<int, string>(2, "●"), new Tuple<int, string>(3, "Style name must be at least 4 characters long") }, MARG_LST1));
			NewNameHelpInfo.Add(new HelpInfo(FIRST_CHAR, MARG_LST1));
			NewNameHelpInfo.Add(new HelpInfo(LAST_CHAR, MARG_LST1));
			NewNameHelpInfo.Add(new HelpInfo(ANY_CHAR, MARG_LST1));
		}

		public static string NewDescHelpInfoTitle { get; private set; }
		public static string NewDescHelpInfoHeader { get; private set; }
		public static List<HelpInfo> NewDescHelpInfo { get; set; } = new List<HelpInfo>();

		private static void configNewDescMsgs()
		{
			NewDescHelpInfoTitle = "Provide a Description for the New Style";
			NewDescHelpInfoHeader = "Style Description information";

			NewDescHelpInfo.Add(new HelpInfo(
				new [] { new Tuple<int, string>(0, "The description helps you remember the styles purpose") }, MARG_HDR1));
			NewDescHelpInfo.Add(new HelpInfo(
				new [] { new Tuple<int, string>(1, "The description must follow these rules:") }, MARG_HDR2));
			NewDescHelpInfo.Add(new HelpInfo(
				new [] { new Tuple<int, string>(2, "●"), new Tuple<int, string>(3, "Style description must be at least 6 characters long") }, MARG_LST1));
			NewDescHelpInfo.Add(new HelpInfo(FIRST_CHAR, MARG_LST1));
			NewDescHelpInfo.Add(new HelpInfo(LAST_CHAR, MARG_LST1));
			NewDescHelpInfo.Add(new HelpInfo(ANY_CHAR, MARG_LST1));
		}

		public static string PosIdxHelpInfoTitle { get; private set; }
		public static string PosIdxHelpInfoHeader { get; private set; }
		public static List<HelpInfo> PosIdxHelpInfo { get; set; } = new List<HelpInfo>();

		private static void configPosIdxMsgs()
		{
			PosIdxHelpInfoTitle = "Insertion Index";
			PosIdxHelpInfoHeader = "Provide the row number to determine where to insert the new style";

			PosIdxHelpInfo.Add(new HelpInfo(
				new [] { new Tuple<int, string>(2, "●"), new Tuple<int, string>(3, "The new style will be placed before this row") }, MARG_LST1));
			PosIdxHelpInfo.Add(new HelpInfo(
				new [] { new Tuple<int, string>(2, "●"), new Tuple<int, string>(3, "The new style cannot be placed before row 1") }, MARG_LST1));
			PosIdxHelpInfo.Add(new HelpInfo(
				new [] { new Tuple<int, string>(2, "●"), new Tuple<int, string>(3, "The insertion row cannot be greater than the number of styles") }, MARG_LST1));
			PosIdxHelpInfo.Add(new HelpInfo(
				new [] { new Tuple<int, string>(1, "Note:"), new Tuple<int, string>(4, "The \"last\" button adds the new style to the end of the list") }, MARG_HDR3));
		}

		public static string CtrlsLstHelpInfoTitle { get; private set; }
		public static string CtrlsLstHelpInfoHeader { get; private set; }
		public static List<HelpInfo> CtrlsLstHelpInfo { get; set; } = new List<HelpInfo>();

		private static void configCtrlsLstMsgs()
		{
			CtrlsLstHelpInfoTitle = "List Control Buttons";
			CtrlsLstHelpInfoHeader = "Modify the order or remove the styles in the list";

			CtrlsLstHelpInfo.Add(new HelpInfo(
				new [] { new Tuple<int, string>(0, "Use these controls to adjust the order or remove styles from the saved style list") }, MARG_HDR1 ));
			CtrlsLstHelpInfo.Add(new HelpInfo(
				new [] { new Tuple<int, string>(1, "Notes")}, MARG_HDR3));
			CtrlsLstHelpInfo.Add(new HelpInfo(
				new [] { new Tuple<int, string>(2, "●"), new Tuple<int, string>(3, "Style 1 is locked and cannot be moved or removed") }, MARG_LST1 ));
			CtrlsLstHelpInfo.Add(new HelpInfo(
				new [] { new Tuple<int, string>(2, "●"), new Tuple<int, string>(3, "The up arrow button ▲ moves the row up one position") }, MARG_LST1 ));
			CtrlsLstHelpInfo.Add(new HelpInfo(
				new [] { new Tuple<int, string>(2, "●"), new Tuple<int, string>(3, "The down arrow button ▼ moves the row down one position") }, MARG_LST1));
			CtrlsLstHelpInfo.Add(new HelpInfo(
				new [] { new Tuple<int, string>(2, "●"), new Tuple<int, string>(3, "The Trash can button permanently removes the style from the list") }, MARG_LST1));
		}


		public static string CtrlsDlgHelpInfoTitle { get; private set; }
		public static string CtrlsDlgHelpInfoHeader { get; private set; }
		public static List<HelpInfo> CtrlsDlgHelpInfo { get; set; } = new List<HelpInfo>();

		private static void configCtrlsDlgMsgs()
		{
			CtrlsDlgHelpInfoTitle = "Dialog Control Buttons";
			CtrlsDlgHelpInfoHeader = "Primary Controls for this Dialog";

			CtrlsDlgHelpInfo.Add(new HelpInfo(
				new [] { new Tuple<int, string>(0, "Dialog control buttons preform these operations") }, MARG_HDR1 ));
			CtrlsDlgHelpInfo.Add(new HelpInfo(
				new [] { new Tuple<int, string>(1, "Done"), new Tuple<int, string>(4, "Close the dialog - everything is completed (disabled when there are un-saved changes)") }, MARG_LST1 ));
			CtrlsDlgHelpInfo.Add(new HelpInfo(
				new [] { new Tuple<int, string>(1, "Apply"), new Tuple<int, string>(4, "Save the changes (disabled when there are no changes)") }, MARG_LST1 ));
			CtrlsDlgHelpInfo.Add(new HelpInfo(
				new [] { new Tuple<int, string>(1, "Cancel"), new Tuple<int, string>(4, "Close the dialog regardless of changes (changes are lost - a warning will display if there are changes)") }, MARG_LST1));
			CtrlsDlgHelpInfo.Add(new HelpInfo(
				new [] { new Tuple<int, string>(1, "Reset"), new Tuple<int, string>(4, "Reset all changes to the values when the Dialog box was opened regardless of changes (a warning will display if there are changes)") }, MARG_LST1));
			CtrlsDlgHelpInfo.Add(new HelpInfo(
				new [] { new Tuple<int, string>(1, "Adjust Style Order")}, MARG_LST1));
			CtrlsDlgHelpInfo.Add(new HelpInfo(
				new [] { new Tuple<int, string>(4, "(far right): Opens the Adjust Style Order dialog") }, MARG_LST1));
		}

		public static string SoCtrlsDlgHelpInfoTitle { get; private set; }
		public static string SoCtrlsDlgHelpInfoHeader { get; private set; }
		public static List<HelpInfo> SoCtrlsDlgHelpInfo { get; set; } = new List<HelpInfo>();

		private static void configSoCtrlsDlgMsgs()
		{
			SoCtrlsDlgHelpInfoTitle = "Dialog Control Buttons";
			SoCtrlsDlgHelpInfoHeader = "Primary Controls for this Dialog";

			SoCtrlsDlgHelpInfo.Add(new HelpInfo(
				new [] { new Tuple<int, string>(0, "Dialog control buttons preform these operations") }, MARG_HDR1 ));
			SoCtrlsDlgHelpInfo.Add(new HelpInfo(
				new [] { new Tuple<int, string>(1, "Done"), new Tuple<int, string>(4, "Close the dialog - everything is completed (disabled when there are un-saved changes)") }, MARG_LST1 ));
			SoCtrlsDlgHelpInfo.Add(new HelpInfo(
				new [] { new Tuple<int, string>(1, "Apply"), new Tuple<int, string>(4, "Save the changes, for all lists (disabled when there are no changes)") }, MARG_LST1 ));
			SoCtrlsDlgHelpInfo.Add(new HelpInfo(
				new [] { new Tuple<int, string>(1, "Cancel"), new Tuple<int, string>(4, "Close the dialog regardless of changes (changes are lost for all lists - a warning will display if there are changes)") }, MARG_LST1));
			SoCtrlsDlgHelpInfo.Add(new HelpInfo(
				new [] { new Tuple<int, string>(1, "Reset"), new Tuple<int, string>(4, "Reset the changes, for all lists, to the values when the Dialog box was opened (a warning will display if there are changes)") }, MARG_LST1));
		}

		
		public static string SoCtrlsPosHelpInfoTitle { get; private set; }
		public static string SoCtrlsPosHelpInfoHeader { get; private set; }
		public static List<HelpInfo> SoCtrlsPosHelpInfo { get; set; } = new List<HelpInfo>();

		private static void configSoCtrlsPosMsgs()
		{
			SoCtrlsPosHelpInfoTitle = "List Position Buttons";
			SoCtrlsPosHelpInfoHeader = "Modify the order of the saved styles in the list";

			SoCtrlsPosHelpInfo.Add(new HelpInfo(
				new [] { new Tuple<int, string>(0, "Use these controls to adjust the order of the saved style in the lists") }, MARG_HDR1 ));
			SoCtrlsPosHelpInfo.Add(new HelpInfo(
				new [] { new Tuple<int, string>(2, "●"), new Tuple<int, string>(3, "The up arrow button ▲ moves the row up one position") }, MARG_LST1 ));
			SoCtrlsPosHelpInfo.Add(new HelpInfo(
				new [] { new Tuple<int, string>(2, "●"), new Tuple<int, string>(3, "The down arrow button ▼ moves the row down one position") }, MARG_LST1));
			SoCtrlsPosHelpInfo.Add(new HelpInfo(
				new [] { new Tuple<int, string>(2, "●"), new Tuple<int, string>(3, "Enter a number to relocate the current item to that location in the list") }, MARG_LST1));
			SoCtrlsPosHelpInfo.Add(new HelpInfo(
				new [] { new Tuple<int, string>(1, "Notes")}, MARG_HDR3));
			SoCtrlsPosHelpInfo.Add(new HelpInfo(
				new [] { new Tuple<int, string>(2, "Project Units, Sequence 100, cannot be re-ordered") }, MARG_LST1));
		}

		
		
		public static string SoCtrlsLstHelpInfoTitle { get; private set; }
		public static string SoCtrlsLstHelpInfoHeader { get; private set; }
		public static List<HelpInfo> SoCtrlsLstHelpInfo { get; set; } = new List<HelpInfo>();

		private static void configSoCtrlsLstMsgs()
		{
			SoCtrlsLstHelpInfoTitle = "List Control Buttons";
			SoCtrlsLstHelpInfoHeader = "Reset or Apply changes for this list";

			SoCtrlsLstHelpInfo.Add(new HelpInfo(
				new [] { new Tuple<int, string>(0, "Use these controls to adjust the order of the saved style in various lists") }, MARG_HDR1 ));
			SoCtrlsLstHelpInfo.Add(new HelpInfo(
				new [] { new Tuple<int, string>(1, "Notes")}, MARG_HDR3));
			SoCtrlsLstHelpInfo.Add(new HelpInfo(
				new [] { new Tuple<int, string>(1, "Apply"), new Tuple<int, string>(4, "Save the changes, in this list (disabled when there are no changes)") }, MARG_LST1 ));
			SoCtrlsLstHelpInfo.Add(new HelpInfo(
				new [] { new Tuple<int, string>(1, "Reset"), new Tuple<int, string>(4, "Reset the changes, in this list, to the values when the Dialog box was opened (a warning will display if there are changes)") }, MARG_LST1));

		}


				
		public static string SoDialogHelpInfoTitle { get; private set; }
		public static string SoDialogHelpInfoHeader { get; private set; }
		public static List<HelpInfo> SoDialogHelpInfo { get; set; } = new List<HelpInfo>();

		private static void configSoMsgs()
		{
			SoDialogHelpInfoTitle = "Style Order Dialog";
			SoDialogHelpInfoHeader = "Adjust the order of the Saved Styles";

			SoDialogHelpInfo.Add(new HelpInfo(
				new [] { new Tuple<int, string>(0, "Each tab allows the changing of the order of the Saved Styles in each list") }, MARG_HDR1 ));

			SoDialogHelpInfo.Add(new HelpInfo(
				new [] { new Tuple<int, string>(1, "Ribbon"), new Tuple<int, string>(5, "This tab determines the order of") }, MARG_LST2A ));
			SoDialogHelpInfo.Add(new HelpInfo(
				new [] { new Tuple<int, string>(1, "Button"), new Tuple<int, string>(5, "Saved Styles in the Unit Styles Ribbon Button") }, MARG_LST2B ));
			
			SoDialogHelpInfo.Add(new HelpInfo(
				new [] { new Tuple<int, string>(1, "Dialog"), new Tuple<int, string>(5, "This tab determines the order of") }, MARG_LST2A));			
			SoDialogHelpInfo.Add(new HelpInfo(
				new [] { new Tuple<int, string>(1, "Left"), new Tuple<int, string>(5, "the Saved Styles in the Left list on the Delux Measure dialog") }, MARG_LST2B));
			
			SoDialogHelpInfo.Add(new HelpInfo(
				new [] { new Tuple<int, string>(1, "Dialog"), new Tuple<int, string>(5, "This tab determines the order of") }, MARG_LST2A));
			SoDialogHelpInfo.Add(new HelpInfo(
				new [] { new Tuple<int, string>(1, "Right"), new Tuple<int, string>(5, "the Saved Styles in the Right list on the Delux Measure dialog") }, MARG_LST2B));

		}



		/*

		help info:

		subjects:
		sample
		ribbon favs cbx
		left dialog cbx
		right dialog cbx

		new name
		new desc
		index

		list controls
		dialog controls


		sample:
		title:
		Style Sample Information
		header
		The sample shows an actual usage of this style
		Body
		The sample is just a number but note the following when entering a number:
		 * You can enter a number without units. This is considered a length in the current units of the model
		 * You can enter a number with the unit symbology for the style.
		 * You cannot use any other unit symbology
		Note: The number, within parentheses, is the length without units.



		ribbon favs cbx:
		title:
		Ribbon Fav Checkbox
		header:
		Set style for the ribbon favorites
		Body:
		Checked off styles will be included as a ribbon favorite
		Suggestion: Keep the number of ribbon favorites to around 4 or 5 maximum for ease of use.

		left dialog cbx:
		title:
		Left Unit Style List Checkbox 
		header:
		Set style for the left list in the Delux Measure app
		Body
		Checked off styles will be included in the left unit style list in the Delux Measure app.  
		The left unit style list is the primary length measurement.
		Suggestion: Keep the number to less than 10 for ease of use.

		right dialog cbx:
		title:
		Right Unit Style List Checkbox 
		Header:
		Set style for the right list in the Delux Measure app
		Body:
		Checked off styles will be included in the right unit style list in the Delux Measure app.  
		The right unit style list is the alternate length measurement.
		Note: Keep the number to less than 10 for ease of use.

		new name
		title:
		Provide a Name for the New Style
		header:
		(match)

		new description
		title:
		Provide a Description for the New Style
		(match)

		index
		title:
		Insertion Index
		header:
		Provide the row number to determine where to insert the new style
		body:
		 * The new style will be placed before this row
		 * The new style cannot be placed before row 1
		 * The insertion row cannot be greater than the number of styles
		Note: The "last" button adds the new style to the end of the list

		list controls
		title:
		List Control Buttons
		header:
		Modify the order or remove the styles in the list
		body:
		Use these controls to adjust the order or to remove styles in the saved style list
		 * Style 1 is locked and cannot be moved or removed
		 * The up arrow button ^ moves the row up one position
		 * The down arrow button v moves the row down one position
		 * The Trash can button permanently removes the row from the list

		dialog controls
		title:
		Dialog Control Buttons
		header
		Primary Controls for this Dialog
		body
		These buttons work this way
		 * Done: Close the dialog - everything is completed (not available if you have made changes)
		 * Apply: Save all of the changes (not available if there are no changes).
		 * Cancel: Close the dialog regardless of changes (a warning will display if there are changes)
		 * Reset: Reset all changes to the values when the Dialog box was opened regardless of changes (a warning will display if there are changes)
		 * Adjust Style Order (far right): Opens the Adjust Style Order dialog
		(bold): 


		*/

		/*
		public static List<string[]> SampleSyntaxRules { get; private set; }

		private static void configSampleMsgs()
		{
			SampleSyntaxRules = new List<string[]>();
			SampleSyntaxRules.Add(new [] { "●", "", null, null, null });
			SampleSyntaxRules.Add(new [] { "●", "", null, null, null });
			SampleSyntaxRules.Add(new [] { "●", "", null, null, null });
			SampleSyntaxRules.Add(new [] { "●", "", null, null, null });
		}
		*/
	}
}