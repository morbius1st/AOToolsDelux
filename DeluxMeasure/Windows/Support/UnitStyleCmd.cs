﻿#region using

using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using DeluxMeasure.UnitsUtil;

#endregion

// username: jeffs
// created:  2/20/2022 10:39:34 PM

namespace DeluxMeasure.Windows.Support
{
	public abstract class UnitStyleCmd
	{
		public const short MAX_STYLE_CMDS = 8;

		protected abstract int Index { get; }

		private UnitsManager UnitsManager = UnitsManager.Instance;

		public Result Execute(
			ExternalCommandData commandData,
			ref string message,
			ElementSet elements)
		{
			Document doc = commandData.Application.ActiveUIDocument.Document;

			return SetUnit( doc, UnitsManager.StyleList[Index]);
		}

		private Result SetUnit( Document doc, UnitStyles.UnitStyle style)
		{
			// ForgeTypeId id = unitStyles.Styles[idx].Id;
			ForgeTypeId id = style.Id;

			UnitsData.UnitInfo ui = UnitsData.UnitTypes[id];

			using (Transaction tg = new Transaction(doc, $"Set Units to {ui.Desc}"))
			{
				tg.Start();

				bool result = UnitsManager.SetUnit(doc, style);

				if (result)
				{
					tg.Commit();
				}
				else
				{
					tg.RollBack();
					return Result.Failed;
				}
			}

			return Result.Succeeded;
		}

	#if DEBUGUNITPREC
		public void showPrecision(UnitStyles.UnitStyle style)
		{
			string precString;
			string msg = "";

			precString = UnitsData.GetPrecString( style.UCat,
				style.Precision);
			msg += $"for unit| {style.Name}\n"
				+ $"with a precision of| {style.Precision}\n"
				+ $"precision string| {precString}\n\n";

			TaskDialog td = new TaskDialog("Unit Precision String");
			td.MainInstruction = msg;
			td.Show();
		}
	#endif
	}

	[Transaction(TransactionMode.Manual)]
	public class UnitStyleCmd0 : UnitStyleCmd, IExternalCommand
	{
		protected override int Index { get; } = 0;

		// #if DEBUGUNITPREC
		// 	private static int idx = 0;
		// 	public static int idxMax = 6;
		// 	
		// #endif

		// public Result Execute(
		// 	ExternalCommandData commandData,
		// 	ref string message,
		// 	ElementSet elements)
		// {
		// 	Document doc   = commandData.Application.ActiveUIDocument.Document;
		//
		// #if DEBUGUNITPREC
		// 	/*
		// 	 get the current project unit object
		// 	Units u = doc.GetUnits();
		// 	FormatOptions f = u.GetFormatOptions(SpecTypeId.Length);
		// 	double d = f.Accuracy;
		// 	*/
		//
		// 	idx %= idxMax;
		//
		// 	UnitStyles.UnitStyle style = unitStyles.TestStyles[idx++];
		//
		// 	showPrecision(style);
		//
		// 	return SetUnit( doc, style);
		// #else
		// 	return SetUnit( doc, UnitsManager.StyleList[1]);
		// 	
		// #endif
		//
		// }

		
	}

	[Transaction(TransactionMode.Manual)]
	public class UnitStyleCmd1 : UnitStyleCmd, IExternalCommand
	{
		protected override int Index { get; } = 1;

		// public Result Execute(
		// 	ExternalCommandData commandData,
		// 	ref string message,
		// 	ElementSet elements)
		// {
		// 	Document doc = commandData.Application.ActiveUIDocument.Document;
		//
		// 	return SetUnit( doc, UnitsManager.StyleList[1]);
		// }

	}

	[Transaction(TransactionMode.Manual)]
	public class UnitStyleCmd2 : UnitStyleCmd, IExternalCommand
	{
		protected override int Index { get; } = 2;

		// public Result Execute(
		// 	ExternalCommandData commandData,
		// 	ref string message,
		// 	ElementSet elements)
		// {
		// 	Document doc = commandData.Application.ActiveUIDocument.Document;
		//
		// 	return SetUnit( doc, UnitsManager.StyleList[2]);
		// }

	}

	[Transaction(TransactionMode.Manual)]
	public class UnitStyleCmd3 : UnitStyleCmd, IExternalCommand
	{
		protected override int Index { get; } = 3;

		// public Result Execute(
		// 	ExternalCommandData commandData,
		// 	ref string message,
		// 	ElementSet elements)
		// {
		// 	Document doc = commandData.Application.ActiveUIDocument.Document;
		//
		// 	return SetUnit( doc, UnitsManager.StyleList[3]);
		// }

	}

	[Transaction(TransactionMode.Manual)]
	public class UnitStyleCmd4 : UnitStyleCmd, IExternalCommand
	{
		protected override int Index { get; } = 4;
	}
	
	[Transaction(TransactionMode.Manual)]
	public class UnitStyleCmd5 : UnitStyleCmd, IExternalCommand
	{
		protected override int Index { get; } = 5;
	}
	
	[Transaction(TransactionMode.Manual)]
	public class UnitStyleCmd6 : UnitStyleCmd, IExternalCommand
	{
		protected override int Index { get; } = 6;
	}
	
	[Transaction(TransactionMode.Manual)]
	public class UnitStyleCmd7 : UnitStyleCmd, IExternalCommand
	{
		protected override int Index { get; } = 7;
	}
	
	[Transaction(TransactionMode.Manual)]
	public class UnitStyleCmd8 : UnitStyleCmd, IExternalCommand
	{
		protected override int Index { get; } = 8;
	}
}