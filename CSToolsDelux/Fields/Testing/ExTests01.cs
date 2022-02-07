#region + Using Directives

using System;
using System.Text.RegularExpressions;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using Autodesk.Revit.UI;
using CSToolsDelux.Fields.FieldsManagement;
using CSToolsDelux.Utility;
using SharedCode.Windows;
using SharedCode.Fields.ExStorage.ExStorManagement;

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


		public ExStoreRtnCodes StartProcess(string dsKey)
		{
			ExStoreRtnCodes result;
	
			result = fm.DataStorExist(dsKey);

			if (result == ExStoreRtnCodes.XRC_GOOD)
			{
				// go read data
				result = step101();
				return result;
			}
			// not exist - proceed to add?
			result = step201(result);
			return result;
		}

		// read data
		private ExStoreRtnCodes step101()
		{
			ExStoreRtnCodes result;

			result = fm.ReadData();

			return result;
		}

		// datastorage does not exist
		// ask - ok to modify model to allow Fields to function
		private ExStoreRtnCodes step201(ExStoreRtnCodes status)
		{
			ExStoreRtnCodes result = ExStoreRtnCodes.XRC_FAIL;

			switch (status)
			{
			case ExStoreRtnCodes.XRC_SEARCH_FOUND_PRIOR_AND_NEW:
				{
					break;
				}
			case ExStoreRtnCodes.XRC_SEARCH_FOUND_PRIOR:
				{
					break;
				}
			default:
				{
					TaskDialogResult choice = okToProceed();

					switch (choice)
					{
					case TaskDialogResult.Yes:
						{
							result = ExStoreRtnCodes.XRC_PROCEED_GET_DATA;
							break;
						}
					case TaskDialogResult.Retry:
						{
							result = step401();
							break;
						}
					// case TaskDialogResult.No:
					// 	{
					// 		result = ExStoreRtnCodes.XRC_FAIL;
					// 		break;
					// 	}
					}
					break;
				}
			}

			return result;
		}

		// search for a prior DataStorage element 
		// the model name may have changed
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


		private TaskDialogResult okToProceed()
		{
			TaskDialog td = new TaskDialog("OK to Proceed");

			td.MainInstruction = "Fields has not been setup for this model";
			td.MainContent = "Is it OK to add Fields\n"
				+ "to this model?";
			td.ExpandedContent = "If you feel this is not correct\n"
				+ "and that Fields has previously been\n"
				+ "setup, select the 'Retry' button";
			td.MainIcon = TaskDialogIcon.TaskDialogIconShield;
			td.CommonButtons = TaskDialogCommonButtons.Yes |
				TaskDialogCommonButtons.No | TaskDialogCommonButtons.Retry;
			td.TitleAutoPrefix = true;

			TaskDialogResult result = td.Show();

			return result;
		}

		private TaskDialogResult gotPrior()
		{
			TaskDialog td = new TaskDialog("Prior Configurations Found");

			td.MainInstruction = "It appears that Fields is not configured\n"
				+ "for this model however, I found some\n"
				+ "prior configurations";
			td.MainContent = "Modify a prior configuration to\n"
				+ "be used for this model?";
			td.ExpandedContent = "Yes will reuse and revise the\n"
				+ "configuration previously saved.\n"
				+ "No will create a new configuration and remove\n"
				+ "the prior configuration to eliminate this\n"
				+ "condition in the future";
			td.MainIcon = TaskDialogIcon.TaskDialogIconShield;
			td.CommonButtons = TaskDialogCommonButtons.Yes |
				TaskDialogCommonButtons.No | TaskDialogCommonButtons.Cancel;
			td.TitleAutoPrefix = true;

			TaskDialogResult result = td.Show();

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