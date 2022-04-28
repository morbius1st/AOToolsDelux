#region using

using System;
using System.Collections.Generic;
using System.Windows.Data;
using Autodesk.Revit.DB;

using static Autodesk.Revit.DB.FormatOptions;
using SettingsManager;

#endregion

// username: jeffs
// created:  2/26/2022 9:26:35 AM

/*

data collections

StdStyles  (AppStyles)		(Dictionary<ForgeTypeId, UnitsDataR>)  to / from app setting file
StyleList  (UserStyles)		(List<UnitsDataR>)  to / from user setting file



data map
** data
UnitsDataR		a standard styles that includes Revit specific information
				+ holds a pointer to a UnitUStyle
UnitsDataD		same as UnitsDataR except does not have any Revit specific information
				only intended for debugging
				+ holds a pointer to a UnitUStyle

UnitStdStylesR	collection of UnitsDataR (standard styles) and includes Revit specific information
UnitStdStylesD	same as UnitStdStylesR except does not have any Revit specific information
				only intended for debugging
	
UnitsManager.StyleList (working styles | specific to a user)
UnitsManager.StdStyles (working styles | specific to the app / starts out as the system dafault)

** base
UnitUStyle		base unit style | specifically does not include any Revit specific information (which allows this to be used as d:DataContext
UnitsStdUStyles	collection of standard styles - one per unit display system.

** support
UnitSupport		(parts static) data / settings for units | conversion routines

UnitSettings	(not static) I/O for writing / reading data to user or app setting file (front end for SettingsManager)

UnitsManager	(is static) manager if the information
				+ holds "UnitsUStyles"
				+ holds "UnitSupport"
				+ manages the working lists
					-> StyleList (access point to the data saved in the user's setting file)
					-> StdStyles (access point to the data saved in the app setting file)
				+ provides access to the current project unit style

** UI
UnitStylesManager	manager for the interface

** usage

allow user to create and organize unit styles
* organize
	+ determine which list a style appears
	+ determine the order in each list a style appears
* create
	+ make a new style from the current project unit settings
	+ allow to only change
		-> name
		-> description
		-> sample
	+ cannot change any other unit settings / done only from revit

*/


namespace DeluxMeasure.UnitsUtil
{
	public class UnitsManager
	{

	#region private fields

		private static readonly Lazy<UnitsManager> instance =
			new Lazy<UnitsManager>(() => new UnitsManager());

		// private Dictionary<ForgeTypeId, UnitsDataR> styleList = null;
		// private Dictionary<ForgeTypeId, UnitsDataR> stdStyles = null;

		private UnitsSettings uStg;
		private UnitsSupport uSup;

	#endregion

	#region ctor

		public UnitsManager()
		{
		#if PATH
			MethodBase mb = new StackTrace().GetFrame(1).GetMethod();
			Debug.WriteLine($"@UnitsManager: ctor: {(mb.ReflectedType?.FullName ?? "is null")} > {mb.Name}");
			#endif

			uSup = new UnitsSupport();
			uStg = new UnitsSettings();
		}

	#endregion

	#region public properties

		public static UnitsManager Instance {
			
			get
			{

			#if PATH
				MethodBase mb = new StackTrace().GetFrame(1).GetMethod();
				Debug.WriteLine($"@UnitsManager: Instance: {(mb.ReflectedType?.FullName ?? "is null")} > {mb.Name}");
			#endif

				return instance.Value;
			}
	}

		public List<UnitsDataR> StyleList
		{
			get
			{
			#if PATH
				MethodBase mb = new StackTrace().GetFrame(1).GetMethod();
				Debug.WriteLine($"@UnitsManager: StyleList: {(mb.ReflectedType?.FullName ?? "is null")} > {mb.Name}");
			#endif

				return SettingsManager.UserSettings.Data.UserStyles;
			}
			// set => SettingsManager.UserSettings.Data.UserStyles = value;
		}

		public Dictionary<string, UnitsDataR> StdStyles
		{
			get => SettingsManager.AppSettings.Data.AppStyles;
			// set => SettingsManager.AppSettings.Data.AppStyles = value;
		}

		public static Document Doc { get; set ; }

		public UnitsDataR ProjectUnitStyle => uSup.GetProjectUnitData(Doc);

	#endregion

	#region private properties

	#endregion

	#region public methods

		public void SetInitialSequence()
		{
			uSup.SetInitialSequence(StyleList);
		}
		
		public void ResetInitialSequence()
		{
			uSup.ResetInitialSequence(StyleList);
		}

		public void UnDelete()
		{
			uSup.UnDelete(StyleList);
		}

		public void WriteUser()
		{
			uSup.RemoveDeleted(StyleList);

			UserSettings.Admin.Write();
		}

		public void ReSequenceStylesList(ListCollectionView styles, int start)
		{
			if (start == styles.Count - 1) return;

			uSup.ReSequence(styles, start);
		}

		public void ReadUnitSettings()
		{

		#if PATH
			MethodBase mb = new StackTrace().GetFrame(1).GetMethod();
			Debug.WriteLine($"@UnitsManager: ReadUnitSettings: {(mb.ReflectedType?.FullName ?? "is null")} > {mb.Name}");
		#endif

			uStg.ReadStyles();
		}

		public bool SetUnit( /*Document doc, */ UnitsDataR style)
		{
			Units units = makeStdLengthUnit( style);

			if (units == null) return false;

			if (setUnit(Doc, units)) return true;

			return false;
		}

		public string FormatLength(double value, UnitsDataR style)
		{
			Units units = makeStdLengthUnit(style);

			if (units == null) return "N/A";

			string result =  UnitFormatUtils.Format(units, SpecTypeId.Length, value, false);
			return result;
		}

		public FormatOptions GetFormatOptions(UnitsDataR style)
		{
			FormatOptions fmtOpts;
			UStyle us = style.Ustyle;

			try
			{
				fmtOpts = new FormatOptions(style.Id);
				fmtOpts.Accuracy = us.Precision;

				if (CanHaveSymbol(style.Id))
				{
					fmtOpts.SetSymbolTypeId(style.Symbol);
				}

				if (CanSuppressLeadingZeros(style.Id))
				{
					fmtOpts.SuppressLeadingZeros = setFmtOpt(us.SuppressLeadZeros);
				}

				if (CanSuppressTrailingZeros(style.Id))
				{
					fmtOpts.SuppressTrailingZeros = setFmtOpt(us.SuppressTrailZeros);
				}

				if (CanSuppressSpaces(style.Id))
				{
					fmtOpts.SuppressSpaces = setFmtOpt(us.SuppressSpaces);
				}

				if (CanUsePlusPrefix(style.Id))
				{
					fmtOpts.UsePlusPrefix = setFmtOpt(us.UsePlusPrefix);
				}
			}
			catch (Exception e)
			{
				return null;
			}

			return fmtOpts;
		}

	#endregion

	#region private methods

		private bool setFmtOpt(bool? opt)
		{
			if (opt == null) return false;

			return opt.Value;
		}

		private bool setUnit(Document doc, Units unit)
		{
			try { doc.SetUnits(unit); }
			catch (Exception e)
			{
				return false;
			}

			return true;
		}
		
		private Units makeStdLengthUnit(UnitsDataR udr)
		{
			// if (udr.Ustyle.IsLocked == null) return null;

			Units units;
			FormatOptions fmtOpts;

			try
			{
				fmtOpts = GetFormatOptions(udr);
			}
			catch (Exception e)
			{
				return null;
			}

			units = new Units(udr.USystem);
			units.SetFormatOptions(SpecTypeId.Length, fmtOpts);

			return units;
		}
		

		// private void populateDefaultStyleList()
		// {
		// 	styleList = new List<UnitStyle>();
		//
		// 	foreach (UnitStyle unitStyle in UnitStyles.StdStyles)
		// 	{
		// 		styleList.Add(unitStyle);
		// 	}
		// }

	#endregion

	#region event consuming

	#endregion

	#region event publishing

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is UnitUtils";
		}

	#endregion
	}

}