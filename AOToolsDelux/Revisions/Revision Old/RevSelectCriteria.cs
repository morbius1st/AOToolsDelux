using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

using static AOToolsDelux.SelectCriteria.ECompare;
using static AOToolsDelux.SelectCriteria.Filter;
using static AOToolsDelux.SelectCriteria.EVisibility;


namespace AOToolsDelux
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

	public class SelectCriteria
	{
		public enum EVisibility
		{
			VISIBILITY_ALL = -1,
			VISIBILITY_HIDDEN = 0,
			VISIBILITY_CLOUDANDTAG = 1,
			VISIBILITY_TAGONLY = 2,
			VISIBILITY_COUNT = VISIBILITY_TAGONLY + 2
		}

		public enum ECompare
		{
			LESS_THEN_OR_EQUAL = -3,
			LESS_THEN = -2,
			NOT_EQUAL= -1,
			ANY	 = 0,
			EQUAL = 1,
			GREATER_THEN = 2,
			GREATER_THEN_OR_EQUAL = 3
		}

		// items to check
		public enum Filter
		{
			REVID,
			REVALTID,
			BLOCKTITLE,
			DELTATITLE,
			BASIS,
			COUNT
		}

		private const int TEST_COUNT = (int) COUNT + 1;

		private RevisionVisibility[] _visCrossRef = new RevisionVisibility[(int) VISIBILITY_COUNT];


		#region + Selection Criteria Class Elements

		public SelectCriteria() : this(VISIBILITY_ALL) { }

		public SelectCriteria(EVisibility visibility)
		{
			init();

			Visible = visibility;
		}

		public EVisibility Visible { get; set; }

		private ECompare[] _filterCompare;
		private string[] _filterValue;

		private void init()
		{
			_visCrossRef[(int) VISIBILITY_HIDDEN] = RevisionVisibility.Hidden;
			_visCrossRef[(int) VISIBILITY_CLOUDANDTAG] = RevisionVisibility.CloudAndTagVisible;
			_visCrossRef[(int) VISIBILITY_TAGONLY] = RevisionVisibility.TagVisible;

			_filterCompare = new ECompare[(int) COUNT];
			_filterValue = new string[(int) COUNT];

			for (int i = 0; i < (int) COUNT; i++)
			{
				_filterCompare[i] = ECompare.ANY;
				_filterValue[i] = "";
			}
		}
		#endregion

		#region + RevId
		// setter
		public void RevId(ECompare c, string value = "")
		{
			Setter(REVID, c, value);
		}
		// getter
		public string RevId()
		{
			return _filterValue[(int) Filter.REVID];
		}
		// validate 
		private bool CompareRevId(string test)
		{
			return CompareString(test, Filter.REVID);
		}
		#endregion

		#region + RevAltId
		// setter
		public void RevAltId(ECompare c, string value = "")
		{
			Setter(Filter.REVALTID, c, value);
		}
		// getter
		public string RevAltId()
		{
			return _filterValue[(int) Filter.REVALTID];
		}
		// validate 
		private bool CompareRevAltId(string test)
		{
			return CompareString(test, Filter.REVALTID);
		}
		#endregion

		#region + BlockTitle
		// setter
		public void BlockTitle(ECompare c, string value)
		{
			Setter(Filter.BLOCKTITLE, c, value);
		}
		// getter
		public string BlockTitle()
		{
			return _filterValue[(int) Filter.BLOCKTITLE];
		}
		// validate 
		private bool CompareBlockTitle(string test)
		{
			return CompareString(test, Filter.BLOCKTITLE);
		}
		#endregion

		#region + DeltaTitle
		// setter
		public void DeltaTitle(ECompare c, string value)
		{
			Setter(Filter.DELTATITLE, c, value);
		}
		// getter
		public string DeltaTitle()
		{
			return _filterValue[(int) Filter.DELTATITLE];
		}
		// validate 
		private bool CompareDeltaTitle(string test)
		{
			return CompareString(test, Filter.DELTATITLE);
		}

		#endregion

		#region + Basis
		// setter
		public void Basis(ECompare c, string value)
		{
			Setter(Filter.BASIS, c, value);
		}
		// getter
		public string Basis()
		{
			return _filterValue[(int) Filter.BASIS];
		}
		// validate 
		private bool CompareBasis(string test)
		{
			return CompareString(test, Filter.BASIS);
		}
		
		#endregion

		#region + Utility

		// utility - getter
		private void Setter(Filter f, ECompare c, string value)
		{
			_filterCompare[(int) f] = c;
			_filterValue[(int) f] = "";

			if (c != ECompare.ANY)
			{
				if (string.IsNullOrWhiteSpace(value))
				{
					_filterCompare[(int) f] = ECompare.ANY;
				} else 
				{
					_filterValue[(int) f] = value;
				}
			}
		}

		// check fields against criteria to determine if it
		// matches / passes
		public bool Match(RevDataKey key, RevDataItems items)
		{
			bool result = false;

			for (int i = 0; i < TEST_COUNT; i++)
			{
				result = false;

				switch (i)
				{
				case 0:	// visibility
					{
						result = Visible == VISIBILITY_ALL ||
							items.RevVisible == _visCrossRef[(int) Visible];
						break;
					}
				case 1:	// revid
					{
						result = CompareRevId(items.RevId);
						break;
					}
				case 2:	// revaltid
					{
						result = CompareRevAltId(key.RevAltId);
						break;
					}
				case 3:	// blocktitle
					{
						result = CompareBlockTitle(items.RevBlockTitle);
						break;
					}
				case 4:	// deltatitle
					{
						result = CompareDeltaTitle(key.RevDeltaTitle);
						break;
					}
				case 5:	// basis
					{
						result = CompareBasis(items.RevBasis);
						break;
					}
				}

				if (result == false) break;
			}

			return result;
		}


		// compare the test string to the stored test value
		// if filterCompare is n/a, skip
		private bool CompareString(string test, Filter filter)
		{
			int f = (int) filter;

			bool result = _filterCompare[f] == ANY;

			if (!result)
			{
				int compare = test.ToLower().CompareTo(_filterValue[f].ToLower());

				if (compare == 0 && ( _filterCompare[f] == EQUAL ||
						_filterCompare[f] == GREATER_THEN_OR_EQUAL ||
						_filterCompare[f] == LESS_THEN_OR_EQUAL
					))
				{
					result = true;
				} 
				else if (compare > 0 &&
					(_filterCompare[f] == GREATER_THEN_OR_EQUAL ||
						_filterCompare[f] == GREATER_THEN))
				{
					result = true;

				} else if (_filterCompare[f] == LESS_THEN_OR_EQUAL ||
					_filterCompare[f] == LESS_THEN)
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