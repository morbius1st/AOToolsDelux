namespace AOTools
{
	public static class SortKey
	{
		// note: must be careful here as L will also match LS, Lt & LV
		// so longer letter codes must occur before shorter
		// letter codes - don't place these is ID code order
		private static string[ , ] disicplineSortCodes = new string[,]
		{
			{"CS",	"00.00" },
			{"T",	"00.02" },
			{"LS",	"00.04" },
			{"A",	"07.00" },
			{"ID",	"09.00" },
			{"S",	"11.00" },
		};

		// append a number at the end to prevent duplicate keys
		public static int UniqueCode { get; set; } = 0;

		private const int SortKeyQty = 5;
		public static int[] SortKeyOrder { get; set; } = new [] {0, 1, 2, 3, 4};

		private static string[] SortStrings = new string[SortKeyQty];

		private const string REVID_FMT_PATTERN   = " >{0:D4}"; 
		private const string REVTL_FMT_PATTERN   = "<>{0,-16}"; 
		private const string REVSN_FMT_PATTERN   = "<>{0,-20}"; 
		private const string UNIQUE_CODE_PATTERN = "<>{0:D4}<";

		public static string GetSortKey (string revAltId, 
			string revTypdCode, string revDispCode,
			string revTitle, string shtNum)
		{
			string result = "";

			SortStrings[0] = string.Format(REVID_FMT_PATTERN, revAltId);
			SortStrings[1] = revTypdCode;
			SortStrings[2] = revDispCode;
			SortStrings[3] = string.Format(REVSN_FMT_PATTERN, shtNum);
			SortStrings[4] = string.Format(REVTL_FMT_PATTERN, revTitle);

			for (int i = 0; i < SortKeyQty; i++)
			{
				result += SortStrings[SortKeyOrder[i]];
			}

			// add a unique number at the back to insure no duplicate
			// sort keys
			result += string.Format(UNIQUE_CODE_PATTERN, UniqueCode++);

			return result;
		}

		public static string GetTypeSortCode(string revDeltaTitle)
		{
			string result = ".99";

			switch (revDeltaTitle.Substring(0,3).ToUpper())
			{
			case "BUL":
				{
					result = ".00";
					break;
				}
			case "ASI":
				{
					result = ".10";
					break;
				}
			case "RFI":
				{
					result = ".20";
					break;
				}
			}
			return result;
		}

		public static string GetDisciplineSortCode(string shtNum)
		{
			string result = ".99";
			string shtNumPrefix = shtNum.ToUpper();

			int p = shtNumPrefix.IndexOf(' ');

			if (p > 0)
			{
				shtNumPrefix = shtNumPrefix.Substring(p + 1);
			}

			// read through each item and check for a match
			for (int i = 0; i < disicplineSortCodes.Length; i++)
			{
				if (shtNumPrefix.Substring(0, disicplineSortCodes[i, 0].Length)
					== disicplineSortCodes[i, 0])
				{
					result = "." + disicplineSortCodes[i, 1];
					break;
				}
			}

			return result;
		}

	}
}