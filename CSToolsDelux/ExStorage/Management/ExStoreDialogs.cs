#region using directives

using System;
using Autodesk.Revit.UI;

#endregion


// projname: $projectname$
// itemname: ExStoreDialogs
// username: jeffs
// created:  10/23/2021 9:04:29 AM

namespace CSToolsDelux.ExStorage.Management
{
	public class ExStoreDialogs
	{
	#region private fields

		private static readonly Lazy<ExStoreDialogs> instance =
			new Lazy<ExStoreDialogs>(() => new ExStoreDialogs());

	#endregion

	#region ctor

		private ExStoreDialogs() { }

	#endregion

	#region public properties

		public static ExStoreDialogs Instance => instance.Value;

	#endregion

	#region private properties

	#endregion

	#region public methods


		internal TaskDialogResult MsgDlgNoDocName()
		{
			TaskDialog td = new TaskDialog("Unsaved Model");

			td.MainInstruction = "This model has not been saved";
			td.MainContent = "The model must be saved before Fields\n"
				+ "can be used with this model";
			td.ExpandedContent = "Fields uses the name of the model to\n"
				+ "identify its objects.  This ensures that the correct\n"
				+ "objects are found and used.";
			td.MainIcon = TaskDialogIcon.TaskDialogIconShield;
			td.CommonButtons = TaskDialogCommonButtons.Ok;
			td.TitleAutoPrefix = true;

			TaskDialogResult result = td.Show();

			return result;

		}

		internal TaskDialogResult okToProceed()
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

		internal TaskDialogResult gotPrior()
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

	#endregion

	#region private methods

	#endregion

	#region event consuming

	#endregion

	#region event publishing

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is ExStoreDialogs";
		}

	#endregion
	}
}