#region using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Autodesk.Revit.DB;
using SettingsManager;

#endregion

// projname: CsDeluxMeasure
// itemname: MainWindow
// username: jeffs
// created:  2/12/2022 8:46:31 AM

namespace CsDeluxMeasure.Windows
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : AWindow, INotifyPropertyChanged
	{

	#region private fields

	#endregion

	#region ctor

		public MainWindow()
		{
			InitializeComponent();
		}

	#endregion

	#region public properties

		public string MessageBoxText
		{
			get => textMsg01;
			set
			{
				textMsg01 = value;
			}
		}

	#endregion

	#region private properties

	#endregion

	#region public methods

		public void TestAppSettings()
		{

			IList<ForgeTypeId> validUnits = UnitUtils.GetValidUnits(SpecTypeId.Length);

			Tuple<ForgeTypeId, bool, IList<ForgeTypeId>>[] answers = 
			new Tuple<ForgeTypeId, bool, IList<ForgeTypeId>>[validUnits.Count];

			for (int i = 0; i < validUnits.Count; i++)
			{
				bool result = FormatOptions.CanHaveSymbol(validUnits[i]);
				IList<ForgeTypeId> symbols = null;

				if (result)
				{
					symbols = FormatOptions.GetValidSymbols(validUnits[i]);
				}

				answers[i] = new Tuple<ForgeTypeId, bool, IList<ForgeTypeId>>(validUnits[i], result, symbols);
			}


			WriteNewLine();
			WriteLine2("status", "| ", AppSettings.Admin.Status);
			WriteLine2("path", "| ", AppSettings.Admin.Path);
			WriteLine2("status", "| ", "reading");
			AppSettings.Admin.Read();
			WriteLine2("test value", "| ", AppSettings.Data.AppSettingsValue);
			WriteLine2("change value", "| ", "to += 1");

			AppSettings.Data.AppSettingsValue += 1;

			AppSettings.Admin.Write();

			WriteLine2("app settings", "| ", "written");
			AppSettings.Data.AppSettingsValue = 0;
			WriteLine2("change value", "| ", $"to {AppSettings.Data.AppSettingsValue}");

			ShowMsg();

			MessageBox.Show(this, textMsg01);

		}

	#endregion

	#region private methods

	#endregion

	#region event consuming


		private void Btn_SelPoints_OnClick(object sender, RoutedEventArgs e)
		{
			TestAppSettings();
		}

	#endregion

	#region event publishing

		public event PropertyChangedEventHandler PropertyChanged;

		protected override void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is MainWindow";
		}

	#endregion

	}
}