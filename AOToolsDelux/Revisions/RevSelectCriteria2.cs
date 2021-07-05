using System;

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

using static AOTools.Revisions.SelectCriteria2.ESelCompare;
using static AOTools.Revisions.SelectCriteria2.ESelVisibility;
using static AOTools.Revisions.SelectCriteria2.Filter;


namespace AOTools.Revisions
{
	// selection method:
	//	this item
	// visible or not visible
	//	and / or
	//	one of these items
	// revision id (==, >, >=, <, <=, !=)#
	// revision alt id == ## (will get master and childern
	// block title == ""
	// delta title == ""
	//	and / or this item
	// basis


	#region + Selection Criteria

	public class SelectCriteria2
	{
		public enum ESelVisibility
		{
			VISIBILITY_ALL = -1,
			VISIBILITY_HIDDEN = 0,
			VISIBILITY_CLOUDANDTAG = 1,
			VISIBILITY_TAGONLY = 2,
			VISIBILITY_COUNT = VISIBILITY_TAGONLY + 2
		}

		public enum ESelCompare
		{
			DOES_NOT_CONTAIN = -13,
			DOES_NOT_START_WITH = -12,
			IS_NOT_EMPTY = -11,
			LESS_THEN_OR_EQUAL = -3,
			LESS_THEN = -2,
			NOT_EQUAL= -1,
			ANY	 = 0,
			EQUAL = 1,
			GREATER_THEN = 2,
			GREATER_THEN_OR_EQUAL = 3,
			IS_EMPTY = 11,
			STARTS_WITH = 12,
			CONTAINS = 13,
		}

		// items to check
		public enum Filter
		{
			REVID,
			REVALTID,
			SHTNUM,
			BLOCKTITLE,
			DELTATITLE,
			BASIS,
			VISIBILITY,
		}

		private const int COUNT = (int) BASIS + 1;

		private const int TOTAL_COUNT = (int) COUNT + 1;

		private RevisionVisibility[] _visCrossRef = 
			new RevisionVisibility[(int) VISIBILITY_COUNT];


		#region + Selection Criteria Class Elements

		public SelectCriteria2() : this(VISIBILITY_ALL) { }

		public SelectCriteria2(ESelVisibility visibility)
		{
			init();

			Visible = visibility;
		}

		public ESelVisibility Visible { get; set; }

		private ESelCompare[] _filterSelCompare;
		private string[] _filterValue;

		private void init()
		{
			_visCrossRef[(int) VISIBILITY_HIDDEN] = 
				RevisionVisibility.Hidden;
			_visCrossRef[(int) VISIBILITY_CLOUDANDTAG] = 
				RevisionVisibility.CloudAndTagVisible;
			_visCrossRef[(int) VISIBILITY_TAGONLY] = 
				RevisionVisibility.TagVisible;

			_filterSelCompare = new ESelCompare[(int) COUNT];
			_filterValue = new string[(int) COUNT];

			for (int i = 0; i < (int) COUNT; i++)
			{
				_filterSelCompare[i] = ANY;
				_filterValue[i] = "";
			}
		}
		#endregion


		#region + RevId
		// setter
		public void RevId(ESelCompare c, string value = "")
		{
			Setter(REVID, c, value);
		}
		// getter
		public string RevId()
		{
			return _filterValue[(int) REVID];
		}
		// validate 
		private bool CompareRevId(string test)
		{
			return CompareString(REVID, test);
		}
		#endregion

		#region + RevAltId
		// setter
		public void RevAltId(ESelCompare c, string value = "")
		{
			Setter(REVALTID, c, value);
		}
		// getter
		public string RevAltId()
		{
			return _filterValue[(int) REVALTID];
		}
		// validate 
		private bool CompareRevAltId(string test)
		{
			return CompareString( REVALTID, test);
		}
		#endregion

		#region + BlockTitle
		// setter
		public void BlockTitle(ESelCompare c, string value)
		{
			Setter(BLOCKTITLE, c, value);
		}
		// getter
		public string BlockTitle()
		{
			return _filterValue[(int) BLOCKTITLE];
		}
		// validate 
		private bool CompareBlockTitle(string test)
		{
			return CompareString( BLOCKTITLE, test);
		}
		#endregion

		#region + DeltaTitle
		// setter
		public void DeltaTitle(ESelCompare c, string value)
		{
			Setter(DELTATITLE, c, value);
		}
		// getter
		public string DeltaTitle()
		{
			return _filterValue[(int) DELTATITLE];
		}
		// validate 
		private bool CompareDeltaTitle(string test)
		{
			return CompareString( DELTATITLE, test);
		}

		#endregion

		#region + Basis
		// setter
		public void Basis(ESelCompare c, string value)
		{
			Setter(BASIS, c, value);
		}
		// getter
		public string Basis()
		{
			return _filterValue[(int) BASIS];
		}
		// validate 
		private bool CompareBasis(string test)
		{
			return CompareString( BASIS, test);
		}
		
		#endregion


		#region + Comparisons

		// check fields against criteria to determine if it
		// matches / passes
		public bool Match(string key, RevDataItems2 items)
		{
			bool result = false;

			foreach (Filter value in Enum.GetValues(typeof(Filter)))
			{
				result = false;

				switch (value)
				{
				case VISIBILITY:	// visibility
					{
						result = Visible == VISIBILITY_ALL ||
							items.Visibility == _visCrossRef[(int) Visible];
						break;
					}
				case REVID:	// revid
					{
						result = CompareRevId(items.RevisionId);
						break;
					}
				case REVALTID:	// revaltid
					{
						result = CompareRevAltId(items.AltId);
						break;
					}
				case BLOCKTITLE:	// blocktitle
					{
						result = CompareBlockTitle(items.BlockTitle);
						break;
					}
				case DELTATITLE:	// deltatitle
					{
						result = CompareDeltaTitle(items.DeltaTitle);
						break;
					}
				case BASIS:	// basis
					{
						result = CompareBasis(items.Basis);
						break;
					}
				case SHTNUM:	// shtnum
					{
						result = CompareBasis(items.ShtNum);
						break;
					}
				}

				if (result == false) break;
			}

			return result;
		}

		#endregion


		#region + Utility

		// getter
		private void Setter(Filter f, ESelCompare c, string value)
		{
			if ((int) f >= (int) COUNT) return;

			_filterSelCompare[(int) f] = c;
			_filterValue[(int) f] = "";

			if (c != ANY)
			{
				if (string.IsNullOrWhiteSpace(value))
				{
					_filterSelCompare[(int) f] = ANY;
				} else 
				{
					_filterValue[(int) f] = value;
				}
			}
		}


		// compare the test string to the stored test value
		// if filterCompare is n/a, skip
		private bool CompareString(Filter filter, string test)
		{
			int f = (int) filter;

			if (f >= (int) COUNT) return false;

			bool result = _filterSelCompare[f] == ANY;

			if (!result)
			{
				int compare = test.ToLower().CompareTo(_filterValue[f].ToLower());

				if (compare == 0 && ( _filterSelCompare[f] == EQUAL ||
						_filterSelCompare[f] == GREATER_THEN_OR_EQUAL ||
						_filterSelCompare[f] == LESS_THEN_OR_EQUAL
					))
				{
					result = true;
				} 
				else if (compare > 0 &&
					(_filterSelCompare[f] == GREATER_THEN_OR_EQUAL ||
						_filterSelCompare[f] == GREATER_THEN))
				{
					result = true;

				} else if (_filterSelCompare[f] == LESS_THEN_OR_EQUAL ||
					_filterSelCompare[f] == LESS_THEN)
				{
					result = true;
				}
			}

			return result;
		}
		#endregion

	}
	#endregion
}