#region + Using Directives

using System.Diagnostics;
using System.Windows.Forms;
using AOTools.Utility;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using View = Autodesk.Revit.DB.View;

using static AOTools.Utility.Util;

using DluxMeasure;


#endregion

// projname: AOTools
// itemname: DxMeasure
// username: jeffs
// created:  12/1/2018 6:51:01 PM

namespace AOTools
{

	[Transaction(TransactionMode.Manual)]
	public class DxMeasure : IExternalCommand
	{
		public Result Execute(
			ExternalCommandData commandData,
			ref string message, ElementSet elements)
		{

			UIApplication uiApp = commandData.Application;
			Document _doc = uiApp.ActiveUIDocument.Document;

			DlxMeasure mx = DlxMeasure.Instance();

			// this cleaned up the text display problem
			//			Application.SetCompatibleTextRenderingDefault(false);
			using (TransactionGroup tg = new TransactionGroup(_doc, "AO delux measure"))
			{
				tg.Start();
				mx.Measure(uiApp);
				tg.RollBack();
			}

			return Result.Succeeded;
		}

	}
}
