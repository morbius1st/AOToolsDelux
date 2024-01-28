#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using AOToolsDelux.Cells.SchemaDefinition;
using static UtilityLibrary.MessageUtilities;

using AOToolsDelux.Cells.ExStorage;
using AOToolsDelux.Cells.Tests;
using Autodesk.Revit.DB.ExtensibleStorage;
using Microsoft.Office.Interop.Excel;
using Microsoft.Win32;
using static AOToolsDelux.Cells.ExStorage.ExStoreMgr;

#endregion

// itemname:	UnitStylesCommand
// username:	jeffs
// created:		1/6/2018 3:55:08 PM


namespace AOToolsDelux
{

	[Transaction(TransactionMode.Manual)]
	class ReadAppRibbonInfo : IExternalCommand
	{
		// public static Schema SchemaUnit;
		// public static Entity EntityUnit;
		//
		// public static Schema SchemaDS;
		// public static Entity EntityDS;

		ExStorageTests xsTest = new ExStorageTests(null);

		private ExStoreMgr xm; // = new ExStoreMgr();
		public int idx2x = 0;

		public int idx2
		{
			get
			{
				idx2x = (idx2x + 1) % 4;
				return idx2x;
			}
		}

		public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
		{

			AppRibbon.UiApp = commandData.Application;
			AppRibbon.Uidoc = AppRibbon.UiApp.ActiveUIDocument;
			AppRibbon.App =  AppRibbon.UiApp.Application;
			AppRibbon.Doc =  AppRibbon.Uidoc.Document;

			OutLocation = OutputLocation.DEBUG;

			if (xm == null) xm = new ExStoreMgr();

			return Test01();
		}

		private Result Test01()
		{
			ExStoreHelper xsHlpr = new ExStoreHelper();

			// ExStoreRoot xRoot = ExStoreRoot.Instance();

			try
			{
				if (AppRibbon.Doc.IsDetached) return Result.Cancelled;

				test01b();

			}
			catch (OperationCanceledException)
			{
				return Result.Failed;
			}

			return Result.Succeeded;
		}

		private void test01b()
		{
			IList<Schema> schemaList = Schema.ListSchemas();

			string msg1 = "Read App-Ribbon Info";

			// AppRibbon.idx = AppRibbon.idx++;

			string ms = AppRibbon.getTestMsg1s();
			string m1s = AppRibbon.testMessageS;
			string m1 = AppRibbon.appR.getTestMsg1();
			string m1t = AppRibbon.appR.testMessage1;

			string mxs = ExStoreMgr.ms;
			string mx1 = XsMgr.m1;

			string ixs = ExStoreMgr.idx.ToString();
			string ix1 = XsMgr.mi.ToString();

			string xm1 = xm.mx;

			string msg2 = $"info| {idx2}\n"
				// + $"idx| {AppRibbon.idx}\n"
				// + $"rib static| {ms}\n"
				// + $"rib static| {m1s}\n"
				// + $"rib static indirect meth| {m1}\n"
				// + $"rib static indirect prop| {m1t}\n"
				// + $" xs index static| {ixs}\n"
				// + $" xs index indirect| {ix1}\n"
				// + $" xs static| {mxs}\n"
				// + $" xs static indirect prop| {mx1}\n\n"
				+ $" xm not static| {xm1} ({xm.idx2x})"
				+ "\n";			

			string msg3;

			foreach (Schema s in schemaList)
			{
				msg3 = s.GUID.ToString();
				msg2 += $"{s.SchemaName} ::   {msg3.Substring(msg3.Length-8, 8)}\n";
			}

			xsTest.taskDialogWarning_Ok("schema lookup",
				$"{msg1}", 
				$"\nSchema|\n{msg2}");



		}



		// read the schema - this is just the field list
		private void test01a()
		{
			IList<Schema> schemaList = Schema.ListSchemas();

			Schema s = Schema.Lookup(XsMgr?.XRoot.ExStoreGuid ?? Guid.Empty);

			string fieldName = XsMgr.XRoot.SchemaDefinition.Fields[SchemaRootKey.RK_APP_GUID].Name;
			Field field = s.GetField(fieldName);

			string msg1 = s == null ? "null" : "not null";

			string msg2 = field.ToString();

			IList<Field> fields = s.ListFields();

			foreach (Field f in fields)
			{
				msg2 += $"{f.FieldName}\n";
			}

			xsTest.taskDialogWarning_Ok("schema lookup",
				$"schema is| {msg1}", 
				$"\nfields|\n{msg2}");
		}


	}

}