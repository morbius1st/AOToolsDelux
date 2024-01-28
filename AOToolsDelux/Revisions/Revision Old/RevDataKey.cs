using System;
using System.Collections;
using System.Collections.Generic;
using static AOToolsDelux.RevDataKey.ERevDataKey;

using static UtilityLibrary.MessageUtilities2;

namespace AOToolsDelux
{
	public class RevDataKey : IEnumerable, IComparable<RevDataKey>
	{
		public enum ERevDataKey
		{
			REV_KEY_ALTID,          // (from issued by) a cross-reference to the REV_REVID associated with this item
			REV_KEY_TYPE_CODE,      // code based on the document type
			REV_KEY_DISCIPLINE_CODE,// code based on the discipline
			REV_KEY_DELTA_TITLE,    // (from issued to) simple name for this issuance (goes below the delta)
			REV_KEY_SHTNUM,         // (calculated) sheet number of this tag
			REV_KEY_UNIQUE_CODE,    // unique code to prevent duplicate keys
			REV_KEY_LEN         // the number of fields in the enum
		}

		private const string UNIQUE_CODE_FORMAT = "D6";

		private static int uniqueCode = 0;

		private string[] _revDataKey;

		public static RevDataKeyColumn Columns = new RevDataKeyColumn();

		public RevDataKey()
		{
			_revDataKey = new string[(int) ERevDataKey.REV_KEY_LEN];
			_revDataKey[(int) ERevDataKey.REV_KEY_UNIQUE_CODE] = 
				uniqueCode++.ToString(UNIQUE_CODE_FORMAT);
		}

		public RevDataKey(string altId, string typeCode,
			string discCode, string deltaTitle, string shtNum)
		{
			_revDataKey = new string[(int) ERevDataKey.REV_KEY_LEN];

			_revDataKey[(int) ERevDataKey.REV_KEY_ALTID] = altId;
			_revDataKey[(int) ERevDataKey.REV_KEY_TYPE_CODE] = typeCode;
			_revDataKey[(int) ERevDataKey.REV_KEY_DISCIPLINE_CODE] = discCode;
			_revDataKey[(int) ERevDataKey.REV_KEY_DELTA_TITLE] = deltaTitle;
			_revDataKey[(int) ERevDataKey.REV_KEY_SHTNUM] = shtNum;
			_revDataKey[(int) ERevDataKey.REV_KEY_UNIQUE_CODE] = 
				uniqueCode++.ToString(UNIQUE_CODE_FORMAT);
		}

		public int CompareTo(RevDataKey other)
		{
			int result = 0;

			for (int i = 0; i < (int) REV_KEY_LEN; i++)
			{
				result = _revDataKey[i].CompareTo(other[i]);

				if (result != 0) break;
			}

			return result;
		}

		public IEnumerator GetEnumerator()
		{
			return _revDataKey.GetEnumerator();
		}

		public string this[int idx] // indexer declaration
		{
			get => _revDataKey[idx];
			set => _revDataKey[idx] = value;
		}

		public string this[ERevDataKey idx] // indexer declaration
		{
			get => _revDataKey[(int) idx];
			set => _revDataKey[(int) idx] = value;
		}

		public string RevAltId
		{
			get => _revDataKey[(int) ERevDataKey.REV_KEY_ALTID];
			set => _revDataKey[(int) ERevDataKey.REV_KEY_ALTID] = value;
		}

		public string RevTypeCode
		{
			get => _revDataKey[(int) ERevDataKey.REV_KEY_TYPE_CODE];
			set => _revDataKey[(int) ERevDataKey.REV_KEY_TYPE_CODE] = value;
		}

		public string RevDisciplineCode
		{
			get => _revDataKey[(int) ERevDataKey.REV_KEY_DISCIPLINE_CODE];
			set => _revDataKey[(int) ERevDataKey.REV_KEY_DISCIPLINE_CODE] = value;
		}

		public string RevDeltaTitle
		{
			get => _revDataKey[(int) ERevDataKey.REV_KEY_DELTA_TITLE];
			set => _revDataKey[(int) ERevDataKey.REV_KEY_DELTA_TITLE] = value;
		}

		public string RevShtNumber
		{
			get => _revDataKey[(int) ERevDataKey.REV_KEY_SHTNUM];
			set => _revDataKey[(int) ERevDataKey.REV_KEY_SHTNUM] = value;
		}

		public string RevUniqueCode => 
			_revDataKey[(int) ERevDataKey.REV_KEY_UNIQUE_CODE];


		public class RevDataKeyColumn
		{
			private readonly string[] Cols = new string[(int) REV_KEY_LEN];

			public RevDataKeyColumn()
			{
				Cols[(int) REV_KEY_ALTID]		    = "Revision Alt Number";
				Cols[(int) REV_KEY_TYPE_CODE]	    = "Revision Type Code";
				Cols[(int) REV_KEY_DISCIPLINE_CODE] = "Revision Discipline Code";
				Cols[(int) REV_KEY_DELTA_TITLE]	    = "Delta Title";
				Cols[(int) REV_KEY_SHTNUM]		    = "Sheet Number";
				Cols[(int) REV_KEY_UNIQUE_CODE]	    = "Unique Code";
			}

			public string this[int idx] => Cols[idx];
			public string this[ERevDataKey idx] => Cols[(int) idx];
		}

		
	}
}