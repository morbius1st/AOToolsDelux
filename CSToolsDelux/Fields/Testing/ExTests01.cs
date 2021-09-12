#region + Using Directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using CSToolsDelux.Fields.ExStorage.ExStorManagement;
using CSToolsDelux.Fields.FieldsManagement;
using CSToolsDelux.Fields.SchemaInfo.SchemaData;
using CSToolsDelux.Utility;
using CSToolsDelux.WPF;

#endregion

// user name: jeffs
// created:   9/3/2021 10:34:15 PM

namespace CSToolsDelux.Fields.Testing
{
	public class ExTests01
	{
		private AWindow w;
		private Document doc;

		/// <inheritdoc cref="FieldsManager"/>
		private FieldsManager fm;


		public ExTests01(AWindow w, Document doc)
		{
			this.w = w;
			this.doc = doc;

			fm = new FieldsManager(w, doc);
		}

		// start process

		// step 1, does data storage & data exist?
		// yes -> step 101
		// no -> step 201

		// data exists
		// step 101, read data
		// step 102, show data
		// done

		// data does not exist
		// step 201, notify, ok to proceed
		// yes -> step 301
		// no -> step 401

		// data not exist and ok to proceed
		// step 301, get data
		// step 302, save data
		// step 303, goto step 1

		// data not exist and do not proceed
		// step 401, confirm, should exist, need to search?
		// yes -> step 501
		// no -> step 1000

		// not found but may exist
		// step 501, provide old document name
		// step 502, search for old name
		// found -> step 601
		// not found -> step 701

		// possible data found
		// step 601, show data, confirm
		// step 602, convert to new name
		// step 603, to to step 1

		// again not found
		// step 701, try again?
		// yes -> step 501
		// no -> step 1000


		// no choices left
		// step 1000, bye

		public ExStoreRtnCodes StartProcess(string docKey)
		{
			ExStoreRtnCodes result;

			result = fm.DataStorExist(docKey);
			if (result == ExStoreRtnCodes.XRC_GOOD)
			{
				// go read data
				result = step101();
				return result;
			}
			// not exist - proceed to add?
			result = step201();
			return result;
		}

		private ExStoreRtnCodes step101()
		{
			ExStoreRtnCodes result;

			result = fm.ReadData();

			return ExStoreRtnCodes.XRC_GOOD;
		}

		// datastorage does not exist
		// ask - ok to modify model to allow Fields to function
		private ExStoreRtnCodes step201()
		{
			ExStoreRtnCodes result;
			bool answer = false;
			// bool = ok to proceed?

			if (answer)
			{
				// step 301
				result = ExStoreRtnCodes.XRC_PROCEED_GET_DATA;
			}
			else
			{
				result = step401();
			}

			return result;
		}

		private ExStoreRtnCodes step401()
		{
			ExStoreRtnCodes result;
			bool answer = false;

			if (answer)
			{
				// step 501
				result = ExStoreRtnCodes.XRC_SEARCH_FOR_PRIOR;
			}
			else
			{
				result = ExStoreRtnCodes.XRC_FAIL;
			}

			return result;
		}

		public ExStoreRtnCodes Search(string oldDocName)
		{
			ExStoreRtnCodes result;
			bool answer = false;

			if (answer)
			{
				// step 501
				result = ExStoreRtnCodes.XRC_SEARCH_FOR_PRIOR;
			}
			else
			{
				result = ExStoreRtnCodes.XRC_FAIL;
			}

			return result;
		}

		public void testNames()
		{
			string test;
			string testA;
			string testB;

			SchemaBuilder sb = new SchemaBuilder(Guid.NewGuid());

			// failed - the "." is no good
			testA = Util.GetVendorId();

			w.WriteLineAligned($"is ok?| {testA}| ", $"{sb.AcceptableName(testA)}");

			// worked
			testA = Util.GetVendorId().Replace(".", "_");

			w.WriteLineAligned($"is ok?| {testA}| ", $"{sb.AcceptableName(testA)}");

			// this worked (but see below)
			testB = AppRibbon.Doc.Title;
			test = (testA + "_" + testB);

			w.WriteLineAligned($"is ok?| {test}| ", $"{sb.AcceptableName(test)}");

			// this worked (but see below)
			testB = AppRibbon.Doc.Title.Replace(' ', '_');
			test = (testA + "_" + testB);

			w.WriteLineAligned($"is ok?| {test}| ", $"{sb.AcceptableName(test)}");

			// this worked (but see below)
			testB = AppRibbon.Doc.Title.Replace(" ", null);
			test = (testA + "_" + testB);

			w.WriteLineAligned($"is ok?| {test}| ", $"{sb.AcceptableName(test)}");

			// this failed because the spaces are no-good
			// the above titles worked because the model's title already has no spaces
			testB = "this is a test";
			test = (testA + "_" + testB);

			w.WriteLineAligned($"is ok?| {test}| ", $"{sb.AcceptableName(test)}");

			// worked (eliminated the spaces)
			testB = "this is a test".Replace(" ", null);
			test = (testA + "_" + testB);

			w.WriteLineAligned($"is ok?| {test}| ", $"{sb.AcceptableName(test)}");

			// worked (eliminate the spaces)
			testB = "this is a test".Replace(" ", "");
			test = (testA + "_" + testB);

			w.WriteLineAligned($"is ok?| {test}| ", $"{sb.AcceptableName(test)}");

			testB = "this is a test TEST+123-456&789 =0";
			testB = Regex.Replace(testB, @"[^0-9a-zA-Z]", "");
			test = (testA + "_" + testB);

			w.WriteLineAligned($"is ok?| {test}| ", $"{sb.AcceptableName(test)}");


			w.ShowMsg();
		}
	}
}