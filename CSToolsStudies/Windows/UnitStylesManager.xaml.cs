using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace CSToolsStudies.Windows
{
	/// <summary>
	/// Interaction logic for UnitStylesManager.xaml
	/// </summary>
	public partial class UnitStylesManager : Window, INotifyPropertyChanged
	{
	#region private fields

	#endregion

	#region ctor

		public UnitStylesManager()
		{
			InitializeComponent();
		}

	#endregion


	#region public properties

		// list items
		public bool IsSelected { get; set; } = false;
		public bool IsEditing { get; set; } = false;
		public bool ReadOnly { get; set; } = false;
		public bool IsLocked { get; set; } = false;


		// can add 
		public bool IsAddBeforeEnabled { get; set; } = false;
		public bool? IsInsPosOk { get; set; } = true;

		public int InsPosition { get; set; } = 1;

		// dialog control
		public bool IsAdjStylesEnabled { get; set; } = false;
		public bool IsDoneEnabled { get; set; } = false;
		public bool IsApplyEnabled { get; set; } = false;
		public bool IsCancelEnabled { get; set; } = false;
		public bool IsResetEnabled { get; set; } = false;


		public int DialogTypeIdx { get; set; } = 0;

	#endregion

	#region private properties

	#endregion

	#region public methods

	#endregion

	#region private methods

	#endregion

	#region event consuming

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
			return "this is UnitStylesManager";
		}

	#endregion
	}
}