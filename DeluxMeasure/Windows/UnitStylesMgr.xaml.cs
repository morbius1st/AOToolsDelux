using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CSToolsStudies.Annotations;

namespace DeluxMeasure.Windows
{
	/// <summary>
	/// Interaction logic for UnitStylesMgr.xaml
	/// </summary>
	
	public partial class UnitStylesMgr : Window, INotifyPropertyChanged
	{
		private int dialogIdx = 0;


		public UnitStylesMgr()
		{
			InitializeComponent();
		}

		public int DialogIndex
		{
			[DebuggerStepThrough]
			get => dialogIdx;

			set
			{
				if (value == dialogIdx) return;
				dialogIdx = value;
				OnPropertyChanged();
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		[DebuggerStepThrough]
		private void OnPropertyChanged([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}
	}
}
