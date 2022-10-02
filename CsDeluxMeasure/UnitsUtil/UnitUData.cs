using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Security.Policy;
using System.Windows.Data;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Autodesk.Revit.DB;
using CsDeluxMeasure.Annotations;
using CsDeluxMeasure.Windows.Support;
using UtilityLibrary;
using static CsDeluxMeasure.UnitsUtil.UnitsStdUStyles;
using static CsDeluxMeasure.Windows.Support.UnitStylesMgrWinData;

// Solution:     AOToolsDelux
// Project:       CsDeluxMeasure
// File:             UnitStdStyles.cs
// Created:      2022-03-27 (7:23 AM)

namespace CsDeluxMeasure.UnitsUtil
{
#region data types

	[DataContract(Namespace = "")]
	public abstract class AUnitsData<T, U> : INotifyPropertyChanged
	{
		
		private const string DROP_NAME_PREFACE = "Show Std. Style ";

		private bool unitLeadZeroEnable;
		private bool unitTrailZeroEnable;
		private bool unitExtraSpacesEnable;
		private bool unitShowPlusEnable;
		private bool unitDigitGrpEnable;

		private int sequence;

		private bool deleteStyle = false;

		[DataMember(Order = 1)]
		public int Sequence
		{
			get => sequence;
			set
			{
				sequence = value;
				OnPropertyChanged();
				OnPropertyChanged(nameof(SeqFormatted));
			}
		}

		[IgnoreDataMember]
		public int InitialSequence { get; set; }

		[DataMember(Order = 2)]
		public abstract T Id { get; set; }

		[DataMember(Order = 4)]
		public abstract T Symbol { get; set; }

		[IgnoreDataMember]
		public abstract string Name { get; set; }

		[IgnoreDataMember]
		public abstract string Description { get; set; }

		[IgnoreDataMember]
		public abstract string Sample { get; set; }

		[IgnoreDataMember]
		public abstract U USystem { get; }

		[DataMember(Order = 8)]
		public abstract UStyle Ustyle { get; set; }

		[IgnoreDataMember]
		public bool DeleteStyle
		{
			get => deleteStyle;
			set
			{
				if (deleteStyle == value) return;
				deleteStyle = value;
				OnPropertyChanged();
			}
		}

		// public string UnitSystem      => "* Imperial";

		// public string UnitPrecision   => Ustyle.Precision.ToString();

		public string UnitLeadZero
		{
			get
			{
				bool status = true;
				string result = convertFmtOpt(Ustyle.SuppressLeadZeros ,
					"Yes", "No", "Not Available", ref status);
				UnitLeadZeroEnable = status;
				return result;
			}
		}

		public bool UnitLeadZeroEnable
		{
			get => unitLeadZeroEnable;
			set
			{
				unitLeadZeroEnable = value;
				OnPropertyChanged();
			}
		}

		public string UnitTrailZero
		{
			get
			{
				bool status = true;
				string result = convertFmtOpt(Ustyle.SuppressTrailZeros ,
					"Yes", "No",  "Not Available", ref status);
				UnitTrailZeroEnable = status;
				return result;
			}
		}

		public bool UnitTrailZeroEnable
		{
			get => unitTrailZeroEnable;
			set
			{
				unitTrailZeroEnable = value;
				OnPropertyChanged();
			}
		}

		public string UnitExtraSpaces
		{
			get
			{
				bool status = true;
				string result = convertFmtOpt(Ustyle.SuppressSpaces ,
					"Yes", "No",  "Not Applicable", ref status);
				UnitExtraSpacesEnable = status;
				return result;
			}
		}

		public bool UnitExtraSpacesEnable
		{
			get => unitExtraSpacesEnable;
			set
			{
				unitExtraSpacesEnable = value;
				OnPropertyChanged();
			}
		}

		public string UnitShowPlus
		{
			get
			{
				bool status = true;
				string result = convertFmtOpt(Ustyle.UsePlusPrefix ,
					"Yes", "No", "Not Applicable", ref status);
				UnitShowPlusEnable = status;
				return result;
			}
		}

		public bool UnitShowPlusEnable
		{
			get => unitShowPlusEnable;
			set
			{
				unitShowPlusEnable = value;
				OnPropertyChanged();
			}
		}

		public string UnitDigitGrp
		{
			get
			{
				bool status = true;
				string result = convertFmtOpt(Ustyle.UseDigitGrouping ,
					"Yes", "No",  "Not Available", ref status);
				UnitDigitGrpEnable = status;
				return result;
			}
		}

		public bool UnitDigitGrpEnable
		{
			get => unitDigitGrpEnable;
			set
			{
				unitDigitGrpEnable = value;
				OnPropertyChanged();
			}
		}

		public bool Skip()
		{
			return deleteStyle;
		}

		public string DropDownName
		{
			get
			{
				if (Ustyle.IsControl)
				{
					return Ustyle.Description;
				}

				// 
				return $"{DROP_NAME_PREFACE}: {Ustyle.Name}";
			}
		}

		public string SeqFormatted => (sequence + 1).ToString("00");

		public string UnitSymbolFormated => formatSymbol();
		protected abstract string formatSample(bool isEditing = false);

		public string UnitPrecisionFormatted => formatPrecision();
		protected abstract string formatPrecision();

		public string UnitSampleFormatted => formatSample();
		protected abstract string formatSymbol();

		private string convertFmtOpt(bool? opt, string whenTrue,
			string whenFalse, string whenNull, ref bool enable)
		{
			if (!opt.HasValue)
			{
				enable = false;
				return whenNull;
			}

			return opt.Value ? whenTrue : whenFalse;
		}

		public void UpdateProperties()
		{
			Ustyle.UpdateProperties();

			OnPropertyChanged(nameof(UnitLeadZero));
			OnPropertyChanged(nameof(UnitTrailZero));
			OnPropertyChanged(nameof(UnitShowPlus));
			OnPropertyChanged(nameof(UnitShowPlus));
			OnPropertyChanged(nameof(UnitDigitGrp));
			OnPropertyChanged(nameof(UnitExtraSpaces));
		}


		public event PropertyChangedEventHandler PropertyChanged;

		[DebuggerStepThrough]
		[NotifyPropertyChangedInvocator]
		protected void OnPropertyChanged([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}
	}

	// this is the data map for any type fo sample
	// debug version only  (i.e. ...D
	public class UnitsDataD : AUnitsData<string, string>
	{
		private UStyle ustyle;

		public UnitsDataD(  string id, string sample, string symbol, UStyle us)
		{
			Id = id;
			Ustyle = us;
			Sample = sample;
		}

		[IgnoreDataMember]
		// public BitmapImage Ux => CsUtilitiesMedia.GetBitmapImage(Ustyle.IconId, "CsDeluxMeasure.Resources");
		public BitmapImage Ux => CsUtilitiesMedia.GetBitmapImageResource($"{AppRibbon.ICON_FOLDER}/{Ustyle.IconId}");

		public override string Id { get; set; }

		public override string Symbol
		{
			get => "sample 1";
			set { }
		}

		public override string Sample { get; set; }

		public override string Name
		{
			get => Ustyle.Name;
			set
			{
				int a = 1;
			}
		}

		public override string Description
		{
			get => Ustyle.Description;
			set
			{
				int a = 1;
			}
		}

		public override string USystem => Ustyle.UnitSys.ToString();

		public override UStyle Ustyle

		{
			get => ustyle;
			set
			{
				ustyle = value;
				OnPropertyChanged();
			}
		}

		protected string fmtSymbol()
		{
			// if (Ustyle.Symbol != null) return Ustyle.Symbol;

			string s = UnitsSupport.GetSymbol(Ustyle.UnitCat);

			return s;
		}

		protected override string formatSymbol()
		{
			string s = fmtSymbol();

			if (string.IsNullOrWhiteSpace(s)) return "none";

			return s;
		}

		protected override string formatPrecision()
		{
			string uSym = null;

			if (Ustyle.Precision < 0) return "*invalid";

			if (Ustyle.UnitCat == UnitCat.UC_DECIMAL)
			{
				uSym = USystem.Equals("US_METRIC") ? " " : "";
				uSym += fmtSymbol();
			}

			string s =
				UnitsSupport.GetPrecString(Ustyle.UnitCat,
					Ustyle.Precision, uSym);

			return s;
		}

		protected override string formatSample(bool isEditing = false)
		{
			return Ustyle.Sample?.ToString("F5") ?? "not set";
		}
	}

	// this is the data map for any type of style
	// release version  (i.e. ...R)
	[DataContract(Namespace = "")]
	public class UnitsDataR : AUnitsData<ForgeTypeId, UnitSystem>
	{
		// private static UnitsSupport uSup;

		private ForgeTypeId id;
		private ForgeTypeId symbol;
		private UStyle ustyle;

		


		private string name;
		private ValMsgNameId nameMsgId;
		private bool? isNameOk;


		private string desc;
		private ValMsgDescId descMsgId;
		private bool? isDescOk;

		private string sample;
		private bool? isSampleOk;


		protected UnitsDataR() { }

		public UnitsDataR(ForgeTypeId id, ForgeTypeId symbol, UStyle us)
		{
			Id = id;
			Symbol = symbol;
			Ustyle = us;

			ResetValidateInfo();

			// UStyle.ShowInChanged+= OnShowInPropertyChanged;
		}

		[OnDeserialized]
		void OnDeserialized(StreamingContext context)
		{
			ResetValidateInfo();

			// UStyle.ShowInChanged+= OnShowInPropertyChanged;
		}


	#region public properties

		[System.Diagnostics.DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[IgnoreDataMember]
		public BitmapImage Ux => CsUtilitiesMedia.GetBitmapImageResource($"{AppRibbon.ICON_FOLDER}/{Ustyle.IconId}");

		[DataMember(Order = 2)]
		public override ForgeTypeId Id
		{
			get => id;
			set
			{
				id = value;
				OnPropertyChanged();
			}
		}

		[DataMember(Order = 4)]
		public override ForgeTypeId Symbol
		{
			get => symbol;
			set
			{
				symbol = value;
				OnPropertyChanged();
			}
		}

		[IgnoreDataMember]
		public override string Name
		{
			// process: 
			// connection point with consumers
			// get: the raw value
			// set: a proposed value
			//	set will validate for syntax
			//  set has event that will allow secondary validation
			get
			{
				if (name == null) return Ustyle.Name;

				return name;
			}
			set
			{
				isNameOk = ValidateName(value, out nameMsgId);

				if (isNameOk.Value)
				{
					name = null;

					if (!value.Equals(Ustyle.Name))
					{
						RaiseNameChangedEvent(EventArgs.Empty);
						Ustyle.Name = value;
					}
				}
				else
				{
					name = value;
				}

				OnPropertyChanged();
				OnPropertyChanged(nameof(IsNameOk));
				OnPropertyChanged(nameof(NameValidationMsg));
				OnPropertyChanged(nameof(NameValidationId));
			}
		}

		[IgnoreDataMember]
		public override string Description
		{
			get
			{
				if (desc == null) return Ustyle.Description;

				return desc;
			}
			set
			{
				isDescOk = ValidateDesc(value, out descMsgId);

				if (isDescOk.Value)
				{
					desc = null;

					if (!value.Equals(Ustyle.Description))
					{
						RaiseDescriptionChangedEvent(EventArgs.Empty);
						Ustyle.Description = value;
					}
				}
				else
				{
					desc = value;
				}

				OnPropertyChanged();
				OnPropertyChanged(nameof(IsDescOk));
				OnPropertyChanged(nameof(DescValidationMsg));
				OnPropertyChanged(nameof(DescValidationId));
			}
		}

		[IgnoreDataMember]
		public override string Sample
		{
			// communication point with consumers
			// they get / send a string
			// 

			get => formatSampleWithValue();

			set
			{
				if (value.Equals(""))
				{
					isSampleOk = null;
					sample = "";
				}
				else
				{
					double d = UnitsSupport.ConvertSampleToDbl(this, value);

					if (Double.IsNaN(d))
					{
						isSampleOk = false;
						sample = "Invalid length";
					}
					else
					{
						isSampleOk = true;
						sample = null;
						Ustyle.Sample = d;
					}
				}

				OnPropertyChanged();
				OnPropertyChanged(nameof(IsSampleOk));
				OnPropertyChanged(nameof(SampleForEditing));
			}
		}

		[IgnoreDataMember]
		public override UnitSystem USystem => (UnitSystem) (int) Ustyle.UnitSys;

		[DataMember(Order = 20)]
		public override UStyle Ustyle

		{
			get => ustyle;
			set
			{
				ustyle = value;
				OnPropertyChanged();
			}
		}

		[IgnoreDataMember]
		public bool? IsNameOk
		{
			get => isNameOk;
			set
			{
				if (value == isNameOk) return;

				isNameOk = value;
				OnPropertyChanged();
			}
		}

		[IgnoreDataMember]
		public string NameValidationMsg
		{
			get => UnitStylesMgrWinData.NameErrMsgs[(int) nameMsgId];
		}

		[IgnoreDataMember]
		public ValMsgNameId NameValidationId
		{
			get => nameMsgId;
		}

		[IgnoreDataMember]
		public bool? IsDescOk
		{
			get => isDescOk;
			set
			{
				if (value == isDescOk) return;

				isDescOk = value;
				OnPropertyChanged();
			}
		}

		[IgnoreDataMember]
		public string DescValidationMsg
		{
			get => UnitStylesMgrWinData.NameErrMsgs[(int) descMsgId];
		}

		[IgnoreDataMember]
		public ValMsgDescId DescValidationId
		{
			get => descMsgId;
		}

		[IgnoreDataMember]
		public bool? IsSampleOk
		{
			get => isSampleOk;
			set
			{
				if (value == isSampleOk) return;

				isSampleOk = value;
				OnPropertyChanged();
			}
		}

		[IgnoreDataMember]
		public string SampleForEditing
		{
			// get => UnitsSupport.FormatLength(this, Ustyle.Sample.Value);
			get => formatSample(true);
			set => Sample = value;
		}

	#endregion

	#region format info for user

		protected override string formatSymbol()
		{
			string sym = UnitsSupport.GetSymbol(Symbol, Ustyle.UnitCat);

			return sym;
		}

		protected override string formatPrecision()
		{
			if (Ustyle.Precision < 0) return "*invalid";

			string uSym = formatSymbol();

			// if (Ustyle.UnitCat == UnitCat.UC_DECIMAL)
			// {
			// 	uSym = USystem == UnitSystem.Metric ? " " : "";
			// 	uSym += formatSymbol();
			// }

			string s =
				UnitsSupport.GetPrecString(Ustyle.UnitCat,
					Ustyle.Precision, uSym);

			return s;
		}

		protected override string formatSample(bool isEditing = false)
		{
			if (sample != null) return sample;

			if (!Ustyle.Sample.HasValue)
			{
				return "Not Set";
			}

			string formatted = UnitsSupport.FormatLength(this, Ustyle.Sample.Value, isEditing);

			return formatted;
		}

		protected string formatSampleWithValue()
		{
			string formatted = formatSample(false);

			if (sample != null) return formatted;

			if (!Ustyle.Sample.HasValue) return formatted;

			return $"{formatted} ({Ustyle.Sample.Value:G})";
		}

	#endregion

	#region validate info

		public bool ValidateName(string testName, out ValMsgNameId msgId)
		{
			// check syntax - set both result and msg id
			bool result = UnitsSupport.CheckStyleNameSyntax(testName, out msgId);

			if (result)
			{
				// syntax good, check for custom validation
				ChangeValueEventArgs<ValMsgNameId> e = new ChangeValueEventArgs<ValMsgNameId>(testName, ValMsgNameId.VN_GOOD);
				RaiseNameChangingEvent(e);

				result = !e.Cancel;
				msgId = e.Response;
			}

			return result;
		}

		public bool ValidateDesc(string testDesc, out ValMsgDescId msgId)
		{
			// check syntax - set both result and msg id
			bool result = UnitsSupport.CheckStyleDescSyntax(testDesc, out msgId);

			if (result)
			{
				// syntax good, check for custom validation
				ChangeValueEventArgs<ValMsgDescId> e = new ChangeValueEventArgs<ValMsgDescId>(testDesc, ValMsgDescId.VD_GOOD);
				RaiseDescriptionChangingEvent(e);

				result = !e.Cancel;
				msgId = e.Response;
			}

			return result;
		}

	#endregion

		public void ResetValidateInfo()
		{
			name = null;
			nameMsgId = ValMsgNameId.VN_GOOD;
			isNameOk = null;

			desc = null;
			descMsgId = ValMsgDescId.VD_GOOD;
			isDescOk = null;

			sample = null;
			isSampleOk = null;

		}

	#region system overrides

		public UnitsDataR Clone()
		{
			UnitsDataR copy = new UnitsDataR(Id, Symbol, Ustyle.Clone());

			copy.Sequence = Sequence;

			return copy;
		}

		public override string ToString()
		{
			return $"name| {Name}| ribbon order| {Ustyle.OrderInRibbon}| left order| {Ustyle.OrderInDialogLeft}| right order| {Ustyle.OrderInDialogRight}";
		}

	#endregion

		// private void OnShowInPropertyChanged(object sender, CheckBoxChangedEventArgs e)
		// {
		// 	// RaiseShowInChangedEvent(new CheckBoxChangedEventArgs(e.WhichCheckBox));
		// }

	#region raise events

		public static event UnitsDataR.NameChangingEventHandler OnNameChanging;
		public static event UnitsDataR.DescriptionChangingEventHandler OnDescriptionChanging;

		public static event UnitsDataR.NameChangedEventHandler OnNameChanged;
		public static event UnitsDataR.DescriptionChangedEventHandler OnDescriptionChanged;


		public delegate void NameChangingEventHandler(object sender, ChangeValueEventArgs<ValMsgNameId> e);

		protected virtual void RaiseNameChangingEvent(ChangeValueEventArgs<ValMsgNameId> e)
		{
			OnNameChanging?.Invoke(this, e);
		}


		public delegate void DescriptionChangingEventHandler(object sender, ChangeValueEventArgs<ValMsgDescId> e);

		protected virtual void RaiseDescriptionChangingEvent(ChangeValueEventArgs<ValMsgDescId> e)
		{
			OnDescriptionChanging?.Invoke(this, e);
		}


		public delegate void NameChangedEventHandler(object sender, EventArgs e);

		protected virtual void RaiseNameChangedEvent(EventArgs e)
		{
			OnNameChanged?.Invoke(this, e);
		}


		public delegate void DescriptionChangedEventHandler(object sender, EventArgs e);

		protected virtual void RaiseDescriptionChangedEvent(EventArgs e)
		{
			OnDescriptionChanged?.Invoke(this, e);
		}



		// public static event UnitsDataR.ShowInChangedEventHandler OnShowInChanged;
		//
		// public delegate void ShowInChangedEventHandler(object sender, CheckBoxChangedEventArgs e);
		//
		// protected virtual void RaiseShowInChangedEvent(CheckBoxChangedEventArgs e)
		// {
		// 	OnShowInChanged?.Invoke(this, e);
		// }

	#endregion
	}

#endregion

	// public class UdrCompareInListOrder
	// {
	// 	public static int Compare(UnitsDataR x, UnitsDataR y, InList which)
	// 	{
	// 		if (x == null && y == null) { return 0; }
	// 		if (y == null ||
	// 			x.Ustyle.OrderInList[(int) which] > y.Ustyle.OrderInList[(int) which]) { return 1; }
	// 		if (x == null ||
	// 			x.Ustyle.OrderInList[(int) which] < y.Ustyle.OrderInList[(int) which]) { return -1; }
	//
	// 		return 0;
	// 	}
	// }

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

	public struct WkgInListItemD
	{
		public int ProposedOrder { get; set; }
		public int CurrentOrder { get; set; }
		public UnitsDataD Data { get; set; }

		public WkgInListItemD(int proposedOrder, int currentOrder, UnitsDataD data)
		{
			ProposedOrder = proposedOrder;
			CurrentOrder = currentOrder;
			Data = data;
		}

	}


#region data containers

	[DataContract(Namespace = "")]
	public abstract class AUnitsStdStyles<T, U> : INotifyPropertyChanged
	{
		protected ICollectionView list;
		public abstract Dictionary<T, U> StdStyles { get; protected set; }
		public ICollectionView List { get; set; }

		public static event PropertyChangedEventHandler PropertyChanged_S;

		protected static void OnPropertyChanged_S([CallerMemberName] string memberName = "")
		{
			PropertyChanged_S?.Invoke(null, new PropertyChangedEventArgs(memberName));
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChanged([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}
	}


	[DataContract(Namespace = "")]
	public class UnitStdStylesD : AUnitsStdStyles<string, UnitsDataD>, IEqualityComparer<UnitsDataD>
	{
		public UnitStdStylesD() { }

		static UnitStdStylesD()
		{
			initialize();
		}

		// private new static ICollectionView list;
		private static List<UnitsDataD> listD;
		// private static List<UnitsDataD> listDribbon;
		// private static List<UnitsDataD> listDleft;
		// private static List<UnitsDataD> listDright;

		public static List<WkgInListItemD> InListsRibbon { get; protected set; }
		public static List<WkgInListItemD> InListsDlgLeft { get; protected set; }
		public static List<WkgInListItemD> InListsDlgRight { get; protected set; }

		public static Dictionary<string, UnitsDataD> SStdStyles { get; protected set; }
		public override Dictionary<string, UnitsDataD> StdStyles { get; protected set; }

		public static List<UnitsDataD> ListD
		{
			get => listD;
			set
			{
				listD = value;
				OnPropertyChanged_S();
			}
		}
		

		public bool Equals(UnitsDataD x, UnitsDataD y)
		{
			if (x == null || y == null) return false;
			return x.Id.Equals(y.Id);
		}

		public int GetHashCode(UnitsDataD obj)
		{
			return 0;
		}

		private static void addStyleItem(int i, STYLE_DATA sid, string sample)
		{
			UnitsDataD udd;

			udd = new UnitsDataD(
				sid.TypeId, sample, 
				sid.Symbol,
				StdSysStyles[sid.NameId]);
			udd.Sequence = i;
			ListD.Add(udd);
			SStdStyles.Add(udd.Ustyle.Name, udd);

			if (udd.Ustyle.ShowInRibbon)
			{
				
				InListsRibbon.Add(new WkgInListItemD(udd.Ustyle.OrderInRibbon, udd.Ustyle.OrderInRibbon, udd));
			}
			OnPropertyChanged_S(nameof(InListsRibbon));

			if (udd.Ustyle.ShowInDialogLeft)
			{
				InListsDlgLeft.Add(new WkgInListItemD(udd.Ustyle.OrderInDialogLeft, udd.Ustyle.OrderInDialogLeft, udd));
				// listDleft.Add(udd);
			}
			OnPropertyChanged_S(nameof(InListsDlgLeft));

			if (udd.Ustyle.ShowInDialogRight)
			{
				InListsDlgRight.Add(new WkgInListItemD(udd.Ustyle.OrderInDialogRight, udd.Ustyle.OrderInDialogRight, udd));
				// listDright.Add(udd);
			}
			OnPropertyChanged_S(nameof(InListsDlgRight));

		}

		private static void initialize()
		{
			InListsRibbon =   new List<WkgInListItemD>();
			InListsDlgLeft =  new List<WkgInListItemD>();
			InListsDlgRight = new List<WkgInListItemD>();

			SStdStyles = new Dictionary<string, UnitsDataD>(12);
			ListD = new List<UnitsDataD>();
			// listDribbon = new List<UnitsDataD>();
			// listDleft = new List<UnitsDataD>();
			// listDright = new List<UnitsDataD>();

			

			int i = 0;

			addStyleItem(i++, STYLE_DATA.Project, "1,123'-11 255/256\"");
			// udd = new UnitsDataD(
			// 	STYLE_ID_PROJECT, "1,123'-11 255/256\"",
			// 	UnitsStdUStyles.StdStyles[STYLE_ID_PROJECT]);
			// udd.Sequence = i++;
			// ListD.Add(udd);
			// SStdStyles.Add(udd.Ustyle.Name, udd);


			addStyleItem(i++, STYLE_DATA.FtFracIn, "123'-4 5/8\"");
			// udd = new UnitsDataD(
			// 	STYLE_ID_FT_FRAC_IN, "123'-4 5/8\"",
			// 	UnitsStdUStyles.StdStyles[STYLE_ID_FT_FRAC_IN]);
			// udd.Sequence = i++;
			// ListD.Add(udd);
			//  SStdStyles.Add(udd.Ustyle.Name, udd);


			addStyleItem(i++, STYLE_DATA.FtDecIn, "123'-4.625\"");
			// udd = new UnitsDataD(
			// 	STYLE_ID_FT_DEC_IN, "123'-4.625\"",
			// 	UnitsStdUStyles.StdStyles[STYLE_ID_FT_DEC_IN]);
			// udd.Sequence = i++;
			// ListD.Add(udd);
			//  SStdStyles.Add(udd.Ustyle.Name, udd);


			addStyleItem(i++, STYLE_DATA.UsSurvey, "123'-4 5/8\"");
			// udd = new UnitsDataD(
			// 	STYLE_ID_US_SURVEY, "123'-4 5/8\"",
			// 	UnitsStdUStyles.StdStyles[STYLE_ID_US_SURVEY]);
			// udd.Sequence = i++;
			//  SStdStyles.Add(udd.Ustyle.Name, udd);


			addStyleItem(i++, STYLE_DATA.Feet, "1,234.567'");
			// udd = new UnitsDataD(
			// 	STYLE_ID_FEET, "1,234.567'",
			// 	UnitsStdUStyles.StdStyles[STYLE_ID_FEET]);
			// udd.Sequence = i++;
			// ListD.Add(udd);
			//  SStdStyles.Add(udd.Ustyle.Name, udd);


			addStyleItem(i++, STYLE_DATA.DecInches, "123.456\"");
			// udd = new UnitsDataD(
			// 	STYLE_ID_DEC_INCHES, "123.456\"",
			// 	UnitsStdUStyles.StdStyles[STYLE_ID_DEC_INCHES]);
			// udd.Sequence = i++;
			// ListD.Add(udd);
			//  SStdStyles.Add(udd.Ustyle.Name, udd);


			addStyleItem(i++, STYLE_DATA.FracInches, "14 129/256\"");
			// udd = new UnitsDataD(
			// 	STYLE_ID_FRAC_INCHES, "14 129/256\"",
			// 	UnitsStdUStyles.StdStyles[STYLE_ID_FRAC_INCHES]);
			// udd.Sequence = i++;
			// ListD.Add(udd);
			//  SStdStyles.Add(udd.Ustyle.Name, udd);


			addStyleItem(i++, STYLE_DATA.Meters, "123.456m");
			// udd = new UnitsDataD(
			// 	STYLE_ID_METERS, "123.456m",
			// 	UnitsStdUStyles.StdStyles[STYLE_ID_METERS]);
			// udd.Sequence = i++;
			// ListD.Add(udd);
			//  SStdStyles.Add(udd.Ustyle.Name, udd);


			addStyleItem(i++, STYLE_DATA.Decimeters, "123.456dm");
			// udd = new UnitsDataD(
			// 	STYLE_ID_DECIMETERS, "123.456dm",
			// 	UnitsStdUStyles.StdStyles[STYLE_ID_DECIMETERS]);
			// udd.Sequence = i++;
			// ListD.Add(udd);
			//  SStdStyles.Add(udd.Ustyle.Name, udd);


			addStyleItem(i++, STYLE_DATA.Centimeters, "123.456cm");
			// udd = new UnitsDataD(
			// 	STYLE_ID_CENTIMETERS, "123.456cm",
			// 	UnitsStdUStyles.StdStyles[STYLE_ID_CENTIMETERS]);
			// udd.Sequence = i++;
			// ListD.Add(udd);
			//  SStdStyles.Add(udd.Ustyle.Name, udd);


			addStyleItem(i++, STYLE_DATA.Millimeters, "123.456mm");
			// udd = new UnitsDataD(
			// 	STYLE_ID_MILLIMETERS, "123.456mm",
			// 	UnitsStdUStyles.StdStyles[STYLE_ID_MILLIMETERS]);
			// udd.Sequence = i++;
			// ListD.Add(udd);
			//  SStdStyles.Add(udd.Ustyle.Name, udd);


			addStyleItem(i++, STYLE_DATA.MetersCentimeters, "123m 456cm");
			// udd = new UnitsDataD(
			// 	STYLE_ID_METERS_CENTIMETERS, "123m 456cm",
			// 	UnitsStdUStyles.StdStyles[STYLE_ID_METERS_CENTIMETERS]);
			// udd.Sequence = i++;
			// ListD.Add(udd);
			//  SStdStyles.Add(udd.Ustyle.Name, udd);

			// ListD = new List<UnitsDataD>(SStdStyles.Values);
		}
	}

	[DataContract(Namespace = "")]
	public class UnitStdStylesR : AUnitsStdStyles<string, UnitsDataR>
	{
		public UnitStdStylesR()
		{
			initialize();
		}

		[DataMember(Order = 2)]
		public override Dictionary<string, UnitsDataR> StdStyles { get; protected set; }

		private void addStyleItem(int i, ForgeTypeId uid, ForgeTypeId sid, string name)
		{
			UnitsDataR udr = new UnitsDataR(uid, sid, UnitsStdUStyles.StdSysStyles[name]);
			udr.Sequence = i;
			StdStyles.Add(udr.Ustyle.Name, udr);
		}

		private void initialize()
		{
			StdStyles = new Dictionary<string, UnitsDataR>(12);
			int i = 0;

			UnitsDataR udr;

			addStyleItem(i++, UnitTypeId.General, null, STYLE_DATA.Project.NameId);


			// udr = new UnitsDataR(UnitTypeId.General, null, UnitsStdUStyles.StdStyles[STYLE_ID_PROJECT]);
			// udr.Sequence = i++;
			// StdStyles.Add(udr.Ustyle.Name, udr);
			//
			addStyleItem(i++, UnitTypeId.FeetFractionalInches, null, STYLE_DATA.FtFracIn.NameId);
			//
			// udr = new UnitsDataR(
			// 	UnitTypeId.FeetFractionalInches, null,
			// 	UnitsStdUStyles.StdStyles[STYLE_ID_FT_FRAC_IN]);
			// udr.Sequence = i++;
			// StdStyles.Add(STYLE_ID_FT_FRAC_IN, udr);
			//
			addStyleItem(i++, UnitTypeId.Custom, null, STYLE_DATA.FtDecIn.NameId);
			//
			// udr = new UnitsDataR(
			// 	UnitTypeId.Custom, null,
			// 	UnitsStdUStyles.StdStyles[STYLE_ID_FT_DEC_IN]);
			// udr.Sequence = i++;
			// StdStyles.Add(STYLE_ID_FT_DEC_IN, udr);
			//
			addStyleItem(i++, UnitTypeId.UsSurveyFeet, SymbolTypeId.Usft, STYLE_DATA.UsSurvey.NameId);
			//
			// udr = new UnitsDataR(
			// 	UnitTypeId.UsSurveyFeet, SymbolTypeId.Usft,
			// 	UnitsStdUStyles.StdStyles[STYLE_ID_US_SURVEY]);
			// udr.Sequence = i++;
			// StdStyles.Add(STYLE_ID_US_SURVEY, udr);
			//
			addStyleItem(i++, UnitTypeId.Feet, SymbolTypeId.FootSingleQuote, STYLE_DATA.Feet.NameId);
			//
			// udr = new UnitsDataR(
			// 	UnitTypeId.Feet, SymbolTypeId.Ft,
			// 	UnitsStdUStyles.StdStyles[STYLE_ID_FEET]);
			// udr.Sequence = i++;
			// StdStyles.Add(STYLE_ID_FEET, udr);
			//
			addStyleItem(i++, UnitTypeId.Inches, SymbolTypeId.InchDoubleQuote, STYLE_DATA.DecInches.NameId);
			//
			// udr = new UnitsDataR(
			// 	UnitTypeId.Inches, SymbolTypeId.InchDoubleQuote,
			// 	UnitsStdUStyles.StdStyles[STYLE_ID_DEC_INCHES]);
			// udr.Sequence = i++;
			// StdStyles.Add(STYLE_ID_DEC_INCHES, udr);
			//
			addStyleItem(i++, UnitTypeId.FractionalInches, null, STYLE_DATA.FracInches.NameId);
			//
			// udr = new UnitsDataR(
			// 	UnitTypeId.FractionalInches, null,
			// 	UnitsStdUStyles.StdStyles[STYLE_ID_FRAC_INCHES]);
			// udr.Sequence = i++;
			// StdStyles.Add(STYLE_ID_FRAC_INCHES, udr);
			//
			addStyleItem(i++, UnitTypeId.Meters, SymbolTypeId.Meter, STYLE_DATA.Meters.NameId);
			//
			// udr = new UnitsDataR(
			// 	UnitTypeId.Meters, SymbolTypeId.Meter,
			// 	UnitsStdUStyles.StdStyles[STYLE_ID_METERS]);
			// udr.Sequence = i++;
			// StdStyles.Add(STYLE_ID_METERS, udr);
			//
			addStyleItem(i++, UnitTypeId.Decimeters, SymbolTypeId.Dm, STYLE_DATA.Decimeters.NameId);
			//
			// udr = new UnitsDataR(
			// 	UnitTypeId.Decimeters, SymbolTypeId.Dm,
			// 	UnitsStdUStyles.StdStyles[STYLE_ID_DECIMETERS]);
			// udr.Sequence = i++;
			// StdStyles.Add(STYLE_ID_DECIMETERS, udr);
			//
			addStyleItem(i++, UnitTypeId.Centimeters, SymbolTypeId.Cm, STYLE_DATA.Centimeters.NameId);
			//
			// udr = new UnitsDataR(
			// 	UnitTypeId.Centimeters, SymbolTypeId.Cm,
			// 	UnitsStdUStyles.StdStyles[STYLE_ID_CENTIMETERS]);
			// udr.Sequence = i++;
			// StdStyles.Add(STYLE_ID_CENTIMETERS, udr);
			//
			addStyleItem(i++, UnitTypeId.Millimeters, SymbolTypeId.Mm, STYLE_DATA.Millimeters.NameId);
			//
			// udr = new UnitsDataR(
			// 	UnitTypeId.Millimeters, SymbolTypeId.Mm,
			// 	UnitsStdUStyles.StdStyles[STYLE_ID_MILLIMETERS]);
			// udr.Sequence = i++;
			// StdStyles.Add(STYLE_ID_MILLIMETERS, udr);
			//
			//
			addStyleItem(i++, UnitTypeId.MetersCentimeters, null, STYLE_DATA.MetersCentimeters.NameId);
			//
			// udr = new UnitsDataR(
			// 	UnitTypeId.MetersCentimeters, null,
			// 	UnitsStdUStyles.StdStyles[STYLE_ID_METERS_CENTIMETERS]);
			// udr.Sequence = i++;
			// StdStyles.Add(STYLE_ID_METERS_CENTIMETERS, udr);

			// addStyleItem(i++, UnitTypeId.Custom, null, STYLE_DATA.Control01.NameId);

			// List = CollectionViewSource.GetDefaultView(StdStyles);

			// ICollectionView a = CollectionViewSource.GetDefaultView(StdStyles);
		}
	}

#endregion
}