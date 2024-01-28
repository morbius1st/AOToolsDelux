#region using

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CsDeluxMeasure.Windows;
using static CsDeluxMeasure.RevitSupport.RevitUtil;

#endregion

// username: jeffs
// created:  9/17/2023 7:10:55 PM

namespace CsDeluxMeasure.RevitSupport
{
	/*
	public class Prime : INotifyPropertyChanged
	{
	#region private fields

		private UIDocument uiDoc;

		private Window revitWindow;

		private DxMeasure dx;

		private MiniMain mm;

		private MainWindow mw;

	#endregion

	#region ctor

		public Prime(UIDocument uiDoc, Window revitWindow)
		{
			this.uiDoc = uiDoc;
			this.revitWindow = revitWindow;

			dx = new DxMeasure(uiDoc, revitWindow);

			mm = new MiniMain(uiDoc);

			mm.Owner = revitWindow;
		}

	#endregion

		
	#region public methods

	#endregion

	#region private methods

		private bool measure()
		{
			bool results;

			mw = new MainWindow(uiDoc);
			mw.Owner = revitWindow;
			mw.SetPosition(revitWindow);

			mm = new MiniMain(uiDoc);
			mm.Owner = revitWindow;
			mm.SetPosition(revitWindow);

			results = dx.Measure();

			// show mini win - if

			// show dialog - if

			return results;
		}

	#endregion

	#region event publishing

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

	#endregion

	#region system overrides

		public override string ToString()
		{
			return $"this is {nameof(Prime)}";
		}

	#endregion
	}
	*/
}